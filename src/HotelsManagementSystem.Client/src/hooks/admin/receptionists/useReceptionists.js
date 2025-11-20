import { useAuth } from "../../useAuth";
import { useNavigate } from "react-router-dom";
import {
  getReceptionistsByHotelId,
  createReceptionist,
} from "../../../services/admin/receptionsts/receptionists_service";
import { useEffect, useState } from "react";
import { validateRegisterData } from "../../../validations/auth/register_form_validations";

export function useGetHotelReceptionists(hotelId) {
  const { token, clearTokenAndUser } = useAuth();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const [receptionists, setReceptionists] = useState([]);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchReceptionists = async () => {
      setIsLoading(true);
      try {
        const data = await getReceptionistsByHotelId(hotelId, token);

        setReceptionists(data.receptionists);
      } catch (err) {
        switch (err.message) {
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
          case "404 Not Found":
            navigate("/404");
            break;
          case "429 Too Many Requests":
            navigate("/429");
            break;
          default:
            setError("Failed to fetch receptionists. Please try again later.");
        }
      } finally {
        setIsLoading(false);
      }
    };
    fetchReceptionists();
  }, [hotelId, token, navigate, clearTokenAndUser]);

  const refreshReceptionists = async () => {
    setIsLoading(true);
    try {
      const data = await getReceptionistsByHotelId(hotelId, token);

      setReceptionists(data.receptionists);
    } catch (err) {
      switch (err.message) {
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
        case "404 Not Found":
          navigate("/404");
          break;
        case "429 Too Many Requests":
          navigate("/429");
          break;
        default:
          setError("Failed to fetch receptionists. Please try again later.");
      }
    } finally {
      setIsLoading(false);
    }
  };

  return {
    receptionists,
    isLoading,
    error,
    refreshReceptionists,
  };
}

export function useCreateReceptionist(hotelId) {
  const { token, clearTokenAndUser } = useAuth();
  const navigate = useNavigate();
  const [isCreating, setIsCreating] = useState(false);
  const [creationError, setCreationError] = useState(null);
  const [formErrors, setFormErrors] = useState({});
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    userName: "",
    email: "",
    phoneNumber: "",
    password: "",
  });

  const handleInputChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleCreateReceptionistFormSubmit = async (e) => {
    e.preventDefault();

    const formDataAsFormData = new FormData();
    formDataAsFormData.append("firstName", formData.firstName);
    formDataAsFormData.append("lastName", formData.lastName);
    formDataAsFormData.append("userName", formData.userName);
    formDataAsFormData.append("email", formData.email);
    formDataAsFormData.append("phoneNumber", formData.phoneNumber);
    formDataAsFormData.append("password", formData.password);

    const validation = validateRegisterData(formDataAsFormData);

    if (!validation.isValid) {
      setCreationError("Please fix the errors below.");
      setFormErrors(validation.errors);
    }

    try {
      setIsCreating(true);
      const result = await createReceptionist(hotelId, formData, token);

      if (result) {
        if (result.success) {
          navigate(`/admin/hotels/${hotelId}/receptionists`);
        }

        if (result.error) {
          setCreationError(result.error);
        }

        if (result.errors) {
          const apiErrors = {
            firstName: result.errors.FirstName,
            lastName: result.errors.LastName,
            userName: result.errors.UserName,
            email: result.errors.Email,
            phoneNumber: result.errors.PhoneNumber,
            password: result.errors.Password,
          };
          setCreationError("Please fix the errors below.");
          setFormErrors({ ...apiErrors });
        }
      }
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
          setCreationError(
            "Failed to create receptionist. Please try again later."
          );
      }
    } finally {
      setIsCreating(false);
    }
  };

  return {
    formData,
    formErrors,
    isCreating,
    creationError,
    handleInputChange,
    handleCreateReceptionistFormSubmit,
  };
}

export function useDeleteReceptionist(refreshReceptionists) {
  const { token, clearTokenAndUser } = useAuth();
  const navigate = useNavigate();
  const [isDeleting, setIsDeleting] = useState(false);
  const [deletionError, setDeletionError] = useState(null);
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
  const [receptionist, setReceptionist] = useState({});

  const toggleDeleteModal = (receptionistInfo) => {
    setIsDeleteModalOpen(true);
    setReceptionist(receptionistInfo);
  };

  const closeDeleteModal = () => {
    setIsDeleteModalOpen(false);
    setReceptionist({});
  };

  const onConfirmDeletion = async () => {
    console.log("Deleting receptionist with ID:", receptionist);
  };

  return {
    isDeleting,
    isDeleteModalOpen,
    deletionError,
    receptionist,
    toggleDeleteModal,
    closeDeleteModal,
    onConfirmDeletion,
  };
}
