using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagementDAL;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.Commons.CommonClass
{
   public class DeviceControlBLL
    {
 
       #region 控制数据


       //List<DeviceControl> dclist = new List<DeviceControl>();

       //bool ISBusyTimerDeviceControl = false;
       //private void timerDeviceControl_Tick(object sender, EventArgs e)
       //{
       //    if (!ISBusyTimerDeviceControl)
       //    {
       //        ISBusyTimerDeviceControl = true;
       //        GetDeviceControl();
       //        ISBusyTimerDeviceControl = false;

       //    }
       //}

       /// <summary>
       /// 根据当前门岗获取设备控制数据
       /// </summary>
       public static List<DeviceControl> GetDeviceControl()
       {
           List<DeviceControl> listDC = new List<DeviceControl>();
           try
           {
               string strSql = "select * from DeviceControl where DeviceControl_PositionValue='" + SystemClass.PosistionValue + "'";
               listDC = DeviceControlDAL.GetDeviceControl(strSql);
              
           }
           catch
           {
               EMEWE.CarManagement.Commons.CommonClass.CommonalityEntity.WriteTextLog("记录设备控制表获取数据异常：" );
           }
           return listDC;

       }


       /// <summary>
       /// 查找是否已存在设备控制实体
       /// </summary>
       /// <param name="strDeviceControl_DrivewayAddress">通道物理值</param>
       /// <param name="strDeviceControl_PositionValue">门岗值</param>
       /// <param name="strDeviceControl_FanValue">地感值</param>
       /// <param name="strDeviceControl_ReadValue">读卡器地址码</param>
       /// <param name="strDeviceControl_DrivewayValue">通道值</param>
       /// <param name="ISFind">False 未找到，true找到数据</param>
       /// 说明：查询1根据“物理值和门岗值、地感值”，查询2根据“门岗值和读卡器地址码”，查询3根据“物理值和门岗值、通道值”
       /// <returns>DeviceControl 找到是有数据的实体，如果未找到则返回空实体</returns>
       public static DeviceControl FindDeviceControl(  List<DeviceControl> listDC ,string strDeviceControl_DrivewayAddress, string strDeviceControl_PositionValue, string strDeviceControl_FanValue, string strDeviceControl_ReadValue, string strDeviceControl_DrivewayValue, out bool ISFind)
       {
           DeviceControl rentity = new DeviceControl();
           ISFind = false;//是否找到相同的数据 True是，false否
           List<DeviceControl> list = listDC;
           foreach (DeviceControl entity in listDC)
           {
               short shDrivewayAddress = Common.Converter.ToShort(entity.DeviceControl_DrivewayAddress, 0);
               if (strDeviceControl_DrivewayAddress == entity.DeviceControl_DrivewayAddress && strDeviceControl_PositionValue == entity.DeviceControl_PositionValue && entity.DeviceControl_FanValue == strDeviceControl_FanValue)//物理值和门岗值、地感值
               {
                   rentity = entity;
                   ISFind = true;
                   // DeviceControlDAL.UpdateDevice()
                   break;
               }
               else if (strDeviceControl_ReadValue == entity.DeviceControl_ReadValue && strDeviceControl_PositionValue == entity.DeviceControl_PositionValue)//门岗值和读卡器地址码相同。
               {
                   rentity = entity;
                   ISFind = true;
                   break;
               }
               else if (strDeviceControl_DrivewayValue == entity.DeviceControl_DrivewayValue && strDeviceControl_PositionValue == entity.DeviceControl_PositionValue && strDeviceControl_DrivewayAddress == entity.DeviceControl_DrivewayAddress)//物理值和门岗值、通道值
               {
                   rentity = entity;
                   ISFind = true;
                   break;
               }
               else
               {
                   ISFind = false;
               }
           }

           return rentity;
       }

       /// <summary>
       /// 新增设备控制数据
       /// 说明：每次获取数据前先检查是否有新增的数据并进行新增
       /// </summary>
       public static void AddDeviceControl(List<DeviceControl> dclist)
       {
           try
           {
             if(dclist.Count<=0){
              dclist=  GetDeviceControl();
             }
           
               string strSql = "select * from View_FVN_Driveway_Position where Position_Value='" + SystemClass.PosistionValue + "'";
               List<View_FVN_Driveway_Position> vfdpList = View_FVN_Driveway_PositionDAL.GetSQLList(strSql);
               if (vfdpList.Count > dclist.Count)
               {
                   bool isFind = false;
                   foreach (View_FVN_Driveway_Position vfdp in vfdpList)
                   {

                       foreach (DeviceControl dc in dclist)
                       {
                           if (vfdp.FVN_Value.ToString() == dc.DeviceControl_FanValue && vfdp.Driveway_Value == dc.DeviceControl_DrivewayValue && vfdp.Driveway_ReadCardPort == dc.DeviceControl_ReadValue && vfdp.Position_Value == dc.DeviceControl_PositionValue)
                           {
                               isFind = true;
                               break;
                           }
                           else
                           {
                               isFind = false;
                           }
                       }
                       if (!isFind)
                       {
                           DeviceControl rentity = new DeviceControl();
                           //rentity.DeviceControl_CardNo = "";
                           rentity.DeviceControl_DrivewayAddress = vfdp.Driveway_Address;
                           rentity.DeviceControl_DrivewayValue = vfdp.Driveway_Value;
                           rentity.DeviceControl_FanValue = vfdp.FVN_Value.ToString();
                           rentity.DeviceControl_FanSate = "0";//无地感
                           rentity.DeviceControl_ISCardRelease = false;//未刷卡放行
                           rentity.DeviceControl_OutSate = "无";//输出无命令
                           rentity.DeviceControl_PositionValue = vfdp.Position_Value;
                           rentity.DeviceControl_ReadValue = vfdp.Driveway_ReadCardPort;
                           DeviceControlDAL.InsertOneDevice(rentity);
                       }
                   }
               }
           }
           catch 
           {
               EMEWE.CarManagement.Commons.CommonClass.CommonalityEntity.WriteTextLog("记录设备控制表新增异常：" );

           }

       }
       /// <summary>
       /// 修改设备控制数据
       /// </summary>
       /// <param name="pc">查询函数</param>
       /// <param name="ap">修改字段函数</param>
       /// 说明：如果在修改时遇到错误，执行一次新增设备控制数据，再次进行修改操作
       public static void UpdateDeviceControl(List<DeviceControl> dclist,Expression<Func<DeviceControl, bool>> pc, Action<DeviceControl> ap)
       {

           try
           {
               if (pc != null && ap != null)
               {

                   DeviceControlDAL.UpdateDevice(pc, ap);
               }
           }
           catch 
           {
               EMEWE.CarManagement.Commons.CommonClass.CommonalityEntity.WriteTextLog("记录设备控制表修改异常：" );
               try
               {
                   AddDeviceControl(dclist);
                   if (pc != null && ap != null)
                   {

                       DeviceControlDAL.UpdateDevice(pc, ap);
                   }
               }
               catch 
               {
                   EMEWE.CarManagement.Commons.CommonClass.CommonalityEntity.WriteTextLog("记录设备控制表新增后再次修改异常：" );
               }
           }

       }
       #endregion
    }
}

  