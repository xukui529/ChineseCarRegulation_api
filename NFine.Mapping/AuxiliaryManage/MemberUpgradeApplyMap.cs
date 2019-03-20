
using NFine.Domain.Entity.AuxiliaryManage;
using System.Data.Entity.ModelConfiguration;
namespace NFine.Mapping.AuxiliaryManage
{
    /// <summary>
    /// MemberUpgradeApplyMap
    /// </summary>	
    public class MemberUpgradeApplyMap : EntityTypeConfiguration<MemberUpgradeApplyEntity>
    {
        public MemberUpgradeApplyMap()
        {
            this.ToTable("Auxiliary_MemberUpgradeApply");
            this.HasKey(t => t.F_Id);
        }
    }
}