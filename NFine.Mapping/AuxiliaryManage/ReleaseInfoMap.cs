
using NFine.Domain.Entity.AuxiliaryManage;
using System.Data.Entity.ModelConfiguration;
namespace NFine.Mapping.AuxiliaryManage
{
    /// <summary>
    /// ReleaseInfoMap
    /// </summary>	
    public class ReleaseInfoMap : EntityTypeConfiguration<ReleaseInfoEntity>
    {
        public ReleaseInfoMap()
        {
            this.ToTable("Auxiliary_ReleaseInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}