using DataAccess.Models.DTO.UserCollection;

namespace DataAccess.Services
{
    public interface IUserCollectionService
    {
        Task<List<UserCollectionDto>> GetUserCollectionByUserIdAsync(int userId);
        Task<List<UserCollectionDto>> GetWishlistAsync(int userId);
        Task<List<UserCollectionDto>> GetBoughtUserCollectionAsync(int userId);
        Task<UserCollectionDto> AddAlbumToWishlistAsync(int userId, AddAlbumToWishlistDto wishlistDto);
        Task<UserCollectionDto> BuyAlbumAsync(int userId, BuyAlbumDto buyDto);
    }
}