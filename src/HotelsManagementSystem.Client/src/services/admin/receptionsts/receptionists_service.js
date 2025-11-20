const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function getReceptionistsByHotelId(hotelId, token) {
  const response = await fetch(
    `${API_BASE_URL}/hotel/${hotelId}/receptionists`,
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
      case 400:
        throw new Error("400 Bad Request");
      case 404:
        throw new Error("404 Not Found");
      case 401:
        throw new Error("401 Unauthorized");
      case 403:
        throw new Error("403 Forbidden");
      case 429:
        throw new Error("429 Too Many Requests");
      default:
        throw new Error("Failed to fetch receptionists");
    }
  }
  return response.json();
}

export async function createReceptionist(hotelId, receptionistData, token) {
  const response = await fetch(
    `${API_BASE_URL}/adminReceptionist/${hotelId}/create-receptionist`,
    {
      method: "POST",
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
      body: JSON.stringify(receptionistData),
    }
  );

  if (!response.ok) {
    switch (response.status) {
      case 400: {
        const errorData = await response.json();
        return errorData;
      }
      case 401:
        throw new Error("401 Unauthorized");
      case 403:
        throw new Error("403 Forbidden");
      case 404:
        throw new Error("404 Not Found");
      case 429:
        throw new Error("429 Too Many Requests");
      default:
        throw new Error("Failed to create receptionist");
    }
  }

  const responseData = await response.json();

  return responseData;
}

export async function deleteReceptionist(hotelId, receptionistId, token) {
  const response = await fetch(
    `${API_BASE_URL}/adminReceptionist/${hotelId}/delete-receptionist/${receptionistId}`,
    {
      method: "DELETE",
      headers: {
        Authorization: `Bearer ${token}`,
        "Content-Type": "application/json",
      },
    }
  );

  if (!response.ok) {
    switch (response.status) {
      case 400:
        throw new Error("400 Bad Request");
      case 401:
        throw new Error("401 Unauthorized");
      case 403:
        throw new Error("403 Forbidden");
      case 404:
        throw new Error("404 Not Found");
      case 429:
        throw new Error("429 Too Many Requests");
      default:
        throw new Error("Failed to delete receptionist");
    }
  }
  return await response.json();
}
