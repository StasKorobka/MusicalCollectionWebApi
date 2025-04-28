using DataAccess.Models.DTO.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    public interface IReviewService
    {
        Task<List<ReviewDto>> GetAllReviewsAsync(int userId);
        Task<ReviewDto> AddReviewAsync(int userId, AddReviewDto addReviewDto);
        Task<ReviewDto> EditReviewAsync(int reviewId, int userId, EditReviewDto editReviewDto);
        Task DeleteReviewAsync(int reviewId, int userId);
    }
}
