using RestWithASPNET.API.models.Dtos;

namespace RestWithASPNET.API.Services.Interfaces
{
    public interface IFileService
    {
        public byte[] GetFile(string fileName);
        public Task<FileDetailDTO> SaveFileToDisk(IFormFile file);
        public Task<List<FileDetailDTO>> SaveFilesToDisk(List<IFormFile> files);
    }
}
