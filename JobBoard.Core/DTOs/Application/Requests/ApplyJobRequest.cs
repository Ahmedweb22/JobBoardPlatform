using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JobBoard.Core.DTOs.Application.Requests
{
    public class ApplyJobRequest
    {
        [Required]
        public int JobId { get; set; }
    }
}
