using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Models;

namespace InfluencerMarketplace.Core.Interfaces.Services
{
    public interface INavigationService
    {
        Task<IEnumerable<NavigationItem>> GetUserNavigationAsync(Guid userId);
        Task<IEnumerable<NavigationGroup>> GetAllNavigationGroupsAsync();
        Task<IEnumerable<NavigationItem>> GetAllNavigationItemsAsync();
    }
}

