import { TrendingUp, Activity, PieChart } from 'lucide-react';

export default function DashboardAnalytics() {
  return (
    <div>
      <h2 style={{ fontSize: '1.5rem', marginBottom: '0.5rem', display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
        <TrendingUp color="var(--accent-primary)" /> Analytics (Code-Split Component)
      </h2>
      <p style={{ color: 'var(--text-secondary)', marginBottom: '1.5rem' }}>
        This page was loaded dynamically via <code>React.lazy()</code> and wrapped in <code>&lt;Suspense&gt;</code> to optimize initial bundle size.
      </p>

      <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))', gap: '1rem' }}>
        <div style={{ background: 'var(--bg-main)', border: '1px solid var(--border-color)', padding: '1rem', borderRadius: '8px' }}>
          <Activity color="var(--accent-secondary)" size={20} />
          <h4 style={{ margin: '0.5rem 0 0.25rem 0', color: 'var(--text-secondary)', fontSize: '0.85rem' }}>Route Load Time</h4>
          <span style={{ fontSize: '1.4rem', fontWeight: 700 }}>0ms (Cached)</span>
        </div>

        <div style={{ background: 'var(--bg-main)', border: '1px solid var(--border-color)', padding: '1rem', borderRadius: '8px' }}>
          <PieChart color="var(--accent-success)" size={20} />
          <h4 style={{ margin: '0.5rem 0 0.25rem 0', color: 'var(--text-secondary)', fontSize: '0.85rem' }}>Bundle Splitting</h4>
          <span style={{ fontSize: '1.4rem', fontWeight: 700, color: 'var(--accent-success)' }}>Active</span>
        </div>
      </div>
    </div>
  );
}
