using AutoMapper;
using DataAccess.Data;
using DataAccess.Models.DTO.Album;
using DataAccess.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace DataAccess.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly MusicalCollectionDbContext _context;
        private readonly IMapper _mapper;

        public AlbumService(MusicalCollectionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<AlbumDto>> GetAllAlbumsAsync()
        {
            var albums = await _context.Albums
                .Include(a => a.Tracks)
                .ToListAsync();
            List<AlbumDto> albumsDto = new List<AlbumDto>();

            foreach (var album in albums) 
            {
                albumsDto.Add(_mapper.Map<AlbumDto>(album));
            }

            return albumsDto;
        }
        public async Task<AlbumDto> GetAlbumByNameAsync(string albumName)
        {
            if (string.IsNullOrWhiteSpace(albumName))
            {
                throw new ArgumentException("Album name cannot be empty or whitespace.");
            }

            //check if the title is the same
            var album = await _context.Albums
                .Include(a => a.Tracks)
                .Where(a => a.Title.ToLower().Contains(albumName.ToLower()))
                .FirstOrDefaultAsync();

            if (album == null)
            {
                throw new KeyNotFoundException($"Album with name '{albumName}' not found.");
            }

            return _mapper.Map<AlbumDto>(album);
        }
        public async Task<List<AlbumDto>> GetAlbumsByGenreAsync(string genre)
        {
            if (string.IsNullOrWhiteSpace(genre))
            {
                throw new ArgumentException("Genre cannot be empty or whitespace.");
            }
            var albums = await _context.Albums
                .Include(a => a.Tracks)
                .Where(a => a.Genres != null) // only basic filtering in SQL
                .ToListAsync(); // fetch to memory

            var filteredAlbums = albums
                .Where(a => a.Genres.Any(g => string.Equals(g, genre, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            return _mapper.Map<List<AlbumDto>>(filteredAlbums);
        }
        public async Task<AlbumDto> GetAlbumByIdAsync(int albumId)
        {
            // check if the id is the same as searched
            var album = await _context.Albums
                .Include(a => a.Tracks)
                .Where(a => a.AlbumId == albumId)
                .FirstOrDefaultAsync();

            if (album == null)
            {
                throw new KeyNotFoundException($"Album with ID {albumId} not found.");
            }

            return _mapper.Map<AlbumDto>(album);
        }
    }
}