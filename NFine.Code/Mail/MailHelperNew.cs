using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NFine.Code.Mail
{
    public class MailHelperNew
    {
        public static bool SendEmail(string fromEmail, string fromPsaaWord, string toEmail, string subject, string body, string[] attachmentsPath)
        {
            List<string> toEmails = new List<string>();
            var emails = toEmail.Split(',');
            if (emails.Length > 0)
            {
                foreach (var item in emails)
                {
                    toEmails.Add(item);
                }
            }
            return SendEmail(fromEmail, fromPsaaWord, toEmails, subject, body, attachmentsPath);
        }
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="fromEmail">发送者邮箱址</param>
        /// <param name="fromPsaaWord">发送者邮箱密码</param>
        /// <param name="toEmail">收件邮箱址</param>
        /// <param name="subject">邮件主题</param>
        /// <param name="body">邮件主要内容</param>
        /// <param name="attachmentsPath">邮件附件(附件null)</param>
        /// <returns>发送功返true,失败false</returns>
        public static bool SendEmail(string fromEmail, string fromPsaaWord, List<string> toEmail, string subject, string body, string[] attachmentsPath)
        {
            bool Success = true;
            var errorMsg = new System.Text.StringBuilder();
            if (!Regex.IsMatch(fromEmail, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
            {
                errorMsg.Append("参数fromEmail格式不正确!");
                Success = false;
            }
            foreach (var item in toEmail)
            {
                if (!Regex.IsMatch(item, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
                {
                    errorMsg.Append("参数toEmail格式不正确!");
                    Success = false;
                }
            }

            if (subject.Trim() == "")
            {
                errorMsg.Append("不支持无主题邮件!");
                Success = false;
            }
            if (body.Trim() == "")
            {
                errorMsg.Append("不支持无内容邮件!");
                Success = false;
            }
            try
            {
                SendReady(fromEmail, fromPsaaWord, toEmail, subject, body, attachmentsPath);
            }
            catch (Exception err)
            {
                Success = false;
                throw new Exception(errorMsg.ToString() + err);
            }
            return Success;
        }
        public static void SendReady(string fromEmail, string fromPsaaWord, List<string> toEmail, string subject, string body, string[] attachmentsPath)
        {
            //验证合邮箱表达式,确返true
            if (!Regex.IsMatch(fromEmail, @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
            {
                //邮箱合
            }
            //发件址
            var from = new MailAddress(fromEmail);
            var message = new MailMessage()
            {
                Subject = subject,//主题
                SubjectEncoding = System.Text.Encoding.UTF8,
                Body = body,//内容
                BodyEncoding = System.Text.Encoding.UTF8,
                IsBodyHtml = true,
            };
            message.From = from;
            foreach (var item in toEmail)
            {
                //收件址
                var to = new MailAddress(item);
                message.To.Add(to);
            }
            //附件情况添加附件
            try
            {
                if (attachmentsPath != null && attachmentsPath.Length > 0)
                {
                    foreach (string path in attachmentsPath)
                    {
                        var attachFile = new Attachment(path, MediaTypeNames.Application.Octet);
                        var disposition = attachFile.ContentDisposition;
                        //disposition.CreationDate = File.GetCreationTime(path);
                        //disposition.ModificationDate = File.GetLastWriteTime(path);
                        //disposition.ReadDate = File.GetLastAccessTime(path);
                        attachFile.ContentType.MediaType = MediaTypeNames.Application.Octet;
                        //disposition.DispositionType = MediaTypeNames.Application.Octet;
                        //attachFile.Name = Path.GetFileName(path).Split('_')[0] + ".xls";
                        attachFile.Name = Path.GetFileName(path).Split('_')[0];
                        message.Attachments.Add(attachFile);
                    }
                }
            }
            catch (Exception err)
            {
                throw new Exception("添加附件错误:" + err);
            }
            try
            {
                //部邮件服务器均加smtp.前缀
                //var client = new SmtpClient("smtp.ym.163.com");
                //var client = new SmtpClient("smtpcom.263xmail.com");
                var client = new SmtpClient(GetSmtpClient(fromEmail));
                SendMail(client, from, fromPsaaWord, message);
            }
            catch (SmtpException err)
            {
                //错误原没找服务器则尝试加smtp.前缀服务器
                if (err.StatusCode == SmtpStatusCode.GeneralFailure)
                {
                    try
                    {
                        //些邮件服务器加smtp.前缀
                        SmtpClient client = new SmtpClient(from.Host);
                        SendMail(client, from, fromPsaaWord, message);
                    }
                    catch (SmtpException)
                    { }
                }
                else
                { }
            }
        }


        //根据指定参数发送邮件
        public static void SendMail(SmtpClient client, MailAddress from, string password, MailMessage message)
        {
            //使用默认凭证,注意句必须放client.Credentials面
            client.UseDefaultCredentials = true;
            //指定用户名、密码
            client.Credentials = new NetworkCredential(from.Address, password);
            //邮件通网络发送服务器
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                client.Send(message);
            }
            finally
            {
                //及释放占用资源
                message.Dispose();
            }
        }

        /// <summary>
        /// 根据邮箱 选择Smtp服务器
        /// </summary>
        /// <param name="fromEmail"></param>
        /// <returns></returns>
        private static string GetSmtpClient(string fromEmail)
        {
            string strTemp = fromEmail.Substring(fromEmail.IndexOf('@') + 1, fromEmail.Length - fromEmail.IndexOf('@') - 1);
            //switch (strTemp)
            //{ 
            //    case ".com":
            //        return "smtp.ym.163.com";
            //    case "yingu.com":
            //        return "smtp.qiye.163.com";
            //    case "163.com":
            //        return "smtp.ym.163.com";
            //    case "dfyg.cn":
            //        return "smtpcom.263xmail.com";
            //}
            return "smtp.163.com";
        }
    }
}
