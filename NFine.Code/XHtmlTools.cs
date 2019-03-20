using System;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Mozilla.NUniversalCharDet;
using System.Xml.Serialization;

namespace NFine.Code
{
	public class XHtmlTools
	{
		private const string RegBody = @"<body[\s\S]*?>(?<body>[\s\S]*)</body>";
		public XmlDocument GetXmlDocument(string html)
		{
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

			if (html == null)
				return null;

			try
			{
				XmlDocument xmlDoc = new XmlDocument(); 
                xmlDoc.XmlResolver = null;
				var strxml = string.Format("{0}{1}", XMLHEAD.ToString(), html);

                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);


                xmlDoc.LoadXml(strxml);

				return xmlDoc;
			}
			catch (XmlException ex)
			{
				return null;
			}
		}

   
        /// <summary>
        /// 获取xml文档
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public XmlDocument GetXmlDocument(byte[] html)
		{
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

			if (html == null)
				return null;

			string xml = Convert(html);

			if (string.IsNullOrEmpty(xml))
				return null;

			try
			{
				XmlDocument xmlDoc = new XmlDocument();
				xmlDoc.XmlResolver = null;
				var strxml = string.Format("{0}{1}", XMLHEAD.ToString(), xml);
				xmlDoc.LoadXml(strxml);

				return xmlDoc;
			}
			catch (XmlException ex)
			{
				return null;
			}
		}


		/// <summary>
		/// 将html转为xml
		/// </summary>
		/// <param name="html"></param>
		/// <returns></returns>
		public string Convert(byte[] html)
		{
			string xml = string.Empty;
			try
			{
				using (HtmlReader reader = new HtmlReader(GetString(html)))
				{
					StringBuilder sb = new StringBuilder();

					using (HtmlWriter writer = new HtmlWriter(sb))
					{
						while (!reader.EOF)
						{
							writer.WriteNode(reader, true);
						}
					}

					xml = sb.ToString();
				}
			}
			catch (Exception)
			{
			}

			Match match = Regex.Match(xml, RegBody, RegexOptions.IgnoreCase);
			if (match.Success)
			{
				xml = match.Value;
			}

			if (string.IsNullOrEmpty(xml))
			{
				xml = "<body></body>";
			}

			return xml;
		}


		/// <summary>
		/// 解析编码并获得字符串
		/// </summary>
		/// <param name="buffer"></param>
		/// <returns></returns>
		public string GetString(byte[] buffer)
		{
			string result = string.Empty;

			if (buffer == null)
				return result;

			using (MemoryStream msTemp = new MemoryStream(buffer))
			{
				if (msTemp.Length > 0)
				{
					msTemp.Seek(0, SeekOrigin.Begin);
					int DetLen = 0;
					byte[] DetectBuff = new byte[4096];

					UniversalDetector det = new UniversalDetector(null);
					while ((DetLen = msTemp.Read(DetectBuff, 0, DetectBuff.Length)) > 0 && !det.IsDone())
					{
						det.HandleData(DetectBuff, 0, DetectBuff.Length);
					}
					det.DataEnd();
					if (det.GetDetectedCharset() != null)
					{
						try
						{
							result = System.Text.Encoding.GetEncoding(det.GetDetectedCharset()).GetString(buffer);
						}
						catch (ArgumentException)
						{
						}
					}
				}
			}

			return result;
		}

        public XmlDocument GetXml(string url)
        { 
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.XmlResolver = null; 
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty); 
                xmlDoc.LoadXml(url);

                return xmlDoc;
            }
            catch (XmlException ex)
            {
                return null;
            }
        }

    }

	public class HtmlReader : Sgml.SgmlReader
	{
		public HtmlReader(TextReader reader)
			: base()
		{
			base.InputStream = reader;
			base.DocType = "HTML";
		}
		public HtmlReader(string content)
			: base()
		{
			base.InputStream = new StringReader(System.Web.HttpUtility.HtmlDecode(content));
			base.DocType = "HTML";
		}

		public override bool Read()
		{
			bool status = false;
			try
			{
				status = base.Read();
				if (status)
				{
					//if (base.NodeType == XmlNodeType.Element)
					//{
					//    // Got a node with prefix. This must be one of those "<o:p>" or something else.
					//    // Skip this node entirely. We want prefix less nodes so that the resultant XML 
					//    // requires not namespace.
					//    if (base.Name.IndexOf(':') > -1)
					//        base.Skip();
					//}
					//else if (base.NodeType == XmlNodeType.CDATA || base.NodeType == XmlNodeType.Comment)
					//{
					//    base.Skip();
					//}

					if (base.NodeType == XmlNodeType.Element
						&& (string.Compare(base.Name, "head", true) == 0
							|| string.Compare(base.Name, "script", true) == 0))
					{
						base.Skip();
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return status;
		}
	}

	public class HtmlWriter : XmlTextWriter
	{
		private char[] chArrFilter = new char[] { '\'', '=', '?', '\"', '.', ';', '：', ')', '(', ' ', '　' };

		public HtmlWriter(TextWriter writer)
			: base(writer)
		{
		}

		public HtmlWriter(StringBuilder builder)
			: base(new StringWriter(builder))
		{
		}

		public HtmlWriter(Stream stream, Encoding enc)
			: base(stream, enc)
		{

		}

		public override void WriteCData(string text)
		{
			// base.WriteCData(text);
		}

		public override void WriteComment(string text)
		{

		}

		public override void WriteWhitespace(string ws)
		{
			if (ws.IndexOf("\r\n") > -1 || ws.IndexOf("\t") > -1)
			{
				return;
			}

			if (ws != " ")
			{
				// 处理空白字符
				base.WriteWhitespace(ws);
			}
		}

		/// <summary>
		/// This method is overriden to filter out tags which are not allowed
		/// </summary>
		public override void WriteStartElement(string prefix, string localName, string ns)
		{
			if (localName != "")
			{
				int index = localName.LastIndexOf(':');

				if (index > -1)
				{
					// 防止带有前缀
					localName = localName.Substring(index + 1);
				}

				localName = string.Join("", localName.Split(chArrFilter)).ToLower();

				base.WriteStartElement("", localName, "");
			}
		}

		/// <summary>
		/// This method is overriden to filter out attributes which are not allowed
		/// </summary>
		public override void WriteAttributes(XmlReader reader, bool defattr)
		{
			if ((reader.NodeType == XmlNodeType.Element) || (reader.NodeType == XmlNodeType.XmlDeclaration))
			{
				if (reader.MoveToFirstAttribute())
				{
					this.WriteAttributes(reader, defattr);
					reader.MoveToElement();
				}
			}
			else if (reader.NodeType == XmlNodeType.Attribute)
			{
				string localName = "";
				string value = "";
				do
				{
					localName = reader.LocalName.ToLower();

					// 单过滤
					if (localName != "xml:space" && (localName.LastIndexOf(':') > -1 || localName.StartsWith("xml")))
					{
						// 防止带有前缀
						continue;
					}

					localName = string.Join("", localName.Split(chArrFilter));

					if (localName == "")
					{
						continue;
					}

					this.WriteStartAttribute("", localName, "");

					while (reader.ReadAttributeValue())
					{
						// if (reader.NodeType == XmlNodeType.EntityReference)
						// {
						//     this.WriteEntityRef(reader.Name);
						//     continue;
						// }

						value = reader.Value;

						if (value == "")
						{
							continue;
						}

						this.WriteString(value);

						// this.WriteRawString(reader.Value);
						// this.WriteAttributeString(localName, reader.Value);
					}

					this.WriteEndAttribute();

					// ===========================================
					//string attributeLocalName = reader.LocalName;
					//while (reader.ReadAttributeValue())
					//{
					//    string str =  reader.Name;
					//}

					//string strValue = reader.Value;
					//attributeLocalName = reader.Name;

					//// 过滤无效的属性
					//if (attributeLocalName == "" || strValue == "")
					//{
					//    attributeLocalName = attributeLocalName.TrimStart(new char[] { '\'', '=', '?', '\"', '.' }).ToLower();
					//    this.WriteAttributeString(attributeLocalName, strValue);
					//}

				} while (reader.MoveToNextAttribute());
			}
		}

	}
}
