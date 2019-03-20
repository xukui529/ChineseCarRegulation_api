using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Software.Web.Models
{
	public class MyFile
	{
		public MyFile()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}

		private string innerText;
		/// <summary>
		/// 纯文本
		/// </summary>
		public string InnerText
		{
			get
			{
				return innerText;
			}

			set
			{
				innerText = value;
			}
		}
		private string outerXml;
		/// <summary>
		/// 带样式
		/// </summary>
		public string OuterXml
		{
			get
			{
				return outerXml;
			}

			set
			{
				outerXml = value;
			}
		}


	}
}