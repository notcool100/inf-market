using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Models;

namespace InfluencerMarketplace.Core.Interfaces.Services
{
    public interface IPermissionService
    {
        Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId);
        Task<bool> HasPermissionAsync(Guid userId, string permissionCode);
        Task<IEnumerable<Permission>> GetAllPermissionsAsync();
    }
}

