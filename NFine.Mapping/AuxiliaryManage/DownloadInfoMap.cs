
using NFine.Domain.Entity.AuxiliaryManage;
using System.Data.Entity.ModelConfiguration;
namespace NFine.Mapping.AuxiliaryManage
{
    /// <summary>
    /// DownloadInfoMap
    /// </summary>	
    public class DownloadInfoMap : EntityTypeConfiguration<DownloadInfoEntity>
    {
        public DownloadInfoMap()
        {
            this.ToTable("Auxiliary_DownloadInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}