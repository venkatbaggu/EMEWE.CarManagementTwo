using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using System.Data;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
    public class PositionLEDDAL
    {
        /// <summary>
        /// 查询可以显示的字段
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="viewName">视图名称</param>
        /// <returns></returns>
        public static  List<string> GetLEDShow(string procName,string viewName)
        {
            List<string> list = new List<string>();
            list= LinQBaseDao.GetView(procName, viewName);
            return list;
        }
        /// <summary>
        /// 查询LED显示信息
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
        /// 获取LED设置
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static PositionLED GetLED(string sql)
        {
            DataSet dataset = LinQBaseDao.Query(sql);
            PositionLED pLED = new PositionLED();
            foreach (DataRow dr in dataset.Tables[0].Rows)
            {
                pLED.PositionLED_Position_ID = int.Parse(dr["PositionLED_Position_Id"].ToString());
                pLED.PositionLED_Type = int.Parse(dr["PositionLED_Type"].ToString());
                pLED.PositionLED_ID = int.Parse(dr["PositionLED_ID"].ToString());
                pLED.PositionLED_ScreenHeight = int.Parse(dr["PositionLED_ScreenHeight"].ToString());
                pLED.PositionLED_ScreenWeight = int.Parse(dr["PositionLED_ScreenWeight"].ToString());
                pLED.PositionLED_X = int.Parse(dr["PositionLED_X"].ToString());
                pLED.PositionLED_Y = int.Parse(dr["PositionLED_Y"].ToString());
                //pLED.PositionLED_IntervalX = int.Parse(dr["PositionLED_IntervalX"].ToString());
                //pLED.PositionLED_IntervalY = int.Parse(dr["PositionLED_IntervalY"].ToString());
                pLED.PositionLED_Count = int.Parse(dr["PositionLED_Count"].ToString());
                pLED.PositionLED_Remark = dr["PositionLED_Remark"].ToString();
                pLED.PositionLED_Content = dr["PositionLED_Content"].ToString();
                //pLED.PositionLED_PassageState = dr["PositionLED_PassageState"].ToString();
                pLED.PositionLED_State = dr["PositionLED_State"].ToString();
                pLED.PositionLED_Font = dr["PositionLED_Font"].ToString();
                pLED.PositionLED_FontSize = dr["PositionLED_FontSize"].ToString();
                pLED.PositionLED_Color = dr["PositionLED_Color"].ToString();
                pLED.PositionLED_Operate = dr["PositionLED_Operate"].ToString();
                pLED.PositionLED_Time = Convert.ToDateTime(dr["PositionLED_Time"].ToString());
            }
            return pLED;
        }
        /// <summary>
        /// 添加一条LED信息
        /// </summary>
        /// <param name="pLED">LED实体</param>
        /// <returns>返回执行结果 true or false</returns>
        public static bool InsertPositionLED(PositionLED pLED)
        {
            bool rbool = true;
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne(db, pLED);
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
        /// Linq的更新方法
        /// </summary>
        /// <param name="fun">条件</param>
        /// <param name="action">修改的参数</param>
        public static void UpdatePositionLED(Expression<Func<PositionLED,bool>> fun,Action<PositionLED> action)
        {
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    LinQBaseDao.Update<PositionLED>(db, fun, action);
                }
                
                finally { db.Connection.Close(); }

            }
            
        }
        /// <summary>
        /// Linq根据条件删除数据
        /// </summary>
        /// <param name="fun"></param>
        public static void DeletePositionLED(Expression<Func<PositionLED,bool>> fun)
        {
            using (DCCarManagementDataContext db = new DCCarManagementDataContext())
            {
                try
                {
                    LinQBaseDao.DeleteToMany<PositionLED>(db, fun);
                }

                finally { db.Connection.Close(); }

            }
            
        }
        /// <summary>
        /// 根据传入的sql进行验证
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ChkPositionLEDState(string sql)
        {
            DataSet dataset = new DataSet();
            dataset= LinQBaseDao.Query(sql);
            if (dataset.Tables[0].Rows.Count>0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int LEDcount(string sql)
        {
            DataSet dscount = new DataSet();
            dscount = LinQBaseDao.Query(sql);
            return Convert.ToInt32(dscount.Tables[0].Rows[0][0]);
        }
    }
}
