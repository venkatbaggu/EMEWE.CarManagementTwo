using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
    public class CarTypeDAL
    {
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(CarType qcRecord)
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
        public static bool InsertOneQCRecord(CarType qcRecord, out int rint)
        {
            rint = 0;
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
              
                try
                {
                    rbool = LinQBaseDao.InsertOne(db, qcRecord);
                    rint = db.CarType.Max(p => p.CarType_ID);
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
        /// <param name="tentitys"></param>
        /// <returns></returns>
        public static bool InsertToMany(IEnumerable<CarType> tentitys)
        {
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {

                try
                {
                    LinQBaseDao.InsertToMany<CarType>(db, tentitys);

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
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static CarType Single(Expression<Func<CarType, bool>> fun)
        {

            return LinQBaseDao.Single<CarType>(new DCCarManagementDataContext(), fun);
        }
        /// <summary>
        /// LINQ更新方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="fun"></param>
        /// <param name="tentity"></param>
        /// <param name="action"></param>
        public static bool Update(Expression<Func<CarType, bool>> fun, Action<CarType> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<CarType>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<CarType, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<CarType>(new DCCarManagementDataContext(), fun);

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
        public static bool DeleteToManyByCondition(IEnumerable<CarType> tentitys)
        {
            bool rbool = true;
            try
            {
                LinQBaseDao.DeleteToManyByCondition<CarType>(new DCCarManagementDataContext(), tentitys);
            }
            catch
            {
                rbool = false;
            }
           return rbool;
        }

        /// <summary>
        /// 查询车辆类型的通行策略和管控策略
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<View_ManagementStrategy_CarType_DrivewayStrategy_ControlInfo> GetView(string sql)
        {
            return LinQBaseDao.GetItemsForListing<View_ManagementStrategy_CarType_DrivewayStrategy_ControlInfo>(sql);
        }
        /// <summary>
        /// 查询车辆类型
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<CarType> GetViewCarTypeName(string sql)
        {
            return LinQBaseDao.GetItemsForListing<CarType>(sql);
        }
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CarType> Query()
        {

            return LinQBaseDao.Query<CarType>(new DCCarManagementDataContext());

        }
    }
}
