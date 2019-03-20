
using System;
using NFine.Code;
using NFine.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.IRepository.AuxiliaryManage;
using NFine.Domain.ViewModel;

namespace NFine.Repository.AuxiliaryManage
{
    /// <summary>
    /// DocumentSearchKeysRepository
    /// </summary>	
    public class DocumentSearchKeysRepository : RepositoryBase<DocumentSearchKeysEntity>, IDocumentSearchKeysRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<DocumentSearchKeysEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(DocumentSearchKeysEntity entity, string keyValue)
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
            var query = dbcontext.Set<DocumentSearchKeysEntity>().Where(
                x => x.F_CreatorTime.ToString() == DateTime.Now.ToString()
                ).Count();
            return "OD" + DateTime.Now.ToString("yyyyMMdd") + (query.ToInt() == 0 ? (query + 1).ToString("000") : query.ToString("000"));
        }


        public new List<DocumentSearchKeysEntity> FindList(Pagination pagination, string keyWord)
        {

            var p = ExtLinq.True<DocumentSearchKeysEntity>();
            var tempData = dbcontext.Set<DocumentSearchKeysEntity>().Where(p);
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
                var parameter = Expression.Parameter(typeof(DocumentSearchKeysEntity), "t");
                var property = typeof(DocumentSearchKeysEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(DocumentSearchKeysEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<DocumentSearchKeysEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<DocumentSearchKeysEntity>(pagination.rows * (pagination.page - 1)).Take<DocumentSearchKeysEntity>(pagination.rows).AsQueryable();

            var resultData = from table1 in tempData
                                 // join table2 in dbcontext.Set<CardTemplateEntity>() on table1.F_TemplateID equals table2.F_Id into table3
                                 //   from table2 in table3.DefaultIfEmpty()
                             orderby table1.F_CreatorTime descending
                             select new DocumentSearchKeysEntity
                             {
                                 F_Id = table1.F_Id,
                                 F_Keys = table1.F_Keys,
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

        public List<DocumentSearchKeysStatModel> GetStat( )
        {
            string strSql = "select top 10  convert(varchar(50), Num)  Num ,F_Keys  as  Keys  from ( select   count(1)  Num, F_Keys from Auxiliary_DocumentSearchKeys   group by F_Keys ) a order by  Num desc";
            return dbcontext.Database.SqlQuery<DocumentSearchKeysStatModel>(strSql.ToString() ).ToList();
        }
    }
}