
using NFine.Data;
using NFine.Domain.Entity.AuxiliaryManage;
namespace NFine.Domain.IRepository.AuxiliaryManage
{
    /// <summary>
    /// SendSubscriptionInfoRepository
    /// </summary>	
    public interface ISendSubscriptionInfoRepository : IRepositoryBase<SendSubscriptionInfoEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(SendSubscriptionInfoEntity entity, string keyValue);
    }
}