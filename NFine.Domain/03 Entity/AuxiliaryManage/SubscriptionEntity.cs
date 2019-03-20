
using System;
namespace NFine.Domain.Entity.AuxiliaryManage
{
    /// <summary>
    /// SubscriptionEntity
    /// </summary>	
    public class SubscriptionEntity : IEntity<SubscriptionEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public bool? F_IsSend { get; set; }
        public int? F_SendNum { get; set; } 
        public string F_ExcludeDocIds { get; set; }
        
        public DateTime? F_BeginTime { get; set; }
        public DateTime? F_EndTime { get; set; } 
        public string F_CreatorUserId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public bool? F_DeleteMark { get; set; }
        public string F_DeleteUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }

    }
}