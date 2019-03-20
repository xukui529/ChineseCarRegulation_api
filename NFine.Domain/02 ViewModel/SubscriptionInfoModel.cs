using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.ViewModel
{
    public   class SubscriptionInfoModel
    {
        public int? F_SendNum { get; set; }
        public bool? F_IsSend { get; set; }
        public string F_Id { get; set; }
        public bool? F_IsExclude { get; set; }
        public DateTime? F_ReleaseDate { get; set; }
        public DateTime? F_BeginTime { get; set; }
        
        public DateTime? F_EndTime { get; set; }
        public string F_ChineseTitle { get; set; }
        public string F_EnglishTitle { get; set; } 
    }
}
