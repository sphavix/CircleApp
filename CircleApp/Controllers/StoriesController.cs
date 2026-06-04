using CircleApp.Data.Persistence.Entities;
using CircleApp.Models;
using CircleApp.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace CircleApp.Controllers
{
    public class StoriesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CircleAppDbContext _context;

        public StoriesController(ILogger<HomeController> logger, CircleAppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var stories = await _context.Stories.Include(u => u.User).ToListAsync();
            return View(stories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStory(StoriesViewModel model)
        {
            int loggedInUserId = 1; // For simplicity, we assume a user with ID 1

            var newStory = new Story
            {
                UserId = loggedInUserId,
                isDeleted = false,
                DateCreated = DateTime.UtcNow
            };

            // create a new post for the story
            if (model.Image != null && model.Image.Length > 0)
            {
                // save the image to wwwroot/images and get the URL

                var rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                if (model.Image.ContentType.Contains("image"))
                {
                    string rootFolderPathImages = Path.Combine(rootFolderPath, "images/stories");
                    Directory.CreateDirectory(rootFolderPathImages);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.Image.FileName);
                    var filePath = Path.Combine(rootFolderPathImages, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Image.CopyToAsync(stream);
                    }

                    // set the image URL to be used in the post
                    newStory.ImageUrl = "/images/uploaded/" + fileName;
                }
            }
            await _context.Stories.AddAsync(newStory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
