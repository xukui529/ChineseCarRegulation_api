using NFine.Domain.Entity.ContentManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.ViewModel
{
    public class DownloadInfoModel
    {
        public string F_Id { get; set; }
        public string F_Type { get; set; }
        public string F_UserId { get; set; }
        public string F_SuperUId { get; set; }
        public int F_Num { get; set; }
        public string F_DocumentId { get; set; }
        public string num { get; set; }
        /// <summary>
        /// 中文标题
        /// </summary>
        public string F_DocumentChineseTitle { get; set; }
        /// <summary>
        /// 英文标题
        /// </summary>
        public string F_DocumentEnglishTitle { get; set; }


        public string F_CreatorUserId { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public bool? F_DeleteMark { get; set; }
        public string F_DeleteUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
    }
}
