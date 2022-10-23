using Maikelvdb.Xprtz.Assessment.Api.Services.TvMazeApi;
using Timer = System.Threading.Timer;
using Microsoft.Data.SqlClient;
using Dapper;
using Maikelvdb.Xprtz.Assessment.Api.Services.TvMazeApi.Models;

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
            var tvMazeApiService = scope.ServiceProvider.GetService<ITvMazeApiService>();

            using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
            var existingShows = await connection.QueryAsync<Show>("SELECT ExternalId FROM dbo.Shows WHERE ExternalId IS NOT NULL");
            var mazeShows = await tvMazeApiService.CollectShowsAsync(new DateTime(2014, 1, 1));

            var config = new MapperConfiguration(cfg => cfg.CreateMap<MazeTvShow, Show>()
                .ForMember(d => d.Id, m => m.Ignore())
                .ForMember(d => d.CreatedDate, m => m.Ignore())
                .ForMember(d => d.ModifiedDate, m => m.Ignore())
                .ForMember(d => d.GenresJson, m => m.Ignore())
                .ForMember(d => d.ExternalId, m => m.MapFrom(s => s.Id))
                );
            var mapper = config.CreateMapper();
            var shows = mapper.Map<List<Show>>(mazeShows.Where(x => !existingShows.Any(s => s.ExternalId == x.Id)).ToList());

            context.AddRange(shows);
            await context.SaveChangesAsync();
        }
    }
}
