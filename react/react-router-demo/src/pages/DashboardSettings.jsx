import { Settings, Sliders, ToggleLeft } from 'lucide-react';

export default function DashboardSettings() {
  return (
    <div>
      <h2 style={{ fontSize: '1.5rem', marginBottom: '0.5rem', display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
        <Settings color="var(--accent-warning)" /> Dashboard Settings Sub-Route
      </h2>
      <p style={{ color: 'var(--text-secondary)', marginBottom: '1.5rem' }}>
        Renders at <code>/dashboard/settings</code> inside the same parent DashboardLayout.
      </p>

      <div style={{ background: 'var(--bg-main)', border: '1px solid var(--border-color)', borderRadius: '10px', padding: '1.25rem', display: 'flex', flexDirection: 'column', gap: '1rem' }}>
        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
            <Sliders size={18} />
            <span>Enable Strict Route Matching</span>
          </div>
          <ToggleLeft size={24} color="var(--accent-primary)" />
        </div>

        <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
          <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
            <Sliders size={18} />
            <span>Simulate Slow Route Loading</span>
          </div>
          <ToggleLeft size={24} color="var(--text-secondary)" />
        </div>
      </div>
    </div>
  );
}
