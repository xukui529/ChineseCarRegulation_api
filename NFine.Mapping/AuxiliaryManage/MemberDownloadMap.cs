using NFine.Domain.Entity.AuxiliaryManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.AuxiliaryManage
{
    public class MemberDownloadMap : EntityTypeConfiguration<MemberDownloadEntity>
    {
        public MemberDownloadMap()
        {
            this.ToTable("Auxiliary_MemberDownload");
            this.HasKey(t => t.F_Id);
        }
    }
}
