import { useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import { useFormik } from 'formik';
import * as Yup from 'yup';
import { useAuth } from '../context/AuthContext';

// Validation Schema using Yup
const LoginSchema = Yup.object({
  userName: Yup.string()
    .trim()
    .min(3, 'Username must be at least 3 characters.')
    .required('Username is required.'),
  password: Yup.string()
    .min(4, 'Password must be at least 4 characters.')
    .required('Password is required.'),
});

export const LoginPage = () => {
  const { login } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const [loading, setLoading] = useState<boolean>(false);
  const [errorMsg, setErrorMsg] = useState<string | null>(null);

  // Destination after login
  const from = (location.state as any)?.from?.pathname || '/service';

  const formik = useFormik({
    initialValues: {
      userName: '',
      password: '',
    },
    validationSchema: LoginSchema,
    onSubmit: async (values) => {
      setLoading(true);
      setErrorMsg(null);

      try {
        await login(values);
        navigate(from, { replace: true });
      } catch (err: any) {
        setErrorMsg(err.message || 'Invalid username or password.');
      } finally {
        setLoading(false);
      }
    },
  });

  return (
    <div className="login-wrapper">
      <div className="login-card">
        <div className="login-header">
          <h2>🔐 Admin Authentication</h2>
          <p>Login to manage services & pricing</p>
        </div>

        {errorMsg && <div className="banner error">{errorMsg}</div>}

        <form onSubmit={formik.handleSubmit} noValidate className="form-card">
          <div className="form-group">
            <label htmlFor="userName">Username</label>
            <input
              id="userName"
              name="userName"
              type="text"
              placeholder="Enter your username..."
              value={formik.values.userName}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              disabled={loading}
              className={formik.touched.userName && formik.errors.userName ? 'input-error' : ''}
            />
            {formik.touched.userName && formik.errors.userName && (
              <span className="error-text">{formik.errors.userName}</span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="password">Password</label>
            <input
              id="password"
              name="password"
              type="password"
              placeholder="Enter your password..."
              value={formik.values.password}
              onChange={formik.handleChange}
              onBlur={formik.handleBlur}
              disabled={loading}
              className={formik.touched.password && formik.errors.password ? 'input-error' : ''}
            />
            {formik.touched.password && formik.errors.password && (
              <span className="error-text">{formik.errors.password}</span>
            )}
          </div>

          <button type="submit" disabled={loading || formik.isSubmitting} className="btn primary block">
            {loading ? 'Authenticating...' : 'Login'}
          </button>
        </form>
      </div>
    </div>
  );
};
