using System;
using System.Collections.Generic;
using System.Text;
using JobBoard.Core.DTOs.Job.Requests;
using JobBoard.Core.DTOs.Job.Responses;

namespace JobBoard.Core.Interfaces.IServices
{
    public interface IJobService
    {
        Task<JobResponse> CreateJobAsync(CreateJobRequest request, string userId);
        Task<PaginationResult<JobResponse>> GetJobsAsync(int pageNumber, int pageSize, string? location, string? jobTybe, string sortBy);
    }
}
