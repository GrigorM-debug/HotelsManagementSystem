const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function getReceptionistDashboardData(token) {
  const response = await fetch(`${API_BASE_URL}/receptionist/dashboard`, {
    method: "GET",
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
      case 429:
        throw new Error("429 Too Many Requests");
      default:
        throw new Error("Failed to fetch receptionist dashboard data");
    }
  }

  return response.json();
}
