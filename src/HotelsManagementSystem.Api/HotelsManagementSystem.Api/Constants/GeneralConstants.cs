namespace HotelsManagementSystem.Api.Constants
{
    public static class GeneralConstants
    {
        public const string ValueRequiredErrorMessage = "Value for {0} is required";
        public const string ValueLengthErrorMessage = "{0} must be between {2} and {1} characters!";
        public const string ValueRangeErrorMessage = "{0} must be between {1} and {2}";
        public const string ValueInvalidPatternErrorMessage = "{0} is invalid pattern!";

        public const int CityMinLength = 2;
        public const int CityMaxLength = 100;
        public const string CityRegexPattern = @"^[\p{L}\p{M}\s\.\-']{2,100}$";

        public const int CountryMinLength = 2;
        public const int CountryMaxLength = 64;
        public const string CountryRegexPattern = @"^(?=.{2,64}$)[\p{L}\p{M}]+(?:[ \-'\u2019][\p{L}\p{M}]+)*$";

        public const int AddressMinLength = 5;
        public const int AddressMaxLength = 250;
        public const string AddressRegexPattern = @"^(?=.{5,250}$)[\p{L}\p{M}\p{N}\s.,''""\-#/]+$";

        public const int ImageUploadMaxCount = 3;
        public static List<string> AllowedImageTypes = new List<string>() { "image/jpeg", "image/jpg", "image/png" };
    }
}
