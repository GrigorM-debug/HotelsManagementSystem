using HotelsManagementSystem.Api.DTOs.Admin.Hotels;

namespace HotelsManagementSystem.Api.Services.Admin.Hotels
{
    public interface IHotelService
    {
        public Task<bool> HotelExistsByNameAsync(string hotelName);

        public Task<Guid> CreateHotelAsync(CreateHotelDto inputDto, Guid adminId);
    }
}
