import { useState, useEffect } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import { fetchService } from '../api/fetchService';

// Validation Schema using Yup
const ServiceSchema = Yup.object({
  serviceName: Yup.string()
    .trim()
    .min(2, 'Service name must be at least 2 characters.')
    .max(100, 'Service name cannot exceed 100 characters.')
    .required('Service name is required.'),
  servicePrice: Yup.number()
    .typeError('Price must be a valid number.')
    .min(1, 'Price must be greater than 0.')
    .required('Service price is required.'),
  serviceDuration: Yup.number()
    .typeError('Duration must be a valid number.')
    .min(1, 'Duration must be at least 1 minute.')
    .required('Service duration is required.'),
});

export const ServiceFormPage = () => {
  const { id } = useParams<{ id?: string }>();
  const isEditMode = Boolean(id);
  const navigate = useNavigate();

  const [loading, setLoading] = useState<boolean>(false);
  const [apiError, setApiError] = useState<string | null>(null);

  const formik = useFormik({
    initialValues: {
      serviceID: isEditMode && id ? Number(id) : 0,
      serviceName: '',
      servicePrice: 100,
      serviceDuration: 30,
    },
    validationSchema: ServiceSchema,
    enableReinitialize: true,
    onSubmit: async (values) => {
      setLoading(true);
      setApiError(null);

      try {
        await fetchService.addEditService({
          serviceID: isEditMode && id ? Number(id) : 0,
          serviceName: values.serviceName,
          servicePrice: Number(values.servicePrice),
          serviceDuration: Number(values.serviceDuration),
        });

        // Navigate back to services listing
        navigate('/service');
      } catch (err: any) {
        setApiError(err.message || 'Failed to save service.');
      } finally {
        setLoading(false);
      }
    },
  });

  // Pre-fill form when in Edit Mode
  useEffect(() => {
    if (isEditMode && id) {
      const loadServiceData = async () => {
        setLoading(true);
        try {
          const res = await fetchService.getServiceById(id);
          if (res.data) {
            formik.setValues({
              serviceID: res.data.serviceID,
              serviceName: res.data.serviceName,
              servicePrice: res.data.servicePrice,
              serviceDuration: res.data.serviceDuration,
            });
          }
        } catch (err: any) {
          setApiError(err.message || 'Failed to load service data for editing.');
        } finally {
          setLoading(false);
        }
      };
      loadServiceData();
    }
  }, [id, isEditMode]);

  return (
    <div className="page-container small">
      <div className="page-header">
        <h2>{isEditMode ? `Edit Service #${id}` : 'Create New Service'}</h2>
        <Link to="/service" className="btn secondary">
          Cancel
        </Link>
      </div>

      <p className="page-subtitle">
        URL Route: <code>{isEditMode ? `/service/edit/${id}` : '/service/new'}</code> • Powered by <strong>Formik & Yup</strong>
      </p>

      {apiError && <div className="banner error">{apiError}</div>}

      <form onSubmit={formik.handleSubmit} noValidate className="form-card">
        {/* Service Name Input */}
        <div className="form-group">
          <label htmlFor="serviceName">
            Service Name <span className="required-star">*</span>
          </label>
          <input
            id="serviceName"
            name="serviceName"
            type="text"
            placeholder="e.g. Hair Spa, Facial, Massage..."
            value={formik.values.serviceName}
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            disabled={loading}
            className={formik.touched.serviceName && formik.errors.serviceName ? 'input-error' : ''}
          />
          {formik.touched.serviceName && formik.errors.serviceName && (
            <span className="error-text">{formik.errors.serviceName}</span>
          )}
        </div>

        {/* Service Price Input */}
        <div className="form-group">
          <label htmlFor="servicePrice">
            Service Price (₹) <span className="required-star">*</span>
          </label>
          <input
            id="servicePrice"
            name="servicePrice"
            type="number"
            placeholder="e.g. 500"
            value={formik.values.servicePrice}
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            disabled={loading}
            className={formik.touched.servicePrice && formik.errors.servicePrice ? 'input-error' : ''}
          />
          {formik.touched.servicePrice && formik.errors.servicePrice && (
            <span className="error-text">{formik.errors.servicePrice}</span>
          )}
        </div>

        {/* Service Duration Input */}
        <div className="form-group">
          <label htmlFor="serviceDuration">
            Service Duration (Minutes) <span className="required-star">*</span>
          </label>
          <input
            id="serviceDuration"
            name="serviceDuration"
            type="number"
            placeholder="e.g. 60"
            value={formik.values.serviceDuration}
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            disabled={loading}
            className={formik.touched.serviceDuration && formik.errors.serviceDuration ? 'input-error' : ''}
          />
          {formik.touched.serviceDuration && formik.errors.serviceDuration && (
            <span className="error-text">{formik.errors.serviceDuration}</span>
          )}
        </div>

        <div className="form-actions">
          <button type="submit" className="btn primary" disabled={loading || formik.isSubmitting}>
            {loading
              ? 'Saving Service...'
              : isEditMode
              ? 'Update Service (AddEditService)'
              : 'Save Service (AddEditService)'}
          </button>
        </div>
      </form>
    </div>
  );
};
