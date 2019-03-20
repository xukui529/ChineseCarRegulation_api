using NFine.Domain.Entity.ContentManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.ViewModel
{
    public class DocumentContentModel
    {
        public string F_Id { get; set; }
        public int? F_Type { get; set; }
        public string F_DocumentId { get; set; }
        public string F_ChineseContent { get; set; }
        public string F_EnglishContent { get; set; }
        public string F_Label { get; set; }

        /// <summary>
        /// 英文标题
        /// </summary>
        public string F_EnglishTitle { get; set; }
        /// <summary>
        /// 中文标题
        /// </summary>
        public string F_ChineseTitle { get; set; }
        
        /// <summary>
		/// 状态表
		/// </summary>
		public string F_FileState { get; set; }
        /// <summary>
        /// 领域
        /// </summary>
        public string F_Domain { get; set; }
        /// <summary>
        ///  采标
        /// </summary>
        public string F_AcquisitionStandard { get; set; } 
        /// <summary>
        /// 发布方
        /// </summary>
        public string F_Publisher { get; set; }
        /// <summary>
        /// 方向
        /// </summary>
        public string F_Direction { get; set; }
        /// <summary>
        ///  发布日期
        /// </summary>
        public DateTime? F_ReleaseDate { get; set; }
        /// <summary>
        ///  实施日期
        /// </summary>
        public DateTime? F_ImplementDate { get; set; }
        public DateTime?  F_CreatorTime { get; set; }

        /// <summary>
		///  原文改版内容
		/// </summary>
		public string F_TotalUrl { get; set; }
        /// <summary>
        ///  原文内容
        /// </summary>
        public string F_HtmlUrl { get; set; }
        /// <summary>
        ///  中文内容
        /// </summary>
        public string F_ChineseUrl { get; set; }
        /// <summary>
        ///  英文内容
        /// </summary>
        public string F_EnglishUrl { get; set; }
        /// <summary>
        /// 中文内容中关键字出现的次数
        /// </summary>
        public Int64 F_ChineseContentAppearNum { get; set; }
        /// <summary>
        /// 英文内容中关键字出现的次数
        /// </summary>
        public Int64 F_EnglishContentAppearNum { get; set; }
    }
}
