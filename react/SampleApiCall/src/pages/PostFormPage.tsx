import { useState, useEffect, type FormEvent } from 'react';
import { useParams, useNavigate, Link, useOutletContext } from 'react-router-dom';
import { fetchService } from '../api/fetchService';
import { axiosService } from '../api/axiosService';

interface LayoutContext {
  apiMode: 'fetch' | 'axios';
}

interface FormErrors {
  title?: string;
  body?: string;
}

interface FormTouched {
  title?: boolean;
  body?: boolean;
}

export const PostFormPage = () => {
  const { id } = useParams<{ id?: string }>();
  const isEditMode = Boolean(id);
  const navigate = useNavigate();
  const { apiMode } = useOutletContext<LayoutContext>();

  const [title, setTitle] = useState('');
  const [body, setBody] = useState('');
  const [errors, setErrors] = useState<FormErrors>({});
  const [touched, setTouched] = useState<FormTouched>({});
  const [loading, setLoading] = useState<boolean>(false);
  const [apiError, setApiError] = useState<string | null>(null);

  const getService = () => (apiMode === 'fetch' ? fetchService : axiosService);

  // Custom Validation Logic
  const validateForm = (currentTitle: string, currentBody: string): FormErrors => {
    const newErrors: FormErrors = {};

    // 1. Title Validation Rules
    if (!currentTitle.trim()) {
      newErrors.title = 'Post title is required.';
    } else if (currentTitle.trim().length < 5) {
      newErrors.title = 'Title must be at least 5 characters long.';
    } else if (currentTitle.trim().length > 100) {
      newErrors.title = 'Title cannot exceed 100 characters.';
    }

    // 2. Body Validation Rules
    if (!currentBody.trim()) {
      newErrors.body = 'Post body content is required.';
    } else if (currentBody.trim().length < 10) {
      newErrors.body = 'Body content must be at least 10 characters long.';
    } else if (currentBody.trim().length > 1000) {
      newErrors.body = 'Body content cannot exceed 1000 characters.';
    }

    return newErrors;
  };

  useEffect(() => {
    if (isEditMode && id) {
      const loadExistingPost = async () => {
        setLoading(true);
        try {
          const post = await getService().getPostById(id);
          setTitle(post.title);
          setBody(post.body);
        } catch (err: any) {
          setApiError(err.message || 'Failed to load post data for editing');
        } finally {
          setLoading(false);
        }
      };
      loadExistingPost();
    }
  }, [id, isEditMode, apiMode]);

  // Handle Field Blur (on focus out)
  const handleBlur = (field: 'title' | 'body') => {
    setTouched((prev) => ({ ...prev, [field]: true }));
    const validationErrors = validateForm(title, body);
    setErrors(validationErrors);
  };

  // Real-time Title Change Handler
  const handleTitleChange = (val: string) => {
    setTitle(val);
    if (touched.title) {
      const validationErrors = validateForm(val, body);
      setErrors((prev) => ({ ...prev, title: validationErrors.title }));
    }
  };

  // Real-time Body Change Handler
  const handleBodyChange = (val: string) => {
    setBody(val);
    if (touched.body) {
      const validationErrors = validateForm(title, val);
      setErrors((prev) => ({ ...prev, body: validationErrors.body }));
    }
  };

  // Submit Handler with Full Validation Guard
  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();

    // Mark all fields as touched to trigger inline error messages
    setTouched({ title: true, body: true });

    // Perform validation
    const validationErrors = validateForm(title, body);
    setErrors(validationErrors);

    // If validation fails, stop form submission
    if (Object.keys(validationErrors).length > 0) {
      return;
    }

    setLoading(true);
    setApiError(null);

    try {
      if (isEditMode && id) {
        // PUT API Call
        await getService().updatePost(id, { title, body });
      } else {
        // POST API Call
        await getService().createPost({ title, body, userId: 1 });
      }

      // Navigate back to Listing screen
      navigate('/');
    } catch (err: any) {
      setApiError(err.message || 'Failed to save post.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="page-container small">
      <div className="page-header">
        <h2>{isEditMode ? `Edit Post #${id} (PUT API)` : 'Create New Post (POST API)'}</h2>
        <Link to="/" className="btn secondary">
          Cancel
        </Link>
      </div>

      <p className="page-subtitle">
        Submitting via <strong>{apiMode.toUpperCase()}</strong> engine with Custom Form Validation.
      </p>

      {apiError && <div className="banner error">{apiError}</div>}

      <form onSubmit={handleSubmit} noValidate className="form-card">
        {/* Title Input Group */}
        <div className="form-group">
          <label htmlFor="title">
            Post Title <span className="required-star">*</span>
          </label>
          <input
            id="title"
            type="text"
            placeholder="Enter post title (min 5 characters)..."
            value={title}
            onChange={(e) => handleTitleChange(e.target.value)}
            onBlur={() => handleBlur('title')}
            disabled={loading}
            className={touched.title && errors.title ? 'input-error' : ''}
          />
          {touched.title && errors.title && (
            <span className="error-text">{errors.title}</span>
          )}
        </div>

        {/* Body Textarea Group */}
        <div className="form-group">
          <label htmlFor="body">
            Post Body / Content <span className="required-star">*</span>
          </label>
          <textarea
            id="body"
            rows={5}
            placeholder="Enter post content (min 10 characters)..."
            value={body}
            onChange={(e) => handleBodyChange(e.target.value)}
            onBlur={() => handleBlur('body')}
            disabled={loading}
            className={touched.body && errors.body ? 'input-error' : ''}
          />
          {touched.body && errors.body && (
            <span className="error-text">{errors.body}</span>
          )}
        </div>

        <div className="form-actions">
          <button type="submit" className="btn primary" disabled={loading}>
            {loading
              ? 'Saving...'
              : isEditMode
              ? 'Save Changes (PUT)'
              : 'Create Post (POST)'}
          </button>
        </div>
      </form>
    </div>
  );
};
