using System.Web.Mvc;

namespace InfoPublishSystem.Filters
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserId"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/Login");
                return;
            }

            if (filterContext.HttpContext.Session["Role"]?.ToString() != "Admin")
            {
                filterContext.Result = new HttpNotFoundResult();
            }
        }
    }
}
