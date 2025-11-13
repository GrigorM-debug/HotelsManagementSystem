import { getReceptionistDashboardData } from "../../services/receptionist/receptionist_service";
import { useState, useEffect } from "react";
import { useAuth } from "../useAuth";
import { useNavigate } from "react-router-dom";

export function useReceptionistDashboard() {
  const [dashboardData, setDashboardData] = useState({
    receptionistName: "",
  });
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const { token, clearTokenAndUser } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    const fetchDashboardData = async () => {
      try {
        setIsLoading(true);
        const data = await getReceptionistDashboardData(token);
        setDashboardData(data);
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
            setError("Failed to fetch dashboard data. Please try again later.");
        }
      } finally {
        setIsLoading(false);
      }
    };

    fetchDashboardData();
  }, [token, navigate, clearTokenAndUser]);

  return {
    isLoading,
    dashboardData,
    error,
  };
}
