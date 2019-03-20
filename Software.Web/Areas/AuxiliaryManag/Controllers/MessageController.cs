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
    public class MessageController : Controller
    {
        private MessageApp messageApp = new MessageApp();
        // GET: AuxiliaryManag/Message
        [HttpGet]
        public ActionResult Index(string keyword, int page = 1, int take = 10)
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public JsonResult GetGridJson(int index, string sidx, string sord, int limit, int offset)
        {
			string keyword = string.Empty;
			if (index == 2)
			{
				keyword = "DFWDGM";
			}
			if (index == 3)
			{
				keyword = "YJYJY";
			} 
            int total = 0;
            IList<MessageEntity> entityList = messageApp.GetDataList((offset / limit) + 1, limit, keyword, out total); 
            var totalCount = total;
            var pageSize = entityList.ToList();
            return Json(new { total = totalCount, rows = pageSize }, JsonRequestBehavior.AllowGet);
        }

        [HandlerAuthorize]
        public ActionResult CreateModel(string keyValue)
        {
            MessageEntity entity = new MessageEntity();
            ViewData["id"] = "";
			ViewData["title"] = "编辑留言";
			if (!string.IsNullOrEmpty(keyValue))
            {
                ViewData["id"] = keyValue;
				ViewData["title"] = "新建留言";
				entity = messageApp.GetForm(keyValue);
            }
            return View(entity);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
		[ValidateInput(false)]
		public ActionResult CreateModel(MessageEntity entity, string keyValue)
        {
			string markup = Request.Form["Content"];
			entity.F_DeleteMark = false;
			entity.F_Memo = markup;
			if (string.IsNullOrEmpty(keyValue))
			{
				entity.F_State = false;
			}
			messageApp.SubmitForm(entity, keyValue);
            return Content("操作成功");
        }

        public ActionResult DetailModel(string keyValue)
        {
            MessageEntity entity = new MessageEntity();
            entity = messageApp.GetForm(keyValue);
            return View(entity);
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult UpdateStateForm(string keyValue)
        {
            messageApp.UpdateStateForm(keyValue);
            return Content("操作成功");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult DeleteForm(string keyValue)
        {
            messageApp.DeleteForm(keyValue);
            return Content("删除成功");
        }

    }
}