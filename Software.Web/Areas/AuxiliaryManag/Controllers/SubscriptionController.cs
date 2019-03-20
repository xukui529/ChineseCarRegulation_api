using NFine.Application.AuxiliaryManage;
using NFine.Application.ContentManage;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.Entity.ContentManage;
using NFine.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.IO;
using System.Web.Mvc;
using System.Text;

namespace Software.Web.Areas.AuxiliaryManag.Controllers
{
    [HandlerLogin]
    public class SubscriptionController : Controller
    {
        MemberApp memberApp = new MemberApp();
        private SubscriptionApp subscriptionApp = new SubscriptionApp();
        private DocumentApp documentApp = new DocumentApp();
        // GET: AuxiliaryManag/Subscription
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public JsonResult GetGridJson(string keyword, string sidx, string sord, int limit, int offset)
        {
            List<SubscriptionInfoModel> entityList = new List<SubscriptionInfoModel>();
            //取一下本期 Subscription，在根据开始时间 结束时间 去取上一周的数据并且把排除的数据加上

            //先判断本期 数据 是否已经在 Auxiliary_Subscription 里面了，如果在 就取这里的数据，不存在，查出数据后生成一条这个数据
            var temptime = DateTime.Now.AddDays(-7);
            var time = new DateTime(temptime.Year, temptime.Month, temptime.Day);


            var beginTime = DateHelper.GetMondayDate(time);
            var endTime = DateHelper.GetSundayDate(time).AddDays(1).AddSeconds(-1);
            SubscriptionEntity subscriptionEntity = subscriptionApp.GetSubDataByTime(beginTime, endTime, time);
            string[] excludeDocIds;
            if (subscriptionEntity != null)
            {
                excludeDocIds = subscriptionEntity.F_ExcludeDocIds.Split(',');

                //根据时间获取数据 ,然后设置是否排除
                List<DocumentEntity> docList = documentApp.GetSubscriptionDataList(beginTime, endTime);

                foreach (var item in docList)
                {
                    entityList.Add(new SubscriptionInfoModel
                    {
                        F_IsSend = subscriptionEntity.F_IsSend,
                        F_SendNum = subscriptionEntity.F_SendNum,
                        F_BeginTime = subscriptionEntity.F_BeginTime,
                        F_EndTime = subscriptionEntity.F_EndTime,
                        F_Id = item.F_Id,
                        F_ChineseTitle = item.F_ChineseTitle,
                        F_EnglishTitle = item.F_EnglishTitle,
                        F_ReleaseDate = item.F_ReleaseDate,
                        F_IsExclude = excludeDocIds.Contains(item.F_Id)
                    });
                }
            }
            return Json(new { total = entityList.Count(), rows = entityList.ToList() }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult NoExclude(string keyValue)
        {
            //先判断本期 数据 是否已经在 Auxiliary_Subscription 里面了，如果在 就取这里的数据，不存在，查出数据后生成一条这个数据
            var temptime = DateTime.Now.AddDays(-7);
            var time = new DateTime(temptime.Year, temptime.Month, temptime.Day);


            var beginTime = DateHelper.GetMondayDate(time);
            var endTime = DateHelper.GetSundayDate(time).AddDays(1).AddSeconds(-1);
            SubscriptionEntity subscriptionEntity = subscriptionApp.GetSubDataByTime(beginTime, endTime, time);

            if (subscriptionEntity != null)
            {
                subscriptionEntity.F_ExcludeDocIds = subscriptionEntity.F_ExcludeDocIds.Replace(keyValue + ",", "");
                subscriptionApp.SubmitForm(subscriptionEntity, subscriptionEntity.F_Id);
            }


            return Content("操作成功");
        }
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult Exclude(string keyValue)
        {
            //先判断本期 数据 是否已经在 Auxiliary_Subscription 里面了，如果在 就取这里的数据，不存在，查出数据后生成一条这个数据
            var temptime = DateTime.Now.AddDays(-7);
            var time = new DateTime(temptime.Year, temptime.Month, temptime.Day);


            var beginTime = DateHelper.GetMondayDate(time);
            var endTime = DateHelper.GetSundayDate(time).AddDays(1).AddSeconds(-1);
            SubscriptionEntity subscriptionEntity = subscriptionApp.GetSubDataByTime(beginTime, endTime, time);

            if (subscriptionEntity != null)
            {
                subscriptionEntity.F_ExcludeDocIds = subscriptionEntity.F_ExcludeDocIds + keyValue + ",";
                subscriptionApp.SubmitForm(subscriptionEntity, subscriptionEntity.F_Id);
            }

            return Content("操作成功");
        }

        /// <summary>
        /// 异步发送订阅邮件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult SubscriptionEmail()
        {
            //先判断本期 数据 是否已经在 Auxiliary_Subscription 里面了，如果在 就取这里的数据，不存在，查出数据后生成一条这个数据
            var temptime = DateTime.Now.AddDays(-7);
            var time = new DateTime(temptime.Year, temptime.Month, temptime.Day);


            var beginTime = DateHelper.GetMondayDate(time);
            var endTime = DateHelper.GetSundayDate(time).AddDays(1).AddSeconds(-1);
            SubscriptionEntity subscriptionEntity = subscriptionApp.GetSubDataByTime(beginTime, endTime, time);
            string[] excludeDocIds;
            if (subscriptionEntity != null)
            {
                excludeDocIds = subscriptionEntity.F_ExcludeDocIds.Split(',');
                subscriptionEntity.F_IsSend = true;
                subscriptionApp.SubmitForm(subscriptionEntity, subscriptionEntity.F_Id);

                //此处异步发送邮件
                string toListStr = "";
                string titleListStr = "";
                List<DocumentEntity> docList = documentApp.GetSubscriptionDataList(beginTime, endTime);

                foreach (var item in docList)
                {
                    if (!excludeDocIds.Contains(item.F_Id))
                    {
                        titleListStr = titleListStr + "<br/>" + item.F_ChineseTitle + " " + item.F_EnglishTitle;
                    }
                }

                var userList = memberApp.GetListSubscription();
                foreach (var item in userList)
                {
                    toListStr = toListStr + item.F_Email + "|";
                }
                toListStr = "kehaitao_0@sina.com";
                string[] toList = toListStr.Split('|');


                //发送订阅邮件 
                MailHelper mailHelper = new MailHelper();
                mailHelper.MailServer = ConfigurationManager.AppSettings["MailServer"];
                mailHelper.MailUserName = ConfigurationManager.AppSettings["MailBatchUserName"];
                mailHelper.MailPassword = ConfigurationManager.AppSettings["MailPassword"];
                mailHelper.MailName = "本周更新 Update this week";
                string title = "本周更新 Update this week";
                string templetpath = Request.PhysicalApplicationPath + "\\dingyue.html";// Server.MapPath("dingyue.html");
                NameValueCollection myCol = new NameValueCollection();
                myCol.Add("link", titleListStr);
                string body = MailHelper.BulidByFile(templetpath, myCol);
                mailHelper.SendBatchMail(toList, title, body);
            }
            return Content("操作成功");
        }


        /// <summary>
        /// 处理pdf
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreatePdf(string id)
        {
            Common.Log("log", "log", "CreatePdf-begin ");


            DocumentEntity entity = documentApp.GetForm(id);

            Common.Log("log", "log", "CreatePdf-begin：" + entity.F_Id);

            //都是中英文的 64条数据，所以可以都生成 
            var dir = entity.F_HtmlUrl.Substring(0, entity.F_HtmlUrl.LastIndexOf('\\'));
            var ConfigPath = entity.F_HtmlUrl.Replace(".html", "");
            PDFHelper.html2pdf(ConfigPath + ".html", Common.GetPdfFileName(dir, entity.F_EnglishTitle, 3) + ".pdf");

            //中文的 需要先加上 标记 utf-8 防止乱码

            var refile = System.IO.File.ReadAllText(ConfigPath + "-中.html");
            if (System.IO.File.Exists(ConfigPath + "-中.html"))
            {
                System.IO.File.Delete(ConfigPath + "-中.html");
            }
            var star = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml; charset=utf-8\" /><title></title></head><body >";
            var end = "</body></html>";
            refile = star + refile + end;
            System.IO.File.AppendAllText(ConfigPath + "-中.html", refile, Encoding.UTF8);

            PDFHelper.html2pdf(ConfigPath + "-中.html", Common.GetPdfFileName(dir, entity.F_ChineseTitle, 1) + ".pdf");


            PDFHelper.html2pdf(ConfigPath + "-英.html", Common.GetPdfFileName(dir, entity.F_EnglishTitle, 2) + ".pdf");

            Common.Log("log", "log", "CreatePdf-end：" + entity.F_Id);


            //List<DocumentEntity> doclist = documentApp.GetDataListAll();
            //foreach (var item in doclist)
            //{
            //    DocumentEntity entity = item;
            //    if (item.F_Type == 1 || item.F_Type == 2)
            //    {
            //        Common.Log("log", "log", "CreatePdf-begin：" + item.F_Id);

            //        //都是中英文的 64条数据，所以可以都生成 
            //        var dir = entity.F_HtmlUrl.Substring(0, entity.F_HtmlUrl.LastIndexOf('\\'));
            //        var ConfigPath = entity.F_HtmlUrl.Replace(".html", "");
            //        PDFHelper.html2pdf(ConfigPath + ".html", Common.GetPdfFileName(dir, entity.F_EnglishTitle, 3) + ".pdf");

            //        //中文的 需要先加上 标记 utf-8 防止乱码

            //        var refile =System.IO.File.ReadAllText(ConfigPath + "-中.html");
            //        if (System.IO.File.Exists(ConfigPath + "-中.html"))
            //        {
            //            System.IO.File.Delete(ConfigPath + "-中.html");
            //        } 
            //        var star = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml; charset=utf-8\" /><title></title></head><body >";
            //        var end = "</body></html>";
            //        refile = star + refile + end;
            //        System.IO.File.AppendAllText(ConfigPath + "-中.html", refile, Encoding.UTF8); 

            //        PDFHelper.html2pdf(ConfigPath + "-中.html", Common.GetPdfFileName(dir, entity.F_ChineseTitle, 1) + ".pdf");


            //        PDFHelper.html2pdf(ConfigPath + "-英.html", Common.GetPdfFileName(dir, entity.F_EnglishTitle, 2) + ".pdf");

            //        Common.Log("log", "log", "CreatePdf-end：" + item.F_Id);
            //    }
            //}

            Common.Log("log", "log", "CreatePdf-end ");
            return Content("操作成功");
        }
    }
}