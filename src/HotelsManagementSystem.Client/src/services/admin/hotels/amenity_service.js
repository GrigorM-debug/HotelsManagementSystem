const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function getAmenities(token) {
  const response = await fetch(`${API_BASE_URL}/amenity`, {
    method: "GET",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    switch (response.status) {
      case 401:
        throw new Error("401 Unauthorized");
      case 404:
        throw new Error("404 Not Found");
      case 403:
        throw new Error("403 Forbidden");
      default:
        throw new Error("Failed to fetch amenities");
    }
  }

  const data = await response.json();
  return data;
}
