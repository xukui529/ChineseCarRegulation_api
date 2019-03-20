using NFine.Application.AuxiliaryManage;
using NFine.Code;
using NFine.Domain.Entity.AuxiliaryManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Software.Web.Areas.AuxiliaryManag.Controllers
{
    [HandlerLogin]
    public class LinkManageController : Controller
    {
        private LinkApp linkApp = new LinkApp();
        // GET: AuxiliaryManag/LinkManage
        [HttpGet]
        public ActionResult Index(string keyword, int page = 1, int take = 10)
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public JsonResult GetGridJson(string keyword, string sidx, string sord, int limit, int offset)
        { 
            int total = 0;
            IList<LinkEntity> entityList = linkApp.GetDataList((offset / limit) + 1, limit, keyword, out   total);
            var totalCount = total;
            var pageSize = entityList.ToList();
            return Json(new { total = totalCount, rows = pageSize }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = linkApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HandlerAuthorize]
        public ActionResult CreateModel(string keyValue)
        {
            ViewData["id"] = string.Empty;
            ViewData["title"] = "新建链接";
			LinkEntity entity = new LinkEntity();
            if (!string.IsNullOrEmpty(keyValue))
            {
                ViewData["id"] = keyValue;
                ViewData["title"] = "编辑链接";
				entity = linkApp.GetForm(keyValue);
            }
            return View(entity);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateModel(LinkEntity entity, string keyValue)
        {
			entity.F_DeleteMark = false;
			linkApp.SubmitForm(entity, keyValue);
            return Content("操作成功");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult DeleteForm(string keyValue)
        {
            linkApp.DeleteForm(keyValue);
            return Content("删除成功");
        }
    }
}