using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
    public class UnusualRecordDAL
    {
        public static DCCarManagementDataContext dc = new DCCarManagementDataContext();
        /// <summary>
        /// 添加一条LED信息
        /// </summary>
        /// <param name="pLED">LED实体</param>
        /// <returns>返回执行结果 true or false</returns>
        public static bool InsertUnusualRecord(UnusualRecord pLED)
        {
            return LinQBaseDao.InsertOne<UnusualRecord>(dc, pLED);
        }
        /// <summary>
        /// Linq的更新方法
        /// </summary>
        /// <param name="fun">条件</param>
        /// <param name="action">修改的参数</param>
        public static void UpdateUnusualRecord(Expression<Func<UnusualRecord, bool>> fun, Action<UnusualRecord> action)
        {
            LinQBaseDao.Update<UnusualRecord>(dc, fun, action);
        }
        /// <summary>
        /// Linq根据条件删除数据
        /// </summary>
        /// <param name="fun"></param>
        public static void DeleteUnusualRecord(Expression<Func<UnusualRecord, bool>> fun)
        {
            LinQBaseDao.DeleteToMany<UnusualRecord>(dc, fun);
        }
        /// <summary>
        /// LINQ更新方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="fun"></param>
        /// <param name="tentity"></param>
        /// <param name="action"></param>
        public static bool Update(Expression<Func<UnusualRecord, bool>> fun, Action<UnusualRecord> action)
        {

            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {

                    LinQBaseDao.Update<UnusualRecord>(db, fun, action);
                }
                catch
                {
                    rbool = false;
                }
                finally
                {
                    db.Connection.Close();
                }
                return rbool;
            }
        }

        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">用户实体</param>
        ///    /// <param name="rint">新增后自动增长编号</param>
        /// <returns></returns>
        public static bool InsertOneUnuRecord(UnusualRecord qcRecord, out int rint)
        {
            rint = 0;
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne(db, qcRecord);
                    rint = db.UnusualRecord.Max(p => p.UnusualRecord_ID);
                }
                catch
                {
                    rbool = false;
                }
                finally { db.Connection.Close(); }

            }
            return rbool;
        }
    }
}
