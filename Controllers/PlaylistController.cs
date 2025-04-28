using DataAccess.Models.DTO.Playlist;
using DataAccess.Models.DTO.PlaylistTrack;
using DataAccess.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MusicalCollectionWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController : ControllerBase
    {
        private readonly IPlaylistService _playlistService;
        private readonly ILogger<PlaylistController> _logger;
        public PlaylistController(IPlaylistService playlistService, 
            ILogger<PlaylistController> logger)
        {
            _playlistService = playlistService;
            _logger = logger;
        }

        [HttpGet("users/{userId}/playlists")]
        public async Task<ActionResult<List<PlaylistDto>>> GetPlaylistsByUserId(int userId)
        {
            var playlists = await _playlistService.GetPlaylistsByUserIdAsync(userId);
            if (playlists == null) 
            {
                _logger.LogError($"Error: User {userId} doesn't have playlists");
                return NotFound();//ch
            }
            _logger.LogInformation($"User got all playlists");
            return Ok(playlists);
        }

        
        [HttpGet("users/{userId}/playlists/{playlistId}")]
        public async Task<ActionResult<PlaylistDto>> GetPlaylistByIdAndUserId(int playlistId, int userId)
        {
            try
            {
                var playlist = await _playlistService.GetPlaylistByIdAndUserIdAsync(playlistId, userId);
                _logger.LogInformation($"User {userId} got playlist {playlistId}");
                return Ok(playlist);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: Playlist({playlistId}) or user({userId}) doesn't exitst");
                return NotFound(ex.Message);
            }
        }
        
        
        [HttpGet("{playlistId}/user/{userId}/tracks")]
        public async Task<ActionResult<List<PlaylistTrackDto>>> GetTracksFromPlaylist(int playlistId, int userId)
        {
            try
            {
                var tracks = await _playlistService.GetTracksFromPlaylistAsync(playlistId, userId);
                _logger.LogInformation($"User {userId} got all tracks from playlist {playlistId}");
                return Ok(tracks);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: Playlist({playlistId}) or user({userId}) doesn't exitst");
                return NotFound(ex.Message);
            }
        }
        
        
        [HttpPost("users/{userId}/playlists")]
        public async Task<ActionResult<PlaylistDto>> CreatePlaylist(int userId, [FromBody] CreatePlaylistDto createDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Error: Playlist is ivalid");
                return BadRequest(ModelState);
            }

            try
            {
                var playlist = await _playlistService.CreatePlaylistAsync(userId, createDto);
                _logger.LogInformation($"User {userId} Created playlist");
                return CreatedAtAction(nameof(GetPlaylistByIdAndUserId),
                    new { playlistId = playlist.PlaylistId, userId }, playlist);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: Invalid playlist");
                return NotFound(ex.Message);
            }
        }

        
        [HttpPut("users/{userId}/playlists/{playlistId}")]
        public async Task<ActionResult<PlaylistDto>> UpdatePlaylist(int playlistId, int userId, [FromBody] UpdatePlaylistDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Error: Playlist is ivalid");
                return BadRequest(ModelState);
            }

            try
            {
                var playlist = await _playlistService.UpdatePlaylistAsync(playlistId, userId, updateDto);
                _logger.LogInformation($"User({userId}) updated playlist({playlistId})");
                return Ok(playlist);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: Invalid playlist");
                return NotFound(ex.Message);
            }
        }
        
       
        [HttpPost("users/{userId}/playlists/{playlistId}/tracks")]
        public async Task<ActionResult<PlaylistDto>> AddTrackToPlaylist(int playlistId, int userId, [FromBody] AddTrackToPlaylistDto addTrackDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Error: Playlist is ivalid");
                return BadRequest(ModelState);
            }

            try
            {
                var playlist = await _playlistService.AddTrackToPlaylistAsync(playlistId, userId, addTrackDto);
                _logger.LogInformation($"User({userId}) Added track to playlist({playlistId})");
                return Ok(playlist);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: User({userId}) or playlist({playlistId}) not found");
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Error: Playlist is ivalid");
                return Conflict(ex.Message);
            }
        }

        
        [HttpDelete("users/{userId}/playlists/{playlistId}/tracks/{trackId}")]
        public async Task<ActionResult<PlaylistDto>> DeleteTrackFromPlaylist(int playlistId, int userId, int trackId)
        {
            try
            {
                var playlist = await _playlistService.DeleteTrackFromPlaylistAsync(playlistId, userId, trackId);
                _logger.LogInformation($"User({userId}) Deleted track from playlist({playlistId})");
                return Ok(playlist);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: User({userId}) or playlist({playlistId}) not found");
                return NotFound(ex.Message);
            }
        }

        
        [HttpDelete("users/{userId}/playlists/{playlistId}")]
        public async Task<IActionResult> DeletePlaylist(int playlistId, int userId)
        {
            try
            {
                await _playlistService.DeletePlaylistAsync(playlistId, userId);
                _logger.LogInformation($"User({userId}) Deleted playlist({playlistId})");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: User({userId}) or playlist({playlistId}) not found");
                return NotFound(ex.Message);
            }
        }
    }
}