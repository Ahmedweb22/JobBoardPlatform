using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Core.Models
{
    public class Job
    {
        public int JobId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public string Location { get; set; } = string.Empty;
        public string JobType { get; set; } = "OnSite";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int EmployerId { get; set; }
        public Employer Employer { get; set; } = null!;

        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
