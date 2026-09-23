import { create } from "zustand";

import {
  loginRequest,
  refreshRequest,
  logoutRequest,
  logoutRequestOnExit,
} from "../api/authApi";

import {
  decodeJwt,
  getExpiryMs,
} from "../utils/jwt";


const ACCESS_KEY = "hospital_access_token";
const REFRESH_KEY = "hospital_refresh_token";

let refreshTimer = null;


// =====================================================
// BUILD USER FROM JWT
// =====================================================

function buildUser(accessToken) {

  const payload = decodeJwt(accessToken);

  if (!payload) {
    return null;
  }


  return {

    // =========================================
    // USER ID
    // =========================================

    userId:
      payload[
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
      ] ||
      payload.nameid ||
      payload.sub,


    // =========================================
    // USERNAME
    // =========================================

    username:
      payload[
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"
      ] ||
      payload.name ||
      payload.unique_name,


    // =========================================
    // EMAIL
    // =========================================

    email:
      payload[
        "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
      ] ||
      payload.email ||
      payload.Email ||
      "",


    // =========================================
    // ROLE
    // =========================================

    role:
      payload[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ] ||
      payload.role ||
      "",


    // =========================================
    // PERMISSIONS
    // =========================================

    permissionMask1:
      payload.PermissionMask1 || "0",

    permissionMask2:
      payload.PermissionMask2 || "0",
  };
}


// =====================================================
// AUTH STORE
// =====================================================

const useAuthStore = create((set, get) => ({

  // ===================================================
  // INITIAL STATE
  // ===================================================

  accessToken:
    localStorage.getItem(ACCESS_KEY) || null,

  refreshToken:
    localStorage.getItem(REFRESH_KEY) || null,

  user: null,

  loading: false,

  error: "",


  // ===================================================
  // INITIALIZE
  // ===================================================

  init: async () => {

    const accessToken =
      get().accessToken;

    const refreshToken =
      get().refreshToken;


    // -----------------------------------------------
    // No tokens
    // -----------------------------------------------

    if (!accessToken || !refreshToken) {

      set({
        accessToken: null,
        refreshToken: null,
        user: null,
      });

      return false;
    }


    // -----------------------------------------------
    // Check Access Token
    // -----------------------------------------------

    const expiryMs =
      getExpiryMs(accessToken);


    // -----------------------------------------------
    // Access Token already expired
    // -----------------------------------------------

    if (
      expiryMs &&
      expiryMs <= Date.now()
    ) {

      const newToken =
        await get().refresh();

      return !!newToken;
    }


    // -----------------------------------------------
    // Access Token still valid
    // -----------------------------------------------

    const user =
      buildUser(accessToken);


    if (!user) {

      await get().logout();

      return false;
    }


    set({
      user,
    });


    // -----------------------------------------------
    // Schedule automatic refresh
    // -----------------------------------------------

    get().scheduleRefresh(
      accessToken
    );


    return true;
  },


  // ===================================================
  // LOGIN
  // ===================================================

  login: async (
    username,
    password
  ) => {

    set({
      loading: true,
      error: "",
    });


    try {

      const data =
        await loginRequest(
          username,
          password
        );


      // -----------------------------------------------
      // Validate response
      // -----------------------------------------------

      if (
        !data?.accessToken ||
        !data?.refreshToken
      ) {

        throw new Error(
          "Invalid login response."
        );
      }


      // -----------------------------------------------
      // Save tokens
      // -----------------------------------------------

      localStorage.setItem(
        ACCESS_KEY,
        data.accessToken
      );

      localStorage.setItem(
        REFRESH_KEY,
        data.refreshToken
      );


      // -----------------------------------------------
      // Build user
      // -----------------------------------------------

      const user =
        buildUser(
          data.accessToken
        );


      // -----------------------------------------------
      // Update store
      // -----------------------------------------------

      set({

        accessToken:
          data.accessToken,

        refreshToken:
          data.refreshToken,

        user,

        loading: false,

        error: "",
      });


      // -----------------------------------------------
      // Schedule refresh
      // -----------------------------------------------

      get().scheduleRefresh(
        data.accessToken
      );


      return true;

    } catch (err) {

      set({

        loading: false,

        error:
          err?.message ||
          "Login failed.",
      });


      return false;
    }
  },


  // ===================================================
  // REFRESH TOKEN
  // ===================================================

  refresh: async () => {

    const currentRefresh =
      get().refreshToken;


    // -----------------------------------------------
    // No refresh token
    // -----------------------------------------------

    if (!currentRefresh) {

      await get().logout();

      return null;
    }


    try {

      const data =
        await refreshRequest(
          currentRefresh
        );


      // -----------------------------------------------
      // Validate response
      // -----------------------------------------------

      if (
        !data?.accessToken ||
        !data?.refreshToken
      ) {

        throw new Error(
          "Invalid refresh response."
        );
      }


      // -----------------------------------------------
      // Save new tokens
      // -----------------------------------------------

      localStorage.setItem(
        ACCESS_KEY,
        data.accessToken
      );

      localStorage.setItem(
        REFRESH_KEY,
        data.refreshToken
      );


      // -----------------------------------------------
      // Build new user
      // -----------------------------------------------

      const user =
        buildUser(
          data.accessToken
        );


      // -----------------------------------------------
      // Update state
      // -----------------------------------------------

      set({

        accessToken:
          data.accessToken,

        refreshToken:
          data.refreshToken,

        user,
      });


      // -----------------------------------------------
      // Schedule next refresh
      // -----------------------------------------------

      get().scheduleRefresh(
        data.accessToken
      );


      return data.accessToken;

    } catch {

      // -----------------------------------------------
      // Refresh failed
      // Session is no longer valid
      // -----------------------------------------------

      await get().logout();

      return null;
    }
  },


  // ===================================================
  // AUTOMATIC REFRESH
  // ===================================================

  scheduleRefresh: (accessToken) => {

    // -----------------------------------------------
    // Clear previous timer
    // -----------------------------------------------

    if (refreshTimer) {

      clearTimeout(refreshTimer);

      refreshTimer = null;
    }


    const expiryMs =
      getExpiryMs(accessToken);


    if (!expiryMs) {
      return;
    }


    // -----------------------------------------------
    // Refresh 10 seconds before expiration
    // -----------------------------------------------

    const delay =
      Math.max(
        expiryMs -
          Date.now() -
          10_000,

        1_000
      );


    refreshTimer =
      setTimeout(() => {

        get().refresh();

      }, delay);
  },


  // ===================================================
  // LOGOUT
  // ===================================================

  logout: async () => {

    const currentRefresh =
      get().refreshToken;

    const currentUser =
      get().user;


    // -----------------------------------------------
    // Get email
    // -----------------------------------------------

    const email =
      currentUser?.email || "";


    // -----------------------------------------------
    // Stop refresh timer
    // -----------------------------------------------

    if (refreshTimer) {

      clearTimeout(refreshTimer);

      refreshTimer = null;
    }


    // -----------------------------------------------
    // Clear local storage
    // -----------------------------------------------

    localStorage.removeItem(
      ACCESS_KEY
    );

    localStorage.removeItem(
      REFRESH_KEY
    );


    // -----------------------------------------------
    // Clear Zustand
    // -----------------------------------------------

    set({

      accessToken: null,

      refreshToken: null,

      user: null,

      loading: false,

      error: "",
    });


    // -----------------------------------------------
    // Tell Backend
    // -----------------------------------------------

    if (currentRefresh) {

      await logoutRequest(
        email,
        currentRefresh
      );
    }
  },


  // ===================================================
  // LOGOUT WHEN TAB / PAGE CLOSES
  // ===================================================

  logoutOnExit: () => {

    const currentRefresh =
      get().refreshToken;

    const currentUser =
      get().user;


    if (!currentRefresh) {
      return;
    }


    const email =
      currentUser?.email || "";


    // -----------------------------------------------
    // Notify Backend
    // -----------------------------------------------

    logoutRequestOnExit(
      email,
      currentRefresh
    );


    // -----------------------------------------------
    // Clear local storage
    // -----------------------------------------------

    localStorage.removeItem(
      ACCESS_KEY
    );

    localStorage.removeItem(
      REFRESH_KEY
    );
  },

}));


export default useAuthStore;