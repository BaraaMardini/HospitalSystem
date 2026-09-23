import { useEffect, useMemo, useState } from "react";

import {
  Users,
  Stethoscope,
  CalendarDays,
  Clock3,
  BedDouble,
  Building2,
  RefreshCw,
  ArrowUpRight,
  CheckCircle2,
  AlertCircle,
  Hourglass,
  Activity,
} from "lucide-react";

// ------------------------------------------------------------
// Entities + ad-hoc read-only stores.
// The Dashboard has no entity/store of its own, so it borrows the
// existing store hooks where they already exist, and spins up a
// lightweight read-only store for the rest (patients, rooms,
// employees, invoices, departments) purely to power the KPIs below.
// If any of the import paths below don't match your project's file
// names, just point them at the right file — the shape (createEntityStore(entity))
// is identical to every other *Store.js file in /stores.
// ------------------------------------------------------------

import useAppointmentStore from "../stores/AppointmentStore";
import useDoctorStore from "../stores/DoctorStore";
import { createEntityStore } from "../stores/createEntityStore";

import { patientEntity } from "../entities/PatientEntity";
import { roomEntity } from "../entities/RoomEntity";
import { employeeEntity } from "../entities/EmployeeEntity";
import { invoicesEntity } from "../entities/InvoicesEntity";
import { DepartmentEntity } from "../entities/DepartmentEntity";

const usePatientStore = createEntityStore(patientEntity);
const useRoomStore = createEntityStore(roomEntity);
const useEmployeeStore = createEntityStore(employeeEntity);
const useInvoiceStore = createEntityStore(invoicesEntity);
const useDepartmentStore = createEntityStore(DepartmentEntity);

// ============================================================
// HELPERS
// ============================================================

function isSameDay(dateLike, reference) {
  if (!dateLike) return false;

  const d = new Date(dateLike);
  if (Number.isNaN(d.getTime())) return false;

  return (
    d.getFullYear() === reference.getFullYear() &&
    d.getMonth() === reference.getMonth() &&
    d.getDate() === reference.getDate()
  );
}

function statusBucket(value) {
  const v = String(value ?? "").toLowerCase();

  if (["completed", "confirmed", "done"].some((s) => v.includes(s))) {
    return "completed";
  }

  if (["cancelled", "canceled", "rejected", "failed"].some((s) => v.includes(s))) {
    return "cancelled";
  }

  if (["pending", "scheduled", "waiting", "in progress"].some((s) => v.includes(s))) {
    return "pending";
  }

  return "other";
}

function formatCurrency(value) {
  const number = Number(value);
  if (Number.isNaN(number)) return "0";

  return new Intl.NumberFormat("en-US", {
    minimumFractionDigits: 0,
    maximumFractionDigits: 0,
  }).format(number);
}

function formatClock(date) {
  return date.toLocaleTimeString("en-US", {
    hour: "2-digit",
    minute: "2-digit",
  });
}

function formatDateLabel(date) {
  return date.toLocaleDateString("en-US", {
    weekday: "long",
    day: "numeric",
    month: "long",
    year: "numeric",
  });
}

function formatDateTime(value) {
  if (!value) return "—";
  const d = new Date(value);
  if (Number.isNaN(d.getTime())) return String(value);

  return d.toLocaleString("en-US", {
    day: "2-digit",
    month: "short",
    hour: "2-digit",
    minute: "2-digit",
  });
}

function getInitials(name) {
  if (!name) return "?";

  return String(name)
    .trim()
    .split(/\s+/)
    .slice(0, 2)
    .map((w) => w[0])
    .join("")
    .toUpperCase();
}

// ============================================================
// STAT CARD
// ============================================================

function StatCard({ icon: Icon, label, value, description, tone = "emerald" }) {
  const toneMap = {
    emerald: "border-emerald-100 bg-emerald-50 text-emerald-600",
    cyan: "border-cyan-100 bg-cyan-50 text-cyan-600",
    violet: "border-violet-100 bg-violet-50 text-violet-600",
    amber: "border-amber-100 bg-amber-50 text-amber-600",
  };

  return (
    <div className="group relative overflow-hidden rounded-2xl border border-slate-200 bg-white p-5 transition duration-300 hover:-translate-y-0.5 hover:border-slate-300 hover:shadow-lg hover:shadow-slate-200/50">
      <div className="absolute -right-8 -top-8 h-24 w-24 rounded-full bg-emerald-500/[0.04] blur-2xl transition group-hover:bg-emerald-500/[0.08]" />

      <div className="relative flex items-start justify-between">
        <div>
          <p className="text-[11px] font-semibold uppercase tracking-[0.16em] text-slate-400">
            {label}
          </p>

          <p className="mt-3 text-3xl font-semibold tracking-tight text-slate-900">
            {value}
          </p>

          {description && (
            <p className="mt-1 text-xs text-slate-400">{description}</p>
          )}
        </div>

        <div
          className={`flex h-10 w-10 items-center justify-center rounded-xl border ${toneMap[tone]}`}
        >
          <Icon className="h-5 w-5" />
        </div>
      </div>
    </div>
  );
}

// ============================================================
// STATUS BREAKDOWN BAR
// ============================================================

function StatusRow({ label, value, total, tone }) {
  const pct = total > 0 ? Math.round((value / total) * 100) : 0;

  const toneMap = {
    emerald: "bg-emerald-500",
    amber: "bg-amber-500",
    red: "bg-red-500",
    slate: "bg-slate-400",
  };

  return (
    <div>
      <div className="mb-1.5 flex items-center justify-between text-xs">
        <span className="font-medium text-slate-600">{label}</span>
        <span className="font-semibold text-slate-800">
          {value} <span className="font-normal text-slate-400">({pct}%)</span>
        </span>
      </div>

      <div className="h-2 w-full overflow-hidden rounded-full bg-slate-100">
        <div
          className={`h-full rounded-full ${toneMap[tone]} transition-all duration-500`}
          style={{ width: `${pct}%` }}
        />
      </div>
    </div>
  );
}

// ============================================================
// DASHBOARD PAGE
// ============================================================

export default function Dashboard() {
  const { getAllState: appointmentsState, fetchAll: fetchAppointments } =
    useAppointmentStore();

  const { getAllState: doctorsState, fetchAll: fetchDoctors } = useDoctorStore();
  const { getAllState: patientsState, fetchAll: fetchPatients } = usePatientStore();
  const { getAllState: roomsState, fetchAll: fetchRooms } = useRoomStore();
  const { getAllState: employeesState, fetchAll: fetchEmployees } = useEmployeeStore();
  const { getAllState: invoicesState, fetchAll: fetchInvoices } = useInvoiceStore();
  const { getAllState: departmentsState, fetchAll: fetchDepartments } =
    useDepartmentStore();

  const [now, setNow] = useState(() => new Date());
  const [refreshing, setRefreshing] = useState(false);

  // --------------------------------------------------------
  // Clock
  // --------------------------------------------------------

  useEffect(() => {
    const timer = window.setInterval(() => setNow(new Date()), 30000);
    return () => window.clearInterval(timer);
  }, []);

  // --------------------------------------------------------
  // Initial load
  // --------------------------------------------------------

  const loadAll = () =>
    Promise.all([
      fetchAppointments?.(),
      fetchDoctors?.(),
      fetchPatients?.(),
      fetchRooms?.(),
      fetchEmployees?.(),
      fetchInvoices?.(),
      fetchDepartments?.(),
    ]);

  useEffect(() => {
    loadAll();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const refresh = async () => {
    setRefreshing(true);
    try {
      await loadAll();
    } finally {
      window.setTimeout(() => setRefreshing(false), 500);
    }
  };

  const loading =
    appointmentsState?.loading ||
    doctorsState?.loading ||
    patientsState?.loading ||
    roomsState?.loading;

  // --------------------------------------------------------
  // Derived data
  // --------------------------------------------------------

  const appointments = appointmentsState?.data || [];
  const doctors = doctorsState?.data || [];
  const patients = patientsState?.data || [];
  const rooms = roomsState?.data || [];
  const employees = employeesState?.data || [];
  const invoices = invoicesState?.data || [];
  const departments = departmentsState?.data || [];

  const todayAppointments = useMemo(
    () => appointments.filter((a) => isSameDay(a.startDate, now)),
    [appointments, now]
  );

  const statusCounts = useMemo(() => {
    const counts = { completed: 0, pending: 0, cancelled: 0, other: 0 };

    appointments.forEach((a) => {
      const bucket = statusBucket(a.statusName ?? a.StatusName);
      counts[bucket] += 1;
    });

    return counts;
  }, [appointments]);

  const activeRooms = useMemo(
    () =>
      rooms.filter((r) => {
        const v = r.isActive;
        return v === true || v === "true" || v === 1 || v === "1";
      }).length,
    [rooms]
  );

  const totalRevenue = useMemo(
    () => invoices.reduce((sum, inv) => sum + (Number(inv.totalAmount) || 0), 0),
    [invoices]
  );

  const departmentBreakdown = useMemo(() => {
    const counts = new Map();

    doctors.forEach((d) => {
      const name = d.departmentName || "Unassigned";
      counts.set(name, (counts.get(name) || 0) + 1);
    });

    return Array.from(counts.entries())
      .sort((a, b) => b[1] - a[1])
      .slice(0, 5);
  }, [doctors]);

  const recentAppointments = useMemo(() => {
    return [...appointments]
      .sort((a, b) => new Date(b.startDate || 0) - new Date(a.startDate || 0))
      .slice(0, 6);
  }, [appointments]);

  const maxDepartmentCount = departmentBreakdown.length
    ? departmentBreakdown[0][1]
    : 0;

  // ==========================================================
  // RENDER
  // ==========================================================

  return (
    <div className="min-h-full w-full bg-slate-50 text-slate-900">
      <div className="w-full px-5 py-6 sm:px-7 lg:px-9 xl:px-10">
        {/* ====================================================
            HEADER
        ==================================================== */}

        <section className="rounded-3xl border border-slate-200 bg-white">
          <div className="flex flex-col gap-6 px-6 py-7 lg:flex-row lg:items-end lg:justify-between lg:px-8 lg:py-8">
            <div>
              <div className="mb-3 flex items-center gap-2">
                <span className="h-2 w-2 rounded-full bg-emerald-500" />
                <span className="text-[11px] font-semibold uppercase tracking-[0.2em] text-emerald-600">
                  Clinical operations
                </span>
              </div>

              <h1 className="text-4xl font-semibold tracking-[-0.04em] text-slate-900 sm:text-5xl">
                Dashboard
              </h1>

              <p className="mt-3 max-w-2xl text-sm leading-6 text-slate-500 sm:text-base">
                A live overview of appointments, staff and clinic activity.
              </p>
            </div>

            <div className="flex items-center gap-3">
              <div className="rounded-2xl border border-slate-200 bg-slate-50 px-4 py-3 text-right">
                <p className="text-sm font-semibold text-slate-800">
                  {formatClock(now)}
                </p>
                <p className="mt-0.5 text-xs text-slate-400">
                  {formatDateLabel(now)}
                </p>
              </div>

              <button
                type="button"
                onClick={refresh}
                disabled={refreshing}
                className="inline-flex items-center gap-2 rounded-xl border border-slate-200 bg-white px-4 py-3 text-sm font-medium text-slate-600 transition hover:bg-slate-50 hover:text-slate-900 disabled:opacity-50"
              >
                <RefreshCw className={`h-4 w-4 ${refreshing ? "animate-spin" : ""}`} />
                Refresh
              </button>
            </div>
          </div>
        </section>

        {/* ====================================================
            KPI GRID
        ==================================================== */}

        <section className="mt-5 grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-3">
          <StatCard
            icon={CalendarDays}
            label="Appointments today"
            value={todayAppointments.length}
            description={`${appointments.length} total records`}
            tone="emerald"
          />

          <StatCard
            icon={Hourglass}
            label="Pending / scheduled"
            value={statusCounts.pending}
            description="Awaiting or upcoming"
            tone="amber"
          />

          <StatCard
            icon={Users}
            label="Patients"
            value={patients.length}
            description="Registered in the system"
            tone="cyan"
          />

          <StatCard
            icon={Stethoscope}
            label="Doctors"
            value={doctors.length}
            description={`${departments.length || departmentBreakdown.length} departments covered`}
            tone="violet"
          />

          <StatCard
            icon={BedDouble}
            label="Rooms available"
            value={`${activeRooms}/${rooms.length}`}
            description="Active vs total rooms"
            tone="emerald"
          />

          <StatCard
            icon={Activity}
            label="Billed revenue"
            value={formatCurrency(totalRevenue)}
            description={`${invoices.length} invoices`}
            tone="amber"
          />
        </section>

        {/* ====================================================
            MAIN GRID
        ==================================================== */}

        <section className="mt-5 grid grid-cols-1 gap-5 xl:grid-cols-3">
          {/* RECENT APPOINTMENTS */}

          <div className="overflow-hidden rounded-2xl border border-slate-200 bg-white xl:col-span-2">
            <div className="flex items-center justify-between border-b border-slate-100 px-6 py-5">
              <div>
                <h2 className="text-sm font-semibold text-slate-900">
                  Recent appointments
                </h2>
                <p className="mt-1 text-xs text-slate-400">
                  The latest scheduled visits across the clinic
                </p>
              </div>

              <span className="inline-flex items-center gap-1.5 rounded-full border border-slate-200 bg-slate-50 px-3 py-1.5 text-xs font-medium text-slate-500">
                <Clock3 className="h-3.5 w-3.5" />
                Live
              </span>
            </div>

            {loading ? (
              <div className="space-y-3 p-6">
                {Array.from({ length: 5 }).map((_, i) => (
                  <div key={i} className="h-12 w-full animate-pulse rounded-xl bg-slate-100" />
                ))}
              </div>
            ) : recentAppointments.length === 0 ? (
              <div className="flex flex-col items-center justify-center px-6 py-16 text-center">
                <div className="mb-4 flex h-14 w-14 items-center justify-center rounded-2xl border border-slate-200 bg-slate-50 text-slate-400">
                  <CalendarDays className="h-6 w-6" />
                </div>
                <p className="text-sm font-semibold text-slate-900">
                  No appointments yet
                </p>
                <p className="mt-1 max-w-sm text-xs leading-5 text-slate-500">
                  Once appointments are scheduled, they will show up here.
                </p>
              </div>
            ) : (
              <div className="divide-y divide-slate-100">
                {recentAppointments.map((a, idx) => {
                  const bucket = statusBucket(a.statusName);

                  const toneMap = {
                    completed: "bg-emerald-50 text-emerald-700 border-emerald-200",
                    pending: "bg-amber-50 text-amber-700 border-amber-200",
                    cancelled: "bg-red-50 text-red-700 border-red-200",
                    other: "bg-slate-50 text-slate-600 border-slate-200",
                  };

                  return (
                    <div
                      key={a.id ?? idx}
                      className="flex items-center justify-between gap-4 px-6 py-4 transition hover:bg-slate-50/70"
                    >
                      <div className="flex min-w-0 items-center gap-3">
                        <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-xl border border-slate-200 bg-slate-50 text-xs font-semibold text-slate-500">
                          {getInitials(a.patientName)}
                        </div>

                        <div className="min-w-0">
                          <p className="truncate text-sm font-medium text-slate-800">
                            {a.patientName || "Unnamed patient"}
                          </p>
                          <p className="mt-0.5 truncate text-xs text-slate-400">
                            Dr. {a.doctorName || "—"} · Room {a.roomNumber || "—"}
                          </p>
                        </div>
                      </div>

                      <div className="flex shrink-0 items-center gap-3">
                        <span className="text-xs text-slate-400">
                          {formatDateTime(a.startDate)}
                        </span>

                        <span
                          className={`inline-flex items-center rounded-full border px-2.5 py-1 text-[11px] font-semibold ${toneMap[bucket]}`}
                        >
                          {a.statusName || "—"}
                        </span>
                      </div>
                    </div>
                  );
                })}
              </div>
            )}
          </div>

          {/* SIDE COLUMN */}

          <div className="flex flex-col gap-5">
            {/* STATUS BREAKDOWN */}

            <div className="rounded-2xl border border-slate-200 bg-white p-6">
              <h2 className="text-sm font-semibold text-slate-900">
                Appointment status
              </h2>
              <p className="mt-1 text-xs text-slate-400">
                Breakdown across all records
              </p>

              <div className="mt-5 space-y-4">
                <StatusRow
                  label="Completed"
                  value={statusCounts.completed}
                  total={appointments.length}
                  tone="emerald"
                />
                <StatusRow
                  label="Pending"
                  value={statusCounts.pending}
                  total={appointments.length}
                  tone="amber"
                />
                <StatusRow
                  label="Cancelled"
                  value={statusCounts.cancelled}
                  total={appointments.length}
                  tone="red"
                />
                {statusCounts.other > 0 && (
                  <StatusRow
                    label="Other"
                    value={statusCounts.other}
                    total={appointments.length}
                    tone="slate"
                  />
                )}
              </div>
            </div>

            {/* DEPARTMENTS */}

            <div className="rounded-2xl border border-slate-200 bg-white p-6">
              <div className="flex items-center justify-between">
                <div>
                  <h2 className="text-sm font-semibold text-slate-900">
                    Doctors by department
                  </h2>
                  <p className="mt-1 text-xs text-slate-400">Top 5 departments</p>
                </div>

                <Building2 className="h-4 w-4 text-slate-300" />
              </div>

              {departmentBreakdown.length === 0 ? (
                <p className="mt-5 text-xs text-slate-400">No department data yet.</p>
              ) : (
                <div className="mt-5 space-y-4">
                  {departmentBreakdown.map(([name, count]) => (
                    <div key={name}>
                      <div className="mb-1.5 flex items-center justify-between text-xs">
                        <span className="font-medium text-slate-600">{name}</span>
                        <span className="font-semibold text-slate-800">{count}</span>
                      </div>

                      <div className="h-2 w-full overflow-hidden rounded-full bg-slate-100">
                        <div
                          className="h-full rounded-full bg-violet-500 transition-all duration-500"
                          style={{
                            width: `${
                              maxDepartmentCount
                                ? Math.round((count / maxDepartmentCount) * 100)
                                : 0
                            }%`,
                          }}
                        />
                      </div>
                    </div>
                  ))}
                </div>
              )}
            </div>

            {/* QUICK FACTS */}

            <div className="rounded-2xl border border-slate-200 bg-white p-6">
              <h2 className="text-sm font-semibold text-slate-900">Staff overview</h2>

              <div className="mt-4 space-y-3 text-sm">
                <div className="flex items-center justify-between">
                  <span className="flex items-center gap-2 text-slate-500">
                    <Users className="h-4 w-4 text-slate-400" />
                    Employees
                  </span>
                  <span className="font-semibold text-slate-800">
                    {employees.length}
                  </span>
                </div>

                <div className="flex items-center justify-between">
                  <span className="flex items-center gap-2 text-slate-500">
                    <Stethoscope className="h-4 w-4 text-slate-400" />
                    Doctors
                  </span>
                  <span className="font-semibold text-slate-800">
                    {doctors.length}
                  </span>
                </div>

                <div className="flex items-center justify-between">
                  <span className="flex items-center gap-2 text-slate-500">
                    <Building2 className="h-4 w-4 text-slate-400" />
                    Departments
                  </span>
                  <span className="font-semibold text-slate-800">
                    {departments.length || departmentBreakdown.length}
                  </span>
                </div>
              </div>
            </div>
          </div>
        </section>

        {/* ====================================================
            FOOTER STRIP
        ==================================================== */}

        <section className="mt-5 flex flex-col gap-3 rounded-2xl border border-slate-200 bg-white px-6 py-4 text-xs text-slate-400 sm:flex-row sm:items-center sm:justify-between">
          <span className="inline-flex items-center gap-2">
            <CheckCircle2 className="h-3.5 w-3.5 text-emerald-500" />
            All systems synchronized
          </span>

          <span className="inline-flex items-center gap-2">
            <ArrowUpRight className="h-3.5 w-3.5" />
            {appointments.length} appointments · {patients.length} patients ·{" "}
            {doctors.length} doctors
          </span>
        </section>
      </div>
    </div>
  );
}