import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";

export default function CustomerUser() {
  const { isAuthenticated, user } = useAuth();

  if (!isAuthenticated && user == null && !user.roles[0] === "Customer") {
    return <Navigate to="/login" />;
  }
  return <Outlet />;
}
