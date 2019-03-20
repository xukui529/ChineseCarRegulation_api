
using NFine.Data;
using NFine.Domain.Entity.AuxiliaryManage;
namespace NFine.Domain.IRepository.AuxiliaryManage
{
    /// <summary>
    /// SubscriptionInfoRepository
    /// </summary>	
    public interface ISubscriptionInfoRepository : IRepositoryBase<SubscriptionInfoEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(SubscriptionInfoEntity entity, string keyValue);
    }
}