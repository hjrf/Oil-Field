//using System;
//using System.Data;
//using System.Text.RegularExpressions;
//using System.Security.Cryptography;
//using System.Text;
//using System.Net.Mail;
//using System.Net;
//using System.Data.SqlClient;
//using System.IO;
///// <summary>
/////Main 的摘要说明
///// </summary>
//public class Main : guandongren.MakeHtml
//{
//    public Main()
//    {
//        //
//        //TODO: 在此处添加构造函数逻辑
//        //
//    }

//    /// <summary>
//    /// Excel文件导成Datatable
//    /// </summary>
//    /// <param name="strFilePath">Excel文件目录地址</param>
//    /// <param name="strTableName">Datatable表名</param>
//    /// <param name="iSheetIndex">Excel sheet index</param>
//    /// <returns></returns>
//    public static DataTable XlSToDataTable(string strFilePath, string strTableName, int iSheetIndex)
//    {

//        string strExtName = Path.GetExtension(strFilePath);

//        DataTable dt = new DataTable();
//        if (!string.IsNullOrEmpty(strTableName))
//        {
//            dt.TableName = strTableName;
//        }

//        if (strExtName.Equals(".xls") || strExtName.Equals(".xlsx"))
//        {
//            using (FileStream file = new FileStream(strFilePath, FileMode.Open, FileAccess.Read))
//            {
//                HSSFWorkbook workbook = new HSSFWorkbook(file);
//                ISheet sheet = workbook.GetSheetAt(iSheetIndex);

//                //列头
//                foreach (ICell item in sheet.GetRow(sheet.FirstRowNum).Cells)
//                {
//                    dt.Columns.Add(item.ToString(), typeof(string));
//                }

//                //写入内容
//                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
//                while (rows.MoveNext())
//                {
//                    IRow row = (HSSFRow)rows.Current;
//                    if (row.RowNum == sheet.FirstRowNum)
//                    {
//                        continue;
//                    }

//                    DataRow dr = dt.NewRow();
//                    foreach (ICell item in row.Cells)
//                    {
//                        switch (item.CellType)
//                        {
//                            case CellType.Boolean:
//                                dr[item.ColumnIndex] = item.BooleanCellValue;
//                                break;
//                            //case CellType.Error:
//                            //    dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
//                            //    break;
//                            case CellType.Formula:
//                                switch (item.CachedFormulaResultType)
//                                {
//                                    case CellType.Boolean:
//                                        dr[item.ColumnIndex] = item.BooleanCellValue;
//                                        break;
//                                    //case CellType.Error:
//                                    //    dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
//                                    //    break;
//                                    case CellType.Numeric:
//                                        if (DateUtil.IsCellDateFormatted(item))
//                                        {
//                                            dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
//                                        }
//                                        else
//                                        {
//                                            dr[item.ColumnIndex] = item.NumericCellValue;
//                                        }
//                                        break;
//                                    case CellType.String:
//                                        string str = item.StringCellValue;
//                                        if (!string.IsNullOrEmpty(str))
//                                        {
//                                            dr[item.ColumnIndex] = str.ToString();
//                                        }
//                                        else
//                                        {
//                                            dr[item.ColumnIndex] = null;
//                                        }
//                                        break;
//                                    case CellType.Unknown:
//                                    case CellType.Blank:
//                                    default:
//                                        dr[item.ColumnIndex] = string.Empty;
//                                        break;
//                                }
//                                break;
//                            case CellType.Numeric:
//                                if (DateUtil.IsCellDateFormatted(item))
//                                {
//                                    dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
//                                }
//                                else
//                                {
//                                    dr[item.ColumnIndex] = item.NumericCellValue;
//                                }
//                                break;
//                            case CellType.String:
//                                string strValue = item.StringCellValue;
//                                if (!string.IsNullOrEmpty(strValue))
//                                {
//                                    dr[item.ColumnIndex] = strValue.ToString();
//                                }
//                                else
//                                {
//                                    dr[item.ColumnIndex] = null;
//                                }
//                                break;
//                            case CellType.Unknown:
//                            case CellType.Blank:
//                            default:
//                                dr[item.ColumnIndex] = string.Empty;
//                                break;
//                        }
//                    }
//                    dt.Rows.Add(dr);
//                }
//            }
//        }

//        return dt;
//    }//2016.6.30

//    public override string functionhtml(string html, string str)
//    {
//        html = html.ToLower();
//        switch (html)
//        {
//            //截取字符长度,用法 {function:cut@字符,长度}
//            case "cut":
//                return CutString(str);
//            //时间格式,用法 {function:date@时间字符,格式}
//            case "date":
//                return DateFormat(str);
//            //替换字符串,用法 {function:replace@字符,oldValue|newValue}
//            case "replace":
//                return replace(str);
//            //运算,用法 {function:operate@字符,运算符|数字}
//            case "operate":
//                return operate(str);
//            case "get_htmlclass":
//                return Get_htmlclass(str);
//            case "limit_select":
//                return limit_select(str);
//            case "get_limit":
//                return get_limit(str);//显示权限信息
//            case "timespantonoral":
//                return GetNoralTime(str);
//            case "material_over":
//                return material_over(str);
//            case "groupname":
//                return groupname(str);
//            case "adddays":
//                return AddDays(str);
//            case "order_confirm":
//                return order_confirm(str);
//            case "add":
//                return Add(str);
//            case "operate1":
//                return operate1(str);
//            case "get_number":
//                return get_number(str);
//        }

//        return "";
//    }
//    private string get_number(string str)
//    {
//        string[] str_number = str.Split('-');
//        string str_num = str_number[0];

//        string sql = "select f_username from g_farm where f_id=(select f_name from g_file where f_number=" + str_num + ")";
//        string c = guandongren.MakehtmlSQL.Query_object(sql).ToString();
//        return c;
//    }




//    /// <summary>
//    /// 管理数值
//    /// </summary>
//    /// <param name="str"></param>
//    /// <returns></returns>
//    private string get_guanli(string str)
//    {
//        try
//        {
//            string[] list = str.Split(',');
//            string a1 = list[0];
//            string a2 = list[1];
//            string a3 = list[2];
//            string showtime = string.Empty;
//            if (list.Length > 3)
//            {
//                showtime = list[3];
//            }
//            else
//            {
//                showtime = "0";
//            }
//            //string p_sql4 = "select  * from dbo.g_zhibiao where z_owner = " + list[0] +
//            //    " and DATEDIFF(d,z_date, (select top 1 convert(char(10),z_date,120) as Date  from g_zhibiao where z_owner = " + list[0] + " group by convert(char(10),z_date,120) order by Date desc))=0  and z_subject=" + list[1] + " order by z_date desc";

//            string p_sql4 = "select  * from dbo.g_zhibiao where z_owner = " + list[0] +
//                "   and z_subject=" + list[1] + " and default_value=1 order by z_date desc";
//            string condition = string.Empty;
//            double count = 0;
//            if (a2 == "27" || a2 == "28" || a2 == "29")
//            {

//                string c_27 = guandongren.MakehtmlSQL.Query_object("select top 1 z_count from g_zhibiao where z_subject=27 and z_owner=" + a1 + " and default_value=1 order by z_id desc").ToString();
//                string c_28 = guandongren.MakehtmlSQL.Query_object("select top 1 z_count from g_zhibiao where z_subject=28 and z_owner=" + a1 + " and default_value=1 order by z_id desc").ToString();
//                string c_29 = guandongren.MakehtmlSQL.Query_object("select top 1 z_count from g_zhibiao where z_subject=29 and z_owner=" + a1 + " and default_value=1 order by z_id desc").ToString();
//                count = Convert.ToDouble(c_27) + Convert.ToDouble(c_28) + Convert.ToDouble(c_29);

//            }
//            else if (a2 == "24" || a2 == "25" || a2 == "26")
//            {
//                condition = "24,25,26";
//                string c_24 = guandongren.MakehtmlSQL.Query_object("select top 1 z_count from g_zhibiao where z_subject=24 and z_owner=" + a1 + " and default_value=1 order by z_id desc").ToString();
//                string c_25 = guandongren.MakehtmlSQL.Query_object("select top 1 z_count from g_zhibiao where z_subject=25 and z_owner=" + a1 + " and default_value=1 order by z_id desc").ToString();
//                string c_26 = guandongren.MakehtmlSQL.Query_object("select top 1 z_count from g_zhibiao where z_subject=26 and z_owner=" + a1 + " and default_value=1 order by z_id desc").ToString();
//                count = Convert.ToDouble(c_24) + Convert.ToDouble(c_25) + Convert.ToDouble(c_26);

//            }
//            else
//            {
//                return "输入有误" + a2;

//            }

//            string f1_sql = "select top 1 z_count from g_zhibiao where z_subject=" + a2 + " and z_owner=" + a1 + " order by z_id desc";
//            string f1 = guandongren.MakehtmlSQL.Query_object(f1_sql).ToString();
//            double f_1 = Convert.ToDouble(f1);
//            double fen = Convert.ToDouble(f_1 / count);
//            DataTable p4 = guandongren.MakehtmlSQL.Query(p_sql4).Tables[0];
//            string a_date = string.Empty;
//            if (showtime == "1")
//            {
//                a_date += "<br><span style='float:right;margin-right:10px;color:#3388FF';> " + Convert.ToDateTime(p4.Rows[0]["z_date"]).ToString("yyyyMMdd") + "</span>";
//            }
//            double s = Math.Round((fen) * 100, 2);
//            if (string.IsNullOrEmpty(s.ToString()))
//            {
//                return "无内容";
//            }
//            else
//            {
//                return s.ToString("#0.00") + "%" + a_date;
//            }
//        }
//        catch (Exception st)
//        {
//            return "无数据";
//        }
//    }

//    private string get_guanli_history(string str)
//    {
//        string farm_id = str;
//        string sql = "select top 39 * from g_zhibiao as a where not exists(select 1 from g_zhibiao as b where b.z_subject=a.z_subject and b.z_owner=a.z_owner and  DATEDIFF(m,a.z_date,b.z_date)=0 and b.z_date>a.z_date) and z_subject in(27,28,29) and z_owner=@owner order by z_id desc ";
//        System.Data.SqlClient.SqlParameter[] param ={
//                                   new System.Data.SqlClient.SqlParameter("@owner",farm_id)
//                                            };

//        SqlDataReader reader = guandongren.MakehtmlSQL.ExecuteReader(sql, param);
//        double z_27 = 0;
//        double z_28 = 0;
//        double z_29 = 0;
//        string html = string.Empty;
//        int itemid = 0;
//        while (reader.Read())
//        {

//            if (reader["z_subject"].ToString() == "29")
//            {
//                z_29 = Convert.ToDouble(reader["z_count"]);
//            }
//            if (reader["z_subject"].ToString() == "28")
//            {
//                z_28 = Convert.ToDouble(reader["z_count"]);
//            }
//            if (reader["z_subject"].ToString() == "27")
//            {

//                z_27 = Convert.ToDouble(reader["z_count"]);
//                string z_date = Convert.ToDateTime(reader["z_date"]).ToString("yyyyMMdd");
//                double z_count = z_27 + z_28 + z_29;

//                if (itemid > 0)
//                {
//                    if (z_count != 0)
//                    {
//                        html += " 上层&nbsp;" + Math.Round((z_27 / z_count) * 100, 2).ToString("#0.00") + "%&nbsp;中层&nbsp;" + Math.Round((z_28 / z_count) * 100, 2).ToString("#0.00") + "%&nbsp;下层&nbsp;" + Math.Round((z_29 / z_count) * 100, 2).ToString("#0.00") + "%" + "<br><span style='float:right;margin-right:10px;color:#ff0000';> " + z_date + "</span><br />";
//                    }
//                    else
//                    {
//                        html += " 上层&nbsp;无内容&nbsp;中层&nbsp;无内容&nbsp;下层&nbsp;无内容" + "<br><span style='float:right;margin-right:10px;color:#ff0000';> " + z_date + "</span><br />";
//                    }
//                }


//                itemid += 1;

//            }



//        }

//        return html;

//    }

//    private string get_guanli_history_b_1(string str)
//    {
//        string farm_id = str;
//        string sql = "select top 39 * from g_zhibiao as a where not exists(select 1 from g_zhibiao as b where b.z_subject=a.z_subject and b.z_owner=a.z_owner and  DATEDIFF(m,a.z_date,b.z_date)=0 and b.z_date>a.z_date)and z_subject in(24,25,26) and z_owner=@owner order by z_id desc ";
//        System.Data.SqlClient.SqlParameter[] param ={
//                                   new System.Data.SqlClient.SqlParameter("@owner",farm_id)
//                                            };

//        SqlDataReader reader = guandongren.MakehtmlSQL.ExecuteReader(sql, param);
//        double z_24 = 0;
//        double z_25 = 0;
//        double z_26 = 0;
//        string html = string.Empty;
//        int itemid = 0;
//        while (reader.Read())
//        {

//            if (reader["z_subject"].ToString() == "26")
//            {
//                z_26 = Convert.ToDouble(reader["z_count"]);
//            }
//            if (reader["z_subject"].ToString() == "25")
//            {
//                z_25 = Convert.ToDouble(reader["z_count"]);
//            }
//            if (reader["z_subject"].ToString() == "24")
//            {

//                z_24 = Convert.ToDouble(reader["z_count"]);
//                string z_date = Convert.ToDateTime(reader["z_date"]).ToString("yyyyMMdd");
//                double z_count = z_24 + z_25 + z_26;

//                if (itemid > 0)
//                {
//                    if (z_count != 0)
//                    {
//                        html += " 上层&nbsp;" + Math.Round((z_24 / z_count) * 100, 2).ToString("#0.00") + "%&nbsp;中层&nbsp;" + Math.Round((z_25 / z_count) * 100, 2).ToString("#0.00") + "%&nbsp;下层&nbsp;" + Math.Round((z_26 / z_count) * 100, 2).ToString("#0.00") + "%" + "<br><span style='float:right;margin-right:10px;color:#ff0000';>" + z_date + "</span><br />";
//                    }
//                    else
//                    {
//                        html += " 上层&nbsp;无内容&nbsp;中层&nbsp;无内容&nbsp;下层&nbsp;无内容" + "<br><span style='float:right;margin-right:10px;color:#ff0000';> " + z_date + "</span><br />";
//                    }
//                }
//                itemid += 1;
//            }
//        }
//        return html;
//    }
//    /// <summary>
//    /// 滨州趋势
//    /// </summary>
//    /// <param name="str"></param>
//    /// <returns></returns>
//    private string get_guanli_history_qushi(string str)
//    {
//        string tbPoint = string.Empty;//用于显示图表
//        string tbPoint1 = string.Empty;//用于显示图表
//        string tbPoint2 = string.Empty;//用于显示图表
//        string tbPoint3 = string.Empty;
//        string tbPoint4 = string.Empty;
//        string tbPoint5 = string.Empty;
//        string farm_id = str;
//        string sql = "select top 21 * from g_zhibiao where z_subject in( 24,25,26,27,28,29) and z_owner=@owner order by z_id desc ";
//        System.Data.SqlClient.SqlParameter[] param ={
//                                   new System.Data.SqlClient.SqlParameter("@owner",farm_id)
//                                            };

//        SqlDataReader reader = guandongren.MakehtmlSQL.ExecuteReader(sql, param);
//        double z_24 = 0;
//        double z_25 = 0;
//        double z_26 = 0;
//        double z_27 = 0;
//        double z_28 = 0;
//        double z_29 = 0;
//        string html = string.Empty;
//        while (reader.Read())
//        {
//            if (reader["z_subject"].ToString() == "26")
//            {
//                double z_count = z_24 + z_25 + z_26;
//                z_26 = Convert.ToDouble(reader["z_count"]);
//                tbPoint2 += Math.Round((z_25 / z_count)) + ",";
//                tbPoint2 = tbPoint.TrimEnd(',');
//                html += "<input type=\"hidden\" value=\"" + tbPoint2 + "\" id=\"tbPoint_qushi_map2\">";
//            }
//            if (reader["z_subject"].ToString() == "25")
//            {
//                double z_count = z_24 + z_25 + z_26;
//                z_25 = Convert.ToDouble(reader["z_count"]);
//                tbPoint1 += Math.Round((z_25 / z_count)) + ",";
//                tbPoint1 = tbPoint.TrimEnd(',');
//                html += "<input type=\"hidden\" value=\"" + tbPoint1 + "\" id=\"tbPoint_qushi_map1\">";
//            }
//            if (reader["z_subject"].ToString() == "24")
//            {
//                z_24 = Convert.ToDouble(reader["z_count"]);
//                string z_date = Convert.ToDateTime(reader["z_date"]).ToString("yyyyMMdd");
//                double z_count = z_24 + z_25 + z_26;

//                tbPoint += Math.Round((z_24 / z_count)) + ",";
//                tbPoint = tbPoint.TrimEnd(',');
//                html += "<input type=\"hidden\" value=\"" + tbPoint + "\" id=\"tbPoint_qushi_map\">";

//            }
//            if (reader["z_subject"].ToString() == "29")
//            {
//                double z_count = z_27 + z_28 + z_29;
//                z_29 = Convert.ToDouble(reader["z_count"]);
//                tbPoint5 += Math.Round((z_29 / z_count)) + ",";
//                tbPoint5 = tbPoint.TrimEnd(',');
//                html += "<input type=\"hidden\" value=\"" + tbPoint5 + "\" id=\"tbPoint_qushi_map5\">";
//            }
//            if (reader["z_subject"].ToString() == "28")
//            {
//                double z_count = z_27 + z_28 + z_29;
//                z_28 = Convert.ToDouble(reader["z_count"]);
//                tbPoint4 += Math.Round((z_28 / z_count)) + ",";
//                tbPoint4 = tbPoint.TrimEnd(',');
//                html += "<input type=\"hidden\" value=\"" + tbPoint4 + "\" id=\"tbPoint_qushi_map4\">";
//            }
//            if (reader["z_subject"].ToString() == "27")
//            {
//                double z_count = z_27 + z_28 + z_29;
//                z_27 = Convert.ToDouble(reader["z_count"]);
//                tbPoint3 += Math.Round((z_27 / z_count)) + ",";
//                tbPoint3 = tbPoint.TrimEnd(',');
//                html += "<input type=\"hidden\" value=\"" + tbPoint3 + "\" id=\"tbPoint_qushi_map3\">";
//            }




//        }

//        return html;

//    }

//    private string itemIdName(string id)
//    {
//        string itemIdName = string.Empty;
//        switch (id)
//        {
//            case "21":
//                itemIdName = "反刍";
//                break;
//            case "20":
//                itemIdName = "低产：趴卧率";
//                break;
//            case "19":
//                itemIdName = "体况评分";
//                break;
//            case "18":
//                itemIdName = "瘤胃评分";
//                break;
//            case "17":
//                itemIdName = "粪便评分";
//                break;
//            case "10":
//                itemIdName = "乳房炎率";
//                break;
//            case "9":
//                itemIdName = "CMT≥+++";
//                break;
//            case "8":
//                itemIdName = "乳头评分≥3分";
//                break;
//            case "7":
//                itemIdName = "乳房卫生评分≥3分";
//                break;
//            case "6":
//                itemIdName = "步态评分≥3分";
//                break;
//            case "5":
//                itemIdName = "反刍";
//                break;
//            case "4":
//                itemIdName = "反刍";
//                break;
//            case "3":
//                itemIdName = "中产：趴卧率";
//                break;
//            case "2":
//                itemIdName = "高产：趴卧率";
//                break;

//        }
//        return itemIdName;

//    }




//    /// <summary>
//    /// 获取公斤奶成本
//    /// </summary>
//    /// <param name="str"></param>
//    /// <returns></returns>
//    //private string get_mike(string str)
//    //{
//    //    try
//    //    {
//    //        string[] list = str.Split(',');
//    //        string sa1 = list[0];//farm_id
//    //        string sa2 = list[1];//1
//    //        //最新时间
//    //        string last_date = "select top 1 k_date from g_kilograms where k_farm = " + sa1 + " order by k_date desc";
//    //          object obj = guandongren.MakehtmlSQL.Query_object(last_date);
//    //          string last_date_n = System.DateTime.Now.ToString();
//    //        if(obj!=null)
//    //        {
//    //           last_date_n= obj.ToString();
//    //        }

//    //        //最新的配方组
//    //        string date_sql = "select top 1 p_date from g_peifang where p_owner = " + sa1 + " order by p_date desc";
//    //        string new_date = guandongren.MakehtmlSQL.Query_object(date_sql).ToString();
//    //        string sql1 = " select p_price,p_dichan,p_zhongchan,p_gaochan from dbo.g_peifang where p_owner = " + sa1 + " and p_date='" + new_date + "'";

//    //        DataTable t1 = guandongren.MakehtmlSQL.Query(sql1).Tables[0];
//    //        //最新的指标组
//    //        string sql_time = "select top 1 z_date from g_zhibiao where z_owner = " + sa1 + " order by z_date desc";
//    //        string date_new = guandongren.MakehtmlSQL.Query_object(sql_time).ToString();


//    //        string p_sql1 = "select z_count,z_date from dbo.g_zhibiao where z_owner = " + sa1 + " and z_date='" + date_new + "' and z_subject=18";
//    //        string p_sql2 = "select z_count,z_date from dbo.g_zhibiao where z_owner = " + sa1 + " and z_date='" + date_new + "' and z_subject=11";
//    //        string p_sql3 = "select z_count,z_date from dbo.g_zhibiao where z_owner = " + sa1 + " and z_date='" + date_new + "' and z_subject=10 ";
//    //        //总产奶量
//    //        string p_sql4 = "select z_count,z_date from dbo.g_zhibiao where z_owner = " + sa1 + " and z_date='" + date_new + "'  and z_subject=9";
//    //        //低产奶牛数
//    //        DataTable p1 = guandongren.MakehtmlSQL.Query(p_sql1).Tables[0];
//    //        DataTable p2 = guandongren.MakehtmlSQL.Query(p_sql2).Tables[0];
//    //        DataTable p3 = guandongren.MakehtmlSQL.Query(p_sql3).Tables[0];
//    //        DataTable p4 = guandongren.MakehtmlSQL.Query(p_sql4).Tables[0];
//    //        double a1 = 0;//低成本
//    //        double a2 = 0;//中成本
//    //        double a3 = 0;//高成本
//    //        double a4 = 0;//总产奶量

//    //        double pc1 = 0;//低奶牛数
//    //        if (p1.Rows.Count > 0)
//    //        {
//    //            pc1 = Convert.ToDouble(p1.Rows[0]["z_count"]);
//    //        }
//    //        double pc2 = 0;//中奶牛数
//    //        if (p2.Rows.Count > 0)
//    //        {
//    //            pc2 = Convert.ToDouble(p2.Rows[0]["z_count"]);
//    //        }
//    //        double pc3 = 0;//高奶牛数
//    //        if (p3.Rows.Count > 0)
//    //        {
//    //            pc3 = Convert.ToDouble(p3.Rows[0]["z_count"]);
//    //        }

//    //        if (p4.Rows.Count > 0)
//    //        {
//    //            a4 = Convert.ToDouble(p4.Rows[0]["z_count"]);
//    //        }

//    //        for (int i = 0; i < t1.Rows.Count; i++)
//    //        {
//    //            a1 += Convert.ToDouble(t1.Rows[i]["p_price"]) * Convert.ToDouble(t1.Rows[i]["p_dichan"]);
//    //            a2 += Convert.ToDouble(t1.Rows[i]["p_price"]) * Convert.ToDouble(t1.Rows[i]["p_zhongchan"]);
//    //            a3 += Convert.ToDouble(t1.Rows[i]["p_price"]) * Convert.ToDouble(t1.Rows[i]["p_gaochan"]);
//    //        }

//    //        double r_count = 0;

//    //        if (a4 > 0)
//    //        {
//    //            r_count = Math.Round((a1 * pc1 + a2 * pc2 + a3 * pc3) / a4, 2);
//    //            //r_count = (a1 * pc1 + a2 * pc2 + a3 * pc3) / a4;
//    //        }
//    //        //string z_date = System.DateTime.Now.ToString("yyyyMMdd");// "无数据";
//    //        //if (p2.Rows.Count > 0)
//    //        //{
//    //        //    z_date = Convert.ToDateTime(p2.Rows[0]["z_date"]).ToString("yyyyMMdd");
//    //        //}
//    //        string m_date = " <br><span style='float:right;margin-right:10px;color:#3388FF';>" + Convert.ToDateTime(last_date_n).ToString("yyyyMMdd") + "</span>";

//    //        if (sa2 == "0")
//    //        {
//    //            m_date = "";
//    //        }


//    //        return r_count.ToString("#0.00") + "元" + m_date;
//    //    }
//    //    catch (Exception st)
//    //    {
//    //        return "无数据" + st.Message;
//    //    }
//    //}
//    private string get_mike(string str)
//    {
//        string sql = "select top 1 k_date,gongjinnaichengben from v_t_gjncb where f_id=" + str.Split(',')[0] + " order by k_date desc";
//        System.Data.DataTable table = guandongren.MakehtmlSQL.GetList(sql).Tables[0];
//        if (table.Rows.Count > 0)
//        {
//            string gongjinnaichengben = table.Rows[0]["gongjinnaichengben"].ToString();
//            string k_date = Convert.ToDateTime(table.Rows[0]["k_date"]).ToString("yyyyMMdd");
//            string result = gongjinnaichengben + "元<br><span style='float:right;margin-right:10px;color:#3388FF';>" + k_date + "</span>";
//            return result;
//        }
//        else
//        {
//            return "无数据";
//        }
//    }

//    /// <summary>
//    /// 公斤奶成本趋势图Y轴
//    /// </summary>
//    /// <param name="str"></param>
//    /// <returns></returns>
//    private string get_mike_map_y(string str)
//    {
//        try
//        {
//            DataTable time_rs = guandongren.MakehtmlSQL.Query("select convert(char(10),p_date,120) as Date  from g_peifang where p_owner=" + str + " group by convert(char(10),p_date,120) order by Date asc").Tables[0];
//            string all_price = "";
//            for (int j = 0; j < time_rs.Rows.Count; j++)
//            {
//                //最新的配方组
//                string sql1 = " select top 10 * from dbo.g_peifang where p_owner = " + str + " and DATEDIFF(d,p_date, '" + time_rs.Rows[j][0] + "')=0  order by p_date asc";
//                DataTable t1 = guandongren.MakehtmlSQL.Query(sql1).Tables[0];

//                //最新的指标组

//                string p_sql1 = "select * from dbo.g_zhibiao where z_owner = " + str + " and DATEDIFF(d,z_date,'" + time_rs.Rows[j][0] + "')=0  and z_subject=18  order by z_date desc";
//                string p_sql2 = "select * from dbo.g_zhibiao where z_owner = " + str + " and DATEDIFF(d,z_date, '" + time_rs.Rows[j][0] + "')=0  and z_subject=11  order by z_date desc";
//                string p_sql3 = "select * from dbo.g_zhibiao where z_owner = " + str + " and DATEDIFF(d,z_date, '" + time_rs.Rows[j][0] + "')=0  and z_subject=10  order by z_date desc";
//                //总产奶量
//                string p_sql4 = "select * from dbo.g_zhibiao where z_owner = " + str + " and DATEDIFF(d,z_date, '" + time_rs.Rows[j][0] + "')=0  and z_subject=9  order by z_date desc";
//                //低产奶牛数
//                DataTable p1 = guandongren.MakehtmlSQL.Query(p_sql1).Tables[0];
//                DataTable p2 = guandongren.MakehtmlSQL.Query(p_sql2).Tables[0];
//                DataTable p3 = guandongren.MakehtmlSQL.Query(p_sql3).Tables[0];
//                DataTable p4 = guandongren.MakehtmlSQL.Query(p_sql4).Tables[0];
//                double a1 = 0;//低成本
//                double a2 = 0;//中成本
//                double a3 = 0;//高成本
//                double a4 = 0;//总产奶量

//                double pc1 = 0;//低奶牛数
//                if (p1.Rows.Count > 0)
//                {
//                    pc1 = Convert.ToDouble(p1.Rows[0]["z_count"]);
//                }
//                double pc2 = 0;//中奶牛数
//                if (p2.Rows.Count > 0)
//                {
//                    pc2 = Convert.ToDouble(p2.Rows[0]["z_count"]);
//                }
//                double pc3 = 0;//高奶牛数
//                if (p3.Rows.Count > 0)
//                {
//                    pc3 = Convert.ToDouble(p3.Rows[0]["z_count"]);
//                }

//                if (p4.Rows.Count > 0)
//                {
//                    a4 = Convert.ToDouble(p4.Rows[0]["z_count"]);
//                }

//                for (int i = 0; i < t1.Rows.Count; i++)
//                {
//                    a1 += Convert.ToDouble(t1.Rows[i]["p_price"]) * Convert.ToDouble(t1.Rows[i]["p_dichan"]);
//                    a2 += Convert.ToDouble(t1.Rows[i]["p_price"]) * Convert.ToDouble(t1.Rows[i]["p_zhongchan"]);
//                    a3 += Convert.ToDouble(t1.Rows[i]["p_price"]) * Convert.ToDouble(t1.Rows[i]["p_gaochan"]);
//                }

//                double r_count = 0;

//                if (a4 > 0)
//                {
//                    r_count = Math.Round((a1 * pc1 + a2 * pc2 + a3 * pc3) / a4, 2);
//                }

//                all_price += r_count + ",";
//            }

//            all_price = all_price.TrimEnd(',');

//            return all_price;
//        }
//        catch (Exception st)
//        {
//            return "无数据";
//        }
//    }

//    /// <summary>
//    /// 产奶量Y轴数据
//    /// </summary>
//    /// <returns></returns>
//    private string get_chanliang_map_y(string str)
//    {
//        try
//        {
//            DataTable time_rs = guandongren.MakehtmlSQL.Query("select convert(char(10),p_date,120) as Date  from g_peifang where p_owner=" + str + " group by convert(char(10),p_date,120) order by Date asc").Tables[0];
//            string all_price = "";
//            for (int j = 0; j < time_rs.Rows.Count; j++)
//            {

//                //总产奶量
//                string p_sql4 = "select z_count from dbo.g_zhibiao where z_owner = " + str + " and DATEDIFF(d,z_date, '" + time_rs.Rows[j][0] + "')=0  and z_subject=9  order by z_date desc";

//                string p_sql5 = "select z_count from dbo.g_zhibiao where z_owner = " + str + " and DATEDIFF(d,z_date, (select top 1 convert(char(10),z_date,120) as Date  from g_zhibiao group by convert(char(10),z_date,120) order by Date desc))=0  and z_subject=17  order by z_date desc";
//                string p_sql6 = "select z_count from dbo.g_zhibiao where z_owner = " + str + " and DATEDIFF(d,z_date, (select top 1 convert(char(10),z_date,120) as Date  from g_zhibiao group by convert(char(10),z_date,120) order by Date desc))=0  and z_subject=11  order by z_date desc";
//                string p_sql7 = "select z_count from dbo.g_zhibiao where z_owner = " + str + " and DATEDIFF(d,z_date, (select top 1 convert(char(10),z_date,120) as Date  from g_zhibiao group by convert(char(10),z_date,120) order by Date desc))=0  and z_subject=10  order by z_date desc";


//                DataTable p4 = guandongren.MakehtmlSQL.Query(p_sql4).Tables[0];
//                DataTable p5 = guandongren.MakehtmlSQL.Query(p_sql5).Tables[0];//低
//                DataTable p6 = guandongren.MakehtmlSQL.Query(p_sql6).Tables[0];//中
//                DataTable p7 = guandongren.MakehtmlSQL.Query(p_sql7).Tables[0];//高

//                double a4 = 0;//总产奶量
//                double b1 = 0;//低产奶牛
//                double b2 = 0;//中产奶牛
//                double b3 = 0;//高产奶牛

//                if (p4.Rows.Count > 0)
//                {
//                    a4 = Convert.ToDouble(p4.Rows[0]["z_count"]);
//                }

//                if (p5.Rows.Count > 0)
//                {
//                    b1 = Convert.ToDouble(p5.Rows[0]["z_count"]);
//                }

//                if (p6.Rows.Count > 0)
//                {
//                    b2 = Convert.ToDouble(p6.Rows[0]["z_count"]);
//                }

//                if (p7.Rows.Count > 0)
//                {
//                    b3 = Convert.ToDouble(p7.Rows[0]["z_count"]);
//                }

//                a4 = Math.Round(a4 / (b1 + b2 + b3), 2);


//                all_price += a4 + ",";
//            }

//            all_price = all_price.TrimEnd(',');

//            return all_price;
//        }
//        catch (Exception st)
//        {
//            return "无数据";
//        }
//    }

//    /// <summary>
//    /// 公斤奶成本趋势图X轴
//    /// </summary>
//    /// <param name="str"></param>
//    /// <returns></returns>
//    private string get_mike_map_x(string str)
//    {
//        try
//        {
//            DataTable time_rs = guandongren.MakehtmlSQL.Query("select convert(char(10),p_date,120) as Date  from g_peifang where p_owner=" + str + " group by convert(char(10),p_date,120) order by Date asc").Tables[0];
//            string all_price = "";
//            for (int j = 0; j < time_rs.Rows.Count; j++)
//            {
//                all_price += "'" + time_rs.Rows[j][0] + "',";
//            }
//            all_price = all_price.TrimEnd(',');

//            return all_price;
//        }
//        catch (Exception es)
//        {
//            return "";
//        }
//    }



//    /// <summary>
//    /// 总奶产量历史记录
//    /// </summary>
//    /// <param name="str"></param>
//    /// <returns></returns>
//    private string get_chanliang_map_history(string str)
//    {
//        try
//        {
//            string farm_id = str.Split(',')[0];
//            string day = str.Split(',')[1];

//            string all_price = "";
//            //  string tbPoint = string.Empty;
//            //  string tbPoint_date = string.Empty;//用于显示图表
//            //总产奶量
//            string p_sql4 = "select z_date,z_count from dbo.g_zhibiao a where NOT EXISTS (SELECT 1 FROM g_zhibiao b WHERE 1=1 AND CONVERT(CHAR(10), a.z_date, 120) = CONVERT(CHAR(10), b.z_date, 120)AND b.z_date > a.z_date and default_value=1 and z_owner = " + farm_id + "  and z_subject=9 and datediff(mm,z_date,getdate())<='" + day + "') and default_value=1 and z_owner = " + farm_id + "  and z_subject=9 and datediff(mm,z_date,getdate())<='" + day + "' order by z_date desc";
//            string p_sql8 = "select z_count from dbo.g_zhibiao a where NOT EXISTS (SELECT 1 FROM g_zhibiao b WHERE 1=1 AND CONVERT(CHAR(10), a.z_date, 120) = CONVERT(CHAR(10), b.z_date, 120)AND b.z_date > a.z_date and z_owner = " + farm_id + "  and z_subject=19 and datediff(mm,z_date,getdate())<='" + day + "') and z_owner = " + farm_id + "  and z_subject=19 and datediff(mm,z_date,getdate())<='" + day + "' order by z_date desc";


//            DataTable p4 = guandongren.MakehtmlSQL.Query(p_sql4).Tables[0];
//            DataTable p8 = guandongren.MakehtmlSQL.Query(p_sql8).Tables[0];//产奶牛合计
//            // double a4 = 0;//总产奶量
//            //double b8 = 0;//总牛
//            string d5 = string.Empty;
//            string a1 = string.Empty;
//            string a2 = string.Empty;
//            string a5 = string.Empty;
//            string a4 = string.Empty;

//            //System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();

//            string builder_value = string.Empty;
//            string builder_date = string.Empty;
//            for (int i = p4.Rows.Count - 1; i >= 0; i--)
//            {

//                double d1 = Convert.ToDouble(p4.Rows[i]["z_count"]);
//                double d2 = Convert.ToDouble(p8.Rows[i]["z_count"]);
//                string d_value = Math.Round(d1 / d2, 2).ToString("#0.00");
//                d5 = Convert.ToDateTime(p4.Rows[i]["z_date"]).ToString("yyyyMMdd");
//                builder_value += d_value + ",";
//                builder_date += d5 + ",";
//                // list.Add();
//                // 
//            }
//            builder_date = builder_date.TrimEnd(',');
//            builder_value = builder_value.TrimEnd(',');
//            string builder = builder_value + "|" + builder_date + ",";
//            // builder = builder.TrimEnd(',');
//            //  string[] builder_array = builder.Split('|');
//            //  return builder;





//            //for (int i = 0; i < list.Count; i++)
//            //{
//            //    string[] strs = list[i].ToString().Split(',');
//            //    string aa = strs[0];
//            //    string bb = strs[1];
//            //    d5 = strs[2];
//            //    a4 = Math.Round(Convert.ToDouble(aa) / Convert.ToDouble(bb), 2).ToString();
//            //}

//            //string  a4 = Math.Round(Convert.ToDouble(a1) /Convert.ToDouble(a2), 2).ToString("#0.00");



//            //all_price += "<li><span style='color:red;'>平均产奶量Kg&nbsp;" + a4 + "</span><span style='float:right;margin-right:10px;color:red;'>" + d5 + "</span></li>";

//            //string[] tbPoint = builder_array[0];
//            //string[] tbPoint_date = builder_array[1];

//            //tbPoint = Common.DisplayArray(tbPoint.Split(','));
//            //tbPoint_date = Common.DisplayArray(tbPoint_date.Split(','));
//            //all_price = all_price.TrimEnd(',');
//            //tbPoint = tbPoint.TrimEnd(',');
//            //all_price += "<input type=\"hidden\" value=\"" + tbPoint + "\" id=\"tbPoint_chanliang_map\">";
//            all_price += "<input type=\"hidden\" value=\"" + builder + "\" id=\"tbPoint_milk_chanliang_date\">";
//            return all_price;
//        }
//        catch (Exception st)
//        {
//            return st + "无数据";
//        }
//    }

//    private string Add(string s)
//    {
//        string s1 = s.Split(',')[0];
//        string s2 = s.Split(',')[1];
//        return decimal.Round(decimal.Parse(s1) + decimal.Parse(s2), 2).ToString();
//    }
//    #region 常用函数
//    /// <summary>
//    ///  将unix时间戳转换为一般时间格式
//    /// </summary>
//    /// <param name="now">unix时间戳</param>
//    /// <returns>一般时间格式</returns>
//    private string GetNoralTime(string now)
//    {
//        string timeStamp = now;
//        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
//        long lTime = long.Parse(timeStamp + "0000000");
//        TimeSpan toNow = new TimeSpan(lTime);
//        DateTime dtResult = dtStart.Add(toNow);
//        return dtResult.ToString();
//    }
//    /// <summary>
//    /// 字符串截取
//    /// </summary>
//    /// <param name="str"></param>
//    /// <returns></returns>
//    public static string CutString(string str)
//    {
//        str = NoHTML(str);
//        string title = str.Substring(0, str.LastIndexOf(","));
//        string leng = str.Substring(str.LastIndexOf(",") + 1, str.Length - (str.LastIndexOf(",") + 1));
//        if (title.Length > Convert.ToInt32(leng))
//        {
//            try
//            {
//                title = str.Substring(0, Convert.ToInt32(leng)) + "...";
//            }
//            catch
//            { }
//        }
//        return title;
//    }
//    /// <summary>
//    /// 替换字符串
//    /// </summary>
//    /// <param name="str"></param>
//    /// <returns></returns>
//    public static string replace(string str)
//    {
//        //str = NoHTML(str);
//        string title = str.Substring(0, str.LastIndexOf(","));
//        string content = str.Substring(str.LastIndexOf(",") + 1, str.Length - (str.LastIndexOf(",") + 1));
//        string[] ruleStr = content.Split('|');
//        if (ruleStr.Length < 2)
//        {
//            ruleStr[1] = string.Empty;
//        }
//        title = title.Replace(ruleStr[0], ruleStr[1]);
//        return title;
//    }
//    /// <summary>
//    /// 运算
//    /// </summary>
//    /// <param name="str"></param>
//    /// <returns></returns>
//    public static string operate(string str)
//    {
//        //str = NoHTML(str);
//        string title = str.Substring(0, str.LastIndexOf(","));
//        string content = str.Substring(str.LastIndexOf(",") + 1, str.Length - (str.LastIndexOf(",") + 1));
//        string[] ruleStr = content.Split('|');
//        if (ruleStr.Length > 1)
//        {

//            if (isNumber(title, ruleStr[1]))
//            {
//                int t1 = Convert.ToInt32(title);
//                int t2 = Convert.ToInt32(ruleStr[1]);
//                switch (ruleStr[0])
//                {
//                    case "+":
//                        title = (t1 + t2).ToString();
//                        break;
//                    case "-":
//                        title = (t1 - t2).ToString();
//                        break;
//                    case "*":
//                        title = (t1 * t2).ToString();
//                        break;
//                    case "/":
//                        if (t2 != 0)
//                        {
//                            title = (t1 / t2).ToString();
//                        }
//                        break;
//                    case "%":
//                        title = (t1 % t2).ToString();
//                        break;
//                    default:
//                        string r = ruleStr[0];
//                        int n = 0;
//                        if (r.Length > 1 && r.StartsWith("%"))
//                        {
//                            r = r.TrimStart('%');

//                            if (Main.isNumber(r) && r != "0")
//                            {
//                                n = Convert.ToInt32(r);
//                                title = (t1 % n).ToString();
//                                if (title == "0")
//                                {
//                                    title = (Convert.ToInt32(title) + t1).ToString();
//                                }
//                                title = getletter(title);
//                            }
//                        }
//                        break;
//                }
//            }
//        }
//        return title;

//    }
//    public static string operate1(string str)
//    {
//        //str = NoHTML(str);
//        string title = str.Substring(0, str.LastIndexOf(","));
//        string content = str.Substring(str.LastIndexOf(",") + 1, str.Length - (str.LastIndexOf(",") + 1));
//        string[] ruleStr = content.Split('|');
//        if (ruleStr.Length > 1)
//        {


//            decimal t1 = Convert.ToDecimal(title);
//            decimal t2 = Convert.ToDecimal(ruleStr[1]);
//            switch (ruleStr[0])
//            {
//                case "*":
//                    title = decimal.Round(t1 * t2 / 100, 2).ToString();
//                    break;
//            }
//        }
//        return title;

//    }
//    private static string getletter(string title)
//    {
//        string result = "";
//        switch (title)
//        {
//            case "1":
//                result = "a";
//                break;
//            case "2":
//                result = "b";
//                break;
//            case "3":
//                result = "c";
//                break;
//            case "4":
//                result = "d";
//                break;
//            case "5":
//                result = "e";
//                break;
//            case "6":
//                result = "f";
//                break;
//            case "7":
//                result = "g";
//                break;
//            default:
//                break;
//        }
//        return result;
//    }

//    /// <summary>
//    /// 格式化日期
//    /// </summary>
//    /// <param name="date"></param>
//    /// <param name="format"></param>
//    /// <returns></returns>
//    public static string DateFormat(string str)
//    {
//        try
//        {
//            string[] t = str.Split(',');
//            string date = t[0].ToString();
//            string format = t[1].ToString();
//            return Convert.ToDateTime(date).ToString("yyyyMMdd");
//        }
//        catch (Exception st)
//        {
//            return "";
//        }
//    }

//    //删除HTML脚本
//    public static string NoHTML(string Htmlstring)
//    {
//        //删除脚本
//        Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
//        //删除HTML
//        Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
//        Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
//        Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
//        Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
//        Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
//        Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
//        Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
//        Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
//        Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
//        Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
//        Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
//        Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
//        Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
//        Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);
//        Htmlstring.Replace("<", "");
//        Htmlstring.Replace(">", "");
//        Htmlstring.Replace("\r\n", "");
//        Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim().Replace("'", "`");
//        return Htmlstring;
//    }

//    /// <summary>
//    /// 检查是否为空，不为空返回true
//    /// </summary>
//    /// <param name="param"></param>
//    /// <returns></returns>
//    public static bool CheckNull(params string[] param)
//    {
//        foreach (object item in param)
//        {
//            if (object.Equals(item, null))
//            {
//                return false;
//            }
//            if (string.IsNullOrEmpty(item.ToString()))
//            {
//                return false;
//            }
//        }
//        return true;
//    }
//    /// <summary>
//    /// 为NULL返回String.Empty
//    /// </summary>
//    /// <param name="o"></param>
//    /// <returns></returns>
//    public static string CheckNullString(object o)
//    {
//        string result = string.Empty;
//        if (o != null)
//        {
//            result = string.IsNullOrEmpty(o.ToString()) ? "" : o.ToString();
//        }
//        return result;
//    }

//    /// <summary>
//    /// 判断是否数字，是数字返回true
//    /// </summary>
//    /// <param name="param"></param>
//    /// <returns></returns>
//    public static bool isNumber(params string[] param)
//    {
//        foreach (string item in param)
//        {
//            try
//            {
//                Convert.ToInt32(item);
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }
//        return true;
//    }
//    /// <summary>
//    /// 判断是否数字，是数字返回true
//    /// </summary>
//    /// <param name="param"></param>
//    /// <returns></returns>
//    public static bool isNumberNotNull(params string[] param)
//    {
//        foreach (string item in param)
//        {
//            try
//            {
//                if (item == null)
//                {
//                    return false;
//                }
//                Convert.ToInt32(item);
//            }
//            catch (Exception)
//            {
//                return false;
//            }
//        }
//        return true;
//    }
//    /// <summary>
//    /// 弹出提示信息
//    /// </summary>
//    /// <param name="num">1:没有返回值2:有返回地址3:直接关闭当期窗口4:返回上一级5:直接跳转</param>
//    /// <param name="str">弹出的内容</param>
//    /// <param name="url">跳转的路径</param>
//    /// <returns></returns>
//    public static string alert_text(int num, string str, string url)
//    {
//        switch (num)
//        {
//            case 1://普通的弹出，没有返回值
//                return "<script>alert('" + str + "');</script>";
//            case 2://普通的弹出窗口，有返回地址
//                return "<script>alert('" + str + "');location='" + url + "';</script>";
//            case 3://普通的弹出窗口，直接关闭当期窗口
//                return "<script>alert('" + str + "');location='javascript:window.close();';</script>";
//            case 4://普通的弹出窗口，返回上一级
//                return "<script>alert('" + str + "');location='javascript:window.history.go(-1);';</script>";
//            case 5:
//                return "<script>location='" + url + "';</script>";
//        }
//        return "";
//    }

//    /// <summary>
//    /// 设置上传文件的名称
//    /// </summary>
//    /// <param name="filename"></param>
//    /// <returns></returns>
//    public static string SetFileName(string filename)
//    {
//        string sxiff = filename.Substring(filename.LastIndexOf(".") + 1, filename.Length - (filename.LastIndexOf(".") + 1)).ToLower();
//        string newName = DateTime.Now.ToString("yyyyMMddhhmmssyyy") + "." + sxiff;
//        return newName;
//    }


//    #endregion


//    /// <summary>
//    /// 生成静态页连锁默认修改匹配
//    /// </summary>
//    /// <param name="str"></param>
//    public static string Get_htmlclass(string str)
//    {
//        string[] st = str.Split('|');
//        string[] dd = st[0].Split(',');
//        for (int i = 0; i < dd.Length; i++)
//        {
//            if (dd[i].ToString() == st[1].ToString())
//            {
//                return " checked='checked'";
//            }

//        }

//        return str;
//    }

//    /// <summary>
//    /// 权限选择
//    /// </summary>
//    /// <param name="str"></param>
//    public static string limit_select(string str)
//    {
//        string[] st = str.Split(',');
//        string[] dd = st[0].Split('|');
//        for (int i = 0; i < dd.Length; i++)
//        {
//            if (dd[i].ToString() == st[1].ToString())
//            {
//                return " checked='checked'";
//            }

//        }

//        return str;
//    }

//    /// <summary>
//    /// 权限显示
//    /// </summary>
//    /// <param name="str"></param>
//    public static string get_limit(string str)
//    {
//        string[] st = str.Split(',');//解析权限和用户

//        if (st.Length != 2)
//        {
//            return "设置错误";
//        }

//        if (st[1].ToString() == "0")
//        {
//            return "超级管理员权限";
//        }
//        string[] dd = st[0].Split('|');//单独权限
//        string limit = "";

//        for (int i = 0; i < dd.Length; i++)
//        {
//            if (dd[i].ToString() == "1")
//            {
//                limit += " 列表显示 ";
//            }

//            if (dd[i].ToString() == "2")
//            {
//                limit += " 添加权限 ";
//            }

//            if (dd[i].ToString() == "3")
//            {
//                limit += " 修改权限 ";
//            }

//            if (dd[i].ToString() == "4")
//            {
//                limit += " 删除权限 ";
//            }

//        }



//        return limit;
//    }

//    /// <summary>
//    /// 通用删除方法
//    /// checkboxName：页面复选框name
//    /// table：表名
//    /// primary：主键
//    /// url：跳转路径
//    /// </summary>
//    /// <param name="table">表名</param>
//    /// <param name="primary">主键</param>
//    /// <param name="url">跳转路径</param>
//    public static void Delete(string checkboxName, string table, string primary, string url)
//    {
//        try
//        {
//            string str = checkboxName;
//            if (string.IsNullOrEmpty(str))
//            {
//                HttpContext.Current.Response.Write(alert_text(4, "没有选择任何项！", ""));
//                return;
//            }
//            string[] id = str.Split(',');
//            for (int i = 0; i < id.Length; i++)
//            {
//                //guandongren.MakehtmlSQL.ExecuteSql("delete from " + table + " where " + primary + " in (" + id[i] + ")");
//                if (table == "p_address")
//                {
//                    string sql = "delete from g_product where p_id in (select p_id from dbo.view_ASPO where  a_topid='" + id[i] + "')";
//                    guandongren.MakehtmlSQL.ExecuteSql(sql);
//                    sql = "delete from dbo.g_shop where s_address in (select a_id  from p_address where a_topid='" + id[i] + "')";
//                    guandongren.MakehtmlSQL.ExecuteSql(sql);
//                    sql = "delete from p_address where a_id in (select a_id  from p_address where a_topid='" + id[i] + "')";
//                    guandongren.MakehtmlSQL.ExecuteSql(sql);
//                    sql = "delete from " + table + " where " + primary + "='" + id[i] + "'";
//                    guandongren.MakehtmlSQL.ExecuteSql(sql);
//                }
//                else
//                {
//                    guandongren.MakehtmlSQL.ExecuteSql("delete from " + table + " where " + primary + " in (" + id[i] + ")");
//                }

//            }
//            if (url != "")
//            {
//                HttpContext.Current.Response.Write(alert_text(2, "删除成功！", "" + url + ""));
//            }
//        }
//        catch (Exception ex)
//        {
//            HttpContext.Current.Response.Write(alert_text(2, "错误，" + ex + "", "" + url + ""));
//        }
//    }

//    /// <summary>
//    /// 删除带文件的信息
//    /// </summary>
//    /// <param name="checkboxName">ID列表</param>
//    /// <param name="table">表名</param>
//    /// <param name="primary">主键</param>
//    /// <param name="url">跳转路径</param>
//    /// <param name="fileColunm"></param>
//    public static void DataDel_file(string checkboxName, string table, string primary, string url, string fileColunm)
//    {
//        try
//        {
//            string str = checkboxName;
//            if (string.IsNullOrEmpty(str))
//            {
//                HttpContext.Current.Response.Write(alert_text(4, "没有选择任何项！", ""));
//                return;
//            }
//            string[] id = str.Split(',');
//            for (int i = 0; i < id.Length; i++)
//            {
//                DataTable dt = guandongren.MakehtmlSQL.GetList("select " + fileColunm + " from " + table + " where " + primary + "=" + id[i]).Tables[0];
//                if (dt.Rows.Count > 0)
//                {
//                    try
//                    {
//                        System.IO.File.Delete(HttpContext.Current.Server.MapPath(dt.Rows[0][0].ToString()));
//                        if ("p_img" == fileColunm)
//                        {
//                            System.IO.File.Delete(HttpContext.Current.Server.MapPath((dt.Rows[0][0].ToString()).Replace("UploadFiles/", "UploadFiles/small/")));
//                        }
//                    }
//                    catch
//                    { }
//                }
//                guandongren.MakehtmlSQL.ExecuteSql("delete from " + table + " where " + primary + "=" + id[i]);
//            }
//            HttpContext.Current.Response.Write(alert_text(2, "删除成功！", "" + url + ""));
//        }
//        catch (Exception ex)
//        {
//            HttpContext.Current.Response.Write(alert_text(2, "错误，" + ex + "", "" + url + ""));
//        }
//    }


//    /// <summary>
//    /// 发送邮件功能
//    /// </summary>
//    /// <param name="to">收件人邮箱</param>
//    /// <param name="from">发件人邮箱</param>
//    /// <param name="fromname">发件人名称</param>
//    /// <param name="subject">邮件标题</param>
//    /// <param name="body">邮件内容</param>
//    /// <param name="userName">服务器主机用户名</param>
//    /// <param name="password">服务器主机密码</param>
//    /// <param name="smtpHost">服务器地址,smtp</param>
//    /// <param name="port">服务器端口,一般默认25</param>
//    /// <returns></returns>

//    public static bool SendEmail(string to, string from, string fromname, string subject, string body, string userName, string password, string smtpHost, int port)
//    {
//        try
//        {
//            MailAddress fromq = new MailAddress(from, fromname);
//            MailAddress toq = new MailAddress(to);
//            MailMessage message = new MailMessage(fromq, toq);
//            message.Subject = subject;//设置邮件主题
//            message.IsBodyHtml = true;//设置邮件正文为html格式
//            message.Body = body;//设置邮件内容
//            SmtpClient client = new SmtpClient(smtpHost, port);
//            //设置发送邮件身份验证方式
//            //注意如果发件人地址是abc@def.com，则用户名是abc而不是abc@def.com
//            client.Credentials = new NetworkCredential(userName, password);
//            client.Send(message);
//            return true;
//        }
//        catch
//        {
//            return false;
//        }
//    }

//    /// <summary>
//    /// 权限管理设置
//    /// </summary>
//    /// <param name="t">权限类型(1，显示2、添加 3、修改 4、删除)</param>
//    /// <param name="username">用户类型</param>
//    /// <param name="url">页面参数</param>
//    public static void limit_manager(string t, string username, string url)
//    {
//        Object u = guandongren.MakehtmlSQL.Query_object("select a_type from g_admin where a_name ='" + username + "'");
//        bool islist = true;

//        if (u != null)
//        {
//            if (u.ToString().ToLower() != "administrator")//排除最高管理
//            {
//                object limit = guandongren.MakehtmlSQL.Query_object("select m_display from g_menu where m_url like '%" + url + "%'");

//                if (limit != null)
//                {
//                    if (limit.ToString().IndexOf(t) < 0)
//                    {
//                        islist = false;
//                    }
//                }
//                //1:列表权限
//                //2:添加权限
//                //3:修改权限
//                //4:删除权限
//            }
//        }

//        if (!islist)
//        {
//            HttpContext.Current.Response.Write(alert_text(4, "您没有权限操作！", ""));
//            HttpContext.Current.Response.End();
//            return;
//        }

//    }


//    public static string getMd5Hash(string input)
//    {
//        // Create a new instance of the MD5CryptoServiceProvider object.
//        MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
//        // Convert the input string to a byte array and compute the hash.        
//        byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
//        // Create a new Stringbuilder to collect the bytes        
//        // and create a string.        
//        StringBuilder sBuilder = new StringBuilder();
//        // Loop through each byte of the hashed data         
//        // and format each one as a hexadecimal string.        
//        for (int i = 0; i < data.Length; i++)
//        {
//            sBuilder.Append(data[i].ToString("x2"));
//        }
//        // Return the hexadecimal string.       
//        return sBuilder.ToString();
//    }
//    // Verify a hash against a string.    
//    public static bool verifyMd5Hash(string input, string hash)
//    {
//        // Hash the input.        
//        string hashOfInput = getMd5Hash(input);
//        // Create a StringComparer an compare the hashes.        
//        StringComparer comparer = StringComparer.OrdinalIgnoreCase;
//        if (0 == comparer.Compare(hashOfInput, hash))
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }


//    /// <summary>
//    /// 字符串MD5加密
//    /// </summary>
//    /// <param name="str">输入的字符串</param>
//    /// <returns>返回MD5</returns>
//    public static string Md5(string str)
//    {
//        return getMd5Hash(str);
//    }

//    /// <summary>
//    /// 验证MD5内容和输入的字符串是否相同
//    /// </summary>
//    /// <param name="str">要匹配的字符串</param>
//    /// <param name="md5string">匹配的MD5</param>
//    /// <returns>返回true,false</returns>

//    public static bool IsMd5(string str, string md5string)
//    {
//        return verifyMd5Hash(str, md5string);
//    }



//    #region 采用递归将字符串分割成数组
//    /// <summary>
//    /// 采用递归将字符串分割成数组
//    /// </summary>
//    /// <param name="strSource"></param>
//    /// <param name="strSplit"></param>
//    /// <param name="attachArray"></param>
//    /// <returns></returns>
//    private static string[] StringSplit(string strSource, string strSplit, string[] attachArray)
//    {
//        string[] strtmp = new string[attachArray.Length + 1];
//        attachArray.CopyTo(strtmp, 0);


//        int index = strSource.IndexOf(strSplit, 0);
//        if (index < 0)
//        {
//            strtmp[attachArray.Length] = strSource;
//            return strtmp;
//        }
//        else
//        {
//            strtmp[attachArray.Length] = strSource.Substring(0, index);
//            return StringSplit(strSource.Substring(index + strSplit.Length), strSplit, strtmp);
//        }
//    }
//    #endregion


//    #region 拆分字符串
//    /// <summary>
//    /// 根据字符串拆分字符串
//    /// </summary>
//    /// <param name="strSource">要拆分的字符串</param>
//    /// <param name="strSplit">拆分符</param>
//    /// <returns></returns>
//    public static string[] StringSplit(string strSource, string strSplit)
//    {
//        string[] strtmp = new string[1];
//        int index = strSource.IndexOf(strSplit, 0);
//        if (index < 0)
//        {
//            strtmp[0] = strSource;
//            return strtmp;
//        }
//        else
//        {
//            strtmp[0] = strSource.Substring(0, index);
//            return StringSplit(strSource.Substring(index + strSplit.Length), strSplit, strtmp);
//        }
//    }
//    #endregion

//    #region 随机数字
//    public static string getRandomNum(int num, int minValue, int maxValue)
//    {
//        Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
//        int[] arrNum = new int[num];
//        int tmp = 0;
//        for (int i = 0; i <= num - 1; i++)
//        {
//            //随机取数
//            tmp = ra.Next(minValue, maxValue);
//            //取出值赋到数组中
//            arrNum[i] = getNum(arrNum, tmp, minValue, maxValue, ra);
//        }
//        int t = 0;
//        string temp = "";
//        while (t <= arrNum.Length - 1)
//        {

//            temp += arrNum[t].ToString();

//            t++;

//        }
//        return temp;

//    }

//    public static int getNum(int[] arrNum, int tmp, int minValue, int maxValue, Random ra)
//    {
//        int n = 0;
//        while (n <= arrNum.Length - 1)
//        {
//            if (arrNum[n] == tmp) //利用循环判断是否有重复
//            {
//                //重新随机获取。
//                tmp = ra.Next(minValue, maxValue);
//                //递归:如果取出来的数字和已取得的数字有重复就重新随机获取。
//                getNum(arrNum, tmp, minValue, maxValue, ra);
//            }
//            n++;
//        }
//        return tmp;
//    }

//    #endregion


//    /// <summary>
//    /// 数据库链接设置
//    /// </summary>
//    /// <param name="conn"></param>
//    /// <param name="data_type"></param>
//    public static void Setting(string conn, string data_type, string template, string m_template)
//    {
//        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
//        AppSettingsSection app = config.AppSettings;

//        app.Settings["Conn"].Value = conn;
//        app.Settings["Datatype"].Value = data_type;
//        app.Settings["template"].Value = template;
//        app.Settings["manager_template"].Value = m_template;
//        config.Save(ConfigurationSaveMode.Modified);

//    }

//    public static void Update_weixin(string conn)
//    {
//        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
//        AppSettingsSection app = config.AppSettings;
//        app.Settings["weixin"].Value = conn;
//        config.Save(ConfigurationSaveMode.Modified);
//    }

//    /// <summary>
//    /// 获取全局变量值
//    /// </summary>
//    /// <param name="conn">主键</param>
//    /// <returns></returns>
//    public static string Get_webconfig(string conn)
//    {
//        Configuration config = WebConfigurationManager.OpenWebConfiguration("~");
//        AppSettingsSection app = config.AppSettings;
//        return app.Settings[conn].Value;
//    }




//    /// <summary>
//    /// 读取模板地址
//    /// </summary>
//    /// <param name="i"></param>
//    /// <returns></returns>
//    public static string Read_setting(int i)
//    {
//        string t = WebConfigurationManager.AppSettings["template"];
//        string m = WebConfigurationManager.AppSettings["manager_template"];
//        if (i == 0)
//        {
//            return t;
//        }
//        if (i == 1)
//        {
//            return m;
//        }
//        if (i == 2)
//        {
//            return "/cms_manager/template_role/";
//        }

//        return "";
//    }


//    public static bool createfile(string path)
//    {
//        try
//        {
//            if (!File.Exists(path))
//            {
//                FileStream files = File.Create(path);
//                files.Close();
//                return true;


//            }
//            else
//            {
//                File.Delete(path);
//                FileStream files = File.Create(path);
//                files.Close();
//                return true;
//            }
//        }
//        catch
//        {
//            return false;
//        }
//    }

//    public static bool DeleteFile(string path)
//    {
//        try
//        {
//            if (File.Exists(path))
//            {
//                File.Delete(path);

//                return true;
//            }
//        }
//        catch
//        {
//            return false;
//        }

//        return false;
//    }
//    /// <summary>
//    /// 根据文件名读取内容
//    /// </summary>
//    /// <param name="url">文件路径</param>
//    /// <returns>返回文件内容</returns>
//    public static string readfiles(string url)
//    {
//        //读取文件，采用数据库换成机制，利用模板代码写入数据库中，从数据库里面读取模板代码
//        string fiel_content = url;

//        string html;//模板全部内容
//        try
//        {
//            StreamReader reader = new StreamReader(fiel_content, System.Text.Encoding.UTF8);
//            html = reader.ReadToEnd();
//            reader.Close();
//        }
//        catch (Exception e)
//        {
//            throw new Exception(e.Message);

//        }
//        return html;
//    }

//    public static bool isfile(string files)
//    {
//        if (!File.Exists(files))
//        {
//            return false;
//        }
//        else
//        {
//            return true;
//        }
//    }

//    private string timespantonoral(string str)
//    {

//        return string.Empty;
//    }

//    private string material_over(string str)
//    {
//        string[] ts = str.Split(',');
//        if (ts.Length == 2)
//        {
//            if (ts[1].ToString() == "newslocal")
//            {
//                return "未过期";
//            }

//            if (Convert.ToDateTime(DateTime.Now.ToShortDateString()).AddDays(-3) <= Convert.ToDateTime(ts[0]))
//            {
//                return "未过期";
//            }
//        }


//        return "<span style='color:#ff0000;'>已过期</span>";
//    }

//    private string groupname(string str)
//    {
//        try
//        {
//            Convert.ToInt16(str);
//            object u = guandongren.MakehtmlSQL.Query_object("select u_groupname from g_weixin_usergroup where u_groupid ='" + str + "'");

//            if (u != null)
//            {
//                return u.ToString();
//            }
//        }
//        catch { }

//        return "所有人";
//    }


//    public static void AddCookie(string userid, string username)
//    {
//        string cookieName = "cls_user_cookie";
//        if (HttpContext.Current.Request.Cookies[cookieName] != null)
//        {
//            HttpContext.Current.Request.Cookies.Remove(cookieName);
//        }
//        HttpCookie cookie = new HttpCookie(cookieName);
//        cookie.Values.Add("userid", userid);
//        cookie.Values.Add("username", username);
//        HttpContext.Current.Response.AppendCookie(cookie);
//    }

//    public static void AddCookie(string phone, string userid, string username)
//    {
//        string cookieName = "cls_user_cookie";
//        if (HttpContext.Current.Request.Cookies[cookieName] != null)
//        {
//            HttpContext.Current.Request.Cookies.Remove(cookieName);
//        }

//        HttpCookie cookie = new HttpCookie(cookieName);
//        HttpContext.Current.Session["phone"] = phone;
//        HttpContext.Current.Session["userid"] = userid;
//        HttpContext.Current.Session["username"] = username;
//        cookie.Values.Add("phone", phone);
//        cookie.Values.Add("userid", userid);
//        cookie.Values.Add("username", username);
//        //cookie.Expires = System.DateTime.Now.AddMinutes(120);
//        HttpContext.Current.Response.AppendCookie(cookie);
//    }
//    public static void AddCookie(string phone, string userid, string username, string usrerole)
//    {
//        string cookieName = "cls_user_cookie";
//        if (HttpContext.Current.Request.Cookies[cookieName] != null)
//        {
//            HttpContext.Current.Request.Cookies.Remove(cookieName);
//        }

//        HttpCookie cookie = new HttpCookie(cookieName);
//        HttpContext.Current.Session["phone"] = phone;
//        HttpContext.Current.Session["userid"] = userid;
//        HttpContext.Current.Session["username"] = username;
//        HttpContext.Current.Session["usrerole"] = usrerole;
//        cookie.Values.Add("phone", phone);
//        cookie.Values.Add("userid", userid);
//        cookie.Values.Add("username", username);
//        cookie.Values.Add("usrerole", usrerole);
//        //cookie.Expires = System.DateTime.Now.AddMinutes(120);
//        HttpContext.Current.Response.AppendCookie(cookie);
//    }

//    /// <summary>
//    /// 返回USERID
//    /// </summary>
//    /// <param name="phone"></param>
//    /// <param name="userid"></param>
//    /// <param name="username"></param>
//    /// <returns></returns>
//    public static string GetCookie()
//    {
//        string cookieName = "cls_user_cookie";
//        string userId = "";
//        //获取用户名密码
//        HttpCookie cooks = HttpContext.Current.Request.Cookies[cookieName];
//        if (HttpContext.Current.Request.Cookies[cookieName] != null)
//        {
//            userId = HttpContext.Current.Request.Cookies[cookieName].Values["userid"];
//        }

//        return userId;
//    }


//    public static void GetCookie(ref string userId, ref string userName, ref string phone)
//    {
//        string cookieName = "cls_user_cookie";
//        //获取用户名密码
//        HttpCookie cooks = HttpContext.Current.Request.Cookies[cookieName];
//        if (HttpContext.Current.Request.Cookies[cookieName] != null)
//        {
//            userName = HttpContext.Current.Request.Cookies[cookieName].Values["username"];
//            userId = HttpContext.Current.Request.Cookies[cookieName].Values["userid"];
//            phone = HttpContext.Current.Request.Cookies[cookieName].Values["phone"];
//        }
//        else if (HttpContext.Current.Session["userid"] != null)
//        {
//            userName = HttpContext.Current.Session["username"].ToString();
//            userId = HttpContext.Current.Session["userid"].ToString();
//            phone = HttpContext.Current.Session["phone"].ToString();
//        }
//    }

//    public static void GetCookie(ref string userId, ref string userName)
//    {
//        string cookieName = "cls_user_cookie";
//        //获取用户名密码
//        HttpCookie cooks = HttpContext.Current.Request.Cookies[cookieName];
//        if (HttpContext.Current.Request.Cookies[cookieName] != null)
//        {
//            userName = HttpContext.Current.Request.Cookies[cookieName].Values["username"];
//            userId = HttpContext.Current.Request.Cookies[cookieName].Values["userid"];

//        }
//    }
//    public static void GetCookie(ref string userId, ref string userName, ref string phone, ref string role)
//    {
//        string cookieName = "agent_user_cookie";
//        //获取用户名密码
//        HttpCookie cooks = HttpContext.Current.Request.Cookies[cookieName];
//        if (HttpContext.Current.Request.Cookies[cookieName] != null)
//        {
//            userName = HttpContext.Current.Request.Cookies[cookieName].Values["username"];
//            userId = HttpContext.Current.Request.Cookies[cookieName].Values["userid"];
//            phone = HttpContext.Current.Request.Cookies[cookieName].Values["phone"];
//            role = HttpContext.Current.Request.Cookies[cookieName].Values["usrerole"];
//        }
//        else if (HttpContext.Current.Session["userid"] != null)
//        {
//            userName = HttpContext.Current.Session["username"].ToString();
//            userId = HttpContext.Current.Session["userid"].ToString();
//            phone = HttpContext.Current.Session["phone"].ToString();
//            role = HttpContext.Current.Session["usrerole"].ToString();//
//        }
//    }

//    //退出登录
//    public static void GetoverCookie()
//    {
//        string cookieName = "agent_user_cookie";
//        HttpContext.Current.Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(-30);
//        //HttpContext.Current.Response.Cookies[cookieName]["userid"] = null;
//        //HttpContext.Current.Response.Cookies[cookieName]["username"] = null;
//        //HttpContext.Current.Response.Cookies[cookieName]["phone"] = null;
//        HttpContext.Current.Session["username"] = null;
//        HttpContext.Current.Session["userid"] = null;
//        HttpContext.Current.Session["phone"] = null;
//    }
//    /// <summary>
//    /// 周排行或月排行
//    /// </summary>
//    /// <param name="date"></param>
//    /// <param name="format"></param>
//    /// <returns></returns>
//    public static string AddDays(string str)
//    {
//        int i = int.Parse(str);
//        DateTime time = DateTime.Now;
//        DateTime time1 = time.AddDays(i);
//        return time1.ToString();
//    }
//    public static string order_confirm(string str)
//    {
//        string returns = "<div class=\"ui-grid-b\">";
//        double money = 0.00;
//        int count = 0;
//        foreach (string s in str.Split('-'))
//        {
//            returns += "<div class=\"ui-block-a\">";
//            returns += " <span>" + s.Split(',')[1] + "</span></div>";
//            returns += " <div class=\"ui-block-b\">";
//            returns += " <span>" + s.Split(',')[2] + "份</span></div>";
//            returns += " <div class=\"ui-block-c\">";
//            returns += " <span>￥" + s.Split(',')[3] + "</span></div>";
//            count += int.Parse(s.Split(',')[2].ToString());
//            money += double.Parse(s.Split(',')[3].ToString());
//        }
//        returns += "</div>  <div class=\"content_list_price\">";
//        returns += "<span>数量：<font>" + count + "</font>&nbsp;份&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;合计：<font>" + money + "</font>&nbsp;元</span>";
//        returns += "</div>";
//        return returns;
//    }
//    /// <summary>
//    /// 判断来访设备终端
//    /// </summary>
//    /// <returns></returns>
//    public static bool IsMobileDevice()
//    {
//        string[] mobileAgents = { "iphone", "android", "phone", "mobile", "wap", "netfront", "java", "opera mobi", "opera mini", "ucweb", "windows ce", "symbian", "series", "webos", "sony", "blackberry", "dopod", "nokia", "samsung", "palmsource", "xda", "pieplus", "meizu", "midp", "cldc", "motorola", "foma", "docomo", "up.browser", "up.link", "blazer", "helio", "hosin", "huawei", "novarra", "coolpad", "webos", "techfaith", "palmsource", "alcatel", "amoi", "ktouch", "nexian", "ericsson", "philips", "sagem", "wellcom", "bunjalloo", "maui", "smartphone", "iemobile", "spice", "bird", "zte-", "longcos", "pantech", "gionee", "portalmmm", "jig browser", "hiptop", "benq", "haier", "^lct", "320x320", "240x320", "176x220", "w3c ", "acs-", "alav", "alca", "amoi", "audi", "avan", "benq", "bird", "blac", "blaz", "brew", "cell", "cldc", "cmd-", "dang", "doco", "eric", "hipt", "inno", "ipaq", "java", "jigs", "kddi", "keji", "leno", "lg-c", "lg-d", "lg-g", "lge-", "maui", "maxo", "midp", "mits", "mmef", "mobi", "mot-", "moto", "mwbp", "nec-", "newt", "noki", "oper", "palm", "pana", "pant", "phil", "play", "port", "prox", "qwap", "sage", "sams", "sany", "sch-", "sec-", "send", "seri", "sgh-", "shar", "sie-", "siem", "smal", "smar", "sony", "sph-", "symb", "t-mo", "teli", "tim-", "tosh", "tsm-", "upg1", "upsi", "vk-v", "voda", "wap-", "wapa", "wapi", "wapp", "wapr", "webc", "winw", "winw", "xda", "xda-", "Googlebot-Mobile" };
//        bool isMoblie = false;
//        if (HttpContext.Current.Request.UserAgent.ToString().ToLower() != null)
//        {
//            for (int i = 0; i < mobileAgents.Length; i++)
//            {
//                if (HttpContext.Current.Request.UserAgent.ToString().ToLower().IndexOf(mobileAgents[i]) >= 0)
//                {
//                    isMoblie = true;
//                    break;
//                }
//            }
//        }
//        if (isMoblie)
//        {
//            return true;
//        }
//        else
//        {
//            return false;
//        }
//    }

//    public static string GetURL()
//    {

//        try
//        {
//            //string sql = "select c_url from g_config";
//            //string url = guandongren.MakehtmlSQL.Query_object(sql).ToString();
//            //return url;

//            string t_url = HttpContext.Current.Request.Url.Host;

//            if (!t_url.Contains("http://"))
//            {
//                t_url = "http://" + t_url;
//            }
//            return t_url;

//        }
//        catch (Exception)
//        {

//            throw;
//        }
//    }


//}