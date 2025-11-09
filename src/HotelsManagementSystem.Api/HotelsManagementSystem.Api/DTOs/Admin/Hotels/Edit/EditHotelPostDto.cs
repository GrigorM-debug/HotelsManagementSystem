using HotelsManagementSystem.Api.Constants;
using System.ComponentModel.DataAnnotations;

namespace HotelsManagementSystem.Api.DTOs.Admin.Hotels.Edit
{
    public class EditHotelPostDto : IValidatableObject
    {
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public int? Stars { get; set; }
        public TimeSpan? CheckInTime { get; set; }
        public TimeSpan? CheckOutTime { get; set; }
        public IEnumerable<Guid> AmenityIds { get; set; } = new List<Guid>();
        public IEnumerable<IFormFile> NewImages { get; set; } = new List<IFormFile>();
        public IEnumerable<Guid> ExistingImagesIds { get; set; } = new List<Guid>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //Validate Name if provided
            if (!string.IsNullOrEmpty(Name))
            {
                if(Name.Length < HotelConstants.NameMinLength || Name.Length > HotelConstants.NameMaxLength)
                {
                    yield return new ValidationResult(
                        $"Hotel name must be between {HotelConstants.NameMinLength} and {HotelConstants.NameMaxLength} characters long.",
                        new[] { nameof(Name) });
                }

                if (!System.Text.RegularExpressions.Regex.IsMatch(Name, HotelConstants.NameRegexPattern))
                {
                    yield return new ValidationResult(
                        GeneralConstants.ValueInvalidPatternErrorMessage,
                        new[] { nameof(Name) });
                }

            }

            // Validate Description if provided
            if (!string.IsNullOrEmpty(Description))
            {
                if(Description.Length < HotelConstants.DescriptionMinLength || Description.Length > HotelConstants.DescriptionMaxLength)
                {
                    yield return new ValidationResult(
                        $"Description must be between {HotelConstants.DescriptionMinLength} and {HotelConstants.DescriptionMaxLength} characters long.",
                        new[] { nameof(Description) });
                }
            }

            // Validate CheckInTime and CheckOutTime if both provided
            if (CheckInTime.HasValue && CheckOutTime.HasValue)
            {
                if (CheckOutTime <= CheckInTime)
                {
                    yield return new ValidationResult(
                        "Check-out time cannot be earlier than or equal to check-in time.",
                        new[] { nameof(CheckOutTime), nameof(CheckInTime) });
                }
            }

            // Validate Images count
            if (NewImages.Count() > GeneralConstants.ImageUploadMaxCount)
            {
                yield return new ValidationResult(
                    $"A hotel cannot have more than {GeneralConstants.ImageUploadMaxCount} images.",
                    new[] { nameof(Images) });
            }

            foreach (var image in NewImages)
            {
                if (!GeneralConstants.AllowedImageTypes.Contains(image.ContentType))
                {
                    yield return new ValidationResult(
                        $"Image type {image.ContentType} is not allowed. Allowed types are: {string.Join(", ", GeneralConstants.AllowedImageTypes)}",
                        new[] { nameof(Images) });
                }
            }
        }
    }
}
