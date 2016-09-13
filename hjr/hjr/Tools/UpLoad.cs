using System;
using System.Configuration;
using System.IO;
using System.Web;

namespace hjr.Tools
{
    /// <summary>
    /// 上传工具
    /// </summary>
    public class Upload
    {
        ///// <summary>
        ///// 上传图片，自动尺寸
        ///// </summary>
        ///// <param name="path">保存路径</param>
        ///// <param name="smallwidth">预设宽度</param>
        ///// <param name="Filesize">预设文件大小</param>
        ///// <param name="UpFile"></param>
        ///// <param name="docType">文件类型</param>
        ///// <returns></returns>
        //public static string UploadImg(string path, int smallwidth, int Filesize, HttpPostedFile UpFile, string docType)
        //{
        //    string modifyFileName = DateTime.Now.Year.ToString();
        //    modifyFileName += DateTime.Now.Month.ToString();
        //    modifyFileName += DateTime.Now.Day.ToString();
        //    modifyFileName += DateTime.Now.Hour.ToString();
        //    modifyFileName += DateTime.Now.Minute.ToString();
        //    modifyFileName += DateTime.Now.Second.ToString();
        //    modifyFileName += DateTime.Now.Millisecond.ToString();

        //    ////////////
        //    string imageType = ",image/bmp,image/gif,image/pjpeg,image/jpeg,image/x-png,";
        //    if (!imageType.Contains("," + UpFile.ContentType + ","))
        //    {
        //        return "2,上传失败:请选择正规的图片";//-3
        //    }

        //    //-------------
        //    int FileLength;

        //    FileLength = UpFile.ContentLength; //记录文件长度 

        //    Image u_img = new Bitmap(UpFile.InputStream);

        //    string value = UpFile.FileName;
        //    string tFileType = value.Substring(value.LastIndexOf(".") + 1).ToLower();

        //    string return_file = "";

        //    if (value == "" || value == null)
        //    {
        //        return "2,上传失败:请选择图片";  //0
        //    }
        //    else
        //    {

        //        if (FileLength > (Filesize * 1024))
        //        {
        //            return "2,上传失败:文件太大超过了" + Filesize * 1024; //1
        //        }

        //        if (docType.ToLower().IndexOf(tFileType) < 0)
        //        {
        //            return "2,上传失败:文件类型不正确"; // 2
        //        }





        //        //不存在目录创建一个
        //        string path_url = System.Web.HttpContext.Current.Server.MapPath(path);


        //        System.IO.DirectoryInfo dirsmall = new System.IO.DirectoryInfo(path_url);

        //        if (!dirsmall.Exists)
        //        {
        //            dirsmall.Create();
        //        }
        //        //-----------end


        //        return_file = path + modifyFileName + "." + tFileType;



        //        if (u_img.Width > 0)
        //        {

        //            int swidth = 0;
        //            int weidth = 0;

        //            if (u_img.Width > smallwidth)
        //            {
        //                swidth = smallwidth;
        //                weidth = smallwidth * u_img.Height / u_img.Width;
        //            }
        //            else
        //            {
        //                swidth = u_img.Width;
        //                weidth = u_img.Height;
        //            }

        //            Bitmap b = new Bitmap(swidth, weidth);

        //            Graphics g = System.Drawing.Graphics.FromImage(b);

        //            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

        //            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        //            g.Clear(Color.Transparent);

        //            g.DrawImage(u_img, new Rectangle(0, 0, swidth, weidth), new Rectangle(0, 0, u_img.Width, u_img.Height), GraphicsUnit.Pixel);


        //            b.Save(HttpContext.Current.Server.MapPath(return_file), System.Drawing.Imaging.ImageFormat.Jpeg);
        //            g.Dispose();
        //            b.Dispose();
        //        }
        //        else
        //        {
        //            u_img.Dispose();
        //            DeleteFile(path);
        //            return "2,上传失败:文件非法格式";
        //        }
        //        // }
        //        //catch
        //        //{
        //        //    DeleteFile(path);

        //        //    return "4";
        //        //}
        //        return return_file;     //Upload succeed!
        //    }
        //}

        //public static bool createfile(string path)
        //{
        //    try
        //    {
        //        if (!File.Exists(path))
        //        {
        //            FileStream files = File.Create(path);
        //            files.Close();
        //            return true;


        //        }
        //        else
        //        {
        //            File.Delete(path);
        //            FileStream files = File.Create(path);
        //            files.Close();
        //            return true;
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public int SearchFile(string path, string fileName)
        //{
        //    System.IO.DirectoryInfo SearchDir = new System.IO.DirectoryInfo(System.Web.HttpContext.Current.Server.MapPath(path));
        //    foreach (System.IO.FileInfo File in SearchDir.GetFiles())
        //    {
        //        if (File.Name.IndexOf(fileName) != -1)
        //        {
        //            return 1;// Find
        //        }
        //    }
        //    return 0;//Not find
        //}

        //public static int DeleteFile(string path)
        //{

        //    System.IO.FileInfo File = new System.IO.FileInfo(System.Web.HttpContext.Current.Server.MapPath(path));
        //    if (File.Exists)
        //    {
        //        if (File.IsReadOnly)
        //        {
        //            return -1;//The file is readonly
        //        }
        //        else
        //        {
        //            File.Delete();
        //            return 1;//Delete succeed
        //        }
        //    }
        //    else
        //    {
        //        return 0;//The file is not exsits
        //    }
        //}
        ///**/
        ///// <summary>
        ///// 生成缩略图
        ///// </summary>
        ///// <param name="originalImagePath">源图路径（物理路径）</param>
        ///// <param name="thumbnailPath">缩略图路径（物理路径）</param>
        ///// <param name="width">缩略图宽度</param>
        ///// <param name="height">缩略图高度</param>
        ///// <param name="mode">生成缩略图的方式</param>    
        //public static void MakeThumbnail(string originalImagePath, string thumbnailPath, int width, int height)
        //{
        //    try
        //    {
        //        Image originalImage = Image.FromFile(originalImagePath);

        //        int towidth = width;
        //        int toheight = height;

        //        int x = 0;
        //        int y = 0;
        //        int ow = originalImage.Width;
        //        int oh = originalImage.Height;

        //        if ((double)originalImage.Width > (double)originalImage.Height)//宽大于高
        //        {
        //            //指定宽，高按比例             
        //            toheight = originalImage.Height * width / originalImage.Width;
        //        }
        //        else
        //        {
        //            //指定高，宽按比例
        //            towidth = originalImage.Width * height / originalImage.Height;
        //        }


        //        //新建一个bmp图片
        //        Image bitmap = new System.Drawing.Bitmap(towidth, toheight);

        //        //新建一个画板
        //        Graphics g = System.Drawing.Graphics.FromImage(bitmap);

        //        //设置高质量插值法
        //        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

        //        //设置高质量,低速度呈现平滑程度
        //        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

        //        //清空画布并以透明背景色填充
        //        g.Clear(Color.Transparent);

        //        //在指定位置并且按指定大小绘制原图片的指定部分
        //        g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight),
        //            new Rectangle(x, y, ow, oh),
        //            GraphicsUnit.Pixel);


        //        //以jpg格式保存缩略图
        //        bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);

        //        originalImage.Dispose();
        //        bitmap.Dispose();
        //        g.Dispose();
        //    }
        //    catch
        //    {
        //        // return "-1";
        //    }
        //}


    }
}

