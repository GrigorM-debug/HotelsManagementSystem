import { useAuth } from "../../hooks/useAuth";
import { useNavigate } from "react-router-dom";
export default function Home() {
  const { user, isAuthenticated } = useAuth();
  const navigate = useNavigate();

  if (isAuthenticated && user && user.roles[0] === "Admin") {
    navigate("/admin-dashboard");
  } else if (isAuthenticated && user && user.roles[0] === "Receptionist") {
    navigate("/receptionist-dashboard");
  }

  return <h1>Home Page</h1>;
}
