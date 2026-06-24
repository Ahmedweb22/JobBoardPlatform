using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Core.Interfaces.IRepositories
{
    public interface IApplicationRepository : IRepository<Application>
    {
        Task<IEnumerable<Application>> GetApplicationsByEmployerAsync(int employerId);
        Task<IEnumerable<Application>> GetApplicationsByCandidateAsync(int candidateId);
    }
}
