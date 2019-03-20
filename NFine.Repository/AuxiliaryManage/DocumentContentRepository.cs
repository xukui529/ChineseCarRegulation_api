
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
using NFine.Domain.ViewModel;
using System.Data.Common;
using System.Data.SqlClient;  

namespace NFine.Repository.AuxiliaryManage
{
    /// <summary>
    /// DocumentContentRepository
    /// </summary>	
    public class DocumentContentRepository : RepositoryBase<DocumentContentEntity>, IDocumentContentRepository
    {
        public void DeleteForm(string keyValue)
        {
            using (var db = new RepositoryBase().BeginTrans())
            {
                db.Delete<DocumentContentEntity>(t => t.F_Id == keyValue);
                db.Commit();
            }
        }
        public void SubmitForm(DocumentContentEntity entity, string keyValue)
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

 


        public new List<DocumentContentEntity> FindList(Pagination pagination, string keyWord)
        {

            var p = ExtLinq.True<DocumentContentEntity>();
            var tempData = dbcontext.Set<DocumentContentEntity>().Where(p);
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
                var parameter = Expression.Parameter(typeof(DocumentContentEntity), "t");
                var property = typeof(DocumentContentEntity).GetProperty(_orderField);
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                resultExp = Expression.Call(typeof(Queryable), isAsc ? "OrderBy" : "OrderByDescending", new Type[] { typeof(DocumentContentEntity), property.PropertyType }, tempData.Expression, Expression.Quote(orderByExp));
            }
            tempData = tempData.Provider.CreateQuery<DocumentContentEntity>(resultExp);
            pagination.records = tempData.Count();
            tempData = tempData.Skip<DocumentContentEntity>(pagination.rows * (pagination.page - 1)).Take<DocumentContentEntity>(pagination.rows).AsQueryable();

            var resultData = from table1 in tempData
                                 // join table2 in dbcontext.Set<CardTemplateEntity>() on table1.F_TemplateID equals table2.F_Id into table3
                                 //   from table2 in table3.DefaultIfEmpty()
                             orderby table1.F_CreatorTime descending
                             select new DocumentContentEntity
                             {
                                 F_Id = table1.F_Id,
                                 F_Type = table1.F_Type,
                                 F_DocumentId = table1.F_DocumentId,
                                 F_ChineseContent = table1.F_ChineseContent,
                                 F_EnglishContent = table1.F_EnglishContent,
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
        public List<DocumentContentModel> SearchForIndexByContent(int type, string fileState, string domain, string acquisitionStandard
            , string publisher, string direction, string keyword)
        {
            string strSql = "";
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql = " SELECT len(F_ChineseContent)-len(replace(F_ChineseContent, '" + keyword + "', ''))  F_ChineseContentAppearNum , len(F_EnglishContent)-len(replace(F_EnglishContent, '" + keyword + "', '')) F_EnglishContentAppearNum ,";
            }
            else
            {
                strSql = @" SELECT 0 F_ChineseContentAppearNum ,   0 F_EnglishContentAppearNum ,";
            }

            strSql = strSql+ @" 
  '' F_ChineseContent,
  '' F_EnglishContent,
  a.F_Id
  ,a.F_Type
  ,b.F_Id AS F_DocumentId
  ,b.F_ChineseTitle,
  b.F_EnglishTitle,
  b.F_CreatorTime,
  b.F_Label,
  b.F_FileState,
  b.F_Domain,
  b.F_AcquisitionStandard,
  b.F_Publisher,
  b.F_Direction,
  b.F_ReleaseDate,
  b.F_ImplementDate,
  b.F_ChineseUrl,
  b.F_EnglishUrl,
  b.F_HtmlUrl,
  b.F_TotalUrl
  FROM [Auxiliary_DocumentContent] a
  LEFT JOIN [dbo].[Auxiliary_Document] b ON a.F_DocumentId=b.F_Id
  WHERE 1=1  
 
  ";
            strSql = strSql + " AND b.F_Type=" + type; 
            if (!string.IsNullOrEmpty(fileState))
            {
                strSql = strSql + " AND b.F_FileState='" + fileState + "'";
            }
            if (!string.IsNullOrEmpty(publisher))
            {
                strSql = strSql + " AND b.F_Publisher='" + publisher + "'";
            }
            if (!string.IsNullOrEmpty(domain))
            {
                strSql = strSql + " AND b.F_Domain='" + domain + "'";
            }
            if (!string.IsNullOrEmpty(acquisitionStandard))
            {
                strSql = strSql + " AND b.F_AcquisitionStandard='" + acquisitionStandard + "'";
            }
            if (!string.IsNullOrEmpty(direction))
            {
                strSql = strSql + " AND b.F_Direction='" + direction + "'";
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                strSql = strSql + @" AND ( a.F_ChineseContent LIKE '%" + keyword + "%' OR a.F_EnglishContent LIKE '%" + keyword + "%' )";
            }
            strSql = strSql + " ORDER BY a.F_CreatorTime DESC ";
            
            
            return dbcontext.Database.SqlQuery<DocumentContentModel>(strSql.ToString()).ToList();
        }
    }
}