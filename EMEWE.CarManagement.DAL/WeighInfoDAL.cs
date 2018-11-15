using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
   public class WeighInfoDAL
    {
        /// <summary>
        /// 连接Linq To Sql 实例
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        public static IEnumerable<WeighInfo> GetCameraForListing(string strsql)
        {
            DCCarManagementDataContext db = new DCCarManagementDataContext();

            var products = db.ExecuteQuery<WeighInfo>(strsql).AsEnumerable();
            return products;

        }

        /// <summary>
        /// 查询所有信息预置皮重信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<WeighInfo> Query()
        {

            return LinQBaseDao.Query<WeighInfo>(new DCCarManagementDataContext());

        }

        /// <summary>
        /// summary>
        /// 按条件查询信息eh_Camera
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<WeighInfo> Query(Expression<Func<WeighInfo, bool>> fun)
        {

            return LinQBaseDao.Query<WeighInfo>(new DCCarManagementDataContext(), fun);

        }

        /// <summary>
        /// 根据传入的sql在eh_FVN中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<WeighInfo> GetViewCamera(string sql)
        {
            return LinQBaseDao.GetItemsForListing<WeighInfo>(sql);
        }

        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static WeighInfo Single(Expression<Func<WeighInfo, bool>> fun)
        {
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    return LinQBaseDao.Single<WeighInfo>(db, fun);
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
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneCamera(WeighInfo eh_Fvn)
        {
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne(db, eh_Fvn);
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
        public static bool Update(Expression<Func<WeighInfo, bool>> fun, Action<WeighInfo> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<WeighInfo>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<WeighInfo, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<WeighInfo>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }
    }
}
