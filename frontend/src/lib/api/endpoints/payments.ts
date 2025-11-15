/**
 * Payments API Endpoints
 */

import apiClient from "../client";
import type { PaymentDto } from "../types";

export const paymentsApi = {
	/**
	 * Get sent payments
	 */
	getSentPayments: async (): Promise<PaymentDto[]> => {
		return apiClient.get<PaymentDto[]>("/api/payment/sent");
	},

	/**
	 * Get received payments
	 */
	getReceivedPayments: async (): Promise<PaymentDto[]> => {
		return apiClient.get<PaymentDto[]>("/api/payment/received");
	},

	/**
	 * Get payment by ID
	 */
	getPayment: async (id: string): Promise<PaymentDto> => {
		return apiClient.get<PaymentDto>(`/api/payment/${id}`);
	},

	/**
	 * Create payment
	 */
	createPayment: async (data: {
		campaignId?: string;
		recipientId: string;
		amount: number;
		paymentMethod: string;
		notes?: string;
	}): Promise<PaymentDto> => {
		return apiClient.post<PaymentDto>("/api/payment", data);
	},

	/**
	 * Release payment from escrow
	 */
	releasePayment: async (id: string): Promise<PaymentDto> => {
		return apiClient.post<PaymentDto>(`/api/payment/${id}/release`);
	},
};
