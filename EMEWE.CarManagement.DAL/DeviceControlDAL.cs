using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Data;
using EMEWE.CarManagement.DAL;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL 
{
    public class DeviceControlDAL
    {

        /// <summary>
        /// 获取车辆类型
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<DeviceControl> GetDeviceControl(string sql)
        {
            DataSet dataset = LinQBaseDao.Query(sql);
            List<DeviceControl> list = new List<DeviceControl>();
            if (dataset != null)
            {
                if (dataset.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataset.Tables[0].Rows)
                    {
                        DeviceControl dcl = new DeviceControl();
                        dcl.DeviceControl_CardNo = dr["DeviceControl_CardNo"].ToString();
                        dcl.DeviceControl_CardType = dr["DeviceControl_CardType"].ToString();
                        dcl.DeviceControl_DrivewayAddress = dr["DeviceControl_DrivewayAddress"].ToString();
                        dcl.DeviceControl_DrivewayValue = dr["DeviceControl_DrivewayValue"].ToString();
                        dcl.DeviceControl_FanSate = dr["DeviceControl_FanSate"].ToString();
                        dcl.DeviceControl_FanValue = dr["DeviceControl_FanValue"].ToString();
                        dcl.DeviceControl_ID = int.Parse(dr["DeviceControl_ID"].ToString());
                        dcl.DeviceControl_ISCardRelease = Boolean.Parse(dr["DeviceControl_ISCardRelease"].ToString());
                        dcl.DeviceControl_OutSate = dr["DeviceControl_OutSate"].ToString();
                        dcl.DeviceControl_PositionValue = dr["DeviceControl_PositionValue"].ToString();
                        dcl.DeviceControl_ReadValue = dr["DeviceControl_ReadValue"].ToString();
                        dcl.DeviceControl_Remark = dr["DeviceControl_Remark"].ToString();
                        list.Add(dcl);
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 根据传入的sql进行查询，返回查询结果
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet GetQuery(string sql)
        {
            return LinQBaseDao.Query(sql);
        }
        /// <summary>
        /// Linq的更新方法
        /// </summary>
        /// <param name="fun">条件</param>
        /// <param name="action">修改的参数</param>
        public static void UpdateDevice(Expression<Func<DeviceControl, bool>> fun, Action<DeviceControl> action)
        {
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    LinQBaseDao.Update<DeviceControl>(db, fun, action);
                }
                finally {
                    db.Connection.Close();
                }
               
            }

        }
        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneDevice(DeviceControl dcl)
        {
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne(db, dcl);
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
        public static bool Update(Expression<Func<DeviceControl, bool>> fun, Action<DeviceControl> action)
        {
            bool rbool = true;
            try
            {
                LinQBaseDao.Update<DeviceControl>(new DCCarManagementDataContext(), fun, action);
            }
            catch
            {
                rbool = false;
            }
            return rbool;
        }
    }
}
