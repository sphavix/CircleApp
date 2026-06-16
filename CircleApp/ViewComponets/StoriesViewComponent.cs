using CircleApp.Controllers;
using CircleApp.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CircleApp.ViewComponets
{
    public class StoriesViewComponent : ViewComponent
    {
        private readonly CircleAppDbContext _context;

        public StoriesViewComponent(CircleAppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var stories = await _context.Stories.Include(u => u.User).ToListAsync();
            return View(stories);
        }
    }
}
