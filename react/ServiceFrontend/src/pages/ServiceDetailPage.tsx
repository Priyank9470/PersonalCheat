import { useState, useEffect } from 'react';
import { useParams, useLocation, Link } from 'react-router-dom';
import { fetchService } from '../api/fetchService';
import type { ServiceItem } from '../types/service';

export const ServiceDetailPage = () => {
  const { serviceNumber } = useParams<{ serviceNumber: string }>();
  const location = useLocation();

  // Retrieve passed service state if coming from listing page click
  const stateService = (location.state as any)?.service as ServiceItem | undefined;

  const [service, setService] = useState<ServiceItem | null>(stateService || null);
  const [loading, setLoading] = useState<boolean>(!stateService);
  const [errorMsg, setErrorMsg] = useState<string | null>(null);

  useEffect(() => {
    // If service was not passed in location state, fetch all services to find matching serviceNumber
    if (!stateService && serviceNumber) {
      const fetchDetail = async () => {
        setLoading(true);
        setErrorMsg(null);
        try {
          const res = await fetchService.getAllServices(serviceNumber, 1, 10);
          const found = res.data?.find((s) => s.serviceNumber === serviceNumber) || res.data?.[0];
          if (found) {
            setService(found);
          } else {
            setErrorMsg(`Service with number "${serviceNumber}" was not found.`);
          }
        } catch (err: any) {
          setErrorMsg(err.message || 'Failed to retrieve service details.');
        } finally {
          setLoading(false);
        }
      };
      fetchDetail();
    }
  }, [serviceNumber, stateService]);

  return (
    <div className="page-container small">
      <div className="page-header">
        <Link to="/service" className="btn secondary">
          ← Back to All Services
        </Link>
        <span className="badge primary">Dynamic URL Route</span>
      </div>

      <div className="url-banner">
        <span>Dynamic Detail URL Path: </span>
        <code>/service/{serviceNumber}</code>
      </div>

      {loading ? (
        <p className="loading-text">Loading service details...</p>
      ) : errorMsg ? (
        <div className="banner error">{errorMsg}</div>
      ) : service ? (
        <div className="detail-card">
          <div className="detail-header">
            <div>
              <h2>{service.serviceName}</h2>
              <span className="subtitle">Service Code: <code>{service.serviceNumber}</code></span>
            </div>
            <span className="badge primary">ID: #{service.serviceID}</span>
          </div>

          <div className="detail-grid">
            <div className="detail-item">
              <span className="detail-label">Service Price</span>
              <span className="detail-value price-text">₹{service.servicePrice}</span>
            </div>

            <div className="detail-item">
              <span className="detail-label">Duration</span>
              <span className="detail-value">{service.serviceDuration} Minutes</span>
            </div>
          </div>

          <div className="detail-actions">
            {/* Dynamic Edit Route URL: /service/edit/[id] */}
            <Link to={`/service/edit/${service.serviceID}`} className="btn warning">
              Edit Service (PUT/POST)
            </Link>
          </div>
        </div>
      ) : null}
    </div>
  );
};
