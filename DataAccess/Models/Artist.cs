namespace DataAccess.Models
{
    public class Artist
    {
        public int ArtistId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTime FormationDate { get; set; }
        public DateTime? DisbandDate { get; set; }
        public string Genres { get; set; }
        public string? Biography { get; set; }

        public List<Album> Albums { get; set; } = new();
    }
}