
using NFine.Data;
using NFine.Domain.Entity.AuxiliaryManage;
namespace NFine.Domain.IRepository.AuxiliaryManage
{
    /// <summary>
    /// SysMessageInfoRepository
    /// </summary>	
    public interface ISysMessageInfoRepository : IRepositoryBase<SysMessageInfoEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(SysMessageInfoEntity entity, string keyValue);
    }
}