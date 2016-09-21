<%@ WebHandler Language="C#" Class="algorithm" %>

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

    private void KAverage()
    {
        Init("kaverage");
        int fenleiNum = Convert.ToInt32(HttpContext.Current.Request["fenleiNum"]);
        //前k条随机训练数据，每条数据代表一个分类
        float[,] randK = new float[fenleiNum, trainData.GetLongLength(1)];
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
            for (int j = 0; j < fenleiNum; j++)
            {
                if (newTrainData[i, newTrainData.GetLongLength(1) - 1] == j + 1)
                {
                    float[] temptd = new float[newTrainData.GetLongLength(1)];

                    for (int k = 0; k < newTrainData.GetLongLength(1); k++)
                    {
                        temptd[k] = newTrainData[i, k];
                        //把分类1的新训练数据赋值到一个数组中，并把这个数组赋给sortNewTrainData[0]
                    }
                    listNewTrainData[j].Add(temptd);
                }
            }
        }
        //将之前的集合转换为三维数组方便存储到数据库


        //根据分类结果把新训练数据分成k个集合
        hjr.SQL.SqlserverHelper.Array2insertSql("o_result_kaverage_newtraindata",newTrainData);
        hjr.SQL.SqlserverHelper.Array2insertSql("o_result_kaverage_odistance",oDistance);

        //分别计算k个方差再求和得出E


        //if  //当E小于设定值后算法停止

        //否则  //求每一个集合的均值作为新的randK[]，重新执行算法






    }





















    public bool IsReusable {
        get {
            return false;
        }


    }





}
