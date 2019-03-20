
using NFine.Code;
using System.Linq;
using System.Collections.Generic;
using NFine.Repository.AuxiliaryManage;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.IRepository.AuxiliaryManage;
namespace NFine.Application.AuxiliaryManage
{
    /// <summary>
    /// SendSubscriptionInfoApp
    /// </summary>	
    public class SendSubscriptionInfoApp
    {
        private ISendSubscriptionInfoRepository service = new SendSubscriptionInfoRepository();
        private SendSubscriptionInfoRepository serviceR = new SendSubscriptionInfoRepository();

        public List<SendSubscriptionInfoEntity> GetList(Pagination pagination, string keyword)
        {
            return serviceR.FindList(pagination, keyword);
        }

        public List<SendSubscriptionInfoEntity> GetList()
        {
            return service.IQueryable().OrderByDescending(x => x.F_CreatorTime).ToList();
        }

        public  SendSubscriptionInfoEntity GetForm()
        {
            return service.IQueryable().FirstOrDefault();
        }
        public SendSubscriptionInfoEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }

        public void Delete(SendSubscriptionInfoEntity entity)
        {
            service.Delete(entity);
        }

        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }

        public void RemoveForm(SendSubscriptionInfoEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
            }
        }
        public void SubmitForm(SendSubscriptionInfoEntity entity, string keyValue)
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