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
    public class NavigationRepository : INavigationRepository
    {
        private readonly IConfiguration _configuration;

        public NavigationRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected IDbConnection CreateConnection()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            return new NpgsqlConnection(connectionString);
        }

        public async Task<IEnumerable<NavigationItem>> GetNavigationItemsByUserIdAsync(Guid userId)
        {
            using var connection = CreateConnection();
            
            // Use the database function to get user navigation
            var sql = @"
                SELECT 
                    id, 
                    label, 
                    icon, 
                    url, 
                    ""order"", 
                    ""parentId"", 
                    ""groupId""
                FROM get_user_navigation(@UserId)";
            
            var items = await connection.QueryAsync<NavigationItem>(sql, new { UserId = userId });
            
            // Group items by group and build hierarchy
            var groupedItems = items
                .GroupBy(item => item.GroupId)
                .SelectMany(group => group.OrderBy(item => item.Order))
                .ToList();
            
            // Build parent-child relationships
            var itemDict = groupedItems.ToDictionary(item => item.Id);
            var rootItems = new List<NavigationItem>();
            
            foreach (var item in groupedItems)
            {
                if (item.ParentId.HasValue && itemDict.ContainsKey(item.ParentId.Value))
                {
                    var parent = itemDict[item.ParentId.Value];
                    if (parent.Children == null)
                        parent.Children = new List<NavigationItem>();
                    parent.Children.Add(item);
                }
                else
                {
                    rootItems.Add(item);
                }
            }
            
            return rootItems;
        }

        public async Task<IEnumerable<NavigationGroup>> GetAllNavigationGroupsAsync()
        {
            using var connection = CreateConnection();
            var sql = @"
                SELECT * FROM navigation_groups 
                WHERE ""isActive"" = true 
                ORDER BY ""order""";
            
            return await connection.QueryAsync<NavigationGroup>(sql);
        }

        public async Task<IEnumerable<NavigationItem>> GetAllNavigationItemsAsync()
        {
            using var connection = CreateConnection();
            var sql = @"
                SELECT * FROM navigation_items 
                WHERE ""isActive"" = true 
                ORDER BY ""order""";
            
            return await connection.QueryAsync<NavigationItem>(sql);
        }

        public async Task<NavigationItem> GetNavigationItemByIdAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM navigation_items WHERE id = @Id";
            return await connection.QueryFirstOrDefaultAsync<NavigationItem>(sql, new { Id = id });
        }

        public async Task<NavigationGroup> GetNavigationGroupByIdAsync(Guid id)
        {
            using var connection = CreateConnection();
            var sql = "SELECT * FROM navigation_groups WHERE id = @Id";
            return await connection.QueryFirstOrDefaultAsync<NavigationGroup>(sql, new { Id = id });
        }
    }
}

