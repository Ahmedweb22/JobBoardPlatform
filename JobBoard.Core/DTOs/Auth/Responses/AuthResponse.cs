using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Core.DTOs.Auth.Responses
{
    public class AuthResponse
    {
        public bool IsAuthenticated { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
