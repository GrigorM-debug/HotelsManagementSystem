import { useAuth } from "../../useAuth";
import { useNavigate } from "react-router-dom";
import { getReceptionistsByHotelId } from "../../../services/admin/receptionsts/receptionists_service";
import { useEffect, useState } from "react";

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

  return {
    receptionists,
    isLoading,
    error,
  };
}
