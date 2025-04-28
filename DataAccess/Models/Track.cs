namespace DataAccess.Models
{
    public class Track
    {
        public int TrackId { get; set; }
        public string Title { get; set; }
        public TimeSpan Length { get; set; }

        public int AlbumId { get; set; }
        public Album Album { get; set; }

        public int PositionInAlbum { get; set; }
        public string Songwriters { get; set; }

        public List<PlaylistTrack> PlaylistTracks { get; set; } = new();
    }
}