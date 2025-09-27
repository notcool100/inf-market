using System;
using System.Collections.Generic;

namespace InfluencerMarketplace.Core.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfilePictureUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public List<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public InfluencerProfile InfluencerProfile { get; set; }
        public Wallet Wallet { get; set; }
    }
}