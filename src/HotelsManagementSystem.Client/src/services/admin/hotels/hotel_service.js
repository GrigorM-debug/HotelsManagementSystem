const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function createHotel(hotelDataAsFormData, token) {
  const response = await fetch(`${API_BASE_URL}/hotel`, {
    method: "POST",
    headers: {
      //   "Content-Type": "multipart/form-data",
      Authorization: `Bearer ${token}`,
    },
    body: hotelDataAsFormData,
  });

  if (!response.ok) {
    switch (response.status) {
      case 400: {
        const errorData = await response.json();
        console.log(errorData);
        return errorData;
      }
      case 401:
        throw new Error("401 Unauthorized");
      case 403:
        throw new Error("403 Forbidden");
      case 404:
        throw new Error("404 Not Found");
      case 409: {
        const errorData = await response.json();
        return errorData;
      }
      default:
        throw new Error("Failed to create hotel");
    }
  }

  return response.json();
}

export async function getAdminHotels(token, appliedFilters) {
  const response = await fetch(
    `${API_BASE_URL}/hotel?Name=${appliedFilters.name}&City=${appliedFilters.city}&Country=${appliedFilters.country}`,
    {
      method: "GET",
      headers: {
        Authorization: `Bearer ${token}`,
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
      default:
        throw new Error("Failed to fetch hotels");
    }
  }

  return response.json();
}

export async function deleteHotel(hotelId, token) {
  const response = await fetch(`${API_BASE_URL}/hotel/${hotelId}`, {
    method: "DELETE",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    switch (response.status) {
      case 401:
        throw new Error("401 Unauthorized");
      case 403:
        throw new Error("403 Forbidden");
      case 404:
        throw new Error("404 Not Found");
      default:
        throw new Error("Failed to delete hotel");
    }
  }

  return response.json();
}
