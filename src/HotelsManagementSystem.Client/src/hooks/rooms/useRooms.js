import { getRoomByIdAndHotelId } from "../../services/rooms/rooms_service";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

export function useGetRoomDetails(id, roomId) {
  const [room, setRoom] = useState({
    id: "",
    roomNumber: "",
    description: "",
    isAvailable: true,
    roomType: {
      id: "",
      name: "",
      capacity: 0,
      pricePerNight: 0,
    },
    features: [],
    images: [],
  });

  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchRoomDetails = async () => {
      setIsLoading(true);
      try {
        const data = await getRoomByIdAndHotelId(id, roomId);
        setRoom(data);
      } catch (err) {
        switch (err.message) {
          case "404 Not Found":
            navigate("/404");
            break;
          case "400 Bad Request":
            navigate("/404");
            break;
          case "429 Too Many Requests":
            navigate("/429");
            break;
          default:
            setError("Failed to fetch room details. Please try again later.");
        }
      } finally {
        setIsLoading(false);
      }
    };
    fetchRoomDetails();
  }, [id, roomId, navigate]);

  return {
    room,
    isLoading,
    error,
  };
}
