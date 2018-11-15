using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.DAL
{
    public class QueueDAL
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
        /// 根据传入的sql进行查询，返回查询结果
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet GetSort(string sql)
        {
            return LinQBaseDao.Query(sql);
        }
        /// <summary>
        /// Linq的更新方法
        /// </summary>
        /// <param name="fun">条件</param>
        /// <param name="action">修改的参数</param>
        public static void UpdateSortNumberInfo(Expression<Func<SortNumberInfo, bool>> fun, Action<SortNumberInfo> action)
        {
            using (DCCarManagementDataContext dc = new DCCarManagementDataContext())
            {
                try
                {
                    LinQBaseDao.Update<SortNumberInfo>(dc, fun, action);
                }

                finally { dc.Connection.Close(); }

            }

        }
    }
}
