using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Domain.Entity.ContentManage
{
	public class DocumentEntity : IEntity<DocumentEntity>, ICreationAudited, IDeleteAudited, IModificationAudited
	{
		/// <summary>
		/// 主键
		/// </summary>
		public string F_Id { get; set; }
		/// <summary>
		/// 分类
		/// </summary>
		public int F_Type { get; set; }
		/// <summary>
		/// 中文标题
		/// </summary>
		public string F_ChineseTitle { get; set; }
		/// <summary>
		/// 英文标题
		/// </summary>
		public string F_EnglishTitle { get; set; }
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
		///  标签
		/// </summary>
		public string F_Label { get; set; }
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
		///  资讯内容
		/// </summary>
		public string F_InfoContent { get; set; }
		/// <summary>
		///  发布状态
		/// </summary>
		public int? F_State { get; set; }
		/// <summary>
		///  是否对付费会员开放
		/// </summary>
		public bool F_IsActivateVip { get; set; }

		public bool F_IsActivate { get; set; }
		/// <summary>
		///  创建人
		/// </summary>
		public string F_CreatorUserId { get; set; }
		/// <summary>
		///  创建时间
		/// </summary>
		public DateTime? F_CreatorTime { get; set; }
		public bool? F_DeleteMark { get; set; }
		public string F_DeleteUserId { get; set; }
		public DateTime? F_DeleteTime { get; set; }
		public string F_LastModifyUserId { get; set; }
		public DateTime? F_LastModifyTime { get; set; }
	}
}
