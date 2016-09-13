using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace hjr.SQL
{
    /// <summary>
    /// sqlserver帮助类
    /// 调用该方法，需要用web.config配置文件存储数据库连接字符串，
    /// 如： <add key="sqlserverConnStr" value="Data Source=服务器（如本机名或ip）;Initial Catalog=数据库名;User ID=用户名;Password=密码" />
    /// </summary>
    public class SqlserverHelper
    {
        private static string connstr = ConfigurationSettings.AppSettings["sqlserverConnStr"];
        private static SqlConnection conn = null;
        private static SqlCommand cmd = null;
        public SqlserverHelper()
        {
            //this.connstr = connstr;
        }

        #region 建立数据库连接对象
        /// <summary>
        /// 建立数据库连接
        /// </summary>
        /// <returns>返回一个数据库的连接SqlConnection对象</returns>
        public static SqlConnection init()
        {
            try
            {
                conn = new SqlConnection(connstr);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            return conn;
        }
        #endregion

        #region 设置SqlCommand对象
        /// <summary>
        /// 设置SqlCommand对象       
        /// </summary>
        /// <param name="cmd">SqlCommand对象 </param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdParms">参数集合</param>
        private static void SetCommand(SqlCommand cmd, string cmdText, CommandType cmdType, SqlParameter[] cmdParms)
        {
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;
            if (cmdParms != null)
            {
                cmd.Parameters.AddRange(cmdParms);
            }
        }
        #endregion

        #region 执行相应的sql语句，返回相应的DataSet对象
        /// <summary>
        /// 执行相应的sql语句，返回相应的DataSet对象
        /// </summary>
        /// <param name="sqlstr">sql语句</param>
        /// <returns>返回相应的DataSet对象</returns>
        public static DataSet GetDataSet(string sqlstr)
        {
            DataSet ds = new DataSet();
            try
            {
                init();
                SqlDataAdapter ada = new SqlDataAdapter(sqlstr, conn);
                ada.Fill(ds);
                conn.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            return ds;
        }
        #endregion

        #region 执行相应的sql语句，返回相应的DataSet对象
        /// <summary>
        /// 执行相应的sql语句，返回相应的DataSet对象
        /// </summary>
        /// <param name="sqlstr">sql语句</param>
        /// <param name="tableName">表名</param>
        /// <returns>返回相应的DataSet对象</returns>
        public static DataSet GetDataSet(string sqlstr, string tableName)
        {
            DataSet ds = new DataSet();
            try
            {
                init();
                SqlDataAdapter ada = new SqlDataAdapter(sqlstr, conn);
                ada.Fill(ds, tableName);
                conn.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            return ds;
        }
        #endregion

        #region 执行不带参数sql语句，返回一个DataTable对象
        /// <summary>
        /// 执行不带参数sql语句，返回一个DataTable对象
        /// </summary>
        /// <param name="cmdText">相应的sql语句</param>
        /// <returns>返回一个DataTable对象</returns>
        public static DataTable GetDataTable(string cmdText)
        {

            SqlDataReader reader;
            DataTable dt = new DataTable();
            try
            {
                init();
                cmd = new SqlCommand(cmdText, conn);
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dt;
        }
        #endregion

        #region 执行带参数的sql语句或存储过程，返回一个DataTable对象
        /// <summary>
        /// 执行带参数的sql语句或存储过程，返回一个DataTable对象
        /// </summary>
        /// <param name="cmdText">sql语句或存储过程名</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdParms">参数集合</param>
        /// <returns>返回一个DataTable对象</returns>
        public static DataTable GetDataTable(string cmdText, CommandType cmdType, SqlParameter[] cmdParms)
        {
            SqlDataReader reader;
            DataTable dt = new DataTable();
            try
            {
                init();
                cmd = new SqlCommand();
                SetCommand(cmd, cmdText, cmdType, cmdParms);
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return dt;
        }
        #endregion

        #region 执行不带参数sql语句，返回所影响的行数
        /// <summary>
        /// 执行不带参数sql语句，返回所影响的行数
        /// </summary>
        /// <param name="cmdText">增，删，改sql语句</param>
        /// <returns>返回所影响的行数</returns>
        public static int ExecuteNonQuery(string cmdText)
        {
            int count;
            try
            {
                init();
                cmd = new SqlCommand(cmdText, conn);
                count = cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return count;
        }
        #endregion

        #region 执行带参数sql语句或存储过程，返回所影响的行数
        /// <summary>
        /// 执行带参数sql语句或存储过程，返回所影响的行数
        /// </summary>
        /// <param name="cmdText">带参数的sql语句和存储过程名</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdParms">参数集合</param>
        /// <returns>返回所影响的行数</returns>
        public static int ExecuteNonQuery(string cmdText, CommandType cmdType, SqlParameter[] cmdParms)
        {
            int count;
            try
            {
                init();
                cmd = new SqlCommand();
                SetCommand(cmd, cmdText, cmdType, cmdParms);
                count = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return count;
        }
        #endregion

        #region 执行不带参数sql语句，返回一个从数据源读取数据的SqlDataReader对象
        /// <summary>
        /// 执行不带参数sql语句，返回一个从数据源读取数据的SqlDataReader对象
        /// </summary>
        /// <param name="cmdText">相应的sql语句</param>
        /// <returns>返回一个从数据源读取数据的SqlDataReader对象</returns>
        public static SqlDataReader ExecuteReader(string cmdText)
        {
            SqlDataReader reader;
            try
            {
                init();
                cmd = new SqlCommand(cmdText, conn);
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return reader;
        }
        #endregion

        #region 执行带参数的sql语句或存储过程，返回一个从数据源读取数据的SqlDataReader对象
        /// <summary>
        /// 执行带参数的sql语句或存储过程，返回一个从数据源读取数据的SqlDataReader对象
        /// </summary>
        /// <param name="cmdText">sql语句或存储过程名</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdParms">参数集合</param>
        /// <returns>返回一个从数据源读取数据的SqlDataReader对象</returns>
        public static SqlDataReader ExecuteReader(string cmdText, CommandType cmdType, SqlParameter[] cmdParms)
        {
            SqlDataReader reader;
            try
            {
                init();
                cmd = new SqlCommand();
                SetCommand(cmd, cmdText, cmdType, cmdParms);
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return reader;
        }
        #endregion

        #region 执行不带参数sql语句,返回结果集首行首列的值object
        /// <summary>
        /// 执行不带参数sql语句,返回结果集首行首列的值object
        /// </summary>
        /// <param name="cmdText">相应的sql语句</param>
        /// <returns>返回结果集首行首列的值object</returns>
        public static object ExecuteScalar(string cmdText)
        {
            object obj;
            try
            {
                init();
                cmd = new SqlCommand(cmdText, conn);
                obj = cmd.ExecuteScalar();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return obj;
        }
        #endregion

        #region 执行带参数sql语句或存储过程,返回结果集首行首列的值object
        /// <summary>
        /// 执行带参数sql语句或存储过程,返回结果集首行首列的值object
        /// </summary>
        /// <param name="cmdText">sql语句或存储过程名</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdParms">返回结果集首行首列的值object</param>
        /// <returns></returns>
        public static object ExecuteScalar(string cmdText, CommandType cmdType, SqlParameter[] cmdParms)
        {
            object obj;
            try
            {
                init();
                cmd = new SqlCommand();
                SetCommand(cmd, cmdText, cmdType, cmdParms);
                obj = cmd.ExecuteScalar();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
            return obj;
        }
        #endregion

        #region 将2维数组导入数据库
        /// <summary>
        /// 自动建一个表并将一个二维数组导入数据库
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="array2">二维数组</param>
        public static void Array2insertSql (string tableName,float [,] array2)
        {
            try
            {
                String sqlStr = null;
                String sqlStr1 = null;
                String sqlStr2 = null;

                for (int i = 0; i < array2.GetLongLength(1); i++)
                {
                    sqlStr += ",d" + i + " varchar(50)";
                    sqlStr1 += "d" + i + ",";
                }
                sqlStr1 = sqlStr1.Substring(0, sqlStr1.Length - 1);

                String sql = "if exists (select 1 from  sysobjects where id = object_id('["+ tableName+"]') and type = 'U') drop table "+ tableName +"; create table "+ tableName +" (id int identity(1,1) primary key " + sqlStr + ")";
                hjr.SQL.SqlserverHelper.ExecuteScalar(sql);      
                for (int i = 0; i < array2.GetLongLength(0); i++)
                {
                    sqlStr2 = null;
                    for (int j = 0; j < array2.GetLongLength(1); j++)
                    {
                        sqlStr2 += "'" + array2[i, j] + "',";//拼接数据库Value字符串
                    }
                    sqlStr2 = sqlStr2.Substring(0, sqlStr2.Length - 1);
                    sql = "insert into "+ tableName +"( " + sqlStr1 + " ) values(" + sqlStr2 + ")";
                    hjr.SQL.SqlserverHelper.ExecuteScalar(sql);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        #endregion

        #region 将3维数组导入数据库
        /// <summary>
        /// 自动建一个表并将一个二维数组导入数据库
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="array3">二维数组</param>
        public static void Array3insertSql(string tableName, float[,,] array3)
        {
            try
            {
                String sqlStr = null;
                String sqlStr1 = null;
                String sqlStr2 = null;

                for (int i = 0; i < array3.GetLongLength(2); i++)
                {
                    sqlStr += ",d" + i + " varchar(50)";
                    sqlStr1 += "d" + i + ",";
                }
                sqlStr1 = sqlStr1.Substring(0, sqlStr1.Length - 1);

                String sql = "if exists (select 1 from  sysobjects where id = object_id('[" + tableName + "]') and type = 'U') drop table " + tableName + "; create table " + tableName + " (id int identity(1,1) primary key " + sqlStr + ")";
                hjr.SQL.SqlserverHelper.ExecuteScalar(sql);
                for (int i = 0; i < array3.GetLongLength(0); i++)
                {
                    sql = "insert into " + tableName + "( d0 ) values ('第"+(i+1)+"维')";
                    hjr.SQL.SqlserverHelper.ExecuteScalar(sql);
                    for (int j = 0; j < array3.GetLongLength(1); j++)
                    {
                        sqlStr2 = null;
                        for (int k = 0; k < array3.GetLongLength(2); k++)
                        {
                            sqlStr2 += "'" + array3[i,j,k] + "',";//拼接数据库Value字符串
                        }
                        sqlStr2 = sqlStr2.Substring(0, sqlStr2.Length - 1);
                        sql = "insert into " + tableName + "( " + sqlStr1 + " ) values(" + sqlStr2 + ")";
                        hjr.SQL.SqlserverHelper.ExecuteScalar(sql);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        #endregion


}
}