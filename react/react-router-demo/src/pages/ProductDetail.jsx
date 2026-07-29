import { useParams, useNavigate, Link } from 'react-router-dom';
import ConceptBadge from '../components/ConceptBadge';
import { ArrowLeft, Tag, DollarSign, CheckCircle } from 'lucide-react';

export default function ProductDetail() {
  // useParams Hook extracts path parameters defined in <Route path="/products/:id" />
  const { id } = useParams();
  const navigate = useNavigate();

  return (
    <div>
      <ConceptBadge 
        concept="Hook: useParams()"
        title={`Dynamic Route Parameter Demo (ID: ${id})`}
        description="Demonstrates capturing dynamic segments from the URL route definition."
        codeSnippet="const { id } = useParams(); // URL: /products/:id"
      />

      <div style={{ marginBottom: '1.5rem' }}>
        <button onClick={() => navigate('/products')} className="btn btn-secondary">
          <ArrowLeft size={16} /> Back to Products List
        </button>
      </div>

      <div className="card" style={{ maxWidth: '600px' }}>
        <div className="concept-tag">Product ID Parameter: {id}</div>
        <h2 className="card-title" style={{ fontSize: '1.6rem', margin: '0.75rem 0' }}>
          Demonstration Item #{id}
        </h2>
        <p className="card-desc">
          This component dynamically rendered because the route matched <code>/products/:id</code> with path parameter <code>id = "{id}"</code>.
        </p>

        <div style={{ display: 'flex', gap: '1.5rem', margin: '1.5rem 0', flexWrap: 'wrap' }}>
          <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', color: 'var(--accent-secondary)' }}>
            <Tag size={18} /> Category: Tech Demo
          </div>
          <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', color: 'var(--accent-success)' }}>
            <DollarSign size={18} /> Status: Dynamic Match
          </div>
          <div style={{ display: 'flex', alignItems: 'center', gap: '0.5rem', color: 'var(--text-secondary)' }}>
            <CheckCircle size={18} /> Verified Hook
          </div>
        </div>

        <div style={{ borderTop: '1px solid var(--border-color)', paddingTop: '1rem', marginTop: '1rem' }}>
          <p style={{ fontSize: '0.9rem', color: 'var(--text-secondary)', marginBottom: '0.75rem' }}>
            Quick-Switch to test other dynamic params:
          </p>
          <div style={{ display: 'flex', gap: '0.5rem' }}>
            <Link to="/products/101" className="btn btn-secondary" style={{ padding: '0.4rem 0.8rem', fontSize: '0.85rem' }}>ID: 101</Link>
            <Link to="/products/102" className="btn btn-secondary" style={{ padding: '0.4rem 0.8rem', fontSize: '0.85rem' }}>ID: 102</Link>
            <Link to="/products/999" className="btn btn-secondary" style={{ padding: '0.4rem 0.8rem', fontSize: '0.85rem' }}>ID: 999</Link>
          </div>
        </div>
      </div>
    </div>
  );
}
