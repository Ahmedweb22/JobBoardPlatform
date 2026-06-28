using System.Security.Claims;
using JobBoard.Core.Interfaces.IRepositories;
using JobBoard.Core.Interfaces.IServices;
using JobBoard.Core.Interfaces.IUnitOfWorks;
using JobBoard.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JopBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Candidate")]
    public class CandidatesController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IRepository<Candidate> _candidateRebo;
        private readonly IUnitOfWork _unitOfWork;
        public CandidatesController (IFileService fileService , IRepository<Candidate> candidateRebo , IUnitOfWork unitOfWork)
        {
            _fileService = fileService;
            _candidateRebo = candidateRebo;
            _unitOfWork = unitOfWork;

        }
        [HttpPost("Upload-cv")]
        public async Task<IActionResult> UploadCV(IFormFile file)
        {
            
                var extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".pdf")
                return BadRequest(new { message = "Only PDF files are allowed for CVs." });
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var candidate = await _candidateRebo.GetOneAsync(c => c.UserId == userId);
            if (candidate == null)
            {
                return BadRequest(new { message = "Candidate profile not found." } );
            }
            if (!string.IsNullOrEmpty(candidate.CVPath))
            {
                _fileService.DeleteFile(candidate.CVPath);
            }
            var relativePath = await _fileService.SaveFileAsync(file, "Uploads/CVs");

            candidate.CVPath = relativePath;
            await _unitOfWork.CommitAsync();
            
            return Ok(new
            {
                message = "CV uploaded successfully.",
                cvPath = relativePath
            });
        }

    }
    }


