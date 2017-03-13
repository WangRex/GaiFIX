
using System.Web;
using System.Web.Mvc;

namespace Apps.Core
{

    /// <summary>
    /// 用户验证类
    /// </summary>
    public class UserAuthorizeAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 重写自定义授权检查
        /// </summary>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["UserName"] == null) return false;
            else return true;
        }
        /// <summary>
        /// 重写未授权的 HTTP 请求处理
        /// </summary>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/User/User/Login");
        }
    }
}