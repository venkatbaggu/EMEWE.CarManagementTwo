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
    public class ICCardTypeDAL
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ICCardType> Query()
        {

            return LinQBaseDao.Query<ICCardType>(new DCCarManagementDataContext());

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息eh_ICCardType
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ICCardType> Query(Expression<Func<ICCardType, bool>> fun)
        {

            return LinQBaseDao.Query<ICCardType>(new DCCarManagementDataContext(), fun);

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息View_ICCardTypePosition
        /// </summary>
        /// <returns></returns>
        //public static IEnumerable<View_DrivewayPosition> Query(Expression<Func<View_ICCardTypePosition, bool>> fun)
        //{

        //    return LinQBaseDao.Query<View_DrivewayPosition>(new DCCarManagementDataContext(), fun);

        //}


        /// <summary>
        /// 根据传入的sql在ICCardType中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<ICCardType> GetViewICCardTypeName(string sql)
        {
            return LinQBaseDao.GetItemsForListing<ICCardType>(sql);
        }

        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static ICCardType Single(Expression<Func<ICCardType, bool>> fun)
        {

            return LinQBaseDao.Single<ICCardType>(new DCCarManagementDataContext(), fun);
        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneICCardType(ICCardType qcICCardType)
        {
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne(db, qcICCardType);
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
        public static bool Update(Expression<Func<ICCardType, bool>> fun, Action<ICCardType> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<ICCardType>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<ICCardType, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<ICCardType>(new DCCarManagementDataContext(), fun);

            }
            catch 
            {
                rbool = false;
            }
            return rbool;

        }
    }
}
