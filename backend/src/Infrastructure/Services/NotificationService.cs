using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Interfaces.Services;
using InfluencerMarketplace.Core.Models;
using InfluencerMarketplace.Shared.DTOs.Notification;

namespace InfluencerMarketplace.Infrastructure.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<NotificationDto> GetNotificationAsync(Guid id)
        {
            var notification = await _notificationRepository.GetByIdAsync(id);
            if (notification == null)
                return null;

            return MapToDto(notification);
        }

        public async Task<IEnumerable<NotificationDto>> GetNotificationsByUserIdAsync(Guid userId)
        {
            var notifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId);
            return notifications.Select(MapToDto);
        }

        public async Task<int> GetUnreadCountAsync(Guid userId)
        {
            return await _notificationRepository.GetUnreadCountAsync(userId);
        }

        public async Task<bool> MarkAsReadAsync(Guid id)
        {
            return await _notificationRepository.MarkAsReadAsync(id);
        }

        public async Task<bool> MarkAllAsReadAsync(Guid userId)
        {
            return await _notificationRepository.MarkAllAsReadAsync(userId);
        }

        public async Task<bool> DeleteNotificationAsync(Guid id)
        {
            return await _notificationRepository.DeleteAsync(id);
        }

        public async Task<Guid> CreateNotificationAsync(
            Guid userId,
            string title,
            string message,
            string type,
            string relatedEntityType = null,
            Guid? relatedEntityId = null)
        {
            if (!Enum.TryParse<NotificationType>(type, out var notificationType))
            {
                notificationType = NotificationType.SystemNotification;
            }

            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = notificationType,
                RelatedEntityType = relatedEntityType,
                RelatedEntityId = relatedEntityId
            };

            return await _notificationRepository.CreateAsync(notification);
        }

        private NotificationDto MapToDto(Notification notification)
        {
            return new NotificationDto
            {
                Id = notification.Id,
                UserId = notification.UserId,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type.ToString(),
                RelatedEntityType = notification.RelatedEntityType,
                RelatedEntityId = notification.RelatedEntityId,
                IsRead = notification.IsRead,
                CreatedAt = notification.CreatedAt,
                ReadAt = notification.ReadAt
            };
        }
    }
}

