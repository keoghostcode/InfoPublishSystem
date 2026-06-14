using InfoPublishSystem.Models;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace InfoPublishSystem.Controllers
{
    public class HomeController : Controller
    {
        private InfoDbContext db = new InfoDbContext();

        public ActionResult Index(int? categoryId)
        {
            var categories = db.Categories.OrderBy(c => c.SortOrder).ToList();
            ViewBag.Categories = categories;

            IQueryable<News> newsQuery = db.News.Where(n => n.IsVisible);

            if (categoryId.HasValue)
            {
                newsQuery = newsQuery.Where(n => n.CategoryId == categoryId.Value);
                ViewBag.SelectedCategoryId = categoryId.Value;
            }

            var newsList = newsQuery
                .OrderByDescending(n => n.PublishTime)
                .ToList();

            // 为视图提供便捷映射（CategoryId -> CategoryName, AuthorId -> UserName）
            ViewBag.CategoryMap = db.Categories.ToDictionary(c => c.CategoryId, c => c.CategoryName);
            ViewBag.UserMap = db.Users.ToDictionary(u => u.UserId, u => u.UserName);

            return View(newsList);
        }

        public ActionResult Details(int id)
        {
            var news = db.News.FirstOrDefault(n => n.NewsId == id && n.IsVisible);

            if (news == null)
            {
                return HttpNotFound();
            }

            var category = db.Categories.FirstOrDefault(c => c.CategoryId == news.CategoryId);
            var author = db.Users.FirstOrDefault(u => u.UserId == news.AuthorId);

            ViewBag.CategoryName = category?.CategoryName ?? "";
            ViewBag.AuthorName = author?.UserName ?? "";

            ViewBag.IsLoggedIn = Session["UserId"] != null;
            ViewBag.UserName = Session["UserName"]?.ToString();

            return View(news);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
