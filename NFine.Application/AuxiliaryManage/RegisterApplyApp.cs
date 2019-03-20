
using NFine.Code;
using System.Linq;
using System.Collections.Generic;
using NFine.Repository.AuxiliaryManage;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.IRepository.AuxiliaryManage;
namespace NFine.Application.AuxiliaryManage
{
    /// <summary>
    /// RegisterApplyApp
    /// </summary>	
    public class RegisterApplyApp
    {
        private IRegisterApplyRepository service = new RegisterApplyRepository();
        private RegisterApplyRepository serviceR = new RegisterApplyRepository();

        public List<RegisterApplyEntity> GetList(Pagination pagination, string keyword)
        {
            return serviceR.FindList(pagination, keyword);
        }
        public bool CheckUserByEmail(string email)
        {
            return service.IQueryable().Any(x => x.F_Email == email);
        }
        public List<RegisterApplyEntity> GetList()
        {
            return service.IQueryable().OrderByDescending(x => x.F_CreatorTime).ToList();
        }

        public RegisterApplyEntity GetFormByEmail(string email)
        {
            return service.IQueryable().Where(x => x.F_Email== email).FirstOrDefault();
        }

        public RegisterApplyEntity GetForm(string keyValue)
        {
            return service.FindEntity(keyValue);
        }
        public void Delete(RegisterApplyEntity entity)
        {
            service.Delete(entity);
        }

        public void DeleteForm(string keyValue)
        {
            service.DeleteForm(keyValue);
        }

        public void RemoveForm(RegisterApplyEntity entity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                entity.Modify(keyValue);
                service.Update(entity);
            }
        }
        public void SubmitForm(RegisterApplyEntity entity, string keyValue)
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