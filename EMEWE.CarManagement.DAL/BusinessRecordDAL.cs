using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
    public class BusinessRecordDAL
    {
        /// <summary>
        /// 按条件删除多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="tentitys"></param>
        /// <param name="fun"></param>
        public static bool DeleteToMany(Expression<Func<BusinessRecord, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<BusinessRecord>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
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
        public static bool Update(Expression<Func<BusinessRecord, bool>> fun, Action<BusinessRecord> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<BusinessRecord>(new DCCarManagementDataContext(), fun, action);
            }
            catch
            {
                rbool = false;
            }
            return rbool;
        }
        /// <summary>
        /// summary>
        /// 按条件查询信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<BusinessRecord> Query(Expression<Func<BusinessRecord, bool>> fun)
        {

            return LinQBaseDao.Query<BusinessRecord>(new DCCarManagementDataContext(), fun);

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<View_BusinessRecord_CarInfo> ViewQuery(Expression<Func<View_BusinessRecord_CarInfo, bool>>fun)
        {

            return LinQBaseDao.Query<View_BusinessRecord_CarInfo>(new DCCarManagementDataContext(), fun);

        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(BusinessRecord qcRecord)
        {
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne(db, qcRecord);
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
