using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Core.Interfaces.IRepositories
{
    public interface IJobRepository : IRepository<Job>
    {
        Task<(IEnumerable<Job> Items, int TotalCount)> GetJobsAsync(int pageNumber, int pageSize, string? location, string? jobType, string? sortBy);

    }
}
