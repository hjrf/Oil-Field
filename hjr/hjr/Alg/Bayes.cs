using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hjr.Alg
{
    public class Bayes
    {
        private static int fenleiResultNum = Convert.ToInt32(ConfigurationSettings.AppSettings["fenlei_result_num"]);
        /// <summary>
        /// 获取条件概率
        /// </summary>
        /// <param name="testData">测试数据</param>
        /// <param name="average">均值</param>
        /// <param name="standardDev">标准差</param>
        /// <returns></returns>
        public static float[,,] GetTiaoJian(float[,] testData, float[,] average, float[,] standardDev)
        {
            float[,,] tiaoJian = new float[testData.GetLongLength(0), fenleiResultNum,testData.GetLongLength(1)-1];
            for (int i = 0; i < testData.GetLongLength(0); i++)
            {
                for (int j = 0; j < fenleiResultNum; j++)
                {
                    for (int k = 0; k < testData.GetLongLength(1) - 1; k++)
                    {
                        tiaoJian[i,j,k] = Convert.ToSingle(Math.Pow((testData[i, k] - average[j, k]), 2) / (2 * Math.Pow(standardDev[j, k], 2)));
                        tiaoJian[i,j,k] = Convert.ToSingle(Math.Exp(-tiaoJian[i,j,k]));
                        tiaoJian[i,j,k] = tiaoJian[i, j, k]/Convert.ToSingle((Math.Sqrt(2 * 3.1415926) * standardDev[j, k]));
                    }
                }
            }
            return tiaoJian;
        }
        /// <summary>
        /// 计算后验概率
        /// </summary>
        /// <param name="beforePro">前验概率</param>
        /// <param name="conditionalPro">条件概率</param>
        /// <returns></returns>
        public static float[,] GetHouYan(float[] qianYan, float[,,] tiaoJian)
        {
            float[,] houYan = new float[tiaoJian.GetLongLength(0),fenleiResultNum];
            for (int i = 0; i < tiaoJian.GetLongLength(0); i++)
            {
                for (int j = 0; j < tiaoJian.GetLongLength(1); j++)
                {
                    houYan[i, j] = 1;
                    for (int k = 0; k < tiaoJian.GetLongLength(2); k++)
                    {
                        houYan[i, j] *= tiaoJian[i, j, k]; 
                    }
                    houYan[i, j] *= qianYan[j];     
                }
            }
            return houYan;
        }
        /// <summary>
        /// 根据后验概率获取bayes分类结果
        /// </summary>
        /// <param name="afterPro"></param>
        /// <returns></returns>
        public static String[] GetBayseResult(float[,] houYan)
        {
            String[] bayesResult = new String[houYan.GetLongLength(0)];
            float[] temp = new float[fenleiResultNum];
            for (int i = 0; i < houYan.GetLongLength(0); i++)
            {
                for (int j = 0; j < houYan.GetLongLength(1); j++)
                {
                    temp[j] = houYan[i,j];
                }
                bayesResult[i] = Convert.ToString(Convert.ToInt32(Common.GetMaxIndex(temp)) + 1);//加1是因为获取的索引是从0开始的，而级别是从1开始的
            }
            return bayesResult;
        }

    }
}
