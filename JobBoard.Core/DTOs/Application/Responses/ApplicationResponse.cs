using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Core.DTOs.Application.Responses
{
    public class ApplicationResponse
    {
        public int ApplicationId { get; set; }
        public int JobId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CandidateName {get; set; } = string.Empty;
        public string? CandidateCVPath {  get; set; } 
        public string Status {  get; set; } = string.Empty;
        public DateTime AppliedDate { get; set; }
    }
}
