import { createContext, useContext, type ReactNode } from 'react';
import { ApiError } from '../lib/api';
import type { SessionUser } from '../types/user';

interface AuthState {
  user: SessionUser | null;
  isAuthenticated: boolean;
  isCheckingSession: boolean;
}

interface AuthContextType extends AuthState {
  login: (identificador: string, password: string) => Promise<void>;
  logout: () => Promise<void>;
  checkSession: () => Promise<void>;
  refreshUser: () => Promise<void>;
}

const GUEST_USER: SessionUser = { nombre: 'Invitado', email: '', rol: 'admin' };

const AuthContext = createContext<AuthContextType | null>(null);

// Portfolio demo: sin login, todos los usuarios tienen permisos de admin.
export function AuthProvider({ children }: { children: ReactNode }) {
  const noop = async () => {};

  return (
    <AuthContext.Provider
      value={{
        user: GUEST_USER,
        isAuthenticated: true,
        isCheckingSession: false,
        login: noop,
        logout: noop,
        checkSession: noop,
        refreshUser: noop,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}

export { ApiError };
