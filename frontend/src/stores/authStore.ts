import { writable } from 'svelte/store';
import { browser } from '$app/environment';

interface AuthState {
  token: string | null;
  userId: string | null;
  email: string | null;
  firstName: string | null;
  lastName: string | null;
  roles: string[];
  expiration: Date | null;
}

const initialState: AuthState = {
  token: null,
  userId: null,
  email: null,
  firstName: null,
  lastName: null,
  roles: [],
  expiration: null,
};

function createAuthStore() {
  const { subscribe, set, update } = writable<AuthState>(initialState);

  // Load auth state from localStorage on initialization
  if (browser) {
    const stored = localStorage.getItem('auth');
    if (stored) {
      try {
        const parsed = JSON.parse(stored);
        if (parsed.expiration && new Date(parsed.expiration) > new Date()) {
          set({
            ...parsed,
            expiration: new Date(parsed.expiration),
          });
        } else {
          // Token expired, clear storage
          localStorage.removeItem('auth');
        }
      } catch (error) {
        console.error('Error parsing stored auth:', error);
        localStorage.removeItem('auth');
      }
    }
  }

  return {
    subscribe,
    login: (authResponse: any) => {
      const authState: AuthState = {
        token: authResponse.token,
        userId: authResponse.userId,
        email: authResponse.email,
        firstName: authResponse.firstName,
        lastName: authResponse.lastName,
        roles: authResponse.roles || [],
        expiration: new Date(authResponse.expiration),
      };

      set(authState);

      if (browser) {
        localStorage.setItem('auth', JSON.stringify(authState));
      }
    },
    logout: () => {
      set(initialState);
      if (browser) {
        localStorage.removeItem('auth');
      }
    },
    updateProfile: (profile: Partial<AuthState>) => {
      update(state => {
        const newState = { ...state, ...profile };
        if (browser) {
          localStorage.setItem('auth', JSON.stringify(newState));
        }
        return newState;
      });
    },
  };
}

export const authStore = createAuthStore();