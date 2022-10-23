using System.Text.Json.Serialization;

namespace Maikelvdb.Xprtz.Assessment.Api.Features.Shows.Commands
{
    public class UpdateShowCommand : IRequest<ShowDto>
    {
        public UpdateShowCommand()
        {
            Name = string.Empty;
            Summary = string.Empty;
            Language = string.Empty;

            Genres = new(0);
        }

        [JsonIgnore]
        public int ShowId { get; set; }
        public int? ExternalId { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Language { get; set; }
        public DateTime? Premiered { get; set; }
        public List<string> Genres { get; set; }
    }
}
