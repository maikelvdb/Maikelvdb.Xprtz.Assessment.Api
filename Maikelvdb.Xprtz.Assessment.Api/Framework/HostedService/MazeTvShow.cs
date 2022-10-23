using System.Text.Json.Serialization;

namespace Maikelvdb.Xprtz.Assessment.Api.Framework.HostedService
{
    public class MazeTvShow
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("premiered")]
        public DateTime? Premiered { get; set; }

        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; }
    }
}
