using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models.DTO.Playlist
{
    public class CreatePlaylistDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Playlist name must be between 1 and 100 characters.")]
        public string PlaylistName { get; set; }
    }
}