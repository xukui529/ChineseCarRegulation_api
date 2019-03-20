
using NFine.Domain.Entity.AuxiliaryManage;
using System.Data.Entity.ModelConfiguration;
namespace NFine.Mapping.AuxiliaryManage
{
    /// <summary>
    /// SysMessageInfoMap
    /// </summary>	
    public class SysMessageInfoMap : EntityTypeConfiguration<SysMessageInfoEntity>
    {
        public SysMessageInfoMap()
        {
            this.ToTable("Auxiliary_SysMessageInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}