using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace JobBoard.Core.DTOs.Job.Requests
{
    public class CreateJobRequest
    {
        [Required, MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; }= string.Empty;
        [Required]
        public string Location { get; set; } = string.Empty;
        [Required , Range(0, double.MaxValue)]
        public decimal Salary { get; set; }
        [Required]
        public string JobType { get; set; } = "OnSite";
    }
}
