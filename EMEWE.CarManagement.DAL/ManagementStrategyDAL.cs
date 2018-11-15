using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
   public class ManagementStrategyDAL
    {
         /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(ManagementStrategy qcRecord)
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
        /// 添加多条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="tentitys"></param>
        public static bool InsertToMany(IEnumerable<ManagementStrategy> tentitys) 
        {
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                     LinQBaseDao.InsertToMany(db, tentitys);
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
        public static bool InsertOneQCRecord(ManagementStrategy qcRecord, out int rint)
        {
            rint = 0;
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {

                try
                {
                    rbool = LinQBaseDao.InsertOne(db, qcRecord);
                    rint = db.ManagementStrategy.Max(p => p.ManagementStrategy_ID);
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
        public static bool Update(Expression<Func<ManagementStrategy, bool>> fun, Action<ManagementStrategy> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<ManagementStrategy>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<ManagementStrategy, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<ManagementStrategy>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
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
        public static bool DeleteToManyByCondition(IEnumerable<ManagementStrategy> tentitys)
        {
            bool rbool = true;
            try
            {
                LinQBaseDao.DeleteToManyByCondition<ManagementStrategy>(new DCCarManagementDataContext(), tentitys);
            }
            catch
            {
                rbool = false;
            }
            return rbool;
        }
    }
}

    

