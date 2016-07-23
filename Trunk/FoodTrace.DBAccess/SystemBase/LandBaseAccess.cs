﻿using FoodTrace.Common.Libraries;
using FoodTrace.DBManage.IContexts;
using FoodTrace.IDBAccess;
using FoodTrace.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodTrace.DBAccess
{
    public class LandBaseAccess : BaseAccess, ILandBaseAccess
    {
        public int GetEntityCount()
        {
            return base.Context.LandBase.Count();
        }

        public int GetEntityCount(int companyID, string name)
        {
            return base.Context.LandBase.Where(m => m.CompanyID == companyID
                                                    && (string.IsNullOrEmpty(name) || m.LandName.Contains(name))).Count();
        }

        public List<LandBaseModel> GetAllEntities()
        {
            return base.Context.LandBase.ToList();
        }

        public LandBaseModel GetEntityById(int id)
        {
            return base.Context.LandBase.FirstOrDefault(m => m.LandID == id);
        }


        public MessageModel InsertSingleEntity(LandBaseModel model)
        {
            Func<IEntityContext, string> operation = delegate (IEntityContext context)
            {
                context.LandBase.Add(model);
                context.SaveChanges();
                return string.Empty;
            };
            return base.DbOperation(operation);
        }

        public LandBaseModel GetOriEntity(int id, DateTime? modifyTime)
        {
            return base.Context.LandBase.FirstOrDefault(m => m.LandID == id && m.ModifyTime == modifyTime);
        }

        public MessageModel UpdateSingleEntity(LandBaseModel model)
        {
            Func<IEntityContext, string> operation = delegate (IEntityContext context)
            {
                var data = context.LandBase.FirstOrDefault(m => m.LandID == model.LandID && m.ModifyTime == model.ModifyTime);
                if (data == null) return "当前数据不存在或被更新，请刷新后再次操作！";
                data.CompanyID = model.CompanyID;
                data.LandCode = model.LandCode;
                data.LandName = model.LandName;
                data.Location = model.Location;
                data.LandTime = model.LandTime;
                data.LandArea = model.LandArea;
                data.EmployeesNum = model.EmployeesNum;
                data.LandState = model.LandState;
                data.LandType = model.LandType;
                data.Address = model.Address;
                data.Lon = model.Lon;
                data.Lat = model.Lat;
                data.Remark = model.Remark;
                data.IsLocked = model.IsLocked;
                data.IsShow = model.IsShow;
                data.ModifyID = UserManagement.CurrentUser.UserID;
                data.ModifyName = UserManagement.CurrentUser.UserName;
                data.ModifyTime = DateTime.Now;
                context.SaveChanges();
                return string.Empty;
            };
            return base.DbOperation(operation);
        }

        public MessageModel DeleteSingleEntity(int id)
        {
            Func<IEntityContext, string> operation = delegate (IEntityContext context)
            {
                //基地名下包含基地地块信息，则不能被直接删除
                if (context.LandBlock.Any(m => m.LandID == id) || context.BreedBase.Any(m => m.LandID == id)) return "该基地信息存在关联数据，不能被删除！";

                var data = Context.LandBase.FirstOrDefault(m => m.LandID == id);
                if (data == null) return "当前数据不存在或被更新，请刷新后再次操作！";

                context.DeleteAndSave<LandBaseModel>(id);
                return string.Empty;
            };

            return base.DbOperation(operation);
        }

        public List<LandBaseModel> GetPagerLandBaseByConditions(int companyID, string name, int pageIndex, int pageSize)
        {
            return base.Context.LandBase.Where(m => m.CompanyID == companyID
                                                    && (string.IsNullOrEmpty(name) || m.LandName.Contains(name)))
                   .OrderBy(m => m.LandID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}