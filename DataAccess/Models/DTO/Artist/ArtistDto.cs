using DataAccess.Models.DTO.Album;
using DataAccess.Models.DTO.Review;
using DataAccess.Models.DTO.Track;
using DataAccess.Models.Enums;

namespace DataAccess.Models.DTO.Artist
{
    public class ArtistDto
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTime FormationDate { get; set; }
        public DateTime? DisbandDate { get; set; }
        public string Genres { get; set; }
        public string? Biography { get; set; }
        public List<AlbumDto> Albums { get; set; } = new();
    }
}