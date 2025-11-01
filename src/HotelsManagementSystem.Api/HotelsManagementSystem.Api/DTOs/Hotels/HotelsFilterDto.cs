using System.ComponentModel.DataAnnotations;
using HotelsManagementSystem.Api.Constants;

namespace HotelsManagementSystem.Api.DTOs.Hotels
{
    public class HotelsFilterDto : IValidatableObject
    {
        public string? Name { get; set; } 
        public string? City { get; set; } 
        public string? Country { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
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

            if (!string.IsNullOrEmpty(City))
            {
                if(City.Length < GeneralConstants.CityMinLength || City.Length > GeneralConstants.CityMaxLength)
                {
                    yield return new ValidationResult(
                        $"City must be between {GeneralConstants.CityMinLength} and {GeneralConstants.CityMaxLength} characters long.",
                        new[] { nameof(City) });
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(City, GeneralConstants.CityRegexPattern))
                {
                    yield return new ValidationResult(
                        GeneralConstants.ValueInvalidPatternErrorMessage,
                        new[] { nameof(City) });
                }
            }

            if (!string.IsNullOrEmpty(Country))
            {
                if(Country.Length < GeneralConstants.CountryMinLength || Country.Length > GeneralConstants.CountryMaxLength)
                {
                    yield return new ValidationResult(
                        $"Country must be between {GeneralConstants.CountryMinLength} and {GeneralConstants.CountryMaxLength} characters long.",
                        new[] { nameof(Country) });
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(Country, GeneralConstants.CountryRegexPattern))
                {
                    yield return new ValidationResult(
                        GeneralConstants.ValueInvalidPatternErrorMessage,
                        new[] { nameof(Country) });
                }
            }
        }
    }
}
