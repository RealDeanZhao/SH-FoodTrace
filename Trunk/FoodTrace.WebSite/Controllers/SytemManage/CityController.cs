﻿using System;
using FoodTrace.Common.Libraries;
using FoodTrace.IService;
using System.Web.Mvc;
using FoodTrace.Model;
using System.Collections.Generic;
using System.Linq;

namespace FoodTrace.WebSite.Controllers
{
    public class CityController : BaseController
    {
        ICityService cityService;
        IProvinceService provinceService;

        public CityController(ICityService cityService, IProvinceService provinceService)
        {
            this.cityService = cityService;
            this.provinceService = provinceService;
        }

        // GET: City
        public ActionResult Index()
        {
            //var cityList = cityService.GetPagerCity(null, string.Empty, 1, 100);
            //return View(cityList);
            return View();
        }

        public ActionResult GetList(int page, int rows)
        {
            var count = cityService.GetCityCount();
            string cityName = RequestHelper.RequestPost("cityName", string.Empty);
            var cityList = cityService.GetPagerCity(null, cityName, page, rows).Select(m => new
            {
                CityID = m.CityID,
                CityCode = m.CityCode,
                CityName = m.CityName,
                CityAreaCode = m.CityAreaCode,
                ProvinceID = m.ProvinceID,
                ProvinceName = m.Province.ProvinceName
            });
            return Json(new{total=count,rows=cityList}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            var provinceList = provinceService.GetPagerProvince(null, string.Empty, 1, 100);
            List<SelectListItem> itemList = new List<SelectListItem>();
            provinceList.ForEach(m =>
            {
                itemList.Add(new SelectListItem() { Text = m.ProvinceName, Value = m.ProvinceID.ToString() });
            });
            ViewBag.ProvinceList = itemList;
            return PartialView(new CityModel());
        }

        [HttpPost]
        public ActionResult Create(CityModel cityModel)
        {
            var result = cityService.InsertSingleCity(cityModel);
            var flag = result.Status == MessageStatus.Success ? true : false;
            var msg = result.Message;
            return Json(new { flag = flag, msg = msg });
        }


        public ActionResult Edit(int id)
        {
            var provinceList = provinceService.GetPagerProvince(null, string.Empty, 1, 100);
            List<SelectListItem> itemList = new List<SelectListItem>();
            provinceList.ForEach(m =>
            {
                itemList.Add(new SelectListItem() { Text = m.ProvinceName, Value = m.ProvinceID.ToString() });
            });
            ViewBag.ProvinceList = itemList;
            var city = cityService.GetCityById(id);
            return PartialView(city);
        }

        [HttpPost]
        public ActionResult Edit(CityModel cityModel)
        {
            var result = cityService.UpdateSingleCity(cityModel);
            var flag = result.Status == MessageStatus.Success ? true : false;
            var msg = result.Message;
            return Json(new { flag = flag, msg = msg });
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            var result = cityService.DeleteSingleCity(id);
            var flag = result.Status == MessageStatus.Success ? true : false;
            var msg = result.Message;
            return Json(new { flag = flag, msg = msg });
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public JsonResult DeleteByIds(string ids)
        {
            var result = cityService.DeleteByIds(ids);
            var flag = result.Status == MessageStatus.Success ? true : false;
            var msg = result.Message;
            return Json(new { flag = flag, msg = msg });
        }

        /// <summary>
        /// 根据省份获取城市列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetCityListByProvinceId(int id)
        {
            var result = new ResultJson();
            try
            {
                var data = cityService.GetCityListByProvinceId(id);
                result.Data = data;
                result.IsSuccess = true;
            }
            catch (Exception)
            {
                
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}