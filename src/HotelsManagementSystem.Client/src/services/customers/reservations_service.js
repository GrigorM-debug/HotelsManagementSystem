const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function getHotelAvailableRooms(hotelId, appliedFilters, token) {
  const response = await fetch(
    `${API_BASE_URL}/reservation/hotel/${hotelId}/available-rooms?CheckInDate=${appliedFilters.checkInDate}&CheckOutDate=${appliedFilters.checkOutDate}&NumberOfGuests=${appliedFilters.numberOfGuests}`,
    {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
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
      case 400:
        throw new Error("400 Bad Request");
      case 429:
        throw new Error("429 Too Many Requests");
      default:
        throw new Error("Failed to fetch available rooms");
    }
  }

  return response.json();
}
