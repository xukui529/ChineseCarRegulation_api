using NFine.Domain.Entity.ContentManage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.ViewModel
{
    public class DocumentShowListModel
    {
        public string Id { get; set; }
        public int Type { get; set; }
        /// <summary>
        /// 1 中文单独 2 英文单独 3 中英文单独 4 中英文混排上传（带左右结构的）
        /// </summary>
        public int DocShowType { get; set; } 
        public string Title { get; set; }
        public DateTime? CreatorTime { get; set; } 
        public string  FileState { get; set; }
        public DateTime?  ReleaseDate { get; set; }
        public DateTime?  ImplementDate { get; set; } 
    }
}
