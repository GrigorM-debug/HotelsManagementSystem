import {
  HOTEL_NAME_REGEX_PATTERN,
  HOTEL_NAME_MIN_LENGTH,
  HOTEL_NAME_MAX_LENGTH,
  HOTEL_NAME_LENGTH_ERROR_MESSAGE,
  INVALID_HOTEL_NAME_ERROR_MESSAGE,
  DESCRIPTION_MIN_LENGTH,
  DESCRIPTION_MAX_LENGTH,
  DESCRIPTION_LENGTH_ERROR_MESSAGE,
  STARS_MIN_VALUE,
  STARS_MAX_VALUE,
  STARS_VALUE_ERROR_MESSAGE,
  CITY_REGEX_PATTERN,
  CITY_MIN_LENGTH,
  CITY_MAX_LENGTH,
  CITY_LENGTH_ERROR_MESSAGE,
  INVALID_CITY_ERROR_MESSAGE,
  COUNTRY_REGEX_PATTERN,
  COUNTRY_MIN_LENGTH,
  COUNTRY_MAX_LENGTH,
  COUNTRY_LENGTH_ERROR_MESSAGE,
  INVALID_COUNTRY_ERROR_MESSAGE,
  ADDRESS_REGEX_PATTERN,
  ADDRESS_MIN_LENGTH,
  ADDRESS_MAX_LENGTH,
  ADDRESS_LENGTH_ERROR_MESSAGE,
  INVALID_ADDRESS_ERROR_MESSAGE,
} from "../../constants/hotel_constants.js";

export function validateHotelName(hotelName) {
  // Hotel Name is required validation
  if (!hotelName || hotelName.trim() === "") {
    return "Hotel name is required.";
  }

  // Hotel Name length validation
  if (
    hotelName.length < HOTEL_NAME_MIN_LENGTH ||
    hotelName.length > HOTEL_NAME_MAX_LENGTH
  ) {
    return HOTEL_NAME_LENGTH_ERROR_MESSAGE;
  }

  // Hotel Name format validation
  if (!HOTEL_NAME_REGEX_PATTERN.test(hotelName)) {
    return INVALID_HOTEL_NAME_ERROR_MESSAGE;
  }

  return null;
}

export function validateDescription(description) {
  // Description is required validation
  if (!description || description.trim() === "") {
    return "Description is required.";
  }

  // Description length validation
  const trimmedDescription = description.trim();
  if (
    trimmedDescription.length < DESCRIPTION_MIN_LENGTH ||
    trimmedDescription.length > DESCRIPTION_MAX_LENGTH
  ) {
    return DESCRIPTION_LENGTH_ERROR_MESSAGE;
  }

  return null;
}

export function validateStars(stars) {
  // Stars is required validation
  if (!stars) {
    return "Stars rating is required.";
  }

  // Stars range validation
  if (stars < STARS_MIN_VALUE || stars > STARS_MAX_VALUE) {
    return STARS_VALUE_ERROR_MESSAGE;
  }

  //Check if stars is an integer
  if (!Number.isInteger(stars)) {
    return "Stars rating must be an integer.";
  }

  return null;
}

export function validateCity(city) {
  // City is required validation
  if (!city || city.trim() === "") {
    return "City is required.";
  }

  const trimmedCity = city.trim();

  // City length validation
  if (
    trimmedCity.length < CITY_MIN_LENGTH ||
    trimmedCity.length > CITY_MAX_LENGTH
  ) {
    return CITY_LENGTH_ERROR_MESSAGE;
  }

  // City format validation
  if (!CITY_REGEX_PATTERN.test(trimmedCity)) {
    return INVALID_CITY_ERROR_MESSAGE;
  }

  return null;
}

export function validateCountry(country) {
  // Country is required validation
  if (!country || country.trim() === "") {
    return "Country is required.";
  }

  const trimmedCountry = country.trim();

  // Country length validation
  if (
    trimmedCountry.length < COUNTRY_MIN_LENGTH ||
    trimmedCountry.length > COUNTRY_MAX_LENGTH
  ) {
    return COUNTRY_LENGTH_ERROR_MESSAGE;
  }

  // Country format validation
  if (!COUNTRY_REGEX_PATTERN.test(trimmedCountry)) {
    return INVALID_COUNTRY_ERROR_MESSAGE;
  }
  return null;
}

export function validateAddress(address) {
  // Address is required validation
  if (!address || address.trim() === "") {
    return "Address is required.";
  }

  const trimmedAddress = address.trim();

  // Address length validation
  if (
    trimmedAddress.length < ADDRESS_MIN_LENGTH ||
    trimmedAddress.length > ADDRESS_MAX_LENGTH
  ) {
    return ADDRESS_LENGTH_ERROR_MESSAGE;
  }

  // Address format validation
  if (!ADDRESS_REGEX_PATTERN.test(trimmedAddress)) {
    return INVALID_ADDRESS_ERROR_MESSAGE;
  }

  return null;
}

export function validateHotelForm(formData) {
  const errors = {};

  const hotelName = formData.get("hotelName");
  const description = formData.get("description");
  const stars = formData.get("stars");
  const city = formData.get("city");
  const country = formData.get("country");
  const address = formData.get("address");

  // Validate Hotel Name
  const hotelNameError = validateHotelName(hotelName);
  if (hotelNameError) {
    errors.hotelName = hotelNameError;
  }

  // Validate Description
  const descriptionError = validateDescription(description);
  if (descriptionError) {
    errors.description = descriptionError;
  }

  // Validate stars
  const starsError = validateStars(stars);
  if (starsError) {
    errors.stars = starsError;
  }

  // Validate City
  const cityError = validateCity(city);
  if (cityError) {
    errors.city = cityError;
  }

  // Validate Country
  const countryError = validateCountry(country);
  if (countryError) {
    errors.country = countryError;
  }

  // Validate Address
  const addressError = validateAddress(address);
  if (addressError) {
    errors.address = addressError;
  }

  return {
    isValid: Object.keys(errors).length === 0,
    errors: errors,
  };
}
