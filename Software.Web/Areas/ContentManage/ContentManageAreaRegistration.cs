using System.Web.Mvc;

namespace Software.Web.Areas.ContentManage
{
    public class ContentManageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ContentManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
            this.AreaName + "_Default",
            this.AreaName + "/{controller}/{action}/{id}",
            new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
            namespaces: new string[] { "Software.Web.Areas.ContentManage.Controllers" }
            );
        }
    }
}