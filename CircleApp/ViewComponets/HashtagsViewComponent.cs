using CircleApp.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CircleApp.ViewComponets
{
    public class HashtagsViewComponent : ViewComponent
    {
        private readonly CircleAppDbContext _context;
        public HashtagsViewComponent(CircleAppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var hashtags = await _context.Hashtags
                .OrderByDescending(h => h.Count)
                .Take(10)
                .ToListAsync();
            return View(hashtags);
        }
    }
}
