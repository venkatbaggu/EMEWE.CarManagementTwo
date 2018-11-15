using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
    public class MenuTypeDAL
    {
        public static IEnumerable<MenuType> GetItemsForListing(string strsql)
        {
            DCCarManagementDataContext db = new DCCarManagementDataContext();

            var products = db.ExecuteQuery<MenuType>(strsql).AsEnumerable();
            return products;

        }
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<MenuType> Query()
        {

            return LinQBaseDao.Query<MenuType>(new DCCarManagementDataContext());

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息MenuType
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<MenuType> Query(Expression<Func<MenuType, bool>> fun)
        {

            return LinQBaseDao.Query<MenuType>(new DCCarManagementDataContext(), fun);

        }
        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static MenuType Single(Expression<Func<MenuType, bool>> fun)
        {
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    return LinQBaseDao.Single<MenuType>(db, fun);
                }
                catch 
                {
                    return null;
                }
                finally
                {
                    db.Connection.Close();
                }
            }
        }
        /// <summary>
        /// 新增一条质检记录
        /// </summary>
        /// <param name="qcRecord">用户实体</param>
        ///    /// <param name="rint">新增后自动增长编号</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(MenuType qcRecord, out int rint)
        {
            rint = 0;
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne(db, qcRecord);
                    rint = db.UserInfo.Max(p => p.UserId);
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
        public static bool Update(Expression<Func<MenuType, bool>> fun, Action<MenuType> action)
        {

            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {

                    LinQBaseDao.Update<MenuType>(db, fun, action);
                }
                catch
                {
                    rbool = false;
                }
                finally
                {
                    db.Connection.Close();
                }
                return rbool;
            }
        }
        /// <summary>
        /// 按条件删除多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="tentitys"></param>
        /// <param name="fun"></param>
        public static bool DeleteToMany(Expression<Func<MenuType, bool>> fun)
        {
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {

                    LinQBaseDao.DeleteToMany<MenuType>(db, fun);

                }
                catch
                {
                    rbool = false;
                }
                finally { db.Connection.Close(); }
                return rbool;
            }
        }
    }
}