using NFine.Application.AuxiliaryManage;
using NFine.Application.ContentManage;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.Entity.ContentManage;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.ViewModel;
using Software.Web.Areas.ContentManage.Models;
using Software.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;

namespace Software.Web.Areas.ContentManage.Controllers
{
    [HandlerLogin]
    public class DocumentUploadController : Controller
    {
        private DocumentApp documentApp = new DocumentApp();
        private DocumentContentApp documentContentApp = new DocumentContentApp();
        private ReleaseInfoApp releaseInfoApp = new ReleaseInfoApp();
        // GET: ContentManage/DocumentUpload 
        #region old
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
        public ActionResult CreateModel(DocumentEntity entity, string keyValue)
        {
            try
            {
                Common.Log("log", "log", "11");
                string markup = Request.Form["Hid_ZiXunContent"];
                string htmlUrl = Request.Form["F_HtmlUrl"];
                FileContentModel model = new FileContentModel();
                markup = Common.ReplenishHtml(markup, htmlUrl);
                Common.Log("log", "log", "22");
                //此功能逻辑：
                /* 旧的逻辑：
                 * 1、上传word，UploadFile 后台保存了文件并返回路径
                 * 2、调用 UnUpLoadWord 读取word文档内容到html返回给客户端操作（这个是bug，应该返回原文混排的），这里 拆分了 ：将上传的中英混排word文档转成html并拆分
                 * 3、前台修改好内容后，提交到CreateModel ，有资讯两个方法
                 * 新逻辑：
                 * 1、上传word，UploadFile 后台保存了文件并返回路径
                 * 2、调用 UnUpLoadWordGetWordContent 读取word文档内容到html 原文的内容， 是混排的数据
                 * 3、前台修改内容后提交 CreateModel，这里重新更新数据，生成新的html内容 
                 * */
                //先更新html markup 是原生混排html 要更新 [F_InfoContent] 文档表的 
                OfficeDocHelper helper = new OfficeDocHelper();
                Common.Log("log", "log", "33");
                model = helper.SaveContentByHtml(markup, htmlUrl);//F_HtmlUrl 原文  F_TotalUrl 中英 F_ChineseUrl 中F_EnglishUrl英

                Common.Log("log", "log", "44");
                //model = helper.SaveContentByHtmlNew(markup, htmlUrl);//F_HtmlUrl 原文  F_TotalUrl 中英 F_ChineseUrl 中F_EnglishUrl英
                //传入的entity 不带状态
                if (!string.IsNullOrEmpty(keyValue))
                {
                    DocumentEntity entityTemp = documentApp.GetForm( keyValue);
                    if(entityTemp!=null)
                    {
                        if (entityTemp.F_State == (int)NFine.Code.Enum.DocumentStatus.未通过)
                        {
                            entity.F_State = (int)NFine.Code.Enum.DocumentStatus.未提交;
                        }
                    }
                }


              
                //结论就是 无论新增还是修改，都更新内容
                documentApp.SubmitForm(entity, keyValue);


                Common.Log("log", "log", "55");
                DocumentContentEntity documentContentEntity = documentContentApp.GetFormByDocumentId(entity.F_Id);
                if (documentContentEntity == null)
                {
                    documentContentEntity = new DocumentContentEntity();
                    documentContentEntity.F_DocumentId = entity.F_Id;
                    documentContentEntity.F_Type = entity.F_Type;
                }
                foreach (var item in model.YWList)
                {
                    documentContentEntity.F_EnglishContent += item;
                }
                foreach (var item in model.ZWList)
                {
                    documentContentEntity.F_ChineseContent += item;
                }

                Common.Log("log", "log", "66");
                documentContentApp.SubmitForm(documentContentEntity, documentContentEntity.F_Id);
            }
            catch(Exception e)
            {
                Common.Log("log", "err", "err1" + e.Message);
                Common.Log("log", "err", "err" + e.InnerException);
            }

           
            return Content("操作成功");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateZiXunModel(DocumentEntity entity, string keyValue)
        {
            string markup = Request.Form["Hid_ZiXunContent"];
            //资讯内容 用 ZiXunSeparator 分割，内容保存到 内容表   
            documentApp.SubmitForm(entity, keyValue);
            DocumentContentEntity documentContentEntity = documentContentApp.GetFormByDocumentId(entity.F_Id);
            if (documentContentEntity == null)
            {
                documentContentEntity = new DocumentContentEntity();
                documentContentEntity.F_DocumentId = entity.F_Id;
                documentContentEntity.F_Type = entity.F_Type;
            }
            Regex regex = new Regex(Configs.GetValue("ZiXunSeparator"));//以cjlovefl分割
            string[] bit = regex.Split(markup);
            if (bit.Length == 2)
            {
                documentContentEntity.F_ChineseContent = bit[0];
                documentContentEntity.F_EnglishContent = bit[1];
            }
            else if (bit.Length == 1)
            {
                documentContentEntity.F_ChineseContent = bit[0];
            }
            documentContentEntity.F_DocumentId = entity.F_Id;
            documentContentEntity.F_Type = entity.F_Type;
            documentContentApp.SubmitForm(documentContentEntity, documentContentEntity.F_Id);
            return Content("操作成功");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateInput(false)]
        public ActionResult SetEditHtmlContent()
        {
            string markup = Request.Form["hidTempCode"];
            string htmlUrl = Request.Form["F_HtmlUrl"];


            markup = Common.ReplenishHtml(markup, htmlUrl);
            OfficeDocHelper helper = new OfficeDocHelper();
            FileContentModel model = helper.NoSaveContentByHtml(markup, htmlUrl);
            return Content(model.TotalCode);
        }




        //[HttpPost]
        //public ActionResult UploadFilePreview()
        //{
        //    string zwPath = UploadFile();//使用
        //    //此处是bug ，应该返回是不拆分的数据 List<string> UnUpLoadWordGetWordContent(string strFile)
        //    //string preWord = OfficeDocHelper.UnUpLoadWord(zwPath);//使用
        //    string preWord = OfficeDocHelper.UnUpLoadWordGetWordContent(zwPath);//使用
        //    return Content(preWord);
        //}


        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult DeleteForm(string keyValue)
        {
            documentApp.DeleteForm(keyValue);
            return Content("删除成功");
        }

        public ActionResult UpdateState(string keyValue, int state)
        {
            if(state== (int)NFine.Code.Enum.DocumentStatus.已发布)
            {
                //新增一条 发布统计记录
                DocumentEntity documentEntity = documentApp.GetForm(keyValue);
                if (documentEntity != null)
                {
                    ReleaseInfoEntity entity = new ReleaseInfoEntity();
                    entity.F_Versions = "";
                    entity.F_DocumentId = keyValue;
                    entity.F_Remark = "";
                    entity.F_Type = documentEntity.F_Type;
                    releaseInfoApp.SubmitForm(entity, entity.F_Id);
                } 
            }
            documentApp.UpdateState(keyValue, state);
            return Content("操作成功");
        }
        #endregion

        #region 编辑标签

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
            return View(entity);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult EditLableModel(DocumentEntity entity, string keyValue)
        {
            documentApp.SubmitForm(entity, keyValue);
            return Content("操作成功");
        }
        #endregion
         
        #region 中英文混排文档
        /// <summary>
        /// 初始化 中英文混排文档
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult CreateModelMixtureUploadFile(string keyValue)
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
        /// <summary>
        /// 上传 中英文混排 word 
        /// </summary>
        /// <param name="documentEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public string UploadFile(DocumentEntity documentEntity)
        {
            Common.Log("log", "log", "UploadFile开始：" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss fff"));
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
            string zwPath = "";
            if (documentEntity != null && !string.IsNullOrEmpty(documentEntity.F_Id))
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
            Common.Log("log", "log", "UploadFile结束：" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss fff"));

            return zwPath;
        }
        /// <summary>
        /// 单独上传 中英文混合文件 生成的文件是全的
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveFile(string keyWord,string F_EnglishTitle,string F_ChineseTitle)
        {//要修改为 如果已有文件，要在源文件删除修改 
            DocumentEntity entity  ;
            if (!string.IsNullOrEmpty(keyWord))
            {
               entity = documentApp.GetForm(keyWord);
            }
            else
            {
                entity = new DocumentEntity();
                entity.F_EnglishTitle = F_EnglishTitle;
                entity.F_ChineseTitle = F_ChineseTitle;
            }
            var re = new OfficeDocHelper().SaveContent(UploadFile(entity), entity);
            return Json(re);
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetWordContent(string keyValue, bool isEdit)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                try
                {
                    FileStream fs = new FileStream(keyValue, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                    string re = sr.ReadToEnd();

                    string filename = Path.GetFileNameWithoutExtension(keyValue);
                    sr.Close();
                    fs.Close();
                    return Content(re);
                }
                catch(Exception e)
                {
                    return null;
                } 
            }
            else
            {
                return null;
            }
        }
       

        private string CheckFile(HttpPostedFileBase uploadfile)
        {
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
            return "ok";
        }
        private string GetNewFileName(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName).ToLower();
            string subFileName = Path.GetFileNameWithoutExtension(fileName) + DateTime.Now.ToString("yyyyMMddHHmmss");
            //被转换的html文档保存的位置 
            subFileName = subFileName.Replace(" ", "_");

            return OfficeDocHelper.GetPath(subFileName + fileExtension); 
        }

        #endregion

        #region 单独中 英 文件上传
        /// <summary>
        ///  展示内容
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="isEdit"></param>
        /// <returns></returns>
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetWordContentZYW(string chineseUrl, string englishUrl)
        {
            string re = "";
            try
            {
                if (!string.IsNullOrEmpty(chineseUrl))
                {

                    FileStream fs = new FileStream(chineseUrl, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                    re = sr.ReadToEnd();
                    sr.Close();
                    fs.Close();
                }
            }
            catch
            {

            }
            try
            {
                if (!string.IsNullOrEmpty(englishUrl))
                {

                    FileStream fs = new FileStream(englishUrl, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                    re = re + "<br/>" + sr.ReadToEnd();
                    sr.Close();
                    fs.Close();

                }
            }
            catch
            {

            }
           
            return Content(re);
        }

        /// <summary>
        /// 单独上传 中文 或者英文 分别保存俩html 俩pdf pdf与html名称一致后缀不同
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveFileZWOrYW(string keyWord)
        {//要修改为 如果已有文件，要在源文件删除修改 
            DocumentEntity entity = documentApp.GetForm(keyWord);
            HttpPostedFileBase uploadfileZW = Request.Files["uploadfileZW"];
            HttpPostedFileBase uploadfileYW = Request.Files["uploadfileYW"];
            FileContentModel fileContentModel = new FileContentModel();
            OfficeDocHelper officeDocHelper = new OfficeDocHelper();
            if (CheckFile(uploadfileZW) == "ok")
            {
                string strFile = GetSaveFilePath(entity, 1, uploadfileZW.FileName);
                uploadfileZW.SaveAs(strFile);
                fileContentModel.ChineseUrl = officeDocHelper.SaveContentForZWOrYW(strFile, entity, 1);
            }
            if (CheckFile(uploadfileYW) == "ok")
            {
                string strFile = GetSaveFilePath(entity, 2, uploadfileYW.FileName);
                uploadfileYW.SaveAs(strFile);
                fileContentModel.EnglishUrl = officeDocHelper.SaveContentForZWOrYW(strFile, entity, 2);
            }
            return Json(fileContentModel);
        }


        /// <summary>
        /// 初始化 单独中文 英文上传
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult CreateModelAloneUploadFile(string keyValue)
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

        /// <summary>
        /// 单独上传 中 英文文件 保存 只保存内容到 内容表以及保存文档实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateModelAloneUploadFile(DocumentEntity entity, string keyValue)
        {
           
            string chineseUrl = Request.Form["F_ChineseUrl"];
            string englishUrl = Request.Form["F_EnglishUrl"];
            FileContentModel model = new FileContentModel();  
            OfficeDocHelper helper = new OfficeDocHelper(); 
            if(entity.F_State == (int)NFine.Code.Enum.DocumentStatus.未通过)
            {
                entity.F_State = (int)NFine.Code.Enum.DocumentStatus.未提交;
            }
            //结论就是 无论新增还是修改，都更新内容
            documentApp.SubmitForm(entity, keyValue);
            DocumentContentEntity documentContentEntity = documentContentApp.GetFormByDocumentId(entity.F_Id);
           
            if (documentContentEntity == null)
            {
                documentContentEntity = new DocumentContentEntity();
                documentContentEntity.F_DocumentId = entity.F_Id;
                documentContentEntity.F_Type = entity.F_Type;
            }
            if (chineseUrl != "")
            {
                //读取内容放到内容表
                documentContentEntity.F_ChineseContent = helper.GetContentByHtmlUrl(chineseUrl);
            }
            if (englishUrl != "")
            {
                documentContentEntity.F_EnglishContent = helper.GetContentByHtmlUrl(englishUrl);
                //读取内容放到内容表
            }
            documentContentApp.SubmitForm(documentContentEntity, documentContentEntity.F_Id);
            return Content("操作成功");
        }

        #endregion

        #region 资讯 单独上传
        public ActionResult CreateInformationWordModel(string keyValue)
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

        /// <summary>
        /// 单独上传 中 英文文件 保存 只保存内容到 内容表以及保存文档实体
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult CreateInformationWordModel(DocumentEntity entity, string keyValue)
        { 
            string chineseUrl = Request.Form["F_ChineseUrl"];
            string englishUrl = Request.Form["F_EnglishUrl"];
            FileContentModel model = new FileContentModel();
            OfficeDocHelper helper = new OfficeDocHelper();
            //结论就是 无论新增还是修改，都更新内容
            entity.F_Type = (int)NFine.Code.Enum.DocumentType.资讯;
            if (entity.F_State == (int)NFine.Code.Enum.DocumentStatus.未通过)
            {
                entity.F_State = (int)NFine.Code.Enum.DocumentStatus.未提交;
            }
            documentApp.SubmitForm(entity, keyValue);
            DocumentContentEntity documentContentEntity = documentContentApp.GetFormByDocumentId(entity.F_Id);

            if (documentContentEntity == null)
            {
                documentContentEntity = new DocumentContentEntity();
                documentContentEntity.F_DocumentId = entity.F_Id;
                documentContentEntity.F_Type = entity.F_Type;
            }
            if (chineseUrl != "")
            {
                //读取内容放到内容表
                documentContentEntity.F_ChineseContent = helper.GetContentByHtmlUrl(chineseUrl);
            }
            if (englishUrl != "")
            {
                documentContentEntity.F_EnglishContent = helper.GetContentByHtmlUrl(englishUrl);
                //读取内容放到内容表
            }
            documentContentApp.SubmitForm(documentContentEntity, documentContentEntity.F_Id);
            return Content("操作成功");
        }
        /// <summary>
        /// 单独上传 中文 或者英文 分别保存俩html 俩pdf pdf与html名称一致后缀不同 资讯用
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveFileInfomationZWOrYW(string keyWord)
        {//要修改为 如果已有文件，要在源文件删除修改 
            DocumentEntity entity = documentApp.GetForm(keyWord);
            HttpPostedFileBase uploadfileZW = Request.Files["uploadfileZW"];
            HttpPostedFileBase uploadfileYW = Request.Files["uploadfileYW"];
            FileContentModel fileContentModel = new FileContentModel();
            OfficeDocHelper officeDocHelper = new OfficeDocHelper();
            if (CheckFile(uploadfileZW) == "ok")
            {
                string strFile = GetSaveFilePath(entity, 1, uploadfileZW.FileName);
                uploadfileZW.SaveAs(strFile);
                fileContentModel.ChineseUrl = officeDocHelper.SaveContentForZWOrYW(strFile, entity, 1);
            }
            if (CheckFile(uploadfileYW) == "ok")
            {
                string strFile = GetSaveFilePath(entity, 2, uploadfileYW.FileName);
                uploadfileYW.SaveAs(strFile);
                fileContentModel.EnglishUrl = officeDocHelper.SaveContentForZWOrYW(strFile, entity, 2);
            }
            return Json(fileContentModel);
        }
        //
        //    
        #endregion

        #region 资讯 上传pdf
        /// <summary>
        /// 资讯 加载页面
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public ActionResult CreateInformationModel(string keyValue)
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
                //DocumentContentEntity documentContentEntity = documentContentApp.GetFormByDocumentId(entity.F_Id);

                //if (documentContentEntity != null)
                //{
                //    entity.F_InfoContent = documentContentEntity.F_ChineseContent + Configs.GetValue("ZiXunSeparator") + documentContentEntity.F_EnglishContent;
                //}
            }
            return View(entity);
        }
        /// <summary>
        /// 保存月报 pdf
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SaveZiXunModel(DocumentEntity entity, string keyValue)
        {
            entity.F_Type = (int)NFine.Code.Enum.DocumentType.资讯;  
            entity.F_TotalUrl = Request.Form["F_TotalUrl"];
            //审核拒绝 F_State=3 如果修改在提交到这里，应该改为0
            if (entity.F_State == (int)NFine.Code.Enum.DocumentStatus.未通过)
            {
                entity.F_State = (int)NFine.Code.Enum.DocumentStatus.未提交;
            }
            documentApp.SubmitForm(entity, keyValue);
           
            return Content("操作成功");
        }


        //[HttpGet]
        //[HandlerAjaxOnly]
        //public ActionResult GetZiXunContent(string keyValue, bool isEdit)
        //{
        //    if (!string.IsNullOrEmpty(keyValue))
        //    {
        //        string re = "";
        //        DocumentContentEntity documentContentEntity = documentContentApp.GetFormByDocumentId(keyValue);
        //        if (documentContentEntity != null)
        //        {
        //            re = documentContentEntity.F_ChineseContent + Configs.GetValue("ZiXunSeparator") + documentContentEntity.F_EnglishContent;
        //        }
        //        return Content(re);
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}


        [HttpPost]
        public ActionResult SavePDFFile(string keyWord)
        {

            FileContentModel model = new FileContentModel();
            HttpPostedFileBase uploadfile = Request.Files["uploadfile"];
            string reStr = "";
            if (uploadfile == null || string.IsNullOrEmpty(uploadfile.FileName))
            {
                reStr = "请选择上传文件！";
            }
            //取得上传文件之扩展文件名，并转换成小写字母
            string fileExtension = Path.GetExtension(uploadfile.FileName).ToLower();
            if (fileExtension.ToLower() != ".pdf")
            {
                reStr = "只能上传pdf文件！";
            }
            if (!string.IsNullOrEmpty(reStr))
            {
                //验证失败
                model.TotalUrl = "";
                return Json(model);
            }
            string hostAddr = ConfigurationManager.AppSettings["HostAddr"];
            string filePath = "";
            if (!string.IsNullOrEmpty(keyWord))
            {//覆盖
                DocumentEntity entity = documentApp.GetForm(keyWord);
                if (entity != null)
                {
                    filePath = entity.F_TotalUrl;
                }
                //取相对路径，在转换绝对路径，在删除 覆盖 
                string jdPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + model.TotalUrl.Replace(hostAddr, "");

                if (System.IO.File.Exists(jdPath))
                {
                    System.IO.File.Delete(jdPath);
                }
                uploadfile.SaveAs(jdPath);
            }
            else
            {
                //新建
                string subFileName = Path.GetFileNameWithoutExtension(uploadfile.FileName) + DateTime.Now.ToString("yyyyMMddHHmmss");
                subFileName = subFileName.Replace(" ", "_");
                filePath = subFileName + fileExtension;
                model.TotalUrl = hostAddr + "Temp/Word/" + subFileName + "/" + filePath;

                string dirStr = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "Temp/Word/" + subFileName;

                // 判断指定目录下是否存在文件夹，如果不存在，则创建 
                if (!Directory.Exists(dirStr))
                {
                    // 创建up文件夹 
                    Directory.CreateDirectory(dirStr);
                }
                //取相对路径，在转换绝对路径，在删除 覆盖
               
                string jdPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase+model.TotalUrl.Replace(hostAddr, "");

                if (System.IO.File.Exists(jdPath))
                {
                    System.IO.File.Delete(jdPath);
                }
                uploadfile.SaveAs(jdPath);
            } 
            return Json(model);
        }


        #endregion

        #region 显示文档
        private ItemsDetailApp itemsDetailApp = new ItemsDetailApp();

        
        public ActionResult ShowInfo(string id)
        { 
            ViewData["id"] = id;
            DocumentEntity entity = documentApp.GetForm(id);
            int DocShowType = 4;
            if (entity != null)
            {
                DocShowType = GetDocShowType(entity);
            }
            ViewData["DocShowType"] = DocShowType;
            return View(entity);
        }



        public ActionResult ShowPdfInfo(string id)
        {
            ViewData["id"] = id;
            DocumentEntity entity = documentApp.GetForm(id);
            int DocShowType = 4;
            if (entity != null)
            {
                DocShowType = GetDocShowType(entity);
            }
            ViewData["DocShowType"] = DocShowType;
            return View(entity);
        }


        [HttpPost]
        public void GetDocumentInfoById(string id, int type)
        { 
            string strChinese = ""; 
            DocumentEntity documentEntity = documentApp.GetForm(id);
            if (documentEntity != null)
            { 
                strChinese = GetDocumentContent(documentEntity, type);  
            }
            ChangeJson(strChinese);// Content( strChinese); 
        } 
        public void ChangeJson(object obj)
        {
            Response.Clear();
            Response.Charset = "UTF-8"; //设置字符集类型  
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Response.ContentType = "application/json";
            Jayrock.Json.JsonTextWriter writer = new Jayrock.Json.JsonTextWriter();
            Jayrock.Json.Conversion.JsonConvert.Export(obj, writer);
            Response.Write(writer);
            Response.Flush();
            Response.End();
        }
        private string GetDocUrl(DocumentEntity data, int type)
        {
            string path = data.F_HtmlUrl;
            switch (type)
            {
                case 1:
                    path = data.F_HtmlUrl;
                    break;
                case 2:
                    path = data.F_ChineseUrl;
                    break;
                case 3:
                    path = data.F_EnglishUrl;
                    break;
                case 4:
                    path = data.F_TotalUrl;
                    break;
                default:
                    break;
            }
            return path;
        }
        private string GetDocumentContent(DocumentEntity data, int type)
        {
            string strChinese = "";
            try
            {
                string path = GetDocUrl(data, type);
                if (!string.IsNullOrEmpty(path))
                {
                    FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                    strChinese = sr.ReadToEnd(); 
                    fs.Flush();
                    sr.Close();
                    fs.Close();
                }
            }
            catch (Exception e)
            {
                NFine.Code.Common.Log("log", "err", "读文件错误" + e.InnerException.Message);
            }
            return strChinese;
        }
        private int GetDocShowType(DocumentEntity data)
        {
            return GetDocShowTypeByString(data.F_TotalUrl, data.F_HtmlUrl, data.F_ChineseUrl, data.F_EnglishUrl);
        }

        private int GetDocShowTypeByString(string F_TotalUrl, string F_HtmlUrl, string F_ChineseUrl, string F_EnglishUrl)
        {
            if (!string.IsNullOrEmpty(F_TotalUrl)
                && !string.IsNullOrEmpty(F_HtmlUrl)
                && !string.IsNullOrEmpty(F_ChineseUrl)
                && !string.IsNullOrEmpty(F_EnglishUrl))
            {
                return 4;
            }
            else if (!string.IsNullOrEmpty(F_ChineseUrl) && !string.IsNullOrEmpty(F_EnglishUrl))
            {
                return 3;
            }

            else if (!string.IsNullOrEmpty(F_EnglishUrl))
            {
                return 2;
            }
            else if (!string.IsNullOrEmpty(F_ChineseUrl))
            {
                return 1;
            }
            return 0;
        }
        #endregion

        #region 其它


        private string GetSaveFilePath(DocumentEntity entity, int type, string fileName)
        {
            string path = "";
            if (entity != null)
            {
                if (type == 1)
                {
                    path = entity.F_ChineseUrl;
                }
                else
                {
                    path = entity.F_EnglishUrl;
                }
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            else
            {
                path = GetNewFileName(fileName);
            }
            if (string.IsNullOrEmpty(path))
            {
                path = GetNewFileName(fileName);
            }
            return path;
        }
        [HttpGet]
        public ActionResult Index(string keyword, int page = 1, int take = 10)
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public JsonResult GetGridJson(string keyword, string sidx, string sord, int limit, int offset, int index, int type)
        {
            int total = 0;
            IList<DocumentModel> entityList = documentApp.GetDataList((offset/ limit)+1, limit, keyword, index, type, out total);
            var totalCount = total;
            var pageSize = entityList.ToList();
            return Json(new { total = totalCount, rows = pageSize }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetModelTotal()
        {
            var data = documentApp.GetDataTotal();
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = documentApp.GetForm(keyValue);
            return Content(data.ToJson());
        }
        #endregion
    }
}