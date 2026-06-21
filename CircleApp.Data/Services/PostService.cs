
using CircleApp.Data.Persistence.Entities;
using CircleApp.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CircleApp.Data.Services
{
    public class PostService(CircleAppDbContext context) : IPostsService
    {
        private readonly CircleAppDbContext _context = context;

        public async Task<List<Post>> GetPostsForUserAsync(int userId)
        {
            int loggedInUserId = 1; // For simplicity, we assume a user with ID 1
            var posts = await _context.Posts.Where(x => (!x.isPrivate || x.UserId == loggedInUserId) && x.Reports.Count < 5 && !x.isDeleted)
                                            .Include(u => u.User)
                                            .Include(p => p.Likes)
                                            .Include(f => f.Favourites)
                                            .Include(r => r.Reports)
                                            .Include(p => p.Comments)
                                            .ThenInclude(c => c.User)
                                            .OrderByDescending(n => n.DateCreated)
                                            .ToListAsync();
            return posts;
        }

        public async Task<List<Post>> GetFavoritedPostsAsync(int userId)
        {
            var favoritedPosts = await _context.Favourites
                .Where(f => f.UserId == userId
                    && !f.Post.isDeleted
                    && f.Post.Reports.Count < 5)
                .Include(f => f.Post)
                    .ThenInclude(p => p.Reports)
                .Include(f => f.Post)
                    .ThenInclude(p => p.Comments)
                        .ThenInclude(c => c.User)
                .Include(f => f.Post)
                    .ThenInclude(p => p.Likes)
                .Select(f => f.Post)
                .OrderByDescending(n => n.DateCreated)
                .ToListAsync();
            return favoritedPosts;
        }

        public async Task<Post> CreatePostAsync(Post post)
        {
            // Save the new post to the database
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return post;
        }

        public async Task<Post> DeletePostAsync(int postId)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == postId);
            if (post != null)
            {
                post.isDeleted = true;
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            }
            return post;
        }

        public async Task<Post> GetPostByIdAsync(int postId)
        {
            var post = await _context.Posts
                .Include(u => u.User)
                .Include(p => p.Likes)
                .Include(f => f.Favourites)
                .Include(r => r.Reports)
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(x => x.Id == postId);
            if (post == null)
            {
                throw new ArgumentNullException($"Post with ID {postId} not found.");
            }
            return post;
        }


        public async Task AddPostCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }

        public async Task RemovePostCommentAsync(int commentId)
        {
           var comment = _context.Comments.FirstOrDefault(x => x.Id == commentId);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ReportPostAsync(int postId, int userId)
        {
            // Create a new comment entity and save it to the database
            var newReport = new Report
            {
                UserId = userId,
                PostId = postId,
                DateCreated = DateTime.UtcNow
            };
            await _context.Reports.AddAsync(newReport);
            await _context.SaveChangesAsync();
        }

        public async Task TogglePostFavouriteAsync(int postId, int userId)
        {
            // Check if the like already exists for the given post and user
            var existingFavourite = await _context.Favourites
                                            .Where(l => l.PostId == postId && l.UserId == userId)
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
                    PostId = postId,
                    UserId = userId,
                    DateCreated = DateTime.UtcNow
                };
                await _context.Favourites.AddAsync(newFavourite);
            }
            await _context.SaveChangesAsync();
        }

        public async Task TogglePostLikeAsync(int postId, int userId)
        {
            // Check if the like already exists for the given post and user
            var existingLike = await _context.Likes
                                            .Where(l => l.PostId == postId && l.UserId == userId)
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
                    PostId = postId,
                    UserId = userId
                };
                await _context.Likes.AddAsync(newLike);
            }
            await _context.SaveChangesAsync();
        }

        public async Task ToggleVisibilityAsync(int postId, int userId)
        {
            // Check if the like already exists for the given post and user
            var post = await _context.Posts.FirstOrDefaultAsync(l => l.Id == postId && l.UserId == userId);
            if (post != null)
            {
                // toggle post
                post.isPrivate = !post.isPrivate;
                _context.Posts.Update(post);
                await _context.SaveChangesAsync();
            }
        }
    }
}
