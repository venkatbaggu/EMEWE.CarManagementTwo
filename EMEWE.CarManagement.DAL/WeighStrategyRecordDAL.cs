using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
    public class WeighStrategyRecordDAL
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<WeighStrategyRecord> Query()
        {

            return LinQBaseDao.Query<WeighStrategyRecord>(new DCCarManagementDataContext());

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息StaffInfo
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<WeighStrategyRecord> Query(Expression<Func<WeighStrategyRecord, bool>> fun)
        {

            return LinQBaseDao.Query<WeighStrategyRecord>(new DCCarManagementDataContext(), fun);

        }

        /// <summary>
        /// 根据传入的sql在eh_StaffInfo中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<WeighStrategyRecord> GetViewStaffInfoName(string sql)
        {
            return LinQBaseDao.GetItemsForListing<WeighStrategyRecord>(sql);
        }

        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static WeighStrategyRecord Single(Expression<Func<WeighStrategyRecord, bool>> fun)
        {

            return LinQBaseDao.Single<WeighStrategyRecord>(new DCCarManagementDataContext(), fun);
        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(WeighStrategyRecord qcRecord)
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
        /// <summary>
        /// LINQ更新方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="fun"></param>
        /// <param name="tentity"></param>
        /// <param name="action"></param>
        public static bool Update(Expression<Func<WeighStrategyRecord, bool>> fun, Action<WeighStrategyRecord> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<WeighStrategyRecord>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<WeighStrategyRecord, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<WeighStrategyRecord>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }
    }
}
