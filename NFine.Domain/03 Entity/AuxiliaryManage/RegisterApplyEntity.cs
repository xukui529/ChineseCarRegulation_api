
using System;
namespace NFine.Domain.Entity.AuxiliaryManage
{
    /// <summary>
    /// RegisterApplyEntity
    /// </summary>	
    public class RegisterApplyEntity : IEntity<RegisterApplyEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; } 
        public string F_Password { get; set; }
        public string F_Email { get; set; }
        public bool? F_IsSubscription { get; set; }
        public int? F_SubscriptionLanguage { get; set; }
        public string F_SecretKey { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public bool? F_DeleteMark { get; set; }
        public string F_DeleteUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }

    }
}