using AutoMapper;
using DataAccess.Data;
using DataAccess.Models;
using DataAccess.Models.DTO.Album;
using DataAccess.Models.DTO.Artist;
using DataAccess.Models.DTO.Track;
using DataAccess.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public class ArtistService : IArtistService
    {
        private readonly MusicalCollectionDbContext _context;
        private readonly IMapper mapper;

        public ArtistService(MusicalCollectionDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        public async Task<List<ArtistDto>> GetAllArtistsAsync()
        {
            var artists = await _context.Artists
                .Include(a => a.Albums)
                .ThenInclude(a => a.Tracks)
                .ToListAsync();

            List<ArtistDto> artistsDto = new List<ArtistDto>();

            foreach (var artist in artists) 
            {
                artistsDto.Add(mapper.Map<ArtistDto>(artist));
            }

            return artistsDto;
        }
        public async Task<ArtistDto> GetArtistByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Artist name cannot be empty or whitespace.");
            }

            var artist = await _context.Artists
                .Include(a => a.Albums)
                .ThenInclude(a => a.Tracks)
                .Where(a => a.Name.ToLower() == name.ToLower())
                .FirstOrDefaultAsync();

            if (artist == null)
            {
                throw new KeyNotFoundException($"Artist with name '{name}' not found.");
            }

            return mapper.Map<ArtistDto>(artist);
        }
        public async Task<ArtistDto> GetArtistByIdAsync(int artistId)
        {
            if (artistId <= 0)
            {
                throw new ArgumentException("Id must be bigger that 0");
            }

            var artist = await _context.Artists
                .Include(a => a.Albums)
                .ThenInclude(a => a.Tracks)
                .Where(a => a.ArtistId == artistId)
                .FirstOrDefaultAsync();
            if (artist == null)
            {
                throw new KeyNotFoundException($"Artist with id {artistId} not found.");
            }

            return mapper.Map<ArtistDto>(artist);
        }
    }
}