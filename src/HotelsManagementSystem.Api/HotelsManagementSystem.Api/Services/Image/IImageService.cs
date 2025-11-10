using HotelsManagementSystem.Api.DTOs.Admin.Rooms.Create;
using HotelsManagementSystem.Api.DTOs.Images;

namespace HotelsManagementSystem.Api.Services.Image
{
    public interface IImageService
    {
        public Task<HotelImageUploadResponse> UploadHotelImageAsync(Guid hotelId, IFormFile imageFile);

        public Task<RoomImageUploadResponse> UploadRoomImageAsync(Guid roomId, IFormFile imageFile);

        public Task<bool> DeleteImageAsync(string publicId);
    }
}
