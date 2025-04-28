using DataAccess.Models.DTO.Review;

namespace DataAccess.Services.Interfaces
{
    public interface IReviewService
    {
        Task<List<ReviewDto>> GetAllReviewsAsync(int userId);
        Task<ReviewDto> AddReviewAsync(int userId, AddReviewDto addReviewDto);
        Task<ReviewDto> EditReviewAsync(int reviewId, int userId, EditReviewDto editReviewDto);
        Task DeleteReviewAsync(int reviewId, int userId);
    }
}
