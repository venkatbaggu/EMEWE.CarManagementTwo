using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
  public  class RoleInfoAdd
    {
        //查询所有信息
        public static IEnumerable<RoleInfo> Query()
        {
            return LinQBaseDao.Query<RoleInfo>(new DCCarManagementDataContext());
        }
        /// <summary>
        /// 按条件查询 RoleInfo
        /// </summary>
        /// <param name="fun"></param>
        /// <returns></returns>
        public static IEnumerable<RoleInfo> Query(Expression<Func<RoleInfo, bool>> fun)
        {
            return LinQBaseDao.Query<RoleInfo>(new DCCarManagementDataContext(), fun);
        }
        /// 新增一条质检记录
        /// </summary>
        /// <param name="qcRecord">QCRecord质检实体</param>
        /// <param name="rint">新增后自动增长编号</param>
        /// <returns></returns>
        public static bool InsertOneRoleInfo(RoleInfo qcRecord, out int rint)
        {
            rint = 0;
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne(db, qcRecord);
                    rint = db.RoleInfo.Max(p => p.Role_Id);

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
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">对象</param>
        /// <returns></returns>
        public static bool InsertOneRoleInfo(RoleInfo qcRecord)
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
                finally
                {
                    db.Connection.Close();
                }

            }
            return rbool;
        }
        /// <summary>
        /// 根据ID删除数据（单条）
        /// </summary>
        /// <param name="RoleInfoid">角色编号</param>
        /// <returns></returns>
        public static bool DeleteToMany(int RoleInfoid)
        {
            bool rbool = true;
            Expression<Func<RoleInfo, bool>> p = n => n.Role_Id == RoleInfoid;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    LinQBaseDao.DeleteToMany<RoleInfo>(db, p);
                }
                catch
                {
                    return rbool = false;
                }
                finally { db.Connection.Close(); }

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
        public static bool DeleteToMany(Expression<Func<RoleInfo, bool>> fun)
        {
            bool rbool = true;
            try
            {

                LinQBaseDao.DeleteToMany<RoleInfo>(new DCCarManagementDataContext(), fun);

            }
            catch
            {
                rbool = false;
            }
            return rbool;

        }
        /// <summary>
        /// 更新
        /// </summary>
        public static bool UpdateOneRoleInfo(Expression<Func<RoleInfo, bool>> fun, Action<RoleInfo> ap)
        {
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    LinQBaseDao.Update<RoleInfo>(db, fun, ap);
                }
                catch
                {
                    return rbool = false;
                }
                finally
                {
                    db.Connection.Close();
                }
            }
            return rbool;

        }

    }
}
