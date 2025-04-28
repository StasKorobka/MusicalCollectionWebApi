using DataAccess.Models.DTO.Track;
using DataAccess.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MusicalCollectionWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TracksController : ControllerBase
    {
        private readonly ITrackService _trackService;
        private readonly ILogger<TracksController> _logger;
        public TracksController(ITrackService trackService, ILogger<TracksController> logger)
        {
            _trackService = trackService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<TrackDto>>> GetAllTracks()
        {
            var tracks = await _trackService.GetAllTracksAsync();
            _logger.LogInformation($"User got all tracks");
            return Ok(tracks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TrackDto>> GetTrackById(int id)
        {
            try
            {
                var track = await _trackService.GetTrackByIdAsync(id);
                _logger.LogInformation($"User searched for track with id: {id}");
                return Ok(track);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: track with id - {id} not found");
                return NotFound(ex.Message);
            }
        }

        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<TrackDto>> GetTrackByName(string name)
        {
            try
            {
                var track = await _trackService.GetTrackByNameAsync(name);
                _logger.LogInformation($"User searched for track: {name}");
                return Ok(track);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: track with name - {name} not found");
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error: Invalid name - {name} ");
                return BadRequest(ex.Message);
            }
        }

    }
}