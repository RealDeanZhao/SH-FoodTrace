﻿using FoodTrace.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodTrace.IDBAccess
{
    public interface ICityAccess : IBaseAccess<CityModel>
    {
        int GetEntityCount(int? provinceId, string name);

        List<CityModel> GetPagerCityByConditions(int? provinceId, string name, int pageIndex, int pageSize);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        MessageModel DeleteCityByIds(string ids);

        /// <summary>
        /// 根据省份获取城市列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<ZtreeModel> GetCityListByProvinceId(int id);
    }
}
