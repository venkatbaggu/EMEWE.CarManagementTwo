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
    public class CustomerInfoDAL
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CustomerInfo> Query()
        {

            return LinQBaseDao.Query<CustomerInfo>(new DCCarManagementDataContext());

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息eh_CustomerInfo
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CustomerInfo> Query(Expression<Func<CustomerInfo, bool>> fun)
        {

            return LinQBaseDao.Query<CustomerInfo>(new DCCarManagementDataContext(), fun);

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息View_CustomerInfoPosition
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<View_BlackList_CarInfo_StaffInfo_CustomerInfo> Query(Expression<Func<View_BlackList_CarInfo_StaffInfo_CustomerInfo, bool>> fun)
        {

            return LinQBaseDao.Query<View_BlackList_CarInfo_StaffInfo_CustomerInfo>(new DCCarManagementDataContext(), fun);

        }




        /// <summary>
        /// 根据传入的sql在eh_CustomerInfo中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<CustomerInfo> GetViewCustomerInfoName(string sql)
        {
            return LinQBaseDao.GetItemsForListing<CustomerInfo>(sql);
        }



        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static CustomerInfo Single(Expression<Func<CustomerInfo, bool>> fun)
        {

            return LinQBaseDao.Single<CustomerInfo>(new DCCarManagementDataContext(), fun);
        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(CustomerInfo qcRecord)
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
        public static bool Update(Expression<Func<CustomerInfo, bool>> fun, Action<CustomerInfo> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<CustomerInfo>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<CustomerInfo, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<CustomerInfo>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }
    }
}
