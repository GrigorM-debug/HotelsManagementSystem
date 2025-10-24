using HotelsManagementSystem.Api.Constants;
using System.ComponentModel.DataAnnotations;

namespace HotelsManagementSystem.Api.DTOs.Auth
{
    public class LoginDTO
    {
        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(UserConstants.UserNameMaxLength,
            MinimumLength = UserConstants.UserNameMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(UserConstants.PasswordMaxLength,
            MinimumLength = UserConstants.PasswordMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        [RegularExpression(UserConstants.PasswordRegexPattern, ErrorMessage = GeneralConstants.ValueInvalidPatternErrorMessage)]
        public string Password { get; set; } = string.Empty;
    }
}
