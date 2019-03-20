
using NFine.Domain.Entity.AuxiliaryManage;
using System.Data.Entity.ModelConfiguration;
namespace NFine.Mapping.AuxiliaryManage
{
    /// <summary>
    /// DocumentContentMap
    /// </summary>	
    public class DocumentContentMap : EntityTypeConfiguration<DocumentContentEntity>
    {
        public DocumentContentMap()
        {
            this.ToTable("Auxiliary_DocumentContent");
            this.HasKey(t => t.F_Id);
        }
    }
}