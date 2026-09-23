import {
  Navigate,
  Outlet,
  useLocation,
} from "react-router-dom";

import useAuthStore from "../stores/AuthStore";


export default function ProtectedRoute() {

  const location =
    useLocation();

  const accessToken =
    useAuthStore(
      (state) => state.accessToken
    );


  // =========================================
  // NOT AUTHENTICATED
  // =========================================

  if (!accessToken) {

    return (
      <Navigate
        to="/login"
        replace
        state={{
          from:
            location.pathname +
            location.search,
        }}
      />
    );
  }


  // =========================================
  // AUTHENTICATED
  // =========================================

  return <Outlet />;
}
