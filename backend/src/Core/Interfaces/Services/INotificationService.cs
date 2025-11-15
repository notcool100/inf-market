using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Shared.DTOs.Notification;

namespace InfluencerMarketplace.Core.Interfaces.Services
{
    public interface INotificationService
    {
        Task<NotificationDto> GetNotificationAsync(Guid id);
        Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(Guid userId);
        Task<int> GetUnreadCountAsync(Guid userId);
        Task<bool> MarkAsReadAsync(Guid id);
        Task<bool> MarkAllAsReadAsync(Guid userId);
        Task<bool> DeleteNotificationAsync(Guid id);
        Task<Guid> CreateNotificationAsync(
            Guid userId,
            string title,
            string message,
            string type,
            string relatedEntityType = null,
            Guid? relatedEntityId = null);
    }
}

