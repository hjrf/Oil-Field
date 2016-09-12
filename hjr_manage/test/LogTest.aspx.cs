using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test_LogTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DateTime time = DateTime.Now;
        hjr.Log.TextLog.WriteTextLog("测试","第一条日志",time);
    }
}