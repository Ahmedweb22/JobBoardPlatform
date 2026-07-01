using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Core.Models
{
    public class Candidate
    {
        public int CandidateId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? CVPath { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
    