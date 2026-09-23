import {
  Bell,
  ChevronDown,
  LogOut,
  Menu,
  ShieldCheck,
} from "lucide-react";

import {
  useNavigate,
} from "react-router-dom";

import useAuthStore from "../stores/AuthStore";


function Header({
  activePage,
  onMenuClick,
}) {

  const navigate =
    useNavigate();


  const {
    user,
    logout,
  } =
    useAuthStore();


  // =========================================
  // LOGOUT
  // =========================================

  const handleLogout =
    async () => {

      await logout();

      navigate(
        "/login",
        {
          replace: true,
        }
      );
    };


  // =========================================
  // USER DISPLAY
  // =========================================

  const displayName =
    user?.username ||
    user?.email ||
    "User";


  const initials =
    displayName
      .split(" ")
      .filter(Boolean)
      .slice(0, 2)
      .map(
        (part) =>
          part[0]?.toUpperCase()
      )
      .join("") || "U";


  return (

    <header className="sticky top-0 z-30 flex h-[82px] shrink-0 items-center justify-between border-b border-slate-200/80 bg-white/95 px-4 backdrop-blur-xl sm:px-6 lg:px-8">


      {/* =====================================
          LEFT
      ===================================== */}

      <div className="flex min-w-0 items-center gap-4">


        {/* MOBILE MENU */}

        <button

          type="button"

          onClick={onMenuClick}

          aria-label="Open navigation"

          className="flex h-10 w-10 items-center justify-center rounded-xl border border-slate-200 bg-white text-slate-500 transition hover:border-cyan-200 hover:bg-cyan-50 hover:text-cyan-600 lg:hidden"

        >

          <Menu size={20} />

        </button>


        {/* PAGE TITLE */}

        <div className="min-w-0">

          <div className="flex items-center gap-3">

            <h1 className="truncate text-xl font-bold tracking-tight text-slate-900 sm:text-2xl">

              {activePage}

            </h1>


            <span className="hidden items-center gap-1.5 rounded-full bg-emerald-50 px-2.5 py-1 text-[10px] font-extrabold tracking-wide text-emerald-600 sm:inline-flex">

              <span className="h-1.5 w-1.5 rounded-full bg-emerald-500" />

              LIVE

            </span>

          </div>


          <p className="mt-1 hidden text-xs font-medium text-slate-400 sm:block">

            Overview of hospital operations

          </p>

        </div>

      </div>


      {/* =====================================
          RIGHT
      ===================================== */}

      <div className="flex items-center gap-2 sm:gap-3">


        {/* SYSTEM SECURE */}

        <div className="hidden items-center gap-2 rounded-xl border border-emerald-100 bg-emerald-50 px-3 py-2 text-emerald-700 md:flex">

          <ShieldCheck size={17} />

          <span className="text-xs font-bold">

            System Secure

          </span>

        </div>


        {/* NOTIFICATIONS */}

        <button

          type="button"

          aria-label="Notifications"

          className="relative flex h-10 w-10 items-center justify-center rounded-xl border border-slate-200 bg-white text-slate-500 transition hover:border-cyan-200 hover:bg-cyan-50 hover:text-cyan-600"

        >

          <Bell size={19} />

          <span className="absolute right-2.5 top-2.5 h-2 w-2 rounded-full border-2 border-white bg-red-500" />

        </button>


        {/* =================================
            USER
        ================================= */}

        <div className="group relative">


          <button

            type="button"

            className="flex items-center gap-2 rounded-xl border border-slate-200 bg-white px-2 py-1.5 transition hover:border-slate-300 hover:bg-slate-50"

          >

            <span className="flex h-8 w-8 items-center justify-center rounded-lg bg-[#07111F] text-[11px] font-extrabold text-white">

              {initials}

            </span>


            <span className="hidden max-w-[150px] text-left sm:block">

              <span className="block truncate text-sm font-bold text-slate-700">

                {displayName}

              </span>


              <span className="block truncate text-[10px] font-medium text-slate-400">

                {user?.role || "User"}

              </span>

            </span>


            <ChevronDown
              size={15}
              className="text-slate-400"
            />

          </button>


          {/* =================================
              DROPDOWN
          ================================= */}

          <div className="invisible absolute right-0 top-full mt-2 w-52 translate-y-1 rounded-xl border border-slate-200 bg-white p-1.5 opacity-0 shadow-xl shadow-slate-200/50 transition-all group-hover:visible group-hover:translate-y-0 group-hover:opacity-100">


            {/* USER INFO */}

            <div className="border-b border-slate-100 px-3 py-2.5">

              <p className="truncate text-sm font-bold text-slate-800">

                {displayName}

              </p>


              {user?.email && (

                <p className="mt-0.5 truncate text-xs text-slate-400">

                  {user.email}

                </p>

              )}

            </div>


            {/* LOGOUT */}

            <button

              type="button"

              onClick={handleLogout}

              className="mt-1 flex w-full items-center gap-2 rounded-lg px-3 py-2.5 text-sm font-semibold text-red-600 transition hover:bg-red-50"

            >

              <LogOut size={16} />

              <span>
                Sign out
              </span>

            </button>

          </div>

        </div>

      </div>

    </header>
  );
}


export default Header;