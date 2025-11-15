/**
 * Notifications API Endpoints
 */

import apiClient from "../client";
import type { NotificationDto } from "../types";

export const notificationsApi = {
	/**
	 * Get all notifications for current user
	 */
	getNotifications: async (): Promise<NotificationDto[]> => {
		return apiClient.get<NotificationDto[]>("/api/notifications");
	},

	/**
	 * Get unread notifications count
	 */
	getUnreadCount: async (): Promise<number> => {
		const response = await apiClient.get<{ count: number }>(
			"/api/notifications/unread",
		);
		return response.count;
	},

	/**
	 * Mark notification as read
	 */
	markAsRead: async (id: string): Promise<void> => {
		return apiClient.put(`/api/notifications/${id}/read`);
	},

	/**
	 * Mark all notifications as read
	 */
	markAllAsRead: async (): Promise<void> => {
		return apiClient.put("/api/notifications/read-all");
	},

	/**
	 * Delete notification
	 */
	deleteNotification: async (id: string): Promise<void> => {
		return apiClient.delete(`/api/notifications/${id}`);
	},
};
