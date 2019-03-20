
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
    /// RegisterApplyRepository
    /// </summary>	
    public class RegisterApplyRepository : RepositoryBase<RegisterApplyEntity>, IRegisterApplyRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<RegisterApplyEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(RegisterApplyEntity entity, string keyValue)
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
            var query = dbcontext.Set<RegisterApplyEntity>().Where(
                x => x.F_CreatorTime.ToString() == DateTime.Now.ToString()
                ).Count();
            return "OD" + DateTime.Now.ToString("yyyyMMdd") + (query.ToInt() == 0 ? (query + 1).ToString("000") : query.ToString("000"));
        }


        public new List<RegisterApplyEntity> FindList(Pagination pagination, string keyWord)
        {

            var p = ExtLinq.True<RegisterApplyEntity>();
            var tempData = dbcontext.Set<RegisterApplyEntity>().Where(p);
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
                var parameter = Expression.Parameter(typeof(RegisterApplyEntity), "t");
                var property = typeof(RegisterApplyEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(RegisterApplyEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<RegisterApplyEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<RegisterApplyEntity>(pagination.rows * (pagination.page - 1)).Take<RegisterApplyEntity>(pagination.rows).AsQueryable();

            var resultData = from table1 in tempData
                                 // join table2 in dbcontext.Set<CardTemplateEntity>() on table1.F_TemplateID equals table2.F_Id into table3
                                 //   from table2 in table3.DefaultIfEmpty()
                             orderby table1.F_CreatorTime descending
                             select new RegisterApplyEntity
                             {
                                 F_Id = table1.F_Id, 
                                 F_Password = table1.F_Password,
                                 F_Email = table1.F_Email,
                                 F_IsSubscription = table1.F_IsSubscription,
                                 F_SubscriptionLanguage = table1.F_SubscriptionLanguage,
                                 F_SecretKey = table1.F_SecretKey,
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