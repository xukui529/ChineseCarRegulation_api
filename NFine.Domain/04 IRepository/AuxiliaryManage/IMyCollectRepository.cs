
using NFine.Data;
using NFine.Domain.Entity.AuxiliaryManage;
namespace NFine.Domain.IRepository.AuxiliaryManage
{
    /// <summary>
    /// MyCollectRepository
    /// </summary>	
    public interface IMyCollectRepository : IRepositoryBase<MyCollectEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(MyCollectEntity entity, string keyValue);
    }
}