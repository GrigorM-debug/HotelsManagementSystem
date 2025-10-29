using HotelsManagementSystem.Api.Constants;
using System.ComponentModel.DataAnnotations;

namespace HotelsManagementSystem.Api.DTOs.Admin.Hotels
{
    public class CreateHotelDto : IValidatableObject
    {
        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(HotelConstants.NameMaxLength,
            MinimumLength = HotelConstants.NameMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        [RegularExpression(HotelConstants.NameRegexPattern, ErrorMessage = GeneralConstants.ValueInvalidPatternErrorMessage)]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(HotelConstants.DescriptionMaxLength,
            MinimumLength = HotelConstants.DescriptionMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(GeneralConstants.AddressMaxLength,
            MinimumLength = GeneralConstants.AddressMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        [RegularExpression(GeneralConstants.AddressRegexPattern, ErrorMessage = GeneralConstants.ValueInvalidPatternErrorMessage)]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(GeneralConstants.CityMaxLength,
            MinimumLength = GeneralConstants.CityMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        [RegularExpression(GeneralConstants.CityRegexPattern, ErrorMessage = GeneralConstants.ValueInvalidPatternErrorMessage)]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(GeneralConstants.CountryMaxLength,
            MinimumLength = GeneralConstants.CountryMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        [RegularExpression(GeneralConstants.CountryRegexPattern, ErrorMessage = GeneralConstants.ValueInvalidPatternErrorMessage)]
        public string Country { get; set; } = string.Empty;
        
        [Range(HotelConstants.StarsMinValue, HotelConstants.StarsMaxValue, ErrorMessage = GeneralConstants.ValueRangeErrorMessage)]
        public int Stars { get; set; }

        public TimeSpan CheckInTime { get; set; }

        public TimeSpan CheckOutTime { get; set; }

        public List<Guid> AmenityIds { get; set; } = new List<Guid>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(CheckOutTime <= CheckInTime)
            {
                yield return new ValidationResult("Check-out time cannot be earlier than or equal to check-in time.", new[] { nameof(CheckOutTime), nameof(CheckInTime) });
            }
        }
    }
}
