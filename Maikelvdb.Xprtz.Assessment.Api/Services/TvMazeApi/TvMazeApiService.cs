using Maikelvdb.Xprtz.Assessment.Api.Services.TvMazeApi.Models;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;

namespace Maikelvdb.Xprtz.Assessment.Api.Services.TvMazeApi
{
    public class TvMazeApiService : ITvMazeApiService
    {
        public async Task<IList<MazeTvShow>> CollectShowsAsync(DateTime fromDate)
        {
            var client = new HttpClient { 
                BaseAddress = new Uri("https://api.tvmaze.com"),
            };

            var response = await client.GetAsync("schedule/full?embed=show");
            if (!response.IsSuccessStatusCode)
            {
                return new List<MazeTvShow>(0);
            }

            var result = await response.Content.ReadAsStringAsync();
            var episodes = System.Text.Json.JsonSerializer.Deserialize<IList<MazeTvEpisode>>(result, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            });

            var shows = episodes.Where(x => x.Embedded.Show.Premiered >= fromDate).GroupBy(x => x.Embedded.Show.Id).Select(x => x.First().Embedded.Show).ToList();

            return shows;
        }
    }
}
