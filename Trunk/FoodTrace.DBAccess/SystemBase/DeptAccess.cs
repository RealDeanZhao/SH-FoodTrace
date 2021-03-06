﻿using FoodTrace.Common.Libraries;
using FoodTrace.DBManage.IContexts;
using FoodTrace.IDBAccess;
using FoodTrace.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodTrace.Model.BaseDto;

namespace FoodTrace.DBAccess
{
    public class DeptAccess : BaseAccess, IDeptAccess
    {
        public int GetEntityCount()
        {
            return base.Context.Dept.Count();
        }

        public int GetEntityCount(int companyID, string name)
        {
            return base.Context.Dept.Where(m => m.CompanyID == companyID
                                            && (string.IsNullOrEmpty(name) || m.DeptName.Contains(name))).Count();
        }

        public List<DeptModel> GetAllEntities()
        {
            return base.Context.Dept.ToList();
        }

        public DeptModel GetEntityById(int id)
        {
            return base.Context.Dept.FirstOrDefault(m => m.DeptID == id);
        }

        public MessageModel InsertSingleEntity(DeptModel model)
        {
            Func<IEntityContext, string> operation = delegate (IEntityContext context)
            {
                model.ModifyID = UserManagement.CurrentUser.UserID;
                model.ModifyName = UserManagement.CurrentUser.UserName;
                model.ModifyTime = DateTime.Now;
                context.Dept.Add(model);
                context.SaveChanges();
                return string.Empty;
            };
            return base.DbOperation(operation);
        }

        public DeptModel GetOriEntity(int id, DateTime? modifyTime)
        {
            return base.Context.Dept.FirstOrDefault(m => m.DeptID == id && m.ModifyTime == modifyTime);
        }

        public MessageModel UpdateSingleEntity(DeptModel model)
        {
            Func<IEntityContext, string> operation = delegate (IEntityContext context)
            {
                var data = context.Dept.FirstOrDefault(m => m.DeptID == model.DeptID);
                if (data == null) return "当前数据不存在或被更新，请刷新后再次操作！";
                data.CompanyID = model.CompanyID;
                data.DeptName = model.DeptName;
                data.UpperDeptID = model.UpperDeptID;
                data.DeptRemark = model.DeptRemark;
                data.SortID = model.SortID;
                data.ModifyID = UserManagement.CurrentUser.UserID;
                data.ModifyName = UserManagement.CurrentUser.UserName;
                data.ModifyTime = DateTime.Now;
                context.UpdateAndSave(data);
                return string.Empty;
            };
            return base.DbOperation(operation);
        }

        public MessageModel DeleteSingleEntity(int id)
        {
            Func<IEntityContext, string> operation = delegate (IEntityContext context)
            {
                //公司名下包含基地信息，则不能被直接删除
                if (context.UserBase.Any(m => m.DeptID == id)) return "该部门信息存在关联数据，不能被删除！";

                var data = Context.Dept.FirstOrDefault(m => m.DeptID == id);
                if (data == null) return "当前数据不存在或被更新，请刷新后再次操作！";
                context.DeleteAndSave<DeptModel>(id);
                return string.Empty;
            };

            return base.DbOperationInTransaction(operation);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public MessageModel DeleteDepts(string ids)
        {
            Func<IEntityContext, string> opera = delegate(IEntityContext context)
            {
                var idsArray = ids.Split(',');
                var dept =(from s in context.Dept
                            join id in idsArray on s.DeptID.ToString() equals  id
                            select s).ToList();
                if (dept.Any())
                {
                    context.BatchDelete(dept);
                }

                return string.Empty;
            };

            return base.DbOperationInTransaction(opera);
        }
        public List<DeptModel> GetPagerDeptByConditions(string name, int pageIndex, int pageSize,int? companyID )
        {
            var query = Context.Dept.AsQueryable();

            if (companyID != null)
            {
                query=query.Where(s=>s.CompanyID==companyID);
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(s => s.DeptName.Contains(name));
            }
            return query.OrderBy(m => m.DeptID).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        /// <summary>
        /// 部门分页数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pIndex"></param>
        /// <param name="pSize"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        public GridList<DeptDto> GetDeptPagingList(string name, int pIndex, int pSize, int? companyId)
        {
            var query =(from s in  Context.Dept
                        join d in Context.Dept on s.UpperDeptID equals d.DeptID into dl   
                        join com in Context.Company on s.CompanyID equals com.CompanyID into coml
                        from dleft in dl.DefaultIfEmpty()
                        from comleft in coml.DefaultIfEmpty()
                        select new DeptDto()
                        {
                            DeptID = s.DeptID,
                            DeptName = s.DeptName,
                            CompanyId = s.CompanyID,
                            CompanyName =comleft.CompanyName,
                            UpperDeptName = dleft.DeptName,
                            DeptRemark=s.DeptRemark,
                            SortID = s.SortID
                        }).AsQueryable();

            if (companyId != null)
            {
                query = query.Where(s => s.CompanyId == companyId);
            }
            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(s => s.DeptName.Contains(name));
            }
            var list= query.OrderBy(m => m.DeptID).Skip((pIndex - 1) * pSize).Take(pSize).ToList();

            return new GridList<DeptDto>(){rows = list,total = query.Count()}; 
        }

        /// <summary>
        /// 获取部门树
        /// </summary>
        /// <param name="comid"></param>
        /// <returns></returns>
        public ComboxTreeDto GetDeptComTree(int comid)
        {
            var qeury = (from s in Context.Dept
                where s.CompanyID == comid
                select s).ToList();
            var treeNode = new ComboxTreeDto(){id="0",text = "全部"};
            InitDeptTree(qeury, treeNode, null);

            return treeNode;
        }

        /// <summary>
        /// 构造树形数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="node"></param>
        /// <param name="pId"></param>
        private void InitDeptTree(List<DeptModel> list, ComboxTreeDto node, int? pId)
        {
            var temp=new List<DeptModel>();

            if (pId != null)
            {
                temp = list.Where(a => a.UpperDeptID == pId).ToList();
            }
            else
            {
                temp = list;
            }

            foreach (var item in temp)
            {
                var nodeItem = new ComboxTreeDto(item.DeptID.ToString(),item.DeptName);
               
                node.children.Add(nodeItem);
                InitDeptTree(list, nodeItem, item.DeptID);
            }
        }
    }
}
