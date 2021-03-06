﻿using FoodTrace.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodTrace.Model.BaseDto;
using FoodTrace.Model.DtoModel;

namespace FoodTrace.IService
{
    /// <summary>
    /// 基地管理
    /// </summary>
    public interface ILandBaseService
    {
        /// <summary>
        /// 获取LandBase总条数
        /// </summary>
        /// <returns></returns>
        int GetLandBaseCount();

        /// <summary>
        /// 获取LandBase总条数
        /// </summary>
        /// <param name="name">查询条件：基地名称（模糊查询）</param>
        /// <returns></returns>
        int GetLandBaseCount(string name);

        /// <summary>
        /// 获取当前用户的所在公司管理的基地（分页）
        /// </summary>
        /// <param name="name">查询条件：基地名称（模糊查询）</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <returns></returns>
        List<LandBaseModel> GetPagerLandBase(string name, int pageIndex, int pageSize);

        /// <summary>
        /// 通过ID获取LandBase
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        LandBaseModel GetLandBaseById(int id);

        /// <summary>
        /// 新增单条LandBase
        /// </summary>
        /// <param name="model">种子信息实体</param>
        /// <returns></returns>
        MessageModel InsertSingleLandBase(LandBaseModel model);

        /// <summary>
        /// 编辑单条LandBase
        /// </summary>
        /// <param name="model">种子信息实体</param>
        /// <returns></returns>
        MessageModel UpdateSingleLandBase(LandBaseModel model);

        /// <summary>
        /// 删除单条LandBase
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        MessageModel DeleteSingleLandBase(int id);

        /// <summary>
        /// 养殖基地分页数据
        /// </summary>
        /// <param name="pIndex"></param>
        /// <param name="pSize"></param>
        /// <returns></returns>
        GridList<LandBaseDto> GetLandBaseListPaging(int pIndex, int pSize, string name);

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        MessageModel DelLandBaseByIds(string ids);

        /// <summary>
        /// 根据类别获取基地1:种植,2:养殖
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        List<LandBaseDto> GetLandBaseListByType(int type);

        LandBaseModel GetLandBaseByCode(string code);
    }
}
