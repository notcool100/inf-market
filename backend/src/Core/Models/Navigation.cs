using System;
using System.Collections.Generic;

namespace InfluencerMarketplace.Core.Models
{
    public class NavigationGroup
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<NavigationItem> Items { get; set; } = new List<NavigationItem>();
    }

    public class NavigationItem
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public int Order { get; set; }
        public Guid? ParentId { get; set; }
        public NavigationItem Parent { get; set; }
        public Guid? GroupId { get; set; }
        public NavigationGroup Group { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<NavigationItem> Children { get; set; } = new List<NavigationItem>();
    }

    public class RoleNavigation
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public Guid NavigationItemId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

