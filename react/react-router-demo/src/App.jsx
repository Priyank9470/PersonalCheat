import { lazy, Suspense } from 'react';
import { Routes, Route } from 'react-router-dom';
import MainLayout from './layouts/MainLayout';
import DashboardLayout from './layouts/DashboardLayout';
import Home from './pages/Home';
import Products from './pages/Products';
import ProductDetail from './pages/ProductDetail';
import DashboardOverview from './pages/DashboardOverview';
import DashboardSettings from './pages/DashboardSettings';
import Login from './pages/Login';
import NotFound from './pages/NotFound';
import ProtectedRoute from './components/ProtectedRoute';

// Lazy loading the Analytics route component for performance demonstration
const DashboardAnalytics = lazy(() => import('./pages/DashboardAnalytics'));

function LoadingFallback() {
  return (
    <div className="loading-spinner">
      <div className="spinner"></div>
      <p>Loading code-split route bundle (Suspense)...</p>
    </div>
  );
}

export default function App() {
  return (
    <Routes>
      {/* Root Main Layout Route */}
      <Route path="/" element={<MainLayout />}>
        {/* Public Routes */}
        <Route index element={<Home />} />
        <Route path="products" element={<Products />} />
        <Route path="products/:id" element={<ProductDetail />} />
        <Route path="login" element={<Login />} />

        {/* Protected Dashboard Routes (Nested) */}
        <Route 
          path="dashboard" 
          element={
            <ProtectedRoute>
              <DashboardLayout />
            </ProtectedRoute>
          }
        >
          <Route index element={<DashboardOverview />} />
          <Route 
            path="analytics" 
            element={
              <Suspense fallback={<LoadingFallback />}>
                <DashboardAnalytics />
              </Suspense>
            } 
          />
          <Route path="settings" element={<DashboardSettings />} />
        </Route>

        {/* Catch-all 404 Route */}
        <Route path="*" element={<NotFound />} />
      </Route>
    </Routes>
  );
}
