using System;
using System.Collections.Generic;

namespace InfluencerMarketplace.Shared.DTOs.Auth
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expiration { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Roles { get; set; }
        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
        public List<NavigationItemDto> NavigationItems { get; set; } = new List<NavigationItemDto>();
        public string Message { get; set; }
    }

    public class PermissionDto
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Module { get; set; }
        public string Action { get; set; }
    }

    public class NavigationItemDto
    {
        public Guid Id { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public int Order { get; set; }
        public Guid? ParentId { get; set; }
        public Guid? GroupId { get; set; }
        public string GroupName { get; set; }
        public int? GroupOrder { get; set; }
        public List<NavigationItemDto> Children { get; set; } = new List<NavigationItemDto>();
    }
}