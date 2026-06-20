using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JobBoard.Core.DTOs.Auth.Requests
{
    public class RegisterRequest
    {
        [Required , EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required , MinLength(6)]
        public string Password { get; set; } = string.Empty;
        
        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required]
        public string Role { get; set; } = string.Empty;
        //Candidate
        public string? FullName { get; set; }
            public string? PhoneNumber { get; set; }
        //Employer
        public string? CompanyName { get; set; }
        public string? Description { get; set; }
    }
}
