using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maikelvdb.Xprtz.Assessment.Database.Entities
{
    public class Show : DbEntity
    {
        public Show()
        {
            Name = string.Empty;
            Summary = string.Empty;
            Language = string.Empty;
            IsArchived = false;
            Genres = new(0);
        }

        public int? ExternalId { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Summary { get; set; }

        public string? Language { get; set; }

        public DateTime? Premiered { get; set; }

        [NotMapped]
        public List<string> Genres
        {
            get
            {
                if (string.IsNullOrEmpty(GenresJson))
                {
                    return new(0);
                }

                return System.Text.Json.JsonSerializer.Deserialize<List<string>>(GenresJson);
            }
            set
            {
                GenresJson = System.Text.Json.JsonSerializer.Serialize(value);
            }
        }

        public string GenresJson { get; private set; }

        public bool IsArchived { get; set; }
    }
}
