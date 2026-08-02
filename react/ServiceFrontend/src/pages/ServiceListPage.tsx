import { useState, useEffect, type FormEvent } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { fetchService } from '../api/fetchService';
import type { ServiceItem } from '../types/service';

export const ServiceListPage = () => {
  const navigate = useNavigate();

  const [services, setServices] = useState<ServiceItem[]>([]);
  const [totalRecords, setTotalRecords] = useState<number>(0);
  const [searchText, setSearchText] = useState<string>('');
  const [searchInput, setSearchInput] = useState<string>('');
  const [pageNumber, setPageNumber] = useState<number>(1);
  const pageSize = 10;

  const [loading, setLoading] = useState<boolean>(true);
  const [errorMsg, setErrorMsg] = useState<string | null>(null);
  const [successMsg, setSuccessMsg] = useState<string | null>(null);

  const notifySuccess = (msg: string) => {
    setSuccessMsg(msg);
    setTimeout(() => setSuccessMsg(null), 4000);
  };

  const loadServices = async () => {
    setLoading(true);
    setErrorMsg(null);
    try {
      const response = await fetchService.getAllServices(searchText, pageNumber, pageSize);
      if (response.isSuccess && response.data) {
        setServices(response.data);
        setTotalRecords(response.totalRecords || response.data.length);
      } else {
        setServices([]);
        setTotalRecords(0);
      }
    } catch (err: any) {
      setServices([]);
      setTotalRecords(0);
      setErrorMsg(err.message || 'Failed to load services.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadServices();
  }, [searchText, pageNumber]);

  const handleSearchSubmit = (e: FormEvent) => {
    e.preventDefault();
    setPageNumber(1);
    setSearchText(searchInput.trim());
  };

  const handleResetSearch = () => {
    setSearchInput('');
    setSearchText('');
    setPageNumber(1);
  };

  const handleDelete = async (id: number, name: string) => {
    if (!window.confirm(`Are you sure you want to delete service "${name}" (ID: #${id})?`)) {
      return;
    }

    setLoading(true);
    try {
      await fetchService.deleteService(id);
      notifySuccess(`Service "${name}" deleted successfully.`);
      loadServices();
    } catch (err: any) {
      setErrorMsg(err.message || 'Failed to delete service.');
      setLoading(false);
    }
  };

  const totalPages = Math.ceil(totalRecords / pageSize) || 1;

  return (
    <div className="page-container">
      <div className="page-header">
        <div>
          <h2>Services Directory</h2>
          <p className="page-subtitle">Manage salon and spa service listings</p>
        </div>
        <Link to="/service/new" className="btn primary">
          + Add New Service
        </Link>
      </div>

      {successMsg && <div className="banner success">{successMsg}</div>}
      {errorMsg && <div className="banner error">{errorMsg}</div>}

      {/* Search & Filter Toolbar */}
      <form onSubmit={handleSearchSubmit} className="search-toolbar">
        <input
          type="text"
          placeholder="Search by service name or number..."
          value={searchInput}
          onChange={(e) => setSearchInput(e.target.value)}
          className="search-input"
        />
        <button type="submit" className="btn primary-sm">
          Search
        </button>
        {searchText && (
          <button type="button" onClick={handleResetSearch} className="btn secondary-sm">
            Clear Search
          </button>
        )}
      </form>

      {/* Services Data Table / Empty State */}
      {loading ? (
        <p className="loading-text">Loading services catalog...</p>
      ) : services.length === 0 ? (
        <div className="empty-state">
          <div className="empty-icon">🔍</div>
          <h3 className="no-service-found-title">No Service Found</h3>
          <p className="empty-description">
            {searchText
              ? `No services matched your search term "${searchText}".`
              : 'No services are currently registered in the database.'}
          </p>
          <div className="empty-actions">
            {searchText && (
              <button type="button" onClick={handleResetSearch} className="btn secondary-sm">
                Clear Search
              </button>
            )}
            <Link to="/service/new" className="btn primary-sm">
              + Add New Service
            </Link>
          </div>
        </div>
      ) : (
        <div className="table-responsive">
          <table className="data-table">
            <thead>
              <tr>
                <th>ID</th>
                <th>Service Number</th>
                <th>Service Name</th>
                <th>Price (₹)</th>
                <th>Duration (mins)</th>
                <th>Actions</th>
              </tr>
            </thead>
            <tbody>
              {services.map((service) => (
                <tr key={service.serviceID}>
                  <td><strong>#{service.serviceID}</strong></td>
                  <td>
                    <code>{service.serviceNumber}</code>
                  </td>
                  <td>
                    <strong className="service-name-text">{service.serviceName}</strong>
                  </td>
                  <td className="price-text">₹{service.servicePrice}</td>
                  <td>{service.serviceDuration} mins</td>
                  <td className="actions-cell">
                    {/* Dynamic Detail Route URL: /service/[ServiceNumber] */}
                    <button
                      onClick={() =>
                        navigate(`/service/${service.serviceNumber}`, {
                          state: { service },
                        })
                      }
                      className="btn info-sm"
                    >
                      View
                    </button>

                    {/* Dynamic Edit Route URL: /service/edit/[id] */}
                    <Link to={`/service/edit/${service.serviceID}`} className="btn warning-sm">
                      Edit
                    </Link>

                    <button
                      onClick={() => handleDelete(service.serviceID, service.serviceName)}
                      className="btn danger-sm"
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Pagination Controls */}
      {totalRecords > pageSize && (
        <div className="pagination">
          <button
            onClick={() => setPageNumber((prev) => Math.max(prev - 1, 1))}
            disabled={pageNumber === 1 || loading}
            className="btn secondary-sm"
          >
            ← Previous
          </button>
          <span className="page-indicator">
            Page {pageNumber} of {totalPages} ({totalRecords} Total Records)
          </span>
          <button
            onClick={() => setPageNumber((prev) => Math.min(prev + 1, totalPages))}
            disabled={pageNumber >= totalPages || loading}
            className="btn secondary-sm"
          >
            Next →
          </button>
        </div>
      )}
    </div>
  );
};
