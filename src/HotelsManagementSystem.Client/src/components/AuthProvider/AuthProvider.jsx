import { useState, useEffect } from "react";
import AuthContext from "../../contexts/AuthContext";

export default function AuthProvider({ children }) {
  const [token, setToken] = useState(null);
  const [user, setUser] = useState(null);

  useEffect(() => {
    const storedToken = localStorage.getItem("authToken");
    if (storedToken) {
      setToken(storedToken);
    }
  }, []);

  useEffect(() => {
    if (token) {
      localStorage.setItem("authToken", token);
    } else {
      localStorage.removeItem("authToken");
    }
  }, [token]);

  const clearTokenAndUser = () => {
    setToken(null);
    setUser(null);
    localStorage.removeItem("authToken");
  };

  const setTokenAndUser = (data) => {
    console.log("Data to be set: ", data);
    setToken(data.token);
    setUser(data.user);
    localStorage.setItem("authToken", data.token);
  };

  const isAuthenticated = !!token;

  console.log("Data in the provider: ", { user, token, isAuthenticated });

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
