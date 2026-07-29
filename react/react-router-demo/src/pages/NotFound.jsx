import { Link, useLocation } from 'react-router-dom';
import { AlertOctagon, Home } from 'lucide-react';

export default function NotFound() {
  const location = useLocation();

  return (
    <div style={{ textAlign: 'center', padding: '4rem 1rem', maxWidth: '600px', margin: '0 auto' }}>
      <div style={{ display: 'inline-flex', background: 'rgba(239, 68, 68, 0.15)', color: 'var(--accent-danger)', padding: '1.5rem', borderRadius: '50%', marginBottom: '1.5rem' }}>
        <AlertOctagon size={64} />
      </div>

      <h1 style={{ fontSize: '3rem', fontWeight: 800, color: '#fff', marginBottom: '0.5rem' }}>404</h1>
      <h2 style={{ fontSize: '1.5rem', marginBottom: '1rem' }}>Page Not Found</h2>

      <p style={{ color: 'var(--text-secondary)', marginBottom: '2rem' }}>
        No route match found for URL path: <code style={{ color: 'var(--accent-danger)' }}>{location.pathname}</code>
      </p>

      <div style={{ background: 'var(--bg-card)', border: '1px solid var(--border-color)', borderRadius: '10px', padding: '1rem', marginBottom: '2rem', textAlign: 'left', fontSize: '0.85rem' }}>
        <strong>React Router Mechanics:</strong> This page is rendered via the catch-all wildcard route:
        <div className="code-snippet" style={{ marginTop: '0.5rem' }}>
          <code>&lt;Route path="*" element={'{<NotFound />}'} /&gt;</code>
        </div>
      </div>

      <Link to="/" className="btn btn-primary">
        <Home size={18} /> Return to Home
      </Link>
    </div>
  );
}
