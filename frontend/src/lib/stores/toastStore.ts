/**
 * Toast Notification Store
 * Manages toast notifications throughout the application
 */

import { writable } from 'svelte/store';

export type ToastType = 'success' | 'error' | 'info' | 'warning';

export interface Toast {
  id: string;
  message: string;
  type: ToastType;
  duration?: number;
}

function createToastStore() {
  const { subscribe, update, set } = writable<Toast[]>([]);

  const addToast = (message: string, type: ToastType = 'info', duration: number = 5000) => {
    const id = Date.now().toString() + Math.random().toString(36).substr(2, 9);
    const toast: Toast = { id, message, type, duration };

    update((toasts) => [...toasts, toast]);

    // Auto remove after duration
    if (duration > 0) {
      setTimeout(() => {
        removeToast(id);
      }, duration);
    }

    return id;
  };

  const removeToast = (id: string) => {
    update((toasts) => toasts.filter((t) => t.id !== id));
  };

  const clear = () => {
    set([]);
  };

  // Convenience methods
  const success = (message: string, duration?: number) => addToast(message, 'success', duration);
  const error = (message: string, duration?: number) => addToast(message, 'error', duration);
  const info = (message: string, duration?: number) => addToast(message, 'info', duration);
  const warning = (message: string, duration?: number) => addToast(message, 'warning', duration);

  return {
    subscribe,
    add: addToast,
    remove: removeToast,
    clear,
    success,
    error,
    info,
    warning,
  };
}

export const toastStore = createToastStore();

