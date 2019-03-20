
using NFine.Code;
using System.Linq;
using System.Collections.Generic;
using NFine.Repository.AuxiliaryManage;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.IRepository.AuxiliaryManage;
using NFine.Domain.ViewModel;

namespace NFine.Application.AuxiliaryManage
{
    /// <summary>
    /// DocumentSearchKeysApp
    /// </summary>	
    public class DocumentSearchKeysApp
    {
        private IDocumentSearchKeysRepository service = new DocumentSearchKeysRepository();
        private DocumentSearchKeysRepository serviceR = new DocumentSearchKeysRepository();

        public List<DocumentSearchKeysEntity> GetList(Pagination pagination, string keyword)
        {
            return serviceR.FindList(pagination, keyword);
        }

        public List<DocumentSearchKeysEntity> GetList()
        {
            return service.IQueryable().OrderByDescending(x => x.F_CreatorTime).ToList();
        }

        public DocumentSearchKeysEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(DocumentSearchKeysEntity entity)
        {
            service.Delete(entity);
        }

        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }

        public void RemoveForm(DocumentSearchKeysEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
            }
        }
        public void SubmitForm(DocumentSearchKeysEntity entity, string keyValue)
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

        public List<DocumentSearchKeysStatModel> GetStat()
        {
            return serviceR.GetStat( );
        }
    }
}