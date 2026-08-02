import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export const Navbar = () => {
  const { isAuthenticated, userName, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <header className="navbar">
      <div className="navbar-brand">
        <Link to="/service" className="logo">
          💇 Service Management System
        </Link>
        <span className="env-badge">API: {import.meta.env.VITE_API_BASE_URL}</span>
      </div>

      {isAuthenticated && (
        <nav className="navbar-links">
          <Link to="/service" className="nav-link">
            All Services
          </Link>
          <Link to="/service/new" className="nav-link highlight">
            + Add New Service
          </Link>
        </nav>
      )}

      <div className="auth-status">
        {isAuthenticated ? (
          <div className="user-profile">
            <span className="user-badge">👤 Welcome, <strong>{userName || 'Admin'}</strong></span>
            <button onClick={handleLogout} className="btn danger-sm">
              Logout
            </button>
          </div>
        ) : (
          <Link to="/login" className="btn primary-sm">
            Login
          </Link>
        )}
      </div>
    </header>
  );
};
