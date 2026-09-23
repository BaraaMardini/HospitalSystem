import {
  LayoutDashboard,
  Users,
  Stethoscope,
  CalendarCheck,
  Building2,
  UserCog,
  ShieldCheck,
  FileText,
  CreditCard,
  ClipboardList,
  CalendarDays,
  CalendarRange,
  KeyRound,
  UserCheck,
  DoorOpen,
  Bed,
  GraduationCap,
  ListChecks,
  Tags,
  FileStack,
  ScrollText,
} from "lucide-react";

export const NAV_GROUPS = [

  // =========================================================
  // OVERVIEW
  // =========================================================

  {
    label: "Overview",

    items: [
      {
        to: "/",
        label: "Dashboard",
        icon: LayoutDashboard,
        end: true,
      },
    ],
  },


  // =========================================================
  // CLINICAL CARE
  // =========================================================

  {
    label: "Clinical Care",

    items: [
      {
        to: "/people",
        label: "People",
        icon: Users,
      },

      {
        to: "/patients",
        label: "Patients",
        icon: UserCheck,
      },

      {
        to: "/doctors",
        label: "Doctors",
        icon: Stethoscope,
      },

      {
        to: "/specializations",
        label: "Specializations",
        icon: GraduationCap,
      },

      {
        to: "/appointments",
        label: "Appointments",
        icon: CalendarCheck,
      },

      {
        to: "/medical-history",
        label: "Medical History",
        icon: ClipboardList,
      },
    ],
  },


  // =========================================================
  // FACILITIES
  // =========================================================

  {
    label: "Facilities",

    items: [
      {
        to: "/departments",
        label: "Departments",
        icon: Building2,
      },

      {
        to: "/rooms",
        label: "Rooms",
        icon: DoorOpen,
      },

      {
        to: "/room-types",
        label: "Room Types",
        icon: Bed,
      },
    ],
  },


  // =========================================================
  // WORKFORCE
  // =========================================================

  {
    label: "Workforce",

    items: [
      {
        to: "/employee",
        label: "Employees",
        icon: UserCog,
      },

      {
        to: "/work-days",
        label: "Work Days",
        icon: CalendarDays,
      },

      {
        to: "/work-schedule",
        label: "Work Schedule",
        icon: CalendarRange,
      },
    ],
  },


  // =========================================================
  // BILLING & INSURANCE
  // =========================================================

  {
    label: "Billing & Insurance",

    items: [
      {
        to: "/insurance",
        label: "Insurance",
        icon: ShieldCheck,
      },

      {
        to: "/insurance-subscriptions",
        label: "Insurance Subscriptions",
        icon: FileStack,
      },

      {
        to: "/invoices",
        label: "Invoices",
        icon: FileText,
      },

      {
        to: "/invoice-payments",
        label: "Invoice Payments",
        icon: CreditCard,
      },
    ],
  },


  // =========================================================
  // SYSTEM ADMINISTRATION
  // =========================================================

  {
    label: "System Administration",

    items: [
      {
        to: "/statuses",
        label: "Statuses",
        icon: ListChecks,
      },

      {
        to: "/status-types",
        label: "Status Types",
        icon: Tags,
      },

      {
        to: "/permissions",
        label: "Permissions",
        icon: ShieldCheck,
      },

      {
        to: "/role",
        label: "Roles & Access",
        icon: KeyRound,
      },

      {
        to: "/users",
        label: "Users",
        icon: Users,
      },

      {
        to: "/securityLogs",
        label: "Security Logs",
        icon: ScrollText,
      },
    ],
  },
];

export default NAV_GROUPS;