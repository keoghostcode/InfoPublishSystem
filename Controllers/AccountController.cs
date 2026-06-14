using InfoPublishSystem.Models;
using InfoPublishSystem.Models.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace InfoPublishSystem.Controllers
{
    public class AccountController : Controller
    {
        private InfoDbContext db = new InfoDbContext();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(u => u.UserName == model.UserName))
                {
                    ModelState.AddModelError("UserName", "用户名已存在");
                    return View(model);
                }

                var user = new User
                {
                    UserName = model.UserName,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                    Email = model.Email,
                    Role = "User",
                    RegisterTime = DateTime.Now
                };

                db.Users.Add(user);
                db.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == model.UserName);

                if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    Session["UserId"] = user.UserId;
                    Session["UserName"] = user.UserName;
                    Session["Role"] = user.Role;

                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "用户名或密码错误");
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
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
