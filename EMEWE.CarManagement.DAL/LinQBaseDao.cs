using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.SqlClient;
using System.Data.Linq;
using System.Data;
using System.Collections;
using System.IO;

namespace EMEWE.CarManagement.DAL
{
    public class LinQBaseDao
    {
        //数据库连接字符串(web.config来配置)，可以动态更改connectionString支持多数据库.
        /// <summary>
        /// 车辆管理系统连接
        /// </summary>
        public static readonly string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EMEWEQCConnectionString"].ToString();
        public static readonly string connectionString2 = System.Configuration.ConfigurationManager.ConnectionStrings["EMEWEQCConnectionString2"].ToString();
        /// <summary>
        /// 查询所有的记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(DCCarManagementDataContext dc) where T : class
        {

            return dc.GetTable<T>().AsEnumerable<T>().ToList();
        }
        /// <summary>
        /// 按条件查询记录拼接
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(DCCarManagementDataContext dc, Expression<Func<T, bool>> expr) where T : class
        {
            return dc.GetTable<T>().Where<T>(expr).AsEnumerable<T>().ToList();

        }
        public static IEnumerable<T> Query<T>(DCCarManagementDataContext dc, Action<T> tentity) where T : class
        {
            return dc.GetTable<T>().AsEnumerable<T>();
        }
        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static T Single<T>(DCCarManagementDataContext dc, Expression<Func<T, bool>> fun) where T : class
        {
            return dc.GetTable<T>().Single<T>(fun);
        }

        /// <summary>
        /// 添加一条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="tentity"></param>
        public static bool InsertOne<T>(DCCarManagementDataContext dc, T tentity) where T : class
        {
            bool rbool = true;
            try
            {
                var table = dc.GetTable<T>();
                table.InsertOnSubmit(tentity);
                dc.SubmitChanges();

            }
            catch
            {

                rbool = false;
            }
            return rbool;

        }
        /// <summary>
        /// 添加多条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="tentitys"></param>
        public static bool InsertToMany<T>(DCCarManagementDataContext dc, IEnumerable<T> tentitys) where T : class
        {
            //var table = dc.GetTable<T>();
            //table.InsertAllOnSubmit(tentitys);
            //dc.SubmitChanges();
            bool rbool = true;
            try
            {
                var table = dc.GetTable<T>();
                table.InsertAllOnSubmit(tentitys);
                dc.SubmitChanges();
            }
            catch (ChangeConflictException)
            {
                dc.ChangeConflicts.ResolveAll(RefreshMode.KeepCurrentValues);
                dc.SubmitChanges();
                rbool = false;
            }
            return rbool;
        }

        /// <summary>
        /// 按条件删除多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="tentitys"></param>
        /// <param name="fun"></param>
        public static bool DeleteToMany<T>(DCCarManagementDataContext dc, Expression<Func<T, bool>> fun) where T : class
        {
            //var table = dc.GetTable<T>();
            //var result = table.Where<T>(fun).AsEnumerable<T>();
            //table.DeleteAllOnSubmit<T>(result);
            //dc.SubmitChanges();
            bool rbool = true;
            try
            {
                var table = dc.GetTable<T>();
                var result = table.Where<T>(fun).AsEnumerable<T>();
                table.DeleteAllOnSubmit<T>(result);
                dc.SubmitChanges();

            }
            catch (ChangeConflictException)
            {
                dc.ChangeConflicts.ResolveAll(RefreshMode.KeepCurrentValues);
                dc.SubmitChanges();
                rbool = false;
            }
            return rbool;
        }

        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="tentitys"></param>
        public static bool DeleteToManyByCondition<T>(DCCarManagementDataContext dc, IEnumerable<T> tentitys) where T : class
        {
            //var table = dc.GetTable<T>();
            //table.DeleteAllOnSubmit<T>(tentitys);
            //dc.SubmitChanges();
            bool rbool = true;
            try
            {
                var table = dc.GetTable<T>();
                table.DeleteAllOnSubmit<T>(tentitys);
                dc.SubmitChanges();
            }
            catch (ChangeConflictException)
            {
                dc.ChangeConflicts.ResolveAll(RefreshMode.KeepCurrentValues);
                dc.SubmitChanges();
                rbool = false;
            }
            return rbool;
        }

        /// <summary>
        /// LINQ更新方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="fun"></param>
        /// <param name="tentity"></param>
        /// <param name="action"></param>
        public static bool Update<T>(DCCarManagementDataContext dc, Expression<Func<T, bool>> fun, Action<T> action) where T : class
        {
            //var table = dc.GetTable<T>().Single<T>(fun);
            //action(table);
            //dc.SubmitChanges();
            bool rbool = true;
            try
            {
                var table = dc.GetTable<T>().Single<T>(fun);
                action(table);

                dc.SubmitChanges();
            }
            catch (ChangeConflictException)
            {
                dc.ChangeConflicts.ResolveAll(RefreshMode.KeepCurrentValues);
                dc.SubmitChanges();
                rbool = false;
            }
            return rbool;
        }
        /// <summary>
        /// 执行多操作
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="dc"></param>
        /// <param name="intindex">0：添加操作 1：修改操作 2：删除操作 3:添加多条记录 4:删除多条记录</param>
        /// <param name="tentity">要添加的实体</param>
        /// <param name="fun">修改或删除的条件</param>
        /// <param name="action">修改的值</param>
        /// <param name="rbool">ture:执行操作 FALSE:不执行</param>
        /// <param name="tentitys">要添加或都要删除的实体集合</param>
        /// <returns></returns>
        public static bool ADD_Delete_UpdateMethod<T>(DCCarManagementDataContext dc, int intindex, T tentity, Expression<Func<T, bool>> fun, Action<T> action, bool rbool, IEnumerable<T> tentitys) where T : class
        {
            bool falg = true;
            if (intindex == 0)//添加
            {
                var table = dc.GetTable<T>();
                table.InsertOnSubmit(tentity);
            }
            if (intindex == 3)//添加多条记录
            {
                var table = dc.GetTable<T>();
                table.InsertAllOnSubmit(tentitys);

            }
            if (intindex == 1)//修改
            {
                var table = dc.GetTable<T>().Single<T>(fun);
                action(table);
            }
            if (intindex == 2)//删除
            {
                var table = dc.GetTable<T>();
                var result = table.Where<T>(fun).AsEnumerable<T>();

                table.DeleteAllOnSubmit<T>(result);
            }
            if (intindex == 4)//删除多条记录
            {
                var table = dc.GetTable<T>();
                table.InsertAllOnSubmit(tentitys);

            }
            if (rbool)
            {
                try
                {

                    dc.SubmitChanges();
                }
                catch (ChangeConflictException err)
                {
                    dc.ChangeConflicts.ResolveAll(RefreshMode.KeepCurrentValues);
                    dc.SubmitChanges();
                    falg = false;
                }

            }
            return falg;

        }
        /// <summary>
        /// 传入SQL 查询
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        public static IEnumerable<T> GetItemsForListing<T>(string strsql) where T : class
        {
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                return db.ExecuteQuery<T>(strsql).ToList();
            }
        }
        #region  执行SQL语句
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        cmd.CommandTimeout = 3;
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (Exception ex)
                    {
                        string str = ex.Message;
                        if (str.IndexOf("无法访问服务器") >= 0)
                        {
                            WriteTextLog(ex.Message);
                        }
                        return 0;
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static int ExecuteSqlTran(ArrayList SQLStringList)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                SqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    int count = 0;
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            count += cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                    return count;
                }
                catch
                {
                    tx.Rollback();
                    return 0;
                }
                finally
                {
                    cmd.Dispose();
                    conn.Close();
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static System.Data.DataSet Query(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                SqlDataAdapter command = null;
                try
                {
                    connection.Open();
                    command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                    if (str.IndexOf("无法访问服务器") >= 0)
                    {
                        WriteTextLog(ex.Message);
                    }
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return ds;
            }
        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static System.Data.DataSet Querys(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString2))
            {
                DataSet ds = new DataSet();
                SqlDataAdapter command = null;
                try
                {
                    connection.Open();
                    command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (Exception ex)
                {
                    string str = ex.Message;
                    if (str.IndexOf("无法访问服务器") >= 0)
                    {
                        WriteTextLog(ex.Message);
                    }
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return ds;
            }
        }
        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (Exception ex)
                    {
                        string str = ex.Message;
                        if (str.IndexOf("无法访问服务器") >= 0)
                        {
                            WriteTextLog(ex.Message);
                        }
                        return null;
                    }
                    finally
                    {
                        cmd.Dispose();
                        connection.Close();
                    }
                }
            }
        }
        #endregion
        public static List<T> ListT<T>(DCCarManagementDataContext dc, Expression<Func<T, bool>> fun) where T : class
        {
            return dc.GetTable<T>().Where<T>(fun).AsEnumerable<T>().ToList();

        }


        /// <summary>
        ///检测连接是否可以打开
        /// </summary>
        /// <param name="conn">连接字符串</param>
        /// <returns></returns>
        public static bool DetectionConn(string conn)
        {
            bool f = false;
            try
            {
                using (DataContext dc = new DataContext(conn))
                {

                    dc.Connection.Open();
                    f = true;
                    dc.Connection.Close();
                }
            }
            catch
            {
                f = false;
                // Util.WriteTextLog("DetectionConn():" + ex.ToString());
                //throw ex;
            }
            return f;
        }

        public static List<string> GetView(string procName, string ViewName)
        {
            List<string> list = new List<string>();
            SqlConnection conn = new SqlConnection(connectionString);

            // 建立一个SqlCommand对象,用于数据库操作
            SqlCommand sqlcmd = new SqlCommand(procName, conn);
            //cmd.CommandText = procName; //CouldLogin为SP的名字
            sqlcmd.CommandType = CommandType.StoredProcedure; //指定命令类型为存储过程
            //Input参数声明
            SqlParameter sqlp = new SqlParameter("@ViewName", SqlDbType.VarChar, 50);
            sqlp.Value = ViewName;
            sqlcmd.Parameters.Add(sqlp);
            //cmd.Parameters.Add("@ViewName", SqlDbType.VarChar, 50);
            //cmd.Parameters["@ViewName"].Value = ViewName;
            //输出
            sqlcmd.Parameters.Add("@AllFieldname", SqlDbType.VarChar, 5000);
            sqlcmd.Parameters["@AllFieldname"].Direction = ParameterDirection.Output;
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                SqlDataReader sdr = sqlcmd.ExecuteReader();
                while (sdr.Read())
                {
                    list.Add(sdr["字段名"].ToString());
                }
            }
            catch
            {

            }
            finally
            {
                conn.Close();
            }
            return list;
        }
        /// <summary>
        /// 返回查出的行数
        /// </summary>
        /// <param name="arraylist">存放查询语句的数组</param>
        /// <returns></returns>
        public static int GetCount(ArrayList arraylist)
        {
            int intcount = 0;
            try
            {

                for (int i = 0; i < arraylist.Count; i++)
                {
                    intcount += Query(arraylist[i].ToString()).Tables[0].Rows.Count;
                }
            }
            catch
            { }
            return intcount;

        }
        /// <summary>
        /// 记录测试记事本
        /// </summary>
        /// <param name="text">信息</param>
        public static void WriteTextLog(string text)
        {
            try
            {
                string dirpath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "LOG//" + GetServersTime().ToString("yyyyMMdd") + "log.txt";
                if (!File.Exists(dirpath))
                {
                    FileStream fs1 = new FileStream(dirpath, FileMode.Create, FileAccess.Write);//创建写入文件
                    fs1.Close();
                }
                StreamWriter sw = new StreamWriter(dirpath, true);
                sw.WriteLine(GetServersTime().ToString("yyyy-MM-dd HH:mm:ss") + ": " + text);
                sw.Close();
            }
            catch
            {

            }
        }
        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetServersTime()
        {
            try
            {
                return DateTime.Parse(LinQBaseDao.Query("select GETDATE()").Tables[0].Rows[0][0].ToString());
            }
            catch
            {

                return DateTime.Parse("1900-01-01 00:00:00.000");
            }

        }

    }
}
