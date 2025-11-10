const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function createRoomGet(hotelId, token) {
  const response = await fetch(
    `${API_BASE_URL}/room/hotel/${hotelId}/rooms/create`,
    {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );

  if (!response.ok) {
    switch (response.status) {
      case 401:
        throw new Error("401 Unauthorized");
      case 403:
        throw new Error("403 Forbidden");
      case 404:
        throw new Error("404 Not Found");
      case 429:
        throw new Error("429 Too Many Requests");
      default:
        throw new Error("Failed to fetch room creation data");
    }
  }
  return response.json();
}
