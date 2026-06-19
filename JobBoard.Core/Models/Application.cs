using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Core.Models
{
    public class Application
    {
        public int ApplicationId { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        public int JobId { get; set; }
        public Job Job { get; set; } = null!;
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; } = null!;
    }
}
