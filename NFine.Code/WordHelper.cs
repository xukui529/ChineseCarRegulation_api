using Spire.Doc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace NFine.Code
{
    public class WordHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sFile"></param>
        /// <param name="tFile"></param>
        /// <param name="dir">不带 中 英 等后缀</param>
        /// <returns></returns>
        public static string word2Html(string sFile, string tFile,string dir)
        {
            try
            {
                string htmlCode = "";
                Document document = new Document();
                document.LoadFromFile(sFile);
                if (File.Exists(tFile))
                    File.Delete(tFile);
                document.SaveToFile(tFile, FileFormat.Html);
                if (File.Exists(tFile))
                {
                    var refile = File.ReadAllText(tFile).Replace("Evaluation Warning: The document was created with Spire.Doc for .NET.", "");
                    File.Delete(tFile);
                    //读取文件的图片 要加一个目录，按跟目录查看算 
                    refile = GetImgSrc(refile, dir);
                    File.AppendAllText(tFile, refile);
                    htmlCode = refile;
                }
                return htmlCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string GetImgSrc(string code, string fileName)
        {
         
            string hostAddr = Configs.GetValue("HostAddr");
            string imgSrc = "<img src=\"" + hostAddr + "Temp/Word/" + fileName + "/"; 
            code = code.Replace("<img src=\"", imgSrc);
            code = GetCSSSrc(code, fileName);
            return code;
        }
        public static string GetCSSSrc(string code, string fileName)
        {
            string hostAddr = Configs.GetValue("HostAddr");
            string cssSrc = "<link href=\"" + hostAddr + "Temp/Word/" + fileName + "/";
            //string cssSrc1 = "<link href=\"\\Temp\\Word\\\\";
            //string cssSrc2 = "<link href=\"\\Temp\\Word\\" + fileName + "\\\\Temp\\Word\\" + fileName + "\\";
            //code = code.Replace(cssSrc1, "<link href=\"");
            //code = code.Replace(cssSrc2, "<link href=\"");
            code = code.Replace("<link href=\"", cssSrc);
            return code;
        }
        public static string MergeHtml(string sName, string eName, string showHtml)
        {
            try
            {
                List<string> list1 = readhtml2(sName);
                List<string> list2 = readhtml2(eName);
                int j = list2.Count();
                List<string> list3 = new List<string>();
                for (int i = 0; i < list1.Count(); i++)
                {
                    list3.Add(list1[i]);
                    if (j > i)
                    {
                        list3.Add(list2[i]);
                    }
                }
                var star = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head><meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml; charset=utf-8\" /><title>标准名称</title><link href=\"zhongwenBZ_styles.css\" type=\"text/css\" rel=\"stylesheet\"/><link href=\"yingwenBZ_styles.css\" type=\"text/css\" rel=\"stylesheet\"/></head><body style = \"pagewidth:595.3pt;pageheight:841.9pt;docgridtype:lines;docgridlinepitch:15.6pt;\" >";
                var end = "</body></html>";
                list3.Insert(0, star);
                list3.Add(end);
                //var all = @"D:\doc\my\GB\BZ02.docx";
                if (File.Exists(showHtml))
                    File.Delete(showHtml);
                File.AppendAllLines(showHtml, list3);

                return "OK";
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        private static List<string> readhtml2(string FilePath)
        {

            List<string> list = new List<string>();
            StreamReader sr = new StreamReader(FilePath, Encoding.UTF8);

            string HtmlCode = sr.ReadToEnd();
            //xml convert
            //byte[] bytes = System.Text.Encoding.Default.GetBytes(HtmlCode);
            XHtmlTools xmlt = new XHtmlTools();
            HtmlCode = HtmlCode.Replace("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">", "");//&nbsp;
            XmlDocument xmld = xmlt.GetXmlDocument(HtmlCode);

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
                                   || (thrNode.ChildNodes[k].ChildNodes.Count == 1 && thrNode.ChildNodes[k].LastChild.InnerXml.Trim() == "")
                                   || thrNode.ChildNodes[k].Name.ToLower() == "br"
                                   || (thrNode.ChildNodes[k].OuterXml.Contains("class=\"一级条标题\"") && thrNode.ChildNodes[k].ChildNodes.Count == 1 && thrNode.ChildNodes[k].LastChild.InnerText.Trim() == "")
                                    )
                                {
                                    continue;
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
                                list.Add(thrNode.ChildNodes[k].OuterXml);
                            }

                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 读取目录 新方法
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static string readhtmlCatalogue(string FilePath)
        {
            string re = "";
            List<string> list = new List<string>();
            StreamReader sr = new StreamReader(FilePath, Encoding.UTF8);

            string HtmlCode = sr.ReadToEnd();
            //xml convert
            //byte[] bytes = System.Text.Encoding.Default.GetBytes(HtmlCode);
            XHtmlTools xmlt = new XHtmlTools();
            HtmlCode = HtmlCode.Replace("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">", "");//&nbsp;
            XmlDocument xmld = xmlt.GetXmlDocument(HtmlCode);

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



                                //  if (thrNode.ChildNodes[k].OuterXml.Contains("Evaluation Warning")
                                //     || thrNode.ChildNodes[k].OuterXml.Contains("标准书眉")
                                //     || (thrNode.ChildNodes[k].ChildNodes.Count == 1 && thrNode.ChildNodes[k].LastChild.InnerXml.Trim() == "")
                                //     || thrNode.ChildNodes[k].Name.ToLower() == "br"
                                //     || (thrNode.ChildNodes[k].OuterXml.Contains("class=\"一级条标题\"") && thrNode.ChildNodes[k].ChildNodes.Count == 1 && thrNode.ChildNodes[k].LastChild.InnerText.Trim() == "")
                                //      )
                                //  {
                                //      continue;
                                //  }
                                //  if (thrNode.ChildNodes[k].OuterXml.Contains("class=\"一级条标题\"") && thrNode.ChildNodes[k].LastChild.InnerText.Trim() == "")
                                //  {
                                //      l++;
                                //      thrNode.ChildNodes[k].LastChild.InnerText = "3." + l.ToString();
                                //      // continue;
                                //  }
                                //  if ((thrNode.ChildNodes[k].Name.ToLower() == "div" && thrNode.ChildNodes[k].FirstChild == null) ||
                                //(thrNode.ChildNodes[k].Name.ToLower() == "div" && thrNode.ChildNodes[k].FirstChild != null && thrNode.ChildNodes[k].FirstChild.Name.ToLower() != "table"))
                                //  {
                                //      continue;
                                //  } 
                                if (thrNode.ChildNodes[k].OuterXml.Contains("class=\"TOC-1\"") ||
                                thrNode.ChildNodes[k].OuterXml.Contains("class=\"TOC-2\"") ||
                                thrNode.ChildNodes[k].OuterXml.Contains("class=\"TOC-3\"") ||
                                    thrNode.ChildNodes[k].OuterXml.Contains("class=\"TOC-4\"") ||
                                thrNode.ChildNodes[k].OuterXml.Contains("class=\"TOC-5\"") ||
                                thrNode.ChildNodes[k].OuterXml.Contains("class=\"TOC-6\"") ||
                                thrNode.ChildNodes[k].OuterXml.Contains("class=\"TOC-7\"") ||
                                thrNode.ChildNodes[k].OuterXml.Contains("class=\"TOC-8\"") ||
                                thrNode.ChildNodes[k].OuterXml.Contains("class=\"TOC-9\"") ||
                                thrNode.ChildNodes[k].OuterXml.Contains("class=\"TOC-10\""))
                                {
                                    re += thrNode.ChildNodes[k].OuterXml;

                                }
                            }

                        }
                    }
                }
            }
            return re;
        }

        /// <summary>
        /// 读取目录 新方法
        /// </summary>
        /// <param name="htmlCode"></param>
        /// <returns></returns>
        public static string ReadXmlByCode(string htmlCode,out string content)
        {
            content = htmlCode;
            string reStr = "";
            List<string> re = new List<string>();
            if (string.IsNullOrEmpty(htmlCode))
            {
                return reStr;
            }
            try
            { 
                XHtmlTools xmlt = new XHtmlTools();
                htmlCode = htmlCode.Replace("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">", "");//&nbsp;

                XmlDocument xmld = xmlt.GetXmlDocument(htmlCode); 
                //获取当前XML文档的根 一级
                XmlNode oNode = xmld.DocumentElement;
                //获取根节点的所有子节点列表  
                XmlNodeList oList = oNode.ChildNodes; 
                //标记当前节点
                XmlNode secNode;  
                for (int i = 0; i < oList.Count; i++)
                {
                    secNode = oList[i];
                    DD(secNode , re); 
                }
                foreach (var item in re)
                {
                    string tempStr = item;
                    //都默认生成了xmlns="http://www.w3.org/1999/xhtml"
                    tempStr = tempStr.Replace(" xmlns=\"http://www.w3.org/1999/xhtml\"", "");
                    content = content.Replace(tempStr, "");
                }  
                StringBuilder XMLHEAD = new StringBuilder();
                XMLHEAD.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");//<?xml version="1.0" encoding="utf-8" ?> 
                XMLHEAD.Append("<!DOCTYPE ARTICLE[");
                XMLHEAD.Append("<!ENTITY nbsp \" \"><!ENTITY iexcl \"¡\"><!ENTITY cent \"¢\"><!ENTITY pound \"£\"><!ENTITY curren \"¤\"><!ENTITY yen \"¥\">");
                XMLHEAD.Append("<!ENTITY brvbar \"¦\"><!ENTITY sect \"§\"><!ENTITY uml \"¨\"><!ENTITY copy \"©\"><!ENTITY ordf \"ª\"><!ENTITY laquo \"«\">");
                XMLHEAD.Append("<!ENTITY not \"¬\"><!ENTITY shy \"-\"><!ENTITY reg \"®\"><!ENTITY macr \"¯\"><!ENTITY deg \"°\"><!ENTITY plusmn \"±\">");
                XMLHEAD.Append("<!ENTITY sup2 \"²\"><!ENTITY sup3 \"³\"><!ENTITY acute \"´\"><!ENTITY micro \"µ\"><!ENTITY para \"¶\"><!ENTITY middot \"·\">");
                XMLHEAD.Append("<!ENTITY cedil \"¸\"><!ENTITY sup1 \"¹\"><!ENTITY ordm \"º\"><!ENTITY raquo \"»\"><!ENTITY frac14 \"¼\"><!ENTITY frac12 \"½\">");
                XMLHEAD.Append("<!ENTITY frac34 \"¾\"><!ENTITY iquest \"¿\"><!ENTITY times \"×\"><!ENTITY divide \"÷\"><!ENTITY Agrave \"À\"><!ENTITY Aacute \"Á\">");
                XMLHEAD.Append("<!ENTITY Acirc \"Â\"><!ENTITY Atilde \"Ã\"><!ENTITY Auml \"Ä\"><!ENTITY Aring \"Å\"><!ENTITY AElig \"Æ\"><!ENTITY Ccedil \"Ç\">");
                XMLHEAD.Append("<!ENTITY Egrave \"È\"><!ENTITY Eacute \"É\"><!ENTITY Ecirc \"Ê\"><!ENTITY Euml \"Ë\"><!ENTITY Igrave \"Ì\"><!ENTITY Iacute \"Í\">");
                XMLHEAD.Append("<!ENTITY Icirc \"Î\"><!ENTITY Iuml \"Ï\"><!ENTITY ETH \"Ð\"><!ENTITY Ntilde \"Ñ\"><!ENTITY Ograve \"Ò\"><!ENTITY Oacute \"Ó\">");
                XMLHEAD.Append("<!ENTITY Ocirc \"Ô\"><!ENTITY Otilde \"Õ\"><!ENTITY Ouml \"Ö\"><!ENTITY Oslash \"Ø\"><!ENTITY Ugrave \"Ù\"><!ENTITY Uacute \"Ú\">");
                XMLHEAD.Append("<!ENTITY Ucirc \"Û\"><!ENTITY Uuml \"Ü\"><!ENTITY Yacute \"Ý\"><!ENTITY THORN \"Þ\"><!ENTITY szlig \"ß\"><!ENTITY agrave \"à\">");
                XMLHEAD.Append("<!ENTITY aacute \"á\"><!ENTITY acirc \"â\"><!ENTITY atilde \"ã\"><!ENTITY auml \"ä\"><!ENTITY aring \"å\"><!ENTITY aelig \"æ\">");
                XMLHEAD.Append("<!ENTITY ccedil \"ç\"><!ENTITY egrave \"è\"><!ENTITY eacute \"é\"><!ENTITY ecirc \"ê\"><!ENTITY euml \"ë\"><!ENTITY igrave \"ì\">");
                XMLHEAD.Append("<!ENTITY iacute \"í\"><!ENTITY icirc \"î\"><!ENTITY iuml \"ï\"><!ENTITY eth \"ð\"><!ENTITY ntilde \"ñ\"><!ENTITY ograve \"ò\">");
                XMLHEAD.Append("<!ENTITY oacute \"ó\"><!ENTITY ocirc \"ô\"><!ENTITY otilde \"õ\"><!ENTITY ouml \"ö\"><!ENTITY oslash \"ø\"><!ENTITY ugrave \"ù\">");
                XMLHEAD.Append("<!ENTITY uacute \"ú\"><!ENTITY ucirc \"û\"><!ENTITY uuml \"ü\"><!ENTITY yacute \"ý\"><!ENTITY thorn \"þ\"><!ENTITY yuml \"ÿ\">");
                XMLHEAD.Append("<!ENTITY lsquo \"‘\"><!ENTITY rsquo \"’\"><!ENTITY ldquo \"“\"><!ENTITY rdquo \"”\"><!ENTITY sbquo \"'\"><!ENTITY mdash \"—\">");
                XMLHEAD.Append("<!ENTITY Prime \"′\"><!ENTITY hellip \"…\">");
                XMLHEAD.Append("]>");
                content = content.Replace(XMLHEAD.ToString(), "");
            }
            catch (Exception e)
            {
                Common.Log("log", "err", "解析文件标题" + e.InnerException.Message);
            }
            foreach (var item in re)
            {
                if(item.IndexOf("目录")>0 || item.IndexOf("Content") > 0)
                {
                   
                }
                else
                {
                    reStr += item;
                }
                content = content.Replace(item, "");
            }
           
            return reStr;
        }
        
        public static void DD(XmlNode node ,   List<string> re)
        {
            foreach (XmlNode secNode in node.ChildNodes)
            {
                if (secNode.Name.ToLower().Equals("p"))
                {
                    //p里的目录 文字要去掉
                    if (secNode.Attributes["class"].Value != null)
                    {
                        if (secNode.ChildNodes.Count == 1)
                        {
                            if (secNode.ChildNodes[0].ChildNodes.Count == 1)
                            {
                                if (secNode.ChildNodes[0].ChildNodes[0].NodeType == XmlNodeType.Text)
                                {
                                    if (secNode.ChildNodes[0].ChildNodes[0].Value == "目录" ||
                                        secNode.ChildNodes[0].ChildNodes[0].Value == "Content")
                                    {
                                        re.Add(secNode.OuterXml);
                                    }

                                }
                            } 
                        } 
                    }
                        if (secNode.Attributes["class"].Value != null)
                    {
                        if (secNode.OuterXml.Contains("class=\"TOC-1\"") ||
                        secNode.OuterXml.Contains("class=\"TOC-2\"") ||
                        secNode.OuterXml.Contains("class=\"TOC-3\"") ||
                        secNode.OuterXml.Contains("class=\"TOC-4\"") ||
                        secNode.OuterXml.Contains("class=\"TOC-5\"") ||
                        secNode.OuterXml.Contains("class=\"TOC-6\"") ||
                        secNode.OuterXml.Contains("class=\"TOC-7\"") ||
                        secNode.OuterXml.Contains("class=\"TOC-8\"") ||
                        secNode.OuterXml.Contains("class=\"TOC-9\"") ||
                        secNode.OuterXml.Contains("class=\"TOC-10\""))
                        {
                            re.Add(secNode.OuterXml); 
                            //node.RemoveChild(secNode) ;
                        }
                    }
                }
                else
                { 
                    if (secNode.ChildNodes.Count >= 1)
                    { 
                        DD(secNode, re );
                    } 
                } 
            }
        }

        /// <summary>
        /// 将XmlDocument转化为string
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static string ConvertXmlToString(XmlDocument xmlDoc)
        {
            MemoryStream stream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(stream, null);
            writer.Formatting = Formatting.Indented;
            xmlDoc.Save(writer);
            StreamReader sr = new StreamReader(stream, System.Text.Encoding.UTF8);
            stream.Position = 0;
            string xmlString = sr.ReadToEnd();
            sr.Close();
            stream.Close();
            return xmlString;
        } 
        public static string NewReadXmlByCode(string htmlCode, out string content)
        {
            content = htmlCode;
            string re = "";
            if (string.IsNullOrEmpty(htmlCode))
            {
                return re;
            }
            try
            { 
                XHtmlTools xmlt = new XHtmlTools();
                htmlCode = htmlCode.Replace("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\">", "");//&nbsp;

                XmlDocument xmld = xmlt.GetXmlDocument(htmlCode); 
                XmlNodeList xnl = xmld.SelectSingleNode("p").ChildNodes;
                foreach (XmlNode xn in xnl)
                {
                    XmlElement xe = (XmlElement)xn;
                    if (xe.Attributes["class"].Value != null&& (
                        xe.OuterXml.Contains("class=\"TOC-1\"") ||
                        xe.OuterXml.Contains("class=\"TOC-2\"") ||
                        xe.OuterXml.Contains("class=\"TOC-3\"") ||
                        xe.OuterXml.Contains("class=\"TOC-4\"") ||
                        xe.OuterXml.Contains("class=\"TOC-5\"") ||
                        xe.OuterXml.Contains("class=\"TOC-6\"") ||
                        xe.OuterXml.Contains("class=\"TOC-7\"") ||
                        xe.OuterXml.Contains("class=\"TOC-8\"") ||
                        xe.OuterXml.Contains("class=\"TOC-9\"") ||
                        xe.OuterXml.Contains("class=\"TOC-10\"")))
                    {
                        re += xe.OuterXml;
                        xe.RemoveAll();//删除该节点的全部内容
                    }
                } 
                content = xmld.ToString();
            }
            catch (Exception e)
            {
                Common.Log("log", "err", "解析文件标题" + e.InnerException.Message);
            }
           
            return re;
        }


        /// <summary>
        /// 遍历所有节点的type、name、Attributes
        /// </summary>
        /// <param name="childnodelist"></param>
        static void PrintChildNodes(XmlNodeList childnodelist)
        {
            try
            {
                foreach (XmlNode node in childnodelist)
                {
                    //(node.NodeType 是Text时，即是最内层 即innertext值，node.Attributes为null。
                    if (node.NodeType == XmlNodeType.Text)
                    {
                        Console.WriteLine("NodeType:" + node.NodeType + "\t" + node.Name + "\t:" + node.Value);
                        continue;
                    }
                    Console.WriteLine("==========node.Name:" + node.Name + "===========");
                    foreach (XmlAttribute atr in node.Attributes)
                    {
                        Console.WriteLine("NodeType:" + atr.NodeType + "\t" + atr.Name + "\t:" + atr.Value);
                    }
                    if (node.ChildNodes.Count > 0)
                    {
                        PrintChildNodes(node.ChildNodes);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }      
    }
}