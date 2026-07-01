using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Infrastructure.Repositories
{
    public class JobRepository : Repository<Job>, IJobRepository
    {
        public JobRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<(IEnumerable<Job> Items, int TotalCount)> GetJobsAsync(int pageNumber, int pageSize, string? location, string? jobType, string? sortBy)
        { 
        var query = _context.Jobs.Include(j => j.Employer).AsQueryable();
            if (!string.IsNullOrEmpty(location))
            {
                query = query.Where(j => j.Location.Contains(location));
            }
            if (!string.IsNullOrEmpty(jobType))
            {
                query = query.Where(j => j.JobType == jobType);
            }
            var totalCount = await query.CountAsync();
            query = sortBy?.ToLower() switch
            {
                "date" => query.OrderByDescending(j => j.CreatedAt),
                "salary" => query.OrderByDescending(j => j.Salary),
                _ => query.OrderByDescending(j => j.CreatedAt)
            };
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return (items, totalCount);
        }
    }
    }

