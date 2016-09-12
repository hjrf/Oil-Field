using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using hjr.SQL;
using System.Configuration;

public partial class test_SqlTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        String sql = "if exists (select 1 from  sysobjects where id = object_id('[testTable]') and type = 'U') drop table testTable; create table testTable (id int identity(1,1) primary key,filedName1 varchar(30))";
        SqlserverHelper.ExecuteScalar(sql);
    }
}