using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.AuxiliaryManage
{
    public class MemberDownloadEntity : IEntity<MemberDownloadEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; } 
        public int F_OldSeasonNumber { get; set; }
        public int F_OldYearNumber { get; set; }
        public int F_SeasonNumber { get; set; }
        public int F_YearNumber { get; set; }

        public DateTime? F_EffectiveDate { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public bool? F_DeleteMark { get; set; }
        public string F_DeleteUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
    }
}
