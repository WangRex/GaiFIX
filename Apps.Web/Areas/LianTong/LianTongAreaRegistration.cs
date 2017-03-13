using System.Web.Mvc;

namespace Apps.Web.Areas.LianTong
{
    public class LianTongAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "LianTong";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {

            context.MapRoute(
               "LianTongGlobalization", // 路由名称
               "{lang}/LianTong/{controller}/{action}/{id}", // 带有参数的 URL
               new { lang = "zh", controller = "Home", action = "Index", id = UrlParameter.Optional }, // 参数默认值
               new { lang = "^[a-zA-Z]{2}(-[a-zA-Z]{2})?$" }    //参数约束
           );
            context.MapRoute(
                "LianTong_default",
                "LianTong/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
