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
  if (!hotelName || hotelName.trim() === "") {
    return "Hotel Name is required.";
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
  if (stars == null || stars === "") {
    return "Stars rating is required.";
  }

  // Stars value validation
  if (stars < STARS_MIN_VALUE || stars > STARS_MAX_VALUE) {
    return STARS_VALUE_ERROR_MESSAGE;
  }
  return null;
}

export function validateCity(city) {
  if (!city || city.trim() === "") {
    return "City is required.";
  }

  // City length validation
  if (city.length < CITY_MIN_LENGTH || city.length > CITY_MAX_LENGTH) {
    return CITY_LENGTH_ERROR_MESSAGE;
  }

  // City format validation
  if (!CITY_REGEX_PATTERN.test(city)) {
    return INVALID_CITY_ERROR_MESSAGE;
  }
  return null;
}

export function validateCountry(country) {
  if (!country || country.trim() === "") {
    return "Country is required.";
  }

  // Country length validation
  if (
    country.length < COUNTRY_MIN_LENGTH ||
    country.length > COUNTRY_MAX_LENGTH
  ) {
    return COUNTRY_LENGTH_ERROR_MESSAGE;
  }

  // Country format validation
  if (!COUNTRY_REGEX_PATTERN.test(country)) {
    return INVALID_COUNTRY_ERROR_MESSAGE;
  }
  return null;
}

export function validateAddress(address) {
  if (!address || address.trim() === "") {
    return "Address is required.";
  }

  // Address length validation
  if (
    address.length < ADDRESS_MIN_LENGTH ||
    address.length > ADDRESS_MAX_LENGTH
  ) {
    return ADDRESS_LENGTH_ERROR_MESSAGE;
  }

  // Address format validation
  if (!ADDRESS_REGEX_PATTERN.test(address)) {
    return INVALID_ADDRESS_ERROR_MESSAGE;
  }
  return null;
}

export function validateCheckInAndCheckOut(checkIn, checkOut) {
  // Check-in time is required validation
  if (!checkIn) {
    return "Check-in time is required.";
  }
  // Check-out time is required validation
  if (!checkOut) {
    return "Check-out time is required.";
  }

  const checkInTime = new Date(`1970-01-01T${checkIn}:00`);
  const checkOutTime = new Date(`1970-01-01T${checkOut}:00`);
  if (checkInTime >= checkOutTime) {
    return "Check-out time must be after check-in time.";
  }
  return null;
}

export function validateEditHotelForm(formData) {
  const errors = {};

  const hotelNameError = validateHotelName(formData.name);
  if (hotelNameError) {
    errors.name = hotelNameError;
  }

  const descriptionError = validateDescription(formData.description);
  if (descriptionError) {
    errors.description = descriptionError;
  }

  const starsError = validateStars(formData.stars);
  if (starsError) {
    errors.stars = starsError;
  }

  const cityError = validateCity(formData.city);
  if (cityError) {
    errors.city = cityError;
  }

  const countryError = validateCountry(formData.country);
  if (countryError) {
    errors.country = countryError;
  }

  const addressError = validateAddress(formData.address);
  if (addressError) {
    errors.address = addressError;
  }

  const checkInOutError = validateCheckInAndCheckOut(
    formData.checkIn,
    formData.checkOut
  );

  if (checkInOutError) {
    errors.checkInOut = checkInOutError;
  }

  if (
    formData.selectedAmenities == null ||
    formData.selectedAmenities.length === 0
  ) {
    errors.selectedAmenities = "At least one amenity must be selected.";
  }

  const totalImages = formData.images.length + formData.newImages.length;
  if (totalImages === 0) {
    errors.images = "At least one image must be uploaded.";
  }

  return {
    isValid: Object.keys(errors).length === 0,
    errors,
  };
}
