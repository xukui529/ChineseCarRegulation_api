using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.SystemManage
{
	public class ClassifySetEntity : IEntity<ClassifySetEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
	{
		public string F_Id { get; set; }
		public string ParentId { get; set; }
		public string Name { get; set; }
		public string F_CreatorUserId { get; set; }
		public DateTime? F_CreatorTime { get; set; }
		public bool? F_DeleteMark { get; set; }
		public string F_DeleteUserId { get; set; }
		public DateTime? F_DeleteTime { get; set; }
		public string F_LastModifyUserId { get; set; }
		public DateTime? F_LastModifyTime { get; set; }
	}
}
