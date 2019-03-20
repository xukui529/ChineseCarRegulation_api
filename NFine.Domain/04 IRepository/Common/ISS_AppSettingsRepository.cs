using NFine.Data;
using NFine.Domain.Entity.CustomCommon;

namespace NFine.Domain.IRepository.Common
{
    public interface ISS_AppSettingsRepository : IRepositoryBase<SS_AppSettingsEntity>
    {
        SS_AppSettingsEntity FindEntityByName(string name);
    }
}
