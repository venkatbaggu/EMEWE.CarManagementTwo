using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement;
using System.Linq.Expressions;
using System.Web;
using EMEWE.CarManagement.CommonClass;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagementDAL;
using EMEWE.CarManagement.Commons.CommonClass;

namespace EMEWE.CarManagement.HelpClass
{

    public static class common
    {
        /// <summary>
        /// 是否刷卡放行
        /// true刷卡放行，False未刷卡放行
        /// </summary>
        public static bool ISCardRelease = false;
        /// <summary>
        ///进入车辆信息界面
        ///true进入；false不进入或者关闭CarInfoForm
        /// </summary>
        public static bool INCarInfoForm = false;
        /// <summary>
        /// 当前输入的手动控制密码
        /// </summary>
        public static string CurrentControlPWD = "jiulong";
        /// <summary>
        /// 数据库连接状态
        /// </summary>
        public static bool ISService = false;

        /// <summary>
        /// 数据库所在IP地址
        /// </summary>
        public static string SQLIP = "";

        #region 用户信息
        /// <summary>
        /// 用户角色ID
        /// </summary>

        public static int ROLE = 0;
        /// <summary>
        /// 登录名
        /// </summary>
        public static string USERNAME = "admin";
        /// <summary>
        /// 真实姓名
        /// </summary>
        public static string NAME = "管理员";

        /// <summary>
        /// 用户编号
        /// </summary>
        public static string USERID = "2";

        #endregion

        #region  IC卡和车牌信息
        /// <summary>
        /// 进出厂IC识别，在mainfrom界面跳转页面时赋值
        /// true:进厂
        /// </summary>
        public static bool ICBool;//进出厂IC识别
        /// <summary>
        /// IC卡编号
        /// </summary>
        public static string NumberID;//IC卡编号
        /// <summary>
        /// 车牌号
        /// </summary>
        public static string CarNumber;//车牌号
        /// <summary>
        /// 当前通道Value值
        /// IC卡 在验证时赋值，小票号在数据库中读取对应的通道与当前通道对比
        /// </summary>
        public static string CurrentChannelNumber = "";
        /// <summary>
        /// 卡号使用人姓名
        /// IC卡在校验时赋值
        /// </summary>
        public static string strCardName;
        #endregion

        #region 获取系统设置模块：初始化设置、串口设置、LED设置、打印标题设置、客户端设置的信息
        /// <summary>
        /// 硬盘录像机的IP
        /// </summary>
        public static string DVRIP;
        /// <summary>
        /// 硬盘录像机的服务器端口号
        /// </summary>
        public static string DVRServerPort;
        /// <summary>
        /// 硬盘录像机的登录名称
        /// </summary>
        public static string DVRLoginName;
        /// <summary>
        /// 硬盘录像机的登录密码
        /// </summary>
        public static string DVRPwd;
        /// <summary>
        /// 硬盘录像机的IP
        /// </summary>
        public static string DVRIPTwo;
        /// <summary>
        /// 硬盘录像机的服务器端口号
        /// </summary>
        public static string DVRServerPortTwo;
        /// <summary>
        /// 硬盘录像机的登录名称
        /// </summary>
        public static string DVRLoginNameTwo;
        /// <summary>
        /// 硬盘录像机的登录密码
        /// </summary>
        public static string DVRPwdTwo;
        /// <summary>
        /// PCL串口号
        /// </summary>
        public static string PCL;
        /// <summary>
        ///  读卡器1串口号
        /// </summary>
        public static string CARD;
        /// <summary>
        /// 读卡器2串口号
        /// </summary>
        public static string CARD2;
        /// <summary>
        /// 加密狗识别码
        /// </summary>
        public static string NTCode;
        /// <summary>
        /// 门岗
        /// </summary>
        public static string MenGangName;
        /// <summary>
        /// 门岗状态
        /// </summary>
        public static string Position_State;
        /// <summary>
        /// 数据源
        /// </summary>
        public static string DataSource;
        /// <summary>
        /// 数据库
        /// </summary>
        public static string Database;
        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName;
        /// <summary>
        /// 密码
        /// </summary>
        public static string Pwd;
        /// <summary>
        /// 服务器图片存储路径
        /// </summary>
        public static string ImageServerPath;

        /// <summary>
        /// 串口设置
        /// </summary>
        public static string Parity_NameNumber;
        public static string Parity_Name;
        public static string Parity_Number;
        public static string Parity_BaudRate;
        public static string Parity_DataeBit;
        public static string Parity_StopBit;
        /// <summary>
        /// LED设置
        /// </summary>
        public static string LEDFromX;
        public static string LEDFromY;
        public static string LEDX;
        public static string LEDY;
        /// <summary>
        /// 打印标题设置
        /// </summary>
        public static string PrintDemoTitle;
        /// <summary>
        /// 客户端设置
        /// </summary>
        public static string ClientName;
        public static string ClientPhone;
        public static string IsBuys;
        public static string ClientAddress;
        public static string ClientRemark;
        #endregion

        #region  车辆登记
        /// <summary>
        /// 车辆类型 
        /// </summary>
        public static string CarType = "成品车辆";
        /// <summary>
        /// 车辆登记
        /// </summary>
        public static bool SerialCarDengji = true;// (true：非SAP校验登记)
        /// <summary>
        /// 非SAP校验登记验证黑名单的message参数信息
        /// </summary>
        public static string Blacklist = "";

        #endregion

        /// <summary>
        /// 进出厂小票识别 (true：进厂)
        /// 在mainfrom界面跳转页面时赋值
        /// </summary>
        public static bool Serialnumberbool = true;//
        /// <summary>
        /// 小票号
        /// </summary>
        public static string Serialnumber = "";//小票号
        /// <summary>
        /// 小票 IC卡标识
        /// (true:小票)
        /// </summary>
        public static bool SerialnumberICbool = true;//
        /// <summary>
        /// 当前门岗
        /// 在登录时：将客户端配置的SystemClass.PositionName
        /// </summary>
        public static string CurrentGate = "";
        /// <summary>
        /// 门岗编号
        /// 在登录时：将客户端配置的SystemClass.PositionName
        /// </summary>
        public static int CurrentGate_ID;
        /// <summary>
        /// 通道
        /// </summary>
        public static string Driveway_Name = "";
        /// <summary>
        /// 通道编号
        /// </summary>
        public static string Driveway_Id = "";
        /// <summary>
        /// 刷卡保安名称
        /// 在CarInfoForm的timer中读取当前卡号姓名
        /// </summary>
        public static string SecurityStaffName = "";
        public static string I_WTD_ID = "";
        public static string I_CRFLG = "";

        #region 李灵 通道校验
        /// <summary>
        ///进厂门岗
        /// </summary>
        public static string strGate_Entry = "";
        /// <summary>
        ///出厂门岗
        /// </summary>
        public static string strGate_Exit = "";
        /// <summary>
        ///进厂通道
        /// </summary>
        public static string strDriveway_Name = "";
        /// <summary>
        ///出厂通道
        /// </summary>
        public static string strDriveway_OutName = "";


        #endregion

        #region 类型转换

        /// <summary>
        /// 转换为double类型的数据
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static double GetDouble(object num)
        {
            try
            {
                return double.Parse(num.ToString());
            }
            catch
            {

                return 0.00;
            }

        }
        /// <summary>
        /// 转换为decimal类型的数据
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static decimal GetDecimal(object num)
        {
            try
            {
                return decimal.Parse(num.ToString());
            }
            catch
            {

                return 0;
            }

        }

        /// <summary>
        /// 转化换为int类型的数据
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static int GetInt(object num)
        {
            try
            {
                return int.Parse(num.ToString());
            }
            catch
            {

                return 0;
            }

        }


        /// <summary>
        /// 转化换为int类型的数据，输入默认值
        /// </summary>
        /// <param name="num">转换对象</param>
        /// <param name="rint">默认值</param>
        /// <returns></returns>
        public static int GetInt(object num, int rint)
        {

            try
            {
                int r = int.Parse(num.ToString());
                if (r == 0)
                {
                    r = rint;
                }
                return r;
            }
            catch
            {

                return rint;
            }

        }

        /// <summary>
        /// 转化换为string类型的数据
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string GetString(object num)
        {
            try
            {
                return num.ToString();
            }
            catch
            {

                return " ";
            }

        }
        /// <summary>
        /// 转换为Datetime类型的数据默认值为1900-01-01 00:00:00.000
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(object num)
        {
            try
            {
                if (num != null)
                {
                    return DateTime.Parse(num.ToString());
                }
                else
                {
                    return DateTime.Parse("1900-01-01 00:00:00.000");
                }
            }
            catch
            {
                return DateTime.Parse("1900-01-01 00:00:00.000");
            }
        }


        public static string GetStr(object str)
        {
            string rstr = "0";
            if (str != null)
            {
                rstr = str.ToString().Replace("%", "");
            }
            if (rstr == "合格")
            {
                rstr = "0";
            }
            return rstr;
        }
        #endregion

        #region 日志记录
        /// <summary>
        /// 记录测试记事本
        /// </summary>
        /// <param name="text">信息</param>
        public static void WriteTextLog(string text)
        {
            try
            {
                string dirpath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "LOG//" + CommonalityEntity.GetServersTime().ToString("yyyyMMdd") + "log.txt";
                if (!File.Exists(dirpath))
                {
                    FileStream fs1 = new FileStream(dirpath, FileMode.Create, FileAccess.Write);//创建写入文件
                    fs1.Close();
                }
                StreamWriter sw = new StreamWriter(dirpath, true);
                sw.WriteLine(CommonalityEntity.GetServersTime().ToString("yyyy-MM-dd HH:mm:ss") + ": " + text);
                sw.Close();
            }
            catch
            {

            }
        }

        /// <summary>
        /// 操作记录到数据库表fh_operateinfo
        /// </summary>
        /// <param name="operType">操作类型</param>
        /// <param name="content">内容</param>
        /// <param name="oper_name">操作人</param>
        public static void WriteLogData(string operType, string content, string oper_name)
        {
            try
            {
                //LogInfo qcRecord = new LogInfo();
                //if (oper_name == "")
                //{
                //    oper_name = Common.USERNAME;
                //}
                //qcRecord.Log_Name = oper_name;
                //qcRecord.Log_Dictionary_ID = DictionaryDAL.GetDictionaryID(operType);
                //qcRecord.Log_Content = content;
                //qcRecord.Log_Time = CommonalityEntity.GetServersTime();
                //LogInfoDAL.InsertOneQCRecord(qcRecord);
            }
            catch
            {
                common.WriteTextLog("Common.WriteLogData:");
            }
        }
        #endregion

        #region 列表控件 公共方法

        /// <summary>
        /// 设置当前获取焦点的列
        /// </summary>
        /// <param name="dgv">控件名称</param>
        /// <param name="currnetRow">当前行数</param>
        /// <param name="cellName">设置的列名称</param>
        public static void SetCurrentCell(DataGridView dgv, int currnetRow, string cellName)
        {
            dgv.CurrentCell = dgv.Rows[0].Cells[cellName];
            dgv.EditMode = DataGridViewEditMode.EditProgrammatically;//.EditOnKeystroke;//.EditOnEnter;
            dgv.BeginEdit(true);
        }

        /// <summary>
        /// 点击选中行的颜色
        /// </summary>
        /// <param name="dgv"></param>
        public static void SetSelectionBackColor(DataGridView dgv)
        {
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect; //点击选中整行
            dgv.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.YellowGreen; //付给颜色
        }
        #endregion




        /// <summary>
        /// 验证道闸是否关闭
        /// </summary>
        /// <param name="strGateEntry">进门岗</param>
        /// <param name="strGateExit">出门岗</param>
        /// <param name="strDrivewayName">进通道</param>
        /// <param name="strDrivewayName">出通道</param>
        /// <returns>true为正常，false为关闭</returns>
        public static bool CheckedSignoControl(string strGateEntry, string strGateExit, string strDrivewayName, string strOutDrivewayName)
        {
            bool bfalg = true;
            string positionName = "0";
            string gateExit = "0";
            string drivewayName = "0";
            string outdrivewayName = "0";
            switch (strGateEntry)
            {
                case "1#门岗":
                    positionName = "1";
                    break;
                case "2#门岗":
                    positionName = "2";
                    break;
                case "3#门岗":
                    positionName = "3";
                    break;
                case "4#门岗":
                    positionName = "4";
                    break;
            }
            switch (strGateExit)
            {
                case "1#门岗":
                    gateExit = "1";
                    break;
                case "2#门岗":
                    gateExit = "2";
                    break;
                case "3#门岗":
                    gateExit = "3";
                    break;
                case "4#门岗":
                    gateExit = "4";
                    break;
            }

            switch (strDrivewayName)
            {
                case "1#通道":
                    drivewayName = "1";
                    break;
                case "2#通道":
                    drivewayName = "2";
                    break;
                case "3#通道":
                    drivewayName = "3";
                    break;
                case "4通道":
                    drivewayName = "4";
                    break;
            }
            switch (strOutDrivewayName)
            {
                case "1#通道":
                    outdrivewayName = "1";
                    break;
                case "2#通道":
                    outdrivewayName = "2";
                    break;
                case "3#通道":
                    outdrivewayName = "3";
                    break;
                case "4通道":
                    outdrivewayName = "4";
                    break;
            }
            string strsql = "select  count(*) from eh_Control  where  1=1 ";
            if (positionName != "0" && gateExit != "0")
            {
                strsql += " and (usher='" + positionName + "' or usher='" + gateExit + "') ";
            }
            else if (gateExit != "0")
            {
                strsql += " and usher='" + gateExit + "'";
            }
            else if (positionName != "0")
            {
                strsql += " and usher='" + positionName + "'";
            }

            if (drivewayName != "0" && outdrivewayName != "0")
            {
                strsql += "and (channel='" + drivewayName + "' or channel='" + outdrivewayName + "') ";
            }
            else if (drivewayName != "0")
            {
                strsql += "and channel='" + drivewayName + "'";
            }
            else if (outdrivewayName != "0")
            {
                strsql += "and channel='" + outdrivewayName + "'";
            }
            strsql += " and [state]='0'";

            object objCount = LinQBaseDao.GetSingle(strsql);
            if (objCount != null)
            {
                int icount = 0;
                int.TryParse(objCount.ToString(), out icount);
                if (icount > 0)
                {
                    bfalg = false;

                }

            }

            return bfalg;
        }


        /// <summary>
        /// 新增修改数据的日志记录
        /// </summary>
        /// <param name="operateContent">操作内容</param>
        public static void AddUpdateLog(string operateContent)
        {
            string operateUser = common.NAME;
            string operateTime = CommonalityEntity.GetServersTime().ToString();
            string operateIP = "";
            string sqlLog = string.Format("insert into eh_Operate(Operate_Content,Operate_User,Operate_Time,Operate_IP) values('{0}','{1}','{2}','{3}')", operateContent, operateUser, operateTime, operateIP);
            LinQBaseDao.ExecuteSql(sqlLog);
        }

    }
}
