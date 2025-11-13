import { useState, useEffect } from "react";
import { useAuth } from "../useAuth";
import { useNavigate } from "react-router-dom";
import { getAdminDashboardData } from "../../services/admin/admin_service";

export function useAdminDashboard() {
  const [dashboardData, setDashboardData] = useState({
    adminName: "",
    totalHotels: 0,
    totalRooms: 0,
    totalReservations: 0,
    latestHotels: [],
  });
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const { token, clearTokenAndUser } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    const fetchDashboardData = async () => {
      setIsLoading(true);
      try {
        const data = await getAdminDashboardData(token);
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
    dashboardData,
    isLoading,
    error,
  };
}
