/**
 * API Client
 * Centralized HTTP client with interceptors and error handling
 */

import axios, { AxiosError } from "axios";
import type { AxiosInstance, InternalAxiosRequestConfig } from "axios";
import { env } from "../config/env";
import { authStore } from "../../stores/authStore";
import { get } from "svelte/store";
import type { ApiError, ApiResponse } from "./types";

class ApiClient {
	private client: AxiosInstance;

	constructor() {
		this.client = axios.create({
			baseURL: env.api.baseUrl,
			timeout: env.api.timeout,
			headers: {
				"Content-Type": "application/json",
			},
		});

		this.setupInterceptors();
	}

	private setupInterceptors(): void {
		// Request interceptor - Add auth token
		this.client.interceptors.request.use(
			(config: InternalAxiosRequestConfig) => {
				const auth = get(authStore);
				if (auth.token && config.headers) {
					config.headers.Authorization = `Bearer ${auth.token}`;
				}
				return config;
			},
			(error) => {
				return Promise.reject(this.handleError(error));
			},
		);

		// Response interceptor - Handle errors and auth
		this.client.interceptors.response.use(
			(response) => response,
			(error: AxiosError) => {
				// Handle 401 Unauthorized
				if (error.response?.status === 401) {
					authStore.logout();
					if (typeof window !== "undefined") {
						window.location.href = "/login";
					}
				}
				return Promise.reject(this.handleError(error));
			},
		);
	}

	private handleError(error: unknown): ApiError {
		if (axios.isAxiosError(error)) {
			const axiosError = error as AxiosError<ApiResponse>;
			return {
				message:
					axiosError.response?.data?.message ||
					axiosError.message ||
					"An error occurred",
				errors: axiosError.response?.data?.errors,
				statusCode: axiosError.response?.status,
				timestamp: axiosError.response?.data?.timestamp,
			};
		}

		return {
			message:
				error instanceof Error ? error.message : "An unknown error occurred",
		};
	}

	// HTTP Methods
	async get<T = any>(url: string, config?: any): Promise<T> {
		const response = await this.client.get<ApiResponse<T>>(url, config);
		return response.data.data ?? (response.data as unknown as T);
	}

	async post<T = any>(url: string, data?: any, config?: any): Promise<T> {
		const response = await this.client.post<ApiResponse<T>>(url, data, config);
		return response.data.data ?? (response.data as unknown as T);
	}

	async put<T = any>(url: string, data?: any, config?: any): Promise<T> {
		const response = await this.client.put<ApiResponse<T>>(url, data, config);
		return response.data.data ?? (response.data as unknown as T);
	}

	async patch<T = any>(url: string, data?: any, config?: any): Promise<T> {
		const response = await this.client.patch<ApiResponse<T>>(url, data, config);
		return response.data.data ?? (response.data as unknown as T);
	}

	async delete<T = any>(url: string, config?: any): Promise<T> {
		const response = await this.client.delete<ApiResponse<T>>(url, config);
		return response.data.data ?? (response.data as unknown as T);
	}

	// File upload helper
	async uploadFile(
		url: string,
		file: File,
		onProgress?: (progress: number) => void,
	): Promise<any> {
		const formData = new FormData();
		formData.append("file", file);

		const config = {
			headers: {
				"Content-Type": "multipart/form-data",
			},
			onUploadProgress: (progressEvent: any) => {
				if (onProgress && progressEvent.total) {
					const percentCompleted = Math.round(
						(progressEvent.loaded * 100) / progressEvent.total,
					);
					onProgress(percentCompleted);
				}
			},
		};

		return this.post(url, formData, config);
	}

	// Get raw axios instance for advanced use cases
	getInstance(): AxiosInstance {
		return this.client;
	}
}

// Export singleton instance
export const apiClient = new ApiClient();
export default apiClient;
