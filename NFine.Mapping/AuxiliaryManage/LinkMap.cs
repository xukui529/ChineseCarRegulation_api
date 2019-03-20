using NFine.Domain.Entity.AuxiliaryManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.AuxiliaryManage
{
    public class LinkMap : EntityTypeConfiguration<LinkEntity>
    {
        public LinkMap()
        {
            this.ToTable("Auxiliary_Link");
            this.HasKey(t => t.F_Id);
        }
    }
}
