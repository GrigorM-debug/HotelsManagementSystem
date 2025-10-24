const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function login(loginData) {
  const response = await fetch(`${API_BASE_URL}/auth/login`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(loginData),
  });

  if (!response.ok) {
    if (response.status === 404) {
      const errorData = await response.json();
      return errorData;
    } else if (response.status === 400) {
      const errorData = await response.json();
      return errorData;
    } else {
      throw new Error("Failed to login");
    }
  }

  const data = await response.json();
  return data;
}

export async function register(registerData) {
  const response = await fetch(`${API_BASE_URL}/auth/register`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(registerData),
  });

  if (!response.ok) {
    if (response.status === 400) {
      const errorData = await response.json();
      console.log(errorData);
      return errorData;
    } else {
      throw new Error("Failed to register");
    }
  }
}

export async function logout(token) {
  const response = await fetch(`${API_BASE_URL}/auth/logout`, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });

  if (!response.ok) {
    switch (response.status) {
      case 401:
        throw new Error("401 Unauthorized");
      case 404:
        throw new Error("404 User Not Found");
      default:
        throw new Error("Failed to logout");
    }
  }
  return;
}
