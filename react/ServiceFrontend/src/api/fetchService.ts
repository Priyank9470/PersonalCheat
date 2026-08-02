import type {
  ApiResponse,
  LoginRequest,
  LoginResponseData,
  ServiceItem,
  AddEditServiceRequest,
} from '../types/service';

const BASE_URL = import.meta.env.VITE_API_BASE_URL;

// Helper to get Authorization headers
const getHeaders = (token?: string, includeContentType = true): HeadersInit => {
  const headers: Record<string, string> = {
    accept: '*/*',
  };

  if (includeContentType) {
    headers['Content-Type'] = 'application/json';
  }

  const authToken = token || localStorage.getItem('authToken');
  if (authToken) {
    headers['Authorization'] = `Bearer ${authToken}`;
  }

  return headers;
};

/**
 * Fetch API Service Layer for Service Management Application
 */
export const fetchService = {
  /**
   * 1. Login API Call
   * POST /api/Auth/login
   */
  async login(credentials: LoginRequest): Promise<ApiResponse<LoginResponseData>> {
    const response = await fetch(`${BASE_URL}/Auth/login`, {
      method: 'POST',
      headers: getHeaders(undefined, true),
      body: JSON.stringify(credentials),
    });

    const result: ApiResponse<LoginResponseData> = await response.json();
    
    if (!response.ok || !result.isSuccess) {
      throw new Error(result.message || `Login failed with status ${response.status}`);
    }

    return result;
  },

  /**
   * 2. GetAllServices API Call
   * GET /api/Service/GetAllServices?searchText=...&pageNumber=...&pageSize=...
   */
  async getAllServices(
    searchText = '',
    pageNumber = 1,
    pageSize = 10,
    token?: string
  ): Promise<ApiResponse<ServiceItem[]>> {
    const queryParams = new URLSearchParams({
      searchText,
      pageNumber: pageNumber.toString(),
      pageSize: pageSize.toString(),
    });

    const response = await fetch(`${BASE_URL}/Service/GetAllServices?${queryParams.toString()}`, {
      method: 'GET',
      headers: getHeaders(token, false),
    });

    const result: ApiResponse<ServiceItem[]> = await response.json();

    if (!response.ok || !result.isSuccess) {
      throw new Error(result.message || `Failed to retrieve services (Status ${response.status})`);
    }

    return result;
  },

  /**
   * 3. GetServiceById API Call
   * GET /api/Service/GetServiceById?id=...
   */
  async getServiceById(id: number | string, token?: string): Promise<ApiResponse<ServiceItem>> {
    const response = await fetch(`${BASE_URL}/Service/GetServiceById?id=${id}`, {
      method: 'GET',
      headers: getHeaders(token, false),
    });

    const result: ApiResponse<ServiceItem> = await response.json();

    if (!response.ok || !result.isSuccess) {
      throw new Error(result.message || `Failed to retrieve service #${id} (Status ${response.status})`);
    }

    return result;
  },

  /**
   * 4. AddEditService API Call
   * POST /api/Service/AddEditService
   */
  async addEditService(
    payload: AddEditServiceRequest,
    token?: string
  ): Promise<ApiResponse<number>> {
    const response = await fetch(`${BASE_URL}/Service/AddEditService`, {
      method: 'POST',
      headers: getHeaders(token, true),
      body: JSON.stringify(payload),
    });

    const result: ApiResponse<number> = await response.json();

    if (!response.ok || !result.isSuccess) {
      throw new Error(result.message || `Failed to save service (Status ${response.status})`);
    }

    return result;
  },

  /**
   * 5. DeleteService API Call
   * DELETE /api/Service/DeleteService?id=...
   */
  async deleteService(id: number | string, token?: string): Promise<ApiResponse<boolean>> {
    const response = await fetch(`${BASE_URL}/Service/DeleteService?id=${id}`, {
      method: 'DELETE',
      headers: getHeaders(token, false),
    });

    const result: ApiResponse<boolean> = await response.json();

    if (!response.ok || !result.isSuccess) {
      throw new Error(result.message || `Failed to delete service #${id} (Status ${response.status})`);
    }

    return result;
  },
};
