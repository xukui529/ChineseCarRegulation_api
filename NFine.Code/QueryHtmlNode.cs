using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFine.Code
{
    public class QueryHtmlNode
    {
        public static HtmlDocument GetHtmlDocByHtmlCode(string htmlCode)
        { 
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(htmlCode);
               
                return doc;
            }
            catch (Exception ex)
            {
                return null;
            } 
        }

        //public static string GetList()
        //{
            
        //    //var trs = doc.DocumentNode.SelectNodes("//tr").Where(it => null != it.Attributes["bgColor"] && it.Attributes["bgColor"].Value.Equals("#FFFF99")).ToList();
        //    //foreach (var tr in trs)
        //    //{
        //    //    Console.WriteLine(new string('#', 50));
        //    //    #region --- 特定一些数据 ---
        //    //    //if (null != tr.SelectSingleNode("td/font"))
        //    //    //{
        //    //    //    //时间
        //    //    //    Console.WriteLine(tr.SelectSingleNode("td/font").InnerText);
        //    //    //}
        //    //    //if (null != tr.SelectSingleNode("td[2]/a[@href]"))
        //    //    //{
        //    //    //    //链接
        //    //    //    Console.WriteLine(tr.SelectSingleNode("td[2]/a[@href]").Attributes["href"].Value);
        //    //    //} 
        //    //    #endregion
        //    //    #region --- 所有 ---
        //    //    //foreach (var td in tr.SelectNodes("td"))
        //    //    //{
        //    //    //    Console.WriteLine(td.InnerText);

        //    //    //    foreach (var a in tr.SelectNodes("td/a[@href]"))
        //    //    //    {
        //    //    //        Console.WriteLine(a.InnerText + "---------" + a.Attributes["href"].Value);
        //    //    //    }
        //    //    //}
        //    //    #endregion
        //    //    #region --- 按列取数据 ---
        //    //    var tds = tr.SelectNodes("td");
        //    //    for (int i = 0; i < tds.Count; i++)
        //    //    {
        //    //        switch (i)
        //    //        {
        //    //            case 0:
        //    //                Console.WriteLine("服务器名称:" + tds[i].InnerText);
        //    //                if (null != tds[i].SelectSingleNode("a[@href]"))
        //    //                {
        //    //                    Console.WriteLine("URL:" + tds[i].SelectSingleNode("a[@href]").Attributes["href"].Value);
        //    //                }
        //    //                break;
        //    //            case 2:
        //    //                Console.WriteLine("开放时间:" + tds[i].InnerText);
        //    //                break;
        //    //            case 4:
        //    //                Console.WriteLine("版本介绍:" + tds[i].InnerText);
        //    //                break;
        //    //            case 5:
        //    //                Console.WriteLine("QQ:" + tds[i].InnerText);
        //    //                break;
        //    //            default:
        //    //                break;
        //    //        }
        //    //    }

        //    //    #endregion
        //    //}
        //    //Console.WriteLine(trs.Count());
        //}
    }
}
