import {
  useEffect,
  useState,
} from "react";

import {
  useLocation,
  Outlet,
} from "react-router-dom";

import Header from "../components/Header";
import Sidebar from "../components/Sidebar";
import Footer from "../components/Footer";


export default function DashboardLayout() {

  const location =
    useLocation();


  const [
    sidebarCollapsed,
    setSidebarCollapsed,
  ] = useState(false);


  const [
    mobileOpen,
    setMobileOpen,
  ] = useState(false);


  const pageTitles = {

    "/":
      "Dashboard",

    "/role":
      "Role & Access",

    "/people":
      "People",

    "/doctors":
      "Doctors",

    "/appointments":
      "Appointments",

    "/admissions":
      "Admissions",

    "/departments":
      "Departments",

    "/pharmacy":
      "Pharmacy",

    "/laboratory":
      "Laboratory",

    "/radiology":
      "Radiology",

    "/billing":
      "Billing",

    "/inventory":
      "Inventory",

    "/staff":
      "Staff",

    "/reports":
      "Reports",

    "/settings":
      "Settings",

    "/insurance":
      "Insurance",

    "/insurance-subscriptions":
      "Insurance Subscriptions",

    "/invoices":
      "Invoices",

    "/invoice-payments":
      "Invoice Payments",

    "/medical-history":
      "Medical History",

    "/patients":
      "Patients",

    "/rooms":
      "Rooms",

    "/room-types":
      "Room Types",

    "/security-logs":
      "Security Logs",

    "/specializations":
      "Specializations",

    "/statuses":
      "Statuses",

    "/status-types":
      "Status Types",

    "/work-days":
      "Work Days",

    "/work-schedule":
      "Work Schedule",

    "/permissions":
      "Permissions",

    "/users":
      "Users",
  };


  const activePage =
    pageTitles[
      location.pathname
    ] ||
    "Hospital System";


  // =========================================
  // CLOSE MOBILE SIDEBAR AFTER NAVIGATION
  // =========================================

  useEffect(() => {

    setMobileOpen(false);

  }, [
    location.pathname,
  ]);


  return (

    <div className="flex min-h-screen bg-[#f5f7fa]">

      {/* =====================================
          SIDEBAR
      ===================================== */}

      <Sidebar

        collapsed={
          sidebarCollapsed
        }

        mobileOpen={
          mobileOpen
        }

        onClose={() =>
          setMobileOpen(false)
        }

        onToggle={() =>
          setSidebarCollapsed(
            (current) =>
              !current
          )
        }

      />


      {/* =====================================
          MAIN
      ===================================== */}

      <div className="flex min-w-0 flex-1 flex-col">


        {/* HEADER */}

        <Header

          activePage={
            activePage
          }

          onMenuClick={() =>
            setMobileOpen(true)
          }

        />


        {/* CONTENT */}

        <main className="flex-1">

          <Outlet />

        </main>


        {/* FOOTER */}

        <Footer />

      </div>

    </div>
  );
}