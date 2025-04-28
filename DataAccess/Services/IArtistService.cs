using DataAccess.Models.DTO.Artist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public interface IArtistService
    {
        Task<List<ArtistDto>> GetAllArtistsAsync();
        Task<ArtistDto> GetArtistByNameAsync(string name);
        Task<ArtistDto> GetArtistByIdAsync(int id);
    }
}
