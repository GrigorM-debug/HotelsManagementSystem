import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";

export default function AdminUser() {
  const { isAuthenticated, user } = useAuth();

  if (!isAuthenticated && user == null && !user.roles[0] === "Admin") {
    return <Navigate to="/login" />;
  }

  return <Outlet />;
}
