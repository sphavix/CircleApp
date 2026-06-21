using CircleApp.Data.Helpers.Enums;
using CircleApp.Data.Persistence.Entities;
using CircleApp.Data.Services;
using CircleApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CircleApp.Controllers
{
    public class StoriesController(ILogger<HomeController> logger,
                                   IStoriesService storiesService,
                                   IFilesService filesService) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IStoriesService _storiesService = storiesService;
        private readonly IFilesService _filesService = filesService;

        [HttpPost]
        public async Task<IActionResult> CreateStory(StoriesViewModel model)
        {
            int loggedInUserId = 1; // For simplicity, we assume a user with ID 1

            var imageUploadPath = await _filesService.UploadImageAsync(model.Image, ImageFileType.StoryImage);

            var newStory = new Story
            {
                UserId = loggedInUserId,
                isDeleted = false,
                ImageUrl = imageUploadPath,
                DateCreated = DateTime.UtcNow
            };

            // create a new post for the story
            await _storiesService.CreateStoryAsync(newStory);

            return RedirectToAction("Index", "Home");
        }
    }
}
