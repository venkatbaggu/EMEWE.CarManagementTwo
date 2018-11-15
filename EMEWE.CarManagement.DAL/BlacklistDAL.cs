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
    public class BlacklistDAL
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Blacklist> Query()
        {

            return LinQBaseDao.Query<Blacklist>(new DCCarManagementDataContext());

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息eh_Blacklist
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Blacklist> Query(Expression<Func<Blacklist, bool>> fun)
        {

            return LinQBaseDao.Query<Blacklist>(new DCCarManagementDataContext(), fun);

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息View_BlackList_CarInfo_StaffInfo_CustomerInfo
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<View_BlackList_CarInfo_StaffInfo_CustomerInfo> Query(Expression<Func<View_BlackList_CarInfo_StaffInfo_CustomerInfo, bool>> fun)
        {

            return LinQBaseDao.Query<View_BlackList_CarInfo_StaffInfo_CustomerInfo>(new DCCarManagementDataContext(), fun);

        }


        /// <summary>
        /// 根据传入的sql在eh_Blacklist中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<Blacklist> GetViewBlacklistName(string sql)
        {
            return LinQBaseDao.GetItemsForListing<Blacklist>(sql);
        }

        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static Blacklist Single(Expression<Func<Blacklist, bool>> fun)
        {

            return LinQBaseDao.Single<Blacklist>(new DCCarManagementDataContext(), fun);
        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(Blacklist qcRecord)
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
        public static bool Update(Expression<Func<Blacklist, bool>> fun, Action<Blacklist> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<Blacklist>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<Blacklist, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<Blacklist>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }
    }
}
