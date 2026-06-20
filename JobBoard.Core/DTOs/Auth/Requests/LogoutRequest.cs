using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Core.DTOs.Auth.Requests
{
    public class LogoutRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
