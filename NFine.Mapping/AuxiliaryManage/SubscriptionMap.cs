
using NFine.Domain.Entity.AuxiliaryManage;
using System.Data.Entity.ModelConfiguration;
namespace NFine.Mapping.AuxiliaryManage
{
    /// <summary>
    /// SubscriptionMap
    /// </summary>	
    public class SubscriptionMap : EntityTypeConfiguration<SubscriptionEntity>
    {
        public SubscriptionMap()
        {
            this.ToTable("Auxiliary_Subscription");
            this.HasKey(t => t.F_Id);
        }
    }
}