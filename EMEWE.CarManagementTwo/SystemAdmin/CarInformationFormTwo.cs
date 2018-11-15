using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.HelpClass;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.Entity;
using System.IO;
using EMEWE.CarManagement.CommonClass;
using System.Threading;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class CarInformationFormTwo : Form
    {
        public CarInformationFormTwo(string _carIds, bool carinfo)
        {
            ///carinfo为false： 卡
            if (carinfo == false)
            {
                CommonalityEntity.CarNO = _carIds;
                CommonalityEntity.SerialnumberICbool = carinfo;
            }
            else
            {
                CommonalityEntity.Serialnumber = _carIds;
                CommonalityEntity.SerialnumberICbool = carinfo;
            }
            InitializeComponent();
        }
        string cartypename = "";
        /// <summary>
        ///  是否修改同行策略
        /// </summary>
        bool IsUpdatedri;
        /// <summary>
        /// 卡类型
        /// </summary>
        private string ictype = "";
        /// <summary>
        /// 卡号
        /// </summary>
        private string icvalue = "";

        public static CarInformationFormTwo cinfo = null;

        public MainForm mf;
        /// <summary>
        /// 标识
        /// </summary>
        private bool ISRelease = false;
        /// <summary>
        /// 存放异常/正常标识
        /// </summary>
        private string strAbnormal = "";
        /// <summary>
        /// 信息提示
        /// </summary>
        private string strmessagebox = "";
        /// <summary>
        /// 当前车辆信息编号
        /// </summary>
        private string strCarInOutRecord_CarInfo_ID = "";
        /// <summary>
        /// 图片集合
        /// </summary>
        private List<PictureBox> picList = new List<PictureBox>();

        private CheckProperties checkPr = new CheckProperties();
        /// <summary>
        /// 通行详细记录
        /// </summary>
        private string CarInOutInfoRecord_ID = "";

        public int openshort = 0;

        private string pathpic1 = "", pathpic2 = "";

        private string carinoutRid = "";
        /// <summary>
        /// 车辆类型ID
        /// </summary>
        int carType_ID;
        /// <summary>
        /// 通行策略名称
        /// </summary>
        private string Strategy_DriSName = "";
        /// <summary>
        /// 时间差
        /// </summary>
        string strtime = "";
        /// <summary>
        /// 车辆信息
        /// </summary>
        DataTable DTCheckProperties;
        /// <summary>
        /// 是否刷卡放行
        /// </summary>
        bool rboolIc;
        /// <summary>
        /// 验证保安卡
        /// </summary>
        bool EnsureSafetyICHave;
        /// <summary>
        /// 进出标示
        /// </summary>
        bool ISInOut;
        /// <summary>
        /// 小票IC卡标示
        /// </summary>
        bool SerialnumberICbool;
        /// <summary>
        /// 是否指登记一次
        /// </summary>
        bool Car_ISRegister;
        /// <summary>
        /// 业务是否完成
        /// </summary>
        bool boolYesNoCarInOutRecord_ISFulfill;
        /// <summary>
        /// 同行策略ID
        /// </summary>
        int DrivewayStrategy_ID;
        /// <summary>
        /// 通道名称
        /// </summary>
        string Driveway_Name;
        /// <summary>
        /// 通道值
        /// </summary>
        string Driveway_Value;
        /// <summary>
        /// 通道ID
        /// </summary>
        int Driveway_ID;
        /// <summary>
        /// 车牌号
        /// </summary>
        string strCardNo;
        /// <summary>
        /// 卡I
        /// </summary>
        string ICC1;
        /// <summary>
        /// 卡2
        /// </summary>
        string ICC2;
        /// <summary>
        /// 存放有效图片
        /// </summary>
        private List<string> listCarPicEffective;
        /// <summary>
        /// 存放过期图片
        /// </summary>
        List<string> listCarPic;
        /// <summary>
        /// 页面传值，用于清空对应通道下的信息
        /// </summary>
        int contio = 0;
        /// <summary>
        /// 是否点击放行按钮
        /// </summary>
        bool isBtnclick = false;
        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {
                btn_ConfirmationRelease.Enabled = true;
                btn_ConfirmationRelease.Visible = true;

                button1.Enabled = true;
                button1.Visible = true;
                isBtnclick = true;
            }
            else
            {
                btn_ConfirmationRelease.Visible = ControlAttributes.BoolControl("btn_ConfirmationRelease", "CarInformationForm", "Visible");
                btn_ConfirmationRelease.Enabled = ControlAttributes.BoolControl("btn_ConfirmationRelease", "CarInformationForm", "Enabled");

                button1.Visible = ControlAttributes.BoolControl("button1", "CarInformationForm", "Visible");
                button1.Enabled = ControlAttributes.BoolControl("button1", "CarInformationForm", "Enabled");
                isBtnclick = ControlAttributes.BoolControl("button2", "CarInformationForm", "Visible");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarInformationForm_Load(object sender, EventArgs e)
        {
            DateTime statetime = DateTime.Now;
            CommonalityEntity.isRestFXTwo = false;
            CommonalityEntity.strCardNo = "";
            Control.CheckForIllegalCrossThreadCalls = false;
            userContext();
            UserLoadMethod();
            double db = (DateTime.Now - statetime).TotalSeconds;
            if ((DateTime.Now - statetime).TotalSeconds > 5)
            {
                CommonalityEntity.WriteTextLog(CheckProperties.dt.Rows[0]["CarInfo_Name"].ToString() + "扫小票号后页面打开时间：" + (DateTime.Now - statetime).TotalSeconds);
            }
            this.Text = CommonalityEntity.Driveway_Name;
            DTCheckProperties = CheckProperties.dt;
            ISInOut = CommonalityEntity.ISInOut;
            SerialnumberICbool = CommonalityEntity.SerialnumberICbool;
            Car_ISRegister = CommonalityEntity.Car_ISRegister;
            boolYesNoCarInOutRecord_ISFulfill = CommonalityEntity.boolYesNoCarInOutRecord_ISFulfill;
            DrivewayStrategy_ID = CommonalityEntity.DrivewayStrategy_ID;
            Driveway_Name = CommonalityEntity.Driveway_Name;
            Driveway_Value = CommonalityEntity.Driveway_Value;
            Driveway_ID = CommonalityEntity.Driveway_ID;
            strCardNo = CommonalityEntity.strCardNo;
            ICC1 = CommonalityEntity.ICC1;
            ICC2 = CommonalityEntity.ICC2;

            rboolIc = CheckMethod.rboolIc;
            EnsureSafetyICHave = CommonalityEntity.EnsureSafetyICHave;
            carType_ID = CheckProperties.ce.carType_ID;
            IsUpdatedri = CommonalityEntity.IsUpdatedri;
            contio = CommonalityEntity.contolint;
            if (isBtnclick)
            {
                if (string.IsNullOrEmpty(txtUnusual_Info.Text.Trim()))
                {
                    btn_ConfirmationRelease_Click(sender, e);
                }
                else
                {
                    button1_Click(sender, e);
                }
            }
        }

        private void UserLoadMethod()
        {
            try
            {
                CommonalityEntity.rboolInCar = true;
                CommonalityEntity.EnsureSafetyICHave = false;
                CheckMethod.rboolIc = false; // 是否刷卡放行
                CheckMethod.AbnormalInformation = "";
                CheckMethod.listUnusualRecord = new List<UnusualRecord>();
                CheckMethod.listMessage.Clear();
                CommonalityEntity.listCarPic.Clear();
                CommonalityEntity.listCarPicEffective.Clear();
                CheckProperties.ce.isImageList = false;//是否拍照
                CommonalityEntity.CarType_Name = "";
                CheckProperties.ce.Imagelist.Clear();

                lbl_Prompt.Text = "信息正在读取中......................";
                ISAddCarInfo();
                CheckProperties.CheckCarInOutRecordISCompleteMethod();
                AddCarInOutRecordMethod();
                CarInformationMethod();

                CommonalityEntity.CarInfo_ID = CheckProperties.dt.Rows[0]["CarInfo_ID"].ToString();
                CheckProperties.ce.CarType_Name = CheckProperties.dt.Rows[0]["CarType_Name"].ToString();
                CheckProperties.ce.carType_Value = CheckProperties.dt.Rows[0]["carType_Value"].ToString();
                CheckProperties.ce.carType_ID = Convert.ToInt32(CheckProperties.dt.Rows[0]["carType_ID"].ToString());
                CheckProperties.ce.carInfo_Name = CheckProperties.dt.Rows[0]["carInfo_Name"].ToString();
                CheckProperties.ce.CarInfo_ID = Convert.ToInt32(CheckProperties.dt.Rows[0]["CarInfo_ID"].ToString());
                CheckProperties.ce.ChengPinNumber = CheckProperties.dt.Rows[0]["CarInfo_Name"].ToString();
                CheckProperties.ce.serialNumber = CommonalityEntity.Serialnumber;
                CheckProperties.ce.SongHuoNumber = CheckProperties.dt.Rows[0]["CarInfo_PO"].ToString();
                CheckProperties.ce.SangFeiNumber = CheckProperties.dt.Rows[0]["CarInfo_PO"].ToString();

                CommonalityEntity.CarInoutid = CheckProperties.dt.Rows[0]["CarInOutRecord_ID"].ToString();
                carinoutRid = CommonalityEntity.CarInoutid;
                strCarInOutRecord_CarInfo_ID = CommonalityEntity.CarInfo_ID;
                string strsql = "select CarType_Property from CarType where CarType_Name='" + CheckProperties.ce.CarType_Name + "'";
                DataTable dtcartypety = LinQBaseDao.Query(strsql).Tables[0];
                if (dtcartypety.Rows.Count > 0)
                {
                    CommonalityEntity.CarType_Name = dtcartypety.Rows[0][0].ToString();
                }

                //执行管控策略
                if (CommonalityEntity.IsUpdatedri)
                {
                    strsql = "select * from ManagementStrategyRecord where ManagementStrategyRecord_DrivewayStrategy_ID=" + CommonalityEntity.DrivewayStrategy_ID + "  and ManagementStrategyRecord_CarInfo_ID=" + CheckProperties.dt.Rows[0]["CarInfo_ID"] + " and ManagementStrategyRecord_State='启动' and ManagementStrategyRecord_Rule not in ('ChkChengPin','ChkSongHuo','ChkSanFei')";
                }
                else
                {
                    strsql = "select * from ManagementStrategy where ManagementStrategy_DrivewayStrategy_ID=" + CommonalityEntity.DrivewayStrategy_ID + " and ManagementStrategy_DriSName='" + Strategy_DriSName + "' and ManagementStrategy_State='启动' and ManagementStrategy_Rule not in ('ChkChengPin','ChkSongHuo','ChkSanFei')";
                }
                checkPr.ExecutionMethod(strsql);//执行指定车辆类型的管控

                if (CheckMethod.listMessage.Count > 0)
                {
                    StringBuilder strMessage = new StringBuilder();
                    foreach (string item in CheckMethod.listMessage)
                    {
                        strMessage.Append(item.Trim());
                    }
                    txtUnusual_Info.Text = strMessage.ToString();
                    btn_ConfirmationRelease.Enabled = false;
                }

                ThreadPool.QueueUserWorkItem(new WaitCallback(ShowImage), null);

                if (CheckProperties.ce.isImageList)
                {
                    if (CommonalityEntity.listCarPicEffective.Count < 1 && CommonalityEntity.listCarPic.Count < 1)
                    {
                        btn_ConfirmationRelease.Enabled = false;
                        txtUnusual_Info.Text += "\r\n没有拍照！！！";
                    }
                }
                if (CheckProperties.dt.Rows.Count > 0 && CommonalityEntity.Driveway_Value != null)
                {
                    ClearDeviceControlDeviceControl_CardNoMethod(SystemClass.PosistionValue, CommonalityEntity.Driveway_Value);//清空该通道下的所有卡信息
                }
                txtUnusual_Info.Text += CheckMethod.AbnormalInformation;//显示异常信息
                if (!string.IsNullOrEmpty(txtUnusual_Info.Text))
                {
                    btn_ConfirmationRelease.Enabled = false;
                }
                lbl_Prompt.Text = "信息已全部显示，请核对无误后刷卡放行!!!";
            }
            catch (Exception ex)
            {
                CommonalityEntity.WriteTextLog("CarInformationForm.UserLoadMethod()" + ex.Message.ToString());
            }

        }
        /// <summary>
        /// 添加总通行记录   
        /// </summary>
        private void AddCarInOutRecordMethod()
        {

            bool isbool = false;
            string state1 = "";
            bool isfull = Convert.ToBoolean(CheckProperties.dt.Rows[0]["CarInOutRecord_ISFulfill"].ToString());
            bool isupdir = Convert.ToBoolean(CheckProperties.dt.Rows[0]["CarInOutRecord_Update"].ToString());
            CommonalityEntity.IsUpdatedri = isupdir;

            if (CheckProperties.dt.Rows.Count > 0)
            {
                state1 = CheckProperties.dt.Rows[0]["SortNumberInfo_TongXing"].ToString();
                if (CommonalityEntity.ISInOut)
                {
                    int i = state1.IndexOf("进");
                    if (i >= 0)
                    {
                        isbool = true;
                    }
                }
                else
                {
                    int i = state1.IndexOf("出");
                    if (i >= 0)
                    {
                        isbool = true;
                    }
                }
                int s = state1.IndexOf("注销");
                if (s >= 0)
                {
                    isbool = true;
                }
            }

            if (!CommonalityEntity.IsUpdatedri)
            {
                DataTable DrivewayStrategy_IDDT = LinQBaseDao.Query("select DrivewayStrategy_ID,DrivewayStrategy_Sort,DrivewayStrategy_Driveway_ID,DrivewayStrategy_Name from DrivewayStrategy where DrivewayStrategy_State='启动' and  DrivewayStrategy_Name ='" + CheckProperties.dt.Rows[0]["CarInOutRecord_Remark"].ToString() + "' order by DrivewayStrategy_Sort ").Tables[0];
                if (DrivewayStrategy_IDDT.Rows.Count <= 0)
                {
                    MessageBox.Show("该车辆没有配通行策略！", "提示");
                    return;
                }
                string WarrantyID = CommonalityEntity.Driveway_ID.ToString();
                if (CommonalityEntity.isDriState)
                {
                    WarrantyID = CommonalityEntity.DriWarrantyID;
                }
                Strategy_DriSName = DrivewayStrategy_IDDT.Rows[0]["DrivewayStrategy_Name"].ToString();
                for (int i = 0; i < DrivewayStrategy_IDDT.Rows.Count; i++)
                {
                    if (DrivewayStrategy_IDDT.Rows[i]["DrivewayStrategy_Driveway_ID"].ToString() == WarrantyID)
                    {
                        CommonalityEntity.DrivewayStrategy_ID = Convert.ToInt32(DrivewayStrategy_IDDT.Rows[i]["DrivewayStrategy_ID"].ToString());
                        break;
                    }
                }
            }
            else
            {
                DataTable DrivewayStrategy_IDDT = LinQBaseDao.Query("select DrivewayStrategyRecord_ID,DrivewayStrategyRecord_Sort,DrivewayStrategyRecord_Driveway_ID from DrivewayStrategyRecord where DrivewayStrategyRecord_State='启动' and  DrivewayStrategyRecord_CarInfo_ID =" + CheckProperties.dt.Rows[0]["CarInfo_ID"].ToString() + "  order by DrivewayStrategyRecord_Sort ").Tables[0];
                if (DrivewayStrategy_IDDT.Rows.Count <= 0)
                {
                    MessageBox.Show("该车辆没有配通行策略！", "提示");
                    return;
                }
                string WarrantyID = CommonalityEntity.Driveway_ID.ToString();
                if (CommonalityEntity.isDriState)
                {
                    WarrantyID = CommonalityEntity.DriWarrantyID;
                }
                for (int i = 0; i < DrivewayStrategy_IDDT.Rows.Count; i++)
                {
                    if (DrivewayStrategy_IDDT.Rows[i]["DrivewayStrategyRecord_Driveway_ID"].ToString() == WarrantyID)
                    {
                        CommonalityEntity.DrivewayStrategy_ID = Convert.ToInt32(DrivewayStrategy_IDDT.Rows[i]["DrivewayStrategyRecord_ID"].ToString());
                        break;
                    }
                }
            }
            if (state1 == "待通行")
            {
                return;
            }
            if ((isfull && CommonalityEntity.Car_ISRegister) || (isbool && CommonalityEntity.Car_ISRegister))  //业务完成，并且只登记一次车辆，添加车辆通行总记录等信息；当本次的通行状态与前一次通行状态相同时，为异常进出厂，添加通行总记录
            {
                string sql = "Select * from Car where Car_Name='" + CommonalityEntity.CarNO + "'";
                Car car = new Car();
                car = LinQBaseDao.GetItemsForListing<Car>(sql).FirstOrDefault();

                sql = "select * from CarType where CarType_ID=" + car.Car_CarType_ID;
                CarType cartype = new CarType();
                cartype = LinQBaseDao.GetItemsForListing<CarType>(sql).FirstOrDefault();

                DataTable DrivewayStrategy_dt = LinQBaseDao.Query("select DrivewayStrategy_ID,DrivewayStrategy_Sort,DrivewayStrategy_Driveway_ID,DrivewayStrategy_Name from DrivewayStrategy where DrivewayStrategy_State='启动' and  DrivewayStrategy_Name ='" + cartype.CarType_DriSName + "' order by DrivewayStrategy_Sort  ").Tables[0];
                DataTable driiddt = LinQBaseDao.Query("select DrivewayStrategy_ID,DrivewayStrategy_Sort,DrivewayStrategy_Driveway_ID,DrivewayStrategy_Name from DrivewayStrategy where DrivewayStrategy_State='启动' and  DrivewayStrategy_Name in (select Car_DriSName from Car  where Car_Name='" + CheckProperties.dt.Rows[0]["CarInfo_Name"].ToString() + "') order by DrivewayStrategy_Sort ").Tables[0];
                if (driiddt.Rows.Count > 0)
                {
                    DrivewayStrategy_dt = driiddt;
                }

                Strategy_DriSName = DrivewayStrategy_dt.Rows[0]["DrivewayStrategy_Name"].ToString();
                if (DrivewayStrategy_dt.Rows.Count <= 0)
                {
                    MessageBox.Show("该车辆没有配通行策略！", "提示");
                    return;
                }
                string WarrantyID = CommonalityEntity.Driveway_ID.ToString();
                if (CommonalityEntity.isDriState)
                {
                    WarrantyID = CommonalityEntity.DriWarrantyID;
                }
                for (int i = 0; i < DrivewayStrategy_dt.Rows.Count; i++)
                {
                    if (DrivewayStrategy_dt.Rows[i]["DrivewayStrategy_Driveway_ID"].ToString() == WarrantyID)
                    {
                        CommonalityEntity.IsUpdatedri = false;
                        CommonalityEntity.DrivewayStrategy_ID = Convert.ToInt32(DrivewayStrategy_dt.Rows[i]["DrivewayStrategy_ID"].ToString());
                        break;
                    }
                }
                if (!ISadd())
                {
                    return;
                }
                DataTable dt;

                #region 车辆信息

                object objid = LinQBaseDao.GetSingle("select top(1) CarInfo_ID from CarInfo where CarInfo_Name='" + CheckProperties.dt.Rows[0]["CarInfo_Name"] + "' and CarInfo_CarType_ID =" + car.Car_CarType_ID + " order by CarInfo_ID desc ");
                if (objid == null)
                {
                    return;
                }
                string carSql = "insert into CarInfo select 	CarInfo_CustomerInfo_ID,	CarInfo_CarType_ID,	CarInfo_BusinessType_ID,	CarInfo_Name	,CarInfo_Type,	CarInfo_State,	CarInfo_Carriage,	CarInfo_Weight,	CarInfo_Height	,CarInfo_Bail,	CarInfo_PO,	CarInfo_LevelWaste,	GETDATE(),	CarInfo_Remark,	CarInfo_Operate,	CarInfo_Car_ID from CarInfo where CarInfo_ID=" + objid.ToString() + " select @@identity";
                int carinfo_id = int.Parse(LinQBaseDao.GetSingle(carSql).ToString());//得到当前的车辆编号

                #endregion

                #region 登记图片信息

                string picSql = "";
                picSql = "Insert into CarPic(CarPic_CarInfo_ID,CarPic_State,CarPic_Add,CarPic_Type,CarPic_Time,CarPic_Match) values(" + carinfo_id + ",'启动','" + car.Car_AddPic + "','车辆登记照片',getdate(),'匹配')";
                picSql = picSql + " select @@identity";
                LinQBaseDao.GetSingle(picSql).ToString();//得到当前的图片编号

                #endregion

                #region 进出凭证信息
                SmallTicket stk = new SmallTicket();
                string icid = "", count = "", hour = "";
                icid = CommonalityEntity.CheckICValue(CommonalityEntity.ICC1, CommonalityEntity.ICC2, out count, out hour);
                if (string.IsNullOrEmpty(count))
                {
                    stk.SmallTicket_Allowcount = 1;
                }
                else
                {
                    stk.SmallTicket_Allowcount = int.Parse(count);
                }
                if (string.IsNullOrEmpty(hour))
                {
                    stk.SmallTicket_Allowhour = 0;
                }
                else
                {
                    stk.SmallTicket_Allowhour = int.Parse(hour);
                }
                stk.SmallTicket_CarInfo_ID = carinfo_id;
                if (!string.IsNullOrEmpty(icid))
                {
                    stk.SmallTicket_Type = "IC卡";
                    stk.SmallTicket_ICCard_ID = Convert.ToInt32(icid);
                }
                stk.SmallTicket_Position_ID = SystemClass.PositionID;
                stk.SmallTicket_PrintNumber = "";
                stk.SmallTicket_Remark = "";
                stk.SmallTicket_State = "有效";
                stk.SmallTicket_Time = CommonalityEntity.GetServersTime();
                string SmalltSql = "insert into SmallTicket values(" + stk.SmallTicket_ICCard_ID + ",'" + stk.SmallTicket_Serialnumber + "'," + stk.SmallTicket_Position_ID + "," + stk.SmallTicket_CarInfo_ID + ",'" + stk.SmallTicket_Time + "','" + stk.SmallTicket_SortNumber + "','" + stk.SmallTicket_PrintNumber + "','" + stk.SmallTicket_State + "','" + stk.SmallTicket_Type + "','" + stk.SmallTicket_Allowcounted + "','" + stk.SmallTicket_Allowcount + "','" + stk.SmallTicket_Allowhour + "','" + stk.SmallTicket_Remark + "')";
                SmalltSql = SmalltSql + " select @@identity";
                int smallTicket_ID = int.Parse(LinQBaseDao.GetSingle(SmalltSql).ToString());//得到当前的车辆编号
                #endregion

                #region 排队信息
                SortNumberInfo sort = new SortNumberInfo();
                dt = LinQBaseDao.Query("select CarType_Value from CarType where CarType_ID =" + CheckProperties.dt.Rows[0]["CarInfo_CarType_ID"].ToString()).Tables[0];

                //添加排队信息
                string sort_Sql = "insert into SortNumberInfo( SortNumberInfo_SmallTicket_ID,SortNumberInfo_Time,SortNumberInfo_Reasons,SortNumberInfo_Operate,SortNumberInfo_Type,SortNumberInfo_PositionValue,SortNumberInfo_CarTypeValue, SortNumberInfo_TongXing,SortNumberInfo_DrivewayValue,SortNumberInfo_State,SortNumberInfo_CallTime,SortNumberInfo_ISEffective,SortNumberInfo_ISNoteRecord,SortNumberInfo_SortValue,SortNumberInfo_CallCount,SortNumberInfo_SMSCount,SortNumberInfo_LEDCount) values('" + smallTicket_ID + "','" + CommonalityEntity.GetServersTime() + "','进厂生成','" + CommonalityEntity.USERNAME + "','系统生成','" + SystemClass.PosistionValue + "','" + dt.Rows[0][0].ToString() + "','待通行','" + CommonalityEntity.Driveway_Value + "','启动','1990-01-01',1,0,0,0,0,0)";
                sort_Sql = sort_Sql + " select @@identity";
                int sortNumberInfo_ID = int.Parse(LinQBaseDao.GetSingle(sort_Sql).ToString());
                #endregion

                #region 添加通行总记录
                CarInOutRecord cir = new CarInOutRecord();
                string strstrategy = "";
                bool issort = false;
                cir.CarInOutRecord_DrivewayStrategy_ID = CommonalityEntity.DrivewayStrategy_ID;

                for (int i = 0; i < DrivewayStrategy_dt.Rows.Count; i++)
                {
                    if (Convert.ToInt32(DrivewayStrategy_dt.Rows[i]["DrivewayStrategy_Sort"].ToString()) == 1)
                    {
                        issort = false;
                        strstrategy += DrivewayStrategy_dt.Rows[i]["DrivewayStrategy_ID"].ToString() + ",";
                    }
                    else
                    {
                        issort = true;
                        strstrategy += DrivewayStrategy_dt.Rows[i]["DrivewayStrategy_ID"].ToString() + ",";
                    }
                }
                cir.CarInOutRecord_ISFulfill = false;
                cir.CarInOutRecord_Time = CommonalityEntity.GetServersTime();
                cir.CarInOutRecord_Abnormal = "正常";
                cir.CarInOutRecord_State = "启动";
                cir.CarInOutRecord_DrivewayStrategyS = strstrategy.TrimEnd(',');
                cir.CarInOutRecord_CarInfo_ID = carinfo_id;
                cir.CarInOutRecord_Update = false;
                cir.CarInOutRecord_Driveway_ID = Convert.ToInt32(LinQBaseDao.Query("select DrivewayStrategy_Driveway_ID from DrivewayStrategy where DrivewayStrategy.DrivewayStrategy_ID=" + cir.CarInOutRecord_DrivewayStrategy_ID).Tables[0].Rows[0]["DrivewayStrategy_Driveway_ID"]);
                if (issort)
                {
                    cir.CarInOutRecord_Sort = "有序";
                }
                else
                {
                    cir.CarInOutRecord_Sort = "无序";
                }

                string CarInOutRecordSql = "Insert into CarInOutRecord(CarInOutRecord_CarInfo_ID,CarInOutRecord_DrivewayStrategy_ID,CarInOutRecord_ISFulfill,CarInOutRecord_Time,CarInOutRecord_Abnormal,CarInOutRecord_State,CarInOutRecord_DrivewayStrategyS,CarInOutRecord_Sort,CarInOutRecord_Update,CarInOutRecord_Driveway_ID,CarInOutRecord_InCheck,CarInOutRecord_Remark) values(" + cir.CarInOutRecord_CarInfo_ID + "," + cir.CarInOutRecord_DrivewayStrategy_ID + ",'" + cir.CarInOutRecord_ISFulfill + "','" + cir.CarInOutRecord_Time + "','正常','启动','" + cir.CarInOutRecord_DrivewayStrategyS + "','" + cir.CarInOutRecord_Sort + "','" + cir.CarInOutRecord_Update + "'," + cir.CarInOutRecord_Driveway_ID + ",'否','" + Strategy_DriSName + "')";
                CarInOutRecordSql = CarInOutRecordSql + " select @@identity";
                int CarInOutRecord_ID = int.Parse(LinQBaseDao.GetSingle(CarInOutRecordSql).ToString());
                #endregion

                #region  业务记录进门授权
                //BusinessRecord busr = new BusinessRecord();
                //busr.BusinessRecord_CarInOutRecord_ID = CarInOutRecord_ID;
                //busr.BusinessRecord_UnloadEmpower = 1;
                //busr.BusinessRecord_Type = "进门授权";
                //busr.BusinessRecord_State = "启动";
                ////新增一条记录
                //LinQBaseDao.InsertOne<BusinessRecord>(new DCCarManagementDataContext(), busr);
                //string BusinessRecordsql = "select BusinessRecord_ID from BusinessRecord where BusinessRecord_CarInOutRecord_ID=" + CarInOutRecord_ID;
                //int BusinessRecord_ID = int.Parse(LinQBaseDao.GetSingle(BusinessRecordsql).ToString());
                #endregion

                #region 添加关联表信息
                string staid = ""; bool istrue = false;
                if (!string.IsNullOrEmpty(CommonalityEntity.ICC2))
                {
                    dt = LinQBaseDao.Query(" select StaffInfo_ID from StaffInfo where StaffInfo_ICCard_ID in (select ICCard_ID from ICCard where ICCard_Value='" + CommonalityEntity.ICC2 + "')").Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        staid = dt.Rows[0]["StaffInfo_ID"].ToString();
                        string sfCrSql = "Insert into StaffInfo_CarInfo values(" + staid + "," + smallTicket_ID + ")";
                        sfCrSql = sfCrSql + " select @@identity";
                        int sfCrID = int.Parse(LinQBaseDao.GetSingle(sfCrSql).ToString());
                        istrue = true;
                    }
                }
                if (!istrue)
                {
                    if (string.IsNullOrEmpty(CommonalityEntity.CarIC))
                    {
                        dt = LinQBaseDao.Query(" select Car_StaffInfo_IDS from Car where Car_ICCard_ID in (select ICCard_ID from ICCard where ICCard_Value='" + CommonalityEntity.CarIC + "')").Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            staid = dt.Rows[0]["Car_StaffInfo_IDS"].ToString();
                            string sfCrSql = "Insert into StaffInfo_CarInfo values(" + staid + "," + smallTicket_ID + ")";
                            sfCrSql = sfCrSql + " select @@identity";
                            int sfCrID = int.Parse(LinQBaseDao.GetSingle(sfCrSql).ToString());
                        }
                    }
                }

                #endregion

                #region 判断是否添加成功
                if (sortNumberInfo_ID > 0 && smallTicket_ID > 0 && carinfo_id > 0 && CarInOutRecord_ID != -1)
                {
                    CheckProperties.CheckCarInOutRecordISCompleteMethod();
                    CommonalityEntity.WriteLogData("新增", "新增：" + CheckProperties.dt.Rows[0]["CarInfo_Name"] + "车辆通行总记录", CommonalityEntity.USERNAME);//添加操作日志
                }
                #endregion
            }
        }

        private bool ISadd()
        {
            try
            {
                if (CheckProperties.dt.Rows[0]["SortNumberInfo_TongXing"].ToString() == "待通行")
                {
                    return false;
                }
            }
            catch (Exception)
            {

            }
            return true;
        }
        /// <summary>
        /// 只登记一次车辆是否登记,没有登记信息则添加
        /// </summary>
        private void ISAddCarInfo()
        {
            if (!CommonalityEntity.ISDengji && CommonalityEntity.Car_ISRegister)
            {
                string sql = "Select * from Car where Car_Name='" + CommonalityEntity.CarNO + "'";
                Car car = new Car();
                car = LinQBaseDao.GetItemsForListing<Car>(sql).FirstOrDefault();

                sql = "select * from CarType where CarType_ID=" + car.Car_CarType_ID;
                CarType cartype = new CarType();
                cartype = LinQBaseDao.GetItemsForListing<CarType>(sql).FirstOrDefault();

                DataTable DrivewayStrategy_IDDT = LinQBaseDao.Query("select DrivewayStrategy_ID,DrivewayStrategy_Sort,DrivewayStrategy_Driveway_ID,DrivewayStrategy_Name from DrivewayStrategy where DrivewayStrategy_State='启动' and  DrivewayStrategy_Name ='" + cartype.CarType_DriSName + "'  order by DrivewayStrategy_Sort ").Tables[0];
                DataTable driiddt = LinQBaseDao.Query("select DrivewayStrategy_ID,DrivewayStrategy_Sort,DrivewayStrategy_Driveway_ID,DrivewayStrategy_Name from DrivewayStrategy where DrivewayStrategy_State='启动' and  DrivewayStrategy_Name in (select Car_DriSName from Car  where Car_Name='" + CommonalityEntity.CarNO + "') order by DrivewayStrategy_Sort ").Tables[0];
                if (driiddt.Rows.Count > 0)
                {
                    DrivewayStrategy_IDDT = driiddt;
                }
                if (DrivewayStrategy_IDDT.Rows.Count <= 0)
                {
                    MessageBox.Show("该车辆没有配通行策略！", "提示");
                    return;
                }
                string WarrantyID = CommonalityEntity.Driveway_ID.ToString();
                if (CommonalityEntity.isDriState)
                {
                    WarrantyID = CommonalityEntity.DriWarrantyID;
                }
                Strategy_DriSName = DrivewayStrategy_IDDT.Rows[0]["DrivewayStrategy_Name"].ToString();
                for (int i = 0; i < DrivewayStrategy_IDDT.Rows.Count; i++)
                {
                    if (DrivewayStrategy_IDDT.Rows[i]["DrivewayStrategy_Driveway_ID"].ToString() == WarrantyID)
                    {
                        CommonalityEntity.DrivewayStrategy_ID = Convert.ToInt32(DrivewayStrategy_IDDT.Rows[i]["DrivewayStrategy_ID"].ToString());
                        CommonalityEntity.IsUpdatedri = false;
                        break;
                    }
                }
                if (!ISadd())
                {
                    return;
                }
                DataTable dt;

                #region 车辆信息

                CarInfo carInfo = new CarInfo();
                carInfo.CarInfo_CarType_ID = car.Car_CarType_ID;
                carInfo.CarInfo_Name = car.Car_Name;
                carInfo.CarInfo_State = "启动";
                carInfo.CarInfo_Remark = car.Car_Remark;
                carInfo.CarInfo_CustomerInfo_ID = car.Car_CustomerInfo_ID;
                carInfo.CarInfo_Operate = CommonalityEntity.USERNAME;
                carInfo.CarInfo_Car_ID = car.Car_ID;
                string carSql = "";

                if (carInfo.CarInfo_CustomerInfo_ID != null)
                {
                    carSql = "insert into CarInfo(CarInfo_CustomerInfo_ID,CarInfo_CarType_ID,CarInfo_Name,CarInfo_State,CarInfo_Bail,CarInfo_Time,CarInfo_Remark,CarInfo_Operate,CarInfo_Car_ID) values(" + carInfo.CarInfo_CustomerInfo_ID + "," + carInfo.CarInfo_CarType_ID + ",'" + carInfo.CarInfo_Name + "','启动','False',getdate(),'" + carInfo.CarInfo_Remark + "','" + carInfo.CarInfo_Operate + "'," + carInfo.CarInfo_Car_ID + ")";
                }
                else
                {
                    carSql = "insert into CarInfo(CarInfo_CustomerInfo_ID,CarInfo_CarType_ID,CarInfo_Name,CarInfo_State,CarInfo_Bail,CarInfo_Time,CarInfo_Remark,CarInfo_Operate,CarInfo_Car_ID) values(NULL," + carInfo.CarInfo_CarType_ID + ",'" + carInfo.CarInfo_Name + "','启动','False',getdate(),'" + carInfo.CarInfo_Remark + "','" + carInfo.CarInfo_Operate + "'," + carInfo.CarInfo_Car_ID + ")";
                }

                carSql = carSql + " select @@identity";
                int carinfo_id = int.Parse(LinQBaseDao.GetSingle(carSql).ToString());//得到当前的车辆编号
                #endregion

                #region 登记图片信息
                string picSql = "";
                picSql = "Insert into CarPic(CarPic_CarInfo_ID,CarPic_State,CarPic_Add,CarPic_Type,CarPic_Time,CarPic_Match) values(" + carinfo_id + ",'启动','" + car.Car_AddPic + "','车辆登记照片',getdate(),'匹配')";
                picSql = picSql + " select @@identity";
                LinQBaseDao.GetSingle(picSql).ToString();//得到当前的图片编号

                #endregion

                #region 进出凭证信息
                SmallTicket stk = new SmallTicket();
                string icid = "", count = "", hour = "";
                icid = CommonalityEntity.CheckICValue(CommonalityEntity.ICC1, CommonalityEntity.ICC2, out count, out hour);
                if (string.IsNullOrEmpty(count))
                {
                    stk.SmallTicket_Allowcount = 1;
                }
                else
                {
                    stk.SmallTicket_Allowcount = int.Parse(count);
                }
                if (string.IsNullOrEmpty(hour))
                {
                    stk.SmallTicket_Allowhour = 0;
                }
                else
                {
                    stk.SmallTicket_Allowhour = int.Parse(hour);
                }
                stk.SmallTicket_CarInfo_ID = carinfo_id;
                if (!string.IsNullOrEmpty(icid))
                {
                    stk.SmallTicket_Type = "IC卡";
                    stk.SmallTicket_ICCard_ID = Convert.ToInt32(icid);
                }
                stk.SmallTicket_Position_ID = SystemClass.PositionID;
                stk.SmallTicket_PrintNumber = "";
                stk.SmallTicket_Remark = "";
                stk.SmallTicket_State = "有效";
                stk.SmallTicket_Time = CommonalityEntity.GetServersTime();
                string SmalltSql = "insert into SmallTicket values(" + stk.SmallTicket_ICCard_ID + ",'" + stk.SmallTicket_Serialnumber + "'," + stk.SmallTicket_Position_ID + "," + stk.SmallTicket_CarInfo_ID + ",'" + stk.SmallTicket_Time + "','" + stk.SmallTicket_SortNumber + "','" + stk.SmallTicket_PrintNumber + "','" + stk.SmallTicket_State + "','" + stk.SmallTicket_Type + "','" + stk.SmallTicket_Allowcounted + "','" + stk.SmallTicket_Allowcount + "','" + stk.SmallTicket_Allowhour + "','" + stk.SmallTicket_Remark + "')";
                SmalltSql = SmalltSql + " select @@identity";
                int smallTicket_ID = int.Parse(LinQBaseDao.GetSingle(SmalltSql).ToString());//得到当前的车辆编号
                #endregion

                #region 排队信息
                //添加排队信息
                string sort_Sql = "insert into SortNumberInfo( SortNumberInfo_SmallTicket_ID,SortNumberInfo_Time,SortNumberInfo_Reasons,SortNumberInfo_Operate,SortNumberInfo_Type,SortNumberInfo_PositionValue,SortNumberInfo_CarTypeValue, SortNumberInfo_TongXing,SortNumberInfo_DrivewayValue,SortNumberInfo_State,SortNumberInfo_CallTime,SortNumberInfo_ISEffective,SortNumberInfo_ISNoteRecord,SortNumberInfo_SortValue,SortNumberInfo_CallCount,SortNumberInfo_SMSCount,SortNumberInfo_LEDCount) values('" + smallTicket_ID + "',getdate(),'进厂生成','" + CommonalityEntity.USERNAME + "','系统生成','" + SystemClass.PosistionValue + "','" + cartype.CarType_Value + "','待通行','" + CommonalityEntity.Driveway_Value + "','启动','1990-01-01',1,0,0,0,0,0)";
                sort_Sql = sort_Sql + " select @@identity";
                int sortNumberInfo_ID = int.Parse(LinQBaseDao.GetSingle(sort_Sql).ToString());
                #endregion

                #region 添加通行总记录
                CarInOutRecord cir = new CarInOutRecord();
                string strstrategy = "";
                bool issort = false;
                cir.CarInOutRecord_DrivewayStrategy_ID = Convert.ToInt32(DrivewayStrategy_IDDT.Rows[0]["DrivewayStrategy_ID"].ToString());

                for (int i = 0; i < DrivewayStrategy_IDDT.Rows.Count; i++)
                {
                    if (Convert.ToInt32(DrivewayStrategy_IDDT.Rows[i]["DrivewayStrategy_Sort"].ToString()) == 1)
                    {
                        issort = false;
                        strstrategy += DrivewayStrategy_IDDT.Rows[i]["DrivewayStrategy_ID"].ToString() + ",";
                    }
                    else
                    {
                        issort = true;
                        strstrategy += DrivewayStrategy_IDDT.Rows[i]["DrivewayStrategy_ID"].ToString() + ",";
                    }
                }
                cir.CarInOutRecord_ISFulfill = false;
                cir.CarInOutRecord_Abnormal = "正常";
                cir.CarInOutRecord_State = "启动";
                cir.CarInOutRecord_DrivewayStrategyS = strstrategy.TrimEnd(',');
                cir.CarInOutRecord_CarInfo_ID = carinfo_id;
                cir.CarInOutRecord_Update = false;
                cir.CarInOutRecord_Driveway_ID = Convert.ToInt32(LinQBaseDao.Query("select DrivewayStrategy_Driveway_ID from DrivewayStrategy where DrivewayStrategy.DrivewayStrategy_ID=" + cir.CarInOutRecord_DrivewayStrategy_ID).Tables[0].Rows[0]["DrivewayStrategy_Driveway_ID"]);
                if (issort)
                {
                    cir.CarInOutRecord_Sort = "有序";
                }
                else
                {
                    cir.CarInOutRecord_Sort = "无序";
                }

                string CarInOutRecordSql = "Insert into CarInOutRecord(CarInOutRecord_CarInfo_ID,CarInOutRecord_DrivewayStrategy_ID,CarInOutRecord_ISFulfill,CarInOutRecord_Time,CarInOutRecord_Abnormal,CarInOutRecord_State,CarInOutRecord_DrivewayStrategyS,CarInOutRecord_Sort,CarInOutRecord_Update,CarInOutRecord_Driveway_ID,CarInOutRecord_InCheck,CarInOutRecord_Remark) values(" + cir.CarInOutRecord_CarInfo_ID + "," + cir.CarInOutRecord_DrivewayStrategy_ID + ",'" + cir.CarInOutRecord_ISFulfill + "',getdate(),'正常','启动','" + cir.CarInOutRecord_DrivewayStrategyS + "','" + cir.CarInOutRecord_Sort + "','" + cir.CarInOutRecord_Update + "'," + cir.CarInOutRecord_Driveway_ID + ",'否','" + Strategy_DriSName + "')";
                CarInOutRecordSql = CarInOutRecordSql + " select @@identity";
                int CarInOutRecord_ID = int.Parse(LinQBaseDao.GetSingle(CarInOutRecordSql).ToString());
                #endregion

                #region  业务记录进门授权
                //BusinessRecord busr = new BusinessRecord();
                //busr.BusinessRecord_CarInOutRecord_ID = CarInOutRecord_ID;
                //busr.BusinessRecord_UnloadEmpower = 1;
                //busr.BusinessRecord_Type = "进门授权";
                //busr.BusinessRecord_State = "启动";
                ////新增一条记录
                //LinQBaseDao.InsertOne<BusinessRecord>(new DCCarManagementDataContext(), busr);
                //string BusinessRecordsql = "select BusinessRecord_ID from BusinessRecord where BusinessRecord_CarInOutRecord_ID=" + CarInOutRecord_ID;
                //int BusinessRecord_ID = int.Parse(LinQBaseDao.GetSingle(BusinessRecordsql).ToString());
                #endregion

                #region 添加关联表信息
                string staid = "";
                if (!string.IsNullOrEmpty(CommonalityEntity.ICC2))
                {
                    dt = LinQBaseDao.Query(" select StaffInfo_ID from StaffInfo where StaffInfo_ICCard_ID in (select ICCard_ID from ICCard where ICCard_Value='" + CommonalityEntity.ICC2 + "')").Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        staid = dt.Rows[0]["StaffInfo_ID"].ToString();
                        string sfCrSql = "Insert into StaffInfo_CarInfo values(" + staid + "," + smallTicket_ID + ")";
                        sfCrSql = sfCrSql + " select @@identity";
                        int sfCrID = int.Parse(LinQBaseDao.GetSingle(sfCrSql).ToString());
                    }
                }
                #endregion

                #region 判断是否添加成功
                if (sortNumberInfo_ID > 0 && smallTicket_ID > 0 && carinfo_id > 0 && CarInOutRecord_ID != -1)
                {
                    CheckProperties.CheckCarInOutRecordISCompleteMethod();
                    CommonalityEntity.WriteLogData("新增", "新增：" + CheckProperties.dt.Rows[0]["CarInfo_Name"] + "车辆通行总记录", CommonalityEntity.USERNAME);//添加操作日志
                }
                #endregion
            }
        }

        /// <summary>
        /// 显示车辆信息
        /// </summary>
        private void CarInformationMethod()
        {
            try
            {
                DataTable dt = CheckProperties.dt;
                if (!CommonalityEntity.SerialnumberICbool)
                {
                    CommonalityEntity.Serialnumber = dt.Rows[0]["SmallTicket_Serialnumber"].ToString();
                }
                txt_QueueNumber.Text = dt.Rows[0]["SmallTicket_SortNumber"].ToString(); // 扫描小票车辆排队序号：

                //txt_Gate.Text = dt.Rows[0]["Position_Name"].ToString(); //扫描小票进门门岗：
                txt_CurrentGate.Text = SystemClass.PositionName + CommonalityEntity.Driveway_Name;// 当前门岗位置：
                txt_SerialNumber.Text = CommonalityEntity.Serialnumber;// 扫描小票车辆排队流水号：
                CommonalityEntity.Car_Type = dt.Rows[0]["CarType_Name"].ToString();//车辆类型
                CommonalityEntity.Car_Type_ID = dt.Rows[0]["CarInfo_CarType_ID"].ToString();
                txt_Cartype.Text = CommonalityEntity.Car_Type;
                cartypename = txt_Cartype.Text;

                txt_TransportCompany.Text = dt.Rows[0]["CustomerInfo_Name"].ToString();//运输公司
                txt_Carnumber.Text = dt.Rows[0]["CarInfo_Name"].ToString();//车牌号
                txt_CarryCargo.Text = dt.Rows[0]["CarInfo_Weight"].ToString();//运载货物
                txt_GetReadyLoadingWeight.Text = dt.Rows[0]["CarInfo_Carriage"].ToString();//准备装货重量
                txt_LoadingHeight.Text = dt.Rows[0]["CarInfo_Height"].ToString();//装车高度

                //是否有委托书
                bool iscarriage = false;
                if (dt.Rows[0]["CarInfo_Bail"] != null)
                {
                    iscarriage = Convert.ToBoolean(dt.Rows[0]["CarInfo_Bail"].ToString());
                }
                chb_YesNOEntrustbook.Checked = iscarriage;

                txt_EstablishTime.Text = dt.Rows[0]["CarInfo_Time"].ToString();//创建时间
                txt_EstablishPeople.Text = dt.Rows[0]["CarInfo_Operate"].ToString();//创建人
                txt_DrivingLicenseNumber.Text = dt.Rows[0]["CarInfo_Type"].ToString();
                txt_DriverName.Text = dt.Rows[0]["StaffInfo_Name"].ToString();
                txt_DriverTel.Text = dt.Rows[0]["StaffInfo_Phone"].ToString();
            }
            catch (Exception ex)
            {
                CommonalityEntity.WriteTextLog("CarInformationForm.CarInformationMethod()" + ex.Message.ToString());
            }
        }


        private PictureBox pb;

        /// <summary>
        /// 显示图片信息
        /// </summary>
        public void ShowImage(object obj)
        {
            try
            {
                int i = 0;
                if (CommonalityEntity.listCarPicEffective.Count > 0)
                {
                    foreach (string pathStr in CommonalityEntity.listCarPicEffective)
                    {
                        if (i < 2)
                        {
                            string fileName = pathStr.Substring(pathStr.LastIndexOf('\\') + 1);
                            string fileKind = fileName.Substring(fileName.LastIndexOf('.') + 1);
                            if (fileKind.ToLower() != "jpg" && fileKind.ToLower() != "jpeg" && fileKind.ToLower() != "png" && fileKind.ToLower() != "gif" && fileKind.ToLower() != "bmp")
                            {
                                continue;
                            }
                            if (i == 0)
                            {
                                pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage; ;
                                pictureBox3.MouseHover += new System.EventHandler(this.pbInImageOne_MouseHover);
                                pictureBox3.MouseLeave += new System.EventHandler(this.pbInImageOne_MouseLeave);
                                pictureBox3.Tag = SystemClass.BaseFile + "Car" + SystemClass.PosistionValue + CommonalityEntity.Driveway_Value + "\\" + pathStr.ToString();
                                pictureBox3.ImageLocation = SystemClass.BaseFile + "Car" + SystemClass.PosistionValue + CommonalityEntity.Driveway_Value + "\\" + pathStr.ToString();
                            }
                            else
                            {
                                pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage; ;
                                pictureBox4.MouseHover += new System.EventHandler(this.pbInImageOne_MouseHover);
                                pictureBox4.MouseLeave += new System.EventHandler(this.pbInImageOne_MouseLeave);
                                pictureBox4.Tag = SystemClass.BaseFile + "Car" + SystemClass.PosistionValue + CommonalityEntity.Driveway_Value + "\\" + pathStr.ToString();
                                pictureBox4.ImageLocation = SystemClass.BaseFile + "Car" + SystemClass.PosistionValue + CommonalityEntity.Driveway_Value + "\\" + pathStr.ToString();
                            }
                            i++;
                        }
                    }
                }
                DataTable dtc = LinQBaseDao.Query(" select CarPic_Add from CarPic where CarPic_Type='车辆登记照片'and CarPic_CarInfo_ID=" + CheckProperties.dt.Rows[0]["CarInfo_ID"].ToString() + " order by CarPic_ID desc").Tables[0];
                if (dtc.Rows.Count > 0)
                {
                    string Dimage = dtc.Rows[0][0].ToString();
                    pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                    pictureBox2.MouseHover += new System.EventHandler(this.pbInImageOne_MouseHover);
                    pictureBox2.MouseLeave += new System.EventHandler(this.pbInImageOne_MouseLeave);
                    pictureBox2.ImageLocation = SystemClass.SaveFile + Dimage;
                    pictureBox2.Tag = SystemClass.SaveFile + Dimage;
                }
                else
                {
                    CheckMethod.AbnormalInformation += "车辆没有登记照片！\r\n";
                }
            }
            catch (Exception ex)
            {
                common.WriteTextLog("CarInformationForm.ShowImage()" + ex.Message.ToString());
            }

        }

        #region 图片放大缩小事件
        /// <summary>
        /// 鼠标离开控件可见部分时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbInImageOne_MouseLeave(object sender, EventArgs e)
        {
            ShowD();
        }
        /// <summary>
        /// 鼠标悬停一定时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbInImageOne_MouseHover(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    PictureBox pic = sender as PictureBox;
                    if (pic.Tag != null)
                    {
                        ShowD(pic.Tag.ToString());
                    }
                }
            }
            catch
            {
                common.WriteTextLog("CarInformationForm.pbInImageOne_MouseHover()" + "");
            }
        }
        public Bitmap b = null;
        public void ShowD(string FileName)
        {
            groupBox1.BringToFront();
            b = new Bitmap(FileName);
            pictureBox1.Image = b;
            groupBox1.Visible = true;

        }
        /// <summary>
        /// 改变图片大小(放大)
        /// </summary>
        /// <param name="pb"></param>
        public void ShowD()
        {
            try
            {
                groupBox1.Visible = false;
                b.Dispose();
                //移至底层
                //groupBox1.SendToBack();
            }
            catch
            {
                //common.WriteTextLog("ShowD()" + "");
            }
        }
        #endregion

        /// <summary>
        /// 刷卡放行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void timer_Tick()
        {
            try
            {
                GetPhoto();//获取照片
                //进厂
                if (ISInOut)
                {
                    if (SerialnumberICbool)//小票
                    {
                        if (OpenDoor())
                        {
                            DateTime statetime = DateTime.Now;
                            CarRecordLog("已进厂");
                            mf.OpenDoor((short)openshort);
                            UpLoad(null);
                            if ((DateTime.Now - statetime).TotalSeconds > 5)
                            {
                                CommonalityEntity.WriteTextLog(DTCheckProperties.Rows[0]["CarInfo_Name"].ToString() + "验证到卡后的放行时间为：" + (DateTime.Now - statetime).TotalSeconds);
                            }
                            CommonalityEntity.isRestFXTwo = false;
                            // MessageBox.Show(this, Driveway_Name + "放行成功");
                            MessageBoxForm mbf = new MessageBoxForm(Driveway_Name + "放行成功");
                            mbf.Show();
                            Car_ISRegister = false;
                            strCardNo = "";
                            ICC1 = "";
                            ICC2 = "";
                            mf.Dispose();
                            this.Close();
                        }

                    }
                    else//IC卡
                    {
                        if (OpenDoor())
                        {
                            DateTime statetime = DateTime.Now;
                            CarRecordLog("已进厂");
                            mf.OpenDoor((short)openshort);
                            UpLoad(null);
                            CommonalityEntity.isRestFXTwo = false;
                            if ((DateTime.Now - statetime).TotalSeconds > 5)
                            {
                                CommonalityEntity.WriteTextLog(DTCheckProperties.Rows[0]["CarInfo_Name"].ToString() + "验证到卡后的放行时间为：" + (DateTime.Now - statetime).TotalSeconds);
                            }
                            // MessageBox.Show(this, Driveway_Name + "放行成功");
                            MessageBoxForm mbf = new MessageBoxForm(Driveway_Name + "放行成功");
                            mbf.Show();
                            Car_ISRegister = false;
                            strCardNo = "";
                            ICC1 = "";
                            ICC2 = "";
                            mf.Dispose();
                            this.Close();
                        }
                    }
                }
                else //出厂
                {
                    if (SerialnumberICbool)//小票
                    {
                        if (OpenDoor())
                        {
                            DateTime statetime = DateTime.Now;
                            CarRecordLog("已出厂");
                            mf.OpenDoor((short)openshort);
                            UpLoad(null);
                            CommonalityEntity.isRestFXTwo = false;
                            if ((DateTime.Now - statetime).TotalSeconds > 5)
                            {
                                CommonalityEntity.WriteTextLog(DTCheckProperties.Rows[0]["CarInfo_Name"].ToString() + "验证到卡后的放行时间为：" + (DateTime.Now - statetime).TotalSeconds);
                            }
                            // MessageBox.Show(this, Driveway_Name + "放行成功");
                            MessageBoxForm mbf = new MessageBoxForm(Driveway_Name + "放行成功");
                            mbf.Show();
                            Car_ISRegister = false;
                            strCardNo = "";
                            ICC1 = "";
                            ICC2 = "";
                            mf.Dispose();
                            this.Close();
                        }
                    }
                    else//IC卡
                    {
                        if (OpenDoor())
                        {
                            DateTime statetime = DateTime.Now;
                            CarRecordLog("已出厂");
                            mf.OpenDoor((short)openshort);
                            UpLoad(null);
                            CommonalityEntity.isRestFXTwo = false;
                            if ((DateTime.Now - statetime).TotalSeconds > 5)
                            {
                                CommonalityEntity.WriteTextLog(DTCheckProperties.Rows[0]["CarInfo_Name"].ToString() + "验证到卡后的放行时间为：" + (DateTime.Now - statetime).TotalSeconds);
                            }
                            // MessageBox.Show(this, Driveway_Name + "放行成功");
                            MessageBoxForm mbf = new MessageBoxForm(Driveway_Name + "放行成功");
                            mbf.Show();
                            strCardNo = "";
                            ICC1 = "";
                            ICC2 = "";
                            Car_ISRegister = false;
                            mf.Dispose();
                            this.Close();
                        }
                    }
                }
            }
            catch
            {
                strCardNo = "";
                MessageBox.Show(this, "放行失败");
                Car_ISRegister = false;
                CommonalityEntity.WriteTextLog("CarInformationForm.timer_Tick()");
            }

        }

        /// <summary>
        /// 关闭清空图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarInformationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {

                if (picList.Count > 0)
                {
                    foreach (PictureBox item in picList)
                    {
                        item.ImageLocation = "";
                        item.ImageLocation = null;
                        item.Image = null;
                        item.Tag = "";
                        item.Tag = null;
                        item.Controls.Clear();
                        item.Dispose();
                        groupBox1.Controls.Clear();
                        groupBox1.Dispose();

                        this.gbShowImage.Controls.Remove(item);
                        this.gbShowImage.Controls.Clear();
                        this.gbShowImage.Dispose();
                        if (b != null)
                        {
                            b.Dispose();
                        }
                    }
                }
                pictureBox1.Tag = "";
                pictureBox1.ImageLocation = "";
                pictureBox1.ImageLocation = null;
                pictureBox1.Image = null;
                pictureBox1.Controls.Clear();
                pictureBox2.Tag = "";
                pictureBox2.ImageLocation = "";
                pictureBox2.ImageLocation = null;
                pictureBox2.Image = null;
                pictureBox2.Controls.Clear();
                pictureBox2.Dispose();
                CommonalityEntity.isRestFXTwo = false;
                CarInformationFormTwo.cinfo = null;
                CommonalityEntity.CarValue.Remove(Driveway_Value);
                GC.Collect();

            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 添加车辆图片信息
        /// </summary>
        public void AddCarPicture(int cardinfo_ID, string path, string item, string inout)
        {
            try
            {
                if (CheckProperties.dt.Rows.Count > 0)//
                {
                    CarPic cpc = new CarPic();
                    cpc.CarPic_CarInfo_ID = cardinfo_ID;
                    if (!string.IsNullOrEmpty(CarInOutInfoRecord_ID))
                    {
                        cpc.CarPic_CarInOutInfoRecord_ID = int.Parse(CarInOutInfoRecord_ID);
                    }
                    cpc.CarPic_State = "启动";
                    cpc.CarPic_Time = CommonalityEntity.GetServersTime();
                    if (inout == "进")
                    {
                        cpc.CarPic_Type = "进厂";
                    }
                    else
                    {
                        cpc.CarPic_Type = "出厂";
                    }
                    cpc.CarPic_Add = path + item;
                    string CpSql = item;
                    if (item.Length == 21)
                    {
                        CpSql = "select * from Camera where Camera_Driveway_ID in (select Driveway_ID from Driveway where Driveway_Value='" + Driveway_Name + "' and Driveway_Position_ID in (select Position_ID from Position where Position_Value='" + SystemClass.PosistionValue + "')) and  camera_addCard='" + item.Substring(16, 1) + "'";
                    } if (item.Length == 22)
                    {
                        CpSql = "select * from Camera where Camera_Driveway_ID in (select Driveway_ID from Driveway where Driveway_Value='" + Driveway_Name + "' and Driveway_Position_ID in (select Position_ID from Position where Position_Value='" + SystemClass.PosistionValue + "')) and  camera_addCard='" + item.Substring(16, 2) + "'";
                    }

                    DataTable dt = LinQBaseDao.Query(CpSql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        cpc.CarPic_Seat = dt.Rows[0][0].ToString();
                    }
                    cpc.CarPic_Match = "匹配";
                    cpc.CarPic_Remark = SystemClass.PositionName + Driveway_Name + cpc.CarPic_Type + "照片";
                    LinQBaseDao.InsertOne<CarPic>(new DCCarManagementDataContext(), cpc);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInformationForm.AddCarPicture()");
            }
        }
        /// <summary>
        /// 根据门岗值和通道值清空卡号
        /// </summary>
        /// <param name="DeviceControl_PositionValue">门岗实际值</param>
        /// <param name="DeviceControl_DrivewayValue">通道实际值</param>
        private void ClearDeviceControlDeviceControl_CardNoMethod(string DeviceControl_PositionValue, string DeviceControl_DrivewayValue)
        {
            try
            {
                LinQBaseDao.Query("update DeviceControl set DeviceControl_CardNo=''  where DeviceControl_DrivewayValue='" + DeviceControl_DrivewayValue + "' and DeviceControl_PositionValue='" + DeviceControl_PositionValue + "'");
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInformationForm.ClearDeviceControlDeviceControl_CardNoMethod()");
            }
        }
        /// <summary>
        /// 确认放行按键事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ConfirmationRelease_Click(object sender, EventArgs e)
        {
            try
            {
                string strts = lbl_Prompt.Text;
                if (strts.IndexOf("读取中") >= 0)
                {
                    txtUnusual_Info.Text += "车辆信息读取失败，请重新读取！" + "\r\n";
                    return;
                }
                CommonalityEntity.isRestFXTwo = true;
                this.WindowState = FormWindowState.Minimized;
                btn_ConfirmationRelease.Enabled = false;
                ISRelease = false;
                if (txtUnusual_Info.Text.Trim() != "" || !string.IsNullOrEmpty(txtUnusual_Info.Text.Trim()))
                {
                    CommonalityEntity.ADDUnusualRecord(2, "异常放行", txtUnusual_Info.Text.Trim(), CommonalityEntity.USERNAME, Convert.ToInt32(strCarInOutRecord_CarInfo_ID));

                    MessageBox.Show(this, "车辆存在异常信息，不能正常放行");
                    return;
                }
                strtime = "确认放行：" + CommonalityEntity.GetServersTime() + ";";
                timer_Tick();
            }
            catch
            {
                CommonalityEntity.isRestFXTwo = false;
                CommonalityEntity.WriteTextLog("CarInformationForm.btn_ConfirmationRelease_Click()");
            }
        }
        private void GetPhoto()
        {
            //使用该方法，得到有效的照片信息，使用前需要为CheckProperties.ce.carType_ID(车辆类型编号),SystemClass.PositionID(门岗编号),SystemClass.BaseFile(图片临时路径),SystemClass.PosistionValue(门岗值)赋值，在为该方法配置管控信息时，其默认值为图片的有效时间，返回CheckProperties.ce.Imagelist图片名称集合,无图片则为null
            try
            {
                listCarPicEffective = new List<string>();///存放有效图片
                listCarPic = new List<string>();//存放无效图片
                string[] image = null;

                //根据门岗获取当前门岗的拍照通道值
                string path = "";
                //组合文件夹名称
                path = SystemClass.BaseFile + "Car" + SystemClass.PosistionValue + Driveway_Value + "\\";

                //获取指定文件夹下的图片
                image = Directory.GetFiles(path);
                var images = from m in image orderby m descending select m;
                //根据通道编号得到该通道下的摄像机值
                DataSet dataset = LinQBaseDao.Query("select * from Camera where Camera_Driveway_id=" + Driveway_ID + "");
                string stc = "";
                string stc1 = "";

                if (dataset.Tables[0].Rows.Count > 0)
                {
                    if (dataset.Tables[0].Rows.Count == 1)
                    {
                        stc = dataset.Tables[0].Rows[0]["Camera_AddCard"].ToString();
                    }
                    else
                    {
                        stc = dataset.Tables[0].Rows[0]["Camera_AddCard"].ToString();
                        stc1 = dataset.Tables[0].Rows[1]["Camera_AddCard"].ToString();
                    }
                }
                else
                {
                    return;
                }


                //照片有效时间 分钟
                object objDouble;
                if (ISInOut)
                {
                    objDouble = LinQBaseDao.GetSingle("select ControlInfo_Value from ControlInfo where ControlInfo_IDValue='19'");
                }
                else
                {
                    objDouble = LinQBaseDao.GetSingle("select ControlInfo_Value from ControlInfo where ControlInfo_IDValue='26'");
                }
                double doubleMin = 0;
                if (objDouble != null && objDouble != "")
                {
                    doubleMin = double.Parse(objDouble.ToString());
                }
                //验证图片是否有效
                foreach (string pathStr in images)
                {
                    string fileName = pathStr.Substring(pathStr.LastIndexOf('\\') + 1);//图片名称
                    string fileKind = fileName.Substring(fileName.LastIndexOf('.') + 1);//图片后缀
                    //验证是否为图片
                    if (fileKind.ToLower() != "jpg" && fileKind.ToLower() != "jpeg" && fileKind.ToLower() != "png" && fileKind.ToLower() != "gif" && fileKind.ToLower() != "bmp")
                    {
                        continue;
                    }
                    //保存所有的图片名称
                    // CheckProperties.ce.AllImageList.Add(fileName);
                    string mi = ImageFile.GetTime(fileName.ToString());//获取时间差

                    if (double.Parse(mi) <= doubleMin)//判断是否过期
                    {
                        if (fileName.Length == 21)//判断图片名称字符长度
                        {

                            string stf = fileName.Substring(16, 1);
                            if (stc == stf || stc1 == stf)
                            {
                                listCarPicEffective.Add(fileName);
                            }
                            else
                            {
                                listCarPic.Add(fileName);//添加过期图片
                            }
                        }
                        else if (fileName.Length == 22)
                        {
                            string stf = fileName.Substring(16, 2);
                            if (stc == stf || stc1 == stf)
                            {
                                listCarPicEffective.Add(fileName);
                            }
                            else
                            {
                                listCarPic.Add(fileName);//添加过期图片
                            }
                        }
                    }
                    else
                    {
                        listCarPic.Add(fileName);//添加过期图片
                    }
                }
            }
            catch (Exception ex)
            {
                CommonalityEntity.WriteTextLog("CheckMethod GetPhoto():" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 道闸(开\关\停\无)
        /// </summary>
        /// <param name="strDeviceControl_DrivewayValue">通道值</param>
        /// <param name="strDeviceControl_OutSate">(开\关\停\无)</param>
        public void DeviceControl_Open_Off_Stop(string strDeviceControl_DrivewayValue, string strDeviceControl_OutSate, bool istrue)
        {
            try
            {
                int s = 0;
                if (istrue)
                {
                    s = 1;
                }
                LinQBaseDao.Query("update DeviceControl set DeviceControl_OutSate='" + strDeviceControl_OutSate + "' ,DeviceControl_ISCardRelease=" + s + " where DeviceControl_DrivewayValue='" + strDeviceControl_DrivewayValue + "' and DeviceControl_PositionValue='" + SystemClass.PosistionValue + "'");
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInformationForm.DeviceControl_Open_Off_Stop()");
            }
        }

        /// <summary>
        ///刷卡记录
        /// </summary>
        public void cardinfoRecord()
        {
            try
            {
                DataTable dt = LinQBaseDao.Query("select top(1) CardInfo_ID from CardInfo where CardInfo_Type='" + ictype + "' and CardInfo_Driveway_ID=" + CommonalityEntity.Driveway_ID + " and CardInfo_ICCard_ID in (select ICCard_ID from ICCard where ICCard_Value='" + icvalue + "' )  order by CardInfo_ID desc ").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string CardInfo_ID = dt.Rows[0][0].ToString();
                    dt = LinQBaseDao.Query("select CarPic_Add from CarPic where CarPic_CarInOutInfoRecord_ID=" + CarInOutInfoRecord_ID).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows.Count == 1)
                        {
                            pathpic1 = dt.Rows[0][0].ToString();
                            pathpic2 = "";
                        }
                        else
                        {
                            pathpic1 = dt.Rows[0][0].ToString();
                            pathpic2 = dt.Rows[1][0].ToString();
                        }
                        LinQBaseDao.Query("update CardInfo set  CardInfo_CarInOutInfoRecord_ID=" + CarInOutInfoRecord_ID + ",  CardInfo_Pic='" + pathpic1 + "',CardInfo_Remark='" + pathpic2 + "' where CardInfo_ID =" + CardInfo_ID);
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInformationForm.DeviceControl_Open_Off_Stop()");
            }
        }

        /// <summary>
        /// 道闸
        /// </summary>
        /// <param name="strDeviceControl_DrivewayValue">通道值</param>
        /// <param name="strDeviceControl_OutSate">(开\关\停\无)</param>
        public void DeviceControl_Open(string strDeviceControl_DrivewayValue, string cardno, string cardtype)
        {
            try
            {
                LinQBaseDao.Query("update DeviceControl set DeviceControl_CardNo='" + cardno + "',DeviceControl_CardType='" + cardtype + "',DeviceControl_ISCardRelease=0  where DeviceControl_DrivewayValue='" + strDeviceControl_DrivewayValue + "' and DeviceControl_PositionValue='" + SystemClass.PosistionValue + "'");
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInformationForm.DeviceControl_Open_Off_Stop()");
            }
        }


        /// <summary> 
        /// 判断是否有放行权限 及相应的操作
        /// </summary>
        /// <returns> true:有放行权限 false:无放行权限</returns>
        public bool IsReleaseJurisdictionMethod(string ICcardNO, string strinout)
        {
            bool rbool = true;
            try
            {
                string str = Driveway_Value;
                string strsql = "select ICCard.ICCard_Permissions,ICCardType.ICCardType_Permissions from ICCard INNER JOIN ICCardType  ICCardType ON ICCard.ICCard_ICCardType_ID = ICCardType.ICCardType_ID where ICCard.ICCard_Value='" + ICcardNO + "'";

                DataTable dtstrsql = new DataTable();
                dtstrsql = LinQBaseDao.Query(strsql).Tables[0];
                if (dtstrsql.Rows.Count <= 0)
                {
                    strmessagebox = "";
                    return false;
                }
                else
                {
                    string strs = dtstrsql.Rows[0]["ICCard_Permissions"].ToString();
                    string strctype = dtstrsql.Rows[0]["ICCardType_Permissions"].ToString();
                    int i = strctype.IndexOf(strinout);
                    bool isions = false;

                    if (i < 0)
                    {
                        rbool = false;
                    }
                    else
                    {
                        DataTable dts = LinQBaseDao.Query("select Position_Name,Driveway_Name from View_DrivewayPosition where Position_Value='" + SystemClass.PosistionValue + "' and Driveway_Value='" + Driveway_Value + "'").Tables[0];
                        if (dts.Rows.Count > 0)
                        {
                            string[] strpostion = strs.Split('.');
                            foreach (var item in strpostion)
                            {
                                int f = item.IndexOf('：');
                                if (f >= 0)
                                {
                                    string strpost = item.Substring(0, f);
                                    if (strpost == dts.Rows[0]["Position_Name"].ToString())
                                    {
                                        string[] strdir = item.Substring(f + 1, item.Length - (f + 1)).Split(',');
                                        foreach (var dir in strdir)
                                        {
                                            if (dir == dts.Rows[0]["Driveway_Name"].ToString())
                                            {
                                                isions = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }

                    if (!isions)
                    {
                        rbool = false;
                    }
                    else
                    {
                        rbool = true;
                    }
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInformationForm.IsReleaseJurisdictionMethod()");
                rbool = false;
            }
            return rbool;
        }

        /// <summary>
        /// 开闸
        /// </summary>
        private bool OpenDoor()
        {
            bool isopentrue = false;
            ClearDeviceControlDeviceControl_CardNoMethod(SystemClass.PosistionValue, Driveway_Value);
            if (rboolIc || EnsureSafetyICHave)
            {
                bool ischeckopen = false;
                if (string.IsNullOrEmpty(txtUnusual_Info.Text.Trim()))
                {
                    DeviceControl_Open_Off_Stop(Driveway_Value, "开", true);
                }
                if (MainForm.dicCard.Count > 0)
                {
                    foreach (KeyValuePair<string, List<CardEntity>> temp in MainForm.dicCard)
                    {
                        string dname = temp.Key;
                        if (dname == Driveway_Value)
                        {
                            foreach (var it in temp.Value)
                            {
                                if (it.CardTyep == "保安卡" || it.CardTyep == "特权卡" || it.CardTyep == "万能卡" || it.CardTyep == "班长卡")
                                {

                                    if (IsReleaseJurisdictionMethod(it.CardNo, "放行"))
                                    {
                                        DeviceControl_Open(Driveway_Value, it.CardNo, it.CardTyep);
                                        ischeckopen = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (ischeckopen)
                    {
                        DataTable dtDeviceControl = LinQBaseDao.Query("select * from DeviceControl where DeviceControl_PositionValue='" + SystemClass.PosistionValue + "' and DeviceControl_DrivewayValue='" + Driveway_Value + "'").Tables[0];
                        if (dtDeviceControl.Rows.Count > 0)
                        {
                            string devIC = dtDeviceControl.Rows[0]["DeviceControl_CardNo"].ToString();
                            string decoutsate = dtDeviceControl.Rows[0]["DeviceControl_OutSate"].ToString();
                            string devCardtype = dtDeviceControl.Rows[0]["DeviceControl_CardType"].ToString();
                            int devAddress = Convert.ToInt32(dtDeviceControl.Rows[0]["DeviceControl_DrivewayAddress"].ToString());
                            bool IScheck = Convert.ToBoolean(dtDeviceControl.Rows[0]["DeviceControl_ISCardRelease"].ToString());
                            ictype = devCardtype;
                            icvalue = devIC;
                            if (!string.IsNullOrEmpty(devIC))
                            {
                                if (decoutsate == "开" || (devCardtype == "保安卡" || devCardtype == "特权卡" || devCardtype == "万能卡" || devCardtype == "班长卡"))
                                {
                                    if (ISRelease)
                                    {
                                        if (devCardtype == "保安卡" || devCardtype == "班长卡")
                                        {
                                            if (IsReleaseJurisdictionMethod(devIC, "异常放行"))
                                            {
                                                if (devAddress > -1 && !IScheck)
                                                {
                                                    isopentrue = opDoorPort(devAddress);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (devAddress > -1 && !IScheck)
                                            {
                                                isopentrue = opDoorPort(devAddress);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (devAddress > -1 && !IScheck)
                                        {
                                            isopentrue = opDoorPort(devAddress);
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
                bool ischeckopen = false;
                DeviceControl_Open_Off_Stop(Driveway_Value, "开", false);
                if (MainForm.dicCard.Count > 0)
                {
                    foreach (KeyValuePair<string, List<CardEntity>> temp in MainForm.dicCard)
                    {
                        string dname = temp.Key;
                        if (dname == Driveway_Value)
                        {
                            if (MainForm.dicCard[Driveway_Value].Count > 0)
                            {
                                foreach (var it in temp.Value)
                                {
                                    if (!string.IsNullOrEmpty(it.CardNo))
                                    {
                                        DeviceControl_Open(Driveway_Value, it.CardNo, it.CardTyep);
                                        ischeckopen = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (ischeckopen)
                    {
                        DataTable dtDeviceControl = LinQBaseDao.Query("select * from DeviceControl where DeviceControl_PositionValue='" + SystemClass.PosistionValue + "' and DeviceControl_DrivewayValue='" + Driveway_Value + "'").Tables[0];
                        if (dtDeviceControl.Rows.Count > 0)
                        {
                            string devIC = dtDeviceControl.Rows[0]["DeviceControl_CardNo"].ToString();
                            string decoutsate = dtDeviceControl.Rows[0]["DeviceControl_OutSate"].ToString();
                            string devCardtype = dtDeviceControl.Rows[0]["DeviceControl_CardType"].ToString();
                            int devAddress = Convert.ToInt32(dtDeviceControl.Rows[0]["DeviceControl_DrivewayAddress"].ToString());
                            bool IScheck = Convert.ToBoolean(dtDeviceControl.Rows[0]["DeviceControl_ISCardRelease"].ToString());
                            ictype = devCardtype;
                            icvalue = devIC;
                            if (!string.IsNullOrEmpty(devIC))
                            {
                                if (decoutsate == "开")
                                {
                                    if (ISRelease)//异常放行
                                    {
                                        if (devCardtype == "保安卡" || devCardtype == "班长卡")
                                        {
                                            if (IsReleaseJurisdictionMethod(devIC, "异常放行"))//判断保安卡权限
                                            {
                                                if (devAddress > -1 && !IScheck)
                                                {
                                                    isopentrue = opDoorPort(devAddress);
                                                }
                                            }
                                        }
                                        else if (devCardtype == "特权卡" || devCardtype == "万能卡")
                                        {
                                            if (devAddress > -1 && !IScheck)
                                            {
                                                isopentrue = opDoorPort(devAddress);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (devAddress > -1 && !IScheck)
                                        {
                                            isopentrue = opDoorPort(devAddress);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        isopentrue = issershuaka();
                    }
                }
                else
                {
                    isopentrue = issershuaka();
                }
            }
            return isopentrue;
        }

        /// <summary>
        /// 判断小票是否刷保安卡
        /// </summary>
        /// <returns></returns>
        private bool issershuaka()
        {
            string driid = LinQBaseDao.GetSingle("select  Driveway_ID from Driveway where Driveway_Value='" + Driveway_Value + "' and Driveway_Position_ID=" + SystemClass.PositionID).ToString();
            bool isopentrue = false;
            bool isfalse = true;
            if (IsUpdatedri)
            {
                if (ISInOut)
                {
                    string sqlMan = "select count(*) from  ManagementStrategyRecord where ManagementStrategyRecord_CarInfo_ID=" + strCarInOutRecord_CarInfo_ID + " and ManagementStrategyRecord_ControlInfo_ID in(select ControlInfo_ID from ControlInfo where ControlInfo_Name='进门是否刷保安卡放行') and ManagementStrategyRecord_State='启动'  and ManagementStrategyRecord_DrivewayStrategy_ID= " + DrivewayStrategy_ID;
                    int dtMan = Convert.ToInt32(LinQBaseDao.GetSingle(sqlMan).ToString());
                    if (dtMan == 0)
                    {
                        isfalse = false;
                    }
                }
                else
                {
                    string sqlMan = "select count(*) from  ManagementStrategyRecord where ManagementStrategyRecord_CarInfo_ID=" + strCarInOutRecord_CarInfo_ID + " and ManagementStrategyRecord_ControlInfo_ID in(select ControlInfo_ID from ControlInfo where ControlInfo_Name='出门是否刷保安卡放行') and ManagementStrategyRecord_State='启动' and ManagementStrategyRecord_DrivewayStrategy_ID= " + DrivewayStrategy_ID;
                    int dtMan = Convert.ToInt32(LinQBaseDao.GetSingle(sqlMan).ToString());
                    if (dtMan == 0)
                    {
                        isfalse = false;
                    }
                }
            }
            else
            {
                if (ISInOut)
                {
                    string sqlMan = "select count(*) from  ManagementStrategy where ManagementStrategy_DriSName='" + Strategy_DriSName + "' and ManagementStrategy_ControlInfo_ID in(select ControlInfo_ID from ControlInfo where ControlInfo_Name='进门是否刷保安卡放行') and ManagementStrategy_State='启动' and ManagementStrategy_DrivewayStrategy_ID= " + DrivewayStrategy_ID;
                    int dtMan = Convert.ToInt32(LinQBaseDao.GetSingle(sqlMan).ToString());
                    if (dtMan == 0)
                    {
                        isfalse = false;
                    }
                }
                else
                {
                    string sqlMan = "select count(*) from  ManagementStrategy where ManagementStrategy_DriSName='" + Strategy_DriSName + "' and ManagementStrategy_ControlInfo_ID in(select ControlInfo_ID from ControlInfo where ControlInfo_Name='出门是否刷保安卡放行') and ManagementStrategy_State='启动' and ManagementStrategy_DrivewayStrategy_ID= " + DrivewayStrategy_ID;
                    int dtMan = Convert.ToInt32(LinQBaseDao.GetSingle(sqlMan).ToString());
                    if (dtMan == 0)
                    {
                        isfalse = false;
                    }
                }
            }
            if (!isfalse)
            {
                DeviceControl_Open_Off_Stop(Driveway_Value, "开", false);
                DataTable dtDeviceControl = LinQBaseDao.Query("select * from DeviceControl where DeviceControl_PositionValue='" + SystemClass.PosistionValue + "' and DeviceControl_DrivewayValue='" + Driveway_Value + "'").Tables[0];
                if (dtDeviceControl.Rows.Count > 0)
                {
                    string devIC = dtDeviceControl.Rows[0]["DeviceControl_CardNo"].ToString();
                    string decoutsate = dtDeviceControl.Rows[0]["DeviceControl_OutSate"].ToString();
                    string devCardtype = dtDeviceControl.Rows[0]["DeviceControl_CardType"].ToString();
                    int devAddress = Convert.ToInt32(dtDeviceControl.Rows[0]["DeviceControl_DrivewayAddress"].ToString());
                    bool IScheck = Convert.ToBoolean(dtDeviceControl.Rows[0]["DeviceControl_ISCardRelease"].ToString());
                    ictype = devCardtype;
                    icvalue = devIC;
                    if (ISRelease)//异常放行
                    {
                        if ((devCardtype == "保安卡" && devIC != "") || (devCardtype == "班长卡" && devIC != ""))
                        {
                            if (IsReleaseJurisdictionMethod(devIC, "异常放行"))//判断保安卡权限
                            {
                                if (devAddress > -1)
                                {
                                    isopentrue = opDoorPort(devAddress);
                                }
                            }
                        }
                        else if (devCardtype == "特权卡" || devCardtype == "万能卡" && devIC != "")
                        {
                            if (devAddress > -1)
                            {
                                isopentrue = opDoorPort(devAddress);
                            }
                        }
                    }
                    else
                    {
                        if (devAddress > -1)
                        {
                            isopentrue = opDoorPort(devAddress);
                        }
                    }
                }
            }
            return isopentrue;
        }
        /// <summary>
        /// 开闸
        /// </summary>
        /// <param name="address"></param>
        private bool opDoorPort(int address)
        {
            openshort = address;
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
                            break;
                        }
                    }
                }
            }
            CommonalityEntity.contoltwo = contio;
            DeviceControl_Open_Off_Stop(Driveway_Value, "关", false);

            return true;
        }

        /// <summary>
        /// 车辆进出厂记录添加
        /// </summary>
        /// <param name="str">已进厂、已出厂</param>
        private void CarRecordLog(string str)
        {
            try
            {
                AddCarInOutInfoRecordMethod();
                ISDrivewayStrategy();
                if (chkchenpin.Checked)
                {
                    boolYesNoCarInOutRecord_ISFulfill = true;
                }
                //ThreadPool.QueueUserWorkItem(new WaitCallback(UpLoad), null);
                //UpLoad(null);
                UpdateCarInOutRecordMethod(str);
                UpSortNumberInfoMethod(str);
                updateSmallTicket(null);

                if (txt_Remarks.Text.Trim() != "")
                {
                    LinQBaseDao.Query("update CarInfo set CarInfo_Remark='" + txt_Remarks.Text.Trim() + "' where CarInfo_ID=" + DTCheckProperties.Rows[0]["CarInfo_ID"].ToString());
                }
                if (chkchenpin.Checked)
                {
                    DataTable dtc = LinQBaseDao.Query("select cartype_id from cartype where cartype_name='内部成品车'").Tables[0];
                    if (dtc.Rows.Count > 0)
                    {
                        LinQBaseDao.Query("update CarInfo set CarInfo_CarType_ID=" + dtc.Rows[0][0].ToString() + " where CarInfo_ID=" + DTCheckProperties.Rows[0]["CarInfo_ID"].ToString());
                        LinQBaseDao.Query("update CarInOutRecord set CarInOutRecord_ISFulfill=1,CarInOutRecord_FulfillTime=GETDATE() where CarInOutRecord_CarInfo_ID=" + DTCheckProperties.Rows[0]["CarInfo_ID"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                CommonalityEntity.WriteTextLog("CarRecordLog异常:" + ex.Message.ToString());
            }
        }


        /// <summary>
        /// 判断当前通行策略是是否是最后进出门策略
        /// </summary>
        private void ISDrivewayStrategy()
        {
            try
            {

                if (DTCheckProperties.Rows[0]["CarInOutRecord_Sort"].ToString() == "有序")
                {
                    string StrategyS = DTCheckProperties.Rows[0]["CarInOutRecord_DrivewayStrategyS"].ToString();
                    string strategysout = StrategyS.Substring(StrategyS.LastIndexOf(',') + 1);//车辆最后通行策略，如果等于则需出门授权
                    if (DrivewayStrategy_ID.ToString() == strategysout)
                    {
                        boolYesNoCarInOutRecord_ISFulfill = true;
                    }
                    else
                    {
                        boolYesNoCarInOutRecord_ISFulfill = false;
                    }
                }
                else
                {
                    //内部车辆的起点在厂内
                    if (Car_ISRegister)
                    {
                        if (ISInOut)
                        {
                            boolYesNoCarInOutRecord_ISFulfill = true;
                        }
                        else
                        {
                            boolYesNoCarInOutRecord_ISFulfill = false;
                        }
                        string StrategyS = DTCheckProperties.Rows[0]["CarInOutRecord_DrivewayStrategyS"].ToString();
                        int iis = StrategyS.LastIndexOf(',');
                        if (iis < 0)
                        {
                            boolYesNoCarInOutRecord_ISFulfill = true;
                        }
                    }
                    else
                    {
                        if (ISInOut)
                        {
                            boolYesNoCarInOutRecord_ISFulfill = false;
                        }
                        else
                        {
                            boolYesNoCarInOutRecord_ISFulfill = true;
                        }
                        string StrategyS = DTCheckProperties.Rows[0]["CarInOutRecord_DrivewayStrategyS"].ToString();
                        int iis = StrategyS.LastIndexOf(',');
                        if (iis < 0)
                        {
                            boolYesNoCarInOutRecord_ISFulfill = true;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 添加过期图片
        /// </summary>
        private void addlistcarpic(string inout, string path, string item)
        {
            try
            {

                CarPic cp = new CarPic();
                cp.CarPic_Match = "未匹配";
                if (inout == "进")
                {
                    cp.CarPic_Type = "进厂";
                }
                else
                {
                    cp.CarPic_Type = "出厂";
                }
                cp.CarPic_State = "启动";
                cp.CarPic_Add = path + item;
                string CpSql = item;
                if (item.Length == 21)
                {
                    CpSql = "select camera_Name from Camera where camera_addCard='" + item.Substring(16, 1) + "'";
                } if (item.Length == 22)
                {
                    CpSql = "select camera_Name from Camera where camera_addCard='" + item.Substring(16, 2) + "'";
                }
                DataTable dt = LinQBaseDao.Query(CpSql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    cp.CarPic_Seat = dt.Rows[0][0].ToString();
                }
                cp.CarPic_Time = CommonalityEntity.GetServersTime();
                cp.CarPic_Remark = SystemClass.PositionName + Driveway_Name + cp.CarPic_Type + "照片";
                LinQBaseDao.InsertOne<CarPic>(new DCCarManagementDataContext(), cp);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 添加车辆通行详细记录
        /// </summary>
        public void AddCarInOutInfoRecordMethod()
        {
            try
            {
                CarInOutInfoRecord cioir = new CarInOutInfoRecord();
                cioir.CarInOutInfoRecord_CarInOutRecord_ID = CommonalityEntity.GetInt(carinoutRid);//车辆通行编号
                cioir.CarInOutInfoRecord_DrivewayStrategy_ID = DrivewayStrategy_ID;//通行策略编号
                cioir.CarInOutInfoRecord_Time = CommonalityEntity.GetServersTime();//车辆通行详细记录时间
                cioir.CarInOutInfoRecord_State = "启动";//车辆通行详细状态(启动、暂停、注销)
                if (txtUnusual_Info.Text.Trim() == "")
                {
                    strAbnormal = "正常";
                }
                else
                {
                    strAbnormal = txtUnusual_Info.Text.Trim();
                }
                string inout = "";
                if (ISInOut)
                {
                    inout = "进";
                }
                else
                {
                    inout = "出";
                }
                cioir.CarInOutInfoRecord_ICType = ictype;
                cioir.CarInOutInfoRecord_ICValue = icvalue;
                if (icvalue != "")
                {
                    object objsta = LinQBaseDao.GetSingle("select StaffInfo_Name from View_StaffInfo_ICCard where ICCard_Value='" + icvalue + "'");
                    if (objsta != null)
                    {
                        cioir.CarInOutInfoRecord_UserName = objsta.ToString();
                    }
                }
                if (ICC2 != "")
                {
                    if (!SerialnumberICbool)
                    {
                        object objsta = LinQBaseDao.GetSingle("select StaffInfo_ID from View_StaffInfo_ICCard where ICCard_Value='" + ICC2 + "'");
                        if (objsta != null)
                        {
                            cioir.CarInOutInfoRecord_StaffInfo_ID = Convert.ToInt32(objsta);
                        }
                    }
                    else
                    {
                        cioir.CarInOutInfoRecord_StaffInfo_ID = Convert.ToInt32(DTCheckProperties.Rows[0]["StaffInfo_ID"].ToString());
                    }
                }
                else
                {
                    cioir.CarInOutInfoRecord_StaffInfo_ID = Convert.ToInt32(DTCheckProperties.Rows[0]["StaffInfo_ID"].ToString());
                }
                cioir.CarInOutInfoRecord_Remark = SystemClass.PositionName + Driveway_Name + inout;
                cioir.CarInOutInfoRecord_Abnormal = strAbnormal;//是否异常
                cioir.CarInOutInfoRecord_DrivewayID = Driveway_ID;//当前通道编号
                cioir.CarInOutInfoRecord_Time = CommonalityEntity.GetServersTime();
                CarInOutInfoRecordDAL.InsertOne(cioir);
                CarInOutInfoRecord_ID = cioir.CarInOutInfoRecord_ID.ToString();

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInformationForm.AddCarInOutInfoRecordMethod()");
            }

        }


        /// <summary>
        /// 修改通行总记录
        /// </summary>
        /// <param name="rboolISFulfill"></param>
        public void UpdateCarInOutRecordMethod(object inOut)
        {
            try
            {
                string strsql = "";
                strsql = " CarInOutRecord_DrivewayStrategy_ID=" + DrivewayStrategy_ID;
                if (!string.IsNullOrEmpty(strAbnormal))
                {
                    strsql += ",CarInOutRecord_Abnormal='" + strAbnormal + "'";
                }
                if (!string.IsNullOrEmpty(DTCheckProperties.Rows[0]["CarInOutRecord_DrivewayStrategy_IDS"].ToString()))
                {
                    strsql += ",CarInOutRecord_DrivewayStrategy_IDS += '," + DrivewayStrategy_ID + "'";
                }
                else
                {
                    strsql += ",CarInOutRecord_DrivewayStrategy_IDS = '" + DrivewayStrategy_ID + "'";
                }

                //已通行策略
                if (boolYesNoCarInOutRecord_ISFulfill)
                {
                    strsql += ",CarInOutRecord_ISFulfill = 1";//车辆通行是否完成
                    strsql += ",CarInOutRecord_FulfillTime = '" + CommonalityEntity.GetServersTime() + "'";//车辆通行完成时间
                }
                if (inOut.ToString() == "已出厂")
                {
                    strsql += ",CarInoutRecord_OutTime = '" + CommonalityEntity.GetServersTime() + "'";//出厂时间
                }
                if (inOut.ToString() == "已进厂")
                {
                    if (string.IsNullOrEmpty(DTCheckProperties.Rows[0]["CarInoutRecord_InTime"].ToString()))
                    {
                        strsql += ",CarInoutRecord_InTime = '" + CommonalityEntity.GetServersTime() + "'";//进厂时间
                    }
                }
                strsql = "update CarInOutRecord set " + strsql + " where CarInOutRecord_ID=" + carinoutRid;
                LinQBaseDao.Query(strsql);
                CommonalityEntity.WriteLogData("修改", "记录车辆：" + DTCheckProperties.Rows[0]["CarInfo_Name"].ToString() + inOut, CommonalityEntity.USERNAME);
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInformationForm.UpdateCarInOutRecordMethod()");
            }
        }
        /// <summary>
        /// 修改车辆通行状态
        /// </summary>
        public void UpSortNumberInfoMethod(object strTongXingSteta)
        {
            try
            {
                int intSmallTicket_ID = 0;
                string strsql = "";
                intSmallTicket_ID = CommonalityEntity.GetInt(DTCheckProperties.Rows[0]["SmallTicket_ID"].ToString());
                strsql = "update SortNumberInfo set SortNumberInfo_TongXing='" + strTongXingSteta.ToString() + "' where SortNumberInfo_SmallTicket_ID=" + intSmallTicket_ID;
                LinQBaseDao.Query(strsql);
                if (strTongXingSteta == "已出厂")
                {
                    strsql = "CarInoutRecord_OutTime = '" + CommonalityEntity.GetServersTime() + "'";//出厂时间
                }
                if (strTongXingSteta == "已进厂")
                {
                    if (string.IsNullOrEmpty(DTCheckProperties.Rows[0]["CarInoutRecord_InTime"].ToString()))
                    {
                        if (string.IsNullOrEmpty(strsql))
                        {
                            strsql = "CarInoutRecord_InTime = '" + CommonalityEntity.GetServersTime() + "'";////进厂时间
                        }
                        else
                        {
                            strsql = ",CarInoutRecord_InTime = '" + CommonalityEntity.GetServersTime() + "'";////进厂时间
                        }
                    }
                }
                strsql = "update CarInoutRecord set " + strsql + "  where CarInOutRecord_ID=" + carinoutRid;
                LinQBaseDao.Query(strsql);



            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInformationForm.UpSortNumberInfoMethod()");
            }

        }
        /// <summary>
        /// 上传图片
        /// </summary>
        private void UpLoad(object obj)
        {
            try
            {
                string path = SystemClass.BaseFile + "Car" + SystemClass.PosistionValue + Driveway_Value + "\\";
                string stryear = "Car" + SystemClass.PosistionValue + Driveway_Value + "\\" + CommonalityEntity.GetServersTime().Year + "\\" + CommonalityEntity.GetServersTime().ToString("yyyyMMdd") + "\\";
                string inout = LinQBaseDao.GetSingle("select Driveway_Type from Driveway where Driveway_Value='" + Driveway_Value + "' and  Driveway_Position_ID in (select Position_ID from Position where Position_Value='" + SystemClass.PosistionValue + "')").ToString();
                if (listCarPicEffective.Count > 0)
                {
                    int i = 0;
                    foreach (var item in listCarPicEffective)
                    {
                        if (i < 2)//有效照片为最新两张
                        {
                            ImageFile.UpLoadFile(path + item, SystemClass.SaveFile + stryear);//上传图片到指定路径
                            gbShowImage.Dispose();//释放图片
                            AddCarPicture(Convert.ToInt32(strCarInOutRecord_CarInfo_ID), stryear, item, inout);
                            if (b != null)
                            {
                                b.Dispose();
                            }
                            i++;
                        }
                        ImageFile.Delete(path + item);//上传完成后，删除图片
                    }
                    //把照片关联到刷卡信息上
                    cardinfoRecord();
                }
                if (listCarPic.Count > 0)
                {
                    foreach (var item in listCarPic)
                    {
                        ImageFile.UpLoadFile(path + item, SystemClass.SaveFile + stryear);//上传图片到指定路径
                        gbShowImage.Dispose();//释放图片
                        addlistcarpic(inout, stryear, item);
                        if (b != null)
                        {
                            b.Dispose();
                        }
                        ImageFile.Delete(path + item);//上传完成后，删除图片
                    }
                }
            }
            catch (System.Exception ex)
            {

            }
        }
        /// <summary>
        /// 修改小票、IC卡已通行时间次数
        /// </summary>
        private void updateSmallTicket(object obj)
        {
            DateTime dtbegin = Convert.ToDateTime(DTCheckProperties.Rows[0]["CarInoutRecord_Time"].ToString());
            int hour = Convert.ToInt32((CommonalityEntity.GetServersTime() - dtbegin).TotalHours);
            if (boolYesNoCarInOutRecord_ISFulfill)
            {
                if (SerialnumberICbool)//小票
                {
                    LinQBaseDao.Query("update SmallTicket set SmallTicket_Allowcounted+=1 where SmallTicket_CarInfo_ID=" + DTCheckProperties.Rows[0]["CarInfo_ID"].ToString());
                }
                else
                {
                    if (!string.IsNullOrEmpty(ICC1))
                    {
                        DataTable dts = LinQBaseDao.Query("select ICCard_HasCount from ICCard   where ICCard_Value='" + ICC1 + "'").Tables[0];
                        if (dts.Rows.Count > 0)
                        {
                            string hascount = dts.Rows[0]["ICCard_HasCount"].ToString();
                            if (string.IsNullOrEmpty(hascount))
                            {
                                LinQBaseDao.Query("update ICCard set ICCard_HasCount=1 where ICCard_Value='" + ICC1 + "'");
                            }
                            else
                            {
                                LinQBaseDao.Query("update ICCard set ICCard_HasCount+=1 where ICCard_Value='" + ICC1 + "'");
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(ICC2))
                    {
                        DataTable dts = LinQBaseDao.Query("select ICCard_HasCount from ICCard   where ICCard_Value='" + ICC2 + "'").Tables[0];
                        if (dts.Rows.Count > 0)
                        {
                            string hascount = dts.Rows[0]["ICCard_HasCount"].ToString();
                            if (string.IsNullOrEmpty(hascount))
                            {
                                LinQBaseDao.Query("update ICCard set ICCard_HasCount=1 where ICCard_Value='" + ICC2 + "'");
                            }
                            else
                            {
                                LinQBaseDao.Query("update ICCard set ICCard_HasCount+=1 where ICCard_Value='" + ICC2 + "'");
                            }
                        }
                        ICC2 = "";
                    }
                }
            }
        }

        /// <summary>
        ///黑名单修改 
        /// </summary>
        private void BlackDown()
        {
            if (LinQBaseDao.Query("select * from Blacklist where Blacklist_CarInfo_ID =(select Car_ID from Car where Car_Name ='" + txt_Carnumber.Text + "')").Tables[0].Rows.Count >= 1)
            {
                //黑名单次数减1
                LinQBaseDao.Query("update Blacklist set Blacklist_UpgradeCount-=1 where Blacklist_CarInfo_ID in(select Car_ID from Car where Car_Name='" + txt_Carnumber.Text + "')");

                DataTable BlacklistDT = LinQBaseDao.Query("select Blacklist_ID,Blacklist_Dictionary_ID,Blacklist_UpgradeCount,Blacklist_DowngradeCount,Dictionary_Name,Dictionary_Spare_int1,Dictionary_Spare_int2 from Blacklist,Dictionary where Blacklist.Blacklist_CarInfo_ID = (select Car_ID from Car where Car_Name='" + txt_Carnumber.Text + "') and Dictionary_ID=Blacklist_Dictionary_ID").Tables[0];
                if (BlacklistDT.Rows.Count > 0)
                {
                    int Blacklist_ID = Convert.ToInt32(BlacklistDT.Rows[0]["Blacklist_ID"]);
                    int Blacklist_Dictionary_ID = Convert.ToInt32(BlacklistDT.Rows[0]["Blacklist_Dictionary_ID"]);
                    int Blacklist_DowngradeCount = Convert.ToInt32(BlacklistDT.Rows[0]["Blacklist_DowngradeCount"]);
                    int Dictionary_Spare_int2 = Convert.ToInt32(BlacklistDT.Rows[0]["Dictionary_Spare_int2"]);

                    //达到降级次数就降级
                    if (Blacklist_DowngradeCount == Dictionary_Spare_int2)
                    {
                        //查询出当前黑名单的下级，并降级
                        DataTable UpBlacklistDT = LinQBaseDao.Query("select top(1)Dictionary_ID from  Dictionary where Dictionary_Sort < " + Blacklist_Dictionary_ID).Tables[0];
                        if (UpBlacklistDT.Rows.Count == 1)
                        {
                            Expression<Func<Blacklist, bool>> fn = n => n.Blacklist_ID == Blacklist_ID;
                            Action<Blacklist> action = a => a.Blacklist_Dictionary_ID = Convert.ToInt32(UpBlacklistDT.Rows[0][0]);
                            bool b = BlacklistDAL.Update(fn, action);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// 异常放行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string strts = lbl_Prompt.Text;
                if (strts.IndexOf("读取中") >= 0)
                {
                    txtUnusual_Info.Text += "车辆信息读取失败，请重新读取！" + "\r\n";
                    return;
                }
                CommonalityEntity.isRestFXTwo = true;
                this.WindowState = FormWindowState.Minimized;
                button1.Enabled = false;
                ISRelease = true;
                if (!string.IsNullOrEmpty(txtUnusual_Info.Text.Trim()))
                {
                    CommonalityEntity.ADDUnusualRecord(2, "异常放行", txtUnusual_Info.Text.Trim(), CommonalityEntity.USERNAME, CommonalityEntity.GetInt(strCarInOutRecord_CarInfo_ID));
                }
                timer_Tick();
            }
            catch
            {
                CommonalityEntity.isRestFXTwo = false;
            }
        }

        private void CarInformationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                CarInformationFormTwo.cinfo = null;
                CommonalityEntity.CarNO = "";
                CommonalityEntity.CarValue.Remove(Driveway_Value);
                if (!SerialnumberICbool)
                {
                    CommonalityEntity.contoltwo = contio;
                }
            }
            catch (Exception)
            {
            }
        }

        private void chkchenpin_CheckedChanged(object sender, EventArgs e)
        {

            if (chkchenpin.Checked)
            {
                txt_Cartype.Text = "内部成品车";
            }
            else
            {
                txt_Cartype.Text = cartypename;
            }
        }

    }
}
