
using NFine.Data;
using NFine.Domain.Entity.AuxiliaryManage;
namespace NFine.Domain.IRepository.AuxiliaryManage
{
    /// <summary>
    /// ReleaseInfoRepository
    /// </summary>	
    public interface IReleaseInfoRepository : IRepositoryBase<ReleaseInfoEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(ReleaseInfoEntity entity, string keyValue);
    }
}