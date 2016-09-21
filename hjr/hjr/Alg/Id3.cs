using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace hjr.Alg
{
    public static class Id3
    {
        private static int fenleiResultNum = Convert.ToInt32(ConfigurationSettings.AppSettings["fenlei_result_num"]);
        public static float GetfenLeiShang(float[] xianYan)
        {
            float fenleiShang = 0;
            float temp = 0;
            for (int i = 0; i < xianYan.Length; i++)
            {
                temp = Convert.ToSingle(-(xianYan[i]) * (Math.Log(xianYan[i], 2)));//累加（负的前验概率乘以（log以2为底前验概率的对数））
                if (float.IsNaN(temp))
                {
                    temp = 0;
                }
                fenleiShang += temp;
            }
            return fenleiShang;
        }
        /// <summary>
        /// 连续数据离散化，获取分裂点
        /// </summary>
        /// <param name="testData"></param>
        /// <param name="pointNum"></param>
        /// <returns></returns>
        public static float[,] GetFenLiePoint(float[,] testData,int pointNum)
        {
            float[,] fenLiePoint = new float[testData.GetLongLength(1)-1,pointNum];
            float[] temp = new float[testData.GetLongLength(0)];
            float tempMax =0;
            float tempMin = 0;
            float point = 0;

            for (int i = 0; i < testData.GetLongLength(1)-1; i++)
            {
                for (int j = 0; j < testData.GetLongLength(0); j++)
                {
                    temp[j] = testData[j, i];
                }
                tempMax = hjr.Alg.Common.GetMax(temp);
                tempMin = hjr.Alg.Common.GetMin(temp);
                point = (tempMax - tempMin) / fenleiResultNum; 
                for (int j = 0; j < pointNum; j++)
                {
                    tempMin += point;
                    fenLiePoint[i, j] = tempMin;
                }
            }
            return fenLiePoint;
        }

        /// <summary>
        /// 满足[属性，分裂点]
        /// </summary>
        /// <param name="trainData"></param>
        /// <param name="fenLiePoint"></param>
        /// <returns></returns>
        public static float[,] GetShuXingAndFenLieNum(float [,] trainData,float[,] fenLiePoint)
        {
            //统计每种
            float[,] countTemp = new float [trainData.GetLongLength(1)-1,fenLiePoint.GetLongLength(1)+1];//属性，分裂点数+1
            for(int i = 0;i<trainData.GetLongLength(1)-1;i++)
            {
                for (int j = 0; j < trainData.GetLongLength(0); j++)
                {
                    for (int k = 0; k < fenLiePoint.GetLongLength(1); k++)
                    {
                        if (k == 0)
                        {
                            if (trainData[j, i] < fenLiePoint[i,k])
                            {
                                countTemp[i,k]++;
                            }
                            continue;
                        }
                        if (k == fenLiePoint.GetLongLength(1)-1)
                        {
                            if (trainData[j, i] > fenLiePoint[i,k])
                            {
                                countTemp[i,k+1]++;
                            }
                        }
                        if (trainData[j, i] > fenLiePoint[i,k-1] && trainData[j, i] < fenLiePoint[i,k])
                        {
                            countTemp[i,k]++;
                        }
                    }
                } 
            }
            return countTemp;
        }
        /// <summary>
        /// 满足[属性，分裂点，分类]
        /// </summary>
        /// <param name="trainData"></param>
        /// <param name="fenLiePoint"></param>
        /// <returns></returns>
        public static float[,,] GetShuXingAndFenLieAndFenleiNum(float[,] trainData, float[,] fenLiePoint)
        {
            //统计每种
            float[,,] countTemp = new float[trainData.GetLongLength(1) - 1, fenLiePoint.GetLongLength(1) + 1,fenleiResultNum];//属性，分裂点数+1，分类
            for (int i = 0; i < trainData.GetLongLength(1) - 1; i++)
            {
                for (int j = 0; j < trainData.GetLongLength(0); j++)
                {
                    for (int k = 0; k < fenLiePoint.GetLongLength(1); k++)
                    {
                        if (k == 0)
                        {
                            if (trainData[j, i] < fenLiePoint[i, k])
                            {
                                for (int g = 0; g < fenleiResultNum; g++)
                                {
                                    if (trainData[j, trainData.GetLongLength(1) - 1] == g + 1)
                                    {
                                        countTemp[i, k, g]++;
                                    }
                                }
                            }
                            continue;
                        }
                        if (k == fenLiePoint.GetLongLength(1) - 1)
                        {
                            if (trainData[j, i] > fenLiePoint[i, k])
                            {
                                for (int g = 0; g < fenleiResultNum; g++)
                                {
                                    if (trainData[j, trainData.GetLongLength(1) - 1] == g + 1)
                                    {
                                        countTemp[i, k, g]++;
                                    }
                                }
                            }
                        }
                        if (trainData[j, i] > fenLiePoint[i, k - 1] && trainData[j, i] < fenLiePoint[i, k])
                        {
                            for (int g = 0; g < fenleiResultNum; g++)
                            {
                                if (trainData[j, trainData.GetLongLength(1) - 1] == g + 1)
                                {
                                    countTemp[i, k, g]++;
                                }
                            }
                        }
                    }
                }
            }
            return countTemp;
        }
        /// <summary>
        /// 各个属性的信息熵
        /// </summary>
        /// <param name="trainData"></param>
        /// <param name="shuXingAndFenLieNum"></param>
        /// <param name="shuXingAndFenLieAndFenleiNum"></param>
        /// <returns></returns>
        public static float[] GetshuXingShang(float[,] trainData,float[,] shuXingAndFenLieNum, float[,,] shuXingAndFenLieAndFenleiNum)
        {
            float[] shuXingShang = new float[trainData.GetLongLength(1)-1];
            float Pjk = 0;
            float temp = 0;
            float temp1 = 0;
            float temp2 = 0;
            for (int i = 0; i < trainData.GetLongLength(1)-1;i++)//属性
            {
                for (int j = 0; j < shuXingAndFenLieNum.GetLongLength(1); j++)//分裂点
                {
                    temp1 = shuXingAndFenLieNum[i,j]/trainData.GetLongLength(0);
                    if (float.IsNaN(temp1))
                    {
                        temp1 = 0;
                    }
                    for (int k = 0; k < fenleiResultNum; k++)//分类
                    {
                        temp = shuXingAndFenLieAndFenleiNum[i, j, k] / shuXingAndFenLieNum[i, j];
                        temp2 += Convert.ToSingle(-(temp*Math.Log(temp,2)));
                        if (float.IsNaN(temp2))
                        {
                            temp2 = 0;
                        }
                    }
                    Pjk += temp1 * temp2;
                    shuXingShang[i] = Pjk;
                }
                Pjk = 0;
            }
            return shuXingShang;
        }

        /// <summary>
        /// 使用泛型构建决策树
        /// </summary>
        public static List<List<String>> GetTree()
        {
            List<List<String>> tree = new List<List<String>>();
            List<String> root = new List<string>();
            List<String> node = new List<string>();
            root.Add("树根");//树根
            node.Add("2");//树枝，节点

            tree.Add(root);
            tree.Add(node);
            return tree;
        }



    }
}
