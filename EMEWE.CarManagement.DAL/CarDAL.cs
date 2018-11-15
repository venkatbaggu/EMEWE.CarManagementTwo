using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
    public class CarDAL
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Car> Query()
        {
            return LinQBaseDao.Query<Car>(new DCCarManagementDataContext());
        }
        /// <summary>
        /// summary>
        /// 按条件查询信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Car> Query(Expression<Func<Car, bool>> fun)
        {
            return LinQBaseDao.Query<Car>(new DCCarManagementDataContext(), fun);
        }

        /// <summary>
        /// 根据传入的sql在ICCard中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<Car> GetViewICCardName(string sql)
        {
            return LinQBaseDao.GetItemsForListing<Car>(sql);
        }

        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static Car Single(Expression<Func<Car, bool>> fun)
        {
            return LinQBaseDao.Single<Car>(new DCCarManagementDataContext(), fun);
        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool Insert(Car qcI)
        {
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne(db, qcI);
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
        public static bool Update(Expression<Func<Car, bool>> fun, Action<Car> action)
        {
            bool rbool = true;
            try
            {
                LinQBaseDao.Update<Car>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<Car, bool>> fun)
        {
            bool rbool = true;
            try
            {
                LinQBaseDao.DeleteToMany<Car>(new DCCarManagementDataContext(), fun);
            }
            catch
            {
                rbool = false;
            }
            return rbool;
        }

        /// <summary>
        /// 根据传入的sql在eh_EnsureCard中查询数据
        /// </summary>
        /// <param name="sql">查询SQl语句</param>
        /// <returns></returns>
        public static IEnumerable<Car> GetItemsForListingEh_EnsureCard(string sql)
        {
            return LinQBaseDao.GetItemsForListing<Car>(sql);
        }
    }
}
