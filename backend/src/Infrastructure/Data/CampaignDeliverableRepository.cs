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
    public class CampaignDeliverableRepository : BaseRepository<CampaignDeliverable>, ICampaignDeliverableRepository
    {
        public CampaignDeliverableRepository(IConfiguration configuration)
            : base(configuration, "CampaignDeliverables")
        {
        }

        public async Task<IEnumerable<CampaignDeliverable>> GetDeliverablesByCampaignIdAsync(Guid campaignId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM CampaignDeliverables WHERE CampaignId = @CampaignId ORDER BY CreatedAt DESC";
            return await connection.QueryAsync<CampaignDeliverable>(sql, new { CampaignId = campaignId });
        }

        public async Task<bool> UpdateStatusAsync(Guid id, DeliverableStatus status, string feedbackNotes = null)
        {
            using var connection = CreateConnection();
            var sql = @"
                UPDATE CampaignDeliverables 
                SET Status = @Status,
                    FeedbackNotes = COALESCE(@FeedbackNotes, FeedbackNotes),
                    ReviewedAt = CASE WHEN @Status IN ('Approved', 'Rejected') THEN NOW() ELSE ReviewedAt END,
                    UpdatedAt = NOW()
                WHERE Id = @Id";
            
            var affectedRows = await connection.ExecuteAsync(sql, new 
            { 
                Id = id,
                Status = status.ToString(),
                FeedbackNotes = feedbackNotes
            });
            
            return affectedRows > 0;
        }

        public override async Task<Guid> CreateAsync(CampaignDeliverable entity)
        {
            using var connection = CreateConnection();
            
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            
            var sql = @"
                INSERT INTO CampaignDeliverables (
                    Id, CampaignId, Title, Description, DeliverableType, 
                    ProofUrl, ScreenshotUrl, FeedbackNotes, Status, DueDate, 
                    SubmittedAt, ReviewedAt, CreatedAt, UpdatedAt
                )
                VALUES (
                    @Id, @CampaignId, @Title, @Description, @DeliverableType,
                    @ProofUrl, @ScreenshotUrl, @FeedbackNotes, @Status, @DueDate,
                    @SubmittedAt, @ReviewedAt, @CreatedAt, @UpdatedAt
                )
                RETURNING Id";
            
            var parameters = new
            {
                entity.Id,
                entity.CampaignId,
                entity.Title,
                entity.Description,
                entity.DeliverableType,
                entity.ProofUrl,
                entity.ScreenshotUrl,
                entity.FeedbackNotes,
                Status = entity.Status.ToString(),
                entity.DueDate,
                entity.SubmittedAt,
                entity.ReviewedAt,
                entity.CreatedAt,
                entity.UpdatedAt
            };
            
            var id = await connection.ExecuteScalarAsync<Guid>(sql, parameters);
            return id;
        }

        public override async Task<bool> UpdateAsync(CampaignDeliverable entity)
        {
            using var connection = CreateConnection();
            
            entity.UpdatedAt = DateTime.UtcNow;
            
            var sql = @"
                UPDATE CampaignDeliverables 
                SET CampaignId = @CampaignId,
                    Title = @Title,
                    Description = @Description,
                    DeliverableType = @DeliverableType,
                    ProofUrl = @ProofUrl,
                    ScreenshotUrl = @ScreenshotUrl,
                    FeedbackNotes = @FeedbackNotes,
                    Status = @Status,
                    DueDate = @DueDate,
                    SubmittedAt = @SubmittedAt,
                    ReviewedAt = @ReviewedAt,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id";
            
            var parameters = new
            {
                entity.Id,
                entity.CampaignId,
                entity.Title,
                entity.Description,
                entity.DeliverableType,
                entity.ProofUrl,
                entity.ScreenshotUrl,
                entity.FeedbackNotes,
                Status = entity.Status.ToString(),
                entity.DueDate,
                entity.SubmittedAt,
                entity.ReviewedAt,
                entity.UpdatedAt
            };
            
            var affectedRows = await connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }

        public override async Task<CampaignDeliverable> GetByIdAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = $"SELECT * FROM {_tableName} WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<CampaignDeliverable>(sql, new { Id = id });
        }
    }
}

