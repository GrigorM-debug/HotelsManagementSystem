import { useState, useEffect } from "react";
import {
  getHotelDetails,
  getHotels,
} from "../../services/hotels/hotels_service";
import { useNavigate } from "react-router-dom";
import { validateHotelsFilter } from "../../validations/hotel/hotels_filter_validations";

export function useGetHotelDetails(hotelId) {
  const [hotel, setHotel] = useState({});
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchHotelDetails = async () => {
      setIsLoading(true);
      try {
        const data = await getHotelDetails(hotelId);
        setHotel(data);
      } catch (err) {
        switch (err.message) {
          case "400 Bad Request":
            navigate("/404");
            break;
          case "404 Not Found":
            navigate("/404");
            break;
          case "429 Too Many Requests":
            navigate("/429");
            break;
          default:
            setError("Failed to fetch hotel details");
        }
      } finally {
        setIsLoading(false);
      }
    };

    fetchHotelDetails();
  }, [hotelId, navigate]);

  return {
    hotel,
    isLoading,
    error,
  };
}

export function useGetHotels() {
  const initialFilterState = {
    name: "",
    country: "",
    city: "",
  };
  const [hotels, setHotels] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const navigate = useNavigate();
  const [filter, setFilter] = useState(initialFilterState);
  const [filterValidations, setFilterValidations] = useState({});
  const [appliedFilters, setAppliedFilters] = useState(initialFilterState);

  const handleFilterChange = (e) => {
    setFilter({
      ...filter,
      [e.target.name]: e.target.value,
    });
  };

  const handleFilterReset = () => {
    setFilter(initialFilterState);
    setAppliedFilters(initialFilterState);
    setFilterValidations({});
  };

  const handleApplyFilters = () => {
    //Validate filters
    const validation = validateHotelsFilter(filter);

    if (!validation.isValid) {
      setFilterValidations(validation.errors);
      return;
    } else {
      setAppliedFilters({ ...filter });
    }
  };

  useEffect(() => {
    const fetchHotels = async () => {
      try {
        setIsLoading(true);
        const data = await getHotels(appliedFilters);
        setHotels(data.fetchedHotels);
      } catch (err) {
        switch (err.message) {
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
  }, [navigate, appliedFilters]);

  const refreshHotels = async () => {
    setIsLoading(true);
    try {
      const result = await getHotels(appliedFilters);

      if (result) {
        if (result.errors) {
          const apiErrors = {
            name: result.errors.Name || null,
            country: result.errors.Country || null,
            city: result.errors.City || null,
          };
          setFilterValidations(apiErrors);
          return;
        }

        if (result.fetchedHotels) {
          setHotels(result.fetchedHotels);
        }
      }
    } catch (err) {
      switch (err.message) {
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

  return {
    hotels,
    isLoading,
    error,
    filter,
    handleFilterChange,
    handleFilterReset,
    handleApplyFilters,
    filterValidations,
    refreshHotels,
  };
}
