import { useAuth } from "../../useAuth";
import { useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import {
  createRoomGet,
  createRoomPost,
  getRoomsByHotelId,
  deleteRoom,
} from "../../../services/admin/rooms/room_service";
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
          case "400 Bad Request":
            navigate("/404");
            break;
          case "401 Unauthorized":
            clearTokenAndUser();
            navigate("/login");
            break;
          case "403 Forbidden":
            navigate("/login");
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
  const { token, clearTokenAndUser } = useAuth();
  const navigate = useNavigate();
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

  const handleSubmit = async (e) => {
    e.preventDefault();

    const validation = validateCreateRoomForm(formData);

    if (!validation.isValid) {
      setFormSubmitError("Please fix the errors below.");
      setValidationErrors(validation.errors);
      return;
    }

    try {
      const roomDataAsFormData = new FormData();
      roomDataAsFormData.append("RoomNumber", formData.roomNumber);
      roomDataAsFormData.append("Description", formData.description);
      roomDataAsFormData.append("RoomTypeId", formData.selectedRoomType);
      formData.images.forEach((imageFile) => {
        roomDataAsFormData.append("Images", imageFile);
      });
      formData.selectedFeatures.forEach((featureId) => {
        roomDataAsFormData.append("FeatureIds", featureId);
      });

      setIsSubmitting(true);

      const result = await createRoomPost(hotelId, roomDataAsFormData, token);

      if (result) {
        if (result.error) {
          setFormSubmitError(result.error);
        }

        if (result.errors) {
          setFormSubmitError("Please fix the errors below.");
          setValidationErrors(result.errors);
        }

        const roomId = result.roomId;
        console.log("Created room with ID:", roomId);
        navigate(`/admin/hotels/${hotelId}/rooms`);
      }
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
          clearTokenAndUser();
          break;
        case "429 Too Many Requests":
          navigate("/429");
          break;
        default:
          setFormSubmitError("Failed to create room");
      }
    } finally {
      setIsSubmitting(false);
    }
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

export function useGetHotelRooms(hotelId) {
  const { token, clearTokenAndUser } = useAuth();
  const navigate = useNavigate();
  const [rooms, setRooms] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchRooms = async () => {
      setIsLoading(true);
      try {
        const data = await getRoomsByHotelId(hotelId, token);
        setRooms(data);
      } catch (err) {
        switch (err.message) {
          case "404 Not Found":
            navigate("/404");
            break;
          case "400 Bad Request":
            navigate("/404");
            break;
          case "401 Unauthorized":
            clearTokenAndUser();
            navigate("/login");
            break;
          case "403 Forbidden":
            navigate("/login");
            clearTokenAndUser();
            break;
          case "429 Too Many Requests":
            navigate("/429");
            break;
          default:
            setError("Failed to fetch hotel rooms");
        }
      } finally {
        setIsLoading(false);
      }
    };

    fetchRooms();
  }, [hotelId, token, navigate, clearTokenAndUser]);

  const refreashRooms = async () => {
    setIsLoading(true);
    try {
      const data = await getRoomsByHotelId(hotelId, token);
      setRooms(data);
    } catch (err) {
      switch (err.message) {
        case "404 Not Found":
          navigate("/404");
          break;
        case "400 Bad Request":
          navigate("/404");
          break;
        case "401 Unauthorized":
          clearTokenAndUser();
          navigate("/login");
          break;
        case "403 Forbidden":
          navigate("/login");
          clearTokenAndUser();
          break;
        case "429 Too Many Requests":
          navigate("/429");
          break;
        default:
          setError("Failed to fetch hotel rooms");
      }
    } finally {
      setIsLoading(false);
    }
  };

  return {
    rooms,
    isLoading,
    error,
    refreashRooms,
  };
}

export function useDeleteRoom(hotelId, refreshRooms) {
  const { token, clearTokenAndUser } = useAuth();
  const navigate = useNavigate();
  const [isDeleting, setIsDeleting] = useState(false);
  const [deleteError, setDeleteError] = useState(null);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [roomData, setRoomData] = useState({});

  const toggleDeleteModal = (roomData) => {
    setRoomData(roomData);
    setIsDeleteModalOpen(true);
  };

  const closeDeleteModal = () => {
    setIsDeleteModalOpen(false);
    setRoomData({});
  };

  const onConfirmDeletion = async () => {
    try {
      setIsDeleting(true);
      const result = await deleteRoom(hotelId, roomData.id, token);

      if (result && result.success) {
        closeDeleteModal();
        setRoomData({});
        await refreshRooms();
      }
    } catch (err) {
      switch (err.message) {
        case "400 Bad Request":
          navigate("/404");
          break;
        case "404 Not Found":
          navigate("/404");
          break;
        case "401 Unauthorized":
          clearTokenAndUser();
          navigate("/login");
          break;
        case "403 Forbidden":
          navigate("/login");
          clearTokenAndUser();
          break;
        case "429 Too Many Requests":
          navigate("/429");
          break;
        default:
          setDeleteError("Failed to delete room");
      }
    } finally {
      setIsDeleting(false);
    }
  };

  return {
    isDeleteModalOpen,
    toggleDeleteModal,
    closeDeleteModal,
    onConfirmDeletion,
    deleteError,
    isDeleting,
    roomData,
  };
}
