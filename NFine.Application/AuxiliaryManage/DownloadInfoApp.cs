
using NFine.Code;
using System.Linq;
using System.Collections.Generic;
using NFine.Repository.AuxiliaryManage;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.IRepository.AuxiliaryManage;
using NFine.Domain.ViewModel;
using NFine.Domain.Entity.ContentManage;
using NFine.Repository.ContentManage;
using System;

namespace NFine.Application.AuxiliaryManage
{
    /// <summary>
    /// DownloadInfoApp
    /// </summary>	
    public class DownloadInfoApp
    {
        private IDownloadInfoRepository service = new DownloadInfoRepository();
        private DownloadInfoRepository serviceR = new DownloadInfoRepository();
        private DocumentRepository documentservice = new DocumentRepository();

        public List<DownloadInfoEntity> GetList(Pagination pagination, string keyword)
        {
            return serviceR.FindList(pagination, keyword);
        }

        public List<DownloadInfoRankingModel> GetRankingList()
        {
            return serviceR.GetRankingList();
        }
        public List<DocumentVisitInfoStatModel> GetItemList(DateTime beginTime, DateTime endTime, int type)
        {
            return serviceR.GetItemList(beginTime, endTime, type);
        }
        public bool CheckMemberIdAndDocumentId(string memberId,string documentId)
        {
            return service.IQueryable().Where(x=>x.F_UserId== memberId && x.F_DocumentId== documentId).Any();
        }
        public List<DownloadInfoEntity> GetList()
        {
            return service.IQueryable().OrderByDescending(x => x.F_CreatorTime).ToList();
        }

        public DownloadInfoEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(DownloadInfoEntity entity)
        {
            service.Delete(entity);
        }

        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }

        public void RemoveForm(DownloadInfoEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
            }
        }
        public void SubmitForm(DownloadInfoEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
            }
            else
            {
                entity.F_DeleteMark = false;
                entity.Create();
                service.Insert(entity);
            }
        }

        public int GetDownloadCountByUserId(string userid)
        {
            return service.IQueryable().Where(x =>x.F_UserId== userid || x.F_SuperUId == userid).Count();
        }

        public List<DownloadInfoModel> GetDataList(int page, int rows, string userid , string type)
        {
            List<DocumentEntity> documentList = new List<DocumentEntity>();
            List<DownloadInfoEntity> downloadInfoList = new List<DownloadInfoEntity>(); 
            downloadInfoList = service.IQueryable().ToList();
            documentList = documentservice.IQueryable().ToList();
            var list = (from table1 in downloadInfoList
                        join document in documentList on table1.F_DocumentId equals document.F_Id 
                        where (table1.F_UserId== userid || table1.F_SuperUId == userid)
                        where table1.F_Type == type
                        where table1.F_DeleteMark == false 
                        orderby table1.F_CreatorTime descending
                        select new DownloadInfoModel()
                        {
                            F_Id = table1.F_Id,
                            F_Type = table1.F_Type,
                            F_UserId = table1.F_UserId,
                            F_SuperUId = table1.F_SuperUId,
                            F_Num = (int)table1.F_Num,
                            F_DocumentId = table1.F_DocumentId,

                            F_DocumentChineseTitle = document.F_ChineseTitle,
                            F_DocumentEnglishTitle = document.F_EnglishTitle,

                            F_CreatorUserId = table1.F_CreatorUserId,
                            F_CreatorTime = table1.F_CreatorTime,
                            F_DeleteMark = table1.F_DeleteMark,
                            F_DeleteUserId = table1.F_DeleteUserId,
                            F_DeleteTime = table1.F_DeleteTime,
                            F_LastModifyUserId = table1.F_LastModifyUserId,
                            F_LastModifyTime = table1.F_LastModifyTime,
                        }).ToList();
            list = list.Skip((page-1)*rows).Take(rows).AsQueryable().ToList();
            return list;
        }


        public List<DownloadInfoModel> GetDataList(int page, int rows, string userid, out int total)
        {
            List<DocumentEntity> documentList = new List<DocumentEntity>();
            List<DownloadInfoEntity> downloadInfoList = new List<DownloadInfoEntity>();
            downloadInfoList = service.IQueryable().ToList();
            documentList = documentservice.IQueryable().ToList();
            var list = (from table1 in downloadInfoList
                        join document in documentList on table1.F_DocumentId equals document.F_Id
                        where (table1.F_UserId == userid || table1.F_SuperUId == userid) 
                        where table1.F_DeleteMark == false
                        orderby table1.F_CreatorTime descending
                        select new DownloadInfoModel()
                        {
                            F_Id = table1.F_Id,
                            F_Type = table1.F_Type,
                            F_UserId = table1.F_UserId,
                            F_SuperUId = table1.F_SuperUId,
                            F_Num = (int)table1.F_Num,
                            F_DocumentId = table1.F_DocumentId,

                            F_DocumentChineseTitle = document.F_ChineseTitle,
                            F_DocumentEnglishTitle = document.F_EnglishTitle,

                            F_CreatorUserId = table1.F_CreatorUserId,
                            F_CreatorTime = table1.F_CreatorTime,
                            F_DeleteMark = table1.F_DeleteMark,
                            F_DeleteUserId = table1.F_DeleteUserId,
                            F_DeleteTime = table1.F_DeleteTime,
                            F_LastModifyUserId = table1.F_LastModifyUserId,
                            F_LastModifyTime = table1.F_LastModifyTime,
                        }).ToList();
            total = list.Count();
            list = list.Skip((page-1)*rows).Take(rows).AsQueryable().ToList();
            return list;
        }
    }
}