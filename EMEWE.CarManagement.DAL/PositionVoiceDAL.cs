using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Data;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
    public class PositionVoiceDAL
    {
        /// <summary>
        /// 查询可以显示的字段
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="viewName">视图名称</param>
        /// <returns></returns>
        public static List<string> GetShow(string procName, string viewName)
        {
            List<string> list = new List<string>();
            list = LinQBaseDao.GetView(procName, viewName);
            return list;
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sql">查询sql语句</param>
        /// <returns></returns>
        public static DataSet GetCarType(string sql)
        {
            DataSet dataset = new DataSet();
            dataset = LinQBaseDao.Query(sql);
            return dataset;
        }
        /// <summary>
        /// 获取语音设置
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static PositionVoice GetVoice(string sql)
        {
            DataSet dataset = LinQBaseDao.Query(sql);
            PositionVoice pv = new PositionVoice();
            foreach (DataRow dr in dataset.Tables[0].Rows)
            {
                pv.PositionVoice_Content = dr["PositionVoice_Content"].ToString();
                pv.PositionVoice_Count = int.Parse(dr["PositionVoice_Count"].ToString());
                pv.PositionVoice_ID = int.Parse(dr["PositionVoice_ID"].ToString());
                pv.PositionVoice_Operate = dr["PositionVoice_Operate"].ToString();
                pv.PositionVoice_Position_ID = int.Parse(dr["PositionVoice_Position_ID"].ToString());
                pv.PositionVoice_Remark = dr["PositionVoice_Remark"].ToString();
                pv.PositionVoice_State = dr["PositionVoice_State"].ToString();
                pv.PositionVoice_Time = Convert.ToDateTime(dr["PositionVoice_Time"].ToString());
                //pv.PositionVoice_Type = int.Parse(dr["PositionVoice_Type"].ToString());
            }
            return pv;
        }
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="pLED">LED实体</param>
        /// <returns>返回执行结果 true or false</returns>
        public static bool InsertPositionVoice(PositionVoice pv)
        {
            bool rbool = false;
            using (DCCarManagementDataContext dc = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne<PositionVoice>(dc, pv);
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
        public static void UpdatePositionVoice(Expression<Func<PositionVoice, bool>> fun, Action<PositionVoice> action)
        {
            using (DCCarManagementDataContext dc = new DCCarManagementDataContext())
            {
                try
                {
                    LinQBaseDao.Update<PositionVoice>(dc, fun, action);
                }

                finally { dc.Connection.Close(); }

            }

        }
        /// <summary>
        /// Linq根据条件删除数据
        /// </summary>
        /// <param name="fun"></param>
        public static void DeletePositionVoice(Expression<Func<PositionVoice, bool>> fun)
        {
            using (DCCarManagementDataContext dc = new DCCarManagementDataContext())
            {
                try
                {
                    LinQBaseDao.DeleteToMany<PositionVoice>(dc, fun);
                }

                finally { dc.Connection.Close(); }

            }
            
        }
        /// <summary>
        /// 根据传入的sql进行验证
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ChkPositionVoiceState(string sql)
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
    }
}
