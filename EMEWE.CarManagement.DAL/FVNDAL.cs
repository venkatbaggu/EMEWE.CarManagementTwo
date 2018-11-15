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
    public class FVNDAL
    {

        /// <summary>
        /// 连接Linq To Sql 实例
        /// </summary>
        /// <param name="strsql"></param>
        /// <returns></returns>
        public static IEnumerable<FVNInfo> GetFVNForListing(string strsql)
        {
            DCCarManagementDataContext db = new DCCarManagementDataContext();

            var products = db.ExecuteQuery<FVNInfo>(strsql).AsEnumerable();
            return products;

        }

        /// <summary>
        /// 查询所有信息预置皮重信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<FVNInfo> Query()
        {

            return LinQBaseDao.Query<FVNInfo>(new DCCarManagementDataContext());

        }
        /// <summary>
        /// 按条件查询信息View_FVN_Driveway_Position
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<View_FVN_Driveway_Position> QueryView(Expression<Func<View_FVN_Driveway_Position, bool>> fun)
        {

            return LinQBaseDao.Query<View_FVN_Driveway_Position>(new DCCarManagementDataContext(), fun);

        }

        /// <summary>
        /// summary>
        /// 按条件查询信息View_PresetTare_Dictionary
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<FVNInfo> Query(Expression<Func<FVNInfo, bool>> fun)
        {

            return LinQBaseDao.Query<FVNInfo>(new DCCarManagementDataContext(), fun);

        }

        /// <summary>
        /// 根据传入的sql在eh_FVN中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<FVNInfo> GetViewFVNType(string sql)
        {
            return LinQBaseDao.GetItemsForListing<FVNInfo>(sql);
        }

        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static FVNInfo Single(Expression<Func<FVNInfo, bool>> fun)
        {
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    return LinQBaseDao.Single<FVNInfo>(db, fun);
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
        public static bool InsertOneQCRecord(FVNInfo eh_Fvn)
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
        public static bool Update(Expression<Func<FVNInfo, bool>> fun, Action<FVNInfo> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<FVNInfo>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<FVNInfo, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<FVNInfo>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }
    }
}
