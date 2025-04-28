using AutoMapper;
using DataAccess.Data;
using DataAccess.Models;
using DataAccess.Models.DTO.Playlist;
using DataAccess.Models.DTO.PlaylistTrack;
using DataAccess.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly MusicalCollectionDbContext _context;
        private readonly IMapper _mapper;
        public PlaylistService(MusicalCollectionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<ShowPlaylistDto>> GetPlaylistsByUserIdAsync(int userId)
        {
            var playlists = await _context.Playlists
                .Include(p => p.PlaylistTracks)
                .ThenInclude(pt => pt.Track)
                .ThenInclude(t => t.Album)
                .Where(p => p.UserId == userId)
                .ToListAsync();

            List<ShowPlaylistDto> playlistDtos = new List<ShowPlaylistDto>();
            foreach (var playlist in playlists) 
            {
                playlistDtos.Add(_mapper.Map<ShowPlaylistDto>(playlist));
            }

            return playlistDtos;
        }
        public async Task<ShowPlaylistDto> GetPlaylistByIdAndUserIdAsync(int playlistId, int userId)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistTracks)
                .ThenInclude(pt => pt.Track)
                .ThenInclude(t => t.Album)
                .FirstOrDefaultAsync(p => p.PlaylistId == playlistId && p.UserId == userId);

            if (playlist == null)
            {
                throw new KeyNotFoundException($"Playlist with ID {playlistId} not found for user ID {userId}.");
            }

            return _mapper.Map<ShowPlaylistDto>(playlist);
        }
        public async Task<PlaylistDto> CreatePlaylistAsync(int userId, CreatePlaylistDto createDto)
        {
            // verify that user exists
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var playlist = new Playlist
            {
                PlaylistName = createDto.PlaylistName,
                UserId = userId,
                CreationDate = DateTime.UtcNow,
                PlaylistTracks = new List<PlaylistTrack>()
            };

            _context.Playlists.Add(playlist);
            await _context.SaveChangesAsync();

            return _mapper.Map<PlaylistDto>(playlist);
            //return new PlaylistDto
            //{
            //    PlaylistId = playlist.PlaylistId,
            //    PlaylistName = playlist.PlaylistName,
            //    CreationDate = playlist.CreationDate,
            //    Tracks = new List<PlaylistTrackDto>()
            //};
        }
        public async Task<PlaylistDto> UpdatePlaylistAsync(int playlistId, int userId, UpdatePlaylistDto updateDto)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistTracks)
                .ThenInclude(pt => pt.Track)
                .ThenInclude(t => t.Album)
                .FirstOrDefaultAsync(p => p.PlaylistId == playlistId &&
                p.UserId == userId);

            if (playlist == null) // check that playlist exitst
            {
                throw new KeyNotFoundException($"Playlist with ID {playlistId} not found for user ID {userId}.");
            }

            playlist.PlaylistName = updateDto.PlaylistName;
            await _context.SaveChangesAsync();

            return _mapper.Map<PlaylistDto>(playlist);
        }
        public async Task<PlaylistDto> AddTrackToPlaylistAsync(int playlistId, int userId, AddTrackToPlaylistDto addTrackDto)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistTracks)
                .ThenInclude(pt => pt.Track)
                .ThenInclude(t => t.Album)
                .FirstOrDefaultAsync(p => p.PlaylistId == playlistId && p.UserId == userId);

            if (playlist == null)
            {
                throw new KeyNotFoundException($"Playlist with ID {playlistId} not found for user ID {userId}.");
            }

            var track = await _context.Tracks.FindAsync(addTrackDto.TrackId);
            if (track == null)
            {
                throw new KeyNotFoundException($"Track with ID {addTrackDto.TrackId} not found.");
            }

            if (playlist.PlaylistTracks.Any(pt => pt.TrackId == addTrackDto.TrackId))
            {
                throw new InvalidOperationException($"Track with ID {addTrackDto.TrackId} is already in playlist ID {playlistId}.");
            }

            var maxPosition = playlist.PlaylistTracks.Any() ?
                playlist.PlaylistTracks.Max(pt => pt.Position) : 0;
            var playlistTrack = new PlaylistTrack
            {
                PlaylistId = playlistId,
                TrackId = addTrackDto.TrackId,
                Position = maxPosition + 1
            };

            playlist.PlaylistTracks.Add(playlistTrack);
            await _context.SaveChangesAsync();

            return _mapper.Map<PlaylistDto>(playlist);
        }
        public async Task<PlaylistDto> DeleteTrackFromPlaylistAsync(int playlistId, int userId, int trackId)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistTracks)
                .ThenInclude(pt => pt.Track)
                .ThenInclude(t => t.Album)
                .FirstOrDefaultAsync(p => p.PlaylistId == playlistId && p.UserId == userId);

            if (playlist == null)
            {
                throw new KeyNotFoundException($"Playlist with ID {playlistId} not found for user ID {userId}.");
            }

            var playlistTrack = playlist.PlaylistTracks.FirstOrDefault(pt => pt.TrackId == trackId);
            if (playlistTrack == null)
            {
                throw new KeyNotFoundException($"Track with ID {trackId} not found in playlist ID {playlistId}.");
            }

            playlist.PlaylistTracks.Remove(playlistTrack);

            // Reassign positions to maintain sequential order
            var remainingTracks = playlist.PlaylistTracks.OrderBy(pt => pt.Position)
                .ToList();
            for (int i = 0; i < remainingTracks.Count; i++)
            {
                remainingTracks[i].Position = i + 1;
            }

            await _context.SaveChangesAsync();
            return _mapper.Map<PlaylistDto>(playlist);
        }
        public async Task DeletePlaylistAsync(int playlistId, int userId)
        {
            var playlist = await _context.Playlists
                .FirstOrDefaultAsync(p => p.PlaylistId == playlistId && p.UserId == userId);

            if (playlist == null)
            {
                throw new KeyNotFoundException($"Playlist with ID {playlistId} not found for user ID {userId}.");
            }

            _context.Playlists.Remove(playlist);
            await _context.SaveChangesAsync();
        }
        public async Task<List<ShowPlaylistTrackDto>> GetTracksFromPlaylistAsync(int playlistId, int userId)
        {
            var playlist = await _context.Playlists
                .Include(p => p.PlaylistTracks)
                .ThenInclude(pt => pt.Track)
                .ThenInclude(t => t.Album)
                .FirstOrDefaultAsync(p => p.PlaylistId == playlistId &&
                p.UserId == userId);

            if (playlist == null)
            {
                throw new KeyNotFoundException($"Playlist with ID {playlistId} not found for user ID {userId}.");
            }

            var playlistTracks = playlist.PlaylistTracks
                .OrderBy(pt => pt.Position)
                .ToList();
            List<ShowPlaylistTrackDto> playlistTrackDtos = new List<ShowPlaylistTrackDto>();

            foreach (var track in playlistTracks) 
            {
                playlistTrackDtos.Add( _mapper.Map<ShowPlaylistTrackDto>(track));
            }
            return playlistTrackDtos;
        }
    }
}