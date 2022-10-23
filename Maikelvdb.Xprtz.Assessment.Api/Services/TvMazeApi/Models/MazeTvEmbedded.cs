using System.Text.Json.Serialization;

namespace Maikelvdb.Xprtz.Assessment.Api.Services.TvMazeApi.Models
{
    public class MazeTvEmbedded
    {
        [JsonPropertyName("show")]
        public MazeTvShow Show { get; set; }
    }
}
