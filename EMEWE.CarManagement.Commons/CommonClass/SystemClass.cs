using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using EMEWE.CarManagement.DAL;
using System.Data;
using EMEWE.CarManagement.Entity;
using System.Configuration;
using WindowsFormsApplication3;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.Commons.CommonClass
{
    public class SystemClass
    {
        /// <summary>
        /// 门岗数量限制
        /// </summary>
        public static int postionCount = 5;
        /// <summary>
        /// 登记IC卡读卡器通道值
        /// </summary>
        public static string ReadValue = "1";

        /// <summary>
        /// 门岗名称
        /// </summary>
        public static string PositionName = "1#门岗";

        /// <summary>
        /// 门岗编号
        /// </summary>
        public static int PositionID = 2;

        /// <summary>
        /// 呼叫间隔
        /// </summary>
        public static int HuJiaoJianGe = 1;
        /// <summary>
        /// 呼叫次数
        /// </summary>
        public static int HuJiaoCount = 1;

        /// <summary>
        /// 门岗值
        /// </summary>
        public static string PosistionValue = "01";
        public static int PositionLED_ID = 0;
        /// <summary>
        /// 通道值
        /// </summary>
        public static string DrivewayValue = "01";
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
        public static string DVRUserName = "admin";
        /// <summary>
        /// 硬盘录像机密码
        /// </summary>
        public static string DVRPassword = "12345";

        /// <summary>
        /// 硬盘录像机IP地址
        /// </summary>
        public static string DVRIPAddressTwo = "10.185.234.184";

        /// <summary>
        /// 硬盘录像机服务端口号
        /// </summary>
        public static Int16 DVRPortNumberTwo = 8000;
        /// <summary>
        /// 硬盘录像机用户名
        /// </summary>
        public static string DVRUserNameTwo = "admin";
        /// <summary>
        /// 硬盘录像机密码
        /// </summary>
        public static string DVRPasswordTwo = "12345";
        /// <summary>
        /// 获取和设置包含该应用程序的目录的名称。
        /// result: X:\xxx\xxx\ (.exe文件所在的目录+"\CarPhoto")
        /// </summary>
        public static string BaseFile = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + ConfigurationManager.ConnectionStrings["CarFolder"].ToString() + "\\";

        /// <summary>
        /// 获取图片服务器地址
        /// </summary>
        public static string SaveFile = "";
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
        public static string NTCode = "emewesoft";

        /// <summary>
        /// 手动控制密码
        /// </summary>
        public static string ControlPWD = "jiulong";
        /// <summary>
        /// 拍照延迟时间
        /// </summary>
        public static int CaptureJPEGPictureSleep = 100;
        /// <summary>
        /// 登记拍照地址
        /// </summary>
        public static int lChannel = 1;
        /// <summary>
        /// 是否显示LED
        /// </summary>
        public static string ISLED = "是";

        public static string Skin = "默认样式.ssk";

        public static bool ISChkSortNumber = false;
        public static bool ISVoicCall = false;
        public static bool ISPromptout = false;

        /// <summary>
        /// 获得系统设置信息: 初始化设置   SystemSet.xml
        /// </summary>
        public static string GetSystemSet(string filepath)
        {
            string rstr = "";
            //string filepath = System.IO.Directory.GetCurrentDirectory() + "\\SystemSet.xml";


            try
            {
                string DVRIP = "";
                string DVRServerPort = "";
                string DVRLoginName = "";
                string DVRPwd = "";

                string DVRIPTwo = "";
                string DVRServerPortTwo = "";
                string DVRLoginNameTwo = "";
                string DVRPwdTwo = "";

                string SaveFiel = "";
                string CARD = "";
                string CARD2 = "";
                string PCL = "";
                string PositionName = "";
                string PosistionValue = "";
                int HuJiaoJianGe = 0;
                int lChanneld = 1;
                string controlpwd = "";
                string skin = "";
                string pcount = "";
                string ntcode = "";

                string System = "";
                string MessageServer = "";
                string GroupName = "";
                string Client = "";
                string Language = "";
                string SAPUser = "";
                string SAPPassword = "";
                string isled = "";

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filepath);
                XmlNode xn = xmlDoc.SelectSingleNode("param");//查找<bookstore>
                XmlNodeList xnl = xn.ChildNodes;
                if (xnl.Count > 0)
                {
                    foreach (XmlNode xnf in xnl)
                    {
                        XmlElement xe = (XmlElement)xnf;
                        DVRIP = xe.GetAttribute("DVRIP").ToString();  // 硬盘录像机的IP
                        DVRServerPort = xe.GetAttribute("DVRServerPort").ToString(); // 硬盘录像机的服务器端口号
                        DVRLoginName = xe.GetAttribute("DVRLoginName").ToString();   // 硬盘录像机的登录名称
                        DVRPwd = xe.GetAttribute("DVRPwd").ToString(); // 硬盘录像机的登录密码

                        DVRIPTwo = xe.GetAttribute("DVRIPTwo").ToString();  // 硬盘录像机的IP
                        DVRServerPortTwo = xe.GetAttribute("DVRServerPortTwo").ToString(); // 硬盘录像机的服务器端口号
                        DVRLoginNameTwo = xe.GetAttribute("DVRLoginNameTwo").ToString();   // 硬盘录像机的登录名称
                        DVRPwdTwo = xe.GetAttribute("DVRPwdTwo").ToString(); // 硬盘录像机的登录密码

                        SaveFiel = xe.GetAttribute("ServerImageLoad").ToString(); // 获取图片服务器地址
                        CARD = xe.GetAttribute("CARD").ToString(); // 读卡器1串口号
                        CARD2 = xe.GetAttribute("CARD2").ToString(); // 读卡器2串口号
                        PCL = xe.GetAttribute("PCL").ToString(); // PCL串口号
                        PositionName = xe.GetAttribute("MenGangName").ToString(); // 门岗名称
                        PosistionValue = xe.GetAttribute("PosistionValue").ToString(); // 门岗值
                        HuJiaoJianGe = int.Parse(xe.GetAttribute("HuJiaoJianGe").ToString());
                        lChanneld = int.Parse(xe.GetAttribute("lChannel").ToString());//登记拍照摄像头地址路数
                        controlpwd = xe.GetAttribute("ControlPWD").ToString();//道闸手动控制密码
                        skin = xe.GetAttribute("Skin").ToString();//系统皮肤
                        pcount = xe.GetAttribute("PCount").ToString();//可添加门岗数量
                        ntcode = xe.GetAttribute("NTCode").ToString();//识别码
                        isled = xe.GetAttribute("ISLED").ToString();//LED显示
                        //SAP信息
                        System = xe.GetAttribute("System").ToString();
                        MessageServer = xe.GetAttribute("MessageServer").ToString();
                        GroupName = xe.GetAttribute("GroupName").ToString();
                        Client = xe.GetAttribute("Client").ToString();
                        Language = xe.GetAttribute("Language").ToString();
                        SAPUser = xe.GetAttribute("User").ToString();
                        SAPPassword = xe.GetAttribute("Password").ToString();


                        SystemClass.ISChkSortNumber = Convert.ToBoolean(xe.GetAttribute("ISChkSortNumber").ToString());
                        SystemClass.ISVoicCall = Convert.ToBoolean(xe.GetAttribute("ISVoicCall").ToString());
                        SystemClass.ISPromptout = Convert.ToBoolean(xe.GetAttribute("ISPromptout").ToString());

                    }
                    SystemClass.DVRIPAddress = DVRIP; // 硬盘录像机IP地址
                    SystemClass.DVRPortNumber = Common.Converter.ToShort(DVRServerPort); // 硬盘录像机服务端口号
                    SystemClass.DVRUserName = DVRLoginName; // 硬盘录像机用户名
                    SystemClass.DVRPassword = DVRPwd; // 硬盘录像机密码

                    SystemClass.DVRIPAddressTwo = DVRIPTwo; // 硬盘录像机IP地址
                    SystemClass.DVRPortNumberTwo = Common.Converter.ToShort(DVRServerPortTwo); // 硬盘录像机服务端口号
                    SystemClass.DVRUserNameTwo = DVRLoginNameTwo; // 硬盘录像机用户名
                    SystemClass.DVRPasswordTwo = DVRPwdTwo; // 硬盘录像机密码

                    SystemClass.SaveFile = SaveFiel;  // 获取图片服务器地址
                    SystemClass.PLCCom = Common.Converter.ToInt(PCL);// PLC串口号
                    SystemClass.CardReadComOne = Common.Converter.ToInt(CARD);// 读卡器1串口号
                    SystemClass.CardReadComTwo = Common.Converter.ToInt(CARD2);// 读卡器2串口号
                    SystemClass.PositionName = PositionName; // 门岗名称
                    SystemClass.PosistionValue = PosistionValue;
                    SystemClass.HuJiaoJianGe = HuJiaoJianGe;
                    SystemClass.lChannel = lChanneld;
                    SystemClass.ControlPWD = controlpwd;
                    SystemClass.Skin = skin;
                    SystemClass.postionCount = Convert.ToInt32(pcount);
                    SystemClass.NTCode = ntcode;
                    SystemClass.ISLED = isled;

                    Class1.strSystem = System;
                    Class1.strMessageServer = MessageServer;
                    Class1.strGroupName = GroupName;
                    Class1.strClient = Client;
                    Class1.strLanguage = Language;
                    Class1.strUser = SAPUser;
                    Class1.strPassword = SAPPassword;
                }



            }
            catch
            {
                rstr = "获得系统设置失败!";
                //MessageBox.Show("获得系统设置失败!", "运行信息", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
            return rstr;
        }

        /// <summary>
        /// 获取门岗信息
        /// </summary>
        public static void GetPosition()
        {
            try
            {
                DataTable dtpost = LinQBaseDao.Query("select Position_ID,Position_Value from Position where Position_Name='" + SystemClass.PositionName + "'").Tables[0];
                SystemClass.PositionLED_ID = Convert.ToInt32(dtpost.Rows[0][0].ToString()); // 门岗编号
                SystemClass.PositionID = Convert.ToInt32(dtpost.Rows[0][0].ToString()); // 门岗编号
                SystemClass.PosistionValue = dtpost.Rows[0][1].ToString();
                DataTable dt = LinQBaseDao.Query("select PositionVoice_Count from PositionVoice where PositionVoice_Position_ID=" + SystemClass.PositionID).Tables[0];

                if (dt.Rows.Count > 0)
                {
                    SystemClass.HuJiaoCount = Convert.ToInt32(dt.Rows[0][0].ToString());//呼叫次数
                }
            }
            catch (Exception)
            {

            }
        }

        /////---预定为30--/////
        /// <summary>
        /// 添加的数据ID
        /// </summary>
        /// <param name="ids">步调,增加为1 减少为-1</param>
        /// <returns></returns>
        public static bool BackListMode(int ids, int ints)
        {
            Expression<Func<Blacklist, bool>> p = n => n.Blacklist_ID == ids;
            Action<Blacklist> ap = s =>
            {
                if (ints == 1)
                {
                    if (s.Blacklist_UpgradeCount == null)
                    {
                        s.Blacklist_UpgradeCount = 1;
                    }
                    else
                    {
                        if (s.Blacklist_UpgradeCount == 30)
                        {
                            s.Blacklist_UpgradeCount = 0;
                            DataTable table = LinQBaseDao.Query("select * from dbo.Dictionary where Dictionary.Dictionary_ID=(select Blacklist_Dictionary_ID from dbo.Blacklist where Blacklist_ID=" + s.Blacklist_Dictionary_ID + ")").Tables[0];
                            s.Blacklist_Dictionary_ID = Convert.ToInt32(LinQBaseDao.Query("select Dictionary_ID from dbo.Dictionary where Menu_OtherID=" + Convert.ToInt32(table.Rows[0]["Menu_OtherID"]) + 1).Tables[0].Rows[1]["Dictionary_ID"]);
                        }
                        else
                            s.Blacklist_UpgradeCount += 1;
                    }
                }
                if (ints == -1)
                {
                    if (s.Blacklist_DowngradeCount == null)
                    {
                        s.Blacklist_DowngradeCount = 1;
                    }
                    else
                        if (s.Blacklist_UpgradeCount == 30)
                        {
                            s.Blacklist_DowngradeCount = 0;
                            DataTable table = LinQBaseDao.Query("select * from dbo.Dictionary where Dictionary.Dictionary_ID=(select Blacklist_Dictionary_ID from dbo.Blacklist where Blacklist_ID=" + s.Blacklist_Dictionary_ID + ")").Tables[0];
                            if (Convert.ToInt32(table.Rows[0]["Menu_OtherID"]) + (-1) == 0)
                            {
                                return;
                            }
                            s.Blacklist_Dictionary_ID = Convert.ToInt32(LinQBaseDao.Query("select Dictionary_ID from dbo.Dictionary where Menu_OtherID=" + Convert.ToInt32(table.Rows[0]["Menu_OtherID"]) + (-1)).Tables[0].Rows[1]["Dictionary_ID"]);
                        }
                        else
                            s.Blacklist_UpgradeCount += 1;
                }
            };
            if (BlacklistDAL.Update(p, ap))
            {
                return true;
            }
            else
                return false;
        }
    }
}