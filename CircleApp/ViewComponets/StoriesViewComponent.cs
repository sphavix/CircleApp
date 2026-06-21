using CircleApp.Controllers;
using CircleApp.Data.Services;
using CircleApp.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace CircleApp.ViewComponets
{
    public class StoriesViewComponent(IStoriesService storiesService) : ViewComponent
    {
        private readonly IStoriesService _storiesService = storiesService;
       
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var stories = await _storiesService.GetStoriesAsync();
            return View(stories);
        }
    }
}
