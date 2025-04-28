using DataAccess.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Album
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }

        public int ArtistId { get; set; }
        public Artist Artist { get; set; }

        public List<string> Genres { get; set; } = new();
        public DateOnly ReleaseDate { get; set; }
        public int NumberOfTracks { get; set; }
        public TimeSpan Length { get; set; }
        public string Label { get; set; }
        public AlbumFormat Format { get; set; }

        public List<Track> Tracks { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
        public List<UserCollection> UserCollections { get; set; } = new();
        public string GetGenres()
        {
            return string.Join(", ", Genres);
        }
    }
}