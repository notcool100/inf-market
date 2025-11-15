using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Interfaces;
using InfluencerMarketplace.Core.Interfaces.Services;
using InfluencerMarketplace.Core.Models;

namespace InfluencerMarketplace.Infrastructure.Services
{
    public class NavigationService : INavigationService
    {
        private readonly INavigationRepository _navigationRepository;

        public NavigationService(INavigationRepository navigationRepository)
        {
            _navigationRepository = navigationRepository;
        }

        public async Task<IEnumerable<NavigationItem>> GetUserNavigationAsync(Guid userId)
        {
            return await _navigationRepository.GetNavigationItemsByUserIdAsync(userId);
        }

        public async Task<IEnumerable<NavigationGroup>> GetAllNavigationGroupsAsync()
        {
            return await _navigationRepository.GetAllNavigationGroupsAsync();
        }

        public async Task<IEnumerable<NavigationItem>> GetAllNavigationItemsAsync()
        {
            return await _navigationRepository.GetAllNavigationItemsAsync();
        }
    }
}

