export default function PlaceholderPage({ title }) {
  return (
    <div className="p-6">
      <div className="rounded-2xl border border-slate-200 bg-white p-8 shadow-sm">
        <h1 className="text-2xl font-bold text-slate-900">
          {title}
        </h1>

        <p className="mt-2 text-sm text-slate-500">
          This page is under development.
        </p>
      </div>
    </div>
  );
}