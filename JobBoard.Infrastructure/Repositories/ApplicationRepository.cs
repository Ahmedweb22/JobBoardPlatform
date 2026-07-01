using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Infrastructure.Repositories
{
    public class ApplicationRepository : Repository<Application>, IApplicationRepository
    {
        public ApplicationRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Application>> GetApplicationsByEmployerAsync(int employerId)
        {
            var applications = await _context.Applications
                .Include(a => a.Job)
                .Include(a => a.Candidate)
                .Where(a => a.Job.EmployerId == employerId)
                .OrderByDescending(a => a.AppliedAt)
                .ToListAsync();
            return applications;
        }
        public async Task<IEnumerable<Application>> GetApplicationsByCandidateAsync(int candidateId)
        {
            var applications = await _context.Applications
                .Include(a => a.Job)
                .ThenInclude(j => j.Employer)
                .Where(a => a.CandidateId == candidateId)
                .OrderByDescending(a => a.AppliedAt)
                .ToListAsync();

            return applications;
    }
}
}
