using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.DAL;
using System.Collections;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Data;
using System.Linq.Expressions;
using EMEWE.CarManagementDAL;
using WindowsFormsApplication3;
using System.Threading;
using GemBox.ExcelLite;

namespace EMEWE.CarManagement.Commons.CommonClass
{
    public class CommonalityEntity
    {
        /// <summary>
        /// IC卡号
        /// </summary>
        public static string strCardNo = "";
        /// <summary>
        /// 是否是复制策略进入通行策略界面
        /// </summary>
        public static bool boolCopyDrivewayStrategy = true;
        /// <summary>
        /// 是否是复制策略进入管控策略界面
        /// </summary>
        public static bool boolCepyManagementStrategy = true;
        public static List<DeviceControl> listCarIC = new List<DeviceControl>();
        /// <summary>
        /// 登录名
        /// </summary>
        public static string USERNAME = "Admin";
        /// <summary>
        /// 用户编号
        /// </summary>
        public static int USERID = 1;
        /// <summary>
        /// 真实姓名
        /// </summary>
        public static string NAME = "管理员";
        /// <summary>
        /// 温馨提示
        /// </summary>
        public static string message = "呼叫后,你将有10分钟的通行\n时间，请注意按时通行";
        /// <summary>
        /// 角色ID
        /// </summary>
        public static int USERRoleID = 0;
        /// <summary>
        /// LED设置编号
        /// </summary>
        public static int PositionLED_ID = -1;
        /// <summary>
        /// 保存设定的通道通行的车辆类型
        /// key 通道  value车辆类型
        /// </summary>
        public static Dictionary<string, string> CarType = new Dictionary<string, string>();

        /// <summary>
        /// key 通道值  当前放行的通道
        /// </summary>
        public static List<string> CarValue = new List<string>();
        /// <summary>
        /// 放行页面是否点击确认放行
        /// </summary>
        public static bool isRestFXOne = false;
        public static bool isRestFXTwo = false;
        public static bool isRestFXThree = false;
        public static bool isRestFXFour = false;
        public static bool isRestFXFive = false;
        public static bool isRestFXSix = false;
        /// <summary>
        /// LED显示状态
        /// </summary>
        public static bool IsCancellation = false;

        /// <summary>
        /// 当前通道Value值
        /// IC卡 在验证时赋值，小票号在数据库中读取对应的通道与当前通道对比
        /// </summary>
        public static string CurrentChannelNumber = "";
        /// <summary>
        /// 通道值，刷车卡，人卡，小票后赋值
        /// </summary>
        public static string Driveway_Value = "";
        /// <summary>
        /// 通道名称
        /// </summary>
        public static string Driveway_Name = "";
        /// <summary>
        /// 车辆类型
        /// </summary>
        public static string Car_Type = "";
        /// <summary>
        /// 车辆类型名称
        /// </summary>
        public static string CarType_Name = "";
        /// <summary>
        /// SAP车辆属性
        /// </summary>
        public static string CarAttribute = "";
        /// <summary>
        /// SAP凭证标识
        /// </summary>
        public static string SapNumber = "";
        /// <summary>
        /// 车辆类型编号
        /// </summary>
        public static string Car_Type_ID = "";
        /// <summary>
        /// 车辆标识(不同车辆类型 标识不同)
        /// </summary>
        public static string CarNumber = "";
        /// <summary>
        /// 车辆信息编号(车辆信息界面查询详细信息时或修改车辆信息时赋值)
        /// </summary>
        public static string CarInfo_ID = "";
        /// <summary>
        /// 是否修改车辆基本信息
        /// </summary>
        public static bool UpdateCar = false;
        /// <summary>
        /// 是否完成业务
        /// </summary>
        public static bool boolYesNoCarInOutRecord_ISFulfill = false;
        /// <summary>
        ///   CarInOutForm页面用于清空文本框的值
        /// </summary>
        public static int contolone = 0;
        public static int contoltwo = 0;
        public static int contolthree = 0;
        public static int contolfour = 0;
        public static int contolfive = 0;
        public static int contolsix = 0;
        /// <summary>
        /// CarInOutForm页面用于清空文本框的值
        /// </summary>
        public static int contolint = 0;
        /// <summary>
        /// 是否Sap登记
        /// </summary>
        public static bool ISsap = false;
        /// <summary>
        /// 是否显示手动控制道闸
        /// </summary>
        public static int iscontrol = 0;

        /// <summary>
        /// 道闸
        /// </summary>
        public static int intss = 0;

        /// <summary>
        /// 登记标识
        /// </summary>
        public static bool ISlogin = false;

        /// <summary>
        /// 车辆照片ID
        /// </summary>
        public static string CarPic_ID = "";
        /// <summary>
        /// IC卡
        /// </summary>
        public static string ICC1 = "";
        public static string ICC2 = "";
        /// <summary>
        /// 成品二次排队是否优先校验
        /// </summary>
        public static bool ISSecondXY = false;
        /// <summary>
        /// 成品二次排队是否时间校验
        /// </summary>
        public static bool ISOutTime = false;
        /// <summary>
        /// 车辆基础信息ID
        /// </summary>
        public static int carid = 0;
        /// <summary>
        /// 是否优先车校验
        /// </summary>
        public static bool ISYX = false;
        public static bool yxcar = false;//车
        public static bool yxstaffinfo = false;//驾驶员
        public static bool yxcustomerinfo = false;//公司
        public static bool yxincheck = false;//优先车是否自动进门授权

        public static int CarInOutInfoRecord_ID = 0;

        /// <summary>
        /// 打印设置浏览
        /// </summary>
        public static int printindex = -1;

        /// <summary>
        /// 是否播放欢迎语
        /// </summary>
        public static bool ishujiao = true;
        public static bool isvoic = true;
        /// <summary>
        /// 登录系统时间
        /// </summary>
        public static DateTime LoginTime;
        /// <summary>
        /// 是否第一次加载1是0否
        /// </summary>
        public static int isLoad = 0;

        /// <summary>
        /// 通道地感  
        /// </summary>
        public static List<string> FvnValue = new List<string>();


        #region 用户控件
        /// <summary>
        /// 表名
        /// </summary>
        public static string tablename = "";
        /// <summary>
        /// 字段一
        /// </summary>
        public static string tabcom1 = "";
        /// <summary>
        /// 字段二
        /// </summary>
        public static string tabcom2 = "";
        /// <summary>
        /// 字段二
        /// </summary>
        public static string tabcom3 = "";
        /// <summary>
        /// 字段ID
        /// </summary>
        public static string tabid = "";

        /// <summary>
        /// 传入的查询字符串，对字段一进行模糊查询
        /// </summary>
        public static string strlike = "";
        #endregion

        #region 菜单权限信息
        public static ArrayList arraylist;//存放菜单ID
        public static int RoleID;//要修改的角色ID
        public static int UserID;//要修改的用户ID
        public static bool YesNoBoolRoleUser;//true为用户，false为角色
        public static bool rbools;

        #endregion

        #region  车辆进出厂
        /// <summary>
        /// 是否进出车辆信息窗体 true:进  false：没进
        /// </summary>
        public static bool rboolInCar = false;
        /// <summary>
        /// 存放有效图片
        /// </summary>
        public static List<string> listCarPicEffective = new List<string>();

        /// <summary>
        /// 存放过期图片
        /// </summary>
        public static List<string> listCarPic = new List<string>();
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
        /// 管控策略类型
        /// </summary>
        public static string strManagementStrategy_Type = "";
        /// <summary>
        /// 是否能放行标识TRUE:可以放行 FALSE：不能放行
        /// </summary>
        public static bool rboolRelease = true;
        /// <summary>
        /// 车辆校验异常记录编号
        /// </summary>
        public static int intCarInOutInfoRecord_ID = 0;
        /// <summary>
        /// 车辆校验异常位置类型表名
        /// </summary>
        public static string strUnusualRecordTable = "";
        /// <summary>
        /// 当前校验的通道名称
        /// </summary>
        public static string strDriveway_Name = "";
        /// <summary>
        /// 通道的实际值
        /// </summary>
        public static string strDriveway_Value = "";
        /// <summary>
        /// ASP标识
        /// </summary>
        public static string strASP = "";
        /// <summary>
        /// 车卡卡号
        /// </summary>
        public static string CarIC = "";
        /// <summary>
        /// false没有验证车卡,true验证车卡
        /// </summary>
        public static bool carICHave = false;

        /// <summary>
        /// 人卡卡号
        /// </summary>
        public static string UserIC = "";
        /// <summary>
        /// 人员是否有卡，或者卡号是否启动(false 没有卡或者未启动 true 有卡且状态正常)
        /// </summary>
        public static bool UserICHave = false;

        /// <summary>
        /// 保安卡
        /// </summary>
        public string EnsureSafetyIC = "";
        /// <summary>
        /// false没有验证保安卡,true验证保安卡
        /// </summary>
        public static bool EnsureSafetyICHave = false;

        /// <summary>
        /// 车牌号
        /// </summary>
        public static string CarNO;//车牌号
        /// <summary>
        /// 总记录编号
        /// </summary>
        public static string CarInoutid = "";
        /// <summary>
        /// 是否修改通行策略
        /// </summary>
        public static bool IsUpdatedri = false;
        /// <summary>
        /// 通行策略编号
        /// </summary>
        public static int DrivewayStrategy_ID = -1;
        /// <summary>
        ///  通道ID
        /// </summary>
        public static int Driveway_ID = -1;
        /// <summary>
        ///  门岗ID
        /// </summary>
        public static int Position_ID = -1;

        /// <summary>
        ///  门岗ID
        /// </summary>
        public static string Position_Value = "01";

        /// <summary>
        /// 通道true进厂、false出厂
        /// </summary>
        public static bool ISInOut = false;
        /// <summary>
        /// 是否只登记一次
        /// </summary>
        public static bool Car_ISRegister = false;
        /// <summary>
        /// 车辆是否存在登记记录
        /// </summary>
        public static bool ISDengji = true;
        #region 车辆过磅类型
        /// <summary>
        /// 车辆过磅类型:进门地磅
        /// </summary>
        public static string upWeight = "进门地磅";
        /// <summary>
        /// 车辆过磅类型:出门地磅
        /// </summary>
        public static string outWeight = "出门地磅";
        /// <summary>
        /// 车辆过磅类型:装货点第一次过磅
        /// </summary>
        public static string loadFirstWeight = "装货点第一次过磅";
        /// <summary>
        /// 车辆过磅类型:装货点第二次过磅
        /// </summary>
        public static string loadSecondWeight = "装货点第二次过磅";
        /// <summary>
        /// 车辆过磅类型:卸货点第一次过磅
        /// </summary>
        public static string unloadFirstWeight = "卸货点第一次过磅";
        /// <summary>
        /// 车辆过磅类型:卸货点第二次过磅
        /// </summary>
        public static string unloadSecondWeight = "卸货点第二次过磅";
        #endregion

        /// <summary>
        /// 通道是否报修
        /// </summary>
        public static bool isDriState = false;
        /// <summary>
        /// 启用的备用通道ID
        /// </summary>
        public static string DriWarrantyID = "0";

        #endregion

        private static byte[] Key64 = { 42, 16, 93, 156, 78, 4, 218, 32 };
        private static byte[] IV64 = { 55, 103, 246, 79, 36, 99, 167, 3 };
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

        /// <summary>
        /// 获取服务器时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetServersTime()
        {
            try
            {
                return DateTime.Parse(LinQBaseDao.Query("select GETDATE()").Tables[0].Rows[0][0].ToString());
            }
            catch
            {

                return DateTime.Parse("1900-01-01 00:00:00.000");
            }

        }
        /// <summary>
        /// 自动增长 生成小票号
        /// </summary>
        /// <param name="number">初始值</param>
        /// <returns>返回生成的小票号</returns>
        public static string Number(int nb)
        {
            string str = "";
            if (nb + 1 < 10)
            {
                str = "00000" + (nb + 1).ToString();
            }
            else if (nb + 1 < 100)
            {
                str = "0000" + (nb + 1).ToString();
            }
            else if (nb + 1 < 1000)
            {
                str = "000" + (nb + 1).ToString();
            }
            else if (nb + 1 < 10000)
            {
                str = "00" + (nb + 1).ToString();
            }
            else if (nb + 1 < 100000)
            {
                str = "0" + (nb + 1).ToString();
            }
            else if (nb + 1 < 1000000)
            {
                str = (nb + 1).ToString();
            }
            else if (nb + 1 < 10000000)
            {
                str = (nb + 1).ToString();
            }
            else
            {
                str = (nb + 1).ToString();
            }
            return str;
        }
        /// <summary>
        /// 自动增长 生成排队号
        /// </summary>
        /// <param name="number">初始值</param>
        /// <returns>返回生成的排队号</returns>
        public static string SortNumber(int nb)
        {
            string str = "";
            if (nb + 1 < 10)
            {
                str = "000" + (nb + 1).ToString();
            }
            else if (nb + 1 < 100)
            {
                str = "00" + (nb + 1).ToString();
            }
            else if (nb + 1 < 1000)
            {
                str = "0" + (nb + 1).ToString();
            }
            else if (nb + 1 < 10000)
            {
                str = (nb + 1).ToString();
            }
            else
            {
                str = (nb + 1).ToString();
            }
            return str;
        }
        /// <summary>
        /// 设置文本框只能输入数字
        /// </summary>
        /// <param name="e"></param>
        public static bool DigitalMethod(KeyPressEventArgs e)
        {
            bool rbool = true;
            try
            {

                if (!(e.KeyChar <= '9' && e.KeyChar >= '0') && e.KeyChar != '\r' && e.KeyChar != '\b')
                {
                    e.Handled = true;
                }
                else
                {
                    rbool = false;
                }
            }
            catch
            {

                WriteTextLog("CommonalityEntity.DigitalMethod()");
            }
            return rbool;
        }

        #region 日志记录
        /// <summary>
        /// 记录测试记事本
        /// </summary>
        /// <param name="text">信息</param>
        public static void WriteTextLog(string text)
        {
            try
            {
                string dirpath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "LOG//" +DateTime.Now.ToString("yyyyMMdd") + "log.txt";
                if (!File.Exists(dirpath))
                {
                    FileStream fs1 = new FileStream(dirpath, FileMode.Create, FileAccess.Write);//创建写入文件
                    fs1.Close();
                }
                StreamWriter sw = new StreamWriter(dirpath, true);
                sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + text);
                sw.Close();
            }
            catch
            {

            }
        }

        /// <summary>
        /// 记录测试记事本
        /// </summary>
        /// <param name="text">信息</param>
        public static void WriteTextLogTime(string text)
        {
            try
            {
                string dirpath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "logTime.txt";
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
                LogInfo qcRecord = new LogInfo();
                if (oper_name == "")
                {
                    oper_name = CommonalityEntity.USERNAME;
                    //oper_name = common.USERNAME;
                }
                qcRecord.Log_Name = oper_name;
                qcRecord.Log_Type = DictionaryDAL.GetDictionaryID1(operType);
                qcRecord.Log_Content = content;
                qcRecord.Log_Time = CommonalityEntity.GetServersTime();
                LogInfoDAL.InsertOneLogInfo(qcRecord);
            }
            catch
            {
                WriteTextLog("CommonalityEntity.WriteLogData:");
            }
        }
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求不超过16位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString)
        {

            try
            {
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                ICryptoTransform transform1 = dCSP.CreateEncryptor(Key64, IV64);
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, transform1, CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                cStream.Close();
                return Convert.ToBase64String(mStream.ToArray());

            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求不超过16位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string DecryptDES(string decryptString)
        {

            try
            {
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(Key64, IV64), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                cStream.Close();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }



        #endregion

        #region SAP校验
        /// <summary>
        /// 验证该登记车辆是否为重复登记
        /// </summary>
        /// <param name="carName">车牌号</param>
        /// <returns>返回true(重复) or false(不重复)</returns>
        public static bool ChkRepeat(string carName)
        {
            bool rbool = false;
            string sql = "select Car_CarType_ID from View_Car_ICard_CarType where Car_Name='" + carName + "' and CarType_OtherProperty='内部车辆'";
            DataTable dtcar = LinQBaseDao.Query(sql).Tables[0];
            string cartypeid = "";
            if (dtcar.Rows.Count > 0)
            {
                cartypeid = dtcar.Rows[0][0].ToString();
            }
            sql = "select top (1) CarInfo_ID,CarInfo_CarType_ID,CarInOutRecord_ISFulfill,SortNumberInfo_TongXing,SmallTicket_Allowcounted from View_CarState where CarInfo_name='" + carName + "'  order by carInfo_ID desc";
            DataTable objdt = LinQBaseDao.Query(sql).Tables[0];
            if (objdt.Rows.Count > 0)
            {
                string obj = objdt.Rows[0][0].ToString();
                string cartid = objdt.Rows[0][1].ToString();
                bool isfulfill = Convert.ToBoolean(objdt.Rows[0]["CarInOutRecord_ISFulfill"].ToString());
                if (isfulfill)
                {
                    rbool = false;
                }
                else
                {
                    if (cartid != cartypeid)
                    {
                        rbool = true;
                    }
                    if (cartypeid == "")
                    {
                        if (objdt.Rows[0]["SortNumberInfo_TongXing"].ToString() != "已注销" && objdt.Rows[0]["SortNumberInfo_TongXing"].ToString() != "已出厂")
                        {
                            rbool = true;
                        }
                        else
                        {
                            rbool = false;
                        }
                    }
                }

            }
            else
            {
                rbool = false;
            }
            ///车辆多次进出厂时
            if (rbool)
            {
                if (Convert.ToInt32(objdt.Rows[0]["SmallTicket_Allowcounted"].ToString()) > 0)//大于零代表该车辆已经通行过
                {
                    rbool = false;
                }
            }
            return rbool;
        }
        /// <summary>
        /// SAP登记编号(校验登记成功赋值)
        /// </summary>
        public static string SAP_ID = "";
        /// <summary>
        /// 获取SAP验证结构
        /// </summary>
        /// <param name="strWTD_ID">成品车辆委托单号/送货车辆采购凭证号</param>
        /// <param name="strCRFLG">出入标识：F-验证，A-表示到达，I-表示放入，C-表示出场</param>
        /// <param name="strZCARNO">车号</param>
        /// <param name="SapSerialnumber">小票号</param>
        /// <param name="falg">1为设置为修改，2为是否验证</param>
        /// <returns>true (成功或无需验证）  false 失败</returns>
        public static bool GetSAP(string strWTD_ID, string strCRFLG, string strZCARNO, string SapSerialnumber, string falg)
        {
            try
            {
                bool rbool = true;
                Expression<Func<eh_SAPRecord, bool>> expr = n => n.Sap_InCRFLG == null && n.Sap_Identify != null;
                expr = (Expression<Func<eh_SAPRecord, bool>>)PredicateExtensionses.True<eh_SAPRecord>();
                if (strZCARNO != "" && strCRFLG != "A")
                {

                    //Sap_State =1";//进出厂有效标识
                    expr = expr.And(n => n.Sap_InCarNumber == strZCARNO && n.Sap_State == 1);
                }
                if (SapSerialnumber != "")
                {

                    expr = expr.And(n => n.Sap_Serialnumber == SapSerialnumber);
                }
                if (strWTD_ID != "")
                {
                    expr = expr.And(n => n.Sap_InNO == strWTD_ID);
                }

                var eh_saprecordfun = from p in eh_SAPRecordDAL.Query(expr)
                                      orderby p.Sap_ID descending
                                      select p;
                if (eh_saprecordfun != null)
                {
                    foreach (var m in eh_saprecordfun)
                    {
                        string sapType = "";
                        string sapIdentify = "";
                        if (!string.IsNullOrEmpty(m.Sap_InNO))
                        {
                            strWTD_ID = m.Sap_InNO;
                        }
                        if (!string.IsNullOrEmpty(m.Sap_Type))
                        {
                            sapType = m.Sap_Type;
                        }
                        if (!string.IsNullOrEmpty(m.Sap_Identify))
                        {
                            sapIdentify = m.Sap_Identify;
                        }
                        if (sapIdentify.Contains("," + strCRFLG + ","))
                        {
                            if (falg == "1")//进出厂状态更新
                            {
                                if (sapType == "成品车辆")
                                {
                                    rbool = SendSAP(strWTD_ID, strCRFLG, strZCARNO, SapSerialnumber);
                                }
                                else if (sapType == "送货车辆")
                                {
                                    rbool = DelivergoodsSendSAP(strWTD_ID, strCRFLG, strZCARNO, SapSerialnumber);
                                }
                                else
                                {
                                    rbool = SAPVBLN(strWTD_ID, strCRFLG, strZCARNO, SapSerialnumber);
                                }
                            }
                            else if (falg == "2")
                            {
                                rbool = false;//如果falg == "2", rbool = false时，是执行车辆管理系统进出厂的验证。
                            }
                        }

                    }
                }
                return rbool;

            }
            catch (Exception)
            {
                return false;
            }

        }
        /// <summary>
        /// 验证SAP送货车辆是否成功
        /// </summary>
        /// <param name="strEBELN">采购凭证号</param>
        /// <param name="strCRFLG">出入标识：A-表示到达，I-表示放入，C-表示出场</param>
        /// <param name="strZCARNO">车号</param>
        /// <returns></returns>
        public static bool DelivergoodsSendSAP(string strEBELN, string strCRFLG, string strZCARNO, string SapSerialnumber)
        {

            DCCarManagementDataContext dmdc = new DCCarManagementDataContext();
            string strSql = "";
            bool rbool = false;
            try
            {
                if (strZCARNO != "" && strEBELN != "")
                {
                    SAPClass de = new SAPClass();
                    Thread thread = new Thread(new ThreadStart(de.SendGoods));
                    de.I_CRFLG = strCRFLG;
                    de.I_EBELN = strEBELN;
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    thread.Join();

                    #region 测试数据
                    //DataTable dtReturn = new DataTable();
                    //DataRow dr2 = dtReturn.NewRow();
                    //dtReturn.Columns.Add("type");
                    //dtReturn.Columns.Add("message");
                    ////dr2["type"] = "E";
                    ////dr2["message"] = "该车对应该采购订单今天已经存在放行记录";

                    //dr2["type"] = "S";
                    //dr2["message"] = "成功";
                    //dtReturn.Rows.Add(dr2);
                    //de.Table2 = dtReturn;
                    #endregion
                    DataTable dt = de.Table2;
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["type"].ToString().ToUpper() == "S")//消息类型:S-成功，F-失败 G-查询失败 L-连接失败 E-更新
                            {
                                rbool = true;
                            }
                            else if (dt.Rows[0]["type"].ToString().ToUpper().Trim() == "E" && dt.Rows[0]["message"].ToString().Trim() == "该车对应该采购订单今天已经存在放行记录")
                            {
                                ADDUnusualRecord(2, "送货车辆", "送货车辆更新" + strCRFLG + "状态，" + dt.Rows[0]["type"].ToString().ToUpper().Trim() + dt.Rows[0]["message"].ToString(), CommonalityEntity.USERNAME, CheckProperties.ce.CarInfo_ID);
                                rbool = true;
                            }
                            else
                            {
                                ADDUnusualRecord(2, "送货车辆", "送货车辆更新" + strCRFLG + "状态，" + dt.Rows[0]["type"].ToString().ToUpper().Trim() + dt.Rows[0]["message"].ToString(), CommonalityEntity.USERNAME, CheckProperties.ce.CarInfo_ID);
                                rbool = true;
                            }

                            strSql = "INSERT INTO [eh_SAPRecord] ([Sap_Type] ,[Sap_InNO],[Sap_InCRFLG],[Sap_InTime],[Sap_InCarNumber],[Sap_OutETYPE],[Sap_OutEMSG],Sap_Remark,Sap_Serialnumber)VALUES('送货车辆','" + strEBELN + "','" + strCRFLG + "',getdate(),'" + strZCARNO + "','" + dt.Rows[0]["type"].ToString().ToUpper() + "','" + dt.Rows[0]["message"].ToString().ToUpper() + "','" + strCRFLG + "','" + SapSerialnumber + "')";
                            LinQBaseDao.ExecuteSql(strSql);
                        }
                        else
                        {

                            strSql = "INSERT INTO [eh_SAPRecord] ([Sap_Type] ,[Sap_InNO],[Sap_InCRFLG],[Sap_InTime],[Sap_InCarNumber],[Sap_OutETYPE],[Sap_OutEMSG],Sap_Remark,Sap_Serialnumber)VALUES('送货车辆','" + strEBELN + "','" + strCRFLG + "',getdate(),'" + strZCARNO + "','E','失败，没有返回数据','" + strCRFLG + "','" + SapSerialnumber + "')";
                            LinQBaseDao.ExecuteSql(strSql);
                        }
                    }
                    else
                    {

                        strSql = "INSERT INTO [eh_SAPRecord] ([Sap_Type] ,[Sap_InNO],[Sap_InCRFLG],[Sap_InTime],[Sap_InCarNumber],[Sap_OutETYPE],[Sap_OutEMSG],Sap_Remark,Sap_Serialnumber)VALUES('送货车辆','" + strEBELN + "','" + strCRFLG + "',getdate(),'" + strZCARNO + "','E','失败，没有返回数据','" + strCRFLG + "','" + SapSerialnumber + "')";
                        LinQBaseDao.ExecuteSql(strSql);
                    }
                }
            }
            catch
            {

            }
            return rbool;
            // return true;
        }
        /// <summary>
        /// 验证SAP成品纸车辆是否成功或无需验证
        /// </summary>
        /// <param name="strWTD_ID">成品车辆委托单号</param>
        /// <param name="strCRFLG">出入标识：A-表示到达，I-表示放入，C-表示出场</param>
        ///  <param name="strZCARNO">车号</param>
        /// <returns>true (成功或无需验证）  false 失败</returns>
        public static bool SendSAP(string strWTD_ID, string strCRFLG, string strZCARNO, string SapSerialnumber)
        {
            DCCarManagementDataContext dmdc = new DCCarManagementDataContext();
            bool rbool = false;
            try
            {
                if (strWTD_ID != "")
                {
                    SAPClass de = new SAPClass();
                    Thread thread = new Thread(new ThreadStart(de.SaveData));
                    de.I_WTD_ID = strWTD_ID;
                    de.I_CRFLG = strCRFLG;
                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();
                    thread.Join();

                    #region 测试数据
                    //DataTable dtReturn = new DataTable();
                    //DataRow dr2 = dtReturn.NewRow();
                    //dtReturn.Columns.Add("type");
                    //dtReturn.Columns.Add("message");
                    //dr2["type"] = "E";
                    //dr2["message"] = "该车对应该采购订单今天已经存在放行记录";

                    ////dr2["type"] = "S";
                    ////dr2["message"] = "成功";
                    //dtReturn.Rows.Add(dr2);
                    //de.Table2 = dtReturn;
                    #endregion
                    DataTable dt = de.Table2;
                    if (dt != null)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["type"].ToString().ToUpper() == "S")
                            {
                                rbool = true;
                            }
                            else if (dt.Rows[0]["type"].ToString().ToUpper().Trim() == "E" && dt.Rows[0]["message"].ToString().Trim() == "该车对应该采购订单今天已经存在放行记录")
                            {
                                ADDUnusualRecord(2, "成品车辆SAP校验", "成品车辆更新" + strCRFLG + "状态，" + dt.Rows[0]["type"].ToString().ToUpper().Trim() + dt.Rows[0]["message"].ToString(), CommonalityEntity.USERNAME, CheckProperties.ce.CarInfo_ID);
                                rbool = true;
                            }
                            else
                            {
                                ADDUnusualRecord(2, "成品车辆SAP校验", "成品车辆更新" + strCRFLG + "状态，" + dt.Rows[0]["type"].ToString().ToUpper().Trim() + dt.Rows[0]["message"].ToString(), CommonalityEntity.USERNAME, CheckProperties.ce.CarInfo_ID);
                                rbool = true;
                            }

                            string strSql = "INSERT INTO [eh_SAPRecord] ([Sap_Type] ,[Sap_InNO],[Sap_InCRFLG],[Sap_InTime],[Sap_OutETYPE],[Sap_OutEMSG],Sap_Remark,[Sap_InCarNumber],Sap_Serialnumber)VALUES('成品车辆','" + strWTD_ID + "','" + strCRFLG + "',getdate(),'" + dt.Rows[0]["type"].ToString().ToUpper() + "','" + dt.Rows[0]["message"].ToString().ToUpper() + "','" + strCRFLG + "','" + strZCARNO + "','" + SapSerialnumber + "')";
                            LinQBaseDao.ExecuteSql(strSql);

                        }
                        else
                        {

                            string strSql = "INSERT INTO [eh_SAPRecord] ([Sap_Type] ,[Sap_InNO],[Sap_InCRFLG],[Sap_InTime],[Sap_OutETYPE],[Sap_OutEMSG],Sap_Remark,[Sap_InCarNumber],Sap_Serialnumber)VALUES('成品车辆','" + strWTD_ID + "','" + strCRFLG + "',getdate(),'E','失败，没有返回数据','" + strCRFLG + "','" + strZCARNO + "','" + SapSerialnumber + "')";
                            string str = strSql + " Select @@identity";
                            LinQBaseDao.GetSingle(str);
                        }
                    }
                    else
                    {
                        string strSql = "INSERT INTO [eh_SAPRecord] ([Sap_Type] ,[Sap_InNO],[Sap_InCRFLG],[Sap_InTime],[Sap_OutETYPE],[Sap_OutEMSG],Sap_Remark,[Sap_InCarNumber],Sap_Serialnumber)VALUES('成品车辆','" + strWTD_ID + "','" + strCRFLG + "',getdate(),'E','失败，没有返回数据','" + strCRFLG + "','" + strZCARNO + "','" + SapSerialnumber + "')";
                        LinQBaseDao.ExecuteSql(strSql);
                    }
                }
            }
            catch
            {

            }
            return rbool;

        }
        /// <summary>
        /// 验证SAP三废车辆是否成功或无需验证
        /// </summary>
        /// <param name="strWTD_ID">成品车辆委托单号</param>
        /// <param name="strCRFLG">出入标识：A-表示到达，I-表示放入，C-表示出场</param>
        ///  <param name="strZCARNO">车号</param>
        /// <returns>true (成功或无需验证）  false 失败</returns>
        public static bool SAPVBLN(string strWTD_ID, string strCRFLG, string strZCARNO, string SapSerialnumber)
        {
            DCCarManagementDataContext dmdc = new DCCarManagementDataContext();
            bool rbool = false;
            try
            {
                SAPClass de = new SAPClass();
                Thread thread = new Thread(new ThreadStart(de.SanFei));
                de.I_WTD_ID = strWTD_ID;
                de.I_CRFLG = strCRFLG;
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();


                #region 测试


                //DataTable dts = new DataTable();
                //DataRow dr = dts.NewRow();
                //dts.Columns.Add("VBELN");//(交货单号)
                //dts.Columns.Add("NAME1_C");//（客户名称）
                //dts.Columns.Add("MAKTX");//（三废物料描述）
                //dr["VBELN"] = txtNumber.Text.Trim();
                //dr["NAME1_C"] = "cc";
                //dr["MAKTX"] = "三废物料";
                //dts.Rows.Add(dr);

                //dr = dts.NewRow();
                //dr["VBELN"] = txtNumber.Text.Trim();
                //dr["NAME1_C"] = "ww";
                //dr["MAKTX"] = "三废物料22";
                //dts.Rows.Add(dr);

                //de.Table1 = dts;

                //DataTable dtReturn = new DataTable();
                //DataRow dr2 = dtReturn.NewRow();
                //dtReturn.Columns.Add("type");
                //dtReturn.Columns.Add("message");
                //dr2["type"] = "S";
                //dr2["message"] = "成功";
                //dtReturn.Rows.Add(dr2);

                //dr2 = dtReturn.NewRow();
                //dr2["type"] = "S";
                //dr2["message"] = "成功";
                //dtReturn.Rows.Add(dr2);

                //de.Table2 = dtReturn;
                #endregion
                DataTable dt = de.Table2;
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["type"].ToString().ToUpper() == "S")
                        {
                            rbool = true;
                        }
                        else if (dt.Rows[0]["type"].ToString().ToUpper().Trim() == "E" && dt.Rows[0]["message"].ToString().Trim() == "该车对应该采购订单今天已经存在放行记录")
                        {
                            ADDUnusualRecord(2, "三废车辆SAP校验", "三废车辆更新" + strCRFLG + "状态，" + dt.Rows[0]["type"].ToString().ToUpper().Trim() + dt.Rows[0]["message"].ToString(), CommonalityEntity.USERNAME, CheckProperties.ce.CarInfo_ID);
                            rbool = true;
                        }
                        else
                        {
                            ADDUnusualRecord(2, "三废车辆SAP校验", "三废车辆更新" + strCRFLG + "状态，" + dt.Rows[0]["type"].ToString().ToUpper().Trim() + dt.Rows[0]["message"].ToString(), CommonalityEntity.USERNAME, CheckProperties.ce.CarInfo_ID);
                            rbool = true;
                        }

                        string strSql = "INSERT INTO [eh_SAPRecord] ([Sap_Type] ,[Sap_InNO],[Sap_InCRFLG],[Sap_InTime],[Sap_OutETYPE],[Sap_OutEMSG],Sap_Remark,[Sap_InCarNumber],Sap_Serialnumber)VALUES('三废车辆','" + strWTD_ID + "','" + strCRFLG + "',getdate(),'" + dt.Rows[0]["type"].ToString().ToUpper() + "','" + dt.Rows[0]["message"].ToString().ToUpper() + "','" + strCRFLG + "','" + strZCARNO + "','" + SapSerialnumber + "')";
                        LinQBaseDao.ExecuteSql(strSql);

                    }
                    else
                    {

                        string strSql = "INSERT INTO [eh_SAPRecord] ([Sap_Type] ,[Sap_InNO],[Sap_InCRFLG],[Sap_InTime],[Sap_OutETYPE],[Sap_OutEMSG],Sap_Remark,[Sap_InCarNumber],Sap_Serialnumber)VALUES('三废车辆','" + strWTD_ID + "','" + strCRFLG + "',getdate(),'E','失败，没有返回数据','" + strCRFLG + "','" + strZCARNO + "','" + SapSerialnumber + "')";
                        LinQBaseDao.ExecuteSql(strSql);
                    }
                }
                else
                {
                    string strSql = "INSERT INTO [eh_SAPRecord] ([Sap_Type] ,[Sap_InNO],[Sap_InCRFLG],[Sap_InTime],[Sap_OutETYPE],[Sap_OutEMSG],Sap_Remark,[Sap_InCarNumber],Sap_Serialnumber)VALUES('三废车辆','" + strWTD_ID + "','" + strCRFLG + "',getdate(),'E','失败，没有返回数据','" + strCRFLG + "','" + strZCARNO + "','" + SapSerialnumber + "')";
                    LinQBaseDao.ExecuteSql(strSql);
                }
            }
            catch
            {

            }
            return rbool;
        }

        /// <summary>
        /// 新增SAP三废车辆信息
        /// </summary>
        /// <param name="VBELN">交货单号</param>
        /// <param name="NAME1_C">客户名称</param>
        /// <param name="MAKTX">三废物料描述</param>
        /// <returns></returns>
        public static bool AddSAPVBELNInfo(string VBELN, string NAME1_C, string MAKTX, int indexRow, DataTable dtMessage)
        {
            string strSAPID = "";
            if (dtMessage != null)
            {
                if (dtMessage.Rows[indexRow]["type"].ToString().ToUpper() == "S")
                {
                    if (VBELN == "&nbsp;")
                    {
                        VBELN = "";
                    }
                    if (NAME1_C == "&nbsp;")
                    {
                        NAME1_C = "";
                    }
                    if (MAKTX == "&nbsp;")
                    {
                        MAKTX = "";
                    }
                    string strSql = "INSERT INTO [eh_SAPRecord] ([Sap_Type] ,[Sap_InNO],[Sap_OutNAME1C],[Sap_OutMAKTX],[Sap_OutETYPE],[Sap_OutEMSG],[Sap_Identify],Sap_Remark,Sap_InTime)values('三废车辆','" + VBELN + "','" + NAME1_C + "','" + MAKTX + "','" + dtMessage.Rows[indexRow]["type"].ToString().ToUpper() + "','" + dtMessage.Rows[0]["message"].ToString().ToUpper() + "','','F',getdate())";
                    strSql += " select @@identity";
                    object obj = LinQBaseDao.GetSingle(strSql);
                    if (obj != null)
                    {
                        strSAPID = obj.ToString();
                        CommonalityEntity.SAP_ID = strSAPID;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    string strSql = "INSERT INTO [eh_SAPRecord] ([Sap_Type] ,[Sap_InNO],[Sap_InTime],[Sap_OutETYPE],[Sap_OutEMSG],Sap_Remark)VALUES('三废车辆','" + CheckProperties.ce.SangFeiNumber + "',getdate(),'" + dtMessage.Rows[indexRow]["type"].ToString().ToUpper() + "','" + dtMessage.Rows[0]["message"].ToString().ToUpper() + "','F')";
                    LinQBaseDao.ExecuteSql(strSql);
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 新增SAP成品车信息
        /// </summary>
        /// <param name="CARNO">车号</param>
        /// <param name="WTD_ID">委托单</param>
        /// <param name="O_FLAG">是否开装货通知单</param>
        /// <param name="TEL_NUMBER">电话</param>
        /// <param name="HG">货高</param>
        /// <param name="XZ">限重</param>
        /// <param name="KDATB">有效起始日期</param>
        /// <param name="KDATE">有效截止日期</param>
        /// <param name="NAME1_C">客户名称</param>
        /// <param name="NAME1_P">外协车队供应商名称</param>
        /// <returns>true成功 flase失败</returns>
        public static bool AddWTDIDInfo(string CARNO, string WTD_ID, string O_FLAG, string TEL_NUMBER, string HG, string XZ, string KDATB, string KDATE, string NAME1_C, string NAME1_P, int indexRow, string Prodh, DataTable dtMessage)
        {
            if (CARNO == "&nbsp;")
            {
                CARNO = "";
            }
            if (WTD_ID == "&nbsp;")
            {
                WTD_ID = "";
            }
            if (O_FLAG == "&nbsp;")
            {
                O_FLAG = "";
            }
            if (TEL_NUMBER == "&nbsp;")
            {
                TEL_NUMBER = "";
            }
            if (NAME1_C == "&nbsp;")
            {
                NAME1_C = "";
            }
            if (NAME1_P == "&nbsp;")
            {
                NAME1_P = "";
            }
            string strSAPID = "";
            if (dtMessage != null)//验证成功失败
            {

                if (dtMessage.Rows[indexRow]["type"].ToString().ToUpper() == "S")
                {
                    if (O_FLAG != "")
                    {
                        O_FLAG = O_FLAG.ToUpper();
                    }
                    if (KDATB == "" || KDATB == "&nbsp;") KDATB = "1900-01-01 00:00:00.000";
                    if (KDATE == "" || KDATE == "&nbsp;") KDATE = "1900-01-01 00:00:00.000";
                    string strSql = "INSERT INTO [eh_SAPRecord] ([Sap_Type] ,[Sap_InNO],[Sap_InCarNumber],[Sap_OutOFLAG],[Sap_OutTELNUMBER],[Sap_OutHG],[Sap_OutXZ],[Sap_OutKDATB],[Sap_OutKDATE],[Sap_OutNAME1C],[Sap_OutNAME1P],[Sap_OutETYPE],[Sap_OutEMSG],[Sap_Identify],Sap_Remark,[Sap_InTime],Sap_Prodh)values('成品车辆','" + WTD_ID + "','" + CARNO + "','" + O_FLAG + "','" + TEL_NUMBER + "','" + HG + "','" + XZ + "','" + KDATB + "','" + KDATE + "','" + NAME1_C + "','" + NAME1_P + "','" + dtMessage.Rows[indexRow]["type"].ToString().ToUpper() + "','" + dtMessage.Rows[indexRow]["message"].ToString().ToUpper() + "',',A,,I,,C,','F',getdate(),'" + Prodh + "')";
                    string identity = "select @@identity";
                    object obj = LinQBaseDao.GetSingle(strSql + ";" + identity);
                    if (obj != null)
                    {
                        strSAPID = obj.ToString();
                        CommonalityEntity.SAP_ID = strSAPID;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    string strSql = "INSERT INTO [eh_SAPRecord] ([Sap_Type] ,[Sap_InCarNumber],[Sap_InTime],[Sap_OutETYPE],[Sap_OutEMSG],Sap_Remark)VALUES('成品车辆','" + CheckProperties.ce.ChengPinNumber + "',getdate(),'" + dtMessage.Rows[indexRow]["type"].ToString().ToUpper() + "','" + dtMessage.Rows[indexRow]["message"].ToString().ToUpper() + "','F')";
                    LinQBaseDao.ExecuteSql(strSql);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("连接失败");
                return false;
            }
            return true;
        }
        /// <summary>
        /// 新增SAP送货车辆信息
        /// </summary>
        /// <param name="EBELN">PO号</param>
        /// <param name="NAME1_P">供应商名称</param>
        /// <param name="MAKTX">物料描述</param>
        /// <param name="KDATB">有效起始日期</param>
        /// <param name="KDATE">有效截止日期</param>
        /// <param name="indexRow">选中下标</param>
        /// <returns>true成功 flase失败</returns>
        public static bool AddPOInfo(string EBELN, string NAME1_P, string MAKTX, string KDATB, string KDATE, int indexRow, DataTable dtMessage)
        {
            string strSAPID = "";
            if (dtMessage != null)
            {
                if (dtMessage.Rows[indexRow]["type"].ToString().ToUpper() == "S")
                {
                    if (KDATB == "" || KDATB == "&nbsp;") KDATB = "1900-01-01 00:00:00.000";
                    if (KDATE == "" || KDATE == "&nbsp;") KDATE = "1900-01-01 00:00:00.000";
                    if (EBELN == "&nbsp;")
                    {
                        EBELN = "";
                    }
                    if (NAME1_P == "&nbsp;")
                    {
                        NAME1_P = "";
                    }
                    if (MAKTX == "&nbsp;")
                    {
                        MAKTX = "";
                    }

                    string strSql = "INSERT INTO [eh_SAPRecord] ([Sap_Type] ,[Sap_InNO],[Sap_InTime],[Sap_OutNAME1P],[Sap_OutMAKTX],[Sap_OutKDATB],[Sap_OutKDATE],[Sap_OutETYPE],[Sap_OutEMSG],[Sap_Identify],Sap_Remark)VALUES('送货车辆','" + EBELN + "',getdate(),'" + NAME1_P + "','" + MAKTX + "','" + KDATB + "','" + KDATE + "','" + dtMessage.Rows[indexRow]["type"].ToString().ToUpper() + "','" + dtMessage.Rows[0]["message"].ToString().ToUpper() + "',',I,','F')";
                    string identity = "select @@identity";
                    object obj = LinQBaseDao.GetSingle(strSql + ";" + identity);
                    if (obj != null)
                    {
                        strSAPID = obj.ToString();
                        CommonalityEntity.SAP_ID = strSAPID;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    string strMessage = "信息验证失败或无此信息";
                    string strSql = "INSERT INTO [eh_SAPRecord] ([Sap_Type] ,[Sap_InNO],[Sap_InTime],[Sap_OutETYPE],[Sap_OutEMSG],Sap_Remark)VALUES('送货车辆','" + CheckProperties.ce.SongHuoNumber + "',getdate(),'" + dtMessage.Rows[indexRow]["type"].ToString().ToUpper() + "','" + dtMessage.Rows[0]["message"].ToString().ToUpper() + "','F')";
                    int rint = LinQBaseDao.ExecuteSql(strSql);
                    if (rint < 1)
                    {
                        strMessage += ",保存验证失败记录失败!";
                    }
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 验证新单号是否有效[SAP验证]
        /// </summary>
        /// <param name="sapType">SAP验证类型</param>
        /// <param name="strPo">单号</param>
        /// <param name="rStr">返回验证信息</param>
        /// <returns></returns>
        public static bool ISSapCheck(string sapType, string strPo, out string rStr)
        {
            bool rbl = true;
            string str = "";

            try
            {
                if (sapType.Trim() == "送货车辆")
                {
                    #region 送货车辆
                    if (string.IsNullOrEmpty(strPo.Trim()))
                    {
                        str += "请输入PO号!" + "\r\n";
                        rbl = false;
                    }
                    else
                    {
                        SAPClass de = new SAPClass();
                        Thread thread = new Thread(new ThreadStart(de.SendGoods));
                        de.LV_EBELN = CheckProperties.ce.SongHuoNumber;
                        thread.SetApartmentState(ApartmentState.STA);
                        thread.Start();
                        thread.Join();
                        #region 测试
                        //// 返回数据
                        //DataTable dts = new DataTable();
                        //DataRow dr = dts.NewRow();
                        //dts.Columns.Add("EBELN");//(PO号)
                        //dts.Columns.Add("NAME1_P");//（供应商名称）
                        //dts.Columns.Add("MAKTX");//（物料描述）
                        //dts.Columns.Add("KDATB");//（有效起始日期）
                        //dts.Columns.Add("KDATE");//（有效截止日期）

                        //dr["EBELN"] = strPo;
                        //dr["NAME1_P"] = "";
                        //dr["MAKTX"] = "123";
                        //dr["KDATB"] = "";
                        //dr["KDATE"] = "2011-07-29";
                        //dts.Rows.Add(dr);

                        //dr = dts.NewRow();
                        //dr["EBELN"] = strPo;
                        //dr["NAME1_P"] = "cc";
                        //dr["MAKTX"] = "1235";
                        //dr["KDATB"] = "2011-07-09";
                        //dr["KDATE"] = "2011-07-29";
                        //dts.Rows.Add(dr);
                        //de.Table1 = dts;

                        //DataTable dtReturn = new DataTable();
                        //DataRow dr2 = dtReturn.NewRow();
                        //dtReturn.Columns.Add("type");
                        //dtReturn.Columns.Add("message");
                        ////成功
                        //dr2["type"] = "S";
                        //dr2["message"] = "成功";
                        //dtReturn.Rows.Add(dr2);

                        //////////失败
                        ////////dr2 = dtReturn.NewRow();
                        ////////dr2["type"] = "S";
                        ////////dr2["message"] = "成功";
                        ////////dtReturn.Rows.Add(dr2);

                        //de.Table2 = dtReturn;
                        #endregion
                        if (de.Table2 != null)
                        {
                            if (de.Table2.Rows[0]["type"].ToString().ToUpper() != "S")
                            {
                                str += "送货车辆PO号SAP校验失败," + de.Table2.Rows[0]["type"].ToString().ToUpper() + de.Table2.Rows[0]["message"].ToString() + "\r\n";
                                rbl = false;
                            }
                        }
                        else
                        {
                            str += "连接失败!";
                            rbl = false;

                        }
                    }

                    #endregion
                }
                else if (sapType.Trim() == "成品车辆")
                {
                    #region 成品车辆
                    if (string.IsNullOrEmpty(strPo.Trim()))
                    {
                        str += "请输入车牌号! " + "\r\n";
                        rbl = false;
                    }
                    else
                    {
                        SAPClass de = new SAPClass();
                        Thread thread = new Thread(new ThreadStart(de.ChengPin));
                        de.LV_CARNO = CheckProperties.ce.ChengPinNumber;
                        thread.SetApartmentState(ApartmentState.STA);
                        thread.Start();
                        thread.Join();

                        if (de.Table2 != null)
                        {
                            if (de.Table2.Rows[0]["type"].ToString().ToUpper() != "S")
                            {
                                str += "成品车辆车牌号SAP校验失败," + de.Table2.Rows[0]["type"].ToString().ToUpper() + de.Table2.Rows[0]["message"].ToString() + "\r\n"; ;
                                rbl = false;
                            }
                        }
                        else
                        {
                            str += "连接失败!";
                            rbl = false;
                        }
                    }
                    #endregion
                }
                else if (strPo == "三废车辆")
                {
                    #region 三废车辆
                    if (string.IsNullOrEmpty(strPo.Trim()))
                    {
                        str += "请输入三废交货单号!" + "\r\n"; ;
                        rbl = false;
                    }
                    else
                    {
                        SAPClass de = new SAPClass();
                        Thread thread = new Thread(new ThreadStart(de.SanFei));
                        de.LV_EBELN = CheckProperties.ce.SongHuoNumber;
                        thread.SetApartmentState(ApartmentState.STA);
                        thread.Start();
                        thread.Join();

                        if (de.Table2 != null)
                        {
                            if (de.Table2.Rows[0]["type"].ToString().ToUpper() != "S")
                            {
                                str += "三废车辆交货单号SAP校验失败," + de.Table2.Rows[0]["type"].ToString().ToUpper() + de.Table2.Rows[0]["message"].ToString() + "\r\n"; ;

                                rbl = false;
                            }
                        }
                        else
                        {
                            str += "连接失败!";
                            rbl = false;


                        }
                    }

                    #endregion
                }
            }
            catch
            {
                str += "验证失败" + "!";
                rbl = false;
            }
            rStr = str;
            return rbl;
        }

        #endregion
        /// <summary>
        /// 添加系统异常记录
        /// </summary>
        /// <param name="inttype">异常类型编号</param>
        /// <param name="type">管控信息异常类型:</param>
        /// <param name="strUnusualRecord_Reason">异常描述</param>
        /// <param name="strUnusualRecord_Operate">异常记录人</param>
        public static UnusualRecord ADDUnusualRecord(int inttype, string type, string strUnusualRecord_Reason, string strUnusualRecord_Operate, int carid)
        {
            UnusualRecord ur = new UnusualRecord();
            try
            {

                ur.UnusualRecord_UnusualType_ID = inttype;
                ur.UnusualRecord_State = "启动";
                ur.UnusualRecord_Type = type;
                ur.UnusualRecord_Reason = strUnusualRecord_Reason;
                ur.UnusualRecord_Operate = strUnusualRecord_Operate;
                ur.UnusualRecord_Time = CommonalityEntity.GetServersTime();
                ur.UnusualRecord_Remark = strUnusualRecord_Reason;
                ur.UnusualRecord_Site = strUnusualRecordTable;
                ur.UnusualRecord_SiteID = intCarInOutInfoRecord_ID;
                ur.UnusualRecord_CarInfo_ID = carid;
                LinQBaseDao.InsertOne<UnusualRecord>(new DCCarManagementDataContext(), ur);
            }
            catch
            {
                WriteTextLog("CommonalityEntity.ADDUnusualRecord()");
            }
            return ur;
        }
        #region checkedListBox
        /// <summary>
        /// checkedListBox操作（0：(GetValue)：获取已选的实际值     1：(GetText)：获取获取已选的文本  2：全选  3:反选  4：取消全选 5:选中其他某些项）
        /// </summary>
        /// <param name="chklb"></param>
        /// <param name="intidex"></param>
        /// <returns></returns>
        public static string checkedListBoxMethod(CheckedListBox chklb, int intidex, List<int> listint)
        {
            string checkedText = string.Empty;
            try
            {
                switch (intidex)
                {
                    case 0://(GetValue)：获取已选的实际值 
                        for (int i = 0; i < chklb.Items.Count; i++)
                        {
                            if (chklb.GetItemChecked(i))
                            {
                                chklb.SetSelected(i, true);
                                checkedText += (String.IsNullOrEmpty(checkedText) ? "" : ",") + chklb.SelectedValue.ToString();
                            }
                        }
                        break;

                    case 1:
                        //(GetText)：获取获取已选的文本   
                        for (int i = 0; i < chklb.Items.Count; i++)
                        {
                            if (chklb.GetItemChecked(i))
                            {
                                checkedText += (String.IsNullOrEmpty(checkedText) ? "" : ",") + chklb.GetItemText(chklb.Items[i]);
                            }
                        }
                        break;
                    case 2://全选
                        for (int i = 0; i < chklb.Items.Count; i++)
                        {
                            chklb.SetItemChecked(i, true);
                        }

                        break;
                    case 3://反选
                        for (int i = 0; i < chklb.Items.Count; i++)
                        {
                            if (chklb.GetItemChecked(i))
                            {
                                chklb.SetItemChecked(i, false);
                            }
                            else
                            {
                                chklb.SetItemChecked(i, true);
                            }
                        }

                        break;

                    case 4://取消全选
                        for (int i = 0; i < chklb.Items.Count; i++)
                        {
                            chklb.SetItemChecked(i, false);
                        }

                        break;
                    case 5://选中某些项(根据下标)
                        if (listint.Count() > 0)
                        {
                            foreach (var tem in listint)
                            {
                                chklb.SetItemChecked(tem, true);
                            }
                        }
                        break;
                    case 6://第几项被选中
                        for (int i = 0; i < chklb.Items.Count; i++)
                        {
                            if (chklb.GetItemChecked(i))
                            {
                                checkedText += (String.IsNullOrEmpty(checkedText) ? "" : ",") + i.ToString();
                            }
                        }

                        break;
                    case 7://选中某些项(根据Value)
                        for (int i = 0; i < chklb.Items.Count; i++)
                        {
                            chklb.SetSelected(i, true);
                            if (listint.Contains(GetInt(chklb.SelectedValue.ToString())))
                            {
                                chklb.SetItemChecked(i, true);
                            }

                        }
                        break;
                }
            }
            catch
            {

                WriteTextLog("CommonalityEntity.JudgeBusinessMethod()");
            }
            return checkedText;
        }
        /// <summary> 
        /// 获取checkedListBox 的 (GetValue)：获取已选的实际值   (GetText)：获取获取已选的文本  返回checkedListBox中第几项被选中 
        /// </summary>
        /// <param name="txt">显示GetText的文本框</param>
        /// <param name="intindex">0：返回  List<int>（GetValue） 1：List<int>（第几项被选中）</param>
        /// <returns></returns>
        public static List<int> BingListMethod(CheckedListBox chklb, TextBox txt, int intindex)
        {
            List<int> listcheckedListBox = new List<int>();

            try
            {
                switch (intindex)
                {
                    case 0:
                        var p = checkedListBoxMethod(chklb, 0, null);
                        if (p != "")
                        {

                            foreach (var tem in p.Split(','))
                            {
                                listcheckedListBox.Add(GetInt(tem.ToString()));
                            }
                        }
                        break;
                    case 2:
                        p = checkedListBoxMethod(chklb, 6, null);
                        if (p != "")
                        {
                            foreach (var tem in p.Split(','))
                            {
                                listcheckedListBox.Add(GetInt(tem.ToString()));
                            }
                        }
                        break;


                }
                txt.Text = checkedListBoxMethod(chklb, 1, null);
            }
            catch
            {

                WriteTextLog("CommonalityEntity.btn_ChklbDetermine_Click()");
            }
            return listcheckedListBox;
        }
        /// <summary>
        ///传入SQL和查询字段返回LIST<int>
        /// </summary>
        /// <param name="strsql">查询语句</param>
        /// <param name="str">要返回的字段（一个）</param>
        /// <returns></returns>
        public static List<int> GetListID(string strsql, string str)
        {
            List<int> listID = new List<int>();
            try
            {
                var p = LinQBaseDao.Query(strsql).Tables[0].AsEnumerable().ToList();
                if (p.Count() > 0)
                {
                    foreach (var tem in p)
                    {
                        if (tem.Field<int>(str) > 0)
                        {
                            listID.Add(GetInt(tem.Field<int>(str).ToString()));
                        }
                    }
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("CommonalityEntity.GetListID()");
            }
            return listID;
        }
        #endregion
        static public string FormatSQLIDs(string ids)
        {
            string[] arryIDs = GetArryIDs(ids);
            string tempStr = "";
            for (int i = 0; i < arryIDs.Length; i++)
                tempStr += arryIDs[i].ToString() + ",";
            if (tempStr != "")
                tempStr = tempStr.Remove(tempStr.Length - 1);
            return tempStr;
        }
        /// <summary>
        /// 截取字符串方法
        /// </summary>
        /// <param name="ids">字符串</param>
        /// <returns></returns>
        static public bool IsValidIDs(string ids)
        {
            if (ids == null) return false;
            ids = ids.Replace(",", "");
            return true;
        }
        /// <summary>
        /// 字符串转换为Int数组
        /// </summary>
        /// <param name="ids">要转换的字符串</param>
        /// <returns></returns>
        static public string[] GetArryIDs(string ids)
        {
            string[] strIDs = ids.Split(',');
            string[] arryIDs = new string[strIDs.Length];
            for (int i = 0; i < strIDs.Length; i++)
                arryIDs[i] = strIDs[i].ToString();
            return arryIDs;
        }

        /// <summary>
        /// 验证IC卡的有效性
        /// </summary>
        /// <param name="ICValue"></param>
        public static string CheckICValue(string ICValue, string ICValueTwo, out string count, out string hour)
        {
            string icid = "";
            DataTable dt;
            count = "";
            hour = "";
            #region 卡一
            if (!string.IsNullOrEmpty(ICValue))
            {
                dt = LinQBaseDao.Query("select * from ICCard where ICCard_Value='" + ICValue + "'").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string ICCard_EffectiveType = dt.Rows[0]["ICCard_EffectiveType"].ToString();
                    string ICCard_count = dt.Rows[0]["ICCard_count"].ToString();
                    string ICCard_HasCount = dt.Rows[0]["ICCard_HasCount"].ToString();
                    string ICCard_State = dt.Rows[0]["ICCard_State"].ToString();
                    icid = dt.Rows[0]["ICCard_ID"].ToString();
                    if (ICCard_EffectiveType == "次数")
                    {
                        int ct = Convert.ToInt32(ICCard_count);
                        int ht = Convert.ToInt32(ICCard_HasCount);
                        if (ct > ht)
                        {
                            count = (ct - ht).ToString();
                        }
                        else
                        {
                            count = "1";
                        }
                    }
                    if (ICCard_EffectiveType == "有效期")
                    {
                        DateTime ICCard_BeginTime = Convert.ToDateTime(dt.Rows[0]["ICCard_BeginTime"].ToString());
                        DateTime ICCard_EndTime = Convert.ToDateTime(dt.Rows[0]["ICCard_EndTime"].ToString());
                        if (CommonalityEntity.GetServersTime() > ICCard_BeginTime && CommonalityEntity.GetServersTime() < ICCard_EndTime)
                        {
                            TimeSpan th = ICCard_EndTime - CommonalityEntity.GetServersTime();
                            int s = Convert.ToInt32(th.TotalHours);
                            if (s > 0)
                            {
                                hour = s.ToString();
                            }
                            else
                            {
                                hour = "1000";
                            }
                        }
                    }
                    if (ICCard_EffectiveType == "永久")
                    {
                        count = "1";
                    }
                }
            }
            #endregion

            #region 卡二
            if (!string.IsNullOrEmpty(ICValueTwo))
            {
                dt = LinQBaseDao.Query(" select * from ICCard where ICCard_Value='" + ICValueTwo + "'").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(count) || !string.IsNullOrEmpty(hour))
                    {
                        string ICCard_EffectiveType = dt.Rows[0]["ICCard_EffectiveType"].ToString();
                        string ICCard_count = dt.Rows[0]["ICCard_count"].ToString();
                        string ICCard_HasCount = dt.Rows[0]["ICCard_HasCount"].ToString();
                        string ICCard_State = dt.Rows[0]["ICCard_State"].ToString();

                        if (ICCard_EffectiveType == "次数")
                        {
                            int ct = Convert.ToInt32(ICCard_count);
                            int ht = Convert.ToInt32(ICCard_HasCount);
                            if (ct > ht)
                            {
                                count = (ct - ht).ToString();
                            }
                            else
                            {
                                count = "1";
                            }
                        }
                        if (ICCard_EffectiveType == "有效期")
                        {
                            DateTime ICCard_BeginTime = Convert.ToDateTime(dt.Rows[0]["ICCard_BeginTime"].ToString());
                            DateTime ICCard_EndTime = Convert.ToDateTime(dt.Rows[0]["ICCard_EndTime"].ToString());
                            if (CommonalityEntity.GetServersTime() > ICCard_BeginTime && CommonalityEntity.GetServersTime() < ICCard_EndTime)
                            {
                                TimeSpan th = ICCard_EndTime - CommonalityEntity.GetServersTime();
                                int s = Convert.ToInt32(th.TotalHours);
                                if (s > 0)
                                {
                                    hour = s.ToString();
                                }
                                else
                                {
                                    hour = "1000";
                                }
                            }
                        }
                        if (ICCard_EffectiveType == "永久")
                        {
                            count = "1";
                        }
                    }
                }
            }
            #endregion

            return icid;
        }

    }
}