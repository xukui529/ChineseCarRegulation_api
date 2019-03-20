
using System;
namespace NFine.Domain.Entity.AuxiliaryManage
{
    /// <summary>
    /// SendSubscriptionInfoEntity
    /// </summary>	
    public class SendSubscriptionInfoEntity : IEntity<SendSubscriptionInfoEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public DateTime? F_LastTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public bool? F_DeleteMark { get; set; }
        public string F_DeleteUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }

    }
}