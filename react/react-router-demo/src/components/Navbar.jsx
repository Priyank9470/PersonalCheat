import { NavLink, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { Compass, Home, Package, LayoutDashboard, LogIn, LogOut, ShieldAlert } from 'lucide-react';

export default function Navbar() {
  const { user, isAuthenticated, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  return (
    <header className="navbar">
      <div className="navbar-container">
        <NavLink to="/" className="brand-link">
          <Compass className="brand-icon" size={28} />
          <span>React Router Mastery</span>
        </NavLink>

        <nav>
          <ul className="nav-links">
            <li>
              <NavLink 
                to="/" 
                className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}
                end
              >
                <Home size={16} /> Home
              </NavLink>
            </li>
            <li>
              <NavLink 
                to="/products" 
                className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}
              >
                <Package size={16} /> Products (Params)
              </NavLink>
            </li>
            <li>
              <NavLink 
                to="/dashboard" 
                className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}
              >
                <LayoutDashboard size={16} /> Dashboard (Protected)
              </NavLink>
            </li>
          </ul>
        </nav>

        <div className="auth-controls">
          {isAuthenticated ? (
            <>
              <div className="user-badge">
                <ShieldAlert size={14} />
                <span>{user.name}</span>
              </div>
              <button onClick={handleLogout} className="btn btn-danger">
                <LogOut size={16} /> Logout
              </button>
            </>
          ) : (
            <NavLink to="/login" className="btn btn-primary">
              <LogIn size={16} /> Demo Login
            </NavLink>
          )}
        </div>
      </div>
    </header>
  );
}
