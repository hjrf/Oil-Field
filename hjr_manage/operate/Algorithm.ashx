﻿<%@ WebHandler Language="C#" Class="algorithm" %>

using System;
using System.Web;
using System.Data;
using System.Collections.Generic;
using System.Configuration;

public class algorithm : IHttpHandler {

    string trainsNum = null;
    string trainProportion = null;
    string testProportion = null;
    float[,] trainData = null;
    float[,] testData = null;
    float trainNum = 0;
    private static int fenleiResultNum = Convert.ToInt32(ConfigurationSettings.AppSettings["fenlei_result_num"]);

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string action = context.Request["a"];
        switch (action)
        {
            case "table_show":
                TableShow();
                break;
            case "bayes"://bayes算法
                Bayes();
                break;
            case "knn"://bayes算法
                Knn();
                break;
            case "id3"://bayes算法
                Id3();
                break;
            case "kw"://bayes算法
                Kw();
                break;
            case "kaverage"://bayes算法
                KAverage();
                break;
            case "hmm"://bayes算法
                Hmm();
                break;
            default:
                break;
        }
    }
    private void Init(String algName)
    {
        trainsNum = HttpContext.Current.Request["trainsNum"];
        trainProportion = HttpContext.Current.Request["trainProportion"];
        testProportion = HttpContext.Current.Request["testProportion"];
        trainData = hjr.Alg.Common.GetTrainsData(trainsNum,trainProportion,testProportion,0);
        testData = hjr.Alg.Common.GetTrainsData(trainsNum,trainProportion,testProportion,1);
        trainNum = Convert.ToSingle(trainData.GetLongLength(0));
        //创建训练表与测试表并导入
        hjr.SQL.SqlserverHelper.Array2insertSql("o_data_train_"+algName+"", trainData);
        hjr.SQL.SqlserverHelper.Array2insertSql("o_data_test_"+algName+"", testData);
    }
    private void TableShow()
    {
        try
        {
            string tableName = HttpContext.Current.Request["tableName"];
            DataTable dt = hjr.SQL.SqlserverHelper.GetDataTable("select * from "+ tableName +"");
            string state = string.Empty;
            state = hjr.Tools.Table.GetTableStr(dt);
            //string realName = HttpContext.Current.Request["realName"];    
            HttpContext.Current.Response.Write(state);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message);
        }
    }

    private void Bayes()
    {
        try
        {
            Init("bayes");
            //利用训练数据与验证数据数组进行Bayes运算
            //获取每种分类结果所占数目
            float[] eveResultNum = hjr.Alg.Common.GetEveResultNum(trainData);
            //把没出现的分类填充成1
            eveResultNum = hjr.Alg.Common.FillArray1(eveResultNum);
            //获取前验概率
            float[] xianYan = hjr.Alg.Common.GetXianYan(eveResultNum,trainNum);
            //获取均值(每种分类结果，每种特征参数)
            float[,] average = hjr.Alg.Common.GetAverage(trainData,eveResultNum);
            hjr.SQL.SqlserverHelper.Array2insertSql("o_result_bayes_average", average);
            //获取标准差（每种分类结果，每种特征参数）
            float[,] standardDev = hjr.Alg.Common.GetStandardDev(trainData,eveResultNum,average);
            hjr.SQL.SqlserverHelper.Array2insertSql("o_result_bayes_standard_dev", standardDev);
            //获取条件概率（每条测试记录，每种分类结果，每种特征参数）
            float[,,] tiaoJian = hjr.Alg.Bayes.GetTiaoJian(testData, average, standardDev);
            hjr.SQL.SqlserverHelper.Array3insertSql("o_result_bayes_tiaojian", tiaoJian);
            float[,] houYan = hjr.Alg.Bayes.GetHouYan(xianYan, tiaoJian);
            hjr.SQL.SqlserverHelper.Array2insertSql("o_result_bayes_houyan",houYan);
            String [] bayesResult = hjr.Alg.Bayes.GetBayseResult(houYan);
            //获取验证数据的正确结果，之后用于统计正确率
            String[] check = hjr.Alg.Common.GetTestResult(testData);
            float correct = hjr.Alg.Common.CheckCorrect(bayesResult,check);
            HttpContext.Current.Response.Write(correct);
            //把beyes预测结果与验证数据的正确结果拼接到一起再插入验证数据表
            String sql = null;
            for (int i = 0; i < bayesResult.Length; i++)
            {
                bayesResult[i] = testData[i,testData.GetLongLength(1)-1].ToString() + "," + bayesResult[i];
                sql = "update o_data_test_bayes set d"+ (testData.GetLongLength(1)-Convert.ToInt32(1)) +"='"+ bayesResult[i] +"' where id="+ (i+1) +"";
                hjr.SQL.SqlserverHelper.ExecuteScalar(sql);
            }
            sql = "if exists (select 1 from  sysobjects where id = object_id('[o_result_bayes_xianyan]') and type = 'U') drop table o_result_bayes_xianyan; create table o_result_bayes_xianyan (r_id int identity(1,1) primary key,result_num varchar(50),xianyan varchar(50))";
            hjr.SQL.SqlserverHelper.ExecuteScalar(sql);
            for (int i = 0; i < xianYan.Length; i++)
            {
                sql = "insert into o_result_bayes_xianyan (result_num,xianyan) values ('" + eveResultNum[i] + "','" + xianYan[i] + "')";
                hjr.SQL.SqlserverHelper.ExecuteScalar(sql);
            }
            string state = string.Empty;
            //string realName = HttpContext.Current.Request["realName"];    
            HttpContext.Current.Response.Write(state);
        }
        catch (Exception ex)
        {
            HttpContext.Current.Response.Write(ex.Message);
        }
    }

    private void  Knn()
    {
        //try
        //{
        Init("knn");
        string trainsNum = HttpContext.Current.Request["trainsNum"];
        string trainProportion = HttpContext.Current.Request["trainProportion"];
        string testProportion = HttpContext.Current.Request["testProportion"];
        float[,] trainData = hjr.Alg.Common.GetTrainsData(trainsNum,trainProportion,testProportion,0);
        float[,] testData = hjr.Alg.Common.GetTrainsData(trainsNum,trainProportion,testProportion,1);
        //创建训练表与测试表并导入
        hjr.SQL.SqlserverHelper.Array2insertSql("o_data_train_knn", trainData);
        hjr.SQL.SqlserverHelper.Array2insertSql("o_data_test_knn", testData);
        //利用训练数据与验证数据数组进行Knn运算
        float[,,] allODistance = hjr.Alg.Knn.GetAllODistance(trainData,testData);
        hjr.SQL.SqlserverHelper.Array3insertSql("o_result_knn_all_odistance",allODistance);
        float[,,] sortKODistance = hjr.Alg.Knn.GetSortKODistance(allODistance,5);//k值为5，获取前5条欧氏距离
        hjr.SQL.SqlserverHelper.Array3insertSql("o_result_knn_k_odistance",sortKODistance);
        String[] knnResult = hjr.Alg.Knn.GetKnnResult(sortKODistance);
        //获取验证数据的正确结果，之后用于统计正确率
        String[] check = hjr.Alg.Common.GetTestResult(testData);
        float correct = hjr.Alg.Common.CheckCorrect(knnResult,check);
        HttpContext.Current.Response.Write(correct);
        //把预测分类更新到验证数据表里
        hjr.Alg.Common.UpdataTestResult(testData,knnResult,"o_data_test_knn");
        //}
        //    catch (Exception ex)
        //    {
        //        HttpContext.Current.Response.Write(ex.Message);
        //    }
    }

    private void  Id3()
    {
        //try
        //{
        Init("id3");
        float[] eveResultNum = hjr.Alg.Common.GetEveResultNum(trainData);
        //获取前验概率
        float[] xianYan = hjr.Alg.Common.GetXianYan(eveResultNum,trainNum);
        float fenLeiShang = hjr.Alg.Id3.GetfenLeiShang(xianYan);
        float[,] fenliePoint = hjr.Alg.Id3.GetFenLiePoint(trainData,2);
        hjr.SQL.SqlserverHelper.Array2insertSql("o_result_id3_fenlie_point",fenliePoint);
        float[,] shuXingAndFenLieNum = hjr.Alg.Id3.GetShuXingAndFenLieNum(trainData,fenliePoint);
        hjr.SQL.SqlserverHelper.Array2insertSql("o_result_id3_shuxing_and__fenlie_num",shuXingAndFenLieNum);
        float[,,] shuXingAndFenLeiAndFenleiNum = hjr.Alg.Id3.GetShuXingAndFenLieAndFenleiNum(trainData,fenliePoint);
        hjr.SQL.SqlserverHelper.Array3insertSql("o_result_id3_shuxing_and__fenlie_and_fenlei_num",shuXingAndFenLeiAndFenleiNum);

        float[] shuXingShang = hjr.Alg.Id3.GetshuXingShang(trainData,shuXingAndFenLieNum,shuXingAndFenLeiAndFenleiNum);
        //将分类熵和属性熵和做差之后的信息增益插入数据库
        String sql = null;
        sql = "if exists (select 1 from  sysobjects where id = object_id('[o_result_id3_gain]') and type = 'U') drop table o_result_id3_gain; create table o_result_id3_gain (r_id int identity(1,1) primary key,fenlei_shang varchar(50),shuxing_shang varchar(50),gain varchar(50))";
        hjr.SQL.SqlserverHelper.ExecuteScalar(sql);
        for (int i = 0; i < shuXingShang.Length; i++)
        {
            sql = "insert into o_result_id3_gain (fenlei_shang,shuxing_shang,gain) values ('" + fenLeiShang + "','" + shuXingShang[i] +"','"+ (Convert.ToSingle(fenLeiShang)-Convert.ToSingle(shuXingShang[i])).ToString() +"')";
            hjr.SQL.SqlserverHelper.ExecuteScalar(sql);
        }







        //}
        //    catch (Exception ex)
        //    {
        //        HttpContext.Current.Response.Write(ex.Message);
        //    }
    }

    private void  Kw()
    {
        //try
        //{
        Init("kw");



        //}
        //    catch (Exception ex)
        //    {
        //        HttpContext.Current.Response.Write(ex.Message);
        //    }
    }

    private void KAverageRecursive()
    {
        //返回值randk，并作为参数出入递归
        KAverage();
        //最终目标得出最佳randK
    }
    private void KAverage()
    {
        Init("kaverage");
        int fenleiNum = Convert.ToInt32(HttpContext.Current.Request["fenleiNum"]);
        //前k条随机训练数据，每条数据代表一个分类
        float[,] randK = new float[fenleiNum, trainData.GetLongLength(1)];
        //保留k均值历史纪录
        List<float[,]> countRandK = new List<float[,]>();
        //新训练数据，即去除前k条数据和所有分类结果清空后的数据
        float[,] newTrainData = new float[trainData.GetLongLength(0) - fenleiNum, trainData.GetLongLength(1)];

        float[,] fenlei = new float[fenleiNum, trainData.GetLongLength(1) - 1];
        List<float[,]> fenleiResult = new List<float[,]>();
        float[,] oDistance = new float[trainData.GetLongLength(0)-fenleiNum,fenleiNum];
        //取训练数据中前k条作为随机选取的特征点，没有分类结果
        for (int i = 0; i < randK.GetLongLength(0); i++)
        {
            for (int j = 0; j < randK.GetLongLength(1); j++)
            {
                randK[i, j] = trainData[i, j];
            }
        }
        countRandK.Add(randK);
        //将除了前k条训练数据之外的数据重新赋值到一个数组，没有分类结果
        for (int i = fenleiNum; i < newTrainData.GetLongLength(0)+fenleiNum; i++)
        {
            for (int j = 0; j < newTrainData.GetLongLength(1); j++)
            {
                newTrainData[i-fenleiNum, j] = trainData[i, j];
            }
        }


        //除前k条记录的记录与前k条记录求欧氏距离
        for (int i = 0; i < newTrainData.GetLongLength(0); i++)
        {
            for (int j = 0; j < fenleiNum; j++)
            {
                for (int k = 0; k < newTrainData.GetLongLength(1) - 1; k++)
                {
                    fenlei[j, 0] += Convert.ToSingle(Math.Pow((newTrainData[i, j] - randK[j, k]), 2));
                }
                oDistance[i,j] = Convert.ToSingle(Math.Sqrt(fenlei[j, 0]));
            }
            //将最小欧氏距离对应的训练数据添加到相应分类集合中
            int minIndex = 1;
            float min = oDistance[i,0];
            for (int j = 0; j < fenleiNum; j++)
            {
                if (oDistance[i,j] < min)
                {
                    min = oDistance[i,j];
                    minIndex = j+1 ;
                }
            }
            //把新训练数据都标上分类结果
            newTrainData[i, newTrainData.GetLongLength(1) - 1] = minIndex;
        }

        //第一维是分类数目，之后的二维数组是新训练数据中遍历出的归类后的记录
        List<List<float[]>> listNewTrainData = new List <List<float[]>> ();

        for (int i = 0; i < newTrainData.GetLongLength(0); i++)
        {
            List<float[]> listTemptd = new List<float[]>();
            for (int j = 0; j < fenleiNum; j++)
            {
                if (newTrainData[i, newTrainData.GetLongLength(1) - 1] == j + 1)
                {
                    float[] temptd = new float[newTrainData.GetLongLength(1)];
                    for (int k = 0; k < newTrainData.GetLongLength(1); k++)
                    {
                        temptd[k] = newTrainData[i, k];
                        //把分类1的新训练数据赋值到一个数组中，并把这个数组赋给2维集合中
                    }
                    listTemptd.Add(temptd);
                }
            }
            //把2维集合赋值给长度为fenleiNum的集合中
            listNewTrainData.Add(listTemptd);
        }

        //统计每个均值，重新赋值给randK
        for (int i = 0; i < fenleiNum; i++)
        {
            for (int j = 0; j < listNewTrainData[i].Count; j++)
            {
                for (int k = 0; k < trainData.GetLongLength(1) - 1; k++)
                {
                    randK[i, k] += listNewTrainData[i][j][k];
                }
                for (int k = 0; k < trainData.GetLongLength(1) - 1; k++)
                {
                    randK[i, k] /= listNewTrainData[i].Count;
                }
            }
        }
        countRandK.Add(randK);

        //将k均值统计转化为三维数组,方便存入数据库

        float[,,] countRandKToArray = new float[countRandK.Count,countRandK[0].GetLongLength(0),countRandK[0].GetLongLength(1)];

        for (int i = 0; i < countRandK.Count; i++)
        {
            for (int j = 0; j < countRandK[0].GetLongLength(0); j++)
            {
                for (int k = 0; k < countRandK[0].GetLongLength(1); k++)
                {
                    countRandKToArray[i, j, k] = countRandK[i][j,k];
                }
            }
        }
        //分别计算k个方差再求和得出E
        //if  //当E小于设定值后算法停止
        //否则  //求每一个集合的均值作为新的randK[]，重新执行算法

        hjr.SQL.SqlserverHelper.Array2insertSql("o_result_kaverage_newtraindata",newTrainData);
        hjr.SQL.SqlserverHelper.Array2insertSql("o_result_kaverage_odistance",oDistance);
        hjr.SQL.SqlserverHelper.Array3insertSql("o_result_kaverage_count_randk",countRandKToArray);


    }


    private void Hmm()
    {
        Init("hmm");

        int fenlieNum = 3;//属性分裂的区间数
        //int selectPara = 2;//选择的属性
        int selectPara = Convert.ToInt32(HttpContext.Current.Request["selectPara"]);
        String[] hidden = new String[5] { "极高", "高", "中", "低", "极低" };
        float[,] fenliePoint = hjr.Alg.Id3.GetFenLiePoint(trainData,fenlieNum);
        float[] eveResultNum = hjr.Alg.Common.GetEveResultNum(trainData);
        float[] xianYan = hjr.Alg.Common.GetXianYan(eveResultNum,trainNum);
        float[,] hiddenTransform = new float[fenleiResultNum,fenleiResultNum];//隐含状态转移概率矩阵,任一状态变为任一状态的概率
        float[,] viewTransform = new float[fenleiResultNum,fenlieNum+1];//每种分类对应某属性每个分裂区间的概率矩阵

        //维特比算法 
        float[,] hmmResult = new float[testData.GetLongLength(0),fenleiResultNum];
        float[] input = new float[testData.GetLongLength(0)];

        //计算隐含状态概率转移矩阵
        for (int j = 0; j < fenleiResultNum; j++)
        {
            float[] temp = new float[fenleiResultNum];
            for (int k = 0; k < fenleiResultNum; k++)
            {
                for (int i = 0; i < trainData.GetLongLength(0) - 1; i++)
                {
                    if (trainData[i, trainData.GetLongLength(1) - 1] == j + 1 && trainData[i + 1, trainData.GetLongLength(1) - 1] == k + 1)
                    {
                        temp[k]++;
                    }
                }
            }
            float tempCount = 0;
            for (int k = 0; k < fenleiResultNum; k++)
            {
                tempCount += temp[k];
            }
            for (int k = 0; k < fenleiResultNum; k++)
            {
                hiddenTransform[j, k] = temp[k] /tempCount;
            }
        }

        //计算观测状态转移概率矩阵

        for (int k = 0; k < trainData.GetLongLength(0); k++)
        {
            for (int i = 0; i < fenleiResultNum; i++)
            {
                if (trainData[k, trainData.GetLongLength(1) - 1] == i + 1)
                {
                    if (trainData[k, selectPara - 1] < fenliePoint[selectPara - 1, 0])
                    {
                        viewTransform[i, 0]++;
                    }
                    if (trainData[k, selectPara - 1] > fenliePoint[selectPara - 1, 0] && trainData[k, selectPara - 1] <= fenliePoint[selectPara - 1, 1])
                    {
                        viewTransform[i, 1]++;
                    }
                    if (trainData[k, selectPara - 1] > fenliePoint[selectPara - 1, 1] && trainData[k, selectPara - 1] <= fenliePoint[selectPara - 1, 2])
                    {
                        viewTransform[i, 2]++;
                    }
                    if (trainData[k, selectPara - 1] > fenliePoint[selectPara - 1, 2])
                    {
                        viewTransform[i, 3]++;
                    }
                }
            }
        }

        for (int i = 0; i < fenleiResultNum; i++)
        {
            for (int j = 0; j < fenlieNum + 1; j++)
            {
                viewTransform[i, j] = viewTransform[i, j] / eveResultNum[i];
            }
        }

        hjr.SQL.SqlserverHelper.Array1insertSql("o_result_hmm_hidden", hidden);
        hjr.SQL.SqlserverHelper.Array2insertSql("o_result_hmm_fenlie", fenliePoint);
        hjr.SQL.SqlserverHelper.Array1insertSql("o_result_hmm_xianYan",xianYan);
        hjr.SQL.SqlserverHelper.Array2insertSql("o_result_hmm_hidden_transform",hiddenTransform);
        hjr.SQL.SqlserverHelper.Array2insertSql("o_result_hmm_view_transform",viewTransform);
    }























    public bool IsReusable {
        get {
            return false;
        }


    }





}
