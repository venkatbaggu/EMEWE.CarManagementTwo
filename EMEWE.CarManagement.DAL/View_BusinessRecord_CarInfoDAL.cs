using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;

namespace EMEWE.CarManagement.DAL
{
    public class View_BusinessRecord_CarInfoDAL
    {

        /// <summary>
        /// 新增一条记录
        /// </summary>
        /// <param name="qcRecord">质检实体</param>
        /// <returns></returns>
        public static bool InsertOneQCRecord(View_BusinessRecord_CarInfo qcRecord)
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
        /// summary>
        /// 按条件查询信息View_Camera_Driveway_Position
        /// </summary>
        /// <returns></returns>
        public static List<View_Camera_Driveway_Position> GetList(Func<View_Camera_Driveway_Position, bool> fun)
        {

            return LinQBaseDao.Query<View_Camera_Driveway_Position>(new DCCarManagementDataContext(), fun).ToList();

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息View_FVN_Driveway_Position
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<View_Camera_Driveway_Position> Query(Func<View_Camera_Driveway_Position, bool> fun)
        {

            return LinQBaseDao.Query<View_Camera_Driveway_Position>(new DCCarManagementDataContext(), fun);

        }
    }
}
