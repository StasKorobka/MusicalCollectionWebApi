using DataAccess.Models.DTO.UserCollection;
using DataAccess.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MusicalCollectionWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCollectionsController : ControllerBase
    {
        private readonly IUserCollectionService _userCollectionService;
        private readonly ILogger<UserCollectionsController> _logger;

        public UserCollectionsController(IUserCollectionService userCollectionService,
            ILogger<UserCollectionsController> logger)
        {
            _userCollectionService = userCollectionService;
            _logger = logger;
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<UserCollectionDto>>> GetUserCollectionByUserId(int userId)
        {
            try
            {
                var collections = await _userCollectionService.GetUserCollectionByUserIdAsync(userId);
                _logger.LogInformation($"User({userId}) got all userCollections");
                return Ok(collections);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: User({userId}) is not found");
                return NotFound(ex.Message);
            }
        }
        [HttpGet("{userId}/wishlist")]
        public async Task<ActionResult<List<UserCollectionDto>>> GetWishlist(int userId)
        {
            try
            {
                var wishlist = await _userCollectionService.GetWishlistAsync(userId);
                if (wishlist == null) 
                {
                    _logger.LogWarning($"User's({userId}) wishlist is empty");
                    return NotFound();
                }
                _logger.LogInformation($"User({userId}) got wishlist");
                return Ok(wishlist);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: User({userId}) is not found");
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error: Operation failed");
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{userId}/purchased")]
        public async Task<ActionResult<List<UserCollectionDto>>> GetBoughtUserCollection(int userId)
        {
            try
            {
                var boughtAlbums = await _userCollectionService.GetBoughtUserCollectionAsync(userId);
                _logger.LogInformation($"User({userId}) got a list of bought albums");
                return Ok(boughtAlbums);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: User({userId}) is not found");
                return NotFound(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error: Operation failed");
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("user/{userId}/addToWishlist")]
        public async Task<ActionResult<UserCollectionDto>> AddAlbumToWishlist(int userId, [FromBody] AddAlbumToWishlistDto wishlistDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Error: Ivalid albumm");
                return BadRequest(ModelState);
            }
            try
            {
                var collection = await _userCollectionService.
                    AddAlbumToWishlistAsync(userId, wishlistDto);

                _logger.LogInformation($"User({userId}) Added album to wishlist");
                return CreatedAtAction(nameof(GetUserCollectionByUserId),
                    new { userId }, collection);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: User({userId}) or album is not found");
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Error: Operation failed");
                return Conflict(ex.Message);
            }
        }
        [HttpPost("user/{userId}/purchase")]
        public async Task<ActionResult<UserCollectionDto>> BuyAlbum(int userId, [FromBody] BuyAlbumDto buyDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Error: Ivalid albumm");
                return BadRequest(ModelState);
            }

            try
            {
                var collection = await _userCollectionService.BuyAlbumAsync(userId, buyDto);

                _logger.LogInformation($"User({userId}) Bought album");
                return CreatedAtAction(nameof(GetUserCollectionByUserId),
                    new { userId }, collection);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: User({userId}) or album is not found");
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Error: Ivalid Operation");
                return Conflict(ex.Message);
            }
        }
        [HttpDelete("{userId}/wishlist/{albumId}")]
        public async Task<ActionResult> DeleteFromWishlist(int userId, int albumId)
        {
            try
            {
                await _userCollectionService.DeleteFromWishlistAsync(userId, albumId);
                _logger.LogInformation($"User's({userId}) Album({albumId}) deleted from wishlist");
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"Error: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }
    }
}