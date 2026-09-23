import { useEffect, useMemo, useState } from "react";
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
  ShieldCheck,
} from "lucide-react";
import { usersEntity } from "../entities/UsersEntity";
import useUsersStore from "../stores/UsersStore";
import { createEntityStore } from "../stores/createEntityStore";

// Referenced entities used by this page's relational (`source`) fields.
import { roleEntity } from "../entities/RoleEntity";
import { permissionsEntity } from "../entities/PermissionsEntity";

const inputClass =
  "w-full rounded-xl border border-slate-200 bg-white px-4 py-3 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-400 focus:ring-4 focus:ring-emerald-500/10";

const config = usersEntity;
const operations = config.operations || {};

const columns = operations.getAll?.columns || operations.search?.columns || [];

const hasSearch = Boolean(operations.search);
const hasAdd = Boolean(operations.add);
const hasUpdate = Boolean(operations.update);
const hasDelete = Boolean(operations.delete);
const hasActionsColumn = hasUpdate || hasDelete;

const title = config.title || config.entity;
const description = config.description || "";
const addLabel = config.addLabel || `Add ${title}`;

const searchFilters = operations.search?.filters || [];

const sourceEntities = {
  Role: roleEntity,
  Permissions: permissionsEntity,
};

// Users' update/delete use a compound key (username + passwordHash) instead
// of a single id. createEntityApi.js only replaces a single "{value}"
// placeholder in the endpoint template (see UsersEntity.js: endpoint:
// "{value}"), so we build the composite "username/passwordHash" path
// segment ourselves here and pass it in as that one value.
function buildKey(usernameValue, passwordHashValue) {
  return `${encodeURIComponent(usernameValue)}/${encodeURIComponent(passwordHashValue)}`;
}

// The Add API expects permissions as [{ permissionID: <id> }, ...], not a
// flat array of ids — the picker itself works with flat ids for the UI, so
// we convert only right before sending.
function toPermissionsPayload(ids) {
  return (ids || []).map((id) => ({ permissionID: id }));
}

function nativeInputType(type) {
  if (type === "number") return "number";
  if (type === "email") return "email";
  if (type === "date") return "date";
  if (type === "password") return "password";
  return "text";
}

function useSourceOptions(source, filters) {
  const [options, setOptions] = useState([]);

  const store = useMemo(() => {
    const entityConfig = source ? sourceEntities[source.entity] : null;
    return entityConfig ? createEntityStore(entityConfig) : null;
  }, [source]);

  useEffect(() => {
    if (!store || !source) return;
    let cancelled = false;

    const load = async () => {
      const op = filters ? "search" : source.operation || "getAll";
      const state = store.getState();
      if (op === "search" && state.search) {
        await state.search(filters || {});
      } else if (state.fetchAll) {
        await state.fetchAll();
      }
      if (cancelled) return;
      const fresh = store.getState();
      const rows = op === "search" ? fresh.searchState?.data : fresh.getAllState?.data;
      setOptions(
        (rows || []).map((r) => ({
          value: r[source.valueField],
          label: r[source.displayField],
        })),
      );
    };

    load();
    return () => {
      cancelled = true;
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [store, source?.valueField, source?.displayField, JSON.stringify(filters)]);

  return options;
}

// Same idea as useSourceOptions, but buckets the rows by `source.groupBy`
// (e.g. Permissions grouped by ModuleName) instead of returning a flat list.
function useGroupedSourceOptions(source) {
  const [groups, setGroups] = useState([]);
  const [loading, setLoading] = useState(false);

  const store = useMemo(() => {
    const entityConfig = source ? sourceEntities[source.entity] : null;
    return entityConfig ? createEntityStore(entityConfig) : null;
  }, [source]);

  useEffect(() => {
    if (!store || !source) return;
    let cancelled = false;

    const load = async () => {
      setLoading(true);
      const op = source.operation || "getAll";
      const state = store.getState();
      if (op === "search" && state.search) {
        await state.search({});
      } else if (state.fetchAll) {
        await state.fetchAll();
      }
      if (cancelled) return;
      const fresh = store.getState();
      const rows = op === "search" ? fresh.searchState?.data : fresh.getAllState?.data;

      const groupField = source.groupBy;
      const byGroup = new Map();
      (rows || []).forEach((r) => {
        const groupName = groupField ? (r[groupField] ?? "Other") : "All";
        const opt = { value: r[source.valueField], label: r[source.displayField] };
        if (!byGroup.has(groupName)) byGroup.set(groupName, []);
        byGroup.get(groupName).push(opt);
      });

      const nextGroups = Array.from(byGroup.entries())
        .map(([name, options]) => ({ name, options }))
        .sort((a, b) => a.name.localeCompare(b.name));

      if (!cancelled) {
        setGroups(nextGroups);
        setLoading(false);
      }
    };

    load();
    return () => {
      cancelled = true;
    };
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [store, source?.valueField, source?.displayField, source?.groupBy, source?.operation]);

  return { groups, loading };
}

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

function StatusPill({ active }) {
  return active ? (
    <span className="inline-flex items-center gap-1.5 rounded-full border border-emerald-200 bg-emerald-50 px-2.5 py-1 text-xs font-semibold text-emerald-700">
      <span className="h-1.5 w-1.5 rounded-full bg-emerald-500" />
      Active
    </span>
  ) : (
    <span className="inline-flex items-center gap-1.5 rounded-full border border-slate-200 bg-slate-50 px-2.5 py-1 text-xs font-semibold text-slate-500">
      <span className="h-1.5 w-1.5 rounded-full bg-slate-400" />
      Inactive
    </span>
  );
}

function GroupedMultiSelect({ groups, loading, selected, onChange, placeholder }) {
  const [query, setQuery] = useState("");
  const [expanded, setExpanded] = useState({});

  const selectedSet = new Set((selected || []).map(String));

  const q = query.trim().toLowerCase();
  const filteredGroups = groups
    .map((group) => {
      if (!q) return group;
      const groupMatches = group.name.toLowerCase().includes(q);
      const options = groupMatches
        ? group.options
        : group.options.filter((o) => o.label.toLowerCase().includes(q));
      return { ...group, options };
    })
    .filter((group) => group.options.length > 0);

  const toggleGroup = (name) => setExpanded((prev) => ({ ...prev, [name]: !prev[name] }));

  const toggleOption = (value) => {
    const key = String(value);
    const next = selectedSet.has(key)
      ? (selected || []).filter((v) => String(v) !== key)
      : [...(selected || []), value];
    onChange(next);
  };

  const toggleGroupAll = (group) => {
    const groupValues = group.options.map((o) => o.value);
    const allSelected = groupValues.every((v) => selectedSet.has(String(v)));
    if (allSelected) {
      onChange((selected || []).filter((v) => !groupValues.some((gv) => String(gv) === String(v))));
    } else {
      const merged = new Set([...(selected || []).map(String), ...groupValues.map(String)]);
      const allOptions = groups.flatMap((g) => g.options);
      onChange(allOptions.filter((o) => merged.has(String(o.value))).map((o) => o.value));
    }
  };

  return (
    <div className="overflow-hidden rounded-xl border border-slate-200">
      <div className="flex items-center justify-between gap-2 border-b border-slate-100 bg-slate-50/60 px-3.5 py-2.5">
        <input
          type="text"
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          placeholder={placeholder || "Search modules or permissions..."}
          className="w-full bg-transparent text-sm text-slate-700 placeholder:text-slate-400 focus:outline-none"
        />
        <span className="whitespace-nowrap rounded-full bg-emerald-50 px-2.5 py-1 text-xs font-semibold text-emerald-700">
          {(selected || []).length} selected
        </span>
      </div>

      <div className="max-h-56 overflow-y-auto">
        {loading && (
          <div className="px-3.5 py-4 text-center text-xs text-slate-400">Loading permissions…</div>
        )}
        {!loading && filteredGroups.length === 0 && (
          <div className="px-3.5 py-4 text-center text-xs text-slate-400">No permissions found.</div>
        )}
        {!loading &&
          filteredGroups.map((group) => {
            const groupValues = group.options.map((o) => o.value);
            const selectedInGroup = groupValues.filter((v) => selectedSet.has(String(v))).length;
            const allSelected = selectedInGroup === groupValues.length && groupValues.length > 0;
            const isOpen = expanded[group.name] ?? Boolean(q);

            return (
              <div key={group.name} className="border-b border-slate-50 last:border-0">
                <button
                  type="button"
                  onClick={() => toggleGroup(group.name)}
                  className="flex w-full items-center justify-between px-3.5 py-2.5 text-left transition hover:bg-slate-50"
                >
                  <span className="flex items-center gap-2">
                    <input
                      type="checkbox"
                      checked={allSelected}
                      onClick={(e) => e.stopPropagation()}
                      onChange={() => toggleGroupAll(group)}
                      className="h-3.5 w-3.5 rounded border-slate-300 text-emerald-600 focus:ring-emerald-100"
                    />
                    <span className="text-sm font-medium text-slate-800">{group.name}</span>
                    <span className="text-xs text-slate-400">
                      {selectedInGroup}/{groupValues.length}
                    </span>
                  </span>
                  <ChevronDown
                    className={`h-4 w-4 text-slate-400 transition-transform ${isOpen ? "rotate-180" : ""}`}
                  />
                </button>
                {isOpen && (
                  <div className="space-y-0.5 px-3.5 pb-2 pl-9">
                    {group.options.map((opt) => (
                      <label
                        key={opt.value}
                        className="flex items-center gap-2 rounded-lg px-1.5 py-1 text-sm text-slate-600 transition hover:bg-slate-50"
                      >
                        <input
                          type="checkbox"
                          checked={selectedSet.has(String(opt.value))}
                          onChange={() => toggleOption(opt.value)}
                          className="h-3.5 w-3.5 rounded border-slate-300 text-emerald-600 focus:ring-emerald-100"
                        />
                        {opt.label}
                      </label>
                    ))}
                  </div>
                )}
              </div>
            );
          })}
      </div>
    </div>
  );
}

function validateFields(fields, values, sourceOptionsByField) {
  const errors = {};
  for (const field of fields) {
    if (field.type === "checkbox") continue;
    if (field.multiple) continue; // grouped/multi selects validated separately if needed

    const value = values[field.name];
    const isEmpty = value === undefined || value === null || String(value).trim() === "";

    if (field.required && isEmpty) {
      errors[field.name] = `${field.label} is required.`;
      continue;
    }
    if (isEmpty) continue;

    if (field.type === "select" && field.source) {
      const dynamicOptions = sourceOptionsByField?.[field.name] || [];
      const valid = dynamicOptions.some((o) => String(o.value) === String(value));
      if (!valid) errors[field.name] = `${field.label} must be one of the listed options.`;
    }
  }
  return errors;
}

function EntityFormModal({
  mode,
  fields,
  initialValues,
  sourceOptionsByField,
  groupedSourceOptionsByField,
  contextNote,
  onClose,
  onSubmit,
  loading,
}) {
  const [values, setValues] = useState(() => {
    const init = { ...(initialValues || {}) };
    fields.forEach((f) => {
      if (f.type === "checkbox" && init[f.name] === undefined) init[f.name] = false;
      if (f.multiple && init[f.name] === undefined) init[f.name] = [];
      // Never pre-fill password-type fields from row data (it's never
      // returned by the API anyway) — always start blank.
      if (f.type === "password") init[f.name] = "";
    });
    return init;
  });
  const [errors, setErrors] = useState({});

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

        {contextNote && (
          <p className="border-b border-slate-100 px-6 py-3 text-xs text-slate-500">{contextNote}</p>
        )}

        <div className="overflow-y-auto px-6 py-6">
          <div className="grid grid-cols-1 gap-5 md:grid-cols-2">
            {fields.map((field) => {
              const isGrouped = field.multiple && field.source?.groupBy;
              const options = field.source ? sourceOptionsByField?.[field.name] || [] : field.options || [];

              return (
                <div
                  key={field.name}
                  className={field.type === "checkbox" || isGrouped ? "md:col-span-2" : ""}
                >
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
                      <label className="mb-2 flex items-center gap-1.5 text-xs font-semibold text-slate-600">
                        {isGrouped && <ShieldCheck className="h-3.5 w-3.5 text-emerald-500" />}
                        {field.label}
                        {field.required && <span className="ml-0.5 text-emerald-600">*</span>}
                      </label>
                      {isGrouped ? (
                        <GroupedMultiSelect
                          groups={groupedSourceOptionsByField?.[field.name]?.groups || []}
                          loading={groupedSourceOptionsByField?.[field.name]?.loading}
                          selected={values[field.name] || []}
                          onChange={(next) => handleChange(field.name, next)}
                        />
                      ) : field.type === "select" ? (
                        <div className="relative">
                          <select
                            className={`${inputClass} appearance-none pr-10`}
                            value={values[field.name] ?? ""}
                            onChange={(e) => handleChange(field.name, e.target.value)}
                          >
                            <option value="">Select...</option>
                            {options.map((opt, idx) => (
                              <option key={opt.value ?? idx} value={opt.value ?? ""}>
                                {opt.label ?? opt.value ?? ""}
                              </option>
                            ))}
                          </select>
                          <ChevronDown className="pointer-events-none absolute right-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                        </div>
                      ) : (
                        <input
                          className={inputClass}
                          type={nativeInputType(field.type)}
                          placeholder={field.placeholder}
                          value={values[field.name] ?? ""}
                          onChange={(e) => handleChange(field.name, e.target.value)}
                          autoComplete={field.type === "password" ? "new-password" : "off"}
                        />
                      )}
                    </>
                  )}
                  {errors[field.name] && (
                    <p className="mt-2 text-xs text-red-600">{errors[field.name]}</p>
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

function ConfirmDialog({ onCancel, onConfirm, loading, requirePassword, password, onPasswordChange }) {
  return (
    <div className="fixed inset-0 z-[90] flex items-center justify-center bg-slate-900/40 p-5 backdrop-blur-sm">
      <div className="w-full max-w-md overflow-hidden rounded-3xl border border-slate-200 bg-white shadow-2xl">
        <div className="p-6">
          <div className="mb-5 flex h-12 w-12 items-center justify-center rounded-2xl bg-red-50 text-red-600">
            <Trash2 className="h-5 w-5" />
          </div>
          <h3 className="text-xl font-semibold tracking-tight text-slate-900">Delete this user?</h3>
          <p className="mt-2 text-sm leading-6 text-slate-500">This action cannot be undone.</p>

          {requirePassword && (
            <div className="mt-4">
              <label className="mb-2 block text-xs font-semibold text-slate-600">Confirm password</label>
              <input
                type="password"
                autoComplete="off"
                className={inputClass}
                value={password}
                onChange={(e) => onPasswordChange(e.target.value)}
              />
            </div>
          )}
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
            disabled={loading || (requirePassword && !password)}
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

export default function UsersPage() {
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
  } = useUsersStore();

  const [filters, setFilters] = useState(() => Object.fromEntries(searchFilters.map((f) => [f.name, ""])));

  const [addOpen, setAddOpen] = useState(false);
  const [editRow, setEditRow] = useState(null);
  const [deleteRow, setDeleteRow] = useState(null);
  const [deletePassword, setDeletePassword] = useState("");

  const { toast, showToast, closeToast } = useToast();

  // --- Add/Edit form source options ---
  const roleIdSource = operations.add?.fields.find((f) => f.name === "roleID")?.source;
  const permissionsSource = operations.add?.fields.find((f) => f.name === "permissions")?.source;

  const roleIdOptions = useSourceOptions(roleIdSource);
  const permissionsGrouped = useGroupedSourceOptions(permissionsSource);

  const formSourceOptions = { roleID: roleIdOptions };
  const groupedFormSourceOptions = { permissions: permissionsGrouped };

  useEffect(() => {
    fetchAll?.();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  const hasActiveFilter = Object.values(filters).some((v) => v !== "" && v !== undefined && v !== null);
  const rows = hasActiveFilter ? searchState?.data || [] : getAllState?.data || [];
  const loading = hasActiveFilter ? searchState?.loading : getAllState?.loading;

  const runSearchOrFetchAll = (nextFilters) => {
    const active = Object.values(nextFilters).some((v) => v !== "" && v !== undefined && v !== null);
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
    const cleared = Object.fromEntries(searchFilters.map((f) => [f.name, ""]));
    setFilters(cleared);
    fetchAll?.();
  };

  const refresh = () => {
    if (hasActiveFilter && search) {
      search(filters);
    } else {
      fetchAll?.();
    }
  };

  const handleAdd = async (values) => {
    const payload = {
      ...values,
      permissions: toPermissionsPayload(values.permissions),
    };
    await add(payload);
    const result = useUsersStore.getState().addState;
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
    // username comes from the row (read-only); passwordHash is the current
    // password the user just typed in the form — both are required to hit
    // PUT api/userss/{username}/{passwordhash}. buildKey turns them into
    // the single composite "{value}" segment expected by createEntityApi.
    const key = buildKey(editRow.username, values.passwordHash);
    await update(key, values);
    const result = useUsersStore.getState().updateState;
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
    const key = buildKey(deleteRow.username, deletePassword);
    await remove(key);
    const result = useUsersStore.getState().deleteState;
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
    setDeletePassword("");
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
            icon={ShieldCheck}
            label="Filters"
            value={hasActiveFilter ? "Active" : "All"}
            description={hasActiveFilter ? "Search filters are active" : "Showing all users"}
          />
          <StatCard icon={Inbox} label="Results" value={rows.length} description="Current result count" />
          <StatCard
            icon={Plus}
            label="Management"
            value={hasAdd ? "Ready" : "View only"}
            description={hasAdd ? "Create and manage users" : "Read-only access"}
          />
        </section>

        {/* SEARCH / FILTERS */}
        {hasSearch && searchFilters.length > 0 && (
          <section className="mt-5 rounded-2xl border border-slate-200 bg-white p-4">
            <div className="mb-3 flex items-center justify-between">
              <div>
                <h2 className="text-sm font-semibold text-slate-900">Search & filters</h2>
                <p className="mt-1 text-xs text-slate-400">Results update automatically while you type.</p>
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

            <div className="grid grid-cols-1 gap-3 md:grid-cols-2">
              {searchFilters.map((filter) => (
                <div key={filter.name}>
                  <label className="mb-2 block text-[11px] font-semibold uppercase tracking-wider text-slate-400">
                    {filter.label}
                  </label>
                  {filter.options ? (
                    <div className="relative">
                      <select
                        className="h-11 w-full appearance-none rounded-xl border border-slate-200 bg-white px-4 pr-10 text-sm text-slate-900 outline-none transition focus:border-emerald-400 focus:ring-4 focus:ring-emerald-500/10"
                        value={filters[filter.name]}
                        onChange={(e) => handleFilterChange(filter.name, e.target.value)}
                      >
                        <option value="">All</option>
                        {filter.options.map((opt) => (
                          <option key={opt.value} value={opt.value}>
                            {opt.label}
                          </option>
                        ))}
                      </select>
                      <ChevronDown className="pointer-events-none absolute right-4 top-1/2 h-4 w-4 -translate-y-1/2 text-slate-400" />
                    </div>
                  ) : (
                    <input
                      className="h-11 w-full rounded-xl border border-slate-200 bg-white px-4 text-sm text-slate-900 outline-none transition placeholder:text-slate-400 focus:border-emerald-400 focus:ring-4 focus:ring-emerald-500/10"
                      type="text"
                      value={filters[filter.name]}
                      onChange={(e) => handleFilterChange(filter.name, e.target.value)}
                    />
                  )}
                </div>
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
                {hasActiveFilter ? "Showing filtered results" : `Complete ${title.toLowerCase()} list`}
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
                {hasActiveFilter ? "No results found" : `No ${title.toLowerCase()} yet`}
              </h3>
              <p className="mt-2 max-w-md text-sm leading-6 text-slate-500">
                {hasActiveFilter ? "Try adjusting your search — nothing matches." : "No data available."}
              </p>
              {hasAdd && !hasActiveFilter && (
                <button
                  type="button"
                  onClick={() => setAddOpen(true)}
                  className="mt-6 inline-flex items-center gap-2 rounded-xl bg-emerald-600 px-5 py-2.5 text-sm font-semibold text-white transition hover:bg-emerald-500"
                >
                  <Plus className="h-4 w-4" />
                  Add first user
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
                        key={row.id ?? row.username ?? i}
                        className="group border-b border-slate-100 transition-colors last:border-0 hover:bg-slate-50/70"
                      >
                        {columns.map((col) => (
                          <td key={col.field} className="px-6 py-5 align-middle">
                            {col.field === "username" ? (
                              <div className="flex items-center gap-3">
                                <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-xl border border-slate-200 bg-slate-50 text-xs font-semibold text-slate-500">
                                  {(row.username || "?").charAt(0).toUpperCase()}
                                </div>
                                <span className="font-medium text-slate-800">{row.username}</span>
                              </div>
                            ) : col.field === "isActive" ? (
                              <StatusPill active={Boolean(row.isActive)} />
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
                                  onClick={() => {
                                    setDeleteRow(row);
                                    setDeletePassword("");
                                  }}
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
          sourceOptionsByField={formSourceOptions}
          groupedSourceOptionsByField={groupedFormSourceOptions}
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
          sourceOptionsByField={formSourceOptions}
          groupedSourceOptionsByField={groupedFormSourceOptions}
          contextNote={`Editing "${editRow.username}" — enter their current password to confirm.`}
          onClose={() => setEditRow(null)}
          onSubmit={handleUpdate}
          loading={updateState?.loading}
        />
      )}

      {deleteRow && (
        <ConfirmDialog
          onCancel={() => setDeleteRow(null)}
          onConfirm={handleDelete}
          loading={deleteState?.loading}
          requirePassword
          password={deletePassword}
          onPasswordChange={setDeletePassword}
        />
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