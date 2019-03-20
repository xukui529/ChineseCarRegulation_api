
using NFine.Data;
using NFine.Domain.Entity.AuxiliaryManage;
namespace NFine.Domain.IRepository.AuxiliaryManage
{
    /// <summary>
    /// DocumentSearchKeysRepository
    /// </summary>	
    public interface IDocumentSearchKeysRepository : IRepositoryBase<DocumentSearchKeysEntity>
    {
        void DeleteForm(string keyValue);
        void SubmitForm(DocumentSearchKeysEntity entity, string keyValue);
    }
}