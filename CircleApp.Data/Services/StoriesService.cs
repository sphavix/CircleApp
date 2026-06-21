using CircleApp.Data.Persistence.Entities;
using CircleApp.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CircleApp.Data.Services
{
    public class StoriesService(CircleAppDbContext context) : IStoriesService
    {
        private readonly CircleAppDbContext _context = context;
        public async Task<List<Story>> GetStoriesAsync()
        {
            var stories = await _context.Stories
               .Where(c => c.DateCreated >= DateTime.UtcNow.AddHours(-24))
               .Include(u => u.User).ToListAsync();
            return stories;
        }
        public async Task<Story> CreateStoryAsync(Story story, IFormFile Image)
        {
            if (Image != null && Image.Length > 0)
            {
                // save the image to wwwroot/images and get the URL

                var rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                if (Image.ContentType.Contains("image"))
                {
                    string rootFolderPathImages = Path.Combine(rootFolderPath, "images/stories");
                    Directory.CreateDirectory(rootFolderPathImages);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Image.FileName);
                    var filePath = Path.Combine(rootFolderPathImages, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    // set the image URL to be used in the post
                    story.ImageUrl = "/images/stories/" + fileName;
                }
            }
            await _context.Stories.AddAsync(story);
            await _context.SaveChangesAsync();

            return story;
        }

    }
}
