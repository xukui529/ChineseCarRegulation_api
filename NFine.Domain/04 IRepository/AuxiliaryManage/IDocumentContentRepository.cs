
using NFine.Data;
using NFine.Domain.Entity.AuxiliaryManage;
namespace NFine.Domain.IRepository.AuxiliaryManage
{
    /// <summary>
    /// DocumentContentRepository
    /// </summary>	
    public interface IDocumentContentRepository : IRepositoryBase<DocumentContentEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(DocumentContentEntity entity, string keyValue);
    }
}