using DataAccess.Models.DTO.Artist;

namespace DataAccess.Services.Interfaces
{
    public interface IArtistService
    {
        Task<List<ArtistDto>> GetAllArtistsAsync();
        Task<ArtistDto> GetArtistByNameAsync(string name);
        Task<ArtistDto> GetArtistByIdAsync(int id);
    }
}
