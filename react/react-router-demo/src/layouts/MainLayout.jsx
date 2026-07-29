import { Outlet } from 'react-router-dom';
import Navbar from '../components/Navbar';

export default function MainLayout() {
  return (
    <div className="app-container">
      <Navbar />
      <main className="main-content">
        <Outlet />
      </main>
      <footer className="footer">
        <p>React Router Demo • Built for Technical Interviews & Architecture Rounds</p>
      </footer>
    </div>
  );
}
