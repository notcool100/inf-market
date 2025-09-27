using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Models;
using Microsoft.Extensions.Configuration;

namespace InfluencerMarketplace.Infrastructure.Data
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(IConfiguration configuration) 
            : base(configuration, "Users")
        {
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM Users WHERE Email = @Email";
            return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
        }

        public async Task<IEnumerable<string>> GetUserRolesAsync(Guid userId)
        {
            using var connection = CreateConnection();
            var sql = @"
                SELECT r.Name 
                FROM Roles r
                INNER JOIN UserRoles ur ON r.Id = ur.RoleId
                WHERE ur.UserId = @UserId";
            
            return await connection.QueryAsync<string>(sql, new { UserId = userId });
        }

        public async Task<bool> AddUserToRoleAsync(Guid userId, string roleName)
        {
            using var connection = CreateConnection();
            
            // First, get the role ID
            var getRoleSql = "SELECT Id FROM Roles WHERE Name = @Name";
            var roleId = await connection.QueryFirstOrDefaultAsync<Guid>(getRoleSql, new { Name = roleName });
            
            if (roleId == Guid.Empty)
                return false;
            
            // Check if the user already has this role
            var checkSql = "SELECT COUNT(1) FROM UserRoles WHERE UserId = @UserId AND RoleId = @RoleId";
            var exists = await connection.ExecuteScalarAsync<int>(checkSql, new { UserId = userId, RoleId = roleId }) > 0;
            
            if (exists)
                return true;
            
            // Add the role to the user
            var insertSql = "INSERT INTO UserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)";
            var affectedRows = await connection.ExecuteAsync(insertSql, new { UserId = userId, RoleId = roleId });
            
            return affectedRows > 0;
        }

        public async Task<bool> RemoveUserFromRoleAsync(Guid userId, string roleName)
        {
            using var connection = CreateConnection();
            
            // First, get the role ID
            var getRoleSql = "SELECT Id FROM Roles WHERE Name = @Name";
            var roleId = await connection.QueryFirstOrDefaultAsync<Guid>(getRoleSql, new { Name = roleName });
            
            if (roleId == Guid.Empty)
                return false;
            
            // Remove the role from the user
            var deleteSql = "DELETE FROM UserRoles WHERE UserId = @UserId AND RoleId = @RoleId";
            var affectedRows = await connection.ExecuteAsync(deleteSql, new { UserId = userId, RoleId = roleId });
            
            return affectedRows > 0;
        }

        public override async Task<Guid> CreateAsync(User entity)
        {
            using var connection = CreateConnection();
            
            var sql = @"
                INSERT INTO Users (Id, Email, PasswordHash, FirstName, LastName, PhoneNumber, 
                                  ProfilePictureUrl, CreatedAt, UpdatedAt, IsActive)
                VALUES (@Id, @Email, @PasswordHash, @FirstName, @LastName, @PhoneNumber, 
                        @ProfilePictureUrl, @CreatedAt, @UpdatedAt, @IsActive)
                RETURNING Id";
            
            if (entity.Id == Guid.Empty)
                entity.Id = Guid.NewGuid();
            
            entity.CreatedAt = DateTime.UtcNow;
            entity.UpdatedAt = DateTime.UtcNow;
            
            var id = await connection.ExecuteScalarAsync<Guid>(sql, entity);
            return id;
        }

        public override async Task<bool> UpdateAsync(User entity)
        {
            using var connection = CreateConnection();
            
            var sql = @"
                UPDATE Users 
                SET Email = @Email, 
                    FirstName = @FirstName, 
                    LastName = @LastName, 
                    PhoneNumber = @PhoneNumber, 
                    ProfilePictureUrl = @ProfilePictureUrl, 
                    UpdatedAt = @UpdatedAt, 
                    IsActive = @IsActive
                WHERE Id = @Id";
            
            entity.UpdatedAt = DateTime.UtcNow;
            
            var affectedRows = await connection.ExecuteAsync(sql, entity);
            return affectedRows > 0;
        }
    }
}