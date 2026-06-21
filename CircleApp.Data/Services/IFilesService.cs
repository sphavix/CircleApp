using CircleApp.Data.Helpers.Enums;
using Microsoft.AspNetCore.Http;

namespace CircleApp.Data.Services
{
    public interface IFilesService
    {
        Task<string> UploadImageAsync(IFormFile file, ImageFileType imageFileType);
    }
}
