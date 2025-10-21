import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";

export default function AuthenticatedUser() {
  const { isAuthenticated, user } = useAuth();

  if (!isAuthenticated && user == null) {
    return <Navigate to="/login" />;
  }

  return <Outlet />;
}
