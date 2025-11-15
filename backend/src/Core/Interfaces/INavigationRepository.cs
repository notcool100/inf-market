using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfluencerMarketplace.Core.Models;

namespace InfluencerMarketplace.Core.Interfaces
{
    public interface INavigationRepository
    {
        Task<IEnumerable<NavigationItem>> GetNavigationItemsByUserIdAsync(Guid userId);
        Task<IEnumerable<NavigationGroup>> GetAllNavigationGroupsAsync();
        Task<IEnumerable<NavigationItem>> GetAllNavigationItemsAsync();
        Task<NavigationItem> GetNavigationItemByIdAsync(Guid id);
        Task<NavigationGroup> GetNavigationGroupByIdAsync(Guid id);
    }
}

