using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Core.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; 

        public Employer? Employer { get; set; }
        public Candidate? Candidate { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
