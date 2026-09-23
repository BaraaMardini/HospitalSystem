import { useEffect, useState, useRef, useCallback } from "react";
import {
  Plus,
  Pencil,
  Trash2,
  Inbox,
  X,
  CheckCircle2,
  AlertCircle,
  RefreshCw,
  ChevronDown,
} from "lucide-react";
import useSpecializationStore from "../stores/SpecializationStore";
import { specializationEntity as config } from "../entities/SpecializationEntity";

/* ------------------------------------------------------------------ */
/* Derived config (unchanged)                                         */
/* ------------------------------------------------------------------ */

const hasSearch = Boolean(config.operations.search);
const hasAdd = Boolean(config.operations.add);
const hasUpdate = Boolean(config.operations.update);
const hasDelete = Boolean(config.operations.delete);
const hasActionsColumn = hasUpdate || hasDelete;

const idField =
  config.operations.update?.by || config.operations.delete?.by || config.idField || "id";

const columns = config.operations.getAll?.columns || config.operations.search?.columns || [];

const title = config.title || "Specializations";
const description = config.description || `Manage your ${title.toLowerCase()}.`;
const addLabel = config.addLabel || `Add ${title.replace(/s$/, "")}`;
const editTitle = `Edit ${title.replace(/s$/, "")}`;

/* ------------------------------------------------------------------ */
/* Toast (same hook, restyled view)                                    */
/* ------------------------------------------------------------------ */

function useToast() {
  const [toast, setToast] = useState(null);
  const timerRef = useRef(null);

  const showToast = useCallback(({ type, title, description }) => {
    if (timerRef.current) window.clearTimeout(timerRef.current);
    setToast({ type, title, description });
    timerRef.current = window.setTimeout(() => setToast(null), 4000);
  }, []);

  const dismissToast = useCallback(() => {
    if (timerRef.current) window.clearTimeout(timerRef.current);
    setToast(null);
  }, []);

  return { toast, showToast, dismissToast };
}

function Toast({ toast, onDismiss }) {
  if (!toast) return null;
  const isSuccess = toast.type === "success";
  return (
    <div className="fixed bottom-6 right-6 z-[100] w-[360px] max-w-[calc(100vw-32px)]">
      <div className="flex items-start gap-3 rounded-2xl border border-slate-200 bg-white p-4 shadow-xl shadow-slate-200/60">
        <div
          className={`flex h-9 w-9 shrink-0 items-center justify-center rounded-xl ${
            isSuccess ? "bg-emerald-50 text-emerald-600" : "bg-red-50 text-red-600"
          }`}
        >
          {isSuccess ? <CheckCircle2 className="h-5 w-5" /> : <AlertCircle className="h-5 w-5" />}
        </div>
        <div className="min-w-0 flex-1">
          <p className="text-sm font-semibold text-slate-900">{toast.title}</p>
          <p className="mt-1 text-sm leading-5 text-slate-500">{toast.description}</p>
        </div>
        <button
          type="button"
          onClick={onDismiss}
          className="rounded-lg p-1 text-slate-400 transition hover:bg-slate-100 hover:text-slate-700"
        >
          <X className="h-4 w-4" />
        </button>
      </div>
    </div>
  );
}

/* ------------------------------------------------------------------ */
/* Validation & field widgets (unchanged)                             */
/* ------------------------------------------------------------------ */

function validateFields(fields, values) {
  const errors = {};
  for (const field of fields) {
    const value = values[field.name];
    const isEmpty = value === undefined || value === null || String(value).trim() === "";

    if (field.required && isEmpty) {
      errors[field.name] = `${field.label} is required.`;
      continue;
    }
    if (isEmpty) continue;

    if (field.type === "email") {
      if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
        errors[field.name] = "Enter a valid email address.";
      }
    } else if (field.type === "number") {
      if (Number.isNaN(Number(value))) {
        errors[field.name] = "Enter a valid number.";
      }
    } else if (field.type === "select" && field.options) {
      if (!field.options.includes(value)) {
        errors[field.name] = `${field.label} must be one of the listed options.`;
      }
    }
  }
  return errors;
}

const inputBaseClass =
  "w-full rounded-xl border border-slate-200 bg-white px-4 py-3 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-400 focus:ring-4 focus:ring-emerald-500/10";

function fieldInput(field, value, onChange) {
  const type = field.type || "text";

  if (type === "select") {
    return (
      <div className="relative">
        <select
          className={`${inputBaseClass} appearance-none pr-10`}
          value={value ?? ""}
          onChange={(e) => onChange(e.target.value)}
        >
          <option value="" disabled>
            Select…
          </option>
          {(field.options || []).map((opt) => (
            <option key={opt} value={opt}>
              {opt}
            </option>
          ))}
        </select>
        <ChevronDown className="pointer-events-none absolute right-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
      </div>
    );
  }

  if (type === "textarea") {
    return (
      <textarea
        rows={4}
        className={inputBaseClass}
        value={value ?? ""}
        placeholder={field.placeholder}
        onChange={(e) => onChange(e.target.value)}
      />
    );
  }

  return (
    <input
      type={type === "number" ? "number" : type === "email" ? "email" : type === "date" ? "date" : "text"}
      className={inputBaseClass}
      value={value ?? ""}
      placeholder={field.placeholder}
      onChange={(e) => onChange(e.target.value)}
    />
  );
}

/* ------------------------------------------------------------------ */
/* Form modal (shared by Add & Edit)                                   */
/* ------------------------------------------------------------------ */

function EntityFormModal({ open, title, fields, initialValues, onCancel, onSubmit }) {
  const [values, setValues] = useState(initialValues || {});
  const [errors, setErrors] = useState({});

  useEffect(() => {
    if (open) {
      setValues(initialValues || {});
      setErrors({});
    }
  }, [open, initialValues]);

  if (!open) return null;

  const handleChange = (name, value) => {
    setValues((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const nextErrors = validateFields(fields, values);
    setErrors(nextErrors);
    if (Object.keys(nextErrors).length === 0) {
      onSubmit(values);
    }
  };

  return (
    <div className="fixed inset-0 z-[90] flex items-center justify-center bg-slate-900/40 p-4 backdrop-blur-sm">
      <form
        onSubmit={handleSubmit}
        className="flex max-h-[90vh] w-full max-w-2xl flex-col overflow-hidden rounded-3xl border border-slate-200 bg-white shadow-2xl"
      >
        <div className="flex items-center justify-between border-b border-slate-100 px-6 py-5">
          <div>
            <p className="mb-1 text-[11px] font-semibold uppercase tracking-[0.18em] text-emerald-600">
              {title || "Specialization"} management
            </p>
            <h3 className="text-xl font-semibold tracking-tight text-slate-900">{title}</h3>
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
              <div key={field.name} className={field.type === "textarea" ? "md:col-span-2" : ""}>
                <label className="mb-2 block text-xs font-semibold text-slate-600">
                  {field.label}
                  {field.required && <span className="ml-1 text-emerald-600">*</span>}
                </label>
                {fieldInput(field, values[field.name], (v) => handleChange(field.name, v))}
                {errors[field.name] && (
                  <p className="mt-2 text-xs text-red-600">{errors[field.name]}</p>
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
            className="inline-flex items-center gap-2 rounded-xl bg-emerald-600 px-6 py-2.5 text-sm font-semibold text-white transition hover:bg-emerald-500"
          >
            Save record
          </button>
        </div>
      </form>
    </div>
  );
}

/* ------------------------------------------------------------------ */
/* Confirm dialog                                                      */
/* ------------------------------------------------------------------ */

function ConfirmDialog({ open, onCancel, onConfirm }) {
  if (!open) return null;
  return (
    <div className="fixed inset-0 z-[90] flex items-center justify-center bg-slate-900/40 p-5 backdrop-blur-sm">
      <div className="w-full max-w-md overflow-hidden rounded-3xl border border-slate-200 bg-white shadow-2xl">
        <div className="p-6">
          <div className="mb-5 flex h-12 w-12 items-center justify-center rounded-2xl bg-red-50 text-red-600">
            <Trash2 className="h-5 w-5" />
          </div>
          <h3 className="text-xl font-semibold tracking-tight text-slate-900">
            Delete this record?
          </h3>
          <p className="mt-2 text-sm leading-6 text-slate-500">This action cannot be undone.</p>
        </div>
        <div className="flex justify-end gap-3 border-t border-slate-100 bg-slate-50/50 px-6 py-4">
          <button
            type="button"
            onClick={onCancel}
            className="rounded-xl border border-slate-200 bg-white px-4 py-2.5 text-sm font-medium text-slate-600 transition hover:bg-slate-50 hover:text-slate-900"
          >
            Cancel
          </button>
          <button
            type="button"
            onClick={onConfirm}
            className="rounded-xl bg-red-600 px-5 py-2.5 text-sm font-semibold text-white transition hover:bg-red-500"
          >
            Delete record
          </button>
        </div>
      </div>
    </div>
  );
}

/* ------------------------------------------------------------------ */
/* Stat card                                                           */
/* ------------------------------------------------------------------ */

function StatCard({ icon: Icon, label, value, description }) {
  return (
    <div className="group relative overflow-hidden rounded-2xl border border-slate-200 bg-white p-5 transition duration-300 hover:-translate-y-0.5 hover:border-slate-300 hover:shadow-lg hover:shadow-slate-200/50">
      <div className="absolute -right-8 -top-8 h-24 w-24 rounded-full bg-emerald-500/[0.04] blur-2xl transition group-hover:bg-emerald-500/[0.08]" />
      <div className="relative flex items-start justify-between">
        <div>
          <p className="text-[11px] font-semibold uppercase tracking-[0.16em] text-slate-400">
            {label}
          </p>
          <p className="mt-3 text-3xl font-semibold tracking-tight text-slate-900">{value}</p>
          {description && <p className="mt-1 text-xs text-slate-400">{description}</p>}
        </div>
        <div className="flex h-10 w-10 items-center justify-center rounded-xl border border-emerald-100 bg-emerald-50 text-emerald-600">
          <Icon className="h-5 w-5" />
        </div>
      </div>
    </div>
  );
}

/* ------------------------------------------------------------------ */
/* Page                                                                */
/* ------------------------------------------------------------------ */

export default function SpecializationPage() {
  const { getAllState, fetchAll, addState, add, updateState, update, deleteState, remove } =
    useSpecializationStore();

  const { toast, showToast, dismissToast } = useToast();

  const [addOpen, setAddOpen] = useState(false);
  const [editRow, setEditRow] = useState(null);
  const [deleteRow, setDeleteRow] = useState(null);

  useEffect(() => {
    fetchAll?.();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const refresh = useCallback(() => {
    fetchAll?.();
  }, [fetchAll]);

  const rows = getAllState.data || [];
  const loading = getAllState.loading;

  const handleAddSubmit = async (values) => {
    await add?.(values);
    const state = useSpecializationStore.getState().addState;
    if (state.errorCode === 0) {
      showToast({
        type: "success",
        title: "Success",
        description: state.message || `${title} created successfully`,
      });
      setAddOpen(false);
      refresh();
    } else {
      showToast({ type: "error", title: "Error", description: state.message });
    }
  };

  const handleEditSubmit = async (values) => {
    await update?.(editRow[idField], values);
    const state = useSpecializationStore.getState().updateState;
    if (state.errorCode === 0) {
      showToast({
        type: "success",
        title: "Success",
        description: state.message || `${title} updated successfully`,
      });
      setEditRow(null);
      refresh();
    } else {
      showToast({ type: "error", title: "Error", description: state.message });
    }
  };

  const handleDeleteConfirm = async () => {
    await remove?.(deleteRow[idField]);
    const state = useSpecializationStore.getState().deleteState;
    setDeleteRow(null);
    if (state.errorCode === 0) {
      showToast({
        type: "success",
        title: "Success",
        description: state.message || `${title} deleted successfully`,
      });
      refresh();
    } else {
      showToast({ type: "error", title: "Error", description: state.message });
    }
  };

  return (
    <div className="min-h-full w-full bg-slate-50 text-slate-900">
      <div className="w-full px-5 py-6 sm:px-7 lg:px-9 xl:px-10">
        {/* HEADER */}
        <section className="rounded-3xl border border-slate-200 bg-white">
          <div className="flex flex-col gap-6 px-6 py-7 lg:flex-row lg:items-end lg:justify-between lg:px-8 lg:py-8">
            <div>
              <div className="mb-3 flex items-center gap-2">
                <span className="h-2 w-2 rounded-full bg-emerald-500" />
                <span className="text-[11px] font-semibold uppercase tracking-[0.2em] text-emerald-600">
                  {title} management
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
                className="inline-flex items-center gap-2 rounded-xl border border-slate-200 bg-white px-4 py-3 text-sm font-medium text-slate-600 transition hover:bg-slate-50 hover:text-slate-900"
              >
                <RefreshCw className="h-4 w-4" />
                Refresh
              </button>
              {hasAdd && (
                <button
                  type="button"
                  onClick={() => setAddOpen(true)}
                  className="inline-flex items-center gap-2 rounded-xl bg-emerald-600 px-5 py-3 text-sm font-semibold text-white shadow-lg shadow-emerald-200 transition hover:bg-emerald-500"
                >
                  <Plus className="h-4 w-4" strokeWidth={2.5} />
                  {addLabel}
                </button>
              )}
            </div>
          </div>
        </section>

        {/* STATS */}
        <section className="mt-5 grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-3">
          <StatCard icon={Inbox} label="Total records" value={rows.length} description="Records currently visible" />
          <StatCard icon={Pencil} label="Columns" value={columns.length} description="Fields shown in the table" />
          <StatCard
            icon={Plus}
            label="Management"
            value={hasAdd ? "Ready" : "View only"}
            description={hasAdd ? "Create and manage records" : "Read-only access"}
          />
        </section>

        {/* TABLE */}
        <section className="mt-5 overflow-hidden rounded-2xl border border-slate-200 bg-white">
          <div className="flex flex-col gap-3 border-b border-slate-100 px-5 py-4 sm:flex-row sm:items-center sm:justify-between lg:px-6">
            <div>
              <h2 className="text-sm font-semibold text-slate-900">{title} directory</h2>
              <p className="mt-1 text-xs text-slate-400">Complete {title.toLowerCase()} list</p>
            </div>
            <div className="flex items-center gap-2 text-xs text-slate-400">
              <span className="h-2 w-2 rounded-full bg-emerald-500" />
              Live data
            </div>
          </div>

          {!loading && rows.length === 0 ? (
            <div className="flex min-h-[420px] flex-col items-center justify-center px-6 text-center">
              <div className="mb-5 flex h-16 w-16 items-center justify-center rounded-2xl border border-slate-200 bg-slate-50 text-slate-400">
                <Inbox className="h-7 w-7" />
              </div>
              <h3 className="text-lg font-semibold text-slate-900">No {title.toLowerCase()} yet</h3>
              <p className="mt-2 max-w-md text-sm leading-6 text-slate-500">No data available.</p>
              {hasAdd && (
                <button
                  type="button"
                  onClick={() => setAddOpen(true)}
                  className="mt-6 inline-flex items-center gap-2 rounded-xl bg-emerald-600 px-5 py-2.5 text-sm font-semibold text-white transition hover:bg-emerald-500"
                >
                  <Plus className="h-4 w-4" />
                  Add first record
                </button>
              )}
            </div>
          ) : (
            <div className="w-full overflow-x-auto">
              <table className="w-full min-w-[900px] table-auto">
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
                    {Array.from({ length: 6 }).map((_, i) => (
                      <tr key={i} className="border-b border-slate-100">
                        {columns.map((col) => (
                          <td key={col.field} className="px-6 py-5">
                            <div className="h-4 w-2/3 animate-pulse rounded bg-slate-100" />
                          </td>
                        ))}
                        {hasActionsColumn && (
                          <td className="px-6 py-5">
                            <div className="ml-auto h-8 w-20 animate-pulse rounded-lg bg-slate-100" />
                          </td>
                        )}
                      </tr>
                    ))}
                  </tbody>
                ) : (
                  <tbody>
                    {rows.map((row) => (
                      <tr
                        key={row[idField]}
                        className="group border-b border-slate-100 transition-colors last:border-0 hover:bg-slate-50/70"
                      >
                        {columns.map((col, i) => (
                          <td key={col.field} className="px-6 py-5 align-middle">
                            {i === 0 ? (
                              <div className="flex items-center gap-3">
                                <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-xl border border-slate-200 bg-slate-50 text-xs font-semibold text-slate-500">
                                  {String(row[col.field] ?? "?").slice(0, 1).toUpperCase()}
                                </div>
                                <span className="font-medium text-slate-800">{row[col.field]}</span>
                              </div>
                            ) : (
                              <span className="text-sm text-slate-500">{row[col.field]}</span>
                            )}
                          </td>
                        ))}
                        {hasActionsColumn && (
                          <td className="px-6 py-5">
                            <div className="flex items-center justify-end gap-1.5">
                              {hasUpdate && (
                                <button
                                  type="button"
                                  onClick={() => setEditRow(row)}
                                  className="flex h-9 w-9 items-center justify-center rounded-xl border border-slate-200 bg-white text-slate-400 transition hover:border-slate-300 hover:bg-slate-50 hover:text-slate-800"
                                  aria-label="Edit"
                                >
                                  <Pencil className="h-4 w-4" />
                                </button>
                              )}
                              {hasDelete && (
                                <button
                                  type="button"
                                  onClick={() => setDeleteRow(row)}
                                  className="flex h-9 w-9 items-center justify-center rounded-xl border border-slate-200 bg-white text-slate-400 transition hover:border-red-200 hover:bg-red-50 hover:text-red-600"
                                  aria-label="Delete"
                                >
                                  <Trash2 className="h-4 w-4" />
                                </button>
                              )}
                            </div>
                          </td>
                        )}
                      </tr>
                    ))}
                  </tbody>
                )}
              </table>
            </div>
          )}

          {rows.length > 0 && !loading && (
            <div className="flex flex-col gap-3 border-t border-slate-100 px-5 py-4 text-xs text-slate-400 sm:flex-row sm:items-center sm:justify-between lg:px-6">
              <span>
                Showing <span className="font-medium text-slate-600">{rows.length}</span> records
              </span>
              <span>
                Page <span className="font-medium text-slate-600">1</span> of{" "}
                <span className="font-medium text-slate-600">1</span>
              </span>
            </div>
          )}
        </section>
      </div>

      {hasAdd && (
        <EntityFormModal
          open={addOpen}
          title={addLabel}
          fields={config.operations.add.fields}
          initialValues={{}}
          onCancel={() => setAddOpen(false)}
          onSubmit={handleAddSubmit}
        />
      )}

      {hasUpdate && (
        <EntityFormModal
          open={Boolean(editRow)}
          title={editTitle}
          fields={config.operations.update.fields}
          initialValues={editRow || {}}
          onCancel={() => setEditRow(null)}
          onSubmit={handleEditSubmit}
        />
      )}

      {hasDelete && (
        <ConfirmDialog
          open={Boolean(deleteRow)}
          onCancel={() => setDeleteRow(null)}
          onConfirm={handleDeleteConfirm}
        />
      )}

      <Toast toast={toast} onDismiss={dismissToast} />
    </div>
  );
}