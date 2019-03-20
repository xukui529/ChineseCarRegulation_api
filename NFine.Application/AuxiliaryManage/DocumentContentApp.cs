
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
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;

namespace NFine.Application.AuxiliaryManage
{
    /// <summary>
    /// DocumentContentApp
    /// </summary>	
    public class DocumentContentApp
    {
        private IDocumentContentRepository service = new DocumentContentRepository();
        private DocumentContentRepository serviceR = new DocumentContentRepository();
        private DocumentRepository documentservice = new DocumentRepository();

        public List<DocumentContentEntity> GetList(Pagination pagination, string keyword)
        {
            return serviceR.FindList(pagination, keyword);
        }

        public List<DocumentContentEntity> GetList()
        {
            return service.IQueryable().OrderByDescending(x => x.F_CreatorTime).ToList();
        }
        public DocumentContentEntity  GetFormByDocumentId(string documentId)
        {
            return service.IQueryable().Where(x=>x.F_DocumentId== documentId).FirstOrDefault();
        }

        public DocumentContentEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(DocumentContentEntity entity)
        {
            service.Delete(entity);
        }

        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }

        public void RemoveForm(DocumentContentEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
            }
        }
        public void SubmitForm(DocumentContentEntity entity, string keyValue)
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
        /// <summary>
        /// 根据标题查询数据列表
        /// </summary> 
        /// <param name="keyword"></param> 
        /// <returns></returns>
        public List<DocumentContentModel> SearchForIndexByContent(string keyword, int language)
        {
            List<DocumentContentEntity> documentContentList = new List<DocumentContentEntity>();
            List<DocumentEntity> documentList = new List<DocumentEntity>();

            documentList = documentservice.IQueryable().ToList();
            documentContentList = service.IQueryable().ToList();
            var list = (from table1 in documentContentList
                        join document in documentList on table1.F_DocumentId equals document.F_Id
                        where table1.F_DeleteMark == false
                        where document.F_State == (int)Code.Enum.DocumentStatus.已发布
                        where language==1 ? (table1.F_ChineseContent != null && table1.F_ChineseContent.Contains(keyword)): 
                        (table1.F_EnglishContent != null && table1.F_EnglishContent.Contains(keyword))
                        orderby table1.F_CreatorTime descending
                        select new DocumentContentModel()
                        {
                            F_Id = table1.F_Id,
                            F_ChineseContent = table1.F_ChineseContent,
                            F_EnglishContent = table1.F_EnglishContent,
                            F_Type = table1.F_Type,
                            F_DocumentId = document.F_Id,
                            F_ChineseTitle = document.F_ChineseTitle,
                            F_EnglishTitle = document.F_EnglishTitle,
                            F_CreatorTime =  document.F_CreatorTime,
                            F_Label = document.F_Label,
                            F_FileState = document.F_FileState,
                            F_Domain = document.F_Domain,
                            F_AcquisitionStandard = document.F_AcquisitionStandard,
                            F_Publisher = document.F_Publisher,
                            F_Direction = document.F_Direction,
                            F_ReleaseDate =  document.F_ReleaseDate,
                            F_ImplementDate =  document.F_ImplementDate,
                            F_ChineseUrl = document.F_ChineseUrl,
                            F_EnglishUrl = document.F_EnglishUrl,
                            F_HtmlUrl = document.F_HtmlUrl,
                            F_TotalUrl = document.F_TotalUrl
                        }).ToList();

            return list;
        }

        /// <summary>
        /// 根据标题查询数据列表
        /// </summary> 
        /// <param name="keyword"></param> 
        /// <returns></returns>
        public List<DocumentContentModel> SearchForIndexByContent(int type, string fileState, string domain, string acquisitionStandard
            , string publisher, string direction )
        {
            return serviceR.SearchForIndexByContent(type, fileState, domain, acquisitionStandard, publisher, direction, "");

            //List<DocumentContentEntity> documentContentList = new List<DocumentContentEntity>();
            //List<DocumentEntity> documentList = new List<DocumentEntity>();

            //documentList = documentservice.IQueryable().ToList();
            //documentContentList = service.IQueryable().ToList();
            //var list = (from table1 in documentContentList
            //            join document in documentList on table1.F_DocumentId equals document.F_Id
            //            where table1.F_DeleteMark == false


            //            where document.F_Type == type
            //            where document.F_State == (int)Code.Enum.DocumentStatus.已发布
            //            where (fileState == "" ? true : document.F_FileState == fileState)



            //            where (publisher == "" ? true : (document.F_Publisher != null && (document.F_Publisher.Contains(publisher))))
            //            where (direction == "" ? true : (document.F_Direction != null && (document.F_Direction.Contains(direction))))
            //            where (domain == "" ? true : (document.F_Domain != null && (document.F_Domain.Contains(domain))))
            //            where (acquisitionStandard == "" ? true : (document.F_AcquisitionStandard != null && (document.F_AcquisitionStandard.Contains(acquisitionStandard))))



            //            //where (publisher == "" ? true : document.F_Publisher == publisher)
            //            //where (direction == "" ? true : document.F_Direction == direction)
            //            //where (domain == "" ? true : document.F_Domain == domain)
            //            //where (acquisitionStandard == "" ? true : document.F_AcquisitionStandard == acquisitionStandard)


            //            orderby table1.F_CreatorTime descending
            //            select new DocumentContentModel()
            //            {
            //                F_Id = table1.F_Id,
            //                F_ChineseContent = table1.F_ChineseContent,
            //                F_EnglishContent = table1.F_EnglishContent,
            //                F_Type = table1.F_Type,
            //                F_DocumentId = document.F_Id,
            //                F_ChineseTitle = document.F_ChineseTitle,
            //                F_EnglishTitle = document.F_EnglishTitle,
            //                F_CreatorTime = document.F_CreatorTime,
            //                F_Label = document.F_Label,
            //                F_FileState = document.F_FileState,
            //                F_Domain = document.F_Domain,
            //                F_AcquisitionStandard = document.F_AcquisitionStandard,
            //                F_Publisher = document.F_Publisher,
            //                F_Direction = document.F_Direction,
            //                F_ReleaseDate = document.F_ReleaseDate,
            //                F_ImplementDate = document.F_ImplementDate,
            //                 F_ChineseUrl= document.F_ChineseUrl,
            //                 F_EnglishUrl = document.F_EnglishUrl,
            //                 F_HtmlUrl = document.F_HtmlUrl,
            //                 F_TotalUrl = document.F_TotalUrl
            //            }).ToList();

            //return list;
        }


        
        public List<DocumentContentModel> SearchForIndexByContent(int type, string fileState, string domain, string acquisitionStandard
            , string publisher, string direction, string keyword)
        {


            return serviceR.SearchForIndexByContent(type, fileState, domain, acquisitionStandard, publisher, direction, keyword);
            //list = documentContentApp.SearchForIndexByContent(type, fileState, domain, acquisitionStandard, publisher, direction, keyword);

            //List<DocumentContentEntity> documentContentList = new List<DocumentContentEntity>();
            //List<DocumentEntity> documentList = new List<DocumentEntity>();

            
            //documentList = documentservice.IQueryable().ToList();
            //documentContentList = service.IQueryable().ToList();

            ////var templist = (

            ////    //from  table1 in    service.IQueryable()
            ////    //            join document in documentList on table1.F_DocumentId equals document.F_Id

            ////    from document in documentList 
            ////    join table1 in service.IQueryable() on document.F_Id  equals table1.F_DocumentId


            ////    where (document.F_Type == type)
            ////                where (document.F_State == (int)Code.Enum.DocumentStatus.已发布)
            ////                where (fileState == "" ? true : document.F_FileState == fileState)


            ////                where (publisher == "" ? true : (document.F_Publisher != null && (document.F_Publisher.Contains(publisher))))
            ////                where (direction == "" ? true : (document.F_Direction != null && (document.F_Direction.Contains(direction))))
            ////                where (domain == "" ? true : (document.F_Domain != null && (document.F_Domain.Contains(domain))))
            ////                where (acquisitionStandard == "" ? true : (document.F_AcquisitionStandard != null && (document.F_AcquisitionStandard.Contains(acquisitionStandard))))


            ////                //where (publisher == "" ? true : document.F_Publisher == publisher)
            ////                //where (direction == "" ? true : document.F_Direction == direction)
            ////                //where (domain == "" ? true : document.F_Domain == domain)
            ////                //where (acquisitionStandard == "" ? true : document.F_AcquisitionStandard == acquisitionStandard)

            ////                where (table1.F_ChineseContent != null && table1.F_ChineseContent.Contains(keyword)) ||
            ////                (table1.F_EnglishContent != null && table1.F_EnglishContent.Contains(keyword))
            ////                where (table1.F_DeleteMark == false)


            ////                orderby table1.F_CreatorTime descending
            ////                select new DocumentContentModel()
            ////                {
            ////                    F_Id = table1.F_Id,
            ////                    F_ChineseContent = table1.F_ChineseContent,
            ////                    F_EnglishContent = table1.F_EnglishContent,
            ////                    F_Type = table1.F_Type,
            ////                    F_DocumentId = document.F_Id,
            ////                    F_ChineseTitle = document.F_ChineseTitle,
            ////                    F_EnglishTitle = document.F_EnglishTitle,
            ////                    F_CreatorTime = document.F_CreatorTime,
            ////                    F_Label = document.F_Label,
            ////                    F_FileState = document.F_FileState,
            ////                    F_Domain = document.F_Domain,
            ////                    F_AcquisitionStandard = document.F_AcquisitionStandard,
            ////                    F_Publisher = document.F_Publisher,
            ////                    F_Direction = document.F_Direction,
            ////                    F_ReleaseDate = document.F_ReleaseDate,
            ////                    F_ImplementDate = document.F_ImplementDate,
            ////                    F_ChineseUrl = document.F_ChineseUrl,
            ////                    F_EnglishUrl = document.F_EnglishUrl,
            ////                    F_HtmlUrl = document.F_HtmlUrl,
            ////                    F_TotalUrl = document.F_TotalUrl
            ////                }
            ////                ).ToList();

            //var list = (from table1 in documentContentList
            //            join document in documentList on table1.F_DocumentId equals document.F_Id


            //            where document.F_Type == type
            //            where document.F_State == (int)Code.Enum.DocumentStatus.已发布
            //            where (fileState == "" ? true : document.F_FileState == fileState)


            //            where (publisher == "" ? true : (document.F_Publisher != null && (document.F_Publisher.Contains(publisher))))
            //            where (direction == "" ? true : (document.F_Direction != null && (document.F_Direction.Contains(direction))))
            //            where (domain == "" ? true : (document.F_Domain != null && (document.F_Domain.Contains(domain))))
            //            where (acquisitionStandard == "" ? true : (document.F_AcquisitionStandard != null && (document.F_AcquisitionStandard.Contains(acquisitionStandard))))





            //            //where (publisher == "" ? true : document.F_Publisher == publisher)
            //            //where (direction == "" ? true : document.F_Direction == direction)
            //            //where (domain == "" ? true : document.F_Domain == domain)
            //            //where (acquisitionStandard == "" ? true : document.F_AcquisitionStandard == acquisitionStandard)

            //            where (table1.F_ChineseContent != null && table1.F_ChineseContent.Contains(keyword)) ||
            //            (table1.F_EnglishContent != null && table1.F_EnglishContent.Contains(keyword))
            //            where table1.F_DeleteMark == false


            //            orderby table1.F_CreatorTime descending
            //            select new DocumentContentModel()
            //            {
            //                F_Id = table1.F_Id,
            //                F_ChineseContent = table1.F_ChineseContent,
            //                F_EnglishContent = table1.F_EnglishContent,
            //                F_Type = table1.F_Type,
            //                F_DocumentId = document.F_Id,
            //                F_ChineseTitle = document.F_ChineseTitle,
            //                F_EnglishTitle = document.F_EnglishTitle,
            //                F_CreatorTime = document.F_CreatorTime,
            //                F_Label = document.F_Label,
            //                F_FileState = document.F_FileState,
            //                F_Domain = document.F_Domain,
            //                F_AcquisitionStandard = document.F_AcquisitionStandard,
            //                F_Publisher = document.F_Publisher,
            //                F_Direction = document.F_Direction,
            //                F_ReleaseDate = document.F_ReleaseDate,
            //                F_ImplementDate = document.F_ImplementDate,
            //                F_ChineseUrl = document.F_ChineseUrl,
            //                F_EnglishUrl = document.F_EnglishUrl,
            //                F_HtmlUrl = document.F_HtmlUrl,
            //                F_TotalUrl = document.F_TotalUrl
            //            }).ToList();

            //return list;




        }

        /// <summary>
        /// 根据标题查询数据列表
        /// </summary> 
        /// <param name="keyword"></param> 
        /// <returns></returns>
        public List<DocumentContentModel> SearchForIndexByContent()
        {
            List<DocumentContentEntity> documentContentList = new List<DocumentContentEntity>();
            List<DocumentEntity> documentList = new List<DocumentEntity>();

            documentList = documentservice.IQueryable().ToList();
            documentContentList = service.IQueryable().ToList();
            var list = (from table1 in documentContentList
                        join document in documentList on table1.F_DocumentId equals document.F_Id
                        where table1.F_DeleteMark == false
                        where document.F_State == (int)Code.Enum.DocumentStatus.已发布
                        orderby table1.F_CreatorTime descending
                        select new DocumentContentModel()
                        {
                            F_Id = table1.F_Id,
                            F_ChineseContent = table1.F_ChineseContent,
                            F_EnglishContent = table1.F_EnglishContent,
                            F_Type = table1.F_Type,
                            F_DocumentId = document.F_Id,
                            F_ChineseTitle = document.F_ChineseTitle,
                            F_EnglishTitle = document.F_EnglishTitle,
                            F_CreatorTime = document.F_CreatorTime,
                            F_Label = document.F_Label,
                            F_FileState = document.F_FileState,
                            F_Domain = document.F_Domain,
                            F_AcquisitionStandard = document.F_AcquisitionStandard,
                            F_Publisher = document.F_Publisher,
                            F_Direction = document.F_Direction,
                            F_ReleaseDate = document.F_ReleaseDate,
                            F_ImplementDate = document.F_ImplementDate,
                            F_ChineseUrl = document.F_ChineseUrl,
                            F_EnglishUrl = document.F_EnglishUrl,
                            F_HtmlUrl = document.F_HtmlUrl,
                            F_TotalUrl = document.F_TotalUrl
                        }).ToList();

            return list;
        }
        /// <summary>
        ///  
        /// </summary> 
        /// <param name="documentId"></param> 
        /// <returns></returns>
        public DocumentContentModel  GetByDocumentId(string documentId)
        {
            List<DocumentContentEntity> documentContentList = new List<DocumentContentEntity>();
            List<DocumentEntity> documentList = new List<DocumentEntity>();

            documentList = documentservice.IQueryable().ToList();
            documentContentList = service.IQueryable().ToList();
            var entity = (from table1 in documentContentList
                        join document in documentList on table1.F_DocumentId equals document.F_Id
                        where table1.F_DocumentId== documentId
                        where table1.F_DeleteMark == false
                        orderby table1.F_CreatorTime descending
                        select new DocumentContentModel()
                        {
                            F_Id = table1.F_Id,
                            F_ChineseContent = table1.F_ChineseContent,
                            F_EnglishContent = table1.F_EnglishContent,
                            F_Type = table1.F_Type,
                            F_DocumentId = document.F_Id,
                            F_ChineseTitle = document.F_ChineseTitle,
                            F_EnglishTitle = document.F_EnglishTitle,
                            F_CreatorTime =  document.F_CreatorTime,
                            F_Label = document.F_Label,
                            F_FileState = document.F_FileState,
                            F_Domain = document.F_Domain,
                            F_AcquisitionStandard = document.F_AcquisitionStandard,
                            F_Publisher = document.F_Publisher,
                            F_Direction = document.F_Direction,
                            F_ReleaseDate =  document.F_ReleaseDate,
                            F_ImplementDate = document.F_ImplementDate,
                            F_ChineseUrl = document.F_ChineseUrl,
                            F_EnglishUrl = document.F_EnglishUrl,
                            F_HtmlUrl = document.F_HtmlUrl,
                            F_TotalUrl = document.F_TotalUrl
                        }).FirstOrDefault();

            return entity;
        }
         
    }
}