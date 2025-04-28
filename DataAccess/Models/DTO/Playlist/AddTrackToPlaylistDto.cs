using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.Playlist
{
    public class AddTrackToPlaylistDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Track ID must be a positive integer.")]
        public int TrackId { get; set; }
    }
}
