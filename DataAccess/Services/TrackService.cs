using AutoMapper;
using DataAccess.Data;
using DataAccess.Models;
using DataAccess.Models.DTO.Track;
using DataAccess.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services
{
    public class TrackService : ITrackService
    {
        private readonly MusicalCollectionDbContext _context;
        private readonly IMapper mapper;

        public TrackService(MusicalCollectionDbContext context, IMapper mapper)
        {
            _context = context;
            this.mapper = mapper;
        }

        public async Task<List<TrackDto>> GetAllTracksAsync()
        {
            var tracks = await _context.Tracks.ToListAsync();
            List<TrackDto> result = new List<TrackDto>();

            foreach (var track in tracks) 
            {

                result.Add(mapper.Map<TrackDto>(track));
            }
            
            return result;
        }

        public async Task<TrackDto> GetTrackByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Track name cannot be empty or whitespace.");
            }

            //searching for track that matches name
            var track = await _context.Tracks
                .Where(t => t.Title.ToLower() == name.ToLower())
                .FirstOrDefaultAsync();

            if (track == null)
            {
                throw new KeyNotFoundException($"Track with name '{name}' not found.");
            }

            return mapper.Map<TrackDto>(track);
        }

        public async Task<TrackDto> GetTrackByIdAsync(int id)
        {
            var track = await _context.Tracks
                .Where(t => t.TrackId == id)
                //.Select(t => new TrackDto
                //{
                //    TrackId = t.TrackId,
                //    Title = t.Title,
                //    Length = t.Length
                //})
                .FirstOrDefaultAsync();

            if (track == null)
            {
                throw new KeyNotFoundException($"Track with ID {id} not found.");
            }

            return mapper.Map<TrackDto>(track);
        }
    }
}