using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.DAL;
using System.Data;
using System.Linq.Expressions;


namespace EMEWE.CarManagementDAL
{
   public  class View_FVN_Driveway_PositionDAL
    {
        /// <summary>
        /// 查询单条 返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
       public static View_FVN_Driveway_Position Single(Expression<Func<View_FVN_Driveway_Position, bool>> fun)
        {

            return LinQBaseDao.Single<View_FVN_Driveway_Position>(new DCCarManagementDataContext(), fun);
       }

       /// <summary>
       /// 是否是指定通道的拍照地感
       /// </summary>
       /// <param name="fun">指定通道输入序号、地感序号、门岗序号</param>
       /// <returns>是拍照地感返回true</returns>
       public static bool ISPhotographFVN(Expression<Func<View_FVN_Driveway_Position, bool>> fun, out int idrivewayId)
       {
           bool rbool = false;
           idrivewayId = 0;
           try
           {
               View_FVN_Driveway_Position v = Single(fun);
             
               if (v != null)
               {
                   idrivewayId = v.Driveway_ID.Value;
                   rbool = true;
               }
           }
           catch 
           {

           }
           return rbool;
       }

       /// <summary>
       /// summary>
       /// 按条件查询信息View_Camera_Driveway_Position
       /// </summary>
       /// <returns></returns>
       public static List<View_FVN_Driveway_Position> GetList(Expression<Func<View_FVN_Driveway_Position, bool>> fun)
       {

           return LinQBaseDao.Query<View_FVN_Driveway_Position>(new DCCarManagementDataContext(), fun).ToList();

       }

       public static List<View_FVN_Driveway_Position> GetSQLList(string sql)
       {
         
           //DataSet dataset = LinQBaseDao.Query(sql);
           //List<View_FVN_Driveway_Position> list = new List<View_FVN_Driveway_Position>();
           //if (dataset != null)
           //{
           //    if (dataset.Tables.Count > 0)
           //    {
           //        foreach (DataRow dr in dataset.Tables[0].Rows)
           //        {
           //            View_FVN_Driveway_Position dcl = new View_FVN_Driveway_Position();
           //            dcl.Driveway_Add = dr["Driveway_Add"].ToString();
           //            dcl.Driveway_Address = dr["Driveway_Address"].ToString();
           //            dcl.Driveway_CreatTime =Common.Converter.ToDateTime(dr["Driveway_CreatTime"].ToString());
           //            dcl.Driveway_ID = Common.Converter.ToInt(dr["Driveway_ID"].ToString());
           //            dcl.Driveway_Name = dr["Driveway_Name"].ToString();
           //            dcl.Driveway_Position_ID = Common.Converter.ToInt( dr["Driveway_Position_ID"].ToString());
           //            dcl.Driveway_ReadCardPort = dr["Driveway_ReadCardPort"].ToString();
           //            dcl.Driveway_Remark =dr["Driveway_Remark"].ToString();
           //            dcl.Driveway_Remark_Driveway_ID = Common.Converter.ToInt( dr["Driveway_Remark_Driveway_ID"].ToString());
           //            dcl.Driveway_State = dr["Driveway_State"].ToString();
           //            dcl.Driveway_Type = dr["Driveway_Type"].ToString();
           //            dcl.Driveway_UserId =  Common.Converter.ToInt(dr["Driveway_UserId"].ToString());
           //            dcl.Driveway_Value = dr["Driveway_Value"].ToString();
           //            dcl.Driveway_WarrantyState = dr["Driveway_WarrantyState"].ToString();
           //            dcl.FVN_Driveway_ID =  Common.Converter.ToInt(dr["FVN_Driveway_ID"].ToString());
           //            dcl.FVN_ID =  Common.Converter.ToInt(dr["FVN_ID"].ToString());
           //            dcl.FVN_Name = dr["FVN_Name"].ToString();
           //            dcl.FVN_Remark = dr["FVN_Remark"].ToString();
           //            dcl.FVN_State = dr["FVN_State"].ToString();
           //            dcl.FVN_Type = dr["FVN_Type"].ToString();
           //            dcl.FVN_Value =  Common.Converter.ToInt(dr["FVN_Value"].ToString());
           //            dcl.Position_Add = dr["Position_Add"].ToString();
           //            dcl.Position_CameraValue = dr["Position_CameraValue"].ToString();
           //            dcl.Position_CreatTime =  Common.Converter.ToDateTime(dr["Position_CreatTime"].ToString());
           //            dcl.Position_ID =  Common.Converter.ToInt(dr["Position_ID"].ToString());
           //            dcl.Position_Name = dr["Position_Name"].ToString();
           //            dcl.Position_Phone = dr["Position_Phone"].ToString();
           //            dcl.Position_Remark = dr["Position_Remark"].ToString();

           //            dcl.Position_State = dr["Position_State"].ToString();
           //            dcl.Position_UserId = Common.Converter.ToInt( dr["Position_UserId"].ToString());
           //            dcl.Position_Value = dr["Position_Value"].ToString();
           //        }
           //    }
           //}
           //return list;
           IEnumerable<View_FVN_Driveway_Position> list = LinQBaseDao.GetItemsForListing<View_FVN_Driveway_Position>(sql);
           return list.ToList();

       }
       /// <summary>
       /// summary>
       /// 按条件查询信息View_FVN_Driveway_Position
       /// </summary>
       /// <returns></returns>
       public static IEnumerable<View_FVN_Driveway_Position> Query(Expression<Func<View_FVN_Driveway_Position, bool>> fun)
       {

           return LinQBaseDao.Query<View_FVN_Driveway_Position>(new DCCarManagementDataContext(), fun);

       }

    }
}
