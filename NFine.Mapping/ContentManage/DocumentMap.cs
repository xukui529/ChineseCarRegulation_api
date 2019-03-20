using NFine.Domain.Entity.ContentManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.ContentManage
{
	public class DocumentMap : EntityTypeConfiguration<DocumentEntity>
	{
		public DocumentMap()
		{
			this.ToTable("Auxiliary_Document");
			this.HasKey(t => t.F_Id);
		}
	}
}
