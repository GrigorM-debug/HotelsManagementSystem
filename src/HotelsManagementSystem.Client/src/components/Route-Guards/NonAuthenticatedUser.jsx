import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";

export default function NonAuthenticatedUser() {
  const { isAuthenticated, user } = useAuth();

  if (isAuthenticated && user != null) {
    return <Navigate to="/" />;
  }
  return <Outlet />;
}
