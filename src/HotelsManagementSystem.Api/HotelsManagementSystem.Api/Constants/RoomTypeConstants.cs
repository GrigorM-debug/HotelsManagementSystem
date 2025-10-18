namespace HotelsManagementSystem.Api.Constants
{
    public static class RoomTypeConstants
    {
        public const int TypeNameMinLength = 3;
        public const int TypeNameMaxLength = 100;

        public const decimal PricePerNightMinValue = 0.01M;
        public const decimal PricePerNightMaxValue = 10000.00M;

        public const int CapacityMinValue = 1;
        public const int CapacityMaxValue = 20;

        public const string TypeNameRegexPattern = @"^[\p{L}\p{N}\s\.\,'&\-]{3,100}$";

        public const int DescriptionMinLength = 30;
        public const int DescriptionMaxLength = 2000;

        public const string CapasityErrorMessage = "Capacity must be between 1 and 20 guests.";
    }
}
