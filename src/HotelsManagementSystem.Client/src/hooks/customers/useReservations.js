import { getHotelAvailableRooms } from "../../services/customers/reservations_service";
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
          case "400 Bad Request":
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
  };
}
