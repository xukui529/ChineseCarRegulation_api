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
    public class ItemsTypeController : Controller
    {
        private ItemsApp itemsApp = new ItemsApp();

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var data = itemsApp.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (ItemsEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_FullName;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeJson()
        {
            var data = itemsApp.GetList();
            var treeList = new List<TreeViewModel>();
            foreach (ItemsEntity item in data)
            {
                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                tree.id = item.F_Id;
                tree.text = item.F_FullName;
                tree.value = item.F_EnCode;
                tree.parentId = item.F_ParentId;
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson()
        {
            var data = itemsApp.GetList();
            return Content(data.ToJson());
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = itemsApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HandlerAuthorize]
        public ActionResult CreateModel(string keyValue)
        {
            ItemsEntity entity = new ItemsEntity();
            ViewData["id"] = "";
			ViewData["title"] = "新建分类";
			if (!string.IsNullOrEmpty(keyValue))
            {
                ViewData["id"] = keyValue;
				ViewData["title"] = "编辑分类";
				entity = itemsApp.GetForm(keyValue);
            }
            return View(entity);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateModel(ItemsEntity itemsEntity, string keyValue)
        {
           // var data = itemsApp.GetList();
           // ItemsEntity hasSameNameItem = null;
            if (string.IsNullOrEmpty(keyValue))
            {
                itemsEntity.F_IsTree = false;
                itemsEntity.F_Layers = 2;              
              //  hasSameNameItem = data.Find(i => i.F_FullName == itemsEntity.F_FullName);               
            }
            //hasSameNameItem = data.Find(i => i.F_FullName == itemsEntity.F_FullName && i.F_Id != keyValue);
            if (itemsApp.CheckFullName(itemsEntity.F_FullName, keyValue))
            {
                return Json("已经包含该名称，不能重复录入！");
            }
			itemsEntity.F_EnabledMark = true;
			itemsEntity.F_DeleteMark = false;
            itemsApp.SubmitForm(itemsEntity, keyValue);
            return Content("操作成功");
        }

        [HandlerAuthorize]
        public ActionResult DetailModel(string keyValue)
        {
            return View();
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult DeleteForm(string keyValue)
        {
            itemsApp.DeleteForm(keyValue);
            return Content("删除成功");
        }
    }
}