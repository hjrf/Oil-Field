using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Bayes_Learn : System.Web.UI.Page
{
    static string strCon = "Server=(local);Uid=sa;Pwd=123456;Database=oil";
    SqlConnection conn = new SqlConnection(strCon);
    static Double correct = 0d;
    static int parNum = 30;
    static int parStandardNum = 6;
    static int parIntervalNum = 6;
    float entries;
    float entries_train;
    float entries_test;
    int num;
    List<int> index = new List<int>();

    double[][][] parameter = new double[parNum][][];
    double[] pStandard = new double[parStandardNum];
    double[] pStandard_prior = new double[parStandardNum];
    double[] pStandard_after = new double[parStandardNum];
    double[][] average = new double[parNum][];
    double[][] standardDeviation = new double[parNum][];

    List<int> result = new List<int>();
    protected void Page_Load(object sender, EventArgs e)
    {
    }
    int CountFiledTB(String TBName)
    {
        SqlCommand cmd = new SqlCommand("select count(name) from syscolumns where id=object_id('" + TBName + "')", conn);
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = cmd;
        DataSet ds = new DataSet();
        sda.Fill(ds, "" + TBName + "");
        DataTable dt = ds.Tables["" + TBName + ""];
        return (int)dt.Rows[0][0];
    }
    void upset(DataTable dt)
    {
        Random r = new Random();
        int MAX = dt.Rows.Count;
        MAX -= 1;
        while (MAX > 0)
        {
            int newPos = r.Next(0, MAX);
            DataRow dr = dt.NewRow();
            dr.ItemArray = dt.Rows[newPos].ItemArray;
            dt.Rows[newPos].ItemArray = dt.Rows[MAX].ItemArray;
            dt.Rows[MAX].ItemArray = dr.ItemArray;
            MAX--;
        }
    }
    void init(String TBName)
    {
        int filedcount = 0;
        filedcount = CountFiledTB(TBName);
        for (int i = 10; i < filedcount -1; i++)
        {
            index.Add(i);
        }
    }
    void ReadData(DataRow dr, int standard)
    {
        for (int i = 0; i < index.Count; i++)
        {
            parameter[i][standard - 1][Convert.ToInt32(pStandard[standard - 1]) - 1] = Convert.ToSingle(dr[index[i]]);
        }
    }
    int CheckTrainData(String TBName, String standard)
    {
        SqlCommand cmd = new SqlCommand("select count(*) from " + TBName + " where 级别 = '" + standard + "'", conn);
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = cmd;
        DataSet ds = new DataSet();
        sda.Fill(ds, "" + TBName + "");
        DataTable dt = ds.Tables["" + TBName + ""];
        return (int)dt.Rows[0][0];
    }
    void ConditionalPro(String dataAll, String dataTrain)
    {
        if (CheckTrainData(dataTrain, "1") == 0 || CheckTrainData(dataTrain, "2") == 0 || CheckTrainData(dataTrain, "3") == 0 || CheckTrainData(dataTrain, "4") == 0 || CheckTrainData(dataTrain, "5") == 0 || CheckTrainData(dataTrain, "6") == 0)
        {
            Response.Write("<script>alert('训练数据必须包含所有级别的记录！')</script>");
            //return;
        }
        SqlCommand cmd1 = new SqlCommand("select count(*) from " + dataAll + " ", conn);
        SqlDataAdapter sda1 = new SqlDataAdapter();
        sda1.SelectCommand = cmd1;
        DataSet ds1 = new DataSet();
        sda1.Fill(ds1, "" + dataAll + "");
        DataTable dt1 = ds1.Tables["" + dataAll + ""];
        entries = Convert.ToSingle(dt1.Rows[0][0]);
        entries_train = entries / 5 * 3;
        entries_test = entries / 5 * 2;
        TextBox14.Text = entries_train.ToString();
        TextBox1.Text = entries.ToString();
        TextBox6.Text = (100 * entries_train / entries).ToString() + "%";
        TextBox7.Text = (100 * entries_test / entries).ToString() + "%";

        SqlCommand cmd = new SqlCommand("select * from  " + dataTrain + " ", conn);
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = cmd;
        DataSet ds = new DataSet();
        sda.Fill(ds, "" + dataTrain + " ");
        DataTable dt2 = ds.Tables[0];
        //upset(dt2);
        for (int i = 0; i < parNum; i++)
        {
            parameter[i] = new double[parStandardNum][];
            for (int j = 0; j < parStandardNum; j++)
            {
                parameter[i][j] = new double[Convert.ToInt32(entries_test)];
            }
        }

        foreach (DataRow dr in dt2.Rows)
        {
            switch (dr[ds.Tables[0].Columns.Count].ToString().Trim())
            {
                case "1": pStandard[1 - 1]++; ReadData(dr, 1); break;
                case "2": pStandard[2 - 1]++; ReadData(dr, 2); break;
                case "3": pStandard[3 - 1]++; ReadData(dr, 3); break;
                case "4": pStandard[4 - 1]++; ReadData(dr, 4); break;
                default: Response.Write("<script>alert('未读取到级别数据！')</script>"); break;
            }
        }

        for (int i = 0; i < parNum; i++)
        {
            average[i] = new double[parStandardNum];
        }
        for (int i = 0; i < parNum; i++)
        {
            standardDeviation[i] = new double[parStandardNum];
        }
        for (int i = 0; i < parNum; i++)//均值
        {
            //Response.Write("第" + (i + 1) + "参数：");
            for (int j = 0; j < parStandardNum; j++)
            {
                //Response.Write("第" + (j + 1) + "级别：");
                for (int k = 0; k < pStandard[j]; k++)
                {
                    parameter[i][j][parameter[i][j].Length - 2] += parameter[i][j][k];
                }
                if (pStandard[j] != 0)
                {
                    average[i][j] = parameter[i][j][parameter[i][j].Length - 2] / pStandard[j];
                }

                //Response.Write("均值为");
                //Response.Write(average[i][j]);
                //Response.Write("/");
            }
        }
        //Response.Write("-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
        for (int i = 0; i < parNum; i++)//标准差
        {
            //Response.Write("第" + (i + 1) + "参数：");
            for (int j = 0; j < parStandardNum; j++)
            {
                for (int k = 0; k < pStandard[j]; k++)
                {
                    parameter[i][j][parameter[i][j].Length - 1] += Math.Pow((parameter[i][j][k] - average[i][j]), 2);
                }
                standardDeviation[i][j] = Math.Sqrt(parameter[i][j][parameter[i][j].Length - 1] / (pStandard[j] - 1));
                //Response.Write(parameter[i][j].Length);
                //Response.Write("标准差为");
                //Response.Write(standardDeviation[i][j]);
                //Response.Write("/");
            }
        }
        //Response.Write("------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
        for (int i = 0; i < parStandardNum; i++)
        {
            if (pStandard[i] == 0)
            {
                pStandard[i] = 1;
            }
            //Response.Write("第" + (i + 1) + "种级别数目：");
            //Response.Write(pStandard[i]);
            //Response.Write("/");
            pStandard[i] /= entries_train;

            //Response.Write("/");
            //Response.Write("级别" + (i + 1) + "先验概率：");
            //Response.Write(pStandard[i]);
        }//先验概率
        pStandard_prior = pStandard;
    }
    void Check(String dataTest)
    {
        SqlCommand cmd = new SqlCommand("select * from " + dataTest + "  ", conn);
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = cmd;
        DataSet ds = new DataSet();
        sda.Fill(ds, "" + dataTest + "");
        //Response.Write("----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Predict(dr);
            num++;
        }
    }
    double PconditionsCom(int parameter, int standard, double x)  //条件概率
    {
        double pConditions;
        pConditions = Math.Pow(x - average[parameter][standard], 2) / (2 * Math.Pow(standardDeviation[parameter][standard], 2));
        pConditions = Math.Exp(-pConditions);
        pConditions = pConditions / (Math.Sqrt(2 * 3.1415926) * standardDeviation[parameter][standard]);
        return pConditions;
    }
    void Predict(DataRow dr)
    {
        //Response.Write("新纪录：");
        for (int i = 0; i < parStandardNum; i++)
        {
            double p = 1;
            for (int j = 0; j < index.Count; j++)
            {
                p *= PconditionsCom(j, i, Convert.ToDouble(dr[index[j]]));
            }
            pStandard_after[i] = pStandard[i] * p;
            //Response.Write("第" + (i + 1) + "级别后验概率为");
            //Response.Write(pStandard_after[i]);
            //Response.Write("、");
        }

        double max = 0;
        //Response.Write("-------");
        int tem = 1;
        for (int j = 0; j < parStandardNum; j++)
        {
            if (pStandard_after[j] > max)
            {
                max = pStandard_after[j];
                tem = j + 1;
            }
        }
        result.Add(tem);
        //Response.Write("预测结果：");
        //Response.Write(result[num]);
        //Response.Write("/");
        //Response.Write("总数：");
        //Response.Write(result.Count);
    }

    void Correct(String dataTest)
    {
        SqlCommand cmd = new SqlCommand("select * from " + dataTest + " ", conn);
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = cmd;
        DataSet ds = new DataSet();
        sda.Fill(ds, "" + dataTest + "");
        //Response.Write("------------------------------------------------------------------------------------------------------------------------------------------------------");
        double numm = 0;
        double nummm = 0;
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (dr[ds.Tables[0].Columns.Count - 2].ToString().Trim() == result[Convert.ToInt32(numm)].ToString())
            {
                nummm++;
            }
            numm++;
        }

        correct = (nummm / numm) * 100;
        TextBox15.Text = numm.ToString();
        TextBox25.Text = nummm.ToString();
        TextBox26.Text = correct.ToString();
        insert();
        //Response.Write("正确率为");
        //Response.Write(nummm / numm);
    }
    DataTable ResultTB(String TBName)
    {
        SqlCommand cmd = new SqlCommand("select * from " + TBName + " ", conn);
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = cmd;
        DataSet ds = new DataSet();
        sda.Fill(ds, "" + TBName + "");
        DataTable dt = ds.Tables["" + TBName + ""];
        return dt;
    }
    void GridView4_DataBind()
    {
        DataTable dt3 = ResultTB("Bayes_Result");//asc升序，desc倒序
        GridView4.DataSource = dt3;
        GridView4.DataBind();
    }
    protected void GridView4_PageIndexChanged(object sender, EventArgs e)
    {
        GridView4_DataBind();
    }
    protected void GridView4_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView4.PageIndex = e.NewPageIndex;
    }

    void insert()
    {
        conn.Open();
        SqlCommand mon = conn.CreateCommand();
        mon.CommandText = "if exists (select 1 from  sysobjects where id = object_id('[Bayes_Result]') and type = 'U') drop table  Bayes_Result; create table Bayes_Result (SerialNumber int identity(1,1) primary key,correct varchar(30),parameter varchar(30),lv1 varchar(30),lv2 varchar(30),lv3 varchar(30),lv4 varchar(30))";
        mon.ExecuteNonQuery();
        for (int i = 0; i < index.Count; i++)
        {
            SqlCommand mon1 = conn.CreateCommand();
            mon1.CommandText = "insert into [oil].[dbo].[Bayes_Result] values ('" + Convert.ToInt32(correct) + "','参数" + (i + 1) + "均值','" + Math.Round(average[i][0], 3) + "','" + Math.Round(average[i][1], 3) + "','" + Math.Round(average[i][2], 3) + "','" + Math.Round(average[i][3], 3) + "','" + Math.Round(average[i][4], 3) + "','" + Math.Round(average[i][5], 3) + "')";
            mon1.ExecuteNonQuery();
            SqlCommand mon2 = conn.CreateCommand();
            mon2.CommandText = "insert into [oil].[dbo].[Bayes_Result] values ('" + Math.Round(correct, 3) + "','参数" + (i + 1) + "标准差','" + Math.Round(standardDeviation[i][0], 3) + "','" + Math.Round(standardDeviation[i][1], 3) + "','" + Math.Round(standardDeviation[i][2], 3) + "','" + Math.Round(standardDeviation[i][3], 3) + "','" + Math.Round(standardDeviation[i][4], 3) + "','" + Math.Round(standardDeviation[i][5], 3) + "')";
            mon2.ExecuteNonQuery();
        }
        SqlCommand mon3 = conn.CreateCommand();
        mon3.CommandText = "insert into [szpg_new].[dbo].[Bayes_Result] values ('" + Math.Round(correct, 3) + "','各级别先验概率','" + Math.Round(pStandard_prior[0], 3) + "','" + Math.Round(pStandard_prior[1], 3) + "','" + Math.Round(pStandard_prior[2], 3) + "','" + Math.Round(pStandard_prior[3], 3) + "','" + Math.Round(pStandard_prior[4], 3) + "','" + Math.Round(pStandard_prior[5], 3) + "')";
        mon3.ExecuteNonQuery();
        conn.Close();
    }
    protected void Button7_Click(object sender, EventArgs e)
    {
        if (DropDownList5.SelectedValue == "0")
        {
            Response.Write("<script>alert('请先选择数据集！')</script>");
            return;
        }
        else if (DropDownList5.SelectedValue == "1")
        {
            init("Test_Generate");
            ConditionalPro("Data_Generate", "Train_Generate");
            Check("Test_Generate");
            Correct("Test_Generate");
            TextBox8.Text = "Train_Generate";
            TextBox11.Text = "Test_Generate";
        }
        else if (DropDownList5.SelectedValue == "2")
        {
            init("Test_Practice");
            ConditionalPro("Data_Practice", "Train_Practice");
            Check("Test_Practice");
            Correct("Test_Practice");
            TextBox8.Text = "Train_Practice";
            TextBox11.Text = "Test_Practice";
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        GridView4_DataBind();
    }
}