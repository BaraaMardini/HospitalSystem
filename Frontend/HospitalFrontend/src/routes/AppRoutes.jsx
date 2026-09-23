import {
  Routes,
  Route,
  Navigate,
} from "react-router-dom";

import ProtectedRoute from "./ProtectedRoute";

import DashboardLayout from "../layouts/DashboardLayout";

import LoginPage from "../Pages/LoginPage";

import Dashboard from "../Pages/Dashboard";
import RolePage from "../Pages/RolePage";
import PeoplePage from "../Pages/PeoplePage";
import DoctorPage from "../Pages/DoctorPage";
import DepartmentPage from "../Pages/DepartmentPage";

import AppointmentPage from "../Pages/AppointmentPage";
import EmployeePage from "../Pages/EmployeePage";
import InsurancePage from "../Pages/InsurancePage";
import InsuranceSubscriptionsPage from "../Pages/InsuranceSubscriptionsPage";
import InvoicePaymentsPage from "../Pages/InvoicePaymentsPage";
import InvoicesPage from "../Pages/InvoicesPage";
import MedicalHistoryPage from "../Pages/MedicalHistoryPage";
import PatientPage from "../Pages/PatientPage";
import PermissionsPage from "../Pages/PermissionsPage";
import RoomPage from "../Pages/RoomPage";
import RoomTypePage from "../Pages/RoomTypePage";
import SpecializationPage from "../Pages/SpecializationPage";
import StatusPage from "../Pages/StatusPage";
import StatusTypePage from "../Pages/StatusTypePage";
import UsersPage from "../Pages/UsersPage";
import Work_DayPage from "../Pages/Work_DayPage";
import Work_SchedulePage from "../Pages/Work_SchedulePage";

import PlaceholderPage from "../Pages/PlaceholderPage";
import SecurityLogsPage from "../Pages/SecurityLogsPage";

export default function AppRoutes() {
  return (
    <Routes>

      {/* =================================================
          PUBLIC
      ================================================= */}

      <Route
        path="/login"
        element={<LoginPage />}
      />


      {/* =================================================
          PROTECTED
      ================================================= */}

      <Route element={<ProtectedRoute />}>

        <Route element={<DashboardLayout />}>

          {/* =================================================
              OVERVIEW
          ================================================= */}

          <Route
            path="/"
            element={<Dashboard />}
          />


          {/* =================================================
              CLINICAL CARE
          ================================================= */}

          <Route
            path="/people"
            element={<PeoplePage />}
          />

          <Route
            path="/patients"
            element={<PatientPage />}
          />

          <Route
            path="/doctors"
            element={<DoctorPage />}
          />

          <Route
            path="/specializations"
            element={<SpecializationPage />}
          />

          <Route
            path="/appointments"
            element={<AppointmentPage />}
          />

          <Route
            path="/medical-history"
            element={<MedicalHistoryPage />}
          />


          {/* =================================================
              FACILITIES
          ================================================= */}

          <Route
            path="/departments"
            element={<DepartmentPage />}
          />

          <Route
            path="/rooms"
            element={<RoomPage />}
          />

          <Route
            path="/room-types"
            element={<RoomTypePage />}
          />


          {/* =================================================
              WORKFORCE
          ================================================= */}

          <Route
            path="/employee"
            element={<EmployeePage />}
          />

          <Route
            path="/work-days"
            element={<Work_DayPage />}
          />

          <Route
            path="/work-schedule"
            element={<Work_SchedulePage />}
          />


          {/* =================================================
              BILLING & INSURANCE
          ================================================= */}

          <Route
            path="/insurance"
            element={<InsurancePage />}
          />

          <Route
            path="/insurance-subscriptions"
            element={<InsuranceSubscriptionsPage />}
          />

          <Route
            path="/invoices"
            element={<InvoicesPage />}
          />

          <Route
            path="/invoice-payments"
            element={<InvoicePaymentsPage />}
          />


          {/* =================================================
              SYSTEM ADMINISTRATION
          ================================================= */}

          <Route
            path="/statuses"
            element={<StatusPage />}
          />

          <Route
            path="/status-types"
            element={<StatusTypePage />}
          />

          <Route
            path="/permissions"
            element={<PermissionsPage />}
          />

          <Route
            path="/role"
            element={<RolePage />}
          />

          <Route
            path="/users"
            element={<UsersPage />}
          />

          <Route
            path="/securityLogs"
            element={<SecurityLogsPage />}
          />


          {/* =================================================
              PLACEHOLDER MODULES
          ================================================= */}

          <Route
            path="/admissions"
            element={
              <PlaceholderPage title="Admissions" />
            }
          />

          <Route
            path="/pharmacy"
            element={
              <PlaceholderPage title="Pharmacy" />
            }
          />

          <Route
            path="/laboratory"
            element={
              <PlaceholderPage title="Laboratory" />
            }
          />

          <Route
            path="/radiology"
            element={
              <PlaceholderPage title="Radiology" />
            }
          />

          <Route
            path="/billing"
            element={
              <PlaceholderPage title="Billing" />
            }
          />

          <Route
            path="/inventory"
            element={
              <PlaceholderPage title="Inventory" />
            }
          />

          <Route
            path="/reports"
            element={
              <PlaceholderPage title="Reports" />
            }
          />

          <Route
            path="/settings"
            element={
              <PlaceholderPage title="Settings" />
            }
          />

        </Route>
      </Route>


      {/* =================================================
          FALLBACK
      ================================================= */}

      <Route
        path="*"
        element={
          <Navigate
            to="/"
            replace
          />
        }
      />

    </Routes>
  );
}