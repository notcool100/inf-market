/**
 * Environment configuration
 * Loads environment variables with type safety
 */

export const env = {
	api: {
		baseUrl: import.meta.env.VITE_API_BASE_URL || "http://localhost:5192",
		timeout: parseInt(import.meta.env.VITE_API_TIMEOUT || "30000", 10),
	},
	app: {
		name: import.meta.env.VITE_APP_NAME || "Influencer Marketplace",
		version: import.meta.env.VITE_APP_VERSION || "1.0.0",
	},
	features: {
		analytics: import.meta.env.VITE_ENABLE_ANALYTICS === "true",
		errorTracking: import.meta.env.VITE_ENABLE_ERROR_TRACKING === "true",
	},
} as const;

// Validate required environment variables
if (!env.api.baseUrl) {
	console.warn("VITE_API_BASE_URL is not set. Using default:", env.api.baseUrl);
}
