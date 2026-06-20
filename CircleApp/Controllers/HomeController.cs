using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CircleApp.Persistence;
using Microsoft.EntityFrameworkCore;
using CircleApp.Data.Persistence.Entities;
using CircleApp.Models;
using CircleApp.Data.Helpers;

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
        
        return View();
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

        

        

        // Find and store hashtags in the database
        var postHastags = HashtagHelper.ExtractHashtags(post.Content);
        foreach(var hashtag in postHastags)
        {
            var existingHashtag = await _context.Hashtags.FirstOrDefaultAsync(h => h.Name == hashtag);
            if (existingHashtag != null)
            {
                existingHashtag.Count++;
                existingHashtag.DateUpdated = DateTime.UtcNow;

                _context.Hashtags.Update(existingHashtag);
                await _context.SaveChangesAsync();
            }
            else
            {
                var newHashtag = new Hashtag
                {
                    Name = hashtag,
                    Count = 1,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow
                };

                await _context.Hashtags.AddAsync(newHashtag);
                await _context.SaveChangesAsync();
            }
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> TogglePostLike(LikePostViewModel model)
    {
        int loggedInUserId = 1; // For simplicity, we assume a user with ID 1

        
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> TogglePostFavourite(FavouritePostViewModel model)
    {
        int loggedInUserId = 1; // For simplicity, we assume a user with ID 1

        
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> TogglePostVisibility(PostVisibilityViewModel model)
    {
        int loggedInUserId = 1; // For simplicity, we assume a user with ID 1

        
        
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> AddPostComment(PostCommentViewModel model)
    {
        int loggedInUserId = 1; // For simplicity, we assume a user with ID 1

        
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> AddPostReport(PostReportViewModel model)
    {
        int loggedInUserId = 1; // For simplicity, we assume a user with ID 1

       
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
            post.isDeleted = true;
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            // update hashtags in the database
            var hashTag = HashtagHelper.ExtractHashtags(post.Content);
            foreach(var hashtag in hashTag)
            {
                var existingHashtag = await _context.Hashtags.FirstOrDefaultAsync(h => h.Name == hashtag);
                if (existingHashtag != null)
                {
                    existingHashtag.Count--;
                    existingHashtag.DateUpdated = DateTime.UtcNow;

                    _context.Hashtags.Update(existingHashtag);
                    await _context.SaveChangesAsync();
                }
            }
        }
        return RedirectToAction(nameof(Index));
    }


}
