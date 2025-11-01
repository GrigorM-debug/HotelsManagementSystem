﻿using HotelsManagementSystem.Api.DTOs.Admin.Hotels;
using HotelsManagementSystem.Api.DTOs.Hotels;

namespace HotelsManagementSystem.Api.Services.Admin.Hotels
{
    public interface IHotelService
    {
        public Task<bool> HotelExistsByNameAsync(string hotelName);

        public Task<Guid> CreateHotelAsync(CreateHotelDto inputDto, Guid adminId);

        public Task<IEnumerable<HotelListDto>> GetAdminHotelsAsync(Guid adminId, HotelsFilterDto? filter);

        public Task<bool> IsHotelDeletableAsync(Guid hotelId);
    }
}
