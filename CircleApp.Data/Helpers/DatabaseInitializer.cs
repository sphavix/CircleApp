using CircleApp.Data.Persistence.Entities;
using CircleApp.Persistence;
using CircleApp.Persistence.Entities;
namespace CircleApp.Data.Helpers
{
    public static class DatabaseInitializer
    {
        public static async Task SeedDataAsync(CircleAppDbContext context)
        {
            if(!context.Users.Any() && !context.Posts.Any())
            {
                var user = new User()
                {
                    FullName = "Spha Zolakhe",
                    ProfilePictureUrl = "https://images.unsplash.com/photo-1778336259903-90ca56d98dab?q=80&w=1168&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D"
                };

                await context.Users.AddAsync(user);
                await context.SaveChangesAsync();

                var post = new Post()
                {
                    Content = "This is my first post on CircleApp! This has been populated from the code by DB Migrations.",
                    ImageUrl = "",
                    NumOfReports = 0,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    UserId = user.Id
                };
                var postImg = new Post()
                {
                    Content = "This is my first post on CircleApp! This has been populated from the code by DB Migrations. This Post has an image",
                    ImageUrl = "https://images.unsplash.com/photo-1778431193240-72e7d9c4cd38?q=80&w=1227&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D",
                    NumOfReports = 0,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    UserId = user.Id
                };
                await context.Posts.AddRangeAsync(post, postImg);
                await context.SaveChangesAsync();
            }
        }
    }
}
