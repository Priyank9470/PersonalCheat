export interface Post {
  id?: number;
  title: string;
  body: string;
  userId?: number;
}

export interface ApiResponse<T> {
  data: T | null;
  error: string | null;
  loading: boolean;
}
