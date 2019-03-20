using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.SystemManage
{
    public class MemberEntity : IEntity<MemberEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
    {
        public string F_Id { get; set; }
        /// <summary>
        /// 会员类别 0 普通 1 季付 2年付
        /// </summary>
        public int F_HYType { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string F_Email { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string F_Password { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string F_Phone { get; set; }
        /// <summary>
        /// 费用
        /// </summary>
        public string F_Account { get; set; }
        /// <summary>
        /// 开始日期--开通日期
        /// </summary>
        public DateTime? F_StartDate { get; set; }
        /// <summary>
        /// 结束日期--根据充值情况改变
        /// </summary>
        public DateTime? F_EndDate { get; set; }

        /// <summary>
        /// 次数状态 1：首次    2：多次
        /// </summary>
        public int F_TimesState { get; set; } 
        /// <summary>
        /// 访问状态  1：正常   2：超期    3：即将逾期  4：逾期
        /// </summary>
        public int F_UseState { get; set; }
        /// <summary>
        ///  下载与打印次数
        /// </summary>
        public int F_DownAndPrintCount { get; set; }
        /// <summary>
        ///  状态 1：普通会员     3：高级会员  4：逾期  去掉  2：待审核
        /// </summary>
        public int F_State { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
        public bool? F_DeleteMark { get; set; }


        public bool? F_IsSubscription { get; set; }
        public int? F_SubscriptionLanguage { get; set; }


        public bool? F_IsDeputy { get; set; }
        public string F_SuperUId { get; set; }
        public string F_SecretKey { get; set; }
        public DateTime? F_SecretKeyPastDue { get; set; }
        public string F_Country { get; set; }
    }
}
