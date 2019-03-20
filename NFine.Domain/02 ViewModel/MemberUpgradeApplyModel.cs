using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.ViewModel
{
    public class   MemberUpgradeApplyModel 
    {

        public string F_Id { get; set; }
        /// <summary>
        /// 会员id
        /// </summary>
        public string F_MemberId { get; set; }
        public string F_Email { get; set; }
        /// <summary>
        /// 国家
        /// </summary>
        public string F_Country { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string F_Phone { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string F_Account { get; set; }
        /// <summary>
        ///   会员级别 1 季度 2 年度                                 
        /// </summary>
        public int? F_HYType { get; set; }
        /// <summary>
        /// 开通时间
        /// </summary>
        public DateTime? F_OpenUpTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string F_Remark { get; set; }
        /// <summary>
        /// 1待审核 2 审核通过
        /// </summary>
        public int? F_Status { get; set; }
        public string F_CreatorUserId { get; set; }
        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime? F_CreatorTime { get; set; }
        public bool? F_DeleteMark { get; set; }
        public string F_DeleteUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
    }
}
