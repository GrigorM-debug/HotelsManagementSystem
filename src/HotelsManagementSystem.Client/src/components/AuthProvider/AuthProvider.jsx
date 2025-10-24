import { useState, useEffect } from "react";
import AuthContext from "../../contexts/AuthContext";

export default function AuthProvider({ children }) {
  const [token, setToken] = useState(null);
  const [user, setUser] = useState(null);

  useEffect(() => {
    const storedToken = localStorage.getItem("authToken");
    const storedUser = localStorage.getItem("authUser");
    if (storedToken && storedUser) {
      setToken(storedToken);
      setUser(JSON.parse(storedUser));
    }
  }, []);

  useEffect(() => {
    if (token && user) {
      localStorage.setItem("authToken", token);
      localStorage.setItem("authUser", JSON.stringify(user));
    } else {
      localStorage.removeItem("authToken");
      localStorage.removeItem("authUser");
    }
  }, [token, user]);

  //Used on logout to clear token and user info
  const clearTokenAndUser = () => {
    setToken(null);
    setUser(null);
    localStorage.removeItem("authToken");
    localStorage.removeItem("authUser");
  };

  //Used on login to set token and user info
  const setTokenAndUser = (data) => {
    setToken(data.token);
    setUser(data.user);
    localStorage.setItem("authToken", data.token);
  };

  const isAuthenticated = !!token;

  return (
    <AuthContext.Provider
      value={{
        user,
        token,
        isAuthenticated,
        setTokenAndUser,
        clearTokenAndUser,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}
