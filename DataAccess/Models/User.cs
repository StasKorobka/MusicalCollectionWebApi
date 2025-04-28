namespace DataAccess.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<UserCollection> Collections { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
        public List<Playlist> Playlists { get; set; } = new();

        public DateTime CreationDate { get; set; }
    }
}