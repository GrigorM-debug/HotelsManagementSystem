import {
  HOTEL_NAME_MIN_LENGTH,
  HOTEL_NAME_MAX_LENGTH,
  INVALID_HOTEL_NAME_ERROR_MESSAGE,
  HOTEL_NAME_REGEX_PATTERN,
  CITY_LENGTH_ERROR_MESSAGE,
  INVALID_CITY_ERROR_MESSAGE,
  CITY_MIN_LENGTH,
  CITY_MAX_LENGTH,
  CITY_REGEX_PATTERN,
  COUNTRY_LENGTH_ERROR_MESSAGE,
  INVALID_COUNTRY_ERROR_MESSAGE,
  COUNTRY_MIN_LENGTH,
  COUNTRY_MAX_LENGTH,
  COUNTRY_REGEX_PATTERN,
  HOTEL_NAME_LENGTH_ERROR_MESSAGE,
} from "../../constants/hotel_constants";

export function validateHotelsFilter(filter) {
  const errors = {};

  const hotelName = filter.name;

  if (hotelName) {
    if (
      hotelName.length < HOTEL_NAME_MIN_LENGTH ||
      hotelName.length > HOTEL_NAME_MAX_LENGTH
    ) {
      errors.name = HOTEL_NAME_LENGTH_ERROR_MESSAGE;
    }
    if (!HOTEL_NAME_REGEX_PATTERN.test(hotelName)) {
      errors.name = INVALID_HOTEL_NAME_ERROR_MESSAGE;
    }
  }

  const city = filter.city;

  if (city) {
    if (city.length < CITY_MIN_LENGTH || city.length > CITY_MAX_LENGTH) {
      errors.city = CITY_LENGTH_ERROR_MESSAGE;
    }
    if (!CITY_REGEX_PATTERN.test(city)) {
      errors.city = INVALID_CITY_ERROR_MESSAGE;
    }
  }

  const country = filter.country;

  if (country) {
    if (
      country.length < COUNTRY_MIN_LENGTH ||
      country.length > COUNTRY_MAX_LENGTH
    ) {
      errors.country = COUNTRY_LENGTH_ERROR_MESSAGE;
    }
    if (!COUNTRY_REGEX_PATTERN.test(country)) {
      errors.country = INVALID_COUNTRY_ERROR_MESSAGE;
    }
  }

  return {
    isValid: Object.keys(errors).length === 0,
    errors,
  };
}
