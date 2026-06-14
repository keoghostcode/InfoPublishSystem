using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace InfoPublishSystem.Models
{
    public class DbInitializer : DropCreateDatabaseIfModelChanges<InfoDbContext>
    {
        protected override void Seed(InfoDbContext context)
        {
            var admin = new User
            {
                UserName = "admin",
                Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Email = "admin@example.com",
                Role = "Admin",
                RegisterTime = DateTime.Now
            };
            context.Users.Add(admin);

            var categories = new List<Category>
            {
                new Category { CategoryName = "新闻动态", SortOrder = 1 },
                new Category { CategoryName = "通知公告", SortOrder = 2 },
                new Category { CategoryName = "行业资讯", SortOrder = 3 },
                new Category { CategoryName = "政策法规", SortOrder = 4 }
            };
            context.Categories.AddRange(categories);

            context.SaveChanges();
        }
    }
}
