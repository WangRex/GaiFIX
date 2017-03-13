using System.Web.Mvc;

namespace Apps.Web.Areas.Calendar
{
    public class CalendarAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Calendar";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
               "CalendarGlobalization", // 路由名称
               "{lang}/Calendar/{controller}/{action}/{id}", // 带有参数的 URL
               new { lang = "zh", controller = "Home", action = "Index", id = UrlParameter.Optional }, // 参数默认值
               new { lang = "^[a-zA-Z]{2}(-[a-zA-Z]{2})?$" }    //参数约束
           );
            context.MapRoute(
                "Calendar_default",
                "Calendar/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
