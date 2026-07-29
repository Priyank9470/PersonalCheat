import { useSearchParams, Link } from 'react-router-dom';
import ConceptBadge from '../components/ConceptBadge';
import { Filter, ArrowUpDown, ExternalLink } from 'lucide-react';

const SAMPLE_PRODUCTS = [
  { id: '101', name: 'Ultra Wireless Headphones', category: 'electronics', price: 199 },
  { id: '102', name: 'Mechanical Keyboard RGB', category: 'electronics', price: 129 },
  { id: '103', name: 'Clean Code Book by Uncle Bob', category: 'books', price: 45 },
  { id: '104', name: 'Designing Data-Intensive Apps', category: 'books', price: 55 },
  { id: '105', name: 'React Developer Hoodie', category: 'clothing', price: 65 },
  { id: '106', name: 'Ergonomic Standing Desk Mouse', category: 'electronics', price: 89 },
];

export default function Products() {
  // useSearchParams Hook for handling URL Query Parameters
  const [searchParams, setSearchParams] = useSearchParams();

  const currentCategory = searchParams.get('category') || 'all';
  const currentSort = searchParams.get('sort') || 'asc';

  const handleCategoryChange = (e) => {
    const val = e.target.value;
    if (val === 'all') {
      searchParams.delete('category');
    } else {
      searchParams.set('category', val);
    }
    setSearchParams(searchParams);
  };

  const handleSortChange = (e) => {
    searchParams.set('sort', e.target.value);
    setSearchParams(searchParams);
  };

  // Filter & Sort logic
  const filteredProducts = SAMPLE_PRODUCTS
    .filter(p => currentCategory === 'all' || p.category === currentCategory)
    .sort((a, b) => currentSort === 'asc' ? a.price - b.price : b.price - a.price);

  return (
    <div>
      <ConceptBadge 
        concept="Hook: useSearchParams()"
        title="Query Parameters & URL State Sync"
        description="Demonstrates reading and modifying URL query strings (e.g. ?category=electronics&sort=asc) without triggering full page reloads."
        codeSnippet="const [searchParams, setSearchParams] = useSearchParams(); const category = searchParams.get('category');"
      />

      <div className="filter-bar">
        <div className="filter-group">
          <Filter size={18} className="filter-label" />
          <label className="filter-label">Category:</label>
          <select value={currentCategory} onChange={handleCategoryChange} className="select-input">
            <option value="all">All Categories</option>
            <option value="electronics">Electronics</option>
            <option value="books">Books</option>
            <option value="clothing">Clothing</option>
          </select>
        </div>

        <div className="filter-group">
          <ArrowUpDown size={18} className="filter-label" />
          <label className="filter-label">Sort by Price:</label>
          <select value={currentSort} onChange={handleSortChange} className="select-input">
            <option value="asc">Low to High ($)</option>
            <option value="desc">High to Low ($)</option>
          </select>
        </div>

        <div style={{ marginLeft: 'auto', fontSize: '0.85rem', color: 'var(--accent-secondary)' }}>
          Active Query: <code>?{searchParams.toString() || 'none'}</code>
        </div>
      </div>

      <div className="grid-cards">
        {filteredProducts.map((prod) => (
          <div className="card" key={prod.id}>
            <span className="concept-tag">{prod.category}</span>
            <h3 className="card-title" style={{ marginTop: '0.5rem' }}>{prod.name}</h3>
            <p style={{ color: 'var(--accent-success)', fontWeight: 700, fontSize: '1.2rem', marginBottom: '1rem' }}>
              ${prod.price}
            </p>
            <Link to={`/products/${prod.id}`} className="btn btn-secondary" style={{ width: '100%', justifyContent: 'center' }}>
              View Detail (useParams) <ExternalLink size={16} />
            </Link>
          </div>
        ))}
      </div>
    </div>
  );
}
