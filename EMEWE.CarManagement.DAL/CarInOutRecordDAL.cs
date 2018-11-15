using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
    public class CarInOutRecordDAL
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CarInOutRecord> Query()
        {

            return LinQBaseDao.Query<CarInOutRecord>(new DCCarManagementDataContext());

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CarInOutRecord> Query(Expression<Func<CarInOutRecord, bool>> fun)
        {
            return LinQBaseDao.Query<CarInOutRecord>(new DCCarManagementDataContext(), fun);
        }
        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static CarInOutRecord Single(Expression<Func<CarInOutRecord, bool>> fun)
        {
            return LinQBaseDao.Single<CarInOutRecord>(new DCCarManagementDataContext(), fun);
        }
        /// <summary>
        /// LINQ更新方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="fun"></param>
        /// <param name="tentity"></param>
        /// <param name="action"></param>
        public static bool Update(Expression<Func<CarInOutRecord, bool>> fun, Action<CarInOutRecord> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<CarInOutRecord>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<CarInOutRecord, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<CarInOutRecord>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(CarInOutRecord qcRecord)
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
        /// 新增一条质检记录
        /// </summary>
        /// <param name="qcRecord">用户实体</param>
        ///    /// <param name="rint">新增后自动增长编号</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(CarInOutRecord qcRecord, out int rint)
        {
            rint = 0;
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne(db, qcRecord);
                    rint = db.CarInOutRecord.Max(p => p.CarInOutRecord_ID);
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
        /// 删除多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="tentitys"></param>
        public static bool DeleteToManyByCondition(IEnumerable<CarInOutRecord> tentitys)
        {
            bool rbool = true;
            try
            {
                LinQBaseDao.DeleteToManyByCondition<CarInOutRecord>(new DCCarManagementDataContext(), tentitys);
            }
            catch
            {
                rbool = false;
            }
            return rbool;
        }


    }
}
