using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace NFine.Code
{
    /// <summary>
    /// 常用公共类
    /// </summary>
    public class Common
    {
        #region Stopwatch计时器
        /// <summary>
        /// 计时器开始
        /// </summary>
        /// <returns></returns>
        public static Stopwatch TimerStart()
        {
            Stopwatch watch = new Stopwatch();
            watch.Reset();
            watch.Start();
            return watch;
        }
        /// <summary>
        /// 计时器结束
        /// </summary>
        /// <param name="watch"></param>
        /// <returns></returns>
        public static string TimerEnd(Stopwatch watch)
        {
            watch.Stop();
            double costtime = watch.ElapsedMilliseconds;
            return costtime.ToString();
        }
        #endregion

        #region 删除数组中的重复项
        /// <summary>
        /// 删除数组中的重复项
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string[] RemoveDup(string[] values)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < values.Length; i++)//遍历数组成员
            {
                if (!list.Contains(values[i]))
                {
                    list.Add(values[i]);
                };
            }
            return list.ToArray();
        }
        #endregion

        #region 自动生成编号
        /// <summary>
        /// 表示全局唯一标识符 (GUID)。
        /// </summary>
        /// <returns></returns>
        public static string GuId()
        {
            return Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 自动生成编号  201008251145409865
        /// </summary>
        /// <returns></returns>
        public static string CreateNo()
        {
            Random random = new Random();
            string strRandom = random.Next(1000, 10000).ToString(); //生成编号 
            string code = DateTime.Now.ToString("yyyyMMddHHmmss") + strRandom;//形如
            return code;
        }
        #endregion

        #region 生成0-9随机数
        /// <summary>
        /// 生成0-9随机数
        /// </summary>
        /// <param name="codeNum">生成长度</param>
        /// <returns></returns>
        public static string RndNum(int codeNum)
        {
            StringBuilder sb = new StringBuilder(codeNum);
            Random rand = new Random();
            for (int i = 1; i < codeNum + 1; i++)
            {
                int t = rand.Next(9);
                sb.AppendFormat("{0}", t);
            }
            return sb.ToString();

        }
        #endregion

        #region 删除最后一个字符之后的字符
        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }
        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }
        /// <summary>
        /// 删除最后结尾的长度
        /// </summary>
        /// <param name="str"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string DelLastLength(string str, int Length)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            str = str.Substring(0, str.Length - Length);
            return str;
        }
        #endregion

        #region log
        private static Mutex m_mutex = new Mutex();
        //创建记录日志生成状态的日志
        public static void Log(string pathName, string status, string msg)
        {
            string path;
            try
            {
                //配置文件中有指定保存日志地址 
                path = ConfigurationManager.AppSettings["logSaveDirectory"];
                if (string.IsNullOrEmpty(path))
                {
                    path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\" + pathName;
                }
            }
            catch
            {
                path = AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\" + pathName;
            }
            try
            {
                m_mutex.WaitOne();
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string logfile = path + "\\" + DateTime.Today.ToString("yyyy-MM-dd") + ".log";
                System.IO.StreamWriter sw = File.AppendText(logfile);
                sw.WriteLine("{0:-20}\t{1:10}\t{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), status, msg);
                sw.Close();
            }
            catch { }
            finally
            {
                m_mutex.ReleaseMutex();
            }
        }
        #endregion

        /// <summary>
        ///  //此处逻辑，由于富文本问题导致 提交过来的html 缺少 body等标记 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReplenishHtml(string code, string filePath)
        { 
            string filename = Path.GetFileNameWithoutExtension(filePath);

            Regex regImg = new Regex(@"<img\b[^<>]*?\b[\s\t\r\n]*[\s\t\r\n]*?[\s\t\r\n]*[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(code);
            foreach (Match item in matches)
            {
                if (item.Value.IndexOf("/>") > 0)
                {

                }
                else
                {
                    string tempStr = item.Value.Replace(">", " />");
                    code = code.Replace(item.Value, tempStr);
                }
              
            }

            code = code.Replace("<br>", "<br />");
            //code = Regex.Replace(code, "<meta[^>]+>", ""); 这个 html要utf-8编码才能生成pdf 所以此处不能过滤掉这个标记
            code = Regex.Replace(code, "<link[^>]+>", "");
            code = Regex.Replace(code, "<title>", "");
            code = Regex.Replace(code, "</title>", "");
            //code = Regex.Replace(code, "</meta>", "");
            code = Regex.Replace(code, "</link>", "");
            if (!code.ToLower().Contains("<body"))
            {
                code = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.1//EN\" \"http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><head>" +
                    "<meta http-equiv=\"Content-Type\" content=\"application/xhtml+xml; charset=utf-8\" />" +
                    "<title></title>" +
                    "<link href=\"\\Temp\\Word\\" + filename + "_styles.css\" type=\"text/css\" rel=\"stylesheet\"/></head><body style=\"pagewidth: 595.3pt; pageheight: 841.9pt; docgridtype: lines; docgridlinepitch: 16.3pt; \">" +
                                        code + "</body></html>";
            }
            //<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd"><html xmlns="http://www.w3.org/1999/xhtml"><head>
            //<meta http-equiv="Content-Type" content="application/xhtml+xml; charset=utf-8" />
            //<title></title>
            //<link href="180214_法规120181114195602_styles.css" type="text/css" rel="stylesheet"/>
            //</head>
            //<body style="pagewidth:595.3pt;pageheight:841.9pt;docgridtype:lines;docgridlinepitch:16.3pt;">


            //<meta http-equiv="Content-Type" content="application/xhtml+xml; charset=utf-8">
            //<title></title>
            //<link href="\Temp\Word\180214_法规120181114155230\180214_法规120181114155230_styles.css" type="text/css" rel="stylesheet">

            return code;
        }

        /// <summary>
        /// 获取pdf 路径
        /// </summary>
        /// <param name="dir">目录</param>
        /// <param name="chineseTitle">中文标题</param>
        /// <param name="englishTitle">英文标题</param>
        /// <param name="type">1、中文 中文标题 2、英文 英文标题 3 中英文 英文标题+ch&en </param>
        /// <returns></returns>
        public static string GetPdfFileName(string dir, string title  , int type)
        {
            title = title.Replace(" ", "_");

            if (type == 1)
            {
                return dir + "/" + "CH";
            }
            if (type == 2)
            {
                return dir + "/" + "EN";
            }
            if (type == 3)
            {
                return dir + "/" +  "CH_EN";
            }
            return "";
        }

    }
}
