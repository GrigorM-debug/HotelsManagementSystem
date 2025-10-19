using HotelsManagementSystem.Api.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelsManagementSystem.Api.Data.Models.Hotels
{
    [PrimaryKey(nameof(AmenityId), nameof(HotelId))]
    public class HotelAmenity
    {
        public Guid AmenityId { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [ForeignKey(nameof(AmenityId))]
        public Amenity Amenity { get; set; } = null!;

        public Guid HotelId { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [ForeignKey(nameof(HotelId))]
        public Hotel Hotel { get; set; } = null!;
    }
}
