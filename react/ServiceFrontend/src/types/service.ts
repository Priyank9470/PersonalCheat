// Standard API Response Wrapper from C# Backend
export interface ApiResponse<T> {
  isSuccess: boolean;
  statusCode: number;
  message: string;
  data: T;
  totalRecords?: number;
}

// Login Payload Types
export interface LoginRequest {
  userName: string;
  password: string;
}

export type LoginResponseData =
  | string
  | { authToken?: string; token?: string; accessToken?: string; userId?: number }
  | null;

// Service Data Model
export interface ServiceItem {
  serviceID: number;
  serviceNumber: string;
  serviceName: string;
  servicePrice: number;
  serviceDuration: number;
}

// Add/Edit Payload
export interface AddEditServiceRequest {
  serviceID: number;
  serviceName: string;
  servicePrice: number;
  serviceDuration: number;
}

// User Session Model
export interface UserSession {
  userName: string;
  token: string;
}
