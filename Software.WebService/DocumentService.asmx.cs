using NFine.Application.AuxiliaryManage;
using NFine.Application.ContentManage;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.Entity.ContentManage;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace Software.WebService
{
    /// <summary>
    /// DocumentService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://47.92.160.129:8080/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class DocumentService : System.Web.Services.WebService
    {
        private DocumentApp documentApp = new DocumentApp();
        private DocumentContentApp documentContentApp = new DocumentContentApp();
        private ItemsDetailApp itemsDetailApp = new ItemsDetailApp();
        private MyCollectApp myCollectApp = new MyCollectApp();
        private MemberApp memberApp = new MemberApp();
        private DownloadInfoApp downloadInfoApp = new DownloadInfoApp();
        #region old 
        [WebMethod]
        public void GetDataList(int page, int rows, string keyword, int index, int type)
        {
            List<DocumentModel> entityList = documentApp.GetDataListApi(page, rows, keyword, index, type);
            ChangeJson(entityList);
        }
        [WebMethod]
        public void GetDataListByCondition(int page, int rows, string chinesetitle, string englishtitle, string domain, string filestate, string acquisitionstandard, int state, int type)
        {
            List<DocumentModel> entityList = documentApp.GetDataListByCondition(page, rows, chinesetitle, englishtitle, domain, filestate, acquisitionstandard, state, type);
            ChangeJson(entityList);
        }
        [WebMethod]
        public void GetEntityById(string keyValue)
        {
            var data = documentApp.GetForm(keyValue);
            ChangeJson(data);
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void ChangeJson(object obj)
        {
            Context.Response.Clear();
            Context.Response.Charset = "UTF-8"; //设置字符集类型  
            Context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            Context.Response.ContentType = "application/json";
            Jayrock.Json.JsonTextWriter writer = new Jayrock.Json.JsonTextWriter();
            Jayrock.Json.Conversion.JsonConvert.Export(obj, writer);
            Context.Response.Write(writer);
            Context.Response.Flush();
            Context.Response.End();
        }

        [WebMethod]
        public void GetWordContent(string keyword, int type)
        {
            string strChinese = "";
            try
            {
                var data = documentApp.GetForm(keyword);
                string path = data.F_HtmlUrl;
                switch (type)
                {
                    case 1:
                        path = data.F_HtmlUrl;
                        break;

                    case 2:
                        path = data.F_ChineseUrl;
                        break;
                    case 3:
                        path = data.F_EnglishUrl;
                        break;
                    case 4:
                        path = data.F_TotalUrl;
                        break;
                    default:
                        break;
                }
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
                StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                strChinese = sr.ReadToEnd();
                //string filename = Path.GetFileNameWithoutExtension(path);
                //string imgSrc = "<img src=\"\\Temp\\Word\\" + filename + "\\";
                //strChinese = strChinese.Replace("<img src=\"", imgSrc);
                fs.Flush();
                sr.Close();
                fs.Close();

            }
            catch (Exception)
            {


            }
            ChangeJson(strChinese);


        }
        #endregion

        #region new

        #region 首页 
        /// <summary>
        /// 首页标准法规动态、最新翻译 取最近的前n条
        /// </summary>  
        /// <param name="language">1 汉语 2 英语</param> 
        /// <param name="topNum">前几条</param> 
        /// <returns></returns>
        [WebMethod]
        public void GetIndexByTopList(int language, int topNum)
        {
            int o = 0;
            //  类型 1、标准 2、法规3、标准法规动态（资讯） 
            List<DocumentEntity> iList = documentApp.GetList(0, topNum, (int)NFine.Code.Enum.DocumentType.资讯, out o);   //标准法规动态==3资讯 
            List<DocumentEntity> nList = documentApp.GetNewestList(0, topNum, out o);  //最新翻译==最新更新 是不包括资讯的 

            List<object> informationList = new List<object>();
            List<object> newestList = new List<object>();
            foreach (var item in iList)
            {
                informationList.Add(new
                {
                    Id = item.F_Id,
                    Type = item.F_Type,
                    Title = (language == 1 ? item.F_ChineseTitle : item.F_EnglishTitle),
                    CreatorTime = item.F_CreatorTime
                });
            }
            foreach (var item in nList)
            {
                newestList.Add(new
                {
                    Id = item.F_Id,
                    Type = item.F_Type,
                    Title = (language == 1 ? item.F_ChineseTitle : item.F_EnglishTitle),
                    CreatorTime = item.F_CreatorTime
                });
            }
            ChangeJson(new { informationList, newestList });
        }
        #endregion

        #region 标准检索 法规检索
        /// <summary>
        /// 标准检索条件
        /// </summary> 
        [WebMethod]
        public void CriterionType(int language = 1)
        {
            var itemList = itemsDetailApp.GetItemAndItemDetailAll();
            ChangeJson(new
            {
                documentStateList = GetItemDetailList(itemList.Where(x => x.EnCode == "DocumentState").ToList(), language),//DocumentState 文档状态
                documentDomainList = GetItemDetailList(itemList.Where(x => x.EnCode == "DocumentDomain").ToList(), language),//DocumentDomain 文档领域
                caiBiaoList = GetItemDetailList(itemList.Where(x => x.EnCode == "CaiBiao").ToList(), language) //CaiBiao 采标 
            });
        }
        /// <summary>
        /// 法规检索条件
        /// </summary>
        /// <param name="fileState">状态</param>
        /// <param name="publisher">发布方</param>
        /// <param name="direction">方向</param>
        [WebMethod]
        public void RegulationType(int language = 1)
        {
            var itemList = itemsDetailApp.GetItemAndItemDetailAll();
            ChangeJson(new
            {
                documentStateList = GetItemDetailList(itemList.Where(x => x.EnCode == "DocumentState").ToList(), language),//DocumentState 文档状态
                publisherList = GetItemDetailList(itemList.Where(x => x.EnCode == "Publisher").ToList(), language),//Publisher 发布方
                directionList = GetItemDetailList(itemList.Where(x => x.EnCode == "Direction").ToList(), language) //Direction 方向
            });
        }
        /// <summary>
        /// 标准检索
        /// </summary>
        /// <param name="fileState">状态</param>
        /// <param name="domain">领域</param>
        /// <param name="acquisitionStandard">采标</param>
        /// <param name="type"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        [WebMethod]
        public void SearchCriterionByType(string fileState, string domain, string acquisitionStandard, int page, int rows, int language)
        {
            int total = 0;
            var documentList = documentApp.SearchByType(fileState, domain, acquisitionStandard, "", "",
               (int)NFine.Code.Enum.DocumentType.标准, page, rows, out total);

            var itemList = itemsDetailApp.GetItemAndItemDetailAll();
            ChangeJson(new
            {
                total,
                documentList = GetDocList(documentList, language),
                documentStateList = GetItemDetailList(itemList.Where(x => x.EnCode == "DocumentState").ToList(), language),//DocumentState 文档状态
                documentDomainList = GetItemDetailList(itemList.Where(x => x.EnCode == "DocumentDomain").ToList(), language),//DocumentDomain 文档领域
                caiBiaoList = GetItemDetailList(itemList.Where(x => x.EnCode == "CaiBiao").ToList(), language) //CaiBiao 采标 
            });
        }
        /// <summary>
        /// 法规检索
        /// </summary>
        /// <param name="fileState">状态</param>
        /// <param name="publisher">发布方</param>
        /// <param name="direction">方向</param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        [WebMethod]
        public void SearchRegulationByType(string fileState, string publisher, string direction, int page, int rows, int language)
        {
            int total = 0;
            var documentList = documentApp.SearchByType(fileState, "", "", publisher, direction, (int)NFine.Code.Enum.DocumentType.法规, page, rows, out total);

            var itemList = itemsDetailApp.GetItemAndItemDetailAll();
            ChangeJson(new
            {
                total,
                documentList = GetDocList(documentList, language),
                documentStateList = GetItemDetailList(itemList.Where(x => x.EnCode == "DocumentState").ToList(), language),//DocumentState 文档状态
                publisherList = GetItemDetailList(itemList.Where(x => x.EnCode == "Publisher").ToList(), language),//Publisher 发布方
                directionList = GetItemDetailList(itemList.Where(x => x.EnCode == "Direction").ToList(), language) //Direction 方向
            });
        }
        #endregion

        #region 标准法规动态
        /// <summary>
        /// 标准法规动态  ==3资讯 
        /// </summary> 
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="language"></param>
        [WebMethod]
        public void SearchInformationList(int page, int rows, int language)
        {
            int total = 0;
            List<DocumentEntity> dList = documentApp.GetList(page, rows, (int)NFine.Code.Enum.DocumentType.资讯, out total);
            List<object> documentList = new List<object>();
            foreach (var item in dList)
            {
                documentList.Add(new
                {
                    Id = item.F_Id,
                    Title = (language == 1 ? item.F_ChineseTitle : item.F_EnglishTitle),
                    Centent = (language == 1 ? item.F_InfoContent : item.F_InfoContent),
                    IsPDF = (!string.IsNullOrEmpty(item.F_TotalUrl)),
                    CreatorTime = item.F_CreatorTime
                });
            }
            ChangeJson(new
            {
                total,
                documentList
            });
        }
        #endregion

        #region 最新更新  是不包括资讯的
        /// <summary>
        /// 标准法规动态  ==3资讯 
        /// </summary> 
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="language"></param>
        [WebMethod]
        public void SearchNewestList(int page, int rows, int language)
        {
            int total = 0;
            List<DocumentEntity> dList = documentApp.GetNewestList(page, rows, out total);
            List<object> documentList = new List<object>();
            foreach (var item in dList)
            {
                documentList.Add(new
                {
                    Id = item.F_Id,
                    Title = (language == 1 ? item.F_ChineseTitle : item.F_EnglishTitle),
                    CreatorTime = item.F_CreatorTime,
                    DocShowType = GetDocShowType(item)
                });
            }
            ChangeJson(new
            {
                total,
                documentList
            });
        }
        #endregion

        #region 首页标题搜索结果页
        /// <summary>
        /// 首页标题搜索结果页
        /// </summary> 
        /// <param name="keyword"></param> 
        /// <param name="language">1 汉语 2 英语</param> 
        /// <returns></returns>
        [WebMethod]
        public void SearchForIndexByLabelOrTitle(int page, int rows, string keyword, int language)
        {
            //逻辑：先判断标签里是否有搜索的关键字，这里要精确判断
            //然后 在判断标题里是否包含输入的关键字，这个用模糊判断 把两个结果集 合到一起  
            List<DocumentModel> list;
            if (string.IsNullOrEmpty(keyword))
            {
                list = documentApp.SearchForIndexByContent();
            }
            else
            {
                list = documentApp.SearchForIndexByContent(keyword);
            }

            List<object> documentList = new List<object>();
            List<ItemsDetailEntity> itemListAll = itemsDetailApp.GetItemAll();


            foreach (var item in list)
            {
                string fileStateValue = "";
                if (!string.IsNullOrEmpty(item.F_FileState))
                {
                    var itemTemp = itemListAll.Where(x => x.F_ItemCode == item.F_FileState).FirstOrDefault();
                    fileStateValue = (language == 1 ? itemTemp.F_ItemName : itemTemp.F_ItemEnName);
                }
                documentList.Add(new
                {
                    Id = item.F_Id,
                    Type = item.F_Type,
                    Title = (language == 1 ? item.F_ChineseTitle : item.F_EnglishTitle),
                    FileState = fileStateValue,
                    Domain = item.F_Domain,
                    AcquisitionStandard = item.F_AcquisitionStandard,
                    Publisher = item.F_Publisher,
                    Direction = item.F_Direction,
                    ReleaseDate = item.F_ReleaseDate,
                    ImplementDate = item.F_ImplementDate,
                    DocShowType = GetDocShowType(item)
                });
            }
            int total = documentList.Count();
            var reList = documentList.Skip((page - 1) * rows).Take(rows).AsQueryable().ToList();
            ChangeJson(new
            {
                total,
                documentList = reList,
            });
        }
        /// <summary>
        /// 统计 首页查询标题
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="language"></param>
        [WebMethod]
        public void SearchForIndexByLabelOrTitleStat(string keyword, int language)
        {
            //要处理各个类别都有多少文章 string fileState,string domain, string acquisitionStandard,
            // string publisher, string direction 分别按这5个类别分组 

            List<DocumentModel> list;
            if (string.IsNullOrEmpty(keyword))
            {
                list = documentApp.SearchForIndexByContent();
            }
            else
            {
                list = documentApp.SearchForIndexByContent(keyword);
            }

            var typeGroupList = from c in list
                                group c by c.F_Type into g
                                select new
                                {
                                    Type = g.Key,
                                    Num = g.Count(),
                                };

            var fileStateGroupList = from c in list
                                     group c by c.F_FileState into g
                                     select new
                                     {
                                         Type = g.Key,
                                         Num = g.Count(),
                                     };
            var domainGroupList = from c in list
                                  group c by c.F_Domain into g
                                  select new
                                  {
                                      Type = g.Key,
                                      Num = g.Count(),
                                  };
            var acquisitionStandardGroupList = from c in list
                                               group c by c.F_AcquisitionStandard into g
                                               select new
                                               {
                                                   Type = g.Key,
                                                   Num = g.Count(),
                                               };
            var publisherGroupList = from c in list
                                     group c by c.F_Publisher into g
                                     select new
                                     {
                                         Type = g.Key,
                                         Num = g.Count(),
                                     };
            var directionGroupList = from c in list
                                     group c by c.F_Direction into g
                                     select new
                                     {
                                         Type = g.Key,
                                         Num = g.Count(),
                                     };
            var itemList = itemsDetailApp.GetItemAndItemDetailAll();


            ChangeJson(new
            {
                typeGroupList,
                fileStateGroupList,
                domainGroupList,
                acquisitionStandardGroupList,
                publisherGroupList,
                directionGroupList,
                documentStateList = GetItemDetailList(itemList.Where(x => x.EnCode == "DocumentState").ToList(), language),//DocumentState 文档状态
                documentDomainList = GetItemDetailList(itemList.Where(x => x.EnCode == "DocumentDomain").ToList(), language),//DocumentDomain 文档领域
                caiBiaoList = GetItemDetailList(itemList.Where(x => x.EnCode == "CaiBiao").ToList(), language), //CaiBiao 采标 
                publisherList = GetItemDetailList(itemList.Where(x => x.EnCode == "Publisher").ToList(), language),//Publisher 发布方
                directionList = GetItemDetailList(itemList.Where(x => x.EnCode == "Direction").ToList(), language) //Direction 方向
            });

        }

        /// <summary>
        /// 首页标题搜索结果页 二次查询 加类型
        /// </summary>
        /// <param name="type">1标准2法规3资讯</param>
        /// <param name="fileState"></param>
        /// <param name="domain"></param>
        /// <param name="acquisitionStandard"></param>
        /// <param name="publisher"></param>
        /// <param name="direction"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="keyword"></param>
        /// <param name="language"></param>
        [WebMethod]
        public void SearchForIndexByLabelOrTitleSecond(int type,string fileState, string domain, string acquisitionStandard, string publisher, string direction, int page, int rows, string keyword, int language)
        {
            //逻辑：先判断标签里是否有搜索的关键字，这里要精确判断
            //然后 在判断标题里是否包含输入的关键字，这个用模糊判断 把两个结果集 合到一起  
            List<DocumentModel> list;
            if (string.IsNullOrEmpty(keyword))
            {
                list = documentApp.SearchForIndexByContent(  type,   fileState,   domain,   acquisitionStandard,   publisher,   direction );
            }
            else
            {
                list = documentApp.SearchForIndexByContent(  type, fileState, domain, acquisitionStandard, publisher, direction,keyword);
            }

            List<object> documentList = new List<object>();
            List<ItemsDetailEntity> itemListAll = itemsDetailApp.GetItemAll();


            foreach (var item in list)
            {
                string fileStateValue = "";
                if (!string.IsNullOrEmpty(item.F_FileState))
                {
                    var itemTemp = itemListAll.Where(x => x.F_ItemCode == item.F_FileState).FirstOrDefault();
                    fileStateValue = (language == 1 ? itemTemp.F_ItemName : itemTemp.F_ItemEnName);
                }
                documentList.Add(new
                {
                    Id = item.F_Id,
                    Type = item.F_Type,
                    Title = (language == 1 ? item.F_ChineseTitle : item.F_EnglishTitle),
                    FileState = fileStateValue,
                    Domain = item.F_Domain,
                    AcquisitionStandard = item.F_AcquisitionStandard,
                    Publisher = item.F_Publisher,
                    Direction = item.F_Direction,
                    ReleaseDate = item.F_ReleaseDate,
                    ImplementDate = item.F_ImplementDate,
                    DocShowType = GetDocShowType(item)
                });
            }
            int total = documentList.Count();
            var reList = documentList.Skip((page - 1) * rows).Take(rows).AsQueryable().ToList();
            ChangeJson(new
            {
                total,
                documentList = reList,
            });
        }
        #endregion

        #region 首页内容搜索结果页
        /// <summary>
        /// 首页内容搜索结果页
        /// </summary> 
        /// <param name="keyword"></param> 
        /// <param name="language">1 汉语 2 英语</param> 
        /// <returns></returns>
        [WebMethod]
        public void SearchForIndexByContent(int page, int rows, string keyword, int language)
        {
            //逻辑：把查到的文章都取出来，在判断每个文章里有多少个这个字符串 //前端做？后台做？
            List<DocumentContentModel> list;
            if (string.IsNullOrEmpty(keyword))
            {
                list = documentContentApp.SearchForIndexByContent();
            }
            else
            {
                list = documentContentApp.SearchForIndexByContent(keyword,   language);
            }

            List<object> documentList = new List<object>();

            List<ItemsDetailEntity> itemListAll = itemsDetailApp.GetItemAll();


            foreach (var item in list)
            {
                string fileStateValue = "";
                if (!string.IsNullOrEmpty(item.F_FileState))
                {
                    var itemTemp = itemListAll.Where(x => x.F_ItemCode == item.F_FileState).FirstOrDefault();
                    fileStateValue = (language == 1 ? itemTemp.F_ItemName : itemTemp.F_ItemEnName);
                }
                documentList.Add(new
                {
                    Id = item.F_DocumentId,
                    Type = item.F_Type,
                    Title = (language == 1 ? item.F_ChineseTitle : item.F_EnglishTitle),
                    ContentAppearNum = SubstringCount((language == 1 ? item.F_ChineseContent : item.F_EnglishContent), keyword),
                    FileState = fileStateValue,
                    Domain = item.F_Domain,
                    AcquisitionStandard = item.F_AcquisitionStandard,
                    Publisher = item.F_Publisher,
                    Direction = item.F_Direction,
                    ReleaseDate = item.F_ReleaseDate,
                    ImplementDate = item.F_ImplementDate,
                    DocShowType = GetDocShowType(item)
                });
            }

            int total = documentList.Count();
            var reList = documentList.Skip((page - 1) * rows).Take(rows).AsQueryable().ToList();
            ChangeJson(new
            {
                total,
                documentList = reList,
            });
        }
        /// <summary>
        /// 统计 首页内容搜索结果页
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="language"></param>
        [WebMethod]
        public void SearchForIndexByContentStat(string keyword, int language)
        {
            var list = documentContentApp.SearchForIndexByContent(keyword,   language);
            var typeGroupList = from c in list
                                group c by c.F_Type into g
                                select new
                                {
                                    Type = g.Key,
                                    Num = g.Count(),
                                };

            var fileStateGroupList = from c in list
                                     group c by c.F_FileState into g
                                     select new
                                     {
                                         Type = g.Key,
                                         Num = g.Count(),
                                     };
            var domainGroupList = from c in list
                                  group c by c.F_Domain into g
                                  select new
                                  {
                                      Type = g.Key,
                                      Num = g.Count(),
                                  };
            var acquisitionStandardGroupList = from c in list
                                               group c by c.F_AcquisitionStandard into g
                                               select new
                                               {
                                                   Type = g.Key,
                                                   Num = g.Count(),
                                               };
            var publisherGroupList = from c in list
                                     group c by c.F_Publisher into g
                                     select new
                                     {
                                         Type = g.Key,
                                         Num = g.Count(),
                                     };
            var directionGroupList = from c in list
                                     group c by c.F_Direction into g
                                     select new
                                     {
                                         Type = g.Key,
                                         Num = g.Count(),
                                     };

            var itemList = itemsDetailApp.GetItemAndItemDetailAll();
            ChangeJson(new
            {
                typeGroupList,
                fileStateGroupList,
                domainGroupList,
                acquisitionStandardGroupList,
                publisherGroupList,
                directionGroupList,
                documentStateList = GetItemDetailList(itemList.Where(x => x.EnCode == "DocumentState").ToList(), language),//DocumentState 文档状态
                documentDomainList = GetItemDetailList(itemList.Where(x => x.EnCode == "DocumentDomain").ToList(), language),//DocumentDomain 文档领域
                caiBiaoList = GetItemDetailList(itemList.Where(x => x.EnCode == "CaiBiao").ToList(), language), //CaiBiao 采标 
                publisherList = GetItemDetailList(itemList.Where(x => x.EnCode == "Publisher").ToList(), language),//Publisher 发布方
                directionList = GetItemDetailList(itemList.Where(x => x.EnCode == "Direction").ToList(), language) //Direction 方向
            });
        }

        /// <summary>
        /// 首页内容查询 二次查询
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fileState"></param>
        /// <param name="domain"></param>
        /// <param name="acquisitionStandard"></param>
        /// <param name="publisher"></param>
        /// <param name="direction"></param>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="keyword"></param>
        /// <param name="language"></param>
        [WebMethod]
        public void SearchForIndexByContentSecond(int type, string fileState, string domain, string acquisitionStandard, string publisher, string direction, int page, int rows, string keyword, int language)
        {
            //逻辑：把查到的文章都取出来，在判断每个文章里有多少个这个字符串 //前端做？后台做？
            List<DocumentContentModel> list;
            if (string.IsNullOrEmpty(keyword))
            {
                list = documentContentApp.SearchForIndexByContent(type, fileState, domain, acquisitionStandard, publisher, direction );
            }
            else
            {
                list = documentContentApp.SearchForIndexByContent(type, fileState, domain, acquisitionStandard, publisher, direction, keyword);
            }

            List<object> documentList = new List<object>();

            List<ItemsDetailEntity> itemListAll = itemsDetailApp.GetItemAll();


            foreach (var item in list)
            {
                string fileStateValue = "";
                if (!string.IsNullOrEmpty(item.F_FileState))
                {
                    var itemTemp = itemListAll.Where(x => x.F_ItemCode == item.F_FileState).FirstOrDefault();
                    fileStateValue = (language == 1 ? itemTemp.F_ItemName : itemTemp.F_ItemEnName);
                }
                documentList.Add(new
                {
                    Id = item.F_DocumentId,
                    Type = item.F_Type,
                    Title = (language == 1 ? item.F_ChineseTitle : item.F_EnglishTitle),
                    ContentAppearNum =  (language == 1 ? item.F_ChineseContentAppearNum : item.F_EnglishContentAppearNum) ,


                    //ContentAppearNum = SubstringCount((language == 1 ? item.F_ChineseContent : item.F_EnglishContent), keyword),
                    FileState = fileStateValue,
                    Domain = item.F_Domain,
                    AcquisitionStandard = item.F_AcquisitionStandard,
                    Publisher = item.F_Publisher,
                    Direction = item.F_Direction,
                    ReleaseDate = item.F_ReleaseDate,
                    ImplementDate = item.F_ImplementDate,
                    DocShowType = GetDocShowType(item)
                });
            }

            int total = documentList.Count();
            var reList = documentList.Skip((page - 1) * rows).Take(rows).AsQueryable().ToList();
            ChangeJson(new
            {
                total,
                documentList = reList,
            });
        }

        #endregion

        #region 获取文章详情 是否被收藏
        [WebMethod]
        public void GetDocumentInfoById(string documentId, string memberId, int language, int type)
        {
            bool isCollect = false;
            string strChinese = "";
            string catalogue = "";
            string StateChName = "", StateEnName = "";
            DocumentEntity documentEntity = documentApp.GetForm(documentId);
            if (documentEntity != null)
            {
                if (!string.IsNullOrEmpty(memberId))
                {
                    isCollect = myCollectApp.CheckIsCollect(memberId, documentId);
                }
                strChinese = GetDocumentContent(documentEntity, type);

                if (!string.IsNullOrEmpty(strChinese))
                {
                    string content = "";

                    string path = GetDocUrl(documentEntity, type);
                    strChinese = Common.ReplenishHtml(strChinese, path);
                    //前端读取文件 图片 img 要替换，加跟目录

                    catalogue = NFine.Code.WordHelper.ReadXmlByCode(strChinese, out content);
                    strChinese = content;

                    //前端显示 去掉 英文带中文的 红色 样式
                    strChinese = strChinese.Replace("#F4AEB0", "#FFFFFF");// 
                }
                List<ItemsDetailEntity> itemListAll = itemsDetailApp.GetItemAll();
                var itemTemp = itemListAll.Where(x => x.F_ItemCode == documentEntity.F_FileState).FirstOrDefault();
                documentEntity.F_FileState = (language == 1 ? itemTemp.F_ItemName : itemTemp.F_ItemEnName);
                StateChName = itemTemp.F_ItemName;
                StateEnName = itemTemp.F_ItemEnName;
            }
            ChangeJson(new
            {
                isCollect,
                strChinese,
                catalogue,
                documentEntity, 
                DocShowType = GetDocShowType(documentEntity),
                StateChName,
                StateEnName
            });
        }
        [WebMethod]
        public void GetDocumentInfoByIdTest(string documentId, string memberId, int language, int type)
        {
            bool isCollect = false;
            string strChinese = "";
            string catalogue = "";
            DocumentEntity documentEntity = documentApp.GetForm(documentId);
            if (documentEntity != null)
            {
                if (!string.IsNullOrEmpty(memberId))
                {
                    isCollect = myCollectApp.CheckIsCollect(memberId, documentId);
                }
                strChinese = GetDocumentContent(documentEntity, type);

                if (!string.IsNullOrEmpty(strChinese))
                {
                    string content = "";
                    catalogue = NFine.Code.WordHelper.ReadXmlByCode(strChinese, out content);
                    strChinese = content;
                }
                List<ItemsDetailEntity> itemListAll = itemsDetailApp.GetItemAll();
                var itemTemp = itemListAll.Where(x => x.F_ItemCode == documentEntity.F_FileState).FirstOrDefault();
                documentEntity.F_FileState = (language == 1 ? itemTemp.F_ItemName : itemTemp.F_ItemEnName);
            }
            ChangeJson(new
            {
                isCollect,
                strChinese,
                catalogue,
                documentEntity
            });
        }
        [WebMethod]
        public void GetDocumentInformationInfoById(string documentId, int language, int type)
        {
            string strChinese = "";
            string catalogue = "";
            DocumentEntity documentEntity = documentApp.GetForm(documentId);
            if (documentEntity != null)
            {
                //资讯 在内容表里取 1 原文 2 中文 3 英文 4 混排
                if (documentEntity.F_Type == (int)NFine.Code.Enum.DocumentType.资讯)
                {
                    DocumentContentEntity model = documentContentApp.GetFormByDocumentId(documentEntity.F_Id);
                    if (model != null)
                    {
                        switch (type)
                        {
                            case 1:
                            case 2:
                                strChinese = model.F_ChineseContent;
                                break;
                            case 3:
                                strChinese = model.F_EnglishContent;
                                break;
                        }
                    }
                }
                else
                {
                    strChinese = GetDocumentContent(documentEntity, type);
                    if (!string.IsNullOrEmpty(strChinese))
                    {
                        string content = "";

                        catalogue = NFine.Code.WordHelper.ReadXmlByCode(strChinese, out content);
                        strChinese = content;
                    }
                }


                if (!string.IsNullOrEmpty(documentEntity.F_FileState))
                {
                    List<ItemsDetailEntity> itemListAll = itemsDetailApp.GetItemAll();
                    var itemTemp = itemListAll.Where(x => x.F_ItemCode == documentEntity.F_FileState).FirstOrDefault();
                    documentEntity.F_FileState = (language == 1 ? itemTemp.F_ItemName : itemTemp.F_ItemEnName);

                }
            }
            ChangeJson(new
            {
                strChinese,
                catalogue,
                documentEntity,
                DocShowType = GetDocShowType(documentEntity)
            });
        }
        #endregion

        #region 文档下载
        /// <summary>
        /// Webservice中的下载文件处理函数
        /// </summary>
        /// <param name="documentId">文件id</param>
        /// <param name="memberId">会员id</param>
        /// <returns>返回文件流</returns>
        [WebMethod]
        public byte[] DownloadFile(string documentId, string memberId, int language)
        {
            //根据文档id 获取文档地址，根据会员id 判断会员是否可以下载，下载完成后新增一条下载记录

            DocumentEntity documentEntity = documentApp.GetForm(documentId);
            if (documentEntity != null)
            {
                //验证会员下载次数，判断是否可以下载
                MemberEntity memberEntity = memberApp.GetForm(memberId);
                if (memberEntity != null)
                {
                    int downAndPrintCount = 0;//总点数
                    int downloadCount = 0;
                    MemberEntity PmemberEntity;
                    if (memberEntity.F_IsDeputy==true)
                    {
                        PmemberEntity= memberApp.GetForm(memberEntity.F_SuperUId);
                        //如果是副账号 要取父账号的总点数 记录是记录副账号的下载次数
                        downAndPrintCount = PmemberEntity.F_DownAndPrintCount;
                        downloadCount = downloadInfoApp.GetDownloadCountByUserId(PmemberEntity.F_Id);
                    }
                    else
                    {
                        //如果是副账号 要取父账号的总点数 记录是记录副账号的下载次数
                        downAndPrintCount = memberEntity.F_DownAndPrintCount;
                        downloadCount = downloadInfoApp.GetDownloadCountByUserId(memberEntity.F_Id);
                    } 
                  
                    if (downAndPrintCount > downloadCount)
                    {
                        //可以下载，下载并新增一条下载记录
                        string strFilePath = (language == 1 ? documentEntity.F_ChineseUrl : documentEntity.F_EnglishUrl);
                        if (string.IsNullOrEmpty(strFilePath))
                        {
                            return new byte[0];//文件不存在
                        }
                        FileStream fs = null;  
                        string CurrentUploadFilePath = strFilePath;
                        if (File.Exists(CurrentUploadFilePath))
                        {
                            try
                            {
                                ///打开现有文件以进行读取。
                                fs = File.OpenRead(CurrentUploadFilePath);
                                int b1;
                                System.IO.MemoryStream tempStream = new System.IO.MemoryStream();
                                while ((b1 = fs.ReadByte()) != -1)
                                {
                                    tempStream.WriteByte(((byte)b1));
                                }
                                //下载之前 加一个 下载记录
                                if (!downloadInfoApp.CheckMemberIdAndDocumentId(memberId, documentId))
                                {
                                    DownloadInfoEntity entity = new DownloadInfoEntity();
                                    entity.F_Type = documentEntity.F_Type.ToString();
                                    entity.F_UserId = memberId;
                                    entity.F_SuperUId = memberEntity.F_SuperUId;
                                    entity.F_Num = -1;
                                    entity.F_DocumentId = documentId;
                                    downloadInfoApp.SubmitForm(entity, entity.F_Id);
                                }

                                return tempStream.ToArray();
                            }
                            catch (Exception ex)
                            {
                                return new byte[0];
                            }
                            finally
                            {
                                fs.Close();
                            }
                        }
                        else
                        {
                            return new byte[0];
                        }

                    }
                }
            }
            return new byte[0];
        }
        /// <summary>
        /// Webservice中的下载文件处理函数
        /// DocShowType 1 中文单独 2 英文单独 3 中英文单独 4 中英文混排上传（带左右结构的）
        /// language下载类型 1 单独上传中文下载，2 单独上传英文下载，3 混合上传下载中文 4 混合下载英文 5混合下载混合
        /// </summary>
        /// <param name="documentId">文件id</param>
        /// <param name="memberId">会员id</param>
        /// <returns>返回文件流</returns>
        [WebMethod]
        public void DownloadPDFFile(string documentId, string memberId, int language,int docShowType)
        {

            int downloadDocType = language;
            Common.Log("log", "log", "下载pdf开始：");
            //根据文档id 获取文档地址，根据会员id 判断会员是否可以下载，下载完成后新增一条下载记录
            try
            {
                Common.Log("log", "log", "下载pdf开始：1");
                DocumentEntity documentEntity = documentApp.GetForm(documentId);
                Common.Log("log", "log", "下载pdf开始：1");
                if (documentEntity != null)
                {
                    Common.Log("log", "log", "下载pdf开始：1");
                    //验证会员下载次数，判断是否可以下载
                    MemberEntity memberEntity = memberApp.GetForm(memberId);
                    Common.Log("log", "log", "下载pdf开始：1");
                    if (memberEntity != null)
                    {
                        Common.Log("log", "log", "下载pdf开始：1");
                        int downAndPrintCount = 0;//总点数
                        int downloadCount = 0;
                        MemberEntity PmemberEntity;
                        if (memberEntity.F_IsDeputy == true)
                        {
                            PmemberEntity = memberApp.GetForm(memberEntity.F_SuperUId);
                            //如果是副账号 要取父账号的总点数 记录是记录副账号的下载次数
                            downAndPrintCount = PmemberEntity.F_DownAndPrintCount;
                            downloadCount = downloadInfoApp.GetDownloadCountByUserId(PmemberEntity.F_Id);
                        }
                        else
                        {
                            //如果是副账号 要取父账号的总点数 记录是记录副账号的下载次数
                            downAndPrintCount = memberEntity.F_DownAndPrintCount;
                            downloadCount = downloadInfoApp.GetDownloadCountByUserId(memberEntity.F_Id);
                        } 
                        Common.Log("log", "log", "下载pdf开始：1");
                        if (downAndPrintCount > downloadCount)
                        { 
                            Common.Log("log", "log", "下载pdf开始：1");
                            //可以下载，下载并新增一条下载记录
                            string strFilePath = GetUrlByDocShowType(documentEntity , docShowType, language);
                            string reFilePath = GetReFilePath(documentEntity, docShowType, language).Replace("、","_");
                            string dir = GetDir(documentEntity, docShowType, language) ;
                            strFilePath = dir + "\\" + strFilePath;
                            // string strFilePath = (languageType == 1 ? documentEntity.F_ChineseUrl : documentEntity.F_EnglishUrl);
                            if (string.IsNullOrEmpty( strFilePath))
                            {
                                return;//文件不存在
                            } 
                            FileStream fs = null;
                            Common.Log("log", "log", "下载pdf开始：1");
                            string CurrentUploadFilePath = strFilePath;
                            Common.Log("log", "log", "下载pdf开始：1");
                            if (File.Exists(CurrentUploadFilePath))
                            {
                                Common.Log("log", "log", "下载pdf开始：1");
                              //  string path = Path.GetFileName(CurrentUploadFilePath);
                                Common.Log("log", "log", "下载pdf开始：1");
                                Context.Response.AddHeader("Content-Disposition", "attachment;filename=" +


                                    HttpUtility.UrlEncode(reFilePath.Replace(" ","_").Replace("/","_")) 
                                    
                                    );
                                Context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
                                Context.Response.ContentType = "application/pdf";
                                Common.Log("log", "log", "下载pdf开始：1");
                                try
                                {
                                    Common.Log("log", "log", "下载pdf开始：1");
                                    if (!downloadInfoApp.CheckMemberIdAndDocumentId(memberId, documentId))
                                    {
                                        DownloadInfoEntity entity = new DownloadInfoEntity();
                                        entity.F_Type = documentEntity.F_Type.ToString();
                                        entity.F_UserId = memberId;
                                        entity.F_SuperUId = memberEntity.F_SuperUId;
                                        entity.F_Num = -1;
                                        entity.F_DocumentId = documentId;
                                        downloadInfoApp.SubmitForm(entity, entity.F_Id);
                                    }
                                    Common.Log("log", "log", "下载pdf开始：15=" + CurrentUploadFilePath);
                                    ///打开现有文件以进行读取。
                                    fs = new FileStream(CurrentUploadFilePath, FileMode.Open);
                                    Common.Log("log", "log", "下载pdf开始：1");
                                    while (true)
                                    {
                                        byte[] buffer = new byte[fs.Length];
                                        //将文件读取成byte字节
                                        int len = fs.Read(buffer, 0, (int)fs.Length);
                                        if (len == 0) break;
                                        if (len == 1024)
                                        {
                                            Context.Response.BinaryWrite(buffer);
                                            break;
                                        }
                                        else
                                        {
                                            //读出文件数据比缓冲区小，重新定义缓冲区大小，只用于读取文件的最后一个数据块  
                                            byte[] b = new byte[len];
                                            for (int i = 0; i < len; i++)
                                            {
                                                b[i] = buffer[i];
                                            }
                                            Context.Response.BinaryWrite(b);
                                            break;
                                        }
                                    }
                                    Common.Log("log", "log", "下载pdf开始：1");
                                }
                                catch (Exception ex)
                                {
                                    Common.Log("log", "log", "下载pdf错误：" + ex.InnerException.Message);
                                }
                                finally
                                {
                                    fs.Close();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Common.Log("log", "log", "下载pdf错误2：" + ex.InnerException.Message);
            }

        }
        #endregion

        #region other

        // <summary>
        /// 计算字符串中子串出现的次数
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="substring">子串</param>
        /// <returns>出现的次数</returns>
        static int SubstringCount(string str, string substring)
        {
            if (!string.IsNullOrEmpty(substring))
            {
                if (str.Contains(substring))
                {
                    string strReplaced = str.Replace(substring, "");
                    return (str.Length - strReplaced.Length) / substring.Length;
                }
            }
            return 0;
        }
        private List<DocumentShowListModel> GetDocList(List<DocumentModel> list, int language)
        {
            List<DocumentShowListModel> documentList = new List<DocumentShowListModel>();
            List<ItemsDetailEntity> itemListAll = itemsDetailApp.GetItemAll();
            foreach (var item in list)
            {
                string fileStateValue = "";
                if (!string.IsNullOrEmpty(item.F_FileState))
                {
                    var itemTemp = itemListAll.Where(x => x.F_ItemCode == item.F_FileState).FirstOrDefault();
                    fileStateValue = (language == 1 ? itemTemp.F_ItemName : itemTemp.F_ItemEnName);
                }
                documentList.Add(new DocumentShowListModel
                {
                    Id = item.F_Id,
                    Type = item.F_Type,
                    Title = (language == 1 ? item.F_ChineseTitle : item.F_EnglishTitle),
                    CreatorTime = item.F_CreatorTime,
                    FileState = fileStateValue,
                    ReleaseDate = item.F_ReleaseDate,
                    ImplementDate = item.F_ImplementDate,
                    DocShowType= GetDocShowType(item)
                });
            }
            return documentList;
        }
        private static List<ItemDetailModel> GetItemDetailList(List<ItemAndItemDetailModel> list, int language)
        {
            List<ItemDetailModel> entityList = new List<ItemDetailModel>();
            foreach (var item in list)
            {
                entityList.Add(new ItemDetailModel
                {
                    Id = item.Id,
                    ItemName = item.ItemName,
                    ItemEnName = item.ItemEnName,
                    //Title = (languageType == 1 ? item.F_ChineseTitle : item.F_EnglishTitle), 
                    ItemCode = item.ItemCode
                });
            }
            return entityList;
        }

        private string GetDocumentContent(DocumentEntity data, int type)
        {
            string strChinese = "";
            try
            {
                string path = GetDocUrl(data, type);
                if (!string.IsNullOrEmpty(path))
                {
                    FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
                    StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                    strChinese = sr.ReadToEnd();

                    //接口要显示文件内容，需要配置一个网站后台的站点
                    //string hostAddr = ConfigurationManager.AppSettings["HostAddr"];  
                    //strChinese = strChinese.Replace("<img src=\"", "<img src=\"" + hostAddr);
                    //strChinese = strChinese.Replace("<link href=\"", "<link href=\"" + hostAddr);
                    fs.Flush();
                    sr.Close();
                    fs.Close();
                }
            }
            catch (Exception e)
            {
                NFine.Code.Common.Log("log", "err", "读文件错误" + e.InnerException.Message);
            }
            return strChinese;
        }

        private string GetDocUrl(DocumentEntity data, int type)
        {
            string path = data.F_HtmlUrl;
            switch (type)
            {
                case 1:
                    path = data.F_HtmlUrl;
                    break;
                case 2:
                    path = data.F_ChineseUrl;
                    break;
                case 3:
                    path = data.F_EnglishUrl;
                    break;
                case 4:
                    path = data.F_TotalUrl;
                    break;
                default:
                    break;
            }
            return path;
        }

        /// <summary>
        /// 根据路径判断 是什么类型上传的：
        /// 1 中文单独 2 英文单独 3 中英文单独 4 中英文混排上传（带左右结构的）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private int GetDocShowType(DocumentModel data)
        {
            return GetDocShowTypeByString(data.F_TotalUrl, data.F_HtmlUrl, data.F_ChineseUrl, data.F_EnglishUrl);
        }
        private int GetDocShowType(DocumentEntity data)
        {
            return GetDocShowTypeByString(data.F_TotalUrl, data.F_HtmlUrl, data.F_ChineseUrl, data.F_EnglishUrl);
        }
        private int GetDocShowType(DocumentContentModel data)
        {
            return GetDocShowTypeByString(data.F_TotalUrl, data.F_HtmlUrl, data.F_ChineseUrl, data.F_EnglishUrl);
        }
        
        private int GetDocShowTypeByString(string F_TotalUrl,string F_HtmlUrl,string F_ChineseUrl,string F_EnglishUrl)
        {
            if (!string.IsNullOrEmpty( F_TotalUrl)
                && !string.IsNullOrEmpty( F_HtmlUrl)
                && !string.IsNullOrEmpty(F_ChineseUrl)
                && !string.IsNullOrEmpty(F_EnglishUrl))
            {
                return 4;
            }
            else if (!string.IsNullOrEmpty( F_ChineseUrl) && !string.IsNullOrEmpty( F_EnglishUrl))
            {
                return 3;
            }

            else if (!string.IsNullOrEmpty( F_EnglishUrl))
            {
                return 2;
            }
            else if (!string.IsNullOrEmpty( F_ChineseUrl))
            {
                return 1;
            }
            return 0;
        }


        /// <summary>
        /// downloadDocType
        /// </summary> 
        /// <param name="docShowType"> 1 中文单独 2 英文单独 3 中英文单独 4 中英文混排上传（带左右结构的）</param>
        /// <param name="downloadDocType"> language 1-上下混排；2-中文；3-英文；4-左右混排</param>
        /// <returns></returns>
        private string  GetUrlByDocShowType(DocumentEntity documentEntity ,int docShowType,int language)
        {
            if (docShowType == 1)
            {
                //1 中文单独 ：单独上传 中文 或者英文 分别保存俩html 俩pdf pdf与html名称一致后缀不同
                 
                return documentEntity.F_ChineseUrl.Substring(documentEntity.F_ChineseUrl.LastIndexOf('\\')).Split('.')[0] + ".pdf";
            }
            else if(docShowType == 2)
            {
                return documentEntity.F_EnglishUrl.Substring(documentEntity.F_EnglishUrl.LastIndexOf('\\')).Split('.')[0] + ".pdf"; 
            }
            else if (docShowType == 3)
            {
                if (language == 2)
                {
                    //汉语 标题
                    return  "CH.pdf";
                }
                else if (language == 3)
                {
                    //英语标题
                    return   "EN.pdf";
                } 
            }
            else if (docShowType == 4)
            {
                if (language == 2)
                {
                    //汉语 标题
                    return "CH.pdf";
                }
                else if (language == 3)
                {
                    //英语标题
                    return "EN.pdf";
                }
                else if (language == 1 || language == 4)
                {
                    //英语标题 +CH_EN
                    return "CH_EN.pdf";
                }
            
            }
            return "";
        }
        /// <summary>
        /// 获取 下载pdf文件的文件名
        /// </summary>
        /// <param name="documentEntity"></param>
        /// <param name="docShowType"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        private string GetReFilePath(DocumentEntity documentEntity, int docShowType, int language)
        {
            if (docShowType == 1)
            {
                //1 中文单独 ：单独上传 中文 或者英文 分别保存俩html 俩pdf pdf与html名称一致后缀不同

                return documentEntity.F_ChineseTitle.Replace(" ", "_") + ".pdf";
            }
            else if (docShowType == 2)
            {
                return documentEntity.F_EnglishTitle.Replace(" ", "_") + ".pdf";
            }
            else if (docShowType == 3)
            { 
                if (language == 2)
                {
                    //汉语 标题
                    return documentEntity.F_ChineseTitle.Replace(" ", "_") + ".pdf";
                }
                else if (language == 3)
                {
                    //英语标题
                    return documentEntity.F_EnglishTitle.Replace(" ", "_") + ".pdf";
                }
            }
            else if (docShowType == 4)
            {
                if (language == 2)
                {
                    //汉语 标题
                    //汉语 标题
                    return documentEntity.F_ChineseTitle.Replace(" ", "_") + ".pdf";
                }
                else if (language == 3)
                {
                    //英语标题
                    return documentEntity.F_EnglishTitle.Replace(" ", "_") + ".pdf";
                }
                else if (language == 1 || language == 4)
                {
                    //英语标题 +CH_EN
                    return documentEntity.F_EnglishTitle.Replace(" ", "_") + "CH_EN.pdf";
                }

            } 
            return "";
        }
        /// <summary>
        /// 获取目录
        /// </summary>
        /// <param name="documentEntity"></param>
        /// <param name="docShowType"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        private string GetDir(DocumentEntity documentEntity, int docShowType, int language)
        {
            if (docShowType == 1)
            {
                //1 中文单独 ：单独上传 中文 或者英文 分别保存俩html 俩pdf pdf与html名称一致后缀不同

                return documentEntity.F_ChineseUrl.Substring(0, documentEntity.F_ChineseUrl.LastIndexOf('\\'));
            }
            else if (docShowType == 2)
            {
                return documentEntity.F_EnglishUrl.Substring(0, documentEntity.F_EnglishUrl.LastIndexOf('\\'));
            }
            else 
            {
                return documentEntity.F_HtmlUrl.Substring(0, documentEntity.F_HtmlUrl.LastIndexOf('\\'));

            }  
        }
        #endregion

        #endregion
    }
}