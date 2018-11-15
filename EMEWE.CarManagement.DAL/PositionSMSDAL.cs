using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Data;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
    public class PositionSMSDAL
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql">查询sql语句</param>
        /// <returns>dataset</returns>
        public static DataSet GetCarType(string sql)
        {
            DataSet dataset = new DataSet();
            dataset = LinQBaseDao.Query(sql);
            return dataset;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="pLED">实体</param>
        /// <returns>返回执行结果 true or false</returns>
        public static bool InsertPositionSMS(PositionSMS pSMS)
        {
            bool rbool = false;
            using (DCCarManagementDataContext dc = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne<PositionSMS>(dc, pSMS);
                }
                catch
                {
                    rbool = false;
                }
                finally { dc.Connection.Close(); }
            }
            return rbool;
        }
        /// <summary>
        /// Linq的更新方法
        /// </summary>
        /// <param name="fun">条件</param>
        /// <param name="action">修改的参数</param>
        public static bool UpdatePositionSMS(Expression<Func<PositionSMS, bool>> fun, Action<PositionSMS> action)
        {
            bool rbool = false;
            using (DCCarManagementDataContext dc = new DCCarManagementDataContext())
            {
                try
                {
                    if (LinQBaseDao.Update<PositionSMS>(dc, fun, action))
                    {
                        rbool = true;
                    }
                    else
                    {
                        rbool = false;
                    }
                }

                finally { dc.Connection.Close(); }

            }
            return rbool;
        }
        /// <summary>
        /// Linq根据条件删除数据
        /// </summary>
        /// <param name="fun"></param>
        public static bool DeletePositionSMS(Expression<Func<PositionSMS, bool>> fun)
        {
            bool rbool = false;
            using (DCCarManagementDataContext dc = new DCCarManagementDataContext())
            {
                try
                {
                    if (LinQBaseDao.DeleteToMany<PositionSMS>(dc, fun))
                    {
                        rbool = true;
                    }
                    else
                    {
                        rbool = false;
                    }
                }

                finally { dc.Connection.Close(); }

            }
            return rbool;

        }
        /// <summary>
        /// 根据传入的sql进行验证
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ChkPositionSMSState(string sql)
        {
            DataSet dataset = new DataSet();
            dataset = LinQBaseDao.Query(sql);
            if (dataset.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 获取LED设置
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static PositionSMS GetSMS(string sql)
        {
            DataSet dataset = LinQBaseDao.Query(sql);
            PositionSMS ps = new PositionSMS();
            foreach (DataRow dr in dataset.Tables[0].Rows)
            {
                ps.PositionSMS_Content = dr["PositionSMS_Content"].ToString();
                ps.PositionSMS_Count = int.Parse(dr["PositionSMS_Count"].ToString());
                ps.PositionSMS_ID = int.Parse(dr["PositionSMS_ID"].ToString());
                ps.PositionSMS_Operate = dr["PositionSMS_Operate"].ToString();
                ps.PositionSMS_Position_ID = int.Parse(dr["PositionSMS_Position_ID"].ToString());
                ps.PositionSMS_Remark = dr["PositionSMS_Remark"].ToString();
                ps.PositionSMS_State = dr["PositionSMS_State"].ToString();
                ps.PositionSMS_Time = Convert.ToDateTime(dr["PositionSMS_Time"].ToString());
                //ps.PositionSMS_Type = int.Parse(dr["PositionSMS_Type"].ToString());
            }
            return ps;
        }
    }
}
