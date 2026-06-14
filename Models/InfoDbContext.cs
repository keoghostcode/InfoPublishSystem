using System.Data.Entity;

namespace InfoPublishSystem.Models
{
    public class InfoDbContext : DbContext
    {
        public InfoDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
