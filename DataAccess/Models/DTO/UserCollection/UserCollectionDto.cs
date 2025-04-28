using DataAccess.Models.Enums;

namespace DataAccess.Models.DTO.UserCollection
{
    public class UserCollectionDto
    {
        public int AlbumId { get; set; }
        public string AlbumTitle { get; set; }
        public DateTime AdditionDate { get; set; } 
        public CollectionStatus Status { get; set; }
    }
}