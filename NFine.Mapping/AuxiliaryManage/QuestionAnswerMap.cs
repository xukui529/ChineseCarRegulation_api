using NFine.Domain.Entity.AuxiliaryManage;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Mapping.AuxiliaryManage
{
    public class QuestionAnswerMap : EntityTypeConfiguration<QuestionAnswerEntity>
    {
        public QuestionAnswerMap()
        {
            this.ToTable("Auxiliary_QuestionAnswer");
            this.HasKey(t => t.F_Id);
        }
    }
}
