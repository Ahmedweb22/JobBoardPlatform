using System.Security.Claims;
using JobBoard.Core.Interfaces.IRepositories;
using JobBoard.Core.Interfaces.IUnitOfWorks;
using JobBoard.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JopBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IRepository<Application> _applicationRepository;
        private readonly IRepository<Candidate> _candidateRepository;
        private readonly IRepository<Employer> _employerRepository;

        private readonly IUnitOfWork _unitOfWork;
        public ApplicationsController(IRepository<Application> applicationRepository, IRepository<Candidate> candidateRepository, IRepository<Employer> employerRepository, IUnitOfWork unitOfWork)
        {
            _applicationRepository = applicationRepository;
            _candidateRepository = candidateRepository;
            _employerRepository = employerRepository;
            _unitOfWork = unitOfWork;
        }
        [HttpPost("apply/{jobId}")]
        [Authorize(Roles = "Candidate")]
        public async Task<IActionResult> ApplyToJob(int jobId)
        {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var candidate = await _candidateRepository.GetOneAsync(c => c.UserId == userId);

            if (candidate == null) 
            {
            return NotFound(new { message ="Candidate profile not found"});
            }
            var alreadyApplied = (await _applicationRepository.GetAllAsync()).Any(a => a.JobId == jobId && a.CandidateId == candidate.CandidateId);
            if (alreadyApplied)
            {
                return BadRequest(new { message = "You have already applied to this job." });
            }
            var application = new Application
            {
                JobId = jobId,
                CandidateId = candidate.CandidateId,
                Status = "Pending",
                AppliedAt = DateTime.UtcNow
            };
            await _applicationRepository.AddAsync(application);
            await _unitOfWork.CommitAsync();
            return Ok(new
            {
                message = "Applied successfully.",
                applicationId = application.ApplicationId
            });
    }
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody]UpdateApplicationStatusRequest  model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employer = await _employerRepository.GetOneAsync(e => e.UserId == userId);
            if (employer == null)
            {
                return NotFound(new {message = "Employer profile not found."});
            }
            var application = await _applicationRepository.GetOneAsync(a => a.ApplicationId == id , includes: [a => a.Job]);
            if (application == null)
            {
                return NotFound(new { message = "Application not found." });
            }
            if (application.Job.EmployerId != employer.EmployerId)
            {
                return Forbid();
            }
            application.Status = model.Status;
            await _unitOfWork.CommitAsync();
            return Ok(new
            {
                message = "Application status updated successfully.",
                status = application.Status
            });
}
}
}


