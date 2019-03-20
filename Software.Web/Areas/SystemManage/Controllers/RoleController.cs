using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Software.Web.Areas.SystemManage.Controllers
{
    [HandlerLogin]
    public class RoleController : Controller
    {
        private RoleApp roleApp = new RoleApp();
        private RoleAuthorizeApp roleAuthorizeApp = new RoleAuthorizeApp();
        private ModuleApp moduleApp = new ModuleApp();
        private ModuleButtonApp moduleButtonApp = new ModuleButtonApp();


        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = roleApp.GetList(keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = roleApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAuthorize]
        public ActionResult CreateModel(string keyValue)
        {
            ViewData["id"] = string.Empty;
			ViewData["title"] = "新建角色";
			RoleEntity entity = new RoleEntity();
            if (!string.IsNullOrEmpty(keyValue))
            {
                ViewData["id"] = keyValue;
				ViewData["title"] = "编辑角色";
				entity = roleApp.GetForm(keyValue);
            }
            return View(entity);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateModel(RoleEntity roleEntity, string permissionIds, string keyValue)
        {
            roleApp.SubmitForm(roleEntity, permissionIds.Split(','), keyValue);
            return Content("操作成功");
        }
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        public ActionResult DeleteForm(string keyValue)
        {
            roleApp.DeleteForm(keyValue);
            return Content("删除成功");
        }
    }
}