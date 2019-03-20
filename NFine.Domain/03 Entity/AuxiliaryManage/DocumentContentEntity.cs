
using System;
namespace NFine.Domain.Entity.AuxiliaryManage
{
    /// <summary>
    /// DocumentContentEntity
    /// </summary>	
    public class DocumentContentEntity : IEntity<DocumentContentEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public int? F_Type { get; set; }
        public string F_DocumentId { get; set; }
        public string F_ChineseContent { get; set; }
        public string F_EnglishContent { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public bool? F_DeleteMark { get; set; }
        public string F_DeleteUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }

    }
}