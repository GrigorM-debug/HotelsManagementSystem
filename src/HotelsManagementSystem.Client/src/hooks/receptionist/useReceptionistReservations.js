import { useAuth } from "../useAuth";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  getReservations,
  confirmReservation,
} from "../../services/receptionist/receptionist_reservations";

export function useGetReservations() {
  const { token, clearTokenAndUser } = useAuth();
  const [reservations, setReservations] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchReservations = async () => {
      setIsLoading(true);
      try {
        const data = await getReservations(token);
        setReservations(data);
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
          case "429 Too Many Requests":
            navigate("/429");
            break;
          default:
            setError("Failed to fetch reservations. Please try again later.");
        }
      } finally {
        setIsLoading(false);
      }
    };
    fetchReservations();
  }, [token, navigate, clearTokenAndUser]);

  const refreshReservations = async () => {
    setIsLoading(true);
    try {
      const data = await getReservations(token);
      setReservations(data);
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
        case "429 Too Many Requests":
          navigate("/429");
          break;
        default:
          setError("Failed to fetch reservations. Please try again later.");
      }
    } finally {
      setIsLoading(false);
    }
  };

  return {
    reservations,
    isLoading,
    error,
    refreshReservations,
  };
}

export function useConfirmReservation(refreshReservations) {
  const { token, clearTokenAndUser } = useAuth();
  const [confirmError, setConfirmError] = useState(null);
  const navigate = useNavigate();

  const confirmReservationCallBack = async (reservationId, customerId) => {
    try {
      const result = await confirmReservation(reservationId, customerId, token);

      if (result) {
        if (result.success) {
          await refreshReservations();
        }

        if (result.error) {
          setConfirmError(result.error);
        }

        if (result.errors) {
          if (result.errors.reservationId || result.errors.customerId) {
            navigate("/404");
          }
        }
      }
    } catch (err) {
      switch (err.message) {
        case "401 Unauthorized":
          clearTokenAndUser();
          navigate("/login");
          break;
        case "404 Not Found":
          navigate("/404");
          break;
        case "403 Forbidden":
          clearTokenAndUser();
          navigate("/login");
          break;
        case "429 Too Many Requests":
          navigate("/429");
          break;
        default:
          setConfirmError(
            "Failed to confirm reservation. Please try again later."
          );
      }
    }
  };

  return {
    confirmError,
    confirmReservationCallBack,
  };
}
