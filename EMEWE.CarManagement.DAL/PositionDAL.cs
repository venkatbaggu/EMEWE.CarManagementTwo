using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//导入引用
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using System.Data.Linq;
using System.Collections;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
    public class PositionDAL
    {
        /// <summary>
        /// 根据创建人获取所有门岗名称
        /// </summary>
        /// <param name="strValue">字典值</param>
        /// <returns></returns>
        //public static List<Position> GetDoorValue(string strOper)
        //{
        //    List<Position> list = new List<Position>();
        //    using (DCCarManagementDataContext db = new DCCarManagementDataContext())
        //    {
        //        try
        //        {
        //            int itemid = db.Position.First(d => (d.UserInfo.UserName == strOper && d.Position_ID != 0)).Position_ID;
        //            var v = (from c in db.Position
        //                     where c.Position_ID == itemid && c.Position_ID != 0
        //                     select c.Position_ID).ToArray();
        //            list = (from c in db.Position
        //                    where c.Position_ID != 0
        //                    select c).ToList();
        //            //----------2012-04-10 徐东冬 ----------------
        //            //Position dic = new Position();
        //            //dic.Position_ID = -1;
        //            //dic.Position_Name = "全部";
        //            //list.Add(dic);
        //        }
        //        catch 
        //        {
        //            throw new Exception(string.Format("获取所有门岗名称:{0}", ""));
        //        }
        //        finally { db.Connection.Close(); }
        //    }
        //    return list;

        //}

        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Position> Query()
        {

            return LinQBaseDao.Query<Position>(new DCCarManagementDataContext());

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息eh_Position
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Position> Query(Expression<Func<Position, bool>> fun)
        {

            return LinQBaseDao.Query<Position>(new DCCarManagementDataContext(), fun);

        }
        /// <summary>
        /// 根据传入的sql在门岗表中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<Position> GetPositionID(string sql)
        {
            return LinQBaseDao.GetItemsForListing<Position>(sql);
        }
        /// <summary>
        /// 根据传入的sql在字典表中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<Dictionary> GetDictionary(string sql)
        {
            return LinQBaseDao.GetItemsForListing<Dictionary>(sql);
        }
        /// <summary>
        /// 根据传入的sql在Position中查询数据
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static IEnumerable<Position> GetViewPosition(string sql)
        {
            return LinQBaseDao.GetItemsForListing<Position>(sql);
           
        }
        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static Position Single(Expression<Func<Position, bool>> fun)
        {

            return LinQBaseDao.Single<Position>(new DCCarManagementDataContext(), fun);
        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(Position qcRecord)
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
        public static bool Update(Expression<Func<Position, bool>> fun, Action<Position> action)
        {
            bool rbool = true;
            try
            {
                LinQBaseDao.Update<Position>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<Position, bool>> fun)
        {
            bool rbool = true;
            try
            {
                LinQBaseDao.DeleteToMany<Position>(new DCCarManagementDataContext(), fun);
            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }

        public static int MaxID(string sql)
        {
            try
            {
                return Convert.ToInt32(LinQBaseDao.Query(sql).Tables[0].Rows[0][0]);
            }
            catch
            {
                return 0;
            }
        }
    }
}

