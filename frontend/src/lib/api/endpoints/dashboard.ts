/**
 * Dashboard API Endpoints
 */

import apiClient from "../client";
import type { BrandDashboardStats, InfluencerDashboardStats } from "../types";

export const dashboardApi = {
	/**
	 * Get brand dashboard statistics
	 */
	getBrandStats: async (): Promise<BrandDashboardStats> => {
		return apiClient.get<BrandDashboardStats>("/api/brand/dashboard/stats");
	},

	/**
	 * Get influencer dashboard statistics
	 */
	getInfluencerStats: async (): Promise<InfluencerDashboardStats> => {
		return apiClient.get<InfluencerDashboardStats>(
			"/api/influencer/dashboard/stats",
		);
	},
};
