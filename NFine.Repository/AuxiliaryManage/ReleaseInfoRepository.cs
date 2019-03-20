
using System;
using NFine.Code;
using NFine.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.IRepository.AuxiliaryManage;
using System.Data.Common;
using NFine.Domain.ViewModel;
using System.Data.SqlClient;
using System.Data;

namespace NFine.Repository.AuxiliaryManage
{
    /// <summary>
    /// ReleaseInfoRepository
    /// </summary>	
    public class ReleaseInfoRepository : RepositoryBase<ReleaseInfoEntity>, IReleaseInfoRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<ReleaseInfoEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(ReleaseInfoEntity entity, string keyValue)
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
            var query = dbcontext.Set<ReleaseInfoEntity>().Where(
                x => x.F_CreatorTime.ToString() == DateTime.Now.ToString()
                ).Count();
            return "OD" + DateTime.Now.ToString("yyyyMMdd") + (query.ToInt() == 0 ? (query + 1).ToString("000") : query.ToString("000"));
        }


        public new List<ReleaseInfoEntity> FindList(Pagination pagination, string keyWord)
        {

            var p = ExtLinq.True<ReleaseInfoEntity>();
            var tempData = dbcontext.Set<ReleaseInfoEntity>().Where(p);
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
                var parameter = Expression.Parameter(typeof(ReleaseInfoEntity), "t");
                var property = typeof(ReleaseInfoEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(ReleaseInfoEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<ReleaseInfoEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<ReleaseInfoEntity>(pagination.rows * (pagination.page - 1)).Take<ReleaseInfoEntity>(pagination.rows).AsQueryable();

            var resultData = from table1 in tempData
                                 // join table2 in dbcontext.Set<CardTemplateEntity>() on table1.F_TemplateID equals table2.F_Id into table3
                                 //   from table2 in table3.DefaultIfEmpty()
                             orderby table1.F_CreatorTime descending
                             select new ReleaseInfoEntity
                             {
                                 F_Id = table1.F_Id,
                                 F_Versions = table1.F_Versions,
                                 F_DocumentId = table1.F_DocumentId,
                                  F_Type = table1.F_Type,
                                 F_Remark = table1.F_Remark,
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

        public List<ReleaseInfoStatModel> GetItemList(DateTime beginTime, DateTime endTime, int type)
        {
            string strSql = "";
            switch (type)
            {
                case 1:
                    strSql = " select convert(varchar(50) ,DATEPART(HH,F_CreatorTime)) Title, convert( varchar(50), count(1) ) Num,F_Type from [Auxiliary_ReleaseInfo] where F_CreatorTime >@begintime and  F_CreatorTime <@endtime group by DATEPART(HH,F_CreatorTime),F_Type ";
                    break;
                case 2:
                case 3:
                case 4:
                    strSql = " select  convert(varchar(50) , DATEPART(day,F_CreatorTime)) Title,  convert( varchar(50), count(1) )  Num,F_Type from [Auxiliary_ReleaseInfo] where F_CreatorTime >@begintime and  F_CreatorTime <@endtime group by DATEPART(day,F_CreatorTime),F_Type ";
                    break; 
            }
            DbParameter[] parameter =
            {
                 new SqlParameter("@begintime" , SqlDbType.DateTime ),
                 new SqlParameter("@endTime" , SqlDbType.DateTime)
            };
            parameter[0].Value = beginTime;
            parameter[1].Value = endTime;
            return dbcontext.Database.SqlQuery<ReleaseInfoStatModel>(strSql.ToString(), parameter).ToList();
        }
    }
}