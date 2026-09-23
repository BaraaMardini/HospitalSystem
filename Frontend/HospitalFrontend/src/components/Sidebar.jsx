import { NavLink } from "react-router-dom";

import {
  X,
  ChevronLeft,
  ChevronRight,
  HeartPulse,
  LogOut,
} from "lucide-react";

import NAV_GROUPS from "../config/navigation";

export default function Sidebar({
  collapsed,
  mobileOpen,
  onClose,
  onToggle,
}) {
  return (
    <>
      {/* Mobile overlay */}

      {mobileOpen && (
        <div
          className="fixed inset-0 z-40 bg-slate-950/50 backdrop-blur-sm lg:hidden"
          onClick={onClose}
        />
      )}

      <aside
        className={`
          fixed inset-y-0 left-0 z-50
          flex h-screen flex-col
          bg-[#07111F]
          text-white
          shadow-[20px_0_60px_rgba(15,23,42,0.12)]
          transition-all duration-300
          lg:sticky lg:top-0
          ${collapsed ? "lg:w-[84px]" : "lg:w-[286px]"}
          ${mobileOpen
            ? "translate-x-0"
            : "-translate-x-full lg:translate-x-0"}
        `}
      >

        {/* =====================================================
            BRAND
        ===================================================== */}

        <div className="flex h-[82px] shrink-0 items-center border-b border-white/[0.07] px-5">

          <div
            className={`
              flex min-w-0 items-center
              ${collapsed
                ? "w-full justify-center"
                : "gap-3"}
            `}
          >

            <div className="relative flex h-11 w-11 shrink-0 items-center justify-center rounded-[14px] bg-[#10BCE7] shadow-[0_10px_30px_rgba(16,188,231,0.25)]">

              <HeartPulse
                size={22}
                strokeWidth={2.5}
              />

              <span className="absolute -right-1 -top-1 h-3 w-3 rounded-full border-2 border-[#07111F] bg-emerald-400" />

            </div>

            {!collapsed && (
              <div className="min-w-0">

                <div className="flex items-center gap-2">

                  <span className="truncate text-[18px] font-extrabold tracking-[-0.04em]">
                    MedCore
                  </span>

                  <span className="rounded-md bg-white/[0.07] px-1.5 py-0.5 text-[9px] font-bold text-white/40">
                    HIS
                  </span>

                </div>

                <p className="mt-0.5 truncate text-[10px] font-medium text-white/35">
                  Hospital Intelligence System
                </p>

              </div>
            )}

          </div>


          <button
            type="button"
            onClick={onClose}
            className="ml-auto flex h-9 w-9 items-center justify-center rounded-xl text-white/40 transition hover:bg-white/[0.06] hover:text-white lg:hidden"
          >
            <X size={18} />
          </button>

        </div>


        {/* =====================================================
            NAVIGATION
        ===================================================== */}

        <nav className="flex-1 overflow-y-auto px-3 py-6 [scrollbar-width:thin]">

          {NAV_GROUPS.map((group) => (

            <div
              key={group.label}
              className="mb-7"
            >

              {!collapsed && (
                <div className="mb-3 px-3">

                  <p className="text-[10px] font-bold uppercase tracking-[0.16em] text-white/25">
                    {group.label}
                  </p>

                </div>
              )}


              <div className="space-y-1">

                {group.items.map((item) => {

                  const Icon = item.icon;

                  return (
                    <div
                      key={item.to}
                      className="group relative"
                    >

                      <NavLink
                        to={item.to}
                        end={item.end}
                        onClick={onClose}
                        className={({ isActive }) => `
                          relative flex min-h-[48px]
                          items-center
                          rounded-[14px]
                          transition-all duration-200
                          ${collapsed
                            ? "justify-center px-0"
                            : "gap-3 px-3"}
                          ${
                            isActive
                              ? "bg-[#10BCE7] text-white shadow-[0_10px_25px_rgba(16,188,231,0.18)]"
                              : "text-white/45 hover:bg-white/[0.045] hover:text-white/90"
                          }
                        `}
                      >

                        {({ isActive }) => (
                          <>

                            <span
                              className={`
                                flex h-9 w-9 shrink-0
                                items-center justify-center
                                rounded-[11px]
                                ${
                                  isActive
                                    ? "bg-white/[0.15]"
                                    : "group-hover:bg-white/[0.04]"
                                }
                              `}
                            >

                              <Icon
                                size={19}
                                strokeWidth={1.9}
                              />

                            </span>


                            {!collapsed && (
                              <span className="truncate text-[13px] font-semibold">
                                {item.label}
                              </span>
                            )}


                            {!collapsed && isActive && (
                              <span className="ml-auto h-1.5 w-1.5 rounded-full bg-white" />
                            )}

                          </>
                        )}

                      </NavLink>


                      {/* Tooltip */}

                      {collapsed && (
                        <div
                          className="
                            pointer-events-none
                            absolute left-[calc(100%+12px)]
                            top-1/2 z-[100]
                            -translate-y-1/2
                            whitespace-nowrap
                            rounded-xl
                            border border-white/[0.08]
                            bg-[#111C2D]
                            px-3 py-2
                            text-[11px]
                            font-semibold
                            text-white
                            opacity-0
                            shadow-2xl
                            transition-all
                            group-hover:opacity-100
                          "
                        >
                          {item.label}
                        </div>
                      )}

                    </div>
                  );
                })}

              </div>

            </div>

          ))}

        </nav>


        {/* =====================================================
            FOOTER
        ===================================================== */}

        <div className="shrink-0 border-t border-white/[0.07] p-3">

          {!collapsed && (
            <div className="mb-3 rounded-[15px] border border-white/[0.06] bg-white/[0.035] p-3.5">

              <div className="flex items-center gap-3">

                <div className="relative">

                  <div className="flex h-9 w-9 items-center justify-center rounded-full bg-[#10BCE7]/10 text-[#10BCE7]">
                    <HeartPulse size={17} />
                  </div>

                  <span className="absolute -right-0.5 bottom-0 h-2.5 w-2.5 rounded-full border-2 border-[#07111F] bg-emerald-400" />

                </div>

                <div className="min-w-0">

                  <p className="truncate text-[11px] font-bold text-white/80">
                    System Online
                  </p>

                  <p className="truncate text-[9.5px] text-white/30">
                    All hospital services active
                  </p>

                </div>

              </div>

            </div>
          )}


          <button
            type="button"
            onClick={onToggle}
            className="
              flex h-11 w-full
              items-center justify-center
              gap-2
              rounded-[13px]
              border border-white/[0.06]
              bg-white/[0.025]
              text-white/35
              transition
              hover:border-white/[0.1]
              hover:bg-white/[0.05]
              hover:text-white
            "
          >

            {collapsed ? (
              <ChevronRight size={18} />
            ) : (
              <>
                <ChevronLeft size={17} />

                <span className="text-[11px] font-semibold">
                  Collapse navigation
                </span>
              </>
            )}

          </button>


          {!collapsed && (
            <div className="mt-2 flex items-center justify-center gap-2 py-2 text-white/20">

              <LogOut size={13} />

              <span className="text-[10px] font-medium">
                Secure session
              </span>

            </div>
          )}

        </div>

      </aside>
    </>
  );
}