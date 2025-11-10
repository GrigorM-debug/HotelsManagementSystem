using HotelsManagementSystem.Api.DTOs.Hotels.Details;

namespace HotelsManagementSystem.Api.Services.Hotels
{
    public interface IHotelService
    {
        public Task<bool> HotelExistsByIdAsync(Guid hotelId);
        public Task<HotelDetailsDto> GetHotelDetailsByIdAsync(Guid hotelId);
    }
}
