using CircleApp.Data.Persistence.Entities;
using CircleApp.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CircleApp.Persistence
{
    public class CircleAppDbContext : DbContext
    {
        public CircleAppDbContext(DbContextOptions<CircleAppDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
