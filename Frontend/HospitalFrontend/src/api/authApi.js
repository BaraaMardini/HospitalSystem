import { API_URL } from "./apiConfig";

const AUTH_URL = `${API_URL}/LoginRequest`;


// =====================================================
// LOGIN
// =====================================================

export async function loginRequest(username, password) {

  const response = await fetch(`${AUTH_URL}/Login`, {
    method: "POST",

    headers: {
      "Content-Type": "application/json",
    },

    body: JSON.stringify({
      userName: username,
      password,
    }),
  });


  if (!response.ok) {

    let message = "Invalid username or password.";

    try {

      const text = await response.text();

      if (text) {
        message = text.replace(/^"|"$/g, "");
      }

    } catch {
      // ignore
    }

    throw new Error(message);
  }


  return response.json();
}


// =====================================================
// REFRESH
// =====================================================

export async function refreshRequest(refreshToken) {

  const response = await fetch(`${AUTH_URL}/refresh`, {

    method: "POST",

    headers: {
      "Content-Type": "application/json",
    },

    body: JSON.stringify({
      refreshToken,
    }),
  });


  if (!response.ok) {

    throw new Error(
      "Session expired. Please log in again."
    );
  }


  return response.json();
}


// =====================================================
// LOGOUT
// =====================================================

export async function logoutRequest(
  email,
  refreshToken
) {

  if (!refreshToken) {
    return;
  }


  try {

    await fetch(`${AUTH_URL}/logout`, {

      method: "POST",

      headers: {
        "Content-Type": "application/json",
      },

      body: JSON.stringify({

        email: email || "",

        refreshToken,
      }),
    });

  } catch {

    // Logout is best effort.
  }
}


// =====================================================
// LOGOUT WHEN PAGE / TAB IS CLOSED
// =====================================================

export function logoutRequestOnExit(
  email,
  refreshToken
) {

  if (!refreshToken) {
    return;
  }


  try {

    const body = new Blob(
      [
        JSON.stringify({

          email: email || "",

          refreshToken,
        }),
      ],
      {
        type: "application/json",
      }
    );


    navigator.sendBeacon(
      `${AUTH_URL}/logout`,
      body
    );

  } catch {

    // best effort
  }
}