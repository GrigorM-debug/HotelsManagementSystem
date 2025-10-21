import { Navigate, Outlet } from "react-router-dom";
import { useAuth } from "../../hooks/useAuth";

export default function ReceptionistUser() {
  const { user, isAuthenticated } = useAuth();

  if (!isAuthenticated && user == null && user.roles[0] !== "Receptionist") {
    return <Navigate to="/login" />;
  }

  return <Outlet />;
}
