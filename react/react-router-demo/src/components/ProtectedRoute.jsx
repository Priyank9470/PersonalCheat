import { Navigate, useLocation } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

/**
 * ProtectedRoute Component:
 * - Checks if the user is authenticated.
 * - If NOT authenticated, redirects to /login using <Navigate />.
 * - Passes the current location state so the user can be redirected back after logging in.
 */
export default function ProtectedRoute({ children }) {
  const { isAuthenticated } = useAuth();
  const location = useLocation();

  if (!isAuthenticated) {
    // Redirect to login, storing current page location in state
    return <Navigate to="/login" state={{ from: location }} replace />;
  }

  return children;
}
