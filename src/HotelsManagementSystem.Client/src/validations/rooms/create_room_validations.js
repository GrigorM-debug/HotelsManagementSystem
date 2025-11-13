import { validateRoomNumber, validateDescription } from "./room_validations";

export function validateCreateRoomForm(formData) {
  const errors = {};

  const roomNumberError = validateRoomNumber(formData.roomNumber);
  if (roomNumberError) {
    errors.roomNumber = roomNumberError;
  }

  const descriptionError = validateDescription(formData.description);
  if (descriptionError) {
    errors.description = descriptionError;
  }

  if (formData.images.length < 0) {
    errors.images = "At least one image is required.";
  }

  if (!formData.selectedRoomType) {
    errors.selectedRoomType = "A room type must be selected.";
  }

  if (formData.selectedFeatures.length === 0) {
    errors.selectedFeatures = "At least one feature must be selected.";
  }

  return {
    isValid: Object.keys(errors).length === 0,
    errors,
  };
}
