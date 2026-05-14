using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CircleApp.Persistence;
using Microsoft.EntityFrameworkCore;
using CircleApp.Data.Persistence.Entities;
using CircleApp.Models;

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
        var posts = await _context.Posts.Include(u => u.User)
                                        .OrderByDescending(n => n.DateCreated)
                                        .ToListAsync();
        return View(posts);
    }

    [HttpPost]
    public async Task<IActionResult> CreatePost(PostViewModel post)
    {
        // Get the current user (for simplicity, we assume a user with ID 1)
        int loggedInUserId = 1;
        var newPost = new Post
        {
            Content = post.Content,
            UserId = loggedInUserId,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow,
            NumOfReports = 0,
            ImageUrl = ""
        };

        await _context.Posts.AddAsync(newPost);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


}
