using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using NFine.Application.AuxiliaryManage;
using NFine.Application.ContentManage;
using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Code.Mail;
using NFine.Domain.Entity.ContentManage;

namespace Software.SendEmail
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string strFile = "C:\\SoftwareWeb\\Temp\\Word\\GBT_19233-XXXX_轻型汽车燃料消耗量试验方法_已上传20190130161300\\GBT_19233-XXXX_轻型汽车燃料消耗量试验方法_已上传20190130161300.html";
            //PDFHelper.html2pdf("E://临时//111//GB_11550-200920190123162622//ff.html", "C://Debug//ff.pdf");
            string filename = strFile.Substring(0,strFile.LastIndexOf('\\'));
            string s = "";
            //DocumentApp documentApp = new DocumentApp();

            //List<DocumentEntity> doclist = documentApp.GetDataListAll();
            //foreach (var item in doclist)
            //{
            //    DocumentEntity entity = item;
            //    if(item.F_Type ==1 || item.F_Type == 2)
            //    {
            //        //都是中英文的 64条数据，所以可以都生成 
            //        var dir = entity.F_HtmlUrl.Substring(0, entity.F_HtmlUrl.LastIndexOf('\\'));
            //        var ConfigPath = entity.F_HtmlUrl.Replace(".html",""); 
            //        PDFHelper.html2pdf(ConfigPath + ".html", Common.GetPdfFileName(dir, entity.F_EnglishTitle, 3) + ".pdf");

            //        PDFHelper.html2pdf(ConfigPath + "-中.html", Common.GetPdfFileName(dir, entity.F_ChineseTitle, 1) + ".pdf");

            //        PDFHelper.html2pdf(ConfigPath + "-英.html", Common.GetPdfFileName(dir, entity.F_EnglishTitle, 2) + ".pdf");
            //    }
            //}

            //查询所有 中文 html 英文 html 中英文html 生成 pdf，根据命名规则


            //  PDFHelper.html2pdf("e://f.html", "e://f.pdf");

            //string refile = "<!DOCTYPE ><html xmlns='http://www.w3.org/1999/xhtml'><head></head><body >";

            //if (refile.IndexOf("<meta") >= 0)
            //{
            //    refile = System.Text.RegularExpressions.Regex.Replace(refile, @"<meta[^>]*>", "<meta http-equiv=Content-Type content='text/html;charset=utf-8'>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            //}
            //else
            //{
            //    refile = refile.Replace("<head>", "<head><meta http-equiv=Content-Type content='text/html;charset=utf-8'>");
            //}

            //DateTime dateTime = new DateTime(2018, 1, 30);
            //DateTime dt = dateTime.AddMonths(1);


            //DateTime dt1 = dt.AddMonths(12);

            return;
            //string thissid = Md5.md5("kehaitao_2@163.com" + "|" + "86ffea3e34473cad", 32).ToLower();


            //string code = "<p class=\"Normal\"><img src=\"\\Temp\\Word\\GB_34660-20172018111520582820181116025420\\GB_34660-20172018111520582820181116025420_images\\GB_34660-20172018111520582820181116025420_img1.png\" width=\"554\" height=\"120\" alt=\"img1\"><span style=\"color:#666666;font-size:12pt;font-family:微软雅黑;mso-fareast-font-family:微软雅黑;mso-bidi-font-family:'Times New Roman';\">&nbsp;</span></p><p class=\"Normal\"><img src=\"\\Temp\\Word\\GB_34660-20172018111520582820181116025420\\GB_34660-20172018111520582820181116025420_images\\GB_34660-20172018111520582820181116025420_img2.png\" width=\"554\" height=\"61\" alt=\"img2\"><span style=\"color:#666666;font-size:12pt;font-family:微软雅黑;mso-fareast-font-family:微软雅黑;mso-bidi-font-family:'Times New Roman';\">&nbsp;</span></p>";
            
            //Regex regImg = new Regex(@"<img\b[^<>]*?\b[\s\t\r\n]*[\s\t\r\n]*?[\s\t\r\n]*[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            //return;
            //// 搜索匹配的字符串 
            //MatchCollection matches = regImg.Matches(code);
           
            //string[] sUrlList = new string[matches.Count];

            //// 取得匹配项列表 
            ////foreach (Match match in matches)
            ////{
            ////    sUrlList[i++] = match.Value;
            ////}

            ////string[] sUrlLists = sUrlList;


            //foreach (Match item in matches)
            //{
            //    string tempStr = item.Value.Replace(">", " />");
            //    code = code.Replace(item.Value, tempStr);
            //}
            //string s = code;
            //return;
            MailHelper mailHelper = new MailHelper();
            mailHelper.MailServer = ConfigurationManager.AppSettings["MailServer"];
            mailHelper.MailUserName = ConfigurationManager.AppSettings["MailUserName"];
            mailHelper.MailPassword = ConfigurationManager.AppSettings["MailPassword"];

            string title = "";
            string body = "";

            mailHelper.MailName = "本周更新(Update this week)";
            title = "本周更新(Update this week)";
            body = @"<!doctype html>
<html>
<head>
<meta charset=\""UTF-8\"">
<title>订阅</title>
</head> 
<body style=\""font-family:Cambria,'MicrosoftTaiLe'  'Hoefler Text', 'Liberation Serif', Times, 'Times New Roman',serif; margin: 0; padding: 0; border: 0; outline: 0; line-height: 1.3; text-decoration: none; font-size: 100%; list-style: none; color:#353535; \"">
<!-- Unnamed () -->
<div style=\""padding:40px;\"">

      <div style=\""visibility: visible;\"">
           <h3><p style=\""font-weight:bold;\"">本周更新</p><p><span> Update this week</span></p></h3>
          </div>
      <div style=\""visibility: visible; font-size:15px;\"">
          <p><span>boca</span> 您好,Hello！</p>
          <p><span>本周中国汽车法规标准网发布了以下标准法规，请查阅。</span></p><p><span>This week, the Chinese Auto Regulations has issued the following standards and regulations. Please check it.</span></p>
            
        </div> 
	<div > 
        <div style=\""visibility: visible; border-top:1px solid #B3B3B3; font-size:13px; color:#6F6F6F; display:block; float:left; margin-top:80px;\"">
          <p><span>亲爱的用户，如果您不想再收到此类通知，可以在个人中心选择取消订阅。 </span></p><p><span>Dear users, if you do not want to receive such notification again, you can choose to cancel subscriptions at the individual center.</span></p>
          <p><span>中国汽车法规标准网</span><span> Chinese Auto Regulations</span></p>
        </div>
      </div> 
    </div>
</body>
</html>";

            
            mailHelper.SendMail("kehaitao_0@sina.com", title, body, Convert.ToInt32(ConfigurationManager.AppSettings["MailPost"].ToString()));


            //获取现在时间点

            //附件地址

            //发送邮件
            //MailHelperNew.SendEmail(ConfigurationManager.AppSettings["MailUserName"], ConfigurationManager.AppSettings["MailPassword"], "kehaitao_0@sina.com", title, body, null);
            //mailHelper.SendByThread("kehaitao_0@163.com", title, body, Convert.ToInt32(ConfigurationManager.AppSettings["MailPost"].ToString()));
            //NFine.Code.PDFHelper.html2pdf("e:\\GBT_25978—2018测试文件20181019164159.html", "e:\\GBT_25978—2018测试文件20181019164159.pdf");
            //DocumentContentApp documentContentApp = new DocumentContentApp();
            //var model = documentContentApp.GetByDocumentId("20658da8-0848-431d-a4e2-52030070437b");

            //string s = model.F_ChineseContent;

            //List<string> ss = GetHtmlAttr(s, "p", "TOC-1");


            // string sss = NFine.Code.WordHelper.readhtmlCatalogue("GBT 25978—2018测试文件20181019164159.html");

            //var s = "";
            return;
           

        }


        private static void SendMail()
        {
            try
            {
                DocumentApp documentApp = new DocumentApp();
                SendSubscriptionInfoApp sendSubscriptionInfoApp = new SendSubscriptionInfoApp();
                MemberApp memberApp = new MemberApp();
                var sendsub = sendSubscriptionInfoApp.GetForm();
                //给默认一年之前，第一次就能发出所有数据
                DateTime lastTime = new DateTime(DateTime.Now.Year - 1, DateTime.Now.Month, DateTime.Now.Day);
                if (sendsub != null)
                {
                    lastTime = (DateTime)sendsub.F_LastTime; //上次发送订阅时间，判断是否有新文档
                }
                string url = "http://localhost:8888/doc/showdocinfo?id=";
                if (documentApp.CheckIsHaveNoSubscription(lastTime))
                {
                    int total = 0;
                    List<DocumentEntity> docList = documentApp.GetList(0, 5, (int)NFine.Code.Enum.DocumentType.标准, out total);
                    string docStr = "";
                    foreach (var item in docList)
                    {
                        docStr += "<p style=\"text-decoration:underline;\" ><a href =\"" + url + item.F_Id + "\" ><span >" + item.F_ChineseTitle + "</span></a><br><a href=\"" + url + item.F_Id + "\"><span >" + item.F_EnglishTitle + "</span></a></p>";
                    }
                    //有新文章，发送邮件
                    var userList = memberApp.GetListSubscription();
                    foreach (var item in userList)
                    {
                        //此处异步发邮件  最好记录日志
                        //发送激活邮件 
                        MailHelper mailHelper = new MailHelper();
                        mailHelper.MailServer = ConfigurationManager.AppSettings["MailServer"];
                        mailHelper.MailUserName = ConfigurationManager.AppSettings["MailUserName"];
                        mailHelper.MailPassword = ConfigurationManager.AppSettings["MailPassword"];

                        string title = "";
                        string body = "";

                        mailHelper.MailName = "本周更新(Update this week)";
                        title = "本周更新(Update this week)";
                        body = @"<!doctype html>
<html>
<head>
<meta charset=\""UTF-8\"">
<title>订阅</title>
</head> 
<body style=\""font-family:Cambria,'MicrosoftTaiLe'  'Hoefler Text', 'Liberation Serif', Times, 'Times New Roman',serif; margin: 0; padding: 0; border: 0; outline: 0; line-height: 1.3; text-decoration: none; font-size: 100%; list-style: none; color:#353535; \"">
<!-- Unnamed () -->
<div style=\""padding:40px;\"">

      <div style=\""visibility: visible;\"">
           <h3><p style=\""font-weight:bold;\"">本周更新</p><p><span> Update this week</span></p></h3>
          </div>
      <div style=\""visibility: visible; font-size:15px;\"">
          <p><span>boca</span> 您好,Hello！</p>
          <p><span>本周中国汽车法规标准网发布了以下标准法规，请查阅。</span></p><p><span>This week, the Chinese Auto Regulations has issued the following standards and regulations. Please check it.</span></p>
           
          " + docStr + @" 
        </div> 
	<div > 
        <div style=\""visibility: visible; border-top:1px solid #B3B3B3; font-size:13px; color:#6F6F6F; display:block; float:left; margin-top:80px;\"">
          <p><span>亲爱的用户，如果您不想再收到此类通知，可以在个人中心选择取消订阅。 </span></p><p><span>Dear users, if you do not want to receive such notification again, you can choose to cancel subscriptions at the individual center.</span></p>
          <p><span>中国汽车法规标准网</span><span> Chinese Auto Regulations</span></p>
        </div>
      </div> 
    </div>
</body>
</html>";
                        mailHelper.SendByThread(item.F_Email, title, body, 25);
                    }
                }

                sendsub.F_LastTime = DateTime.Now;
                sendSubscriptionInfoApp.SubmitForm(sendsub, sendsub.F_Id);
            }
            catch (Exception e)
            {
                //邮件未发送成功错误日志记录
                //BLL.Util.Log("log", "err", e.Message);

            }
        }
        /// <summary> 
        /// 获取Html字符串中指定标签的指定属性的值  
        /// </summary> 
        /// <param name="html">Html字符</param> 
        /// <param name="tag">指定标签名</param> 
        /// <param name="attr">指定属性名</param> 
        /// <returns></returns>

        private static List<string> GetHtmlAttr(string html, string tag, string attr) 
        { 
            Regex re = new Regex(@"(<" + tag + @"[\w\W].+?>)");

            MatchCollection imgreg = re.Matches(html);

            List<string> m_Attributes = new List<string>();

            Regex attrReg = new Regex(@"([a-zA-Z1-9_-]+)\s*=\s*(\x27|\x22)([^\x27\x22]*)(\x27|\x22)", RegexOptions.IgnoreCase); 
            for (int i = 0; i < imgreg.Count; i++) 
            { 
                MatchCollection matchs = attrReg.Matches(imgreg[i].ToString()); 
                for (int j = 0; j < matchs.Count; j++) 
                { 
                    GroupCollection groups = matchs[j].Groups; 
                    if (attr.ToUpper() == groups[1].Value.ToUpper()) 
                    { 
                        m_Attributes.Add(groups[3].Value); 
                        break; 
                    } 
                } 
            } 
            return m_Attributes; 
        } 
    }
}