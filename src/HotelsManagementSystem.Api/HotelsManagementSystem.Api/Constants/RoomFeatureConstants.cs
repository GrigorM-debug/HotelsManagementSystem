namespace HotelsManagementSystem.Api.Constants
{
    public static class RoomFeatureConstants
    {
        public const int NameMinLength = 3;
        public const int NameMaxLength = 100;
        public const decimal PriceMinValue = 0.01M;
        public const decimal PriceMaxValue = 10000.00M;
        public const string NameRegexPattern = @"^[\p{L}\p{N}\s\.\,'&\-]{3,100}$";
    }
}
