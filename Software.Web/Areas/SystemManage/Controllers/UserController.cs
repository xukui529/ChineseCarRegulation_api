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
    public class UserController : Controller
    {
        private UserApp userApp = new UserApp();
        private UserLogOnApp userLogOnApp = new UserLogOnApp();
        // GET: SystemManage/User
        [HttpGet] 
        public ActionResult Index(string keyword, int page = 1, int take = 10)
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public JsonResult GetGridJson(string keyword,string sidx,string sord,int limit, int offset)
        {
            int total = 0;
            //Pagination pagination = new Pagination();
            //pagination.page = (offset / limit) + 1;
            //pagination.rows = limit;
            //pagination.sord = "asc";
            //pagination.sidx = "F_DepartmentId asc,F_CreatorTime desc";
            IList<UserEntity> entityList = userApp.GetDataList((offset / limit) + 1, limit, keyword, out total); 
            var totalCount = total;
            var pageSize = entityList.ToList();
            return Json(new { total = totalCount, rows = pageSize }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = userApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HandlerAuthorize]
        public ActionResult CreateModel(string keyValue)
        {
            ViewData["id"] = string.Empty;
			ViewData["title"] = "新建员工";
			UserEntity entity = new UserEntity();
            if (!string.IsNullOrEmpty(keyValue))
            {
                ViewData["id"] = keyValue;
				ViewData["title"] = "编辑员工";
				entity = userApp.GetForm(keyValue);
            }
            return View(entity);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateModel(UserEntity userEntity, UserLogOnEntity userLogOnEntity, string keyValue)
        {
			userEntity.F_EnabledMark = true;
			userApp.SubmitForm(userEntity, userLogOnEntity, keyValue);
            return Content("操作成功");
        }

        // 修改密码
        [HandlerAuthorize]
        public ActionResult UpdatePwdModel(string keyValue,string account, string realName)
        {
            ViewData["id"] = keyValue;
            ViewData["account"] = account;
            ViewData["realName"] = realName;
            return View();
        }

        // 重置密码
        [HttpPost]
        [HandlerAjaxOnly]
        //[HandlerAuthorize]
        //[ValidateAntiForgeryToken]
        public ActionResult SubmitRevisePassword(string userPassword, string keyValue)
        {
            userLogOnApp.RevisePassword(userPassword, keyValue);
            return Content("操作成功");
        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult DeleteForm(string keyValue)
        {
            userApp.DeleteForm(keyValue);
            return Content("删除成功");
        }
    }
}