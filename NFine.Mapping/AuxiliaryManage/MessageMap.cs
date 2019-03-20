using NFine.Domain.Entity.AuxiliaryManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.AuxiliaryManage
{
    public class MessageMap : EntityTypeConfiguration<MessageEntity>
    {
        public MessageMap()
        {
            this.ToTable("Auxiliary_Message");
            this.HasKey(t => t.F_Id);
        }
    }
}
