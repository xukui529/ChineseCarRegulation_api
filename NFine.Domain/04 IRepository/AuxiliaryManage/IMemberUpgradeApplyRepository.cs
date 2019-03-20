
using NFine.Data;
using NFine.Domain.Entity.AuxiliaryManage;
namespace NFine.Domain.IRepository.AuxiliaryManage
{
    /// <summary>
    /// MemberUpgradeApplyRepository
    /// </summary>	
    public interface IMemberUpgradeApplyRepository : IRepositoryBase<MemberUpgradeApplyEntity>
    {
        void UpdateState(string keyValue, int status);
        void DeleteForm(string keyValue);
        void SubmitForm(MemberUpgradeApplyEntity entity, string keyValue);
    }
}