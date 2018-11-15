using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//导入引用
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagementDAL
{
    public static class DrivewayDAL
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Driveway> Query()
        {

            return LinQBaseDao.Query<Driveway>(new DCCarManagementDataContext());

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息eh_Driveway
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Driveway> Query(Expression<Func<Driveway, bool>> fun)
        {

            return LinQBaseDao.Query<Driveway>(new DCCarManagementDataContext(), fun);

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息View_DrivewayPosition
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<View_DrivewayPosition> Query(Expression<Func<View_DrivewayPosition, bool>> fun)
        {

            return LinQBaseDao.Query<View_DrivewayPosition>(new DCCarManagementDataContext(), fun);

        }


        /// <summary>
        /// 根据传入的sql在eh_Driveway中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<Driveway> GetViewDrivewayName(string sql)
        {
            return LinQBaseDao.GetItemsForListing<Driveway>(sql);
        }

        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static Driveway Single(Expression<Func<Driveway, bool>> fun)
        {

            return LinQBaseDao.Single<Driveway>(new DCCarManagementDataContext(), fun);
        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(Driveway qcRecord)
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
        public static bool Update(Expression<Func<Driveway, bool>> fun, Action<Driveway> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<Driveway>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<Driveway, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<Driveway>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }
    }
}
