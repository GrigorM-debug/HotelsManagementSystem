const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function getRoomByIdAndHotelId(id, roomId) {
  const response = await fetch(
    `${API_BASE_URL}/room/hotel/${id}/rooms/${roomId}`,
    {
      method: "GET",
    }
  );

  if (!response.ok) {
    switch (response.status) {
      case 404:
        throw new Error("404");
      case 400:
        throw new Error("400");
      case 429:
        throw new Error("429");
      default:
        throw new Error("Failed to fetch room details");
    }
  }

  return response.json();
}
