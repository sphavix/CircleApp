using CircleApp.Data.Persistence.Entities;
using CircleApp.Data.Services;
using CircleApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CircleApp.Controllers
{
    public class StoriesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStoriesService _storiesService;

        public StoriesController(ILogger<HomeController> logger, IStoriesService storiesService)
        {
            _logger = logger;
            _storiesService = storiesService;
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
            await _storiesService.CreateStoryAsync(newStory, model.Image);

            return RedirectToAction("Index", "Home");
        }
    }
}
