using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
    public  class PrintInfoDAL
    {
        /// <summary>
        /// 获取车辆类型
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static List<CarType> GetCarType(string sql)
        {
            DataSet dataset = LinQBaseDao.Query(sql);
            List<CarType> list = new List<CarType>();
            foreach (DataRow dr in dataset.Tables[0].Rows)
            {
                CarType cartype = new CarType();
                cartype.CarType_ID = int.Parse(dr["CarType_ID"].ToString());
                cartype.CarType_Name = dr["CarType_Name"].ToString();
                list.Add(cartype);
            }
            return list;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="pLED">实体</param>
        /// <returns>返回执行结果 true or false</returns>
        public static bool InsertPrint(PrintInfo pLED)
        {
            bool rbool = false;
            using (DCCarManagementDataContext dc = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne<PrintInfo>(dc, pLED);
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
        /// 添加
        /// </summary>
        /// <param name="pr">实体</param>
        /// <returns>返回执行结果 true or false</returns>
        public static bool InsertPrint(PrintRecord pr)
        {
            bool rbool = false;
            using (DCCarManagementDataContext dc = new DCCarManagementDataContext())
            {
                try
                {
                    rbool = LinQBaseDao.InsertOne<PrintRecord>(dc, pr);
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
        public static void UpdatePrint(Expression<Func<PrintInfo, bool>> fun, Action<PrintInfo> action)
        {
            using (DCCarManagementDataContext dc = new DCCarManagementDataContext())
            {
                try
                {
                    LinQBaseDao.Update<PrintInfo>(dc, fun, action);
                }

                finally { dc.Connection.Close(); }

            }
            
        }
        /// <summary>
        /// Linq根据条件删除数据
        /// </summary>
        /// <param name="fun"></param>
        public static void DeletePrintInfo(Expression<Func<PrintInfo, bool>> fun)
        {
            using (DCCarManagementDataContext dc = new DCCarManagementDataContext())
            {
                try
                {
                    LinQBaseDao.DeleteToMany<PrintInfo>(dc, fun);
                }

                finally { dc.Connection.Close(); }

            }
            
        }
        /// <summary>
        /// 获取设置
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static PrintInfo GetPrint(string sql)
        {
            DataSet dataset = LinQBaseDao.Query(sql);
            PrintInfo pInfo = new PrintInfo();
            foreach (DataRow dr in dataset.Tables[0].Rows)
            {
                pInfo.Print_CarType_ID = int.Parse(dr["Print_CarType_ID"].ToString());
                pInfo.Print_Content = dr["Print_Content"].ToString();
                pInfo.Print_ID=int.Parse(dr["Print_ID"].ToString());
                pInfo.Print_State = dr["Print_State"].ToString();
            }
            return pInfo;
        }

        /// <summary>
        /// 根据传入的sql进行验证
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static bool ChkPrintState(string sql)
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
