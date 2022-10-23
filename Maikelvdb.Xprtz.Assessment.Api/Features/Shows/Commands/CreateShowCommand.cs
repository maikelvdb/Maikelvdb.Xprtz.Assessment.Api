namespace Maikelvdb.Xprtz.Assessment.Api.Features.Shows.Commands
{
    public class CreateShowCommand : IRequest<ShowDto>
    {
        public CreateShowCommand()
        {
            Name = string.Empty;
            Summary = string.Empty;
            Language = string.Empty;

            Genres = new(0);
        }

        public string Name { get; set; }
        public string Summary { get; set; }
        public string Language { get; set; }
        public DateTime? Premiered { get; set; }
        public List<string> Genres { get; set; }

    }
}
