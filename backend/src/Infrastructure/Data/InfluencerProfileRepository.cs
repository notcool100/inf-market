using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Models;
using Microsoft.Extensions.Configuration;

namespace InfluencerMarketplace.Infrastructure.Data
{
    public class InfluencerProfileRepository : BaseRepository<InfluencerProfile>, IInfluencerProfileRepository
    {
        public InfluencerProfileRepository(IConfiguration configuration)
            : base(configuration, "InfluencerProfiles")
        {
        }

        public async Task<InfluencerProfile> GetByUserIdAsync(Guid userId)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM InfluencerProfiles WHERE UserId = @UserId";
            var profile = await connection.QueryFirstOrDefaultAsync<InfluencerProfile>(sql, new { UserId = userId });
            
            if (profile != null)
            {
                DeserializeJsonFields(profile);
            }
            
            return profile;
        }

        public async Task<IEnumerable<InfluencerProfile>> SearchInfluencersAsync(
            string nicheFocus, 
            string location, 
            int? minFollowers, 
            decimal? maxRate)
        {
            using var connection = CreateConnection();
            
            var conditions = new List<string>();
            var parameters = new DynamicParameters();
            
            if (!string.IsNullOrWhiteSpace(nicheFocus))
            {
                conditions.Add("LOWER(NicheFocus) LIKE LOWER(@NicheFocus)");
                parameters.Add("NicheFocus", $"%{nicheFocus}%");
            }
            
            if (!string.IsNullOrWhiteSpace(location))
            {
                conditions.Add("LOWER(Location) LIKE LOWER(@Location)");
                parameters.Add("Location", $"%{location}%");
            }
            
            if (minFollowers.HasValue)
            {
                conditions.Add("FollowersCount >= @MinFollowers");
                parameters.Add("MinFollowers", minFollowers.Value);
            }
            
            if (maxRate.HasValue)
            {
                conditions.Add("MinCampaignRate <= @MaxRate");
                parameters.Add("MaxRate", maxRate.Value);
            }
            
            var whereClause = conditions.Any() 
                ? "WHERE " + string.Join(" AND ", conditions)
                : "";
            
            var sql = $"SELECT * FROM InfluencerProfiles {whereClause} ORDER BY AverageRating DESC, FollowersCount DESC";
            
            var profiles = await connection.QueryAsync<InfluencerProfile>(sql, parameters);
            
            foreach (var profile in profiles)
            {
                DeserializeJsonFields(profile);
            }
            
            return profiles;
        }

        public async Task<bool> UpdateRatingAsync(Guid influencerProfileId, double newAverageRating)
        {
            using var connection = CreateConnection();
            var sql = @"
                UPDATE InfluencerProfiles 
                SET AverageRating = @AverageRating, 
                    UpdatedAt = NOW()
                WHERE Id = @Id";
            
            var affectedRows = await connection.ExecuteAsync(sql, new 
            { 
                Id = influencerProfileId, 
                AverageRating = newAverageRating
            });
            
            return affectedRows > 0;
        }

        public async Task<bool> IncrementCompletedCampaignsAsync(Guid influencerProfileId)
        {
            using var connection = CreateConnection();
            var sql = @"
                UPDATE InfluencerProfiles 
                SET CompletedCampaigns = CompletedCampaigns + 1, 
                    UpdatedAt = NOW()
                WHERE Id = @Id";
            
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = influencerProfileId });
            return affectedRows > 0;
        }

        public override async Task<Guid> CreateAsync(InfluencerProfile entity)
        {
            using var connection = CreateConnection();
            
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            
            var sql = @"
                INSERT INTO InfluencerProfiles (
                    Id, UserId, Bio, NicheFocus, FollowersCount, 
                    InstagramHandle, TikTokHandle, YouTubeChannel, 
                    FacebookPage, LinkedInProfile, WebsiteUrl, 
                    MinCampaignRate, ContentTypes, Demographics, 
                    Location, IsVerified, AverageRating, 
                    CompletedCampaigns, CreatedAt, UpdatedAt
                )
                VALUES (
                    @Id, @UserId, @Bio, @NicheFocus, @FollowersCount,
                    @InstagramHandle, @TikTokHandle, @YouTubeChannel,
                    @FacebookPage, @LinkedInProfile, @WebsiteUrl,
                    @MinCampaignRate, @ContentTypes, @Demographics,
                    @Location, @IsVerified, @AverageRating,
                    @CompletedCampaigns, @CreatedAt, @UpdatedAt
                )
                RETURNING Id";
            
            var parameters = new
            {
                entity.Id,
                entity.UserId,
                entity.Bio,
                entity.NicheFocus,
                entity.FollowersCount,
                entity.InstagramHandle,
                entity.TikTokHandle,
                entity.YouTubeChannel,
                entity.FacebookPage,
                entity.LinkedInProfile,
                entity.WebsiteUrl,
                entity.MinCampaignRate,
                ContentTypes = SerializeJsonField(entity.ContentTypes),
                Demographics = SerializeJsonField(entity.Demographics),
                entity.Location,
                entity.IsVerified,
                entity.AverageRating,
                entity.CompletedCampaigns,
                entity.CreatedAt,
                entity.UpdatedAt
            };
            
            var id = await connection.ExecuteScalarAsync<Guid>(sql, parameters);
            return id;
        }

        public override async Task<bool> UpdateAsync(InfluencerProfile entity)
        {
            using var connection = CreateConnection();
            
            entity.UpdatedAt = DateTime.UtcNow;
            
            var sql = @"
                UPDATE InfluencerProfiles 
                SET UserId = @UserId,
                    Bio = @Bio,
                    NicheFocus = @NicheFocus,
                    FollowersCount = @FollowersCount,
                    InstagramHandle = @InstagramHandle,
                    TikTokHandle = @TikTokHandle,
                    YouTubeChannel = @YouTubeChannel,
                    FacebookPage = @FacebookPage,
                    LinkedInProfile = @LinkedInProfile,
                    WebsiteUrl = @WebsiteUrl,
                    MinCampaignRate = @MinCampaignRate,
                    ContentTypes = @ContentTypes,
                    Demographics = @Demographics,
                    Location = @Location,
                    IsVerified = @IsVerified,
                    AverageRating = @AverageRating,
                    CompletedCampaigns = @CompletedCampaigns,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id";
            
            var parameters = new
            {
                entity.Id,
                entity.UserId,
                entity.Bio,
                entity.NicheFocus,
                entity.FollowersCount,
                entity.InstagramHandle,
                entity.TikTokHandle,
                entity.YouTubeChannel,
                entity.FacebookPage,
                entity.LinkedInProfile,
                entity.WebsiteUrl,
                entity.MinCampaignRate,
                ContentTypes = SerializeJsonField(entity.ContentTypes),
                Demographics = SerializeJsonField(entity.Demographics),
                entity.Location,
                entity.IsVerified,
                entity.AverageRating,
                entity.CompletedCampaigns,
                entity.UpdatedAt
            };
            
            var affectedRows = await connection.ExecuteAsync(sql, parameters);
            return affectedRows > 0;
        }

        public override async Task<InfluencerProfile> GetByIdAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = $"SELECT * FROM {_tableName} WHERE Id = @Id";
            var profile = await connection.QueryFirstOrDefaultAsync<InfluencerProfile>(sql, new { Id = id });
            
            if (profile != null)
            {
                DeserializeJsonFields(profile);
            }
            
            return profile;
        }

        public override async Task<IEnumerable<InfluencerProfile>> GetAllAsync()
        {
            using var connection = CreateConnection();
            var sql = $"SELECT * FROM {_tableName} ORDER BY CreatedAt DESC";
            var profiles = await connection.QueryAsync<InfluencerProfile>(sql);
            
            foreach (var profile in profiles)
            {
                DeserializeJsonFields(profile);
            }
            
            return profiles;
        }

        private void DeserializeJsonFields(InfluencerProfile profile)
        {
            // JSONB fields are already stored as JSON strings in the database
            // Dapper will map them as strings, which is what we want
            // The application layer can deserialize them if needed
        }

        private string SerializeJsonField(string jsonString)
        {
            return jsonString ?? "null";
        }
    }
}

