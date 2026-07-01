using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Core.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public bool IsActive => !IsExpired;

        public string UserId { get; set; } = string.Empty;
    }
}
