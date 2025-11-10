import { useAuth } from "../../useAuth";
import { useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import { createRoomGet } from "../../../services/admin/rooms/room_service";
import {
  VALID_ROOM_IMAGE_TYPES,
  MAX_IMAGES_UPLOAD,
} from "../../../constants/room_constants";
import { validateCreateRoomForm } from "../../../validations/rooms/create_room_validations";

export function useCreateRoomGet(hotelId) {
  const { token, clearTokenAndUser } = useAuth();
  const navigate = useNavigate();
  const [roomData, setRoomData] = useState({});
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchRoomData = async () => {
      setIsLoading(true);
      try {
        const data = await createRoomGet(hotelId, token);
        setRoomData(data);
      } catch (err) {
        switch (err.message) {
          case "404 Not Found":
            navigate("/404");
            break;
          case "401 Unauthorized":
            clearTokenAndUser();
            navigate("/login");
            break;
          case "403 Forbidden":
            navigate("/403");
            break;
          case "429 Too Many Requests":
            navigate("/429");
            break;
          default:
            setError("Failed to fetch room creation data");
        }
      } finally {
        setIsLoading(false);
      }
    };

    fetchRoomData();
  }, [hotelId, token, navigate, clearTokenAndUser]);

  return {
    roomData,
    isLoading,
    error,
  };
}

export function useCreateRoomPost(hotelId) {
  const [formData, setFormData] = useState({
    roomNumber: "",
    description: "",
    images: [],
    selectedRoomType: null,
    selectedFeatures: [],
  });

  const [previewImages, setPreviewImages] = useState([]);
  const [formSubmitError, setFormSubmitError] = useState(null);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [validationErrors, setValidationErrors] = useState({});

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleImageUpload = (e) => {
    const files = Array.from(e.target.files);

    const validTypes = VALID_ROOM_IMAGE_TYPES;
    const validFiles = files.filter((file) => validTypes.includes(file.type));

    if (validFiles.length !== files.length) {
      setFormSubmitError("Please select only image files (JPEG, PNG, JPG)");
      return;
    }

    if (validFiles.length > MAX_IMAGES_UPLOAD) {
      setFormSubmitError(
        `You can upload a maximum of ${MAX_IMAGES_UPLOAD} images total.`
      );
      return;
    }

    setFormData((prev) => ({
      ...prev,
      images: [...prev.images, ...validFiles],
    }));

    // Create preview URLs
    const newPreviews = validFiles.map((file) => URL.createObjectURL(file));
    setPreviewImages((prev) => [...prev, ...newPreviews]);
  };

  const removeImage = (index) => {
    setFormData((prev) => ({
      ...prev,
      images: prev.images.filter((_, i) => i !== index),
    }));

    URL.revokeObjectURL(previewImages[index]);
    setPreviewImages((prev) => prev.filter((_, i) => i !== index));
  };

  const handleRoomTypeChange = (roomTypeId) => {
    setFormData((prev) => ({
      ...prev,
      selectedRoomType:
        prev.selectedRoomType === roomTypeId ? null : roomTypeId,
    }));
  };

  const handleFeatureChange = (featureId) => {
    setFormData((prev) => ({
      ...prev,
      selectedFeatures: prev.selectedFeatures.includes(featureId)
        ? prev.selectedFeatures.filter((id) => id !== featureId)
        : [...prev.selectedFeatures, featureId],
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    const validation = validateCreateRoomForm(formData);

    if (!validation.isValid) {
      setFormSubmitError("Please fix the errors below.");
      setValidationErrors(validation.errors);
      return;
    }

    // TODO: Implement room creation logic
    console.log("Form data:", formData);
  };

  return {
    formData,
    previewImages,
    handleInputChange,
    handleImageUpload,
    removeImage,
    handleRoomTypeChange,
    handleFeatureChange,
    handleSubmit,
    formSubmitError,
    isSubmitting,
    validationErrors,
  };
}
