using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.HelpClass;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.DAL;
using System.Text.RegularExpressions;
using System.Threading;
using EMEWE.CarManagement.CommonClass;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class CarInOutForm : Form
    {
        public CarInOutForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 车牌号
        /// </summary>
        public string carIds = "";
        public static CarInOutForm cfin = null;

        //public MainForm mf = new MainForm();
        /// <summary>
        /// 通道值，默认为1通道
        /// </summary>
        public string PositionValue = "01";
        /// <summary>
        /// 当前通行车辆
        /// </summary>
        public string InCarId = null;
        /// <summary>
        /// IC卡类型
        /// </summary>
        public string iccardType = null;
        /// <summary>
        /// IC卡ID
        /// </summary>
        private string iccardid = null;

        /// <summary>
        /// key通道值 value通道名称
        /// </summary>
        Dictionary<string, string> dicValues = new Dictionary<string, string>();
        /// <summary>
        /// 定时器取数据的集合
        /// </summary>
        List<DeviceControl> lists = new List<DeviceControl>();

        /// 存放车牌号
        /// </summary>
        List<CarInfo> listCarNo = new List<CarInfo>();
        /// <summary>
        /// 是否异常放行
        /// </summary>
        private bool ischeckinout = false;
        /// <summary>
        /// 车牌号
        /// </summary>
        private string carName = "";
        /// <summary>
        /// 车辆类型ID
        /// </summary>
        private string cartypeid = "0";
        /// <summary>
        /// 车辆ID
        /// </summary>
        private string carinid = "0";
        /// <summary>
        /// 放行页面：0,CarInformationFormOne;1,CarInformationFormTwo;2,CarInformationFormThree;
        /// </summary>
        private int intfx = 0;

        /// <summary>
        /// 通行策略名称
        /// </summary>
        private string Strategy_DriSName = "";

        /// <summary>
        /// 清空按钮双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Clear_Click(object sender, EventArgs e)
        {
            try
            {
                dicicvalues.Clear();
                txt_Serialnumber.Text = "";
                if (MainForm.dicCard.Count > 0)
                {
                    MainForm.dicCard.Clear();
                }
                txtCar1.Text = "";
                txtCar2.Text = "";
                txtCar3.Text = "";
                txtCar4.Text = "";
                txtCar5.Text = "";
                txtCar6.Text = "";
                txtCar7.Text = "";
                txtCar8.Text = "";
                txtUser1.Text = "";
                txtUser2.Text = "";
                txtUser3.Text = "";
                txtUser4.Text = "";
                txtUser5.Text = "";
                txtUser6.Text = "";
                txtUser7.Text = "";
                txtUser8.Text = "";
                cmbCarNum1.DataSource = null;
                cmbCarNum2.DataSource = null;
                cmbCarNum3.DataSource = null;
                cmbCarNum4.DataSource = null;
                cmbCarNum5.DataSource = null;
                cmbCarNum6.DataSource = null;
                cmbCarNum7.DataSource = null;
                cmbCarNum8.DataSource = null;
                lbPrompt1.Text = "";
                lbPrompt2.Text = "";
                lbPrompt3.Text = "";
                lbPrompt4.Text = "";
                lbPrompt5.Text = "";
                lbPrompt6.Text = "";
                lbPrompt7.Text = "";
                lbPrompt8.Text = "";
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInOutForm.btn_Clear_Click()");
            }
            finally
            {
                btn_Clear.Enabled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CarInOutForm_Load(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
            groupBox3.Visible = false;
            groupBox4.Visible = false;
            groupBox5.Visible = false;
            groupBox6.Visible = false;
            groupBox7.Visible = false;
            groupBox8.Visible = false;
            groupBox9.Visible = false;

            userContext();
            CreateControl();
            BindDriveway();
        }
        private void BindDriveway()
        {
            DataTable dt = LinQBaseDao.Query("  select Driveway_Name,Driveway_ID from Driveway where Driveway_Position_ID=" + SystemClass.PositionID + "  and Driveway_State='启动'").Tables[0];
            if (dt.Rows.Count > 0)
            {
                cmbDiv.DataSource = dt;
                cmbDiv.DisplayMember = "Driveway_Name";
                cmbDiv.ValueMember = "Driveway_ID";
                cmbDiv.SelectedIndex = 0;
            }

        }

        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {

                btn_check.Enabled = true;
                btn_check.Visible = true;
                btn_outSumbit.Enabled = true;
                btn_outSumbit.Visible = true;
                btnInout1.Enabled = true;
                btnInout1.Visible = true;
                btnInout2.Enabled = true;
                btnInout2.Visible = true;
                btnInout3.Enabled = true;
                btnInout3.Visible = true;
                btnInout4.Enabled = true;
                btnInout4.Visible = true;
                btnInout5.Enabled = true;
                btnInout5.Visible = true;
                btnInout6.Enabled = true;
                btnInout6.Visible = true;
                btnInout7.Enabled = true;
                btnInout7.Visible = true;
                btnInout8.Enabled = true;
                btnInout8.Visible = true;

                btnWarn1.Enabled = true;
                btnWarn1.Visible = true;
                btnWarn2.Enabled = true;
                btnWarn2.Visible = true;
                btnWarn3.Enabled = true;
                btnWarn3.Visible = true;
                btnWarn4.Enabled = true;
                btnWarn4.Visible = true;
                btnWarn5.Enabled = true;
                btnWarn5.Visible = true;
                btnWarn6.Enabled = true;
                btnWarn6.Visible = true;
                btnWarn7.Enabled = true;
                btnWarn7.Visible = true;
                btnWarn8.Enabled = true;
                btnWarn8.Visible = true;


            }
            else
            {

                btn_check.Enabled = ControlAttributes.BoolControl("btn_check", "CarInOutForm", "Visible");
                btn_check.Visible = ControlAttributes.BoolControl("btn_check", "CarInOutForm", "Enabled");
                btn_outSumbit.Enabled = ControlAttributes.BoolControl("btn_outSumbit", "CarInOutForm", "Visible");
                btn_outSumbit.Visible = ControlAttributes.BoolControl("btn_outSumbit", "CarInOutForm", "Enabled");

                btnInout1.Visible = ControlAttributes.BoolControl("btn", "CarInOutForm", "Visible");
                btnInout1.Enabled = ControlAttributes.BoolControl("btn", "CarInOutForm", "Enabled");
                btnInout2.Visible = ControlAttributes.BoolControl("btn", "CarInOutForm", "Visible");
                btnInout2.Enabled = ControlAttributes.BoolControl("btn", "CarInOutForm", "Enabled");
                btnInout3.Visible = ControlAttributes.BoolControl("btn", "CarInOutForm", "Visible");
                btnInout3.Enabled = ControlAttributes.BoolControl("btn", "CarInOutForm", "Enabled");
                btnInout4.Visible = ControlAttributes.BoolControl("btn", "CarInOutForm", "Visible");
                btnInout4.Enabled = ControlAttributes.BoolControl("btn", "CarInOutForm", "Enabled");
                btnInout5.Visible = ControlAttributes.BoolControl("btn", "CarInOutForm", "Visible");
                btnInout5.Enabled = ControlAttributes.BoolControl("btn", "CarInOutForm", "Enabled");
                btnInout6.Visible = ControlAttributes.BoolControl("btn", "CarInOutForm", "Visible");
                btnInout6.Enabled = ControlAttributes.BoolControl("btn", "CarInOutForm", "Enabled");
                btnInout7.Visible = ControlAttributes.BoolControl("btn", "CarInOutForm", "Visible");
                btnInout7.Enabled = ControlAttributes.BoolControl("btn", "CarInOutForm", "Enabled");
                btnInout8.Visible = ControlAttributes.BoolControl("btn", "CarInOutForm", "Visible");
                btnInout8.Enabled = ControlAttributes.BoolControl("btn", "CarInOutForm", "Enabled");

                btnWarn1.Visible = ControlAttributes.BoolControl("btnblck", "CarInOutForm", "Visible");
                btnWarn1.Enabled = ControlAttributes.BoolControl("btnblck", "CarInOutForm", "Enabled");
                btnWarn2.Visible = ControlAttributes.BoolControl("btnblck", "CarInOutForm", "Visible");
                btnWarn2.Enabled = ControlAttributes.BoolControl("btnblck", "CarInOutForm", "Enabled");
                btnWarn3.Visible = ControlAttributes.BoolControl("btnblck", "CarInOutForm", "Visible");
                btnWarn3.Enabled = ControlAttributes.BoolControl("btnblck", "CarInOutForm", "Enabled");
                btnWarn4.Visible = ControlAttributes.BoolControl("btnblck", "CarInOutForm", "Visible");
                btnWarn4.Enabled = ControlAttributes.BoolControl("btnblck", "CarInOutForm", "Enabled");
                btnWarn5.Visible = ControlAttributes.BoolControl("btnblck", "CarInOutForm", "Visible");
                btnWarn5.Enabled = ControlAttributes.BoolControl("btnblck", "CarInOutForm", "Enabled");
                btnWarn6.Visible = ControlAttributes.BoolControl("btnblck", "CarInOutForm", "Visible");
                btnWarn6.Enabled = ControlAttributes.BoolControl("btnblck", "CarInOutForm", "Enabled");
                btnWarn7.Visible = ControlAttributes.BoolControl("btnblck", "CarInOutForm", "Visible");
                btnWarn7.Enabled = ControlAttributes.BoolControl("btnblck", "CarInOutForm", "Enabled");
                btnWarn8.Visible = ControlAttributes.BoolControl("btnblck", "CarInOutForm", "Visible");
                btnWarn8.Enabled = ControlAttributes.BoolControl("btnblck", "CarInOutForm", "Enabled");

            }
        }


        private void CreateControl()
        {
            DataTable table = LinQBaseDao.Query("select Driveway_ID, Driveway_Value, Driveway_Name from Driveway where Driveway_Position_ID in (select Position_ID from Position where Position_Value='" + SystemClass.PosistionValue + "' and Position_State='启动') and Driveway_State='启动' ").Tables[0];
            if (table.Rows.Count > 0)
            {
                BindData(table);
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    dicValues.Add(table.Rows[i]["Driveway_Value"].ToString(), table.Rows[i]["Driveway_Name"].ToString());
                }
            }
            else
            {
                this.groupBoxs.Height = 90;//当前容器的高度
                Label lbms = new Label();
                lbms.Name = "labIDs";
                lbms.Text = "当前门岗没有通道信息！";
                lbms.Size = new Size(280, 31);
                lbms.ForeColor = System.Drawing.Color.Fuchsia;
                lbms.Font = new Font("宋体", 16);
                lbms.Location = new Point(229, 32);
                lbms.Margin = new Padding(3, 0, 3, 0);
                lbms.Anchor = AnchorStyles.Top;
                this.groupBoxs.Controls.Add(lbms);
                lbms.BringToFront();
            }
        }

        string Drivewayid1, Drivewayid2, Drivewayid3, Drivewayid4, Drivewayid5, Drivewayid6, Drivewayid7, Drivewayid8;
        /// <summary>
        /// 绑定通道
        /// </summary>
        private void BindData(DataTable dt)
        {
            int count = dt.Rows.Count;
            if (count == 1)
            {
                dicValues.Clear();
                groupBox2.Visible = true;
                groupBox2.Text = dt.Rows[0]["Driveway_Name"].ToString();
                Drivewayid1 = dt.Rows[0]["Driveway_ID"].ToString();
            }
            else if (count == 2)
            {
                dicValues.Clear();
                groupBox2.Visible = true;
                groupBox3.Visible = true;
                groupBox2.Text = dt.Rows[0]["Driveway_Name"].ToString();
                groupBox3.Text = dt.Rows[1]["Driveway_Name"].ToString();
                Drivewayid1 = dt.Rows[0]["Driveway_ID"].ToString();
                Drivewayid2 = dt.Rows[1]["Driveway_ID"].ToString();
            }
            else if (count == 3)
            {
                dicValues.Clear();
                groupBox2.Visible = true;
                groupBox3.Visible = true;
                groupBox4.Visible = true;
                groupBox2.Text = dt.Rows[0]["Driveway_Name"].ToString();
                groupBox3.Text = dt.Rows[1]["Driveway_Name"].ToString();
                groupBox4.Text = dt.Rows[2]["Driveway_Name"].ToString();
                Drivewayid1 = dt.Rows[0]["Driveway_ID"].ToString();
                Drivewayid2 = dt.Rows[1]["Driveway_ID"].ToString();
                Drivewayid3 = dt.Rows[2]["Driveway_ID"].ToString();
            }
            else if (count == 4)
            {
                dicValues.Clear();
                groupBox2.Visible = true;
                groupBox3.Visible = true;
                groupBox4.Visible = true;
                groupBox5.Visible = true;
                groupBox2.Text = dt.Rows[0]["Driveway_Name"].ToString();
                groupBox3.Text = dt.Rows[1]["Driveway_Name"].ToString();
                groupBox4.Text = dt.Rows[2]["Driveway_Name"].ToString();
                groupBox5.Text = dt.Rows[3]["Driveway_Name"].ToString();
                Drivewayid1 = dt.Rows[0]["Driveway_ID"].ToString();
                Drivewayid2 = dt.Rows[1]["Driveway_ID"].ToString();
                Drivewayid3 = dt.Rows[2]["Driveway_ID"].ToString();
                Drivewayid4 = dt.Rows[3]["Driveway_ID"].ToString();
            }
            else if (count == 5)
            {
                dicValues.Clear();
                groupBox2.Visible = true;
                groupBox3.Visible = true;
                groupBox4.Visible = true;
                groupBox5.Visible = true;
                groupBox6.Visible = true;
                groupBox2.Text = dt.Rows[0]["Driveway_Name"].ToString();
                groupBox3.Text = dt.Rows[1]["Driveway_Name"].ToString();
                groupBox4.Text = dt.Rows[2]["Driveway_Name"].ToString();
                groupBox5.Text = dt.Rows[3]["Driveway_Name"].ToString();
                groupBox6.Text = dt.Rows[4]["Driveway_Name"].ToString();
                Drivewayid1 = dt.Rows[0]["Driveway_ID"].ToString();
                Drivewayid2 = dt.Rows[1]["Driveway_ID"].ToString();
                Drivewayid3 = dt.Rows[2]["Driveway_ID"].ToString();
                Drivewayid4 = dt.Rows[3]["Driveway_ID"].ToString();
                Drivewayid5 = dt.Rows[4]["Driveway_ID"].ToString();
            }
            else if (count == 6)
            {
                dicValues.Clear();
                groupBox2.Visible = true;
                groupBox3.Visible = true;
                groupBox4.Visible = true;
                groupBox5.Visible = true;
                groupBox6.Visible = true;
                groupBox7.Visible = true;
                groupBox2.Text = dt.Rows[0]["Driveway_Name"].ToString();
                groupBox3.Text = dt.Rows[1]["Driveway_Name"].ToString();
                groupBox4.Text = dt.Rows[2]["Driveway_Name"].ToString();
                groupBox5.Text = dt.Rows[3]["Driveway_Name"].ToString();
                groupBox6.Text = dt.Rows[4]["Driveway_Name"].ToString();
                groupBox7.Text = dt.Rows[5]["Driveway_Name"].ToString();
                Drivewayid1 = dt.Rows[0]["Driveway_ID"].ToString();
                Drivewayid2 = dt.Rows[1]["Driveway_ID"].ToString();
                Drivewayid3 = dt.Rows[2]["Driveway_ID"].ToString();
                Drivewayid4 = dt.Rows[3]["Driveway_ID"].ToString();
                Drivewayid5 = dt.Rows[4]["Driveway_ID"].ToString();
                Drivewayid6 = dt.Rows[5]["Driveway_ID"].ToString();

            }
            else if (count == 7)
            {
                dicValues.Clear();
                groupBox2.Visible = true;
                groupBox3.Visible = true;
                groupBox4.Visible = true;
                groupBox5.Visible = true;
                groupBox6.Visible = true;
                groupBox7.Visible = true;
                groupBox8.Visible = true;
                groupBox2.Text = dt.Rows[0]["Driveway_Name"].ToString();
                groupBox3.Text = dt.Rows[1]["Driveway_Name"].ToString();
                groupBox4.Text = dt.Rows[2]["Driveway_Name"].ToString();
                groupBox5.Text = dt.Rows[3]["Driveway_Name"].ToString();
                groupBox6.Text = dt.Rows[4]["Driveway_Name"].ToString();
                groupBox7.Text = dt.Rows[5]["Driveway_Name"].ToString();
                groupBox8.Text = dt.Rows[6]["Driveway_Name"].ToString();
                Drivewayid1 = dt.Rows[0]["Driveway_ID"].ToString();
                Drivewayid2 = dt.Rows[1]["Driveway_ID"].ToString();
                Drivewayid3 = dt.Rows[2]["Driveway_ID"].ToString();
                Drivewayid4 = dt.Rows[3]["Driveway_ID"].ToString();
                Drivewayid5 = dt.Rows[4]["Driveway_ID"].ToString();
                Drivewayid6 = dt.Rows[5]["Driveway_ID"].ToString();
                Drivewayid7 = dt.Rows[6]["Driveway_ID"].ToString();
            }
            else if (count == 8)
            {
                dicValues.Clear();
                groupBox2.Visible = true;
                groupBox3.Visible = true;
                groupBox4.Visible = true;
                groupBox5.Visible = true;
                groupBox6.Visible = true;
                groupBox7.Visible = true;
                groupBox8.Visible = true;
                groupBox9.Visible = true;
                groupBox2.Text = dt.Rows[0]["Driveway_Name"].ToString();
                groupBox3.Text = dt.Rows[1]["Driveway_Name"].ToString();
                groupBox4.Text = dt.Rows[2]["Driveway_Name"].ToString();
                groupBox5.Text = dt.Rows[3]["Driveway_Name"].ToString();
                groupBox6.Text = dt.Rows[4]["Driveway_Name"].ToString();
                groupBox7.Text = dt.Rows[5]["Driveway_Name"].ToString();
                groupBox8.Text = dt.Rows[6]["Driveway_Name"].ToString();
                groupBox9.Text = dt.Rows[7]["Driveway_Name"].ToString();
                Drivewayid1 = dt.Rows[0]["Driveway_ID"].ToString();
                Drivewayid2 = dt.Rows[1]["Driveway_ID"].ToString();
                Drivewayid3 = dt.Rows[2]["Driveway_ID"].ToString();
                Drivewayid4 = dt.Rows[3]["Driveway_ID"].ToString();
                Drivewayid5 = dt.Rows[4]["Driveway_ID"].ToString();
                Drivewayid6 = dt.Rows[5]["Driveway_ID"].ToString();
                Drivewayid7 = dt.Rows[6]["Driveway_ID"].ToString();
                Drivewayid8 = dt.Rows[7]["Driveway_ID"].ToString();
            }
        }
        /// <summary>
        /// 黑名单警告放行，黑名单等级添加一次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private bool blacktnClick(string carname)
        {
            bool istrue = true;
            try
            {

                //1.查询黑名单是否存在车辆
                //2.如果不存在需要添加
                //3.如果存在，等级次数加1
                //放行成功

                if (LinQBaseDao.Query("select * from Blacklist where Blacklist_CarInfo_ID in(select Car_ID from Car where Car_Name ='" + carname + "')").Tables[0].Rows.Count >= 1)
                {
                    //黑名单次数加1
                    LinQBaseDao.Query("update Blacklist set Blacklist_UpgradeCount+=1 where Blacklist_CarInfo_ID in(select Car_ID from Car where Car_Name='" + carname + "')");


                    DataTable BlacklistDT = LinQBaseDao.Query("select Blacklist_ID,Blacklist_Dictionary_ID,Blacklist_UpgradeCount,Blacklist_DowngradeCount,Dictionary_Name,Dictionary_Spare_int1,Dictionary_Spare_int2 from Blacklist,Dictionary where Blacklist.Blacklist_CarInfo_ID = (select Car_ID from Car where Car_Name='" + carname + "') and Dictionary_ID=Blacklist_Dictionary_ID").Tables[0];
                    if (BlacklistDT.Rows.Count > 0)
                    {
                        int Blacklist_ID = Convert.ToInt32(BlacklistDT.Rows[0]["Blacklist_ID"]);
                        int Blacklist_Dictionary_ID = Convert.ToInt32(BlacklistDT.Rows[0]["Blacklist_Dictionary_ID"]);
                        int Blacklist_UpgradeCount = Convert.ToInt32(BlacklistDT.Rows[0]["Blacklist_UpgradeCount"]);
                        int Dictionary_Spare_int1 = Convert.ToInt32(BlacklistDT.Rows[0]["Dictionary_Spare_int1"]);

                        //达到升级次数就升级
                        if (Blacklist_UpgradeCount == Dictionary_Spare_int1)
                        {
                            //查询出当前黑名单的上级，并升级
                            DataTable UpBlacklistDT = LinQBaseDao.Query("select top(1)Dictionary_ID from  Dictionary where Dictionary_Sort > " + Blacklist_Dictionary_ID).Tables[0];

                            if (UpBlacklistDT.Rows.Count == 1)
                            {
                                Expression<Func<Blacklist, bool>> fn = n => n.Blacklist_ID == Blacklist_ID;
                                Action<Blacklist> action = a => a.Blacklist_Dictionary_ID = Convert.ToInt32(UpBlacklistDT.Rows[0][0]);
                                if (!BlacklistDAL.Update(fn, action))
                                {
                                    MessageBox.Show("警告放行失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    istrue = false;
                                }
                            }

                        }

                    }

                }
                else
                {//添加到黑名单，黑名单次数添加一个，如果达到拒绝入场则不能进入

                    Blacklist bk = new Blacklist();
                    DataTable dt = LinQBaseDao.Query("select Car_ID from Car where Car_Name='" + carname + "'").Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        bk.Blacklist_CarInfo_ID = Convert.ToInt32(dt.Rows[0][0]);
                        istrue = false;
                    }
                    bk.Blacklist_UpgradeCount = 1;
                    bk.Blacklist_Name = "该车进入" + SystemClass.PosistionValue + "号门岗时，警告放行！";
                    bk.Blacklist_People = CommonalityEntity.USERNAME;
                    bk.Blacklist_Time = CommonalityEntity.GetServersTime();
                    bk.Blacklist_Dictionary_ID = Convert.ToInt32(LinQBaseDao.Query("select Dictionary_ID from Dictionary where Dictionary_Value='0902'").Tables[0].Rows[0][0]);
                    bk.Blacklist_State = "警告";
                    bk.Blacklist_Type = "车";
                    if (!BlacklistDAL.InsertOneQCRecord(bk))
                    {
                        MessageBox.Show("警告放行失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        istrue = false;
                    }
                }
            }
            catch
            {

                istrue = false;
            }
            return istrue;
        }
        /// <summary>
        /// 校验放行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Click(object sender, EventArgs e, int num)
        {
            try
            {
                if (num == 1)
                {
                    carIds = cmbCarNum1.Text;
                    string str = "";
                    str = IntionICCarprint(txtCar1.Text.Trim(), txtUser1.Text.Trim(), carIds, Drivewayid1);
                    if (!string.IsNullOrEmpty(str))
                    {
                        PromptInfo(str, num);
                        return;
                    }
                }
                else if (num == 2)
                {
                    carIds = cmbCarNum2.Text;
                    string str = "";
                    str = IntionICCarprint(txtCar2.Text.Trim(), txtUser2.Text.Trim(), carIds, Drivewayid2);
                    if (!string.IsNullOrEmpty(str))
                    {
                        PromptInfo(str, num);
                        return;
                    }
                }
                else if (num == 3)
                {
                    carIds = cmbCarNum3.Text;
                    string str = "";
                    str = IntionICCarprint(txtCar3.Text.Trim(), txtUser3.Text.Trim(), carIds, Drivewayid3);
                    if (!string.IsNullOrEmpty(str))
                    {
                        PromptInfo(str, num);
                        return;
                    }
                }
                else if (num == 4)
                {
                    carIds = cmbCarNum4.Text;
                    string str = "";
                    str = IntionICCarprint(txtCar4.Text.Trim(), txtUser4.Text.Trim(), carIds, Drivewayid4);
                    if (!string.IsNullOrEmpty(str))
                    {
                        PromptInfo(str, num);
                        return;
                    }
                }
                else if (num == 5)
                {
                    carIds = cmbCarNum5.Text;
                    string str = "";
                    str = IntionICCarprint(txtCar5.Text.Trim(), txtUser5.Text.Trim(), carIds, Drivewayid5);
                    if (!string.IsNullOrEmpty(str))
                    {
                        PromptInfo(str, num);
                        return;
                    }
                }
                else if (num == 6)
                {
                    carIds = cmbCarNum6.Text;
                    string str = "";
                    str = IntionICCarprint(txtCar6.Text.Trim(), txtUser6.Text.Trim(), carIds, Drivewayid6);
                    if (!string.IsNullOrEmpty(str))
                    {
                        PromptInfo(str, num);
                        return;
                    }
                }
                else if (num == 7)
                {
                    carIds = cmbCarNum7.Text;
                    string str = "";
                    str = IntionICCarprint(txtCar7.Text.Trim(), txtUser7.Text.Trim(), carIds, Drivewayid7);
                    if (!string.IsNullOrEmpty(str))
                    {
                        PromptInfo(str, num);
                        return;
                    }
                }
                else if (num == 8)
                {
                    carIds = cmbCarNum8.Text;
                    string str = "";
                    str = IntionICCarprint(txtCar8.Text.Trim(), txtUser8.Text.Trim(), carIds, Drivewayid8);
                    if (!string.IsNullOrEmpty(str))
                    {
                        PromptInfo(str, num);
                        return;
                    }
                }
                string strc = CameraCheck();

                if (!string.IsNullOrEmpty(strc))
                {
                    PromptInfo(strc, num);
                    return;
                }
                if (CarInformationFormOne.cinfo == null)
                {
                    CommonalityEntity.CarValue.Add(CommonalityEntity.Driveway_Value);
                    CarInformationFormOne.cinfo = new CarInformationFormOne(carIds, false);
                    //CarInformationFormOne.cinfo.mf = this.mf;
                    CarInformationFormOne.cinfo.Show();
                    PromptInfo("该车道正在放行中！", num);
                }
                else
                {
                    if (CarInformationFormTwo.cinfo == null)
                    {
                        CommonalityEntity.CarValue.Add(CommonalityEntity.Driveway_Value);
                        CarInformationFormTwo.cinfo = new CarInformationFormTwo(carIds, false);
                        //CarInformationFormTwo.cinfo.mf = this.mf;
                        CarInformationFormTwo.cinfo.Show();
                        PromptInfo("该车道正在放行中！", num);
                    }
                    else
                    {
                        if (CarInformationFormThree.cinfo == null)
                        {
                            CommonalityEntity.CarValue.Add(CommonalityEntity.Driveway_Value);
                            CarInformationFormThree.cinfo = new CarInformationFormThree(carIds, false);
                            //CarInformationFormThree.cinfo.mf = this.mf;
                            CarInformationFormThree.cinfo.Show();
                            PromptInfo("该车道正在放行中！", num);
                        }
                        else
                        {
                            if (CarInformationFormFour.cinfo == null)
                            {
                                CommonalityEntity.CarValue.Add(CommonalityEntity.Driveway_Value);
                                CarInformationFormFour.cinfo = new CarInformationFormFour(carIds, false);
                                //CarInformationFormFour.cinfo.mf = this.mf;
                                CarInformationFormFour.cinfo.Show();
                                PromptInfo("该车道正在放行中！", num);
                            }
                            else
                            {
                                // CarInformationFormFour.cinfo.Activate();
                                if (CarInformationFormFive.cinfo == null)
                                {
                                    CommonalityEntity.CarValue.Add(CommonalityEntity.Driveway_Value);
                                    CarInformationFormFive.cinfo = new CarInformationFormFive(carIds, false);
                                    //CarInformationFormFive.cinfo.mf = this.mf;
                                    CarInformationFormFive.cinfo.Show();
                                    PromptInfo("该车道正在放行中！", num);
                                }
                                else
                                {
                                    if (CarInformationFormSix.cinfo == null)
                                    {
                                        CommonalityEntity.CarValue.Add(CommonalityEntity.Driveway_Value);
                                        CarInformationFormSix.cinfo = new CarInformationFormSix(carIds, false);
                                        //CarInformationFormSix.cinfo.mf = this.mf;
                                        CarInformationFormSix.cinfo.Show();
                                        PromptInfo("该车道正在放行中！", num);
                                    }
                                    else
                                    {
                                        CarInformationFormSix.cinfo.Activate();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }
        /// <summary>
        /// 关闭当前窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarInOutForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer_ICNo.Enabled = false;
            CarInOutForm.cfin = null;
        }

        DataTable table = new DataTable();
        DataTable tables = new DataTable();
        public static List<DeviceControl> listDC = null; //new List<DeviceControl>();
        /// <summary>
        /// key通道值 value通道名称
        /// </summary>
        public static Dictionary<string, List<CardEntity>> dicicvalues = new Dictionary<string, List<CardEntity>>();
        Button senders = new Button();
        EventArgs es = new EventArgs();
        private void timer_ICNo_Tick(object sender, EventArgs e)
        {
            try
            {
                if (MainForm.dicCard.Count > 0)
                {
                    foreach (KeyValuePair<string, List<CardEntity>> temp in MainForm.dicCard)
                    {
                        string dname = temp.Key;
                        if (dicValues.Count > 0)
                        {
                            foreach (var item in dicValues)
                            {
                                if (dname == item.Key)
                                {
                                    if (temp.Value.Count > 0)
                                    {
                                        foreach (var it in temp.Value)
                                        {
                                            contol(it.CardTyep, item.Value, it.CardNo, it.CarNo, it.CardCar);
                                        }
                                    }
                                    emptyContol();
                                    //else
                                    //{
                                    //    emptyContol();
                                    //}
                                }
                            }
                        }
                    }
                }
                if (lbPrompt1.ForeColor != Color.Red)
                {
                    lbPrompt1.ForeColor = Color.Red;
                    lbPrompt2.ForeColor = Color.Red;
                    lbPrompt3.ForeColor = Color.Red;
                    lbPrompt4.ForeColor = Color.Red;
                    lbPrompt5.ForeColor = Color.Red;
                    lbPrompt6.ForeColor = Color.Red;
                    lbPrompt7.ForeColor = Color.Red;
                    lbPrompt8.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                CommonalityEntity.WriteTextLog("CarInOutForm.timer_ICNo_Tick异常:" + ex.Message.ToString());
            }
        }

        private void OPENTick(object sender, EventArgs e, int num)
        {
            if (num == 1)
            {
                btnInout1_Click(sender, e);
            }
            else if (num == 2)
            {
                btnInout2_Click(sender, e);
            }
            else if (num == 3)
            {
                btnInout3_Click(sender, e);
            }
            else if (num == 4)
            {
                btnInout4_Click(sender, e);
            }
            else if (num == 5)
            {
                btnInout5_Click(sender, e);
            }
            else if (num == 6)
            {
                btnInout6_Click(sender, e);
            }
            else if (num == 7)
            {
                btnInout7_Click(sender, e);
            }
            else if (num == 8)
            {
                btnInout8_Click(sender, e);
            }
        }
        /// <summary>
        /// 执行方法
        /// </summary>
        private void OPENPage(object sender, EventArgs e, int num)
        {
            if (CarInformationFormOne.cinfo == null)
            {
                OPENTick(sender, e, num);
            }
            else
            {
                if (CarInformationFormTwo.cinfo == null)
                {
                    OPENTick(sender, e, num);
                }
                else
                {
                    if (CarInformationFormThree.cinfo == null)
                    {
                        OPENTick(sender, e, num);
                    }
                    else
                    {
                        if (CarInformationFormFour.cinfo == null)
                        {
                            OPENTick(sender, e, num);
                        }
                        else
                        {
                            // return;
                            if (CarInformationFormFive.cinfo == null)
                            {
                                OPENTick(sender, e, num);
                            }
                            else
                            {
                                if (CarInformationFormSix.cinfo == null)
                                {
                                    OPENTick(sender, e, num);
                                }
                                else
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 给对应通道文本框赋值
        /// </summary>
        /// <param name="cartype"> 卡类型</param>
        /// <param name="driname">通道名称 </param>
        /// <param name="carnum">IC卡号 </param>
        /// <param name="usercar">IC卡号 </param>
        ///   <param name="usercar">车牌号</param>
        private void contol(string cartype, string driname, string carnum, string usercar, string carno)
        {
            foreach (var item in groupBoxs.Controls)
            {
                if ((GroupBox)item is GroupBox)
                {
                    if (((GroupBox)item).Text == driname)
                    {
                        string gr = ((GroupBox)item).Name;
                        if (gr == "groupBox2")
                        {
                            string st1 = txtCar1.Text;
                            string st2 = txtUser1.Text;
                            if (usercar == "车卡")
                            {
                                if (txtCar1.Text != carnum)
                                {
                                    txtCar1.Text = carnum;
                                    cmbCarNum1.Text = carno;
                                }
                            }
                            if (usercar == "人卡")
                            {
                                if (txtUser1.Text != carnum)
                                {
                                    txtUser1.Text = carnum;
                                }
                            }
                            if (usercar == "车卡人卡")
                            {
                                if (txtCar1.Text != carnum || txtUser1.Text != carnum)
                                {
                                    txtCar1.Text = carnum;
                                    txtUser1.Text = carnum;
                                    cmbCarNum1.Text = carno;
                                }
                            }
                            if (string.IsNullOrEmpty(lbPrompt1.Text.Trim()))
                            {
                                if (!string.IsNullOrEmpty(txtCar1.Text.Trim()) && !string.IsNullOrEmpty(txtUser1.Text.Trim()))
                                {
                                    OPENPage(senders, es, 1);
                                }
                            }

                        }
                        else if (gr == "groupBox3")
                        {
                            string st1 = txtCar2.Text;
                            string st2 = txtUser2.Text;
                            if (usercar == "车卡")
                            {
                                if (txtCar2.Text != carnum)
                                {
                                    txtCar2.Text = carnum;
                                    cmbCarNum2.Text = carno;
                                }
                            }
                            if (usercar == "人卡")
                            {
                                if (txtUser2.Text != carnum)
                                {
                                    txtUser2.Text = carnum;
                                }
                            }
                            if (usercar == "车卡人卡")
                            {
                                if (txtCar2.Text != carnum || txtUser2.Text != carnum)
                                {
                                    txtCar2.Text = carnum;
                                    txtUser2.Text = carnum;
                                    cmbCarNum2.Text = carno;
                                }
                            }
                            if (string.IsNullOrEmpty(lbPrompt2.Text.Trim()))
                            {
                                if (!string.IsNullOrEmpty(txtCar2.Text.Trim()) && !string.IsNullOrEmpty(txtUser2.Text.Trim()))
                                {
                                    OPENPage(senders, es, 2);
                                }
                            }

                        }
                        else if (gr == "groupBox4")
                        {
                            string st1 = txtCar3.Text;
                            string st2 = txtUser3.Text;
                            if (usercar == "车卡")
                            {
                                if (txtCar3.Text != carnum)
                                {
                                    txtCar3.Text = carnum;
                                    cmbCarNum3.Text = carno;
                                }
                            }

                            if (usercar == "人卡")
                            {
                                if (txtUser3.Text != carnum)
                                {
                                    txtUser3.Text = carnum;
                                }
                            }
                            if (usercar == "车卡人卡")
                            {
                                if (txtCar3.Text != carnum || txtUser3.Text != carnum)
                                {
                                    txtCar3.Text = carnum;
                                    txtUser3.Text = carnum;
                                    cmbCarNum3.Text = carno;
                                }
                            }
                            if (string.IsNullOrEmpty(lbPrompt3.Text.Trim()))
                            {
                                if (!string.IsNullOrEmpty(txtCar3.Text.Trim()) && !string.IsNullOrEmpty(txtUser3.Text.Trim()))
                                {
                                    OPENPage(senders, es, 3);
                                }
                            }

                        }
                        else if (gr == "groupBox5")
                        {
                            string st1 = txtCar4.Text;
                            string st2 = txtUser4.Text;
                            if (usercar == "车卡")
                            {
                                if (txtCar4.Text != carnum)
                                {
                                    txtCar4.Text = carnum;
                                    cmbCarNum4.Text = carno;
                                }
                            }
                            if (usercar == "人卡")
                            {
                                if (txtUser4.Text != carnum)
                                {
                                    txtUser4.Text = carnum;
                                }
                            }
                            if (usercar == "车卡人卡")
                            {
                                if (txtCar4.Text != carnum || txtUser4.Text != carnum)
                                {
                                    txtCar4.Text = carnum;
                                    txtUser4.Text = carnum;
                                    cmbCarNum4.Text = carno;
                                }
                            }
                            if (string.IsNullOrEmpty(lbPrompt4.Text.Trim()))
                            {
                                if (!string.IsNullOrEmpty(txtCar4.Text.Trim()) && !string.IsNullOrEmpty(txtUser4.Text.Trim()))
                                {
                                    OPENPage(senders, es, 4);
                                }
                            }

                        }
                        else if (gr == "groupBox6")
                        {
                            string st1 = txtCar5.Text;
                            string st2 = txtUser5.Text;
                            if (usercar == "车卡")
                            {
                                if (txtCar5.Text != carnum)
                                {
                                    txtCar5.Text = carnum;
                                    cmbCarNum5.Text = carno;
                                }
                            }
                            if (usercar == "人卡")
                            {
                                if (txtUser5.Text != carnum)
                                {
                                    txtUser5.Text = carnum;
                                }
                            }
                            if (usercar == "车卡人卡")
                            {
                                if (txtCar5.Text != carnum || txtUser5.Text != carnum)
                                {
                                    txtCar5.Text = carnum;
                                    txtUser5.Text = carnum;
                                    cmbCarNum5.Text = carno;
                                }
                            }
                            if (string.IsNullOrEmpty(lbPrompt5.Text.Trim()))
                            {
                                if (!string.IsNullOrEmpty(txtCar5.Text.Trim()) && !string.IsNullOrEmpty(txtUser5.Text.Trim()))
                                {
                                    OPENPage(senders, es, 5);
                                }
                            }

                        }
                        else if (gr == "groupBox7")
                        {
                            string st1 = txtCar6.Text;
                            string st2 = txtUser6.Text;
                            if (usercar == "车卡")
                            {
                                if (txtCar6.Text != carnum)
                                {
                                    txtCar6.Text = carnum;
                                    cmbCarNum6.Text = carno;
                                }
                            }
                            if (usercar == "人卡")
                            {
                                if (txtUser6.Text != carnum)
                                {
                                    txtUser6.Text = carnum;
                                }
                            }
                            if (usercar == "车卡人卡")
                            {
                                if (txtCar6.Text != carnum || txtUser6.Text != carnum)
                                {
                                    txtCar6.Text = carnum;
                                    txtUser6.Text = carnum;
                                    cmbCarNum6.Text = carno;
                                }
                            }
                            if (string.IsNullOrEmpty(lbPrompt6.Text.Trim()))
                            {
                                if (!string.IsNullOrEmpty(txtCar6.Text.Trim()) && !string.IsNullOrEmpty(txtUser6.Text.Trim()))
                                {
                                    OPENPage(senders, es, 6);
                                }
                            }

                        }
                        else if (gr == "groupBox8")
                        {
                            string st1 = txtCar7.Text;
                            string st2 = txtUser7.Text;
                            if (usercar == "车卡")
                            {
                                if (txtCar7.Text != carnum)
                                {
                                    txtCar7.Text = carnum;
                                    cmbCarNum7.Text = carno;
                                }
                            }
                            if (usercar == "人卡")
                            {
                                if (txtUser7.Text != carnum)
                                {
                                    txtUser7.Text = carnum;
                                }
                            }
                            if (usercar == "车卡人卡")
                            {
                                if (txtCar7.Text != carnum || txtUser7.Text != carnum)
                                {
                                    txtCar7.Text = carnum;
                                    txtUser7.Text = carnum;
                                    cmbCarNum7.Text = carno;
                                }
                            }
                            if (string.IsNullOrEmpty(lbPrompt7.Text.Trim()))
                            {
                                if (!string.IsNullOrEmpty(txtCar7.Text.Trim()) && !string.IsNullOrEmpty(txtUser7.Text.Trim()))
                                {
                                    OPENPage(senders, es, 7);
                                }
                            }

                        }
                        else if (gr == "groupBox9")
                        {
                            string st1 = txtCar8.Text;
                            string st2 = txtUser8.Text;
                            if (usercar == "车卡")
                            {
                                if (txtCar8.Text != carnum)
                                {
                                    txtCar8.Text = carnum;
                                    cmbCarNum8.Text = carno;
                                }
                            }
                            if (usercar == "人卡")
                            {
                                if (txtUser8.Text != carnum)
                                {
                                    txtUser8.Text = carnum;
                                }
                            }
                            if (usercar == "车卡人卡")
                            {
                                if (txtCar8.Text != carnum || txtUser8.Text != carnum)
                                {
                                    txtCar8.Text = carnum;
                                    txtUser8.Text = carnum;
                                    //carno = bindcar(carnum);
                                    cmbCarNum8.Text = carno;
                                }
                            }
                            if (string.IsNullOrEmpty(lbPrompt8.Text.Trim()))
                            {
                                if (!string.IsNullOrEmpty(txtCar8.Text.Trim()) && !string.IsNullOrEmpty(txtUser8.Text.Trim()))
                                {
                                    OPENPage(senders, es, 8);
                                }
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 清空文本框的值
        /// </summary>
        private void emptyContol()
        {
            int i = 0;
            if (CommonalityEntity.contolone > 0)
            {
                i = CommonalityEntity.contolone;
                CommonalityEntity.contolone = 0;
                emptyTxt(i);
                i = 0;
            }
            if (CommonalityEntity.contoltwo > 0)
            {
                i = CommonalityEntity.contoltwo;
                CommonalityEntity.contoltwo = 0;
                emptyTxt(i);
                i = 0;
            }
            if (CommonalityEntity.contolthree > 0)
            {
                i = CommonalityEntity.contolthree;
                CommonalityEntity.contolthree = 0;
                emptyTxt(i);
                i = 0;
            }
            if (CommonalityEntity.contolfour > 0)
            {
                i = CommonalityEntity.contolfour;
                CommonalityEntity.contolfour = 0;
                emptyTxt(i);
                i = 0;
            }
            if (CommonalityEntity.contolfive > 0)
            {
                i = CommonalityEntity.contolfive;
                CommonalityEntity.contolfive = 0;
                emptyTxt(i);
                i = 0;
            }
            if (CommonalityEntity.contolsix > 0)
            {
                i = CommonalityEntity.contolsix;
                CommonalityEntity.contolsix = 0;
                emptyTxt(i);
                i = 0;
            }
        }
        private void emptyTxt(int i)
        {
            if (i == 1)
            {
                txtCar1.Text = "";
                txtUser1.Text = "";
                cmbCarNum1.Text = "";
                lbPrompt1.Text = "";
            }
            else if (i == 2)
            {
                txtCar2.Text = "";
                txtUser2.Text = "";
                cmbCarNum2.Text = "";
                lbPrompt2.Text = "";
            }
            else if (i == 3)
            {
                txtCar3.Text = "";
                txtUser3.Text = "";
                cmbCarNum3.Text = "";
                lbPrompt3.Text = "";
            }
            else if (i == 4)
            {
                txtCar4.Text = "";
                txtUser4.Text = "";
                cmbCarNum4.Text = "";
                lbPrompt4.Text = "";
            }
            else if (i == 5)
            {
                txtCar5.Text = "";
                txtUser5.Text = "";
                cmbCarNum5.Text = "";
                lbPrompt5.Text = "";
            }
            else if (i == 6)
            {
                txtCar6.Text = "";
                txtUser6.Text = "";
                cmbCarNum6.Text = "";
                lbPrompt6.Text = "";
            }
            else if (i == 7)
            {
                txtCar7.Text = "";
                txtUser7.Text = "";
                cmbCarNum7.Text = "";
                lbPrompt7.Text = "";
            }
            else if (i == 8)
            {
                txtCar8.Text = "";
                txtUser8.Text = "";
                cmbCarNum8.Text = "";
                lbPrompt8.Text = "";
            }
        }
        /// <summary>
        /// 绑定车牌号
        /// </summary>
        /// <param name="icnum">IC卡号</param>
        //private string bindcar(string icnum)
        //{
        //    string carno = "";
        //    DataTable dt;
        //    try
        //    {
        //        dt = LinQBaseDao.Query(" select  Car_Name from Car where  Car_ICCard_ID in (select ICCard_ID from ICCard where ICCard_Value='" + icnum + "')").Tables[0];
        //        carno = dt.Rows[0]["Car_Name"].ToString();
        //    }
        //    catch
        //    {
        //        carno = "";
        //    }
        //    return carno;
        //}
        /// <summary>
        /// 判断车人是否对应，并启动，与规定通行策略是否匹配
        /// </summary>
        private string IntionICCarprint(string caric, string useric, string carno, string driid)
        {
            string str = "";

            if (string.IsNullOrEmpty(caric))
            {
                return str = "请刷车卡！";
            }
            if (string.IsNullOrEmpty(useric))
            {
                return str = "请刷人卡！";
            }
            if (string.IsNullOrEmpty(carno))
            {
                return str = "没有车辆信息！";
            }


            DataTable dtDriveway_Value = LinQBaseDao.Query("select Driveway_ID,Driveway_Type,Driveway_Value,Driveway_Name,Driveway_WarrantyState,Driveway_Remark_Driveway_ID  from Driveway where Driveway_Position_ID =" + SystemClass.PositionID + " and Driveway_State='启动' and  Driveway_ID =" + driid).Tables[0];
            if (dtDriveway_Value.Rows[0]["Driveway_WarrantyState"].ToString() == "报修")
            {
                return str = "此通道处在报修状态，请走备用通道";
            }

            if (IsTongdao(dtDriveway_Value.Rows[0][2].ToString()))
            {
                return str = "该车道正在放行其他车辆！";
            }
            CommonalityEntity.Driveway_ID = Convert.ToInt32(driid);
            SystemClass.DrivewayValue = dtDriveway_Value.Rows[0][2].ToString();
            CommonalityEntity.Driveway_Value = SystemClass.DrivewayValue;
            CommonalityEntity.Driveway_Name = dtDriveway_Value.Rows[0]["Driveway_Name"].ToString();
            if (dtDriveway_Value.Rows[0][1].ToString() == "进")
            {
                CommonalityEntity.ISInOut = true;
            }
            else
            {
                CommonalityEntity.ISInOut = false;
            }
            str = CarCheckIC(caric, useric, driid);
            if (!string.IsNullOrEmpty((str)))
            {
                return str;
            }

            Strategy_DriSName = "";
            dtviewcarstate = LinQBaseDao.Query("select top(1) * from View_CarState where Carinfo_Name='" + carno + "' order by CarInOutRecord_ID desc ").Tables[0];
            if (dtviewcarstate.Rows.Count > 0)
            {
                Strategy_DriSName = dtviewcarstate.Rows[0]["CarInOutRecord_Remark"].ToString();
            }
            CheckProperties.dt = dtviewcarstate;
            CommonalityEntity.DrivewayStrategy_ID = 0;
            CommonalityEntity.ISDengji = true;
            CommonalityEntity.CarIC = caric;
            CommonalityEntity.UserIC = useric;

            DataTable dtstacar;
            try
            {
                dtstacar = LinQBaseDao.Query("select Car_CarType_ID,Car_ISRegister from Car where Car_Name='" + carno + "'").Tables[0];
                if (dtstacar.Rows.Count > 0)
                {
                    cartypeid = dtstacar.Rows[0][0].ToString();
                    CommonalityEntity.Car_ISRegister = Convert.ToBoolean(dtstacar.Rows[0][1].ToString());
                }
                #region 查看车是否存在登记记录
                //当车辆属于内部车，但是又作为外部车在拉货时判断
                if (dtviewcarstate.Rows.Count > 0)
                {
                    string Carinfo_CarType_ID = dtviewcarstate.Rows[0]["CarType_ID"].ToString();
                    string SortNumberInfo_TongXing = dtviewcarstate.Rows[0]["SortNumberInfo_TongXing"].ToString();
                    if (cartypeid != Carinfo_CarType_ID)
                    {
                        DataTable CarInfodts = LinQBaseDao.Query("select top(1) CarInfo_ID from CarInfo where CarInfo_Name='" + carno + "' and carinfo_cartype_id=" + cartypeid).Tables[0];
                        if (CarInfodts.Rows.Count > 0)
                        {
                            CommonalityEntity.ISDengji = true;
                        }
                        else
                        {
                            CommonalityEntity.ISDengji = false;
                            CommonalityEntity.IsUpdatedri = false;
                        }
                    }
                    else
                    {
                        CommonalityEntity.ISDengji = true;
                    }

                }
                else
                {
                    CommonalityEntity.ISDengji = false;
                    CommonalityEntity.IsUpdatedri = false;
                }
                dtstacar = LinQBaseDao.Query("select Car_State,Car_ISRegister,Car_CarType_ID,Car_ISStaffInfo,Car_StaffInfo_IDS from Car where Car_ICCard_ID in (select ICCard_ID from ICCard where  ICCard_Value='" + caric + "' and ICCard_State='启动')").Tables[0];
                if (dtstacar.Rows.Count > 0)
                {
                    #region 验证车是否指定驾驶员
                    bool Car_ISStaffInfo = Convert.ToBoolean(dtstacar.Rows[0]["Car_ISStaffInfo"].ToString());
                    if (Car_ISStaffInfo)
                    {
                        string Car_StaffInfo_IDS = dtstacar.Rows[0]["Car_StaffInfo_IDS"].ToString();
                        string StaffInfo_ID = LinQBaseDao.GetSingle(" select StaffInfo_ID from StaffInfo  where StaffInfo_ICCard_ID in (select ICCard_ID from ICCard where  ICCard_Value='" + useric + "')").ToString();
                        string[] strids = Car_StaffInfo_IDS.Split(',');
                        bool sids = false;
                        foreach (var item in strids)
                        {
                            if (item == StaffInfo_ID)
                            {
                                sids = true;
                                break;
                            }
                        }
                        if (!sids)
                        {
                            return str = "该车辆限定了驾驶员！";
                        }
                    }
                    #endregion

                    #region 不存在登记记录、验证通行策略是否正确
                    if (!CommonalityEntity.ISDengji)
                    {
                        bool issort = false;
                        bool isdwy = false;
                        cartypeid = dtstacar.Rows[0]["Car_CarType_ID"].ToString();
                        DataTable dtdirway = LinQBaseDao.Query("select DrivewayStrategy_Driveway_ID,DrivewayStrategy_Sort from DrivewayStrategy where DrivewayStrategy_Name ='" + Strategy_DriSName + "' and DrivewayStrategy_State='启动'").Tables[0];
                        for (int i = 0; i < dtdirway.Rows.Count; i++)
                        {
                            if (Convert.ToInt32(dtdirway.Rows[i]["DrivewayStrategy_Sort"].ToString()) == 1)
                            {
                                issort = false;
                            }
                            else
                            {
                                issort = true;
                            }
                            if (dtdirway.Rows[i]["DrivewayStrategy_Driveway_ID"].ToString() == driid)
                            {
                                isdwy = true;
                            }
                        }

                        if (issort)
                        {
                            if (isdwy)
                            {
                                if (dtdirway.Rows[0]["DrivewayStrategy_Driveway_ID"].ToString() != driid)
                                {
                                    return str = "通行错误！";
                                }
                            }
                            else
                            {
                                if (dtviewcarstate.Rows.Count > 0)
                                {
                                    str = strDir(dtviewcarstate.Rows[0]["CarType_ID"].ToString(), driid, false, carno);
                                }
                                if (!string.IsNullOrEmpty(str))
                                {
                                    return str = "通行错误！";
                                }
                            }
                        }
                        else
                        {
                            if (!isdwy)
                            {
                                if (dtviewcarstate.Rows.Count > 0)
                                {
                                    str = strDir(dtviewcarstate.Rows[0]["CarType_ID"].ToString(), driid, false, carno);
                                }
                                if (!string.IsNullOrEmpty(str))
                                {
                                    return str = "通行错误！";
                                }
                            }
                        }
                    }
                    #endregion
                }

                #endregion
                if (!string.IsNullOrEmpty(str))
                {
                    return str;
                }
                bool isr = CommonalityEntity.Car_ISRegister;

                if (isr)
                {
                    if (CommonalityEntity.ISDengji)
                    {
                        bool isful = Convert.ToBoolean(dtviewcarstate.Rows[0]["CarInOutRecord_ISFulfill"].ToString());
                        if (isful)
                        {
                            if (cartypeid != dtviewcarstate.Rows[0]["CarInfo_CarType_ID"].ToString())
                            {
                                dtviewcarstate = LinQBaseDao.Query("select top(1) * from View_CarState where Carinfo_Name='" + carno + "' and Cartype_ID=" + cartypeid + " order by CarInOutRecord_ID desc ").Tables[0];
                                if (dtviewcarstate.Rows.Count > 0)
                                {
                                    Strategy_DriSName = dtviewcarstate.Rows[0]["CarInOutRecord_Remark"].ToString();
                                }
                            }
                        }
                        else
                        {
                            string SortNumberInfo_TongXing = dtviewcarstate.Rows[0]["SortNumberInfo_TongXing"].ToString();
                            if (SortNumberInfo_TongXing == "已注销")
                            {
                                if (cartypeid != dtviewcarstate.Rows[0]["CarInfo_CarType_ID"].ToString())
                                {
                                    dtviewcarstate = LinQBaseDao.Query("select top(1) * from View_CarState where Carinfo_Name='" + carno + "' and Cartype_ID=" + cartypeid + " order by CarInOutRecord_ID desc ").Tables[0];
                                    if (dtviewcarstate.Rows.Count > 0)
                                    {
                                        Strategy_DriSName = dtviewcarstate.Rows[0]["CarInOutRecord_Remark"].ToString();
                                    }
                                }
                            }
                        }
                    }
                }

                #region 已登记车辆通行策略校验
                if (CommonalityEntity.ISDengji)
                {
                    if (dtviewcarstate.Rows[0]["CarInfo_State"].ToString() == "启动")
                    {
                        cartypeid = dtviewcarstate.Rows[0]["CarInfo_CarType_ID"].ToString();
                        CommonalityEntity.Car_Type_ID = cartypeid;
                        string carinfoid = dtviewcarstate.Rows[0]["CarInfo_ID"].ToString();
                        carinid = carinfoid;

                        #region 通行车辆业务是否完成
                        bool isful = Convert.ToBoolean(dtviewcarstate.Rows[0]["CarInOutRecord_ISFulfill"].ToString());
                        string strSort = dtviewcarstate.Rows[0]["CarInOutRecord_Sort"].ToString();//通行顺序：有序、无序
                        bool isupdate = Convert.ToBoolean(dtviewcarstate.Rows[0]["CarInOutRecord_Update"].ToString());//是否修改通行策略

                        if (!isful)
                        {
                            string StrategyS = dtviewcarstate.Rows[0]["CarInOutRecord_DrivewayStrategyS"].ToString();//通行策略编号组合
                            string StrategyIDS = dtviewcarstate.Rows[0]["CarInOutRecord_DrivewayStrategy_IDS"].ToString();//已通行策略编号组合
                            CommonalityEntity.IsUpdatedri = isupdate;
                            string strategysout = StrategyS.Substring(StrategyS.LastIndexOf(',') + 1);//车辆最后通行策略，如果等于则需出门授权
                            string strategysin = "";//当前应该通行策略编号

                            #region 得到当前车辆应该通行的通行策略
                            if (StrategyS == StrategyIDS && !string.IsNullOrEmpty(StrategyS))
                            {
                                if (!CommonalityEntity.Car_ISRegister)
                                {
                                    str += "该车辆通行已完成";
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(StrategyIDS))
                                {
                                    if (StrategyS.IndexOf(',') < 0)
                                    {
                                        strategysin = StrategyS;
                                    }
                                    else
                                    {
                                        strategysin = StrategyS.Substring(0, StrategyS.IndexOf(','));

                                    }
                                }
                                else
                                {
                                    int i = StrategyIDS.LastIndexOf(',');
                                    if (i < 0)
                                    {
                                        string sy = StrategyS.Substring(StrategyS.IndexOf(',') + 1);
                                        int ii = sy.IndexOf(',');
                                        if (ii < 0)
                                        {
                                            strategysin = sy;
                                        }
                                        else
                                        {
                                            strategysin = sy.Substring(0, ii);
                                        }
                                    }
                                    else
                                    {
                                        string s = StrategyIDS.Substring(StrategyIDS.LastIndexOf(',') + 1);
                                        int ii = StrategyS.IndexOf(s);
                                        if (ii < 0)
                                        {
                                            strategysin = StrategyS.Substring(0, StrategyS.IndexOf(','));
                                        }
                                        else
                                        {
                                            strategysin = StrategyS.Substring(StrategyS.IndexOf(s));
                                            strategysin = strategysin.Substring(strategysin.IndexOf(',') + 1);
                                            if (strategysin == strategysout)
                                            {
                                                strategysin = strategysout;
                                            }
                                            else
                                            {
                                                strategysin = strategysin.Substring(0, strategysin.IndexOf(','));
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion

                            DataTable dtcarinout;
                            if (isupdate)
                            {
                                dtcarinout = LinQBaseDao.Query(" select DrivewayStrategyRecord_Driveway_ID from  DrivewayStrategyRecord where DrivewayStrategyRecord_ID =" + strategysin + " and DrivewayStrategyRecord_CarInfo_ID=" + carinfoid + "  and DrivewayStrategyRecord_State='启动'").Tables[0];
                            }
                            else
                            {
                                dtcarinout = LinQBaseDao.Query("select DrivewayStrategy_Driveway_ID from DrivewayStrategy where DrivewayStrategy_ID =" + strategysin + "  and DrivewayStrategy_State='启动' and DrivewayStrategy_Name ='" + Strategy_DriSName + "'").Tables[0];
                            }

                            //通行策略是否正确
                            if (dtcarinout.Rows.Count > 0)
                            {
                                if (strSort == "有序")
                                {
                                    if (dtcarinout.Rows[0][0].ToString() != driid)
                                    {
                                        return str += "通行错误！";
                                    }
                                    CommonalityEntity.DrivewayStrategy_ID = Convert.ToInt32(strategysin);
                                }
                            }
                            else
                            {
                                return str += "通行错误！";
                            }

                            #region  根据是否修改了通行策略查询正确通行策略
                            if (isupdate)
                            {
                                if (!string.IsNullOrEmpty(StrategyIDS))
                                {
                                    dtcarinout = LinQBaseDao.Query(" select DrivewayStrategyRecord_ID,DrivewayStrategyRecord_Driveway_ID,DrivewayStrategyRecord_Name from  DrivewayStrategyRecord where DrivewayStrategyRecord_ID not in(" + StrategyIDS + ") and DrivewayStrategyRecord_CarInfo_ID=" + carinfoid + " DrivewayStrategyRecord_State='启动' order by DrivewayStrategyRecord_Sort ").Tables[0];
                                }
                                else
                                {
                                    dtcarinout = LinQBaseDao.Query(" select DrivewayStrategyRecord_ID,DrivewayStrategyRecord_Driveway_ID,DrivewayStrategyRecord_Name from  DrivewayStrategyRecord where DrivewayStrategyRecord_CarInfo_ID=" + carinfoid + "  and DrivewayStrategyRecord_State='启动' order by DrivewayStrategyRecord_Sort ").Tables[0];
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(StrategyIDS))
                                {
                                    dtcarinout = LinQBaseDao.Query("select DrivewayStrategy_ID,DrivewayStrategy_Driveway_ID,DrivewayStrategy_Name from DrivewayStrategy where DrivewayStrategy_ID not in (" + StrategyIDS + ")  and DrivewayStrategy_Name ='" + Strategy_DriSName + "' and DrivewayStrategy_State='启动' and DrivewayStrategy_Driveway_ID=" + driid + " order by DrivewayStrategy_Sort ").Tables[0];
                                }
                                else
                                {
                                    dtcarinout = LinQBaseDao.Query("select DrivewayStrategy_ID,DrivewayStrategy_Driveway_ID,DrivewayStrategy_Name from DrivewayStrategy where  DrivewayStrategy_Name ='" + Strategy_DriSName + "' and DrivewayStrategy_Driveway_ID=" + driid + " and DrivewayStrategy_State='启动'  order by DrivewayStrategy_Sort ").Tables[0];
                                }
                            }
                            #endregion

                            //查出当前通行策略对应的通道
                            string Driveway_ID = "";//应该使用的通道ID
                            if (dtcarinout.Rows.Count > 0)
                            {
                                if (isupdate)
                                {
                                    Driveway_ID = dtcarinout.Rows[0]["DrivewayStrategyRecord_Driveway_ID"].ToString();
                                }
                                else
                                {
                                    Driveway_ID = dtcarinout.Rows[0]["DrivewayStrategy_Driveway_ID"].ToString();
                                }
                            }
                            else
                            {
                                if (strSort == "无序" && isr)
                                {
                                    return str += strDir(cartypeid, driid, false, carno);
                                }
                                else
                                {
                                    return str += "通行错误！";
                                }
                            }


                            #region 同时是否有序并是否修改通行策略校验通行是否正确
                            if (strSort == "有序")
                            {
                                if (isupdate)
                                {
                                    if (dtcarinout.Rows.Count > 0)
                                    {
                                        if (Driveway_ID != driid)
                                        {
                                            str += "通行错误！";
                                        }
                                    }
                                    else
                                    {
                                        str += "通行错误！";
                                    }
                                }
                                else
                                {
                                    if (dtcarinout.Rows.Count > 0)
                                    {
                                        if (Driveway_ID != driid)
                                        {
                                            str += "通行错误！";
                                        }
                                    }
                                    else
                                    {
                                        str += "通行错误！";

                                    }
                                }
                            }
                            else
                            {
                                if (isupdate)
                                {
                                    if (dtcarinout.Rows.Count > 0)
                                    {
                                        CommonalityEntity.DrivewayStrategy_ID = Convert.ToInt32(dtcarinout.Rows[0]["DrivewayStrategyRecord_ID"].ToString());
                                        if (Driveway_ID != driid)
                                        {
                                            str += "通行错误！";
                                        }
                                    }
                                    else
                                    {
                                        str += strDir(carinid, driid, true, carno);
                                    }
                                }
                                else
                                {
                                    if (dtcarinout.Rows.Count > 0)
                                    {
                                        CommonalityEntity.DrivewayStrategy_ID = Convert.ToInt32(dtcarinout.Rows[0]["DrivewayStrategy_ID"].ToString());
                                        if (Driveway_ID != driid)
                                        {
                                            str += "通行错误！";
                                        }
                                    }
                                    else
                                    {
                                        str += strDir(cartypeid, driid, false, carno);
                                    }
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            if (!isr)
                            {
                                str += "该车辆还没有登记！";
                            }
                            else
                            {
                                if (isupdate)
                                {
                                    str += strDir(carinid, driid, true, carno);
                                }
                                else
                                {
                                    str += strDir(cartypeid, driid, false, carno);
                                }
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        str += "该车辆已暂停或注销！";
                    }
                }
                else
                {
                    if (CommonalityEntity.Car_ISRegister)
                    {
                        return str;
                    }
                    else
                    {
                        str += "该车辆还没有登记！";
                    }
                }
                #endregion
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInOutForm.IntionICCarprint异常:");
            }
            return str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctyid"></param>
        /// <param name="dwid"></param>
        /// <param name="isu"></param>
        /// <returns></returns>
        private string strDir(string ctyid, string dwid, bool isu, string carno)
        {
            string str = "";
            if (isu)
            {
                DataTable dtca = LinQBaseDao.Query(" select DrivewayStrategyRecord_ID,DrivewayStrategyRecord_Driveway_ID,DrivewayStrategyRecord_Name from  DrivewayStrategyRecord where DrivewayStrategyRecord_CarInfo_ID=" + ctyid + " and DrivewayStrategyRecord_State='启动' and DrivewayStrategyRecord_Driveway_ID=" + dwid + " order by DrivewayStrategyRecord_Sort ").Tables[0];
                if (dtca.Rows.Count <= 0)
                {
                    str += "通行错误！";
                }
            }
            else
            {
                object obj = LinQBaseDao.GetSingle("select CarType_Name from CarType where CarType_ID=" + ctyid);
                if (obj != null)
                {
                    if (obj.ToString() == "内部废纸车" || obj.ToString() == "内部成品车")
                    {
                        object objn = LinQBaseDao.GetSingle("select Car_CarType_ID from View_Car_ICard_CarType where Car_Name='" + carno + "'");
                        if (objn != null)
                        {
                            if (Strategy_DriSName == "内部废纸车策略" || Strategy_DriSName == "内部成品车策略")
                            {
                                DataTable dtca = LinQBaseDao.Query("select DrivewayStrategy_ID,DrivewayStrategy_Driveway_ID,DrivewayStrategy_Name from DrivewayStrategy where DrivewayStrategy_Name ='内部车辆策略' and DrivewayStrategy_Driveway_ID=" + dwid + " and DrivewayStrategy_State='启动'  order by DrivewayStrategy_Sort ").Tables[0];
                                if (dtca.Rows.Count <= 0)
                                {
                                    str += "通行错误！";
                                }
                            }
                            else
                            {
                                DataTable dtca = LinQBaseDao.Query("select DrivewayStrategy_ID,DrivewayStrategy_Driveway_ID,DrivewayStrategy_Name from DrivewayStrategy where DrivewayStrategy_Name ='" + Strategy_DriSName + "' and DrivewayStrategy_Driveway_ID=" + dwid + " and DrivewayStrategy_State='启动'  order by DrivewayStrategy_Sort ").Tables[0];
                                if (dtca.Rows.Count <= 0)
                                {
                                    str += "通行错误！";
                                }
                            }
                        }
                    }
                    else
                    {
                        DataTable dtca = LinQBaseDao.Query("select DrivewayStrategy_ID,DrivewayStrategy_Driveway_ID,DrivewayStrategy_Name from DrivewayStrategy where DrivewayStrategy_Name ='" + Strategy_DriSName + "' and DrivewayStrategy_Driveway_ID=" + dwid + " and DrivewayStrategy_State='启动'  order by DrivewayStrategy_Sort ").Tables[0];
                        if (dtca.Rows.Count <= 0)
                        {
                            if (!CommonalityEntity.Car_ISRegister)
                            {
                                str += "通行错误！";
                            }
                        }
                    }
                }
            }
            return str;
        }

        /// <summary>
        /// 小票进厂校验
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_check_Click(object sender, EventArgs e)
        {
            if (!IScheckSmr())
            {
                return;
            }
            string serialnumber = txt_Serialnumber.Text.Trim().Substring(0, 12);
            if (CarInformationFormOne.cinfo == null)
            {
                txt_Serialnumber.Text = "";
                intfx = 0;
                CarInformationFormOne.cinfo = new CarInformationFormOne(serialnumber, true);
                //CarInformationFormOne.cinfo.mf = this.mf;
                CarInformationFormOne.cinfo.Show();
            }
            else
            {
                if (CarInformationFormTwo.cinfo == null)
                {
                    txt_Serialnumber.Text = "";
                    intfx = 1;
                    CarInformationFormTwo.cinfo = new CarInformationFormTwo(serialnumber, true);
                    //CarInformationFormTwo.cinfo.mf = this.mf;
                    CarInformationFormTwo.cinfo.Show();
                }
                else
                {
                    if (CarInformationFormThree.cinfo == null)
                    {
                        txt_Serialnumber.Text = "";
                        intfx = 2;
                        CarInformationFormThree.cinfo = new CarInformationFormThree(serialnumber, true);
                        //CarInformationFormThree.cinfo.mf = this.mf;
                        CarInformationFormThree.cinfo.Show();
                    }
                    else
                    {
                        if (CarInformationFormFour.cinfo == null)
                        {
                            txt_Serialnumber.Text = "";
                            intfx = 3;
                            CarInformationFormFour.cinfo = new CarInformationFormFour(serialnumber, true);
                            //CarInformationFormFour.cinfo.mf = this.mf;
                            CarInformationFormFour.cinfo.Show();
                        }
                        else
                        {
                            //CarInformationFormFour.cinfo.Activate();

                            if (CarInformationFormFive.cinfo == null)
                            {
                                txt_Serialnumber.Text = "";
                                intfx = 4;
                                CarInformationFormFive.cinfo = new CarInformationFormFive(serialnumber, true);
                                //CarInformationFormFive.cinfo.mf = this.mf;
                                CarInformationFormFive.cinfo.Show();
                            }
                            else
                            {
                                if (CarInformationFormSix.cinfo == null)
                                {
                                    txt_Serialnumber.Text = "";
                                    intfx = 5;
                                    CarInformationFormSix.cinfo = new CarInformationFormSix(serialnumber, true);
                                    //CarInformationFormSix.cinfo.mf = this.mf;
                                    CarInformationFormSix.cinfo.Show();
                                }
                                else
                                {
                                    CarInformationFormSix.cinfo.Activate();
                                }
                            }
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 车辆信息
        /// </summary>
        DataTable dtviewcarstate;

        /// <summary>
        /// 小票校验是否可用
        /// </summary>
        /// <returns></returns>
        private bool IScheckSmr()
        {
            string serialnumber = txt_Serialnumber.Text.Trim();
            if (!serInCheck(serialnumber))
            {
                return false;
            }
            serialnumber = serialnumber.Substring(0, 12);
            dtviewcarstate = LinQBaseDao.Query("select * from View_CarState where SmallTicket_Serialnumber='" + serialnumber + "' order by CarInOutRecord_ID desc").Tables[0];
            CheckProperties.dt = dtviewcarstate;
            string str = ISNumberCheckOne(dtviewcarstate);
            if (!string.IsNullOrEmpty(str))
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", str, txt_Serialnumber, this);
                return false;
            }

            if (IsTongdao(CommonalityEntity.Driveway_Value))
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该车道正在放行其他车辆！", txt_Serialnumber, this);
                return false;
            }
            str = CameraCheck();
            if (!string.IsNullOrEmpty(str))
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", str, txt_Serialnumber, this);
                return false;
            }
            CommonalityEntity.CarValue.Add(CommonalityEntity.Driveway_Value);
            CommonalityEntity.ISDengji = true;
            return true;

        }
        /// <summary>
        /// 因为无序车辆进出为一次业务，所以当需要多门岗通行时需要判断通行次数
        /// </summary>
        private void wuxu(string sert)
        {
            int sn = Convert.ToInt32(LinQBaseDao.GetSingle("select COUNT(0) from DrivewayStrategy where DrivewayStrategy_Name ='" + sert + "'").ToString());
            if (sn > 2)
            {
                int coounted = Convert.ToInt32(dtviewcarstate.Rows[0]["SmallTicket_Allowcount"].ToString());
                if (coounted == 1)
                {
                    LinQBaseDao.Query("update SmallTicket set SmallTicket_Allowcount=SmallTicket_Allowcount*" + sn + "/2 where SmallTicket_ID=" + dtviewcarstate.Rows[0]["SmallTicket_ID"].ToString());
                }
            }
        }

        /// <summary>
        /// 校验当前通道是否正在放车
        /// </summary>
        /// <returns></returns>
        private bool IsTongdao(string dvalue)
        {
            bool istr = false;
            if (CommonalityEntity.CarValue.Count > 0)
            {
                foreach (var item in CommonalityEntity.CarValue)
                {
                    if (item == dvalue)
                    {
                        return istr = true;
                    }
                }
            }
            return istr = false;
        }

        private bool serInCheck(string serialnumber)
        {
            bool isserin = true;
            if (serialnumber == "")
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "小票号不能为空！", txt_Serialnumber, this);
                return isserin = false;
            }
            if (serialnumber.Length < 12)
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "小票号长度必须大于或等于12位！", txt_Serialnumber, this);
                return isserin = false;
            }
            if (serialnumber.Length > 13)
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "无效小票号！", txt_Serialnumber, this);
                return isserin = false;
            }
            serialnumber = serialnumber.Substring(0, 12);
            dtviewcarstate = LinQBaseDao.Query("select * from View_CarState where SmallTicket_Serialnumber='" + serialnumber + "' order by CarInOutRecord_ID desc").Tables[0];
            if (dtviewcarstate.Rows.Count <= 0)
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "无效小票号！", txt_Serialnumber, this);
                return isserin = false;
            }

            //带通行车辆通行策略重置为null
            string SortNumberInfo_TongXing = dtviewcarstate.Rows[0]["SortNumberInfo_TongXing"].ToString();
            string CarInOut_ID = dtviewcarstate.Rows[0]["CarInOutRecord_ID"].ToString();
            if (SortNumberInfo_TongXing == "待通行")
            {
                LinQBaseDao.Query("update CarInOutRecord set CarInOutRecord_DrivewayStrategy_IDS=null where CarInOutRecord_ID=" + CarInOut_ID);
            }

            //无序车辆判断
            wuxu(dtviewcarstate.Rows[0]["CarInOutRecord_Remark"].ToString());


            string str = Serialnumbitcheck(serialnumber);
            bool isfull = Convert.ToBoolean(dtviewcarstate.Rows[0]["CarInOutRecord_ISFulfill"].ToString());
            if (!string.IsNullOrEmpty(str))
            {
                if (str == "小票已过期!")
                {
                    if (isfull)
                    {
                        if (isSerIal(CarInOut_ID))
                        {
                            if (!string.IsNullOrEmpty(dtviewcarstate.Rows[0]["CarInOutRecord_DrivewayStrategy_IDS"].ToString()))
                            {
                                if (dtviewcarstate.Rows[0]["CarInOutRecord_DrivewayStrategy_IDS"].ToString().Length >= dtviewcarstate.Rows[0]["CarInOutRecord_DrivewayStrategyS"].ToString().Length)
                                {
                                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", str, txt_Serialnumber, this);
                                    return isserin = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (dtviewcarstate.Rows[0]["SortNumberInfo_TongXing"].ToString() != "已进厂")
                        {
                            if (MessageBox.Show("小票已过期，是否继续放行", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                            {
                                return isserin = false;
                            }
                        }
                    }
                }
                else
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", str, txt_Serialnumber, this);
                    return isserin = false;
                }
            }
            else
            {
                if (isfull)
                {
                    if (isSerIal(CarInOut_ID))
                    {
                        PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", str, txt_Serialnumber, this);
                        return isserin = false;
                    }
                }
            }
            return isserin;
        }
        /// <summary>
        /// 警告放行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_outSumbit_Click(object sender, EventArgs e)
        {
            if (!IScheckSmr())
            {
                return;
            }
            string serialnumber = txt_Serialnumber.Text.Trim().Substring(0, 12);
            if (!blacktnClick(carName))
            {
                return;
            }
            if (CarInformationFormOne.cinfo == null)
            {
                txt_Serialnumber.Text = "";
                intfx = 0;
                CarInformationFormOne.cinfo = new CarInformationFormOne(serialnumber, true);
                //CarInformationFormOne.cinfo.mf = this.mf;
                CarInformationFormOne.cinfo.Show();
            }
            else
            {
                if (CarInformationFormTwo.cinfo == null)
                {
                    txt_Serialnumber.Text = "";
                    intfx = 1;
                    CarInformationFormTwo.cinfo = new CarInformationFormTwo(serialnumber, true);
                    //CarInformationFormTwo.cinfo.mf = this.mf;
                    CarInformationFormTwo.cinfo.Show();
                }
                else
                {
                    if (CarInformationFormThree.cinfo == null)
                    {
                        txt_Serialnumber.Text = "";
                        intfx = 2;
                        CarInformationFormThree.cinfo = new CarInformationFormThree(serialnumber, true);
                        //CarInformationFormThree.cinfo.mf = this.mf;
                        CarInformationFormThree.cinfo.Show();
                    }
                    else
                    {
                        if (CarInformationFormFour.cinfo == null)
                        {
                            txt_Serialnumber.Text = "";
                            intfx = 3;
                            CarInformationFormFour.cinfo = new CarInformationFormFour(serialnumber, true);
                            //CarInformationFormFour.cinfo.mf = this.mf;
                            CarInformationFormFour.cinfo.Show();
                        }
                        else
                        {
                            if (CarInformationFormFive.cinfo == null)
                            {
                                txt_Serialnumber.Text = "";
                                intfx = 4;
                                CarInformationFormFive.cinfo = new CarInformationFormFive(serialnumber, true);
                                //CarInformationFormFive.cinfo.mf = this.mf;
                                CarInformationFormFive.cinfo.Show();
                            }
                            else
                            {
                                if (CarInformationFormSix.cinfo == null)
                                {
                                    txt_Serialnumber.Text = "";
                                    intfx = 5;
                                    CarInformationFormSix.cinfo = new CarInformationFormSix(serialnumber, true);
                                    //CarInformationFormSix.cinfo.mf = this.mf;
                                    CarInformationFormSix.cinfo.Show();
                                }
                                else
                                {
                                    CarInformationFormSix.cinfo.Activate();
                                }
                            }
                        }
                    }
                }
            }
        }





        /// <summary>
        /// 地感校验(调用前，刷卡的时候，
        /// 需要得到设备管理表中的刷卡通道赋值给
        /// CommonalityEntity.Driveway_Value)
        /// 刷小票的时候，根据小票获取小票对应
        /// 通行策略的通道值赋值
        /// </summary>
        private string CameraCheck()
        {
            string strbox = "";
            try
            {
                string strwhe = "";
                if (CommonalityEntity.IsUpdatedri)
                {
                    if (CommonalityEntity.ISInOut)
                    {
                        string sqlMan = "select count(*) from  ManagementStrategyRecord where ManagementStrategyRecord_CarInfo_ID=" + carinid + " and ManagementStrategyRecord_ControlInfo_ID in(select ControlInfo_ID from ControlInfo where ControlInfo_Name='进门地感校验') and ManagementStrategyRecord_State='启动' ";
                        int dtMan = Convert.ToInt32(LinQBaseDao.GetSingle(sqlMan).ToString());
                        if (dtMan == 0)
                        {
                            return strwhe;
                        }
                    }
                    else
                    {
                        string sqlMan = "select count(*) from  ManagementStrategyRecord where ManagementStrategyRecord_CarInfo_ID=" + carinid + " and ManagementStrategyRecord_ControlInfo_ID in(select ControlInfo_ID from ControlInfo where ControlInfo_Name='出门地感校验') and ManagementStrategyRecord_State='启动' ";
                        int dtMan = Convert.ToInt32(LinQBaseDao.GetSingle(sqlMan).ToString());
                        if (dtMan == 0)
                        {
                            return strwhe;
                        }
                    }
                }
                else
                {
                    if (CommonalityEntity.ISInOut)
                    {
                        string sqlMan = "select count(*) from  ManagementStrategy where ManagementStrategy_DriSName ='" + Strategy_DriSName + "' and ManagementStrategy_ControlInfo_ID in(select ControlInfo_ID from ControlInfo where ControlInfo_Name='进门地感校验') and ManagementStrategy_State='启动' ";
                        int dtMan = Convert.ToInt32(LinQBaseDao.GetSingle(sqlMan).ToString());
                        if (dtMan == 0)
                        {
                            return strwhe;
                        }
                    }
                    else
                    {
                        string sqlMan = "select count(*) from  ManagementStrategy where ManagementStrategy_DriSName ='" + Strategy_DriSName + "' and ManagementStrategy_ControlInfo_ID in(select ControlInfo_ID from ControlInfo where ControlInfo_Name='出门地感校验') and ManagementStrategy_State='启动' ";
                        int dtMan = Convert.ToInt32(LinQBaseDao.GetSingle(sqlMan).ToString());
                        if (dtMan == 0)
                        {
                            return strwhe;
                        }
                    }
                }

                string strsql = String.Format("select * from DeviceControl where DeviceControl_positionValue='" + SystemClass.PosistionValue + "' and DeviceControl_DrivewayValue='" + CommonalityEntity.Driveway_Value + "'");
                List<DeviceControl> list = LinQBaseDao.GetItemsForListing<DeviceControl>(strsql).ToList();
                if (list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        if (item.DeviceControl_FanSate != "1")
                        {
                            strbox = "地感状态不对，请检查！";
                        }
                    }
                }
                else
                {
                    strbox = "无地感记录，请检查！";
                }
                return strbox;
            }
            catch
            {
                strbox = "校验地感异常，请检查！";
                CommonalityEntity.WriteTextLog("CameraCheck()");
            }
            return strbox;
        }

        /// <summary>
        /// 查看小票是否过期
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string Serialnumbitcheck(string strser)
        {
            string str = "";
            if (dtviewcarstate.Rows[0]["SmallTicket_State"].ToString() != "有效")
            {
                return str = "无效小票号！";
            }
            if (!string.IsNullOrEmpty(dtviewcarstate.Rows[0]["SmallTicket_Allowcount"].ToString()))
            {
                if (Convert.ToInt32(dtviewcarstate.Rows[0]["SmallTicket_Allowcount"].ToString()) <= Convert.ToInt32(dtviewcarstate.Rows[0]["SmallTicket_Allowcounted"].ToString()))
                {
                    return str = "小票已过期!";
                }
            }
            if (!string.IsNullOrEmpty(dtviewcarstate.Rows[0]["SmallTicket_Allowhour"].ToString()))
            {
                if (Convert.ToDateTime(dtviewcarstate.Rows[0]["SmallTicket_Time"].ToString()).AddHours(Convert.ToInt32(dtviewcarstate.Rows[0]["SmallTicket_Allowhour"].ToString())) < CommonalityEntity.GetServersTime())
                {
                    return str = "小票已过期!";
                }
            }
            string strstate = dtviewcarstate.Rows[0]["CarInfo_State"].ToString();
            if (strstate != "启动")
            {
                return str = "车辆已暂停或注销";
            }
            return str;
        }
        /// <summary>
        /// 查看小票是否可以多次进出厂
        /// </summary>
        /// <returns></returns>
        private bool isSerIal(string carid)
        {
            //验证是否使用次数或时间过期
            bool isf = true;

            int SmallTicket_Allowcount = 0;
            int SmallTicket_Allowcounted = 0;
            int SmallTicket_Allowhour = 0;
            DateTime SmallTicket_Time = DateTime.Now;
            if (!string.IsNullOrEmpty(dtviewcarstate.Rows[0]["SmallTicket_Allowcount"].ToString()))
            {
                SmallTicket_Allowcount = Convert.ToInt32(dtviewcarstate.Rows[0]["SmallTicket_Allowcount"]);
            }
            if (!string.IsNullOrEmpty(dtviewcarstate.Rows[0]["SmallTicket_Allowcounted"].ToString()))
            {
                SmallTicket_Allowcounted = Convert.ToInt32(dtviewcarstate.Rows[0]["SmallTicket_Allowcounted"]);
            }

            if (!string.IsNullOrEmpty(dtviewcarstate.Rows[0]["SmallTicket_Allowhour"].ToString()))
            {
                SmallTicket_Allowhour = Convert.ToInt32(dtviewcarstate.Rows[0]["SmallTicket_Allowhour"]);
            }

            if (!string.IsNullOrEmpty(dtviewcarstate.Rows[0]["SmallTicket_Time"].ToString()))
            {
                SmallTicket_Time = Convert.ToDateTime(dtviewcarstate.Rows[0]["SmallTicket_Time"]);
            }

            if (SmallTicket_Allowcount > SmallTicket_Allowcounted || SmallTicket_Time.AddHours(SmallTicket_Allowhour) < DateTime.Now)
            {
                LinQBaseDao.Query("update CarInOutRecord set CarInOutRecord_FulfillTime = null,CarInOutRecord_ISFulfill=0,CarInOutRecord_DrivewayStrategy_IDS=null where CarInOutRecord_ID=" + carid);
                isf = false;
            }
            return isf;
        }



        /// <summary>
        /// 依据小票查看通行策略
        /// </summary>
        /// <param name="number">小票</param>
        /// <param name="driid"></param>
        /// <returns></returns>
        private string ISNumberCheckOne(DataTable dtcarstate)
        {
            string str = "";
            string strsql = "";
            if (dtcarstate.Rows.Count > 0)
            {
                Strategy_DriSName = dtcarstate.Rows[0]["CarInOutRecord_Remark"].ToString();//通行策略名称
                string carinfoid = dtcarstate.Rows[0]["CarInfo_ID"].ToString();//车辆信息编号
                carinid = carinfoid;
                carName = dtcarstate.Rows[0]["CarInfo_Name"].ToString();
                cartypeid = dtcarstate.Rows[0]["CarInfo_CarType_ID"].ToString();
                bool isful = Convert.ToBoolean(dtcarstate.Rows[0]["CarInOutRecord_ISFulfill"].ToString());//通行是否完成
                string strSort = dtcarstate.Rows[0]["CarInOutRecord_Sort"].ToString();//通行顺序：有序、无序
                string StrategyS = dtcarstate.Rows[0]["CarInOutRecord_DrivewayStrategyS"].ToString();//通行策略编号组合
                string StrategyIDS = dtcarstate.Rows[0]["CarInOutRecord_DrivewayStrategy_IDS"].ToString();//已通行策略编号组合
                bool isupdate = Convert.ToBoolean(dtcarstate.Rows[0]["CarInOutRecord_Update"].ToString());//是否修改通行策略
                DataTable dtstacar = LinQBaseDao.Query("select Car_CarType_ID,Car_ISRegister from Car where Car_Name='" + carName + "'").Tables[0];
                if (dtstacar.Rows.Count > 0)
                {
                    CommonalityEntity.Car_ISRegister = Convert.ToBoolean(dtstacar.Rows[0][1].ToString());
                }
                if (strSort == "无序")
                {
                    isful = false;
                }
                if (!isful)
                {
                    string strategysout = StrategyS.Substring(StrategyS.LastIndexOf(',') + 1);//车辆最后通行策略，如果等于则需出门授权
                    string strategysin = "";//当前应该通行策略编号
                    #region 当前通行策略
                    if (StrategyS == StrategyIDS && !string.IsNullOrEmpty(StrategyS))
                    {
                        if (!CommonalityEntity.Car_ISRegister)
                        {
                            return str = "该车辆通行已完成";
                        }
                    }
                    else
                    {
                        # region 得到当前车辆该通行的策略通道
                        if (string.IsNullOrEmpty(StrategyIDS))
                        {
                            if (StrategyS.IndexOf(',') < 0)
                            {
                                strategysin = StrategyS;

                            }
                            else
                            {
                                strategysin = StrategyS.Substring(0, StrategyS.IndexOf(','));

                            }
                        }
                        else
                        {
                            int i = StrategyIDS.LastIndexOf(',');
                            if (i < 0)
                            {
                                string sy = StrategyS.Substring(StrategyS.IndexOf(',') + 1);
                                int ii = sy.IndexOf(',');
                                if (ii < 0)
                                {
                                    strategysin = sy;
                                }
                                else
                                {
                                    strategysin = sy.Substring(0, ii);
                                }
                            }
                            else
                            {
                                string s = StrategyIDS.Substring(StrategyIDS.LastIndexOf(',') + 1);
                                int ii = StrategyS.IndexOf(s);
                                if (ii < 0)
                                {
                                    strategysin = StrategyS.Substring(0, StrategyS.IndexOf(','));
                                }
                                else
                                {
                                    strategysin = StrategyS.Substring(StrategyS.IndexOf(s));
                                    strategysin = strategysin.Substring(strategysin.IndexOf(',') + 1);
                                    if (strategysin == strategysout)
                                    {
                                        strategysin = strategysout;
                                    }
                                    else
                                    {
                                        strategysin = strategysin.Substring(0, strategysin.IndexOf(','));
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion

                    if (isupdate)
                    {
                        if (strSort == "无序")
                        {
                            strsql = "select DrivewayStrategyRecord_Driveway_ID from  DrivewayStrategyRecord where  DrivewayStrategyRecord_State='启动'  and DrivewayStrategyRecord_CarInfo_ID=" + carinfoid + " and DrivewayStrategyRecord_Driveway_ID=" + cmbDiv.SelectedValue.ToString();
                        }
                        else
                        {
                            strsql = "select DrivewayStrategyRecord_Driveway_ID from  DrivewayStrategyRecord where DrivewayStrategyRecord_ID =" + strategysin + " and DrivewayStrategyRecord_State='启动'  and DrivewayStrategyRecord_CarInfo_ID=" + carinfoid;
                        }
                    }
                    else
                    {
                        if (strSort == "无序")
                        {
                            strsql = "select DrivewayStrategy_Driveway_ID from DrivewayStrategy where DrivewayStrategy_Name='" + Strategy_DriSName + "' and DrivewayStrategy_State='启动'  and DrivewayStrategy_Driveway_ID=" + cmbDiv.SelectedValue.ToString();
                        }
                        else
                        {
                            strsql = "select DrivewayStrategy_Driveway_ID from DrivewayStrategy where DrivewayStrategy_Name='" + Strategy_DriSName + "' and DrivewayStrategy_State='启动' and DrivewayStrategy_ID=" + strategysin + " order by DrivewayStrategy_Sort ";
                        }
                    }
                    DataTable dtdrisort = LinQBaseDao.Query(strsql).Tables[0];
                    if (dtdrisort.Rows.Count > 0)
                    {
                        string drivewayid = Dirway(dtdrisort.Rows[0][0].ToString());
                        CommonalityEntity.DrivewayStrategy_ID = Convert.ToInt32(strategysin);
                        strsql = "select * from Driveway where Driveway_Position_ID='" + SystemClass.PositionID + "' and Driveway_ID=" + drivewayid;
                        DataTable dtDriveway = LinQBaseDao.Query(strsql).Tables[0];
                        if (dtDriveway.Rows.Count > 0)
                        {
                            if (dtDriveway.Rows[0]["Driveway_State"].ToString() != "启动")
                            {
                                return str = dtDriveway.Rows[0]["Driveway_Name"].ToString() + "状态为：" + dtDriveway.Rows[0]["Driveway_State"].ToString();
                            }
                            CommonalityEntity.Driveway_ID = Convert.ToInt32(dtDriveway.Rows[0]["Driveway_ID"].ToString());
                            CommonalityEntity.Driveway_Name = dtDriveway.Rows[0]["Driveway_Name"].ToString();
                            CommonalityEntity.Driveway_Value = dtDriveway.Rows[0]["Driveway_Value"].ToString();
                            SystemClass.DrivewayValue = CommonalityEntity.Driveway_Value;
                            if (dtDriveway.Rows[0]["Driveway_Type"].ToString() == "进")
                            {
                                CommonalityEntity.ISInOut = true;
                            }
                            else
                            {
                                CommonalityEntity.ISInOut = false;
                            }
                        }
                        else
                        {
                            return str = "通行错误!";
                        }
                    }
                    else
                    {
                        return str = "通行错误!";
                    }
                }
                else
                {
                    return str = "通行错误!";
                }
            }
            return str;

        }

        /// <summary>
        /// 备用通道是否启用
        /// </summary>
        /// <param name="dirid"></param>
        /// <returns></returns>
        private string Dirway(string dirid)
        {
            string drivewayid = "";
            string strsql = "select Driveway_WarrantyState,Driveway_Remark_Driveway_ID from Driveway where Driveway_ID=" + dirid;
            DataTable dtWarrantyState = LinQBaseDao.Query(strsql).Tables[0];
            if (dtWarrantyState.Rows[0][0].ToString() == "报修")
            {
                CommonalityEntity.isDriState = true;
                CommonalityEntity.DriWarrantyID = dtWarrantyState.Rows[0][1].ToString();
                drivewayid = CommonalityEntity.DriWarrantyID;
            }
            else
            {
                drivewayid = dirid;
                CommonalityEntity.isDriState = false;
            }
            return drivewayid;
        }

        # region IC卡校验放行时取值校验
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="fx">放行的页面</param>
        /// <param name="btag">点击的控件</param>
        /// <param name="cont">放行后清空的通道</param>
        /// <param name="IC1">车卡</param>
        /// <param name="IC2">人卡</param>
        private void CarInFrom(object sender, EventArgs e, int fx, int btag, int cont, string IC1, string IC2, bool isback)
        {
            CommonalityEntity.ICC1 = IC1;
            CommonalityEntity.ICC2 = IC2;
            CommonalityEntity.contolint = cont;
            intfx = fx;
            (sender as Button).Tag = btag;
            if (isback)
            {
                string strCarNum = "";
                if (btag == 1)
                {
                    strCarNum = cmbCarNum1.Text;
                }
                else if (btag == 2)
                {
                    strCarNum = cmbCarNum2.Text;
                }
                else if (btag == 3)
                {
                    strCarNum = cmbCarNum3.Text;
                }
                else if (btag == 4)
                {
                    strCarNum = cmbCarNum4.Text;
                }
                else if (btag == 5)
                {
                    strCarNum = cmbCarNum5.Text;
                }
                else if (btag == 6)
                {
                    strCarNum = cmbCarNum6.Text;
                }
                else if (btag == 7)
                {
                    strCarNum = cmbCarNum7.Text;
                }
                else
                {
                    strCarNum = cmbCarNum8.Text;
                }
                if (string.IsNullOrEmpty(strCarNum))
                {
                    PromptInfo("没有车辆信息", btag);
                    return;
                }
                if (!blacktnClick(strCarNum))
                {
                    return;
                }
            }
            Btn_Click(sender, e, btag);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="cont"></param>
        /// <param name="buttag"></param>
        /// <param name="isback">是否黑名单放行</param>
        private void CheckFrom(object sender, EventArgs e, int cont, int buttag, bool isback)
        {
            string stric1 = "";
            string stric2 = "";
            if (buttag == 1)
            {
                stric1 = txtCar1.Text;
                stric2 = txtUser1.Text;
            }
            else if (buttag == 2)
            {
                stric1 = txtCar2.Text;
                stric2 = txtUser2.Text;
            }
            else if (buttag == 3)
            {
                stric1 = txtCar3.Text;
                stric2 = txtUser3.Text;
            }
            else if (buttag == 4)
            {
                stric1 = txtCar4.Text;
                stric2 = txtUser4.Text;
            }
            else if (buttag == 5)
            {
                stric1 = txtCar5.Text;
                stric2 = txtUser5.Text;
            }
            else if (buttag == 6)
            {
                stric1 = txtCar6.Text;
                stric2 = txtUser6.Text;
            }
            else if (buttag == 7)
            {
                stric1 = txtCar7.Text;
                stric2 = txtUser7.Text;
            }
            else
            {
                stric1 = txtCar8.Text;
                stric2 = txtUser8.Text;
            }

            if (CarInformationFormOne.cinfo == null)
            {
                CarInFrom(sender, e, 0, cont, buttag, stric1, stric2, isback);
            }
            else
            {
                if (CarInformationFormTwo.cinfo == null)
                {
                    CarInFrom(sender, e, 1, cont, buttag, stric1, stric2, isback);
                }
                else
                {
                    if (CarInformationFormThree.cinfo == null)
                    {
                        CarInFrom(sender, e, 2, cont, buttag, stric1, stric2, isback);
                    }
                    else
                    {
                        if (CarInformationFormFour.cinfo == null)
                        {
                            CarInFrom(sender, e, 3, cont, buttag, stric1, stric2, isback);
                        }
                        else
                        {
                            // CarInformationFormFour.cinfo.Activate();
                            if (CarInformationFormFive.cinfo == null)
                            {
                                CarInFrom(sender, e, 4, cont, buttag, stric1, stric2, isback);
                            }
                            else
                            {
                                if (CarInformationFormSix.cinfo == null)
                                {
                                    CarInFrom(sender, e, 5, cont, buttag, stric1, stric2, isback);
                                }
                                else
                                {
                                    CarInformationFormSix.cinfo.Activate();
                                }
                            }
                        }
                    }
                }
            }
        }

        # endregion

        #region 校验放行
        /// <summary>
        /// 通道一校验放行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInout1_Click(object sender, EventArgs e)
        {
            CheckFrom(sender, e, 1, 1, false);
        }
        private void btnInout2_Click(object sender, EventArgs e)
        {
            CheckFrom(sender, e, 2, 2, false);
        }
        private void btnInout3_Click(object sender, EventArgs e)
        {
            CheckFrom(sender, e, 3, 3, false);
        }
        private void btnInout4_Click(object sender, EventArgs e)
        {
            CheckFrom(sender, e, 4, 4, false);
        }
        private void btnInout5_Click(object sender, EventArgs e)
        {
            CheckFrom(sender, e, 5, 5, false);
        }
        private void btnInout6_Click(object sender, EventArgs e)
        {
            CheckFrom(sender, e, 6, 6, false);
        }
        private void btnInout7_Click(object sender, EventArgs e)
        {
            CheckFrom(sender, e, 7, 7, false);
        }
        private void btnInout8_Click(object sender, EventArgs e)
        {
            CheckFrom(sender, e, 8, 8, false);
        }
        #endregion


        #region 黑名单校验放行
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWarn1_Click(object sender, EventArgs e)
        {
            CheckFrom(sender, e, 1, 1, true);
        }

        private void btnWarn2_Click(object sender, EventArgs e)
        {
            CheckFrom(sender, e, 2, 2, true);
        }

        private void btnWarn3_Click(object sender, EventArgs e)
        {
            CheckFrom(sender, e, 3, 3, true);
        }

        private void btnWarn4_Click(object sender, EventArgs e)
        {
            CheckFrom(sender, e, 4, 4, true);
        }

        private void btnWarn5_Click(object sender, EventArgs e)
        {
            CheckFrom(sender, e, 5, 5, true);
        }

        private void btnWarn6_Click(object sender, EventArgs e)
        {
            CheckFrom(sender, e, 6, 6, true);
        }

        private void btnWarn7_Click(object sender, EventArgs e)
        {
            CheckFrom(sender, e, 7, 7, true);
        }

        private void btnWarn8_Click(object sender, EventArgs e)
        {
            CheckFrom(sender, e, 8, 8, true);
        }
        #endregion

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="str"></param>
        /// <param name="num"></param>
        private void PromptInfo(string str, int num)
        {
            if (num == 1)
            {
                lbPrompt1.Text = str;
            }
            else if (num == 2)
            {
                lbPrompt2.Text = str;
            }
            else if (num == 3)
            {
                lbPrompt3.Text = str;
            }
            else if (num == 4)
            {
                lbPrompt4.Text = str;
            }
            else if (num == 5)
            {
                lbPrompt5.Text = str;
            }
            else if (num == 6)
            {
                lbPrompt6.Text = str;
            }
            else if (num == 7)
            {
                lbPrompt7.Text = str;
            }
            else if (num == 8)
            {
                lbPrompt8.Text = str;
            }
        }

        /// <summary>
        /// 验证IC卡是否有效
        /// </summary>
        private string ICCardIsValid()
        {
            string str = "";
            try
            {
                DataTable carDT = LinQBaseDao.Query("select ICCard_ID, ICCard_EffectiveType, ICCard_BeginTime,ICCard_EndTime,ICCard_count,ICCard_HasCount ,ICCard_State,Car_Name,Car_State  from View_Car_ICard_CarType   where ICCard_Value='" + CommonalityEntity.ICC1 + "' ").Tables[0];
                DataTable StaffInfoDT = LinQBaseDao.Query("select StaffInfo_ID,StaffInfo_Name, StaffInfo_Identity,StaffInfo_Phone, ICCard_ID, ICCard_EffectiveType, ICCard_BeginTime,ICCard_EndTime,ICCard_count,ICCard_HasCount,ICCard_State,StaffInfo_State   from View_StaffInfo_ICCard where  ICCard_Value='" + CommonalityEntity.ICC2 + "'").Tables[0];

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
                        return str = "IC卡状态为：" + ICCard_State;
                    }
                    if (Car_State != "启动")
                    {
                        return str = "车辆状态为：" + Car_State;
                    }
                    if (ICCard_EffectiveType == "次数")
                    {
                        if (string.IsNullOrEmpty(ICCard_HasCount))
                        {
                            if (ICCard_count == "0" || string.IsNullOrEmpty(ICCard_count))
                            {
                                return str = "车卡已过期";
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(ICCard_count))
                            {
                                int ct = Convert.ToInt32(ICCard_count);
                                int ht = Convert.ToInt32(ICCard_HasCount);
                                if (ct - ht <= 0)
                                {
                                    return str = "车卡已过期";
                                }
                            }
                            else
                            {
                                return str = "车卡已过期";
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
                        }
                        else
                        {
                            return str = "车卡已过期";
                        }
                    }
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
                        return str = "IC卡状态为：" + ICCard_State;
                    }
                    if (StaffInfo_State != "启动")
                    {
                        return str = "驾驶员状态为：" + StaffInfo_State;
                    }

                    if (ICCard_EffectiveType == "次数")
                    {
                        if (string.IsNullOrEmpty(ICCard_HasCount))
                        {
                            if (ICCard_count == "0" || string.IsNullOrEmpty(ICCard_count))
                            {
                                return str = "人卡已过期";
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(ICCard_count))
                            {
                                int ct = Convert.ToInt32(ICCard_count);
                                int ht = Convert.ToInt32(ICCard_HasCount);
                                if (ct - ht <= 0)
                                {
                                    return str = "人卡已过期";
                                }
                            }
                            else
                            {
                                return str = "人卡已过期";
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
                        }
                        else
                        {
                            return str = "人卡已过期";
                        }
                    }
                }
                #endregion
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInOutForm. ICCardIsValid()异常：");
            }
            return str;
        }

        private string CarCheckIC(string IC1, string IC2, string wayname)
        {
            string str = "";

            bool rbool = false;
            try
            {
                string[] list = new string[] { IC1, IC2 };
                foreach (var itic in list)
                {
                    string strsql = "select ICCard_Permissions from ICCard where ICCard_Value='" + itic + "' and ICCard_State='启动'";

                    DataTable dtstrsql = LinQBaseDao.Query(strsql).Tables[0];
                    if (dtstrsql.Rows.Count <= 0)
                    {
                        return str = "卡" + itic + "没有通行此通道权限";
                    }
                    else
                    {
                        string strs = dtstrsql.Rows[0]["ICCard_Permissions"].ToString();

                        bool isbreak = false;
                        string[] strpostion = strs.Split('.');
                        foreach (var item in strpostion)
                        {
                            if (isbreak)
                            {
                                break;
                            }
                            int f = item.IndexOf('：');
                            if (f >= 0)
                            {
                                string strpost = item.Substring(0, f);
                                if (strpost == SystemClass.PositionName)
                                {
                                    string[] strdir = item.Substring(f + 1, item.Length - (f + 1)).Split(',');
                                    foreach (var dir in strdir)
                                    {
                                        if (dir == CommonalityEntity.Driveway_Name)
                                        {
                                            rbool = true;
                                            isbreak = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        if (!rbool)
                        {
                            return str = "卡" + itic + "没有通行此权限";
                        }
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInformationForm.IsReleaseJurisdictionMethod()");
                return str = "卡" + IC1 + "没有通行此权限";
            }
            return str;
        }
    }
}