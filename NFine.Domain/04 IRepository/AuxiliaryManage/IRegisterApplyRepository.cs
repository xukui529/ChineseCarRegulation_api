
using NFine.Data;
using NFine.Domain.Entity.AuxiliaryManage;
namespace NFine.Domain.IRepository.AuxiliaryManage
{
    /// <summary>
    /// RegisterApplyRepository
    /// </summary>	
    public interface IRegisterApplyRepository : IRepositoryBase<RegisterApplyEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(RegisterApplyEntity entity, string keyValue);
    }
}