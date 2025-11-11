using HotelsManagementSystem.Api.Constants;
using System.ComponentModel.DataAnnotations;

namespace HotelsManagementSystem.Api.DTOs.Admin.Rooms.Create
{
    public class CreateRoomPostDto : IValidatableObject
    {
        [Range(RoomConstants.RoomNumberMinValue, RoomConstants.RoomNumberMaxValue, ErrorMessage = GeneralConstants.ValueRangeErrorMessage)]
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(RoomConstants.DescriptionMaxLength,
            MinimumLength = RoomConstants.DescriptionMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        public string Description { get; set; } = string.Empty;
        
        public IEnumerable<IFormFile> Images { get; set; } = new List<IFormFile>();
        
        public IEnumerable<Guid> FeatureIds { get; set; } = new List<Guid>();
        
        public Guid RoomTypeId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Images.Count() > GeneralConstants.ImageUploadMaxCount)
            {
                yield return new ValidationResult(
                    $"You can upload up to {GeneralConstants.ImageUploadMaxCount} images.",
                    new[] { nameof(Images) });
            }

            foreach (var image in Images)
            {
                if (!GeneralConstants.AllowedImageTypes.Contains(image.ContentType))
                {
                    yield return new ValidationResult(
                        $"Image type {image.ContentType} is not allowed. Allowed types are: {string.Join(", ", GeneralConstants.AllowedImageTypes)}",
                        new[] { nameof(Images) });
                }
            }

            if (FeatureIds.Count() == 0)
            {
                yield return new ValidationResult("At least one feature must be selected.", new[] { nameof(FeatureIds) });
            }
        }
    }
}
