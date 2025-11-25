using HotelsManagementSystem.Api.Data;
using HotelsManagementSystem.Api.Data.Models.Reviews;
using HotelsManagementSystem.Api.DTOs.Reviews;
using Microsoft.EntityFrameworkCore;

namespace HotelsManagementSystem.Api.Services.Reviews
{
    public class ReviewService : IReviewService
    {
        private readonly ApplicationDbContext _context;

        public ReviewService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateReviewAsync(Guid customerId, Guid hotelId, CreateReviewDto inputDto)
        {
            var newReview = new Review()
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                HotelId = hotelId,
                Rating = inputDto.Rating,
                Comment = inputDto.Comment,
                CreatedOn = DateTime.UtcNow,
            };

            await _context.Reviews.AddAsync(newReview);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ReviewAlreadyExistsByCustomerIdAndHotelIdAsync(Guid customerId, Guid hotelId)
        {
            var reviewExists = await _context
                .Reviews
                .AnyAsync(r => 
                    r.HotelId == hotelId && 
                    r.CustomerId == customerId && 
                    !r.IsDeleted);

            return reviewExists;
        }
    }
}
