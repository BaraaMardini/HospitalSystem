import { useState, useEffect } from "react";
import {
  Plus,
  Pencil,
  Trash2,
  ShieldCheck,
  X,
  CheckCircle2,
  AlertCircle,
  RefreshCw,
  ChevronDown,
} from "lucide-react";
import { permissionsEntity } from "../entities/PermissionsEntity";
import usePermissionsStore from "../stores/PermissionsStore";

const config = permissionsEntity;
const operations = config.operations || {};

const columns = operations.getAll?.columns || operations.search?.columns || [];
const keyField = operations.update?.by || operations.delete?.by || config.idField || "id";

// `operations.search` exists on this entity, but per explicit instruction it
// is never rendered on this page — the toolbar and filtering logic never
// appear, and the list is always driven by `getAllState`.
const hasSearch = false;
const hasAdd = Boolean(operations.add);
const hasUpdate = Boolean(operations.update);
const hasDelete = Boolean(operations.delete);
const hasActionsColumn = hasUpdate || hasDelete;

const DEFAULTS = {
  title: "Permissions",
  description: "Manage system permissions.",
  addLabel: "Add Permission",
};

const title = config.title || config.entity || DEFAULTS.title;
const description = config.description || DEFAULTS.description;
const addLabel = config.addLabel || DEFAULTS.addLabel;

function nativeInputType(type) {
  if (type === "number") return "number";
  if (type === "email") return "email";
  if (type === "date") return "date";
  return "text";
}

function useToast() {
  const [toast, setToast] = useState(null);
  const showToast = ({ type, title, description }) => {
    setToast({ type, title, description });
    window.clearTimeout(showToast._timer);
    showToast._timer = window.setTimeout(() => setToast(null), 4000);
  };

  const ToastView = toast ? (
    <div className="fixed bottom-6 right-6 z-[100] w-[360px] max-w-[calc(100vw-32px)]">
      <div className="flex items-start gap-3 rounded-2xl border border-slate-200 bg-white p-4 shadow-xl shadow-slate-200/60">
        <div
          className={`flex h-9 w-9 shrink-0 items-center justify-center rounded-xl ${
            toast.type === "success" ? "bg-emerald-50 text-emerald-600" : "bg-red-50 text-red-600"
          }`}
        >
          {toast.type === "success" ? <CheckCircle2 className="h-5 w-5" /> : <AlertCircle className="h-5 w-5" />}
        </div>
        <div className="min-w-0 flex-1">
          <p className="text-sm font-semibold text-slate-900">{toast.title}</p>
          {toast.description && <p className="mt-1 text-sm leading-5 text-slate-500">{toast.description}</p>}
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

function displayValue(value) {
  if (typeof value === "boolean") return value ? "Yes" : "No";
  return value;
}

function validateFields(fields, values) {
  const errors = {};
  for (const field of fields) {
    if (field.type === "checkbox") continue;

    const value = values[field.name];
    const isEmpty = value === undefined || value === null || String(value).trim() === "";

    if (field.required && isEmpty) {
      errors[field.name] = `${field.label} is required.`;
      continue;
    }
    if (isEmpty) continue;

    if (field.type === "email") {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(value)) errors[field.name] = "Enter a valid email address.";
    }
    if (field.type === "number") {
      if (Number.isNaN(Number(value))) errors[field.name] = "Enter a valid number.";
    }
    if (field.type === "select" && field.options) {
      const valid = field.options.some((o) => o.value === value || o === value);
      if (!valid) errors[field.name] = `${field.label} must be one of the listed options.`;
    }
  }
  return errors;
}

function EntityFormModal({ open, mode, fields, initialValues, onClose, onSubmit, loading }) {
  const [values, setValues] = useState(() => {
    const init = { ...(initialValues || {}) };
    fields.forEach((f) => {
      if (f.type === "checkbox" && init[f.name] === undefined) init[f.name] = false;
    });
    return init;
  });
  const [errors, setErrors] = useState({});

  useEffect(() => {
    if (open) {
      const init = { ...(initialValues || {}) };
      fields.forEach((f) => {
        if (f.type === "checkbox" && init[f.name] === undefined) init[f.name] = false;
      });
      setValues(init);
      setErrors({});
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [open, initialValues]);

  if (!open) return null;

  const handleChange = (name, value) => {
    setValues((prev) => ({ ...prev, [name]: value }));
    setErrors((prev) => ({ ...prev, [name]: undefined }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const validationErrors = validateFields(fields, values);
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
      return;
    }
    onSubmit(values);
  };

  const base =
    "w-full rounded-xl border border-slate-200 bg-white px-4 py-3 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-400 focus:ring-4 focus:ring-emerald-500/10";

  return (
    <div className="fixed inset-0 z-[90] flex items-center justify-center bg-slate-900/40 p-4 backdrop-blur-sm">
      <form
        onSubmit={handleSubmit}
        className="flex max-h-[90vh] w-full max-w-2xl flex-col overflow-hidden rounded-3xl border border-slate-200 bg-white shadow-2xl"
      >
        <div className="flex items-center justify-between border-b border-slate-100 px-6 py-5">
          <div>
            <p className="mb-1 text-[11px] font-semibold uppercase tracking-[0.18em] text-emerald-600">
              Permissions management
            </p>
            <h3 className="text-xl font-semibold tracking-tight text-slate-900">
              {mode === "add" ? addLabel : `Edit ${title.replace(/s$/, "")}`}
            </h3>
          </div>
          <button
            type="button"
            onClick={onClose}
            className="rounded-xl border border-slate-200 bg-white p-2 text-slate-400 transition hover:bg-slate-50 hover:text-slate-700"
          >
            <X className="h-5 w-5" />
          </button>
        </div>

        <div className="overflow-y-auto px-6 py-6">
          <div className="grid grid-cols-1 gap-5 md:grid-cols-2">
            {fields.map((field) => (
              <div key={field.name} className={field.type === "textarea" ? "md:col-span-2" : ""}>
                {field.type === "checkbox" ? (
                  <label className="flex items-center gap-2 text-sm text-slate-700">
                    <input
                      type="checkbox"
                      checked={Boolean(values[field.name])}
                      onChange={(e) => handleChange(field.name, e.target.checked)}
                      className="h-4 w-4 rounded border-slate-300 text-emerald-600 focus:ring-emerald-100"
                    />
                    {field.label}
                  </label>
                ) : (
                  <>
                    <label className="mb-2 block text-xs font-semibold text-slate-600">
                      {field.label}
                      {field.required && <span className="ml-1 text-emerald-600">*</span>}
                    </label>
                    {field.type === "textarea" ? (
                      <textarea
                        className={base}
                        rows={4}
                        placeholder={field.placeholder}
                        value={values[field.name] ?? ""}
                        onChange={(e) => handleChange(field.name, e.target.value)}
                      />
                    ) : field.type === "select" ? (
                      <div className="relative">
                        <select
                          className={`${base} appearance-none pr-10`}
                          value={values[field.name] ?? ""}
                          onChange={(e) => handleChange(field.name, e.target.value)}
                        >
                          <option value="">Select...</option>
                          {(field.options || []).map((opt, idx) => (
                            <option key={opt.value ?? idx} value={opt.value ?? ""}>
                              {opt.label ?? opt.value ?? ""}
                            </option>
                          ))}
                        </select>
                        <ChevronDown className="pointer-events-none absolute right-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                      </div>
                    ) : (
                      <input
                        className={base}
                        type={nativeInputType(field.type)}
                        placeholder={field.placeholder}
                        value={values[field.name] ?? ""}
                        onChange={(e) => handleChange(field.name, e.target.value)}
                      />
                    )}
                  </>
                )}
                {errors[field.name] && <p className="mt-2 text-xs text-red-600">{errors[field.name]}</p>}
              </div>
            ))}
          </div>
        </div>

        <div className="flex items-center justify-end gap-3 border-t border-slate-100 bg-slate-50/50 px-6 py-4">
          <button
            type="button"
            onClick={onClose}
            className="rounded-xl border border-slate-200 bg-white px-5 py-2.5 text-sm font-medium text-slate-600 transition hover:bg-slate-50 hover:text-slate-900"
          >
            Cancel
          </button>
          <button
            type="submit"
            disabled={loading}
            className="inline-flex items-center gap-2 rounded-xl bg-emerald-600 px-6 py-2.5 text-sm font-semibold text-white transition hover:bg-emerald-500 disabled:cursor-not-allowed disabled:opacity-50"
          >
            {loading && <RefreshCw className="h-4 w-4 animate-spin" />}
            {loading ? "Saving..." : "Save record"}
          </button>
        </div>
      </form>
    </div>
  );
}

function ConfirmDialog({ open, onCancel, onConfirm, loading }) {
  if (!open) return null;
  return (
    <div className="fixed inset-0 z-[90] flex items-center justify-center bg-slate-900/40 p-5 backdrop-blur-sm">
      <div className="w-full max-w-md overflow-hidden rounded-3xl border border-slate-200 bg-white shadow-2xl">
        <div className="p-6">
          <div className="mb-5 flex h-12 w-12 items-center justify-center rounded-2xl bg-red-50 text-red-600">
            <Trash2 className="h-5 w-5" />
          </div>
          <h3 className="text-xl font-semibold tracking-tight text-slate-900">Delete this record?</h3>
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
            disabled={loading}
            className="rounded-xl bg-red-600 px-5 py-2.5 text-sm font-semibold text-white transition hover:bg-red-500 disabled:cursor-not-allowed disabled:opacity-50"
          >
            {loading ? "Deleting..." : "Delete record"}
          </button>
        </div>
      </div>
    </div>
  );
}

function StatCard({ icon: Icon, label, value, description }) {
  return (
    <div className="group relative overflow-hidden rounded-2xl border border-slate-200 bg-white p-5 transition duration-300 hover:-translate-y-0.5 hover:border-slate-300 hover:shadow-lg hover:shadow-slate-200/50">
      <div className="absolute -right-8 -top-8 h-24 w-24 rounded-full bg-emerald-500/[0.04] blur-2xl transition group-hover:bg-emerald-500/[0.08]" />
      <div className="relative flex items-start justify-between">
        <div>
          <p className="text-[11px] font-semibold uppercase tracking-[0.16em] text-slate-400">{label}</p>
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

export default function PermissionsPage() {
  const { getAllState, fetchAll, addState, add, updateState, update, deleteState, remove } = usePermissionsStore();

  const [addOpen, setAddOpen] = useState(false);
  const [editRow, setEditRow] = useState(null);
  const [deleteRow, setDeleteRow] = useState(null);
  const [refreshing, setRefreshing] = useState(false);

  const { showToast, ToastView } = useToast();

  useEffect(() => {
    fetchAll?.();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const rows = getAllState?.data || [];
  const loading = getAllState?.loading;
  const totalRows = rows.length;

  const refresh = async () => {
    setRefreshing(true);
    try {
      await fetchAll?.();
    } finally {
      window.setTimeout(() => setRefreshing(false), 500);
    }
  };

  const handleAdd = async (values) => {
    await add(values);
    const result = usePermissionsStore.getState().addState;
    if (result.errorCode === 0) {
      showToast({
        type: "success",
        title: "Permission created",
        description: result.message || `${title} created successfully`,
      });
      setAddOpen(false);
      refresh();
    } else {
      showToast({ type: "error", title: "Unable to create record", description: result.message });
    }
  };

  const handleUpdate = async (values) => {
    await update(editRow[keyField], values);
    const result = usePermissionsStore.getState().updateState;
    if (result.errorCode === 0) {
      showToast({
        type: "success",
        title: "Record updated",
        description: result.message || `${title} updated successfully`,
      });
      setEditRow(null);
      refresh();
    } else {
      showToast({ type: "error", title: "Unable to update record", description: result.message });
    }
  };

  const handleDelete = async () => {
    await remove(deleteRow[keyField]);
    const result = usePermissionsStore.getState().deleteState;
    if (result.errorCode === 0) {
      showToast({
        type: "success",
        title: "Record deleted",
        description: result.message || `${title} deleted successfully`,
      });
    } else {
      showToast({ type: "error", title: "Unable to delete record", description: result.message });
    }
    setDeleteRow(null);
    refresh();
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
                  Permissions management
                </span>
              </div>
              <div className="flex flex-wrap items-center gap-3">
                <h1 className="text-4xl font-semibold tracking-[-0.04em] text-slate-900 sm:text-5xl">{title}</h1>
                <span className="inline-flex items-center rounded-full border border-slate-200 bg-slate-50 px-3 py-1.5 text-xs font-medium text-slate-500">
                  {totalRows} records
                </span>
              </div>
              <p className="mt-3 max-w-2xl text-sm leading-6 text-slate-500 sm:text-base">{description}</p>
            </div>

            <div className="flex items-center gap-3">
              <button
                type="button"
                onClick={refresh}
                disabled={refreshing}
                className="inline-flex items-center gap-2 rounded-xl border border-slate-200 bg-white px-4 py-3 text-sm font-medium text-slate-600 transition hover:bg-slate-50 hover:text-slate-900 disabled:opacity-50"
              >
                <RefreshCw className={`h-4 w-4 ${refreshing ? "animate-spin" : ""}`} />
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

        {/* STATS */}
        <section className="mt-5 grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-4">
          <StatCard icon={ShieldCheck} label="Total records" value={totalRows} description="Records currently visible" />
          <StatCard icon={ShieldCheck} label="Results" value={totalRows} description="Current result count" />
          <StatCard
            icon={Plus}
            label="Management"
            value={hasAdd ? "Ready" : "View only"}
            description={hasAdd ? "Create and manage records" : "Read-only access"}
          />
          <StatCard icon={ShieldCheck} label="Access" value="System" description="Permission definitions" />
        </section>

        {/* no toolbar — search is intentionally not rendered on this page */}

        {/* TABLE */}
        <section className="mt-5 overflow-hidden rounded-2xl border border-slate-200 bg-white">
          <div className="flex flex-col gap-3 border-b border-slate-100 px-5 py-4 sm:flex-row sm:items-center sm:justify-between lg:px-6">
            <div>
              <h2 className="text-sm font-semibold text-slate-900">Permissions directory</h2>
              <p className="mt-1 text-xs text-slate-400">Complete permissions directory</p>
            </div>
            <div className="flex items-center gap-2 text-xs text-slate-400">
              <span className="h-2 w-2 rounded-full bg-emerald-500" />
              Live data
            </div>
          </div>

          {!loading && rows.length === 0 ? (
            <div className="flex min-h-[420px] flex-col items-center justify-center px-6 text-center">
              <div className="mb-5 flex h-16 w-16 items-center justify-center rounded-2xl border border-slate-200 bg-slate-50 text-slate-400">
                <ShieldCheck className="h-7 w-7" />
              </div>
              <h3 className="text-lg font-semibold text-slate-900">No {title.toLowerCase()} yet</h3>
              <p className="mt-2 max-w-md text-sm leading-6 text-slate-500">
                There are currently no records available in the system.
              </p>
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
                    {Array.from({ length: 5 }).map((_, i) => (
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
                    {rows.map((row, i) => (
                      <tr
                        key={row[keyField] ?? i}
                        className="group border-b border-slate-100 transition-colors last:border-0 hover:bg-slate-50/70"
                      >
                        {columns.map((col, j) => (
                          <td key={col.field} className="px-6 py-5 align-middle">
                            {j === 0 ? (
                              <span className="font-medium text-slate-800">
                                {String(displayValue(row[col.field]) ?? "—")}
                              </span>
                            ) : (
                              <span className="text-sm text-slate-500">
                                {String(displayValue(row[col.field]) ?? "—")}
                              </span>
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

      {addOpen && (
        <EntityFormModal
          open={addOpen}
          mode="add"
          fields={operations.add.fields}
          initialValues={{}}
          onClose={() => setAddOpen(false)}
          onSubmit={handleAdd}
          loading={addState?.loading}
        />
      )}

      {editRow && (
        <EntityFormModal
          open={Boolean(editRow)}
          mode="edit"
          fields={operations.update.fields}
          initialValues={editRow}
          onClose={() => setEditRow(null)}
          onSubmit={handleUpdate}
          loading={updateState?.loading}
        />
      )}

      <ConfirmDialog
        open={Boolean(deleteRow)}
        onCancel={() => setDeleteRow(null)}
        onConfirm={handleDelete}
        loading={deleteState?.loading}
      />

      {ToastView}
    </div>
  );
}