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
    public class BusinessTypeDAL
    {
        /// <summary>
        /// 连接Linq To Sql 实例
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        public static IEnumerable<BusinessType> GetFVNForListing(string strsql)
        {
            DCCarManagementDataContext db = new DCCarManagementDataContext();

            var products = db.ExecuteQuery<BusinessType>(strsql).AsEnumerable();
            return products;

        }

        /// <summary>
        /// 查询所有信息预置皮重信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<BusinessType> Query()
        {

            return LinQBaseDao.Query<BusinessType>(new DCCarManagementDataContext());

        }
        /// <summary>
        /// 按条件查询信息View_FVN_Driveway_Position
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<BusinessType> QueryView(Expression<Func<BusinessType, bool>> fun)
        {

            return LinQBaseDao.Query<BusinessType>(new DCCarManagementDataContext(), fun);

        }

        /// <summary>
        /// summary>
        /// 按条件查询信息View_PresetTare_Dictionary
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<BusinessType> Query(Expression<Func<BusinessType, bool>> fun)
        {

            return LinQBaseDao.Query<BusinessType>(new DCCarManagementDataContext(), fun);

        }

        /// <summary>
        /// 根据传入的sql在eh_FVN中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<BusinessType> GetViewBusinessType(string sql)
        {
            return LinQBaseDao.GetItemsForListing<BusinessType>(sql);
        }

        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static BusinessType Single(Expression<Func<BusinessType, bool>> fun)
        {
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    return LinQBaseDao.Single<BusinessType>(db, fun);
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
        public static bool InsertOneBusinessType(BusinessType eh_Fvn)
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
        public static bool Update(Expression<Func<BusinessType, bool>> fun, Action<BusinessType> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<BusinessType>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<BusinessType, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<BusinessType>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }
    }
}
