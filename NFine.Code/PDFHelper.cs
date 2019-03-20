using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web;

namespace NFine.Code
{
    public class PDFHelper
    {
        #region 水印
        /// <summary>
        /// 加图片水印
        /// </summary>
        /// <param name="inputfilepath"></param>
        /// <param name="outputfilepath"></param>
        /// <param name="ModelPicName"></param>
        /// <param name="top"></param>
        /// <param name="left"></param>
        /// <returns></returns>
        public static bool AddImageWatermarkPDF(string inputfilepath, string outputfilepath, string ModelPicName, float top, float left)
        {
            //throw new NotImplementedException();
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            try
            {
                pdfReader = new PdfReader(inputfilepath);

                int numberOfPages = pdfReader.NumberOfPages;

                iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(1);

                float width = psize.Width;

                float height = psize.Height;

                pdfStamper = new PdfStamper(pdfReader, new FileStream(outputfilepath, FileMode.Create));

                PdfContentByte waterMarkContent;

                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(ModelPicName);

                image.GrayFill = 20;//透明度，灰色填充
                                    //image.Rotation//旋转
                                    //image.RotationDegrees//旋转角度
                                    //水印的位置
                if (left < 0)
                {
                    left = width / 2 - image.Width + left;
                }

                //image.SetAbsolutePosition(left, (height - image.Height) - top);
                image.SetAbsolutePosition(left, (height / 2 - image.Height) - top);


                //每一页加水印,也可以设置某一页加水印
                for (int i = 1; i <= numberOfPages; i++)
                {
                    //waterMarkContent = pdfStamper.GetUnderContent(i);//内容下层加水印
                    waterMarkContent = pdfStamper.GetOverContent(i);//内容上层加水印

                    waterMarkContent.AddImage(image);
                }
                //strMsg = "success";
                return true;
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {

                if (pdfStamper != null)
                    pdfStamper.Close();

                if (pdfReader != null)
                    pdfReader.Close();
            }
        }
        /// <summary>
        /// 添加普通偏转角度文字水印
        /// </summary>
        /// <param name="inputfilepath"></param>
        /// <param name="outputfilepath"></param>
        /// <param name="waterMarkName"></param>
        /// <param name="permission"></param>
        public static void AddWordWatermark2PDF(string inputfilepath, string outputfilepath, string waterMarkName)
        {
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            try
            {
                pdfReader = new PdfReader(inputfilepath);
                pdfStamper = new PdfStamper(pdfReader, new FileStream(outputfilepath, FileMode.Create));
                int total = pdfReader.NumberOfPages + 1;
                iTextSharp.text.Rectangle psize = pdfReader.GetPageSize(1);
                float width = psize.Width;
                float height = psize.Height;
                PdfContentByte content;
                BaseFont font = BaseFont.CreateFont(@"C:\WINDOWS\Fonts\SIMFANG.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                PdfGState gs = new PdfGState();
                for (int i = 1; i < total; i++)
                {
                    content = pdfStamper.GetOverContent(i);//在内容上方加水印
                                                           //content = pdfStamper.GetUnderContent(i);//在内容下方加水印
                                                           //透明度
                    gs.FillOpacity = 0.5f;
                    content.SetGState(gs);
                    content.SetGrayFill(0.5f);
                    //开始写入文本
                    content.BeginText();
                    //content.SetColorFill(BaseColor.LIGHT_GRAY);
                    //content.SetFontAndSize(font, 100);
                    //content.SetTextMatrix(0, 0);
                    //content.ShowTextAligned(Element.ALIGN_CENTER, waterMarkName, width / 2 - 50, height / 2 - 50, 55);
                    content.SetColorFill(BaseColor.GRAY);
                    content.SetFontAndSize(font, 36);
                    content.ShowTextAligned(Element.ALIGN_CENTER, waterMarkName, width / 2 - 50, height / 2 - 50, 55);
                    content.EndText();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                if (pdfStamper != null)
                    pdfStamper.Close();

                if (pdfReader != null)
                    pdfReader.Close();
            }
        }



        /// <summary>
        /// 添加倾斜水印
        /// </summary>
        /// <param name="inputfilepath"></param>
        /// <param name="outputfilepath"></param>
        /// <param name="waterMarkName"></param>
        /// <param name="userPassWord"></param>
        /// <param name="ownerPassWord"></param>
        /// <param name="permission"></param>
        public static void setWatermark(string inputfilepath, string outputfilepath, string waterMarkName, string userPassWord, string ownerPassWord, int permission)
        {
            PdfReader pdfReader = null;
            PdfStamper pdfStamper = null;
            try
            {
                pdfReader = new PdfReader(inputfilepath);
                pdfStamper = new PdfStamper(pdfReader, new FileStream(outputfilepath, FileMode.Create));
                // 设置密码  
                //pdfStamper.SetEncryption(false,userPassWord, ownerPassWord, permission);

                int total = pdfReader.NumberOfPages + 1;
                PdfContentByte content;
                BaseFont font = BaseFont.CreateFont(@"C:\WINDOWS\Fonts\SIMFANG.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                PdfGState gs = new PdfGState();
                gs.FillOpacity = 0.2f;//透明度

                int j = waterMarkName.Length;
                char c;
                int rise = 0;
                for (int i = 1; i < total; i++)
                {
                    rise = 500;
                    content = pdfStamper.GetOverContent(i);//在内容上方加水印
                                                           //content = pdfStamper.GetUnderContent(i);//在内容下方加水印

                    content.BeginText();
                    content.SetColorFill(BaseColor.DARK_GRAY);
                    content.SetFontAndSize(font, 50);
                    // 设置水印文字字体倾斜 开始
                    if (j >= 15)
                    {
                        content.SetTextMatrix(200, 120);
                        for (int k = 0; k < j; k++)
                        {
                            content.SetTextRise(rise);
                            c = waterMarkName[k];
                            content.ShowText(c + "");
                            rise -= 20;
                        }
                    }
                    else
                    {
                        content.SetTextMatrix(180, 100);
                        for (int k = 0; k < j; k++)
                        {
                            content.SetTextRise(rise);
                            c = waterMarkName[k];
                            content.ShowText(c + "");
                            rise -= 18;
                        }
                    }
                    // 字体设置结束
                    content.EndText();
                    // 画一个圆
                    //content.Ellipse(250, 450, 350, 550);
                    //content.SetLineWidth(1f);
                    //content.Stroke();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                if (pdfStamper != null)
                    pdfStamper.Close();

                if (pdfReader != null)
                    pdfReader.Close();
            }
        }




        #endregion end水印


        /// <summary>
        /// html网页信息转成pdf文档并下载
        /// </summary>
        /// <param name="htmlFileName">html文件绝对路劲</param>
        /// <param name="fileName">pdf文档生成路径</param>
        /// <returns></returns>
        public static bool html2pdf(string htmlFileName, string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }


            //if (!File.Exists(fileName))
            //{
            string file_flvbind = Configs.GetValue("wkhtmltopdfAddr");//Application.StartupPath + @"\dll\wkhtmltopdf.exe";
                                                                      //
            ProcessStartInfo pinfo = new ProcessStartInfo(file_flvbind);
            ////pinfo.WorkingDirectory = htmlFileName.Substring(0, htmlFileName.LastIndexOf("\\") + 1);
            //pinfo.WorkingDirectory = strHtmlSavedPath;
            //设置参数
            StringBuilder sb = new StringBuilder();
            sb.Append(" --page-size A4 ");
            sb.Append("--footer-line --footer-font-size 7 ");
            sb.Append("--footer-center \"The information on this page is provided by Dynaso Consulting to its clients.For more information, please visit www.chineseautoregs.com.\" ");
            sb.Append(htmlFileName);

            sb.Append(" " + fileName);

            pinfo.Arguments = sb.ToString();
            //隐藏窗口
            pinfo.WindowStyle = ProcessWindowStyle.Hidden;
            ////启动程序
            //Process p = System.Diagnostics.Process.Start(@"D:\wkhtmltopdf\wkhtmltopdf.exe", sb.ToString());
            Process p = Process.Start(pinfo);
            p.WaitForExit();
            Common.Log("log", "log", "ppp" + p.ExitCode);
            if (p.ExitCode == 0)
            {
                return true;
            }
            else
            {

                return false;
            }
            //}
            //else
            //{

            //    return true;
            //}
        }
        /// <summary>
		/// html网页信息转成pdf文档并下载
		/// </summary>
		/// <param name="htmlFileName">html文件绝对路劲</param>
		/// <param name="fileName">pdf文档生成路径</param>
		/// <returns></returns>
		public static bool htmlTopdf(string htmlFileName, string fileName)
        {

            if (!File.Exists(fileName))
            {
                ////启动程序
                Process p = System.Diagnostics.Process.Start(Configs.GetValue("wkhtmltopdfAddr"), "http://192.168.31.191:8888/GBT_25978—2018测试文件20181019164159.html " + fileName);
                //Process p = Process.Start(pinfo);
                p.WaitForExit();
                if (p.ExitCode == 0)
                {
                    return true;
                }
                else
                {

                    return false;
                }
            }
            else
            {

                return true;
            }
        }


        //创建一个document
        Document document = null;

        #region 关于文档属性的基础设置
        /// <summary>
        /// 文档显示的方向
        /// 竖向或横向
        /// </summary>
        public enum direction { Virtical, Horizontal };
        #endregion

        #region 构造函数
        /// <summary>
        /// 创建一个无任何修饰的文档
        /// </summary>
        /// <param name="_filename">包含后缀的文件名</param>
        public PDFHelper(string _filename)
        {
            document = new Document();
            InitPathToOpen(_filename);
        }
        /// <summary>
        /// 实例化一个A4大小的文档
        /// </summary>
        /// <param name="_filename">包含后缀的文件名</param>
        /// <param name="_rectangle">内容区域</param>
        public PDFHelper(string _filename, Rectangle _rectangle)
        {
            //指定内容区域
            document = new Document(_rectangle);
            //写入权限控制
            InitPathToOpen(_filename);
            //写入数据
        }
        #endregion

        #region 公共方法

        #region document的基础参数配置方法
        /// <summary>
        /// 初始化路径并打开document的写入权限
        /// </summary>
        /// <param name="_filename">包含后缀的文件名</param>
        private void InitPathToOpen(string _filename)
        {
            //为该Document类创建一个Writer实例
            PdfWriter.GetInstance(document, new FileStream(HttpContext.Current.Server.MapPath("~/PDFS/") + _filename, FileMode.OpenOrCreate));
            //打开document
            document.Open();
        }
        /// <summary>
        /// 关闭document
        /// </summary>
        public void Close()
        {
            document.Close();
        }

        /// <summary>
        /// 得到一个设置好的字体颜色格式
        /// </summary>
        /// <param name="_size">字号大小</param>
        /// <param name="_alpha">透明度</param>
        /// <param name="_red">红色</param>
        /// <param name="_green">绿色</param>
        /// <param name="_blue">蓝色</param>
        /// <param name="_sytle">样式：下划线，加粗，标准？Font.UNDERLINE</param>
        /// <returns></returns>

        public static Font GetFont(float _size, int _alpha, int _red, int _green, int _blue, int _sytle)
        {
            //设置字体
            BaseFont _bfont = BaseFont.CreateFont(HttpContext.Current.Server.MapPath("~/Fonts") + "\\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            //设置字体颜色
            System.Drawing.Color _color = System.Drawing.Color.FromArgb(_alpha, _red, _green, _blue);
            BaseColor _bColor = new BaseColor(_color.ToArgb());
            Font _font = new Font(_bfont, _size, _sytle, _bColor);
            return _font;
        }

        /// <summary>
        /// 设置并获取一个标准的Rectangle类
        /// 若想设置背景色，边框等样式，请直接新建Rectangle实例详细设置
        /// </summary>
        /// <param name="_rectangle">区域，可以直接用 PageSize类选择A4等标准大小默认宽高度</param>
        /// <param name="_direction">文档方向，横向还是竖向</param>
        /// <returns></returns>
        public static Rectangle GetRectangle(Rectangle _rectangle, direction _direction)
        {
            return (_direction == PDFHelper.direction.Horizontal) ? _rectangle.Rotate() : _rectangle;
        }
        #endregion

        #region 添加内容的方法

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="_IElement"></param>
        public void Add(IElement _IElement)
        {
            try
            {
                document.Add(_IElement);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region 下载文件
        /// <summary>
        /// 下载PDF文件
        /// </summary>
        /// <param name="filename">含有后缀的文件名</param>
        public void DownloadPDF(string _filename)
        {
            //服务端存储路径
            string filepath = HttpContext.Current.Server.MapPath("~/PDFS/") + _filename;
            //以字符流的形式下载文件
            byte[] bytes;
            using (FileStream fs = new FileStream(filepath, FileMode.Open))
            {
                bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
            }

            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8;
            HttpContext.Current.Response.ContentType = "application/octet-stream";
            //通知浏览器下载|不打开
            HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + HttpUtility.UrlEncode(_filename, System.Text.Encoding.UTF8));
            HttpContext.Current.Response.BinaryWrite(bytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <returns></returns>
        private string CreateDirectory(string _dirName) //要查找的文件夹名
        {
            string path = "";
            string physicsPath = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath); //将当前虚拟根路径转为实际物理路径
            string toFindDirectoryName = _dirName; //要查找的文件夹名
            FindDirectory(physicsPath + "\\", toFindDirectoryName, out path);//用递归的方式去查找文件夹
            if (!string.IsNullOrEmpty(path)) //如果存在，返回该文件夹所在的物理路径
            {
                //将该物理路径转为虚拟路径
                GetVirtualPath(path, HttpContext.Current.Request.ApplicationPath);
            }
            else
            {
                //没有找到路径,创建新文件夹
                path = physicsPath + "\\" + toFindDirectoryName;
                Directory.CreateDirectory(path);
            }
            return path;
        }
        /// <summary>
        /// 在指定目录下递归查找子文件夹
        /// </summary>
        /// <param name="bootPath">根文件夹路径</param>
        /// <param name="directoryName">要查找的文件夹名</param>
        private void FindDirectory(string bootPath, string directoryName, out string filePath)
        {
            //在指定目录下递归查找子文件夹
            DirectoryInfo dir = new DirectoryInfo(bootPath);
            filePath = "";
            try
            {
                foreach (DirectoryInfo d in dir.GetDirectories()) //查找子文件夹
                {
                    if (d.Name == directoryName) //找到,返回文件夹路径
                    {
                        filePath = d.FullName;
                        break;
                    }
                    FindDirectory(bootPath + d.Name + "\\", directoryName, out filePath); //否则继续查找
                }
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write(e.Message);
            }
        }

        /// <summary>
        /// 将物理路径转为虚拟路径
        /// </summary>
        /// <param name="physicsPath">物理路径</param>
        /// <param name="virtualRootPath">虚拟根路径</param>
        /// <returns></returns>
        private string GetVirtualPath(string physicsPath, string virtualRootPath)
        {
            int index = physicsPath.IndexOf(virtualRootPath.Substring(1));
            return "/" + physicsPath.Substring(index).Replace("\\", "/");
        }
        #endregion
        #endregion

        #region 扩展方法
        /// <summary>
        /// 创建一个Document
        /// </summary>
        public static void CreateDocument(List<string> list, string pdfName)
        {
            try
            {
                //创建一个document操作实例
                Document document = new Document(PageSize.A4);
                //为该Document类创建一个Writer实例
                PdfWriter.GetInstance(document, new FileStream(pdfName, FileMode.OpenOrCreate));
                //打开document
                document.Open();

                //BaseFont.("iTextAsian.dll");
                //BaseFont.AddToResourceSearch("iTextAsianCmaps.dll");
                //载入字体
                //BaseFont baseFont = BaseFont.CreateFont(
                //    "C:\\WINDOWS\\FONTS\\STXINWEI.TTF", //宋体
                //    BaseFont.IDENTITY_H, //横向字体
                //    BaseFont.NOT_EMBEDDED);
                //iTextSharp.text.Font font = new iTextSharp.text.Font(baseFont, 9);
                BaseFont baseFont = BaseFont.CreateFont("C:/Windows/Fonts/SIMYOU.TTF", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

                document.Add(new Paragraph("你好，我的第一个pdf文档"));

                foreach (var item in list)
                {
                    //向document写入内容
                    document.Add(new Paragraph(item));
                }

                //关闭Document
                document.Close();
            }
            catch (DocumentException de)
            {
                //Console.WriteLine(de.Message);
                //Console.ReadKey();
            }
            catch (IOException io)
            {
                //Console.WriteLine(io.Message);
                //Console.ReadKey();
            }
            catch (System.Exception ex)
            {

                throw ex;
            }


        }


        #endregion
    }
}
