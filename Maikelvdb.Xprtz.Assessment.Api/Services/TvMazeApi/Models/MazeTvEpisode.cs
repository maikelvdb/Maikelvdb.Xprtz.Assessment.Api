using System.Text.Json.Serialization;

namespace Maikelvdb.Xprtz.Assessment.Api.Services.TvMazeApi.Models
{
    public class MazeTvEpisode
    {
        [JsonPropertyName("_embedded")]
        public MazeTvEmbedded Embedded { get; set; }
    }
}
