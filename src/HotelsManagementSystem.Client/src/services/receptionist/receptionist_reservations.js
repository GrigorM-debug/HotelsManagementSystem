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

export async function checkInReservation(reservationId, customerId, token) {
  const response = await fetch(
    `${API_BASE_URL}/receptionistReservations/check-in-reservation/${reservationId}/customer/${customerId}`,
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
      case 400: {
        const errorData = await response.json();
        return errorData;
      }
      case 429:
        throw new Error("429 Too Many Requests");
      default:
        throw new Error("Failed to check-in reservation");
    }
  }

  return response.json();
}

export async function checkOutReservation(reservationId, customerId, token) {
  const response = await fetch(
    `${API_BASE_URL}/receptionistReservations/check-out-reservation/${reservationId}/customer/${customerId}`,
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
        throw new Error("Failed to check-out reservation");
    }
  }

  return response.json();
}

export async function cancelReservation(reservationId, customerId, token) {
  const response = await fetch(
    `${API_BASE_URL}/receptionistReservations/cancel-reservation/${reservationId}/customer/${customerId}`,
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
        throw new Error("Failed to cancel reservation");
    }
  }

  return response.json();
}
