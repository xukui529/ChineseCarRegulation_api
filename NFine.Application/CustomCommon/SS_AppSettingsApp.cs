using NFine.Domain.IRepository.Common;
using NFine.Repository.CustomCommon;

namespace NFine.Application.CustomCommon
{
    public class SS_AppSettingsApp
    {
        private ISS_AppSettingsRepository service = new SS_AppSettingsRepository();

        public string GetValue(string name)
        {
            var ss = service.FindEntityByName(name);
            return ss != null ? ss.Value : "";
        }
    }
}
