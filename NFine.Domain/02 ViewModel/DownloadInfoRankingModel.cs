using NFine.Domain.Entity.ContentManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.ViewModel
{
    public class DownloadInfoRankingModel
    {
        public string num { get; set; }
        public string F_DocumentId { get; set; }
        public string F_Type { get; set; }

        /// <summary>
        /// 中文标题
        /// </summary>
        public string F_DocumentChineseTitle { get; set; }
        /// <summary>
        /// 英文标题
        /// </summary>
        public string F_DocumentEnglishTitle { get; set; }
    }
}
