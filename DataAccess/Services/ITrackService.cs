using DataAccess.Models.DTO.Track;

namespace DataAccess.Services
{
    public interface ITrackService
    {
        Task<List<TrackDto>> GetAllTracksAsync();
        Task<TrackDto> GetTrackByNameAsync(string name);
        Task<TrackDto> GetTrackByIdAsync(int id);
    }
}