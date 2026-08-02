import axios from 'axios';

// Read API Base URL from Vite environment variable
const BASE_URL = import.meta.env.VITE_API_BASE_URL;

// Create centralized Axios instance
const axiosInstance = axios.create({
  baseURL: BASE_URL,
  timeout: 10000, // 10 second timeout
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request Interceptor (e.g. attaching auth tokens, logging)
axiosInstance.interceptors.request.use(
  (config) => {
    // You can attach authorization tokens here in real applications:
    // config.headers.Authorization = `Bearer ${token}`;
    console.log(`[Axios Request] ${config.method?.toUpperCase()} -> ${config.url}`);
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response Interceptor (e.g. global error formatting, refresh tokens)
axiosInstance.interceptors.response.use(
  (response) => {
    console.log(`[Axios Response] ${response.status} <- ${response.config.url}`);
    return response;
  },
  (error) => {
    console.error('[Axios Error]', error.response?.status, error.message);
    return Promise.reject(error);
  }
);

export default axiosInstance;
