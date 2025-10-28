﻿using HotelsManagementSystem.Api.Constants;
using System.ComponentModel.DataAnnotations;

namespace HotelsManagementSystem.Api.DTOs.Auth
{
    public class RegisterDTO : IValidatableObject
    {
        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(UserConstants.UserNameMaxLength,
            MinimumLength = UserConstants.UserNameMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [EmailAddress(ErrorMessage = UserConstants.InvalidEmailErrorMessage)]
        [StringLength(UserConstants.EmailMaxLength,
            MinimumLength = UserConstants.EmailMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [RegularExpression(UserConstants.FirstNameAndLastNameRegexPattern, ErrorMessage = GeneralConstants.ValueInvalidPatternErrorMessage)]
        [StringLength(UserConstants.FirstNameAndLastNameMaxLength,
            MinimumLength = UserConstants.FirstNameAndLastNameMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [RegularExpression(UserConstants.FirstNameAndLastNameRegexPattern, ErrorMessage = GeneralConstants.ValueInvalidPatternErrorMessage)]
        [StringLength(UserConstants.FirstNameAndLastNameMaxLength,
            MinimumLength = UserConstants.FirstNameAndLastNameMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [RegularExpression(UserConstants.PhoneNumberRegexPattern, ErrorMessage = UserConstants.InvalidPhoneNumberErrorMessage)]
        [StringLength(UserConstants.PhoneNumberMaxLength,
            MinimumLength = UserConstants.PhoneNumberMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = GeneralConstants.ValueRequiredErrorMessage)]
        [StringLength(UserConstants.PasswordMaxLength,
            MinimumLength = UserConstants.PasswordMinLength, ErrorMessage = GeneralConstants.ValueLengthErrorMessage)]
        [RegularExpression(UserConstants.PasswordRegexPattern, ErrorMessage = GeneralConstants.ValueInvalidPatternErrorMessage)]
        public string Password { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password.ToLower().Contains("password"))
            {
                yield return new ValidationResult("Password should not contain the word password", new[] { nameof(Password) });
            }

            var isPasswordContainsUserName = Password.ToLower().Contains(UserName.ToLower());
            if (isPasswordContainsUserName == true)
            {
                yield return new ValidationResult("Password should not contain the Username", new[] { nameof(Password) });
            }

            var isPasswordContainsFirstName = Password.ToLower().Contains(FirstName.ToLower());
            if (isPasswordContainsFirstName == true)
            {
                yield return new ValidationResult("Password should not contain the Firstname", new[] { nameof(Password) });
            }

            var isPasswordContainsLastName = Password.ToLower().Contains(LastName.ToLower());
            if (isPasswordContainsLastName == true)
            {
                yield return new ValidationResult("Password should not contain the Lastname", new[] { nameof(Password) });
            }

            var isPasswordContainsEmail = Password.ToLower().Contains(Email.ToLower());
            if (isPasswordContainsEmail == true)
            {
                yield return new ValidationResult("Password should not contain the Email", new[] { nameof(Password) });
            }

            var isPasswordContainsPhoneNumber = Password.ToLower().Contains(PhoneNumber.ToLower());
            if (isPasswordContainsPhoneNumber == true)
            {
                yield return new ValidationResult("Password should not contain the Phone number", new[] { nameof(Password) });
            }
        }
    }
}
