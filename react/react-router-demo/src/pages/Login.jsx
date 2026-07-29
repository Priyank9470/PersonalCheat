import { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import ConceptBadge from '../components/ConceptBadge';
import { LogIn, KeyRound, ArrowRight } from 'lucide-react';

export default function Login() {
  const [username, setUsername] = useState('Interview Candidate');
  const { login } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  // Get the intended target location after login, defaulting to /dashboard
  const from = location.state?.from?.pathname || '/dashboard';

  const handleSubmit = (e) => {
    e.preventDefault();
    login(username);
    // Programmatic redirect back to the page the user tried to visit
    navigate(from, { replace: true });
  };

  return (
    <div style={{ maxWidth: '600px', margin: '0 auto' }}>
      <ConceptBadge 
        concept="Hook: useNavigate() & location.state"
        title="Programmatic Navigation & Auth Redirect"
        description="Demonstrates imperatively navigating users after login and restoring their previous destination using location.state.from."
        codeSnippet="const navigate = useNavigate(); navigate('/dashboard', { replace: true });"
      />

      {location.state?.from && (
        <div style={{ background: 'rgba(245, 158, 11, 0.15)', border: '1px solid rgba(245, 158, 11, 0.4)', color: 'var(--accent-warning)', padding: '0.85rem 1rem', borderRadius: '8px', marginBottom: '1.5rem', fontSize: '0.9rem' }}>
          <strong>Redirected Notice:</strong> You must log in to view <code>{location.state.from.pathname}</code>.
        </div>
      )}

      <div className="card">
        <h2 className="card-title" style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', marginBottom: '1rem' }}>
          <KeyRound color="var(--accent-primary)" /> Demo Authentication Form
        </h2>

        <form onSubmit={handleSubmit}>
          <div style={{ marginBottom: '1.25rem' }}>
            <label className="filter-label" style={{ display: 'block', marginBottom: '0.5rem' }}>
              Candidate Name:
            </label>
            <input 
              type="text" 
              value={username} 
              onChange={(e) => setUsername(e.target.value)} 
              className="text-input" 
              style={{ width: '100%' }}
              required 
            />
          </div>

          <button type="submit" className="btn btn-primary" style={{ width: '100%', justifyContent: 'center' }}>
            <LogIn size={18} /> Simulate Login & Redirect to {from} <ArrowRight size={16} />
          </button>
        </form>
      </div>
    </div>
  );
}
