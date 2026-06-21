using CircleApp.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CircleApp.Controllers
{
    public class FavoritesController(IPostsService postsService) : Controller
    {
        private readonly IPostsService _postsService = postsService;
        public async Task<IActionResult> Index()
        {
            int loggedInUserId = 1; // Replace with actual user ID retrieval logic
            var favorites = await _postsService.GetFavoritedPostsAsync(loggedInUserId);

            return View(favorites);
        }
    }
}
