using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;
using System.IO;

namespace EMEWE.CarManagement.Commons.CommonClass
{
    public class DBHelperAccess
    {
        public static string strAccess = System.Configuration.ConfigurationManager.ConnectionStrings["UserAccess"].ToString() + "idr.mdb";
        private static OleDbConnection conn;
        private static OleDbDataAdapter oda = new OleDbDataAdapter();
        private OleDbCommand cmd;
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static System.Data.DataSet Query(string SQLString)
        {
            DataSet ds = new DataSet();
          
            conn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strAccess + ";Jet OLEDB:DataBase PassWord=routon2005");
            try
            {
                conn.Open();
                oda = new OleDbDataAdapter(SQLString, conn);
                oda.Fill(ds, "ds");
            }
            catch
            {
                
            }
            finally
            {
                conn.Close();
            }
            return ds;
        }
        /// <summary>
        /// 查看文件是否存在
        /// </summary>
        /// <returns></returns>
        public static string GetFile()
        {
            string str = "";
            var file = new FileInfo(strAccess);
            if (!file.Exists)
            {
                 str="获取失败！";
            }
            return str;
        }
    }
}
