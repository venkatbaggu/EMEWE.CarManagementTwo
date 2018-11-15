using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//导入引用
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;
 
namespace EMEWE.CarManagement.DAL
{
    public class LogInfoDAL
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<LogInfo> Query()
        {

            return LinQBaseDao.Query<LogInfo>(new DCCarManagementDataContext());

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息LogInfo
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<LogInfo> Query(Expression<Func<LogInfo, bool>> fun)
        {

            return LinQBaseDao.Query<LogInfo>(new DCCarManagementDataContext(), fun);

        }

        /// <summary>
        /// 查询所有信息View_LogInfo_Dictionary
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<View_LogInfo_Dictionary> QueryView_LogInfo_Dictionary()
        {

            return LinQBaseDao.Query<View_LogInfo_Dictionary>(new DCCarManagementDataContext());

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息View_LogInfo_Dictionary
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<View_LogInfo_Dictionary> QueryView_LogInfo_Dictionary(Expression<Func<View_LogInfo_Dictionary, bool>> fun)
        {

            return LinQBaseDao.Query<View_LogInfo_Dictionary>(new DCCarManagementDataContext(), fun);

        }
        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static LogInfo Single(Expression<Func<LogInfo, bool>> fun)
        {

            return LinQBaseDao.Single<LogInfo>(new DCCarManagementDataContext(), fun);
        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">实体</param>
        /// <returns></returns>
        public static bool InsertOneLogInfo(LogInfo loginfo)
        {
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne(db, loginfo);
                }
                catch
                {
                    rbool = false;
                }
                finally { db.Connection.Close(); }

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
        public static bool Update(Expression<Func<LogInfo, bool>> fun, Action<LogInfo> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<LogInfo>(new DCCarManagementDataContext(), fun, action);
            }
            catch
            {
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
        public static bool DeleteToMany(Expression<Func<LogInfo, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<LogInfo>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }



        /// <summary>
        ///  添加日记
        /// </summary>
        /// <param name="Log_Dictionary_Name">日记类型</param>
        /// <param name="Log_Content">操作内容</param>
        /// <param name="UserNmae">操作人</param>
        /// <returns></returns>
        public static bool AddLoginfo(string Log_Dictionary_Name, string Log_Content, string UserNmae)
        {
            bool rbool = true;
            try
            {
                var varloginfo = new LogInfo()
                {
                    Log_Name = UserNmae,        // 操作人
                    Log_Time = DateTime.Parse(LinQBaseDao.Query("select GETDATE()").Tables[0].Rows[0][0].ToString()),    // 操作时间
                    Log_Content = Log_Content,  // 操作内容
                    Log_Type = DictionaryDAL.GetDictionaryID(Log_Dictionary_Name)// 操作类型

                };
                rbool = LogInfoDAL.InsertOneLogInfo(varloginfo);

            }
            catch
            {
                rbool = false;
            }
            return rbool;
        }
        /// <summary>
        /// 根据传入的sql在门岗表中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<LogInfo> GetLogInfo(string sql)
        {
            return LinQBaseDao.GetItemsForListing<LogInfo>(sql);
        }
    }
}
