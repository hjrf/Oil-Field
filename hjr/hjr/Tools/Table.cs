using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace hjr.Tools
{
    /// <summary>
    /// 表格工具
    /// </summary>
    public static class Table
    {
        public static String GetTableStr(DataTable dt)
        {
            String tableStr = null;
            int rowNum = dt.Rows.Count;
            int columnNum = dt.Columns.Count;
            List<String> columnName = new List<String>();
            for (int i = 0; i < columnNum; i++)
            {
                columnName.Add("d"+i.ToString());
            }
            //表头填充
            tableStr += "<thead><tr>";
            for (int i = 0; i < columnName.Count; i++)
            {
                tableStr += "<th>" + columnName[i] + "</th>";
            }
            tableStr += "</tr></thead>";
            //表格数据填充
            tableStr += " <tbody>";
            for (int i = 0; i < rowNum; i++)
            {
                tableStr += "<tr class=\"grade" + i + "\">";
                for (int j = 0; j < columnNum; j++)
                {
                    tableStr += "<td>" + dt.Rows[i][j] + "</td>";
                }
                tableStr += "</tr>";
            }
            tableStr += "</tbody>";
            return tableStr;
            
        }
    }
}
