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
        int loggedInUserId = 1; // For simplicity, we assume a user with ID 1
        var posts = await _context.Posts.Where(x => (!x.isPrivate || x.UserId == loggedInUserId) && x.Reports.Count < 5)
                                        .Include(u => u.User)
                                        .Include(p => p.Likes)
                                        .Include(f => f.Favourites)
                                        .Include(r => r.Reports)
                                        .Include(p => p.Comments)
                                        .ThenInclude(c => c.User)
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

    [HttpPost]
    public async Task<IActionResult> TogglePostLike(LikePostViewModel model)
    {
        int loggedInUserId = 1; // For simplicity, we assume a user with ID 1

        // Check if the like already exists for the given post and user
        var existingLike = await _context.Likes
                                        .Where(l => l.PostId == model.PostId && l.UserId == loggedInUserId)
                                        .FirstOrDefaultAsync();
        if (existingLike != null)
        {
            // If the like already exists, remove it (unlike)
            _context.Likes.Remove(existingLike);
        }
        else
        {
            // If the like does not exist, add it (like)
            var newLike = new Like
            {
                PostId = model.PostId,
                UserId = loggedInUserId
            };
            await _context.Likes.AddAsync(newLike);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> TogglePostFavourite(FavouritePostViewModel model)
    {
        int loggedInUserId = 1; // For simplicity, we assume a user with ID 1

        // Check if the like already exists for the given post and user
        var existingFavourite = await _context.Favourites
                                        .Where(l => l.PostId == model.PostId && l.UserId == loggedInUserId)
                                        .FirstOrDefaultAsync();
        if (existingFavourite != null)
        {
            // If the like already exists, remove it (unlike)
            _context.Favourites.Remove(existingFavourite);
        }
        else
        {
            // If the like does not exist, add it (like)
            var newFavourite = new Favourite
            {
                PostId = model.PostId,
                UserId = loggedInUserId
            };
            await _context.Favourites.AddAsync(newFavourite);
        }
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> TogglePostVisibility(PostVisibilityViewModel model)
    {
        int loggedInUserId = 1; // For simplicity, we assume a user with ID 1

        // Check if the like already exists for the given post and user
        var post = await _context.Posts
                                        .FirstOrDefaultAsync(l => l.Id == model.PostId && l.UserId == loggedInUserId);
        if (post != null)
        {
            // toggle post
            post.isPrivate = !post.isPrivate;
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
        }
        
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> AddPostComment(PostCommentViewModel model)
    {
        int loggedInUserId = 1; // For simplicity, we assume a user with ID 1

        // Create a new comment entity and save it to the database
        var newComment = new Comment
        {
            Content = model.Content,
            UserId = loggedInUserId,
            PostId = model.PostId,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };
        await _context.Comments.AddAsync(newComment);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> AddPostReport(PostReportViewModel model)
    {
        int loggedInUserId = 1; // For simplicity, we assume a user with ID 1

        // Create a new comment entity and save it to the database
        var newReport = new Report
        {
            UserId = loggedInUserId,
            PostId = model.PostId,
            DateCreated = DateTime.UtcNow
        };
        await _context.Reports.AddAsync(newReport);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> RemovePostComment(RemoveCommentViewModel model)
    {
        // Find the comment to be removed
        var commentToRemove = await _context.Comments
                                            .FirstOrDefaultAsync(c => c.Id == model.CommentId);
        if (commentToRemove != null)
        {
            _context.Comments.Remove(commentToRemove);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> DeletePost(DeletePostViewModel model)
    {
        // Find the comment to be removed
        var post = await _context.Posts.FirstOrDefaultAsync(c => c.Id == model.PostId);

        if (post != null)
        {
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }


}
