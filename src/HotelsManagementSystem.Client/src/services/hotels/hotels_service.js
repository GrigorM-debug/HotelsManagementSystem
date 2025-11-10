const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function getHotelDetails(hotelId) {
  const response = await fetch(`${API_BASE_URL}/hotel/${hotelId}`, {
    method: "GET",
  });

  if (!response.ok) {
    switch (response.status) {
      case 404:
        throw new Error("404 Not Found");
      case 429:
        throw new Error("429 Too Many Requests");
      default:
        throw new Error("Failed to fetch hotel details");
    }
  }

  const data = await response.json();
  return data;
}
