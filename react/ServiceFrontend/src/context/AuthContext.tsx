import { createContext, useContext, useState, useEffect, type ReactNode } from 'react';
import { fetchService } from '../api/fetchService';
import type { LoginRequest } from '../types/service';

interface AuthContextType {
  token: string | null;
  userName: string | null;
  isAuthenticated: boolean;
  login: (credentials: LoginRequest) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [token, setToken] = useState<string | null>(() => localStorage.getItem('authToken'));
  const [userName, setUserName] = useState<string | null>(() => localStorage.getItem('authUserName'));

  useEffect(() => {
    if (token) {
      localStorage.setItem('authToken', token);
    } else {
      localStorage.removeItem('authToken');
    }
  }, [token]);

  useEffect(() => {
    if (userName) {
      localStorage.setItem('authUserName', userName);
    } else {
      localStorage.removeItem('authUserName');
    }
  }, [userName]);

  const login = async (credentials: LoginRequest) => {
    const result = await fetchService.login(credentials);

    // Extract Bearer token from data payload (handles authToken, token, or accessToken string)
    let extractedToken = '';
    if (typeof result.data === 'string') {
      extractedToken = result.data;
    } else if (result.data && typeof result.data === 'object') {
      extractedToken = result.data.authToken || result.data.token || result.data.accessToken || '';
    }

    if (!extractedToken) {
      throw new Error('Authentication succeeded but no access token was returned by the server.');
    }

    setToken(extractedToken);
    setUserName(credentials.userName);
  };

  const logout = () => {
    setToken(null);
    setUserName(null);
    localStorage.removeItem('authToken');
    localStorage.removeItem('authUserName');
  };

  return (
    <AuthContext.Provider
      value={{
        token,
        userName,
        isAuthenticated: Boolean(token),
        login,
        logout,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = (): AuthContextType => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
