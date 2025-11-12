import { useState, useEffect } from "react";
import { useAuth } from "../../useAuth";
import { useNavigate } from "react-router-dom";
import { getAmenities } from "../../../services/admin/hotels/amenity_service";

export function useGetAmenities() {
  const [amenities, setAmenities] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const { token } = useAuth();
  const navigate = useNavigate();
  const [amenitiesFetchError, setAmenitiesFetchError] = useState(null);

  useEffect(() => {
    const fetchAmenities = async () => {
      try {
        const data = await getAmenities(token);
        setAmenities(data);
      } catch (error) {
        switch (error.message) {
          case "401 Unauthorized":
            navigate("/login");
            break;
          case "404 Not Found":
            navigate("/404");
            break;
          case "403 Forbidden":
            navigate("/login");
            break;
          case "429 Too Many Requests":
            navigate("/429");
            break;
          default:
            setAmenitiesFetchError(
              "Failed to fetch amenities. Please try again later."
            );
        }
      } finally {
        setIsLoading(false);
      }
    };

    fetchAmenities();
  }, [token, navigate]);

  return { amenities, isLoading, amenitiesFetchError };
}
