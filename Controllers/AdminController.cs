using InfoPublishSystem.Filters;
using InfoPublishSystem.Models;
using InfoPublishSystem.Models.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace InfoPublishSystem.Controllers
{
    [AdminAuthorize]
    public class AdminController : Controller
    {
        private InfoDbContext db = new InfoDbContext();

        public ActionResult Index()
        {
            ViewBag.CategoryCount = db.Categories.Count();
            ViewBag.NewsCount = db.News.Count();
            ViewBag.CommentCount = db.Comments.Count();
            ViewBag.PendingCommentCount = db.Comments.Count(c => !c.IsApproved && !c.IsDeleted);
            return View();
        }

        public ActionResult Categories()
        {
            var categories = db.Categories.OrderBy(c => c.SortOrder).ToList();
            return View(categories);
        }

        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategory(Category model)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(model);
                db.SaveChanges();
                return RedirectToAction("Categories");
            }
            return View(model);
        }

        public ActionResult EditCategory(int id)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCategory(Category model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Categories");
            }
            return View(model);
        }

        public ActionResult DeleteCategory(int id)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            var newsCount = db.News.Count(n => n.CategoryId == id);
            if (newsCount > 0)
            {
                ViewBag.ErrorMessage = "该栏目下有新闻，无法删除";
                var categories = db.Categories.OrderBy(c => c.SortOrder).ToList();
                return View("Categories", categories);
            }

            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("Categories");
        }

        public ActionResult NewsManage()
        {
            var newsList = (from n in db.News
                            join c in db.Categories on n.CategoryId equals c.CategoryId
                            join u in db.Users on n.AuthorId equals u.UserId
                            orderby n.PublishTime descending
                            select new NewsListItemViewModel
                            {
                                NewsId = n.NewsId,
                                Title = n.Title,
                                PublishTime = n.PublishTime,
                                IsVisible = n.IsVisible,
                                CategoryName = c.CategoryName,
                                AuthorName = u.UserName
                            }).ToList();

            ViewBag.Categories = db.Categories.ToList();
            return View(newsList);
        }

        public ActionResult AddNews()
        {
            var model = new NewsViewModel
            {
                Categories = db.Categories.ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddNews(NewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var news = new News
                {
                    Title = model.Title,
                    Content = model.Content,
                    CategoryId = model.CategoryId,
                    AuthorId = (int)Session["UserId"],
                    PublishTime = DateTime.Now,
                    IsVisible = true
                };

                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("NewsManage");
            }

            model.Categories = db.Categories.ToList();
            return View(model);
        }

        public ActionResult EditNews(int id)
        {
            var news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }

            var model = new NewsViewModel
            {
                NewsId = news.NewsId,
                Title = news.Title,
                Content = news.Content,
                CategoryId = news.CategoryId,
                Categories = db.Categories.ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditNews(NewsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var news = db.News.Find(model.NewsId);
                if (news != null)
                {
                    news.Title = model.Title;
                    news.Content = model.Content;
                    news.CategoryId = model.CategoryId;
                    db.SaveChanges();
                }
                return RedirectToAction("NewsManage");
            }

            model.Categories = db.Categories.ToList();
            return View(model);
        }

        public ActionResult DeleteNews(int id)
        {
            var news = db.News.Find(id);
            if (news != null)
            {
                db.News.Remove(news);
                db.SaveChanges();
            }
            return RedirectToAction("NewsManage");
        }

        public ActionResult CommentAudit()
        {
            var comments = (from cm in db.Comments
                            where !cm.IsApproved && !cm.IsDeleted
                            join n in db.News on cm.NewsId equals n.NewsId
                            join u in db.Users on cm.UserId equals u.UserId
                            orderby cm.CreateTime descending
                            select new CommentViewModel
                            {
                                CommentId = cm.CommentId,
                                Content = cm.Content,
                                CreateTime = cm.CreateTime,
                                NewsTitle = n.Title,
                                UserName = u.UserName
                            }).ToList();

            return View(comments);
        }

        [HttpPost]
        public ActionResult AuditComment(int commentId, bool approve)
        {
            var comment = db.Comments.Find(commentId);
            if (comment != null)
            {
                comment.IsApproved = approve;
                db.SaveChanges();
            }
            return RedirectToAction("CommentAudit");
        }

        public ActionResult DeleteComment(int id)
        {
            var comment = db.Comments.Find(id);
            if (comment != null)
            {
                bool hasChildren = db.Comments.Any(c => c.ParentCommentId == id && !c.IsDeleted);

                if (hasChildren)
                {
                    comment.IsDeleted = true;
                    comment.Content = "该留言已被删除";
                }
                else
                {
                    db.Comments.Remove(comment);
                }

                db.SaveChanges();
            }
            return RedirectToAction("CommentAudit");
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
