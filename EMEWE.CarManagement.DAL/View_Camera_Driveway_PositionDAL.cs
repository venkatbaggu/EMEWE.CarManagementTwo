using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.DAL;
using System.Linq.Expressions;

namespace EMEWE.CarManagementDAL
{
    public class View_Camera_Driveway_PositionDAL
    {
        /// <summary>
        /// summary>
        /// 按条件查询信息View_Camera_Driveway_Position
        /// </summary>
        /// <returns></returns>
        public static List<View_Camera_Driveway_Position> GetList(Expression<Func<View_Camera_Driveway_Position, bool>> fun)
        {

            return LinQBaseDao.Query<View_Camera_Driveway_Position>(new DCCarManagementDataContext(), fun).ToList();

        }
        /// <summary>
        /// summary>
        /// 按条件查询信息View_FVN_Driveway_Position
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<View_Camera_Driveway_Position> Query(Expression<Func<View_Camera_Driveway_Position, bool>> fun)
        {

            return LinQBaseDao.Query<View_Camera_Driveway_Position>(new DCCarManagementDataContext(), fun);

        }
    }
}
