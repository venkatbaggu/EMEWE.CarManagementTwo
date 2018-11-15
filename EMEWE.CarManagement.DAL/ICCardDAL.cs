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
    public class ICCardDAL
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ICCard> Query()
        {

            return LinQBaseDao.Query<ICCard>(new DCCarManagementDataContext());

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息eh_ICCard
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ICCard> Query(Expression<Func<ICCard, bool>> fun)
        {

            return LinQBaseDao.Query<ICCard>(new DCCarManagementDataContext(), fun);

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
        /// 根据传入的sql在ICCard中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<ICCard> GetViewICCardName(string sql)
        {
            return LinQBaseDao.GetItemsForListing<ICCard>(sql);
        }

        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static ICCard Single(Expression<Func<ICCard, bool>> fun)
        {

            return LinQBaseDao.Single<ICCard>(new DCCarManagementDataContext(), fun);
        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneICCard(ICCard qcICCard)
        {
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne(db, qcICCard);
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
        public static bool Update(Expression<Func<ICCard, bool>> fun, Action<ICCard> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<ICCard>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<ICCard, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<ICCard>(new DCCarManagementDataContext(), fun);

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
        public static IEnumerable<ICCardType> GetItemsForListingEh_EnsureCard(string sql)
        {
            return LinQBaseDao.GetItemsForListing<ICCardType>(sql);
        }
    }
}
