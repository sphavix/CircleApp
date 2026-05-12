using CircleApp.Data.Persistence.Entities;
using System.ComponentModel.DataAnnotations;

namespace CircleApp.Persistence.Entities
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public int NumOfReports { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        // Foreign key
        public int UserId { get; set; }
        // Navigation property
        public User User { get; set; }
    }
}
