using HotelsManagementSystem.Api.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelsManagementSystem.Api.Data.Models.Rooms
{
    [PrimaryKey(nameof(FeatureId), nameof(RoomId))]
    public class RoomFeature
    {
        public Guid FeatureId { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [ForeignKey(nameof(FeatureId))]
        public Feature Feature { get; set; } = null!;

        public Guid RoomId { get; set; }

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [ForeignKey(nameof(RoomId))]
        public Room Room { get; set; } = null!;
    }
}
