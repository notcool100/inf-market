using System;
using System.Security.Claims;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces.Services;
using InfluencerMarketplace.Shared.DTOs.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfluencerMarketplace.API.Controllers
{
    /// <summary>
    /// Notification management endpoints
    /// </summary>
    [ApiController]
    [Route("api/notifications")]
    [Authorize]
    [Produces("application/json")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Get all notifications for the authenticated user
        /// </summary>
        /// <returns>List of notifications</returns>
        /// <response code="200">Returns the list of notifications</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet]
        [ProducesResponseType(typeof(System.Collections.Generic.IEnumerable<NotificationDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<System.Collections.Generic.IEnumerable<NotificationDto>>> GetNotifications()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var notifications = await _notificationService.GetNotificationsByUserIdAsync(userId);
            return Ok(notifications);
        }

        /// <summary>
        /// Get unread notifications count for the authenticated user
        /// </summary>
        /// <returns>Unread count</returns>
        /// <response code="200">Returns the unread count</response>
        /// <response code="401">Unauthorized</response>
        [HttpGet("unread")]
        [ProducesResponseType(typeof(UnreadCountResponse), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<UnreadCountResponse>> GetUnreadCount()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var count = await _notificationService.GetUnreadCountAsync(userId);
            return Ok(new UnreadCountResponse { Count = count });
        }

        /// <summary>
        /// Mark a notification as read
        /// </summary>
        /// <param name="id">Notification unique identifier</param>
        /// <returns>Success message</returns>
        /// <response code="200">Notification marked as read</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Notification not found</response>
        [HttpPut("{id}/read")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> MarkAsRead(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            // Verify ownership
            var notification = await _notificationService.GetNotificationAsync(id);
            if (notification == null)
                return NotFound();
            
            if (notification.UserId != userId)
                return Forbid();

            var result = await _notificationService.MarkAsReadAsync(id);
            if (!result)
                return NotFound();

            return Ok(new { message = "Notification marked as read" });
        }

        /// <summary>
        /// Mark all notifications as read for the authenticated user
        /// </summary>
        /// <returns>Success message</returns>
        /// <response code="200">All notifications marked as read</response>
        /// <response code="401">Unauthorized</response>
        [HttpPut("read-all")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult> MarkAllAsRead()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _notificationService.MarkAllAsReadAsync(userId);
            return Ok(new { message = "All notifications marked as read" });
        }

        /// <summary>
        /// Delete a notification
        /// </summary>
        /// <param name="id">Notification unique identifier</param>
        /// <returns>No content</returns>
        /// <response code="204">Notification deleted successfully</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Notification not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteNotification(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            // Verify ownership
            var notification = await _notificationService.GetNotificationAsync(id);
            if (notification == null)
                return NotFound();
            
            if (notification.UserId != userId)
                return Forbid();

            var result = await _notificationService.DeleteNotificationAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }

    /// <summary>
    /// Unread notifications count response
    /// </summary>
    public class UnreadCountResponse
    {
        public int Count { get; set; }
    }
}

