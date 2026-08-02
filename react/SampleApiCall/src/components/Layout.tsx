import { useState } from 'react';
import { Navbar } from './Navbar';
import { Outlet } from 'react-router-dom';

export const Layout = () => {
  const [apiMode, setApiMode] = useState<'fetch' | 'axios'>('fetch');

  return (
    <div className="app-container">
      <Navbar apiMode={apiMode} onApiModeChange={setApiMode} />
      
      <main className="main-content">
        {/* Pass active apiMode down to child route pages via Outlet context */}
        <Outlet context={{ apiMode }} />
      </main>

      <footer className="app-footer">
        <p>Technical Interview Prep • React Router v7 + TypeScript + Vite</p>
      </footer>
    </div>
  );
};
