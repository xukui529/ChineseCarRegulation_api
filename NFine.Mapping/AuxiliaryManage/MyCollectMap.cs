
using NFine.Domain.Entity.AuxiliaryManage;
using System.Data.Entity.ModelConfiguration;
namespace NFine.Mapping.AuxiliaryManage
{
    /// <summary>
    /// MyCollectMap
    /// </summary>	
    public class MyCollectMap : EntityTypeConfiguration<MyCollectEntity>
    {
        public MyCollectMap()
        {
            this.ToTable("Auxiliary_MyCollect");
            this.HasKey(t => t.F_Id);
        }
    }
}