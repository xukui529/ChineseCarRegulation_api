
using NFine.Data;
using NFine.Domain.Entity.AuxiliaryManage;
namespace NFine.Domain.IRepository.AuxiliaryManage
{
    /// <summary>
    /// DownloadInfoRepository
    /// </summary>	
    public interface IDownloadInfoRepository : IRepositoryBase<DownloadInfoEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(DownloadInfoEntity entity, string keyValue);
    }
}