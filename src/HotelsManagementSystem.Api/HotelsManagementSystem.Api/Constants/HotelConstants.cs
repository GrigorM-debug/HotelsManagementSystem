namespace HotelsManagementSystem.Api.Constants
{
    public static class HotelConstants
    {
        // Name
        public const int NameMinLength = 3;
        public const int NameMaxLength = 100;
        public const string NameRegexPattern = @"^[\p{L}\p{N}\s\.\,'&\-]{3,100}$";

        // Description
        public const int DescriptionMinLength = 30;
        public const int DescriptionMaxLength = 2000;

        // Stars
        public const int StarsMinValue = 1;
        public const int StarsMaxValue = 5;
    }
}
