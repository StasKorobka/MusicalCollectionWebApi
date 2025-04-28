using DataAccess.Models;
using DataAccess.Models.DTO.Album;
using DataAccess.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace MusicalCollectionWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly IAlbumService _albumService;
        private readonly ILogger<AlbumsController> _logger;

        public AlbumsController(IAlbumService albumService, ILogger<AlbumsController> logger)
        {
            _albumService = albumService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<List<AlbumDto>>> GetAllAlbums()
        {
            try
            {
                var albums = await _albumService.GetAllAlbumsAsync();
                _logger.LogInformation("Got all the albums");
                return Ok(albums);
            }
            catch (Exception ex) 
            {
                _logger.LogError($"Error occured : {ex.Message}");
                return BadRequest(ex.Message);
            }           

        }

        [HttpGet("name/{albumName}")]
        public async Task<ActionResult<AlbumDto>> GetAlbumByName(string albumName)
        {
            try
            {
                var album = await _albumService.GetAlbumByNameAsync(albumName);
                _logger.LogInformation($"User searched for album - {albumName}");
                return Ok(album);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: NotFound album - {albumName}");
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error: invalid album name - {albumName}");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{albumId}")]
        public async Task<ActionResult<AlbumDto>> GetAlbumById(int albumId)
        {
            try
            {
                var album = await _albumService.GetAlbumByIdAsync(albumId);
                _logger.LogInformation($"User searched for album with id - {albumId}");
                return Ok(album);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: NotFound album with id - {albumId}");
                return NotFound(ex.Message);
            }
        }
        
        [HttpGet("genre/{genre}")]
        public async Task<ActionResult<List<AlbumDto>>> GetAlbumByGenre(string genre)
        {
            var albums = await _albumService.GetAlbumsByGenreAsync(genre);
            if (albums == null)
            {
                _logger.LogError($"Error: NotFound album with genre - {genre}");
                return NotFound($"There are no albums with {genre} genre");
            }
            return Ok(albums);
        }
    }
}