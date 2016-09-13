using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Web;

namespace hjr.Tools
{
    public class File
    {
        //读取配置文件中的上传文件允许类型与上传图片允许类型
        private static string allowFileTypes = ConfigurationSettings.AppSettings["fileType"];
        private static string allowImgTypes = ConfigurationSettings.AppSettings["imgType"];
        private static string[] allowFileType = allowFileTypes.Split('|');
        private static string[] allowImgType = allowImgTypes.Split('|');

        /// <summary>
        /// 上传文件，docType限制上传文件类型用|分割如"txt|xml"
        /// </summary>
        /// <param name="path">文件保存路径，如/UploadFiles/</param>
        /// <param name="Filesize">限制文件大小K</param>
        /// <param name="UpFile">上传主体</param>
        /// <param name="docType">限制上传类型</param>
        /// <returns>上传文件服务器路径+文件名</returns>
        public static string UploadFiles(string path, int Filesize, HttpPostedFile UpFile, string docType)
        {
            string modifyFileName = DateTime.Now.Year.ToString();//将当前时间年转化为字符串
            //根据当前时间对上传文件命名
            modifyFileName += DateTime.Now.Month.ToString();
            modifyFileName += DateTime.Now.Day.ToString();
            modifyFileName += DateTime.Now.Hour.ToString();
            modifyFileName += DateTime.Now.Minute.ToString();
            modifyFileName += DateTime.Now.Second.ToString();
            modifyFileName += DateTime.Now.Millisecond.ToString();
            string value = UpFile.FileName;//获取上传文件的名称

            string tFileType = value.Substring(value.LastIndexOf(".") + 1).ToLower();//提取出文件类型
            string[] allowedType = docType.Split('|');//分割并存储所有允许的文件类型数组
            bool TypeMatched = false;//上传的文件类型是否存在于允许类型数组中
            if (value == "" || value == null)//判断是否存在上传文件
            {
                return "2,上传失败：请选择文件"; //Error:Uploaded file is null!
            }
            else
            {
                int FileLength = UpFile.ContentLength;//获取上传文件大小
                if (FileLength > (Filesize * 1024))//与限制大小比较
                {
                    return "2,上传失败：超出文件大小" + (Filesize * 1024) + "K"; //Error:Uploaded file is too large!
                }
                foreach (string type in allowedType)//判断上传文件的类型是否存在与允许类型数组中
                {
                    if (type.ToLower().IndexOf(tFileType) != -1)
                    {
                        TypeMatched = true;
                    }
                }
                if (!TypeMatched)
                {
                    return "2,上传失败：文件类型错误"; //Error:Wrong type of Uploaded file!
                }
           
                string dirPath = HttpContext.Current.Server.MapPath(path);
                DirectoryInfo dir = new DirectoryInfo(dirPath);
                if (!dir.Exists)//如果文件不存在则创建
                {
                    dir.Create();
                }
                path = path + modifyFileName + "." + tFileType;

                UpFile.SaveAs(HttpContext.Current.Server.MapPath(path));//将接受的上传文件存储到服务器上

                return path;     //Upload succeed!
            }
        }
        /// <summary>
        /// 上传文件，使用配置文件中默认的文件类型限制
        /// 配置文件如下<add key="fileType" value="txt|xml" />
        /// </summary>
        /// <param name="path">文件保存路径，如/UploadFiles/ </param>
        /// <param name="Filesize">限制文件大小K</param>
        /// <param name="UpFile">上传主体</param>
        /// <returns>上传文件服务器路径+文件名</returns>
        public static string UploadFiles(string path, int Filesize, HttpPostedFile UpFile)
        {
            string modifyFileName = DateTime.Now.Year.ToString();//将当前时间年转化为字符串
            modifyFileName += DateTime.Now.Month.ToString();
            modifyFileName += DateTime.Now.Day.ToString();
            modifyFileName += DateTime.Now.Hour.ToString();
            modifyFileName += DateTime.Now.Minute.ToString();
            modifyFileName += DateTime.Now.Second.ToString();
            modifyFileName += DateTime.Now.Millisecond.ToString();
            string value = UpFile.FileName;//获取上传文件的名称

            string tFileType = value.Substring(value.LastIndexOf(".") + 1).ToLower();//提取出文件类型
            string[] allowedType = allowFileType;//分割并存储所有允许的文件类型数组
            bool TypeMatched = false;//上传的文件类型是否存在于允许类型数组中
            if (value == "" || value == null)//判断是否存在上传文件
            {
                return "2,上传失败：请选择文件"; //Error:Uploaded file is null!
            }
            else
            {
                int FileLength = UpFile.ContentLength;//获取上传文件大小
                if (FileLength > (Filesize * 1024))//与限制大小比较
                {
                    return "2,上传失败：超出文件大小" + (Filesize * 1024) + "K"; //Error:Uploaded file is too large!
                }
                foreach (string type in allowedType)//判断上传文件的类型是否存在与允许类型数组中
                {
                    if (type.ToLower().IndexOf(tFileType) != -1)
                    {
                        TypeMatched = true;
                    }
                }
                if (!TypeMatched)
                {
                    return "2,上传失败：文件类型错误"; //Error:Wrong type of Uploaded file!
                }

                string dirPath = HttpContext.Current.Server.MapPath(path);
                DirectoryInfo dir = new DirectoryInfo(dirPath);
                if (!dir.Exists)
                {
                    dir.Create();
                }
                path = path + modifyFileName + "." + tFileType;

                UpFile.SaveAs(HttpContext.Current.Server.MapPath(path));

                return path;     //Upload succeed!
            }
        }

        /// <summary>
        /// 上传图片带缩略图
        /// </summary>
        /// <param name="modifyFileName">文件名称</param>
        /// <param name="path">图片上传路径</param>
        /// <param name="smallwidth">缩略图宽度上限</param>
        /// <param name="smallheight">缩略图高度上限</param>
        /// <param name="Filesize">限制图片大小</param>
        /// <param name="UpFile">上传主体</param>
        /// <param name="docType">限制上传类型</param>
        /// <returns></returns>
        public static string UploadImg(string modifyFileName, string path, int smallwidth, int smallheight, int Filesize, HttpPostedFile UpFile, string docType)
        {
            string imageType = ",image/bmp,image/gif,image/pjpeg,image/jpeg,image/x-png,image/png,";

            string value = UpFile.FileName;
            string tFileType = value.Substring(value.LastIndexOf(".") + 1).ToLower();
            string[] allowedType = docType.Split(',');
            bool TypeMatched = false;
            string return_file = "";

            if (!imageType.Contains("," + UpFile.ContentType.ToLower() + ","))
            {
                return "2,上传失败:图片格式有误";//-1
            }
            else
            {
                int FileLength = UpFile.ContentLength;
                if (FileLength > (Filesize * 1024))
                {
                    return "2,上传失败:文件太大超过了" + (Filesize * 1024);   //Error:Uploaded file is too large!
                }
                foreach (string type in allowedType)
                {
                    if (type.ToLower().IndexOf(tFileType) != -1)
                    {
                        TypeMatched = true;
                    }
                }
                if (!TypeMatched)
                {
                    return "2,上传失败：文件类型错误"; //Error:Wrong type of Uploaded file!
                }

                string dirPath = System.Web.HttpContext.Current.Server.MapPath(path);
                System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(dirPath);

                if (!dir.Exists)
                {
                    dir.Create();
                }

                path = path + modifyFileName + "." + tFileType;

                return_file = modifyFileName + "." + tFileType;

                UpFile.SaveAs(System.Web.HttpContext.Current.Server.MapPath(path));

                try
                {
                    System.Drawing.Image u_img = System.Drawing.Image.FromFile(System.Web.HttpContext.Current.Server.MapPath(path));

                    if (u_img.Width > 0)
                    {
                        u_img.Dispose();
                        string file_path = System.Web.HttpContext.Current.Server.MapPath(path);
                        string small_path = dirPath + "pre_" + modifyFileName + "." + tFileType;
                        MakeThumbnail(file_path, small_path, smallwidth, smallheight);//生成缩略图
                    }
                    else
                    {
                        u_img.Dispose();
                        DeleteFile(path);
                        return "2,上传失败：图片非法";
                    }
                }
                catch
                {
                    DeleteFile(path);

                    return "2,上传失败：图片非法";
                }
                return return_file;     //Upload succeed!
            }
        }
        public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height)
        {
            try
            {
                Image originalImage = Image.FromFile(originalImagePath);

                int towidth = width;
                int toheight = height;

                int x = 0;
                int y = 0;
                int ow = originalImage.Width;
                int oh = originalImage.Height;

                if ((double)originalImage.Width > (double)originalImage.Height)//宽大于高
                {
                    //指定宽，高按比例             
                    toheight = originalImage.Height * width / originalImage.Width;
                }
                else
                {
                    //指定高，宽按比例
                    towidth = originalImage.Width * height / originalImage.Height;
                }


                //新建一个bmp图片
                Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

                //新建一个画板
                Graphics g = System.Drawing.Graphics.FromImage(bitmap);

                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                //设置高质量,低速度呈现平滑程度
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //清空画布并以透明背景色填充
                g.Clear(Color.Transparent);

                //在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
                    new Rectangle(x, y, ow, oh),
                    GraphicsUnit.Pixel);


                //以jpg格式保存缩略图
                bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);

                originalImage.Dispose();
                bitmap.Dispose();
                g.Dispose();
            }
            catch
            {
                // return "-1";
            }
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">预删除文件路径</param>
        /// <returns></returns>
        public static int DeleteFile(string path)
        {

            System.IO.FileInfo File = new System.IO.FileInfo(System.Web.HttpContext.Current.Server.MapPath(path));
            if (File.Exists)
            {
                if (File.IsReadOnly)
                {
                    return -1;//The file is readonly
                }
                else
                {
                    File.Delete();
                    return 1;//Delete succeed
                }
            }
            else
            {
                return 0;//The file is not exsits
            }
        }
        /// <summary>
        /// 根据输入路径读取文件内容转化为字符串返回
        /// </summary>
        /// <param name="path">以当前项目为根目录后的路径</param>
        /// <returns></returns>
        public static string GetFileStr(String path)
        {
            String rootPath = System.Web.HttpContext.Current.Server.MapPath("../");
            FileStream fs = new FileStream(rootPath + path, FileMode.Open);
            StreamReader m_streamReader = new StreamReader(fs);
            m_streamReader.BaseStream.Seek(0, SeekOrigin.Begin);
            string arry = "";
            string strLine = m_streamReader.ReadLine();
            do
            {
                arry += strLine;
                strLine = m_streamReader.ReadLine();

            } while (strLine != null && strLine != "");
            m_streamReader.Close();
            m_streamReader.Dispose();
            fs.Close();
            fs.Dispose();
            return arry;
        }
        private static string excelConfig = ConfigurationSettings.AppSettings["excelConfig"];
        public static DataTable ReadExcelToTable(string path)
        {   
            string connstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties='"+excelConfig+"';";
            using (OleDbConnection conn = new OleDbConnection(connstring))
            {
                conn.Open();
                System.Data.DataTable sheetsName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
                string firstSheetName = sheetsName.Rows[0][2].ToString();
                OleDbDataAdapter ada = new OleDbDataAdapter("select * from [" + firstSheetName + "]", connstring);
                DataSet set = new DataSet();
                ada.Fill(set);
                return set.Tables[0];
            }
        }

    }
}
