//using System;
//using System.Collections.Generic;
//using System.Web;
//using System.Data;

///// <summary>
/////Make_html 的摘要说明
///// </summary>
//public class Make_html
//{
//    public Make_html()
//    { }

//    /// <summary>
//    /// 自动生成静态页单个生成
//    /// </summary>
//    /// <param name="table">表名</param>
//    public static void MakeHtml(string table)
//    {
//        DataTable rs = guandongren.MakehtmlSQL.GetList("select * from g_makehtml where m_table='" + table + "'").Tables[0];
//        string template_url = "";
//        string html_url = "";
//        if (rs.Rows.Count > 0)
//        {
//            template_url = HttpContext.Current.Server.MapPath(@"~/" + rs.Rows[0]["m_template"]);//模板名称
//            html_url = HttpContext.Current.Server.MapPath(@"~/" + rs.Rows[0]["m_makeUrl"].ToString());//生成地址
//            Main fun = new Main();
//            fun.Make_page(template_url, html_url);
//            //连锁生成
//            Union_make(rs.Rows[0]["m_classid"].ToString());
//        }
//    }

//    /// <summary>
//    /// 自动生成静态页单个生成
//    /// 新闻列表之类
//    /// </summary>
//    /// <param name="table">表名</param>
//    /// <param name="un">是否需要联合</param>
//    /// <param name="where">条件</param>
//    public static void MakeHtml(string table, string where, string num)
//    {
//        string Sqlwhere = "";
//        //替换变量提取完整条件
//        DataTable rs = guandongren.MakehtmlSQL.GetList("select * from g_makehtml where m_table='" + table + "'").Tables[0];
//        if (rs.Rows.Count > 0)
//        {
//            if (rs.Rows[0]["m_where"].ToString().IndexOf("@id") >= 0)
//            {
//                Sqlwhere = rs.Rows[0]["m_where"].ToString().Replace("@id", where);
//            }
//        }
//        string template_url = "";
//        string html_url = "";
//        string urlparm = "";
//        if (rs.Rows.Count > 0)
//        {
//            template_url = HttpContext.Current.Server.MapPath(@"~/" + rs.Rows[0]["m_template"]);//模板名称
//            Main fun = new Main();
//            //命名原则：模板的文件名前缀自动接"_ID"
//            urlparm = rs.Rows[0]["m_makeUrl"].ToString().Replace("@", "_" + where);
//            html_url = HttpContext.Current.Server.MapPath(@"~/" + urlparm);//生成地址
//            DataTable rst = guandongren.MakehtmlSQL.GetList("select * from " + table + " " + Sqlwhere).Tables[0];
//            Union_make(rs.Rows[0]["m_classid"].ToString());
//            fun.Make_page(template_url, html_url, "[id]", rst.Rows[0][0].ToString());
//        }
//    }

//    /// <summary>
//    /// 自动生成静态页单个生成
//    /// 关于我们、联系我们等等
//    /// </summary>
//    /// <param name="table">表名</param>
//    /// <param name="un">是否需要联合</param>
//    /// <param name="where">条件</param>
//    public static void MakeHtml(string table, string where)
//    {
//        //替换变量提取完整条件
//        DataTable rs = guandongren.MakehtmlSQL.GetList("select * from g_makehtml where m_table='" + table + "' and m_where='" + where + "'").Tables[0];
//        if (rs.Rows.Count > 0)
//        {
//            string template_url = "";
//            string html_url = "";

//            if (rs.Rows.Count > 0)
//            {
//                Main fun = new Main();
//                template_url = HttpContext.Current.Server.MapPath(@"~/" + rs.Rows[0]["m_template"]);//模板名称
//                html_url = HttpContext.Current.Server.MapPath(@"~/" + rs.Rows[0]["m_makeUrl"]);//生成地址
//                Union_make(rs.Rows[0]["m_classid"].ToString());
//                fun.Make_page(template_url, html_url);
//            }
//        }
//    }

//    /// <summary>
//    /// 自动生成静态页批量生成,常用列表全部生成
//    /// </summary>
//    /// <param name="table">表名</param>
//    public static void MakeHtmlS(string table)
//    {
//        DataTable rs = guandongren.MakehtmlSQL.GetList("select * from g_makehtml where m_table='" + table + "'").Tables[0];
//        string template_url = "";
//        string html_url = "";
//        string urlparm = "";
//        string tableID = "";
//        if (rs.Rows.Count > 0)
//        {
//            template_url = HttpContext.Current.Server.MapPath(@"~/" + rs.Rows[0]["m_template"]);//模板名称
//            Main fun = new Main();
//            DataTable t = guandongren.MakehtmlSQL.GetList("selec * from " + table).Tables[0];
//            for (int i = 0; i < t.Rows.Count; i++)
//            {
//                tableID = t.Rows[i][0].ToString();//主键
//                //命名原则：模板的文件名前缀自动接"_ID"
//                urlparm = rs.Rows[0]["m_makeUrl"].ToString().Split('.')[0] + "_" + tableID + rs.Rows[0]["m_makeUrl"].ToString().Split('.')[1];
//                html_url = HttpContext.Current.Server.MapPath(@"~/" + urlparm);//生成地址
//                fun.Make_page(template_url, html_url);
//            }
//            Union_make(rs.Rows[0]["m_classid"].ToString());
//        }
//    }

//    /// <summary>
//    /// 自动生成静态页批量生成(主键替换)
//    /// </summary>
//    /// <param name="table">表名</param>
//    /// <param name="parm">替换的参数，替换生成主键</param>
//    public static void MakeHtmlS(string table, string parm)
//    {
//        DataTable rs = guandongren.MakehtmlSQL.GetList("select * from g_makehtml where m_table='" + table + "'").Tables[0];
//        string template_url = "";
//        string html_url = "";
//        string urlparm = "";
//        string tableID = "";
//        if (rs.Rows.Count > 0)
//        {
//            template_url = HttpContext.Current.Server.MapPath(@"~/" + rs.Rows[0]["m_template"]);//模板名称
//            Main fun = new Main();
//            DataTable t = guandongren.MakehtmlSQL.GetList("selec * from " + table).Tables[0];
//            for (int i = 0; i < t.Rows.Count; i++)
//            {
//                tableID = t.Rows[i][0].ToString();//主键
//                //命名原则：模板的文件名前缀自动接"_ID"
//                urlparm = rs.Rows[0]["m_makeUrl"].ToString().Split('.')[0] + "_" + tableID + rs.Rows[0]["m_makeUrl"].ToString().Split('.')[1];
//                html_url = HttpContext.Current.Server.MapPath(@"~/" + urlparm);//生成地址
//                fun.Make_page(template_url, html_url, parm, tableID);
//            }
//            Union_make(rs.Rows[0]["m_classid"].ToString());
//        }
//    }

//    /// <summary>
//    /// 自动生成静态页批量生成(主键替换)
//    /// </summary>
//    /// <param name="table">表名</param>
//    /// <param name="where">条件</param>
//    /// <param name="parm">替换的参数，替换生成主键</param>
//    public static void MakeHtmlS(string table, string where, string parm)
//    {
//        DataTable rs = guandongren.MakehtmlSQL.GetList("select * from g_makehtml where m_table='" + table + "'").Tables[0];
//        string template_url = "";
//        string html_url = "";
//        string urlparm = "";
//        string tableID = "";
//        if (rs.Rows.Count > 0)
//        {
//            template_url = HttpContext.Current.Server.MapPath(@"~/" + rs.Rows[0]["m_template"]);//模板名称
//            Main fun = new Main();
//            DataTable t = guandongren.MakehtmlSQL.GetList("select * from " + table + " " + where).Tables[0];
//            for (int i = 0; i < t.Rows.Count; i++)
//            {
//                tableID = t.Rows[i][0].ToString();//主键
//                //命名原则：模板的文件名前缀自动接"_ID"
//                urlparm = rs.Rows[0]["m_makeUrl"].ToString().Replace("@", "_" + tableID);
//                html_url = HttpContext.Current.Server.MapPath(@"~/" + urlparm);//生成地址
//                fun.Make_page(template_url, html_url, parm, tableID);
//            }
//            Union_make(rs.Rows[0]["m_classid"].ToString());
//        }
//    }

//    /// <summary>
//    /// 连锁生成
//    /// </summary>
//    /// <param name="m_id"></param>
//    public static void Union_make(string m_id)
//    {
//        string trTm = "";
//        string trUrl = "";
//        string[] pmid = m_id.Split(',');
//        Main fun = new Main();
//        for (int j = 0; j < pmid.Length; j++)
//        {
//            DataTable tr = guandongren.MakehtmlSQL.GetList("select * from g_makehtml where m_id =" + pmid[j]).Tables[0];
//            for (int i = 0; i < tr.Rows.Count; i++)
//            {
//                trTm = HttpContext.Current.Server.MapPath(@"~/" + tr.Rows[i]["m_template"]);//模板名称
//                trUrl = HttpContext.Current.Server.MapPath(@"~/" + tr.Rows[i]["m_makeUrl"]);//生成地址
//                fun.Make_page(trTm, trUrl);     //生成地址
//            }
//        }
//    }

//    /// <summary>
//    /// 删除数据对应的静态页面
//    /// </summary>
//    /// <param name="table">表名</param>
//    /// <param name="where">条件</param>
//    public static void MakeHtmlDelete(string table, string where)
//    {
//        DataTable rs = guandongren.MakehtmlSQL.GetList("select * from g_makehtml where m_table='" + table + "'").Tables[0];
//        string html_url = "";
//        string urlparm = "";
//        string tableID = "";
//        if (rs.Rows.Count > 0)
//        {

//            DataTable t = guandongren.MakehtmlSQL.GetList("select * from " + table + " " + where).Tables[0];
//            for (int i = 0; i < t.Rows.Count; i++)
//            {
//                tableID = t.Rows[i][0].ToString();//主键
//                //命名原则：模板的文件名前缀自动接"_ID"
//                urlparm = rs.Rows[0]["m_makeUrl"].ToString().Replace("@", "_" + tableID);
//                html_url = HttpContext.Current.Server.MapPath(@"~/" + urlparm);//生成地址
//                Main.DeleteFile(html_url);
//            }
//            Union_make(rs.Rows[0]["m_classid"].ToString());
//        }
//    }
//}