import { useState, useEffect, useRef } from "react";
import {
  Plus,
  Search,
  Pencil,
  Trash2,
  ClipboardList,
  X,
  CheckCircle2,
  AlertCircle,
  RefreshCw,
  ChevronDown,
} from "lucide-react";
import { medicalHistoryEntity } from "../entities/MedicalHistoryEntity";
import useMedicalHistoryStore from "../stores/MedicalHistoryStore";
import usePatientStore from "../stores/PatientStore";

// ============================================================
// CONFIG
// ============================================================

const DEFAULTS = {
  title: "Medical Historys",
  description: "Manage medical history records.",
  addLabel: "Add Medical History",
};

const title = medicalHistoryEntity.title || DEFAULTS.title;
const description = medicalHistoryEntity.description || DEFAULTS.description;
const addLabel = medicalHistoryEntity.addLabel || DEFAULTS.addLabel;

const operations = medicalHistoryEntity.operations || {};
const columns = operations.getAll?.columns || operations.search?.columns || [];
const filters = operations.search?.filters || [];
const addFields = operations.add?.fields || [];
const editFields = operations.update?.fields || [];

const hasSearch = Boolean(operations.search);
const hasAdd = Boolean(operations.add);
const hasUpdate = Boolean(operations.update);
const hasDelete = Boolean(operations.delete);
const hasActionsColumn = hasUpdate || hasDelete;

const keyField =
  operations.update?.by || operations.delete?.by || medicalHistoryEntity.idField || "id";

// first non-numeric, non-relational filter becomes the free-text search box
const mainFilter = filters.find((f) => !f.source && f.type !== "number" && f.type !== "int");
const inlineFilters = filters.filter((f) => f !== mainFilter);

function badgeTone(value) {
  const v = String(value ?? "").toLowerCase();
  if (
    ["active", "confirmed", "completed", "paid", "in stock", "ready", "admitted"].some((k) =>
      v.includes(k),
    )
  )
    return "bg-emerald-50 text-emerald-700 border-emerald-200";
  if (
    ["pending", "low stock", "processing", "under observation", "scheduled", "under review"].some(
      (k) => v.includes(k),
    )
  )
    return "bg-amber-50 text-amber-700 border-amber-200";
  if (["cancelled", "overdue", "out of stock", "on leave"].some((k) => v.includes(k)))
    return "bg-red-50 text-red-700 border-red-200";
  return "bg-slate-50 text-slate-600 border-slate-200";
}

function useToast() {
  const [toast, setToast] = useState(null);
  const timerRef = useRef(null);

  const showToast = ({ type, title: toastTitle, description: toastDescription }) => {
    if (timerRef.current) clearTimeout(timerRef.current);
    setToast({ type, title: toastTitle, description: toastDescription });
    timerRef.current = setTimeout(() => setToast(null), 4000);
  };

  const ToastView = toast ? (
    <div className="fixed bottom-6 right-6 z-[100] w-[360px] max-w-[calc(100vw-32px)]">
      <div className="flex items-start gap-3 rounded-2xl border border-slate-200 bg-white p-4 shadow-xl shadow-slate-200/60">
        <div
          className={`flex h-9 w-9 shrink-0 items-center justify-center rounded-xl ${
            toast.type === "success" ? "bg-emerald-50 text-emerald-600" : "bg-red-50 text-red-600"
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
            <p className="mt-1 text-sm leading-5 text-slate-500">{toast.description}</p>
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

  return { showToast, ToastView, closeToast: () => setToast(null) };
}

function validateFields(fields, values, dynamicOptions) {
  const errors = {};
  fields.forEach((field) => {
    const value = values[field.name];
    const isEmpty = value === undefined || value === null || String(value).trim() === "";

    if (field.required && isEmpty) {
      errors[field.name] = `${field.label} is required.`;
      return;
    }
    if (isEmpty) return;

    if (field.type === "email" && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
      errors[field.name] = "Enter a valid email address.";
      return;
    }
    if (field.type === "number" && Number.isNaN(Number(value))) {
      errors[field.name] = "Enter a valid number.";
      return;
    }
    if (field.type === "select") {
      if (field.source) {
        const options = dynamicOptions[field.name] || [];
        if (!options.some((opt) => String(opt.value) === String(value))) {
          errors[field.name] = `${field.label} must be one of the listed options.`;
        }
      } else if (field.options && !field.options.includes(value)) {
        errors[field.name] = `${field.label} must be one of the listed options.`;
      }
    }
  });
  return errors;
}

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
          <option value="">Select {field.label}</option>
          {(options || field.options || []).map((opt) =>
            typeof opt === "object" ? (
              <option key={opt.value} value={opt.value}>
                {opt.label}
              </option>
            ) : (
              <option key={opt} value={opt}>
                {opt}
              </option>
            ),
          )}
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
        placeholder={field.placeholder}
        value={value ?? ""}
        onChange={(e) => onChange(e.target.value)}
      />
    );
  }

  return (
    <input
      className={base}
      type={["text", "number", "email", "date"].includes(field.type) ? field.type : "text"}
      placeholder={field.placeholder}
      value={value ?? ""}
      onChange={(e) => onChange(e.target.value)}
    />
  );
}

function EntityFormModal({ open, fields, initialValues, dynamicOptions, submitting, onCancel, onSubmit }) {
  const [values, setValues] = useState(initialValues || {});
  const [errors, setErrors] = useState({});

  useEffect(() => {
    if (open) {
      setValues(initialValues || {});
      setErrors({});
    }
  }, [open, initialValues]);

  if (!open) return null;

  const handleChange = (name, value) => setValues((prev) => ({ ...prev, [name]: value }));

  const handleSubmit = (e) => {
    e.preventDefault();
    const nextErrors = validateFields(fields, values, dynamicOptions);
    setErrors(nextErrors);
    if (Object.keys(nextErrors).length > 0) return;
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
              Medical history management
            </p>
            <h3 className="text-xl font-semibold tracking-tight text-slate-900">
              {initialValues && initialValues[keyField] ? `Edit ${title.replace(/s$/, "")}` : addLabel}
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
              <div key={field.name} className={field.type === "textarea" ? "md:col-span-2" : ""}>
                <label className="mb-2 block text-xs font-semibold text-slate-600">
                  {field.label}
                  {field.required && <span className="ml-1 text-emerald-600">*</span>}
                </label>
                <FieldInput
                  field={field}
                  value={values[field.name]}
                  options={dynamicOptions[field.name]}
                  onChange={(v) => handleChange(field.name, v)}
                />
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

function ConfirmDialog({ open, onCancel, onConfirm }) {
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
            className="rounded-xl bg-red-600 px-5 py-2.5 text-sm font-semibold text-white transition hover:bg-red-500"
          >
            Delete record
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

export default function MedicalHistoryPage() {
  const store = useMedicalHistoryStore();
  const patientStore = usePatientStore();

  const { getAllState, searchState, addState, updateState, deleteState, fetchAll, search, add, update, remove } =
    store;

  const [searchQuery, setSearchQuery] = useState("");
  const [filterValues, setFilterValues] = useState({});
  const [showAdd, setShowAdd] = useState(false);
  const [editRow, setEditRow] = useState(null);
  const [deleteRow, setDeleteRow] = useState(null);
  const [submitting, setSubmitting] = useState(false);
  const [refreshing, setRefreshing] = useState(false);

  const { showToast, ToastView } = useToast();

  const patientOptions = (patientStore.getAllState.data || []).map((r) => ({
    value: r.id,
    label: r.personName,
  }));

  useEffect(() => {
    fetchAll?.();
  }, []);

  useEffect(() => {
    patientStore.fetchAll?.();
  }, []);

  const currentFilters = () => ({
    ...(mainFilter ? { [mainFilter.name]: searchQuery } : {}),
    ...filterValues,
  });

  const isFilterActive = () =>
    Boolean(searchQuery) || Object.values(filterValues).some((v) => v !== undefined && v !== "");

  useEffect(() => {
    if (!hasSearch) return;
    search?.(currentFilters());
  }, [searchQuery, filterValues]);

  const activeState = hasSearch && isFilterActive() ? searchState : getAllState;
  const rows = activeState.data || [];
  const loading = hasSearch && isFilterActive() ? searchState.loading : getAllState.loading;
  const totalRows = rows.length;

  const refresh = async () => {
    setRefreshing(true);
    try {
      if (hasSearch && isFilterActive()) {
        await search?.(currentFilters());
      } else {
        await fetchAll?.();
      }
    } finally {
      window.setTimeout(() => setRefreshing(false), 500);
    }
  };

  const clearFilters = () => {
    setSearchQuery("");
    setFilterValues({});
    fetchAll?.();
  };

  const handleAddSubmit = async (values) => {
    setSubmitting(true);
    await add?.(values);
    setSubmitting(false);
    const result = useMedicalHistoryStore.getState().addState;
    if (result.errorCode === 0) {
      showToast({
        type: "success",
        title: "Record created",
        description: result.message || `${title} created successfully`,
      });
      setShowAdd(false);
      refresh();
    } else {
      showToast({ type: "error", title: "Unable to create record", description: result.message });
    }
  };

  const handleEditSubmit = async (values) => {
    setSubmitting(true);
    await update?.(editRow[keyField], values);
    setSubmitting(false);
    const result = useMedicalHistoryStore.getState().updateState;
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

  const handleDeleteConfirm = async () => {
    await remove?.(deleteRow[keyField]);
    const result = useMedicalHistoryStore.getState().deleteState;
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
                  Medical history management
                </span>
              </div>
              <div className="flex flex-wrap items-center gap-3">
                <h1 className="text-4xl font-semibold tracking-[-0.04em] text-slate-900 sm:text-5xl">
                  {title}
                </h1>
                <span className="inline-flex items-center rounded-full border border-slate-200 bg-slate-50 px-3 py-1.5 text-xs font-medium text-slate-500">
                  {totalRows} records
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
                <RefreshCw className={`h-4 w-4 ${refreshing ? "animate-spin" : ""}`} />
                Refresh
              </button>
              {hasAdd && (
                <button
                  type="button"
                  onClick={() => setShowAdd(true)}
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
          <StatCard icon={ClipboardList} label="Total records" value={totalRows} description="Records currently visible" />
          <StatCard
            icon={Search}
            label="Search"
            value={isFilterActive() ? "Active" : "All"}
            description={isFilterActive() ? "Search filters are active" : "Showing all records"}
          />
          <StatCard icon={ClipboardList} label="Results" value={totalRows} description="Current result count" />
          <StatCard
            icon={Plus}
            label="Management"
            value={hasAdd ? "Ready" : "View only"}
            description={hasAdd ? "Create and manage records" : "Read-only access"}
          />
        </section>

        {/* SEARCH / FILTERS */}
        {hasSearch && (
          <section className="mt-5 rounded-2xl border border-slate-200 bg-white p-4">
            <div className="mb-3 flex items-center justify-between">
              <div>
                <h2 className="text-sm font-semibold text-slate-900">Search & filters</h2>
                <p className="mt-1 text-xs text-slate-400">
                  Results update automatically while you type.
                </p>
              </div>
              {isFilterActive() && (
                <button
                  type="button"
                  onClick={clearFilters}
                  className="inline-flex items-center gap-1.5 rounded-lg px-3 py-2 text-xs font-medium text-slate-500 transition hover:bg-slate-50 hover:text-slate-900"
                >
                  <X className="h-3.5 w-3.5" />
                  Clear
                </button>
              )}
            </div>

            <div className="grid grid-cols-1 gap-3 md:grid-cols-2 lg:grid-cols-3">
              {mainFilter && (
                <div>
                  <label className="mb-2 block text-[11px] font-semibold uppercase tracking-wider text-slate-400">
                    {mainFilter.label}
                  </label>
                  <div className="relative">
                    <Search className="pointer-events-none absolute left-3.5 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                    <input
                      className="h-11 w-full rounded-xl border border-slate-200 bg-white pl-10 pr-4 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-400 focus:ring-4 focus:ring-emerald-500/10"
                      placeholder={`Search by ${mainFilter.label}`}
                      value={searchQuery}
                      onChange={(e) => setSearchQuery(e.target.value)}
                    />
                  </div>
                </div>
              )}
              {inlineFilters.map((f) => (
                <div key={f.name}>
                  <label className="mb-2 block text-[11px] font-semibold uppercase tracking-wider text-slate-400">
                    {f.label}
                  </label>
                  <input
                    className="h-11 w-full rounded-xl border border-slate-200 bg-white px-4 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-400 focus:ring-4 focus:ring-emerald-500/10"
                    type={f.type === "number" || f.type === "int" ? "number" : f.type === "DateTime" ? "date" : "text"}
                    value={filterValues[f.name] ?? ""}
                    onChange={(e) =>
                      setFilterValues((prev) => ({ ...prev, [f.name]: e.target.value }))
                    }
                  />
                </div>
              ))}
            </div>
          </section>
        )}

        {/* TABLE */}
        <section className="mt-5 overflow-hidden rounded-2xl border border-slate-200 bg-white">
          <div className="flex flex-col gap-3 border-b border-slate-100 px-5 py-4 sm:flex-row sm:items-center sm:justify-between lg:px-6">
            <div>
              <h2 className="text-sm font-semibold text-slate-900">Medical history directory</h2>
              <p className="mt-1 text-xs text-slate-400">
                {isFilterActive() ? "Showing filtered results" : "Complete medical history directory"}
              </p>
            </div>
            <div className="flex items-center gap-2 text-xs text-slate-400">
              <span className="h-2 w-2 rounded-full bg-emerald-500" />
              Live data
            </div>
          </div>

          {!loading && rows.length === 0 ? (
            <div className="flex min-h-[420px] flex-col items-center justify-center px-6 text-center">
              <div className="mb-5 flex h-16 w-16 items-center justify-center rounded-2xl border border-slate-200 bg-slate-50 text-slate-400">
                <ClipboardList className="h-7 w-7" />
              </div>
              <h3 className="text-lg font-semibold text-slate-900">
                {isFilterActive() ? "No matching records" : `No ${title.toLowerCase()} yet`}
              </h3>
              <p className="mt-2 max-w-md text-sm leading-6 text-slate-500">
                {isFilterActive()
                  ? `Nothing matches "${searchQuery || Object.values(filterValues).find(Boolean) || ""}". Try changing the filters.`
                  : "There are currently no records available in the system."}
              </p>
              {hasAdd && !isFilterActive() && (
                <button
                  type="button"
                  onClick={() => setShowAdd(true)}
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
                    {rows.map((row, rowIndex) => (
                      <tr
                        key={row[keyField] ?? rowIndex}
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
                              <span className="font-medium text-slate-800">
                                {String(row[col.field] ?? "—")}
                              </span>
                            ) : (
                              <span className="text-sm text-slate-500">
                                {String(row[col.field] ?? "—")}
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

      <EntityFormModal
        open={showAdd}
        fields={addFields}
        initialValues={{}}
        dynamicOptions={{ patientID: patientOptions }}
        submitting={submitting}
        onCancel={() => setShowAdd(false)}
        onSubmit={handleAddSubmit}
      />

      <EntityFormModal
        open={Boolean(editRow)}
        fields={editFields}
        initialValues={editRow || {}}
        dynamicOptions={{ patientID: patientOptions }}
        submitting={submitting}
        onCancel={() => setEditRow(null)}
        onSubmit={handleEditSubmit}
      />

      <ConfirmDialog open={Boolean(deleteRow)} onCancel={() => setDeleteRow(null)} onConfirm={handleDeleteConfirm} />

      {ToastView}
    </div>
  );
}