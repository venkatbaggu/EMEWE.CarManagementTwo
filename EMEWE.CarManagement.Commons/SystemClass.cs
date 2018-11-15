using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EMEWE.CarManagement.Commons
{
   public  class SystemClass
    {

       /// <summary>
       /// 门岗名称
       /// </summary>
       public static string PositionName = "2#门岗";

       /// <summary>
       /// 门岗编号
       /// </summary>
        public static int PositionID=2;
       
       /// <summary>
        /// 门岗值
       /// </summary>
        public static string PosistionValue = "04";
        /// <summary>
        /// 硬盘录像机IP地址
        /// </summary>
        public static string DVRIPAddress = "10.185.234.184";
        /// <summary>
        /// 硬盘录像机服务端口号
        /// </summary>
        public static Int16 DVRPortNumber = 8000;
        /// <summary>
        /// 硬盘录像机用户名
        /// </summary>
        public static string DVRUserName="admin" ;
       /// <summary>
       /// 硬盘录像机密码
       /// </summary>
        public static string DVRPassword = "12345";

       /// <summary>
        /// 获取和设置包含该应用程序的目录的名称。
        /// result: X:\xxx\xxx\ (.exe文件所在的目录+"\")
       /// </summary>
       public static  string BaseFile = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

       /// <summary>
       /// 获取图片服务器地址
       /// </summary>
       public static string SaveFiel = "";
       /// <summary>
       /// PLC串口号
       /// </summary>
       public static int PLCCom = 1;
       /// <summary>
       /// 读卡器1串口号
       /// </summary>
       public static int CardReadComOne = 3;
       /// <summary>
       /// 读卡器2串口号
       /// </summary>
       public static int CardReadComTwo = 5;

       /// <summary>
       /// 加密锁识别码
       /// </summary>
       public static string NTCode = "emewe00201";

       /// <summary>
       /// 手动控制密码
       /// </summary>
       public static string ControlPWD = "jiulong";
       /// <summary>
       /// 拍照延迟时间
       /// </summary>
       public static int CaptureJPEGPictureSleep = 100;
    }
}
