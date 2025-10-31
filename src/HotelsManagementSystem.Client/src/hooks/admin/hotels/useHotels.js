import { useState } from "react";
import { validateHotelForm } from "../../../validations/hotel/hotel_forms_validations";
import {
  HOTEL_MAX_IMAGES_UPLOAD,
  HOTEL_ALLOWED_IMAGE_TYPES,
} from "../../../constants/hotel_constants";

export function useCreateHotel() {
  const [formData, setFormData] = useState({
    name: "",
    description: "",
    address: "",
    city: "",
    country: "",
    checkIn: "",
    checkOut: "",
    stars: 1,
    selectedAmenities: [],
    images: [],
  });

  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [validationErrors, setValidationErrors] = useState({});
  const [imagePreviews, setImagePreviews] = useState([]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevData) => ({ ...prevData, [name]: value }));
  };

  const handleAmenityChange = (amenityId) => {
    setFormData((prev) => ({
      ...prev,
      selectedAmenities: prev.selectedAmenities.includes(amenityId)
        ? prev.selectedAmenities.filter((id) => id !== amenityId)
        : [...prev.selectedAmenities, amenityId],
    }));
  };

  const handleImageChange = (e) => {
    const files = Array.from(e.target.files);

    const validTypes = HOTEL_ALLOWED_IMAGE_TYPES;
    const validFiles = files.filter((file) => validTypes.includes(file.type));

    if (validFiles.length !== files.length) {
      setError("Please select only image files (JPEG, PNG, WebP)");
      return;
    }

    if (validFiles.length > HOTEL_MAX_IMAGES_UPLOAD) {
      setError(
        `You can upload a maximum of ${HOTEL_MAX_IMAGES_UPLOAD} images.`
      );
      return;
    }

    setFormData((prev) => ({
      ...prev,
      images: [...prev.images, ...validFiles],
    }));

    // Create image previews
    validFiles.forEach((file) => {
      const reader = new FileReader();
      reader.onload = (e) => {
        setImagePreviews((prev) => [
          ...prev,
          {
            id: Date.now() + Math.random(),
            url: e.target.result,
            file: file,
          },
        ]);
      };
      reader.readAsDataURL(file);
    });
  };

  const removeImage = (indexToRemove) => {
    setFormData((prev) => ({
      ...prev,
      images: prev.images.filter((_, index) => index !== indexToRemove),
    }));
    setImagePreviews((prev) =>
      prev.filter((_, index) => index !== indexToRemove)
    );
  };

  const handleSubmit = (e, hotelData) => {
    e.preventDefault();

    const validation = validateHotelForm(hotelData);

    if (!validation.isValid) {
      setError("Please fix the errors below.");
      setValidationErrors(validation.errors);
      return;
    }

    console.log("Submitting hotel data:", hotelData);
  };

  return {
    isLoading,
    handleSubmit,
    error,
    validationErrors,
    imagePreviews,
    removeImage,
    handleImageChange,
    handleAmenityChange,
    handleInputChange,
    formData,
  };
}
