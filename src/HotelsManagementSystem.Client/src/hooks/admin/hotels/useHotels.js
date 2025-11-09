import { useState, useEffect } from "react";
import { validateHotelForm } from "../../../validations/hotel/hotel_forms_validations";
import { validateHotelsFilter } from "../../../validations/hotel/hotels_filter_validations";
import { useAuth } from "../../useAuth";
import { useNavigate } from "react-router-dom";
import {
  HOTEL_MAX_IMAGES_UPLOAD,
  HOTEL_ALLOWED_IMAGE_TYPES,
} from "../../../constants/hotel_constants";
import {
  createHotel,
  getAdminHotels,
  deleteHotel,
  editGetHotel,
} from "../../../services/admin/hotels/hotel_service";

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

      const createdHotelId = result.hotelId;
      navigate(`/hotels/${createdHotelId}`);
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
        case "429 Too Many Requests":
          navigate("/429");
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

export function useGetAdminHotels() {
  const initialFilterState = {
    name: "",
    country: "",
    city: "",
  };
  const [hotels, setHotels] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const { token, clearTokenAndUser } = useAuth();
  const navigate = useNavigate();
  const [filter, setFilter] = useState(initialFilterState);
  const [appliedFilters, setAppliedFilters] = useState(initialFilterState);
  const [validationErrors, setValidationErrors] = useState({});

  const handleFilterChange = (e) => {
    setFilter({
      ...filter,
      [e.target.name]: e.target.value,
    });
  };

  const handleFilterReset = () => {
    setFilter(initialFilterState);
    setAppliedFilters(initialFilterState);
  };

  const handleApplyFilters = () => {
    //Validte filters
    const validation = validateHotelsFilter(filter);

    if (!validation.isValid) {
      setValidationErrors(validation.errors);
      return;
    } else {
      setAppliedFilters({ ...filter });
    }
  };

  useEffect(() => {
    const fetchHotels = async () => {
      setIsLoading(true);
      try {
        const fetchedHotels = await getAdminHotels(token, appliedFilters);

        // Filter validation errors from the api
        if (fetchedHotels.errors) {
          setValidationErrors(fetchedHotels.errors);
          return;
        }
        setHotels(fetchedHotels);
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
          case "429 Too Many Requests":
            navigate("/429");
            break;
          default:
            setError("Failed to fetch hotels");
        }
      } finally {
        setIsLoading(false);
      }
    };

    fetchHotels();
  }, [token, clearTokenAndUser, navigate, appliedFilters]);

  const refreshHotels = async () => {
    setIsLoading(true);
    try {
      const fetchedHotels = await getAdminHotels(token, appliedFilters);
      setHotels(fetchedHotels);
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
        default:
          setError("Failed to fetch hotels");
      }
    } finally {
      setIsLoading(false);
    }
  };
  return {
    hotels,
    isLoading,
    error,
    handleFilterChange,
    handleFilterReset,
    handleApplyFilters,
    validationErrors,
    filter,
    refreshHotels,
  };
}

export function useDeleteHotel(refreshHotels) {
  const { token, clearTokenAndUser } = useAuth();
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [hotelToDeleteInfo, setHotelToDeleteInfo] = useState({});
  const navigate = useNavigate();

  const toggleDeleteModal = (hotelToDeleteData) => {
    setHotelToDeleteInfo(hotelToDeleteData);
    setIsDeleteModalOpen(true);
  };

  const closeDeleteModal = () => {
    setHotelToDeleteInfo({});
    setIsDeleteModalOpen(false);
  };

  const onConfirmDeletion = async () => {
    try {
      const result = await deleteHotel(hotelToDeleteInfo.id, token);

      if (result.success) {
        if (refreshHotels) {
          await refreshHotels();
          closeDeleteModal();
        }
      }
    } catch (error) {
      switch (error.message) {
        case "401 Unauthorized":
          clearTokenAndUser();
          navigate("/login");
          break;
        case "403 Forbidden":
          clearTokenAndUser();
          navigate("/login");
          break;
        case "404 Not Found":
          navigate("/404");
          break;
        case "429 Too Many Requests":
          navigate("/429");
          break;
      }
    }
  };

  return {
    isDeleteModalOpen,
    toggleDeleteModal,
    closeDeleteModal,
    hotelToDeleteInfo,
    onConfirmDeletion,
  };
}

export function useEditHotel(hotelId) {
  const { token, clearTokenAndUser } = useAuth();
  const navigate = useNavigate();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [validationErrors, setValidationErrors] = useState({});
  const [imagePreviews, setImagePreviews] = useState([]);

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

  useEffect(() => {
    const fetchHotelForEdit = async () => {
      try {
        setIsLoading(true);
        const hotelData = await editGetHotel(hotelId, token);

        setFormData({
          name: hotelData.name,
          description: hotelData.description,
          address: hotelData.address,
          city: hotelData.city,
          country: hotelData.country,
          checkIn: hotelData.checkInTime,
          checkOut: hotelData.checkOutTime,
          stars: hotelData.stars,
          selectedAmenities: hotelData.amenities.map((a) => a.id),
          images: hotelData.images,
        });

        setImagePreviews(
          hotelData.images.map((image) => ({
            id: image.id,
            url: image.imageUrl,
            file: null,
          }))
        );
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
            navigate("/404");
            break;
          case "429 Too Many Requests":
            navigate("/429");
            break;
          default:
            setError("Failed to fetch hotel details for editing");
        }
      } finally {
        setIsLoading(false);
      }
    };

    fetchHotelForEdit();
  }, [hotelId, token, clearTokenAndUser, navigate]);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
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

  const handleFormSubmit = async (e, hotelData) => {
    e.preventDefault();

    const validation = validateHotelForm(hotelData);

    if (!validation.isValid) {
      setValidationErrors(validation.errors);
      setError("Please fix the errors below.");
      return;
    }
    console.log("Submitting edited hotel data:", hotelData);
    //Before submitting, prepare FormData
  };

  return {
    formData,
    handleInputChange,
    handleAmenityChange,
    handleImageChange,
    removeImage,
    handleFormSubmit,
    isLoading,
    error,
    validationErrors,
    imagePreviews,
  };
}
