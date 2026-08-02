import { useEffect, useState } from 'react';
import { useParams, useNavigate, Link, useOutletContext } from 'react-router-dom';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import { fetchService } from '../api/fetchService';
import { axiosService } from '../api/axiosService';

interface LayoutContext {
  apiMode: 'fetch' | 'axios';
}

// 1. Define Validation Schema using Yup
const PostValidationSchema = Yup.object({
  title: Yup.string()
    .trim()
    .min(5, 'Title must be at least 5 characters long.')
    .max(100, 'Title cannot exceed 100 characters.')
    .required('Post title is required.'),
  body: Yup.string()
    .trim()
    .min(10, 'Body content must be at least 10 characters long.')
    .max(1000, 'Body content cannot exceed 1000 characters.')
    .required('Post body content is required.'),
});

export const PostFormikPage = () => {
  const { id } = useParams<{ id?: string }>();
  const isEditMode = Boolean(id);
  const navigate = useNavigate();
  const { apiMode } = useOutletContext<LayoutContext>();

  const [loading, setLoading] = useState<boolean>(false);
  const [apiError, setApiError] = useState<string | null>(null);

  const getService = () => (apiMode === 'fetch' ? fetchService : axiosService);

  // 2. Initialize Formik
  const formik = useFormik({
    initialValues: {
      title: '',
      body: '',
    },
    validationSchema: PostValidationSchema,
    onSubmit: async (values) => {
      setLoading(true);
      setApiError(null);

      try {
        if (isEditMode && id) {
          await getService().updatePost(id, values);
        } else {
          await getService().createPost({ ...values, userId: 1 });
        }
        navigate('/');
      } catch (err: any) {
        setApiError(err.message || 'Failed to save post.');
      } finally {
        setLoading(false);
      }
    },
  });

  // Pre-fill form in Edit Mode
  useEffect(() => {
    if (isEditMode && id) {
      const loadExistingPost = async () => {
        setLoading(true);
        try {
          const post = await getService().getPostById(id);
          formik.setValues({
            title: post.title,
            body: post.body,
          });
        } catch (err: any) {
          setApiError(err.message || 'Failed to load post data for editing');
        } finally {
          setLoading(false);
        }
      };
      loadExistingPost();
    }
  }, [id, isEditMode, apiMode]);

  return (
    <div className="page-container small">
      <div className="page-header">
        <h2>{isEditMode ? `Edit Post #${id} (Formik + Yup)` : 'Create New Post (Formik + Yup)'}</h2>
        <Link to="/" className="btn secondary">
          Cancel
        </Link>
      </div>

      <p className="page-subtitle">
        Submitting via <strong>{apiMode.toUpperCase()}</strong> engine using <strong>Formik + Yup Schema</strong>.
      </p>

      {apiError && <div className="banner error">{apiError}</div>}

      <form onSubmit={formik.handleSubmit} noValidate className="form-card">
        {/* Title Input Group */}
        <div className="form-group">
          <label htmlFor="title">
            Post Title <span className="required-star">*</span>
          </label>
          <input
            id="title"
            name="title"
            type="text"
            placeholder="Enter post title (min 5 characters)..."
            value={formik.values.title}
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            disabled={loading}
            className={formik.touched.title && formik.errors.title ? 'input-error' : ''}
          />
          {formik.touched.title && formik.errors.title && (
            <span className="error-text">{formik.errors.title}</span>
          )}
        </div>

        {/* Body Textarea Group */}
        <div className="form-group">
          <label htmlFor="body">
            Post Body / Content <span className="required-star">*</span>
          </label>
          <textarea
            id="body"
            name="body"
            rows={5}
            placeholder="Enter post content (min 10 characters)..."
            value={formik.values.body}
            onChange={formik.handleChange}
            onBlur={formik.handleBlur}
            disabled={loading}
            className={formik.touched.body && formik.errors.body ? 'input-error' : ''}
          />
          {formik.touched.body && formik.errors.body && (
            <span className="error-text">{formik.errors.body}</span>
          )}
        </div>

        <div className="form-actions">
          <button type="submit" className="btn primary" disabled={loading || formik.isSubmitting}>
            {loading ? 'Saving...' : isEditMode ? 'Save Changes (Yup)' : 'Create Post (Yup)'}
          </button>
        </div>
      </form>
    </div>
  );
};
