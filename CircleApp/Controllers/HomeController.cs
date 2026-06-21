using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CircleApp.Persistence;
using Microsoft.EntityFrameworkCore;
using CircleApp.Data.Persistence.Entities;
using CircleApp.Models;
using CircleApp.Data.Helpers;
using CircleApp.Data.Services;

namespace CircleApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IPostsService _postService;
    private readonly IHashtagsService _hashtagService;

    public HomeController(ILogger<HomeController> logger,
                          IPostsService postsService,
                          IHashtagsService hashtagService)
    {
        _logger = logger;
        _postService = postsService;
        _hashtagService = hashtagService;
    }

    public async Task<IActionResult> Index()
    {
        int loggedInUserId = 1;
        var posts = await _postService.GetPostsForUserAsync(loggedInUserId);

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

        await _postService.CreatePostAsync(newPost, post.Image);

        // Find and store hashtags in the database
        await _hashtagService.CreateHashtagForNewPostAsync(post.Content);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> TogglePostLike(LikePostViewModel model)
    {
        int loggedInUserId = 1; // For simplicity, we assume a user with ID 1

        await _postService.TogglePostLikeAsync(model.PostId, loggedInUserId);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> TogglePostFavourite(FavouritePostViewModel model)
    {
        int loggedInUserId = 1; // For simplicity, we assume a user with ID 1

        await _postService.TogglePostFavouriteAsync(model.PostId, loggedInUserId);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> TogglePostVisibility(PostVisibilityViewModel model)
    {
        int loggedInUserId = 1; // For simplicity, we assume a user with ID 1

        await _postService.ToggleVisibilityAsync(model.PostId, loggedInUserId);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> AddPostComment(PostCommentViewModel model)
    {
        int loggedInUserId = 1; // For simplicity, we assume a user with ID 1

        var comment = new Comment
        {
            PostId = model.PostId,
            UserId = loggedInUserId,
            Content = model.Content,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow
        };

        await _postService.AddPostCommentAsync(comment);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> AddPostReport(PostReportViewModel model)
    {
        int loggedInUserId = 1; // For simplicity, we assume a user with ID 1

        await _postService.ReportPostAsync(model.PostId, loggedInUserId);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> RemovePostComment(RemoveCommentViewModel model)
    {
        await _postService.RemovePostCommentAsync(model.CommentId);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> DeletePost(DeletePostViewModel model)
    {
        var post = await _postService.DeletePostAsync(model.PostId);

        // update hashtags in the database
        await _hashtagService.DeleteHashtagForPostAsync(post.Content);

        return RedirectToAction(nameof(Index));
    }
}
