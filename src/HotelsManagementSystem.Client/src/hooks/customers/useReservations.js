import {
  getHotelAvailableRooms,
  bookRoom,
  getCustomerReservations,
  cancelReservation,
} from "../../services/customers/reservations_service";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../useAuth";

export function useGetHotelAvailableRooms(hotelId) {
  const initialFilterState = {
    checkInDate: "",
    checkOutDate: "",
    numberOfGuests: 0,
  };
  const [rooms, setRooms] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const { token, clearTokenAndUser } = useAuth();
  const navigate = useNavigate();
  const [filter, setFilter] = useState(initialFilterState);
  const [appliedFilters, setAppliedFilters] = useState(initialFilterState);
  const [validationErrors, setValidationErrors] = useState({});

  const handleFilterChange = (e) => {
    const { name, value } = e.target;
    setFilter((prevFilter) => ({
      ...prevFilter,
      [name]: value,
    }));
  };

  const handleFilterReset = () => {
    setFilter(initialFilterState);
    setAppliedFilters(initialFilterState);
  };

  const handleApplyFilters = () => {
    setAppliedFilters({ ...filter });
  };

  useEffect(() => {
    const fetchAvailableRooms = async () => {
      setIsLoading(true);
      try {
        const data = await getHotelAvailableRooms(
          hotelId,
          appliedFilters,
          token
        );

        if (data.errors) {
          if (data.errors.hotelId) {
            navigate("/404");
            return;
          }

          const apiErrors = {
            checkInDate: data.errors.checkInDate,
            checkOutDate: data.errors.checkOutDate,
            numberOfGuests: data.errors.numberOfGuests,
          };

          setValidationErrors(apiErrors);
          return;
        }

        setRooms(data);
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
            setError(
              "Failed to fetch available rooms. Please try again later."
            );
        }
      } finally {
        setIsLoading(false);
      }
    };
    fetchAvailableRooms();
  }, [hotelId, appliedFilters, token, navigate, clearTokenAndUser]);

  return {
    rooms,
    isLoading,
    error,
    filter,
    appliedFilters,
    handleFilterChange,
    handleFilterReset,
    handleApplyFilters,
    validationErrors,
  };
}

export function useBookRoom() {
  const [isBooking, setIsBooking] = useState(false);
  const [bookingError, setBookingError] = useState(null);
  const { token, clearTokenAndUser } = useAuth();
  const navigate = useNavigate();

  const handleBookRoom = async (hotelId, roomId, reservationInfo) => {
    setIsBooking(true);
    try {
      const result = await bookRoom(hotelId, roomId, reservationInfo, token);

      if (result) {
        if (result.error) {
          setBookingError(result.error);
          return;
        }

        if (result.errors) {
          if (result.errors.hotelId || result.errors.roomId) {
            navigate("/404");
            return;
          }
        }

        if (result.success) {
          navigate("/my-reservations");
          return;
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
          setBookingError("Failed to book room. Please try again later.");
      }
    } finally {
      setIsBooking(false);
    }
  };

  return {
    isBooking,
    bookingError,
    handleBookRoom,
  };
}

export function useGetCustomerReservations() {
  const [reservations, setReservations] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const { token, clearTokenAndUser } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    const fetchReservations = async () => {
      setIsLoading(true);
      try {
        const data = await getCustomerReservations(token);
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

  const refreshCustomerReservations = async () => {
    setIsLoading(true);
    try {
      const data = await getCustomerReservations(token);
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
    refreshCustomerReservations,
  };
}

export function useCancelReservation(refreshCustomerReservations) {
  const [isCancelling, setIsCancelling] = useState(false);
  const [cancellationError, setCancellationError] = useState(null);
  const { token, clearTokenAndUser } = useAuth();
  const navigate = useNavigate();

  const handleCancelReservation = async (reservationId) => {
    setIsCancelling(true);
    try {
      const result = await cancelReservation(reservationId, token);
      if (result) {
        if (result.error) {
          setCancellationError(result.error);
          return;
        }

        if (result.errors) {
          if (result.errors.reservationId) {
            navigate("/404");
          }
        }

        if (result.success) {
          await refreshCustomerReservations();
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
        case "429 Too Many Requests":
          navigate("/429");
          break;
        default:
          setCancellationError(
            "Failed to cancel reservation. Please try again later."
          );
      }
    } finally {
      setIsCancelling(false);
    }
  };

  return {
    isCancelling,
    cancellationError,
    handleCancelReservation,
  };
}
