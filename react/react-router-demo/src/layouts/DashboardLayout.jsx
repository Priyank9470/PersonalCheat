import { NavLink, Outlet } from 'react-router-dom';
import ConceptBadge from '../components/ConceptBadge';
import { BarChart3, Settings, Grid } from 'lucide-react';

export default function DashboardLayout() {
  return (
    <div>
      <ConceptBadge 
        concept="Nested Routing & <Outlet />"
        title="Dashboard Nested Routes Demo"
        description="Demonstrates how parent route components can render sub-route content dynamically using the <Outlet /> component."
        codeSnippet='<Route path="/dashboard" element={<DashboardLayout />}> <Route index element={<Overview />} /> <Route path="analytics" element={<Analytics />} /> </Route>'
      />

      <div className="dashboard-container">
        <aside className="dashboard-sidebar">
          <div className="sidebar-title">Dashboard Sections</div>
          <NavLink 
            to="/dashboard" 
            end 
            className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}
          >
            <Grid size={16} /> Overview (Index)
          </NavLink>
          <NavLink 
            to="/dashboard/analytics" 
            className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}
          >
            <BarChart3 size={16} /> Analytics (Lazy)
          </NavLink>
          <NavLink 
            to="/dashboard/settings" 
            className={({ isActive }) => `nav-link ${isActive ? 'active' : ''}`}
          >
            <Settings size={16} /> Settings
          </NavLink>
        </aside>

        <section className="dashboard-main">
          {/* <Outlet /> renders whichever sub-route matches the URL */}
          <Outlet />
        </section>
      </div>
    </div>
  );
}
