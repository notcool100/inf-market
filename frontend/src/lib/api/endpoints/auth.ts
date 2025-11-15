/**
 * Authentication API Endpoints
 */

import apiClient from "../client";
import type { LoginRequest, RegisterRequest, AuthResponse } from "../types";

export const authApi = {
	/**
	 * Login user
	 */
	login: async (credentials: LoginRequest): Promise<AuthResponse> => {
		return apiClient.post<AuthResponse>("/api/auth/login", credentials);
	},

	/**
	 * Register new user
	 */
	register: async (data: RegisterRequest): Promise<AuthResponse> => {
		return apiClient.post<AuthResponse>("/api/auth/register", data);
	},

	/**
	 * Logout user (client-side only, token removed by interceptor)
	 */
	logout: async (): Promise<void> => {
		// Token removal is handled by authStore
		return Promise.resolve();
	},
};
