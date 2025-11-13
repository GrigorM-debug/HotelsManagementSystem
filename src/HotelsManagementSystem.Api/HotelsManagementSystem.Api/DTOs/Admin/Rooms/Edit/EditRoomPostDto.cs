using HotelsManagementSystem.Api.Constants;
using System.ComponentModel.DataAnnotations;

namespace HotelsManagementSystem.Api.DTOs.Admin.Rooms.Edit
{
    public class EditRoomPostDto : IValidatableObject
    {
        [Range(RoomConstants.RoomNumberMinValue, RoomConstants.RoomNumberMaxValue, ErrorMessage = GeneralConstants.ValueRangeErrorMessage)]
        public int RoomNumber { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(RoomConstants.DescriptionMaxLength,
            MinimumLength = RoomConstants.DescriptionMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        public string Description { get; set; } = string.Empty;
        public IEnumerable<Guid> FeatureIds { get; set; } = new List<Guid>();
        public Guid RoomTypeId { get; set; }
        public IEnumerable<Guid> ExistingImagesIds { get; set; } = new List<Guid>();
        public IEnumerable<IFormFile> NewImages { get; set; } = new List<IFormFile>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var totalImagesCount = ExistingImagesIds.Count() + NewImages.Count();
            if (totalImagesCount == 0)
            {
                yield return new ValidationResult(
                    "At least one image must be associated with the room.",
                    new[] { nameof(ExistingImagesIds), nameof(NewImages) });
            }

            if(NewImages.Count() > GeneralConstants.ImageUploadMaxCount)
            {
                yield return new ValidationResult(
                    $"You can upload up to {GeneralConstants.ImageUploadMaxCount} images.",
                    new[] { nameof(NewImages) });
            }

            foreach (var image in NewImages)
            {
                if (!GeneralConstants.AllowedImageTypes.Contains(image.ContentType))
                {
                    yield return new ValidationResult(
                        $"Image type {image.ContentType} is not allowed. Allowed types are: {string.Join(", ", GeneralConstants.AllowedImageTypes)}",
                        new[] { nameof(NewImages) });
                }
            }

            if (FeatureIds.Count() == 0)
            {
                yield return new ValidationResult(
                    "At least one feature must be selected.",
                    new[] { nameof(FeatureIds) });
            }
        }
    }
}
