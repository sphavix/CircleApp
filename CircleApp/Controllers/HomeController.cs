using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CircleApp.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CircleApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CircleAppDbContext _context;

    public HomeController(ILogger<HomeController> logger, CircleAppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var posts = await _context.Posts.Include(u => u.User).ToListAsync();
        return View(posts);
    }

    
}
