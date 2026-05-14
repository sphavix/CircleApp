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

        // check if an image was uploaded
        if(post.Image != null && post.Image.Length > 0)
        {
            // save the image to wwwroot/images and get the URL
            
            var rootFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            if (post.Image.ContentType.Contains("image"))
            {
                string rootFolderPathImages = Path.Combine(rootFolderPath, "images/uploaded");
                Directory.CreateDirectory(rootFolderPathImages);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(post.Image.FileName);
                var filePath = Path.Combine(rootFolderPathImages, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await post.Image.CopyToAsync(stream);
                }

                // set the image URL to be used in the post
                newPost.ImageUrl = "/images/uploaded/" + fileName;
            }
        }
        await _context.Posts.AddAsync(newPost);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


}
