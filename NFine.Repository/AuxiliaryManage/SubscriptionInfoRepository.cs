
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
    /// SubscriptionInfoRepository
    /// </summary>	
    public class SubscriptionInfoRepository : RepositoryBase<SubscriptionInfoEntity>, ISubscriptionInfoRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<SubscriptionInfoEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(SubscriptionInfoEntity entity, string keyValue)
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
            var query = dbcontext.Set<SubscriptionInfoEntity>().Where(
                x => x.F_CreatorTime.ToString() == DateTime.Now.ToString()
                ).Count();
            return "OD" + DateTime.Now.ToString("yyyyMMdd") + (query.ToInt() == 0 ? (query + 1).ToString("000") : query.ToString("000"));
        }


        public new List<SubscriptionInfoEntity> FindList(Pagination pagination, string keyWord)
        {

            var p = ExtLinq.True<SubscriptionInfoEntity>();
            var tempData = dbcontext.Set<SubscriptionInfoEntity>().Where(p);
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
                var parameter = Expression.Parameter(typeof(SubscriptionInfoEntity), "t");
                var property = typeof(SubscriptionInfoEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(SubscriptionInfoEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<SubscriptionInfoEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<SubscriptionInfoEntity>(pagination.rows * (pagination.page - 1)).Take<SubscriptionInfoEntity>(pagination.rows).AsQueryable();

            var resultData = from table1 in tempData
                                 // join table2 in dbcontext.Set<CardTemplateEntity>() on table1.F_TemplateID equals table2.F_Id into table3
                                 //   from table2 in table3.DefaultIfEmpty()
                             orderby table1.F_CreatorTime descending
                             select new SubscriptionInfoEntity
                             {
                                 F_Id = table1.F_Id,
                                 F_SubscriptionId = table1.F_SubscriptionId,
                                 F_SendNum = table1.F_SendNum,
                                 F_F_DocumentId = table1.F_F_DocumentId,
                                 F_IsExclude = table1.F_IsExclude,
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

    }
}