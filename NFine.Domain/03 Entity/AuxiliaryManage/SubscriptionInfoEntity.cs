
using System;
namespace NFine.Domain.Entity.AuxiliaryManage
{
    /// <summary>
    /// SubscriptionInfoEntity
    /// </summary>	
    public class SubscriptionInfoEntity : IEntity<SubscriptionInfoEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_SubscriptionId { get; set; }
        public int? F_SendNum { get; set; }
        public string F_F_DocumentId { get; set; }
        public bool? F_IsExclude { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public bool? F_DeleteMark { get; set; }
        public string F_DeleteUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }

    }
}