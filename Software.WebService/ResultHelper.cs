
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NFine.Code;

namespace Software.WebService
{
    public static class ResultHelper
    {
        private static List<ResultDataItem> ResultDataItemList
        {
            get
            {
                List<ResultDataItem> list = CacheFactory.Cache().GetCache<List<ResultDataItem>>("ResultDataItem");
                if (list != null)
                {

                }
                else
                {
                    list = LoadXml();
                    CacheFactory.Cache().WriteCache(list, "ResultDataItem", DateTime.Now.AddDays(1));
                }
                return list;
            }
        }

        /// <summary>
        /// 获取返回的提示 
        /// </summary>
        /// <param name="language">1 ch 2 en</param>
        /// <param name="apiName">接口名称</param>
        /// <param name="resultCode">对应的编码</param>
        /// <returns></returns>
        public static string GetMessage(int language, string apiName, string resultCode)
        {
            List<ResultDataItem> list = ResultDataItemList;
            foreach (var item in list)
            {
                if (item.ApiName == apiName)
                {
                    foreach (var i in item.ItemList)
                    {
                        if (resultCode == i.ResultCode)
                        {
                            if (language == 1)
                            {
                                return i.ChResultMessage;
                            }
                            else if (language == 2)
                            {
                                return i.EnResultMessage;
                            }
                        }

                    }
                }
            }
            return "";
        }
        private static List<ResultDataItem> LoadXml()
        {
            StreamReader sr = new StreamReader(System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "data.xml", Encoding.UTF8);
            string xmlCode = sr.ReadToEnd();

            List<ResultDataItem> resultDataItems = new List<ResultDataItem>();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.XmlResolver = null;
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                xmlDoc.LoadXml(xmlCode);
                //获取当前XML文档的根 一级
                XmlNode oNode = xmlDoc.DocumentElement;

                //获取根节点的所有子节点列表 
                XmlNodeList oList = oNode.ChildNodes;

                //标记当前节点
                XmlNode secNode;
                XmlNode thrNode;
                XmlNode fourNode;

                for (int i = 0; i < oList.Count; i++)
                {
                    secNode = oList[i];
                    if (secNode.Name.Equals("ResultDataItem"))
                    {
                        ResultDataItem resultDataItem = new ResultDataItem();
                        List<ResultItem> resultItems = new List<ResultItem>();
                        for (int j = 0; j < secNode.ChildNodes.Count; j++)
                        {
                            thrNode = secNode.ChildNodes[j];
                            ResultItem resultItem = new ResultItem();

                            foreach (XmlNode item in thrNode.ChildNodes)
                            {
                                fourNode = item;
                                if (fourNode.Name.Equals("ResultCode"))
                                {
                                    resultItem.ResultCode = fourNode.InnerText;
                                }
                                if (fourNode.Name.Equals("ChResultMessage"))
                                {
                                    resultItem.ChResultMessage = fourNode.InnerText;
                                }
                                if (fourNode.Name.Equals("EnResultMessage"))
                                {
                                    resultItem.EnResultMessage = fourNode.InnerText;
                                }
                            } 
                            resultItems.Add(resultItem);
                        }
                        resultDataItem.ItemList = resultItems;
                        resultDataItem.ApiName = secNode.Attributes["ApiName"].Value;
                        resultDataItems.Add(resultDataItem);
                    }
                }
            }
            catch (XmlException ex)
            {
                return null;
            } 
            return resultDataItems;
        }
    }

    public class ResultData
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// 返回编码
        /// </summary>
        public int ResultCode { get; set; }
        /// <summary>
        /// 返回信息
        /// </summary>
        public string ResultMessage { get; set; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 返回日期
        /// </summary>
        public DateTime Time { get { return DateTime.Now; } }
    }
    public class ResultDataItem
    {
        public string ApiName { get; set; }
        public List<ResultItem> ItemList;
    }
    public class ResultItem
    {
        public string ResultCode { get; set; }
        public string ChResultMessage { get; set; }
        public string EnResultMessage { get; set; }
    }
}