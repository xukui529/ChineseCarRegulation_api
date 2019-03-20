using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.ViewModel
{
	public class QuestionAnswerModel
	{
		public string F_Id { get; set; }
		public string F_EnTitle { get; set; }
        public string F_ChTitle { get; set; }
        public string F_Type { get; set; }
		public string F_EnAnswer { get; set; }
        public string F_ChAnswer { get; set; }
        public string F_CreatorUserName { get; set; }
		public string F_CreatorUserId { get; set; }
		public DateTime? F_CreatorTime { get; set; }
		public bool? F_DeleteMark { get; set; }
		public string F_DeleteUserId { get; set; }
		public DateTime? F_DeleteTime { get; set; }
		public string F_LastModifyUserId { get; set; }
		public DateTime? F_LastModifyTime { get; set; }
	}
}
