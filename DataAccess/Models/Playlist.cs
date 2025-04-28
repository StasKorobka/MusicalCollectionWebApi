namespace DataAccess.Models
{
    public class Playlist
    {
        public int PlaylistId { get; set; }
        public string PlaylistName { get; set; }

        public int UserId { get; set; }
        public User Creator { get; set; }

        public List<PlaylistTrack> PlaylistTracks { get; set; } = new();
        public DateTime CreationDate { get; set; }
    }
}