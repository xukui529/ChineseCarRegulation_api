
using NFine.Domain.Entity.AuxiliaryManage;
using System.Data.Entity.ModelConfiguration;
namespace NFine.Mapping.AuxiliaryManage
{
    /// <summary>
    /// RegisterApplyMap
    /// </summary>	
    public class RegisterApplyMap : EntityTypeConfiguration<RegisterApplyEntity>
    {
        public RegisterApplyMap()
        {
            this.ToTable("Auxiliary_RegisterApply");
            this.HasKey(t => t.F_Id);
        }
    }
}