using System.Globalization;
using System.Security.Claims;
using JobBoard.Core.Interfaces.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JopBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;
        public JobsController(IJobService jobService) 
        {
            _jobService = jobService;
        }
        [HttpPost]
        [Authorize(Roles ="Employer")]
        public async Task<IActionResult> CreateJob([FromBody] CreateJobRequest model) 
        {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if(string.IsNullOrEmpty(userId)) 
            {
            return Unauthorized(new { message = "Invalid user token" });
            }
            var result = await _jobService.CreateJobAsync(model, userId);
            return CreatedAtAction(nameof(GetJobById) , new {id = result.JobId} , result);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Getjobs([FromQuery] int pageNumber = 1 , [FromQuery] int pageSize = 10 , [FromQuery] string? location = null, [FromQuery] string? jobType = null , [FromQuery] string? sort = null)
        {
            var result = await _jobService.GetJobsAsync( pageNumber,  pageSize,  location, jobType,  sort);
            return Ok(result);
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetJobById(int id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if(job == null)
                return NotFound(new {message = $"Job With ID {id} not found."});

            return Ok(job);
}
}
}

