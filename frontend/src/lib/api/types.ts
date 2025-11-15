/**
 * API Types and Interfaces
 * Centralized type definitions for API requests and responses
 */

// Common types
export interface ApiResponse<T = any> {
	success: boolean;
	data?: T;
	message?: string;
	errors?: string[];
	timestamp?: string;
}

export interface PaginatedResponse<T> {
	items: T[];
	total: number;
	page: number;
	pageSize: number;
	totalPages: number;
}

export interface ApiError {
	message: string;
	errors?: string[];
	statusCode?: number;
	timestamp?: string;
}

// Auth types
export interface LoginRequest {
	email: string;
	password: string;
}

export interface RegisterRequest {
	email: string;
	password: string;
	confirmPassword: string;
	firstName: string;
	lastName: string;
	phoneNumber: string;
	userType: "Brand" | "Influencer";
}

export interface AuthResponse {
	token: string;
	userId: string;
	email: string;
	firstName: string;
	lastName: string;
	roles: string[];
	expiration: string;
}

// Campaign types
export interface CampaignDto {
	id: string;
	title: string;
	description: string;
	brandId: string;
	influencerId?: string;
	budget: number;
	startDate: string;
	endDate: string;
	requirements: string;
	deliverables: string[];
	targetAudience: Record<string, any>;
	targetPlatforms: string[];
	status: string;
	createdAt: string;
	updatedAt: string;
}

export interface CreateCampaignRequest {
	title: string;
	description: string;
	budget: number;
	startDate: string;
	endDate: string;
	requirements: string;
	deliverables: string[];
	targetAudience?: Record<string, any>;
	targetPlatforms: string[];
}

// Influencer Profile types
export interface InfluencerProfileDto {
	id: string;
	userId: string;
	fullName: string;
	email: string;
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
	isVerified: boolean;
	averageRating: number;
	completedCampaigns: number;
	createdAt: string;
	updatedAt: string;
}

// Deliverable types
export interface CampaignDeliverableDto {
	id: string;
	campaignId: string;
	title: string;
	description: string;
	deliverableType: string;
	proofUrl?: string;
	screenshotUrl?: string;
	feedbackNotes?: string;
	status: string;
	dueDate: string;
	submittedAt?: string;
	reviewedAt?: string;
	createdAt: string;
	updatedAt: string;
}

export interface SubmitDeliverableRequest {
	proofUrl: string;
	screenshotUrl?: string;
}

export interface ReviewDeliverableRequest {
	status: string;
	feedbackNotes?: string;
}

// Review types
export interface ReviewDto {
	id: string;
	campaignId: string;
	reviewerId: string;
	influencerProfileId: string;
	rating: number;
	comment: string;
	isPublic: boolean;
	createdAt: string;
	updatedAt: string;
}

export interface CreateReviewRequest {
	rating: number;
	comment: string;
	isPublic: boolean;
}

// Notification types
export interface NotificationDto {
	id: string;
	userId: string;
	title: string;
	message: string;
	type: string;
	relatedEntityType?: string;
	relatedEntityId?: string;
	isRead: boolean;
	createdAt: string;
	readAt?: string;
}

// Payment types
export interface PaymentDto {
	id: string;
	campaignId?: string;
	senderId: string;
	recipientId: string;
	amount: number;
	commissionAmount: number;
	netAmount: number;
	currency: string;
	status: string;
	type: string;
	transactionReference?: string;
	paymentMethod?: string;
	notes?: string;
	createdAt: string;
	completedAt?: string;
}

// Wallet types
export interface WalletDto {
	id: string;
	userId: string;
	balance: number;
	currency: string;
	createdAt: string;
	updatedAt: string;
}

export interface WalletTransactionDto {
	id: string;
	walletId: string;
	type: string;
	amount: number;
	balance: number;
	description: string;
	relatedEntityId?: string;
	relatedEntityType?: string;
	createdAt: string;
}

// Dashboard types
export interface BrandDashboardStats {
	totalCampaigns: number;
	activeCampaigns: number;
	totalSpent: number;
	pendingPayments: number;
}

export interface InfluencerDashboardStats {
	totalCampaigns: number;
	activeCampaigns: number;
	totalEarnings: number;
	pendingPayments: number;
	averageRating: number;
	completedCampaigns: number;
}

// File Upload types
export interface FileUploadResult {
	success: boolean;
	fileUrl?: string;
	fileName?: string;
	errorMessage?: string;
}
