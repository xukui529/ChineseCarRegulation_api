
using NFine.Data;
using NFine.Domain.Entity.AuxiliaryManage;
namespace NFine.Domain.IRepository.AuxiliaryManage
{
    /// <summary>
    /// DocumentVisitInfoRepository
    /// </summary>	
    public interface IDocumentVisitInfoRepository : IRepositoryBase<DocumentVisitInfoEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(DocumentVisitInfoEntity entity, string keyValue);
    }
}