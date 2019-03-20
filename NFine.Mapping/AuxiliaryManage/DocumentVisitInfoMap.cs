
using NFine.Domain.Entity.AuxiliaryManage;
using System.Data.Entity.ModelConfiguration;
namespace NFine.Mapping.AuxiliaryManage
{
    /// <summary>
    /// DocumentVisitInfoMap
    /// </summary>	
    public class DocumentVisitInfoMap : EntityTypeConfiguration<DocumentVisitInfoEntity>
    {
        public DocumentVisitInfoMap()
        {
            this.ToTable("Auxiliary_DocumentVisitInfo");
            this.HasKey(t => t.F_Id);
        }
    }
}