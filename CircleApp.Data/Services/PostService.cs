
using CircleApp.Data.Persistence.Entities;
using CircleApp.Persistence;
using Microsoft.AspNetCore.Http;

namespace CircleApp.Data.Services
{
    public class PostService(CircleAppDbContext context) : IPostsService
    {
        private readonly CircleAppDbContext _context = context;

        public Task AddPostCommentAsync(Comment comment)
        {
            throw new NotImplementedException();
        }

        public Task CreatePostAsync(Post post, IFormFile Image)
        {
            throw new NotImplementedException();
        }

        public Task DeletePostAsync(int postId)
        {
            throw new NotImplementedException();
        }

        public Task<Post> GetPostByIdAsync(int postId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Post>> GetPostsForUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task RemovePostCommentAsync(int commentId)
        {
            throw new NotImplementedException();
        }

        public Task ReportPostAsync(int postId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task TogglePostFavouriteAsync(int postId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task TogglePostLikeAsync(int postId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task ToggleVisibilityAsync(int postId, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
