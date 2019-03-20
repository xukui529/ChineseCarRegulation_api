
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
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace NFine.Repository.AuxiliaryManage
{
    /// <summary>
    /// DownloadInfoRepository
    /// </summary>	
    public class DownloadInfoRepository : RepositoryBase<DownloadInfoEntity>, IDownloadInfoRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<DownloadInfoEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(DownloadInfoEntity entity, string keyValue)
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
        

        public new List<DownloadInfoEntity> FindList(Pagination pagination, string keyWord)
        {

            var p = ExtLinq.True<DownloadInfoEntity>();
            var tempData = dbcontext.Set<DownloadInfoEntity>().Where(p);
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
                var parameter = Expression.Parameter(typeof(DownloadInfoEntity), "t");
                var property = typeof(DownloadInfoEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(DownloadInfoEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<DownloadInfoEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<DownloadInfoEntity>(pagination.rows * (pagination.page - 1)).Take<DownloadInfoEntity>(pagination.rows).AsQueryable();

            var resultData = from table1 in tempData 
                             orderby table1.F_CreatorTime descending
                             select new DownloadInfoEntity
                             {
                                 F_Id = table1.F_Id,
                                 F_Type = table1.F_Type,
                                 F_UserId = table1.F_UserId,
                                 F_SuperUId = table1.F_SuperUId,
                                 F_Num = table1.F_Num,
                                 F_DocumentId = table1.F_DocumentId,
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
        public List<DocumentVisitInfoStatModel> GetItemList(DateTime beginTime, DateTime endTime, int type)
        {
            string strSql = "";
            switch (type)
            {
                case 1:
                    strSql = " select convert(varchar(50) ,DATEPART(HH,F_CreatorTime)) Title, convert( varchar(50), count(1) ) Num from [Auxiliary_DownloadInfo] where F_CreatorTime >@begintime and  F_CreatorTime <@endtime group by DATEPART(HH,F_CreatorTime) ";
                    break;
                case 2:
                case 3:
                    strSql = " select  convert(varchar(50) , DATEPART(day,F_CreatorTime)) Title,  convert( varchar(50), count(1) )  Num from [Auxiliary_DownloadInfo] where F_CreatorTime >@begintime and  F_CreatorTime <@endtime group by DATEPART(day,F_CreatorTime) ";
                    break;
                case 4:
                    strSql = "select  convert( varchar(50), DATEPART(month,F_CreatorTime)) Title,  convert( varchar(50), count(1) )  Num from [Auxiliary_DownloadInfo] where F_CreatorTime >@begintime and  F_CreatorTime <@endtime group by DATEPART(month,F_CreatorTime) ";
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

        public List<DownloadInfoRankingModel> GetRankingList()
        {
            string   strSql = " select top 10    convert( varchar(50) ,  a.num) as num  ,   convert( varchar(50) ,b.F_Type) F_Type,a.F_DocumentId,b.F_ChineseTitle F_DocumentChineseTitle,b.F_EnglishTitle F_DocumentEnglishTitle from( select[F_DocumentId], count(1) as num from Auxiliary_DownloadInfo group by [F_DocumentId]) a left join[dbo].[Auxiliary_Document] b on a.F_DocumentId = b.F_Id  order by a.num desc ";
       
            return dbcontext.Database.SqlQuery<DownloadInfoRankingModel>(strSql.ToString()).ToList();
        }
    }
}