using HotelsManagementSystem.Api.Constants;
using HotelsManagementSystem.Api.Data.Models.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HotelsManagementSystem.Api.Data.Models.Rooms;
using HotelsManagementSystem.Api.Data.Models.Images;
using HotelsManagementSystem.Api.Data.Models.Reviews;

namespace HotelsManagementSystem.Api.Data.Models.Hotels
{
    public class Hotel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(HotelConstants.NameMaxLength,
            MinimumLength = HotelConstants.NameMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        [RegularExpression(HotelConstants.NameRegexPattern)]
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

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedOn { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        [Range(HotelConstants.StarsMinValue, HotelConstants.StarsMaxValue, ErrorMessage = GeneralConstants.ValueRangeErrorMessage)]
        public int Stars { get; set; }

        public bool IsDeleted { get; set; } = false;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        public TimeSpan CheckInTime { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        public TimeSpan CheckOutTime { get; set; }

        public Guid CreatorId { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [ForeignKey(nameof(CreatorId))]
        public Admin Creator { get; set; } = null!;

        // Navigation properties
        public ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public ICollection<HotelImage> Images { get; set; } = new HashSet<HotelImage>();
        public ICollection<Room> Rooms { get; set; } = new HashSet<Room>();
        public ICollection<HotelAmenity> HotelAmenities { get; set; } = new HashSet<HotelAmenity>();
        public ICollection<Receptionist> Receptionists { get; set; } = new HashSet<Receptionist>();
    }
}
