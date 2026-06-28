using System;
using System.Collections.Generic;
using System.Text;
using JobBoard.Core.Common;
using JobBoard.Core.DTOs.Job.Requests;
using JobBoard.Core.DTOs.Job.Responses;
using JobBoard.Core.Interfaces.IUnitOfWorks;

namespace JobBoard.Infrastructure.Services
{
    public class JobService : IJobService
    {
        private readonly IJobRepository _jobRepository;
        private readonly IRepository<Employer> _employerRepository;
        private readonly IUnitOfWork _unitOfWork;
        public JobService(IJobRepository jobRepository, IRepository<Employer> employerRepository, IUnitOfWork unitOfWork)
        {
            _jobRepository = jobRepository;
            _employerRepository = employerRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<JobResponse> CreateJobAsync(CreateJobRequest request , string userId) 
        {
        var employer = (await _employerRepository.GetAllAsync()).FirstOrDefault(e => e.UserId == userId);
            if (employer == null)
                throw new KeyNotFoundException("Employer profile not found for this user.");
            var job = new Job
            { 
            Title = request.Title,
            Description = request.Description,
            Location = request.Location,
            Salary = request.Salary,
            JobType = request.JobType,
            EmployerId = employer.EmployerId,
            CreatedAt = DateTime.UtcNow,
            };
            await _jobRepository.AddAsync(job);
            await _unitOfWork.CommitAsync();
            return new JobResponse
            { 
            JobId = job.JobId,
            Title =job.Title,
            Description =job.Description,
            Location =job.Location,
            Salary =job.Salary,
            JobType =job.JobType,
            CreatedAt = job.CreatedAt,
            CompanyName = employer.CompanyName
            };
        }
        public async Task<PaginationResult<JobResponse>> GetJobsAsync(int pageNumber , int pageSize , string? location , string? jobTybe , string? sortBy) 
        {
            var (items, totalCount) = await _jobRepository.GetJobsAsync(pageNumber, pageSize, location, jobTybe, sortBy);
            var jobDtos = items.Select(j => new JobResponse
            {
            JobId =j.JobId,
            Title =j.Title,
            Description =j.Description,
            Location =j.Location,
            Salary =j.Salary,
            JobType =j.JobType,
            CreatedAt =j.CreatedAt,
            CompanyName = j.Employer?.CompanyName ?? "Unknown Company"
            });
            return new PaginationResult<JobResponse>(jobDtos, pageNumber, pageSize, totalCount);
            
        }
        public async Task<JobResponse?> GetJobByIdAsync(int id)
        {
            var job = await _jobRepository.GetOneAsync(j => j.JobId == id , includes: [j => j.Employer]);
            if (job == null)
                return null;

            return new JobResponse
            {
                JobId = job.JobId,
                Title = job.Title,
                Description = job.Description,
                Location = job.Location,
                Salary = job.Salary,
                JobType = job.JobType,
                CreatedAt = job.CreatedAt,
                CompanyName = job.Employer?.CompanyName ?? "UnKnown Company"
            };
        }
    }
}
