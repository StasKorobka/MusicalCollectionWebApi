using DataAccess.Models.DTO.Artist;
using DataAccess.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MusicalCollectionWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistService _artistService;
        private readonly ILogger<ArtistsController> _logger;
        public ArtistsController(IArtistService artistService, ILogger<ArtistsController> logger)
        {
            _artistService = artistService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<ArtistDto>>> GetAllArtists()
        {
            var artists = await _artistService.GetAllArtistsAsync();
            _logger.LogInformation("User got all artists");
            return Ok(artists);
        }

        [HttpGet("by-name/{name}")]
        public async Task<ActionResult<ArtistDto>> GetArtistByName(string name)
        {
            try
            {
                var artist = await _artistService.GetArtistByNameAsync(name);
                _logger.LogInformation($"User searched for artist with name: {name}");
                return Ok(artist);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: NotFound artist  - {name}");
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error: artist name is invalid - {name}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{artistId}")]
        public async Task<ActionResult<ArtistDto>> GetArtistByIdAsync(int artistId)
        {
            try
            {
                var artist = await _artistService.GetArtistByIdAsync(artistId);
                _logger.LogInformation($"User searched for artist with id - {artistId}");
                return Ok(artist);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: NotFound artist with id - {artistId}");
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error: artist id is invalid - {artistId}");
                return BadRequest(ex.Message);
            }
        }

    }
}
