import {
  ROOM_NUMBER_MIN_VALUE,
  ROOM_NUMBER_MAX_VALUE,
  ROOM_NUMBER_VALUE_ERROR_MESSAGE,
  DESCRIPTION_MIN_LENGTH,
  DESCRIPTION_MAX_LENGTH,
  DESCRIPTION_LENGTH_ERROR_MESSAGE,
  VALID_ROOM_IMAGE_TYPES,
  MAX_IMAGES_UPLOAD,
} from "../../constants/room_constants";

export function validateRoomNumber(roomNumber) {
  if (roomNumber === "" || roomNumber === null || roomNumber === undefined) {
    return "Room number is required.";
  }

  const roomNumberValue = parseInt(roomNumber, 10);

  if (isNaN(roomNumberValue)) {
    return "Room number must be a valid number.";
  }

  if (
    roomNumberValue < ROOM_NUMBER_MIN_VALUE ||
    roomNumberValue > ROOM_NUMBER_MAX_VALUE
  ) {
    return ROOM_NUMBER_VALUE_ERROR_MESSAGE;
  }

  return null;
}

export function validateDescription(description) {
  if (!description || description.trim() === "") {
    return "Description is required.";
  }
  const trimmedDescription = description.trim();
  if (
    trimmedDescription.length < DESCRIPTION_MIN_LENGTH ||
    trimmedDescription.length > DESCRIPTION_MAX_LENGTH
  ) {
    return DESCRIPTION_LENGTH_ERROR_MESSAGE;
  }

  return null;
}

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
