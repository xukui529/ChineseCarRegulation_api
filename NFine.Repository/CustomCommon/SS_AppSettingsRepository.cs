using NFine.Data;
using NFine.Domain.IRepository.Common;
using NFine.Domain.Entity.CustomCommon;

namespace NFine.Repository.CustomCommon
{
    public class SS_AppSettingsRepository : RepositoryBase<SS_AppSettingsEntity>, ISS_AppSettingsRepository
    {
        public SS_AppSettingsEntity FindEntityByName(string name)
        {
            using (var db = new RepositoryBase())
            {
                return db.FindEntity<SS_AppSettingsEntity>(t => t.Name == name);
            }
        }
    }
}
