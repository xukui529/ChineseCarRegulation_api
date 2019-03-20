using NFine.Application.AuxiliaryManage;
using NFine.Domain.Entity.AuxiliaryManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Software.Web.Areas.AuxiliaryManag.Controllers
{
    [HandlerLogin]
    public class DownloadManageController : Controller
    {
        private MemberDownloadApp memberDownloaApp = new MemberDownloadApp();
        // GET: AuxiliaryManag/DownloadManage
        //[HandlerAuthorize]
        [HttpGet]
        public ActionResult Index()
        {
            ViewData["id"] = string.Empty;
            MemberDownloadEntity entity = new MemberDownloadEntity();
            entity = memberDownloaApp.GetFormEntity();
            if (!string.IsNullOrEmpty(entity.F_Id))
            {
                ViewData["id"] = entity.F_Id;
            }
            return View(entity);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult Index(MemberDownloadEntity entity, string keyValue)
        {
            if (string.IsNullOrEmpty(keyValue))
            {
                entity.F_CreatorTime = new DateTime();
                entity.F_DeleteMark = false;
            }
            //MemberDownloadEntity oldEntity = memberDownloaApp.GetFormEntity();
            //entity.F_OldSeasonNumber = oldEntity.F_SeasonNumber;
            //entity.F_OldYearNumber = oldEntity.F_OldYearNumber;

            memberDownloaApp.SubmitForm(entity, keyValue);
            return Content("操作成功");
        }
    }
}