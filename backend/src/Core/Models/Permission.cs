using System;
using System.Collections.Generic;

namespace InfluencerMarketplace.Core.Models
{
    public class Permission
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class RolePermission
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string PermissionId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

