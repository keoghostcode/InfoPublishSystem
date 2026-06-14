using InfoPublishSystem.Filters;
using InfoPublishSystem.Models;
using InfoPublishSystem.Models.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace InfoPublishSystem.Controllers
{
    public class CommentController : Controller
    {
        private InfoDbContext db = new InfoDbContext();

        [LoginAuthorize]
        [HttpPost]
        public JsonResult Add(int newsId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return Json(new { success = false, message = "留言内容不能为空" });
            }

            var comment = new Comment
            {
                NewsId = newsId,
                UserId = (int)Session["UserId"],
                Content = content,
                ParentCommentId = null,
                IsApproved = false,
                IsDeleted = false,
                CreateTime = DateTime.Now
            };

            db.Comments.Add(comment);
            db.SaveChanges();

            return Json(new { success = true, message = "留言提交成功，等待审核" });
        }

        [LoginAuthorize]
        [HttpPost]
        public JsonResult Reply(int newsId, int parentCommentId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return Json(new { success = false, message = "回复内容不能为空" });
            }

            var comment = new Comment
            {
                NewsId = newsId,
                UserId = (int)Session["UserId"],
                Content = content,
                ParentCommentId = parentCommentId,
                IsApproved = false,
                IsDeleted = false,
                CreateTime = DateTime.Now
            };

            db.Comments.Add(comment);
            db.SaveChanges();

            return Json(new { success = true, message = "回复提交成功，等待审核" });
        }

        [HttpGet]
        public JsonResult GetList(int newsId)
        {
            var comments = (from c in db.Comments
                            join u in db.Users on c.UserId equals u.UserId
                            where c.NewsId == newsId
                                && c.IsApproved
                                && !c.IsDeleted
                                && c.ParentCommentId == null
                            orderby c.CreateTime
                            select new
                            {
                                c.CommentId,
                                c.Content,
                                c.CreateTime,
                                UserName = u.UserName,
                                HasChildren = db.Comments.Any(child => child.ParentCommentId == c.CommentId && child.IsApproved && !child.IsDeleted)
                            })
                            .ToList()
                            .Select(c => new
                            {
                                c.CommentId,
                                c.Content,
                                CreateTime = c.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                                c.UserName,
                                c.HasChildren
                            })
                            .ToList();

            return Json(comments, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetReplies(int parentCommentId)
        {
            var replies = (from c in db.Comments
                           join u in db.Users on c.UserId equals u.UserId
                           where c.ParentCommentId == parentCommentId
                               && c.IsApproved
                               && !c.IsDeleted
                           orderby c.CreateTime
                           select new
                           {
                               c.CommentId,
                               c.Content,
                               c.CreateTime,
                               UserName = u.UserName
                           })
                           .ToList()
                           .Select(c => new
                           {
                               c.CommentId,
                               c.Content,
                               CreateTime = c.CreateTime.ToString("yyyy-MM-dd HH:mm"),
                               c.UserName
                           })
                           .ToList();

            return Json(replies, JsonRequestBehavior.AllowGet);
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
