
using CircleApp.Data.Persistence.Entities;
using Microsoft.AspNetCore.Http;

namespace CircleApp.Data.Services
{
    public interface IPostsService
    {
        Task<List<Post>> GetPostsForUserAsync(int userId);
        Task<Post> GetPostByIdAsync(int postId);
        Task CreatePostAsync(Post post, IFormFile Image);
        Task DeletePostAsync(int postId);

        Task AddPostCommentAsync(Comment comment);
        Task RemovePostCommentAsync(int commentId);

        Task TogglePostLikeAsync(int postId, int userId);
        Task TogglePostFavouriteAsync(int postId, int userId);
        Task ToggleVisibilityAsync(int postId, int userId);
        Task ReportPostAsync(int postId, int userId);
    }
}
