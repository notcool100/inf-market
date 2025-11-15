using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace InfluencerMarketplace.Infrastructure.Data
{
    public class NotificationRepository : BaseRepository<Notification>, INotificationRepository
    {
        public NotificationRepository(IConfiguration configuration)
            : base(configuration, "Notifications")
        {
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Notifications WHERE UserId = @UserId ORDER BY CreatedAt DESC";
            var notifications = await connection.QueryAsync<Notification>(sql, new { UserId = userId });
            return notifications;
        }

        public async Task<int> GetUnreadCountAsync(Guid userId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT COUNT(*) FROM Notifications WHERE UserId = @UserId AND IsRead = false";
            return await connection.ExecuteScalarAsync<int>(sql, new { UserId = userId });
        }

        public async Task<bool> MarkAsReadAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = @"
                UPDATE Notifications 
                SET IsRead = true, ReadAt = NOW(), UpdatedAt = NOW()
                WHERE Id = @Id";
            
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<bool> MarkAllAsReadAsync(Guid userId)
        {
            using var connection = CreateConnection();
            var sql = @"
                UPDATE Notifications 
                SET IsRead = true, ReadAt = NOW(), UpdatedAt = NOW()
                WHERE UserId = @UserId AND IsRead = false";
            
            var affectedRows = await connection.ExecuteAsync(sql, new { UserId = userId });
            return affectedRows >= 0; // Return true even if no rows were updated
        }

        public override async Task<Guid> CreateAsync(Notification entity)
        {
            using var connection = CreateConnection();
            
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.IsRead = false;
            
            var sql = @"
                INSERT INTO Notifications (
                    Id, UserId, Title, Message, Type, 
                    RelatedEntityType, RelatedEntityId, IsRead, 
                    CreatedAt, ReadAt, UpdatedAt
                )
                VALUES (
                    @Id, @UserId, @Title, @Message, @Type,
                    @RelatedEntityType, @RelatedEntityId, @IsRead,
                    @CreatedAt, @ReadAt, @UpdatedAt
                )
                RETURNING Id";
            
            var parameters = new
            {
                entity.Id,
                entity.UserId,
                entity.Title,
                entity.Message,
                Type = entity.Type.ToString(),
                entity.RelatedEntityType,
                entity.RelatedEntityId,
                entity.IsRead,
                entity.CreatedAt,
                entity.ReadAt,
                entity.UpdatedAt
            };
            
            var id = await connection.ExecuteScalarAsync<Guid>(sql, parameters);
            return id;
        }

        public override async Task<bool> UpdateAsync(Notification entity)
        {
            using var connection = CreateConnection();
            
            entity.UpdatedAt = DateTime.UtcNow;
            
            var sql = @"
                UPDATE Notifications 
                SET UserId = @UserId,
                    Title = @Title,
                    Message = @Message,
                    Type = @Type,
                    RelatedEntityType = @RelatedEntityType,
                    RelatedEntityId = @RelatedEntityId,
                    IsRead = @IsRead,
                    ReadAt = @ReadAt,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id";
            
            var parameters = new
            {
                entity.Id,
                entity.UserId,
                entity.Title,
                entity.Message,
                Type = entity.Type.ToString(),
                entity.RelatedEntityType,
                entity.RelatedEntityId,
                entity.IsRead,
                entity.ReadAt,
                entity.UpdatedAt
            };
            
            var affectedRows = await connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }

        public override async Task<Notification> GetByIdAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = $"SELECT * FROM {_tableName} WHERE Id = @Id";
            var notification = await connection.QueryFirstOrDefaultAsync<Notification>(sql, new { Id = id });
            return notification;
        }
    }
}

