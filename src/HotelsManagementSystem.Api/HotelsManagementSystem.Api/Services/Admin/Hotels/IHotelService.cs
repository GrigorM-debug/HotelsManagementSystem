using HotelsManagementSystem.Api.DTOs.Admin.Hotels;
using HotelsManagementSystem.Api.DTOs.Admin.Hotels.Edit;
using HotelsManagementSystem.Api.DTOs.Hotels;

namespace HotelsManagementSystem.Api.Services.Admin.Hotels
{
    public interface IHotelService
    {
        public Task<bool> HotelExistsByNameAsync(string hotelName);

        public Task<Guid> CreateHotelAsync(CreateHotelDto inputDto, Guid adminId);

        public Task<IEnumerable<HotelListDto>> GetAdminHotelsAsync(Guid adminId, HotelsFilterDto? filter);

        public Task<bool> IsHotelDeletableAsync(Guid hotelId);

        public Task<bool> HotelExistsByHotelIdAndAdminIdAsync(Guid hotelId, Guid adminId);

        public Task<bool> DeleteHotelAsync(Guid hotelId, Guid adminId);

        public Task<EditHotelGetDto> GetHotelForEditByIdAsync(Guid hotelId, Guid adminId);

        public Task<bool> EditHotelPostAsync(EditHotelPostDto inputDto, Guid adminId, Guid hotelId);
    }
}
