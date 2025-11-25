import { createReview } from "../../services/customers/review_service";
import { useState } from "react";
import { useAuth } from "../useAuth";
import { useNavigate } from "react-router-dom";
import { validateReviewData } from "../../validations/review/review_validations";

export function useCreateReview(hotelId) {
  const [formData, setFormData] = useState({
    rating: 1,
    comment: "",
  });
  const { token, clearTokenAndUser } = useAuth();
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const navigate = useNavigate();
  const [validationErrors, setValidationErrors] = useState({});

  const handleInputChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value,
    });
  };

  const handleCreateReview = async (reviewData) => {
    const validation = validateReviewData(reviewData);

    if (!validation.isValid) {
      setValidationErrors(validation.errors);
      return;
    }

    setIsLoading(true);
    try {
      const result = await createReview(hotelId, reviewData, token);

      if (result) {
        if (result.error) {
          setError(result.error);
          return;
        }

        if (result.errors) {
          if (result.errors.hotelId) {
            navigate("/404");
            return;
          } else {
            const apiErrors = {
              rating: result.errors.Rating,
              comment: result.errors.Comment,
            };
            setValidationErrors(apiErrors);
            return;
          }
        }

        if (result.success) {
          navigate(`/hotels/${hotelId}`);
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
        default:
          setError("Failed to create review");
      }
    } finally {
      setIsLoading(false);
    }
  };

  return {
    formData,
    isLoading,
    error,
    handleInputChange,
    handleCreateReview,
    validationErrors,
  };
}
