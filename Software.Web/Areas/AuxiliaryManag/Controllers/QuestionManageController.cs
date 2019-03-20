using NFine.Application.AuxiliaryManage;
using NFine.Code;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Software.Web.Areas.AuxiliaryManag.Controllers
{
    [HandlerLogin]
    public class QuestionManageController : Controller
    {
        private QuestionAnswerApp questionApp = new QuestionAnswerApp();
		// GET: AuxiliaryManag/QuestionManage
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
            IList<QuestionAnswerModel> entityList = questionApp.GetDataList((offset / limit) + 1, limit, keyword, out total);
          
            var totalCount = total;
            var pageSize = entityList.ToList();
            return Json(new { total = totalCount, rows = pageSize }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateModel(string keyword)
        {
            QuestionAnswerEntity entity = new QuestionAnswerEntity();
            ViewData["id"] = "";
			ViewData["title"] = "新建问答";
			if (!string.IsNullOrEmpty(keyword))
            {
                ViewData["id"] = keyword;
				ViewData["title"] = "编辑问答";
				entity = questionApp.GetForm(keyword);
            }
            return View(entity);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult CreateModel(QuestionAnswerEntity entity, string keyValue)
        {
			if (string.IsNullOrEmpty(keyValue))
			{
				entity.F_LastModifyTime = DateTime.Now;
			}
			string markup = Request.Form["Content"];
            Regex regex = new Regex(Configs.GetValue("ZiXunSeparator"));//以cjlovefl分割
            string[] bit = regex.Split(markup);
            if (bit.Length == 2)
            {
                entity.F_ChAnswer = bit[0];
                entity.F_EnAnswer = bit[1]; 
            }
            else if (bit.Length == 1)
            { 
                entity.F_ChAnswer = bit[0];
            } 
            //Content 里面用 == 分割了 中文内容 英文内容 
            entity.F_DeleteMark = false;
			
			questionApp.SubmitForm(entity, keyValue);
            return Content("操作成功");
        }

		[HttpPost]
		[HandlerAuthorize]
		[HandlerAjaxOnly]
		public ActionResult DeleteForm(string keyValue)
		{
			questionApp.DeleteForm(keyValue);
			return Content("删除成功");
		}
	}
}