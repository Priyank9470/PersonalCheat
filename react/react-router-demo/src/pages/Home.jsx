import { Link } from 'react-router-dom';
import ConceptBadge from '../components/ConceptBadge';
import { Route, Layers, Key, Search, ArrowRight, ShieldCheck, Zap, AlertTriangle } from 'lucide-react';

export default function Home() {
  const routerConcepts = [
    {
      icon: <Route size={24} />,
      title: '1. Basic Routing',
      desc: 'Map URL paths to React components using BrowserRouter, Routes, and Route.',
      link: '/products',
      tag: 'Core Concept'
    },
    {
      icon: <Layers size={24} />,
      title: '2. Nested Routes & Outlet',
      desc: 'Render nested UI layouts sharing headers/sidebars using <Outlet />.',
      link: '/dashboard',
      tag: 'Layout Pattern'
    },
    {
      icon: <Key size={24} />,
      title: '3. Dynamic URL Params',
      desc: 'Extract path parameters (e.g. /products/:id) dynamically with useParams().',
      link: '/products/42',
      tag: 'Hook: useParams'
    },
    {
      icon: <Search size={24} />,
      title: '4. Search Query Params',
      desc: 'Read and update URL query strings (e.g. ?category=tech) with useSearchParams().',
      link: '/products?category=electronics&sort=asc',
      tag: 'Hook: useSearchParams'
    },
    {
      icon: <ArrowRight size={24} />,
      title: '5. Programmatic Navigation',
      desc: 'Navigate imperatively via code using useNavigate() and pass location state.',
      link: '/login',
      tag: 'Hook: useNavigate'
    },
    {
      icon: <ShieldCheck size={24} />,
      title: '6. Protected Routes',
      desc: 'Guard routes requiring authentication and redirect unauthorized users to Login.',
      link: '/dashboard',
      tag: 'Auth Pattern'
    },
    {
      icon: <Zap size={24} />,
      title: '7. Lazy Loading & Suspense',
      desc: 'Split route bundles with React.lazy() and Suspense to boost load speed.',
      link: '/dashboard/analytics',
      tag: 'Performance'
    },
    {
      icon: <AlertTriangle size={24} />,
      title: '8. 404 Catch-All Route',
      desc: 'Handle unmatched URL paths using path="*" to show a custom Not Found page.',
      link: '/non-existent-page-demo',
      tag: 'Wildcard Route'
    }
  ];

  return (
    <div>
      <ConceptBadge 
        concept="React Router v6/v7 Architecture"
        title="Interactive React Routing Demonstration"
        description="Explore live implementations of all essential React Router features used in technical interview evaluations and production applications."
        codeSnippet="import { BrowserRouter, Routes, Route, useNavigate, useParams, useSearchParams } from 'react-router-dom';"
      />

      <div className="grid-cards">
        {routerConcepts.map((item, idx) => (
          <div className="card" key={idx}>
            <div className="card-icon">{item.icon}</div>
            <span className="concept-tag" style={{ fontSize: '0.7rem', padding: '0.15rem 0.5rem' }}>{item.tag}</span>
            <h2 className="card-title" style={{ marginTop: '0.5rem' }}>{item.title}</h2>
            <p className="card-desc">{item.desc}</p>
            <Link to={item.link} className="btn btn-secondary" style={{ width: '100%', justifyContent: 'center' }}>
              Test Feature <ArrowRight size={16} />
            </Link>
          </div>
        ))}
      </div>
    </div>
  );
}
