import axios from 'axios';
import { authStore } from '../stores/authStore';
import { get } from 'svelte/store';

const API_BASE_URL = 'http://localhost:5192';

export const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Request interceptor to add auth token
apiClient.interceptors.request.use(
  (config) => {
    const auth = get(authStore);
    if (auth.token) {
      config.headers.Authorization = `Bearer ${auth.token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  }
);

// Response interceptor to handle auth errors
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      authStore.logout();
      window.location.href = '/login';
    }
    return Promise.reject(error);
  }
);

export default apiClient;