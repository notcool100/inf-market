using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Interfaces.Services;
using InfluencerMarketplace.Core.Models;

namespace InfluencerMarketplace.Infrastructure.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _permissionRepository;

        public PermissionService(IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }

        public async Task<IEnumerable<Permission>> GetUserPermissionsAsync(Guid userId)
        {
            return await _permissionRepository.GetPermissionsByUserIdAsync(userId);
        }

        public async Task<bool> HasPermissionAsync(Guid userId, string permissionCode)
        {
            return await _permissionRepository.HasPermissionAsync(userId, permissionCode);
        }

        public async Task<IEnumerable<Permission>> GetAllPermissionsAsync()
        {
            return await _permissionRepository.GetAllPermissionsAsync();
        }
    }
}

