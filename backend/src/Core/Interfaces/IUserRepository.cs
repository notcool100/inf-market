using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Models;

namespace InfluencerMarketplace.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<string>> GetUserRolesAsync(Guid userId);
        Task<bool> AddUserToRoleAsync(Guid userId, string roleName);
        Task<bool> RemoveUserFromRoleAsync(Guid userId, string roleName);
    }
}