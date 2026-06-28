using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JobBoard.Core.DTOs.Application.Requests
{
    public class UpdateApplicationStatusRequest
    {
        [Required]
        public string Status { get; set; } = string.Empty;
    }
}
