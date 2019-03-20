using System;
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace NFine.Code
{
    public class MailHelper
    {
        /// <summary>
        /// 邮件服务器地址
        /// </summary>
        public string MailServer { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string MailUserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string MailPassword { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string MailName { get; set; }
        /// <summary>
        /// 目前唯一一个好用的发邮件方法 ssl加密的  异步的
        /// </summary>
        /// <param name="to"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="port"></param>
        public void SendMail(string to, string title, string body, int port)
        {
            new Thread(new ThreadStart(delegate ()
            {
                try
                {
                    Common.Log("log", "log", "11");
                    MailMessage mm = new MailMessage();
                    Common.Log("log", "log", "12");
                    mm.From = new MailAddress(MailUserName, MailName, System.Text.Encoding.UTF8);
                    Common.Log("log", "log", "13");
                    mm.To.Add(to);
                    Common.Log("log", "log", "14");
                    mm.Subject = title;
                    mm.Body = body;
                    Common.Log("log", "log", "15");
                    mm.BodyEncoding = System.Text.Encoding.UTF8;
                    mm.IsBodyHtml = true;

                    Common.Log("log", "log", "16");
                    SmtpClient sc = new SmtpClient();
                    NetworkCredential nc = new NetworkCredential();

                    Common.Log("log", "log", "17");
                    nc.UserName = MailUserName;
                    Common.Log("log", "log", "18");
                    nc.Password = MailPassword;
                    Common.Log("log", "log", "19");
                    sc.UseDefaultCredentials = true;
                    Common.Log("log", "log", "20");
                    sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                    Common.Log("log", "log", "21");
                    sc.Credentials = nc;
                    Common.Log("log", "log", "22");
                    sc.Host = MailServer;
                    Common.Log("log", "log", "23");
                    sc.EnableSsl = false;
                    Common.Log("log", "log", "24");
                    sc.Port = 80;
                    Common.Log("log", "log", "1" );
                    sc.Send(mm);
                    Common.Log("log", "log", "2"  );
                }
                catch (Exception e)
                {
                    Common.Log("log", "err", "发邮件错误1=" + e.Message);
                    Common.Log("log", "err", "发邮件错误=" + e.InnerException);
                }
            })).Start();
        }


        /// <summary>
        /// 批量发邮件  异步的
        /// </summary>
        /// <param name="toList"></param>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="port"></param>
        public void SendBatchMail(string [] toList, string title, string body )
        {
            new Thread(new ThreadStart(delegate ()
            {
                foreach (string  to in toList)
                {
                    try
                    { 
                        MailMessage mm = new MailMessage(); 
                        mm.From = new MailAddress(MailUserName, MailName, System.Text.Encoding.UTF8);
                        mm.To.Add(to);
                        mm.Subject = title;
                        mm.Body = body;
                        mm.BodyEncoding = System.Text.Encoding.UTF8;
                        mm.IsBodyHtml = true;
                        SmtpClient sc = new SmtpClient();
                        NetworkCredential nc = new NetworkCredential();
                        nc.UserName = MailUserName;
                        nc.Password = MailPassword;
                        sc.UseDefaultCredentials = true;
                        sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                        sc.Credentials = nc;
                        sc.Host = MailServer;
                        sc.EnableSsl = false;
                        sc.Port = 80;
                        sc.Send(mm);
                    }
                    catch (Exception e)
                    {
                        Common.Log("log", "err", "发邮件错误1=" + e.Message);
                        Common.Log("log", "err", "发邮件错误=" + e.InnerException);
                    }
                }
                
            })).Start();
        }

        #region 不好使的方法
        /// <summary>
        /// 同步发送邮件
        /// </summary>
        /// <param name="to">收件人邮箱地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="encoding">编码</param>
        /// <param name="isBodyHtml">是否Html</param>
        /// <param name="enableSsl">是否SSL加密连接</param>
        /// <returns>是否成功</returns>
        public bool Send(string to, string subject, string body,  int port = 25, string encoding = "UTF-8", bool isBodyHtml = true, bool enableSsl = false)
        {
            try
            {
                MailMessage message = new MailMessage();
                // 接收人邮箱地址
                message.To.Add(new MailAddress(to));
                message.From = new MailAddress(MailUserName, MailName);
                message.BodyEncoding = Encoding.GetEncoding(encoding);
                message.Body = body;
                //GB2312
                message.SubjectEncoding = Encoding.GetEncoding(encoding);
                message.Subject = subject;
                message.IsBodyHtml = isBodyHtml;
                 
                SmtpClient smtpclient = new SmtpClient(MailServer, port);
                smtpclient.Credentials = new System.Net.NetworkCredential(MailUserName, MailPassword);
                //SSL连接
                //smtpclient.EnableSsl = enableSsl;
                smtpclient.EnableSsl = true;
                smtpclient.Send(message);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 异步发送邮件 独立线程
        /// </summary>
        /// <param name="to">邮件接收人</param>
        /// <param name="title">邮件标题</param>
        /// <param name="body">邮件内容</param>
        /// <param name="port">端口号</param>
        /// <returns></returns>
        public void SendByThread(string to, string title, string body, int port = 25)
        {
            new Thread(new ThreadStart(delegate()
            {
                try
                {
                    SmtpClient smtp = new SmtpClient();
                    //邮箱的smtp地址
                    smtp.Host = MailServer;
                    //端口号
                    smtp.Port = port;
                    smtp.EnableSsl = true;
                    //构建发件人的身份凭据类
                    smtp.Credentials = new NetworkCredential(MailUserName, MailPassword);
                    //构建消息类
                    MailMessage objMailMessage = new MailMessage();
                    //设置优先级
                    objMailMessage.Priority = MailPriority.High;
                    //消息发送人
                    objMailMessage.From = new MailAddress(MailUserName, MailName, System.Text.Encoding.UTF8);
                    //收件人
                    objMailMessage.To.Add(to);
                    //标题
                    objMailMessage.Subject = title.Trim();
                    //标题字符编码
                    objMailMessage.SubjectEncoding = System.Text.Encoding.UTF8;
                    //正文
                    objMailMessage.Body = body.Trim();
                    objMailMessage.IsBodyHtml = true;
                    //内容字符编码
                    objMailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                    //发送
                    smtp.Send(objMailMessage);
                }
                catch (Exception e)
                {
                    Common.Log("log", "err", "发邮件错误"+e.InnerException.Message);
                    Common.Log("log", "err", "发邮件错误" + e.ToJson() );
                    Common.Log("log", "err", "发邮件错误" + e.Message);
                    throw;
                }

            })).Start();
        }

        //public void SendMailUseGmail(string to, string title, string body, int port )
        //{
        //    System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
        //    msg.To.Add(to);

        //    msg.From = new MailAddress(MailUserName, MailName, System.Text.Encoding.UTF8);
        //    /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
        //    msg.Subject = "这是测试邮件";//邮件标题    
        //    msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码    
        //    msg.Body = "邮件内容";//邮件内容    
        //    msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码    
        //    msg.IsBodyHtml = false;//是否是HTML邮件    
        //    msg.Priority = MailPriority.High;//邮件优先级    
        //    SmtpClient client = new SmtpClient();


        //    client.Credentials = new NetworkCredential(MailUserName, MailPassword);
        //    //上述写你的GMail邮箱和密码    
        //    client.Port = port;//Gmail使用的端口    
        //    client.Host = MailServer;
        //    client.EnableSsl = true;//经过ssl加密    
        //    object userState = msg;
        //    try
        //    {
        //        client.Send(msg);
        //        //简单一点儿可以client.Send(msg);   
        //    }
        //    catch (System.Net.Mail.SmtpException ex)
        //    {
        //        Common.Log("log", "err", "发邮件错误" + ex.Message);
        //    }
        //}

      


        /// <summary>
        /// Template File Helper 
        /// </summary>
        /// <param name="templatePath">Templet Path</param>
        /// <param name="values">NameValueCollection</param>
        /// <returns>string</returns>
        public static string BulidByFile(string templatePath, NameValueCollection values)
        {
            return BulidByFile(templatePath, values, "[$", "]");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="templatePath"></param>
        /// <param name="values"></param>
        /// <param name="prefix"></param>
        /// <param name="postfix"></param>
        /// <returns></returns>
        public static string BulidByFile(string templatePath, NameValueCollection values, string prefix, string postfix)
        {
            StreamReader reader = null;
            string template = string.Empty;
            try
            {
                reader = new StreamReader(templatePath);
                template = reader.ReadToEnd();
                reader.Close();
                if (values != null)
                {
                    foreach (string key in values.AllKeys)
                    {
                        template = template.Replace(string.Format("{0}{1}{2}", prefix, key, postfix), values[key]);
                    }
                }
            }
            catch
            {

            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
            return template;
        }
        public static string Build(string template, NameValueCollection values, string prefix, string postfix)
        {
            if (values != null)
            {
                foreach (DictionaryEntry entry in values)
                {
                    template = template.Replace(string.Format("{0}{1}{2}", prefix, entry.Key, postfix), entry.Value.ToString());
                }
            }
            return template;
        }

        #endregion
    }
}
