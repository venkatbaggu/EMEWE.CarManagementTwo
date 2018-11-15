using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
   public class MenuInfoDAL
    {
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<MenuInfo> Query()
        {

            return LinQBaseDao.Query<MenuInfo>(new DCCarManagementDataContext());

        }
        #region --未定义视图--

        ///// <summary>
        ///// 查询所有信息View_MenuInfoRole
        ///// </summary>
        ///// <returns></returns>
        //public static IEnumerable<View_MenuInfoRole> QueryView_MenuInfoRole()
        //{

        //    return LinQBaseDao.Query<View_MenuInfoRole>(new DCCarManagementDataContext());

        //}
        ///// <summary>
        ///// summary>
        ///// 按条件查询信息QueryView_MenuInfoRole
        ///// </summary>
        ///// <returns></returns>
        //public static IEnumerable<View_MenuInfoRole> QueryView_MenuInfoRole(Expression<Func<View_MenuInfoRole, bool>> fun)
        //{

        //    //return LinQBaseDao.Query<View_MenuInfoRole>(new DCCarManagementDataContext(), fun);

        //}
        ///// <summary>
        ///// 查询所有信息View_Menu_ControlRole
        ///// </summary>
        ///// <returns></returns>
        //public static IEnumerable<View_Menu_ControlRole> QueryView_Menu_ControlRole()
        //{
        //    return LinQBaseDao.Query<View_Menu_ControlRole>(new DCCarManagementDataContext());

        //}
        ///// <summary>
        ///// summary>
        ///// 按条件查询信息View_Menu_ControlRole
        ///// </summary>
        ///// <returns></returns>
        //public static IEnumerable<View_Menu_ControlRole> QueryView_Menu_ControlRole(Expression<Func<View_Menu_ControlRole, bool>> fun)
        //{

        //    return LinQBaseDao.Query<View_Menu_ControlRole>(new DCCarManagementDataContext(), fun);

        //}
        ///// <summary>
        ///// summary>
        ///// 按条件查询信息View_Menu_ControlRole
        ///// </summary>
        ///// <returns></returns>
        //public static IEnumerable<View_Menu_ControlRole> QueryView_Menu_ControlRole(Expression<Func<View_Menu_ControlRole, bool>> expr)
        //{

        //    return LinQBaseDao.Query<View_Menu_ControlRole>(new DCCarManagementDataContext(), expr);

        //}
        #endregion
        /// <summary>
        /// summary>
        /// 按条件查询信息MenuInfo
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<MenuInfo> Query(Expression<Func<MenuInfo, bool>> fun)
        {

            return LinQBaseDao.Query<MenuInfo>(new DCCarManagementDataContext(), fun);

        }
        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static MenuInfo Single(Expression<Func<MenuInfo, bool>> fun)
        {

            return LinQBaseDao.Single<MenuInfo>(new DCCarManagementDataContext(), fun);
        }
        /// <summary>
        /// 新增一条质检记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(MenuInfo qcRecord)
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
        public static bool Update(Expression<Func<MenuInfo, bool>> fun, Action<MenuInfo> action)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.Update<MenuInfo>(new DCCarManagementDataContext(), fun, action);
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
        public static bool DeleteToMany(Expression<Func<MenuInfo, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<MenuInfo>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }
        /// <summary>
        /// 根据菜单名称 返回菜单ID
        /// </summary>
        /// <param name="strValue">菜单名称</param>
        /// <returns></returns>
        public static int GetMenuID(string strValue)
        {

            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    return db.MenuInfo.First(d => (d.Menu_ControlText == strValue)).Menu_ID;


                }
                catch 
                {
                    return 0;
                    

                }
                finally { db.Connection.Close(); }


            }

        }
        /// <summary>
        /// 根据菜单ID返回菜单名称 
        /// </summary>
        /// <param name="strValue">菜单ID</param>
        /// <returns></returns>
        public static string GetValueState(int MenuID)
        {

            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    return db.MenuInfo.First(d => (d.Menu_ID == MenuID)).Menu_ControlText;


                }
                catch {
                    return "";
                  

                }
                finally { db.Connection.Close(); }


            }

        }

        public static bool InsertOneQCRecord(MenuType qcRecord, out int rint)
        {
            rint = 0;
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne(db, qcRecord);
                    rint = db.UserInfo.Max(p => p.UserId);
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
        public static bool Update(Expression<Func<MenuType, bool>> fun, Action<MenuType> action)
        {

            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {

                    LinQBaseDao.Update<MenuType>(db, fun, action);
                }
                catch
                {
                    rbool = false;
                }
                finally
                {
                    db.Connection.Close();
                }
                return rbool;
            }
        }
        public static bool DeleteMenuinfo(Expression<Func<MenuType, bool>> fun)
        {
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    LinQBaseDao.DeleteToMany<MenuType>(db, fun);
                }
                catch
                {
                    rbool = false;
                }
                finally { db.Connection.Close(); }
                return rbool;
            }
        }
    }
}
