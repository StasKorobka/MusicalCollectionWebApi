using DataAccess.Models.DTO.Track;

namespace DataAccess.Services.Interfaces
{
    public interface ITrackService
    {
        Task<List<TrackDto>> GetAllTracksAsync();
        Task<TrackDto> GetTrackByNameAsync(string name);
        Task<TrackDto> GetTrackByIdAsync(int id);
    }
}