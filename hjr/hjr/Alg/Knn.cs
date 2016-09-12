using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hjr.Alg
{
   public static class Knn
   {
       /// <summary>
       /// 计算全部欧氏距离[验证样本数][训练样本数+1]加的1是为了存储分类结果 
       /// </summary>
       /// <param name="trainData"></param>
       /// <param name="testData"></param>
       /// <returns></returns>
        public static float[,,] GetAllODistance(float[,] trainData, float[,] testData)
        {
            float [,,] allODistance = new float[testData.GetLongLength(0),trainData.GetLongLength(0),2];
            for (int i = 0; i < testData.GetLongLength(0); i++)
            {
                for (int j = 0; j < trainData.GetLongLength(0); j++)
                {
                    for (int k = 0; k < trainData.GetLongLength(1)-1; k++)
                    {
                        allODistance[i,j,0] += Convert.ToSingle(Math.Pow((trainData[j,k] - testData[i,k]), 2));          
                    }                                                   
                    allODistance[i,j,0] = Convert.ToSingle(Math.Sqrt(allODistance[i,j,0]));
                    allODistance[i, j, 1] = trainData[j, trainData.GetLongLength(1) - 1];
                }
            }
            return allODistance;
        }

        /// <summary>
        /// 冒泡排序法，从小到大获取前k个，并保留分类
        /// </summary>
        /// <param name="array2"></param>
        /// <returns></returns>
        public static float[,,] GetSortKODistance(float[,,] allODistance, int kValue)
        { 

            float[,,] sortKODistance = new float[allODistance.GetLongLength(0),kValue,2];
            for (int i = 0; i < allODistance.GetLongLength(0); i++)
            {
                for (int j = 0; j < kValue; j++)
                {
                    for (int k = j + 1; k < allODistance.GetLongLength(1); k++)
                    {
                        if (allODistance[i,j,0] > allODistance[i,k,0])
                        {
                            float temp = allODistance[i,k,0];
                            allODistance[i,k,0] = allODistance[i,j,0];
                            allODistance[i,j,0] = temp;//欧式距离排序
                            
                            float temp1 = allODistance[i,k,1];
                            allODistance[i,k,1] = allODistance[i,j,1];
                            allODistance[i,j,1] = temp1;//将分类结果与欧式距离一起排序
                        }
                    }
                }
            }
            for (int i = 0; i < sortKODistance.GetLongLength(0); i++)
            {
                for (int j = 0; j < sortKODistance.GetLongLength(1); j++)
                {
                    sortKODistance[i, j, 0] = allODistance[i,j,0];
                    sortKODistance[i, j, 1] = allODistance[i, j, 1];
                }
             }
            return sortKODistance;
        }


        public static String[] GetKnnResult(float[,,] sortKODistance)
        {
            float[] minODistance_fenlei = new float[sortKODistance.GetLongLength(1)];
            String [] knnResult = new String[sortKODistance.GetLongLength(0)];

            for (int i = 0; i < sortKODistance.GetLongLength(0); i++)
            {
                for (int j = 0; j < sortKODistance.GetLongLength(1); j++)
                {
                    minODistance_fenlei[j] = sortKODistance[i, j, 1];
                }

                for (int j = 0; j < sortKODistance.GetLongLength(1); j++)
                {
                    float[] temp = hjr.Alg.Common.GetShwoCount(minODistance_fenlei,6);
                    knnResult[i] = Convert.ToString(Convert.ToInt32(hjr.Alg.Common.GetMaxIndex(temp)) + 1);
                }
            }
            return knnResult;
        }





















    }
}
