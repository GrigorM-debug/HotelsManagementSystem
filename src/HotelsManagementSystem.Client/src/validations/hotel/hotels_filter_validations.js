import {
  validateHotelName,
  validateCity,
  validateCountry,
} from "./hotel_forms_validations";

export function validateHotelsFilter(filter) {
  const errors = {};

  const nameError = validateHotelName(filter.name);
  if (nameError) {
    errors.name = nameError;
  }

  const cityError = validateCity(filter.city);
  if (cityError) {
    errors.city = cityError;
  }

  const countryError = validateCountry(filter.country);
  if (countryError) {
    errors.country = countryError;
  }

  return errors;
}
