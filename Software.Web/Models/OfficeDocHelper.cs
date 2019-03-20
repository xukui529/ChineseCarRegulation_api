using System;
using System.Text;
using System.IO;
using System.Web;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;
using NFine.Code;
using Software.Web.Models;
using NFine.Application.ContentManage;
using NFine.Domain.Entity.ContentManage;
using Software.Web.Areas.ContentManage.Models;
using System.Threading;
using HtmlAgilityPack;

namespace Software.Web.Models
{
	public class OfficeDocHelper
	{
		private DocumentApp documentApp = new DocumentApp();
		/// <summary>
		/// 计算字符串中子串出现的次数
		/// </summary>
		/// <param name="str">字符串</param>
		/// <param name="substring">子串</param>
		/// <returns>出现的次数</returns>
		public static int SubstringCount(string str, string substring)
		{
			if (str.Contains(substring))
			{
				string strReplaced = str.Replace(substring, "");
				return (str.Length - strReplaced.Length) / substring.Length;
			}

			return 0;
		}
		/// <summary>
		/// 将上传的中英混排word文档转成html并拆分
		/// </summary>
		/// <param name="strFile">上传后服务器路径</param>
		public static string UnUpLoadWord(string strFile)
		{
			string filename = strFile.Substring(strFile.LastIndexOf('\\')).Split('.')[0];
			string ConfigPath = GetPath(filename);
			var htmlFile = ConfigPath + ".html";//@"D:\doc\my\Exl2Word\模拟0903.html";

			WordHelper.word2Html(strFile, htmlFile, filename);
			if (!File.Exists(htmlFile))
			{
				throw new Exception("转换文件不存在。");
			}

			//将中英混排html文档拆分
			List<MyFile> allList = readhtml2(htmlFile, false);//读取html文档，将文档按段落标签格式存储到缓存list
			StringBuilder sb = new StringBuilder();
			sb.Append("<table>");
			Regex cn = new Regex("[\u4e00-\u9fa5]+");//正则表达式 表示汉字范围   
			Regex reg = new Regex(@"<td\b[^>]*>([\s\S]*?)</td>");//去除table中td
			List<string> ywList = new List<string>();//英
			List<string> zwList = new List<string>();//中
			int x = 1;
			foreach (var item in allList)
			{

				if (x % 2 == 0)//偶数，英文
				{
					ReMoveHtml(reg, item);
					if (cn.IsMatch(item.InnerText))//是否包含中文
					{
						sb.Append("<td bgcolor=\"#F4AEB0\" style=\"width:50%;\">" + item.OuterXml + "</td>");
					}
					else
					{
						sb.Append("<td style=\"width:50%;\">" + item.OuterXml + "</td>");
					}

					ywList.Add(item.OuterXml);
					sb.Append("</tr>");
				}
				else
				{
					ReMoveHtml(reg, item);
					sb.Append("<tr>");
					sb.Append("<td style=\" width:50%;\">" + item.OuterXml + "</td>");
					zwList.Add(item.OuterXml);
				}

				x++;
            }
            if (!sb.ToString().EndsWith("</tr>"))
            {
                sb.Append("</tr>");
            }
            var zwHtml = ConfigPath + "-中.html"; //@"D:\doc\my\Exl2Word\中.html";
			var ywHtml = ConfigPath + "-英.html";//@"D:\doc\my\Exl2Word\英.html";
			sb.Append("</table>");

            return sb.ToString();// GetImgSrc(sb.ToString(), filename) ;
            
        }

        /// <summary>
        /// 将上传的中英混排word文档转成html 
        /// </summary>
        /// <param name="strFile"></param>
        /// <returns></returns>
        public static string UnUpLoadWordGetWordContent(string strFile)
        {
            string filename = strFile.Substring(strFile.LastIndexOf('\\')).Split('.')[0];
            string ConfigPath = GetPath(filename);
            var htmlFile = ConfigPath + ".html";//@"D:\doc\my\Exl2Word\模拟0903.html";

            WordHelper.word2Html(strFile, htmlFile, filename);
            if (!File.Exists(htmlFile))
            {
                throw new Exception("转换文件不存在。");
            }
           
                //将中英混排html文档拆分
                List<MyFile> allList = readhtml2(htmlFile, false);//读取html文档，将文档按段落标签格式存储到缓存list 
            string re = ""; 
            foreach (var item in allList)
            {
                re+=  item.OuterXml ;
            }

           
            return re ;

        }
        
        /// <summary>
        /// 去除制定td
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="item"></param>
        private static void ReMoveHtml(Regex reg, MyFile item)
		{
			if (item.OuterXml.Contains("<table") && SubstringCount(item.OuterXml, "<td") == 1)
			{
				item.OuterXml = reg.Replace(item.OuterXml, "$1", 1);
			}
		}

        /// <summary>
        /// 中英文混排 生成pdf html等文件
        /// </summary>
        /// <param name="strFile"></param>
        /// <returns></returns>
		public FileContentModel SaveContent(string strFile, DocumentEntity entity)
		{ 
            FileContentModel model = new FileContentModel();
            string filename = strFile.Substring(strFile.LastIndexOf('\\')).Split('.')[0];
            string ConfigPath = GetPath(filename);
            string dir = Path.GetDirectoryName(ConfigPath);
            var htmlFile = "";
            htmlFile = ConfigPath + ".html";
            Common.Log("log", "log", "word2Html 开始：" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss fff"));
            WordHelper.word2Html(strFile, htmlFile, filename);
            Common.Log("log", "log", "word2Html end：" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss fff"));

            model.HtmlUrl = htmlFile;
			if (!File.Exists(htmlFile))
			{
				throw new Exception("转换文件不存在。");
			}
            //生成pdf 生成与名字相同，后缀不同的pdf文件，位置保存一致  
            CreatePdfFile(ConfigPath, Common.GetPdfFileName(dir, entity.F_EnglishTitle, 3)  );
            Common.Log("log", "log", "readhtml2 begin：" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss fff"));
             
            //将中英混排html文档拆分
            List<MyFile> allList = readhtml2(htmlFile, false);//读取html文档，将文档按段落标签格式存储到缓存list
            Common.Log("log", "log", "readhtml2 end：" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss fff"));

            StringBuilder sb = new StringBuilder();
			sb.Append("<table>");
			Regex cn = new Regex("[\u4e00-\u9fa5]+");//正则表达式 表示汉字范围   
			Regex reg = new Regex(@"<td\b[^>]*>([\s\S]*?)</td>");//去除table中td
			List<string> ywList = new List<string>();//英
			List<string> zwList = new List<string>();//中
			int x = 1;
			foreach (var item in allList)
			{
				if (x % 2 == 0)//偶数，英文
				{
					ReMoveHtml(reg, item);
					if (cn.IsMatch(item.InnerText))//是否包含中文
					{
						sb.Append("<td bgcolor=\"#F4AEB0\" style=\"width:50%;\">" + item.OuterXml + "</td>");
					}
					else
					{
						sb.Append("<td style=\"width:50%;\">" + item.OuterXml + "</td>");
					}
					ywList.Add(item.OuterXml);
					sb.Append("</tr>");
				}
				else
				{
					ReMoveHtml(reg, item);
					sb.Append("<tr>");
					sb.Append("<td style=\" width:50%;\">" + item.OuterXml + "</td>");
					zwList.Add(item.OuterXml);
				}
				x++;
			}
            
            if (!sb.ToString().EndsWith("</tr>"))
            {
                sb.Append("</tr>");
            }

            sb.Append("</table>");
            var zwHtml = ConfigPath + "-中.html"; //@"D:\doc\my\Exl2Word\中.html";
		 
            DelFile(zwHtml);
            var star = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml; charset=utf-8\" /><title></title></head><body >";
            var end = "</body></html>";
            zwList.Insert(0, star);
            zwList.Add(end);
            File.AppendAllLines(zwHtml, zwList);  
            var ywHtml = ConfigPath + "-英.html";//@"D:\doc\my\Exl2Word\英.html";
			 
            DelFile(ywHtml); 
            ywList.Insert(0, star);
            ywList.Add(end);
            File.AppendAllLines(ywHtml, ywList); 
            var zywHtml = ConfigPath + "-中英.html";
            
            DelFile(zywHtml);  
            File.AppendAllText(zywHtml, sb.ToString(), Encoding.UTF8);

            //要生成中英文单独的pdf 名字与  

            // 加上html 标记，用于解决出现乱码问题



            CreatePdfFile(ConfigPath + "-中", Common.GetPdfFileName(dir, entity.F_ChineseTitle, 1) );
            CreatePdfFile(ConfigPath + "-英", Common.GetPdfFileName(dir, entity.F_EnglishTitle, 2) );
          
            model.ChineseUrl = zwHtml;
			model.EnglishUrl = ywHtml;
			model.TotalUrl = zywHtml; 

            return model;
		}
      
      
        private void CreatePdfFile(string sfilename,string tfilename)
        {
           // DelFile(sfilename + ".html");
            DelFile(tfilename + ".pdf");
            Common.Log("log", "log", "html2pdf begin：" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss fff"));

            PDFHelper.html2pdf(sfilename + ".html", tfilename + ".pdf");
            Common.Log("log", "log", "html2pdf end：" + DateTime.Now.ToString("yyyyMMdd HH:mm:ss fff"));

        }
        private void DelFile(string fileName)
        {
            //if (Directory.Exists(Path.GetDirectoryName(fileName)))
            //{
            //  //  Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            //}
            //else
            //{
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
            //}
           
        }
        public FileContentModel SaveContentByHtml(string htmlCode,string filePath)
        {
            FileContentModel model = new FileContentModel();
            string filename = filePath.Substring(filePath.LastIndexOf('\\')).Split('.')[0];
            string ConfigPath = GetPath(filename);
            //先更新原始html
            if (File.Exists(ConfigPath + ".html"))
            {
                File.Delete(ConfigPath + ".html");
            }
            File.WriteAllText(ConfigPath + ".html", htmlCode);


            Common.Log("log", "log", "77");
            PDFHelper.html2pdf(ConfigPath + ".html", ConfigPath + ".pdf");
            Common.Log("log", "log", "88");
            //将中英混排html文档拆分
            List<MyFile> allList = readhtmlByHtml(htmlCode, filePath);//读取html文档，将文档按段落标签格式存储到缓存list


            Common.Log("log", "log", "99");
            StringBuilder sb = new StringBuilder();
            sb.Append("<table>");
            Regex cn = new Regex("[\u4e00-\u9fa5]+");//正则表达式 表示汉字范围   
            Regex reg = new Regex(@"<td\b[^>]*>([\s\S]*?)</td>");//去除table中td
            List<string> ywList = new List<string>();//英
            List<string> zwList = new List<string>();//中
            int x = 1;
            foreach (var item in allList)
            {
                if (x % 2 == 0)//偶数，英文
                {
                    ReMoveHtml(reg, item);
                    if (cn.IsMatch(item.InnerText))//是否包含中文
                    {
                        sb.Append("<td bgcolor=\"#F4AEB0\" style=\"width:50%;\">" + item.OuterXml + "</td>");
                    }
                    else
                    {
                        sb.Append("<td style=\"width:50%;\">" + item.OuterXml + "</td>");
                    }
                    ywList.Add(item.OuterXml);
                    sb.Append("</tr>");
                }
                else
                {
                    ReMoveHtml(reg, item);
                    sb.Append("<tr>");
                    sb.Append("<td style=\" width:50%;\">" + item.OuterXml + "</td>");
                    zwList.Add(item.OuterXml);
                }
                x++;
            }

            if (!sb.ToString().EndsWith("</tr>"))
            {
                sb.Append("</tr>");
            }

            var zwHtml = ConfigPath + "-中.html"; //@"D:\doc\my\Exl2Word\中.html";
            if (File.Exists(zwHtml))
            {
                File.Delete(zwHtml);
            }
            File.AppendAllLines(zwHtml, zwList);
            var ywHtml = ConfigPath + "-英.html";//@"D:\doc\my\Exl2Word\英.html";
            if (File.Exists(ywHtml))
            {
                File.Delete(ywHtml);
            }
            File.AppendAllLines(ywHtml, ywList);
            sb.Append("</table>");
            var zywHtml = ConfigPath + "-中英.html";
            if (!Directory.Exists(Path.GetDirectoryName(zywHtml)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(zywHtml));
            }
            if (File.Exists(zywHtml))
            {
                File.Delete(zywHtml);
            }
            File.AppendAllText(zywHtml, sb.ToString(), Encoding.UTF8);

            Common.Log("log", "log", "100");
            model.ChineseUrl = zwHtml;
            model.EnglishUrl = ywHtml;
            model.TotalUrl = zywHtml;
            model.ZWList = zwList;
            model.YWList = ywList; 
            return model;
        }

        public FileContentModel NoSaveContentByHtml(string htmlCode,string filename)
        {
            FileContentModel model = new FileContentModel(); 
            //将中英混排html文档拆分
            List<MyFile> allList = readhtmlByHtmlNoSave(htmlCode, filename);//读取html文档，将文档按段落标签格式存储到缓存list
            
            StringBuilder sb = new StringBuilder();
            sb.Append("<table>");
            Regex cn = new Regex("[\u4e00-\u9fa5]+");//正则表达式 表示汉字范围   
            Regex reg = new Regex(@"<td\b[^>]*>([\s\S]*?)</td>");//去除table中td
            List<string> ywList = new List<string>();//英
            List<string> zwList = new List<string>();//中
            int x = 1;
            foreach (var item in allList)
            {
                if (x % 2 == 0)//偶数，英文
                {
                    ReMoveHtml(reg, item);
                    if (cn.IsMatch(item.InnerText))//是否包含中文
                    {
                        sb.Append("<td bgcolor=\"#F4AEB0\" style=\"width:50%;\">" + item.OuterXml + "</td>");
                    }
                    else
                    {
                        sb.Append("<td style=\"width:50%;\">" + item.OuterXml + "</td>");
                    }
                    ywList.Add(item.OuterXml);
                    sb.Append("</tr>");
                }
                else
                {
                    ReMoveHtml(reg, item);
                    sb.Append("<tr>");
                    sb.Append("<td style=\" width:50%;\">" + item.OuterXml + "</td>");
                    zwList.Add(item.OuterXml);
                }
                x++;
            }
            if (!sb.ToString().EndsWith("</tr>"))
            {
                sb.Append("</tr>");
            }
            sb.Append("</table>"); 
            model.TotalCode = sb.ToString(); 
            return model;
        }

        public static string GetPath(string filename)
		{
			string subFileName = string.Empty;
			string strFilePath = string.Empty;
			int index = filename.IndexOf('.');
			if (index > 0)
			{
				subFileName = filename.Substring(0, index);
				strFilePath = "/Temp/Word/" + subFileName+"/";
			}
			else
			{
				strFilePath = "/Temp/Word/" + filename;
			}
			// 判断指定目录下是否存在文件夹，如果不存在，则创建 
			if (!Directory.Exists(HttpContext.Current.Server.MapPath(strFilePath)))
			{
				// 创建up文件夹 
				Directory.CreateDirectory(HttpContext.Current.Server.MapPath(strFilePath));
			}
            //被转换的html文档保存的位置 
            filename = filename.Replace(" ", "_");

            return HttpContext.Current.Server.MapPath(strFilePath + filename);
		}
        public static string GetFilePath(string filename)
        {
            string subFileName = string.Empty;
            string strFilePath = string.Empty;
            int index = filename.IndexOf('.');
            if (index > 0)
            {
                subFileName = filename.Substring(0, index);
                strFilePath = "/Temp/Word/" + subFileName + "/";
            }
            else
            {
                strFilePath = "/Temp/Word/" + filename;
            }
            // 判断指定目录下是否存在文件夹，如果不存在，则创建 
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(strFilePath)))
            {
                // 创建up文件夹 
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(strFilePath));
            }
            //被转换的html文档保存的位置 
            filename = filename.Replace(" ", "_");

            return  strFilePath;
        }
        public static string GetFileName(string filename)
        {
            string subFileName = string.Empty;
            string strFilePath = string.Empty;
            int index = filename.IndexOf('.');
            if (index > 0)
            {
                subFileName = filename.Substring(0, index);
                strFilePath = "/Temp/Word/" + subFileName + "/";
            }
            else
            {
                strFilePath = "/Temp/Word/" + filename;
            }
            // 判断指定目录下是否存在文件夹，如果不存在，则创建 
            if (!Directory.Exists(HttpContext.Current.Server.MapPath(strFilePath)))
            {
                // 创建up文件夹 
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath(strFilePath));
            }
            //被转换的html文档保存的位置 
            filename = filename.Replace(" ", "_");

            return filename;
        }
        /// <summary>
        /// 根据相对路径获取绝对路径
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetFilePathByCurrentPath(string filename)
        { 
            return HttpContext.Current.Server.MapPath(filename); 
        }
        private static List<MyFile> readhtml2(string filePath, bool isText = true)
		{ 
			List<MyFile> list = new List<MyFile>();
			StreamReader sr = new StreamReader(filePath, Encoding.UTF8);

			string htmlCode = sr.ReadToEnd();
            sr.Close();
            string filename = Path.GetFileNameWithoutExtension(filePath);
           
            XHtmlTools xmlt = new XHtmlTools();
			htmlCode = htmlCode.Replace("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">", "");//&nbsp;
			XmlDocument xmld = xmlt.GetXmlDocument(htmlCode);

			//获取当前XML文档的根 一级
			XmlNode oNode = xmld.DocumentElement;

			//获取根节点的所有子节点列表 
			XmlNodeList oList = oNode.ChildNodes;

			//标记当前节点
			XmlNode secNode;
			XmlNode thrNode;
			//遍历所有二级节点
			for (int i = 0; i < oList.Count; i++)
			{
				//二级
				secNode = oList[i];

				//检测当前节点的名称，节点的值是否与已知匹配
				if (secNode.Name.ToLower().Equals("body"))
				{
					//遍历当前节点的所有子节点
					for (int j = 0; j < secNode.ChildNodes.Count; j++)
					{
						//检测当前节点的子节点名称是否与已知匹配
						if (secNode.ChildNodes[j].Attributes["class"].Value.ToLower().StartsWith("section"))
						{

							//三级
							thrNode = secNode.ChildNodes[j];
							int l = 0;
							for (int k = 0; k < thrNode.ChildNodes.Count; k++)
							{
								if (thrNode.ChildNodes[k].OuterXml.Contains("Evaluation Warning")
								   || thrNode.ChildNodes[k].OuterXml.Contains("标准书眉")
								   || thrNode.ChildNodes[k].Name.ToLower() == "br"
								   || (thrNode.ChildNodes[k].OuterXml.Contains("class=\"一级条标题\"") && thrNode.ChildNodes[k].ChildNodes.Count == 1 && thrNode.ChildNodes[k].LastChild.InnerText.Trim() == "")
									)
								{
									continue;
								}
								//判断空行
								if (thrNode.ChildNodes[k].ChildNodes.Count > 0 && (thrNode.ChildNodes[k].LastChild.InnerXml.Trim() == "" || thrNode.ChildNodes[k].LastChild.InnerXml.Trim() == "&nbsp;")
									&& !thrNode.ChildNodes[k].OuterXml.Contains("<img"))
								{
									if (thrNode.ChildNodes[k].ChildNodes.Count == 1)
									{
										continue;
									}
									else
									{
										var str = "";
										for (int m = 0; m < thrNode.ChildNodes[k].ChildNodes.Count; m++)
										{
											str += thrNode.ChildNodes[k].ChildNodes[m].InnerXml.Trim();
										}
										if (str == "")
										{
											continue;
										}
									}
								}
								if (thrNode.ChildNodes[k].OuterXml.Contains("class=\"一级条标题\"") && thrNode.ChildNodes[k].LastChild.InnerText.Trim() == "")
								{
									l++;
									thrNode.ChildNodes[k].LastChild.InnerText = "3." + l.ToString();
									// continue;
								}
								if ((thrNode.ChildNodes[k].Name.ToLower() == "div" && thrNode.ChildNodes[k].FirstChild == null) ||
							  (thrNode.ChildNodes[k].Name.ToLower() == "div" && thrNode.ChildNodes[k].FirstChild != null && thrNode.ChildNodes[k].FirstChild.Name.ToLower() != "table"))
								{
									continue;
								}
								list.Add(new MyFile() { InnerText = thrNode.ChildNodes[k].InnerText, OuterXml = thrNode.ChildNodes[k].OuterXml });
							}

						}
					}
				}
			}
			return list;
		}


        private static List<MyFile> readhtmlByHtml(string htmlCode,string filePath)
        {
            List<MyFile> list = new List<MyFile>();
         
          
            string filename = Path.GetFileNameWithoutExtension(filePath);
            //htmlCode = WordHelper.GetImgSrc(htmlCode, filename);
            //xml convert
            //byte[] bytes = System.Text.Encoding.Default.GetBytes(HtmlCode);
            XHtmlTools xmlt = new XHtmlTools();
            htmlCode = htmlCode.Replace("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">", "");//&nbsp;
            XmlDocument xmld = xmlt.GetXmlDocument(htmlCode);

            //获取当前XML文档的根 一级
            XmlNode oNode = xmld.DocumentElement;

            //获取根节点的所有子节点列表 
            XmlNodeList oList = oNode.ChildNodes;

            //标记当前节点
            XmlNode secNode;
            XmlNode thrNode;
            //遍历所有二级节点
            for (int i = 0; i < oList.Count; i++)
            {
                //二级
                secNode = oList[i];

                //检测当前节点的名称，节点的值是否与已知匹配
                if (secNode.Name.ToLower().Equals("body"))
                {
                    //遍历当前节点的所有子节点
                    for (int j = 0; j < secNode.ChildNodes.Count; j++)
                    {
                        //检测当前节点的子节点名称是否与已知匹配
                        if (secNode.ChildNodes[j].Attributes["class"].Value.ToLower().StartsWith("section"))
                        {

                            //三级
                            thrNode = secNode.ChildNodes[j];
                            int l = 0;
                            for (int k = 0; k < thrNode.ChildNodes.Count; k++)
                            {
                                if (thrNode.ChildNodes[k].OuterXml.Contains("Evaluation Warning")
                                   || thrNode.ChildNodes[k].OuterXml.Contains("标准书眉")
                                   || thrNode.ChildNodes[k].Name.ToLower() == "br"
                                   || (thrNode.ChildNodes[k].OuterXml.Contains("class=\"一级条标题\"") && thrNode.ChildNodes[k].ChildNodes.Count == 1 && thrNode.ChildNodes[k].LastChild.InnerText.Trim() == "")
                                    )
                                {
                                    continue;
                                }
                                //判断空行
                                if (thrNode.ChildNodes[k].ChildNodes.Count > 0 && (thrNode.ChildNodes[k].LastChild.InnerXml.Trim() == "" || thrNode.ChildNodes[k].LastChild.InnerXml.Trim() == "&nbsp;")
                                    && !thrNode.ChildNodes[k].OuterXml.Contains("<img"))
                                {
                                    if (thrNode.ChildNodes[k].ChildNodes.Count == 1)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        var str = "";
                                        for (int m = 0; m < thrNode.ChildNodes[k].ChildNodes.Count; m++)
                                        {
                                            str += thrNode.ChildNodes[k].ChildNodes[m].InnerXml.Trim();
                                        }
                                        if (str == "")
                                        {
                                            continue;
                                        }
                                    }
                                }
                                if (thrNode.ChildNodes[k].OuterXml.Contains("class=\"一级条标题\"") && thrNode.ChildNodes[k].LastChild.InnerText.Trim() == "")
                                {
                                    l++;
                                    thrNode.ChildNodes[k].LastChild.InnerText = "3." + l.ToString();
                                    // continue;
                                }
                                if ((thrNode.ChildNodes[k].Name.ToLower() == "div" && thrNode.ChildNodes[k].FirstChild == null) ||
                              (thrNode.ChildNodes[k].Name.ToLower() == "div" && thrNode.ChildNodes[k].FirstChild != null && thrNode.ChildNodes[k].FirstChild.Name.ToLower() != "table"))
                                {
                                    continue;
                                }
                                list.Add(new MyFile() { InnerText = thrNode.ChildNodes[k].InnerText, OuterXml = thrNode.ChildNodes[k].OuterXml });
                            }

                        }
                    }
                }
            }
            return list;
        }

        private static List<MyFile> readhtmlByHtmlNoSave(string htmlCode,string filename)
        {
            List<MyFile> list = new List<MyFile>(); 
           // htmlCode = WordHelper.GetImgSrc(htmlCode, filename);
            //xml convert
            //byte[] bytes = System.Text.Encoding.Default.GetBytes(HtmlCode);
            XHtmlTools xmlt = new XHtmlTools();
            htmlCode = htmlCode.Replace("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">", "");//&nbsp;
            XmlDocument xmld = xmlt.GetXmlDocument(htmlCode);

            //获取当前XML文档的根 一级
            XmlNode oNode = xmld.DocumentElement;

            //获取根节点的所有子节点列表 
            XmlNodeList oList = oNode.ChildNodes;

            //标记当前节点
            XmlNode secNode;
            XmlNode thrNode;
            //遍历所有二级节点
            for (int i = 0; i < oList.Count; i++)
            {
                //二级
                secNode = oList[i];

                //检测当前节点的名称，节点的值是否与已知匹配
                if (secNode.Name.ToLower().Equals("body"))
                {
                    //遍历当前节点的所有子节点
                    for (int j = 0; j < secNode.ChildNodes.Count; j++)
                    {
                        //检测当前节点的子节点名称是否与已知匹配
                        if (secNode.ChildNodes[j].Attributes["class"].Value.ToLower().StartsWith("section"))
                        {

                            //三级
                            thrNode = secNode.ChildNodes[j];
                            int l = 0;
                            for (int k = 0; k < thrNode.ChildNodes.Count; k++)
                            {
                                if (thrNode.ChildNodes[k].OuterXml.Contains("Evaluation Warning")
                                   || thrNode.ChildNodes[k].OuterXml.Contains("标准书眉")
                                   || thrNode.ChildNodes[k].Name.ToLower() == "br"
                                   || (thrNode.ChildNodes[k].OuterXml.Contains("class=\"一级条标题\"") && thrNode.ChildNodes[k].ChildNodes.Count == 1 && thrNode.ChildNodes[k].LastChild.InnerText.Trim() == "")
                                    )
                                {
                                    continue;
                                }
                                //判断空行
                                if (thrNode.ChildNodes[k].ChildNodes.Count > 0 && (thrNode.ChildNodes[k].LastChild.InnerXml.Trim() == "" || thrNode.ChildNodes[k].LastChild.InnerXml.Trim() == "&nbsp;")
                                    && !thrNode.ChildNodes[k].OuterXml.Contains("<img"))
                                {
                                    if (thrNode.ChildNodes[k].ChildNodes.Count == 1)
                                    {
                                        continue;
                                    }
                                    else
                                    {
                                        var str = "";
                                        for (int m = 0; m < thrNode.ChildNodes[k].ChildNodes.Count; m++)
                                        {
                                            str += thrNode.ChildNodes[k].ChildNodes[m].InnerXml.Trim();
                                        }
                                        if (str == "")
                                        {
                                            continue;
                                        }
                                    }
                                }
                                if (thrNode.ChildNodes[k].OuterXml.Contains("class=\"一级条标题\"") && thrNode.ChildNodes[k].LastChild.InnerText.Trim() == "")
                                {
                                    l++;
                                    thrNode.ChildNodes[k].LastChild.InnerText = "3." + l.ToString();
                                    // continue;
                                }
                                if ((thrNode.ChildNodes[k].Name.ToLower() == "div" && thrNode.ChildNodes[k].FirstChild == null) ||
                              (thrNode.ChildNodes[k].Name.ToLower() == "div" && thrNode.ChildNodes[k].FirstChild != null && thrNode.ChildNodes[k].FirstChild.Name.ToLower() != "table"))
                                {
                                    continue;
                                }
                                list.Add(new MyFile() { InnerText = thrNode.ChildNodes[k].InnerText, OuterXml = thrNode.ChildNodes[k].OuterXml });
                            }

                        }
                    }
                }
            }
            return list;
        }
     

        #region new
        public FileContentModel SaveContentByHtmlNew(string htmlCode, string filePath)
        {
            FileContentModel model = new FileContentModel();
            string filename = filePath.Substring(filePath.LastIndexOf('\\')).Split('.')[0];
            string ConfigPath = GetPath(filename);
            //先更新原始html
            if (File.Exists(ConfigPath + ".html"))
            {
                File.Delete(ConfigPath + ".html");
            }
            File.WriteAllText(ConfigPath + ".html", htmlCode); 
            PDFHelper.html2pdf(ConfigPath + ".html", ConfigPath + ".pdf");
            //将中英混排html文档拆分

            List<MyFile> allList = readhtmlByHtmlNew(htmlCode, filePath);//读取html文档，将文档按段落标签格式存储到缓存list


            StringBuilder sb = new StringBuilder();
            sb.Append("<table>");
            Regex cn = new Regex("[\u4e00-\u9fa5]+");//正则表达式 表示汉字范围   
            Regex reg = new Regex(@"<td\b[^>]*>([\s\S]*?)</td>");//去除table中td
            List<string> ywList = new List<string>();//英
            List<string> zwList = new List<string>();//中
            int x = 1;
            foreach (var item in allList)
            {
                if (x % 2 == 0)//偶数，英文
                {
                    ReMoveHtml(reg, item);
                    if (cn.IsMatch(item.InnerText))//是否包含中文
                    {
                        sb.Append("<td bgcolor=\"#F4AEB0\" style=\"width:50%;\">" + item.OuterXml + "</td>");
                    }
                    else
                    {
                        sb.Append("<td style=\"width:50%;\">" + item.OuterXml + "</td>");
                    }
                    ywList.Add(item.OuterXml);
                    sb.Append("</tr>");
                }
                else
                {
                    ReMoveHtml(reg, item);
                    sb.Append("<tr>");
                    sb.Append("<td style=\" width:50%;\">" + item.OuterXml + "</td>");
                    zwList.Add(item.OuterXml);
                }
                x++;
            }


            if (!sb.ToString().EndsWith("</tr>"))
            {
                sb.Append("</tr>");
            }
            var zwHtml = ConfigPath + "-中.html"; //@"D:\doc\my\Exl2Word\中.html";
            if (File.Exists(zwHtml))
            {
                File.Delete(zwHtml);
            }
            File.AppendAllLines(zwHtml, zwList);
            var ywHtml = ConfigPath + "-英.html";//@"D:\doc\my\Exl2Word\英.html";
            if (File.Exists(ywHtml))
            {
                File.Delete(ywHtml);
            }
            File.AppendAllLines(ywHtml, ywList);
            sb.Append("</table>");
            var zywHtml = ConfigPath + "-中英.html";
            if (!Directory.Exists(Path.GetDirectoryName(zywHtml)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(zywHtml));
            }
            if (File.Exists(zywHtml))
            {
                File.Delete(zywHtml);
            }
            File.AppendAllText(zywHtml, sb.ToString(), Encoding.UTF8);

            model.ChineseUrl = zwHtml;
            model.EnglishUrl = ywHtml;
            model.TotalUrl = zywHtml;
            model.ZWList = zwList;
            model.YWList = ywList;
            return model;
        }


        private static List<MyFile> readhtmlByHtmlNew(string htmlCode, string filePath)
        {
            List<MyFile> list = new List<MyFile>(); 
            string filename = Path.GetFileNameWithoutExtension(filePath);
            HtmlDocument htmlDocument= QueryHtmlNode.GetHtmlDocByHtmlCode(htmlCode);

           HtmlNodeCollection  htmlNodes = htmlDocument.DocumentNode.SelectNodes("//*[@class='Section0']");
            foreach (var item in htmlNodes)
            {
                var listItems = item.ChildNodes;
                foreach (var itemTemp in listItems)
                {
                    //内容 遍历
                    int l = 0;
                    foreach (var itemTempTemp in itemTemp.ChildNodes)
                    {
                        if (itemTempTemp.OuterHtml.Contains("Evaluation Warning")
                           || itemTempTemp.OuterHtml.Contains("标准书眉")
                           || itemTempTemp.Name.ToLower() == "br"
                           || 
                           (itemTempTemp.OuterHtml.Contains("class=\"一级条标题\"") 
                           && itemTempTemp.ChildNodes.Count == 1 
                           && itemTempTemp.LastChild.InnerText.Trim() == "")
                            )
                        {
                            continue;
                        }
                        //判断空行
                        if (itemTempTemp.ChildNodes.Count > 0 && (itemTempTemp.LastChild.InnerHtml.Trim() == "" || itemTempTemp.LastChild.InnerHtml.Trim() == "&nbsp;")
                            && !itemTempTemp.OuterHtml.Contains("<img"))
                        {
                            if (itemTempTemp.ChildNodes.Count == 1)
                            {
                                continue;
                            }
                            else
                            {
                                var str = "";
                                for (int m = 0; m < itemTempTemp.ChildNodes.Count; m++)
                                {
                                    str += itemTempTemp.ChildNodes[m].InnerHtml.Trim();
                                }
                                if (str == "")
                                {
                                    continue;
                                }
                            }
                        }
                        if (itemTempTemp.OuterHtml.Contains("class=\"一级条标题\"") && itemTempTemp.LastChild.InnerText.Trim() == "")
                        {
                            l++;
                            itemTempTemp.LastChild.InnerHtml = "3." + l.ToString();
                            // continue;
                        }
                        if ((itemTempTemp.Name.ToLower() == "div" && itemTempTemp.FirstChild == null) ||
                      (itemTempTemp.Name.ToLower() == "div" && itemTempTemp.FirstChild != null && itemTempTemp.FirstChild.Name.ToLower() != "table"))
                        {
                            continue;
                        }
                        list.Add(new MyFile() { InnerText = itemTempTemp.InnerHtml, OuterXml = itemTempTemp.OuterHtml });
                    } 
                }
            } 
            return list;
        }

        #endregion

        #region 上传单个文件
        /// <summary>
        ///  单独上传 中文 或者英文 分别保存俩html 俩pdf pdf与html名称一致后缀不同
        /// </summary>
        /// <param name="strFile"></param>
        /// <param name="entity"></param>
        /// <param name="type">1中文 2英文</param>
        /// <returns></returns>
        public string SaveContentForZWOrYW(string strFile, DocumentEntity entity,int type)
        {
          
            string filename = strFile.Substring(strFile.LastIndexOf('\\')).Split('.')[0];
            string ConfigPath = GetPath(filename);//文件名，带 中 英 的
            string dir= ConfigPath;//目录 不带 中 英
            var htmlFile = "";
            if (type == 1)
            {
                ConfigPath = ConfigPath + "-中";
                  
            }
            else
            {
                ConfigPath = ConfigPath + "-英"; 
            }
            htmlFile = ConfigPath + ".html";
            WordHelper.word2Html(strFile, htmlFile, filename);
           
            if (!File.Exists(htmlFile))
            {
                throw new Exception("转换文件不存在。");
            }
            //生成pdf 生成与名字相同，后缀不同的pdf文件，位置保存一致 
            PDFHelper.html2pdf(ConfigPath + ".html", ConfigPath + ".pdf"); 
            return htmlFile;
        }


        public string GetContentByHtmlUrl(string filePath)
        { 
            StreamReader sr = new StreamReader(filePath, Encoding.UTF8); 
            return sr.ReadToEnd(); 
        } 
        #endregion
    }
}