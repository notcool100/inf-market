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
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(IConfiguration configuration)
            : base(configuration, "Reviews")
        {
        }

        public async Task<IEnumerable<Review>> GetReviewsByInfluencerIdAsync(Guid influencerProfileId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Reviews WHERE InfluencerProfileId = @InfluencerProfileId ORDER BY CreatedAt DESC";
            return await connection.QueryAsync<Review>(sql, new { InfluencerProfileId = influencerProfileId });
        }

        public async Task<Review> GetReviewByCampaignIdAsync(Guid campaignId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Reviews WHERE CampaignId = @CampaignId";
            return await connection.QueryFirstOrDefaultAsync<Review>(sql, new { CampaignId = campaignId });
        }

        public override async Task<Guid> CreateAsync(Review entity)
        {
            using var connection = CreateConnection();
            
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            
            var sql = @"
                INSERT INTO Reviews (
                    Id, CampaignId, ReviewerId, InfluencerProfileId, 
                    Rating, Comment, IsPublic, CreatedAt, UpdatedAt
                )
                VALUES (
                    @Id, @CampaignId, @ReviewerId, @InfluencerProfileId,
                    @Rating, @Comment, @IsPublic, @CreatedAt, @UpdatedAt
                )
                RETURNING Id";
            
            var parameters = new
            {
                entity.Id,
                entity.CampaignId,
                entity.ReviewerId,
                entity.InfluencerProfileId,
                entity.Rating,
                entity.Comment,
                entity.IsPublic,
                entity.CreatedAt,
                entity.UpdatedAt
            };
            
            var id = await connection.ExecuteScalarAsync<Guid>(sql, parameters);
            return id;
        }

        public override async Task<bool> UpdateAsync(Review entity)
        {
            using var connection = CreateConnection();
            
            entity.UpdatedAt = DateTime.UtcNow;
            
            var sql = @"
                UPDATE Reviews 
                SET CampaignId = @CampaignId,
                    ReviewerId = @ReviewerId,
                    InfluencerProfileId = @InfluencerProfileId,
                    Rating = @Rating,
                    Comment = @Comment,
                    IsPublic = @IsPublic,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id";
            
            var parameters = new
            {
                entity.Id,
                entity.CampaignId,
                entity.ReviewerId,
                entity.InfluencerProfileId,
                entity.Rating,
                entity.Comment,
                entity.IsPublic,
                entity.UpdatedAt
            };
            
            var affectedRows = await connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }
    }
}

