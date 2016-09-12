using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace hjr.SQL
{
    /// <summary>
    /// Mysql帮助类
    /// 调用该方法，需要用web.config配置文件存储数据库连接字符串，
    /// 如：<add key="mysqlConnStr" value="server=服务器（如localhost或ip）;uid=用户名;pwd=密码;database=数据库名" />
    /// </summary>
    public class MysqlHelper
    {
        private static string connstr = ConfigurationSettings.AppSettings["mysqlConnStr"];
        private static MySqlConnection mysql = new MySqlConnection(connstr);
        public static void Exe(String sql)//执行SQL语句
        {
            mysql.Open();
            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            cmd.ExecuteNonQuery();
            mysql.Close();
        }
        public static DataTable Query(String sql)//查询返回datatable
        {
            mysql.Open();
            MySqlCommand cmd = new MySqlCommand(sql, mysql);
            MySqlDataAdapter mysqlData = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            mysqlData.Fill(dt);
            mysql.Close();
            return dt;
        }
    }
}
