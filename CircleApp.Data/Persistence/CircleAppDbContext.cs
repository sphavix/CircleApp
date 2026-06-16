using CircleApp.Data.Persistence.Entities;
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
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Favourite> Favourites { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the relationship between User and Post

            modelBuilder.Entity<User>()
                .HasMany(u => u.Posts)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Stories)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);


            // Configure the relationship between Post, User and Like
            modelBuilder.Entity<Like>()
                .HasKey(l => new { l.PostId, l.UserId });

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the relationship between Post, User and Comment
            modelBuilder.Entity<Comment>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Comment>()
                .HasOne(l => l.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the relationship between Post, User and Favourite
            modelBuilder.Entity<Favourite>()
                .HasKey(f => new { f.PostId, f.UserId });

            modelBuilder.Entity<Favourite>()
                .HasOne(f => f.Post)
                .WithMany(p => p.Favourites)
                .HasForeignKey(f => f.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Favourite>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favourites)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the relationship between Post, User and Report
            modelBuilder.Entity<Report>()
               .HasKey(f => new { f.PostId, f.UserId });

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Post)
                .WithMany(p => p.Reports)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reports)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);
        }
    }
}
