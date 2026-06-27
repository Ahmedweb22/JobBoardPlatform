using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace JobBoard.Core.Interfaces.IServices
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        bool DeleteFile(string relativeFilePath);
    }
}
