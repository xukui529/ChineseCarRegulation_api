
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
    /// ReleaseInfoApp
    /// </summary>	
    public class ReleaseInfoApp
    {
        private IReleaseInfoRepository service = new ReleaseInfoRepository();
        private ReleaseInfoRepository serviceR = new ReleaseInfoRepository();

        public List<ReleaseInfoEntity> GetList(Pagination pagination, string keyword)
        {
            return serviceR.FindList(pagination, keyword);
        }
        public List<ReleaseInfoStatModel> GetItemList(DateTime beginTime, DateTime endTime, int type)
        {
            return serviceR.GetItemList(beginTime, endTime, type);
        }
        public List<ReleaseInfoEntity> GetList()
        {
            return service.IQueryable().OrderByDescending(x => x.F_CreatorTime).ToList();
        }

        public ReleaseInfoEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(ReleaseInfoEntity entity)
        {
            service.Delete(entity);
        }

        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }

        public void RemoveForm(ReleaseInfoEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
            }
        }
        public void SubmitForm(ReleaseInfoEntity entity, string keyValue)
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