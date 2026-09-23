import { useState, useEffect, useCallback, useRef } from "react";
import {
  Plus,
  Search,
  SlidersHorizontal,
  Pencil,
  Trash2,
  Inbox,
  X,
  Check,
  CheckCircle2,
  AlertCircle,
  Stethoscope,
  Building2,
  GraduationCap,
  WalletCards,
  RotateCcw,
  ChevronDown,
  RefreshCw,
} from "lucide-react";

import { doctorEntity } from "../entities/DoctorEntity";
import useDoctorStore from "../stores/DoctorStore";
import { createEntityApi } from "../api/createEntityApi";

// ============================================================================
// CONFIG
// ============================================================================

const config = doctorEntity;

const title = config.title || config.entity;
const description = config.description || "Manage doctors and clinical staff.";
const addLabel = config.addLabel || "Add Doctor";

const ops = config.operations || {};

const columns = ops.getAll?.columns || ops.search?.columns || [];
const filters = ops.search?.filters || [];

const addFields = ops.add?.fields || [];
const updateFields = ops.update?.fields || [];

const keyField =
  ops.update?.by || ops.delete?.by || config.idField || "id";

const hasSearch = Boolean(ops.search);
const hasAdd = Boolean(ops.add);
const hasUpdate = Boolean(ops.update);
const hasDelete = Boolean(ops.delete);

const hasActionsColumn = hasUpdate || hasDelete;

const textFilter = filters.find((f) => f.type !== "number");
const popoverFilters = filters.filter((f) => f !== textFilter);

// ============================================================================
// SOURCE OPTIONS
// ============================================================================

function useSourceOptions(source) {
  const [options, setOptions] = useState([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (!source) return;

    let cancelled = false;

    setLoading(true);

    const api = createEntityApi({
      entity: source.entity,
      operations: {
        [source.operation || "getAll"]: {
          endpoint: "all",
        },
      },
    });

    const fn =
      source.operation === "getAll" || !source.operation
        ? api.getAll
        : api[source.operation];

    (fn ? fn() : Promise.resolve({ data: [] }))
      .then((result) => {
        if (cancelled) return;

        const rows = Array.isArray(result?.data) ? result.data : [];

        setOptions(
          rows.map((row) => ({
            value: row[source.valueField],
            label: row[source.displayField],
          }))
        );
      })
      .catch(() => {
        if (!cancelled) {
          setOptions([]);
        }
      })
      .finally(() => {
        if (!cancelled) {
          setLoading(false);
        }
      });

    return () => {
      cancelled = true;
    };
  }, [
    source?.entity,
    source?.operation,
    source?.valueField,
    source?.displayField,
  ]);

  return { options, loading };
}

// ============================================================================
// HELPERS
// ============================================================================

function badgeTone(value) {
  const v = String(value ?? "").toLowerCase();

  if (v.includes("active") || v.includes("senior") || v.includes("consultant")) {
    return "bg-emerald-50 text-emerald-700";
  }

  if (v.includes("specialist") || v.includes("resident") || v.includes("under")) {
    return "bg-cyan-50 text-cyan-700";
  }

  if (v.includes("inactive") || v.includes("junior")) {
    return "bg-slate-100 text-slate-600";
  }

  return "bg-slate-100 text-slate-600";
}

function formatSalary(value) {
  if (value === null || value === undefined || value === "") {
    return "—";
  }

  const number = Number(value);

  if (Number.isNaN(number)) {
    return value;
  }

  return new Intl.NumberFormat("en-US", {
    minimumFractionDigits: 0,
    maximumFractionDigits: 2,
  }).format(number);
}

function getColumnValue(row, field) {
  return row?.[field];
}

function getInitials(name) {
  if (!name) return "DR";

  return String(name)
    .trim()
    .split(/\s+/)
    .slice(0, 2)
    .map((word) => word[0])
    .join("")
    .toUpperCase();
}

// ============================================================================
// VALIDATION
// ============================================================================

function validateField(field, value) {
  if (
    field.required &&
    (value === undefined || value === null || String(value).trim() === "")
  ) {
    return `${field.label} is required.`;
  }

  if (value === undefined || value === null || String(value).trim() === "") {
    return "";
  }

  if (field.type === "email" && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
    return "Enter a valid email address.";
  }

  if (field.type === "number" && Number.isNaN(Number(value))) {
    return "Enter a valid number.";
  }

  if (field.type === "select" && field.options && !field.source) {
    const valid = field.options.some((o) => String(o.value) === String(value));

    if (!valid) {
      return `${field.label} must be one of the listed options.`;
    }
  }

  return "";
}

// ============================================================================
// TOAST
// ============================================================================

function useToast() {
  const [toast, setToast] = useState(null);
  const timerRef = useRef(null);

  const showToast = useCallback(
    ({ type, title: toastTitle, description: toastDescription }) => {
      if (timerRef.current) {
        clearTimeout(timerRef.current);
      }

      setToast({
        type,
        title: toastTitle,
        description: toastDescription,
      });

      timerRef.current = setTimeout(() => setToast(null), 4000);
    },
    []
  );

  const hideToast = useCallback(() => {
    if (timerRef.current) {
      clearTimeout(timerRef.current);
    }

    setToast(null);
  }, []);

  return { toast, showToast, hideToast };
}

function Toast({ toast, onClose }) {
  if (!toast) return null;

  const success = toast.type === "success";

  return (
    <div className="fixed bottom-6 right-6 z-[200] w-[360px] max-w-[calc(100vw-32px)]">
      <div className="flex items-start gap-3 rounded-2xl border border-slate-200 bg-white p-4 shadow-xl shadow-slate-200/60">
        <div
          className={`flex h-9 w-9 shrink-0 items-center justify-center rounded-xl ${
            success
              ? "bg-emerald-50 text-emerald-600"
              : "bg-red-50 text-red-600"
          }`}
        >
          {success ? <CheckCircle2 size={18} /> : <AlertCircle size={18} />}
        </div>

        <div className="min-w-0 flex-1">
          <p className="text-sm font-semibold text-slate-900">{toast.title}</p>

          <p className="mt-1 text-sm leading-5 text-slate-500">
            {toast.description}
          </p>
        </div>

        <button
          onClick={onClose}
          className="rounded-lg p-1 text-slate-400 transition hover:bg-slate-100 hover:text-slate-700"
        >
          <X size={15} />
        </button>
      </div>
    </div>
  );
}

// ============================================================================
// SOURCE SELECT
// ============================================================================

function SourceSelect({ field, value, onChange }) {
  const { options, loading } = useSourceOptions(field.source);

  return (
    <div className="relative">
      <select
        value={value ?? ""}
        onChange={(e) => onChange(field.name, e.target.value)}
        className="h-11 w-full appearance-none rounded-xl border border-slate-200 bg-slate-50 px-3.5 pr-10 text-sm font-medium text-slate-700 outline-none transition focus:border-emerald-400 focus:bg-white focus:ring-4 focus:ring-emerald-500/10"
      >
        <option value="">{loading ? "Loading..." : field.label}</option>

        {options.map((option) => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>

      <ChevronDown
        size={16}
        className="pointer-events-none absolute right-3 top-1/2 -translate-y-1/2 text-slate-400"
      />
    </div>
  );
}

// ============================================================================
// FORM FIELD
// ============================================================================

function FormField({ field, value, error, onChange }) {
  const commonProps = {
    value: value ?? "",
    onChange: (e) => onChange(field.name, e.target.value),
    placeholder: field.placeholder,
    className: `
      h-11 w-full rounded-xl
      border
      ${error ? "border-red-300 bg-red-50/30" : "border-slate-200 bg-slate-50"}
      px-3.5
      text-sm font-medium
      text-slate-800
      outline-none
      transition
      placeholder:text-slate-400
      focus:border-emerald-400
      focus:bg-white
      focus:ring-4
      focus:ring-emerald-500/10
    `,
  };

  return (
    <div>
      <label className="mb-1.5 block text-xs font-bold text-slate-600">
        {field.label}

        {field.required && (
          <span className="ml-1 text-red-500">*</span>
        )}
      </label>

      {field.type === "select" && field.source ? (
        <SourceSelect field={field} value={value} onChange={onChange} />
      ) : field.type === "select" ? (
        <div className="relative">
          <select {...commonProps}>
            <option value="">{field.label}</option>

            {(field.options || []).map((option) => (
              <option key={option.value} value={option.value}>
                {option.label}
              </option>
            ))}
          </select>

          <ChevronDown
            size={16}
            className="pointer-events-none absolute right-3 top-1/2 -translate-y-1/2 text-slate-400"
          />
        </div>
      ) : field.type === "textarea" ? (
        <textarea
          rows={3}
          {...commonProps}
          className={`
            w-full resize-none rounded-xl
            border
            ${error ? "border-red-300 bg-red-50/30" : "border-slate-200 bg-slate-50"}
            px-3.5 py-3
            text-sm font-medium
            text-slate-800
            outline-none transition
            placeholder:text-slate-400
            focus:border-emerald-400
            focus:bg-white
            focus:ring-4
            focus:ring-emerald-500/10
          `}
        />
      ) : (
        <input
          type={
            field.type === "number"
              ? "number"
              : field.type === "date"
              ? "date"
              : field.type === "email"
              ? "email"
              : "text"
          }
          {...commonProps}
        />
      )}

      {error && (
        <p className="mt-1 text-[11px] font-medium text-red-600">{error}</p>
      )}
    </div>
  );
}

// ============================================================================
// FORM MODAL
// ============================================================================

function EntityFormModal({
  mode,
  fields,
  initialValues,
  submitting,
  onSubmit,
  onClose,
}) {
  const [values, setValues] = useState(initialValues || {});
  const [errors, setErrors] = useState({});

  const editing = mode === "edit";

  const handleChange = (name, value) => {
    setValues((prev) => ({ ...prev, [name]: value }));
    setErrors((prev) => ({ ...prev, [name]: "" }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    const nextErrors = {};

    fields.forEach((field) => {
      const error = validateField(field, values[field.name]);

      if (error) {
        nextErrors[field.name] = error;
      }
    });

    setErrors(nextErrors);

    if (Object.values(nextErrors).some(Boolean)) {
      return;
    }

    onSubmit(values);
  };

  return (
    <div
      className="fixed inset-0 z-[150] flex items-center justify-center bg-slate-900/40 p-4 backdrop-blur-sm"
      onMouseDown={onClose}
    >
      <div
        className="flex max-h-[90vh] w-full max-w-2xl flex-col overflow-hidden rounded-3xl border border-slate-200 bg-white shadow-2xl"
        onMouseDown={(e) => e.stopPropagation()}
      >
        {/* Modal Header */}

        <div className="flex items-center justify-between border-b border-slate-100 px-6 py-5">
          <div className="flex items-center gap-3">
            <div className="flex h-10 w-10 items-center justify-center rounded-xl bg-emerald-50 text-emerald-600">
              <Stethoscope size={19} />
            </div>

            <div>
              <h2 className="text-base font-bold text-slate-900">
                {editing ? "Edit Doctor" : "Add Doctor"}
              </h2>

              <p className="mt-0.5 text-xs text-slate-400">
                {editing
                  ? "Update doctor information."
                  : "Create a new doctor record."}
              </p>
            </div>
          </div>

          <button
            onClick={onClose}
            className="flex h-9 w-9 items-center justify-center rounded-xl text-slate-400 transition hover:bg-slate-100 hover:text-slate-700"
          >
            <X size={18} />
          </button>
        </div>

        {/* Form */}

        <form onSubmit={handleSubmit} className="overflow-y-auto p-6">
          <div className="grid gap-5 sm:grid-cols-2">
            {fields.map((field) => (
              <div
                key={field.name}
                className={field.type === "textarea" ? "sm:col-span-2" : ""}
              >
                <FormField
                  field={field}
                  value={values[field.name]}
                  error={errors[field.name]}
                  onChange={handleChange}
                />
              </div>
            ))}
          </div>

          {/* Footer */}

          <div className="mt-7 flex justify-end gap-2 border-t border-slate-100 pt-5">
            <button
              type="button"
              onClick={onClose}
              className="rounded-xl px-4 py-2.5 text-sm font-bold text-slate-500 transition hover:bg-slate-100 hover:text-slate-700"
            >
              Cancel
            </button>

            <button
              type="submit"
              disabled={submitting}
              className="inline-flex items-center gap-2 rounded-xl bg-emerald-600 px-5 py-2.5 text-sm font-bold text-white transition hover:bg-emerald-500 disabled:cursor-not-allowed disabled:opacity-60"
            >
              <Check size={16} />

              {submitting ? "Saving..." : editing ? "Save Changes" : "Create Doctor"}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}

// ============================================================================
// DELETE MODAL
// ============================================================================

function ConfirmDialog({ onConfirm, onClose, loading, doctor }) {
  return (
    <div className="fixed inset-0 z-[150] flex items-center justify-center bg-slate-900/40 p-4 backdrop-blur-sm">
      <div className="w-full max-w-sm rounded-3xl border border-slate-200 bg-white p-6 shadow-2xl">
        <div className="flex h-11 w-11 items-center justify-center rounded-xl bg-red-50 text-red-600">
          <Trash2 size={19} />
        </div>

        <h2 className="mt-5 text-lg font-bold text-slate-900">
          Delete doctor?
        </h2>

        <p className="mt-2 text-sm leading-6 text-slate-500">
          Are you sure you want to delete{" "}
          <span className="font-bold text-slate-700">
            {doctor?.personName || "this doctor"}
          </span>
          ? This action cannot be undone.
        </p>

        <div className="mt-6 flex justify-end gap-2">
          <button
            onClick={onClose}
            className="rounded-xl px-4 py-2.5 text-sm font-bold text-slate-500 hover:bg-slate-100"
          >
            Cancel
          </button>

          <button
            onClick={onConfirm}
            disabled={loading}
            className="rounded-xl bg-red-600 px-5 py-2.5 text-sm font-bold text-white transition hover:bg-red-500 disabled:opacity-60"
          >
            {loading ? "Deleting..." : "Delete"}
          </button>
        </div>
      </div>
    </div>
  );
}

// ============================================================================
// FILTER PANEL
// ============================================================================

function FiltersPanel({
  open,
  values,
  onChange,
  onApply,
  onClear,
  onClose,
}) {
  if (!open) return null;

  return (
    <div className="mt-3 rounded-2xl border border-slate-200 bg-slate-50/70 p-4">
      <div className="mb-4 flex items-center justify-between">
        <div>
          <p className="text-sm font-bold text-slate-800">
            Filter doctors
          </p>

          <p className="mt-0.5 text-xs text-slate-400">
            Narrow the results using department or specialization.
          </p>
        </div>

        <button
          onClick={onClose}
          className="rounded-lg p-1.5 text-slate-400 hover:bg-white hover:text-slate-700"
        >
          <X size={16} />
        </button>
      </div>

      <div className="grid gap-3 md:grid-cols-2">
        {popoverFilters.map((field) =>
          field.source ? (
            <div key={field.name}>
              <label className="mb-1.5 block text-xs font-bold text-slate-500">
                {field.label}
              </label>

              <SourceSelect
                field={field}
                value={values[field.name]}
                onChange={onChange}
              />
            </div>
          ) : (
            <div key={field.name}>
              <label className="mb-1.5 block text-xs font-bold text-slate-500">
                {field.label}
              </label>

              <input
                type={field.type === "number" ? "number" : "text"}
                value={values[field.name] ?? ""}
                onChange={(e) => onChange(field.name, e.target.value)}
                className="h-11 w-full rounded-xl border border-slate-200 bg-white px-3.5 text-sm font-medium text-slate-700 outline-none focus:border-emerald-400 focus:ring-4 focus:ring-emerald-500/10"
              />
            </div>
          )
        )}
      </div>

      <div className="mt-4 flex justify-end gap-2">
        <button
          onClick={onClear}
          className="inline-flex items-center gap-1.5 rounded-xl px-4 py-2 text-xs font-bold text-slate-500 hover:bg-white hover:text-slate-700"
        >
          <RotateCcw size={13} />
          Clear
        </button>

        <button
          onClick={onApply}
          className="rounded-xl bg-emerald-600 px-4 py-2 text-xs font-bold text-white hover:bg-emerald-500"
        >
          Apply Filters
        </button>
      </div>
    </div>
  );
}

// ============================================================================
// STAT CARD
// ============================================================================

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

// ============================================================================
// DOCTOR PAGE
// ============================================================================

export default function DoctorPage() {
  const {
    getAllState,
    searchState,
    addState,
    updateState,
    deleteState,
    fetchAll,
    search,
    add,
    update,
    remove,
  } = useDoctorStore();

  const { toast, showToast, hideToast } = useToast();

  const [searchQuery, setSearchQuery] = useState("");
  const [popoverValues, setPopoverValues] = useState({});
  const [appliedFilters, setAppliedFilters] = useState({});
  const [isSearchActive, setIsSearchActive] = useState(false);
  const [filtersOpen, setFiltersOpen] = useState(false);
  const [refreshing, setRefreshing] = useState(false);

  const [addOpen, setAddOpen] = useState(false);
  const [editRow, setEditRow] = useState(null);
  const [deleteRow, setDeleteRow] = useState(null);

  // --------------------------------------------------------------------------
  // Initial load
  // --------------------------------------------------------------------------

  useEffect(() => {
    fetchAll?.();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  // --------------------------------------------------------------------------
  // Search
  // --------------------------------------------------------------------------

  const runSearch = useCallback(
    (nextFilters) => {
      if (!search) return;

      setIsSearchActive(true);
      search(nextFilters);
    },
    [search]
  );

  const refresh = useCallback(async () => {
    setRefreshing(true);

    try {
      if (isSearchActive && hasSearch) {
        const combined = { ...appliedFilters };

        if (textFilter) {
          combined[textFilter.name] = searchQuery;
        }

        await search?.(combined);
      } else {
        await fetchAll?.();
      }
    } finally {
      window.setTimeout(() => setRefreshing(false), 500);
    }
  }, [isSearchActive, appliedFilters, searchQuery, search, fetchAll]);

  const handleSearchInput = (value) => {
    setSearchQuery(value);

    if (!textFilter) return;

    runSearch({ ...appliedFilters, [textFilter.name]: value });
  };

  const handleApplyFilters = () => {
    setAppliedFilters(popoverValues);

    const combined = { ...popoverValues };

    if (textFilter) {
      combined[textFilter.name] = searchQuery;
    }

    runSearch(combined);
    setFiltersOpen(false);
  };

  const handleClearFilters = () => {
    setPopoverValues({});
    setAppliedFilters({});

    const combined = {};

    if (textFilter) {
      combined[textFilter.name] = searchQuery;
    }

    runSearch(combined);
  };

  // --------------------------------------------------------------------------
  // Data
  // --------------------------------------------------------------------------

  const activeState = isSearchActive ? searchState : getAllState;
  const rows = activeState?.data || [];
  const loading = activeState?.loading;

  // --------------------------------------------------------------------------
  // Add
  // --------------------------------------------------------------------------

  const handleAddSubmit = async (values) => {
    await add?.(values);

    const result = useDoctorStore.getState().addState;

    if (result.errorCode === 0) {
      showToast({
        type: "success",
        title: "Doctor created",
        description: result.message || "Doctor created successfully.",
      });

      setAddOpen(false);
      refresh();
    } else {
      showToast({
        type: "error",
        title: "Unable to create doctor",
        description: result.message || "Something went wrong.",
      });
    }
  };

  // --------------------------------------------------------------------------
  // Update
  // --------------------------------------------------------------------------

  const handleEditSubmit = async (values) => {
    await update?.(editRow[keyField], values);

    const result = useDoctorStore.getState().updateState;

    if (result.errorCode === 0) {
      showToast({
        type: "success",
        title: "Doctor updated",
        description: result.message || "Doctor updated successfully.",
      });

      setEditRow(null);
      refresh();
    } else {
      showToast({
        type: "error",
        title: "Unable to update doctor",
        description: result.message || "Something went wrong.",
      });
    }
  };

  // --------------------------------------------------------------------------
  // Delete
  // --------------------------------------------------------------------------

  const handleDeleteConfirm = async () => {
    await remove?.(deleteRow[keyField]);

    const result = useDoctorStore.getState().deleteState;

    if (result.errorCode === 0) {
      showToast({
        type: "success",
        title: "Doctor deleted",
        description: result.message || "Doctor deleted successfully.",
      });
    } else {
      showToast({
        type: "error",
        title: "Unable to delete doctor",
        description: result.message || "Something went wrong.",
      });
    }

    setDeleteRow(null);
    refresh();
  };

  // --------------------------------------------------------------------------
  // Stats
  // --------------------------------------------------------------------------

  const departmentCount = new Set(
    rows.map((row) => row.departmentName).filter(Boolean)
  ).size;

  const specializationCount = new Set(
    rows.map((row) => row.specializationName).filter(Boolean)
  ).size;

  // --------------------------------------------------------------------------
  // Render
  // --------------------------------------------------------------------------

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
                  Clinical
                </span>
              </div>

              <div className="flex flex-wrap items-center gap-3">
                <h1 className="text-4xl font-semibold tracking-[-0.04em] text-slate-900 sm:text-5xl">
                  Doctors
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
                <RefreshCw className={`h-4 w-4 ${refreshing ? "animate-spin" : ""}`} />
                Refresh
              </button>

              {hasAdd && (
                <button
                  onClick={() => setAddOpen(true)}
                  className="inline-flex items-center gap-2 rounded-xl bg-emerald-600 px-5 py-3 text-sm font-semibold text-white shadow-lg shadow-emerald-200 transition hover:bg-emerald-500"
                >
                  <Plus size={17} />
                  {addLabel}
                </button>
              )}
            </div>
          </div>
        </section>

        {/* STATS */}

        <section className="mt-5 grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-4">
          <StatCard
            icon={Stethoscope}
            label="Doctors"
            value={rows.length}
            description="Records currently visible"
          />

          <StatCard
            icon={Building2}
            label="Departments"
            value={departmentCount}
            description="Distinct departments covered"
          />

          <StatCard
            icon={GraduationCap}
            label="Specializations"
            value={specializationCount}
            description="Distinct specializations"
          />

          <StatCard
            icon={WalletCards}
            label="Salary records"
            value={rows.length}
            description="Doctors with salary data"
          />
        </section>

        {/* MAIN TABLE CARD */}

        <section className="mt-5 overflow-hidden rounded-2xl border border-slate-200 bg-white">

          {/* Toolbar */}

          <div className="border-b border-slate-100 p-4 sm:p-5">
            <div className="flex flex-col gap-3 xl:flex-row xl:items-center xl:justify-between">
              <div>
                <h2 className="text-sm font-semibold text-slate-900">
                  Doctor directory
                </h2>

                <p className="mt-1 text-xs text-slate-400">
                  {isSearchActive ? "Showing filtered results" : "Browse and manage doctors"}
                </p>
              </div>

              <div className="flex flex-col gap-2 sm:flex-row">
                {hasSearch && textFilter && (
                  <div className="relative min-w-0 sm:w-80">
                    <Search
                      size={16}
                      className="pointer-events-none absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400"
                    />

                    <input
                      type="text"
                      value={searchQuery}
                      onChange={(e) => handleSearchInput(e.target.value)}
                      placeholder="Search doctors..."
                      className="h-11 w-full rounded-xl border border-slate-200 bg-slate-50 pl-10 pr-3 text-sm font-medium text-slate-700 outline-none transition focus:border-emerald-400 focus:bg-white focus:ring-4 focus:ring-emerald-500/10"
                    />
                  </div>
                )}

                {hasSearch && popoverFilters.length > 0 && (
                  <button
                    onClick={() => setFiltersOpen((value) => !value)}
                    className={`inline-flex h-11 items-center justify-center gap-2 rounded-xl border px-4 text-sm font-bold transition ${
                      filtersOpen
                        ? "border-emerald-200 bg-emerald-50 text-emerald-700"
                        : "border-slate-200 bg-white text-slate-600 hover:bg-slate-50"
                    }`}
                  >
                    <SlidersHorizontal size={16} />
                    Filters

                    {Object.values(appliedFilters).filter(Boolean).length > 0 && (
                      <span className="flex h-5 min-w-5 items-center justify-center rounded-full bg-emerald-600 px-1 text-[10px] text-white">
                        {Object.values(appliedFilters).filter(Boolean).length}
                      </span>
                    )}
                  </button>
                )}
              </div>
            </div>

            <FiltersPanel
              open={filtersOpen}
              values={popoverValues}
              onChange={(name, value) =>
                setPopoverValues((prev) => ({ ...prev, [name]: value }))
              }
              onApply={handleApplyFilters}
              onClear={handleClearFilters}
              onClose={() => setFiltersOpen(false)}
            />
          </div>

          {/* Table */}

          <div className="overflow-x-auto">
            <table className="w-full min-w-[1050px] table-auto">
              <thead>
                <tr className="border-b border-slate-100 bg-slate-50/70">
                  {columns.map((column) => (
                    <th
                      key={column.field}
                      className="whitespace-nowrap px-6 py-4 text-left text-[10px] font-semibold uppercase tracking-[0.16em] text-slate-400"
                    >
                      {column.header}
                    </th>
                  ))}

                  {hasActionsColumn && (
                    <th className="px-6 py-4 text-right text-[10px] font-semibold uppercase tracking-[0.16em] text-slate-400">
                      Actions
                    </th>
                  )}
                </tr>
              </thead>

              <tbody className="divide-y divide-slate-100">

                {/* Loading */}

                {loading ? (
                  Array.from({ length: 6 }).map((_, index) => (
                    <tr key={index}>
                      {columns.map((column) => (
                        <td key={column.field} className="px-6 py-5">
                          <div className="h-4 w-3/4 animate-pulse rounded bg-slate-100" />
                        </td>
                      ))}

                      {hasActionsColumn && (
                        <td className="px-6 py-5">
                          <div className="ml-auto h-8 w-20 animate-pulse rounded-lg bg-slate-100" />
                        </td>
                      )}
                    </tr>
                  ))
                ) : rows.length === 0 ? (

                  /* Empty */

                  <tr>
                    <td
                      colSpan={columns.length + (hasActionsColumn ? 1 : 0)}
                      className="px-6 py-20"
                    >
                      <div className="flex flex-col items-center justify-center text-center">
                        <div className="flex h-16 w-16 items-center justify-center rounded-2xl border border-slate-200 bg-slate-50 text-slate-400">
                          <Inbox size={26} />
                        </div>

                        <p className="mt-5 text-lg font-semibold text-slate-900">
                          {isSearchActive ? "No doctors found" : "No doctors yet"}
                        </p>

                        <p className="mt-2 max-w-md text-sm leading-6 text-slate-500">
                          {isSearchActive
                            ? "Try changing your search or filters."
                            : "There are currently no doctor records available."}
                        </p>
                      </div>
                    </td>
                  </tr>

                ) : (

                  /* Rows */

                  rows.map((row) => (
                    <tr
                      key={row[keyField]}
                      className="group transition-colors hover:bg-slate-50/70"
                    >
                      {columns.map((column, index) => {
                        const value = getColumnValue(row, column.field);

                        // Person name
                        if (column.field === "personName") {
                          return (
                            <td key={column.field} className="px-6 py-5">
                              <div className="flex items-center gap-3">
                                <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-xl border border-emerald-100 bg-emerald-50 text-xs font-extrabold text-emerald-700">
                                  {getInitials(value)}
                                </div>

                                <div className="min-w-0">
                                  <p className="truncate text-sm font-semibold text-slate-800">
                                    {value || "—"}
                                  </p>

                                  <p className="mt-0.5 text-[10px] font-medium text-slate-400">
                                    Doctor
                                  </p>
                                </div>
                              </div>
                            </td>
                          );
                        }

                        // ID
                        if (column.field === "id") {
                          return (
                            <td key={column.field} className="px-6 py-5">
                              <span className="font-mono text-xs font-semibold text-slate-400">
                                #{value}
                              </span>
                            </td>
                          );
                        }

                        // People ID
                        if (column.field === "peopleID") {
                          return (
                            <td key={column.field} className="px-6 py-5">
                              <span className="text-xs font-semibold text-slate-500">
                                {value ?? "—"}
                              </span>
                            </td>
                          );
                        }

                        // Salary
                        if (column.field === "salary") {
                          return (
                            <td key={column.field} className="px-6 py-5">
                              <span className="text-sm font-bold text-slate-700">
                                ${formatSalary(value)}
                              </span>
                            </td>
                          );
                        }

                        // Department
                        if (column.field === "departmentName") {
                          return (
                            <td key={column.field} className="px-6 py-5">
                              <div className="flex items-center gap-2">
                                <Building2 size={14} className="text-slate-400" />

                                <span className="text-sm font-medium text-slate-600">
                                  {value || "—"}
                                </span>
                              </div>
                            </td>
                          );
                        }

                        // Specialization
                        if (column.field === "specializationName") {
                          return (
                            <td key={column.field} className="px-6 py-5">
                              <span className="inline-flex rounded-lg bg-violet-50 px-2.5 py-1 text-xs font-bold text-violet-700">
                                {value || "—"}
                              </span>
                            </td>
                          );
                        }

                        // Qualification / Level
                        if (
                          column.field === "qualification" ||
                          column.field === "doctorLevel"
                        ) {
                          return (
                            <td key={column.field} className="px-6 py-5">
                              <span
                                className={`inline-flex rounded-lg px-2.5 py-1 text-xs font-bold ${badgeTone(
                                  value
                                )}`}
                              >
                                {value || "—"}
                              </span>
                            </td>
                          );
                        }

                        // Generic
                        return (
                          <td
                            key={column.field}
                            className={`px-6 py-5 text-sm ${
                              index === 0
                                ? "font-semibold text-slate-800"
                                : "font-medium text-slate-500"
                            }`}
                          >
                            {String(value ?? "—")}
                          </td>
                        );
                      })}

                      {/* Actions */}

                      {hasActionsColumn && (
                        <td className="px-6 py-5">
                          <div className="flex justify-end gap-1.5 opacity-60 transition-opacity group-hover:opacity-100">
                            {hasUpdate && (
                              <button
                                onClick={() => setEditRow(row)}
                                title="Edit doctor"
                                className="flex h-9 w-9 items-center justify-center rounded-xl border border-slate-200 bg-white text-slate-400 transition hover:border-emerald-200 hover:bg-emerald-50 hover:text-emerald-600"
                              >
                                <Pencil size={15} />
                              </button>
                            )}

                            {hasDelete && (
                              <button
                                onClick={() => setDeleteRow(row)}
                                title="Delete doctor"
                                className="flex h-9 w-9 items-center justify-center rounded-xl border border-slate-200 bg-white text-slate-400 transition hover:border-red-200 hover:bg-red-50 hover:text-red-600"
                              >
                                <Trash2 size={15} />
                              </button>
                            )}
                          </div>
                        </td>
                      )}
                    </tr>
                  ))
                )}
              </tbody>
            </table>
          </div>

          {/* Bottom */}

          <div className="flex flex-col gap-2 border-t border-slate-100 bg-slate-50/40 px-5 py-3.5 sm:flex-row sm:items-center sm:justify-between">
            <p className="text-xs font-medium text-slate-400">
              Showing{" "}
              <span className="font-bold text-slate-700">{rows.length}</span>{" "}
              doctor{rows.length === 1 ? "" : "s"}
            </p>

            <div className="flex items-center gap-2">
              <span className="h-1.5 w-1.5 rounded-full bg-emerald-500" />

              <span className="text-[11px] font-semibold text-slate-400">
                Data synchronized
              </span>
            </div>
          </div>
        </section>
      </div>

      {/* MODALS */}

      {addOpen && (
        <EntityFormModal
          mode="add"
          fields={addFields}
          initialValues={{}}
          submitting={addState?.loading}
          onSubmit={handleAddSubmit}
          onClose={() => setAddOpen(false)}
        />
      )}

      {editRow && (
        <EntityFormModal
          mode="edit"
          fields={updateFields}
          initialValues={editRow}
          submitting={updateState?.loading}
          onSubmit={handleEditSubmit}
          onClose={() => setEditRow(null)}
        />
      )}

      {deleteRow && (
        <ConfirmDialog
          doctor={deleteRow}
          loading={deleteState?.loading}
          onConfirm={handleDeleteConfirm}
          onClose={() => setDeleteRow(null)}
        />
      )}

      <Toast toast={toast} onClose={hideToast} />
    </div>
  );
}