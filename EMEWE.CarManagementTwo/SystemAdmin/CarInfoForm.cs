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
using System.Collections;
using EMEWE.CarManagement.HelpClass;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using EMEWE.CarManagement.CommonClass;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class CarInfoForm : Form
    {
        /// <summary>
        /// 驾驶员编号
        /// </summary>
        private string staffInfo_Id = "";
        /// <summary>
        /// 公司名
        /// </summary>
        private string CustomerInfo_ID = "";
        /// <summary>
        /// 驾驶员姓名集合
        /// </summary>
        private List<string> staffInfo = new List<string>();
        /// <summary>
        /// 是否拍照
        /// </summary>
        private bool isPhoto = false;
        /// <summary>
        /// 是否登记
        /// </summary>
        private bool isInsert = false;
        /// <summary>
        /// 车辆编号
        /// </summary>
        private int carInfo_ID = -1;
        /// <summary>
        /// 车辆照片信息编号
        /// </summary>
        private int carPIC_ID = -1;
        /// <summary>
        /// 主页面
        /// </summary>
        //private MainForm mf;
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
        /// 车辆类型值
        /// </summary>
        private string carType_Value = "";
        /// <summary>
        /// IC卡编号
        /// </summary>
        private int iCNumberID = -1;
        /// <summary>
        /// 图片控件集合
        /// </summary>
        public List<string> ImageList = new List<string>();
        /// <summary>
        ///异常信息
        /// </summary>
        public List<string> LastList = new List<string>();
        /// <summary>
        /// 异常
        /// </summary>
        private UnusualRecord ur = null;
        /// <summary>
        /// 是否修改公司信息
        /// </summary>
        private bool IsCus = false;
        /// <summary>
        /// 通行总记录编号
        /// </summary>
        private int CarInOutRecord_ID = -1;
        /// <summary>
        /// 车辆人员关联表ID集合
        /// </summary>
        private List<string> sfCrID = new List<string>();
        /// <summary>
        /// 执行管控验证
        /// </summary>
        private CheckProperties checkPr = new CheckProperties();
        /// <summary>
        /// 是否绑定优先车辆类型
        /// </summary>
        private bool IsCarPec = false;
        /// <summary>
        /// 是否判断IC卡登记
        /// </summary>
        private bool IsICard = false;
        /// <summary>
        /// 输入数据是否为空
        /// </summary>
        private bool result = true;
        /// <summary>
        /// 小票号
        /// </summary>
        private string SerialNumber = "";
        /// <summary>
        /// 1次数优先，2有效期优先
        /// </summary>
        private int ctype = 0;

        /// <summary>
        /// Sap登记车辆SapID
        /// </summary>
        private string Sapid = "";

        //存取车辆业务类型可通行的门岗值
        private string positionstr = "";

        /// <summary>
        /// 多次数据库操作，错误之后需要撤消执行的SQL
        /// </summary>
        private List<string> ErroSql = new List<string>();
        public CarInfoForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {
                btnSave.Enabled = true;
                btnSave.Visible = true;
            }
            else
            {
                btnSave.Enabled = ControlAttributes.BoolControl("btnSave", "CarInfoForm", "Enabled");
                btnSave.Visible = ControlAttributes.BoolControl("btnSave", "CarInfoForm", "Visible");
            }
        }


        /// <summary>
        /// 加载信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarInfoForm_Load(object sender, EventArgs e)
        {
            try
            {
                chkNumber.Checked = true;
                Sapid = CommonalityEntity.SAP_ID;
                txtCarName.Leave += new EventHandler(txtCarName_Leave);
                LoadCarType();
                BindStandard();
                userContext();
                chkNumbertrue();

                txtNo.Enabled = false;
                CommonalityEntity.IsUpdatedri = false;
                comboxCarType.Enabled = true;
                txtCarName.Focus();
                txtUserName.Text = CommonalityEntity.USERNAME;//绑定当前登录人
                LoadCarState();//绑定状态
                comboxCarState.SelectedIndex = 0;
                if (Sapid != "")
                {
                    SAPChk();
                }
                if (CommonalityEntity.UpdateCar)//修改车辆信息
                {
                    txtNo.Enabled = true;
                    comboxCarType.Enabled = false;
                    btnSave.Text = "修改";
                    GetCarInfo();//车辆信息
                    // ShowImage();//显示图片
                    GetStaffInfo();//绑定驾驶员信息
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoForm CarInfoForm_Load()");
            }

        }
        private void txtCarName_Leave(object sender, EventArgs e)
        {
            try
            {

                DataTable dts = LinQBaseDao.Query("select top(1)StaffInfo_Name,StaffInfo_Identity,StaffInfo_Phone,CustomerInfo_Name from View_CarState where CarInfo_Name='" + txtCarName.Text.Trim() + "' order by CarInOutRecord_ID desc ").Tables[0];
                if (dts.Rows.Count > 0)
                {
                    if (txtStaffName.Text.Trim() == "")
                    {
                        txtStaffName.Text = dts.Rows[0][0].ToString();
                    }
                    if (txtStaffInfo_Identity.Text.Trim() == "")
                    {
                        txtStaffInfo_Identity.Text = dts.Rows[0][1].ToString();
                    }
                    if (txtPhone.Text.Trim() == "")
                    {
                        txtPhone.Text = dts.Rows[0][2].ToString();
                    }
                    if (txtCustomerInfo.Text.Trim() == "")
                    {
                        txtCustomerInfo.Text = dts.Rows[0][3].ToString();
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        public void showImages()
        {
            try
            {
                string[] images = Directory.GetFiles(SystemClass.BaseFile + "Car" + SystemClass.PosistionValue);
                int i = 0;
                CheckProperties.ce.Imagelist.Clear();
                int s = 0;
                foreach (string pathStr in images)
                {
                    s++;
                }
                if (s == 0)
                {
                    groupBox2.Visible = false;
                    groupBox3.Visible = false;
                    groupBox4.Visible = false;
                    groupBox5.Visible = false;

                }
                else if (s == 1)
                {
                    groupBox2.Visible = true;
                    groupBox3.Visible = false;
                    groupBox4.Visible = false;
                    groupBox5.Visible = false;

                }
                else if (s == 2)
                {
                    groupBox2.Visible = true;
                    groupBox3.Visible = true;
                    groupBox4.Visible = false;
                    groupBox5.Visible = false;
                }
                else if (s == 3)
                {
                    groupBox2.Visible = true;
                    groupBox3.Visible = true;
                    groupBox4.Visible = true;
                    groupBox5.Visible = false;

                }
                else if (s == 4)
                {
                    groupBox2.Visible = true;
                    groupBox3.Visible = true;
                    groupBox4.Visible = true;
                    groupBox5.Visible = true;

                }
                foreach (string pathStr in images)
                {
                    if (i < 4)
                    {
                        string fileName = pathStr.Substring(pathStr.LastIndexOf('\\') + 1);
                        string fileKind = fileName.Substring(fileName.LastIndexOf('.') + 1);
                        if (fileKind.ToLower() != "jpg" && fileKind.ToLower() != "jpeg" && fileKind.ToLower() != "png" && fileKind.ToLower() != "gif" && fileKind.ToLower() != "bmp")
                        {
                            continue;
                        }
                        if (i == 0)
                        {
                            pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage; ;
                            pictureBox2.MouseHover += new System.EventHandler(this.pbInImageOne_MouseHover);
                            pictureBox2.MouseLeave += new System.EventHandler(this.pbInImageOne_MouseLeave);
                            pictureBox2.Tag = pathStr.ToString();
                            pictureBox2.ImageLocation = pathStr.ToString();
                            // bt.Click += new EventHandler(btnClick_Click);
                            CheckProperties.ce.Imagelist.Add(fileName);
                        }
                        else if (i == 1)
                        {
                            pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage; ;
                            pictureBox3.MouseHover += new System.EventHandler(this.pbInImageOne_MouseHover);
                            pictureBox3.MouseLeave += new System.EventHandler(this.pbInImageOne_MouseLeave);
                            pictureBox3.Tag = pathStr.ToString();
                            pictureBox3.ImageLocation = pathStr.ToString();
                            CheckProperties.ce.Imagelist.Add(fileName);
                        }
                        else if (i == 2)
                        {
                            pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage; ;
                            pictureBox4.MouseHover += new System.EventHandler(this.pbInImageOne_MouseHover);
                            pictureBox4.MouseLeave += new System.EventHandler(this.pbInImageOne_MouseLeave);
                            pictureBox4.Tag = pathStr.ToString();
                            pictureBox4.ImageLocation = pathStr.ToString();
                            CheckProperties.ce.Imagelist.Add(fileName);
                        }
                        else if (i == 3)
                        {
                            pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage; ;
                            pictureBox5.MouseHover += new System.EventHandler(this.pbInImageOne_MouseHover);
                            pictureBox5.MouseLeave += new System.EventHandler(this.pbInImageOne_MouseLeave);
                            pictureBox5.Tag = pathStr.ToString();
                            pictureBox5.ImageLocation = pathStr.ToString();
                            CheckProperties.ce.Imagelist.Add(fileName);
                        }
                        i++;
                    }
                }

            }
            catch
            {

            }
        }


        /// <summary>
        /// 绑定车辆信息(修改)
        /// </summary>
        private void GetCarInfo()
        {
            try
            {

                if (CommonalityEntity.CarInfo_ID == "")
                {
                    MessageBox.Show("参数错误");
                    this.Close();
                }
                else
                {
                    string sql = "";
                    DataTable dt;
                    sql = "Select * from CarInfo where CarInfo_ID=" + CommonalityEntity.CarInfo_ID + "";
                    CarInfo carinfo = new CarInfo();
                    carinfo = CarInfoDAL.GetViewCarInfoName(sql).FirstOrDefault();
                    if (carinfo.CarInfo_ID > 0)
                    {
                        txtCarInfo_Carriage.Text = carinfo.CarInfo_Carriage;
                        txtCarInfo_Height.Text = carinfo.CarInfo_Height;
                        txtCarInfo_Type.Text = carinfo.CarInfo_Type;
                        txtCarInfo_Weight.Text = carinfo.CarInfo_Weight;
                        txtCarName.Text = carinfo.CarInfo_Name;
                        txtRemark.Text = carinfo.CarInfo_Remark;
                        txtUserName.Text = carinfo.CarInfo_Operate;
                        comboxCarState.Text = carinfo.CarInfo_State;
                        txtNo.Text = carinfo.CarInfo_PO;
                        txtNo.Enabled = false;
                        CustomerInfo_ID = carinfo.CarInfo_CustomerInfo_ID.ToString();
                        sql = "select CustomerInfo_Name from CustomerInfo where CustomerInfo_ID=" + CustomerInfo_ID;
                        txtCustomerInfo.Text = LinQBaseDao.GetSingle(sql).ToString();
                        comboxCarType.SelectedValue = carinfo.CarInfo_CarType_ID;
                        if (!string.IsNullOrEmpty(carinfo.CarInfo_LevelWaste))
                        {
                            txtCustomerInfo_ADD.SelectedValue = carinfo.CarInfo_LevelWaste;
                        }
                        if (!Convert.ToBoolean(carinfo.CarInfo_Bail))
                        {
                            chkCarInfo_Bail.Checked = false;
                        }
                        else
                        {
                            chkCarInfo_Bail.Checked = true;
                        }
                    }

                    string smallTicketSql = "Select * from SmallTicket where  SmallTicket_CarInfo_ID=" + CommonalityEntity.CarInfo_ID + "";
                    SmallTicket st = LinQBaseDao.GetItemsForListing<SmallTicket>(smallTicketSql).FirstOrDefault();
                    if (st.SmallTicket_ID > 0)
                    {
                        SerialNumber = st.SmallTicket_Serialnumber;
                        if (st.SmallTicket_Serialnumber != "")
                        {

                            if (st.SmallTicket_Allowhour > 0)
                            {
                                txtSerTime.Text = st.SmallTicket_Allowhour.ToString();
                                //txtSerTime.Enabled = true;
                            }
                            if (st.SmallTicket_Allowcount > 0)
                            {
                                txtSerCount.Text = st.SmallTicket_Allowcount.ToString();
                                //txtSerCount.Enabled = true;
                            }

                        }
                    }
                }

            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 绑定驾驶员信息（修改）
        /// </summary>
        private void GetStaffInfo()
        {
            try
            {
                string sql = "select * from StaffInfo where StaffInfo_ID in(select  StaffInfo_CarINfo_StaffInfo_ID from StaffInfo_CarInfo where staffInfo_CarInfo_SmallTicket_ID=(select SmallTicket_ID from SmallTicket where SmallTicket_CarInfo_ID=" + CommonalityEntity.CarInfo_ID + "))";

                List<StaffInfo> list = LinQBaseDao.GetItemsForListing<StaffInfo>(sql).ToList();
                if (list.Count > 0)
                {
                    txtStaffName.Text = list[0].StaffInfo_Name;
                    txtPhone.Text = list[0].StaffInfo_Phone;
                    txtStaffInfo_Identity.Text = list[0].StaffInfo_Identity;

                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoForm GetStaffInfo()");
            }
        }
        /// <summary>
        /// 显示图片(修改)
        /// </summary>
        private void ShowImage()
        {
            try
            {

                string sql = "Select * from CarPic where CarPic_CarInfo_ID=" + CommonalityEntity.CarInfo_ID + "";
                List<CarPic> list = new List<CarPic>();
                list = LinQBaseDao.GetItemsForListing<CarPic>(sql).ToList();
                if (list != null)
                {
                    //得到图片的路径
                    string path = SystemClass.SaveFile;

                    //将得到的图片信息绑定到页面
                    int i = 0;
                    foreach (var item in list)
                    {
                        if (i < 5)
                        {
                            PictureBox pb = new PictureBox();
                            Button bt = new Button();
                            bt.Location = new Point(47 + 165 * i, 183);
                            int x = (20 + 165 * i);
                            int y = 20;
                            pb.Location = new Point(x, y);
                            pb.Width = 145;
                            pb.Height = 160;
                            pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage; ;
                            bt.Size = new Size(75, 23);
                            pb.MouseHover += new System.EventHandler(this.pbInImageOne_MouseHover);
                            pb.MouseLeave += new System.EventHandler(this.pbInImageOne_MouseLeave);
                            bt.Name = "btn" + (i + 1).ToString();
                            pb.Tag = "btn_" + (i + 1).ToString();
                            bt.Text = "删除图片";
                            pb.Name = "pictureBox" + (i + 1).ToString();
                            pb.Tag = path + item.CarPic_Add;
                            pb.ImageLocation = path + item.CarPic_Add;
                            bt.Click += new EventHandler(btnClick_Click);
                            bt.Tag = path + item.CarPic_Add;
                            this.gbShowImage.Controls.Add(pb);
                            this.gbShowImage.Controls.Add(bt);
                            // CheckProperties.ce.Imagelist.Add(fileName);
                            i++;
                        }
                    }
                }

            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 绑定车辆类型 车辆类型状态为启动
        /// </summary>
        public void LoadCarType()
        {
            try
            {
                string sql = "";
                if (CommonalityEntity.ISlogin)
                {
                    if (Sapid == "")
                    {
                        sql = "Select CarType_ID,CarType_Name from CarType where CarType_State='启动'  and CarType_Property not in('内部车辆','成品车辆','送货车辆','三废车辆') ";
                    }
                    else
                    {
                        sql = "Select CarType_ID,CarType_Name from CarType  where CarType_State='启动'  and CarType_Property  in ('成品车辆','送货车辆','三废车辆')";
                    }
                }
                else
                {
                    sql = "Select CarType_ID,CarType_Name from CarType where CarType_State='启动'";
                }
                DataTable dt = LinQBaseDao.Query(sql).Tables[0];
                DataRow dr = dt.NewRow();
                dr["CarType_ID"] = "0";
                dr["CarType_Name"] = "";
                dt.Rows.InsertAt(dr, 0);
                comboxCarType.DataSource = dt;
                comboxCarType.ValueMember = "CarType_ID";
                comboxCarType.DisplayMember = "CarType_Name";

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoForm LoadCarType()");
            }
        }
        /// <summary>
        /// 绑定字典状态信息
        /// </summary>
        public void LoadCarState()
        {
            try
            {
                string sql = "Select * from Dictionary where Dictionary_OtherID='1'";
                comboxCarState.DataSource = PositionDAL.GetDictionary(sql);
                comboxCarState.ValueMember = "Dictionary_Value";
                comboxCarState.DisplayMember = "Dictionary_Name";
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoForm LoadCarState()");
            }
        }



        /// <summary>
        ///是否还在优先期内
        /// </summary>
        /// <returns></returns>
        private bool ISHaveCarPrecedence(DataRow dr)
        {
            string CarPrecedence_CarNO = dr["CarPrecedence_CarNO"].ToString();
            string CarPrecedence_StaffInfo_ID = dr["CarPrecedence_StaffInfo_ID"].ToString();
            string CarPrecedence_CustomerInfo_ID = dr["CarPrecedence_CustomerInfo_ID"].ToString();
            string CarPrecedence_TotalCount = dr["CarPrecedence_TotalCount"].ToString();
            string CarPrecedence_TotalCountED = dr["CarPrecedence_TotalCountED"].ToString();
            string CarPrecedence_Type = dr["CarPrecedence_Type"].ToString();
            DateTime CarPrecedence_BeginTime = DateTime.MinValue;
            DateTime CarPrecedence_EndTime = DateTime.MinValue;
            if (dr["CarPrecedence_BeginTime"].ToString() != "" && dr["CarPrecedence_EndTime"].ToString() != "")
            {
                CarPrecedence_BeginTime = Convert.ToDateTime(dr["CarPrecedence_BeginTime"]);
                CarPrecedence_EndTime = Convert.ToDateTime(dr["CarPrecedence_EndTime"]);

            }
            string CarPrecedence_CarTypeNames = dr["CarPrecedence_CarTypeNames"].ToString();

            if (CarPrecedence_TotalCount != "")
            {
                if (CarPrecedence_TotalCountED != "" || string.IsNullOrEmpty(CarPrecedence_TotalCountED))
                {
                    if (CarPrecedence_TotalCountED == CarPrecedence_TotalCount)
                    {
                        return false;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(CarPrecedence_CarTypeNames))
                        {
                            ctype = 1;
                            return true;
                        }
                        else
                        {
                            bool ist = false;
                            string ctname = comboxCarType.Text;
                            string[] ctnames = CarPrecedence_CarTypeNames.Split(';');
                            foreach (var item in ctnames)
                            {
                                if (item.ToString() == ctname)
                                {
                                    ist = true;
                                    break;
                                }
                            }
                            if (ist)
                            {
                                ctype = 1;
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            if (CarPrecedence_BeginTime != DateTime.MinValue || CarPrecedence_EndTime != DateTime.MinValue)
            {
                if (CarPrecedence_BeginTime > CommonalityEntity.GetServersTime())
                {
                    return false;
                }
                if (CarPrecedence_EndTime < CommonalityEntity.GetServersTime())
                {
                    return false;
                }
                if (string.IsNullOrEmpty(CarPrecedence_CarTypeNames))
                {
                    ctype = 2;
                    return true;
                }
                else
                {
                    bool ist = false;
                    string ctname = comboxCarType.Text;
                    string[] ctnames = CarPrecedence_CarTypeNames.Split(';');
                    foreach (var item in ctnames)
                    {
                        if (item.ToString() == ctname)
                        {
                            ist = true;
                            break;
                        }
                    }
                    if (ist)
                    {
                        ctype = 2;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            if (string.IsNullOrEmpty(CarPrecedence_CarTypeNames))
            {
                return true;
            }
            else
            {
                bool ist = false;
                string ctname = comboxCarType.Text;
                string[] ctnames = CarPrecedence_CarTypeNames.Split(';');
                foreach (var item in ctnames)
                {
                    if (item.ToString() == ctname)
                    {
                        ist = true;
                        break;
                    }
                }
                if (ist)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// 保存登记(车辆信息,凭证信息，排队信息)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPhone.Text.Trim() == "")
                {
                    MessageBox.Show("手机号不能为空！", "提示");
                    txtPhone.Enabled = true;
                    return;
                }
                if (txtStaffInfo_Identity.Text.Trim() == "")
                {
                    MessageBox.Show("身份证号不能为空！", "提示");
                    return;
                }
                if (comboxCarType.Text == "")
                {
                    MessageBox.Show("车辆类型不能为空！", "提示");
                    return;
                }
                string phone = "[1]+\\d{10}$";
                if (!Regex.IsMatch(txtPhone.Text.Trim(), phone))
                {
                    MessageBox.Show("手机号格式不正确！", "提示");
                    return;
                }
                if (!Regex.IsMatch(txtStaffInfo_Identity.Text.Trim(), @"(^\d{17}(?:\d{1}|x|X)$)|(^\d{15}$)"))
                {
                    MessageBox.Show("身份证号码格式不正确！", "提示");
                    return;
                }
                string Strategy_DriSName = "";//通行策略名称
                CommonalityEntity.IsUpdatedri = false;
                //查询车辆类型的通行策略
                DataTable DrivewayStrategy_IDDT = LinQBaseDao.Query("select DrivewayStrategy_ID,DrivewayStrategy_Sort ,DrivewayStrategy_Name from DrivewayStrategy where DrivewayStrategy_State='启动' and  DrivewayStrategy_Name in (select CarType_DriSName from CarType  where CarType_Name='" + comboxCarType.Text + "') order by DrivewayStrategy_Sort ").Tables[0];
                if (DrivewayStrategy_IDDT.Rows.Count <= 0)
                {
                    MessageBox.Show("该车辆没有配通行策略！", "提示");
                    return;
                }
                Strategy_DriSName = DrivewayStrategy_IDDT.Rows[0][2].ToString();
                CommonalityEntity.Driveway_ID = Convert.ToInt32(LinQBaseDao.Query("select DrivewayStrategy_Driveway_ID from DrivewayStrategy where DrivewayStrategy.DrivewayStrategy_ID=" + DrivewayStrategy_IDDT.Rows[0][0].ToString()).Tables[0].Rows[0]["DrivewayStrategy_Driveway_ID"]);
                DataTable dtption = LinQBaseDao.Query("select * from Position where Position_ID in (select Driveway_Position_ID from Driveway where Driveway_ID=" + CommonalityEntity.Driveway_ID + ")").Tables[0];
                CommonalityEntity.Driveway_Value = LinQBaseDao.GetSingle("select Driveway_Value from Driveway where Driveway_ID=" + CommonalityEntity.Driveway_ID).ToString();
                CommonalityEntity.Position_ID = Convert.ToInt32(dtption.Rows[0]["Position_ID"].ToString());
                CommonalityEntity.Position_Value = dtption.Rows[0]["Position_Value"].ToString();
                CommonalityEntity.yxincheck = false;

                #region 修改车辆


                if (CommonalityEntity.UpdateCar)
                {
                    try
                    {
                        if (!ChkIsNullOrEmpty()) return;//验证用户输入数据 若数据验证失败，则不进行修改。
                        #region

                        string cSql = "select * from CarType where CarType_id='" + comboxCarType.SelectedValue + "'";
                        CheckProperties.ce.IsPhoto = false;
                        CheckProperties.ce.levelIsWaste = false;
                        CheckProperties.ce.isSort = false;
                        CheckProperties.ce.ISUpdateCredentials = false;
                        CommonalityEntity.ISYX = false;
                        CheckProperties.ce.IsState = false;
                        CommonalityEntity.Car_Type_ID = comboxCarType.SelectedValue.ToString();
                        CheckProperties.ce.CarTypeKey = Convert.ToInt32(comboxCarType.SelectedValue).ToString();
                        CommonalityEntity.CarType[comboxCarType.SelectedValue.ToString()] = comboxCarType.Text.Trim();
                        CheckProperties.ce.CarType_Name = comboxCarType.Text.Trim();
                        CheckProperties.ce.carInfo_LevelWaste = txtCustomerInfo_ADD.Text.Trim();
                        CarType cart = new CarType();
                        cart = LinQBaseDao.GetItemsForListing<CarType>(cSql).FirstOrDefault();
                        if (cart != null)
                        {
                            carType_Value = cart.CarType_Value;
                            CheckProperties.ce.carType_Value = carType_Value;
                        }

                        DataTable dtx = LinQBaseDao.Query("select StaffInfo_ID from StaffInfo where  StaffInfo_Identity = '" + txtStaffInfo_Identity.Text.Trim() + "'").Tables[0];
                        if (dtx.Rows.Count > 0)
                        {
                            staffInfo_Id = dtx.Rows[0][0].ToString();
                            CheckProperties.ce.staffInfo_ID = staffInfo_Id;
                            LinQBaseDao.Query("update StaffInfo  set  StaffInfo_Phone='" + txtPhone.Text.Trim() + "',StaffInfo_Name='" + txtStaffName.Text.Trim() + "'  where StaffInfo_ID=" + staffInfo_Id);
                        }
                        else
                        {
                            staffInfo_Id = LinQBaseDao.GetSingle("Insert into StaffInfo(StaffInfo_Type,StaffInfo_Name,StaffInfo_Phone,StaffInfo_Time,StaffInfo_State,StaffInfo_Identity) values('驾驶员','" + txtStaffName.Text.Trim() + "','" + txtPhone.Text.Trim() + "',GETDATE(),'启动','" + txtStaffInfo_Identity.Text.Trim() + "')      select @@identity").ToString();
                            CheckProperties.ce.staffInfo_ID = staffInfo_Id;
                        }
                        CheckProperties.ce.carInfo_Name = txtCarName.Text.ToString();//车牌号


                        if (txtCustomerInfo.Text.Trim() != "")
                        {
                            DataTable CustomerInfo_IDDT = LinQBaseDao.Query("select CustomerInfo_ID from CustomerInfo where CustomerInfo_Name='" + txtCustomerInfo.Text.Trim() + "'").Tables[0];
                            //如果数据库有就赋值没有就添加
                            if (CustomerInfo_IDDT.Rows.Count > 0)
                            {
                                CustomerInfo_ID = CustomerInfo_IDDT.Rows[0][0].ToString();
                                CheckProperties.ce.customerInfo_ID = int.Parse(CustomerInfo_ID);
                            }
                            else
                            {
                                CustomerInfo_ID = LinQBaseDao.GetSingle("insert CustomerInfo(CustomerInfo_Name,CustomerInfo_State,CustomerInfo_Time) values('" + txtCustomerInfo.Text.Trim() + "','启动',GETDATE())  select @@identity").ToString();

                                ErroSql.Add("delete CustomerInfo where CustomerInfo_ID =" + CustomerInfo_ID);
                                CheckProperties.ce.customerInfo_ID = int.Parse(CustomerInfo_ID);
                            }
                        }
                        CheckProperties.ce.SongHuoNumber = txtNo.Text;
                        CheckProperties.ce.ChengPinNumber = txtCarName.Text;
                        CheckProperties.ce.SangFeiNumber = txtNo.Text;

                        CheckProperties.ce.carType_ID = int.Parse(comboxCarType.SelectedValue.ToString());
                        string carIds = "";

                        Expression<Func<Car, bool>> carFn1 = n => n.Car_Name == txtCarName.Text.Trim();

                        Car car1 = CarDAL.Query(carFn1).FirstOrDefault();
                        if (car1 != null)
                        {
                            carIds = car1.Car_ID.ToString();
                        }
                        else
                        {
                            string Sql = "insert into car(Car_CarType_ID,Car_CustomerInfo_ID,Car_Name,Car_State,Car_ISRegister,Car_CreateTime,Car_ISStaffInfo) values(" + comboxCarType.SelectedValue + "," + CustomerInfo_ID + ",'" + txtCarName.Text.Trim() + "','" + comboxCarState.Text + "',0,'" + CommonalityEntity.GetServersTime() + "',0) select @@identity";
                            carIds = LinQBaseDao.GetSingle(Sql).ToString();
                            ErroSql.Add("delete car where Car_ID=" + carIds);//增删改出错后需要执行的SQL

                        }

                        #endregion

                        #region 修改车辆信息
                        Action<CarInfo> action = c =>
                        {
                            c.CarInfo_Height = txtCarInfo_Height.Text.ToString();
                            c.CarInfo_Carriage = txtCarInfo_Carriage.Text.ToString();
                            c.CarInfo_CustomerInfo_ID = int.Parse(CustomerInfo_ID);
                            c.CarInfo_LevelWaste = txtCustomerInfo_ADD.Text.ToString();
                            c.CarInfo_Name = txtCarName.Text.ToString();
                            c.CarInfo_Weight = txtCarInfo_Weight.Text.Trim();
                            c.CarInfo_State = comboxCarState.Text.ToString();
                            c.CarInfo_Remark = txtRemark.Text.Trim();
                            c.CarInfo_Type = txtCarInfo_Type.Text.Trim();
                            c.CarInfo_PO = txtNo.Text.Trim();
                            if (!string.IsNullOrEmpty(comboxBusinessType.Text))
                            {
                                c.CarInfo_BusinessType_ID = Convert.ToInt32(comboxBusinessType.SelectedValue.ToString());
                            }
                            c.CarInfo_Car_ID = Convert.ToInt32(carIds);
                        };
                        Expression<Func<CarInfo, bool>> func = c => c.CarInfo_ID == int.Parse(CommonalityEntity.CarInfo_ID);
                        if (LinQBaseDao.Update(new DCCarManagementDataContext(), func, action))
                        {
                            //CommonalityEntity.WriteLogData("修改", "修改车辆：" + txtCarName.Text.Trim(), CommonalityEntity.USERNAME);
                        }
                        #endregion

                        #region 修改凭证信息
                        SmallTicket st = new SmallTicket();
                        if (txtCarTime.Text.Trim() != "")
                        {
                            st.SmallTicket_Allowhour = int.Parse(txtCarTime.Text.Trim());
                        }
                        if (txtCarCount.Text.Trim() != "")
                        {
                            st.SmallTicket_Allowcount = int.Parse(txtCarCount.Text.Trim());
                        }
                        if (comboxICNumber.Text.Trim() != "")
                        {
                            DataTable ICCardDT = LinQBaseDao.Query("select ICCard.ICCard_ID from ICCard where ICCard.ICCard_Value='" + comboxICNumber.Text + "'").Tables[0];
                            if (ICCardDT.Rows.Count > 0)
                            {
                                st.SmallTicket_ICCard_ID = int.Parse(ICCardDT.Rows[0][0].ToString());
                            }
                        }
                        if (txtSerTime.Text.Trim() != "")
                        {
                            st.SmallTicket_Allowhour = int.Parse(txtSerTime.Text.Trim());
                        }
                        if (txtSerCount.Text.Trim() != "")
                        {
                            st.SmallTicket_Allowcount = int.Parse(txtSerCount.Text.Trim());
                        }
                        //凭证信息表
                        Action<SmallTicket> sAction = s =>
                        {
                            s.SmallTicket_ICCard_ID = st.SmallTicket_ICCard_ID;
                            s.SmallTicket_Allowcount = st.SmallTicket_Allowcount;
                            s.SmallTicket_Allowhour = st.SmallTicket_Allowhour;
                        };
                        string smallTicketSql = "Select * from SmallTicket where SmallTicket_CarInfo_ID=" + CommonalityEntity.CarInfo_ID + "";
                        SmallTicket stks = LinQBaseDao.GetItemsForListing<SmallTicket>(smallTicketSql).FirstOrDefault();
                        if (stks.SmallTicket_ID > 0)
                        {
                            Expression<Func<SmallTicket, bool>> sFun = s => s.SmallTicket_ID == stks.SmallTicket_ID;
                            if (LinQBaseDao.Update<SmallTicket>(new DCCarManagementDataContext(), sFun, sAction))
                            {
                                //  CommonalityEntity.WriteLogData("修改", "修改凭证：" + comboxICNumber.Text.Trim(), CommonalityEntity.USERNAME);
                            }
                        }
                        #endregion

                        #region 修改驾驶员信息
                        if (staffInfo != null && staffInfo_Id != "")
                        {
                            string sfCrSql = "update StaffInfo_CarInfo set StaffInfo_CarInfo_StaffInfo_ID =" + staffInfo_Id + "   where StaffInfo_CarInfo_SmallTicket_ID=" + stks.SmallTicket_ID;
                            LinQBaseDao.Query(sfCrSql);

                            //CommonalityEntity.WriteLogData("修改", "修改驾驶员：" + txtStaffName.Text.Trim(), CommonalityEntity.USERNAME);
                        }
                        if (staffInfo == null && txtStaffName.Text == "")
                        {
                            MessageBox.Show("请选择或者输入驾驶员名称!");
                            return;
                        }
                        #endregion
                        MessageBox.Show("修改成功！");
                        CommonalityEntity.WriteLogData("修改", "修改车辆：" + txtCarName.Text.Trim() + "的相关信息", CommonalityEntity.USERNAME);
                    }
                    catch
                    {
                        CommonalityEntity.WriteTextLog("修改车辆信息错误：");
                    }
                    finally
                    {
                        comboxCarType.Enabled = true;
                        this.Close();
                        result = false;
                        btnSave.Text = "保存";
                    }
                    return;

                }
                #endregion

                #region 登记车辆信息
                if (!ChkIsNullOrEmpty()) return;//验证用户输入数据 若数据验证失败，则不进行添加操作。
                string sqlMan = "select ManagementStrategy_ID from  ManagementStrategy where ManagementStrategy_DriSName='" + Strategy_DriSName + "' and ManagementStrategy_Menu_ID=1  and ManagementStrategy_State='启动' and ManagementStrategy_Rule='ChkCarIcNumber'  order by ManagementStrategy_No ";
                DataTable dtman = LinQBaseDao.Query(sqlMan).Tables[0];
                if (dtman.Rows.Count > 0)
                {
                    if (comboxICNumber.Text == "")
                    {
                        MessageBox.Show("请刷车卡登记！");
                        return;
                    }
                    else
                    {
                        dtman = LinQBaseDao.Query("select * from Car where Car_ICCard_ID in (select ICCard_ID from ICCard where ICCard_Value='" + comboxICNumber.Text + "')").Tables[0];
                        if (dtman.Rows.Count > 0)
                        {
                            MessageBox.Show("请刷车卡登记！");
                            return;
                        }
                    }
                }

                sqlMan = "select ManagementStrategy_ID from  ManagementStrategy where ManagementStrategy_DriSName='" + Strategy_DriSName + "'  and ManagementStrategy_Menu_ID=1  and ManagementStrategy_State='启动' and ManagementStrategy_Rule='ChkUserIcNumber'  order by ManagementStrategy_No ";
                dtman = LinQBaseDao.Query(sqlMan).Tables[0];
                if (dtman.Rows.Count > 0)
                {
                    if (comboxICNumber.Text == "")
                    {
                        MessageBox.Show("请刷人卡登记！");
                        return;
                    }
                    else
                    {
                        dtman = LinQBaseDao.Query("select * from StaffInfo where StaffInfo_ICCard_ID in (select ICCard_ID from ICCard where ICCard_Value='" + comboxICNumber.Text + "')").Tables[0];
                        if (dtman.Rows.Count > 0)
                        {
                            MessageBox.Show("请刷人卡登记！");
                            return;
                        }
                    }
                }
                if (CommonalityEntity.ISsap)
                {
                    if (string.IsNullOrEmpty(txtNo.Text.Trim()))
                    {
                        MessageBox.Show("请输入单号！");
                        return;
                    }
                }
                if (ChkRepeat(txtCarName.Text.Trim()))
                {
                    string car_state = LinQBaseDao.GetSingle("select CarInfo_State from CarInfo where CarInfo_Name='" + txtCarName.Text.Trim() + "' order by CarInfo_ID desc").ToString();
                    if (car_state != "注销")
                    {
                        return;//查重
                    }
                }

                #region 为登记管控所需参数赋值
                string crSql = "select * from CarType where CarType_id='" + comboxCarType.SelectedValue + "'";
                CheckProperties.ce.IsPhoto = false;
                CheckProperties.ce.levelIsWaste = false;
                CheckProperties.ce.isSort = false;
                CheckProperties.ce.ISUpdateCredentials = false;
                CommonalityEntity.ISYX = false;
                CheckProperties.ce.IsState = false;
                CommonalityEntity.Car_Type_ID = comboxCarType.SelectedValue.ToString();
                CheckProperties.ce.carType_ID = int.Parse(comboxCarType.SelectedValue.ToString());
                CheckProperties.ce.CarTypeKey = Convert.ToInt32(comboxCarType.SelectedValue).ToString();
                CommonalityEntity.CarType[comboxCarType.SelectedValue.ToString()] = comboxCarType.Text.Trim();
                CheckProperties.ce.CarType_Name = comboxCarType.Text.Trim();
                CheckProperties.ce.carInfo_LevelWaste = txtCustomerInfo_ADD.Text.Trim();
                CheckProperties.ce.carInfo_Name = txtCarName.Text.Trim();
                CarType carty = new CarType();
                carty = LinQBaseDao.GetItemsForListing<CarType>(crSql).FirstOrDefault();
                if (carty != null)
                {
                    carType_Value = carty.CarType_Value;
                    CheckProperties.ce.carType_Value = carType_Value;
                }
                #region 车、人、公司,系统存在则取出相应的值，没有则添加
                DataTable DTCustomerInfo = LinQBaseDao.Query("select CustomerInfo_ID from CustomerInfo where CustomerInfo_Name='" + txtCustomerInfo.Text.Trim() + "'").Tables[0];
                //如果数据库有就赋值没有就添加
                if (DTCustomerInfo.Rows.Count > 0)
                {
                    CustomerInfo_ID = DTCustomerInfo.Rows[0][0].ToString();
                    CheckProperties.ce.customerInfo_ID = int.Parse(CustomerInfo_ID);
                }
                else
                {
                    CustomerInfo_ID = LinQBaseDao.GetSingle("insert CustomerInfo(CustomerInfo_Name,CustomerInfo_State,CustomerInfo_Time) values('" + txtCustomerInfo.Text.Trim() + "','启动',GETDATE())  select @@identity").ToString();

                    ErroSql.Add("delete CustomerInfo where CustomerInfo_ID =" + CustomerInfo_ID);
                    CheckProperties.ce.customerInfo_ID = int.Parse(CustomerInfo_ID);
                }
                string carId = "";
                Expression<Func<Car, bool>> carFn = n => n.Car_Name == txtCarName.Text.Trim();
                Car car = CarDAL.Query(carFn).FirstOrDefault();
                if (car != null)
                {
                    carId = car.Car_ID.ToString();
                }
                else
                {
                    string Sql = "insert into car(Car_CarType_ID,Car_CustomerInfo_ID,Car_Name,Car_State,Car_ISRegister,Car_CreateTime,Car_ISStaffInfo) values(" + comboxCarType.SelectedValue + "," + CustomerInfo_ID + ",'" + txtCarName.Text.Trim() + "','" + comboxCarState.Text + "',0,'" + CommonalityEntity.GetServersTime() + "',0) select @@identity";
                    carId = LinQBaseDao.GetSingle(Sql).ToString();
                    ErroSql.Add("delete car where Car_ID=" + carId);//增删改出错后需要执行的SQL
                }

                DataTable dtsf = LinQBaseDao.Query("select StaffInfo_ID from StaffInfo where StaffInfo_Identity = '" + txtStaffInfo_Identity.Text.Trim() + "'").Tables[0];
                if (dtsf.Rows.Count > 0)
                {
                    staffInfo_Id = dtsf.Rows[0][0].ToString();
                    CheckProperties.ce.staffInfo_ID = staffInfo_Id;
                    LinQBaseDao.Query("update StaffInfo  set  StaffInfo_Phone='" + txtPhone.Text.Trim() + "',StaffInfo_Name='" + txtStaffName.Text.Trim() + "'  where StaffInfo_ID=" + staffInfo_Id);
                }
                else
                {
                    staffInfo_Id = LinQBaseDao.GetSingle("Insert into StaffInfo(StaffInfo_Type,StaffInfo_Name,StaffInfo_Phone,StaffInfo_Time,StaffInfo_State,StaffInfo_Identity) values('驾驶员','" + txtStaffName.Text.Trim() + "','" + txtPhone.Text.Trim() + "',GETDATE(),'启动','" + txtStaffInfo_Identity.Text.Trim() + "')      select @@identity").ToString();
                    CheckProperties.ce.staffInfo_ID = staffInfo_Id;
                }
                #endregion

                CheckProperties.ce.SongHuoNumber = txtNo.Text;
                CheckProperties.ce.ChengPinNumber = txtCarName.Text;
                CheckProperties.ce.SangFeiNumber = txtNo.Text;

                #endregion


                #region 执行登记管控

                //根据车辆类型，获取车辆类型的管控策略（登记管控）
                string sql = "";
                if (CommonalityEntity.ISlogin)
                {
                    sql = "select * from  ManagementStrategy where ManagementStrategy_DriSName='" + Strategy_DriSName + "'  and ManagementStrategy_Menu_ID=1  and ManagementStrategy_State='启动' order by ManagementStrategy_No ";
                }
                else
                {
                    sql = "select * from  ManagementStrategy where ManagementStrategy_DriSName='" + Strategy_DriSName + "' and ManagementStrategy_Menu_ID=1  and ManagementStrategy_State='启动' and ManagementStrategy_Rule not in('ChkChengPin', 'ChkSapOflag','ISInCheckSapSave','ChkSongHuo','ChkSanFei')  order by ManagementStrategy_No ";
                }

                checkPr.ExecutionMethod(sql);//执行指定车辆类型的管控

                #endregion

                #region 管控执行结果

                if (CheckMethod.listMessage.Count > 0)
                {
                    StringBuilder strMessage = new StringBuilder();
                    foreach (string item in CheckMethod.listMessage)
                    {
                        strMessage.Append(item.Trim());
                    }
                    MessageBox.Show(strMessage.ToString());
                    return;

                }

                SerialNumber = CheckProperties.ce.serialNumber;
                if (SerialNumber.Trim() == "" && comboxICNumber.Text == null && CheckProperties.ce.stfIsICCard == false)
                {
                    MessageBox.Show("该车辆类型没有配置进出凭证并且驾驶员也没有进出凭证，请先配置!");
                    comboxCarType.SelectedIndex = -1;
                    return;
                }
                //验证该车辆的默认值和输入的通行值
                string CarTypeSql = "Select * from CarType where CarType_ID=" + comboxCarType.SelectedValue + "";
                CarType carTypes = LinQBaseDao.GetItemsForListing<CarType>(CarTypeSql).FirstOrDefault();
                if (carTypes.CarType_ValidityValue == null || carTypes.CarType_ValidityValue.ToString() == "0")
                {

                }
                else
                {
                    if (txtCarCount.Enabled)
                    {
                        if (carTypes.CarType_ValidityValue < int.Parse(txtCarCount.Text.Trim()))
                        {
                            MessageBox.Show("设置的通行次数不能大于该车辆类型的通行次数");
                            return;
                        }
                    }
                    if (txtCarTime.Enabled)
                    {
                        if (carTypes.CarType_ValidityValue < int.Parse(txtCarTime.Text.Trim()))
                        {
                            MessageBox.Show("设置的通行时间不能大于该车辆类型的通行时间");
                            return;
                        }
                    }
                    if (txtSerCount.Enabled)
                    {
                        if (carTypes.CarType_ValidityValue < int.Parse(txtSerCount.Text.Trim()))
                        {
                            MessageBox.Show("设置的通行次数不能大于该车辆类型的通行次数");
                            return;
                        }
                    }
                    if (txtSerTime.Enabled)
                    {
                        if (carTypes.CarType_ValidityValue < int.Parse(txtSerTime.Text.Trim()))
                        {
                            MessageBox.Show("设置的通行时间不能大于该车辆类型的通行时间");
                            return;
                        }
                    }
                }
                #endregion

                #region 优先车校验
                //查询是否属于优先车辆
                bool isPaiDui = false;
                if (CheckProperties.ce.isSort)
                {
                    if (CommonalityEntity.ISYX)
                    {
                        CommonalityEntity.ISYX = false;
                        isPaiDui = ChkIsYX(staffInfo_Id, CustomerInfo_ID);
                        if (isPaiDui)
                        {
                            CheckProperties.ce.isSort = false;
                        }
                        else
                        {
                            CheckProperties.ce.isSort = true;
                        }
                    }
                }
                #endregion

                if (CheckProperties.ce.ISUpdateCredentials)
                {
                    CheckProperties.ce.ISUpdateCredentials = false;
                }

                #region 上传图片
                List<string> list = new List<string>();

                if (CheckProperties.ce.isImageList)
                {
                    if (CheckProperties.ce.Imagelist.Count <= 0)
                    {
                        DialogResult dialogresult = MessageBox.Show("没有照片信息，是否异常登记？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (DialogResult.OK == dialogresult)
                        {
                            #region 车辆信息
                            carInfo_ID = insertcar(carId);//得到当前的车辆编号
                            CheckProperties.ce.CarInfo_ID = carInfo_ID;
                            #endregion
                            CommonalityEntity.WriteLogData("登记", "没有图片，异常登记", CommonalityEntity.USERNAME);
                            CommonalityEntity.ADDUnusualRecord(1, "登记图片校验", "没有图片，异常登记", CommonalityEntity.USERNAME, CheckProperties.ce.CarInfo_ID);
                            #region 记录异常登记
                            ur = new UnusualRecord();
                            ur.UnusualRecord_Operate = CommonalityEntity.USERNAME;
                            ur.UnusualRecord_Reason = "没有图片，异常登记";
                            ur.UnusualRecord_State = "启动";
                            ur.UnusualRecord_Time = CommonalityEntity.GetServersTime();
                            ur.UnusualRecord_Type = "登记异常";
                            //ur.UnusualRecord_UnusualType_ID = 1;
                            ur.UnusualRecord_Site = "CarInfo";
                            ur.UnusualRecord_SiteID = carInfo_ID;
                            int num = 0;
                            UnusualRecordDAL.InsertOneUnuRecord(ur, out num);
                            #endregion
                            isInsert = true;
                            ErroSql.Add("delete UnusualRecord where UnusualRecord_ID=" + num);//增删改出错后需要执行的SQL
                            ErroSql.Add("delete CarInfo where CarInfo_ID=" + carInfo_ID);//增删改出错后需要执行的SQL
                        }
                        else
                        {
                            isInsert = false;
                            return;
                        }
                    }
                    else
                    {
                        #region 车辆信息
                        carInfo_ID = insertcar(carId);//得到当前的车辆编号
                        ErroSql.Add("delete CarInfo where CarInfo_ID=" + carInfo_ID);//增删改出错后需要执行的SQL
                        #endregion
                        //将图片上传到服务器
                        ThreadPool.QueueUserWorkItem(new WaitCallback(upimage), null);
                        isInsert = true;
                    }
                }
                else
                {
                    #region 车辆信息
                    isInsert = true;
                    carInfo_ID = insertcar(carId);//得到当前的车辆编号
                    ErroSql.Add("delete CarInfo where CarInfo_ID=" + carInfo_ID);//增删改出错后需要执行的SQL
                    #endregion
                }
                #endregion

                if (isInsert)
                {
                    CheckProperties.ce.CarInfo_ID = carInfo_ID;
                    //txtSerCount.Text = CheckProperties.ce.serCount;
                    //txtSerTime.Text = CheckProperties.ce.serTime;
                    //emewe 103 取消排队字段： order by DrivewayStrategy_Sort查询
                    string csql = "select * from Driveway where Driveway_ID in (select top 1 drivewayStrategy_Driveway_ID from DrivewayStrategy where DrivewayStrategy_Name in( select CarType_DriSName from CarType where CarType_ID =" + comboxCarType.SelectedValue + ")  ) and Driveway_Type='进'";
                    DataTable dtdriveway = LinQBaseDao.Query(csql).Tables[0];

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
                    cir.CarInOutRecord_Time = CommonalityEntity.GetServersTime();
                    cir.CarInOutRecord_Abnormal = "正常";
                    cir.CarInOutRecord_State = "启动";
                    cir.CarInOutRecord_DrivewayStrategyS = strstrategy.TrimEnd(',');
                    cir.CarInOutRecord_CarInfo_ID = carInfo_ID;
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
                    CarInOutRecord_ID = int.Parse(LinQBaseDao.GetSingle(CarInOutRecordSql).ToString());
                    ErroSql.Add("delete CarInOutRecord where CarInOutRecord_ID=" + CarInOutRecord_ID);//增删改出错后需要执行的SQL
                    #endregion

                    #region  业务记录进门授权
                    //BusinessRecord busr = new BusinessRecord();
                    //busr.BusinessRecord_CarInOutRecord_ID = CarInOutRecord_ID;
                    //busr.BusinessRecord_UnloadEmpower = 0;
                    //busr.BusinessRecord_Type = "进门授权";
                    //busr.BusinessRecord_State = "启动";
                    ////新增一条记录
                    //LinQBaseDao.InsertOne<BusinessRecord>(new DCCarManagementDataContext(), busr);
                    //string BusinessRecordsql = "select BusinessRecord_ID from BusinessRecord where BusinessRecord_CarInOutRecord_ID=" + CarInOutRecord_ID;
                    //BusinessRecord_ID = int.Parse(LinQBaseDao.GetSingle(BusinessRecordsql).ToString());
                    //ErroSql.Add("delete BusinessRecord where BusinessRecord_ID=" + BusinessRecord_ID);//增删改出错后需要执行的SQL
                    #endregion

                    #region 进出凭证信息
                    SmallTicket stk = new SmallTicket();

                    if (txtCarCount.Text.Trim() != "")
                    {
                        stk.SmallTicket_Allowcount = int.Parse(txtCarCount.Text.Trim());
                    }
                    if (txtSerCount.Text.Trim() != "")
                    {
                        stk.SmallTicket_Allowcount = int.Parse(txtSerCount.Text.Trim());
                    }

                    if (txtCarTime.Text.Trim() != "")
                    {
                        stk.SmallTicket_Allowhour = int.Parse(txtCarTime.Text.Trim());
                    }

                    if (txtSerTime.Text.Trim() != "")
                    {
                        stk.SmallTicket_Allowhour = int.Parse(txtSerTime.Text.Trim());
                    }
                    CheckProperties.ce.serCount = "";
                    CheckProperties.ce.serTime = "";

                    //车辆编号
                    stk.SmallTicket_CarInfo_ID = carInfo_ID;
                    if (!string.IsNullOrEmpty(comboxICNumber.Text.Trim()))
                    {
                        stk.SmallTicket_Type += "IC卡";
                        if (iCNumberID == -1)
                        {
                            string icSql = "select * from ICCard where ICCard_Value='" + comboxICNumber.Text.ToString() + "' and ICCard_State='启动'";
                            if (ICCardDAL.GetViewICCardName(icSql).Count() <= 0)
                            {
                                MessageBox.Show("该IC卡状态未启动!");
                                return;
                            }
                            else
                            {
                                iCNumberID = ICCardDAL.GetViewICCardName(icSql).FirstOrDefault().ICCard_ID;
                            }
                        }
                        stk.SmallTicket_ICCard_ID = iCNumberID;
                    }

                    stk.SmallTicket_Position_ID = CommonalityEntity.Position_ID;
                    stk.SmallTicket_PrintNumber = "";
                    stk.SmallTicket_Remark = "";
                    //设置排队序号198
                    if (CheckProperties.ce.isSort)
                    {
                        DataTable dtm = LinQBaseDao.Query("select * from ManagementStrategy where ManagementStrategy_Rule='GetSortNumber' and ManagementStrategy_State='启动' and ManagementStrategy_Menu_ID=1 and ManagementStrategy_DriSName in (select CarType_DriSName from CarType where CarType_ID=" + comboxCarType.SelectedValue + ")").Tables[0];

                        if (dtm.Rows.Count > 0)
                        {
                            CheckMethod.GetSortNumber();
                        }

                        //根据当前排队的序号，生成排队号
                        string number = CommonalityEntity.SortNumber(CheckProperties.ce.sort_Value);
                        stk.SmallTicket_SortNumber = CommonalityEntity.Position_Value + carType_Value + number;
                    }
                    else
                    {
                        stk.SmallTicket_SortNumber = "";
                    }
                    stk.SmallTicket_State = "有效";
                    stk.SmallTicket_Time = CommonalityEntity.GetServersTime();
                    if (SerialNumber.Trim() != "")
                    {
                        stk.SmallTicket_Type += "小票";

                        DataTable dtm = LinQBaseDao.Query("select * from ManagementStrategy where ManagementStrategy_Rule='ChkSerialNumber' and ManagementStrategy_State='启动' and ManagementStrategy_Menu_ID=1 and ManagementStrategy_DriSName in (select CarType_DriSName from CarType where CarType_ID=" + comboxCarType.SelectedValue + ")").Tables[0];

                        if (dtm.Rows.Count > 0)
                        {
                            CheckMethod.ChkSerialNumber();
                            SerialNumber = CheckProperties.ce.serialNumber;
                        }
                        stk.SmallTicket_Serialnumber = SerialNumber;
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


                    ErroSql.Add("delete SmallTicket where SmallTicket_ID=" + smallTicket_ID);//增删改出错后需要执行的SQL


                    //删除人车关联表（！！！重要，位置不可改变）否则不能删除进出凭证信息
                    ErroSql.Add("delete StaffInfo_CarInfo where StaffInfo_CarInfo_SmallTicket_ID=" + smallTicket_ID);//增删改出错后需要执行的SQL

                    #endregion

                    #region 排队信息
                    SortNumberInfo sort = new SortNumberInfo();
                    //根据车辆类型  得到该车辆类型的信息

                    if (dtdriveway.Rows.Count > 0)
                    {
                        sort.SortNumberInfo_DrivewayValue = CommonalityEntity.Driveway_Value;
                        //门岗值
                        sort.SortNumberInfo_PositionValue = CommonalityEntity.Position_Value;

                    }

                    //车辆类型值
                    sort.SortNumberInfo_CarTypeValue = carType_Value;
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
                        sort.SortNumberInfo_SortValue = CheckProperties.ce.sort_Value + 1;
                        sort.SortNumberInfo_TongXing = "排队中";
                    }
                    else
                    {
                        sort.SortNumberInfo_SortValue = null;
                        sort.SortNumberInfo_TongXing = "待通行";
                        if (CommonalityEntity.yxincheck)
                        {
                            LinQBaseDao.Query("update CarInOutRecord set CarInOutRecord_InCheck='是',CarInOutRecord_InCheckTime='" + CommonalityEntity.GetServersTime() + "',CarInOutRecord_InCheckUser='" + CommonalityEntity.USERNAME + "' where CarInOutRecord_ID=" + CarInOutRecord_ID);
                        }
                    }
                    sort.SortNumberInfo_State = "启动";
                    sort.SortNumberInfo_Type = "系统生成";
                    sort.SortNumberInfo_Time = CommonalityEntity.GetServersTime();

                    //if (CheckProperties.ce.QueuingDisorder)
                    //{
                    //    sort.SortNumberInfo_ISQueuingDisorder = 1.ToString();

                    //}
                    //else
                    //{
                    //    sort.SortNumberInfo_ISQueuingDisorder = 0.ToString();
                    //}


                    //添加排队信息
                    ///sort.SortNumberInfo_PositionValue 取消
                    string sort_Sql = "insert into SortNumberInfo values('" + sort.SortNumberInfo_SmallTicket_ID + "','" + sort.SortNumberInfo_Time + "','" + sort.SortNumberInfo_Reasons + "','" + sort.SortNumberInfo_Operate + "','" + sort.SortNumberInfo_Type + "','" + sort.SortNumberInfo_SortValue + "','" + positionstr + "','" + sort.SortNumberInfo_CarTypeValue + "','" + sort.SortNumberInfo_CallCount + "','" + sort.SortNumberInfo_SMSCount + "','" + sort.SortNumberInfo_LEDCount + "','" + sort.SortNumberInfo_Remark + "','" + sort.SortNumberInfo_TongXing + "','" + sort.SortNumberInfo_DrivewayValue + "','" + sort.SortNumberInfo_State + "','" + sort.SortNumberInfo_CallTime + "','" + sort.SortNumberInfo_Number + "', 1 ,0)";
                    sort_Sql = sort_Sql + " select @@identity";
                    sortNumberInfo_ID = int.Parse(LinQBaseDao.GetSingle(sort_Sql).ToString());
                    ErroSql.Add("delete SortNumberInfo where SortNumberInfo_ID=" + sortNumberInfo_ID);//增删改出错后需要执行的SQL

                    #endregion

                    #region 添加关联表信息
                    if (carInfo_ID > 0)
                    {

                        if (string.IsNullOrEmpty(staffInfo_Id))
                        {
                            DataTable dtsfin = LinQBaseDao.Query("select StaffInfo_ID from StaffInfo where StaffInfo_Identity = '" + txtStaffInfo_Identity.Text.Trim() + "'").Tables[0];
                            if (dtsfin.Rows.Count > 0)
                            {
                                staffInfo_Id = dtsfin.Rows[0][0].ToString();
                                CheckProperties.ce.staffInfo_ID = staffInfo_Id;
                                LinQBaseDao.Query("update StaffInfo  set  StaffInfo_Phone='" + txtPhone.Text.Trim() + "',StaffInfo_Name='" + txtStaffName.Text.Trim() + "'  where StaffInfo_ID=" + staffInfo_Id);
                            }
                            else
                            {
                                staffInfo_Id = LinQBaseDao.GetSingle("Insert into StaffInfo(StaffInfo_Type,StaffInfo_Name,StaffInfo_Phone,StaffInfo_Time,StaffInfo_State,StaffInfo_Identity) values('驾驶员','" + txtStaffName.Text.Trim() + "','" + txtPhone.Text.Trim() + "',GETDATE(),'启动','" + txtStaffInfo_Identity.Text.Trim() + "')      select @@identity").ToString();
                                CheckProperties.ce.staffInfo_ID = staffInfo_Id;
                            }
                        }
                        if (string.IsNullOrEmpty(staffInfo_Id))
                        {
                            MessageBox.Show("登记失败！");
                            string sqlstr = "";
                            for (int i = ErroSql.Count - 1; i >= 0; i--)
                            {
                                sqlstr += ErroSql[i] + "     ";
                            }
                            if (sqlstr != "")
                                LinQBaseDao.Query(sqlstr);
                            return;
                        }
                        string sfCrSql = "Insert into StaffInfo_CarInfo values(" + staffInfo_Id + "," + smallTicket_ID + ")";
                        sfCrSql = sfCrSql + " select @@identity";
                        string sc = LinQBaseDao.GetSingle(sfCrSql).ToString();
                        //ErroSql.Add("delete StaffInfo_CarInfo where StaffInfo_CarInfo_ID=" + sc);//增删改出错后需要执行的SQL
                    }
                    #endregion
                    #region 判断是否登记成功
                    if (sortNumberInfo_ID > 0 && smallTicket_ID > 0 && carInfo_ID > 0 && CarInOutRecord_ID != -1)
                    {

                        if (Sapid != "" && SerialNumber != "")
                        {
                            if (comboxCarType.Text.Trim() == "成品纸车辆")
                            {
                                LinQBaseDao.Query("update eh_SAPRecord set Sap_Serialnumber=" + SerialNumber + ", Sap_State=1 where Sap_ID=" + Sapid);
                            }
                            else
                            {
                                LinQBaseDao.Query("update eh_SAPRecord set Sap_Serialnumber=" + SerialNumber + ", Sap_State=1,Sap_InCarNumber='" + txtCarName.Text.Trim() + "' where Sap_ID=" + Sapid);

                            }
                            Sapid = "";
                            if (!CommonalityEntity.GetSAP(txtNo.Text.Trim(), "A", txtCarName.Text.Trim(), SerialNumber, "1"))
                            {
                                MessageBox.Show(CheckProperties.ce.CarType_Name + "SAP验证未通过！");
                                return;
                            }
                        }
                        if (Sapid != "" && SerialNumber == "" && comboxICNumber.Text != "")
                        {
                            if (comboxCarType.Text.Trim() == "成品纸车辆")
                            {
                                LinQBaseDao.Query("update eh_SAPRecord set Sap_Serialnumber=" + SerialNumber + ", Sap_State=1 where Sap_ID=" + Sapid);
                            }
                            else
                            {
                                LinQBaseDao.Query("update eh_SAPRecord set Sap_Serialnumber=" + SerialNumber + ", Sap_State=1,Sap_InCarNumber='" + txtCarName.Text.Trim() + "' where Sap_ID=" + Sapid);

                            }
                            Sapid = "";
                            if (!CommonalityEntity.GetSAP(txtNo.Text.Trim(), "A", txtCarName.Text.Trim(), comboxICNumber.Text, "1"))
                            {
                                MessageBox.Show(CheckProperties.ce.CarType_Name + "SAP验证未通过！");
                                return;
                            }
                        }
                        CommonalityEntity.CarInfo_ID = carInfo_ID.ToString();
                        MessageBox.Show("登记成功！");
                        CommonalityEntity.WriteLogData("登记", "登记" + txtCarName.Text + "车辆", CommonalityEntity.USERNAME);//添加操作日志

                        if (SerialNumber.Trim() == "")
                        {
                            return;
                        }
                        //获取打印设置
                        string prtSql = "select top 1 * from PrintInfo where Print_State='启动' and Print_CarType_ID=" + comboxCarType.SelectedValue + "";
                        if (CheckProperties.ce.isSerialNumber)
                        {
                            CheckProperties.ce.isSerialNumber = false;
                            PrintInfo pinfo = PrintInfoDAL.GetPrint(prtSql);

                            if (pinfo.Print_ID > 0)
                            {
                                if (SerialNumber != "")
                                {
                                    string prSql = "Select top 1  " + pinfo.Print_Content + " from View_LEDShow_zj where 小票号='" + SerialNumber + "' and smallTicket_State='有效' order by  CarInfo_ID desc";
                                    DataSet ds = LinQBaseDao.Query(prSql);
                                    PrintInfoForm pi = new PrintInfoForm(ds);
                                    pi.Serialnumber = SerialNumber;
                                    pi.carinfoid = carInfo_ID.ToString();
                                    pi.cartypeid = comboxCarType.SelectedValue.ToString();
                                    pi.position = cmbposition.Text;
                                    pi.Show();
                                }
                            }
                            else
                            {
                                result = false;
                                MessageBox.Show("没有进行打印设置，请设置打印后，重新打印");
                                return;
                            }
                        }
                    }
                    #endregion

                    #region 车辆记录缓存到本地
                    PublicClass pc = new PublicClass();
                    ///有待优化 string  strsql = "select CarInOutRecord_ID,SmallTicket_Position_ID,SortNumberInfo_DrivewayValue,CarInfo_Name,SmallTicket_Serialnumber,SortNumberInfo_TongXing from View_CarState where CarInOutRecord_ID = " + CarInOutRecord_ID;
                    string strsql = "select CarInOutRecord_ID, SmallTicket_Position_ID, SortNumberInfo_DrivewayValue, CarInfo_Name, SmallTicket_Serialnumber,SortNumberInfo_TongXing from View_CarState where 1 = 1 and SortNumberInfo_TongXing != '已出厂' and datediff(d, CarInOutRecord_Time, getdate())= 0";
                    pc.GetCacheData(strsql); ///emewe 103 ,新增放行车辆记录缓存到本地
                    #endregion
                }
                #endregion
            }
            catch (Exception exc)
            {
                try
                {
                    isInsert = false;
                    if (CheckMethod.listMessage.Count > 0)
                    {
                        StringBuilder strMessage = new StringBuilder();
                        foreach (string item in CheckMethod.listMessage)
                        {
                            strMessage.Append(item.Trim());
                        }
                        MessageBox.Show(strMessage.ToString());
                        return;
                    }
                    MessageBox.Show("登记失败！");

                    string sqlstr = "";
                    for (int i = ErroSql.Count - 1; i >= 0; i--)
                    {
                        sqlstr += ErroSql[i] + "     ";
                    }
                    if (sqlstr != "")
                        LinQBaseDao.Query(sqlstr);
                    CommonalityEntity.WriteTextLog("CarInfoForm btnSave_Click()：" + exc.Message.ToString());

                }
                catch (Exception ex)
                {
                    CommonalityEntity.WriteTextLog("CarInfoForm btnSave_Click()：" + ex.Message.ToString());
                }
            }
            finally
            {
                if (isInsert)
                {
                    Chear();
                    this.Close();
                }
            }
        }
        /// <summary>
        /// 添加车辆信息 
        /// </summary>
        /// <param name="strid">车辆基础信息ID</param>
        /// <returns></returns>
        private int insertcar(string strid)
        {
            int str = 0;
            #region 车辆信息
            CarInfo carInfo = new CarInfo();

            //得到车辆类型编号
            carInfo.CarInfo_CarType_ID = int.Parse(comboxCarType.SelectedValue.ToString());
            //车牌号
            carInfo.CarInfo_Name = txtCarName.Text.ToString();

            carInfo.CarInfo_State = comboxCarState.Text.ToString();
            carInfo.CarInfo_Remark = txtRemark.Text;
            carInfo.CarInfo_Carriage = txtCarInfo_Carriage.Text;
            carInfo.CarInfo_Height = txtCarInfo_Height.Text;
            carInfo.CarInfo_Weight = txtCarInfo_Weight.Text;
            carInfo.CarInfo_Car_ID = Convert.ToInt32(strid);
            //公司编号
            if (!string.IsNullOrEmpty(CustomerInfo_ID))
            {
                carInfo.CarInfo_CustomerInfo_ID = int.Parse(CustomerInfo_ID);
            }
            //业务类型编号
            if (!string.IsNullOrEmpty(comboxBusinessType.Text.Trim()))
            {
                carInfo.CarInfo_BusinessType_ID = int.Parse(comboxBusinessType.SelectedValue.ToString());
            }
            carInfo.CarInfo_Type = txtCarInfo_Type.Text;
            carInfo.CarInfo_Operate = CommonalityEntity.USERNAME;
            carInfo.CarInfo_Time = CommonalityEntity.GetServersTime();
            carInfo.CarInfo_Bail = chkCarInfo_Bail.Checked.ToString();
            carInfo.CarInfo_LevelWaste = txtCustomerInfo_ADD.Text.Trim();
            carInfo.CarInfo_PO = txtNo.Text.Trim();
            string carSql = "";
            if (carInfo.CarInfo_CustomerInfo_ID <= 0 || carInfo.CarInfo_CustomerInfo_ID == null)
            {
                if (carInfo.CarInfo_BusinessType_ID > 0)
                {
                    carSql = "insert into CarInfo values(null," + carInfo.CarInfo_CarType_ID + "," + carInfo.CarInfo_BusinessType_ID + ",'" + carInfo.CarInfo_Name + "','" + carInfo.CarInfo_Type + "','" + carInfo.CarInfo_State + "','" + carInfo.CarInfo_Carriage + "','" + carInfo.CarInfo_Weight + "','" + carInfo.CarInfo_Height + "','" + carInfo.CarInfo_Bail + "','" + carInfo.CarInfo_PO + "','" + carInfo.CarInfo_LevelWaste + "','" + carInfo.CarInfo_Time + "','" + carInfo.CarInfo_Remark + "','" + carInfo.CarInfo_Operate + "'," + strid + ")";

                }
                else
                {
                    carSql = "insert into CarInfo values(null," + carInfo.CarInfo_CarType_ID + ",null,'" + carInfo.CarInfo_Name + "','" + carInfo.CarInfo_Type + "','" + carInfo.CarInfo_State + "','" + carInfo.CarInfo_Carriage + "','" + carInfo.CarInfo_Weight + "','" + carInfo.CarInfo_Height + "','" + carInfo.CarInfo_Bail + "','" + carInfo.CarInfo_PO + "','" + carInfo.CarInfo_LevelWaste + "','" + carInfo.CarInfo_Time + "','" + carInfo.CarInfo_Remark + "','" + carInfo.CarInfo_Operate + "'," + strid + ")";
                }
            }
            else
            {
                if (carInfo.CarInfo_BusinessType_ID > 0)
                {
                    carSql = "insert into CarInfo values(" + carInfo.CarInfo_CustomerInfo_ID + "," + carInfo.CarInfo_CarType_ID + "," + carInfo.CarInfo_BusinessType_ID + ",'" + carInfo.CarInfo_Name + "','" + carInfo.CarInfo_Type + "','" + carInfo.CarInfo_State + "','" + carInfo.CarInfo_Carriage + "','" + carInfo.CarInfo_Weight + "','" + carInfo.CarInfo_Height + "','" + carInfo.CarInfo_Bail + "','" + carInfo.CarInfo_PO + "','" + carInfo.CarInfo_LevelWaste + "','" + carInfo.CarInfo_Time + "','" + carInfo.CarInfo_Remark + "','" + carInfo.CarInfo_Operate + "'," + strid + ")";
                }
                else
                {
                    carSql = "insert into CarInfo values(" + carInfo.CarInfo_CustomerInfo_ID + "," + carInfo.CarInfo_CarType_ID + ",null,'" + carInfo.CarInfo_Name + "','" + carInfo.CarInfo_Type + "','" + carInfo.CarInfo_State + "','" + carInfo.CarInfo_Carriage + "','" + carInfo.CarInfo_Weight + "','" + carInfo.CarInfo_Height + "','" + carInfo.CarInfo_Bail + "','" + carInfo.CarInfo_PO + "','" + carInfo.CarInfo_LevelWaste + "','" + carInfo.CarInfo_Time + "','" + carInfo.CarInfo_Remark + "','" + carInfo.CarInfo_Operate + "'," + strid + ")";
                }
            }
            carSql = carSql + " select @@identity";
            str = int.Parse(LinQBaseDao.GetSingle(carSql).ToString());//得到当前的车辆编号
            #endregion
            return str;
        }

        private void upimage(object obj)
        {
            try
            {
                foreach (var item in CheckProperties.ce.Imagelist)
                {
                    //根据门岗获取当前门岗的拍照通道值
                    object objDriveway_Value = LinQBaseDao.GetSingle("Select Position_CameraValue from Position where Position_Id=" + SystemClass.PositionID + "");
                    if (objDriveway_Value == null)
                    {
                        return;
                    }
                    //得到图片的路径
                    string path = SystemClass.BaseFile + "Car" + SystemClass.PosistionValue + "\\";
                    string stryear = "Car" + SystemClass.PosistionValue + "\\" + CommonalityEntity.GetServersTime().Year + "\\" + CommonalityEntity.GetServersTime().ToString("yyyyMMdd") + "\\";
                    ImageFile.UpLoadFile(path + item, SystemClass.SaveFile + stryear);//上传图片到指定路径
                    //gbShowImage.Controls.Clear();
                    if (b != null)
                    {
                        b.Dispose();
                    }
                    ImageFile.Delete(path + item);//上传完成后，删除图片
                    //保存图片信息
                    string picSql = "Insert into CarPic(CarPic_CarInfo_ID,CarPic_State,CarPic_Add,CarPic_Type,CarPic_Time,CarPic_Match,CarPic_Remark) values(" + carInfo_ID + ",'启动','" + stryear + item + "','车辆登记照片',getdate(),'匹配','" + SystemClass.PositionName + "登记照片')";
                    picSql = picSql + " select @@identity";
                    carPIC_ID = int.Parse(LinQBaseDao.GetSingle(picSql).ToString());//得到当前的图片编号
                    ErroSql.Add("delete CarPic where CarPic_ID=" + carPIC_ID);//增删改出错后需要执行的SQL
                    isInsert = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// 验证该登记车辆是否为重复登记
        /// </summary>
        /// <param name="carName">车牌号</param>
        /// <returns>返回true(重复) or false(不重复)</returns>
        private bool ChkRepeat(string carName)
        {
            bool rbool = false;
            string sql = "select top 1 (CarInfo_ID) from View_CarState where CarInfo_name='" + carName + "' order by carInfo_time desc";
            object obj = LinQBaseDao.GetSingle(sql);
            if (obj != null)
            {
                string inOutSql = "Select * from CarInOutRecord where CarInOutRecord_CarInfo_ID=" + obj.ToString() + "";
                DataSet dataset = LinQBaseDao.Query(inOutSql);
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    DataSet ds = LinQBaseDao.Query("select top(1)* from View_CarState  where  CarInfo_ID =" + obj.ToString() + " and SortNumberInfo_TongXing='已注销' and SortNumberInfo_ISEffective=1 order by carInfo_time desc");
                    if (ds.Tables.Count > 0)
                    {
                        if (dataset.Tables[0].Rows[0]["CarInOutRecord_ISFulfill"].ToString() == "True")
                        {
                            rbool = false;
                        }
                    }
                    else
                    {
                        rbool = true;
                    }
                }
                else
                {
                    rbool = true;
                }
            }
            else
            {
                rbool = false;
            }
            return rbool;
        }

        /// <summary>
        /// 验证数据
        /// </summary>
        /// <returns></returns>
        private bool ChkIsNullOrEmpty()
        {
            result = true;
            if (string.IsNullOrEmpty(comboxCarType.Text.Trim()))
            {
                MessageBox.Show("车辆类型不能为空！");
                return result = false;

            }
            if (string.IsNullOrEmpty(txtCarName.Text.Trim()))
            {
                MessageBox.Show("车牌号不能为空！");
                return result = false;
            }
            if (comboxICNumber.Enabled && SerialNumber.Trim() == null)
            {
                if (string.IsNullOrEmpty(comboxICNumber.Text.Trim()))
                {
                    MessageBox.Show("IC卡编号不能为空！");
                    return result = false;
                }
            }
            if (string.IsNullOrEmpty(txtPhone.Text.Trim()))
            {
                MessageBox.Show("手机号码不能为空！");
                return result = false;
            }
            if (string.IsNullOrEmpty(txtCustomerInfo.Text.Trim()))
            {
                MessageBox.Show("公司名称不能为空！");
                return result = false;
            }
            if (string.IsNullOrEmpty(txtStaffInfo_Identity.Text.Trim()))
            {
                MessageBox.Show("身份证号码不能为空！");
                return result = false;
            }

            return result;
        }

        /// <summary>
        /// 优先校验 true优先，false排队
        /// </summary>
        private bool ChkIsYX(string staffid, string cum)
        {
            bool ispaidui = false;
            DataTable dt;
            #region 车辆优先校验
            if (CommonalityEntity.yxcar)
            {
                CommonalityEntity.yxcar = false;
                dt = LinQBaseDao.Query("SELECT * FROM CarPrecedence CP WHERE CP.CarPrecedence_Sate='启动' and CP.CarPrecedence_CarNO='" + txtCarName.Text.Trim() + "'").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ispaidui = ISHaveCarPrecedence(dt.Rows[i]);
                        if (ispaidui)
                        {
                            string ctid = dt.Rows[i]["CarPrecedence_CustomerInfo_ID"].ToString();
                            string snameid = dt.Rows[i]["CarPrecedence_StaffInfo_ID"].ToString();
                            if (ctid != "" || snameid != "")
                            {
                                if (ctid != "" && snameid != "" && snameid == staffid && ctid == CustomerInfo_ID)
                                {
                                    ispaidui = true;
                                }
                                else
                                {
                                    if (ctid != "" && ctid == CustomerInfo_ID && snameid == "")
                                    {
                                        ispaidui = true;
                                        break;
                                    }
                                    else
                                    {
                                        ispaidui = false;
                                    }
                                    if (snameid != "" && snameid == staffid && ctid == "")
                                    {
                                        ispaidui = true;
                                    }
                                    else
                                    {
                                        ispaidui = false;
                                    }
                                }
                            }
                            if (ispaidui)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            #endregion

            #region 驾驶员优先校验
            if (CommonalityEntity.yxstaffinfo)
            {
                CommonalityEntity.yxstaffinfo = false;
                dt = LinQBaseDao.Query("SELECT * FROM CarPrecedence CP WHERE CP.CarPrecedence_Sate='启动' and CP.CarPrecedence_StaffInfo_ID='" + staffid + "'").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ispaidui = ISHaveCarPrecedence(dt.Rows[i]);
                        if (ispaidui)
                        {
                            string ctid = dt.Rows[i]["CarPrecedence_CustomerInfo_ID"].ToString();
                            string scarn = dt.Rows[i]["CarPrecedence_CarNO"].ToString();
                            if (scarn != "" || ctid != "")
                            {
                                if (scarn != "" && ctid != "" && scarn == txtCarName.Text.Trim() && ctid == CustomerInfo_ID)
                                {
                                    ispaidui = true;
                                }
                                else
                                {
                                    if (scarn != "" && scarn == txtCarName.Text.Trim() && ctid == "")
                                    {
                                        ispaidui = true;
                                        break;
                                    }
                                    else
                                    {
                                        ispaidui = false;
                                    }
                                    if (ctid != "" && ctid == CustomerInfo_ID && scarn == "")
                                    {
                                        ispaidui = true;
                                    }
                                    else
                                    {
                                        ispaidui = false;
                                    }
                                }

                            }
                            if (ispaidui)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            #endregion

            # region 公司优先校验
            if (CommonalityEntity.yxcustomerinfo)
            {
                CommonalityEntity.yxcustomerinfo = false;
                dt = LinQBaseDao.Query("SELECT * FROM CarPrecedence CP WHERE CP.CarPrecedence_Sate='启动' and CP.CarPrecedence_CustomerInfo_ID='" + CustomerInfo_ID + "'").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ispaidui = ISHaveCarPrecedence(dt.Rows[i]);
                        if (ispaidui)
                        {
                            string scarn = dt.Rows[i]["CarPrecedence_CarNO"].ToString();
                            string snameid = dt.Rows[i]["CarPrecedence_StaffInfo_ID"].ToString();
                            if (scarn != "" || snameid != "")
                            {
                                if (scarn != "" && snameid != "" && scarn == txtCarName.Text.Trim() && snameid == staffid)
                                {
                                    ispaidui = true;
                                }
                                else
                                {
                                    if (scarn != "" && scarn == txtCarName.Text.Trim() && snameid == "")
                                    {
                                        ispaidui = true;
                                        break;
                                    }
                                    else
                                    {
                                        ispaidui = false;
                                    }
                                    if (snameid != "" && snameid == staffid && scarn == "")
                                    {
                                        ispaidui = true;
                                    }
                                    else
                                    {
                                        ispaidui = false;
                                    }
                                }
                            }
                            else
                            {
                                ispaidui = true;
                            }
                            if (ispaidui)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            #endregion
            return ispaidui;
        }
        /// <summary>
        /// 优先组合校验
        /// </summary>
        /// <param name="staffid"></param>
        /// <param name="dts"></param>
        /// <returns></returns>
        private bool IsPd(string staffid, DataTable dts)
        {
            bool ispd = false;
            #region 车辆公司驾驶员组合校验
            DataTable dt = LinQBaseDao.Query("select * from CarPrecedence where CarPrecedence_Sate='启动' and  CarPrecedence_CarNO='" + txtCarName.Text.Trim() + "'   and CarPrecedence_CustomerInfo_ID=" + CustomerInfo_ID + " and CarPrecedence_StaffInfo_ID=" + staffid).Tables[0];
            if (dt.Rows.Count > 0)
            {
                if (ISHaveCarPrecedence(dt.Rows[0]))
                {
                    ispd = true;
                    LinQBaseDao.Query("update CarPrecedence set CarPrecedence_TotalCountED+=1 where CarPrecedence_ID=" + dt.Rows[0][0].ToString());
                }
            }
            else
            {
                #region 车辆公司组合校验
                dt = LinQBaseDao.Query("select * from CarPrecedence where CarPrecedence_Sate='启动' and CarPrecedence_CarNO='" + txtCarName.Text.Trim() + "'    and CarPrecedence_CustomerInfo_ID=" + CustomerInfo_ID).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    if (ISHaveCarPrecedence(dt.Rows[0]))
                    {
                        ispd = true;
                        LinQBaseDao.Query("update CarPrecedence set CarPrecedence_TotalCountED+=1 where CarPrecedence_ID=" + dt.Rows[0][0].ToString());
                    }
                }
                else
                {
                    #region 车辆驾驶员组合校验
                    dt = LinQBaseDao.Query("select * from CarPrecedence where CarPrecedence_Sate='启动' and CarPrecedence_CarNO='" + txtCarName.Text.Trim() + "'     and CarPrecedence_StaffInfo_ID=" + staffid).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        if (ISHaveCarPrecedence(dt.Rows[0]))
                        {
                            ispd = true;
                            LinQBaseDao.Query("update CarPrecedence set CarPrecedence_TotalCountED+=1 where CarPrecedence_ID=" + dt.Rows[0][0].ToString());
                        }
                    }
                    else
                    {
                        #region 公司驾驶组合校验
                        dt = LinQBaseDao.Query("select * from CarPrecedence where CarPrecedence_Sate='启动'   and CarPrecedence_CustomerInfo_ID=" + CustomerInfo_ID + " and CarPrecedence_StaffInfo_ID=" + staffid).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            if (ISHaveCarPrecedence(dt.Rows[0]))
                            {
                                ispd = true;
                                LinQBaseDao.Query("update CarPrecedence set CarPrecedence_TotalCountED+=1 where CarPrecedence_ID=" + dt.Rows[0][0].ToString());
                            }
                        }
                        else
                        {
                            ispd = true;
                            LinQBaseDao.Query("update CarPrecedence set CarPrecedence_TotalCountED+=1 where CarPrecedence_ID=" + dts.Rows[0][0].ToString());
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            #endregion
            return ispd;
        }

        /// <summary>
        /// 跳转到人员信息页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStaff_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 输入IC卡信息时，验证IC卡信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboxICNumber_Leave(object sender, EventArgs e)
        {
            try
            {

                if (comboxICNumber.Text.Trim() != "")
                {

                    string sql = "select * from ICCard where ICCard_Value='" + comboxICNumber.Text.ToString() + "'";
                    if (ICCardDAL.GetViewICCardName(sql).Count() <= 0)
                    {
                        DialogResult digresult = MessageBox.Show("不存在该IC卡信息，是否进行登记？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (DialogResult.OK == digresult)
                        {
                            comboxICNumber.Focus();
                            ICCardForm icf = new ICCardForm();
                            icf.Show();
                        }
                        else
                        {
                            comboxICNumber.Text = "";
                            return;
                        }
                    }
                    else
                    {
                        sql = "select * from ICCard where ICCard_Value='" + comboxICNumber.Text.ToString() + "' and ICCard_State='启动'";
                        if (ICCardDAL.GetViewICCardName(sql).Count() <= 0)
                        {
                            MessageBox.Show("该IC卡状态未启动!");
                            return;
                        }
                        else
                        {
                            iCNumberID = ICCardDAL.GetViewICCardName(sql).FirstOrDefault().ICCard_ID;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// 清空文本框内的数据
        /// </summary>
        private void Chear()
        {
            //清空文本框信息

            txtCarName.Text = "";
            txtStaffName.Text = "";
            txtCustomerInfo.Text = "";
            txtPhone.Text = "";
            txtCarInfo_Weight.Text = "";
            txtCarInfo_Height.Text = "";
            txtStaffInfo_Identity.Text = "";
            txtCustomerInfo_ADD.SelectedIndex = -1;
            comboxBusinessType.SelectedIndex = -1;
            comboxCarState.SelectedIndex = 0;
            comboxCarType.SelectedIndex = 0;
            // txtNo.Text = "";
            txtCarInfo_Carriage.Text = "";
            txtCarInfo_Height.Text = "";
            txtCarInfo_Type.Text = "";
            txtCarInfo_Weight.Text = "";
            SerialNumber = "";
            comboxICNumber.Text = "";
            staffInfo_Id = "";
            result = false;
            CustomerInfo_ID = "";
            CheckProperties checkPrC = new CheckProperties();
            CommonalityEntity.strCardNo = "";

        }
        /// <summary>
        /// SAP(获取数据绑定到页面)
        /// </summary>
        public void SAPChk()
        {
            try
            {


                string sql = "Select * from eh_Saprecord where SAP_ID =" + Sapid;
                List<eh_SAPRecord> list = LinQBaseDao.GetItemsForListing<eh_SAPRecord>(sql).ToList();
                if (list != null)
                {
                    eh_SAPRecord item = list[0];
                    string Sap_Type = "";
                    if (item.Sap_Type != "" || item.Sap_Type != null)
                    {
                        Sap_Type = item.Sap_Type;//车辆类型
                    }

                    if (!string.IsNullOrEmpty(item.Sap_InNO))
                    {
                        txtNo.Text = item.Sap_InNO;//送货单号
                    }
                    DataTable dt;
                    string strsqltype = "";
                    comboxCarType.DataSource = null;
                    if (Sap_Type == "送货车辆")
                    {
                        if (txtNo.Text.Substring(0, 2) == "43")
                        {
                            strsqltype = "select CarType_ID,CarType_Name from CarType where CarType_State='启动' and  CarType_Property='" + Sap_Type + "' and CarType_Name  like '%废纸%' ";
                        }
                        else
                        {
                            strsqltype = "select CarType_ID,CarType_Name from CarType where CarType_State='启动' and  CarType_Property='" + Sap_Type + "' and CarType_Name not  like '%废纸%' ";

                        }
                    }
                    else
                    {
                        strsqltype = "select CarType_ID,CarType_Name from CarType where CarType_State='启动' and  CarType_Property='" + Sap_Type + "'";
                    }
                    dt = LinQBaseDao.Query(strsqltype).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        comboxCarType.DataSource = dt;
                        comboxCarType.DisplayMember = "CarType_Name";
                        comboxCarType.ValueMember = "CarType_ID";
                        comboxCarType.SelectedIndex = 0;
                    }
                    if (Sap_Type == "成品车辆")
                    {
                        if (!string.IsNullOrEmpty(item.Sap_Prodh))
                        {
                            string proth = item.Sap_Prodh.Substring(0, 2);
                            strsqltype = "select CarType_ID,CarType_Name from CarType where CarType_State='启动' and  CarType_Property='" + Sap_Type + "' and CarType_Name like '%" + proth + "%'";
                            dt = LinQBaseDao.Query(strsqltype).Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                comboxCarType.DataSource = dt;
                                comboxCarType.DisplayMember = "CarType_Name";
                                comboxCarType.ValueMember = "CarType_ID";
                                comboxCarType.SelectedIndex = 0;
                            }
                        }
                    }
                    if (item.Sap_InCarNumber != null || item.Sap_InCarNumber != "")
                    {
                        txtCarName.Text = item.Sap_InCarNumber;//车牌号
                    }
                    if (item.Sap_OutNAME1C != null || item.Sap_OutNAME1C != "")
                    {
                        txtCustomerInfo.Text = item.Sap_OutNAME1C;//客户
                    }
                    if (item.Sap_OutNAME1C == null || item.Sap_OutNAME1C == "")
                    {
                        if (item.Sap_OutNAME1P != "" || item.Sap_OutNAME1P != null)
                        {
                            txtCustomerInfo.Text = item.Sap_OutNAME1P;//供应商
                        }
                    }

                    if (txtCustomerInfo.Text.Trim() != "")
                    {
                        DataTable CustomerInfo_IDDT = LinQBaseDao.Query("select CustomerInfo_ID from CustomerInfo where CustomerInfo_Name='" + txtCustomerInfo.Text.Trim() + "'").Tables[0];
                        //如果数据库有就赋值没有就添加
                        if (CustomerInfo_IDDT.Rows.Count > 0)
                        {
                            CustomerInfo_ID = CustomerInfo_IDDT.Rows[0][0].ToString();
                        }
                        else
                        {
                            string CustomerInfo_ID = LinQBaseDao.GetSingle("insert CustomerInfo(CustomerInfo_Name,CustomerInfo_State,CustomerInfo_Time) values('" + txtCustomerInfo.Text.Trim() + "','启动',GETDATE())  select @@identity").ToString();

                        }
                    }
                    if (item.Sap_OutMAKTX != null || item.Sap_OutMAKTX != "")
                    {
                        txtCarInfo_Carriage.Text = item.Sap_OutMAKTX;//物料描述
                    }
                    if (item.Sap_OutOFLAG != null || item.Sap_OutOFLAG != "")
                    {
                        if (item.Sap_OutOFLAG == "X")
                        {
                            chkCarInfo_Bail.Checked = true;
                        }
                        else
                        {
                            chkCarInfo_Bail.Checked = false;
                        }
                    }
                    if (item.Sap_OutHG != null || item.Sap_OutHG != "")
                    {
                        txtCarInfo_Height.Text = item.Sap_OutHG;
                    }
                    if (item.Sap_OutXZ != null || item.Sap_OutXZ != "")
                    {
                        txtCarInfo_Weight.Text = item.Sap_OutXZ;
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 点击显示下拉列表框时绑定数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboxCustomerInfo_DropDown(object sender, EventArgs e)
        {
            IsCus = true;
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
                common.WriteTextLog("CarInformationForm.pbInImageOne_MouseHover()");
            }
        }
        public Bitmap b = null;
        public void ShowD(string FileName)
        {
            b = new Bitmap(FileName);
            pictureBox1.Image = b;
            pictureBox1.Visible = true;

        }
        /// <summary>
        /// 改变图片大小(放大)
        /// </summary>
        /// <param name="pb"></param>
        public void ShowD()
        {
            try
            {

                b.Dispose();
                pictureBox1.Visible = false;

            }
            catch
            {
                // common.WriteTextLog("ShowD()");
            }
        }
        /// <summary>
        /// 私有字段驾驶员SName
        /// </summary>
        public string SName = "";

        /// <summary>
        /// 私有字段公司名GName
        /// </summary>
        public string GName = "";

        #endregion

        /// <summary>
        /// 窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            CommonalityEntity.UpdateCar = false;
            CommonalityEntity.SAP_ID = "";
            CommonalityEntity.ISsap = false;
            gbShowImage.Dispose();//释放图片
        }

        //2012-12-3  
        private void BindStandard()
        {
            try
            {
                string sql = "select Check_ID,Check_Name from fh_CheckStandard where check_otherid=0";

                txtCustomerInfo_ADD.DataSource = LinQBaseDao.Querys(sql).Tables[0];
                txtCustomerInfo_ADD.ValueMember = "Check_ID";
                txtCustomerInfo_ADD.DisplayMember = "Check_Name";
                txtCustomerInfo_ADD.SelectedIndex = -1;
            }
            catch
            {
                CommonalityEntity.WriteTextLog("获取国废等级错误，请查看国废数据库连接字符EMEWEQCConnectionString2是否错误");
            }
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (lblCarName.ForeColor != Color.Red)
                {
                    lblCarName.ForeColor = Color.Red;
                    label8.ForeColor = Color.Red;
                    label21.ForeColor = Color.Red;
                    label6.ForeColor = Color.Red;
                    label5.ForeColor = Color.Red;
                    label20.ForeColor = Color.Red;
                    label12.ForeColor = Color.Red;
                }
                showImages();


            }
            catch
            {

            }
        }
        int i = 0;


        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
        }

        /// <summary>
        /// 删除照片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClick_Click(object sender, EventArgs e)
        {
            try
            {
                string path = (sender as Button).Tag.ToString();
                ImageFile.Delete(path);
                this.gbShowImage.Controls.Clear();
            }
            catch (Exception)
            {

            }
        }
        private void btnClick_Click(object path)
        {
            try
            {
                ImageFile.Delete(path.ToString());
            }
            catch (Exception)
            {

            }
        }

        private void chkNumber_CheckedChanged(object sender, EventArgs e)
        {
            chkNumbertrue();
        }

        private void chkNumbertrue()
        {
            try
            {


                if (chkNumber.Checked)
                {
                    CheckMethod.GetSerialNumber1();
                    CheckMethod.GetSerialNumber2();
                    if (CheckProperties.ce.serCount != "")
                    {
                        #region 修改凭证有效性 2012-07-16
                        if (CheckProperties.ce.ISUpdateCredentials)
                        {
                            txtSerCount.Enabled = true;
                        }
                        #endregion

                        txtSerCount.Text = CheckProperties.ce.serCount;
                    }
                    else
                    {
                        txtSerCount.Text = "";
                    }
                    if (CheckProperties.ce.serTime != "")
                    {
                        #region 修改凭证有效性
                        if (CheckProperties.ce.ISUpdateCredentials)
                        {
                            txtSerTime.Enabled = true;
                        }
                        #endregion

                        txtSerTime.Text = CheckProperties.ce.serTime;
                    }
                    else
                    {
                        txtSerTime.Text = "";
                    }
                    string sql = "select * from ManagementStrategy where ManagementStrategy_State='启动' and ManagementStrategy_DriSName in(select CarType_DriSName from CarType where CarType_Name='" + comboxCarType.Text + "') and ManagementStrategy_Rule='ChkUpdateCredentials'";
                    if (LinQBaseDao.Query(sql).Tables[0].Rows.Count > 0)
                    {
                        txtCarCount.Enabled = ControlAttributes.BoolControl("txtCarCount", "CarInfoForm", "Enabled");
                        txtCarTime.Enabled = ControlAttributes.BoolControl("txtCarCount", "CarInfoForm", "Enabled");
                        txtSerCount.Enabled = ControlAttributes.BoolControl("txtCarCount", "CarInfoForm", "Enabled");
                        txtSerTime.Enabled = ControlAttributes.BoolControl("txtCarCount", "CarInfoForm", "Enabled");
                    }
                    else
                    {
                        txtCarCount.Enabled = false;
                        txtCarTime.Enabled = false;
                        txtSerCount.Enabled = false;
                        txtSerTime.Enabled = false;
                    }
                }
                else
                {
                    txtSerCount.Text = "";
                    txtSerTime.Text = "";
                }
            }
            catch (Exception)
            {

            }
        }



        /// <summary>
        /// 验证IC卡是否有效
        /// </summary>
        private void ICCardIsValid(string cardNum)
        {
            try
            {


                DataTable carDT = LinQBaseDao.Query("select ICCard_ID, ICCard_EffectiveType, ICCard_BeginTime,ICCard_EndTime,ICCard_count,ICCard_HasCount ,ICCard_State,Car_Name,Car_State  from View_Car_ICard_CarType   where ICCard_Value='" + cardNum + "' ").Tables[0];
                DataTable StaffInfoDT = LinQBaseDao.Query("select StaffInfo_ID,StaffInfo_Name, StaffInfo_Identity,StaffInfo_Phone, ICCard_ID, ICCard_EffectiveType, ICCard_BeginTime,ICCard_EndTime,ICCard_count,ICCard_HasCount,ICCard_State,StaffInfo_State   from View_StaffInfo_ICCard where  ICCard_Value='" + cardNum + "'").Tables[0];

                #region 车卡
                //如果车辆基础信息不为空
                if (carDT.Rows.Count > 0)
                {
                    string Car_Name = carDT.Rows[0]["Car_Name"].ToString();
                    string ICCard_EffectiveType = carDT.Rows[0]["ICCard_EffectiveType"].ToString();

                    string ICCard_count = carDT.Rows[0]["ICCard_count"].ToString();
                    string ICCard_HasCount = carDT.Rows[0]["ICCard_HasCount"].ToString();
                    string ICCard_State = carDT.Rows[0]["ICCard_State"].ToString();
                    string Car_State = carDT.Rows[0]["Car_State"].ToString();

                    if (ICCard_State != "启动")
                    {
                        txtCarTime.Text = "";
                        txtCarCount.Text = "";
                        comboxICNumber.Text = "";

                        MessageBox.Show("IC卡状态为：" + ICCard_State, "提示");
                        return;
                    }
                    if (Car_State != "启动")
                    {
                        txtCarTime.Text = "";
                        txtCarCount.Text = "";
                        comboxICNumber.Text = "";

                        MessageBox.Show("车辆状态为：" + Car_State, "提示");
                        return;
                    }
                    if (ICCard_EffectiveType == "次数")
                    {
                        if (string.IsNullOrEmpty(ICCard_HasCount))
                        {
                            if (ICCard_count != "0" && !string.IsNullOrEmpty(ICCard_count))
                            {
                                txtCarCount.Text = ICCard_count;
                            }
                            else
                            {
                                txtCarTime.Text = "";
                                txtCarCount.Text = "";
                                comboxICNumber.Text = "";

                                MessageBox.Show("车卡已过期", "提示");
                                return;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(ICCard_count))
                            {
                                int ct = Convert.ToInt32(ICCard_count);
                                int ht = Convert.ToInt32(ICCard_HasCount);
                                if (ct - ht > 0)
                                {
                                    txtCarCount.Text = ICCard_count;
                                }
                                else
                                {
                                    txtCarTime.Text = "";
                                    txtCarCount.Text = "";
                                    comboxICNumber.Text = "";

                                    MessageBox.Show("车卡已过期", "提示");
                                    return;
                                }
                            }
                            else
                            {
                                txtCarTime.Text = "";
                                txtCarCount.Text = "";
                                comboxICNumber.Text = "";

                                MessageBox.Show("车卡已过期", "提示");
                                return;
                            }
                        }
                    }
                    if (ICCard_EffectiveType == "有效期")
                    {
                        DateTime ICCard_BeginTime = Convert.ToDateTime(carDT.Rows[0]["ICCard_BeginTime"].ToString());
                        DateTime ICCard_EndTime = Convert.ToDateTime(carDT.Rows[0]["ICCard_EndTime"].ToString());
                        if (CommonalityEntity.GetServersTime() > ICCard_BeginTime && CommonalityEntity.GetServersTime() < ICCard_EndTime)
                        {
                            TimeSpan th = ICCard_EndTime - CommonalityEntity.GetServersTime();
                            int s = Convert.ToInt32(th.TotalHours);
                            txtCarTime.Text = s.ToString();
                        }
                        else
                        {
                            txtCarTime.Text = "";
                            txtCarCount.Text = "";
                            comboxICNumber.Text = "";

                            MessageBox.Show("车卡已过期", "提示");
                            return;
                        }
                    }
                    if (ICCard_EffectiveType == "永久")
                    {
                        txtCarTime.Text = "1000";
                        txtCarCount.Text = "1";
                    }
                    DataTable dtcust = LinQBaseDao.Query("select CustomerInfo_Name from CustomerInfo where CustomerInfo_ID in ( select top(1)  CarInfo_CustomerInfo_ID from CarInfo where CarInfo_Name='" + Car_Name + "'  order by CarInfo_ID desc )").Tables[0];
                    if (dtcust.Rows.Count > 0)
                    {
                        txtCustomerInfo.Text = dtcust.Rows[0][0].ToString();
                    }
                    txtCarName.Text = Car_Name;
                    txtCarName.Enabled = false;
                    txtStaffName.Enabled = true;
                }
                #endregion

                #region 人卡
                //如果驾驶员信息不为空
                if (StaffInfoDT.Rows.Count > 0)
                {
                    string StaffInfo_ID = StaffInfoDT.Rows[0]["StaffInfo_ID"].ToString();
                    string StaffInfo_Name = StaffInfoDT.Rows[0]["StaffInfo_Name"].ToString();
                    string StaffInfo_Identity = StaffInfoDT.Rows[0]["StaffInfo_Identity"].ToString();
                    string StaffInfo_Phone = StaffInfoDT.Rows[0]["StaffInfo_Phone"].ToString();

                    string ICCard_EffectiveType = StaffInfoDT.Rows[0]["ICCard_EffectiveType"].ToString();
                    string ICCard_count = StaffInfoDT.Rows[0]["ICCard_count"].ToString();
                    string ICCard_HasCount = StaffInfoDT.Rows[0]["ICCard_HasCount"].ToString();
                    string ICCard_State = StaffInfoDT.Rows[0]["ICCard_State"].ToString();
                    string StaffInfo_State = StaffInfoDT.Rows[0]["StaffInfo_State"].ToString();

                    if (ICCard_State != "启动")
                    {
                        txtCarTime.Text = "";
                        txtCarCount.Text = "";
                        comboxICNumber.Text = "";

                        MessageBox.Show("IC卡状态为：" + ICCard_State, "提示");
                        return;
                    }
                    if (StaffInfo_State != "启动")
                    {
                        txtCarTime.Text = "";
                        txtCarCount.Text = "";
                        comboxICNumber.Text = "";

                        MessageBox.Show("驾驶员状态为：" + StaffInfo_State, "提示");
                        return;
                    }

                    if (ICCard_EffectiveType == "次数")
                    {
                        if (string.IsNullOrEmpty(ICCard_HasCount))
                        {
                            if (ICCard_count != "0" && !string.IsNullOrEmpty(ICCard_count))
                            {
                                txtCarCount.Text = ICCard_count;
                            }
                            else
                            {
                                txtCarTime.Text = "";
                                txtCarCount.Text = "";
                                comboxICNumber.Text = "";

                                MessageBox.Show("人卡已过期", "提示");
                                return;
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(ICCard_count))
                            {
                                int ct = Convert.ToInt32(ICCard_count);
                                int ht = Convert.ToInt32(ICCard_HasCount);
                                if (ct - ht > 0)
                                {
                                    txtCarCount.Text = ICCard_count;
                                }
                                else
                                {
                                    txtCarTime.Text = "";
                                    txtCarCount.Text = "";
                                    comboxICNumber.Text = "";

                                    MessageBox.Show("人卡已过期", "提示");
                                    return;
                                }
                            }
                            else
                            {
                                txtCarTime.Text = "";
                                txtCarCount.Text = "";
                                comboxICNumber.Text = "";

                                MessageBox.Show("人卡已过期", "提示");
                                return;
                            }
                        }
                    }
                    if (ICCard_EffectiveType == "有效期")
                    {
                        DateTime ICCard_BeginTime = Convert.ToDateTime(StaffInfoDT.Rows[0]["ICCard_BeginTime"].ToString());
                        DateTime ICCard_EndTime = Convert.ToDateTime(StaffInfoDT.Rows[0]["ICCard_EndTime"].ToString());
                        if (CommonalityEntity.GetServersTime() > ICCard_BeginTime && CommonalityEntity.GetServersTime() < ICCard_EndTime)
                        {
                            TimeSpan th = ICCard_EndTime - CommonalityEntity.GetServersTime();
                            int s = Convert.ToInt32(th.TotalHours);
                            txtCarTime.Text = s.ToString();
                        }
                        else
                        {
                            txtCarTime.Text = "";
                            txtCarCount.Text = "";
                            comboxICNumber.Text = "";

                            MessageBox.Show("人卡已过期", "提示");
                            return;
                        }
                    }
                    if (ICCard_EffectiveType == "永久")
                    {
                        txtCarTime.Text = "1000";
                        txtCarCount.Text = "1";
                    }
                    txtStaffInfo_Identity.Text = StaffInfo_Identity;
                    txtPhone.Text = StaffInfo_Phone;
                    txtStaffName.Text = StaffInfo_Name;
                    txtStaffName.Enabled = false;
                    txtCarName.Enabled = true;
                }
                #endregion
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoForm.ICCardIsValid()异常：");
            }
        }


        /// <summary>
        /// 登记拍照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPIC_Click(object sender, EventArgs e)
        {

            if (CutPic.ct == null)
            {
                CutPic.ct = new CutPic();
                CutPic.ct.CapturePic(SystemClass.lChannel);
            }
            else
            {
                CutPic.ct.CapturePic(SystemClass.lChannel);
            }
        }

        private void comboxCarType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

             //string   positionstr = "";

                CommonalityEntity.IsUpdatedri = false;
                int s = Convert.ToInt32(comboxCarType.SelectedIndex.ToString());
                if (s >= 0)//修改车辆信息
                {
                    if (comboxCarType.SelectedValue.ToString() != "System.Data.DataRowView")
                    {
                        string busSql = "Select * from BusinessType where BusinessType_CarType_ID =" + comboxCarType.SelectedValue.ToString();
                        comboxBusinessType.DataSource = LinQBaseDao.GetItemsForListing<BusinessType>(busSql);
                        comboxBusinessType.ValueMember = "BusinessType_ID";
                        comboxBusinessType.DisplayMember = "BusinessType_Name";
                    }
                }
                if (s >= 0)
                {
                    if (comboxCarType.SelectedValue.ToString() != "System.Data.DataRowView")
                    {
                        DataTable dt = LinQBaseDao.Query(" select * from ManagementStrategy where ManagementStrategy_DriSName in(select CarType_DriSName from CarType where CarType_ID=" + comboxCarType.SelectedValue.ToString() + ") and ManagementStrategy_Rule='ChkLevelWaste' and ManagementStrategy_State='启动'").Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            label12.Visible = true;
                            txtCustomerInfo_ADD.Visible = true;
                            BindStandard();
                        }
                        else
                        {
                            label12.Visible = false;
                            txtCustomerInfo_ADD.Visible = false;
                            txtCustomerInfo_ADD.DataSource = null;
                        }
                        CheckProperties.ce.carType_ID = Convert.ToInt32(comboxCarType.SelectedValue.ToString());
                        chkNumbertrue();
                    }
                }

                //emewe 103 20181024  绑定该车辆业务类型可通行的默认门岗
                string str4 = " select substring(D.DrivewayStrategy_Reason, 2,4) as position from CarType as C , DrivewayStrategy as D  where C.CarType_DriSName = D.DrivewayStrategy_Name and C.CarType_Name = '" + comboxCarType.Text + "' group by D.DrivewayStrategy_Reason";
                DataTable dt4 = LinQBaseDao.Query(str4).Tables[0];

                if (dt4.Rows.Count > 0)
                {                   
                    cmbposition.DataSource = dt4;
                    cmbposition.ValueMember = "position";
                    cmbposition.DisplayMember = "position";

                    ///emewe 103 20181025 新增默认通行门岗赋值
                    for (int i = 0; i < dt4.Rows.Count; i++)
                    {

                        string sql = "select Position_Name,Position_Value from Position where Position_Name ='" + dt4.Rows[i][0].ToString() + "'";
                        DataTable dt3 = LinQBaseDao.Query(sql).Tables[0];

                        if (dt3.Rows.Count > 0)
                        {
                            positionstr += dt3.Rows[0]["Position_Value"].ToString() + ",";
                        }
                    }
                    return;
                }
            }
            catch (Exception err)
            {

            }
        }

        private void btnEmpty_Click(object sender, EventArgs e)
        {
            Chear();
        }

        private void comboxICNumber_KeyUp(object sender, KeyEventArgs e)
        {
            string iccard_id = comboxICNumber.Text.Trim();
            if (iccard_id.Length == 10)
            {
                try
                {
                    iccard_id = "0" + Convert.ToInt64(iccard_id).ToString("X");
                    comboxICNumber.Text = iccard_id;
                }
                catch
                {
                    comboxICNumber.Text = "";
                }

            }
        }
        /// <summary>
        /// 获取驾驶员姓名和身份证号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCarType_Click(object sender, EventArgs e)
        {
            try
            {
                string str = DBHelperAccess.GetFile();
                if (!string.IsNullOrEmpty(str))
                {
                    MessageBox.Show(this, str);
                    return;
                }
                DataTable dt = DBHelperAccess.Query("select * from iDRTable where [追加地址4] is null order by 编号 desc").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    txtStaffName.Text = dt.Rows[0]["姓名"].ToString();
                    txtStaffInfo_Identity.Text = dt.Rows[0]["公民身份号码"].ToString();
                    DBHelperAccess.Query("update iDRTable set [追加地址4]='1' where [追加地址4] is null");
                    DataTable dtst = LinQBaseDao.Query("select top(1) CarInfo_Name,CustomerInfo_Name,StaffInfo_Phone from View_CarState where staffinfo_Name='" + dt.Rows[0]["姓名"].ToString().Trim() + "' and StaffInfo_Identity='" + dt.Rows[0]["公民身份号码"].ToString().Trim() + "' order by carinfo_id desc ").Tables[0];
                    if (dtst.Rows.Count > 0)
                    {
                        if (txtCarName.Text.Trim() == "")
                        {
                            txtCarName.Text = dtst.Rows[0]["CarInfo_Name"].ToString();
                        }
                        if (txtCustomerInfo.Text.Trim() == "")
                        {
                            txtCustomerInfo.Text = dtst.Rows[0]["CustomerInfo_Name"].ToString();
                        }
                        if (txtPhone.Text.Trim() == "")
                        {
                            txtPhone.Text = dtst.Rows[0]["StaffInfo_Phone"].ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
        private void comboxICNumber_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(comboxICNumber.Text) && comboxICNumber.Text.Length == 9)
                {
                    if (!chkCarInfo_Bail.Checked)
                    {

                        ICCardIsValid(comboxICNumber.Text);

                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void btndelPic1_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
            btnClick_Click(pictureBox2.Tag);
        }

        private void btndelPic2_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
            btnClick_Click(pictureBox3.Tag);
        }

        private void btndelPic3_Click(object sender, EventArgs e)
        {
            groupBox4.Visible = false;
            btnClick_Click(pictureBox4.Tag);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox5.Visible = false;
            btnClick_Click(pictureBox5.Tag);
        }

        private void CarInfoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CommonalityEntity.UpdateCar = false;
            CommonalityEntity.SAP_ID = "";
            CommonalityEntity.ISsap = false;
            gbShowImage.Dispose();//释放图片
            GC.Collect();
        }

        private void cmbposition_MouseClick(object sender, MouseEventArgs e)
        {
            
                //emewe 103 20181016  绑定该车辆业务类型可通行门岗
                string sqlstr = " select A.position from(select substring(D.DrivewayStrategy_Record, 0,len(D.DrivewayStrategy_Record) - charindex('门岗', D.DrivewayStrategy_Record) - 1) as position  from CarType as C , DrivewayStrategy as D  where C.CarType_DriSName = D.DrivewayStrategy_Name and C.CarType_Name = '" + comboxCarType.Text + "') as A group by A.position";

                DataTable dt2 = LinQBaseDao.Query(sqlstr).Tables[0];
                cmbposition.DataSource = dt2;
                cmbposition.ValueMember = "position";
                cmbposition.DisplayMember = "A.position";

                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {

                        string sql = "select Position_Name,Position_Value from Position where Position_Name ='" + dt2.Rows[i][0].ToString() + "'";
                        DataTable dt3 = LinQBaseDao.Query(sql).Tables[0];

                        if (dt3.Rows.Count > 0)
                        {
                            positionstr += dt3.Rows[0]["Position_Value"].ToString() + ",";
                        }
                    }

                }
        }
    }
}
