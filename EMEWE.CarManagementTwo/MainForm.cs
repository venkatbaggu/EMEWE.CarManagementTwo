using ComUserCtrl;
using EMEWE.CarManagement.CommonClass;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.HelpClass;
using EMEWE.CarManagement.NetSDK;
using EMEWE.CarManagement.PreviewDemo;
using EMEWE.CarManagement.SystemAdmin;
using EMEWE.CarManagementDAL;
using nModbusPLCCtrl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace EMEWE.CarManagement
{
    public partial class MainForm : Form
    {
        /// <summary>
        /// 当前打开的子窗体
        /// </summary>
        public Form OpenForm;
        //public LoginForm login;
        public static bool isVis = true;
        string msTxt = null;
        public static short opDoorPort = -1;
        public bool isOpenYN = false;//万能卡，特权卡刷卡后是否开门（没地感开门，有地感到通行页面）

        public MainForm()
        {
            InitializeComponent();
        }
        #region 窗体位置
        /// <summary>
        /// 获取屏幕的分辨率设置几个按钮的位置
        /// </summary>
        private void GetScreen()
        {
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
        }
        /// <summary>
        /// 当前打开的子窗体
        /// </summary>
        private Point Position = new Point(0, 0);

        /// <summary>
        /// 读取硬件设备定时器是否忙碌
        /// true 忙碌，false 闲置
        /// </summary>
        bool ISBusyTimerDeviceControl = false;
        /// <summary>
        /// 显示子窗体公共方法
        /// </summary>
        /// <param name="childFrm">子窗体</param>
        /// <param name="fFrm">父窗体</param>
        public void ShowChildForm(Form childFrm, Form fFrm)
        {
            this.CloseOpenForm();
            this.OpenForm = childFrm;
            childFrm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            childFrm.Owner = fFrm;
            childFrm.ShowIcon = false;
            childFrm.ShowInTaskbar = false;
            childFrm.StartPosition = FormStartPosition.CenterParent;
            childFrm.Show();
            childFrm.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - childFrm.Width) / 2, 55);
        }
        #endregion
        private void MainForm_Load(object sender, EventArgs e)
        {

            zlHaiweiPLCCommCtrl.Visible = false;
            if (!string.IsNullOrEmpty(SystemClass.Skin))
            {
                this.skinEngine1.SkinFile = "Skip\\" + SystemClass.Skin;
                skinEngine1.DisableTag = 999;
            }
            else
            {
                this.skinEngine1.SkinFile = "Skip\\Office 2007风格.ssk";
            }
            skinEngine1.Active = false;
            toolName.Text = "当前登录账户：" + CommonalityEntity.USERNAME;
            toolmengang.Text = "当前门岗：" + SystemClass.PosistionValue + "号门岗";
            toolError.Text = "";
            groupBox2.Visible = false; 
            Istimers2();
            OpenCOM();
            if (common.ISService)
            {
                CreateMenu();
                bingmenu();
            }
            CreateMenus();//添加重新启动和退出程序按钮
        }

        /// <summary>
        /// IC卡地址(1号机)
        /// </summary>
        public string strCardPortOne;

        /// <summary>
        /// IC卡地址(2号机)
        /// </summary>
        public string strCardPortTo;

        /// <summary>
        /// 读取当前硬件设备数据
        /// </summary>
        private void threadTime()
        {
            if (!ISBusyTimerDeviceControl)
            {
                ISBusyTimerDeviceControl = true;
                listDC = DeviceControlBLL.GetDeviceControl();
                ISBusyTimerDeviceControl = false;
            }
        }


        /// <summary>
        /// 记录刷卡信息
        /// 当刷卡时,将卡的地址码作为键值，卡号作为值存储
        /// 当业务处理完成时，删除当前键值的卡信息
        /// </summary>
        public static Dictionary<string, List<CardEntity>> dicCard = new Dictionary<string, List<CardEntity>>();

        /// <summary>
        /// 插入读卡器信息
        /// </summary>
        /// <param name="strCarNO">卡号</param>
        /// <param name="shport">卡地址</param>
        /// <param name="iDriverwayIport">通道号</param>
        private void CardReadRecord(string strCarNO, string shport, string iDriverwayIport)
        {
            bool isInOut = false;


            string ICCard_ID = "";
            string Driveway_ID = "";
            string Driveway_Address = "";
            string Position_ID = "";
            string StaffInfo_Name = "";
            string CardInfo_ID = "";
            string ICCarType_Name = "";
            string ICCarType_Value = "";
            try
            {
                string strSql = "";
                if (!string.IsNullOrEmpty(shport))  // 根据读卡器地址获取通道信息
                {
                    strSql = String.Format("select * from View_FVN_Driveway_Position where Position_ID={0} and Driveway_ReadCardPort='{1}'  and FVN_Type='拍照' ", SystemClass.PositionID, shport.ToString());
                }
                else if (!string.IsNullOrEmpty(iDriverwayIport)) //根据通道获取通道信息
                {
                    strSql = String.Format("select * from View_FVN_Driveway_Position where Position_ID={0} and Driveway_ReadCardPort='{1}'  and FVN_Type='拍照' ", SystemClass.PositionID, iDriverwayIport);
                }
                if (!string.IsNullOrEmpty(strSql))
                {
                    View_FVN_Driveway_Position vfdp = GetReadCardView_FVN_Driveway_Position(strSql);
                    if (vfdp != null)
                    {
                        if (!string.IsNullOrEmpty(vfdp.Driveway_Value))
                        {
                            List<string> list = GetImageName(vfdp);

                            try
                            {
                                DataTable dtcartype = LinQBaseDao.Query("select ICCard_ID,ICCardType_Value,ICCardType_Name from ICCard,ICCardType where ICCard_ICCardType_ID=ICCardType_ID and ICCardType_State='启动' and ICCard_Value='" + strCarNO.Trim() + "'").Tables[0];
                                //判断卡数据库是否存在卡信息并为启动状态
                                if (dtcartype.Rows.Count > 0)
                                {
                                    ICCarType_Name = dtcartype.Rows[0]["ICCardType_Name"].ToString();
                                    ICCarType_Value = dtcartype.Rows[0]["ICCardType_Value"].ToString();
                                    ICCard_ID = dtcartype.Rows[0]["ICCard_ID"].ToString();

                                    //如果设备控制表有相应数据就修改数据
                                    Position_ID = SystemClass.PositionID.ToString();
                                    Driveway_Address = vfdp.Driveway_Address;
                                    Driveway_ID = vfdp.Driveway_ID.ToString();

                                    if (DeviceControlDAL.GetQuery("select * from dbo.DeviceControl where DeviceControl_PositionValue = '" + SystemClass.PosistionValue + "' and DeviceControl_DrivewayValue='" + vfdp.Driveway_Value + "'").Tables[0].Rows.Count > 0)
                                    {
                                        Expression<Func<DeviceControl, bool>> p = n => n.DeviceControl_PositionValue == SystemClass.PosistionValue && n.DeviceControl_DrivewayValue == vfdp.Driveway_Value;
                                        Action<DeviceControl> ap = s =>
                                        {
                                            s.DeviceControl_CardType = ICCarType_Name;
                                            s.DeviceControl_DrivewayAddress = Driveway_Address;
                                            s.DeviceControl_ReadValue = shport;
                                            s.DeviceControl_OutSate = "无";
                                            s.DeviceControl_CardNo = strCarNO;//卡号
                                            s.DeviceControl_FanValue = vfdp.FVN_Value.ToString();
                                        };
                                        if (DeviceControlDAL.Update(p, ap))
                                        {
                                            toolIcCard.Text = strCarNO + "刷卡成功！" + DateTime.Now.ToString();
                                        }
                                        else
                                            toolIcCard.Text = strCarNO + "刷卡失败，请重新刷卡！" + DateTime.Now.ToString();
                                    }
                                    else
                                    {

                                        //如果设备控制表没有相应数据就添加数据
                                        DeviceControl dc = new DeviceControl();
                                        dc.DeviceControl_PositionValue = SystemClass.PosistionValue;
                                        dc.DeviceControl_DrivewayValue = vfdp.Driveway_Value;
                                        dc.DeviceControl_DrivewayAddress = Driveway_Address;
                                        dc.DeviceControl_OutSate = "无";
                                        dc.DeviceControl_FanSate = "1";
                                        dc.DeviceControl_FanValue = vfdp.FVN_Value.ToString(); ;
                                        dc.DeviceControl_CardNo = strCarNO;
                                        dc.DeviceControl_ReadValue = shport;
                                        dc.DeviceControl_CardType = ICCarType_Name;
                                        dc.DeviceControl_ISCardRelease = true;
                                        DeviceControlDAL.InsertOneDevice(dc);
                                    }
                                    string carno = cartypeUser(strCarNO);
                                    #region 设置读卡信息
                                    CardEntity cardentiyt = new CardEntity();
                                    cardentiyt.CardNo = strCarNO;
                                    cardentiyt.CardTyep = ICCarType_Name;
                                    cardentiyt.CarId = 0;
                                    cardentiyt.CarNo = carno;
                                    cardentiyt.Driveway_ReadCardPort = vfdp.Driveway_ReadCardPort;
                                    cardentiyt.CardCar = bindcar(strCarNO);

                                    //循环该通道内的卡信息集合 如果有同类型的卡信息就删除
                                    if (dicCard.Keys.Contains(vfdp.Driveway_Value))
                                    {
                                        for (int i = 0; i < dicCard[vfdp.Driveway_Value].Count; i++)
                                        {
                                            if (dicCard[vfdp.Driveway_Value][i].CardTyep == cardentiyt.CardTyep)
                                            {
                                                dicCard[vfdp.Driveway_Value].RemoveAt(i);
                                            }
                                        }
                                        if (dicCard[vfdp.Driveway_Value].Count > 3)
                                        {
                                            // MessageBox.Show("当前通道卡信息超过3条，系统将清空，请重新刷卡！");
                                            dicCard[vfdp.Driveway_Value].Clear();
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        dicCard.Add(vfdp.Driveway_Value, new List<CardEntity>());
                                    }

                                    ////添加卡信息到通道卡信息集合
                                    if (CommonalityEntity.CarValue.Count > 0)
                                    {
                                        foreach (var item in CommonalityEntity.CarValue)
                                        {
                                            if (item == vfdp.Driveway_Value)
                                            {
                                                if (CommonalityEntity.isRestFXOne)//只有确认放行之后才添加保安卡、班长卡
                                                {
                                                    dicCard[vfdp.Driveway_Value].Add(cardentiyt);
                                                }
                                                else
                                                {
                                                    if (CommonalityEntity.isRestFXTwo)//只有确认放行之后才添加保安卡、班长卡信息
                                                    {
                                                        dicCard[vfdp.Driveway_Value].Add(cardentiyt);
                                                    }
                                                    else
                                                    {
                                                        if (CommonalityEntity.isRestFXThree)//只有确认放行之后才添加保安卡、班长卡信息
                                                        {
                                                            dicCard[vfdp.Driveway_Value].Add(cardentiyt);
                                                        }
                                                        else
                                                        {
                                                            if (CommonalityEntity.isRestFXFour)//只有确认放行之后才添加保安卡、班长卡信息
                                                            {
                                                                dicCard[vfdp.Driveway_Value].Add(cardentiyt);
                                                            }
                                                            else
                                                            {
                                                                if (CommonalityEntity.isRestFXFive)//只有确认放行之后才添加保安卡、班长卡信息
                                                                {
                                                                    dicCard[vfdp.Driveway_Value].Add(cardentiyt);
                                                                }
                                                                else
                                                                {
                                                                    if (CommonalityEntity.isRestFXSix)//只有确认放行之后才添加保安卡、班长卡信息
                                                                    {
                                                                        dicCard[vfdp.Driveway_Value].Add(cardentiyt);
                                                                    }
                                                                    else
                                                                    {
                                                                        if (ICCarType_Name != "保安卡" && ICCarType_Name != "班长卡")
                                                                        {
                                                                            dicCard[vfdp.Driveway_Value].Add(cardentiyt);
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (ICCarType_Name != "保安卡" && ICCarType_Name != "班长卡")
                                                {
                                                    dicCard[vfdp.Driveway_Value].Add(cardentiyt);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (ICCarType_Name != "保安卡" && ICCarType_Name != "班长卡")
                                        {
                                            dicCard[vfdp.Driveway_Value].Add(cardentiyt);
                                        }
                                    }



                                    #region 添加刷卡记录
                                    string dirName = vfdp.Driveway_Value;
                                    string dirtype = vfdp.Driveway_Type;
                                    string staName = "";
                                    DataTable dtstaff = LinQBaseDao.Query("select StaffInfo_Name from StaffInfo where StaffInfo_ICCard_ID=" + ICCard_ID).Tables[0];
                                    if (dtstaff.Rows.Count > 0)
                                    {
                                        staName = dtstaff.Rows[0][0].ToString();
                                    }
                                    dtstaff = LinQBaseDao.Query("select Car_Name from Car where Car_ICCard_ID=" + ICCard_ID).Tables[0];
                                    if (dtstaff.Rows.Count > 0)
                                    {
                                        if (string.IsNullOrEmpty(staName))
                                        {
                                            staName = dtstaff.Rows[0][0].ToString();
                                        }
                                        else
                                        {
                                            staName += ":" + dtstaff.Rows[0][0].ToString();
                                        }
                                    }
                                    string CISql = "insert into CardInfo(CardInfo_Driveway_ID,CardInfo_ICCard_ID,CardInfo_State,CardInfo_StaName,CardInfo_Type,CardInfo_Time,CardInfo_InOut) values(" + Driveway_ID + "," + ICCard_ID + ",'启动','" + staName + "','" + ICCarType_Name + "',GETDATE(),'" + SystemClass.PositionName + dirName + dirtype + "厂')    select @@identity";
                                    CardInfo_ID = LinQBaseDao.GetSingle(CISql).ToString();
                                    #endregion

                                    if (CommonalityEntity.isRestFXOne)
                                    {
                                        CarInformationFormOne.cinfo.timer_Tick();
                                    }
                                    if (CommonalityEntity.isRestFXTwo)
                                    {
                                        CarInformationFormTwo.cinfo.timer_Tick();
                                    }
                                    if (CommonalityEntity.isRestFXThree)
                                    {
                                        CarInformationFormThree.cinfo.timer_Tick();
                                    }
                                    if (CommonalityEntity.isRestFXFour)
                                    {
                                        CarInformationFormFour.cinfo.timer_Tick();
                                    }
                                    if (CommonalityEntity.isRestFXFive)
                                    {
                                        CarInformationFormFive.cinfo.timer_Tick();
                                    }
                                    if (CommonalityEntity.isRestFXSix)
                                    {
                                        CarInformationFormSix.cinfo.timer_Tick();
                                    }

                                    #endregion

                                }
                                //万能卡，特权卡刷卡后是否开门（没地感开门，有地感到通行页面）
                                if (!CameraCheck(vfdp.Driveway_Value))
                                {
                                    object objaddress = vfdp.Driveway_Address;
                                    //万能卡
                                    if (ICCarType_Value == "1010")
                                    {
                                        OpenDoor(Common.Converter.ToShort(objaddress.ToString()));//起杆 
                                        isInOut = true;
                                    }
                                    //特权卡
                                    if (ICCarType_Value == "1011")
                                    {
                                        OpenDoor(Common.Converter.ToShort(objaddress.ToString()));//起杆 
                                        isInOut = true;
                                    }
                                }
                                else
                                {
                                    bool isin = false;
                                    if (CommonalityEntity.CarValue.Count > 0)
                                    {
                                        foreach (var item in CommonalityEntity.CarValue)
                                        {
                                            if (item == vfdp.Driveway_Value)
                                            {
                                                isin = true;
                                            }
                                        }
                                    }
                                    if (!isin)//为TRUE是，通道下有车，进入放行页面开闸，false直接开闸
                                    {
                                        object objaddress = vfdp.Driveway_Address;
                                        //万能卡
                                        if (ICCarType_Value == "1010")
                                        {
                                            OpenDoor(Common.Converter.ToShort(objaddress.ToString()));//起杆 
                                            isInOut = true;
                                        }
                                        //特权卡
                                        if (ICCarType_Value == "1011")
                                        {
                                            OpenDoor(Common.Converter.ToShort(objaddress.ToString()));//起杆 
                                            isInOut = true;
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                common.WriteTextLog("MainFrom.CardReadRecord:" + ex.Message.ToString());
                            }
                            finally
                            {
                                #region
                                //如果是特权卡或者万能卡直接开门就删除该通道读卡信息
                                if (isInOut)
                                {
                                    isInOut = false;

                                    dicCard[vfdp.Driveway_Value].Clear();//删除读卡信息

                                    //驾驶员编号
                                    DataTable dt = LinQBaseDao.Query("select StaffInfo_Name  from StaffInfo where StaffInfo_ICCard_ID = (select ICCard_ID from ICCard where ICCard_Value='" + strCarNO + "')").Tables[0];
                                    if (dt.Rows.Count > 0)
                                    {
                                        //刷卡人名称
                                        StaffInfo_Name = dt.Rows[0][0].ToString();
                                    }
                                    //照片上传
                                    string path = SystemClass.BaseFile;
                                    string stryear = "Car" + SystemClass.PosistionValue + vfdp.Driveway_Value + "\\" + CommonalityEntity.GetServersTime().Year + "\\" + CommonalityEntity.GetServersTime().ToString("yyyyMMdd") + "\\";
                                    string uppath = ImageFile.GetStrImageFile(true, vfdp.Driveway_Value);
                                    if (list.Count != 0)//照片名称
                                    {
                                        int i = 1;
                                        foreach (var item in list)
                                        {
                                            //上传图片到服务器
                                            ImageFile.UpLoadFile(uppath + item, SystemClass.SaveFile + stryear);
                                            if (i == 1)
                                            {
                                                LinQBaseDao.Query("update CardInfo set CardInfo_StaName='" + StaffInfo_Name + "', CardInfo_Pic='" + stryear + item + "' where CardInfo_ID=" + CardInfo_ID);
                                            }
                                            if (i == 2)
                                            {
                                                LinQBaseDao.Query("update CardInfo set CardInfo_Remark='" + stryear + item + "' where CardInfo_ID=" + CardInfo_ID);
                                            }
                                            ImageFile.Delete(uppath + item);//删除图片
                                            i++;
                                        }
                                        list.Clear();
                                    }
                                }
                                #endregion
                            }

                        }

                    }

                }
            }
            catch (Exception ext)
            {
                common.WriteTextLog("MainFrom.CardReadRecord:" + ext.Message.ToString());
            }
        }

        /// <summary>
        /// 判断卡号是人卡还是车卡
        /// </summary>
        /// <param name="ICCardNum"></param>
        private string cartypeUser(string ICCardNum)
        {
            string str = "";
            DataTable dt;
            dt = LinQBaseDao.Query("select Car_ID from Car where Car_ICCard_ID in (select ICCard_ID from ICCard where  ICCard_Value='" + ICCardNum + "' and ICCard_State='启动')").Tables[0];
            if (dt.Rows.Count > 0)
            {
                str = "车卡";
            }
            dt = LinQBaseDao.Query("select StaffInfo_ID from StaffInfo where StaffInfo_ICCard_ID in (select ICCard_ID from ICCard where  ICCard_Value='" + ICCardNum + "' and ICCard_State='启动')").Tables[0];
            if (dt.Rows.Count > 0)
            {
                str += "人卡";
            }
            return str;
        }
        /// <summary>
        /// 绑定车牌号
        /// </summary>
        /// <param name="icnum">IC卡号</param>
        private string bindcar(string icnum)
        {
            string carno = "";
            DataTable dt;
            try
            {
                dt = LinQBaseDao.Query(" select  Car_Name from Car where  Car_ICCard_ID in (select ICCard_ID from ICCard where ICCard_Value='" + icnum + "')").Tables[0];
                carno = dt.Rows[0]["Car_Name"].ToString();
            }
            catch
            {
                carno = "";
            }
            return carno;
        }
        /// <summary>
        /// 地感校验
        /// </summary>
        /// <returns></returns>
        private bool CameraCheck(string Driveway_Value)
        {
            bool istrue = true;
            try
            {
                string strsql = String.Format("select * from DeviceControl where DeviceControl_positionValue='" + SystemClass.PosistionValue + "' and DeviceControl_DrivewayValue='" + Driveway_Value + "'");
                List<DeviceControl> list = LinQBaseDao.GetItemsForListing<DeviceControl>(strsql).ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        if (item.DeviceControl_FanSate != "1")
                        {
                            return istrue = false;
                        }
                    }
                }
                else
                {
                    return istrue = false;
                }
            }
            catch
            {
                istrue = false;
                CommonalityEntity.WriteTextLog("CameraCheck()" + "".ToString());
            }
            return istrue;
        }
        /// <summary>
        /// 得到图片名称
        /// </summary>
        /// <param name="vfdp">包含通道编号的视图</param>
        /// <returns></returns>
        private static List<string> GetImageName(View_FVN_Driveway_Position vfdp)
        {
            List<string> list = new List<string>();
            try
            {
                common.CurrentChannelNumber = vfdp.Driveway_Value;//通道Value值
                string strImageFile = ImageFile.GetStrImageFile(true, vfdp.Driveway_Value.ToString());
                if (Directory.Exists(strImageFile))
                {
                    string[] images = Directory.GetFiles(strImageFile);

                    double doubleMin = common.GetDouble(60);//过期时间

                    var image = from m in images
                                orderby m descending
                                select m;
                    //根据通道编号得到该通道下的摄像机名称
                    DataSet camera = LinQBaseDao.Query("select * from Camera where Camera_Driveway_id=" + vfdp.Driveway_ID + "");
                    foreach (DataRow dr in camera.Tables[0].Rows)
                    {
                        string camera_addCard = dr["Camera_AddCard"].ToString();
                        foreach (string str in image)
                        {
                            string fileName = str.Substring(str.LastIndexOf('\\') + 1);//图片名称
                            string mi = ImageFile.GetTime(fileName.ToString());//获取时间差
                            if (double.Parse(mi) <= doubleMin)//判断是否过期
                            {
                                if (fileName.Length == 21)//判断图片名称字符长度
                                {
                                    if (dr["Camera_AddCard"].ToString() == fileName.Substring(16, 1))
                                    {
                                        list.Add(fileName);
                                        break;
                                    }
                                }
                                else if (fileName.Length == 22)
                                {
                                    if (dr["Camera_AddCard"].ToString() == fileName.Substring(16, 2))
                                    {
                                        list.Add(fileName);
                                        break;
                                    }
                                }
                            }
                        }
                        if (list.Count == camera.Tables[0].Rows.Count)
                        {
                            break;
                        }
                    }
                }
            }
            catch
            {
                common.WriteTextLog("MainFrom.GetImageName:" + "");
            }
            return list;
        }

        /// <summary>
        /// 删除卡信息列表中的指定地址的卡号
        /// </summary>
        /// <param name="strKey">卡的地址码</param>
        public void RemoveCard(string strKey)
        {
            dicCard.Remove(strKey);
        }

        /// <summary>
        /// 定时器验证排队呼叫时间是否过期
        /// </summary>
        private static void ChkSortNumber()
        {
            try
            {
                DataTable dt = LinQBaseDao.Query("select SmallTicket_CarInfo_ID,SortNumberInfo_ID,SortNumberInfo_CallTime  from SmallTicket,SortNumberInfo where SmallTicket_ID=SortNumberInfo_SmallTicket_ID and SortNumberInfo_TongXing='待通行' and SmallTicket_SortNumber <>'' and SortNumberInfo_ISEffective=1").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    DataTable dtc = LinQBaseDao.Query("select ControlInfo_Value from ControlInfo where  ControlInfo_Rule='ValTime'").Tables[0];
                    if (dtc.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string cid = dt.Rows[i][0].ToString();
                            string sid = dt.Rows[i][1].ToString();
                            DateTime stime = Convert.ToDateTime(dt.Rows[i][2].ToString());
                            if (stime.ToString() != "1900-01-01 00:00:00.000" && stime.ToString() != "")
                            {
                                TimeSpan ts = CommonalityEntity.GetServersTime() - stime;
                                int met = Convert.ToInt32(ts.TotalMinutes);

                                if (!string.IsNullOrEmpty(dtc.Rows[0][0].ToString()))
                                {
                                    int it = Convert.ToInt32(dtc.Rows[0][0].ToString());
                                    if (met > it)
                                    {
                                        LinQBaseDao.Query("update SortNumberInfo set  SortNumberInfo_TongXing='已注销' where SortNumberInfo_ID= " + sid);
                                        CommonalityEntity.ADDUnusualRecord(2, "呼叫进厂超时", "呼叫后进厂超时，车辆被注销", CommonalityEntity.USERNAME, Convert.ToInt32(cid));
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("MainForm.ChkSortNumber()异常：" + "".ToString());
            }
        }

        private void CreateMenus()
        {
            ToolStripMenuItem topMenu = new ToolStripMenuItem();
            ToolStripMenuItem topMenus = new ToolStripMenuItem();
            topMenu.Text = "重新启动";
            topMenus.Text = "退出系统";
            menuStrip.MdiWindowListItem = topMenu;
            menuStrip.Items.Add(topMenu);
            topMenu.Click += new EventHandler(MyAfterSelected);
            menuStrip.Items.Add(topMenus);
            topMenus.Click += new EventHandler(MyAfterSelected);
        }
        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.notifyIcon1.Visible = false;
            GC.Collect();
            Application.Exit();
            Application.ExitThread();
            Process.GetCurrentProcess().Kill();
            Environment.Exit(Environment.ExitCode);
            Application.ExitThread();
        }
        /// <summary>
        /// 1号机
        /// </summary>
        private CComUserCtrl myComUserCtrl = new CardReaderCtrl();
        /// <summary>
        /// 2号机
        /// </summary>
        private CComUserCtrl myComUserCtr2 = new CardReaderCtrl();
        /// <summary>
        /// 二号串口状态
        /// </summary>
        private bool isComRunningOne = false;

        /// <summary>
        /// 二号串口状态
        /// </summary>
        private bool isComRunningTo = false;                                //串口状态
        /// <summary>
        /// 打开串口
        /// </summary>
        public void OpenCOM()
        {
            //SystemClass.PLCCom = Convert.ToInt32(ConfigurationManager.AppSettings["PCL"]);
            //打开PLC
            if (CheckCom("COM" + SystemClass.PLCCom))
            {
                OpenPLCom(SystemClass.PLCCom);///端口号
            }

            ///检测串口是否可用(1号机)
            if (CheckCom("COM" + SystemClass.CardReadComOne))
            {
                OpenReadCardComOne(SystemClass.CardReadComOne);
            }
            else
            {
                toolCardReadCom.Text = "1号串口不可用!";
            }

            ///检测串口是否可用(2号机)
            if (CheckCom("COM" + SystemClass.CardReadComTwo))
            {
                OpenReadCardCom(SystemClass.CardReadComTwo);
            }
            else
                toolCardReadCom.Text += "2号串口不可用!";
            //硬盘录像机
            //InitSDK();
        }


        private ZLHaiweiDeviceNetOpr _zlHaiweiNetOpr = new ZLHaiweiDeviceNetOpr();
        private string _sHost = "";
        private List<string> _sModuleList = new List<string>();
        /// <summary>
        /// 打开PLC串口
        /// </summary>
        private void OpenPLCom(int com)
        {
            try
            {
                _sModuleList.Clear();
                _sHost = "h_16_16";
                _zlHaiweiNetOpr.CreateZlHaiweiDeviceNet(this._sHost, this._sModuleList);
                int DIAmout = _zlHaiweiNetOpr.GetDeviceNetInputAmout();
                int DOAmount = _zlHaiweiNetOpr.GetDeviceNetOutputAmout();
                zlHaiweiPLCCommCtrl.SetPlcCommAttribute(com, DIAmout, DOAmount);
                zlHaiweiPLCCommCtrl.StartComm();
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 检查串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public bool CheckCom(string coms)
        {
            SerialPort sp = new SerialPort(coms);
            // string port = null; port = Console.ReadLine();
            try
            {
                sp.Close();
                sp.Open();
                Thread.Sleep(100); sp.Close();
                Console.WriteLine("端口尚未打开");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("端口已打开");
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        public void CloseCOM()
        {
            //关闭
            zlHaiweiPLCCommCtrl.EndComm();

            CloseReadCardCom();
            //关闭硬盘录像机
            CloseSDK();
        }
        /// <summary>
        /// 关闭SDK
        /// </summary>
        public void CloseSDK()
        {
            SDKCommon.CloseSDK();
            SDKCommonTwo.CloseSDK();
        }

        /// <summary>
        /// 刷卡2号机
        /// </summary>
        /// <param name="comPort"></param>
        void OpenReadCardCom(int comPort)
        {
            if (!isComRunningTo)
            {
                if (myComUserCtr2.Open(comPort) == 1)     //打开串口,开始采集数据
                {
                    isComRunningTo = true;                  //设置状态
                }
            }
            else
            {
                myComUserCtr2.Stop();    //暂停采集
                isComRunningTo = false;    //设置状态
            }
        }
        /// <summary>
        /// 刷卡1号机
        /// </summary>
        /// <param name="comPort"></param>
        void OpenReadCardComOne(int comPort)
        {
            if (!isComRunningOne)
            {
                if (myComUserCtrl.Open(comPort) == 1)     //打开串口,开始采集数据
                {
                    isComRunningOne = true;                  //设置状态
                }
            }
            else
            {
                myComUserCtrl.Stop();    //暂停采集
                isComRunningOne = false;    //设置状态
            }
        }

        /// <summary>
        /// 刷卡2号机
        /// </summary>
        void CloseReadCardCom()
        {
            if (isComRunningTo)
            {
                isComRunningTo = false;
                myComUserCtrl.Close();     //关闭总线, 在系统退出的时候执行   
            }
        }

        /// <summary>
        /// 加载SDK
        /// </summary>
        public void InitSDK()
        {
            SDKCommonTwo.InitSDK();
            if (SDKCommon.InitSDK() == false)
            {
                toolError.Text += "NET_DVR_Init error!";
                return;
            }
            else
            {
                SetLogin();
            }
        }
        /// <summary>
        /// 登录硬盘录像机
        /// </summary>
        public void SetLogin()
        {
            if (SDKCommon.SetLogin(SystemClass.DVRIPAddress, SystemClass.DVRPortNumber, SystemClass.DVRUserName, SystemClass.DVRPassword) == -1)
            {
                toolError.Text += "硬盘录像机一登录错误!";
                return;
            }
            if (SDKCommonTwo.SetLogin(SystemClass.DVRIPAddressTwo, SystemClass.DVRPortNumberTwo, SystemClass.DVRUserNameTwo, SystemClass.DVRPasswordTwo) == -1)
            {
                toolError.Text += "硬盘录像机二登录错误!";
                return;
            }
        }

        public static List<DeviceControl> listDC = null; //new List<DeviceControl>();

        /// <summary>
        /// 显示工具提示  徐东冬
        /// </summary>
        /// <param name="tti">ToolTipIcon.Info、None、Warning、Error</param>
        /// <param name="str">工具提示标题（ToolTipTitle）</param>
        /// <param name="strMessage">提示信息</param>
        /// <param name="controlName">提示框显示的控件</param>
        /// <param name="form">提示框显示的窗体</param>
        //public void ShowToolTip(ToolTipIcon tti, string strTitle, string strMessage, Control controlName, Form form)
        //{
        //    if (!form.IsDisposed)
        //    {
        //        toolTip1.ToolTipIcon = tti; //ToolTipIcon.Error;
        //        toolTip1.ToolTipTitle = strTitle;
        //        Point showLocation = new Point(
        //            controlName.Location.X + controlName.Width,
        //            controlName.Location.Y);
        //        toolTip1.Show(strMessage, form, showLocation, 5000);
        //        //controlName.SelectAll();
        //        controlName.Focus();
        //    }
        //}

        /// <summary>
        /// 关闭当前打开的窗体  徐东冬  
        /// </summary>
        public void CloseOpenForm()
        {
            if (OpenForm != null)
                OpenForm.Close();
        }


        private void CreateMenu()
        {
            string sql = "";
            if (CommonalityEntity.USERNAME == "emewe")
            {
                sql = "select * from MenuInfo where  Menu_ControlType!=6 and menu_Type=1  order by menu_Order ";
            }
            else
            {
                sql = "select distinct(Permissions_Menu_ID),Menu_ID,Menu_ControlText,menu_Order,menu_otherid,Menu_Visible,Menu_FromName  from View_MenuInfo_P where (userid=" + CommonalityEntity.USERID + " or Permissions_Role_Id=" + CommonalityEntity.USERRoleID + ") and  Menu_ControlType!=6 and Menu_FromName !='PCountSetForm' and Menu_Type=1 order by menu_Order ";
            }
            DataSet dataset = LinQBaseDao.Query(sql);
            //定义一个主菜单  
            //从XML中读取数据。数据结构后面详细讲一下。  
            DataView dv = dataset.Tables[0].DefaultView;
            //通过DataView来过滤数据首先得到最顶层的菜单  
            dv.RowFilter = "menu_otherid=0";
            for (int i = 0; i < dv.Count; i++)
            {
                if (dv[i]["Menu_Visible"].ToString() == "True")
                {
                    //创建一个菜单项
                    ToolStripMenuItem topMenu = new ToolStripMenuItem();
                    //给菜单赋Text值。也就是在界面上看到的值。  
                    topMenu.Text = dv[i]["Menu_ControlText"].ToString();
                    topMenu.Tag = dv[i]["Menu_ID"].ToString();
                    //如果是有下级菜单则通过CreateSubMenu方法来创建下级菜单 
                    //一级菜单没有上级 menu_otherid值为0
                    if (Convert.ToInt32(dv[i]["menu_otherid"]) == 0)
                    {
                        //以ref的方式将顶层菜单传递参数，因为他可以在赋值后再回传。－－也许还有更好的方法^_^.  
                        CreateSubMenu(ref topMenu, Convert.ToInt32(dv[i]["Menu_ID"]), dataset.Tables[0]);
                    }
                    //显示应用程序中已打开的 MDI 子窗体列表的菜单项  
                    menuStrip.MdiWindowListItem = topMenu;
                    //将递归附加好的菜单加到菜单根项上。  
                    menuStrip.Items.Add(topMenu);
                }
            }
            menuStrip.Dock = DockStyle.Top;
            //将窗体的MainMenuStrip梆定为mainMenu.  
            this.MainMenuStrip = menuStrip;
            //这句很重要。如果不写这句菜单将不会出现在主窗体中。  
            this.Controls.Add(menuStrip);
        }

        private void CreateSubMenu(ref ToolStripMenuItem topMenu, int ItemID, DataTable dt)
        {
            DataView dv = new DataView(dt);
            //过滤出当前父菜单下在所有子菜单数据(仅为下一层的)  
            dv.RowFilter = "menu_otherid=" + ItemID.ToString();

            for (int i = 0; i < dv.Count; i++)
            {
                if (dv[i]["Menu_Visible"].ToString() == "True")
                {
                    //创建子菜单项  
                    ToolStripMenuItem subMenu = new ToolStripMenuItem();
                    subMenu.Text = dv[i]["Menu_ControlText"].ToString();
                    subMenu.Tag = dv[i]["Menu_ID"].ToString();

                    //如果还有子菜单则继续递归加载。  
                    if (!string.IsNullOrEmpty(dv[i]["Menu_FromName"].ToString()))
                    {
                        if (dv[i]["Menu_Visible"].ToString() == "True")
                        {
                            //递归调用  
                            subMenu.Click += new EventHandler(MyAfterSelected);
                            CreateSubMenu(ref subMenu, Convert.ToInt32(dv[i]["Menu_ID"]), dt);
                        }
                    }
                    else
                    {
                        if (dv[i]["Menu_Visible"].ToString() == "True")
                        {
                            //扩展属性可以加任何想要的值。这里用formName属性来加载窗体。
                            CreateSubMenu(ref subMenu, Convert.ToInt32(dv[i]["Menu_ID"]), dt);
                        }
                    }
                    topMenu.DropDownItems.Add(subMenu);
                }
            }
        }
        /// <summary>
        /// 为treeNode添加事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyAfterSelected(object sender, EventArgs e)
        {

            object obj = null;
            int msTag;
            try
            {
                msTag = Convert.ToInt32((sender as ToolStripMenuItem).Tag.ToString());
            }
            catch
            {
                msTag = 0;
            }
            msTxt = (sender as ToolStripMenuItem).Text.Trim();

            if (msTxt == "重新启动")
            {
                DialogResult result = MessageBox.Show(this, "确定重新启动?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    Process.Start(Assembly.GetExecutingAssembly().Location);
                    Application.Restart();
                }
            }
            if (msTxt == "退出系统")
            {
                notifyIcon1.Visible = false;
                Application.Exit();
            }
         
            //根据编号查询该编号对应的名称
            if (msTag != 0)
            {
                if (!common.ISService)
                {
                    MessageBox.Show("请检查网络！");
                    return;
                }
                string sql = "Select Menu_FromName from MenuInfo where Menu_ID=" + msTag + "";
                obj = LinQBaseDao.GetSingle(sql);
                if (obj != null)
                {
                    CreateFormInstance(obj.ToString());//主界面菜单点击打开新窗体
                }
            }
        }
        /// <summary>
        /// 节点事件，跳转到对应的页面
        /// </summary>
        /// <param name="formName"></param>
        /// <param name="id"></param>
        private void CreateFormInstance(string formName)
        {
            bool flag = false;
            //遍历主窗口上的所有子菜单
            for (int i = 0; i < Application.OpenForms.Count; i++)
            {
                //如果所点的窗口被打开则重新激活
                if (Application.OpenForms[i].Name.ToLower() == formName.ToLower())
                {
                    Application.OpenForms[i].Activate();
                    Application.OpenForms[i].Show();
                    Application.OpenForms[i].WindowState = FormWindowState.Normal;
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                try
                {
                    //如果不存在则用反射创建form窗体实例。
                    object frmObj = null;
                    frmObj = Activator.CreateInstance(Type.GetType("EMEWE.CarManagement.SystemAdmin." + formName));

                    // object frmObj = asm.CreateInstance("Kimbanx.SecurityDiskSystem.FactoryTools." + formName);//程序集+form的类名。
                    Form frms = (Form)frmObj;
                    //tag属性要重新写一次，否则在第二次的时候取不到。原因还不清楚。有知道的望告知。
                    frms.Tag = formName.ToString();
                    //  frms.Show();
                    this.ShowChildForm(frms, this);
                }
                catch
                {
                    try
                    {
                        //如果不存在则用反射创建form窗体实例。
                        object frmObj = null;
                        Type typeofControl = null;
                        Assembly tempAssembly;
                        tempAssembly = Assembly.LoadFrom("EMEWE.CarManagement.Commons.dll");
                        typeofControl = tempAssembly.GetType("EMEWE.CarManagement.Commons.FlrCommon." + formName);
                        //string str = tempAssembly.FullName;
                        //object result = Activator.CreateInstance(typeofControl);
                        frmObj = Activator.CreateInstance(typeofControl);
                        // object frmObj = asm.CreateInstance("Kimbanx.SecurityDiskSystem.FactoryTools." + formName);//程序集+form的类名。
                        Form frms = (Form)frmObj;
                        //tag属性要重新写一次，否则在第二次的时候取不到。原因还不清楚。有知道的望告知。
                        frms.Tag = formName.ToString();
                        //this.IsMdiContainer = true;
                        //frms.MdiParent = this;
                        //frms.Show();
                        this.ShowChildForm(frms, this);
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void MainFormss_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (msTxt != "重新启动")
            {
                DialogResult result = MessageBox.Show(this, "确定要退出系统?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    CloseCOM();
                    e.Cancel = false;
                    this.notifyIcon1.Visible = false;
                    GC.Collect();
                    Application.Exit();
                    Application.ExitThread();
                    Process.GetCurrentProcess().Kill();
                    System.Environment.Exit(System.Environment.ExitCode);
                    Application.ExitThread();
                }
                else
                {
                    e.Cancel = true;
                    notifyIcon1.Visible = false;
                }
            }
        }

        private void MainFormss_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                notifyIcon1.Visible = true;
                this.Hide();
                this.ShowInTaskbar = false;
                notifyIcon1.ShowBalloonTip(100, "提示", "系统已最小化至托盘", ToolTipIcon.Info);
            }
        }

        private void MainFormss_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)//最小化
            {
                this.ShowInTaskbar = false;
            }
        }
        /// <summary>
        /// 开闸关闸是否空闲
        /// True:空闲
        /// </summary>
        bool DoorBuys = true;
        /// <summary>
        /// 打开通道
        /// </summary>
        /// <param name="shPort">通道地址 >=0</param>
        public void OpenDoor(short shPort)
        {
            try
            {
                zlHaiweiPLCCommCtrl.OpenDoor(shPort);
                Thread.Sleep(200);
                zlHaiweiPLCCommCtrl.CloseDoor(shPort);
                CommonalityEntity.WriteTextLog("打开PLC地址：" + shPort);
            }
            catch
            {
                CommonalityEntity.WriteTextLog("OpenDoor()开门异常：" + "");
            }
        }

        /// <summary>
        /// PLC打开
        /// </summary>
        /// <param name="shPort">通道地址 >=0</param>
        public void OpenPLCDoor(short shPort)
        {
            zlHaiweiPLCCommCtrl.OpenDoor(shPort);
        }
        /// <summary>
        /// PLC关闭
        /// </summary>
        /// <param name="shPort">通道地址 >=0</param>
        public void ClosePLCDoor(short shPort)
        {
            zlHaiweiPLCCommCtrl.CloseDoor(shPort);
        }

        Int32 ilChannel = 1;
        #region 手动控制 事件

        /// <summary>
        /// 手动控制道闸开
        /// </summary>
        /// <param name="drivalue"></param>
        /// <param name="address"></param>
        private void OpenConPLC(string drivalue, short address)
        {
            if (common.ISService)
            {
                DataTable dt = LinQBaseDao.Query("select Driveway_Address from Driveway where Driveway_Value='" + drivalue + "' and  Driveway_Position_ID=" + SystemClass.PositionID).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string oper = dt.Rows[0][0].ToString();
                    if (!string.IsNullOrEmpty(oper))
                    {
                        short ii = Convert.ToInt16(oper);
                        OpenDoor(ii);
                        CommonalityEntity.WriteLogData("手动开闸", "手动开启" + SystemClass.PositionName + "道闸" + drivalue, CommonalityEntity.USERNAME);
                    }
                    else
                    {
                        InsertRecord("手动开启" + SystemClass.PositionName + "道闸" + drivalue, "", address);
                    }
                }
                else
                {
                    InsertRecord("手动开启" + SystemClass.PositionName + "道闸" + drivalue, "", address);
                }
            }
            else
            {
                InsertRecord("手动开启" + SystemClass.PositionName + "道闸" + drivalue, "", address);
            }
        }

        /// <summary>
        /// 手动控制道闸关
        /// </summary>
        /// <param name="drivalue"></param>
        /// <param name="address"></param>
        private void CloseConPLC(string drivalue, short address)
        {
            if (common.ISService)
            {
                DataTable dt = LinQBaseDao.Query("select Driveway_CloseAddress from Driveway where Driveway_Value='" + drivalue + "' and  Driveway_Position_ID=" + SystemClass.PositionID).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string oper = dt.Rows[0][0].ToString();
                    if (!string.IsNullOrEmpty(oper))
                    {
                        short ii = Convert.ToInt16(oper);
                        OpenDoor(ii);
                        CommonalityEntity.WriteLogData("手动开闸", "手动关闭" + SystemClass.PositionName + "道闸" + drivalue, CommonalityEntity.USERNAME);
                    }
                    else
                    {
                        InsertRecord("手动关闭" + SystemClass.PositionName + "道闸" + drivalue, "", address);
                    }
                }
                else
                {
                    InsertRecord("手动关闭" + SystemClass.PositionName + "道闸" + drivalue, "", address);
                }
            }
            else
            {
                InsertRecord("手动关闭" + SystemClass.PositionName + "道闸" + drivalue, "", address);
            }
        }
        private void btnOpen1_Click(object sender, EventArgs e)
        {
            OpenConPLC("01", ControlPLC.OpenAddressOne);
        }
        private void btnClose1_Click(object sender, EventArgs e)
        {
            CloseConPLC("01", ControlPLC.CloseAddressOne);
        }

        public void btnOpen2_Click(object sender, EventArgs e)
        {
            OpenConPLC("02", ControlPLC.OpenAddressTwo);
        }
        private void btnClose2_Click(object sender, EventArgs e)
        {
            CloseConPLC("02", ControlPLC.CloseAddressTwo);
        }

        private void btnOpen3_Click(object sender, EventArgs e)
        {
            OpenConPLC("03", ControlPLC.OpenAddressThree);
        }
        private void btnClose3_Click(object sender, EventArgs e)
        {
            CloseConPLC("03", ControlPLC.CloseAddressThree);
        }

        private void btnOpen4_Click(object sender, EventArgs e)
        {
            OpenConPLC("04", ControlPLC.OpenAddressFour);
        }
        private void btnClose4_Click(object sender, EventArgs e)
        {
            CloseConPLC("04", ControlPLC.CloseAddressFour);
        }

        private void btnOpen5_Click(object sender, EventArgs e)
        {
            OpenConPLC("05", ControlPLC.OpenAddressFive);
        }
        private void btnClose5_Click(object sender, EventArgs e)
        {
            CloseConPLC("05", ControlPLC.CloseAddressFive);
        }

        private void btnOpen6_Click(object sender, EventArgs e)
        {
            OpenConPLC("06", ControlPLC.OpenAddressSix);
        }
        private void btnClose6_Click(object sender, EventArgs e)
        {
            CloseConPLC("06", ControlPLC.CloseAddressSix);
        }

        /// <summary>
        /// 手动拍照
        /// </summary>
        /// <param name="drivalue"></param>
        /// <param name="fvnvalue"></param>
        private void FANPic(string drivalue, int fvnvalue)
        {
            try
            {
                if (common.ISService)
                {
                    DataTable dt = LinQBaseDao.Query("select FVN_Value from View_FVN_Driveway_Position where Position_ID=" + SystemClass.PositionID + " and Driveway_Value='" + drivalue + "'and  FVN_Type='拍照'").Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        fvnvalue = Convert.ToInt32(dt.Rows[0][0].ToString());
                    }
                    ISPhotographFvnAndPhotograph(fvnvalue);
                }
            }
            catch (Exception)
            {

            }
        }

        private void btnPic1_Click(object sender, EventArgs e)
        {
            FANPic("01", 1);
        }

        private void btnPic2_Click(object sender, EventArgs e)
        {
            FANPic("02", 2);
        }

        private void btnPic3_Click(object sender, EventArgs e)
        {
            FANPic("03", 3);
        }

        private void btnPic4_Click(object sender, EventArgs e)
        {
            FANPic("04", 4);
        }

        private void btnPic5_Click(object sender, EventArgs e)
        {
            FANPic("05", 5);
        }

        private void btnPic6_Click(object sender, EventArgs e)
        {
            FANPic("06", 6);
        }
        #endregion
        /// <summary>
        /// 如果是拍照地感，调用拍照
        /// </summary>
        /// <param name="iport">通道值</param>
        /// <param name="iloop">地感值</param>
        private void ISPhotographFvnAndPhotograph(int iport)
        {
            try
            {
                Expression<Func<View_FVN_Driveway_Position, bool>> fun = n => n.Position_ID == SystemClass.PositionID && n.FVN_Value == iport && n.FVN_Type == "拍照";
                int idrivewayId = 0;
                #region 通道值已经设置为唯一值
                if (View_FVN_Driveway_PositionDAL.ISPhotographFVN(fun, out idrivewayId))
                {
                    Expression<Func<View_Camera_Driveway_Position, bool>> fun2 = r => r.Position_ID == SystemClass.PositionID && r.Driveway_ID == idrivewayId && (r.Camera_State == "启动" || r.Camera_State == null);
                    List<View_Camera_Driveway_Position> list = View_Camera_Driveway_PositionDAL.GetList(fun2);

                    if (list.Count > 0)
                    {

                        string strPosition = "";
                        string strDriveway = "";
                        string DVRName = "";
                        string strfileName = "";

                        foreach (View_Camera_Driveway_Position vcdp in list)
                        {
                            strPosition = vcdp.Position_Value;
                            strDriveway = vcdp.Driveway_Value;
                            DVRName = vcdp.Camera_DVIName;
                            strfileName = SystemClass.BaseFile + "Car" + strPosition + strDriveway;
                            if (!Directory.Exists(strfileName))
                            {
                                Directory.CreateDirectory(strfileName);
                            }
                            ilChannel = Common.Converter.ToInt(vcdp.Camera_AddCard);
                            try
                            {
                                if (DVRName != "硬盘录像机一")
                                {
                                    SDKCommonTwo.CaptureJPEGPicture(strfileName, strPosition, strDriveway, ilChannel);
                                }
                                else
                                {
                                    SDKCommon.CaptureJPEGPicture(strfileName, strPosition, strDriveway, ilChannel);
                                }
                            }
                            catch
                            {
                                CloseSDK();
                                //InitSDK();
                                if (DVRName != "硬盘录像机一")
                                {
                                    SDKCommonTwo.CaptureJPEGPicture(strfileName, strPosition, strDriveway, ilChannel);
                                }
                                else
                                {
                                    SDKCommon.CaptureJPEGPicture(strfileName, strPosition, strDriveway, ilChannel);
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            catch
            {
                MessageBox.Show("拍照地感，调用拍照异常:" + "");
            }

        }
        public Dictionary<string, string> dicLoopStatus = new Dictionary<string, string>();
        /// <summary>
        /// 删除地感和通道信息列表中的指定地址的卡号
        /// </summary>
        /// <param name="strKey">卡的地址码</param>
        public void RemoveLoopStatus(string strKey)
        {
            dicLoopStatus.Remove(strKey);
        }
        /// <summary>
        /// 是否拍照 true为拍照，False为不拍照
        /// </summary>
        /// <param name="vcdp1">View_Camera_Driveway_Position实体</param>
        /// <returns></returns>
        private bool ISCamera(View_Camera_Driveway_Position vcdp1)
        {
            bool rbool = false;

            if (dicLoopStatus.Count > 0)
            {
                foreach (var item in dicLoopStatus)
                {
                    //通道值与第一次相同，物理地址值与第一次相同，未确认放行，再拍一次照片，不删除键值
                    if (item.Value == vcdp1.Driveway_Value && item.Key == vcdp1.Driveway_Address && common.ISCardRelease == false)
                    {
                        rbool = true;
                        break;
                    }
                    //通道值与第一次不同，物理地址值与第一次相同，未确认放行，删除之前的键值对，再添加一个dicLoopStatus键值对，再拍一次照片
                    else if (item.Value != vcdp1.Driveway_Value && item.Key == vcdp1.Driveway_Address && common.ISCardRelease == false)
                    {
                        rbool = true;
                        RemoveLoopStatus(item.Key);//移除键值
                        dicLoopStatus.Add(vcdp1.Driveway_Address, vcdp1.Driveway_Value);
                        break;
                    }
                    else//通道值与第一次相同，物理地址值与第一次相同，确认放行，删除键值；通道值与第一次不同，物理地址值与第一次相同，确认放行，删除键值
                    {
                        rbool = false;
                        RemoveLoopStatus(item.Key);//移除键值
                        common.ISCardRelease = false;
                        break;
                    }
                }
            }
            else//第一次添加物理地址值为键，通道值为值
            {
                rbool = true;
                dicLoopStatus.Add(vcdp1.Driveway_Address, vcdp1.Driveway_Value);//保存
            }
            return rbool;
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1.Visible = false;
            this.ShowInTaskbar = true;
            this.Show();
            this.Activate();
            this.WindowState = FormWindowState.Maximized;
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            this.ShowInTaskbar = true;
            this.Show();
            this.Activate();
            this.WindowState = FormWindowState.Normal;
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            this.ShowInTaskbar = true;
            this.Show();
            this.Activate();
            this.WindowState = FormWindowState.Normal;
        }

        private void exitStripMenuI_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(this, "确定要退出系统?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (result == DialogResult.OK)
            {
                this.notifyIcon1.Visible = false;
                GC.Collect();
                Application.ExitThread();
                Application.Exit();
                Process.GetCurrentProcess().Kill();
                System.Environment.Exit(System.Environment.ExitCode);
                Application.ExitThread();
            }
        }

        /// <summary>
        /// 根据条件获取通道信息
        /// 获得当前读卡器编号的地址、根据通道名称获取通道信息
        /// 有地感信息得到View_FVN_Driveway_Position
        /// </summary>
        /// <returns></returns>
        //  private View_FVN_Driveway_Position GetReadCardView_FVN_Driveway_Position(Expression<Func<View_FVN_Driveway_Position, bool>> fun)
        public View_FVN_Driveway_Position GetReadCardView_FVN_Driveway_Position(string strSql)
        {
            View_FVN_Driveway_Position rvfdp = null;
            try
            {
                List<View_FVN_Driveway_Position> list = View_FVN_Driveway_PositionDAL.GetSQLList(strSql);
                if (list.Count > 0)
                {
                    foreach (View_FVN_Driveway_Position vfdp in list)
                    {
                        //测试用
                        if (true) //当前通道读卡器读卡有效
                        {
                            rvfdp = vfdp;
                            break;
                        }
                    }
                }
            }
            catch
            {
                common.WriteTextLog("MainFrom.GetReadCardView_FVN_Driveway_Position:" + "");
            }

            return rvfdp;
        }

        #region PLC
        #region PLC方法
        /// <summary>
        /// 根据车辆到达某一线圈状态1有车辆感应进行拍照或0无感应放行
        /// 功能：在车辆到达或离开某一线圈的时候触发。
        /// [in]long lPort, 线圈所在进出口 为1,2…nPort
        /// in]long lLoop, 线圈 为1,2.
        /// [in]long lStatus 状态为:1有车辆感应,0 无感应;
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zlHaiweiPLCCommCtrl_LoopStatusChanged(object sender, _IModProEvents_LoopStatusChangedEvent e)
        {
            try
            {

                int iloop = e.lLoop;
                int iport = e.lPort;
                int istatus = e.lStatus;
                if (!common.ISService)
                {
                    return;
                }
                //通道编号
                DataTable Driveway_IDDT = LinQBaseDao.Query("select Driveway_Value from View_FVN_Driveway_Position where Position_ID=" + SystemClass.PositionID + " and FVN_Value='" + iport + "'").Tables[0];
                string Driveway_Value = "";
                if (Driveway_IDDT.Rows.Count > 0)
                {
                    Driveway_Value = Driveway_IDDT.Rows[0][0].ToString();
                }
                if (MainForm.dicCard.Count > 0)
                {
                    foreach (KeyValuePair<string, List<CardEntity>> temp in MainForm.dicCard)
                    {
                        string dname = temp.Key;
                        if (dname == Driveway_Value)
                        {
                            if (MainForm.dicCard[Driveway_Value].Count > 0)
                            {
                                MainForm.dicCard[Driveway_Value].Clear();//清空通道下的IC卡信息
                                CommonalityEntity.contolone = Convert.ToInt32(Driveway_Value.Substring(1, 1));
                            }
                        }
                    }
                }
                if (istatus == 1)
                {
                    isOpenYN = false;//万能卡，特权卡刷卡后是否开门（没地感开门，有地感到通行页面）
                    //    压地感信号灯变红
                    FVNSucceed(iport);
                    LinQBaseDao.Query("update DeviceControl set DeviceControl_FanSate='1' where DeviceControl_PositionValue='" + SystemClass.PosistionValue + "' and DeviceControl_FanValue='" + iport.ToString() + "'");
                    //如果是拍照地感，调用拍照
                    ISPhotographFvnAndPhotograph(iport);
                }
                else
                {
                    isOpenYN = true;//万能卡，特权卡刷卡后是否开门（没地感开门，有地感到通行页面）
                    LinQBaseDao.Query("update DeviceControl set DeviceControl_FanSate='0' where DeviceControl_PositionValue='" + SystemClass.PosistionValue + "' and DeviceControl_FanValue='" + iport + "'");
                    // 无压地感状态时信号灯为灰色
                    FVNFail(iport);
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// 手动控制显示关闭
        /// </summary>
        public void INOUTCONTORL()
        {
            //显示手动控制道闸
            if (CommonalityEntity.iscontrol == 1)
            {
                groupBox2.Visible = true;
                CommonalityEntity.iscontrol = 0;
            }
            if (CommonalityEntity.iscontrol == 2)
            {
                groupBox2.Visible = false;
                CommonalityEntity.iscontrol = 0;
            }
        }
        #endregion
        #region PLC手动控制方法
        /// <summary>
        /// 地感上有车 信号灯变红
        /// </summary>
        public void FVNSucceed(int iPort)
        {
            string fvndriveway = "";
            string fvntype = "";

            string strsqlfvn = "select Driveway_Value,FVN_Type from View_FVN_Driveway_Position where Position_Name='" + SystemClass.PositionName + "' and FVN_Value=" + iPort;

            DataTable dtfvn = LinQBaseDao.Query(strsqlfvn).Tables[0];
            if (dtfvn.Rows.Count > 0)
            {
                fvndriveway = dtfvn.Rows[0][0].ToString();
                fvntype = dtfvn.Rows[0][1].ToString();
            }
            switch (fvndriveway)
            {
                case "01":
                    if (fvntype == "拍照")
                    {
                        picFvn1.BackColor = Color.Red;
                    }
                    else if (fvntype == "放行")
                    {
                        picCloseFvn1.BackColor = Color.Red;
                    }
                    break;
                case "02":

                    if (fvntype == "拍照")
                    {
                        picFvn2.BackColor = Color.Red;
                    }
                    else if (fvntype == "放行")
                    {
                        picCloseFvn2.BackColor = Color.Red;
                    }
                    break;
                case "03":

                    if (fvntype == "拍照")
                    {
                        picFvn3.BackColor = Color.Red;
                    }
                    else if (fvntype == "放行")
                    {
                        picCloseFvn3.BackColor = Color.Red;
                    }
                    break;
                case "04":
                    if (fvntype == "拍照")
                    {
                        picFvn4.BackColor = Color.Red;
                    }
                    else if (fvntype == "放行")
                    {
                        picCloseFvn4.BackColor = Color.Red;
                    }
                    break;
                case "05":
                    if (fvntype == "拍照")
                    {
                        picFvn5.BackColor = Color.Red;
                    }
                    else if (fvntype == "放行")
                    {
                        picCloseFvn5.BackColor = Color.Red;
                    }
                    break;
                case "06":

                    if (fvntype == "拍照")
                    {
                        picFvn6.BackColor = Color.Red;
                    }
                    else if (fvntype == "放行")
                    {
                        picCloseFvn6.BackColor = Color.Red;
                    }
                    break;
            }
        }

        /// <summary>
        /// 地感上无车，信号灯边灰色
        /// </summary>
        public void FVNFail(int iPort)
        {
            string fvndriveway = "";
            string fvntype = "";

            string strsqlfvn = "select Driveway_Value,FVN_Type from View_FVN_Driveway_Position where Position_Name='" + SystemClass.PositionName + "' and FVN_Value=" + iPort;

            DataTable dtfvn = LinQBaseDao.Query(strsqlfvn).Tables[0];
            if (dtfvn.Rows.Count > 0)
            {
                fvndriveway = dtfvn.Rows[0][0].ToString();
                fvntype = dtfvn.Rows[0][1].ToString();
            }
            switch (fvndriveway)
            {
                case "01":
                    if (fvntype == "拍照")
                    {
                        picFvn1.BackColor = Color.Gray;
                    }
                    else if (fvntype == "放行")
                    {
                        picCloseFvn1.BackColor = Color.Gray;
                    }
                    break;
                case "02":
                    if (fvntype == "拍照")
                    {
                        picFvn2.BackColor = Color.Gray;
                    }
                    else if (fvntype == "放行")
                    {
                        picCloseFvn2.BackColor = Color.Gray;
                    }
                    break;
                case "03":
                    if (fvntype == "拍照")
                    {
                        picFvn3.BackColor = Color.Gray;
                    }
                    else if (fvntype == "放行")
                    {
                        picCloseFvn3.BackColor = Color.Gray;
                    }
                    break;
                case "04":
                    if (fvntype == "拍照")
                    {
                        picFvn4.BackColor = Color.Gray;
                    }
                    else if (fvntype == "放行")
                    {
                        picCloseFvn4.BackColor = Color.Gray;
                    }
                    break;
                case "05":
                    if (fvntype == "拍照")
                    {
                        picFvn5.BackColor = Color.Gray;
                    }
                    else if (fvntype == "放行")
                    {
                        picCloseFvn5.BackColor = Color.Gray;
                    }
                    break;
                case "06":
                    if (fvntype == "拍照")
                    {
                        picFvn6.BackColor = Color.Gray;
                    }
                    else if (fvntype == "放行")
                    {
                        picCloseFvn6.BackColor = Color.Gray;
                    }
                    break;
            }
        }
        #endregion
        #endregion

        Dictionary<string, string> valueDictOne = null;
        Dictionary<string, string> valueDictTo = null;
        int sssy = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                #region 测试用
                //if (sssy == 0)
                //{
                //    string stric = "090B4CC52";
                //    for (int i = 1; i < 7; i++)
                //    {
                //        CardReadRecord(stric, i.ToString(), "");
                //    }
                //    sssy = 1;
                //}
                #endregion

                common.ISService = Internet.PingIpOrDomainName(common.SQLIP);

                #region 解析1号机数据
                if (isComRunningOne)
                {
                    valueDictOne = new Dictionary<string, string>();
                    string strrecvOne = myComUserCtrl.m_recvDataLine.ToString();

                    //获取解析后的数据
                    myComUserCtrl.GetProcessedData(out valueDictOne);

                    //显示解析后的数据     
                    if (!string.IsNullOrEmpty(valueDictOne["ID"]))
                    {
                        strCardPortOne = valueDictOne["ID"]; //valueDictOne["ID"];
                        CommonalityEntity.strCardNo = valueDictOne["cardInfor"];
                        CommonalityEntity.WriteTextLog("卡地址" + strCardPortOne + "卡号：" + CommonalityEntity.strCardNo);
                        if (common.ISService)
                        {
                            DataTable dt = LinQBaseDao.Query("select ICCard_Value from ICCard where ICCard_Value='" + CommonalityEntity.strCardNo + "' and ICCard_State='启动'").Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                CardReadRecord(CommonalityEntity.strCardNo, strCardPortOne, "");
                            }
                        }
                        else
                        {
                            Contrast(Convert.ToInt32(strCardPortOne), CommonalityEntity.strCardNo);
                        }
                    }
                }

                #endregion

                #region 解析2号机数据
                if (isComRunningTo)
                {

                    //显示采集原始数据
                    valueDictTo = new Dictionary<string, string>();
                    string strrecvTo = myComUserCtr2.m_recvDataLine.ToString();

                    //获取解析后的数据
                    myComUserCtr2.GetProcessedData(out valueDictTo);

                    //显示解析后的数据     
                    if (!string.IsNullOrEmpty(valueDictTo["ID"]))
                    {
                        strCardPortTo = valueDictTo["ID"]; //valueDict["ID"];
                        CommonalityEntity.strCardNo = valueDictTo["cardInfor"];
                        CommonalityEntity.WriteTextLog("卡地址" + strCardPortTo + "卡号：" + CommonalityEntity.strCardNo);
                        if (common.ISService)
                        {
                            DataTable dt = LinQBaseDao.Query("select ICCard_Value from ICCard where ICCard_Value='" + CommonalityEntity.strCardNo + "' and ICCard_State='启动'").Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                CardReadRecord(CommonalityEntity.strCardNo, strCardPortTo, "");
                            }
                        }
                        else
                        {
                            Contrast(Convert.ToInt32(strCardPortTo), CommonalityEntity.strCardNo);
                        }
                    }
                }
                #endregion

            }
            catch
            {
                CommonalityEntity.WriteTextLog("MainForm.timer1_Tick异常：" + "");
            }
        }
        /// <summary>
        /// 注销待通行车辆50天以上的车辆
        /// </summary>
        private void zhuxiao()
        {
            DateTime dtime = CommonalityEntity.GetServersTime();
            DateTime ti = Convert.ToDateTime("2014-01-01");
            if (dtime > ti)
            {
                LinQBaseDao.Query("update  SortNumberInfo set SortNumberInfo_TongXing='已注销' where SortNumberInfo_TongXing='待通行' and SortNumberInfo_Time<'" + dtime.AddDays(-50) + "'");
            }
        }

        /// <summary>
        /// 语音呼叫
        /// </summary>
        private void VoicCall()
        {
            try
            {
                if (SystemClass.PositionName == "停车场")
                {
                    DataTable dtvoic = LinQBaseDao.Query("select VoiceCalls_Content,VoiceCalls_PositionName,VoiceCalls_Number ,VoiceCalls_ID from VoiceCalls  where VoiceCalls_ISVice=0 and VoiceCalls_Time>'" + CommonalityEntity.LoginTime + "'").Tables[0];
                    if (dtvoic.Rows.Count > 0)
                    {
                        string VoiceCalls_Content = "";
                        string VoiceCalls_PositionName = "";
                        int VoiceCalls_Number = 1;
                        string VoiceCalls_ID = "";
                        CommonalityEntity.ishujiao = false;
                        for (int i = 0; i < dtvoic.Rows.Count; i++)
                        {
                            VoiceCalls_Content = dtvoic.Rows[i][0].ToString();
                            VoiceCalls_PositionName = dtvoic.Rows[i][1].ToString();
                            VoiceCalls_Number = Convert.ToInt32(dtvoic.Rows[i][2].ToString());
                            VoiceCalls_ID = dtvoic.Rows[i][3].ToString();
                            int ss = VoiceCalls_Content.IndexOf("进");
                            string str = VoiceCalls_Content.Insert(ss + 1, GetNumberGbkString(VoiceCalls_PositionName) + ",");
                            if (CommonalityEntity.isvoic)
                            {
                                VoiceReade.readCount = VoiceCalls_Number;
                                VoiceReade.readTxt = str;
                                //Task.Factory.StartNew(new Action(() => Commons.CommonClass.VoiceReade.NewRead()));//新语音播放
                                Thread myThread = new Thread(new ThreadStart(read));
                                myThread.Priority = ThreadPriority.Highest;
                                myThread.Start();
                                if (myThread.IsAlive)
                                {
                                }
                                LinQBaseDao.Query("update VoiceCalls set VoiceCalls_ISVice=1 where VoiceCalls_ID=" + VoiceCalls_ID);
                            }
                        }
                    }
                }
                DataTable dtvoicset = LinQBaseDao.Query(" select * from PositionVoice where PositionVoice_Position_ID=" + SystemClass.PositionID + " and PositionVoice_PassageState='欢迎语' and PositionVoice_State='启动'").Tables[0];

                if (dtvoicset.Rows.Count > 0)
                {
                    CommonalityEntity.ishujiao = true;
                    string str = dtvoicset.Rows[0]["PositionVoice_Content"].ToString();
                    Commons.CommonClass.VoiceReade.readCount = SystemClass.HuJiaoCount;
                    Commons.CommonClass.VoiceReade.readTxt = str;
                    //Task.Factory.StartNew(new Action(() => Commons.CommonClass.VoiceReade.NewRead()));//新语音播放
                    Thread myThread = new Thread(new ThreadStart(read));
                    myThread.Priority = ThreadPriority.Highest;
                    myThread.Start();
                    if (myThread.IsAlive)
                    {
                    }
                }
            }
            catch
            {
                MessageBox.Show("MainForm呼叫异常！请确认已正确安装语音包。");
            }
        }
        private void read()
        {
            Commons.CommonClass.VoiceReade.NewRead();
        }
        /// <summary>
        /// 转换为语音识别的数字
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string GetNumberGbkString(string number)
        {

            StringBuilder convertGbk = new StringBuilder();
            foreach (char i in number)
            {
                switch (i)
                {
                    case '0': convertGbk.Append("零");
                        break;
                    case '1': convertGbk.Append("一");
                        break;
                    case '2': convertGbk.Append("二");
                        break;
                    case '3': convertGbk.Append("三");
                        break;
                    case '4': convertGbk.Append("四");
                        break;
                    case '5': convertGbk.Append("五");
                        break;
                    case '6': convertGbk.Append("六");
                        break;
                    case '7': convertGbk.Append("七");
                        break;
                    case '8': convertGbk.Append("八");
                        break;
                    case '9': convertGbk.Append("九");
                        break;
                    case '#': convertGbk.Append("号");
                        break;
                    default: convertGbk.Append(i.ToString());
                        break;
                }
            }
            return convertGbk.ToString();

        }
        private void button1_Click(object sender, EventArgs e)
        {
            string paths = Application.StartupPath + @"\Skip\";
            if (Directory.Exists(paths))
            {
                treeView1.Nodes.Clear();
                treeView1.Enabled = true;
                int i = 1;
                string[] Dir = Directory.GetFileSystemEntries(paths);

                foreach (string item in Dir)
                {
                    string s = item.Substring(item.LastIndexOf('\\') + 1).ToString();
                    TreeNode tr1;
                    {
                        tr1 = new TreeNode();
                        tr1.Tag = i;
                        tr1.Text = s.Substring(0, s.Length - 4);
                        treeView1.Nodes.Add(tr1);
                        i++;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string skin = treeView1.SelectedNode.Text + ".ssk";
                string filepath = Directory.GetCurrentDirectory() + "\\SystemSet.xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filepath);
                XmlNode xn = xmlDoc.SelectSingleNode("param");//查找<bookstore>
                XmlNodeList xnl = xn.ChildNodes;
                if (xnl.Count > 0)
                {
                    foreach (XmlNode xnf in xnl)
                    {
                        XmlElement xe = (XmlElement)xnf;
                        xe.SetAttribute("Skin", skin);  // 系统皮肤
                    }
                    xmlDoc.Save(filepath);
                    SystemClass.Skin = skin; //皮肤应用
                    MessageBox.Show("系统皮肤应用成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                skinEngine1.Active = false;
            }
            catch
            {
                CommonalityEntity.WriteTextLog("SAPCarInfoForm btnChkSAP_Click()" + "");
            }
        }


        private void 手动控制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ControlForm.cf == null)
            {
                ControlForm.cf = new ControlForm();
                ControlForm.cf.mf = this;
                ControlForm.cf.Show();
            }
            else
            {
                ControlForm.cf.Activate();
                ControlForm.cf.TopLevel = true;
                ControlForm.cf.WindowState = FormWindowState.Normal;
            }

        }
        private void 车俩进出厂管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!common.ISService)
                {
                    MessageBox.Show("请检查网络！");
                    return;
                }
                if (CarInOutForm.cfin == null)
                {
                    CarInOutForm.cfin = new CarInOutForm();
                    //CarInOutForm.cfin.mf = this;
                    CarInOutForm.cfin.Show();
                }
                else
                {
                    CarInOutForm.cfin.Activate();
                    CarInOutForm.cfin.TopLevel = true;
                    CarInOutForm.cfin.WindowState = FormWindowState.Normal;
                }
            }
            catch
            {
            }
        }

        private void bingmenu()
        {
            try
            {
                string strsql = "";
                if (CommonalityEntity.USERNAME == "emewe")
                {
                    手动控制ToolStripMenuItem.Visible = true;
                    车辆进出厂管理ToolStripMenuItem.Visible = true;
                    排队管理ToolStripMenuItem.Visible = true;
                }
                else
                {
                    strsql = " select distinct(Permissions_Menu_ID),Menu_ID,Menu_ControlText,menu_Order,menu_otherid,Menu_Visible,Menu_FromName  from View_MenuInfo_P where   (userid=" + CommonalityEntity.USERID + " or Permissions_Role_Id=" + CommonalityEntity.USERRoleID + ") and Menu_ControlType!=6  and Menu_FromName='CarInOutForm' ";
                    DataTable dt = LinQBaseDao.Query(strsql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        车辆进出厂管理ToolStripMenuItem.Visible = true;
                    }
                    else
                    {
                        车辆进出厂管理ToolStripMenuItem.Visible = false;
                    }
                    strsql = " select distinct(Permissions_Menu_ID),Menu_ID,Menu_ControlText,menu_Order,menu_otherid,Menu_Visible,Menu_FromName  from View_MenuInfo_P where  (userid=" + CommonalityEntity.USERID + " or Permissions_Role_Id=" + CommonalityEntity.USERRoleID + ") and Menu_ControlType!=6  and Menu_FromName='ControlForm'";
                    dt = LinQBaseDao.Query(strsql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        手动控制ToolStripMenuItem.Visible = true;
                    }
                    else
                    {
                        手动控制ToolStripMenuItem.Visible = false;
                    }
                    strsql = " select distinct(Permissions_Menu_ID),Menu_ID,Menu_ControlText,menu_Order,menu_otherid,Menu_Visible,Menu_FromName  from View_MenuInfo_P where  (userid=" + CommonalityEntity.USERID + " or Permissions_Role_Id=" + CommonalityEntity.USERRoleID + ") and Menu_ControlType!=6  and Menu_FromName='QueueForm' ";
                    dt = LinQBaseDao.Query(strsql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        排队管理ToolStripMenuItem.Visible = true;
                    }
                    else
                    {
                        排队管理ToolStripMenuItem.Visible = false;
                    }
                }
            }
            catch
            {
            }
        }

        private void 排队管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!common.ISService)
                {
                    MessageBox.Show("请检查网络！");
                    return;
                }
                if (QueueForm.qf == null)
                {
                    QueueForm.qf = new QueueForm();
                    QueueForm.qf.mf = this;
                    QueueForm.qf.Show();
                }
                else
                {
                    QueueForm.qf.Activate();
                    QueueForm.qf.TopLevel = true;
                    QueueForm.qf.WindowState = FormWindowState.Normal;
                }
            }
            catch
            {

            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                if (common.ISService)
                {
                    if (SystemClass.ISChkSortNumber)
                    {
                        ChkSortNumber();
                    }
                    //if (SystemClass.ISVoicCall)
                    //{
                    //    VoicCall();
                    //}
                    if (SystemClass.ISPromptout)
                    {
                        Promptout();
                    }
                }
                else
                {
                    if (PortFrom.pr != null)
                    {
                        PortFrom.pr.Close();
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("MainForm.timer2_Tick异常：" + "");
            }
        }

        private void Promptout()
        {
            if (SystemClass.PositionName == "停车场")
            {
                int prcount = Convert.ToInt32(LinQBaseDao.GetSingle("select COUNT(0) from CarInOutRecord,SmallTicket,SortNumberInfo where  CarInOutRecord_CarInfo_ID=SmallTicket_CarInfo_ID and SmallTicket_ID=SortNumberInfo_SmallTicket_ID  and SortNumberInfo_TongXing='排队中' and CarInOutRecord_InCheck='是'").ToString());
                if (prcount > 0)
                {
                    if (PortFrom.pr == null)
                    {
                        PortFrom.pr = new PortFrom();
                        PortFrom.pr.mf = this;
                        PortFrom.pr.Show();
                    }
                    PortFrom.pr.lbstr.Text = prcount + "条信息";
                }
                else
                {
                    if (PortFrom.pr != null)
                    {
                        PortFrom.pr.Close();
                    }
                }
            }
            else
            {
                int prcount = Convert.ToInt32(LinQBaseDao.GetSingle("select COUNT(0) from CarInOutRecord,SmallTicket,SortNumberInfo where  CarInOutRecord_CarInfo_ID=SmallTicket_CarInfo_ID and SmallTicket_ID=SortNumberInfo_SmallTicket_ID  and SortNumberInfo_TongXing='排队中' and CarInOutRecord_InCheck='是' and SmallTicket_Position_ID=" + SystemClass.PositionID).ToString());
                if (prcount > 0)
                {
                    if (PortFrom.pr == null)
                    {
                        PortFrom.pr = new PortFrom();
                        PortFrom.pr.mf = this;
                        PortFrom.pr.Show();
                    }
                    PortFrom.pr.lbstr.Text = prcount + "条信息";
                }
                else
                {
                    if (PortFrom.pr != null)
                    {
                        PortFrom.pr.Close();
                    }
                }
            }
        }
        private void Istimers2()
        {
            if (!SystemClass.ISChkSortNumber && !SystemClass.ISVoicCall && !SystemClass.ISPromptout)
            {
                timer2.Enabled = false;
            }
        }

        #region 运行中断网

        /// <summary>
        /// 判断数据
        /// </summary>
        /// <param name="key">地址码</param>
        /// <param name="strcarno">IC卡号</param>
        private void Contrast(int key, string strcarno)
        {
            try
            {
                if (!ListData(strcarno))
                {
                    MessageBox.Show("该卡没有权限", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (key == ControlPLC.CardAddressOne)
                {
                    InsertRecord("手动开启" + SystemClass.PositionName + "道闸一", strcarno, ControlPLC.OpenAddressOne);
                }
                if (key == ControlPLC.CardAddressTwo)
                {
                    InsertRecord("手动开启" + SystemClass.PositionName + "道闸二", strcarno, ControlPLC.OpenAddressTwo);
                }
                if (key == ControlPLC.CardAddressThree)
                {
                    InsertRecord("手动开启" + SystemClass.PositionName + "道闸三", strcarno, ControlPLC.OpenAddressThree);
                }
                if (key == ControlPLC.CardAddressFour)
                {
                    InsertRecord("手动开启" + SystemClass.PositionName + "道闸四", strcarno, ControlPLC.OpenAddressFour);
                }
                if (key == ControlPLC.CardAddressFive)
                {
                    InsertRecord("手动开启" + SystemClass.PositionName + "道闸五", strcarno, ControlPLC.OpenAddressFive);
                }
                if (key == ControlPLC.CardAddressSix)
                {
                    InsertRecord("手动开启" + SystemClass.PositionName + "道闸六", strcarno, ControlPLC.OpenAddressSix);
                }
            }
            catch (Exception ex)
            {
                common.WriteTextLog("MainForm.Contrast():" + ex.Message);
            }
        }

        /// <summary>
        /// 判断数据是否在集合中
        /// </summary>
        /// <param name="stritem"></param>
        /// <returns></returns>
        private bool ListData(string stritem)
        {
            if (ControlPLC.ICCARD == "")
            {
                return false;
            }
            string[] list = ControlPLC.ICCARD.Split(',');
            foreach (var item in list)
            {
                if (stritem == item)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 添加记录
        /// </summary>
        private void InsertRecord(string str, string strcarno, Int16 address)
        {
            OpenDoor(address);
            if (strcarno != "")
            {
                CommonalityEntity.WriteTextLog(str + "；刷卡卡号为：" + strcarno);
            }
            else
            {
                CommonalityEntity.WriteTextLog(str);
            }

        }
        #endregion

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            skinEngine1.Active = true;
            groupBox1.Visible = true;
        }

        private void btnCloseFace_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            treeView1.Enabled = false;
            groupBox1.Visible = false;
            skinEngine1.Active = false;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.skinEngine1.SkinFile = "Skip\\" + e.Node.Text + ".ssk";
            skinEngine1.DisableTag = 9999;
        }
    }
}
