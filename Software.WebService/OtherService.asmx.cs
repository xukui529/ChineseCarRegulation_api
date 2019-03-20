using NFine.Application.AuxiliaryManage;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.Entity.SystemManage;
using NFine.Domain.ViewModel;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Script.Services;
using System.Web.Services;
using System.Collections.Specialized;

namespace Software.WebService
{
    /// <summary>
    /// OtherService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class OtherService : System.Web.Services.WebService
    {
        private UserLogOnApp userLogOnApp = new UserLogOnApp();
        private MemberApp memberApp = new MemberApp();
        private QuestionAnswerApp questionAnswerApp = new QuestionAnswerApp();
        private MessageApp messageApp = new MessageApp();
        private DownloadInfoApp downloadInfoApp = new DownloadInfoApp();
        private DocumentVisitInfoApp documentVisitInfoApp = new DocumentVisitInfoApp();
        private ReleaseInfoApp releaseInfoApp = new ReleaseInfoApp();
        private SysMessageInfoApp sysMessageInfoApp = new SysMessageInfoApp();
        private MyCollectApp myCollectApp = new MyCollectApp();
        private DocumentSearchKeysApp documentSearchKeysApp = new DocumentSearchKeysApp();
        private ItemsDetailApp itemsDetailApp = new ItemsDetailApp();
        private LinkApp linkApp = new LinkApp();
        private RegisterApplyApp registerApplyApp = new RegisterApplyApp();
        private ItemsApp itemsApp = new ItemsApp();
        private MemberUpgradeApplyApp memberUpgradeApplyApp = new MemberUpgradeApplyApp();
         

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }


        #region 找回密码 

        /// <summary>
        /// 根据邮箱判断用户是否存在,存在邮箱 才能找回密码，发送邮件
        /// re:1有邮箱并且已发送激活邮件、2 有邮箱但是没查出用户、3系统无此邮箱
        /// </summary>
        /// <param name="email"></param>
        [WebMethod]
        public void FindPasswordCheckUserByEmail(string email, int language)
        {
            int re = 0;
            if (memberApp.CheckUserByEmail(email))
            {
                var member = memberApp.GetFormByEmail(email);
                if (member != null)
                {

                    string sid = "";
                    DateTime now = DateTime.Now;
                    member.F_SecretKey = Md5.md5(Common.CreateNo(), 16).ToLower();
                    member.F_SecretKeyPastDue = now;
                    sid = Md5.md5(email + "|" + member.F_SecretKey + "|" + now.ToString("yyyyMMddHHmmss"), 32).ToLower();
                    memberApp.SubmitForm(member, member.F_Id);
                    //发送激活邮件 
                    MailHelper mailHelper = new MailHelper();
                    mailHelper.MailServer = ConfigurationManager.AppSettings["MailServer"];
                    mailHelper.MailUserName = ConfigurationManager.AppSettings["MailUserName"];
                    mailHelper.MailPassword = ConfigurationManager.AppSettings["MailPassword"];
                    string hostAddr = ConfigurationManager.AppSettings["HostQAddr"];
                    int mailPort = Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"]);
                    //string url = hostAddr + "/q/#/SignIn?sid=" + sid + "&email=" + email;
                    string url = hostAddr + "ResetPassword/" + sid + "/" + email;
                    string title = "";
                    string body = "";
                
                    mailHelper.MailName = "密码重置( Ret Password)";
                    title = "密码重置( Ret Password)";
                    string templetpath = Server.MapPath("mimachongzhi.html");
                    NameValueCollection myCol = new NameValueCollection();
                    myCol.Add("link", url);
                    body = MailHelper.BulidByFile(templetpath, myCol);
                    //此处网站地址要对应修改一下，前端写一下这个url对应的页面，页面调用 ChangePasswordVerify 接口进行核实，核实后 调用修改密码 SubmitChangePassword
                   // mailHelper.SendByThread(email, title, body, MailPort);
                    mailHelper.SendMail(email, title, body, mailPort);

                    re = 1000;
                }
                else
                {
                    re = 1001;
                }
            }
            else
            {
                re = 1002;
            }

            ChangeJson(new ResultData
            {
                Success = true,
                ResultCode = re ,
                ResultMessage = ResultHelper.GetMessage(language, "FindPasswordCheckUserByEmail", re.ToString()),
                Data = null
            });
        }

        //核实 找回密码申请 ,核实成功后可显示修改页面
        [WebMethod]
        public void ChangePasswordVerify(string sid, string email)
        {
            bool re = false;
            var member = memberApp.GetFormByEmail(email);
            if (member != null)
            {
                DateTime now = DateTime.Now;
                now = (DateTime)member.F_SecretKeyPastDue;
                re = (sid == (Md5.md5(email + "|" + member.F_SecretKey + "|" + now.ToString("yyyyMMddHHmmss"), 32).ToLower()));

            }
            ChangeJson(re);
        }

        //修改密码 直接更新密码 没返回值，直接返回true  参数是 密码 跟id 
        [WebMethod]
        public void SubmitChangePassword(string userPassword, string email)
        {
            bool re = false;
            var member = memberApp.GetFormByEmail(email);
            if (member != null)
            {
                member.F_Password = userPassword;
                memberApp.SubmitForm(member, member.F_Id);
                re = true;
            }
            ChangeJson(re);
        }
        #endregion

        #region 修改密码
        [WebMethod]
        public void SubmitNewPassword(string userOldPassword, string userNewPassword, string memberId, int language)
        {
            int re = 0;
            //0初始值 1000修改成功 1001此用户不存在 1002旧密码错误
            var member = memberApp.GetForm(memberId);
            if (member != null)
            {
                if (userOldPassword == member.F_Password)
                {
                    member.F_Password = userNewPassword;
                    memberApp.SubmitForm(member, member.F_Id);
                    re = 1000;
                }
                else
                {
                    re = 1002;
                }
            }
            else
            {
                re = 1001;
            }
            ChangeJson(new ResultData
            {
                Success = true,
                ResultCode = re,
                ResultMessage = ResultHelper.GetMessage(language, "SubmitNewPassword", re.ToString()),
                Data = null
            });
        }
        #endregion

        #region 常见问题 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="type"></param>
        /// <param name="language"></param>
        [WebMethod]
        public void GetQuestionAnswerDataList(int page, int rows, string type, int language)
        {
            var data = itemsDetailApp.GetItemList("WenDaType");
            List<object> questionAnswerTypeList = new List<object>();
            foreach (ItemsDetailEntity item in data)
            {
                questionAnswerTypeList.Add(new {id= item.F_Id, code = item.F_ItemCode, text =
                     (language == 1 ? item.F_ItemName : item.F_ItemEnName)  });
            }
            List<QuestionAnswerModel> list = questionAnswerApp.GetDataList(type);
            List<object> questionAnswerList = new List<object>();
            foreach (var item in list)
            {
                questionAnswerList.Add(new
                {
                    Id = item.F_Id, 
                    Title = (language == 1 ? item.F_ChTitle : item.F_EnTitle),
                    Type = item.F_Type,
                    Answer = (language == 1 ? item.F_ChAnswer : item.F_EnAnswer), 
                    CreatorTime = item.F_CreatorTime,
                });
            }
            int total = questionAnswerList.Count();
            var reList = questionAnswerList.Skip((page-1)*rows).Take(rows).AsQueryable().ToList();
            ChangeJson(new
            {
                total,
                questionAnswerList = reList,
                questionAnswerTypeList
            });
        }
        /// <summary>
        /// 获取问题类型
        /// </summary> 
        [WebMethod]
        public void GetQuestionAnswerTypeList(  int language)
        {
            var data = itemsDetailApp.GetItemList("WenDaType");
            List<object> questionAnswerTypeList = new List<object>();
            foreach (ItemsDetailEntity item in data)
            {
                questionAnswerTypeList.Add(new { id = item.F_Id, code = item.F_ItemCode, text = (language == 1 ? item.F_ItemName : item.F_ItemEnName) });
            }
           
            ChangeJson(new
            { 
                questionAnswerTypeList
            });
        }
        #endregion

        #region 联系我们 新增留言
        [WebMethod]
        public void AddMessage(string title, string email, string type, string memo, string memberId)
        {
            MessageEntity entity = new MessageEntity();
            entity.F_Title = title;
            entity.F_Email = email;
            entity.F_Type = type;
            entity.F_Memo = memo;
            entity.F_State = false;
            entity.F_UserId = memberId;
            entity.F_DeleteMark = false;
            messageApp.SubmitForm(entity, entity.F_Id);
            ChangeJson(true);
        }
        #endregion

        #region 文档下载  Auxiliary_DownloadInfo
        //新增下载记录
        [WebMethod]
        public void AddDownloadInfo(string type, string memberId, string superUId, string documentId)
        {
            //判断如果下过就不扣分数了
            if(!downloadInfoApp.CheckMemberIdAndDocumentId(memberId, documentId))
            {
                DownloadInfoEntity entity = new DownloadInfoEntity();
                entity.F_Type = type;
                entity.F_UserId = memberId;
                entity.F_SuperUId = superUId;
                entity.F_Num = -1;
                entity.F_DocumentId = documentId;
                downloadInfoApp.SubmitForm(entity, entity.F_Id);
            } 
            ChangeJson(true);
        }
        //查询下载记录 （包括副账户消耗的）
        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="userid"></param> 
        /// <param name="language">1中文2英文</param>
        [WebMethod]
        public void GetDownloadInfoList(int page, int rows, string memberId, int language)
        {
            int total = 0;
            List<DownloadInfoModel> modelList = downloadInfoApp.GetDataList(page, rows, memberId, out total);
            List<object> downloadInfoList = new List<object>();
            foreach (var item in modelList)
            {
                downloadInfoList.Add(new
                {
                    Id = item.F_Id,
                    Type = item.F_Type,
                    Title = (language == 1 ? item.F_DocumentChineseTitle : item.F_DocumentEnglishTitle),
                    Num = item.F_Num,
                    CreatorTime = item.F_CreatorTime,
                });
            }
            ChangeJson(new
            {
                total,
                downloadInfoList
            });

        }
        //查询点数  已消耗点数（包括副账户消耗的）、总点数
        [WebMethod]
        public void GetDownloadInfoStat(string memberId)
        {
            MemberEntity memberEntity = memberApp.GetForm(memberId);
            int downAndPrintCount = memberEntity.F_DownAndPrintCount;//总点数
            int downloadCount = downloadInfoApp.GetDownloadCountByUserId(memberId);
            ChangeJson(new
            {
                downAndPrintCount,
                downloadCount
            }
                );
        }
        /// <summary>
        /// 下载排行
        /// </summary>
        [WebMethod]
        public void GetDownloadInfoRanking()
        {
            ChangeJson(downloadInfoApp.GetRankingList());
        }
        /// <summary>
        /// 下载查询统计
        /// </summary>
        /// <param name="searchType">1 日、2 周、3月、4年</param>
        [WebMethod]
        public void GetDownloadInfoReportStat(int searchType)
        {
            var beginTime = DateTime.Now;
            var endTime = DateTime.Now;
            if (searchType == 1)
            {
                var tempTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                beginTime = tempTime;
                endTime = tempTime.AddDays(1).AddSeconds(-1);
            }
            if (searchType == 2)
            {
                var tempTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                beginTime = DateHelper.GetMondayDate(tempTime);
                endTime = DateHelper.GetSundayDate(tempTime).AddDays(1).AddSeconds(-1);
            }
            if (searchType == 3)
            {
                var tempTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                beginTime = tempTime;
                endTime = tempTime.AddMonths(1).AddSeconds(-1);
            }

            ChangeJson(downloadInfoApp.GetItemList(beginTime, endTime, searchType));
        }
        #endregion

        #region 文档访问统计 Auxiliary_DocumentVisitInfo
        //新增访问记录
        [WebMethod]
        public void AddDocumentVisitInfo(string memberId, string documentId, int type)
        {
            DocumentVisitInfoEntity entity = new DocumentVisitInfoEntity();
            entity.F_UserId = memberId;
            entity.F_DocumentId = documentId;
            entity.F_Type = type;
            documentVisitInfoApp.SubmitForm(entity, entity.F_Id);
            ChangeJson(true);
        }
        //查询列表
        /// <summary>
        /// 测试到这里
        /// </summary>
        /// <param name="searchType">1 日、2 周、3月、4指定时间段</param>
        [WebMethod]
        public void GetDocumentVisitInfoStat(int searchType, DateTime beginTime, DateTime endTime)
        {
            var tempTime = DateTime.Now;
            switch (searchType)
            {
                case 1:
                    beginTime = DateTime.Now;
                    endTime = DateTime.Now;
                    tempTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    beginTime = tempTime;
                    endTime = tempTime.AddDays(1).AddSeconds(-1);
                    break;
                case 2:
                    beginTime = DateTime.Now;
                    endTime = DateTime.Now;
                    tempTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                    beginTime = DateHelper.GetMondayDate(tempTime);
                    endTime = DateHelper.GetSundayDate(tempTime).AddDays(1).AddSeconds(-1);
                    break;
                case 3:
                    beginTime = DateTime.Now;
                    endTime = DateTime.Now;
                    tempTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    beginTime = tempTime;
                    endTime = tempTime.AddMonths(1).AddSeconds(-1);
                    break;
                case 4:
                    break;
            }

            ChangeJson(documentVisitInfoApp.GetItemList(beginTime, endTime, searchType));
        }
        #endregion

        #region 发布统计  Auxiliary_ReleaseInfo
        //新增接口
        [WebMethod]
        public void AddReleaseInfo(string documentId, string versions, string remark, int type)
        {
            ReleaseInfoEntity entity = new ReleaseInfoEntity();
            entity.F_Versions = versions;
            entity.F_DocumentId = documentId;
            entity.F_Remark = remark;
            entity.F_Type = type;
            releaseInfoApp.SubmitForm(entity, entity.F_Id);
            ChangeJson(true);
        }
        //查询统计接口 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchType">1 本月，2 时间段</param>
        [WebMethod]
        public void GetReleaseInfoStat(int searchType, DateTime beginTime, DateTime endTime)
        {
            if (searchType == 1)
            {
                beginTime = DateTime.Now;
                endTime = DateTime.Now;
                var tempTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                beginTime = tempTime;
                endTime = tempTime.AddMonths(1).AddSeconds(-1);
            }

            ChangeJson(releaseInfoApp.GetItemList(beginTime, endTime, searchType));
        }
        #endregion

        #region 系统消息 Auxiliary_SysMessageInfo
        ////新增系统消息接口 不提供接口
        //[WebMethod]
        //public void AddSysMessageInfo(string memberId, string title)
        //{
        //    SysMessageInfoEntity entity = new SysMessageInfoEntity();
        //    entity.F_UserId = memberId;
        //    entity.F_Title = title;
        //    sysMessageInfoApp.SubmitForm(entity, entity.F_Id);
        //    ChangeJson(true);
        //}
        //删除接口
        //[WebMethod]
        //public void DelSysMessageInfo(string ids)
        //{
        //    string[] idList = ids.Split(',');
        //    foreach (var item in idList)
        //    {
        //        sysMessageInfoApp.DeleteForm(item);
        //    }
        //    ChangeJson(true);
        //}
        //查询接口
        [WebMethod]
        public void GetSysMessageInfo(int page, int rows, string memberId,int language)
        {
            int total = 0;
            List<SysMessageInfoEntity> modelList = sysMessageInfoApp.GetDataList(page, rows, memberId, out total);
            List<object> sysMessageInfoEntityList = new List<object>();
            foreach (var item in modelList)
            {
                sysMessageInfoEntityList.Add(new
                {
                    Id = item.F_Id,
                    Title = (language == 1 ? item.F_ChTitle : item.F_EnTitle)  ,
                    CreatorTime = item.F_CreatorTime,
                });
            }
            ChangeJson(new
            {
                total,
                sysMessageInfoEntityList
            });
        }
        #endregion

        #region 收藏 Auxiliary_MyCollect
        //新增收藏接口
        [WebMethod]
        public void AddMyCollect(string memberId, string documentId, int language)
        {
            int result = 0;
            //0 默认值 1 新增成功 2 已收藏过
            //验证是否有，有就不收藏了
            MyCollectEntity entity = myCollectApp.GetForm(memberId, documentId);
            if(entity == null)
            {
                entity = new MyCollectEntity();
                entity.F_UserId = memberId;
                entity.F_DocumentId = documentId;
                myCollectApp.SubmitForm(entity, entity.F_Id);
                result = 1000;
            }
            else
            { 
                result = 1001;
            }
          
            ChangeJson(new ResultData
            {
                Success = true,
                ResultCode = result,
                ResultMessage = ResultHelper.GetMessage(language, "AddMyCollect", result.ToString()),
                Data = null
            });
        }
        //删除接口
        [WebMethod]
        public void DelMyCollect(string memberId, string documentId)
        {
            MyCollectEntity entity = myCollectApp.GetForm(memberId, documentId);
            if (entity != null)
            {
                myCollectApp.DeleteForm(entity.F_Id);
            } 
            ChangeJson(true);
        }
        //删除接口
        [WebMethod]
        public void DelMyCollectById(string id)
        {
            MyCollectEntity entity = myCollectApp.GetForm(id);
            if (entity != null)
            {
                myCollectApp.DeleteForm(entity.F_Id);
            }
            ChangeJson(true);
        }
        //查询接口

        [WebMethod]
        public void GetMyCollect(int page, int rows, string memberId, int language)
        {
            int total = 0;
            List<MyCollectModel> modelList = myCollectApp.GetDataList(page, rows, memberId, out total);
            List<object> myCollectList = new List<object>();
            foreach (var item in modelList)
            {
                myCollectList.Add(new
                {
                    Id = item.F_Id,
                    DocumentId=item.F_DocumentId,
                    Title = (language == 1 ? item.F_DocumentChineseTitle : item.F_DocumentEnglishTitle),
                    CreatorTime = item.F_CreatorTime,
                });
            }
            ChangeJson(new
            {
                total,
                myCollectList
            });
        }
        #endregion

        #region 副账户 新增  删除
        [WebMethod]
        public void AddDeputyMember(string email, string userPassword, string superUId, int language)
        {
            int result = 0;
            //0默认值 1创建成功 2邮箱是已有会员 3邮箱是已申请会员 待激活用户 4系统异常 5 您已创建3个副账户，无法继续添加。	You have already created three supplementary accounts. A principal account can  have up to three supplementary accounts.

            try
            {
                //验证邮箱，必须是 注册申请 与会员表都没有的才行

                //0默认值 1000    发送激活邮件成功 1001 次会员已存在 1002次会员已有注册申请 1003系统异常
                //1、验证邮箱在会员表是否存在，验证邮件在注册申请表是否存在
                if (!memberApp.CheckUserByEmail(email))
                {//2、都没有就插入到注册申请表，并且发送邮件返回
                    if (!registerApplyApp.CheckUserByEmail(email))
                    {
                        //您已创建3个副账户，无法继续添加。	You have already created three supplementary accounts. A principal account can  have up to three supplementary accounts.
                        if(memberApp.GetDeputyAllCount(superUId) >=3)
                        {
                            result = 1004;
                        }
                        else
                        {
                            var superMember = memberApp.GetForm(superUId);
                            MemberEntity memberEntity = new MemberEntity();

                            memberEntity.F_HYType = superMember.F_HYType;
                            memberEntity.F_Email = email;
                            memberEntity.F_Password = userPassword;
                            memberEntity.F_Phone = superMember.F_Phone;
                            memberEntity.F_Account = superMember.F_Account;
                            memberEntity.F_TimesState = superMember.F_TimesState;
                            memberEntity.F_UseState = superMember.F_UseState;
                            memberEntity.F_DownAndPrintCount = superMember.F_DownAndPrintCount;
                            memberEntity.F_State = superMember.F_State;
                            memberEntity.F_StartDate = superMember.F_StartDate;
                            memberEntity.F_EndDate = superMember.F_EndDate;
                            memberEntity.F_IsSubscription = superMember.F_IsSubscription;
                            memberEntity.F_SubscriptionLanguage = superMember.F_SubscriptionLanguage;
                            memberEntity.F_DeleteMark = false;
                            //与注册的区别就是新增了两个字段
                            memberEntity.F_IsDeputy = true;//默认为null
                            memberEntity.F_SuperUId = superUId;
                            memberApp.SubmitForm(memberEntity, memberEntity.F_Id);
                            result = 1000;
                        }
                        
                    }
                    else
                    {
                        result = 1002;
                    }
                }
                else
                {
                    result = 1001;
                }
                //先获取  
            }
            catch (Exception )
            {
                result = 1003;
            }
            finally
            {
                ChangeJson(new ResultData
                {
                    Success = true,
                    ResultCode = result,
                    ResultMessage = ResultHelper.GetMessage(language, "AddDeputyMember", result.ToString()),
                    Data = null
                });
            }
        }
        [WebMethod]
        public void UpdateDeputyMember(string id, string newEmail, string newPassword, int language)
        {
            int result = 0;
            //0 默认值 1000     修改成功 1001 新邮箱已被其他人使用 1002新邮箱已申请会员待激活
            var member = memberApp.GetForm(id);
            if (member != null)
            {
                if (member.F_Email != newEmail)
                {
                    if (!memberApp.CheckUserByEmail(newEmail))
                    {//2、都没有就插入到注册申请表，并且发送邮件返回
                        if (!registerApplyApp.CheckUserByEmail(newEmail))
                        {
                            member.F_Email = newEmail;
                            member.F_Password = newPassword;
                            //与注册的区别就是新增了两个字段 
                            memberApp.SubmitForm(member, member.F_Id);
                            result = 1000;
                        }
                        else
                        {
                            result = 1002;
                        }
                    }
                    else
                    {
                        result = 1001;
                    }
                }
            }
            ChangeJson(new ResultData
            {
                Success = true,
                ResultCode = result,
                ResultMessage = ResultHelper.GetMessage(language, "UpdateDeputyMember", result.ToString()),
                Data = null
            });
        }
        [WebMethod]
        public void DeleteDeputyMember(string id)
        { 
            var member = memberApp.GetForm(id);
            if (member != null)
            {
                member.F_DeleteMark = true;
                //与注册的区别就是新增了两个字段 
                memberApp.SubmitForm(member, member.F_Id);
            } 
            ChangeJson(true);
        }


        /// <summary>
        /// 获取本账号的副账户
        /// </summary>
        /// <param name="page"></param>
        /// <param name="rows"></param>
        /// <param name="userid"></param>  
        [WebMethod]
        public void GetDeputyMemberListByUserId(string memberId)
        {
            List<MemberEntity> modelList = memberApp.GetDeputyMemberListByUserId(memberId);
            List<object> memberList = new List<object>();
            foreach (var item in modelList)
            {
                memberList.Add(new
                {
                    Id = item.F_Id,
                    Email = item.F_Email,
                    Password = item.F_Password,
                    CreatorTime = item.F_CreatorTime,
                });
            }
            ChangeJson(new
            {
                memberList
            });

        }

        #endregion

        #region 查询关键字
        [WebMethod]
        public void AddDocumentSearchKeys(string keys)
        {
            bool result = false;
            try
            {
                //先获取 
                DocumentSearchKeysEntity documentSearchKeysEntity = new DocumentSearchKeysEntity();
                documentSearchKeysEntity.F_Keys = keys;
                documentSearchKeysApp.SubmitForm(documentSearchKeysEntity, documentSearchKeysEntity.F_Id);
                result = true;
            }
            catch (Exception )
            {
                result = false;
            }
            finally
            {
                ChangeJson(result);
            }
        }
        /// <summary>
        /// 返回前10 搜索
        /// </summary>
        [WebMethod]
        public void DocumentSearchKeysStatTop()
        {
            ChangeJson(documentSearchKeysApp.GetStat());
        }
        #endregion

        #region 字典 查询
        /// <summary>
        ///  
        /// </summary>
        [WebMethod]
        public void GetItemsDetailListEnCode(string enCode)
        {
            ChangeJson(itemsDetailApp.GetItemList(enCode));
        }
        #endregion

        #region 获取用户级别 信息
        [WebMethod]
        public void GetMemberStatus(string memberId)
        {
            MemberEntity memberEntity = memberApp.GetForm(memberId);

            ChangeJson(new
            {
                HYType = memberEntity.F_HYType,
                UseState = memberEntity.F_UseState
            });
        }
        #endregion

        #region 连接
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""></param>
        /// <param name="languageType">1中文2英语</param>
        [WebMethod]
        public void GetLinkList(int language)
        {
            var item = itemsApp.GetFormByEnCode("LinkType"); 
            var itemDetails = itemsDetailApp.GetItemListByItemId(item.F_Id);
            List<object> reTypeList = new List<object>();
            List<object> reDetailsList = new List<object>();

            List<LinkEntity> list = linkApp.GetList();
            List<object> linkList = new List<object>();

            foreach (var itemType in itemDetails)
            { 
                var items = new List<object>();
                foreach (var i in list)
                {
                    if (i.F_Type == itemType.F_ItemCode)
                    { 
                        items.Add(new
                        {
                            Id = i.F_Id,
                            Title = (language == 1 ? i.F_ChineseName : i.F_EnglishName),
                            Url=i.F_Url
                        }); 
                    } 
                }
                reTypeList.Add(new
                {
                    TypeId = itemType.F_Id,
                    TypeTitle = (language == 1 ? itemType.F_ItemName : itemType.F_ItemEnName) 
                });
                reDetailsList.Add(new
                {
                    TypeId = itemType.F_Id,
                    TypeTitle = (language == 1 ? itemType.F_ItemName : itemType.F_ItemEnName),
                    ItemList= items 
                });
            }

             
            ChangeJson(new {
                TypeList=  reTypeList,
                DetailsList= reDetailsList
            });
        }
        #endregion

        #region 获取会员信息 修改会员订阅状态
        [WebMethod]
        public void GetMemberInfo(string memberId)
        {
            bool result = false;
            MemberEntity memberEntity = memberApp.GetForm(memberId);
            if (memberEntity != null)
            {
                int downAndPrintCount = 0;//总点数
                int downloadCount = 0;
                if (memberEntity.F_IsDeputy == true)
                {
                    downAndPrintCount = memberApp.GetForm(memberEntity.F_SuperUId).F_DownAndPrintCount;
                    downloadCount = downloadInfoApp.GetDownloadCountByUserId(memberEntity.F_SuperUId);
                }
                else
                {
                    downAndPrintCount = memberEntity.F_DownAndPrintCount;
                    downloadCount = downloadInfoApp.GetDownloadCountByUserId(memberEntity.F_Id);
                }
                result = true;
                ChangeJson(new
                {
                    result,
                    memberEntity.F_HYType,
                    memberEntity.F_Phone,
                    memberEntity.F_Email,
                    memberEntity.F_CreatorTime,
                    memberEntity.F_StartDate,
                    memberEntity.F_EndDate,
                    memberEntity.F_DownAndPrintCount,
                    memberEntity.F_IsSubscription,
                    memberEntity.F_SubscriptionLanguage,
                    memberEntity.F_State,
                    memberEntity.F_IsDeputy,
                    memberEntity.F_SuperUId,
                    memberEntity.F_Country,
                    downAndPrintCount,
                    downloadCount,
                    residueDownloadNum = downAndPrintCount - downloadCount

                });
            }
            else
            {
                ChangeJson(new
                {
                    result
                });
            }
        }
        /// <summary>
        /// 修改会员订阅状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isSubscription"></param>
        [WebMethod]
        public void SetMemberIsSubscription(string memberId, bool isSubscription)
        {
            bool result = false;
            MemberEntity memberEntity = memberApp.GetForm(memberId);
            if (memberEntity != null)
            {
                memberEntity.F_IsSubscription = isSubscription;
                memberApp.SubmitForm(memberEntity, memberEntity.F_Id);
                result = true;
            }
            ChangeJson(new
            {
                result
            });
        }
        #endregion



        #region 提交会员升级申请
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <param name="country"></param>
        /// <param name="phone"></param>
        /// <param name="account"></param>
        /// <param name="hYType"></param> 
        /// <param name="languageType">1中文2英语</param>
        [WebMethod]
        public void MemberUpgradeApplySubmit(string memberId,string country,string phone,string account,int hYType,int language)
        {
            int result = 0;
            //0默认值 1000申请成功 1001会员不存在 1002系统异常 

            try
            {
                var member = memberApp.GetForm(memberId);
                if (member != null)
                {
                    MemberUpgradeApplyEntity model = new MemberUpgradeApplyEntity
                    {
                        F_Account = account,
                        F_Country = country,
                        F_HYType = hYType,
                        F_MemberId = memberId,
                        F_Phone = phone,
                        F_Status = 1

                    };
                    memberUpgradeApplyApp.SubmitForm(model, "");
                    result = 1000;
                }
                else
                {
                    result = 1001;
                } 
            }
            catch (Exception)
            {
                result = 1002;
            }
            finally
            {
                ChangeJson(new ResultData
                {
                    Success = true,
                    ResultCode = result,
                    ResultMessage = ResultHelper.GetMessage(language, "MemberUpgradeApplySubmit", result.ToString()),
                    Data = null
                });
            }
        }
        #endregion

        #region other
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
        #endregion
    }
}
