
using System;
namespace NFine.Domain.Entity.AuxiliaryManage
{
    /// <summary>
    /// DownloadInfoEntity
    /// </summary>	
    public class DownloadInfoEntity : IEntity<DownloadInfoEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_Type { get; set; }
        public string F_UserId { get; set; }
        public string F_SuperUId { get; set; }
        public int? F_Num { get; set; }
        public string F_DocumentId { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public bool? F_DeleteMark { get; set; }
        public string F_DeleteUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }

    }
}