using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Data;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
    public class VoiceCallsDAL
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
        public static VoiceCalls GetVoice(string sql)
        {
            DataSet dataset = LinQBaseDao.Query(sql);
            VoiceCalls vc = new VoiceCalls();
            foreach (DataRow dr in dataset.Tables[0].Rows)
            {
                vc.VoiceCalls_ID = int.Parse(dr["VoiceCalls_ID"].ToString());
                vc.VoiceCalls_Number = int.Parse(dr["VoiceCalls_Number"].ToString());
                vc.VoiceCalls_Content = dr["VoiceCalls_Content"].ToString();
                vc.VoiceCalls_PositionName = dr["VoiceCalls_PositionName"].ToString();
                vc.VoiceCalls_PositionValue = dr["VoiceCalls_PositionValue"].ToString();
                vc.VoiceCalls_Remark = dr["VoiceCalls_Remark"].ToString();
            }
            return vc;
        }
        /// <summary>
        /// 添加一条数据
        /// </summary>
        /// <param name="pLED">LED实体</param>
        /// <returns>返回执行结果 true or false</returns>
        public static bool InsertVoiceCalls(VoiceCalls vc)
        {
            bool rbool = false;
            using (DCCarManagementDataContext dc = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne<VoiceCalls>(dc, vc);
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
        public static void UpdateVoiceCalls(Expression<Func<VoiceCalls, bool>> fun, Action<VoiceCalls> action)
        {
            using (DCCarManagementDataContext dc = new DCCarManagementDataContext())
            {
                try
                {
                    LinQBaseDao.Update<VoiceCalls>(dc, fun, action);
                }

                finally { dc.Connection.Close(); }

            }
            
        }
        /// <summary>
        /// Linq根据条件删除数据
        /// </summary>
        /// <param name="fun"></param>
        public static void DeleteVoiceCalls(Expression<Func<VoiceCalls, bool>> fun)
        {
            using (DCCarManagementDataContext dc = new DCCarManagementDataContext())
            {
                try
                {
                    LinQBaseDao.DeleteToMany<VoiceCalls>(dc, fun);
                }

                finally { dc.Connection.Close(); }

            }
           
        }
        /// <summary>
        /// 根据传入的sql进行验证
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ChkVoiceCalls(string sql)
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
