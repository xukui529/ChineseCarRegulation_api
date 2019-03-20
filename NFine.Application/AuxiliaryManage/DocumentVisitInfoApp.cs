
using NFine.Code;
using System.Linq;
using System.Collections.Generic;
using NFine.Repository.AuxiliaryManage;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.IRepository.AuxiliaryManage;
using NFine.Domain.ViewModel;
using System;

namespace NFine.Application.AuxiliaryManage
{
    /// <summary>
    /// DocumentVisitInfoApp
    /// </summary>	
    public class DocumentVisitInfoApp
    {
        private IDocumentVisitInfoRepository service = new DocumentVisitInfoRepository();
        private DocumentVisitInfoRepository serviceR = new DocumentVisitInfoRepository();

        public List<DocumentSearchKeysStatModel> GetStat()
        {
            return serviceR.GetStat();
        }
        public List<DocumentVisitInfoEntity> GetList(Pagination pagination, string keyword)
        {
            return serviceR.FindList(pagination, keyword);
        }
        public List<DocumentVisitInfoStatModel> GetItemList(DateTime beginTime, DateTime endTime, int type)
        {
            return serviceR.GetItemList(beginTime, endTime, type);
        }
        public List<DocumentVisitInfoEntity> GetList()
        {
            return service.IQueryable().OrderByDescending(x => x.F_CreatorTime).ToList();
        }

        public DocumentVisitInfoEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(DocumentVisitInfoEntity entity)
        {
            service.Delete(entity);
        }
        
        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }

        public void RemoveForm(DocumentVisitInfoEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
            }
        }
        public void SubmitForm(DocumentVisitInfoEntity entity, string keyValue)
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

    }
}