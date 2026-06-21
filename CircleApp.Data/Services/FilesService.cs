using CircleApp.Data.Helpers.Enums;
using Microsoft.AspNetCore.Http;

namespace CircleApp.Data.Services
{
    public class FilesService : IFilesService
    {
        public async Task<string> UploadImageAsync(IFormFile file, ImageFileType imageFileType)
        {
            string filePathUpload = imageFileType switch
            {
                ImageFileType.PostImage => "/images/posts",
                ImageFileType.StoryImage => "/images/stories",
                ImageFileType.ProfileImage => "/images/profile-images",
                ImageFileType.CoverImage => "/images/cover-images",
                _ => throw new ArgumentOutOfRangeException(nameof(imageFileType), $"Not expected image file type value: {imageFileType}"),
            };

            if (file != null && file.Length > 0)
            {
                // save the image to wwwroot/images and get the URL

                var rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                if (file.ContentType.Contains("image"))
                {
                    string rootFolderPathImages = Path.Combine(rootFolderPath, filePathUpload);
                    Directory.CreateDirectory(rootFolderPathImages);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(rootFolderPathImages, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return $"/{filePathUpload}/{fileName}";
                }
            }

            return "";
        }
    }
}
