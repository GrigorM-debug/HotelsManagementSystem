using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using HotelsManagementSystem.Api.Data.Models.Hotels;
using HotelsManagementSystem.Api.DTOs.Admin.Rooms.Create;
using HotelsManagementSystem.Api.DTOs.Images;
using HotelsManagementSystem.Api.Helpers;
using Microsoft.Extensions.Options;

namespace HotelsManagementSystem.Api.Services.Image
{
    public class ImageService : IImageService
    {
        private readonly Cloudinary _claudinary;

        public ImageService(IOptions<CloudinarySettings> cloudinarySettings)
        {
            var account = new Account(
                cloudinarySettings.Value.CloudName,
                cloudinarySettings.Value.ApiKey,
                cloudinarySettings.Value.ApiSecret);

            _claudinary = new Cloudinary(account);
        }

        public async Task<bool> DeleteImageAsync(string publicId)
        {
           var deletionParams = new DeletionParams(publicId);
            var deletionResult = await _claudinary.DestroyAsync(deletionParams);
            if(deletionResult.StatusCode != System.Net.HttpStatusCode.OK && deletionResult.Result != "not found")
            {
                throw new Exception("Image deletion failed.");
            }
            return await Task.FromResult(true);
        }

        public async Task<HotelImageUploadResponse> UploadHotelImageAsync(Guid hotelId, IFormFile imageFile)
        {
            if(imageFile.Length <= 0)
            {
                throw new Exception("Invalid image file.");
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageFile.FileName, imageFile.OpenReadStream()),
                Folder = $"hotels/{hotelId}",
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("auto").Quality("auto")
            };

            var uploadResult = await _claudinary.UploadAsync(uploadParams);

            if(uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Image upload failed.");
            }

            var hotelImage = new HotelImageUploadResponse()
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString(),
                HotelId = hotelId
            };

            return hotelImage;
        }

        public async Task<RoomImageUploadResponse> UploadRoomImageAsync(Guid roomId, IFormFile imageFile)
        {
            if (imageFile.Length <= 0)
            {
                throw new Exception("Invalid image file.");
            }

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageFile.FileName, imageFile.OpenReadStream()),
                Folder = $"rooms/{roomId}",
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("auto").Quality("auto")
            };

            var uploadResult = await _claudinary.UploadAsync(uploadParams);

            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception("Image upload failed.");
            }

            var roomImage = new RoomImageUploadResponse()
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.ToString(),
                RoomId = roomId
            };

            return roomImage;
        }
    }
}
