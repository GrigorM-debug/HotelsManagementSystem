using HotelsManagementSystem.Api.DTOs.Images;

namespace HotelsManagementSystem.Api.Services.Image
{
    public interface IImageService
    {
        public Task<HotelImageUploadResponse> UploadHotelImageAsync(Guid hotelId, IFormFile imageFile);
    }
}
