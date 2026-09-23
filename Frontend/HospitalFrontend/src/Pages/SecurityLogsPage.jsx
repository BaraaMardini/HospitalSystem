import { useEffect, useState } from "react";
import {
    RefreshCw,
    ShieldAlert,
    Globe,
    Clock,
} from "lucide-react";

import { securityLogsEntity } from "../entities/SecurityLogsEntity";
import useSecurityLogsStore from "../stores/SecurityLogsStore";

// ============================================================
// CONFIG
// ============================================================

const config = securityLogsEntity;
const operations = config.operations || {};

const DEFAULTS = {
    title: "Security Logs",
    description:
        "Track user activity and security events across the system.",
};

const columns = operations.getAll?.columns || [];

const keyField = config.idField || "id";

const title = config.title || config.entity || DEFAULTS.title;
const description =
    config.description || DEFAULTS.description;

// ============================================================
// HELPERS
// ============================================================

function isNarrowColumn(field) {
    const f = String(field).toLowerCase();

    return f === "id" || f === "userid";
}

// ============================================================
// STAT CARD
// ============================================================

function StatCard({
    icon: Icon,
    label,
    value,
    description,
}) {
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
                        <p className="mt-1 text-xs text-slate-400">
                            {description}
                        </p>
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
// SECURITY LOG PAGE
// ============================================================

export default function SecurityLogsPage() {
    const { getAllState, fetchAll } =
        useSecurityLogsStore();

    const [refreshing, setRefreshing] = useState(false);

    // ========================================================
    // INITIAL LOAD
    // ========================================================

    useEffect(() => {
        if (typeof fetchAll === "function") {
            fetchAll();
        }
    }, [fetchAll]);

    // ========================================================
    // DATA
    // ========================================================

    const rows = Array.isArray(getAllState?.data)
        ? getAllState.data
        : [];

    const loading = Boolean(getAllState?.loading);

    // ========================================================
    // REFRESH
    // ========================================================

    const refresh = async () => {
        if (typeof fetchAll !== "function") {
            return;
        }

        setRefreshing(true);

        try {
            await fetchAll();
        } finally {
            window.setTimeout(() => {
                setRefreshing(false);
            }, 500);
        }
    };

    // ========================================================
    // STATS
    // ========================================================

    const uniqueUsers = new Set(
        rows
            .map((row) => row?.userID)
            .filter(
                (value) =>
                    value !== null &&
                    value !== undefined
            )
    ).size;

    const uniqueIPs = new Set(
        rows
            .map((row) => row?.iPAddress)
            .filter(
                (value) =>
                    value !== null &&
                    value !== undefined
            )
    ).size;

    // ========================================================
    // RENDER
    // ========================================================

    return (
        <div className="min-h-full w-full bg-slate-50 text-slate-900">
            <div className="w-full px-5 py-6 sm:px-7 lg:px-9 xl:px-10">

                {/* ==================================================
                    HEADER
                ================================================== */}

                <section className="rounded-3xl border border-slate-200 bg-white">
                    <div className="flex flex-col gap-6 px-6 py-7 lg:flex-row lg:items-end lg:justify-between lg:px-8 lg:py-8">

                        <div>
                            <div className="mb-3 flex items-center gap-2">
                                <span className="h-2 w-2 rounded-full bg-emerald-500" />

                                <span className="text-[11px] font-semibold uppercase tracking-[0.2em] text-emerald-600">
                                    Security & Audit
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

                        {/* REFRESH BUTTON */}

                        <div className="flex items-center gap-3">
                            <button
                                type="button"
                                onClick={refresh}
                                disabled={
                                    refreshing || loading
                                }
                                className="inline-flex items-center gap-2 rounded-xl border border-slate-200 bg-white px-4 py-3 text-sm font-medium text-slate-600 transition hover:bg-slate-50 hover:text-slate-900 disabled:cursor-not-allowed disabled:opacity-50"
                            >
                                <RefreshCw
                                    className={`h-4 w-4 ${
                                        refreshing
                                            ? "animate-spin"
                                            : ""
                                    }`}
                                />

                                Refresh
                            </button>
                        </div>
                    </div>
                </section>

                {/* ==================================================
                    STATS
                ================================================== */}

                <section className="mt-5 grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-4">

                    <StatCard
                        icon={ShieldAlert}
                        label="Total Logs"
                        value={rows.length}
                        description="Events recorded"
                    />

                    <StatCard
                        icon={ShieldAlert}
                        label="Unique Users"
                        value={uniqueUsers}
                        description="Distinct users logged"
                    />

                    <StatCard
                        icon={Globe}
                        label="Unique IPs"
                        value={uniqueIPs}
                        description="Distinct source addresses"
                    />

                    <StatCard
                        icon={Clock}
                        label="Access"
                        value="View only"
                        description="Read-only audit trail"
                    />

                </section>

                {/* ==================================================
                    TABLE
                ================================================== */}

                <section className="mt-5 overflow-hidden rounded-2xl border border-slate-200 bg-white">

                    {/* TABLE HEADER */}

                    <div className="flex flex-col gap-3 border-b border-slate-100 px-5 py-4 sm:flex-row sm:items-center sm:justify-between lg:px-6">

                        <div>
                            <h2 className="text-sm font-semibold text-slate-900">
                                Security log directory
                            </h2>

                            <p className="mt-1 text-xs text-slate-400">
                                Complete record of system security events
                            </p>
                        </div>

                        <div className="flex items-center gap-2 text-xs text-slate-400">
                            <span className="h-2 w-2 rounded-full bg-emerald-500" />
                            Live data
                        </div>
                    </div>

                    {/* EMPTY STATE */}

                    {!loading && rows.length === 0 ? (

                        <div className="flex min-h-[420px] flex-col items-center justify-center px-6 text-center">

                            <div className="mb-5 flex h-16 w-16 items-center justify-center rounded-2xl border border-slate-200 bg-slate-50 text-slate-400">
                                <ShieldAlert className="h-7 w-7" />
                            </div>

                            <h3 className="text-lg font-semibold text-slate-900">
                                No{" "}
                                {title.toLowerCase()} yet
                            </h3>

                            <p className="mt-2 max-w-md text-sm leading-6 text-slate-500">
                                There are currently no records available in the system.
                            </p>
                        </div>

                    ) : (

                        <div className="w-full overflow-x-auto">

                            <table className="w-full min-w-[900px] table-auto">

                                {/* TABLE HEAD */}

                                <thead>
                                    <tr className="border-b border-slate-100 bg-slate-50/70">

                                        {columns.map((column) => {
                                            const narrow =
                                                isNarrowColumn(
                                                    column.field
                                                );

                                            return (
                                                <th
                                                    key={column.field}
                                                    className={`whitespace-nowrap px-6 py-4 text-left text-[10px] font-semibold uppercase tracking-[0.16em] text-slate-400 ${
                                                        narrow
                                                            ? "text-center"
                                                            : ""
                                                    }`}
                                                >
                                                    {
                                                        column.header
                                                    }
                                                </th>
                                            );
                                        })}

                                    </tr>
                                </thead>

                                {/* TABLE BODY */}

                                {loading ? (

                                    <tbody>

                                        {Array.from({
                                            length: 7,
                                        }).map((_, index) => (

                                            <tr
                                                key={index}
                                                className="border-b border-slate-100"
                                            >

                                                {columns.map(
                                                    (column) => (
                                                        <td
                                                            key={
                                                                column.field
                                                            }
                                                            className="px-6 py-5"
                                                        >
                                                            <div className="h-4 w-2/3 animate-pulse rounded bg-slate-100" />
                                                        </td>
                                                    )
                                                )}

                                            </tr>

                                        ))}

                                    </tbody>

                                ) : (

                                    <tbody>

                                        {rows.map(
                                            (row, index) => (

                                                <tr
                                                    key={
                                                        row?.[
                                                            keyField
                                                        ] ??
                                                        index
                                                    }
                                                    className="group border-b border-slate-100 transition-colors last:border-0 hover:bg-slate-50/70"
                                                >

                                                    {columns.map(
                                                        (
                                                            column,
                                                            columnIndex
                                                        ) => {

                                                            const value =
                                                                row?.[
                                                                    column
                                                                        .field
                                                                ];

                                                            const narrow =
                                                                isNarrowColumn(
                                                                    column.field
                                                                );

                                                            {/* ACTION */}

                                                            if (
                                                                column.field ===
                                                                "action"
                                                            ) {
                                                                return (
                                                                    <td
                                                                        key={
                                                                            column.field
                                                                        }
                                                                        className="px-6 py-5"
                                                                    >
                                                                        <span className="inline-flex rounded-lg bg-amber-50 px-2.5 py-1 text-xs font-bold text-amber-700">
                                                                            {value ??
                                                                                "—"}
                                                                        </span>
                                                                    </td>
                                                                );
                                                            }

                                                            {/* NORMAL CELL */}

                                                            return (
                                                                <td
                                                                    key={
                                                                        column.field
                                                                    }
                                                                    className={`px-6 py-5 align-middle ${
                                                                        narrow
                                                                            ? "text-center"
                                                                            : ""
                                                                    }`}
                                                                >

                                                                    {columnIndex ===
                                                                        0 &&
                                                                    !narrow ? (

                                                                        <span className="font-mono text-xs font-semibold text-slate-400">
                                                                            #
                                                                            {value ??
                                                                                "—"}
                                                                        </span>

                                                                    ) : (

                                                                        <span className="text-sm text-slate-500">
                                                                            {value ??
                                                                                "—"}
                                                                        </span>

                                                                    )}

                                                                </td>
                                                            );
                                                        }
                                                    )}

                                                </tr>

                                            )
                                        )}

                                    </tbody>

                                )}

                            </table>
                        </div>
                    )}

                    {/* ==================================================
                        FOOTER
                    ================================================== */}

                    {rows.length > 0 &&
                        !loading && (

                            <div className="flex flex-col gap-3 border-t border-slate-100 px-5 py-4 text-xs text-slate-400 sm:flex-row sm:items-center sm:justify-between lg:px-6">

                                <span>
                                    Showing{" "}
                                    <span className="font-medium text-slate-600">
                                        {rows.length}
                                    </span>{" "}
                                    records
                                </span>

                                <span>
                                    Page{" "}
                                    <span className="font-medium text-slate-600">
                                        1
                                    </span>{" "}
                                    of{" "}
                                    <span className="font-medium text-slate-600">
                                        1
                                    </span>
                                </span>

                            </div>
                        )}

                </section>
            </div>
        </div>
    );
}

