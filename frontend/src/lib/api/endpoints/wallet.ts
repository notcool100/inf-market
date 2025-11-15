/**
 * Wallet API Endpoints
 */

import apiClient from "../client";
import type { WalletDto, WalletTransactionDto } from "../types";

export const walletApi = {
	/**
	 * Get wallet for current user
	 */
	getWallet: async (): Promise<WalletDto> => {
		return apiClient.get<WalletDto>("/api/wallet");
	},

	/**
	 * Get wallet transactions
	 */
	getTransactions: async (): Promise<WalletTransactionDto[]> => {
		return apiClient.get<WalletTransactionDto[]>("/api/wallet/transactions");
	},

	/**
	 * Deposit funds
	 */
	deposit: async (
		amount: number,
		paymentMethod: string,
	): Promise<WalletDto> => {
		return apiClient.post<WalletDto>("/api/wallet/deposit", {
			amount,
			paymentMethod,
		});
	},

	/**
	 * Withdraw funds
	 */
	withdraw: async (
		amount: number,
		paymentMethod: string,
	): Promise<WalletDto> => {
		return apiClient.post<WalletDto>("/api/wallet/withdraw", {
			amount,
			paymentMethod,
		});
	},

	/**
	 * Transfer funds
	 */
	transfer: async (recipientId: string, amount: number): Promise<WalletDto> => {
		return apiClient.post<WalletDto>("/api/wallet/transfer", {
			recipientId,
			amount,
		});
	},
};
