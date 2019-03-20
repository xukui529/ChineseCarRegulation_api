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
    public class MemberManageController : Controller
    {
        // GET: SystemManage/MemberManage
        private MemberApp memberApp = new MemberApp();
        [HttpGet]
        public ActionResult Index(string keyword, int page = 1, int take = 10)
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public JsonResult GetGridJson(string keyword,int type, int limit, int offset)
        {
            int total;
            IList<MemberEntity> entityList = memberApp.
                GetDataList((offset / limit) + 1, limit,   keyword,   type, out   total) ;
            var totalCount = total;
            var pageSize = entityList.ToList();
            return Json(new { total = totalCount, rows = pageSize }, JsonRequestBehavior.AllowGet);
        }

        [HandlerAuthorize]
        public ActionResult CreateModel(string keyValue)
        {
            ViewData["id"] = string.Empty;
            MemberEntity entity = new MemberEntity();
			ViewData["title"] = "新建会员";
			if (!string.IsNullOrEmpty(keyValue))
            {
                ViewData["id"] = keyValue;
				ViewData["title"] = "编辑会员";
				entity = memberApp.GetForm(keyValue);
            }
            return View(entity);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateModel(MemberEntity entity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.F_CreatorTime = new DateTime();
                if (entity.F_HYType == 0)
                {
                    entity.F_State = 1;
                    entity.F_TimesState = 0;
                }
                else
                {
                    entity.F_State = 3;
                    entity.F_TimesState = 1;
                }
               
                entity.F_DeleteMark = false;
                entity.F_UseState = 1;
            }
            memberApp.SubmitForm(entity, keyValue);
            return Content("操作成功");
        }

        public ActionResult UpdateState(string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                memberApp.UpdateState(keyValue);
                return Content("操作成功");
            }
            else
            {
                return Content("操作失败");
            }
        }
    }
}