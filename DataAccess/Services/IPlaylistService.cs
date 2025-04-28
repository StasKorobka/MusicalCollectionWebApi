using DataAccess.Models.DTO.Playlist;
using DataAccess.Models.DTO.PlaylistTrack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public interface IPlaylistService
    {
        Task<List<ShowPlaylistDto>> GetPlaylistsByUserIdAsync(int userId);
        Task<ShowPlaylistDto> GetPlaylistByIdAndUserIdAsync(int playlistId, int userId);
        Task<PlaylistDto> CreatePlaylistAsync(int userId, CreatePlaylistDto createDto);
        Task<PlaylistDto> UpdatePlaylistAsync(int playlistId, int userId, UpdatePlaylistDto updateDto);
        Task<PlaylistDto> AddTrackToPlaylistAsync(int playlistId, int userId, AddTrackToPlaylistDto addTrackDto);
        Task<PlaylistDto> DeleteTrackFromPlaylistAsync(int playlistId, int userId, int trackId);
        Task DeletePlaylistAsync(int playlistId, int userId);

        Task<List<ShowPlaylistTrackDto>> GetTracksFromPlaylistAsync(int playlistId, int userId);
    }
}
