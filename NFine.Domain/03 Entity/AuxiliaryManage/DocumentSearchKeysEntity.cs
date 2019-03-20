﻿
using System;
namespace NFine.Domain.Entity.AuxiliaryManage
{
    /// <summary>
    /// DocumentSearchKeysEntity
    /// </summary>	
    public class DocumentSearchKeysEntity : IEntity<DocumentSearchKeysEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        public string F_Keys { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public bool? F_DeleteMark { get; set; }
        public string F_DeleteUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }

    }
}