using CircleApp.Data.Persistence.Entities;
using CircleApp.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CircleApp.Data.Services
{
    public class StoriesService(CircleAppDbContext context) : IStoriesService
    {
        private readonly CircleAppDbContext _context = context;
        public async Task<List<Story>> GetStoriesAsync()
        {
            var stories = await _context.Stories
               .Where(c => c.DateCreated >= DateTime.UtcNow.AddHours(-24))
               .Include(u => u.User).ToListAsync();
            return stories;
        }
        public async Task<Story> CreateStoryAsync(Story story)
        {
            await _context.Stories.AddAsync(story);
            await _context.SaveChangesAsync();

            return story;
        }

    }
}
