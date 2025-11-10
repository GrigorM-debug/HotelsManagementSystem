const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export async function sendContactMessage(contactData) {
  const response = await fetch(`${API_BASE_URL}/contact/send-message`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(contactData),
  });

  if (!response.ok) {
    switch (response.status) {
      case 400: {
        const errorData = await response.json();
        return errorData;
      }
      case 429:
        throw new Error("429 Too Many Requests");
      default:
        throw new Error("Failed to send contact message");
    }
  }

  const data = await response.json();

  return data;
}
