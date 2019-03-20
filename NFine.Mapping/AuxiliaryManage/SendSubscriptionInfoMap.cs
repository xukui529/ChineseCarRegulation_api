
using NFine.Domain.Entity.AuxiliaryManage;
using System.Data.Entity.ModelConfiguration;
namespace NFine.Mapping.AuxiliaryManage
{
    /// <summary>
    /// SendSubscriptionInfoMap
    /// </summary>	
    public class SendSubscriptionInfoMap : EntityTypeConfiguration<SendSubscriptionInfoEntity>
    {
        public SendSubscriptionInfoMap()
        {
            this.ToTable("Auxiliary_SendSubscriptionInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}