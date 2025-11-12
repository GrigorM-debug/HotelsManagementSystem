import { useAuth } from "../../useAuth";
import { useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import {
  createRoomGet,
  createRoomPost,
  getRoomsByHotelId,
  deleteRoom,
  editRoomGet,
  editRoomPost,
} from "../../../services/admin/rooms/room_service";
import {
  VALID_ROOM_IMAGE_TYPES,
  MAX_IMAGES_UPLOAD,
} from "../../../constants/room_constants";
import { validateCreateRoomForm } from "../../../validations/rooms/create_room_validations";
import { validateEditRoomForm } from "../../../validations/rooms/edit_room_validations";

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
          const apiErrors = {
            roomNumber: result.errors.RoomNumber,
            description: result.errors.Description,
            images: result.errors.Images,
            selectedRoomType: result.errors.RoomTypeId,
            selectedFeatures: result.errors.FeatureIds,
          };
          setFormSubmitError("Please fix the errors below.");
          setValidationErrors(apiErrors);
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

export function useEditRoom(hotelId, roomId) {
  const { token, clearTokenAndUser } = useAuth();
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    id: "",
    roomNumber: "",
    description: "",
    existingImages: [],
    roomType: null,
    features: [],
    newImages: [],
  });
  const [allRoomTypes, setAllRoomTypes] = useState([]);
  const [allFeatures, setAllFeatures] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [imagePreviews, setImagePreviews] = useState([]);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [formSubmitError, setFormSubmitError] = useState(null);
  const [validationErrors, setValidationErrors] = useState({});

  // Fetch existing room data
  useEffect(() => {
    const fetchRoomData = async () => {
      setIsLoading(true);
      try {
        const data = await editRoomGet(hotelId, roomId, token);
        setFormData({
          id: data.id,
          roomNumber: data.roomNumber,
          description: data.description,
          roomType: data.roomTypeId,
          existingImages: data.images,
          features: data.selectedFeatureIds,
          newImages: [],
        });
        setAllFeatures(data.allFeatures);
        setAllRoomTypes(data.allRoomTypes);
        setImagePreviews(
          data.images.map((image) => ({
            id: image.id,
            url: image.url,
            file: null,
            isExisting: true,
          }))
        );
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
            clearTokenAndUser();
            navigate("/login");
            break;
          case "429 Too Many Requests":
            navigate("/429");
            break;
          default:
            setError("Failed to fetch room edit data");
            break;
        }
      } finally {
        setIsLoading(false);
      }
    };
    fetchRoomData();
  }, [hotelId, roomId, token, navigate, clearTokenAndUser]);

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

    if (imagePreviews.length + validFiles.length > MAX_IMAGES_UPLOAD) {
      setFormSubmitError(
        `You can upload a maximum of ${MAX_IMAGES_UPLOAD} images total.`
      );
      return;
    }

    const newPreviews = validFiles.map((file) => ({
      id: Date.now() + Math.random(),
      url: URL.createObjectURL(file),
      file: file,
      isExisting: false,
    }));

    setFormData((prev) => ({
      ...prev,
      newImages: [...prev.newImages, ...validFiles],
    }));

    setImagePreviews((prev) => [...prev, ...newPreviews]);
  };

  const removeImage = (index) => {
    const imageToRemove = imagePreviews[index];

    if (imageToRemove.isExisting) {
      setFormData((prev) => ({
        ...prev,
        existingImages: prev.existingImages.filter(
          (img) => img.id !== imageToRemove.id
        ),
      }));

      setImagePreviews((prev) =>
        prev.filter((img) => img.id !== imageToRemove.id)
      );
    } else {
      setFormData((prev) => ({
        ...prev,
        newImages: prev.newImages.filter(
          (_, i) => i !== index - formData.existingImages.length
        ),
      }));

      setImagePreviews((prev) => {
        prev.filter((img) => img.id !== imageToRemove.id);
        URL.revokeObjectURL(imageToRemove.url);
      });
    }
  };

  const handleRoomTypeChange = (roomTypeId) => {
    setFormData((prev) => ({
      ...prev,
      roomType: roomTypeId,
    }));
  };

  const handleFeatureChange = (featureId) => {
    setFormData((prev) => ({
      ...prev,
      features: prev.features.includes(featureId)
        ? prev.features.filter((id) => id !== featureId)
        : [...prev.features, featureId],
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const validation = validateEditRoomForm(formData);

    if (!validation.isValid) {
      setFormSubmitError("Please fix the errors below.");
      setValidationErrors(validation.errors);
      return;
    }

    const formDataAsFormData = new FormData();
    formDataAsFormData.append("RoomNumber", formData.roomNumber);
    formDataAsFormData.append("Description", formData.description);
    formDataAsFormData.append("RoomTypeId", formData.roomType);
    formData.existingImages.forEach((image) => {
      formDataAsFormData.append("ExistingImagesIds", image.id);
    });
    formData.newImages.forEach((imageFile) => {
      formDataAsFormData.append("NewImages", imageFile);
    });
    formData.features.forEach((featureId) => {
      formDataAsFormData.append("FeatureIds", featureId);
    });

    try {
      setIsSubmitting(true);
      const result = await editRoomPost(
        hotelId,
        roomId,
        formDataAsFormData,
        token
      );

      if (result) {
        if (result.error) {
          setFormSubmitError(result.error);
        }

        if (result.errors) {
          const apiErrors = {
            roomNumber: result.errors.RoomNumber || null,
            description: result.errors.Description || null,
            images:
              result.errors.NewImages ||
              result.errors.ExistingImagesIds ||
              null,
            selectedRoomType: result.errors.RoomTypeId || null,
            selectedFeatures: result.errors.FeatureIds || null,
          };
          setFormSubmitError("Please fix the errors below.");
          console.log(result.errors);
          setValidationErrors(apiErrors);
        }

        if (result.success) {
          navigate(`/hotels/${hotelId}/rooms/${roomId}`);
        }
      }
    } catch (error) {
      switch (error.message) {
        case "404 Not Found":
          navigate("/404");
          break;
        case "401 Unauthorized":
          clearTokenAndUser();
          navigate("/login");
          break;
        case "403 Forbidden":
          clearTokenAndUser();
          navigate("/login");
          break;
        case "429 Too Many Requests":
          navigate("/429");
          break;
        default:
          setFormSubmitError("Failed to edit room");
      }
    } finally {
      setIsSubmitting(false);
    }
  };

  return {
    formData,
    isLoading,
    error,
    imagePreviews,
    allRoomTypes,
    allFeatures,
    isSubmitting,
    formSubmitError,
    validationErrors,
    handleInputChange,
    handleImageUpload,
    removeImage,
    handleRoomTypeChange,
    handleFeatureChange,
    handleSubmit,
  };
}
