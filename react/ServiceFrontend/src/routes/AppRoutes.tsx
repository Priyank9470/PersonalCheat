import { Routes, Route, Navigate } from 'react-router-dom';
import { Layout } from '../components/Layout';
import { ProtectedRoute } from '../components/ProtectedRoute';
import { LoginPage } from '../pages/LoginPage';
import { ServiceListPage } from '../pages/ServiceListPage';
import { ServiceDetailPage } from '../pages/ServiceDetailPage';
import { ServiceFormPage } from '../pages/ServiceFormPage';

export const AppRoutes = () => {
  return (
    <Routes>
      {/* Public Authentication Route */}
      <Route path="/login" element={<LoginPage />} />

      {/* Protected Routes Wrapper */}
      <Route
        path="/"
        element={
          <ProtectedRoute>
            <Layout />
          </ProtectedRoute>
        }
      >
        {/* Default route redirects to services listing */}
        <Route index element={<Navigate to="/service" replace />} />

        {/* Services Directory Listing */}
        <Route path="service" element={<ServiceListPage />} />

        {/* Create New Service */}
        <Route path="service/new" element={<ServiceFormPage />} />

        {/* Dynamic Edit Route: /service/edit/[id] */}
        <Route path="service/edit/:id" element={<ServiceFormPage />} />

        {/* Dynamic Detail Route: /service/[ServiceNumber] */}
        <Route path="service/:serviceNumber" element={<ServiceDetailPage />} />

        {/* Catch-all redirect */}
        <Route path="*" element={<Navigate to="/service" replace />} />
      </Route>
    </Routes>
  );
};
