import { useEffect, useState } from "react";
import {
  Plus,
  Search,
  Pencil,
  Trash2,
  Inbox,
  X,
  CheckCircle2,
  AlertCircle,
  RefreshCw,
  ChevronDown,
} from "lucide-react";
import { statusEntity } from "../entities/StatusEntity";
import useStatusStore from "../stores/StatusStore";
import useStatusTypeStore from "../stores/StatusTypeStore";

const config = statusEntity;
const operations = config.operations || {};

const columns = operations.getAll?.columns || operations.search?.columns || [];
const keyField = operations.update?.by || operations.delete?.by || config.idField || "id";

const hasSearch = Boolean(operations.search);
const hasAdd = Boolean(operations.add);
const hasUpdate = Boolean(operations.update);
const hasDelete = Boolean(operations.delete);
const hasActionsColumn = hasUpdate || hasDelete;

const title = config.title || config.entity;
const description = config.description || "";
const addLabel = config.addLabel || `Add ${title}`;

// Resolve the free-text search filter (first non-numeric, non-relational
// filter) plus every other declared filter, which always render inline.
const allFilters = operations.search?.filters || [];
const mainFilter = allFilters.find((f) => f.type !== "number" && f.type !== "int" && !f.source);
const inlineFilters = allFilters.filter((f) => f !== mainFilter);

const inputClass =
  "h-11 w-full rounded-xl border border-slate-200 bg-white px-4 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-400 focus:ring-4 focus:ring-emerald-500/10";

function useToast() {
  const [toast, setToast] = useState(null);
  const showToast = ({ type, title, description }) => {
    setToast({ type, title, description });
    setTimeout(() => setToast(null), 4000);
  };
  return { toast, showToast, closeToast: () => setToast(null) };
}

function badgeTone(value) {
  const v = String(value ?? "").toLowerCase();
  if (
    ["active", "confirmed", "completed", "paid", "in stock", "ready", "admitted"].some((k) =>
      v.includes(k)
    )
  )
    return "bg-emerald-50 text-emerald-700 border-emerald-200";
  if (
    ["pending", "low stock", "processing", "under observation", "scheduled", "under review"].some(
      (k) => v.includes(k)
    )
  )
    return "bg-amber-50 text-amber-700 border-amber-200";
  if (["cancelled", "overdue", "out of stock", "on leave"].some((k) => v.includes(k)))
    return "bg-red-50 text-red-700 border-red-200";
  return "bg-slate-50 text-slate-600 border-slate-200";
}

function validateFields(fields, values) {
  const errors = {};
  for (const field of fields) {
    const value = values[field.name];

    // Checkboxes are booleans, not text — they're never "empty" the way a
    // blank string field is, so they skip the standard emptiness/format checks.
    if (field.type === "checkbox") {
      if (field.required && value !== true) {
        errors[field.name] = `${field.label} is required.`;
      }
      continue;
    }

    const isEmpty = value === undefined || value === null || String(value).trim() === "";

    if (field.required && isEmpty) {
      errors[field.name] = `${field.label} is required.`;
      continue;
    }
    if (isEmpty) continue;

    if (field.type === "email") {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(value)) {
        errors[field.name] = "Enter a valid email address.";
      }
    }

    if (field.type === "number") {
      if (Number.isNaN(Number(value))) {
        errors[field.name] = "Enter a valid number.";
      }
    }

    if (field.type === "select" && field.options && !field.source) {
      const valid = field.options.some((o) => o.value === value || o === value);
      if (!valid) {
        errors[field.name] = `${field.label} must be one of the listed options.`;
      }
    }
  }
  return errors;
}

// Loads option lists for any field/filter carrying a `source` block, by
// calling the referenced entity's declared operation once on mount.
function useSourceOptions(source) {
  const [options, setOptions] = useState([]);

  useEffect(() => {
    if (!source) return;
    // Only StatusType is referenced by this entity's fields/filters today.
    if (source.entity === "StatusType") {
      const store = useStatusTypeStore.getState();
      store.fetchAll?.().then(() => {
        const rows = useStatusTypeStore.getState().getAllState?.data || [];
        setOptions(
          rows.map((r) => ({
            value: r[source.valueField],
            label: r[source.displayField],
          }))
        );
      });
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [source?.entity, source?.operation, source?.valueField, source?.displayField]);

  return options;
}

function EntityFormModal({ mode, fields, initialValues, onClose, onSubmit, loading }) {
  // Normalize incoming values so checkbox fields always start as real
  // booleans (edit mode may hand us "true"/"false" strings, or nothing at
  // all in add mode), instead of drifting into stringy/undefined states.
  const buildInitialValues = () => {
    const base = { ...(initialValues || {}) };
    for (const field of fields) {
      if (field.type === "checkbox") {
        base[field.name] = base[field.name] === true || base[field.name] === "true";
      }
    }
    return base;
  };

  const [values, setValues] = useState(buildInitialValues);
  const [errors, setErrors] = useState({});

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

  return (
    <div className="fixed inset-0 z-[90] flex items-center justify-center bg-slate-900/40 p-4 backdrop-blur-sm">
      <form
        onSubmit={handleSubmit}
        className="flex max-h-[90vh] w-full max-w-2xl flex-col overflow-hidden rounded-3xl border border-slate-200 bg-white shadow-2xl"
      >
        <div className="flex items-center justify-between border-b border-slate-100 px-6 py-5">
          <div>
            <p className="mb-1 text-[11px] font-semibold uppercase tracking-[0.18em] text-emerald-600">
              {title} management
            </p>
            <h3 className="text-xl font-semibold tracking-tight text-slate-900">
              {mode === "add" ? addLabel : `Edit ${title}`}
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
              <div
                key={field.name}
                className={
                  field.type === "textarea"
                    ? "md:col-span-2"
                    : field.type === "checkbox"
                    ? "flex items-center gap-3 pt-2"
                    : ""
                }
              >
                {field.type === "checkbox" ? (
                  <>
                    <FormField
                      field={field}
                      value={values[field.name]}
                      onChange={(v) => handleChange(field.name, v)}
                    />
                    <label className="text-xs font-semibold text-slate-600">
                      {field.label}
                      {field.required && <span className="ml-1 text-emerald-600">*</span>}
                    </label>
                  </>
                ) : (
                  <>
                    <label className="mb-2 block text-xs font-semibold text-slate-600">
                      {field.label}
                      {field.required && <span className="ml-1 text-emerald-600">*</span>}
                    </label>
                    <FormField
                      field={field}
                      value={values[field.name]}
                      onChange={(v) => handleChange(field.name, v)}
                    />
                  </>
                )}
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

function FormField({ field, value, onChange }) {
  // `select` fields carrying a `source` block load their options live from
  // the referenced entity's store rather than a static list.
  const sourceOptions = useSourceOptions(field.type === "select" ? field.source : null);
  const inputBase =
    "w-full rounded-xl border border-slate-200 bg-white px-4 py-3 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-400 focus:ring-4 focus:ring-emerald-500/10";

  if (field.type === "textarea") {
    return (
      <textarea
        rows={4}
        className={inputBase}
        placeholder={field.placeholder}
        value={value ?? ""}
        onChange={(e) => onChange(e.target.value)}
      />
    );
  }

  if (field.type === "select") {
    return (
      <div className="relative">
        <select
          className={`${inputBase} appearance-none pr-10`}
          value={value ?? ""}
          onChange={(e) => onChange(e.target.value)}
        >
          <option value="">Select...</option>
          {(field.source ? sourceOptions : field.options || []).map((opt) => (
            <option key={opt.value ?? opt} value={opt.value ?? opt}>
              {opt.label ?? opt}
            </option>
          ))}
        </select>
        <ChevronDown className="pointer-events-none absolute right-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
      </div>
    );
  }

  // Checkbox fields render an actual checkbox bound to a real boolean,
  // instead of falling through to the default text <input> below.
  if (field.type === "checkbox") {
    return (
      <input
        type="checkbox"
        className="h-5 w-5 rounded border-slate-300 text-emerald-600 focus:ring-emerald-500"
        checked={Boolean(value)}
        onChange={(e) => onChange(e.target.checked)}
      />
    );
  }

  return (
    <input
      className={inputBase}
      type={
        field.type === "number" ? "number" : field.type === "email" ? "email" : field.type === "date" ? "date" : "text"
      }
      placeholder={field.placeholder}
      value={value ?? ""}
      onChange={(e) => onChange(e.target.value)}
    />
  );
}

function ConfirmDialog({ onCancel, onConfirm, loading }) {
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

function InlineFilter({ filter, value, onChange }) {
  const sourceOptions = useSourceOptions(filter.source);

  if (filter.type === "number" || filter.type === "int") {
    return (
      <div>
        <label className="mb-2 block text-[11px] font-semibold uppercase tracking-wider text-slate-400">
          {filter.label}
        </label>
        <input type="number" className={inputClass} value={value ?? ""} onChange={(e) => onChange(e.target.value)} />
      </div>
    );
  }

  if (filter.source) {
    return (
      <div>
        <label className="mb-2 block text-[11px] font-semibold uppercase tracking-wider text-slate-400">
          {filter.label}
        </label>
        <div className="relative">
          <select
            className={`${inputClass} appearance-none pr-10`}
            value={value ?? ""}
            onChange={(e) => onChange(e.target.value)}
          >
            <option value="">All</option>
            {sourceOptions.map((opt) => (
              <option key={opt.value} value={opt.value}>
                {opt.label}
              </option>
            ))}
          </select>
          <ChevronDown className="pointer-events-none absolute right-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
        </div>
      </div>
    );
  }

  return (
    <div>
      <label className="mb-2 block text-[11px] font-semibold uppercase tracking-wider text-slate-400">
        {filter.label}
      </label>
      <input className={inputClass} value={value ?? ""} onChange={(e) => onChange(e.target.value)} />
    </div>
  );
}

export default function StatusPage() {
  const {
    getAllState,
    fetchAll,
    searchState,
    search,
    addState,
    add,
    updateState,
    update,
    deleteState,
    remove,
  } = useStatusStore();

  const [filters, setFilters] = useState({});
  const [addOpen, setAddOpen] = useState(false);
  const [editRow, setEditRow] = useState(null);
  const [deleteRow, setDeleteRow] = useState(null);

  const { toast, showToast, closeToast } = useToast();

  useEffect(() => {
    fetchAll?.();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const isFilterActive = Object.values(filters).some(
    (v) => v !== undefined && v !== null && String(v).trim() !== ""
  );

  const rows = isFilterActive ? searchState?.data || [] : getAllState?.data || [];
  const loading = isFilterActive ? searchState?.loading : getAllState?.loading;

  const refresh = () => {
    if (hasSearch && isFilterActive) {
      search?.(filters);
    } else {
      fetchAll?.();
    }
  };

  const handleFilterChange = (name, value) => {
    const next = { ...filters, [name]: value };
    setFilters(next);
    search?.(next);
  };

  const clearFilters = () => {
    setFilters({});
    fetchAll?.();
  };

  const handleAdd = async (values) => {
    await add(values);
    const result = useStatusStore.getState().addState;
    if (result.errorCode === 0) {
      showToast({
        type: "success",
        title: "Success",
        description: result.message || `${title} created successfully`,
      });
      setAddOpen(false);
      refresh();
    } else {
      showToast({ type: "error", title: "Error", description: result.message });
    }
  };

  const handleUpdate = async (values) => {
    await update(editRow[keyField], values);
    const result = useStatusStore.getState().updateState;
    if (result.errorCode === 0) {
      showToast({
        type: "success",
        title: "Success",
        description: result.message || `${title} updated successfully`,
      });
      setEditRow(null);
      refresh();
    } else {
      showToast({ type: "error", title: "Error", description: result.message });
    }
  };

  const handleDelete = async () => {
    await remove(deleteRow[keyField]);
    const result = useStatusStore.getState().deleteState;
    if (result.errorCode === 0) {
      showToast({
        type: "success",
        title: "Success",
        description: result.message || `${title} deleted successfully`,
      });
    } else {
      showToast({ type: "error", title: "Error", description: result.message });
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
              {description && (
                <p className="mt-3 max-w-2xl text-sm leading-6 text-slate-500 sm:text-base">
                  {description}
                </p>
              )}
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
                  <Plus className="h-4 w-4" />
                  {addLabel}
                </button>
              )}
            </div>
          </div>
        </section>

        {/* STATS */}
        <section className="mt-5 grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-4">
          <StatCard icon={Inbox} label="Total records" value={rows.length} description="Records currently visible" />
          <StatCard
            icon={Search}
            label="Search"
            value={isFilterActive ? "Active" : "All"}
            description={isFilterActive ? "Search filters are active" : "Showing all records"}
          />
          <StatCard icon={Inbox} label="Results" value={rows.length} description="Current result count" />
          <StatCard
            icon={Plus}
            label="Management"
            value={hasAdd ? "Ready" : "View only"}
            description={hasAdd ? "Create and manage records" : "Read-only access"}
          />
        </section>

        {/* SEARCH / FILTERS */}
        {hasSearch && (mainFilter || inlineFilters.length > 0) && (
          <section className="mt-5 rounded-2xl border border-slate-200 bg-white p-4">
            <div className="mb-3 flex items-center justify-between">
              <div>
                <h2 className="text-sm font-semibold text-slate-900">Search & filters</h2>
                <p className="mt-1 text-xs text-slate-400">Results update automatically while you type.</p>
              </div>
              {isFilterActive && (
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

            <div className="grid grid-cols-1 gap-3 md:grid-cols-2">
              {mainFilter && (
                <div>
                  <label className="mb-2 block text-[11px] font-semibold uppercase tracking-wider text-slate-400">
                    {mainFilter.label}
                  </label>
                  <div className="relative">
                    <Search className="pointer-events-none absolute left-3.5 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                    <input
                      className={`${inputClass} pl-10`}
                      placeholder={mainFilter.label}
                      value={filters[mainFilter.name] ?? ""}
                      onChange={(e) => handleFilterChange(mainFilter.name, e.target.value)}
                    />
                  </div>
                </div>
              )}
              {inlineFilters.map((filter) => (
                <InlineFilter
                  key={filter.name}
                  filter={filter}
                  value={filters[filter.name]}
                  onChange={(v) => handleFilterChange(filter.name, v)}
                />
              ))}
            </div>
          </section>
        )}

        {/* TABLE */}
        <section className="mt-5 overflow-hidden rounded-2xl border border-slate-200 bg-white">
          <div className="flex flex-col gap-3 border-b border-slate-100 px-5 py-4 sm:flex-row sm:items-center sm:justify-between lg:px-6">
            <div>
              <h2 className="text-sm font-semibold text-slate-900">{title} directory</h2>
              <p className="mt-1 text-xs text-slate-400">
                {isFilterActive ? "Showing filtered results" : `Complete ${title.toLowerCase()} list`}
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
                <Inbox className="h-7 w-7" />
              </div>
              <h3 className="text-lg font-semibold text-slate-900">
                {isFilterActive ? "No matching records" : `No ${title.toLowerCase()} yet`}
              </h3>
              <p className="mt-2 max-w-md text-sm leading-6 text-slate-500">
                {isFilterActive
                  ? `Nothing matches your current search. Try changing the filters.`
                  : "No data available."}
              </p>
              {hasAdd && !isFilterActive && (
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
                    {rows.map((row, i) => (
                      <tr
                        key={row[keyField] ?? i}
                        className="group border-b border-slate-100 transition-colors last:border-0 hover:bg-slate-50/70"
                      >
                        {columns.map((col, j) => {
                          const value = row[col.field];
                          return (
                            <td key={col.field} className="px-6 py-5 align-middle">
                              {col.badge ? (
                                <span
                                  className={`inline-flex items-center gap-1.5 rounded-full border px-2.5 py-1 text-[11px] font-semibold ${badgeTone(
                                    value
                                  )}`}
                                >
                                  <span className="h-1.5 w-1.5 rounded-full bg-current" />
                                  {value}
                                </span>
                              ) : j === 0 ? (
                                <div className="flex items-center gap-3">
                                  <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-xl border border-slate-200 bg-slate-50 text-xs font-semibold text-slate-500">
                                    {String(value ?? "?").slice(0, 1).toUpperCase()}
                                  </div>
                                  <span className="font-medium text-slate-800">
                                    {typeof value === "boolean" ? String(value) : value ?? "—"}
                                  </span>
                                </div>
                              ) : (
                                <span className="text-sm text-slate-500">
                                  {typeof value === "boolean" ? String(value) : value ?? "—"}
                                </span>
                              )}
                            </td>
                          );
                        })}
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
          mode="edit"
          fields={operations.update.fields}
          initialValues={editRow}
          onClose={() => setEditRow(null)}
          onSubmit={handleUpdate}
          loading={updateState?.loading}
        />
      )}

      {deleteRow && (
        <ConfirmDialog onCancel={() => setDeleteRow(null)} onConfirm={handleDelete} loading={deleteState?.loading} />
      )}

      {toast && (
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
              <p className="mt-1 text-sm leading-5 text-slate-500">{toast.description}</p>
            </div>
            <button
              type="button"
              onClick={closeToast}
              className="rounded-lg p-1 text-slate-400 transition hover:bg-slate-100 hover:text-slate-700"
            >
              <X className="h-4 w-4" />
            </button>
          </div>
        </div>
      )}
    </div>
  );
}