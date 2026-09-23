import { useEffect, useMemo, useState } from "react";
import {
  Plus,
  Search,
  Pencil,
  Trash2,
  Users,
  X,
  CheckCircle2,
  AlertCircle,
  RefreshCw,
  ChevronDown,
} from "lucide-react";

import { employeeEntity } from "../entities/EmployeeEntity";
import useEmployeeStore from "../stores/EmployeeStore";
import { createEntityStore } from "../stores/createEntityStore";

// Referenced entities used by this page's relational (`source`) fields/filters.
import { PeopleEntity } from "../entities/PeopleEntity";
import { roleEntity } from "../entities/RoleEntity";
import { DepartmentEntity } from "../entities/DepartmentEntity";
import { statusEntity } from "../entities/StatusEntity";

// ============================================================
// CONFIG
// ============================================================

const config = employeeEntity;
const operations = config.operations || {};

const DEFAULTS = {
  title: "Employees",
  description: "Manage employee records and information.",
  addLabel: "Add Employee",
};

const columns = operations.getAll?.columns || operations.search?.columns || [];

const keyField =
  operations.update?.by || operations.delete?.by || config.idField || "id";

const hasSearch = Boolean(operations.search);
const hasAdd = Boolean(operations.add);
const hasUpdate = Boolean(operations.update);
const hasDelete = Boolean(operations.delete);
const hasActionsColumn = hasUpdate || hasDelete;

const title = config.title || config.entity || DEFAULTS.title;
const description = config.description || DEFAULTS.description;
const addLabel = config.addLabel || DEFAULTS.addLabel;

const searchFilters = operations.search?.filters || [];
// Every declared filter here is a relational `select` (source), so none of
// them qualifies as the plain free-text box: all filters render inline as
// selects, side by side.

const sourceEntities = {
  People: PeopleEntity,
  Role: roleEntity,
  Department: DepartmentEntity,
  Status: statusEntity,
};

function isNarrowColumn(field) {
  const f = field.toLowerCase();
  if (f === "id" || f === "age") return true;
  return false;
}

// ============================================================
// SOURCE OPTIONS
// ============================================================

function useSourceOptions(source, filters) {
  const [options, setOptions] = useState([]);

  const store = useMemo(() => {
    const entityConfig = sourceEntities[source.entity];
    return entityConfig ? createEntityStore(entityConfig) : null;
  }, [source.entity]);

  useEffect(() => {
    if (!store) return;
    let cancelled = false;

    const load = async () => {
      const op = source.operation || "getAll";
      const state = store.getState();

      if (op === "search" && state.search) {
        await state.search(filters || {});
      } else if (state.fetchAll) {
        await state.fetchAll();
      }

      if (cancelled) return;

      const fresh = store.getState();
      const rows =
        op === "search" ? fresh.searchState?.data : fresh.getAllState?.data;

      setOptions(
        (rows || []).map((r) => ({
          value: r[source.valueField],
          label: r[source.displayField],
        }))
      );
    };

    load();

    return () => {
      cancelled = true;
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [store, source.valueField, source.displayField, JSON.stringify(filters)]);

  return options;
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
// CONFIRM DELETE
// ============================================================

function ConfirmDialog({ open, onConfirm, onCancel, loading }) {
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

          <p className="mt-2 text-sm leading-6 text-slate-500">
            This action cannot be undone.
          </p>
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
            className="inline-flex items-center gap-2 rounded-xl bg-red-600 px-5 py-2.5 text-sm font-semibold text-white transition hover:bg-red-500 disabled:cursor-not-allowed disabled:opacity-50"
          >
            {loading && <RefreshCw className="h-4 w-4 animate-spin" />}
            {loading ? "Deleting..." : "Delete record"}
          </button>
        </div>
      </div>
    </div>
  );
}

// ============================================================
// VALIDATION
// ============================================================

function validateFields(fields, values, sourceOptionsByField) {
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

    if (field.type === "email" && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
      errors[field.name] = "Enter a valid email address.";
    }

    if (field.type === "number" && Number.isNaN(Number(value))) {
      errors[field.name] = "Enter a valid number.";
    }

    if (field.type === "select") {
      if (field.source) {
        const dynamicOptions = sourceOptionsByField?.[field.name] || [];
        const valid = dynamicOptions.some(
          (o) => String(o.value) === String(value)
        );
        if (!valid) {
          errors[field.name] = `${field.label} must be one of the listed options.`;
        }
      } else if (field.options) {
        const valid = field.options.some(
          (o) => o.value === value || o === value
        );
        if (!valid) {
          errors[field.name] = `${field.label} must be one of the listed options.`;
        }
      }
    }
  }
  return errors;
}

// ============================================================
// FORM MODAL
// ============================================================

function EntityFormModal({
  open,
  mode,
  fields,
  initialValues,
  sourceOptionsByField,
  onClose,
  onSubmit,
  loading,
}) {
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

    const validationErrors = validateFields(fields, values, sourceOptionsByField);

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
              Employee management
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
            {fields.map((field) => {
              const options = field.source
                ? sourceOptionsByField?.[field.name] || []
                : field.options || [];

              const base =
                "w-full rounded-xl border border-slate-200 bg-white px-4 py-3 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-400 focus:ring-4 focus:ring-emerald-500/10";

              return (
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
                        {options.map((opt) => (
                          <option key={opt.value ?? opt} value={opt.value ?? opt}>
                            {opt.label ?? opt}
                          </option>
                        ))}
                      </select>
                      <ChevronDown className="pointer-events-none absolute right-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                    </div>
                  ) : (
                    <input
                      className={base}
                      type={
                        field.type === "number"
                          ? "number"
                          : field.type === "email"
                          ? "email"
                          : field.type === "date"
                          ? "date"
                          : "text"
                      }
                      placeholder={field.placeholder}
                      value={values[field.name] ?? ""}
                      onChange={(e) => handleChange(field.name, e.target.value)}
                    />
                  )}

                  {errors[field.name] && (
                    <p className="mt-2 text-xs text-red-600">
                      {errors[field.name]}
                    </p>
                  )}
                </div>
              );
            })}
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
// EMPLOYEE PAGE
// ============================================================

export default function EmployeePage() {
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
  } = useEmployeeStore();

  const [filters, setFilters] = useState(() =>
    Object.fromEntries(searchFilters.map((f) => [f.name, ""]))
  );

  const [addOpen, setAddOpen] = useState(false);
  const [editRow, setEditRow] = useState(null);
  const [deleteRow, setDeleteRow] = useState(null);
  const [refreshing, setRefreshing] = useState(false);

  const { showToast, ToastView } = useToast();

  // --- options for the toolbar filters (always show every choice) ---
  const nameSource = searchFilters.find((f) => f.name === "Name")?.source;
  const roleNameSource = searchFilters.find((f) => f.name === "RoleName")?.source;
  const departmentNameSource = searchFilters.find(
    (f) => f.name === "DepartmentName"
  )?.source;
  const statusNameSource = searchFilters.find(
    (f) => f.name === "StatusName"
  )?.source;

  const nameOptions = useSourceOptions(nameSource);
  const roleNameOptions = useSourceOptions(roleNameSource);
  const departmentNameOptions = useSourceOptions(departmentNameSource);
  const statusNameOptions = useSourceOptions(statusNameSource, {
    StatusTypeCode: "Employee",
  });

  const filterOptionsByField = {
    Name: nameOptions,
    RoleName: roleNameOptions,
    DepartmentName: departmentNameOptions,
    StatusName: statusNameOptions,
  };

  // --- options for the Add/Edit form selects ---
  const personIdSource = operations.add?.fields.find(
    (f) => f.name === "personID"
  )?.source;
  const roleIdSource = operations.add?.fields.find(
    (f) => f.name === "roleId"
  )?.source;
  const departmentIdSource = operations.add?.fields.find(
    (f) => f.name === "departmentId"
  )?.source;
  const statusIdSource = operations.add?.fields.find(
    (f) => f.name === "statusID"
  )?.source;

  const personIdOptions = useSourceOptions(personIdSource);
  const roleIdOptions = useSourceOptions(roleIdSource);
  const departmentIdOptions = useSourceOptions(departmentIdSource);
  const statusIdOptions = useSourceOptions(statusIdSource, {
    StatusTypeCode: "Employee",
  });

  const formSourceOptions = {
    personID: personIdOptions,
    roleId: roleIdOptions,
    departmentId: departmentIdOptions,
    statusID: statusIdOptions,
  };

  useEffect(() => {
    fetchAll?.();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const hasActiveFilter = Object.values(filters).some(
    (v) => v !== "" && v !== undefined && v !== null
  );
  const rows = hasActiveFilter ? searchState?.data || [] : getAllState?.data || [];
  const loading = hasActiveFilter ? searchState?.loading : getAllState?.loading;

  const runSearchOrFetchAll = (nextFilters) => {
    const active = Object.values(nextFilters).some(
      (v) => v !== "" && v !== undefined && v !== null
    );
    if (active && search) {
      search(nextFilters);
    } else {
      fetchAll?.();
    }
  };

  const handleFilterChange = (name, value) => {
    const nextFilters = { ...filters, [name]: value };
    setFilters(nextFilters);
    runSearchOrFetchAll(nextFilters);
  };

  const clearFilters = () => {
    const empty = Object.fromEntries(searchFilters.map((f) => [f.name, ""]));
    setFilters(empty);
    fetchAll?.();
  };

  const refresh = async () => {
    setRefreshing(true);
    try {
      if (hasActiveFilter && search) {
        await search(filters);
      } else {
        await fetchAll?.();
      }
    } finally {
      window.setTimeout(() => setRefreshing(false), 500);
    }
  };

  const handleAdd = async (values) => {
    await add(values);
    const result = useEmployeeStore.getState().addState;
    if (result.errorCode === 0) {
      showToast({
        type: "success",
        title: "Employee created",
        description: result.message || `${title} created successfully.`,
      });
      setAddOpen(false);
      refresh();
    } else {
      showToast({
        type: "error",
        title: "Unable to create employee",
        description: result.message,
      });
    }
  };

  const handleUpdate = async (values) => {
    await update(editRow[keyField], values);
    const result = useEmployeeStore.getState().updateState;
    if (result.errorCode === 0) {
      showToast({
        type: "success",
        title: "Employee updated",
        description: result.message || `${title} updated successfully.`,
      });
      setEditRow(null);
      refresh();
    } else {
      showToast({
        type: "error",
        title: "Unable to update employee",
        description: result.message,
      });
    }
  };

  const handleDelete = async () => {
    await remove(deleteRow[keyField]);
    const result = useEmployeeStore.getState().deleteState;
    if (result.errorCode === 0) {
      showToast({
        type: "success",
        title: "Employee deleted",
        description: result.message || `${title} deleted successfully.`,
      });
    } else {
      showToast({
        type: "error",
        title: "Unable to delete employee",
        description: result.message,
      });
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
                  People management
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
          <StatCard
            icon={Users}
            label="Total employees"
            value={rows.length}
            description="Records currently visible"
          />
          <StatCard
            icon={Search}
            label="Search"
            value={hasActiveFilter ? "Active" : "All"}
            description={
              hasActiveFilter ? "Search filters are active" : "Showing all records"
            }
          />
          <StatCard
            icon={Users}
            label="Results"
            value={rows.length}
            description="Current result count"
          />
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
                <h2 className="text-sm font-semibold text-slate-900">
                  Search & filters
                </h2>
                <p className="mt-1 text-xs text-slate-400">
                  Results update automatically while you type.
                </p>
              </div>

              {hasActiveFilter && (
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

            <div className="grid grid-cols-1 gap-3 md:grid-cols-2 xl:grid-cols-4">
              {searchFilters.map((filter) => (
                <div key={filter.name}>
                  <label className="mb-2 block text-[11px] font-semibold uppercase tracking-wider text-slate-400">
                    {filter.label}
                  </label>

                  <div className="relative">
                    {filter.name === "Name" && (
                      <Search className="pointer-events-none absolute left-3.5 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                    )}

                    <select
                      className={`h-11 w-full appearance-none rounded-xl border border-slate-200 bg-white pr-10 text-sm text-slate-900 outline-none transition focus:border-emerald-400 focus:ring-4 focus:ring-emerald-500/10 ${
                        filter.name === "Name" ? "pl-10" : "px-4"
                      }`}
                      value={filters[filter.name]}
                      onChange={(e) => handleFilterChange(filter.name, e.target.value)}
                    >
                      <option value="">All</option>
                      {(filterOptionsByField[filter.name] || []).map((opt) => (
                        <option key={opt.value ?? opt} value={opt.value ?? opt}>
                          {opt.label ?? opt}
                        </option>
                      ))}
                    </select>

                    <ChevronDown className="pointer-events-none absolute right-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                  </div>
                </div>
              ))}
            </div>
          </section>
        )}

        {/* TABLE */}
        <section className="mt-5 overflow-hidden rounded-2xl border border-slate-200 bg-white">
          <div className="flex flex-col gap-3 border-b border-slate-100 px-5 py-4 sm:flex-row sm:items-center sm:justify-between lg:px-6">
            <div>
              <h2 className="text-sm font-semibold text-slate-900">
                Employee directory
              </h2>
              <p className="mt-1 text-xs text-slate-400">
                {hasActiveFilter ? "Showing filtered results" : "Complete employee directory"}
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
                <Users className="h-7 w-7" />
              </div>

              <h3 className="text-lg font-semibold text-slate-900">
                {hasActiveFilter ? "No matching records" : `No ${title.toLowerCase()} yet`}
              </h3>

              <p className="mt-2 max-w-md text-sm leading-6 text-slate-500">
                {hasActiveFilter
                  ? "Nothing matches your current search. Try changing the filters."
                  : "There are currently no records available in the system."}
              </p>

              {hasAdd && !hasActiveFilter && (
                <button
                  type="button"
                  onClick={() => setAddOpen(true)}
                  className="mt-6 inline-flex items-center gap-2 rounded-xl bg-emerald-600 px-5 py-2.5 text-sm font-semibold text-white transition hover:bg-emerald-500"
                >
                  <Plus className="h-4 w-4" />
                  Add first employee
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
                        className={`whitespace-nowrap px-6 py-4 text-left text-[10px] font-semibold uppercase tracking-[0.16em] text-slate-400 ${
                          isNarrowColumn(col.field) ? "text-center" : ""
                        }`}
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
                    {rows.map((row, index) => (
                      <tr
                        key={row[keyField] ?? index}
                        className="group border-b border-slate-100 transition-colors last:border-0 hover:bg-slate-50/70"
                      >
                        {columns.map((col, j) => {
                          const value = row[col.field];
                          const narrow = isNarrowColumn(col.field);

                          return (
                            <td
                              key={col.field}
                              className={`px-6 py-5 align-middle ${narrow ? "text-center" : ""}`}
                            >
                              {j === 0 && !narrow ? (
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
          mode="add"
          fields={operations.add.fields}
          initialValues={{}}
          sourceOptionsByField={formSourceOptions}
          onClose={() => setAddOpen(false)}
          onSubmit={handleAdd}
          loading={addState?.loading}
        />
      )}

      {hasUpdate && (
        <EntityFormModal
          open={Boolean(editRow)}
          mode="edit"
          fields={operations.update.fields}
          initialValues={editRow || {}}
          sourceOptionsByField={formSourceOptions}
          onClose={() => setEditRow(null)}
          onSubmit={handleUpdate}
          loading={updateState?.loading}
        />
      )}

      {hasDelete && (
        <ConfirmDialog
          open={Boolean(deleteRow)}
          onConfirm={handleDelete}
          onCancel={() => setDeleteRow(null)}
          loading={deleteState?.loading}
        />
      )}

      {ToastView}
    </div>
  );
}