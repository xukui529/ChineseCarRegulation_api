﻿
using System;
namespace NFine.Domain.Entity.AuxiliaryManage
{
    /// <summary>
    /// DocumentVisitInfoEntity
    /// </summary>	
    public class DocumentVisitInfoEntity : IEntity<DocumentVisitInfoEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_UserId { get; set; }
        public string F_DocumentId { get; set; }

        public int? F_Type { get; set; }
        
        public string F_CreatorUserId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public bool? F_DeleteMark { get; set; }
        public string F_DeleteUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }

    }
}