<%@ WebHandler Language="C#" Class="Handler" %>

using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data.OleDb;

public class Handler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string action = context.Request["a"];
        switch (action)
        {
            case "register"://注册
                Register();
                break;
            case "login"://登录
                Login();
                break;
            case "upload"://上传
                Upload();
                break;
            case "tableUpload":
                UploadImport();
                TableUpload();
                break;
            case "tableAllData":
                TableAllData();
                break;
            default:
                break;
        }
    }

    public bool IsReusable {
        get {
            return false;
        }
    }

    private void Register()
    {
        try
        {
            string state = string.Empty;
            string realName = HttpContext.Current.Request["realName"];
            string userName = HttpContext.Current.Request["userName"];
            string email = HttpContext.Current.Request["email"];
            string password = HttpContext.Current.Request["password"];

            password = hjr.Tools.Str.MD5(password);

            SqlParameter[] param ={
                  new SqlParameter("@realName",realName),
                  new SqlParameter("@userName",userName),
                  new SqlParameter("@email",email),
                  new SqlParameter("@password",password)
             };


            String sql = "insert into o_users(u_real_name,u_user_name,u_email,u_password) values(@realName,@userName,@email,@password)";
            hjr.SQL.SqlserverHelper.ExecuteScalar(sql, CommandType.Text, param);
            state = "success";
            HttpContext.Current.Response.Write(state);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message);
        }

    }

    private void Login()
    {
        try
        {
            string state = string.Empty;
            state = "success";
            string userName = HttpContext.Current.Request["userName"];
            string password = HttpContext.Current.Request["password"];
            string isCookie = HttpContext.Current.Request["isCookie"];

            password = hjr.Tools.Str.MD5(password);

            SqlParameter[] param ={
                  new SqlParameter("@userName",userName),
                  new SqlParameter("@password",password),
             };

            DataTable dt = null;
            String sql = "select * from o_users where u_user_name=@userName and u_password=@password";
            dt = hjr.SQL.SqlserverHelper.GetDataTable(sql, CommandType.Text, param);

            if (dt == null)
            {
                state = "没有操作数据库进行查询，返回值未实例化，为null！";
            }
            else if (dt.Rows.Count == 0)
            {
                SqlParameter[] cmdParms2 ={
                                        new SqlParameter("@username",userName),
                                    };
                sql = "select * from o_users where u_user_name=@username";
                dt = hjr.SQL.SqlserverHelper.GetDataTable(sql, CommandType.Text, cmdParms2);
                if (dt.Rows.Count == 0)
                {
                    state = "该用户不存在！";
                }
                else
                {
                    state = "密码错误！";
                }
            }


            if (isCookie == "1")
            {
                //添加用户信息到cookie
            }

            HttpContext.Current.Response.Write(state);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message);
        }

    }
    private void Upload()
    {
        try
        {
            HttpPostedFile file = HttpContext.Current.Request.Files["file"];
            String fileName = file.FileName;
            String path = hjr.Tools.File.UploadFiles("/UploadFiles/", 300, file);
            if (!path.Contains("上传失败"))
            {
                string filetypeExpress = @"(?<=\.)[a-z|A-Z]+";
                String fileUrl = path;
                String fileType = hjr.Tools.Str.ZhengZeResult(filetypeExpress, path);
                SqlParameter[] cmdParms ={
                    new SqlParameter("@u_file_name",fileName),
                    new SqlParameter("@u_file_url",fileUrl),
                    new SqlParameter("@u_file_type",fileType),
                    new SqlParameter("@u_is_db","否")
                    };

                String sql = "insert into o_upload(u_file_name,u_file_url,u_file_type,u_is_db) values (@u_file_name,@u_file_url,@u_file_type,@u_is_db)";
                hjr.SQL.SqlserverHelper.ExecuteScalar(sql, CommandType.Text, cmdParms);
            }
            else
            {
                //上传失败
            }
            //HttpContext.Current.Response.Write(state);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message);
        }
    }
  
    private void UploadImport()
    {
        string uploadUrl = HttpContext.Current.Request["upload_url"];
        string uploadId = HttpContext.Current.Request["upload_id"];
        String sql = null;
        if (uploadId != null)
        {
            SqlParameter[] cmdParams1 ={
                                           new SqlParameter("@u_id",uploadId),
                                       };
            sql = "update o_upload set u_is_db='是' where u_id=@u_id";
            hjr.SQL.SqlserverHelper.ExecuteScalar(sql, CommandType.Text, cmdParams1);
            SqlParameter[] cmdParams2 ={
                                           new SqlParameter("@u_id",uploadId),
                                       };
            sql = "select u_file_url from o_upload where u_id=@u_id";
            DataTable dt = hjr.SQL.SqlserverHelper.GetDataTable(sql, CommandType.Text, cmdParams2);
            String path = dt.Rows[0][0].ToString().Trim();
            path = ".." + path;
            dt = hjr.Tools.File.ReadExcelToTable(HttpContext.Current.Server.MapPath(path));
            SqlParameter[] cmdParams3 ={
                                           new SqlParameter("@u_id",uploadId),
                                       };
            //根据excel列数目建表o_data，并导入excel数据到o_data
            int columnNum = dt.Columns.Count;
            int rowNum = dt.Rows.Count;
            String sqlStr = null;
            for (int i = 0; i < columnNum; i++)
            {
                sqlStr += ","+ dt.Rows[0][i] +" varchar(50)";
            }
            sql = "if exists (select 1 from  sysobjects where id = object_id('[o_data]') and type = 'U') drop table o_data; create table o_data (o_id int identity(1,1) primary key" + sqlStr + " )";
            hjr.SQL.SqlserverHelper.ExecuteScalar(sql);
            String sqlStr1 = null;
            String sqlStr2 = null;
            for (int i = 0; i < columnNum; i++)//拼接数据库列名字符串
            {
                sqlStr1 += ""+ dt.Rows[0][i] +",";
            }
            sqlStr1 = sqlStr1.Substring(0, sqlStr1.Length - 1);

            for (int i = 1; i < rowNum; i++)
            {
                sqlStr2 = null;
                for (int j = 0; j < columnNum; j++)
                {
                    sqlStr2 += "'"+ dt.Rows[i][j] +"',";//拼接数据库Value字符串
                }
                sqlStr2 = sqlStr2.Substring(0, sqlStr2.Length - 1);
                sql = "insert into o_data("+ sqlStr1 +") values("+ sqlStr2 +")";
                hjr.SQL.SqlserverHelper.ExecuteScalar(sql);
            }
            System.Diagnostics.Debug.WriteLine(sql);
        }
    }

    private void TableUpload()
    {
        try
        {
            String sql = "";
            List<String> columnName = new List<String>();
            columnName.Add("编号");
            columnName.Add("文件名称");
            columnName.Add("文件下载");
            columnName.Add("文件类型");
            columnName.Add("是否导入数据库");
            columnName.Add("查看或导入");
            columnName.Add("删除");

            string state = string.Empty;
            sql = "select * from o_upload";
            DataTable dt = hjr.SQL.SqlserverHelper.GetDataTable(sql);
            int columnNum = dt.Columns.Count;
            int RowNum = dt.Rows.Count;

            //表头填充
            state += "<thead><tr>";
            for (int i = 0; i < columnName.Count; i++)
            {
                state += "<th>" + columnName[i] + "</th>";
            }
            state += "</tr></thead>";
            //表格数据填充
            state += " <tbody>";
            for (int i = 0; i < RowNum; i++)
            {
                state += "<tr class=\"grade" + i + "\">";
                for (int j = 0; j < columnNum + 2; j++)
                {
                    if (j == 2)
                    {
                        state += "<td><a href=.." + dt.Rows[i][j] + ">下载文件</a></td>";
                        continue;
                    }
                    String express = "(?<=/)\\d+";
                    if (j == 5)
                    {
                        state += "<td><a href=?upload_show_url=.." + dt.Rows[i][2] + ">查看</a>|<a onclick=\"ChangeUrl('_table_show1.html?upload_id=" + dt.Rows[i][0] + "&upload_url=" + hjr.Tools.Str.ZhengZeResult(express, dt.Rows[i][2].ToString()) + "')\" href=_table_show1.html>导入</a></td>";
                        continue;
                    }
                    if (j == 6)
                    {
                        state += "<td><a onclick=\"ChangeUrl('_table_show1.html?upload_id=" + dt.Rows[i][0] + "&upload_url=" + hjr.Tools.Str.ZhengZeResult(express, dt.Rows[i][2].ToString()) + "')\" href=_table_show1.html>删除</a></td>";
                        continue;
                    }
                    state += "<td>" + dt.Rows[i][j] + "</td>";
                }
                state += "</tr>";
            }
            state += "</tbody>";
            HttpContext.Current.Response.Write(state);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message);
        }

    }

    private void TableAllData()
    {
        try
        {
            String sql = "";
            string state = string.Empty;
            sql = "select * from o_data";
            DataTable dt = hjr.SQL.SqlserverHelper.GetDataTable(sql);
            int columnNum = dt.Columns.Count;
            int RowNum = dt.Rows.Count;
                sql = "select name from oil.dbo.syscolumns where id=(select id from oil.dbo.sysobjects where name='o_data')";
                DataTable dt1 = hjr.SQL.SqlserverHelper.GetDataTable(sql);

            List<String> columnName = new List<String>();
            for (int i = 0; i < columnNum; i++)
            {
                columnName.Add(i.ToString());
            }
            //表头填充
            state += "<thead><tr>";
            for (int i = 0; i < columnName.Count; i++)
            {
                state += "<th>" + columnName[i] + "</th>";
            }
            state += "</tr></thead>";
            //表格数据填充
            state += " <tbody>";
            for (int i = 0; i < RowNum; i++)
            {
                state += "<tr class=\"grade" + i + "\">";
                for (int j = 0; j < columnNum; j++)
                {
                    state += "<td>" + dt.Rows[i][j] + "</td>";
                }
                state += "</tr>";
            }
            state += "</tbody>";
            HttpContext.Current.Response.Write(state);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message);
        }
    }





}