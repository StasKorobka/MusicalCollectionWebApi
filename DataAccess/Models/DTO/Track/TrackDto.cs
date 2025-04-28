
namespace DataAccess.Models.DTO.Track
{
    public class TrackDto
    {
        public int TrackId { get; set; }
        public string Title { get; set; }
        public TimeSpan Length { get; set; }
    }
}