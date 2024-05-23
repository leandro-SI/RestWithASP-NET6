using RestWithASPNET.API.models.Dtos;
using RestWithASPNET.API.Services.Interfaces;

namespace RestWithASPNET.API.Services
{
    public class FileService : IFileService
    {
        private readonly string _basePath;
        private readonly IHttpContextAccessor _context;

        public FileService(IHttpContextAccessor context)
        {
            _context = context;
            _basePath = Directory.GetCurrentDirectory() + "\\UploadDir\\";
        }

        public byte[] GetFile(string fileName)
        {
            var filePath = _basePath + fileName;
            return File.ReadAllBytes(filePath);
        }

        public async Task<FileDetailDTO> SaveFileToDisk(IFormFile file)
        {
            FileDetailDTO fileDetail = new FileDetailDTO();

            var fileType = Path.GetExtension(file.FileName);
            var baseurl = _context.HttpContext.Request.Host;

            if (fileType.ToLower() == ".pdf" || fileType.ToLower() == ".jpg" || fileType.ToLower() == ".png")
            {
                var docName = Path.GetFileName(file.FileName);
                if (file != null && file.Length > 0)
                {
                    var destination = Path.Combine(_basePath, "", docName);
                    fileDetail.DocName = docName;
                    fileDetail.DocType = fileType;
                    fileDetail.DocUrl = Path.Combine(baseurl + "/api/v1/file/" + fileDetail.DocName);

                    using var stream = new FileStream(destination, FileMode.Create);

                    await file.CopyToAsync(stream);
                }
            }

            return fileDetail;
        }

        public async Task<List<FileDetailDTO>> SaveFilesToDisk(List<IFormFile> files)
        {
            List<FileDetailDTO> list = new List<FileDetailDTO>();

            foreach (var file in files)
            {
                list.Add(await  SaveFileToDisk(file));
            }

            return list;
        }
    }
}
