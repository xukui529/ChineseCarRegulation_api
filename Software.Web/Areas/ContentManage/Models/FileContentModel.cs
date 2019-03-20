using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Software.Web.Areas.ContentManage.Models
{
	public class FileContentModel
	{
		public  string ChineseUrl { get; set; }
		public string EnglishUrl { get; set; }
		public string HtmlUrl { get; set; }
		public string TotalUrl { get; set; }

        public string HtmlCode { get; set; }
        public string TotalCode { get; set; }
        public List< string> YWList { get; set; }
        public List<string> ZWList { get; set; }
    }
}