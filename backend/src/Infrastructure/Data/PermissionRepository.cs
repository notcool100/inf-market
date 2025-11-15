using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Models;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace InfluencerMarketplace.Infrastructure.Data
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly IConfiguration _configuration;

        public PermissionRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            return new NpgsqlConnection(connectionString);
        }

        public async Task<IEnumerable<Permission>> GetPermissionsByUserIdAsync(Guid userId)
        {
            using var connection = CreateConnection();
            
            // Use the database function to get user permissions
            var sql = "SELECT * FROM get_user_permissions(@UserId)";
            return await connection.QueryAsync<Permission>(sql, new { UserId = userId });
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM permissions ORDER BY \"module\", \"action\"";
            return await connection.QueryAsync<Permission>(sql);
        }

        public async Task<Permission> GetPermissionByIdAsync(string id)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM permissions WHERE id = @Id";
            return await connection.QueryFirstOrDefaultAsync<Permission>(sql, new { Id = id });
        }

        public async Task<bool> HasPermissionAsync(Guid userId, string permissionCode)
        {
            using var connection = CreateConnection();
            
            var sql = @"
                SELECT COUNT(1) 
                FROM permissions p
                INNER JOIN role_permissions rp ON p.id = rp.""permissionId""
                INNER JOIN UserRoles ur ON rp.""roleId"" = ur.""RoleId""
                WHERE ur.""UserId"" = @UserId AND p.code = @PermissionCode";
            
            var count = await connection.ExecuteScalarAsync<int>(sql, new 
            { 
                UserId = userId, 
                PermissionCode = permissionCode 
            });
            
            return count > 0;
        }
    }
}

