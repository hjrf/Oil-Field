using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class test_hmm : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        float[,] a = new float[3, 3] { { 0.5f, 0.2f, 0.3f }, { 0.3f, 0.5f, 0.2f }, { 0.2f, 0.3f, 0.5f } };
        float[,] b = new float[3, 2] { { 0.5f, 0.5f }, { 0.4f, 0.6f }, { 0.7f, 0.3f } };
        float[,] result = new float[4, 3];
        int[] list = new int[4] { 0, 1, 0, 1 };
        int[,] max = new int[4, 3];
        float tmp;

        //step1:Initialization
        result[0, 0] = 0.2f * 0.5f;
        result[0, 1] = 0.4f * 0.4f;
        result[0, 2] = 0.4f * 0.7f;

        int i, j, k, count = 1, max_node = 0;
        float max_v = 0;
        
        //step2:归纳运算
        for (i = 1; i <= 3; i++)
        {
            for (j = 0; j <= 2; j++)
            {
                tmp = result[i - 1, 0] * a[0, j] * b[j, list[count]];
                max[i, j] = 0;
                for (k = 1; k <= 2; k++)
                {
                    if (result[i - 1, k] * a[k, j] * b[j, list[count]] > tmp)
                    {
                        tmp = result[i - 1, k] * a[k, j] * b[j, list[count]];
                        max[i, j] = k;
                    }
                    result[i, j] = tmp;
                }
                max_v = result[3, 0];
                max_node = 0;
                for (k = 1; k <= 2; k++)
                {
                    if (result[3, k] > max_v)
                    {
                        max_v = result[3, k];
                        max_node = k;
                    }
                }
            }
            count += 1;
        }
        //step3:终结
        for (i = 0; i <= 3; i++)
        {
            for (j = 0; j <= 2; j++)
            {
                Response.Write("result[" + (i + 1) + "][" + (j + 1) + "]：" + result[i, j]);
                Response.Write("</br>");
            }
        }
        Response.Write("Pmax=" + max_v);
        Response.Write("</br>");
        Response.Write("step4："+ (max_node + 1));
        Response.Write("</br>");
        //step4:回溯
        for (k = 3; k >= 1; k--)
        {
            Response.Write("step" + k +"："+ (max[k, max_node] + 1));
            Response.Write("</br>");
            max_node = max[k, max_node];
        }
    }
}