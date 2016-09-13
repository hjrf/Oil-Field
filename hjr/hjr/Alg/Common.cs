using System;
using System.Configuration;
using System.Data;

namespace hjr.Alg
{
    public class Common
    {
        private static string fenleiParameterStr = ConfigurationSettings.AppSettings["fenlei_parameter"];
        private static string fenleiResult = ConfigurationSettings.AppSettings["fenlei_result"];
        private static string[] fenleiParameterArray = fenleiParameterStr.Split('|');
        private static int fenleiResultNum = Convert.ToInt32(ConfigurationSettings.AppSettings["fenlei_result_num"]);
        /// <summary>
        /// 根据配置文件，训练参数，查询任意随机记录数的训练数据集
        /// </summary>
        /// <param name="tableName">数据表名</param>
        /// <param name="recordNum">随机查询记录数目</param>
        /// <returns></returns>
        public static DataTable GetTrainsData(String tableName,String recordNum)//执行SQL语句
        {
            String sqlStr = null;
            for (int i = 0; i < fenleiParameterArray.Length; i++)
            {
                sqlStr += ""+ fenleiParameterArray[i]+",";
            }
            sqlStr += fenleiResult;
            String sql = null;
            if (recordNum == "")
            {
                sql = "select " + sqlStr + " from " + tableName + "";
            }
            else
            {
                sql = "select top " + recordNum + " " + sqlStr + " from " + tableName + " order by NewID()";
            }
            return hjr.SQL.SqlserverHelper.GetDataTable(sql);
        }
        /// <summary>
        /// 根据配置文件，训练参数，获取全部训练数据集
        /// </summary>
        /// <param name="tableName">数据表名</param>
        /// <returns></returns>
        public static DataTable GetTrainsData(String tableName)
        {
            String sqlStr = null;
            for (int i = 0; i < fenleiParameterArray.Length; i++)
            {
                sqlStr += "" + fenleiParameterArray[i] + ",";
            }
            sqlStr += fenleiResult;
            String sql = "select  " + sqlStr + " from " + tableName + "";
            System.Diagnostics.Debug.WriteLine(sql);
            return hjr.SQL.SqlserverHelper.GetDataTable(sql);
        }
        /// <summary>
        /// 获取验证数据
        /// </summary>
        /// <param name="tableName">数据表名</param>
        /// <returns></returns>
        public static DataTable GetTestData(String tableName)
        {
            String sqlStr = null;
            for (int i = 0; i < fenleiParameterArray.Length; i++)
            {
                sqlStr += "" + fenleiParameterArray[i] + ",";
            }
            sqlStr = sqlStr.Substring(0, sqlStr.Length - 1);
            String sql = "select  " + sqlStr + " from " + tableName + "";
            System.Diagnostics.Debug.WriteLine(sql);
            return hjr.SQL.SqlserverHelper.GetDataTable(sql);
        }
        /// <summary>
        /// 获取每种分类结果数数目
        /// </summary>
        /// <param name="trainData"></param>
        /// <returns></returns>
        public static float[] GetEveResultNum(float[,] trainData)
        {
            float[] eveResultNum = new float[fenleiResultNum];
           
            for (int i = 0; i < trainData.GetLongLength(0) ; i++)
            {
                for (int j = 0; j < fenleiResultNum; j++)
                {
                    if (trainData[i, trainData.GetLongLength(1)-1] == j+1)
                    {
                        eveResultNum[j]++;
                    }
                }
            }
            return eveResultNum;
        }
        /// <summary>
        /// 把一个数组中为0的值变成1
        /// </summary>
        /// <param name="array1"></param>
        /// <returns></returns>
        public static float[] FillArray1(float[] array1)
        {
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] == 0)
                {
                    array1[i] = 1;
                }
            }
            return array1;
        }

        /// <summary>
        /// 把一个2维数组中为0的值变成1
        /// </summary>
        /// <param name="arra2"></param>
        /// <returns></returns>
        public static float[,] FillArray2(float[,] array2)
        {
            for (int i = 0; i < array2.GetLongLength(0); i++)
            {
                for (int j = 0; j < array2.GetLongLength(1); j++)
                {
                    if (array2[i,j] == 0)
                    {
                        array2[i,j] = 1;
                    }
                }
            }
            return array2;
        }
        /// <summary>
        /// 获取前验概率
        /// </summary>
        /// <param name="eveResultNum"></param>
        /// <param name="trainsNum"></param>
        /// <returns></returns>
        public static float [] GetXianYan(float [] eveResultNum, float trainNum)
        {
            float[] qianYan = new float[eveResultNum.Length];
            for (int i = 0; i < eveResultNum.Length; i++)
            {
                qianYan[i] =Convert.ToSingle(eveResultNum[i]) / Convert.ToSingle(trainNum);
            }
            return qianYan;
        }
        /// <summary>
        /// 获取均值
        /// </summary>
        /// <param name="trainData"></param>
        /// <param name="eveParNum"></param>
        /// <param name="print"></param>
        /// <returns></returns>
        public static float[,] GetAverage(float[,] trainData, float[] eveResultNum)
        {
            float[,] eveParValCount = new float[fenleiResultNum,trainData.GetLongLength(1)-1];//记录的不同分类数，该分类下的所有记录中每个特征参数的值累加
            float[,] average = new float[fenleiResultNum, trainData.GetLongLength(1)-1];//记录的不同分类数，该分类下每个特征参数的平均值
            for (int i = 0; i < fenleiResultNum; i++)
            {
                for (int j = 0; j < trainData.GetLongLength(0); j++)
                {
                    for (int k = 0; k < trainData.GetLongLength(1)-1; k++)
                    {
                        if (trainData[j,trainData.GetLongLength(1) - 1] == i + 1)
                        {
                            eveParValCount[i,k] += trainData[j, k];//每种分类结果下每个特征参数的所有值累加
                        }
                    }
                }  
            }
            for (int i = 0; i < eveParValCount.GetLongLength(0); i++)
            {
                for (int j = 0; j < eveParValCount.GetLongLength(1); j++)
                {
                    average[i, j] = eveParValCount[i, j] / eveResultNum[i];
                    //每个分类结果每个特征参数的均值
                }
            }
            return average;
        }
        /// <summary>
        /// 获取标准差（每个分类结果，每个特征参数）
        /// </summary>
        /// <param name="trainData">训练数据</param>
        /// <param name="average">均值</param>
        /// <returns></returns>
        public static float[,] GetStandardDev(float[,] trainData,float[] eveResultNum, float[,] average)
        {
            float [,] standardDev = new float[fenleiResultNum,trainData.GetLongLength(1)-1];
            float [,] temp = new float[fenleiResultNum, trainData.GetLongLength(1)-1];
            for (int i = 0; i < fenleiResultNum; i++)
            {
                for (int j = 0; j < trainData.GetLongLength(1)-1; j++)//特征参数种类数
                {
                    for (int k = 0; k < trainData.GetLongLength(0); k++)
                    {
                        temp[i,j] += Convert.ToSingle(Math.Pow(trainData[k,j]-average[i,j],2));//每种分类结果，每个特征参数
                    }
                    standardDev[i, j] = Convert.ToSingle(Math.Sqrt(temp[i, j] / eveResultNum[i]));
                }
            }
            return standardDev;
        }
        /// <summary>
        /// 获取最大值对应的索引
        /// </summary>
        /// <param name="array1"></param>
        /// <returns></returns>
        public static String GetMaxIndex(float[] array1)
        {
            String maxIndex = "";
            float f = -1;
            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] > f)
                {
                    f = array1[i];
                    maxIndex = i.ToString();
                }
            }
            return maxIndex;
        }
        /// <summary>
        /// 验证预测正确率
        /// </summary>
        /// <param name="result">预测结果</param>
        /// <param name="check">正确结果</param>
        /// <returns></returns>
        public static float CheckCorrect(String[] result, String[] check)
        {
            float correct = 0;
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i].Trim() == check[i].Trim())
                {
                    correct++;
                }
            }
            correct /= result.Length;
            return correct;
        }


        /// <summary>
        /// 冒泡排序法，从小到大获取前k个
        /// </summary>
        /// <param name="array1"></param>
        /// <returns></returns>
        public static float[] MaoPao(float[] array1,String k = null)
        {
            if (k == null)
            {
                for (int i = 0; i < array1.Length; i++)//冒泡排序法只从小到大排序前kValue条从小到大排序
                {
                    for (int j = i + 1; j < array1.Length; j++)
                    {
                        if (array1[i] < array1[j])
                        {
                            float temp = array1[i];
                            array1[i] = array1[j];
                            array1[j] = temp;
                        }
                    }
                }
                return array1;
            }
            else
            {
                for (int i = 0; i < Convert.ToInt32(k); i++)//冒泡排序法只从小到大排序前kValue条从小到大排序
                {
                    for (int j = i + 1; j < array1.Length; j++)
                    {
                        if (array1[i] < array1[j])
                        {
                            float temp = array1[i];
                            array1[i] = array1[j];
                            array1[j] = temp;
                        }
                    }
                }
                return array1;
            }       
        }

        /// <summary>
        /// 返回一个长度为max的数组，数组的每个元素值的大小代表该值在array1数组中出现的次数
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float [] GetShwoCount(float[] array1, int max)//返回一个长度为max的数组，数组的每个元素值的大小代表该值在array1数组中出现的次数
        {
            float [] showMost = new float[max];
            for (int i = 0; i < array1.Length; i++)
            {
                showMost[(int)array1[i] - 1]++;
            }
            return showMost;
        }
        /// <summary>
        /// 根据验证数据获取验证数据正确分类
        /// </summary>
        /// <param name="testData"></param>
        /// <returns></returns>
        public static String[] GetTestResult(float[,] testData)
        {
            String[] check = new String[testData.GetLongLength(0)];
            for (int i = 0; i < check.Length; i++)
            {
                check[i] = testData[i, testData.GetLongLength(1) - 1].ToString().Trim(); ;
            }
            return check;
        }
        /// <summary>
        /// 把预测分类更新到验证数据表里
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="fenleiResult"></param>
        /// <param name="tableName"></param>
        public static void UpdataTestResult(float[,] testData,String[] fenleiResult,String tableName)
        {
            //把beyes预测结果与验证数据的正确结果拼接到一起再插入验证数据表
            String sql = null;
            for (int i = 0; i < fenleiResult.Length; i++)
            {
                fenleiResult[i] = testData[i, testData.GetLongLength(1) - 1].ToString() + "," + fenleiResult[i];
                sql = "update "+ tableName +" set d" + (testData.GetLongLength(1) - Convert.ToInt32(1)) + "='" + fenleiResult[i] + "' where id=" + (i + 1) + "";
                hjr.SQL.SqlserverHelper.ExecuteScalar(sql);
            }
        }


        /// <summary>
        /// or = 0 返回训练数据，or = 1 返回验证数据
        /// </summary>
        /// <param name="trainsNum"></param>
        /// <param name="trainProportion"></param>
        /// <param name="testProportion"></param>
        /// <param name="or"></param>
        /// <returns></returns>
        public static float[,] GetTrainsData(String trainsNum, String trainProportion, String testProportion, int or)
        {
            DataTable dt = null;
            dt = hjr.SQL.SqlserverHelper.GetDataTable("select * from o_data");
            if (trainsNum == "")
            {
                trainsNum = dt.Rows.Count.ToString();
                dt = hjr.Alg.Common.GetTrainsData("o_data", "");
            }
            else
            {
                dt = hjr.Alg.Common.GetTrainsData("o_data", trainsNum);
            }
            //根据传入的比例计算训练数据与验证数据的数目
            //训练数据行数
            int trainNum = Convert.ToInt32(Convert.ToSingle(trainsNum) * Convert.ToSingle(trainProportion) / (Convert.ToSingle(trainProportion) + Convert.ToSingle(testProportion)));
            //验证数据行数
            int testNum = Convert.ToInt32(Convert.ToSingle(trainsNum) - Convert.ToSingle(trainNum));
            int paraNum = dt.Columns.Count;
            float[,] trainData = new float[trainNum, paraNum];
            float[,] testData = new float[testNum, paraNum];

            //训练数据赋值（带预测结果）
            for (int i = 0; i < Convert.ToInt32(trainNum); i++)
            {
                for (int j = 0; j < paraNum; j++)
                {
                    trainData[i, j] = Convert.ToSingle(dt.Rows[i][j]);
                }
            }
            //验证数据赋值（带带预测结果）
            for (int i = 0; i < Convert.ToInt32(testNum); i++)
            {
                for (int j = 0; j < paraNum; j++)
                {
                    testData[i, j] = Convert.ToSingle(dt.Rows[i + Convert.ToInt32(trainNum)][j]);
                }
            }
            if (or == 0)
            {
                return trainData;
            }
            else
            {
                return testData;
            }
        }

        public static float GetMax(float [] array1)
        {
            if (float.IsNaN(array1[0]))
            {
                array1[0] = -1;
            }
            float max = array1[0];//定义变量  
            for (int i = 1; i < array1.Length; i++)
            {
                if (array1[i] > max)
                {
                    max = array1[i];
                }
            }
            return max;
        }
        public static float GetMin(float[] array1)
        {
            if (float.IsNaN(array1[0]))
            {
                array1[0] = 0;
            }
            float min = array1[0];//定义变量  
            for (int i = 1; i < array1.Length; i++)
            {
                if (array1[i] < min)
                {
                    min = array1[i];
                }
            }
            return min;
        }


        

















    }
}
