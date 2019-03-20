
using System;
using NFine.Code;
using NFine.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.IRepository.AuxiliaryManage;
using System.Data;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using NFine.Domain.ViewModel;

namespace NFine.Repository.AuxiliaryManage
{
    /// <summary>
    /// DocumentVisitInfoRepository
    /// </summary>	
    public class DocumentVisitInfoRepository : RepositoryBase<DocumentVisitInfoEntity>, IDocumentVisitInfoRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<DocumentVisitInfoEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(DocumentVisitInfoEntity entity, string keyValue)
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
         
        public new List<DocumentVisitInfoEntity> FindList(Pagination pagination, string keyWord)
        {

            var p = ExtLinq.True<DocumentVisitInfoEntity>();
            var tempData = dbcontext.Set<DocumentVisitInfoEntity>().Where(p);
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
                var parameter = Expression.Parameter(typeof(DocumentVisitInfoEntity), "t");
                var property = typeof(DocumentVisitInfoEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(DocumentVisitInfoEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<DocumentVisitInfoEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<DocumentVisitInfoEntity>(pagination.rows * (pagination.page - 1)).Take<DocumentVisitInfoEntity>(pagination.rows).AsQueryable();

            var resultData = from table1 in tempData
                                 // join table2 in dbcontext.Set<CardTemplateEntity>() on table1.F_TemplateID equals table2.F_Id into table3
                                 //   from table2 in table3.DefaultIfEmpty()
                             orderby table1.F_CreatorTime descending
                             select new DocumentVisitInfoEntity
                             {
                                 F_Id = table1.F_Id,
                                 F_UserId = table1.F_UserId,
                                 F_DocumentId = table1.F_DocumentId,
                                 F_Type = table1.F_Type,
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
        public List<DocumentVisitInfoStatModel> GetItemList(DateTime beginTime,DateTime endTime,int type)
        {
            string strSql ="";
            switch (type)
            {
                case 1:
                    strSql= " select convert(varchar(50) ,DATEPART(HH,F_CreatorTime)) Title, convert( varchar(50), count(1) ) Num from [Auxiliary_DocumentVisitInfo] where F_CreatorTime >@begintime and  F_CreatorTime <@endtime group by DATEPART(HH,F_CreatorTime) ";
                    break;
                case 2: 
                case 3:
                    strSql = " select  convert(varchar(50) , DATEPART(day,F_CreatorTime)) Title,  convert( varchar(50), count(1) )  Num from [Auxiliary_DocumentVisitInfo] where F_CreatorTime >@begintime and  F_CreatorTime <@endtime group by DATEPART(day,F_CreatorTime) ";
                    break;
                case 4:
                    strSql = "select  convert( varchar(50), DATEPART(day,F_CreatorTime)) Title,  convert( varchar(50), count(1) )  Num from [Auxiliary_DocumentVisitInfo] where F_CreatorTime >@begintime and  F_CreatorTime <@endtime group by DATEPART(day,F_CreatorTime) "; 
                    break;
            } 
            DbParameter[] parameter =
            {
                 new SqlParameter("@begintime" , SqlDbType.DateTime ),
                 new SqlParameter("@endTime" , SqlDbType.DateTime)
            };
            parameter[0].Value = beginTime;
            parameter[1].Value = endTime;
            return dbcontext.Database.SqlQuery<DocumentVisitInfoStatModel>(strSql.ToString(), parameter).ToList();
        }

        public List<DocumentSearchKeysStatModel> GetStat()
        {
            string strSql = "select  convert(varchar(50), Num)  Num ,F_ChineseTitle as Keys from  [dbo].[Auxiliary_Document] left join (select top 10 Num,F_DocumentId  as  F_DocumentId  from ( select   count(1)  Num, F_DocumentId from Auxiliary_DocumentVisitInfo   group by F_DocumentId ) a order by Num  ) b on [Auxiliary_Document].F_Id=b.F_DocumentId order by  b.Num desc";
            return dbcontext.Database.SqlQuery<DocumentSearchKeysStatModel>(strSql.ToString()).ToList();
        }
    }
}