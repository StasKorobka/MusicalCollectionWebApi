using AutoMapper;
using DataAccess.Data;
using DataAccess.Models;
using DataAccess.Models.DTO.UserCollection;
using DataAccess.Models.Enums;
using DataAccess.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Services
{
    public class UserCollectionService : IUserCollectionService
    {
        private readonly MusicalCollectionDbContext _context;
        private readonly IMapper _mapper;

        public UserCollectionService(MusicalCollectionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserCollectionDto>> GetUserCollectionByUserIdAsync(int userId)
        {
            //user validation
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            List<UserCollectionDto> userCollectionDtos = new List<UserCollectionDto>();
            var collections = await _context.UserCollections
                .Include(uc => uc.Album)
                .Where(uc => uc.UserId == userId)
                .ToListAsync();

            foreach (var collection in collections) 
            {
                userCollectionDtos.Add(_mapper.Map<UserCollectionDto>(collection));
            }

            return userCollectionDtos;
        }
        public async Task<List<UserCollectionDto>> GetWishlistAsync(int userId)
        {
            //validate userId
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be a positive integer.");
            }

            //verify user exists
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            //query wishlist entries
            var collections = await _context.UserCollections
                .Include(uc => uc.Album)
                .ThenInclude(a => a.Artist)
                .Where(uc => uc.UserId == userId &&
                uc.Status == CollectionStatus.Wishlist)
                .ToListAsync();
            List<UserCollectionDto> userCollectionDtos = new List<UserCollectionDto>();

            foreach (var collection in collections)
            {
                userCollectionDtos.Add(_mapper.Map<UserCollectionDto>(collection));
            }
            return userCollectionDtos;
        }
        public async Task<List<UserCollectionDto>> GetBoughtUserCollectionAsync(int userId)
        {
            //validate userId
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be a positive integer.");
            }

            //verify user exists
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            //query wishlist entries
            var collections = await _context.UserCollections
                .Include(uc => uc.Album)
                .ThenInclude(a => a.Artist)
                .Where(uc => uc.UserId == userId &&
                uc.Status == CollectionStatus.Purchased)
                .ToListAsync();
            List<UserCollectionDto> userCollectionDtos = new List<UserCollectionDto>();

            foreach (var collection in collections)
            {
                userCollectionDtos.Add(_mapper.Map<UserCollectionDto>(collection));
            }
            return userCollectionDtos;
        }
        public async Task<UserCollectionDto> AddAlbumToWishlistAsync(int userId, AddAlbumToWishlistDto wishlistDto)
        {
            // verify that user exists
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            // verify that album exists
            var album = await _context.Albums.FindAsync(wishlistDto.AlbumId);
            if (album == null)
            {
                throw new KeyNotFoundException($"Album with ID {wishlistDto.AlbumId} not found.");
            }

            // check if album is already in user's collection (wishlist or purchased)
            var existingCollection = await _context.UserCollections
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.AlbumId == wishlistDto.AlbumId);
            if (existingCollection != null)
            {
                throw new InvalidOperationException($"Album with ID {wishlistDto.AlbumId} is already in the user's collection as {existingCollection.Status}.");
            }

            // add to wishlist
            var collection = new UserCollection
            {
                UserId = userId,
                AlbumId = wishlistDto.AlbumId,
                AdditionDate = DateTime.UtcNow,
                Status = CollectionStatus.Wishlist
            };

            _context.UserCollections.Add(collection);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserCollectionDto>(collection);
        }
        public async Task<UserCollectionDto> BuyAlbumAsync(int userId, BuyAlbumDto buyDto)
        {
            // verify user exists
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            // verify album exists
            var album = await _context.Albums.FindAsync(buyDto.AlbumId);
            if (album == null)
            {
                throw new KeyNotFoundException($"Album with ID {buyDto.AlbumId} not found.");
            }

            // check if album is already purchased
            var existingCollection = await _context.UserCollections
                .FirstOrDefaultAsync(uc => uc.UserId == userId &&
                uc.AlbumId == buyDto.AlbumId);
            if (existingCollection != null)
            {
                if (existingCollection.Status == CollectionStatus.Purchased)
                {
                    throw new InvalidOperationException($"Album with ID {buyDto.AlbumId} is already purchased by the user.");
                }
                // if in wishlist, update to purchased
                existingCollection.Status = CollectionStatus.Purchased;
                existingCollection.AdditionDate = DateTime.UtcNow;
            }
            else
            {
                // add as purchased
                existingCollection = new UserCollection
                {
                    UserId = userId,
                    AlbumId = buyDto.AlbumId,
                    AdditionDate = DateTime.UtcNow,
                    Status = CollectionStatus.Purchased
                };
                _context.UserCollections.Add(existingCollection);
            }

            await _context.SaveChangesAsync();
            return _mapper.Map<UserCollectionDto>(existingCollection);
        }
        public async Task DeleteFromWishlistAsync(int userId, int albumId)
        {
            // Validate inputs
            if (userId <= 0)
            {
                throw new ArgumentException("User ID must be a positive integer.");
            }
            if (albumId <= 0)
            {
                throw new ArgumentException("Album ID must be a positive integer.");
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            //check if album exists
            var album = await _context.Albums.FindAsync(albumId);
            if (album == null)
            {
                throw new KeyNotFoundException($"Album with ID {albumId} not found.");
            }

            //check if album is in wishlist
            var collection = await _context.UserCollections
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.AlbumId == albumId && uc.Status == CollectionStatus.Wishlist);
            if (collection == null)
            {
                throw new InvalidOperationException($"Album with ID {albumId} is not in the user's wishlist.");
            }

            //delete the wishlist entry
            _context.UserCollections.Remove(collection);
            await _context.SaveChangesAsync();
        }
    }
}