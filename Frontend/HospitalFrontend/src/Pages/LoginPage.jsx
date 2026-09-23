import {
  useEffect,
  useState,
} from "react";

import {
  useLocation,
  useNavigate,
} from "react-router-dom";

import {
  Cross,
  Lock,
  User,
  AlertCircle,
  ShieldCheck,
} from "lucide-react";

import useAuthStore from "../stores/AuthStore";


export default function LoginPage() {

  const navigate =
    useNavigate();

  const location =
    useLocation();


  const {
    login,
    loading,
    error,
    accessToken,
  } =
    useAuthStore();


  const [
    username,
    setUsername,
  ] = useState("");


  const [
    password,
    setPassword,
  ] = useState("");


  // =========================================
  // IF ALREADY LOGGED IN
  // =========================================

  useEffect(() => {

    if (accessToken) {

      navigate(
        location.state?.from || "/",
        {
          replace: true,
        }
      );
    }

  }, [
    accessToken,
    navigate,
    location.state,
  ]);


  // =========================================
  // LOGIN
  // =========================================

  const handleSubmit =
    async (e) => {

      e.preventDefault();


      const ok =
        await login(
          username.trim(),
          password
        );


      if (ok) {

        navigate(
          location.state?.from || "/",
          {
            replace: true,
          }
        );
      }
    };


  return (

    <div className="flex min-h-screen items-center justify-center bg-slate-50 px-4">

      <div className="w-full max-w-md">

        {/* ===================================
            BRAND
        =================================== */}

        <div className="mb-7 text-center">

          <div className="mx-auto mb-4 flex h-14 w-14 items-center justify-center rounded-2xl bg-[#07111F] text-white shadow-lg">

            <Cross
              className="h-7 w-7"
              strokeWidth={2.5}
            />

          </div>


          <h1 className="text-2xl font-bold tracking-tight text-slate-900">

            MedCore

          </h1>


          <p className="mt-1 text-sm text-slate-500">

            Hospital Information System

          </p>

        </div>


        {/* ===================================
            CARD
        =================================== */}

        <div className="rounded-2xl border border-slate-200 bg-white p-7 shadow-xl shadow-slate-200/40">


          {/* HEADER */}

          <div className="mb-6">

            <div className="mb-2 flex items-center gap-2">

              <ShieldCheck
                size={18}
                className="text-cyan-600"
              />

              <span className="text-xs font-bold uppercase tracking-wider text-cyan-600">

                Secure Access

              </span>

            </div>


            <h2 className="text-xl font-bold text-slate-900">

              Welcome back

            </h2>


            <p className="mt-1 text-sm text-slate-500">

              Sign in to access the hospital system.

            </p>

          </div>


          {/* ERROR */}

          {error && (

            <div className="mb-5 flex items-start gap-2 rounded-xl border border-red-200 bg-red-50 px-3.5 py-3 text-sm text-red-700">

              <AlertCircle
                className="mt-0.5 h-4 w-4 flex-shrink-0"
              />

              <span>
                {error}
              </span>

            </div>

          )}


          {/* FORM */}

          <form
            onSubmit={handleSubmit}
            className="space-y-5"
          >


            {/* USERNAME */}

            <div>

              <label className="mb-1.5 block text-sm font-semibold text-slate-700">

                Username

              </label>


              <div className="relative">

                <User
                  className="pointer-events-none absolute left-3.5 top-1/2 h-4.5 w-4.5 -translate-y-1/2 text-slate-400"
                />


                <input

                  type="text"

                  autoComplete="username"

                  value={username}

                  onChange={(e) =>
                    setUsername(
                      e.target.value
                    )
                  }

                  required

                  placeholder="Enter your username"

                  className="w-full rounded-xl border border-slate-200 bg-white py-3 pl-11 pr-3 text-sm text-slate-700 outline-none transition placeholder:text-slate-400 focus:border-cyan-400 focus:ring-4 focus:ring-cyan-50"

                />

              </div>

            </div>


            {/* PASSWORD */}

            <div>

              <label className="mb-1.5 block text-sm font-semibold text-slate-700">

                Password

              </label>


              <div className="relative">

                <Lock
                  className="pointer-events-none absolute left-3.5 top-1/2 h-4.5 w-4.5 -translate-y-1/2 text-slate-400"
                />


                <input

                  type="password"

                  autoComplete="current-password"

                  value={password}

                  onChange={(e) =>
                    setPassword(
                      e.target.value
                    )
                  }

                  required

                  placeholder="Enter your password"

                  className="w-full rounded-xl border border-slate-200 bg-white py-3 pl-11 pr-3 text-sm text-slate-700 outline-none transition placeholder:text-slate-400 focus:border-cyan-400 focus:ring-4 focus:ring-cyan-50"

                />

              </div>

            </div>


            {/* BUTTON */}

            <button

              type="submit"

              disabled={loading}

              className="inline-flex w-full items-center justify-center rounded-xl bg-[#07111F] px-4 py-3 text-sm font-bold text-white transition hover:bg-slate-800 disabled:cursor-not-allowed disabled:opacity-60"

            >

              {loading
                ? "Signing in..."
                : "Sign in"}

            </button>

          </form>


          {/* FOOTER */}

          <div className="mt-6 border-t border-slate-100 pt-5 text-center">

            <p className="text-xs text-slate-400">

              Authorized hospital personnel only

            </p>

          </div>

        </div>

      </div>

    </div>
  );
}