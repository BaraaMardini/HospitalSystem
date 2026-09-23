import { useEffect, useState } from "react";

import {
  Plus,
  X,
  CheckCircle2,
  AlertCircle,
  RefreshCw,
  ScrollText,
  AlertTriangle,
  Info,
  Bug,
  ChevronDown,
} from "lucide-react";

import { applicationLogsEntity } from "../entities/ApplicationLogsEntity";
import useApplicationLogsStore from "../stores/ApplicationLogsStore";

// ============================================================
// CONFIG
// ============================================================

const config = applicationLogsEntity;
const operations = config.operations || {};

const DEFAULTS = {
  title: "Application Logs",
  description: "Track application-level events, errors and diagnostic messages.",
  addLabel: "Add Log Entry",
};

const columns =
  operations.getAll?.columns || operations.search?.columns || [];

const keyField =
  operations.update?.by ||
  operations.delete?.by ||
  config.idField ||
  "id";

const hasSearch = Boolean(operations.search);
const hasAdd = Boolean(operations.add);
const hasUpdate = Boolean(operations.update);
const hasDelete = Boolean(operations.delete);
const hasActionsColumn = hasUpdate || hasDelete;

const title = config.title || config.entity || DEFAULTS.title;
const description = config.description || DEFAULTS.description;
const addLabel = config.addLabel || DEFAULTS.addLabel;

function nativeInputType(type) {
  if (type === "number") return "number";
  if (type === "email") return "email";
  if (type === "date") return "date";
  return "text";
}

// ============================================================
// BADGE (log level)
// ============================================================

function levelBadgeTone(value) {
  const v = String(value ?? "").toLowerCase();

  if (v.includes("error") || v.includes("fatal") || v.includes("critical")) {
    return "bg-red-50 text-red-700 border-red-200";
  }

  if (v.includes("warn")) {
    return "bg-amber-50 text-amber-700 border-amber-200";
  }

  if (v.includes("info")) {
    return "bg-emerald-50 text-emerald-700 border-emerald-200";
  }

  if (v.includes("debug")) {
    return "bg-slate-100 text-slate-600 border-slate-200";
  }

  return "bg-slate-50 text-slate-600 border-slate-200";
}

// ============================================================
// TOAST
// ============================================================

function useToast() {
  const [toast, setToast] = useState(null);

  function showToast({ type, title, description }) {
    setToast({ type, title, description });

    window.clearTimeout(showToast._timer);

    showToast._timer = window.setTimeout(() => {
      setToast(null);
    }, 4000);
  }

  const ToastView = toast ? (
    <div className="fixed bottom-6 right-6 z-[100] w-[360px] max-w-[calc(100vw-32px)]">
      <div className="flex items-start gap-3 rounded-2xl border border-slate-200 bg-white p-4 shadow-xl shadow-slate-200/60">
        <div
          className={`flex h-9 w-9 shrink-0 items-center justify-center rounded-xl ${
            toast.type === "success"
              ? "bg-emerald-50 text-emerald-600"
              : "bg-red-50 text-red-600"
          }`}
        >
          {toast.type === "success" ? (
            <CheckCircle2 className="h-5 w-5" />
          ) : (
            <AlertCircle className="h-5 w-5" />
          )}
        </div>

        <div className="min-w-0 flex-1">
          <p className="text-sm font-semibold text-slate-900">{toast.title}</p>

          {toast.description && (
            <p className="mt-1 text-sm leading-5 text-slate-500">
              {toast.description}
            </p>
          )}
        </div>

        <button
          type="button"
          onClick={() => setToast(null)}
          className="rounded-lg p-1 text-slate-400 transition hover:bg-slate-100 hover:text-slate-700"
        >
          <X className="h-4 w-4" />
        </button>
      </div>
    </div>
  ) : null;

  return { showToast, ToastView };
}

// ============================================================
// FIELD INPUT
// ============================================================

function FieldInput({ field, value, onChange, options }) {
  const base =
    "w-full rounded-xl border border-slate-200 bg-white px-4 py-3 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-400 focus:ring-4 focus:ring-emerald-500/10";

  if (field.type === "select") {
    return (
      <div className="relative">
        <select
          className={`${base} appearance-none pr-10`}
          value={value ?? ""}
          onChange={(e) => onChange(e.target.value)}
        >
          <option value="" disabled>
            Select...
          </option>

          {(options || []).map((opt, idx) => (
            <option key={opt.value ?? idx} value={opt.value ?? ""}>
              {opt.label ?? opt.value ?? ""}
            </option>
          ))}
        </select>

        <ChevronDown className="pointer-events-none absolute right-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
      </div>
    );
  }

  if (field.type === "textarea") {
    return (
      <textarea
        className={base}
        rows={4}
        value={value ?? ""}
        onChange={(e) => onChange(e.target.value)}
        placeholder={field.placeholder}
      />
    );
  }

  return (
    <input
      className={base}
      type={nativeInputType(field.type)}
      value={value ?? ""}
      onChange={(e) => onChange(e.target.value)}
      placeholder={field.placeholder}
    />
  );
}

// ============================================================
// VALIDATION
// ============================================================

function validateFields(fields, values) {
  const errors = {};

  for (const field of fields) {
    const value = values[field.name];

    const isEmpty =
      value === undefined || value === null || String(value).trim() === "";

    if (field.required && isEmpty) {
      errors[field.name] = `${field.label} is required.`;
      continue;
    }

    if (isEmpty) continue;

    if (field.type === "number" && Number.isNaN(Number(value))) {
      errors[field.name] = "Enter a valid number.";
    }
  }

  return errors;
}

// ============================================================
// FORM MODAL
// ============================================================

function EntityFormModal({ open, fields, onCancel, onSubmit, submitting }) {
  const [values, setValues] = useState({});
  const [errors, setErrors] = useState({});

  useEffect(() => {
    if (open) {
      setValues({});
      setErrors({});
    }
  }, [open]);

  if (!open) return null;

  function handleSubmit(e) {
    e.preventDefault();

    const nextErrors = validateFields(fields, values);

    if (Object.keys(nextErrors).length > 0) {
      setErrors(nextErrors);
      return;
    }

    onSubmit(values);
  }

  return (
    <div className="fixed inset-0 z-[90] flex items-center justify-center bg-slate-900/40 p-4 backdrop-blur-sm">
      <form
        onSubmit={handleSubmit}
        className="flex max-h-[90vh] w-full max-w-2xl flex-col overflow-hidden rounded-3xl border border-slate-200 bg-white shadow-2xl"
      >
        <div className="flex items-center justify-between border-b border-slate-100 px-6 py-5">
          <div>
            <p className="mb-1 text-[11px] font-semibold uppercase tracking-[0.18em] text-emerald-600">
              System diagnostics
            </p>

            <h3 className="text-xl font-semibold tracking-tight text-slate-900">
              {addLabel}
            </h3>
          </div>

          <button
            type="button"
            onClick={onCancel}
            className="rounded-xl border border-slate-200 bg-white p-2 text-slate-400 transition hover:bg-slate-50 hover:text-slate-700"
          >
            <X className="h-5 w-5" />
          </button>
        </div>

        <div className="overflow-y-auto px-6 py-6">
          <div className="grid grid-cols-1 gap-5 md:grid-cols-2">
            {fields.map((field) => (
              <div
                key={field.name}
                className={field.type === "textarea" ? "md:col-span-2" : ""}
              >
                <label className="mb-2 block text-xs font-semibold text-slate-600">
                  {field.label}

                  {field.required && (
                    <span className="ml-1 text-emerald-600">*</span>
                  )}
                </label>

                <FieldInput
                  field={field}
                  value={values[field.name]}
                  options={field.options || []}
                  onChange={(value) =>
                    setValues((prev) => ({ ...prev, [field.name]: value }))
                  }
                />

                {errors[field.name] && (
                  <p className="mt-2 text-xs text-red-600">
                    {errors[field.name]}
                  </p>
                )}
              </div>
            ))}
          </div>
        </div>

        <div className="flex items-center justify-end gap-3 border-t border-slate-100 bg-slate-50/50 px-6 py-4">
          <button
            type="button"
            onClick={onCancel}
            className="rounded-xl border border-slate-200 bg-white px-5 py-2.5 text-sm font-medium text-slate-600 transition hover:bg-slate-50 hover:text-slate-900"
          >
            Cancel
          </button>

          <button
            type="submit"
            disabled={submitting}
            className="inline-flex items-center gap-2 rounded-xl bg-emerald-600 px-6 py-2.5 text-sm font-semibold text-white transition hover:bg-emerald-500 disabled:cursor-not-allowed disabled:opacity-50"
          >
            {submitting && <RefreshCw className="h-4 w-4 animate-spin" />}
            {submitting ? "Saving..." : "Save record"}
          </button>
        </div>
      </form>
    </div>
  );
}

// ============================================================
// STAT CARD
// ============================================================

function StatCard({ icon: Icon, label, value, description }) {
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

        <div className="flex h-10 w-10 items-center justify-center rounded-xl border border-emerald-100 bg-emerald-50 text-emerald-600">
          <Icon className="h-5 w-5" />
        </div>
      </div>
    </div>
  );
}

// ============================================================
// APPLICATION LOGS PAGE
// ============================================================

export default function ApplicationLogsPage() {
  const { getAllState, fetchAll, addState, add } = useApplicationLogsStore();

  const [addOpen, setAddOpen] = useState(false);
  const [refreshing, setRefreshing] = useState(false);

  const { showToast, ToastView } = useToast();

  // ==========================================================
  // LOAD
  // ==========================================================

  useEffect(() => {
    fetchAll?.();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  // ==========================================================
  // DATA
  // ==========================================================

  const rows = getAllState?.data || [];
  const loading = getAllState?.loading;

  const totalLogs = rows.length;

  const errorCount = rows.filter((row) =>
    String(row.logLevel ?? "").toLowerCase().includes("error")
  ).length;

  const warningCount = rows.filter((row) =>
    String(row.logLevel ?? "").toLowerCase().includes("warn")
  ).length;

  const infoCount = rows.filter((row) =>
    String(row.logLevel ?? "").toLowerCase().includes("info")
  ).length;

  // ==========================================================
  // REFRESH
  // ==========================================================

  const refresh = async () => {
    setRefreshing(true);

    try {
      await fetchAll?.();
    } finally {
      window.setTimeout(() => {
        setRefreshing(false);
      }, 500);
    }
  };

  // ==========================================================
  // CRUD
  // ==========================================================

  const handleAdd = async (values) => {
    await add(values);

    const result = useApplicationLogsStore.getState().addState;

    if (result.errorCode === 0) {
      showToast({
        type: "success",
        title: "Log entry created",
        description: result.message || "Log entry created successfully.",
      });

      setAddOpen(false);
      refresh();
    } else {
      showToast({
        type: "error",
        title: "Unable to create log entry",
        description: result.message,
      });
    }
  };

  // ==========================================================
  // PAGE
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
                  System diagnostics
                </span>
              </div>

              <div className="flex flex-wrap items-center gap-3">
                <h1 className="text-4xl font-semibold tracking-[-0.04em] text-slate-900 sm:text-5xl">
                  {title}
                </h1>

                <span className="inline-flex items-center rounded-full border border-slate-200 bg-slate-50 px-3 py-1.5 text-xs font-medium text-slate-500">
                  {rows.length} records
                </span>
              </div>

              <p className="mt-3 max-w-2xl text-sm leading-6 text-slate-500 sm:text-base">
                {description}
              </p>
            </div>

            <div className="flex items-center gap-3">
              <button
                type="button"
                onClick={refresh}
                disabled={refreshing}
                className="inline-flex items-center gap-2 rounded-xl border border-slate-200 bg-white px-4 py-3 text-sm font-medium text-slate-600 transition hover:bg-slate-50 hover:text-slate-900 disabled:opacity-50"
              >
                <RefreshCw
                  className={`h-4 w-4 ${refreshing ? "animate-spin" : ""}`}
                />
                Refresh
              </button>

              {hasAdd && (
                <button
                  type="button"
                  onClick={() => setAddOpen(true)}
                  className="inline-flex items-center gap-2 rounded-xl bg-emerald-600 px-5 py-3 text-sm font-semibold text-white shadow-lg shadow-emerald-200 transition hover:bg-emerald-500"
                >
                  <Plus className="h-4 w-4" />
                  {addLabel}
                </button>
              )}
            </div>
          </div>
        </section>

        {/* ====================================================
            STATS
        ==================================================== */}

        <section className="mt-5 grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-4">
          <StatCard
            icon={ScrollText}
            label="Total logs"
            value={totalLogs}
            description="Records currently visible"
          />

          <StatCard
            icon={AlertTriangle}
            label="Errors"
            value={errorCount}
            description="Error / critical entries"
          />

          <StatCard
            icon={Bug}
            label="Warnings"
            value={warningCount}
            description="Warning-level entries"
          />

          <StatCard
            icon={Info}
            label="Info"
            value={infoCount}
            description="Informational entries"
          />
        </section>

        {/* ====================================================
            TABLE
        ==================================================== */}

        <section className="mt-5 overflow-hidden rounded-2xl border border-slate-200 bg-white">
          <div className="flex flex-col gap-3 border-b border-slate-100 px-5 py-4 sm:flex-row sm:items-center sm:justify-between lg:px-6">
            <div>
              <h2 className="text-sm font-semibold text-slate-900">
                Log records
              </h2>

              <p className="mt-1 text-xs text-slate-400">
                Complete application log list
              </p>
            </div>

            <div className="flex items-center gap-2 text-xs text-slate-400">
              <span className="h-2 w-2 rounded-full bg-emerald-500" />
              Live data
            </div>
          </div>

          {/* EMPTY */}

          {!loading && rows.length === 0 ? (
            <div className="flex min-h-[420px] flex-col items-center justify-center px-6 text-center">
              <div className="mb-5 flex h-16 w-16 items-center justify-center rounded-2xl border border-slate-200 bg-slate-50 text-slate-400">
                <ScrollText className="h-7 w-7" />
              </div>

              <h3 className="text-lg font-semibold text-slate-900">
                No {title.toLowerCase()} yet
              </h3>

              <p className="mt-2 max-w-md text-sm leading-6 text-slate-500">
                Once log entries are recorded, they will appear here.
              </p>

              {hasAdd && (
                <button
                  type="button"
                  onClick={() => setAddOpen(true)}
                  className="mt-6 inline-flex items-center gap-2 rounded-xl bg-emerald-600 px-5 py-2.5 text-sm font-semibold text-white transition hover:bg-emerald-500"
                >
                  <Plus className="h-4 w-4" />
                  Add first log entry
                </button>
              )}
            </div>
          ) : (
            <div className="w-full overflow-x-auto">
              <table className="w-full min-w-[1050px] table-auto">
                <thead>
                  <tr className="border-b border-slate-100 bg-slate-50/70">
                    {columns.map((col) => (
                      <th
                        key={col.field}
                        className="whitespace-nowrap px-6 py-4 text-left text-[10px] font-semibold uppercase tracking-[0.16em] text-slate-400"
                      >
                        {col.header}
                      </th>
                    ))}

                    {hasActionsColumn && (
                      <th className="px-6 py-4 text-right text-[10px] font-semibold uppercase tracking-[0.16em] text-slate-400">
                        Actions
                      </th>
                    )}
                  </tr>
                </thead>

                {loading ? (
                  <tbody>
                    {Array.from({ length: 7 }).map((_, i) => (
                      <tr key={i} className="border-b border-slate-100">
                        {columns.map((col) => (
                          <td key={col.field} className="px-6 py-5">
                            <div className="h-4 w-2/3 animate-pulse rounded bg-slate-100" />
                          </td>
                        ))}
                      </tr>
                    ))}
                  </tbody>
                ) : (
                  <tbody>
                    {rows.map((row, index) => (
                      <tr
                        key={row[keyField] ?? index}
                        className="group border-b border-slate-100 transition-colors last:border-0 hover:bg-slate-50/70"
                      >
                        {columns.map((col, j) => {
                          const value = row[col.field];

                          if (col.field === "logLevel") {
                            return (
                              <td key={col.field} className="px-6 py-5">
                                <span
                                  className={`inline-flex items-center gap-1.5 rounded-full border px-2.5 py-1 text-[11px] font-semibold ${levelBadgeTone(
                                    value
                                  )}`}
                                >
                                  <span className="h-1.5 w-1.5 rounded-full bg-current" />
                                  {value ?? "—"}
                                </span>
                              </td>
                            );
                          }

                          if (col.field === "logMessage" || col.field === "exception") {
                            return (
                              <td key={col.field} className="px-6 py-5">
                                <span className="block max-w-[260px] truncate text-sm text-slate-500">
                                  {value || "—"}
                                </span>
                              </td>
                            );
                          }

                          if (col.field === "iPAddress") {
                            return (
                              <td key={col.field} className="px-6 py-5">
                                <span className="font-mono text-xs font-semibold text-slate-500">
                                  {value ?? "—"}
                                </span>
                              </td>
                            );
                          }

                          return (
                            <td key={col.field} className="px-6 py-5 align-middle">
                              {j === 0 ? (
                                <div className="flex items-center gap-3">
                                  <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-xl border border-slate-200 bg-slate-50 text-xs font-semibold text-slate-500">
                                    {String(value ?? "?").slice(0, 1).toUpperCase()}
                                  </div>

                                  <span className="font-medium text-slate-800">
                                    {value ?? "—"}
                                  </span>
                                </div>
                              ) : (
                                <span className="text-sm text-slate-500">
                                  {value ?? "—"}
                                </span>
                              )}
                            </td>
                          );
                        })}
                      </tr>
                    ))}
                  </tbody>
                )}
              </table>
            </div>
          )}

          {/* FOOTER */}

          {rows.length > 0 && !loading && (
            <div className="flex flex-col gap-3 border-t border-slate-100 px-5 py-4 text-xs text-slate-400 sm:flex-row sm:items-center sm:justify-between lg:px-6">
              <span>
                Showing{" "}
                <span className="font-medium text-slate-600">{rows.length}</span>{" "}
                log records
              </span>

              <span>
                Page <span className="font-medium text-slate-600">1</span> of{" "}
                <span className="font-medium text-slate-600">1</span>
              </span>
            </div>
          )}
        </section>
      </div>

      {/* ======================================================
          ADD
      ====================================================== */}

      {hasAdd && (
        <EntityFormModal
          open={addOpen}
          fields={operations.add?.fields || []}
          onCancel={() => setAddOpen(false)}
          onSubmit={handleAdd}
          submitting={addState?.loading}
        />
      )}

      {ToastView}
    </div>
  );
}