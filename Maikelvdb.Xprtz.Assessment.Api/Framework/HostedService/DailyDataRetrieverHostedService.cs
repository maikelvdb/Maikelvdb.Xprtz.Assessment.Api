using Timer = System.Threading.Timer;

namespace Maikelvdb.Xprtz.Assessment.Api.Framework.HostedService
{
    public class DailyDataRetrieverHostedService : IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public DailyDataRetrieverHostedService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        private Timer? _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(RetrieveDataAsync, null, 0, TimeSpan.FromHours(24).Seconds);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (_timer != null)
            {
                _timer?.Change(Timeout.Infinite, 0);
            }

            return Task.CompletedTask;
        }

        // Return type kan geen Task zijn omdat Timer dat niet ondersteunt, maar wel async
        private async void RetrieveDataAsync(object? state)
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetService<DataContext>();

            var maxShowId = await context.Set<Show>().MaxAsync(x => x.ExternalId);
            var startingPage = maxShowId.HasValue ? decimal.ToInt32(Math.Floor(maxShowId.Value / 250M)) + 1 : 0;

            await CollectShowsAsync(startingPage, context);
        }

        private async Task CollectShowsAsync(int startingPage, DataContext context)
        {
            var shows = new List<MazeTvShow>();

            var client = new HttpClient
            {
                BaseAddress = new Uri("https://api.tvmaze.com/"),
            };

            var requestCount = 0;
            var requestSuccess = true;
            while (requestSuccess)
            {
                var response = await client.GetAsync($"shows?page={startingPage++}");
                if (!response.IsSuccessStatusCode)
                {
                    await SaveShowsAsync(shows, context);
                    requestSuccess = false;
                    continue;
                }

                var result = await response.Content.ReadAsStringAsync();
                var requestShows = System.Text.Json.JsonSerializer.Deserialize<IList<MazeTvShow>>(result);

                shows.AddRange(requestShows);

                if (requestCount == 20)
                {
                    await SaveShowsAsync(shows, context);

                    shows = new List<MazeTvShow>();
                    requestCount = 0;
                    await Task.Delay(TimeSpan.FromSeconds(11));
                    continue;
                }

                requestCount++;
            }
        }
    
        private async Task SaveShowsAsync(List<MazeTvShow> mazeShows, DataContext context)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<MazeTvShow, Show>()
                .ForMember(d => d.Id, m => m.Ignore())
                .ForMember(d => d.CreatedDate, m => m.Ignore())
                .ForMember(d => d.ModifiedDate, m => m.Ignore())
                .ForMember(d => d.GenresJson, m => m.Ignore())
                .ForMember(d => d.ExternalId, m => m.MapFrom(s => s.Id))
                );
            var mapper = config.CreateMapper();
            var shows = mapper.Map<List<Show>>(mazeShows);

            context.AddRange(shows);
            await context.SaveChangesAsync();
        }
    }
}
