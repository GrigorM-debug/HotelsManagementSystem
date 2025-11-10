// Error Messages
export const INVALID_HOTEL_NAME_ERROR_MESSAGE =
  "Hotel name can only contain letters, numbers, spaces, and the characters: . , ' & -";
export const INVALID_CITY_ERROR_MESSAGE =
  "City name can only contain letters, spaces, and the characters: . - '";
export const INVALID_COUNTRY_ERROR_MESSAGE =
  "Country name can only contain letters, spaces, and the characters: - ' '";
export const INVALID_ADDRESS_ERROR_MESSAGE =
  "Address can only contain letters, numbers, spaces, and the characters: . , ' \" - # /";

// Regex Patterns
export const HOTEL_NAME_REGEX_PATTERN = /^[\p{L}\p{N}\s.,'&-]{3,100}$/u;
export const CITY_REGEX_PATTERN = /^[\p{L}\p{N}\s.,'&-]{3,100}$/u;
export const COUNTRY_REGEX_PATTERN =
  /^(?=.{2,64}$)[\p{L}\p{M}]+(?:[ \-'\u2019][\p{L}\p{M}]+)*$/u;
export const ADDRESS_REGEX_PATTERN =
  /^(?=.{5,250}$)[\p{L}\p{M}\p{N}\s.,'"#/-]+$/u;

// Length Constraints
// Hotel Name
export const HOTEL_NAME_MIN_LENGTH = 3;
export const HOTEL_NAME_MAX_LENGTH = 100;
export const HOTEL_NAME_LENGTH_ERROR_MESSAGE = `Hotel name must be between ${HOTEL_NAME_MIN_LENGTH} and ${HOTEL_NAME_MAX_LENGTH} characters long.`;

// Description
export const DESCRIPTION_MIN_LENGTH = 30;
export const DESCRIPTION_MAX_LENGTH = 2000;
export const DESCRIPTION_LENGTH_ERROR_MESSAGE = `Description must be between ${DESCRIPTION_MIN_LENGTH} and ${DESCRIPTION_MAX_LENGTH} characters long.`;

// Stars
export const STARS_MIN_VALUE = 1;
export const STARS_MAX_VALUE = 5;
export const STARS_VALUE_ERROR_MESSAGE = `Stars rating must be between ${STARS_MIN_VALUE} and ${STARS_MAX_VALUE}.`;

// City
export const CITY_MIN_LENGTH = 2;
export const CITY_MAX_LENGTH = 100;
export const CITY_LENGTH_ERROR_MESSAGE = `City name must be between ${CITY_MIN_LENGTH} and ${CITY_MAX_LENGTH} characters long.`;

// Country
export const COUNTRY_MIN_LENGTH = 2;
export const COUNTRY_MAX_LENGTH = 64;
export const COUNTRY_LENGTH_ERROR_MESSAGE = `Country name must be between ${COUNTRY_MIN_LENGTH} and ${COUNTRY_MAX_LENGTH} characters long.`;

// Address
export const ADDRESS_MIN_LENGTH = 5;
export const ADDRESS_MAX_LENGTH = 250;
export const ADDRESS_LENGTH_ERROR_MESSAGE = `Address must be between ${ADDRESS_MIN_LENGTH} and ${ADDRESS_MAX_LENGTH} characters long.`;
export const HOTEL_MAX_IMAGES_UPLOAD = 3;
export const HOTEL_ALLOWED_IMAGE_TYPES = [
  "image/jpeg",
  "image/jpg",
  "image/png",
];
