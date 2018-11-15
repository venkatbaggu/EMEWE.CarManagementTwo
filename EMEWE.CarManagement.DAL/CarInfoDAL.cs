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
public class CarInfoDAL
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CarInfo> Query()
        {

            return LinQBaseDao.Query<CarInfo>(new DCCarManagementDataContext());

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息eh_CarInfo
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CarInfo> Query(Expression<Func<CarInfo, bool>> fun)
        {

            return LinQBaseDao.Query<CarInfo>(new DCCarManagementDataContext(), fun);

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息View_ICCard_ICCardType
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<View_ICCard_ICCardType> Query(Expression<Func<View_ICCard_ICCardType, bool>> fun)
        {

            return LinQBaseDao.Query<View_ICCard_ICCardType>(new DCCarManagementDataContext(), fun);

        }


        /// <summary>
        /// 根据传入的sql在CarInfo中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<CarInfo> GetViewCarInfoName(string sql)
        {
            return LinQBaseDao.GetItemsForListing<CarInfo>(sql);
        }

        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static CarInfo Single(Expression<Func<CarInfo, bool>> fun)
        {

            return LinQBaseDao.Single<CarInfo>(new DCCarManagementDataContext(), fun);
        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneCarInfo(CarInfo qcCarInfo)
        {
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne(db, qcCarInfo);
                }
                catch
                {
                    rbool = false;
                }
                finally { db.Connection.Close(); }

            }
            return rbool;
        }
        ///// <summary>
        ///// 新增多条记录
        ///// </summary>
        ///// <param name="qcRecord">质检实体</param>
        ///// <returns></returns>
        //public static bool InsertToMany(IEnumerable<T> Info)
        //{
        //    bool rbool = true;
        //    using (DCCarManagementDataContext db = new DCCarManagementDataContext())
        //    {
        //        try
        //        {
        //            rbool = LinQBaseDao.InsertToMany<T>(db, Info);
        //        }
        //        catch
        //        {
        //            rbool = false;
        //        }
        //        finally { db.Connection.Close(); }

        //    }
        //    return rbool;
        //}
        /// <summary>
        /// LINQ更新方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="fun"></param>
        /// <param name="tentity"></param>
        /// <param name="action"></param>
        public static bool Update(Expression<Func<CarInfo, bool>> fun, Action<CarInfo> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<CarInfo>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<CarInfo, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<CarInfo>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }
    }
}
