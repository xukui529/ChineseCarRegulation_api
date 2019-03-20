
using System;
namespace NFine.Domain.Entity.AuxiliaryManage
{
    /// <summary>
    /// SysMessageInfoEntity
    /// </summary>	
    public class SysMessageInfoEntity : IEntity<SysMessageInfoEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_UserId { get; set; }
        public string F_EnTitle { get; set; }
        public string F_ChTitle { get; set; } 
        public string F_CreatorUserId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public bool? F_DeleteMark { get; set; }
        public string F_DeleteUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }

    }
}