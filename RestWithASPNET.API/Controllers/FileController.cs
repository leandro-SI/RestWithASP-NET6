using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestWithASPNET.API.models.Dtos;
using RestWithASPNET.API.Services.Interfaces;

namespace RestWithASPNET.API.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpPost("uploadFile")]
        public async Task<IActionResult> UploadOneFile([FromForm] IFormFile file)
        {
            FileDetailDTO detail = await _fileService.SaveFileToDisk(file);

            return new OkObjectResult(detail);
        }

        [HttpPost("uploadMultipleFile")]
        public async Task<IActionResult> UploadManyFiles([FromForm] List<IFormFile> files)
        {
            List<FileDetailDTO> details = await _fileService.SaveFilesToDisk(files);

            return new OkObjectResult(details);
        }

        [HttpGet("downloadFile/{fileName}")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            byte[] buffer = _fileService.GetFile(fileName);

            if (buffer != null)
            {
                HttpContext.Response.ContentType = $"application/{Path.GetExtension(fileName).Replace(".", "")}";
                HttpContext.Response.Headers.Add("content-length", buffer.Length.ToString());
                await HttpContext.Response.Body.WriteAsync(buffer, 0, buffer.Length);
            }

            return new ContentResult();
        }
    }
}
