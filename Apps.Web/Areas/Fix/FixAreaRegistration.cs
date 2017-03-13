using System.Web.Mvc;

namespace Apps.Web.Areas.Fix
{
    public class FixAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Fix";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                  "FixGlobalization", // 路由名称
                  "{lang}/Fix/{controller}/{action}/{id}", // 带有参数的 URL
                  new { lang = "zh", controller = "Home", action = "Index", id = UrlParameter.Optional }, // 参数默认值
                  new { lang = "^[a-zA-Z]{2}(-[a-zA-Z]{2})?$" }    //参数约束
              );
            context.MapRoute(
                "Fix_default",
                "Fix/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}