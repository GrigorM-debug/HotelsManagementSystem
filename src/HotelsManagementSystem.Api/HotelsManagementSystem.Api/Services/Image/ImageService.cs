using CloudinaryDotNet;
using HotelsManagementSystem.Api.Helpers;
using Microsoft.Extensions.Options;
using CloudinaryDotNet.Actions;
using HotelsManagementSystem.Api.DTOs.Images;

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
    }
}
