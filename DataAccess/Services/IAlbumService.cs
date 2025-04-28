using DataAccess.Models.DTO.Album;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public interface IAlbumService
    {
        Task<List<AlbumDto>> GetAllAlbumsAsync();
        Task<AlbumDto> GetAlbumByNameAsync(string albumName);
        Task<AlbumDto> GetAlbumByIdAsync(int albumId);
        Task<List<AlbumDto>> GetAlbumsByGenreAsync(string genre);
    }
}
