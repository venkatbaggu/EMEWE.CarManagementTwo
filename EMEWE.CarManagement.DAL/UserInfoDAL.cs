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
    public class UserInfoDAL
    {
        public static IEnumerable<UserInfo> GetItemsForListing(string strsql)
        {

            DCCarManagementDataContext db = new DCCarManagementDataContext();

            var products = db.ExecuteQuery<UserInfo>(strsql).AsEnumerable();
            return products;

        }
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<UserInfo> Query()
        {

            return LinQBaseDao.Query<UserInfo>(new DCCarManagementDataContext());

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息UserInfo
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<UserInfo> Query(Expression<Func<UserInfo, bool>> fun)
        {

            return LinQBaseDao.Query<UserInfo>(new DCCarManagementDataContext(), fun);

        }

        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static UserInfo Single(Expression<Func<UserInfo, bool>> fun)
        {

            return LinQBaseDao.Single<UserInfo>(new DCCarManagementDataContext(), fun);
        }

        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        //public static UserInfo Single(Expression<Func<UserInfo, bool>> fun)
        //{
        //    using (DCCarManagementDataContext db = new DCCarManagementDataContext())
        //    {
        //        try
        //        {
        //            return LinQBaseDao.Single<UserInfo>(db, fun);
        //        }
        //        catch 
        //        {
        //            return null;
        //        }
        //        finally
        //        {
        //            db.Connection.Close();
        //        }
        //    }
        //}

        /// <summary>
        /// 新增一条质检记录
        /// </summary>
        /// <param name="qcRecord">用户实体</param>
        ///    /// <param name="rint">新增后自动增长编号</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(UserInfo qcRecord, out int rint)
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
        public static bool Update(Expression<Func<UserInfo, bool>> fun, Action<UserInfo> action)
        {

            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {

                    LinQBaseDao.Update<UserInfo>(db, fun, action);
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
        public static bool DeleteToMany(Expression<Func<UserInfo, bool>> fun)
        {
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {

                    LinQBaseDao.DeleteToMany<UserInfo>(db, fun);

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
