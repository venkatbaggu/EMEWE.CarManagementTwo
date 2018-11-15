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
    public class CarAttributeDAL
    {
        /// <summary>
        /// 连接Linq To Sql 实例
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        public static IEnumerable<CarAttribute> GetCarAttributeForListing(string strsql)
        {
            DCCarManagementDataContext db = new DCCarManagementDataContext();

            var products = db.ExecuteQuery<CarAttribute>(strsql).AsEnumerable();
            return products;

        }
        /// <summary>
        /// 根据传入的sql在中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<CarAttribute> GetViewCarAttributeName(string sql)
        {
            return LinQBaseDao.GetItemsForListing<CarAttribute>(sql);
        }
        /// <summary>
        /// 查询所有信息预置皮重信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CarAttribute> Query()
        {

            return LinQBaseDao.Query<CarAttribute>(new DCCarManagementDataContext());

        }
         ///<summary>
         ///按条件查询信息View_FVN_Driveway_Position
         ///</summary>
         ///<returns></returns>
        public static IEnumerable<CarAttribute> QueryView(Expression<Func<CarAttribute, bool>> fun)
        {

            return LinQBaseDao.Query<CarAttribute>(new DCCarManagementDataContext(), fun);

        }

        /// <summary>
        /// summary>
        /// 按条件查询信息View_PresetTare_Dictionary
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CarAttribute> Query(Expression<Func<CarAttribute, bool>> fun)
        {

            return LinQBaseDao.Query<CarAttribute>(new DCCarManagementDataContext(), fun);

        }

        /// <summary>
        /// 根据传入的sql在eh_FVN中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<CarAttribute> GetViewCarAttributeType(string sql)
        {
            return LinQBaseDao.GetItemsForListing<CarAttribute>(sql);
        }

        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static CarAttribute Single(Expression<Func<CarAttribute, bool>> fun)
        {
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    return LinQBaseDao.Single<CarAttribute>(db, fun);
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
        public static bool InsertOneCarAttribute(CarAttribute eh_Fvn)
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
        public static bool Update(Expression<Func<CarAttribute, bool>> fun, Action<CarAttribute> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<CarAttribute>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<CarAttribute, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<CarAttribute>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }
    }
}
