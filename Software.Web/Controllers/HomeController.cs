using NFine.Application.AuxiliaryManage;
using NFine.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Software.Web.Controllers
{
    [HandlerLogin]
    public class HomeController : ControllerBase
    {
        private DocumentVisitInfoApp documentVisitInfoApp = new DocumentVisitInfoApp();
        private DownloadInfoApp downloadInfoApp = new DownloadInfoApp();
        private ReleaseInfoApp releaseInfoApp = new ReleaseInfoApp();
        private DocumentSearchKeysApp documentSearchKeysApp = new DocumentSearchKeysApp();
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page."; 
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page."; 
            return View();
        }

        /// <summary>
        /// 访问统计
        /// </summary>
        /// <param name="searchType">1今日 2本周 3 本月</param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly] 
        public ActionResult GetDocumentVisitInfoStat(int searchType, DateTime beginTime, DateTime endTime)
        {
            var tempTime = DateTime.Now;
            switch (searchType)
            {
                case 1:
                    beginTime = DateTime.Now;
                    endTime = DateTime.Now;
                    tempTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    beginTime = tempTime;
                    endTime = tempTime.AddDays(1).AddSeconds(-1);
                    break;
                case 2:
                    beginTime = DateTime.Now;
                    endTime = DateTime.Now;
                    tempTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    beginTime = DateHelper.GetMondayDate(tempTime);
                    endTime = DateHelper.GetSundayDate(tempTime).AddDays(1).AddSeconds(-1);
                    break;
                case 3:
                    beginTime = DateTime.Now;
                    endTime = DateTime.Now;
                    tempTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    beginTime = tempTime;
                    endTime = tempTime.AddMonths(1).AddSeconds(-1);
                    break;
                case 4:
                    break;
            }
            
            return Content(documentVisitInfoApp.GetItemList(beginTime, endTime, searchType).ToJson()); 
        }

        /// <summary>
        /// 发布统计
        /// </summary>
        /// <param name="searchType">1 今日</param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult GetReleaseInfoStat(int searchType, DateTime beginTime, DateTime endTime)
        {
            if (searchType == 1)
            {
                beginTime = DateTime.Now;
                endTime = DateTime.Now;
                var tempTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                beginTime = tempTime;
                endTime = tempTime.AddMonths(1).AddSeconds(-1);
            }
            return Content(releaseInfoApp.GetItemList(beginTime, endTime, searchType).ToJson()); 
        }

        /// <summary>
        /// 下载统计
        /// </summary>
        /// <param name="searchType">1 今日 2本周 3 本月</param>
        /// <param name="beginTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult GetDownloadInfoStat(int searchType, DateTime beginTime, DateTime endTime)
        {
            var tempTime = DateTime.Now;
            switch (searchType)
            {
                case 1:
                    beginTime = DateTime.Now;
                    endTime = DateTime.Now;
                    tempTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    beginTime = tempTime;
                    endTime = tempTime.AddDays(1).AddSeconds(-1);
                    break;
                case 2:
                    beginTime = DateTime.Now;
                    endTime = DateTime.Now;
                    tempTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    beginTime = DateHelper.GetMondayDate(tempTime);
                    endTime = DateHelper.GetSundayDate(tempTime).AddDays(1).AddSeconds(-1);
                    break;
                case 3:
                    beginTime = DateTime.Now;
                    endTime = DateTime.Now;
                    tempTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    beginTime = tempTime;
                    endTime = tempTime.AddMonths(1).AddSeconds(-1);
                    break;
                case 4:
                    break;
            }
            return Content(downloadInfoApp.GetItemList(beginTime, endTime, searchType).ToJson());
        }
        /// <summary>
        /// 返回前10 搜索关键字
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult DocumentSearchKeysStatTop()
        { 
            return Content(documentSearchKeysApp.GetStat().ToJson());
        }
        /// <summary>
        /// 返回前10 文档浏览
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult DocumentVisitInfoStatTop()
        {
            return Content(documentVisitInfoApp.GetStat().ToJson());
        }

    }
}