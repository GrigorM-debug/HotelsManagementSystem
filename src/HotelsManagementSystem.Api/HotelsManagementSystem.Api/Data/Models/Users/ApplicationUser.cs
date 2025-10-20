
using HotelsManagementSystem.Api.Constants;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HotelsManagementSystem.Api.Data.Models.Users
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(UserConstants.FirstNameAndLastNameMaxLength,
            MinimumLength = UserConstants.FirstNameAndLastNameMinLength, ErrorMessage = GeneralConstants.ValueRangeErrorMessage)]
        [RegularExpression(UserConstants.FirstNameAndLastNameRegexPattern, ErrorMessage = GeneralConstants.ValueInvalidPatternErrorMessage)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(UserConstants.FirstNameAndLastNameMaxLength,
            MinimumLength = UserConstants.FirstNameAndLastNameMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        [RegularExpression(UserConstants.FirstNameAndLastNameRegexPattern)]
        public string LastName { get; set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";

        public DateTime JoinedOn { get; set; } = DateTime.UtcNow;

        // Navigation properties for role-based relationships
        public Admin? Admin { get; set; }
        public Receptionist? Receptionist { get; set; }
        public Customer? Customer { get; set; }
    }
}
