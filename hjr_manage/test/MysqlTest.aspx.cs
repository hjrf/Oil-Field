using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using hjr.SQL;

public partial class test_MysqlTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String sql = "create table if not exists test(_id int primary key auto_increment,temperature varchar(20))";
        MysqlHelper.Exe(sql);
    }
}