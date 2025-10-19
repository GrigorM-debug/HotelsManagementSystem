using HotelsManagementSystem.Api.Constants;
using System.ComponentModel.DataAnnotations;

namespace HotelsManagementSystem.Api.Data.Models.Hotels
{
    public class Amenity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(AmenityConstants.NameMaxLength,
            MinimumLength = AmenityConstants.NameMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        [RegularExpression(AmenityConstants.NameRegexPattern, ErrorMessage = GeneralConstants.ValueInvalidPatternErrorMessage)]
        public string Name { get; set; } = string.Empty;

        // Navigation properties
        public ICollection<HotelAmenity> HotelAmenities { get; set; } = new HashSet<HotelAmenity>();
    }
}
