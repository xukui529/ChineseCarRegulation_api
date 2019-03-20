
using NFine.Domain.Entity.AuxiliaryManage;
using System.Data.Entity.ModelConfiguration;
namespace NFine.Mapping.AuxiliaryManage
{
    /// <summary>
    /// DocumentSearchKeysMap
    /// </summary>	
    public class DocumentSearchKeysMap : EntityTypeConfiguration<DocumentSearchKeysEntity>
    {
        public DocumentSearchKeysMap()
        {
            this.ToTable("Auxiliary_DocumentSearchKeys");
            this.HasKey(t => t.F_Id);
        }
    }
}