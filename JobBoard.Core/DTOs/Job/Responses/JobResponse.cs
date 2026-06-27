using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Core.DTOs.Job.Responses
{
    public class JobResponse
    {
        public int JobId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public string JobType { get; set; } = "OnSite";
        public DateTime CreatedAt { get; set; }
        public string CompanyName { get; set; } = string.Empty;
    }
}
