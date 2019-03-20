
using NFine.Data;
using NFine.Domain.Entity.AuxiliaryManage;
namespace NFine.Domain.IRepository.AuxiliaryManage
{
    /// <summary>
    /// SubscriptionRepository
    /// </summary>	
    public interface ISubscriptionRepository : IRepositoryBase<SubscriptionEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(SubscriptionEntity entity, string keyValue);
    }
}