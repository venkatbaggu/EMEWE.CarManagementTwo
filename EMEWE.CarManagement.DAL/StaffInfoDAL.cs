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
    public class StaffInfoDAL
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<StaffInfo> Query()
        {

            return LinQBaseDao.Query<StaffInfo>(new DCCarManagementDataContext());

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息StaffInfo
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<StaffInfo> Query(Expression<Func<StaffInfo, bool>> fun)
        {

            return LinQBaseDao.Query<StaffInfo>(new DCCarManagementDataContext(), fun);

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息View_StaffInfo_ICCard
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<View_StaffInfo_ICCard> Query(Expression<Func<View_StaffInfo_ICCard, bool>> fun)
        {

            return LinQBaseDao.Query<View_StaffInfo_ICCard>(new DCCarManagementDataContext(), fun);

        }


        /// <summary>
        /// 根据传入的sql在eh_StaffInfo中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<StaffInfo> GetViewStaffInfoName(string sql)
        {
            return LinQBaseDao.GetItemsForListing<StaffInfo>(sql);
        }

        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static StaffInfo Single(Expression<Func<StaffInfo, bool>> fun)
        {

            return LinQBaseDao.Single<StaffInfo>(new DCCarManagementDataContext(), fun);
        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(StaffInfo qcRecord)
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
        public static bool Update(Expression<Func<StaffInfo, bool>> fun, Action<StaffInfo> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<StaffInfo>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<StaffInfo, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<StaffInfo>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }
    }
}
