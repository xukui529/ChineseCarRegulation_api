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
    public class ItemsDataController : Controller
    {
        private ItemsDetailApp itemsDetailApp = new ItemsDetailApp();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string itemId, string keyword)
        {
            var data = itemsDetailApp.GetList(itemId, keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSelectJson(string enCode)
        {
            var data = itemsDetailApp.GetItemList(enCode);
            List<object> list = new List<object>();
            foreach (ItemsDetailEntity item in data)
            {
                list.Add(new { id = item.F_ItemCode, text = item.F_ItemName });
            }
            return Content(list.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSelectJsonByCodes(string  enCodes)
        {
            List<object> reList = new List<object>();
            var codeLists = enCodes.Split(',');
            foreach (var enCode in codeLists)
            {
                if (!string.IsNullOrEmpty(enCode))
                {
                    var data = itemsDetailApp.GetItemList(enCode);
                    List<object> list = new List<object>();
                    foreach (ItemsDetailEntity item in data)
                    {
                        list.Add(new { id = item.F_ItemCode, text = item.F_ItemName });
                    }
                    reList.Add(list);
                }
            } 
            return Content(reList.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = itemsDetailApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HandlerAuthorize]
        public ActionResult CreateModel(string keyValue)
        {
            ItemsDetailEntity entity = new ItemsDetailEntity();
            ViewData["id"] = "";
			ViewData["title"] = "新建字典";
			if (!string.IsNullOrEmpty(keyValue))
            {
                ViewData["id"] = keyValue;
				ViewData["title"] = "编辑字典";
				entity = itemsDetailApp.GetForm(keyValue);
            }
            return View(entity);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateModel(ItemsDetailEntity itemsDetailEntity, string keyValue)
        {
            itemsDetailEntity.F_DeleteMark = false;
            itemsDetailApp.SubmitForm(itemsDetailEntity, keyValue);
            return Content("操作成功");
        }

        public ActionResult DetailModel(string keyValue)
        {
            ItemsDetailEntity entity = new ItemsDetailEntity();
            entity = itemsDetailApp.GetForm(keyValue);
            return View(entity);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        public ActionResult DeleteForm(string keyValue)
        {
            itemsDetailApp.DeleteForm(keyValue);
            return Content("删除成功");
        }
    }
}