using DataAccess.Models.DTO.Album;

namespace DataAccess.Services.Interfaces
{
    public interface IAlbumService
    {
        Task<List<AlbumDto>> GetAllAlbumsAsync();
        Task<AlbumDto> GetAlbumByNameAsync(string albumName);
        Task<AlbumDto> GetAlbumByIdAsync(int albumId);
        Task<List<AlbumDto>> GetAlbumsByGenreAsync(string genre);
    }
}