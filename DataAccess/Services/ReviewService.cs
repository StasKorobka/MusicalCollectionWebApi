using DataAccess.Models.DTO.Review;
using DataAccess.Models;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using DataAccess.Services.Interfaces;

namespace DataAccess.Services
{
    public class ReviewService : IReviewService
    {
        private readonly MusicalCollectionDbContext _context;
        private readonly IMapper _mapper;

        public ReviewService(MusicalCollectionDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ReviewDto> AddReviewAsync(int userId, AddReviewDto addReviewDto)
        {
            // verify user exists
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            // verify album exists
            var album = await _context.Albums.FindAsync(addReviewDto.AlbumId);
            if (album == null)
            {
                throw new KeyNotFoundException($"Album with ID {addReviewDto.AlbumId} not found.");
            }

            // check for existing review by user for this album
            var existingReview = await _context.Reviews
                .FirstOrDefaultAsync(r => r.UserId == userId 
                && r.AlbumId == addReviewDto.AlbumId);
            if (existingReview != null)
            {
                throw new InvalidOperationException($"User with ID {userId} has already reviewed album with ID {addReviewDto.AlbumId}.");
            }

            var review = new Review
            {
                UserId = userId,
                AlbumId = addReviewDto.AlbumId,
                Rating = addReviewDto.Rating,
                Comment = addReviewDto.Comment
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return _mapper.Map<ReviewDto>(review);
        }
        public async Task<ReviewDto> EditReviewAsync(int reviewId, int userId, EditReviewDto editReviewDto)
        {
            // find review and verify it belongs to the user
            var review = await _context.Reviews
                .FirstOrDefaultAsync(r => r.ReviewId == reviewId && r.UserId == userId);
            if (review == null)
            {
                throw new KeyNotFoundException($"Review with ID {reviewId} not found for user ID {userId}.");
            }

            review.Rating = editReviewDto.Rating;
            review.Comment = editReviewDto.Comment;

            await _context.SaveChangesAsync();
            return _mapper.Map<ReviewDto>(review);
        }
        public async Task DeleteReviewAsync(int reviewId, int userId)
        {
            var review = await _context.Reviews
                .FirstOrDefaultAsync(p => p.ReviewId == reviewId && p.UserId == userId);

            if (review == null)
            {
                throw new KeyNotFoundException($"Review with ID {reviewId} not found for user ID {userId}.");
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
        }
        public async Task<List<ReviewDto>> GetAllReviewsAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            var reviews = await _context.Reviews
                .Where(r => r.UserId == userId)
                .ToListAsync();
            List<ReviewDto> reviewDtos = new List<ReviewDto>();
            foreach (var review in reviews) 
            {
                reviewDtos.Add(_mapper.Map<ReviewDto>(review));
            }

            return reviewDtos;
        }
    }
}