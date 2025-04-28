using DataAccess.Models.DTO.PlaylistTrack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.Playlist
{
    public class PlaylistDto
    {
        public int PlaylistId { get; set; }
        public string PlaylistName { get; set; }
        public DateTime CreationDate { get; set; }
        public List<PlaylistTrackDto> Tracks { get; set; }
    }
}
