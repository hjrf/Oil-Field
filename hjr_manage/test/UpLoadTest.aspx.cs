using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test_UpLoadTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Request.Files["file"].ContentLength > 0)
        {
            HttpPostedFile file = Request.Files["file"];
            String path = hjr.Tools.File.UploadFiles ("/UploadFiles/", 300, file);

            Response.Write(hjr.Tools.Print.alert(path));
        }
        else
        {
            Response.Write(hjr.Tools.Print.alert("上传失败！"));
        }
    }
}