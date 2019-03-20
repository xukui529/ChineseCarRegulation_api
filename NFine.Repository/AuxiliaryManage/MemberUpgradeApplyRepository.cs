
using System;
using NFine.Code;
using NFine.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.IRepository.AuxiliaryManage;

namespace NFine.Repository.AuxiliaryManage
{
    /// <summary>
    /// MemberUpgradeApplyRepository
    /// </summary>	
    public class MemberUpgradeApplyRepository : RepositoryBase<MemberUpgradeApplyEntity>, IMemberUpgradeApplyRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<MemberUpgradeApplyEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(MemberUpgradeApplyEntity entity, string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                if (!string.IsNullOrEmpty(keyValue))
                {
                    db.Update(entity);
                }
                else
                {
                    db.Insert(entity);
                }
                db.Commit();
            }
        }


        public string GetMaxCode()
        {
            var query = dbcontext.Set<MemberUpgradeApplyEntity>().Where(
                x => x.F_CreatorTime.ToString() == DateTime.Now.ToString()
                ).Count();
            return "OD" + DateTime.Now.ToString("yyyyMMdd") + (query.ToInt() == 0 ? (query + 1).ToString("000") : query.ToString("000"));
        }


        public new List<MemberUpgradeApplyEntity> FindList(Pagination pagination, string keyWord)
        {

            var p = ExtLinq.True<MemberUpgradeApplyEntity>();
            var tempData = dbcontext.Set<MemberUpgradeApplyEntity>().Where(p);
            if (!string.IsNullOrEmpty(keyWord))
            {
                tempData = tempData.Where(t => t.F_Id == keyWord);
            }

            tempData = tempData.Where(t => t.F_DeleteMark == false);

            bool isAsc = pagination.sord.ToLower() == "asc" ? true : false;
            string[] _order = pagination.sidx.Split(',');
            MethodCallExpression resultExp = null;

            foreach (string item in _order)
            {
                string _orderPart = item;
                _orderPart = Regex.Replace(_orderPart, @"\s+", " ");
                string[] _orderArry = _orderPart.Split(' ');
                string _orderField = _orderArry[0];
                bool sort = isAsc;
                if (_orderArry.Length == 2)
                {
                    isAsc = _orderArry[1].ToUpper() == "ASC" ? true : false;
                }
                var parameter = Expression.Parameter(typeof(MemberUpgradeApplyEntity), "t");
                var property = typeof(MemberUpgradeApplyEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(MemberUpgradeApplyEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<MemberUpgradeApplyEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<MemberUpgradeApplyEntity>(pagination.rows * (pagination.page - 1)).Take<MemberUpgradeApplyEntity>(pagination.rows).AsQueryable();

            var resultData = from table1 in tempData
                                 // join table2 in dbcontext.Set<CardTemplateEntity>() on table1.F_TemplateID equals table2.F_Id into table3
                                 //   from table2 in table3.DefaultIfEmpty()
                             orderby table1.F_CreatorTime descending
                             select new MemberUpgradeApplyEntity
                             {
                                 F_Id = table1.F_Id,
                                 F_MemberId = table1.F_MemberId,
                                 F_Country = table1.F_Country,
                                 F_Phone = table1.F_Phone,
                                 F_Account = table1.F_Account,
                                 F_HYType = table1.F_HYType,
                                 F_OpenUpTime = table1.F_OpenUpTime,
                                 F_Remark = table1.F_Remark,
                                 F_Status = table1.F_Status,
                                 F_CreatorUserId = table1.F_CreatorUserId,
                                 F_CreatorTime = table1.F_CreatorTime,
                                 F_DeleteMark = table1.F_DeleteMark,
                                 F_DeleteUserId = table1.F_DeleteUserId,
                                 F_DeleteTime = table1.F_DeleteTime,
                                 F_LastModifyUserId = table1.F_LastModifyUserId,
                                 F_LastModifyTime = table1.F_LastModifyTime,

                             };
            return resultData.ToList();
        }


       
        /// <summary>
        /// 审批申请 修改状态 审核通过 
        /// </summary>
        /// <param name="keyValue"></param>
        /// <param name="status">1待审核 2 审核通过 （3审核拒绝：目前没有使用）</param>
        public void UpdateState(string keyValue,int status)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                MemberUpgradeApplyEntity entity = new MemberUpgradeApplyEntity();
                entity = db.FindEntity<MemberUpgradeApplyEntity>(keyValue);
                entity.F_Status = status;
                if (entity.F_Status == 2)
                {
                    entity.F_OpenUpTime = DateTime.Now;
                } 
                db.Update(entity);
                db.Commit();
            }
        }
    }
}