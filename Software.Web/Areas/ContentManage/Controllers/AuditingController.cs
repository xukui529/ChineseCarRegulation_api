using NFine.Application.ContentManage;
using NFine.Code;
using NFine.Domain.Entity.ContentManage;
using NFine.Domain.ViewModel;
using Software.Web.Areas.ContentManage.Models;
using Software.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Software.Web.Areas.ContentManage.Controllers
{
    [HandlerLogin]
    public class AuditingController : Controller
    {
        private DocumentApp documentApp = new DocumentApp();
        // GET: ContentManage/Auditing
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public JsonResult GetGridJson(string keyword, string sidx, string sord, int limit, int offset, int index, int type)
        {
            int total = 0;
            IList<DocumentModel> entityList = documentApp.GetAuditDataList((offset / limit) + 1, limit, keyword, index, type, out   total);
            var totalCount =total;
            var pageSize = entityList.ToList();
            return Json(new { total = totalCount, rows = pageSize }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 编辑文档
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
		public ActionResult CreateModel(string keyValue)
        {
            ViewData["id"] = string.Empty;
            DocumentEntity entity = new DocumentEntity();
            if (!string.IsNullOrEmpty(keyValue))
            {
                ViewData["id"] = keyValue;
                entity = documentApp.GetForm(keyValue);
                if (!string.IsNullOrEmpty(entity.F_TotalUrl))
                {
                    entity.F_TotalUrl = entity.F_TotalUrl.Replace("\\", "\\\\");
                }
                if (!string.IsNullOrEmpty(entity.F_HtmlUrl))
                {
                    entity.F_HtmlUrl = entity.F_HtmlUrl.Replace("\\", "\\\\");
                }
                if (!string.IsNullOrEmpty(entity.F_ChineseUrl))
                {
                    entity.F_ChineseUrl = entity.F_ChineseUrl.Replace("\\", "\\\\");
                }
                if (!string.IsNullOrEmpty(entity.F_EnglishUrl))
                {
                    entity.F_EnglishUrl = entity.F_EnglishUrl.Replace("\\", "\\\\");
                }
            }
            return View(entity);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        /// <summary>
        /// 编辑文档
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult CreateModel(DocumentEntity entity, string keyValue)
        {
            string markup = Request.Form["Hid_ZiXunContent"];
            if (!string.IsNullOrEmpty(keyValue))
            {
                if (System.IO.File.Exists(entity.F_TotalUrl))
                {
                    System.IO.File.Delete(entity.F_TotalUrl);
                    System.IO.File.WriteAllText(entity.F_TotalUrl, markup, Encoding.UTF8);
                }
            }
            documentApp.SubmitForm(entity, keyValue);
            return Content("操作成功");
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public JsonResult Audit(string keyValue, int status)
        {
            int result = 0;
            var document = documentApp.GetForm(keyValue);
            if (document != null)
            {
                document.F_State = status;
                documentApp.UpdateState(keyValue, status);
                result = 1;
            }
            else
            {
                result = 2;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 编辑标签
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult EditLableModel(string keyValue)
        {
            ViewData["id"] = keyValue;
            DocumentEntity entity = new DocumentEntity();
            entity = documentApp.GetForm(keyValue);
            ViewData["F_Label"] = entity.F_Label;
            return View(entity);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult EditLableModel(DocumentEntity entity, string keyValue)
        {
            DocumentEntity data = documentApp.GetForm(keyValue);
            data.F_Label = entity.F_Label;
            documentApp.SubmitForm(data, keyValue);
            return Content("操作成功");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateZiXunModel(DocumentEntity entity, string keyValue)
        {
            string markup = Request.Form["Hid_ZiXunContent"];
            entity.F_InfoContent = markup;
            documentApp.SubmitForm(entity, keyValue);
            return Content("操作成功");
        }

        [HttpPost]
        public ActionResult SaveFile(string keyWord)
        {
            DocumentEntity entity = documentApp.GetForm(keyWord);
            return Json(new OfficeDocHelper().SaveContent(UploadFile(entity), entity));
        }

        [HttpPost]
        public string UploadFile(DocumentEntity documentEntity)
        {
            string preWord = string.Empty;
            HttpPostedFileBase uploadfile = Request.Files["uploadfile"];
            if (uploadfile == null || string.IsNullOrEmpty(uploadfile.FileName))
            {
                return "请选择上传文件！";
            }
            //取得上传文件之扩展文件名，并转换成小写字母
            string fileExtension = Path.GetExtension(uploadfile.FileName).ToLower();
            if (fileExtension != ".docx" && fileExtension != ".doc")
            {
                return "只能上传word文件！";
            }

            var zwPath = "";


            if (documentEntity != null)
            {
                zwPath = documentEntity.F_HtmlUrl;
                if (System.IO.File.Exists(zwPath))
                {
                    System.IO.File.Delete(zwPath);
                }
            }
            else
            {
                string subFileName = Path.GetFileNameWithoutExtension(uploadfile.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss");
                //被转换的html文档保存的位置 
                subFileName = subFileName.Replace(" ", "_");
                string zwFileName = subFileName + fileExtension;
                zwPath = OfficeDocHelper.GetPath(zwFileName);
            }
            uploadfile.SaveAs(zwPath);
            return zwPath;
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = documentApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        public ActionResult DetailModel(string keyValue)
        {
            ViewData["id"] = string.Empty;
            DocumentEntity entity = new DocumentEntity();
            if (!string.IsNullOrEmpty(keyValue))
            {
                ViewData["id"] = keyValue;
                entity = documentApp.GetForm(keyValue);
                if (!string.IsNullOrEmpty(entity.F_TotalUrl))
                {
                    entity.F_TotalUrl = entity.F_TotalUrl.Replace("\\", "\\\\");
                }
                if (!string.IsNullOrEmpty(entity.F_HtmlUrl))
                {
                    entity.F_HtmlUrl = entity.F_HtmlUrl.Replace("\\", "\\\\");
                }
                if (!string.IsNullOrEmpty(entity.F_ChineseUrl))
                {
                    entity.F_ChineseUrl = entity.F_ChineseUrl.Replace("\\", "\\\\");
                }
                if (!string.IsNullOrEmpty(entity.F_EnglishUrl))
                {
                    entity.F_EnglishUrl = entity.F_EnglishUrl.Replace("\\", "\\\\");
                }
            }
            return View(entity);
        }
        public ActionResult DetailInformationModel(string keyValue)
        {
            ViewData["id"] = string.Empty;
            DocumentEntity entity = new DocumentEntity();
            if (!string.IsNullOrEmpty(keyValue))
            {
                ViewData["id"] = keyValue;
                entity = documentApp.GetForm(keyValue);
                if (!string.IsNullOrEmpty(entity.F_TotalUrl))
                {
                    entity.F_TotalUrl = entity.F_TotalUrl.Replace("\\", "\\\\");
                }
                if (!string.IsNullOrEmpty(entity.F_HtmlUrl))
                {
                    entity.F_HtmlUrl = entity.F_HtmlUrl.Replace("\\", "\\\\");
                }
                if (!string.IsNullOrEmpty(entity.F_ChineseUrl))
                {
                    entity.F_ChineseUrl = entity.F_ChineseUrl.Replace("\\", "\\\\");
                }
                if (!string.IsNullOrEmpty(entity.F_EnglishUrl))
                {
                    entity.F_EnglishUrl = entity.F_EnglishUrl.Replace("\\", "\\\\");
                }
            }
            return View(entity);
        }
        public ActionResult DetailInfomationWordModel(string keyValue)
        {
            ViewData["id"] = string.Empty;
            DocumentEntity entity = new DocumentEntity();
            if (!string.IsNullOrEmpty(keyValue))
            {
                ViewData["id"] = keyValue;
                entity = documentApp.GetForm(keyValue);
                if (!string.IsNullOrEmpty(entity.F_TotalUrl))
                {
                    entity.F_TotalUrl = entity.F_TotalUrl.Replace("\\", "\\\\");
                }
                if (!string.IsNullOrEmpty(entity.F_HtmlUrl))
                {
                    entity.F_HtmlUrl = entity.F_HtmlUrl.Replace("\\", "\\\\");
                }
                if (!string.IsNullOrEmpty(entity.F_ChineseUrl))
                {
                    entity.F_ChineseUrl = entity.F_ChineseUrl.Replace("\\", "\\\\");
                }
                if (!string.IsNullOrEmpty(entity.F_EnglishUrl))
                {
                    entity.F_EnglishUrl = entity.F_EnglishUrl.Replace("\\", "\\\\");
                }
            }
            return View(entity);
        }


        public ActionResult DetailModelAlone(string keyValue)
        {
            ViewData["id"] = string.Empty;
            DocumentEntity entity = new DocumentEntity();
            if (!string.IsNullOrEmpty(keyValue))
            {
                ViewData["id"] = keyValue;
                entity = documentApp.GetForm(keyValue);
                if (!string.IsNullOrEmpty(entity.F_TotalUrl))
                {
                    entity.F_TotalUrl = entity.F_TotalUrl.Replace("\\", "\\\\");
                }
                if (!string.IsNullOrEmpty(entity.F_HtmlUrl))
                {
                    entity.F_HtmlUrl = entity.F_HtmlUrl.Replace("\\", "\\\\");
                }
                if (!string.IsNullOrEmpty(entity.F_ChineseUrl))
                {
                    entity.F_ChineseUrl = entity.F_ChineseUrl.Replace("\\", "\\\\");
                }
                if (!string.IsNullOrEmpty(entity.F_EnglishUrl))
                {
                    entity.F_EnglishUrl = entity.F_EnglishUrl.Replace("\\", "\\\\");
                }
            }
            return View(entity);
        }
        public ActionResult DetailModelMixture(string keyValue)
        {
            ViewData["id"] = string.Empty;
            DocumentEntity entity = new DocumentEntity();
            if (!string.IsNullOrEmpty(keyValue))
            {
                ViewData["id"] = keyValue;
                entity = documentApp.GetForm(keyValue);
                if (!string.IsNullOrEmpty(entity.F_TotalUrl))
                {
                    entity.F_TotalUrl = entity.F_TotalUrl.Replace("\\", "\\\\");
                }
                if (!string.IsNullOrEmpty(entity.F_HtmlUrl))
                {
                    entity.F_HtmlUrl = entity.F_HtmlUrl.Replace("\\", "\\\\");
                }
                if (!string.IsNullOrEmpty(entity.F_ChineseUrl))
                {
                    entity.F_ChineseUrl = entity.F_ChineseUrl.Replace("\\", "\\\\");
                }
                if (!string.IsNullOrEmpty(entity.F_EnglishUrl))
                {
                    entity.F_EnglishUrl = entity.F_EnglishUrl.Replace("\\", "\\\\");
                }
            }
            return View(entity);
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetWordContent(string keyValue, bool isEdit)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                try
                {
                    FileStream fs = new FileStream(keyValue, FileMode.OpenOrCreate, FileAccess.Read);
                    StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                    string strChinese = sr.ReadToEnd();
                    sr.Close();
                    fs.Close();
                    return Content(strChinese);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }


        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult DeleteForm(string keyValue)
        {
            documentApp.DeleteForm(keyValue);
            return Content("操作成功");
        }

        public ActionResult UpdateActivateState(string keyValue, int isActivate)
        {
            bool state = true;
            if (isActivate == 1)
            {
                state = false;
            }
            DocumentEntity data = documentApp.GetForm(keyValue);
            data.F_IsActivate = state;
            data.F_State = 4;
            documentApp.SubmitForm(data, keyValue);
            return Content("操作成功");
        }



        public ActionResult UpdateFileState(string keyValue, string fileState)
        { 
            DocumentEntity data = documentApp.GetForm(keyValue);
            data.F_FileState = fileState; 
            documentApp.SubmitForm(data, keyValue);
            return Content("操作成功");
        }
        public ActionResult UpdateActivateVipState(string keyValue )
        {
          
            DocumentEntity data = documentApp.GetForm(keyValue);
            if(data != null)
            {
                data.F_IsActivateVip = !data.F_IsActivateVip;
            } 
            documentApp.SubmitForm(data, keyValue);
            return Content("操作成功");
        }

    }
}