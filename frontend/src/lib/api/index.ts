/**
 * API Module
 * Centralized export for all API endpoints
 */

export { apiClient, default } from './client';
export * from './types';

// Export all API endpoints
export { authApi } from './endpoints/auth';
export { campaignsApi } from './endpoints/campaigns';
export { influencersApi } from './endpoints/influencers';
export { dashboardApi } from './endpoints/dashboard';
export { walletApi } from './endpoints/wallet';
export { paymentsApi } from './endpoints/payments';
export { notificationsApi } from './endpoints/notifications';
export { uploadApi } from './endpoints/upload';

