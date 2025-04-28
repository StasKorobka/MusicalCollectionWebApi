using DataAccess.Models.Enums;

namespace DataAccess.Models
{
    public class UserCollection
    {
        public int UserCollectionId { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int AlbumId { get; set; }
        public Album Album { get; set; }

        public DateTime AdditionDate { get; set; }
        public CollectionStatus Status { get; set; }
    }
}