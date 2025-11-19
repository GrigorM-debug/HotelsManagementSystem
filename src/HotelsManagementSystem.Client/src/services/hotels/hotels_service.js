const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function getHotelDetails(hotelId) {
  const response = await fetch(`${API_BASE_URL}/hotel/${hotelId}`, {
    method: "GET",
  });

  if (!response.ok) {
    switch (response.status) {
      case 400:
        throw new Error("400 Bad Request");
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

export async function getHotels(filter) {
  const response = await fetch(
    `${API_BASE_URL}/hotel/hotels?Name=${filter.name}&City=${filter.city}&Country=${filter.country}`,
    {
      method: "GET",
    }
  );

  if (!response.ok) {
    switch (response.status) {
      case 400: {
        const errorData = await response.json();
        return errorData;
      }
      case 429:
        throw new Error("429 Too Many Requests");
      default:
        throw new Error("Failed to fetch hotels");
    }
  }

  const data = await response.json();
  return data;
}
