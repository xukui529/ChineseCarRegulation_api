
using NFine.Code;
using System.Linq;
using System.Collections.Generic;
using NFine.Repository.AuxiliaryManage;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.IRepository.AuxiliaryManage;
using NFine.Domain.ViewModel;
using NFine.Domain.Entity.ContentManage;
using NFine.Repository.ContentManage;

namespace NFine.Application.AuxiliaryManage
{
    /// <summary>
    /// MyCollectApp
    /// </summary>	
    public class MyCollectApp
    {
        private IMyCollectRepository service = new MyCollectRepository();
        private MyCollectRepository serviceR = new MyCollectRepository();

        private DocumentRepository documentservice = new DocumentRepository();
        public List<MyCollectEntity> GetList(Pagination pagination, string keyword)
        {
            return serviceR.FindList(pagination, keyword);
        }

        public List<MyCollectEntity> GetList()
        {
            return service.IQueryable().OrderByDescending(x => x.F_CreatorTime).ToList();
        }

        public MyCollectEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public MyCollectEntity GetForm(string userid,string documentid)
        {
            return service.IQueryable().Where(x=>x.F_UserId==userid&&x.F_DocumentId==documentid).FirstOrDefault();
        }

        public void Delete(MyCollectEntity entity)
        {
            service.Delete(entity);
        }

        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }

        public void RemoveForm(MyCollectEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
            }
        }
        public void SubmitForm(MyCollectEntity entity, string keyValue)
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
        public bool CheckIsCollect(string memberId,string documentId)
        {
            return service.IQueryable().Where(x=>x.F_UserId==memberId && x.F_DocumentId== documentId).Any();
        }

        /// <summary>
		/// 根据标题查询数据列表
		/// </summary>
		/// <param name="pagination"></param>
		/// <param name="keyword"></param>
		/// <param name="index">区别法规，标准，咨询</param>
		/// <param name="type">区别发布状态</param>
		/// <returns></returns>
		public List<MyCollectModel> GetDataList(int page, int rows, string userid,out int   total )
        {
            List<DocumentEntity> documentList = new List<DocumentEntity>();
            List<MyCollectEntity> myCollectList = new List<MyCollectEntity>();
            myCollectList = service.IQueryable().ToList(); 
            documentList = documentservice.IQueryable().ToList(); 
            var list = (from table1 in myCollectList
                        join document in documentList on table1.F_DocumentId equals document.F_Id
                        
                        where document.F_DeleteMark == false
                        where table1.F_UserId== userid
                        where table1.F_DeleteMark == false 
                        orderby table1.F_CreatorTime descending
                        select new MyCollectModel()
                        { 
                            F_Id = table1.F_Id,
                            F_UserId = table1.F_UserId,
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