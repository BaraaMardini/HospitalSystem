import useAuthStore from "../stores/AuthStore";

let refreshPromise = null;

export async function apiFetch(url, options = {}) {
  const getToken = () =>
    useAuthStore.getState().accessToken;

  let accessToken = getToken();

  const headers = {
    ...(options.headers || {}),
    ...(accessToken
      ? {
          Authorization: `Bearer ${accessToken}`,
        }
      : {}),
  };

  let response = await fetch(url, {
    ...options,
    headers,
  });

  // =====================================================
  // ACCESS TOKEN EXPIRED
  // =====================================================

  if (response.status === 401 && accessToken) {

    // إذا في refresh شغال حالياً، ننتظره
    if (!refreshPromise) {
      refreshPromise =
        useAuthStore
          .getState()
          .refresh()
          .finally(() => {
            refreshPromise = null;
          });
    }

    const newToken = await refreshPromise;

    // Refresh failed
    if (!newToken) {
      return response;
    }

    // ===================================================
    // Retry original request with new Access Token
    // ===================================================

    response = await fetch(url, {
      ...options,
      headers: {
        ...(options.headers || {}),
        Authorization: `Bearer ${newToken}`,
      },
    });
  }

  return response;
}