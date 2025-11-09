import { useState, useEffect } from "react";
import { getHotelDetails } from "../../services/hotels/hotels_service";
import { useNavigate } from "react-router-dom";

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
