using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.ViewModel
{
	public class DocumentTotalModel
    {
        /// <summary>
        ///  待提交0
        /// </summary>
        public int WaitSubmitTotal { get; set; }
        /// <summary>
        ///  待审核1
        /// </summary>
        public int UnauditedTotal { get; set; }
        /// <summary>
        /// 待发布2
        /// </summary>
        public int WaitPublishe { get; set; }
        /// <summary>
        ///  未通过3
        /// </summary>
        public int AuditTotal { get; set; }
		/// <summary>
		///  已发布4
		/// </summary>
		public int PublishedTotal { get; set; }
	}
}
