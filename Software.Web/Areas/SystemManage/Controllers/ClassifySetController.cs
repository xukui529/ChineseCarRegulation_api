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
    public class ClassifySetController : Controller
    {
		private ClassifySetApp classifySetApp = new ClassifySetApp();

		// GET: SystemManage/ClassifySet
		[HttpGet]
		public ActionResult Index()
        {
            return View();
        }

		[HttpGet]
		[HandlerAjaxOnly]
		public JsonResult GetGridJson(string keyword, string sidx, string sord, int limit, int offset)
		{
			Pagination pagination = new Pagination();
			pagination.page = (offset / limit) + 1;
			pagination.rows = limit;
			pagination.sord = "desc";
			pagination.sidx = "F_CreatorTime";
			IList<ClassifySetEntity> entityList = classifySetApp.GetDataList(pagination, keyword);
			var totalCount = entityList.Count();
			var pageSize = entityList.ToList();
			return Json(new { total = totalCount, rows = pageSize }, JsonRequestBehavior.AllowGet);
		}

		[HandlerAuthorize]
		public ActionResult CreateModel(string keyValue)
		{
			ViewData["id"] = string.Empty;
			ViewData["title"] = "修改分类";
			ClassifySetEntity entity = new ClassifySetEntity();
			if (!string.IsNullOrEmpty(keyValue))
			{
				ViewData["id"] = keyValue;
				ViewData["title"] = "新建分类";
				entity = classifySetApp.GetForm(keyValue);
			}
			return View(entity);
		}

		[HttpPost]
		[HandlerAjaxOnly]
		[HandlerAuthorize]
		[ValidateAntiForgeryToken]
		public ActionResult CreateModel(ClassifySetEntity entity, string keyValue)
		{
			if (classifySetApp.CheckName(entity.Name))
			{
				return Content("已经存在");
			}
			else
			{
				entity.F_DeleteMark = false;
				classifySetApp.SubmitForm(entity, keyValue);
				return Content("操作成功");
			}
		}
	}
}