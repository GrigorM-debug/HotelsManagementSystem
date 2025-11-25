using HotelsManagementSystem.Api.DTOs.Reviews;

namespace HotelsManagementSystem.Api.Services.Reviews
{
    public interface IReviewService
    {
        public Task<bool> ReviewAlreadyExistsByCustomerIdAndHotelIdAsync(Guid customerId,  Guid hotelId);

        public Task<bool> CreateReviewAsync(Guid customerId, Guid hotelId, CreateReviewDto inputDto);
    }
}
