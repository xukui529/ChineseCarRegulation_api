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
    public class ModuleController : Controller
    {
        private ModuleApp moduleApp = new ModuleApp();
        private ModuleButtonApp moduleButtonApp = new ModuleButtonApp();
        // GET: SystemManage/Module

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeGridJson()
        {
            var data = moduleApp.GetList();
            var treeList = new List<TreeGridModel>();
            foreach (ModuleEntity item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                treeModel.id = item.F_Id;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.F_ParentId;
                treeModel.expanded = hasChildren;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeGridJson());
        }

        // 新增菜单
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetTreeSelectJson()
        {
            var data = moduleApp.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (ModuleEntity item in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = item.F_Id;
                treeModel.text = item.F_FullName;
                treeModel.parentId = item.F_ParentId;
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        // 新增or编辑
        [HandlerAuthorize]
        public ActionResult CreateModel(string keyValue)
        {
            ModuleEntity entity = new ModuleEntity();
            ViewData["id"] = "";
			ViewData["title"] = "新建菜单";
			if (!string.IsNullOrEmpty(keyValue))
            {
                ViewData["id"] = keyValue;
				ViewData["title"] = "编辑菜单";
				entity = moduleApp.GetForm(keyValue);
            }
            return View(entity);
        }

        // 编辑查询
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = moduleApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult CreateModel(ModuleEntity moduleEntity, string keyValue)
        {
            moduleEntity.F_EnabledMark = true;
            moduleEntity.F_IsMenu = true;
            moduleApp.SubmitForm(moduleEntity, keyValue);
            return Content("操作成功");
        }

        // 添加按钮事件
        [HandlerAuthorize]
        public ActionResult ButtonUpdateModel(string keyValue)
        {
            ViewData["id"] = keyValue;
            return View();
        }
        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetButtonTreeGridJson(string keyValue)
        {
            var data = moduleButtonApp.GetList(keyValue);
            var treeList = new List<TreeGridModel>();
            foreach (ModuleButtonEntity item in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = data.Count(t => t.F_ParentId == item.F_Id) == 0 ? false : true;
                treeModel.id = item.F_Id;
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = item.F_ParentId;
                treeModel.expanded = hasChildren;
                treeModel.entityJson = item.ToJson();
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeGridJson());
        }

        public ActionResult CreateButtonModel(string keyValue,string moduleId)
        {
            ModuleButtonEntity entity = new ModuleButtonEntity();
            ViewData["id"] = "";
            ViewData["moduleId"] = "";
            if (!string.IsNullOrEmpty(keyValue))
            {
                ViewData["id"] = keyValue;
                entity = moduleButtonApp.GetForm(keyValue);
            }
            if (!string.IsNullOrEmpty(moduleId))
            {
                ViewData["moduleId"] = moduleId;
            }
            return View(entity);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult CreateButtonModel(ModuleButtonEntity moduleButtonEntity, string keyValue)
        {
            moduleButtonEntity.F_ParentId = "0";
            moduleButtonApp.SubmitForm(moduleButtonEntity, keyValue);
            return Content("操作成功");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult DeleteForm(string keyValue)
        {
            moduleApp.DeleteForm(keyValue);
            return Content("删除成功");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        public ActionResult DeleteButtonForm(string keyValue)
        {
            moduleButtonApp.DeleteForm(keyValue);
            return Content("删除成功");
        }
    }
}