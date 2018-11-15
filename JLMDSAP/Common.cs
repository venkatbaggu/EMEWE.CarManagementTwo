using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace JLMDSAP
{
    public class Common
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public static string USER = "";
        /// <summary>
        /// 密码
        /// </summary>
        public static string PWD = "";
        #region 日志记录
        /// <summary>
        /// 记录测试记事本
        /// </summary>
        /// <param name="text">信息</param>
        public static void WriteTextLog(string text)
        {
            try
            {
                string dirpath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "log.txt";
                StreamWriter sw = new StreamWriter(dirpath, true);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + text);
                sw.Close();
            }
            catch
            {

            }
        }
        #endregion
        /// <summary>
        /// 获取数据库地址
        /// </summary>
        public static void GetDataSet()
        {
            string oldSqlStr = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionStringName"].ToString();
            if (oldSqlStr != "")
            {
                string[] sqlStr = oldSqlStr.Split(';');
                if (sqlStr.Length > 1)
                {
                    foreach (string str1 in sqlStr)
                    {
                        if (str1 != "")
                        {
                            string[] str = str1.Split('=');
                            if (str.Length > 1)
                            {

                                if (str[0] == "User ID")
                                {
                                    USER = str[1].ToString();
                                }
                                else if (str[0] == "Password")
                                {
                                    PWD = str[1].ToString();
                                }
                            }
                        }

                    }
                }
            }
        }


    }
}
