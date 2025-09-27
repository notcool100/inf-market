using System;
using System.Collections.Generic;

namespace InfluencerMarketplace.Shared.DTOs.Auth
{
    public class AuthResponse
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<string> Roles { get; set; }
        public string Message { get; set; }
    }
}