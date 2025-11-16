/**
 * Campaigns API Endpoints
 */

import apiClient from "../client";
import type {
	CampaignDto,
	CreateCampaignRequest,
	CampaignDeliverableDto,
	SubmitDeliverableRequest,
	ReviewDeliverableRequest,
} from "../types";

export const campaignsApi = {
	/**
	 * Get all campaigns for authenticated brand
	 */
	getBrandCampaigns: async (): Promise<CampaignDto[]> => {
		return apiClient.get<CampaignDto[]>("/api/campaign/brand");
	},

	/**
	 * Get all campaigns for authenticated influencer
	 */
	getInfluencerCampaigns: async (): Promise<CampaignDto[]> => {
		return apiClient.get<CampaignDto[]>("/api/campaign/influencer");
	},

	/**
	 * Get available campaigns (for influencers to apply)
	 */
	getAvailableCampaigns: async (): Promise<CampaignDto[]> => {
		return apiClient.get<CampaignDto[]>("/api/campaign/available");
	},

	/**
	 * Get campaign by ID
	 */
	getCampaign: async (id: string): Promise<CampaignDto> => {
		return apiClient.get<CampaignDto>(`/api/campaign/${id}`);
	},

	/**
	 * Create new campaign
	 */
	createCampaign: async (data: CreateCampaignRequest): Promise<CampaignDto> => {
		return apiClient.post<CampaignDto>("/api/campaign", data);
	},

	/**
	 * Update campaign
	 */
	updateCampaign: async (
		id: string,
		data: CreateCampaignRequest,
	): Promise<CampaignDto> => {
		return apiClient.put<CampaignDto>(`/api/campaign/${id}`, data);
	},

	/**
	 * Delete campaign
	 */
	deleteCampaign: async (id: string): Promise<void> => {
		return apiClient.delete(`/api/campaign/${id}`);
	},

	/**
	 * Apply to campaign (influencer)
	 */
	applyToCampaign: async (id: string): Promise<void> => {
		return apiClient.post(`/api/campaign/${id}/apply`);
	},

	/**
	 * Update campaign status
	 */
	updateCampaignStatus: async (id: string, status: string): Promise<void> => {
		return apiClient.put(`/api/campaign/${id}/status`, { status });
	},

	/**
	 * Search campaigns with filters
	 */
	searchCampaigns: async (filters: {
		status?: string;
		minBudget?: number;
		maxBudget?: number;
		startDate?: string;
		endDate?: string;
		platform?: string;
		niche?: string;
	}): Promise<CampaignDto[]> => {
		const params = new URLSearchParams();
		Object.entries(filters).forEach(([key, value]) => {
			if (value !== undefined && value !== null) {
				params.append(key, value.toString());
			}
		});
		return apiClient.get<CampaignDto[]>(
			`/api/campaign/search?${params.toString()}`,
		);
	},

	/**
	 * Get deliverables for a campaign
	 */
	getCampaignDeliverables: async (
		campaignId: string,
	): Promise<CampaignDeliverableDto[]> => {
		return apiClient.get<CampaignDeliverableDto[]>(
			`/api/campaigns/${campaignId}/deliverables`,
		);
	},

	/**
	 * Get deliverable by ID
	 */
	getDeliverable: async (id: string): Promise<CampaignDeliverableDto> => {
		return apiClient.get<CampaignDeliverableDto>(`/api/deliverables/${id}`);
	},

	/**
	 * Create deliverable
	 */
	createDeliverable: async (
		campaignId: string,
		data: {
			title: string;
			description: string;
			deliverableType: string;
			dueDate: string;
		},
	): Promise<CampaignDeliverableDto> => {
		return apiClient.post<CampaignDeliverableDto>(
			`/api/campaigns/${campaignId}/deliverables`,
			data,
		);
	},

	/**
	 * Update deliverable
	 */
	updateDeliverable: async (
		id: string,
		data: {
			title: string;
			description: string;
			deliverableType: string;
			dueDate: string;
		},
	): Promise<CampaignDeliverableDto> => {
		return apiClient.put<CampaignDeliverableDto>(
			`/api/deliverables/${id}`,
			data,
		);
	},

	/**
	 * Submit deliverable (influencer)
	 */
	submitDeliverable: async (
		id: string,
		data: SubmitDeliverableRequest,
	): Promise<void> => {
		return apiClient.post(`/api/deliverables/${id}/submit`, data);
	},

	/**
	 * Approve deliverable (brand)
	 */
	approveDeliverable: async (
		id: string,
		data: ReviewDeliverableRequest,
	): Promise<void> => {
		return apiClient.post(`/api/deliverables/${id}/approve`, data);
	},

	/**
	 * Reject deliverable (brand)
	 */
	rejectDeliverable: async (
		id: string,
		data: ReviewDeliverableRequest,
	): Promise<void> => {
		return apiClient.post(`/api/deliverables/${id}/reject`, data);
	},
};
