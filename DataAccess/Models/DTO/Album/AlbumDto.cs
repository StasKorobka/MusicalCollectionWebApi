using DataAccess.Models.DTO.Track;
using DataAccess.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models.DTO.Album
{
    public class AlbumDto
    {
        public string Title { get; set; }
        public int ArtistId { get; set; }
        public List<string> Genres { get; set; } = new();
        public DateOnly ReleaseDate { get; set; }
        public int NumberOfTracks { get; set; }
        public TimeSpan Length { get; set; }
        public string Label { get; set; }
        public AlbumFormat Format { get; set; }
        public List<TrackDto> Tracks { get; set; } = new();
    }
}
