﻿using FoodTrace.IService;
using FoodTrace.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FoodTrace.WebSite.Controllers
{
    public class MenuController : BaseController
    {
        IMenuService menuService;
        public MenuController(IMenuService mService)
        {
            menuService = mService;
        }
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetList(int page, int rows)
        {
            var list = menuService.GetPagerMenu(string.Empty, page, rows).Select(_ => new
            {
                MenuID = _.MenuID,
                Name = _.Name,
                ParentID = _.ParentID,
                SortID = _.SortID,
                FunctionURL = _.FunctionURL
            });
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var list = menuService.GetPagerMenu(string.Empty, 1, 100).OrderBy(_ => _.SortID).ToList();
            List<SelectListItem> itemList = new List<SelectListItem>();
            list.ForEach(m =>
            {
                itemList.Add(new SelectListItem() { Text = m.Name, Value = m.MenuID.ToString() });
            });
            itemList.Insert(0, new SelectListItem() { Text = "顶级分类", Value = "0" });
            ViewBag.MenuList = itemList;
            return PartialView(new MenuModel());
        }

        [HttpPost]
        public ActionResult Create(MenuModel model)
        {
            var result = menuService.InsertSingleMenu(model);
            var flag = result.Status == MessageStatus.Success ? true : false;
            var msg = result.Message;
            return Json(new { flag = flag, msg = msg });
        }

        public ActionResult Edit(int id)
        {
            var model = menuService.GetMenuById(id);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Edit(MenuModel model)
        {
            var result = menuService.UpdateSingleMenu(model);
            var flag = result.Status == MessageStatus.Success ? true : false;
            var msg = result.Message;
            return Json(new { flag = flag, msg = msg });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = menuService.DeleteSingleMenu(id);
            var flag = result.Status == MessageStatus.Success ? true : false;
            var msg = result.Message;
            return Json(new { flag = flag, msg = msg });
        }
    }
}