namespace Maikelvdb.Xprtz.Assessment.Dto.Shows
{
    public class ShowDto
    {
        public int Id { get; set; }
        public int? ExternalId { get; set; }
        public string Name { get; set; }
        public string Summary { get; set; }
        public string Language { get; set; }
        public DateTime? Premiered { get; set; }
        public List<string> Genres { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
