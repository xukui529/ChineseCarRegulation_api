using System.Web.Mvc;

namespace Software.Web.Areas.AuxiliaryManag
{
    public class AuxiliaryManagAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "AuxiliaryManag";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                        this.AreaName + "_Default",
                        this.AreaName + "/{controller}/{action}/{id}",
                        new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
                        namespaces: new string[] { "Software.Web.Areas.AuxiliaryManag.Controllers" }
                        );
        }
    }
}