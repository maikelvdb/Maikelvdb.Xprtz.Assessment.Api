using Maikelvdb.Xprtz.Assessment.Api.Services.TvMazeApi.Models;

namespace Maikelvdb.Xprtz.Assessment.Api.Services.TvMazeApi
{
    public interface ITvMazeApiService
    {
        Task<IList<MazeTvShow>> CollectShowsAsync(DateTime fromDate);
    }
}
