import { useAuth } from '../context/AuthContext';
import { ShieldCheck, User, Sparkles } from 'lucide-react';

export default function DashboardOverview() {
  const { user } = useAuth();

  return (
    <div>
      <h2 style={{ fontSize: '1.5rem', marginBottom: '0.5rem', display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
        <Sparkles color="var(--accent-secondary)" /> Index Route Overview
      </h2>
      <p style={{ color: 'var(--text-secondary)', marginBottom: '1.5rem' }}>
        This page renders when matching <code>/dashboard</code> (the <code>index</code> route inside DashboardLayout).
      </p>

      <div style={{ background: 'var(--bg-main)', border: '1px solid var(--border-color)', borderRadius: '10px', padding: '1.25rem' }}>
        <h3 style={{ fontSize: '1.1rem', marginBottom: '1rem', color: 'var(--accent-success)', display: 'flex', alignItems: 'center', gap: '0.5rem' }}>
          <ShieldCheck size={18} /> Authenticated User Session
        </h3>
        <p style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', marginBottom: '0.5rem' }}>
          <User size={16} /> <strong>Name:</strong> {user?.name || 'Interview Candidate'}
        </p>
        <p style={{ color: 'var(--text-secondary)', fontSize: '0.9rem' }}>
          <strong>Role:</strong> {user?.role || 'Senior React Engineer'}
        </p>
      </div>
    </div>
  );
}
