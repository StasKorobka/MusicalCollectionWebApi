using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.PlaylistTrack
{
    public class ShowPlaylistTrackDto
    {
        //public int TrackId { get; set; }
        public string Title { get; set; }
        public TimeSpan Length { get; set; }
        public int Position { get; set; }
        public string AlbumTitle { get; set; }
    }
}
