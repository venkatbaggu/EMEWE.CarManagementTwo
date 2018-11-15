using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
    public class PermissionsInfoDAL
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<PermissionsInfo> Query()
        {

            return LinQBaseDao.Query<PermissionsInfo>(new DCCarManagementDataContext());

        }

        /// <summary>
        /// summary>
        /// 按条件查询信息PermissionsInfo
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<PermissionsInfo> Query(Expression<Func<PermissionsInfo, bool>> fun)
        {

            return LinQBaseDao.Query<PermissionsInfo>(new DCCarManagementDataContext(), fun);

        }

        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static PermissionsInfo Single(Expression<Func<PermissionsInfo, bool>> fun)
        {

            return LinQBaseDao.Single<PermissionsInfo>(new DCCarManagementDataContext(), fun);
        }
        /// <summary>
        /// 新增一条质检记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(PermissionsInfo qcRecord)
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
        public static bool Update(Expression<Func<PermissionsInfo, bool>> fun, Action<PermissionsInfo> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<PermissionsInfo>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<PermissionsInfo, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<PermissionsInfo>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }

    }
}
