using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Core.Models
{
    public class Employer
    {
        public int EmployerId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyDescription { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;
        public ICollection<Job> Jobs { get; set; } = new List<Job>();

    }
}
