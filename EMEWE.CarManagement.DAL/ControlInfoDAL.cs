using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
  public  class ControlInfoDAL
    {
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(ControlInfo qcRecord)
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
        /// 新增一条质检记录
        /// </summary>
        /// <param name="qcRecord">用户实体</param>
        ///    /// <param name="rint">新增后自动增长编号</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(ControlInfo qcRecord, out int rint)
        {
            rint = 0;
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {

                try
                {
                    rbool = LinQBaseDao.InsertOne(db, qcRecord);
                    rint = db.ControlInfo.Max(p => p.ControlInfo_ID);
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
        public static bool Update(Expression<Func<ControlInfo, bool>> fun, Action<ControlInfo> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<ControlInfo>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<ControlInfo, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<ControlInfo>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }
        /// <summary>
        /// 删除多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dc"></param>
        /// <param name="tentitys"></param>
        public static bool DeleteToManyByCondition(IEnumerable<ControlInfo> tentitys)
        {
            bool rbool = true;
            try
            {
                LinQBaseDao.DeleteToManyByCondition<ControlInfo>(new DCCarManagementDataContext(), tentitys);
            }
            catch
            {
                rbool = false;
            }
            return rbool;
        }
        /// <summary>
        /// summary>
        /// 按条件查询信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ControlInfo> Query(Expression<Func<ControlInfo, bool>> fun)
        {

            return LinQBaseDao.Query<ControlInfo>(new DCCarManagementDataContext(), fun);

        }
    }
}
