using NFine.Application.AuxiliaryManage;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.AuxiliaryManage;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Web.Script.Services;
using System.Web.Services;

namespace Software.WebService
{
    /// <summary>
    /// LoginService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://47.92.160.129:8080/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]
    public class LoginService : System.Web.Services.WebService
    {
        private UserApp userApp = new UserApp();
        private MemberApp memberApp = new MemberApp();
        private RegisterApplyApp registerApplyApp = new RegisterApplyApp();

        private string HostAddr = ConfigurationManager.AppSettings["HostQAddr"];

        [WebMethod]
        public void Register(string email, string userPassword, bool isSubscription, int subscriptionLanguage)
        {
            bool result = false;
            try
            {
                //判断没有账号才能新增
                if (!memberApp.CheckUserByEmail(email))
                {
                    MemberEntity memberEntity = new MemberEntity();
                    memberEntity.F_Email = email;
                    memberEntity.F_Password = userPassword;
                    memberEntity.F_DeleteMark = false;
                    memberEntity.F_HYType = 0;
                    memberEntity.F_IsSubscription = isSubscription;
                    memberEntity.F_SubscriptionLanguage = subscriptionLanguage;
                    memberApp.SubmitForm(memberEntity, "");
                    result = true;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            finally
            {
                ChangeJson(result);
            }
        }
        /// <summary>
        /// 登录验证
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        [WebMethod]
        public void CheckLogin(string username, string password, int language  )
        {
            // 1000 登录成功，1001密码不对，1002邮箱不存在 1003 未激活
            int LoginStatus = 0;
            MemberEntity memberEntity = memberApp.GetFormByEmail(username);
            var Data = new
            {
                memberId = "",
                HYType = 0,
                UseState = 0
            };
            if (memberEntity != null)
            {
                if (password == memberEntity.F_Password)
                {
                    Data = new
                    {
                        memberId = memberEntity.F_Id,
                        HYType = memberEntity.F_HYType,
                        UseState = memberEntity.F_UseState
                    };
                       LoginStatus = 1000;
                }
                else
                {
                    LoginStatus = 1001;
                }
            }
            else
            {
                if (registerApplyApp.CheckUserByEmail(username))
                {
                    LoginStatus = 1003;
                }
                else
                {
                    LoginStatus = 1002;
                }
              
            }
            
            ChangeJson(new ResultData
            {
                Success = true,
                ResultCode = LoginStatus,
                ResultMessage = ResultHelper.GetMessage(language, "CheckLogin", LoginStatus.ToString()),
                Data = Data
            });
        }

        #region 注册激活    点击激活邮件后，到密码提交页面，输入密码后调用的方法 创建用户
        /// <summary>
        /// 注册并发送激活邮件
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="isSubscription">是否订阅</param> 
        /// <param name="subscriptionLanguage">1中文2英文</param> 
        [WebMethod]
        public void CheckAndRegister(string email, string password, bool isSubscription, int subscriptionLanguage, int language  )
        {
            int result = 0;
            //0默认值 1000 发送激活邮件成功 1001 次会员已存在 1002次会员已有注册申请
            //1、验证邮箱在会员表是否存在，验证邮件在注册申请表是否存在
            if (!memberApp.CheckUserByEmail(email))
            {//2、都没有就插入到注册申请表，并且发送邮件返回
                if (!registerApplyApp.CheckUserByEmail(email))
                {
                    RegisterApplyEntity model = new RegisterApplyEntity();
                    model.F_Email = email;
                    model.F_Password = password;
                    model.F_IsSubscription = isSubscription;
                    model.F_SubscriptionLanguage = subscriptionLanguage;
                    //发送激活邮件 
                    MailHelper mailHelper = new MailHelper();
                    mailHelper.MailServer = ConfigurationManager.AppSettings["MailServer"];
                    mailHelper.MailUserName = ConfigurationManager.AppSettings["MailUserName"];
                    mailHelper.MailPassword = ConfigurationManager.AppSettings["MailPassword"];
                    int MailPort = Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"]);
                     
                    model.F_SecretKey = Md5.md5(Common.CreateNo(), 16).ToLower(); 
                    registerApplyApp.SubmitForm(model, ""); 
                    string sid   = Md5.md5(email + "|" + model.F_SecretKey, 32).ToLower();
                    string url = HostAddr + "SignIn?sid=" + sid + "&id=" + model.F_Id;
                    string title = "";
                    string body = "";

                    mailHelper.MailName = "中国汽车法规标准网账号激活邮件(Chinese Auto Regulation saccount activation mail)";
                    title = "中国汽车法规标准网账号激活邮件(Chinese Auto Regulation saccount activation mail)";
                    string templetpath = Server.MapPath("jihuo.html");
                    NameValueCollection myCol = new NameValueCollection();
                    myCol.Add("link", url);
                    body = MailHelper.BulidByFile(templetpath, myCol);

                    mailHelper.SendMail(email, title, body, Convert.ToInt32(ConfigurationManager.AppSettings["MailPost"].ToString()));


                    //mailHelper.SendByThread(email, title, body, MailPort);
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
            ChangeJson(new ResultData
            {
                Success = true,
                ResultCode = result,
                ResultMessage = ResultHelper.GetMessage(language, "CheckAndRegister", result.ToString()),
                Data =null
            });
        }

        /// <summary>
        /// 单独验证邮箱是否存在
        /// </summary>
        /// <param name="email"></param>
        [WebMethod]
        public void CheckUserByEmail(string email, int language)
        {
            int result = 0;
            // 1000、验证邮箱在注册申请表、会员表都没有数据，可以注册 1001、已有此会员 1002、此邮箱已注册申请、待激活
            //1、验证邮箱在会员表是否存在，验证邮件在注册申请表是否存在
            if (!memberApp.CheckUserByEmail(email))
            {//2、都没有就插入到注册申请表，并且发送邮件返回
                if (!registerApplyApp.CheckUserByEmail(email))
                {
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
            ChangeJson(new ResultData
            {
                Success = true,
                ResultCode = result,
                ResultMessage = ResultHelper.GetMessage(language, "CheckUserByEmail", result.ToString()),
                Data = null
            });
        }
        /// <summary>
        /// 单独发送激活邮件
        /// </summary>
        /// <param name="email"></param>
        [WebMethod]
        public void SendActivateEmail(string email, int language)
        {
            int result = 0;
            // ResultCode  1000、发送成功 1001、已有此会员 1002、此邮箱已注册申请、待激活
            //1、验证邮箱在会员表是否存在  
            if (!memberApp.CheckUserByEmail(email))
            {//2、验证邮箱是否有注册申请，有的话 发送邮件
                if (registerApplyApp.CheckUserByEmail(email))
                {
                    RegisterApplyEntity registerApplyEntity = registerApplyApp.GetFormByEmail(email);
                    MailHelper mailHelper = new MailHelper();
                    mailHelper.MailServer = ConfigurationManager.AppSettings["MailServer"];
                    mailHelper.MailUserName = ConfigurationManager.AppSettings["MailUserName"];
                    mailHelper.MailPassword = ConfigurationManager.AppSettings["MailPassword"]; 
                    int mailPort = Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"]);

                    string sid = "";
                    sid = Md5.md5(email + "|" + registerApplyEntity.F_SecretKey, 32).ToLower();
                    string url = HostAddr + "SignIn?sid=" + sid + "&id=" + registerApplyEntity.F_Id;
                    string title = "";
                    string body = "";

                    mailHelper.MailName = "中国汽车法规标准网账号激活邮件(Chinese Auto Regulation saccount activation mail)";
                    title = "中国汽车法规标准网账号激活邮件(Chinese Auto Regulation saccount activation mail)";

                    string templetpath = Server.MapPath("jihuo.html");
                    NameValueCollection myCol = new NameValueCollection();
                    myCol.Add("link", url);
                    body = MailHelper.BulidByFile(templetpath, myCol);
                    mailHelper.SendMail(email, title, body, mailPort); 
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
            ChangeJson(new ResultData
            {
                Success = true,
                ResultCode = result,
                ResultMessage = ResultHelper.GetMessage(language, "SendActivateEmail", result.ToString()),
                Data = null
            });
        }

        /// <summary>
        /// 点击激活邮件连接后，创建用户
        /// </summary>
        /// <param name="email"></param>
        /// <param name="sid"></param>
        [WebMethod]
        public void ActivateMember(string id, string sid, int language)
        {
            int result = 0;
            //0初始值、1000激活成功、  1001已有此会员、  1002邮箱没有注册申请、1003激活码验证失败
            try
            {
                RegisterApplyEntity registerApplyEntity = registerApplyApp.GetForm(id);
                
                if (registerApplyEntity!=null  )
                {
                    //判断没有会员账号，有注册申请账号才能新增
                    if (!memberApp.CheckUserByEmail(registerApplyEntity.F_Email))
                    {

                        //验证激活码
                        string thissid = Md5.md5(registerApplyEntity.F_Email + "|" + registerApplyEntity.F_SecretKey, 32).ToLower();
                        if (sid == thissid)
                        {
                            MemberEntity memberEntity = new MemberEntity();
                            memberEntity.F_Email = registerApplyEntity.F_Email;
                            memberEntity.F_Password = registerApplyEntity.F_Password;
                            memberEntity.F_DeleteMark = false;
                            memberEntity.F_HYType = 0;
                            memberEntity.F_IsDeputy = false;
                            memberEntity.F_State = 1;

                            memberEntity.F_IsSubscription = registerApplyEntity.F_IsSubscription;
                            memberEntity.F_SubscriptionLanguage = registerApplyEntity.F_SubscriptionLanguage;
                            memberApp.SubmitForm(memberEntity, "");
                            result = 1000;
                        }
                        else
                        {
                            result = 1003;
                        }
                    }
                    else
                    {
                        result = 1001;
                    }
                }
                else
                {
                    result = 1002;
                }
            }
            catch (Exception)
            {
                result = 1003;
            }
            finally
            {
                ChangeJson(new ResultData
                {
                    Success = true,
                    ResultCode = result,
                    ResultMessage = ResultHelper.GetMessage(language, "ActivateMember", result.ToString()),
                    Data = null
                });
            }
        }

        /// <summary>
        /// 单独发送邮件测试
        /// </summary>
        /// <param name="email"></param>
        [WebMethod]
        public void TestSendEmail(string email)
        {
            MailHelper mailHelper = new MailHelper();
            mailHelper.MailServer = ConfigurationManager.AppSettings["MailServer"];
            mailHelper.MailUserName = ConfigurationManager.AppSettings["MailUserName"];
            mailHelper.MailPassword = ConfigurationManager.AppSettings["MailPassword"];
            int mailPort = Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"]); 

            string sid = "";
            sid = Md5.md5(email + "|111111", 32).ToLower();
            string url = HostAddr + "SignIn?sid=" + sid + "&email=" + email;
            string title = "";
            string body = "";

            mailHelper.MailName = "中国汽车法规标准网账号激活邮件(Chinese Auto Regulation saccount activation mail)";
            title = "中国汽车法规标准网账号激活邮件(Chinese Auto Regulation saccount activation mail)";

            string templetpath = Server.MapPath("jihuo.html");
            NameValueCollection myCol = new NameValueCollection();
            Common.Log("log", "log", "templetpath=" + templetpath);
            myCol.Add("link", url);
            body = MailHelper.BulidByFile(templetpath, myCol);
            Common.Log("log", "log", "body=" + body);
            mailHelper.SendMail(email, title, body, mailPort);

            // mailHelper.SendByThread(email, title, body, MailPort);

            Common.Log("log", "log", "over");
        }

        #endregion
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

    }
}