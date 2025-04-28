using DataAccess.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int AlbumId { get; set; }
        public Album Album { get; set; }
        public StarRating Rating { get; set; }
        public string? Comment { get; set; }

        [NotMapped]
        public readonly Dictionary<int, string> starDictionary = new Dictionary<int, string>
        {
            { (int)StarRating.OneStar,      "★☆☆☆☆" },
            { (int)StarRating.TwoStars,     "★★☆☆☆" },
            { (int)StarRating.ThreeStars,   "★★★☆☆" },
            { (int)StarRating.FourStars,    "★★★★☆" },
            { (int)StarRating.FiveStars,    "★★★★★" }
        };
        public string getStarsView(int rating)
        {
            if (rating < 1 || rating > 5) return "";
            return starDictionary[rating];
        }
    }
}