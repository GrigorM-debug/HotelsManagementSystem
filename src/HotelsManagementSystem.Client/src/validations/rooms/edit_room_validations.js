import { validateRoomNumber, validateDescription } from "./room_validations";

export function validateEditRoomForm(formData) {
  const errors = {};

  const roomNumberError = validateRoomNumber(formData.roomNumber);
  if (roomNumberError) {
    errors.roomNumber = roomNumberError;
  }

  const descriptionError = validateDescription(formData.description);
  if (descriptionError) {
    errors.description = descriptionError;
  }

  if (formData.existingImages.length + formData.newImages.length <= 0) {
    errors.images = "At least one image is required.";
  }

  if (!formData.roomType) {
    errors.roomType = "A room type must be selected.";
  }

  if (formData.features.length === 0) {
    errors.features = "At least one feature must be selected.";
  }

  return {
    isValid: Object.keys(errors).length === 0,
    errors,
  };
}
