const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function getReservations(token) {
  const response = await fetch(
    `${API_BASE_URL}/receptionistReservations/reservations`,
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
      case 429:
        throw new Error("429 Too Many Requests");
      default:
        throw new Error("Failed to fetch reservations");
    }
  }

  return response.json();
}

export async function confirmReservation(reservationId, customerId, token) {
  const response = await fetch(
    `${API_BASE_URL}/receptionistReservations/confirm-reservation/${reservationId}/customer/${customerId}`,
    {
      method: "POST",
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
      case 429:
        throw new Error("429 Too Many Requests");
      case 400: {
        const errorData = await response.json();
        return errorData;
      }
      default:
        throw new Error("Failed to confirm reservation");
    }
  }

  return response.json();
}
