import { useState } from "react";
import { validateHotelForm } from "../../../validations/hotel/hotel_forms_validations";
import { useAuth } from "../../useAuth";
import { useNavigate } from "react-router-dom";
import {
  HOTEL_MAX_IMAGES_UPLOAD,
  HOTEL_ALLOWED_IMAGE_TYPES,
} from "../../../constants/hotel_constants";
import { createHotel } from "../../../services/admin/hotels/hotel_service";

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
  const { token, clearTokenAndUser } = useAuth();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [validationErrors, setValidationErrors] = useState({});
  const [imagePreviews, setImagePreviews] = useState([]);
  const navigate = useNavigate();

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

  const handleSubmit = async (e, hotelData) => {
    e.preventDefault();

    const validation = validateHotelForm(hotelData);

    if (!validation.isValid) {
      setError("Please fix the errors below.");
      setValidationErrors(validation.errors);
      return;
    }

    try {
      const hotelDataAsFormData = new FormData();
      hotelDataAsFormData.append("Name", formData.name);
      hotelDataAsFormData.append("Description", formData.description);
      hotelDataAsFormData.append("Address", formData.address);
      hotelDataAsFormData.append("City", formData.city);
      hotelDataAsFormData.append("Country", formData.country);
      hotelDataAsFormData.append("Stars", formData.stars);
      hotelDataAsFormData.append("CheckInTime", formData.checkIn);
      hotelDataAsFormData.append("CheckOutTime", formData.checkOut);
      formData.selectedAmenities.forEach((amenity) => {
        hotelDataAsFormData.append("AmenityIds", amenity);
      });
      formData.images.forEach((imageFile) => {
        hotelDataAsFormData.append("Images", imageFile);
      });

      setIsLoading(true);

      const result = await createHotel(hotelDataAsFormData, token);

      if (result.error) {
        setError(result.error);
        return;
      }

      if (result.errors) {
        setValidationErrors(result.errors);
        setError("Please fix the errors below.");
        return;
      }

      // On success, redirect to hotels list or hotel details page
      const createdHotelId = result.hotelId;
      console.log("Hotel created with ID:", createdHotelId);
      navigate("/admin/hotels");
    } catch (err) {
      switch (err.message) {
        case "401 Unauthorized":
          clearTokenAndUser();
          navigate("/login");
          break;
        case "403 Forbidden":
          clearTokenAndUser();
          navigate("/login");
          break;
        case "404 Not Found":
          clearTokenAndUser();
          navigate("/404");
          break;
      }
    } finally {
      setIsLoading(false);
    }
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
