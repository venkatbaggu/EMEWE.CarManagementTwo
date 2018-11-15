using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class ProductForm : Form
    {
        public ProductForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 驾驶员编号
        /// </summary>
        private string staffInfo_Id = "";
        /// <summary>
        /// 公司名
        /// </summary>
        private string CustomerInfo_ID = "";
        /// <summary>
        /// 车辆编号
        /// </summary>
        private int carInfo_ID = -1;
        /// <summary>
        /// 车辆照片信息编号
        /// </summary>
        private int carPIC_ID = -1;
        /// <summary>
        /// 凭证编号
        /// </summary>
        private int smallTicket_ID = -1;
        /// <summary>
        /// 排队信息编号
        /// </summary>
        private int sortNumberInfo_ID = -1;
        /// <summary>
        /// 业务信息
        /// </summary>
        private int BusinessRecord_ID = -1;
        /// <summary>
        /// 通行总记录编号
        /// </summary>
        private int CarInOutRecord_ID = -1;

        DataTable dtviewcarstate;


        /// <summary>
        /// 执行管控验证
        /// </summary>
        private CheckProperties checkPr = new CheckProperties();
        /// <summary>
        /// 小票号
        /// </summary>
        private string SerialNumberNEW = "";
        /// <summary>
        /// 小票号
        /// </summary>
        private string SerialNumber = "";

        private string ICValue = "";

        private string PNO = "";
        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        /// <summary>
        /// 多次数据库操作，错误之后需要撤消执行的SQL
        /// </summary>
        private List<string> ErroSql = new List<string>();

        private void btnCheck_Click(object sender, EventArgs e)
        {
            try
            {
                CheckProperties.ce.SapChengPinTable = null;
                CheckProperties.ce.SapChengPinTable2 = null;
                CheckMethod.listMessage.Clear();
                CommonalityEntity.yxincheck = false;//是否系统自动进门授权
                SerialNumber = txtNumber.Text.Trim();
                if (string.IsNullOrEmpty(SerialNumber))
                {
                    MessageBox.Show(this, "小票号(IC卡)不能为空");
                    return;
                }
                string strsql = "";
                if (SerialNumber.Length >= 12)
                {
                    strsql = "select top 1 * from View_CarState where SmallTicket_Serialnumber= '" + SerialNumber + "' order by  SmallTicket_ID desc";
                }
                if (SerialNumber.Length == 9)
                {
                    strsql = "select top 1 * from View_CarState where SmallTicket_ICCard_ID in( select ICCard_ID from ICCard where ICCard_Value ='" + SerialNumber + "') order by  SmallTicket_ID desc";
                    ICValue = SerialNumber;
                }
                if (string.IsNullOrEmpty(strsql))
                {
                    MessageBox.Show("小票号(IC卡)无效！");
                    return;
                }
                dtviewcarstate = LinQBaseDao.Query(strsql).Tables[0];
                if (dtviewcarstate.Rows.Count > 0)
                {
                    if (!ISfull())
                    {
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("小票号(IC卡)无效！");
                    return;
                }
                string Strategy_DriSName = "";//通行策略名称
                CommonalityEntity.IsUpdatedri = false;
                //查询车辆类型的通行策略
                DataTable DrivewayStrategy_IDDT = LinQBaseDao.Query("select DrivewayStrategy_ID,DrivewayStrategy_Sort ,DrivewayStrategy_Name,DrivewayStrategy_Driveway_ID from DrivewayStrategy where DrivewayStrategy_State='启动' and  DrivewayStrategy_Name in (select CarType_DriSName from CarType  where  CarType_Name='" + cmbCarType.Text + "') order by DrivewayStrategy_Sort ").Tables[0];

                if (DrivewayStrategy_IDDT.Rows.Count <= 0)
                {
                    MessageBox.Show("该车辆没有配通行策略！", "提示");
                    return;
                }
                Strategy_DriSName = DrivewayStrategy_IDDT.Rows[0][2].ToString();
                CommonalityEntity.Driveway_ID = Convert.ToInt32(DrivewayStrategy_IDDT.Rows[0]["DrivewayStrategy_Driveway_ID"].ToString());
                DataTable dtption = LinQBaseDao.Query("select Driveway_Value,Position_ID,Position_Value,Position_State from View_DrivewayPosition where  Driveway_ID=" + CommonalityEntity.Driveway_ID).Tables[0];
                if (dtption.Rows.Count > 0)
                {
                    if (dtption.Rows[0]["Position_State"].ToString() != "启动")
                    {
                        MessageBox.Show(this, "通行门岗状态已暂停和注销！");
                        return;
                    }
                    CommonalityEntity.Driveway_Value = dtption.Rows[0]["Driveway_Value"].ToString();
                    CommonalityEntity.Position_ID = Convert.ToInt32(dtption.Rows[0]["Position_ID"].ToString());
                    CommonalityEntity.Position_Value = dtption.Rows[0]["Position_Value"].ToString();
                }
                else
                {
                    MessageBox.Show(this, "门岗通道不存在，请检查！");
                    return;
                }

                string carname = dtviewcarstate.Rows[0]["CarInfo_Name"].ToString();
                CheckProperties.ce.CarInfo_ID = Convert.ToInt32(dtviewcarstate.Rows[0]["CarInfo_ID"].ToString());
                CheckProperties.ce.ChengPinNumber = carname;
                CheckProperties.ce.carInfo_Name = carname;
              
                if (ChkRepeat(carname))
                {
                    MessageBox.Show("不能重复登记该车辆！");
                    return;
                }

                string crSql = "select * from CarType where CarType_Name='" + cmbCarType.Text + "'";
                CheckProperties.ce.IsPhoto = false;
                CheckProperties.ce.levelIsWaste = false;
                CheckProperties.ce.isSort = false;
                CheckProperties.ce.ISUpdateCredentials = false;
                CommonalityEntity.ISYX = false;
                CheckProperties.ce.IsState = false;
                CarType cart = LinQBaseDao.GetItemsForListing<CarType>(crSql).FirstOrDefault();
                if (cart != null)
                {
                    CommonalityEntity.Car_Type_ID = cart.CarType_ID.ToString();
                    CheckProperties.ce.carType_ID = cart.CarType_ID;
                    CheckProperties.ce.CarTypeKey = cart.CarType_ID.ToString();
                    CommonalityEntity.CarType[cart.CarType_ID.ToString()] = cart.CarType_Name;
                    CheckProperties.ce.carType_Value = cart.CarType_Value;
                    CheckProperties.ce.CarType_Name = cart.CarType_Name;

                }


                #region 执行登记管控

                //根据车辆类型，获取车辆类型的管控策略（登记管控）
                string sql = "";
                sql = "select * from  ManagementStrategy where ManagementStrategy_DriSName='" + Strategy_DriSName + "' and ManagementStrategy_Menu_ID=1  and ManagementStrategy_State='启动' and ManagementStrategy_Rule  in('ChkChengPin', 'ChkSapOflag','ISInCheckSapSave')  order by ManagementStrategy_No ";
                DataTable dtmstra = LinQBaseDao.Query(sql).Tables[0];

                if (dtmstra.Rows.Count > 0)
                {
                    CommonalityEntity.ISlogin = true;
                }
                else
                {
                    CommonalityEntity.ISlogin = false;
                }

                if (CommonalityEntity.ISlogin)
                {
                    sql = "select * from  ManagementStrategy where ManagementStrategy_DriSName='" + Strategy_DriSName + "' and ManagementStrategy_Menu_ID=1  and ManagementStrategy_State='启动' order by ManagementStrategy_No ";
                }
                else
                {
                    sql = "select * from  ManagementStrategy where ManagementStrategy_DriSName='" + Strategy_DriSName + "' and ManagementStrategy_Menu_ID=1  and ManagementStrategy_State='启动' and ManagementStrategy_Rule not in('ChkChengPin', 'ChkSapOflag','ISInCheckSapSave')  order by ManagementStrategy_No ";
                }
                try
                {
                    checkPr.ExecutionMethod(sql);//执行指定车辆类型的管控
                }
                catch { }


                #endregion

                //执行结果
                if (CheckMethod.listMessage.Count > 0)
                {
                    foreach (var item in CheckMethod.listMessage)
                    {
                        MessageBox.Show(item);
                        return;
                    }
                }

                string carId = "";
                Expression<Func<Car, bool>> carFn = n => n.Car_Name == CheckProperties.ce.carInfo_Name;
                Car car1 = CarDAL.Query(carFn).FirstOrDefault();
                if (car1 != null)
                {
                    carId = car1.Car_ID.ToString();
                }

                #region 车辆信息
                carInfo_ID = insertcar(carId);
                ErroSql.Add("delete carinfo where carinfo_id=" + carInfo_ID);
                #endregion

                #region 登记图片信息

                DataTable dt = LinQBaseDao.Query("select * from CarPic where CarPic_Type='车辆登记照片' and CarPic_CarInfo_ID=" + CheckProperties.ce.CarInfo_ID).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string picSql = "Insert into CarPic(CarPic_CarInfo_ID,CarPic_State,CarPic_Add,CarPic_Type,CarPic_Time,CarPic_Match) values(" + carInfo_ID + ",'启动','" + dt.Rows[0]["CarPic_Add"].ToString() + "','车辆登记照片',getdate(),'匹配')";
                    picSql = picSql + " select @@identity";
                    string picid = LinQBaseDao.GetSingle(picSql).ToString();//得到当前的图片编号
                    ErroSql.Add("delete CarPic where CarPic_ID=" + picid);
                }
                #endregion

                #region 进出凭证信息
                SerialNumberNEW = CheckProperties.ce.serialNumber;
                CommonalityEntity.Serialnumber = SerialNumberNEW;
                SmallTicket stk = new SmallTicket();
                string icid = "", count = "", hour = "";
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
                else
                {
                    hour = CheckProperties.ce.serTime;
                    count = CheckProperties.ce.serCount;
                }

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

                if (CheckProperties.ce.isSort)
                {
                    //根据当前排队的序号，生成排队号
                    if (!CommonalityEntity.ISSecondXY)
                    {
                        DataTable dtm = LinQBaseDao.Query("select * from ManagementStrategy where ManagementStrategy_Rule='GetSortNumber' and ManagementStrategy_State='启动' and ManagementStrategy_Menu_ID=1 and ManagementStrategy_DriSName in (select CarType_DriSName from CarType where CarType_ID=" + cmbCarType.SelectedValue + ")").Tables[0];

                        if (dtm.Rows.Count > 0)
                        {
                            CheckMethod.GetSortNumber();
                        }
                        string number = CommonalityEntity.SortNumber(CheckProperties.ce.sort_Value);
                        stk.SmallTicket_SortNumber = CommonalityEntity.Position_Value + CheckProperties.ce.carType_Value + number;
                    }
                    else
                    {
                        stk.SmallTicket_SortNumber = "";
                    }
                }
                else
                {
                    stk.SmallTicket_SortNumber = "";
                }
                stk.SmallTicket_CarInfo_ID = carInfo_ID;
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
                if (SerialNumber.Trim() != "")
                {
                    DataTable dtm = LinQBaseDao.Query("select * from ManagementStrategy where ManagementStrategy_Rule='ChkSerialNumber' and ManagementStrategy_State='启动' and ManagementStrategy_Menu_ID=1 and ManagementStrategy_DriSName in (select CarType_DriSName from CarType where CarType_ID=" + cmbCarType.SelectedValue + ")").Tables[0];

                    if (dtm.Rows.Count > 0)
                    {
                        CheckMethod.ChkSerialNumber();
                        SerialNumberNEW = CheckProperties.ce.serialNumber;
                    }
                    stk.SmallTicket_Type += "小票";
                    stk.SmallTicket_Serialnumber = SerialNumberNEW.Trim();
                }
                string SmalltSql = "";
                if (stk.SmallTicket_ICCard_ID.ToString() != "")//判断IC卡是否输入
                {
                    SmalltSql = "insert into SmallTicket values(" + stk.SmallTicket_ICCard_ID + ",'" + stk.SmallTicket_Serialnumber + "'," + stk.SmallTicket_Position_ID + "," + stk.SmallTicket_CarInfo_ID + ",'" + stk.SmallTicket_Time + "','" + stk.SmallTicket_SortNumber + "','" + stk.SmallTicket_PrintNumber + "','" + stk.SmallTicket_State + "','" + stk.SmallTicket_Type + "','" + stk.SmallTicket_Allowcounted + "','" + stk.SmallTicket_Allowcount + "','" + stk.SmallTicket_Allowhour + "','" + stk.SmallTicket_Remark + "')";
                }
                else
                {
                    SmalltSql = "insert into SmallTicket values(null,'" + stk.SmallTicket_Serialnumber + "'," + stk.SmallTicket_Position_ID + "," + stk.SmallTicket_CarInfo_ID + ",'" + stk.SmallTicket_Time + "','" + stk.SmallTicket_SortNumber + "','" + stk.SmallTicket_PrintNumber + "','" + stk.SmallTicket_State + "','" + stk.SmallTicket_Type + "','" + stk.SmallTicket_Allowcounted + "','" + stk.SmallTicket_Allowcount + "','" + stk.SmallTicket_Allowhour + "','" + stk.SmallTicket_Remark + "')";
                }
                SmalltSql = SmalltSql + " select @@identity";
                smallTicket_ID = int.Parse(LinQBaseDao.GetSingle(SmalltSql).ToString());//得到当前的车辆编号
                ErroSql.Add("delete SmallTicket where SmallTicket_id=" + smallTicket_ID);
                #endregion

                #region 排队信息
                SortNumberInfo sort = new SortNumberInfo();
                //根据车辆类型  得到该车辆类型的信息
                sort.SortNumberInfo_DrivewayValue = CommonalityEntity.Driveway_Value;
                //门岗值
                sort.SortNumberInfo_PositionValue = CommonalityEntity.Position_Value;

                //车辆类型值
                sort.SortNumberInfo_CarTypeValue = CheckProperties.ce.carType_Value;
                //呼叫次数
                sort.SortNumberInfo_CallCount = 0;

                sort.SortNumberInfo_LEDCount = 0;
                sort.SortNumberInfo_Operate = CommonalityEntity.USERNAME;

                //生成原因
                sort.SortNumberInfo_Reasons = "登记生成";
                sort.SortNumberInfo_Remark = "";
                //凭证编号
                sort.SortNumberInfo_SmallTicket_ID = smallTicket_ID;
                sort.SortNumberInfo_SMSCount = 0;

                if (CheckProperties.ce.isSort)
                {
                    if (!CommonalityEntity.ISSecondXY)
                    {
                        sort.SortNumberInfo_SortValue = CheckProperties.ce.sort_Value + 1;
                        sort.SortNumberInfo_TongXing = "排队中";
                    }
                    else
                    {
                        CommonalityEntity.ISSecondXY = false;
                        sort.SortNumberInfo_SortValue = null;
                        sort.SortNumberInfo_TongXing = "待通行";
                    }
                }
                else
                {
                    sort.SortNumberInfo_SortValue = null;
                    sort.SortNumberInfo_TongXing = "待通行";
                }
                sort.SortNumberInfo_State = "启动";
                sort.SortNumberInfo_Type = "系统生成";
                sort.SortNumberInfo_Time = CommonalityEntity.GetServersTime();

                //添加排队信息
                string sort_Sql = "insert into SortNumberInfo values('" + sort.SortNumberInfo_SmallTicket_ID + "','" + sort.SortNumberInfo_Time + "','" + sort.SortNumberInfo_Reasons + "','" + sort.SortNumberInfo_Operate + "','" + sort.SortNumberInfo_Type + "','" + sort.SortNumberInfo_SortValue + "','" + sort.SortNumberInfo_PositionValue + "','" + sort.SortNumberInfo_CarTypeValue + "','" + sort.SortNumberInfo_CallCount + "','" + sort.SortNumberInfo_SMSCount + "','" + sort.SortNumberInfo_LEDCount + "','" + sort.SortNumberInfo_Remark + "','" + sort.SortNumberInfo_TongXing + "','" + sort.SortNumberInfo_DrivewayValue + "','" + sort.SortNumberInfo_State + "','" + sort.SortNumberInfo_CallTime + "','" + sort.SortNumberInfo_Number + "', 1 ,0)";
                sort_Sql = sort_Sql + " select @@identity";
                sortNumberInfo_ID = int.Parse(LinQBaseDao.GetSingle(sort_Sql).ToString());
                ErroSql.Add("delete SortNumberInfo where SortNumberInfo_ID=" + sortNumberInfo_ID);//增删改出错后需要执行的SQL
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
                cir.CarInOutRecord_CarInfo_ID = carInfo_ID;
                cir.CarInOutRecord_Update = false;
                cir.CarInOutRecord_Driveway_ID = CommonalityEntity.Driveway_ID;
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
                CarInOutRecord_ID = int.Parse(LinQBaseDao.GetSingle(CarInOutRecordSql).ToString());
                ErroSql.Add("delete CarInOutRecord where CarInOutRecord_ID=" + CarInOutRecord_ID);
                #endregion

                #region 添加关联表信息
                string staid = dtviewcarstate.Rows[0]["StaffInfo_ID"].ToString();
                string sfCrSql = "Insert into StaffInfo_CarInfo values(" + staid + "," + smallTicket_ID + ")";
                sfCrSql = sfCrSql + " select @@identity";
                int sfCrID = int.Parse(LinQBaseDao.GetSingle(sfCrSql).ToString());
                ErroSql.Add("delete StaffInfo_CarInfo where StaffInfo_CarInfo_ID=" + sfCrID);
                #endregion

                if (CommonalityEntity.yxincheck)
                {
                    LinQBaseDao.Query("update CarInOutRecord set CarInOutRecord_InCheck='是',CarInOutRecord_InCheckTime=GETDATE(),CarInOutRecord_InCheckUser='" + CommonalityEntity.USERNAME + "' where CarInOutRecord_ID=" + CarInOutRecord_ID);
                }

                #region 判断是否添加成功
                if (sortNumberInfo_ID > 0 && smallTicket_ID > 0 && carInfo_ID > 0 && CarInOutRecord_ID != -1)
                {

                    if (CommonalityEntity.SAP_ID != "" && SerialNumberNEW != "")
                    {
                        LinQBaseDao.Query("update eh_SAPRecord set Sap_Serialnumber=" + SerialNumberNEW + " where Sap_ID=" + CommonalityEntity.SAP_ID);
                        CommonalityEntity.SAP_ID = "";
                        if (!CommonalityEntity.GetSAP(PNO, "A", carname, SerialNumberNEW, "1"))
                        {
                            MessageBox.Show("成品纸车辆SAP验证未通过！");
                            return;
                        }
                    }
                    if (CommonalityEntity.SAP_ID != "" && SerialNumberNEW == "" && ICValue != "")
                    {
                        LinQBaseDao.Query("update eh_SAPRecord set Sap_Serialnumber=" + ICValue + " where Sap_ID=" + CommonalityEntity.SAP_ID);
                        CommonalityEntity.SAP_ID = "";
                        if (!CommonalityEntity.GetSAP(PNO, "A", carname, ICValue, "1"))
                        {
                            MessageBox.Show("成品纸车辆SAP验证未通过！");
                            return;
                        }
                    }

                    CommonalityEntity.CarInfo_ID = carInfo_ID.ToString();

                    MessageBox.Show(this, "成品二次排队成功");
                    CommonalityEntity.WriteLogData("新增", "新增：" + carname + "车辆二次排队", CommonalityEntity.USERNAME);//添加操作日志

                    if (!string.IsNullOrEmpty(CommonalityEntity.SAP_ID))
                    {
                        DataTable dtsap = LinQBaseDao.Query("select  Sap_InNO,Sap_OutHG,Sap_OutXZ  from eh_SAPRecord where Sap_ID=" + CommonalityEntity.SAP_ID).Tables[0];
                        if (dtsap.Rows.Count > 0)
                        {
                            if (CommonalityEntity.SAP_ID != "" && SerialNumber != "")
                            {
                                LinQBaseDao.Query("update eh_SAPRecord set Sap_Serialnumber=" + SerialNumberNEW + " where Sap_ID=" + CommonalityEntity.SAP_ID);
                                CommonalityEntity.SAP_ID = "";
                                if (!CommonalityEntity.GetSAP(dtsap.Rows[0]["Sap_InNO"].ToString(), "A", carname, SerialNumberNEW, "1"))
                                {
                                    MessageBox.Show("成品车辆SAP验证未通过！");
                                    return;
                                }
                            }
                        }
                    }

                    //获取打印设置
                    string prtSql = "select top 1 * from PrintInfo where Print_State='启动' and Print_CarType_ID=" + CheckProperties.ce.carType_ID + "";
                    if (CheckProperties.ce.isSerialNumber)
                    {
                        CheckProperties.ce.isSerialNumber = false;
                        PrintInfo pinfo = PrintInfoDAL.GetPrint(prtSql);

                        if (pinfo.Print_ID > 0)
                        {
                            if (SerialNumberNEW != "")
                            {
                                string prSql = "Select top 1  ";
                                string[] str = pinfo.Print_Content.Split(',');
                                foreach (var item in str)
                                {
                                    prSql += item + ",";
                                }
                                prSql = prSql.Substring(0, prSql.Length - 1);
                                prSql += " from View_LEDShow_zj where 小票号='" + SerialNumberNEW + "' and smallTicket_State='有效'";
                                DataSet ds = LinQBaseDao.Query(prSql);
                                CommonalityEntity.Serialnumber = SerialNumberNEW;
                                PrintInfoForm pi = new PrintInfoForm(ds);
                                pi.Show();
                            }
                        }
                        else
                        {
                            MessageBox.Show("没有进行打印设置，请设置打印后，重新打印");
                            return;
                        }
                    }

                }
                #endregion
            }
            catch
            {
                MessageBox.Show("成品二次排队失败！");
                CommonalityEntity.WriteTextLog("ProductForm btnCheck_Click()");
                try
                {
                    string sqlstr = "";
                    for (int i = 0; i < ErroSql.Count; i++)
                    {
                        sqlstr += ErroSql[i] + "     ";
                    }
                    LinQBaseDao.Query(sqlstr);
                }
                catch
                {
                    CommonalityEntity.WriteTextLog("ProductForm btnCheck_Click()");
                }
            }
        }

        /// <summary>
        /// 出门到二次排队时间校验
        /// </summary>
        /// <returns></returns>
        private bool ISfull()
        {
            bool istrue = true;
            DateTime CarInOutInfoRecordTime;
            int contrsValue = 0;
            if (dtviewcarstate.Rows[0]["SortNumberInfo_TongXing"].ToString() == "已出厂")
            {
                if (string.IsNullOrEmpty(dtviewcarstate.Rows[0]["CarInoutRecord_OutTime"].ToString()))
                {
                    MessageBox.Show("已超时，不能二次排队！");
                    return false;
                }
                CarInOutInfoRecordTime = Convert.ToDateTime(dtviewcarstate.Rows[0]["CarInoutRecord_OutTime"].ToString());
                DataTable dtman = LinQBaseDao.Query("select ManagementStrategy_ID from ManagementStrategy where ManagementStrategy_Rule='OutTimeMethod' and ManagementStrategy_DriSName in (select CarType_DriSName from CarType where CarType_Name ='" + cmbCarType.Text + "')").Tables[0];
                if (dtman.Rows.Count > 0)
                {
                    dtman = LinQBaseDao.Query("select ControlInfo_Value from ControlInfo where  ControlInfo_Rule='OutTimeMethod'").Tables[0];
                    if (dtman.Rows.Count > 0)
                    {
                        contrsValue = int.Parse(dtman.Rows[0][0].ToString());

                        TimeSpan ts = CommonalityEntity.GetServersTime() - CarInOutInfoRecordTime;
                        int m = Convert.ToInt32(ts.TotalMinutes);
                        if (contrsValue < m)
                        {
                            MessageBox.Show("已超时，不能二次排队！");
                            return istrue = false;
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("已超时，不能二次排队！");
                return istrue = false;
            }
            return istrue;
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            txtNumber.Text = CommonalityEntity.strCardNo;
            BindCarType();
        }
        private void BindCarType()
        {

            string strsqltype = "select CarType_ID,CarType_Name from CarType where CarType_State='启动' and  CarType_Property='成品车辆'";
            DataTable dt = LinQBaseDao.Query(strsqltype).Tables[0];
            if (dt.Rows.Count > 0)
            {
                cmbCarType.DataSource = dt;
                cmbCarType.DisplayMember = "CarType_Name";
                cmbCarType.ValueMember = "CarType_ID";
                cmbCarType.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 验证该登记车辆是否为重复登记
        /// </summary>
        /// <param name="carName">车牌号</param>
        /// <returns>返回true(重复) or false(不重复)</returns>
        private bool ChkRepeat(string carName)
        {
            bool rbool = true;
            string sql = "select top(1) CarInOutRecord_ISFulfill,SortNumberInfo_TongXing from View_CarState where CarInfo_Name='" + carName + "' order by CarInOutRecord_ID desc";
            DataTable dtcs = LinQBaseDao.Query(sql).Tables[0];
            if (dtcs.Rows.Count > 0)
            {
                if (dtcs.Rows[0]["CarInOutRecord_ISFulfill"].ToString() == "True")
                {
                    rbool = false;
                }
                if (dtcs.Rows[0]["SortNumberInfo_TongXing"].ToString() == "已注销" || dtcs.Rows[0]["SortNumberInfo_TongXing"].ToString() == "已出厂")
                {
                    rbool = false;
                }
            }
            else
            {
                rbool = false;
            }
            return rbool;
        }

        /// 添加车辆信息 
        /// </summary>
        /// <param name="strid">车辆基础信息ID</param>
        /// <returns></returns>
        private int insertcar(string strid)
        {
            string CarName = "";
            string pno = "";
            string CarInfo_Carriage = "";
            string CustomerInfoName = "";
            string chkCarInfo_Bail = "";
            string txtCarInfo_Height = "";
            string txtCarInfo_Weight = "";
            if (!string.IsNullOrEmpty(CommonalityEntity.SAP_ID))
            {
                string sql = "Select * from eh_Saprecord where SAP_ID =" + CommonalityEntity.SAP_ID;
                List<eh_SAPRecord> list = LinQBaseDao.GetItemsForListing<eh_SAPRecord>(sql).ToList();
                if (list != null)
                {
                    eh_SAPRecord item = list[0];
                    if (!string.IsNullOrEmpty(item.Sap_InNO))
                    {
                        pno = item.Sap_InNO;//送货单号
                        PNO = pno;
                    }
                    DataTable dt;

                    if (item.Sap_InCarNumber != null || item.Sap_InCarNumber != "")
                    {
                        CarName = item.Sap_InCarNumber;//车牌号
                    }
                    if (item.Sap_OutNAME1C != null || item.Sap_OutNAME1C != "")
                    {
                        CustomerInfoName = item.Sap_OutNAME1C;//客户
                    }
                    if (item.Sap_OutNAME1C == null || item.Sap_OutNAME1C == "")
                    {
                        if (item.Sap_OutNAME1P != "" || item.Sap_OutNAME1P != null)
                        {
                            CustomerInfoName = item.Sap_OutNAME1P;//供应商
                        }
                    }

                    if (CustomerInfoName != "")
                    {
                        DataTable CustomerInfo_IDDT = LinQBaseDao.Query("select CustomerInfo_ID from CustomerInfo where CustomerInfo_Name='" + CustomerInfoName + "'").Tables[0];
                        //如果数据库有就赋值没有就添加
                        if (CustomerInfo_IDDT.Rows.Count > 0)
                        {
                            CustomerInfo_ID = CustomerInfo_IDDT.Rows[0][0].ToString();
                        }
                        else
                        {
                            CustomerInfo_ID = LinQBaseDao.GetSingle("insert CustomerInfo(CustomerInfo_Name,CustomerInfo_State,CustomerInfo_Time) values('" + CustomerInfoName + "','启动',GETDATE())  select @@identity").ToString();

                        }
                    }
                    if (item.Sap_OutMAKTX != null || item.Sap_OutMAKTX != "")
                    {
                        CarInfo_Carriage = item.Sap_OutMAKTX;//物料描述
                    }
                    if (item.Sap_OutOFLAG != null || item.Sap_OutOFLAG != "")
                    {
                        if (item.Sap_OutOFLAG == "X")
                        {
                            chkCarInfo_Bail = "True";
                        }
                        else
                        {
                            chkCarInfo_Bail = "False";
                        }
                    }
                    if (item.Sap_OutHG != null || item.Sap_OutHG != "")
                    {
                        txtCarInfo_Height = item.Sap_OutHG;
                    }
                    if (item.Sap_OutXZ != null || item.Sap_OutXZ != "")
                    {
                        txtCarInfo_Weight = item.Sap_OutXZ;
                    }
                }
            }
            else
            {
                chkCarInfo_Bail = "False";
                CarName = dtviewcarstate.Rows[0]["CarInfo_Name"].ToString();
                CustomerInfo_ID = dtviewcarstate.Rows[0]["CustomerInfo_ID"].ToString();
            }

            int str = 0;
            #region 车辆信息
            CarInfo carInfo = new CarInfo();

            //得到车辆类型编号
            carInfo.CarInfo_CarType_ID = CheckProperties.ce.carType_ID;
            //车牌号
            carInfo.CarInfo_Name = CarName;

            carInfo.CarInfo_State = "启动";
            carInfo.CarInfo_Remark = "成品二次排队";
            carInfo.CarInfo_Carriage = CarInfo_Carriage;
            carInfo.CarInfo_Height = txtCarInfo_Height;
            carInfo.CarInfo_Weight = txtCarInfo_Weight;
            carInfo.CarInfo_Car_ID = Convert.ToInt32(strid);
            //公司编号
            if (!string.IsNullOrEmpty(CustomerInfo_ID))
            {
                carInfo.CarInfo_CustomerInfo_ID = int.Parse(CustomerInfo_ID);
            }
            carInfo.CarInfo_Operate = CommonalityEntity.USERNAME;
            carInfo.CarInfo_Time = CommonalityEntity.GetServersTime();
            carInfo.CarInfo_Bail = chkCarInfo_Bail;
            carInfo.CarInfo_PO = pno;
            string carSql = "insert into CarInfo(CarInfo_CustomerInfo_ID,CarInfo_CarType_ID,CarInfo_Name,CarInfo_State,CarInfo_Carriage,CarInfo_Weight,CarInfo_Height,CarInfo_Bail,CarInfo_PO,CarInfo_Time,CarInfo_Remark,CarInfo_Operate,CarInfo_Car_ID) values(" + carInfo.CarInfo_CustomerInfo_ID + "," + carInfo.CarInfo_CarType_ID + ",'" + carInfo.CarInfo_Name + "','" + carInfo.CarInfo_State + "','" + carInfo.CarInfo_Carriage + "','" + carInfo.CarInfo_Weight + "','" + carInfo.CarInfo_Height + "','" + carInfo.CarInfo_Bail + "','" + carInfo.CarInfo_PO + "','" + carInfo.CarInfo_Time + "','" + carInfo.CarInfo_Remark + "','" + carInfo.CarInfo_Operate + "'," + strid + ")";
            carSql = carSql + " select @@identity";
            str = int.Parse(LinQBaseDao.GetSingle(carSql).ToString());//得到当前的车辆编号
            #endregion
            return str;

        }

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnCheck_Click(sender, e);
            }
        }

        private void btnCheck_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnCheck_Click(sender, e);
            }

        }
    }
}
