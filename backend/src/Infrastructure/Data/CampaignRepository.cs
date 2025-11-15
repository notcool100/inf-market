using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Models;
using Microsoft.Extensions.Configuration;

namespace InfluencerMarketplace.Infrastructure.Data
{
    public class CampaignRepository : BaseRepository<Campaign>, ICampaignRepository
    {
        public CampaignRepository(IConfiguration configuration)
            : base(configuration, "Campaigns")
        {
        }

        public async Task<IEnumerable<Campaign>> GetCampaignsByBrandIdAsync(Guid brandId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Campaigns WHERE BrandId = @BrandId ORDER BY CreatedAt DESC";
            var campaigns = await connection.QueryAsync<Campaign>(sql, new { BrandId = brandId });
            
            // Deserialize JSONB fields
            foreach (var campaign in campaigns)
            {
                DeserializeJsonFields(campaign);
            }
            
            return campaigns;
        }

        public async Task<IEnumerable<Campaign>> GetCampaignsByInfluencerIdAsync(Guid influencerId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Campaigns WHERE InfluencerId = @InfluencerId ORDER BY CreatedAt DESC";
            var campaigns = await connection.QueryAsync<Campaign>(sql, new { InfluencerId = influencerId });
            
            // Deserialize JSONB fields
            foreach (var campaign in campaigns)
            {
                DeserializeJsonFields(campaign);
            }
            
            return campaigns;
        }

        public async Task<bool> AssignInfluencerAsync(Guid campaignId, Guid influencerId)
        {
            using var connection = CreateConnection();
            var sql = @"
                UPDATE Campaigns 
                SET InfluencerId = @InfluencerId, 
                    Status = @Status,
                    UpdatedAt = NOW()
                WHERE Id = @CampaignId";
            
            var affectedRows = await connection.ExecuteAsync(sql, new 
            { 
                CampaignId = campaignId, 
                InfluencerId = influencerId,
                Status = CampaignStatus.InProgress.ToString()
            });
            
            return affectedRows > 0;
        }

        public async Task<bool> UpdateCampaignStatusAsync(Guid campaignId, CampaignStatus status)
        {
            using var connection = CreateConnection();
            var sql = @"
                UPDATE Campaigns 
                SET Status = @Status, 
                    UpdatedAt = NOW()
                WHERE Id = @CampaignId";
            
            var affectedRows = await connection.ExecuteAsync(sql, new 
            { 
                CampaignId = campaignId, 
                Status = status.ToString()
            });
            
            return affectedRows > 0;
        }

        public async Task<IEnumerable<Campaign>> GetAvailableCampaignsAsync()
        {
            using var connection = CreateConnection();
            var sql = @"
                SELECT * FROM Campaigns 
                WHERE Status = @Status 
                    AND InfluencerId IS NULL
                ORDER BY CreatedAt DESC";
            
            var campaigns = await connection.QueryAsync<Campaign>(sql, new 
            { 
                Status = CampaignStatus.Open.ToString()
            });
            
            // Deserialize JSONB fields
            foreach (var campaign in campaigns)
            {
                DeserializeJsonFields(campaign);
            }
            
            return campaigns;
        }

        public override async Task<Guid> CreateAsync(Campaign entity)
        {
            using var connection = CreateConnection();
            
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            
            var sql = @"
                INSERT INTO Campaigns (
                    Id, Title, Description, BrandId, InfluencerId, Budget, 
                    StartDate, EndDate, Requirements, Deliverables, 
                    TargetAudience, TargetPlatforms, Status, CreatedAt, UpdatedAt
                )
                VALUES (
                    @Id, @Title, @Description, @BrandId, @InfluencerId, @Budget,
                    @StartDate, @EndDate, @Requirements, @Deliverables,
                    @TargetAudience, @TargetPlatforms, @Status, @CreatedAt, @UpdatedAt
                )
                RETURNING Id";
            
            var parameters = new
            {
                entity.Id,
                entity.Title,
                entity.Description,
                entity.BrandId,
                entity.InfluencerId,
                entity.Budget,
                entity.StartDate,
                entity.EndDate,
                entity.Requirements,
                Deliverables = SerializeJsonField(entity.Deliverables),
                TargetAudience = SerializeJsonField(entity.TargetAudience),
                TargetPlatforms = SerializeJsonField(entity.TargetPlatforms),
                Status = entity.Status.ToString(),
                entity.CreatedAt,
                entity.UpdatedAt
            };
            
            var id = await connection.ExecuteScalarAsync<Guid>(sql, parameters);
            return id;
        }

        public override async Task<bool> UpdateAsync(Campaign entity)
        {
            using var connection = CreateConnection();
            
            entity.UpdatedAt = DateTime.UtcNow;
            
            var sql = @"
                UPDATE Campaigns 
                SET Title = @Title,
                    Description = @Description,
                    BrandId = @BrandId,
                    InfluencerId = @InfluencerId,
                    Budget = @Budget,
                    StartDate = @StartDate,
                    EndDate = @EndDate,
                    Requirements = @Requirements,
                    Deliverables = @Deliverables,
                    TargetAudience = @TargetAudience,
                    TargetPlatforms = @TargetPlatforms,
                    Status = @Status,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id";
            
            var parameters = new
            {
                entity.Id,
                entity.Title,
                entity.Description,
                entity.BrandId,
                entity.InfluencerId,
                entity.Budget,
                entity.StartDate,
                entity.EndDate,
                entity.Requirements,
                Deliverables = SerializeJsonField(entity.Deliverables),
                TargetAudience = SerializeJsonField(entity.TargetAudience),
                TargetPlatforms = SerializeJsonField(entity.TargetPlatforms),
                Status = entity.Status.ToString(),
                entity.UpdatedAt
            };
            
            var affectedRows = await connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }

        public override async Task<Campaign> GetByIdAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = $"SELECT * FROM {_tableName} WHERE Id = @Id";
            var campaign = await connection.QueryFirstOrDefaultAsync<Campaign>(sql, new { Id = id });
            
            if (campaign != null)
            {
                DeserializeJsonFields(campaign);
            }
            
            return campaign;
        }

        public override async Task<IEnumerable<Campaign>> GetAllAsync()
        {
            using var connection = CreateConnection();
            var sql = $"SELECT * FROM {_tableName} ORDER BY CreatedAt DESC";
            var campaigns = await connection.QueryAsync<Campaign>(sql);
            
            foreach (var campaign in campaigns)
            {
                DeserializeJsonFields(campaign);
            }
            
            return campaigns;
        }

        private void DeserializeJsonFields(Campaign campaign)
        {
            // Dapper automatically maps VARCHAR to enum, so no conversion needed
            // JSONB fields are already stored as JSON strings in the database
            // Dapper will map them as strings, which is what we want
            // The application layer can deserialize them if needed
        }

        private string SerializeJsonField(string jsonString)
        {
            // If it's already a JSON string, return as is
            // Otherwise, serialize if it's an object
            return jsonString ?? "null";
        }
    }
}

