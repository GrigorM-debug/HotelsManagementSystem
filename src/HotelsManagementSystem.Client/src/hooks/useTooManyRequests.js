import { useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";

export function useToManyRequests() {
  const [countdown, setCountdown] = useState(60);

  useEffect(() => {
    const timer = setInterval(() => {
      setCountdown((prev) => {
        if (prev <= 1) {
          clearInterval(timer);
          return 0;
        }
        return prev - 1;
      });
    }, 1000);

    return () => clearInterval(timer);
  }, []);
  const navigate = useNavigate();

  const handleRetry = () => {
    window.location.reload();
  };

  const handleGoHome = () => {
    navigate("/");
  };

  return {
    countdown,
    handleRetry,
    handleGoHome,
  };
}
