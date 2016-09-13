using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class web_Default : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        Server.Transfer("login_bg.html");
        //Response.Redirect("../templet/login_bg.html", false);
    }


}