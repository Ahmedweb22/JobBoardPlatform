using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JobBoard.Core.DTOs.Auth.Requests
{
    public class RevokeTokenRequest
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
