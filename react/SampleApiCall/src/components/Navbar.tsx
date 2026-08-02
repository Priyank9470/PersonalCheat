import { Link, useLocation } from 'react-router-dom';

interface NavbarProps {
  apiMode: 'fetch' | 'axios';
  onApiModeChange: (mode: 'fetch' | 'axios') => void;
}

export const Navbar = ({ apiMode, onApiModeChange }: NavbarProps) => {
  const location = useLocation();

  return (
    <header className="navbar">
      <div className="navbar-brand">
        <Link to="/" className="logo">
          ⚡ React API Cheatsheet
        </Link>
        <span className="env-tag">Base: {import.meta.env.VITE_API_BASE_URL}</span>
      </div>

      <nav className="navbar-links">
        <Link
          to="/"
          className={`nav-link ${location.pathname === '/' || location.pathname === '/posts' ? 'active' : ''}`}
        >
          All Posts (Listing)
        </Link>
        <Link
          to="/posts/new"
          className={`nav-link ${location.pathname === '/posts/new' ? 'active' : ''}`}
        >
          + Add (Custom Validation)
        </Link>
        <Link
          to="/posts/formik/new"
          className={`nav-link ${location.pathname === '/posts/formik/new' ? 'active' : ''}`}
        >
          + Add (Formik + Yup)
        </Link>
      </nav>

      <div className="api-mode-toggle">
        <span className="toggle-label">Engine:</span>
        <button
          className={`mode-btn ${apiMode === 'fetch' ? 'active' : ''}`}
          onClick={() => onApiModeChange('fetch')}
        >
          Fetch API
        </button>
        <button
          className={`mode-btn ${apiMode === 'axios' ? 'active' : ''}`}
          onClick={() => onApiModeChange('axios')}
        >
          Axios
        </button>
      </div>
    </header>
  );
};
