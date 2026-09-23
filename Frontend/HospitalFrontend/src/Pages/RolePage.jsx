// src/Pages/RolePage.jsx
import { useEffect, useState } from "react";
import {
  Plus,
  Pencil,
  Trash2,
  Tag,
  X,
  CheckCircle2,
  AlertCircle,
  RefreshCw,
} from "lucide-react";
import useRoleStore from "../stores/RoleStore";
import { roleEntity } from "../entities/RoleEntity";

// ---- derived config ----
const columns = roleEntity.operations.getAll?.columns ?? roleEntity.operations.search?.columns ?? [];
const keyField = roleEntity.operations.update?.by ?? roleEntity.operations.delete?.by ?? roleEntity.idField ?? "id";
const hasSearch = Boolean(roleEntity.operations.search); // false — no search box, no filters
const hasAdd = Boolean(roleEntity.operations.add);
const hasUpdate = Boolean(roleEntity.operations.update);
const hasDelete = Boolean(roleEntity.operations.delete);
const hasActionsColumn = hasUpdate || hasDelete;

const DEFAULTS = {
  title: "Roles",
  description: "Manage system roles.",
  addLabel: "Add Role",
};

const title = roleEntity.title || DEFAULTS.title;
const description = roleEntity.description || DEFAULTS.description;
const addLabel = roleEntity.addLabel || DEFAULTS.addLabel;
const addFields = roleEntity.operations.add?.fields ?? [];
const editFields = roleEntity.operations.update?.fields ?? [];

function badgeTone(value) {
  const v = String(value ?? "").toLowerCase();
  const green = ["active", "confirmed", "completed", "paid", "in stock", "ready", "admitted"];
  const amber = ["pending", "low stock", "processing", "under observation", "scheduled", "under review"];
  const red = ["cancelled", "overdue", "out of stock", "on leave"];
  if (green.some((k) => v.includes(k))) return "bg-emerald-50 text-emerald-700 border-emerald-200";
  if (amber.some((k) => v.includes(k))) return "bg-amber-50 text-amber-700 border-amber-200";
  if (red.some((k) => v.includes(k))) return "bg-red-50 text-red-700 border-red-200";
  return "bg-slate-50 text-slate-600 border-slate-200";
}

function useToast() {
  const [toast, setToast] = useState(null);

  const showToast = ({ type, title, description }) => {
    setToast({ type, title, description });
  };

  useEffect(() => {
    if (!toast) return;
    const t = setTimeout(() => setToast(null), 4000);
    return () => clearTimeout(t);
  }, [toast]);

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
          {toast.description ? <p className="mt-1 text-sm leading-5 text-slate-500">{toast.description}</p> : null}
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

function FormField({ field, value, error, onChange }) {
  const base =
    "w-full rounded-xl border border-slate-200 bg-white px-4 py-3 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-400 focus:ring-4 focus:ring-emerald-500/10";

  let control;
  if (field.type === "textarea") {
    control = (
      <textarea
        id={field.name}
        className={base}
        rows={4}
        value={value ?? ""}
        onChange={(e) => onChange(field.name, e.target.value)}
      />
    );
  } else {
    control = (
      <input
        id={field.name}
        className={base}
        type={field.type === "number" ? "number" : field.type === "email" ? "email" : field.type === "date" ? "date" : "text"}
        placeholder={field.placeholder}
        value={value ?? ""}
        onChange={(e) => onChange(field.name, e.target.value)}
      />
    );
  }

  return (
    <div>
      <label htmlFor={field.name} className="mb-2 block text-xs font-semibold text-slate-600">
        {field.label}
        {field.required ? <span className="ml-1 text-emerald-600">*</span> : null}
      </label>
      {control}
      {error ? <p className="mt-2 text-xs text-red-600">{error}</p> : null}
    </div>
  );
}

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

    if (field.type === "email" && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
      errors[field.name] = "Enter a valid email address.";
    } else if (field.type === "number" && Number.isNaN(Number(value))) {
      errors[field.name] = "Enter a valid number.";
    } else if (field.type === "select" && field.options && !field.options.includes(value)) {
      errors[field.name] = `${field.label} must be one of the listed options.`;
    }
  }
  return errors;
}

function EntityFormModal({ open, mode, fields, initialValues, loading, onSubmit, onClose }) {
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
    setErrors((prev) => ({ ...prev, [name]: undefined }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const nextErrors = validateFields(fields, values);
    if (Object.keys(nextErrors).length > 0) {
      setErrors(nextErrors);
      return;
    }
    onSubmit(values);
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
              Role management
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
                <FormField field={field} value={values[field.name]} error={errors[field.name]} onChange={handleChange} />
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

function ConfirmDialog({ open, loading, onConfirm, onClose }) {
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
            onClick={onClose}
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

export default function RolePage() {
  const { getAllState, addState, updateState, deleteState, fetchAll, add, update, remove } = useRoleStore();

  const { showToast, ToastView } = useToast();

  useEffect(() => {
    fetchAll?.();
  }, []);

  const rows = getAllState.data;
  const isLoading = getAllState.loading;
  const rowCount = rows?.length ?? 0;

  const [refreshing, setRefreshing] = useState(false);

  const refresh = async () => {
    setRefreshing(true);
    try {
      await fetchAll?.();
    } finally {
      window.setTimeout(() => setRefreshing(false), 500);
    }
  };

  const [showAddModal, setShowAddModal] = useState(false);
  const [editingRow, setEditingRow] = useState(null);
  const [deletingRow, setDeletingRow] = useState(null);

  const handleAddSubmit = async (values) => {
    await add(values);
    const result = useRoleStore.getState().addState;
    if (result.errorCode === 0) {
      showToast({ type: "success", title: "Role created", description: result.message || `${title} created successfully` });
      setShowAddModal(false);
      refresh();
    } else {
      showToast({ type: "error", title: "Unable to create record", description: result.message });
    }
  };

  const handleEditSubmit = async (values) => {
    await update(editingRow[keyField], values);
    const result = useRoleStore.getState().updateState;
    if (result.errorCode === 0) {
      showToast({ type: "success", title: "Record updated", description: result.message || `${title} updated successfully` });
      setEditingRow(null);
      refresh();
    } else {
      showToast({ type: "error", title: "Unable to update record", description: result.message });
    }
  };

  const handleDeleteConfirm = async () => {
    await remove(deletingRow[keyField]);
    const result = useRoleStore.getState().deleteState;
    if (result.errorCode === 0) {
      showToast({ type: "success", title: "Record deleted", description: result.message || `${title} deleted successfully` });
    } else {
      showToast({ type: "error", title: "Unable to delete record", description: result.message });
    }
    setDeletingRow(null);
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
                  Role management
                </span>
              </div>
              <div className="flex flex-wrap items-center gap-3">
                <h1 className="text-4xl font-semibold tracking-[-0.04em] text-slate-900 sm:text-5xl">{title}</h1>
                <span className="inline-flex items-center rounded-full border border-slate-200 bg-slate-50 px-3 py-1.5 text-xs font-medium text-slate-500">
                  {rowCount} records
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
              {hasAdd ? (
                <button
                  type="button"
                  onClick={() => setShowAddModal(true)}
                  className="inline-flex items-center gap-2 rounded-xl bg-emerald-600 px-5 py-3 text-sm font-semibold text-white shadow-lg shadow-emerald-200 transition hover:bg-emerald-500"
                >
                  <Plus className="h-4 w-4" />
                  {addLabel}
                </button>
              ) : null}
            </div>
          </div>
        </section>

        {/* STATS */}
        <section className="mt-5 grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-4">
          <StatCard icon={Tag} label="Total records" value={rowCount} description="Records currently visible" />
          <StatCard icon={Tag} label="Results" value={rowCount} description="Current result count" />
          <StatCard
            icon={Plus}
            label="Management"
            value={hasAdd ? "Ready" : "View only"}
            description={hasAdd ? "Create and manage records" : "Read-only access"}
          />
          <StatCard icon={Tag} label="Access" value="System" description="Role definitions" />
        </section>

        {/* no toolbar row — hasSearch is false, so no search box, no filters */}

        {/* TABLE */}
        <section className="mt-5 overflow-hidden rounded-2xl border border-slate-200 bg-white">
          <div className="flex flex-col gap-3 border-b border-slate-100 px-5 py-4 sm:flex-row sm:items-center sm:justify-between lg:px-6">
            <div>
              <h2 className="text-sm font-semibold text-slate-900">Role directory</h2>
              <p className="mt-1 text-xs text-slate-400">Complete role directory</p>
            </div>
            <div className="flex items-center gap-2 text-xs text-slate-400">
              <span className="h-2 w-2 rounded-full bg-emerald-500" />
              Live data
            </div>
          </div>

          {!isLoading && rowCount === 0 ? (
            <div className="flex min-h-[420px] flex-col items-center justify-center px-6 text-center">
              <div className="mb-5 flex h-16 w-16 items-center justify-center rounded-2xl border border-slate-200 bg-slate-50 text-slate-400">
                <Tag className="h-7 w-7" />
              </div>
              <h3 className="text-lg font-semibold text-slate-900">No {title.toLowerCase()} yet</h3>
              <p className="mt-2 max-w-md text-sm leading-6 text-slate-500">
                There are currently no records available in the system.
              </p>
              {hasAdd ? (
                <button
                  type="button"
                  onClick={() => setShowAddModal(true)}
                  className="mt-6 inline-flex items-center gap-2 rounded-xl bg-emerald-600 px-5 py-2.5 text-sm font-semibold text-white transition hover:bg-emerald-500"
                >
                  <Plus className="h-4 w-4" />
                  Add first role
                </button>
              ) : null}
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
                    {hasActionsColumn ? (
                      <th className="px-6 py-4 text-right text-[10px] font-semibold uppercase tracking-[0.16em] text-slate-400">
                        Actions
                      </th>
                    ) : null}
                  </tr>
                </thead>
                {isLoading ? (
                  <tbody>
                    {Array.from({ length: 5 }).map((_, i) => (
                      <tr key={i} className="border-b border-slate-100">
                        {columns.map((col) => (
                          <td key={col.field} className="px-6 py-5">
                            <div className="h-4 w-2/3 animate-pulse rounded bg-slate-100" />
                          </td>
                        ))}
                        {hasActionsColumn ? (
                          <td className="px-6 py-5">
                            <div className="ml-auto h-8 w-20 animate-pulse rounded-lg bg-slate-100" />
                          </td>
                        ) : null}
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
                            {col.badge ? (
                              <span
                                className={`inline-flex items-center gap-1.5 rounded-full border px-2.5 py-1 text-[11px] font-semibold ${badgeTone(
                                  row[col.field],
                                )}`}
                              >
                                <span className="h-1.5 w-1.5 rounded-full bg-current" />
                                {row[col.field]}
                              </span>
                            ) : j === 0 ? (
                              <span className="font-medium text-slate-800">{row[col.field] ?? "—"}</span>
                            ) : (
                              <span className="text-sm text-slate-500">{row[col.field] ?? "—"}</span>
                            )}
                          </td>
                        ))}
                        {hasActionsColumn ? (
                          <td className="px-6 py-5">
                            <div className="flex items-center justify-end gap-1.5">
                              {hasUpdate ? (
                                <button
                                  type="button"
                                  onClick={() => setEditingRow(row)}
                                  className="flex h-9 w-9 items-center justify-center rounded-xl border border-slate-200 bg-white text-slate-400 transition hover:border-slate-300 hover:bg-slate-50 hover:text-slate-800"
                                  aria-label="Edit"
                                >
                                  <Pencil className="h-4 w-4" />
                                </button>
                              ) : null}
                              {hasDelete ? (
                                <button
                                  type="button"
                                  onClick={() => setDeletingRow(row)}
                                  className="flex h-9 w-9 items-center justify-center rounded-xl border border-slate-200 bg-white text-slate-400 transition hover:border-red-200 hover:bg-red-50 hover:text-red-600"
                                  aria-label="Delete"
                                >
                                  <Trash2 className="h-4 w-4" />
                                </button>
                              ) : null}
                            </div>
                          </td>
                        ) : null}
                      </tr>
                    ))}
                  </tbody>
                )}
              </table>
            </div>
          )}

          {rowCount > 0 && !isLoading ? (
            <div className="flex flex-col gap-3 border-t border-slate-100 px-5 py-4 text-xs text-slate-400 sm:flex-row sm:items-center sm:justify-between lg:px-6">
              <span>
                Showing <span className="font-medium text-slate-600">{rowCount}</span> records
              </span>
              <span>
                Page <span className="font-medium text-slate-600">1</span> of{" "}
                <span className="font-medium text-slate-600">1</span>
              </span>
            </div>
          ) : null}
        </section>
      </div>

      {showAddModal ? (
        <EntityFormModal
          open={showAddModal}
          mode="add"
          fields={addFields}
          initialValues={{}}
          loading={addState.loading}
          onSubmit={handleAddSubmit}
          onClose={() => setShowAddModal(false)}
        />
      ) : null}

      {editingRow ? (
        <EntityFormModal
          open={Boolean(editingRow)}
          mode="edit"
          fields={editFields}
          initialValues={editingRow}
          loading={updateState.loading}
          onSubmit={handleEditSubmit}
          onClose={() => setEditingRow(null)}
        />
      ) : null}

      <ConfirmDialog
        open={Boolean(deletingRow)}
        loading={deleteState.loading}
        onConfirm={handleDeleteConfirm}
        onClose={() => setDeletingRow(null)}
      />

      {ToastView}
    </div>
  );
}