using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace hjr.Tools
{
    /// <summary>
    /// 字符串工具
    /// </summary>
    public class Str
    {
        /// <summary>
        /// 检查是否为空，不为空返回true
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool CheckNull(params string[] param)
        {
            foreach (object item in param)
            {
                if (object.Equals(item, null))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(item.ToString()))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="encypStr">加密字符串</param>
        /// <param name="charset">编码方式</param>
        /// <returns></returns>
        public static string MD5(string encypStr, string charset)
        {
            string retStr;
            MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

            //创建md5对象
            byte[] inputBye;
            byte[] outputBye;

            //使用GB2312编码方式把字符串转化为字节数组．
            try
            {
                inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
            }
            catch (Exception ex)
            {
                inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
            }
            outputBye = m5.ComputeHash(inputBye);

            retStr = System.BitConverter.ToString(outputBye);
            retStr = retStr.Replace("-", "").ToUpper();
            return retStr;
        }

        public static string MD5(string inputStr)
        {
            String outputStr = null;
            byte[] outputByte = null;
            byte[] result = Encoding.Default.GetBytes(inputStr);
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            outputByte = md5.ComputeHash(result);
            outputStr = System.BitConverter.ToString(outputByte);
            outputStr = outputStr.Replace("-", "").ToUpper();
            return outputStr;
        }

        #region 随机数字
        public static string getRandomNum(int num, int minValue, int maxValue)
        {
            Random ra = new Random(unchecked((int)DateTime.Now.Ticks));
            int[] arrNum = new int[num];
            int tmp = 0;
            for (int i = 0; i <= num - 1; i++)
            {
                //随机取数
                tmp = ra.Next(minValue, maxValue);
                //取出值赋到数组中
                arrNum[i] = getNum(arrNum, tmp, minValue, maxValue, ra);
            }
            int t = 0;
            string temp = "";
            while (t <= arrNum.Length - 1)
            {

                temp += arrNum[t].ToString();

                t++;

            }
            return temp;

        }
        /// <summary>
        /// 返回一个与传入数组内的值不重复的值
        /// </summary>
        /// <param name="arrNum">数组</param>
        /// <param name="tmp">传入的随机数</param>
        /// <param name="minValue">生成随机数下限</param>
        /// <param name="maxValue">生成随机数上限</param>
        /// <param name="ra">随机数对象</param>
        /// <returns></returns>
        public static int getNum(int[] arrNum, int tmp, int minValue, int maxValue, Random ra)
        {
            int n = 0;
            while (n <= arrNum.Length - 1)
            {
                if (arrNum[n] == tmp) //利用循环判断是否有重复
                {
                    //重新随机获取。
                    tmp = ra.Next(minValue, maxValue);
                    //递归:如果取出来的数字和已取得的数字有重复就重新随机获取。
                    getNum(arrNum, tmp, minValue, maxValue, ra);
                }
                n++;
            }
            return tmp;
        }
        /// <summary>
        /// 正则表达式匹配与否验证
        /// </summary>
        /// <param name="express">表达式</param>
        /// <param name="str">验证字符串</param>
        /// <returns></returns>
        public static bool ZhengZeIs(string express, string str)
        {
            return Regex.IsMatch(str, express);
        }
        public static string ZhengZeResult(string express, string str)
        {
            Regex reg = new Regex(express);
            Match match = reg.Match(str);
            return match.Groups[0].Value;
        }


        #endregion

    }
}
