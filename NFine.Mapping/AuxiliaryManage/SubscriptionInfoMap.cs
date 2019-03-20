
using NFine.Domain.Entity.AuxiliaryManage;
using System.Data.Entity.ModelConfiguration;
namespace NFine.Mapping.AuxiliaryManage
{
    /// <summary>
    /// SubscriptionInfoMap
    /// </summary>	
    public class SubscriptionInfoMap : EntityTypeConfiguration<SubscriptionInfoEntity>
    {
        public SubscriptionInfoMap()
        {
            this.ToTable("Auxiliary_SubscriptionInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}