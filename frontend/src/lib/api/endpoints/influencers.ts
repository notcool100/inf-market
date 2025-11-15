/**
 * Influencers API Endpoints
 */

import apiClient from "../client";
import type {
	InfluencerProfileDto,
	CreateReviewRequest,
	ReviewDto,
} from "../types";

export const influencersApi = {
	/**
	 * Get all influencers (search)
	 */
	search: async (filters?: {
		niche?: string;
		location?: string;
		minFollowers?: number;
		maxFollowers?: number;
		minRate?: number;
		maxRate?: number;
	}): Promise<InfluencerProfileDto[]> => {
		const params = new URLSearchParams();
		if (filters) {
			Object.entries(filters).forEach(([key, value]) => {
				if (value !== undefined && value !== null) {
					params.append(key, value.toString());
				}
			});
		}
		const queryString = params.toString();
		return apiClient.get<InfluencerProfileDto[]>(
			`/api/influencerprofile/search${queryString ? `?${queryString}` : ""}`,
		);
	},

	/**
	 * Get influencer profile by ID
	 */
	getProfile: async (id: string): Promise<InfluencerProfileDto> => {
		return apiClient.get<InfluencerProfileDto>(`/api/influencerprofile/${id}`);
	},

	/**
	 * Get current user's influencer profile
	 */
	getMyProfile: async (): Promise<InfluencerProfileDto> => {
		return apiClient.get<InfluencerProfileDto>("/api/influencerprofile/me");
	},

	/**
	 * Create influencer profile
	 */
	createProfile: async (data: {
		bio: string;
		nicheFocus: string;
		followersCount: number;
		instagramHandle?: string;
		tiktokHandle?: string;
		youtubeChannel?: string;
		facebookPage?: string;
		linkedInProfile?: string;
		websiteUrl?: string;
		minCampaignRate: number;
		contentTypes: string[];
		demographics: Record<string, any>;
		location: string;
	}): Promise<InfluencerProfileDto> => {
		return apiClient.post<InfluencerProfileDto>("/api/influencerprofile", data);
	},

	/**
	 * Update influencer profile
	 */
	updateProfile: async (
		id: string,
		data: {
			bio?: string;
			nicheFocus?: string;
			followersCount?: number;
			instagramHandle?: string;
			tiktokHandle?: string;
			youtubeChannel?: string;
			facebookPage?: string;
			linkedInProfile?: string;
			websiteUrl?: string;
			minCampaignRate?: number;
			contentTypes?: string[];
			demographics?: Record<string, any>;
			location?: string;
		},
	): Promise<InfluencerProfileDto> => {
		return apiClient.put<InfluencerProfileDto>(
			`/api/influencerprofile/${id}`,
			data,
		);
	},

	/**
	 * Get reviews for influencer
	 */
	getReviews: async (influencerId: string): Promise<ReviewDto[]> => {
		return apiClient.get<ReviewDto[]>(
			`/api/influencers/${influencerId}/reviews`,
		);
	},

	/**
	 * Get review for campaign
	 */
	getCampaignReview: async (campaignId: string): Promise<ReviewDto | null> => {
		try {
			return await apiClient.get<ReviewDto>(
				`/api/campaigns/${campaignId}/review`,
			);
		} catch {
			return null;
		}
	},

	/**
	 * Create review
	 */
	createReview: async (
		campaignId: string,
		data: CreateReviewRequest,
	): Promise<ReviewDto> => {
		return apiClient.post<ReviewDto>(
			`/api/campaigns/${campaignId}/review`,
			data,
		);
	},

	/**
	 * Update review
	 */
	updateReview: async (
		id: string,
		data: CreateReviewRequest,
	): Promise<ReviewDto> => {
		return apiClient.put<ReviewDto>(`/api/reviews/${id}`, data);
	},

	/**
	 * Delete review
	 */
	deleteReview: async (id: string): Promise<void> => {
		return apiClient.delete(`/api/reviews/${id}`);
	},
};
