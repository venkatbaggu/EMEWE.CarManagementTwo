using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Linq.Expressions;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.HelpClass;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.OleDb;
using GemBox.ExcelLite;
using EMEWE.CarManagement.CommonClass;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class BlacklistForm : Form
    {
        public BlacklistForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 是否是修改 true修改 false新增
        /// </summary>
        public bool isUpdate = false;
        public MainForm mf;//主窗体
        /// <summary>
        /// 返回一个空的预置皮重
        /// </summary>
        Expression<Func<View_BlackList_CarInfo_StaffInfo_CustomerInfo, bool>> expr = null;
        /// <summary>
        /// 实例化分页控件
        /// </summary>
        PageControl page = new PageControl();
        // 定义一个全局变量： 门岗编号
        int iPositionID = 0;
        /// <summary>
        /// 存放查询条件
        /// </summary>
        public string sqlwhere = " 1=1 ";

        /// <summary>
        /// 私有字段驾驶员SName
        /// </summary>
        public string SName = "";

        /// <summary>
        /// 私有字段公司名GName
        /// </summary>
        public string GName = "";

        /// <summary>
        /// 驾驶员编号
        /// </summary>
        private string staffInfo_Id = "";

        /// <summary>
        /// 记录驾驶员ID
        /// </summary>
        private string prStaffInfo_Id = "";

        /// <summary>
        /// 记录公司ID
        /// </summary>
        private string prCustomerInfo_ID = "";

        /// <summary>
        /// 公司名
        /// </summary>
        private string CustomerInfo_ID = "";

        /// <summary>
        /// 车ID
        /// </summary>
        private string Carid = "";

        /// <summary>
        /// 是否导出所有
        /// </summary>
        private bool ISExecl = false;
        /// <summary>
        /// Load 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BlacklistForm_Load(object sender, EventArgs e)
        {
            myUsertv.Visible = false;
            userContext();
            btnUpdate.Enabled = false;
            BindBlackListState();
            BindBlackListType();
            BindBlackListState1();
            BindBlackListType1();
            this.txtBlacklist_Name.Focus();
            tscbxPageSize.SelectedIndex = 1;
            groupBox1.Visible = false;
            // LoadData();
            mf = new MainForm();
            textBox1.Text = common.USERNAME;

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
                btnDel.Enabled = true;
                btnDel.Visible = true;
                btnUpdate.Enabled = true;
                btnUpdate.Visible = true;
                btnBackAdd.Enabled = true;
                btnBackAdd.Visible = true;
                tslbExecl.Enabled = true;
                tslbExecl.Visible = true;
                tslbInExecl.Enabled = true;
                tslbInExecl.Visible = true;
            }
            else
            {
                btnSave.Visible = ControlAttributes.BoolControl("btnSave", "BlacklistForm", "Visible");
                btnSave.Enabled = ControlAttributes.BoolControl("btnSave", "BlacklistForm", "Enabled");

                btnDel.Visible = ControlAttributes.BoolControl("btnDel", "BlacklistForm", "Visible");
                btnDel.Enabled = ControlAttributes.BoolControl("btnDel", "BlacklistForm", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "BlacklistForm", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "BlacklistForm", "Enabled");

                btnBackAdd.Visible = ControlAttributes.BoolControl("btnBackAdd", "BlacklistForm", "Visible");
                btnBackAdd.Enabled = ControlAttributes.BoolControl("btnBackAdd", "BlacklistForm", "Enabled");

                tslbExecl.Visible = ControlAttributes.BoolControl("tslbExecl", "BlacklistForm", "Visible");
                tslbExecl.Enabled = ControlAttributes.BoolControl("tslbExecl", "BlacklistForm", "Enabled");

                tslbInExecl.Visible = ControlAttributes.BoolControl("tslbInExecl", "BlacklistForm", "Visible");
                tslbInExecl.Enabled = ControlAttributes.BoolControl("tslbInExecl", "BlacklistForm", "Enabled");
            }
        }
        /// <summary>
        /// 菜单栏加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.dgvBlackList.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
                this.dgvBlackList.DataSource = null;

                LogInfoLoad("");
            }
            catch
            {
                CommonalityEntity.WriteTextLog("日记管理 LoadData()");
            }
        }
        #region comboxBox下拉框绑定
        /// <summary>
        /// 绑定黑名单状态
        /// </summary>
        private void BindBlackListState()
        {
            try
            {
                this.comboxBlacklist_State.DataSource = LinQBaseDao.Query("select * from Dictionary where Dictionary_OtherID in( select Dictionary_ID from Dictionary where Dictionary_Name='黑名单状态') order by Dictionary_Sort ").Tables[0];

                if (this.comboxBlacklist_State.DataSource != null)
                {
                    this.comboxBlacklist_State.DisplayMember = "Dictionary_Name";
                    this.comboxBlacklist_State.ValueMember = "Dictionary_ID";
                    this.comboxBlacklist_State.Text = "0";
                }

            }
            catch
            {
            }
        }
        /// <summary>
        /// 绑定黑名单类型
        /// </summary>
        private void BindBlackListType()
        {
            try
            {
                this.comboxBlacklist_Type.DataSource = DictionaryDAL.GetValueDictionary("10").Where(n => n.Dictionary_Name != "全部").ToList();

                if (this.comboxBlacklist_Type.DataSource != null)
                {
                    this.comboxBlacklist_Type.DisplayMember = "Dictionary_Name";
                    this.comboxBlacklist_Type.ValueMember = "Dictionary_ID";
                    this.comboxBlacklist_Type.SelectedValue = -1;
                }

            }
            catch
            {

            }
        }
        /// <summary>
        /// 搜索--绑定黑名单状态
        /// </summary>
        private void BindBlackListState1()
        {
            try
            {
                DataTable dt = LinQBaseDao.Query("select * from Dictionary where Dictionary_OtherID in( select Dictionary_ID from Dictionary where Dictionary_Name='黑名单状态') order by Dictionary_Sort ").Tables[0];
                DataRow dr = dt.NewRow();
                dr["Dictionary_ID"] = "0";
                dr["Dictionary_Name"] = "全部";
                dt.Rows.InsertAt(dr, 0);
                this.comboxBlacklistState.DataSource = dt;

                if (this.comboxBlacklistState.DataSource != null)
                {
                    this.comboxBlacklistState.DisplayMember = "Dictionary_Name";
                    this.comboxBlacklistState.ValueMember = "Dictionary_ID";
                    this.comboxBlacklistState.SelectedIndex = 0;
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// 搜索--绑定黑名单类型
        /// </summary>
        private void BindBlackListType1()
        {
            try
            {
                this.comboxBlacklistType.DataSource = DictionaryDAL.GetValueStateDictionary("10");

                if (this.comboxBlacklistType.DataSource != null)
                {
                    this.comboxBlacklistType.DisplayMember = "Dictionary_Name";
                    this.comboxBlacklistType.ValueMember = "Dictionary_ID";
                    this.comboxBlacklistType.SelectedIndex = 3;
                }

            }
            catch
            {
            }
        }
        #endregion
        /// <summary>
        /// 查重方法
        /// </summary>
        /// <returns></returns>
        private bool btnCheck()
        {
            bool rbool = true;
            try
            {
                string state = this.comboxBlacklist_State.Text.ToString();
                string type = this.comboxBlacklist_Type.Text;
                //判断名称是否已存在
                if (type == "车")
                {
                    string CarNo = this.txtContor.Text;
                    Expression<Func<View_BlackList_CarInfo_StaffInfo_CustomerInfo, bool>> funviewBlacklist = n => n.Car_Name == CarNo;
                    if (BlacklistDAL.Query(funviewBlacklist).Count() > 0)
                    {
                        PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该车牌号已存在于黑名单中！", txtContor, this);
                        txtContor.Focus();
                        rbool = false; ;
                    }
                }
                if (type == "公司")
                {
                    string CustomerName = this.txtContor.Text.Trim();
                    Expression<Func<View_BlackList_CarInfo_StaffInfo_CustomerInfo, bool>> funviewBlacklist1 = n => n.CustomerInfo_Name == CustomerName;
                    if (BlacklistDAL.Query(funviewBlacklist1).Count() > 0)
                    {
                        PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该公司已存在于黑名单中！", txtContor, this);
                        txtContor.Focus();
                        rbool = false; ;
                    }
                }
                if (type == "人员")
                {
                    string StaffInfoName = this.txtContor.Text.Trim();
                    Expression<Func<View_BlackList_CarInfo_StaffInfo_CustomerInfo, bool>> funviewBlacklist2 = n => n.StaffInfo_Name == StaffInfoName;
                    if (BlacklistDAL.Query(funviewBlacklist2).Count() > 0)
                    {
                        PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该人员已存在于黑名单中！", txtContor, this);
                        txtContor.Focus();
                        rbool = false; ;
                    }
                }
                return rbool;
            }
            catch
            {
                CommonalityEntity.WriteTextLog("人员信息管理 btnCheck()");
                rbool = false;
            }
            return rbool;
        }

        /// <summary>
        /// 查重方法
        /// </summary>
        /// <returns></returns>
        private bool btnCheckupdate()
        {
            bool rbool = true;
            try
            {
                string state = this.comboxBlacklist_State.Text.ToString();
                string type = this.comboxBlacklist_Type.Text;
                //判断名称是否已存在
                if (type == "车")
                {
                    string CarNo = this.txtContor.Text;
                    Expression<Func<View_BlackList_CarInfo_StaffInfo_CustomerInfo, bool>> funviewBlacklist = n => n.Car_Name == CarNo && n.Car_Name != this.dgvBlackList.SelectedRows[0].Cells["CarInfo_Name"].Value.ToString();
                    if (BlacklistDAL.Query(funviewBlacklist).Count() > 0)
                    {
                        PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该车牌号已存在于黑名单中！", txtContor, this);
                        txtContor.Focus();
                        rbool = false;
                    }
                }
                if (type == "公司")
                {
                    string CustomerName = this.txtContor.Text.Trim();
                    Expression<Func<View_BlackList_CarInfo_StaffInfo_CustomerInfo, bool>> funviewBlacklist1 = n => n.CustomerInfo_Name == CustomerName && n.CustomerInfo_Name != this.dgvBlackList.SelectedRows[0].Cells["CustomerInfo_Name"].Value.ToString();
                    if (BlacklistDAL.Query(funviewBlacklist1).Count() > 0)
                    {
                        PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该公司已存在于黑名单中！", txtContor, this);
                        txtContor.Focus();
                        rbool = false;
                    }
                }
                if (type == "人员")
                {
                    string StaffInfoName = this.txtContor.Text.Trim();
                    Expression<Func<View_BlackList_CarInfo_StaffInfo_CustomerInfo, bool>> funviewBlacklist2 = n => n.StaffInfo_Name == StaffInfoName & n.StaffInfo_Name != this.dgvBlackList.SelectedRows[0].Cells["StaffInfo_Name"].Value.ToString();
                    if (BlacklistDAL.Query(funviewBlacklist2).Count() > 0)
                    {
                        PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该人员已存在于黑名单中！", txtContor, this);
                        txtContor.Focus();
                        rbool = false;
                    }
                }
                return rbool;
            }
            catch
            {
                CommonalityEntity.WriteTextLog("人员信息管理 btnCheck()");
                rbool = false;
            }
            return rbool;
        }


        private void isADDStaffInfo()
        {

            DataTable dt = LinQBaseDao.Query("select StaffInfo_ID from StaffInfo where StaffInfo_Name='" + txtContor.Text.Trim() + "' and StaffInfo_Identity='" + txtStaffInfo_Identity.Text.Trim() + "'").Tables[0];
            if (dt.Rows.Count > 0)
            {
                staffInfo_Id = dt.Rows[0][0].ToString();
                LinQBaseDao.Query("update StaffInfo  set  StaffInfo_Phone='" + txtPhone.Text.Trim() + "',StaffInfo_Identity='" + txtStaffInfo_Identity.Text.Trim() + "'  where StaffInfo_ID=" + staffInfo_Id);
            }
            else
            {
                staffInfo_Id = LinQBaseDao.GetSingle("Insert into StaffInfo(StaffInfo_Type,StaffInfo_Name,StaffInfo_Phone,StaffInfo_Time,StaffInfo_State,StaffInfo_Identity) values('驾驶员','" + txtContor.Text.Trim() + "','" + txtPhone.Text.Trim() + "',GETDATE(),'启动','" + txtStaffInfo_Identity.Text.Trim() + "')      select @@identity").ToString();
            }
        }
        private void isADDCustomerInfo()
        {
            DataTable dt = LinQBaseDao.Query("select CustomerInfo_ID from CustomerInfo where CustomerInfo_Name='" + txtContor.Text.Trim() + "'").Tables[0];
            if (dt.Rows.Count > 0)
            {
                CustomerInfo_ID = dt.Rows[0][0].ToString();
            }
            else
            {
                CustomerInfo_ID = LinQBaseDao.GetSingle("insert CustomerInfo(CustomerInfo_Name,CustomerInfo_State,CustomerInfo_Time) values('" + txtContor.Text.Trim() + "','启动',GETDATE())  select @@identity").ToString();

            }
        }

        private void isADDCAR()
        {

            DataTable dt = LinQBaseDao.Query("select Car_ID from Car where Car_Name='" + txtContor.Text.Trim() + "'").Tables[0];
            if (dt.Rows.Count > 0)
            {
                Carid = dt.Rows[0][0].ToString();
            }
            else
            {
                Carid = LinQBaseDao.GetSingle("insert Car(Car_Name,Car_State,Car_CreateTime,Car_ISRegister,Car_ISStaffInfo) values('" + txtContor.Text.Trim() + "','启动',GETDATE(),0,0)  select @@identity").ToString();

            }
        }
        /// <summary>
        /// 是否优先校验
        /// </summary>
        private void CheckCarPrecedence()
        {
            string strsql = "";
            DataTable dt;
            if (comboxBlacklist_Type.Text.Trim().Equals("公司"))
            {
                strsql = " select CarPrecedence_CustomerInfo_ID from CarPrecedence where CarPrecedence_CustomerInfo_ID in (select CustomerInfo_ID from CustomerInfo where CustomerInfo_Name='" + txtContor.Text.Trim() + "')";
                dt = LinQBaseDao.Query(strsql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    strsql = " update CarPrecedence set CarPrecedence_Sate='注销' where CarPrecedence_CustomerInfo_ID=" + dt.Rows[0][0].ToString();
                    LinQBaseDao.Query(strsql);
                    CommonalityEntity.WriteLogData("修改", txtContor.Text.Trim() + ",加入黑名单时注销了优先权限", CommonalityEntity.USERNAME);
                }
            }
            else if (comboxBlacklist_Type.Text.Trim().Equals("人员"))
            {
                strsql = "select CarPrecedence_StaffInfo_ID from CarPrecedence where CarPrecedence_StaffInfo_ID  in( select StaffInfo_ID from  StaffInfo where StaffInfo_Name='" + txtContor.Text.Trim() + "' and StaffInfo_Identity='" + txtStaffInfo_Identity.Text.Trim() + "')";
                dt = LinQBaseDao.Query(strsql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    strsql = " update CarPrecedence set CarPrecedence_Sate='注销' where CarPrecedence_StaffInfo_ID=" + dt.Rows[0][0].ToString();
                    LinQBaseDao.Query(strsql);
                    CommonalityEntity.WriteLogData("修改", txtContor.Text.Trim() + ",加入黑名单时注销了优先权限", CommonalityEntity.USERNAME);
                }
            }
            else
            {
                strsql = "select CarPrecedence_CarNO from CarPrecedence where CarPrecedence_CarNO in (select Car_Name from Car where Car_Name='" + txtContor.Text.Trim() + "') ";
                dt = LinQBaseDao.Query(strsql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    strsql = " update CarPrecedence set CarPrecedence_Sate='注销' where CarPrecedence_CarNO='" + dt.Rows[0][0].ToString() + "'";
                    LinQBaseDao.Query(strsql);
                    CommonalityEntity.WriteLogData("修改", txtContor.Text.Trim() + ",加入黑名单时注销了优先权限", CommonalityEntity.USERNAME);
                }
            }
        }
        /// <summary>
        /// “保 存” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (comboxBlacklist_Type.Text.Trim() == "")
            {
                MessageBox.Show("请选择黑名单类型");
                return;
            }

            if (comboxBlacklist_Type.Text.Trim().Equals("人员"))
            {
                #region 选择黑名单类型为人员时
                try
                {
                    if (string.IsNullOrEmpty(txtContor.Text))
                    {
                        MessageBox.Show("请选择人员！");
                        return;
                    }
                    if (txtStaffInfo_Identity.Text.Trim() == "")
                    {
                        MessageBox.Show("身份证不能为空！");
                        return;
                    }
                    if (txtPhone.Text.Trim() == "")
                    {
                        MessageBox.Show("手机号码不能为空！");
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
                    if (myUsertv.S_Name == "")
                    {
                        isADDStaffInfo();
                        myUsertv.S_ID = staffInfo_Id;
                        myUsertv.S_Name = txtContor.Text.Trim();
                    }

                    Blacklist bkl;

                    string StaffInfoName = this.txtContor.Text.Trim();
                    Expression<Func<View_BlackList_CarInfo_StaffInfo_CustomerInfo, bool>> funviewBlacklist2 = n => n.StaffInfo_ID == Convert.ToInt32(myUsertv.S_ID);
                    if (BlacklistDAL.Query(funviewBlacklist2).Count() > 0)
                    {
                        MessageBox.Show("人员：" + StaffInfoName + "已存在黑名单中！");
                        return;
                    }
                    CheckCarPrecedence();



                    //循环驾驶员ID，每循环一次加一条黑名单数据
                    bkl = new Blacklist();
                    bkl.Blacklist_State = this.comboxBlacklist_State.Text.Trim();//正常
                    bkl.Blacklist_Type = this.comboxBlacklist_Type.Text.Trim();//驾驶员
                    bkl.Blacklist_Time = CommonalityEntity.GetServersTime();//时间
                    bkl.Blacklist_Name = this.txtBlacklist_Name.Text.Trim();//原因
                    bkl.Blacklist_Remark = this.txtBlacklist_Remark.Text.Trim();//备注
                    bkl.Blacklist_Dictionary_ID = Convert.ToInt32(this.comboxBlacklist_State.SelectedValue);
                    bkl.Blacklist_UpgradeCount = 0;
                    bkl.Blacklist_DowngradeCount = 0;
                    bkl.Blacklist_StaffInfo_ID = CommonalityEntity.GetInt(myUsertv.S_ID);

                    if (BlacklistDAL.InsertOneQCRecord(bkl))
                    {
                        MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("添加失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    string strContent1 = "黑名单类型为： " + this.comboxBlacklist_Type.Text.Trim();
                    CommonalityEntity.WriteLogData("添加", strContent1, CommonalityEntity.USERNAME);//添加操作日志  

                }
                catch
                {
                    CommonalityEntity.WriteTextLog("黑名单信息管理 btnSave_Click()");
                }
                finally
                {
                    LoadData();
                    Empty();
                }
                #endregion
            }
            else if (comboxBlacklist_Type.Text.Trim().Equals("公司"))
            {
                #region 选择黑名单类型为公司时
                try
                {
                    if (string.IsNullOrEmpty(txtContor.Text))
                    {
                        MessageBox.Show("请选择公司信息！");
                        return;
                    }
                    isADDCustomerInfo();
                    Expression<Func<View_BlackList_CarInfo_StaffInfo_CustomerInfo, bool>> funviewBlacklist2 = n => n.Blacklist_CustomerInfo_ID == Convert.ToInt32(CustomerInfo_ID);
                    if (BlacklistDAL.Query(funviewBlacklist2).Count() > 0)
                    {
                        MessageBox.Show("公司：" + txtContor.Text + "已存在黑名单中！");
                        return;
                    }
                    CheckCarPrecedence();
                    Blacklist bkl = new Blacklist();
                    bkl.Blacklist_State = this.comboxBlacklist_State.Text.Trim();//黑名单状态
                    bkl.Blacklist_Type = this.comboxBlacklist_Type.Text.Trim();//驾驶员
                    bkl.Blacklist_Time = CommonalityEntity.GetServersTime();//时间
                    bkl.Blacklist_Name = this.txtBlacklist_Name.Text.Trim();//原因
                    bkl.Blacklist_Remark = this.txtBlacklist_Remark.Text.Trim();//备注
                    bkl.Blacklist_Dictionary_ID = Convert.ToInt32(this.comboxBlacklist_State.SelectedValue);
                    bkl.Blacklist_UpgradeCount = 0;
                    bkl.Blacklist_DowngradeCount = 0;
                    bkl.Blacklist_CarInfo_ID = null;
                    bkl.Blacklist_StaffInfo_ID = null;
                    //查是否有此公司
                    bkl.Blacklist_CustomerInfo_ID = Convert.ToInt32(CustomerInfo_ID);

                    if (BlacklistDAL.InsertOneQCRecord(bkl))
                    {
                        MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("添加失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    string strContent1 = "黑名单类型为： " + this.comboxBlacklist_Type.Text.Trim();
                    CommonalityEntity.WriteLogData("添加", strContent1, CommonalityEntity.USERNAME);//添加操作日志  

                }
                catch
                {
                    CommonalityEntity.WriteTextLog("黑名单信息管理 btnSave_Click()");
                }
                finally
                {
                    LoadData();
                }
                #endregion
            }
            else if (comboxBlacklist_Type.Text.Trim().Equals("车"))
            {
                #region 选择黑名单类型为车时
                try
                {
                    if (string.IsNullOrEmpty(txtContor.Text))
                    {
                        MessageBox.Show("请选择车牌号信息！");
                        return;
                    }
                    isADDCAR();
                    CheckCarPrecedence();
                    Expression<Func<View_BlackList_CarInfo_StaffInfo_CustomerInfo, bool>> funviewBlacklist2 = n => n.Blacklist_CarInfo_ID == Convert.ToInt32(Carid);
                    if (BlacklistDAL.Query(funviewBlacklist2).Count() > 0)
                    {
                        MessageBox.Show("车辆：" + txtContor.Text + "已存在黑名单中！");
                        return;
                    }

                    Blacklist bkl = new Blacklist();
                    bkl.Blacklist_State = this.comboxBlacklist_State.Text.Trim();//黑名单状态
                    bkl.Blacklist_Type = this.comboxBlacklist_Type.Text.Trim();//驾驶员
                    bkl.Blacklist_Time = CommonalityEntity.GetServersTime();//时间
                    bkl.Blacklist_Name = this.txtBlacklist_Name.Text.Trim();//原因
                    bkl.Blacklist_Remark = this.txtBlacklist_Remark.Text.Trim();//备注
                    bkl.Blacklist_Dictionary_ID = Convert.ToInt32(this.comboxBlacklist_State.SelectedValue);
                    bkl.Blacklist_UpgradeCount = 0;
                    bkl.Blacklist_DowngradeCount = 0;
                    bkl.Blacklist_StaffInfo_ID = null;
                    bkl.Blacklist_CustomerInfo_ID = null;
                    bkl.Blacklist_CarInfo_ID = Convert.ToInt32(Carid);

                    if (BlacklistDAL.InsertOneQCRecord(bkl))
                    {
                        MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("添加失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    string strContent1 = "黑名单类型为： " + this.comboxBlacklist_Type.Text.Trim();
                    CommonalityEntity.WriteLogData("添加", strContent1, CommonalityEntity.USERNAME);//添加操作日志  
                }
                catch
                {
                    CommonalityEntity.WriteTextLog("黑名单信息管理 btnSave_Click()");
                }
                finally
                {
                    LoadData();
                }
                #endregion
            }
        }
        /// <summary>
        /// “修改” 查看数据按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvBlackList.SelectedRows.Count > 0)//选中行
                {
                    if (dgvBlackList.SelectedRows.Count > 1)
                    {
                        MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Blacklist bk = new Blacklist();
                        if (comboxBlacklist_Type.Text.Trim().Equals("人员"))
                        {
                            #region 人员ID
                            if (this.txtContor.Text.ToString() == "")
                            {
                                MessageBox.Show("请选择人员");
                                return;
                            }
                            if (txtStaffInfo_Identity.Text.Trim() == "")
                            {
                                MessageBox.Show("身份证不能为空！");
                                return;
                            }
                            if (txtPhone.Text.Trim() == "")
                            {
                                MessageBox.Show("手机号码不能为空！");
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
                            if (!btnCheckupdate()) return; // 去重复
                            isADDStaffInfo();
                            CheckCarPrecedence();
                            Expression<Func<Blacklist, bool>> p = n => n.Blacklist_ID == int.Parse(this.dgvBlackList.SelectedRows[0].Cells["Blacklist_ID"].Value.ToString());
                            Action<Blacklist> ap = s =>
                            {
                                s.Blacklist_StaffInfo_ID = Convert.ToInt32(staffInfo_Id);//人员ID
                                s.Blacklist_State = this.comboxBlacklist_State.Text.Trim();//黑名单状态
                                s.Blacklist_Type = this.comboxBlacklist_Type.Text.Trim();//黑名单类型
                                s.Blacklist_Time = CommonalityEntity.GetServersTime();
                                s.Blacklist_Name = this.txtBlacklist_Name.Text.Trim();//黑名单原因
                                s.Blacklist_Remark = this.txtBlacklist_Remark.Text.Trim();//黑名单备注
                                s.Blacklist_Dictionary_ID = Convert.ToInt32(this.comboxBlacklist_State.SelectedValue);//字典表ID
                                s.Blacklist_UpgradeCount = 0;//每几次升级
                                s.Blacklist_DowngradeCount = 0;//每几次降级
                            };
                            if (BlacklistDAL.Update(p, ap))
                            {
                                MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Empty();
                            }
                            else
                            {
                                MessageBox.Show("修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            #endregion
                        }
                        else if (comboxBlacklist_Type.Text.Trim().Equals("车"))
                        {
                            #region 车子ID
                            if (this.txtContor.Text.ToString() == "")
                            {
                                MessageBox.Show("请输入车牌号");
                                return;
                            }
                            if (!btnCheckupdate()) return; // 去重复
                            isADDCAR();
                            CheckCarPrecedence();
                            Expression<Func<Blacklist, bool>> p = n => n.Blacklist_ID == int.Parse(this.dgvBlackList.SelectedRows[0].Cells["Blacklist_ID"].Value.ToString());
                            Action<Blacklist> ap = s =>
                            {
                                s.Blacklist_CarInfo_ID = Convert.ToInt32(Carid);//车辆ID
                                s.Blacklist_State = this.comboxBlacklist_State.Text.Trim();//黑名单状态
                                s.Blacklist_Type = this.comboxBlacklist_Type.Text.Trim();//黑名单类型
                                s.Blacklist_Time = CommonalityEntity.GetServersTime();
                                s.Blacklist_Name = this.txtBlacklist_Name.Text.Trim();//黑名单原因
                                s.Blacklist_Remark = this.txtBlacklist_Remark.Text.Trim();//黑名单备注
                                s.Blacklist_Dictionary_ID = Convert.ToInt32(this.comboxBlacklist_State.SelectedValue);//字典表ID
                                s.Blacklist_UpgradeCount = 0;//每几次升级
                                s.Blacklist_DowngradeCount = 0;//每几次降级
                            };
                            if (BlacklistDAL.Update(p, ap))
                            {
                                MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Empty();
                            }
                            else
                            {
                                MessageBox.Show("修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            #endregion
                        }
                        else if (comboxBlacklist_Type.Text.Trim().Equals("公司"))
                        {
                            #region 修改公司
                            //公司ID
                            if (this.txtContor.Text.ToString() == "")
                            {
                                MessageBox.Show("请输入公司名称！");
                                return;
                            }
                            if (!btnCheckupdate()) return; // 去重复
                            isADDCustomerInfo();
                            CheckCarPrecedence();
                            Expression<Func<Blacklist, bool>> p = n => n.Blacklist_ID == int.Parse(this.dgvBlackList.SelectedRows[0].Cells["Blacklist_ID"].Value.ToString());
                            Action<Blacklist> ap = s =>
                            {
                                s.Blacklist_CustomerInfo_ID = Convert.ToInt32(CustomerInfo_ID);//公司名称ID
                                s.Blacklist_State = this.comboxBlacklist_State.Text.Trim();//黑名单状态
                                s.Blacklist_Type = this.comboxBlacklist_Type.Text.Trim();//黑名单类型
                                s.Blacklist_Time = CommonalityEntity.GetServersTime();
                                s.Blacklist_Name = this.txtBlacklist_Name.Text.Trim();//黑名单原因
                                s.Blacklist_Remark = this.txtBlacklist_Remark.Text.Trim();//黑名单备注
                                s.Blacklist_Dictionary_ID = Convert.ToInt32(this.comboxBlacklist_State.SelectedValue);//字典表ID
                                s.Blacklist_UpgradeCount = 0;//每几次升级
                                s.Blacklist_DowngradeCount = 0;//每几次降级
                            };
                            if (BlacklistDAL.Update(p, ap))
                            {
                                MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Empty();
                            }
                            else
                            {
                                MessageBox.Show("修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            #endregion
                        }


                        string strContent = "黑名单编号为：" + this.dgvBlackList.SelectedRows[0].Cells["Blacklist_ID"].Value.ToString();
                        CommonalityEntity.WriteLogData("修改", strContent, CommonalityEntity.USERNAME);//添加操作日志
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("黑名单信息管理 btnSave_Click()");
            }
            finally
            {
                LoadData();
                this.btnUpdate.Enabled = false;
                this.btnSave.Enabled = true;
                Empty();
            }
        }

        /// <summary>
        /// “删除” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            tbtnDelStaffInfo();
        }
        /// <summary>
        /// 删除选中行数据的方法 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbtnDelStaffInfo()
        {
            try
            {
                int j = 0;
                if (this.dgvBlackList.SelectedRows.Count > 0)//选中删除
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //选中数量
                        int count = dgvBlackList.SelectedRows.Count;
                        string id = "";
                        //遍历
                        for (int i = 0; i < count; i++)
                        {
                            Expression<Func<Blacklist, bool>> funuserinfo = n => n.Blacklist_ID == int.Parse(this.dgvBlackList.SelectedRows[i].Cells["Blacklist_ID"].Value.ToString());

                            if (!BlacklistDAL.DeleteToMany(funuserinfo))
                            {
                                j++;
                            }
                        }
                        if (j == 0)
                        {
                            MessageBox.Show("成功删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadData();
                            btnUpdate.Enabled = false;
                            btnSave.Enabled = true;
                        }
                        else
                        {
                            MessageBox.Show("删除失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        string strContent = "黑名单编号为：" + this.dgvBlackList.SelectedRows[0].Cells["Blacklist_ID"].Value.ToString();
                        CommonalityEntity.WriteLogData("删除", strContent, CommonalityEntity.USERNAME);//添加操作日志
                    }
                }
                else//没有选中
                {
                    MessageBox.Show("请选择要删除的行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("黑名单信息管理 tbtnDelUser_delete() 异常！+");
            }
            finally
            {
                LoadData();
                Empty();
            }
        }

        /// <summary>
        /// “清空” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmpty_Click(object sender, EventArgs e)
        {
            Empty();
            isUpdate = false;
            myUsertv.StaffInfo_Name = "";
            this.btnUpdate.Enabled = false;
            this.btnSave.Enabled = true;
            this.btnDel.Enabled = false;
        }
        /// <summary>
        /// 清空的方法
        /// </summary>
        private void Empty()
        {
            isClick = true;
            myUsertv.S_ID = "";
            myUsertv.S_Name = "";
            SName = "";
            staffInfo_Id = "";
            CustomerInfo_ID = "";
            this.txtCarNo.Text = "";
            this.txtCustomer.Text = "";
            this.txtStaffName.Text = "";
            this.txtContor.Text = "";
            this.txtStaffInfo_Identity.Text = "";
            this.txtPhone.Text = "";
            this.comboxBlacklist_State.SelectedIndex = 0;
            this.txtBlacklist_Name.Text = "";
            this.txtBlacklist_Remark.Text = "";
        }

        /// <summary>
        /// “搜索” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSeach_Click(object sender, EventArgs e)
        {
            dgvBlackList.DataSource = null;
            sqlwhere = " 1=1 ";
            //模糊条件查询
            try
            {
                string state = this.comboxBlacklistState.Text.Trim();//黑名单状态
                string type = this.comboxBlacklistType.Text.Trim();//黑名单类型
                string no = this.txtCarNo.Text;//车牌号
                string Company = this.txtCustomer.Text;//公司名
                string StaffName = this.txtStaffName.Text;//人员姓名

                if (!string.IsNullOrEmpty(state))//黑名单状态
                {
                    if (state != "全部")
                    {
                        sqlwhere += String.Format(" and Blacklist_State ='{0}'", state);
                    }
                }
                if (!string.IsNullOrEmpty(type))//黑名单类型
                {
                    if (type != "全部")
                    {
                        sqlwhere += String.Format(" and Blacklist_Type like  '%{0}%'", type);
                    }
                }
                if (!string.IsNullOrEmpty(no))//车牌号
                {
                    sqlwhere += String.Format(" and Car_Name like  '%{0}%'", no);
                }
                if (!string.IsNullOrEmpty(Company))//公司名称
                {
                    sqlwhere += String.Format(" and CustomerInfo_Name like  '%{0}%'", Company);
                }
                if (!string.IsNullOrEmpty(StaffName))//人员名称
                {
                    sqlwhere += String.Format(" and StaffInfo_Name like  '%{0}%'", StaffName);
                }
                LogInfoLoad("");
            }
            catch
            {
                CommonalityEntity.WriteTextLog("BlacklistForm.GetDictionarySeach异常:");
            }
        }

        #region 分页和加载DataGridView
        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslNotChecked()
        {
            for (int i = 0; i < this.dgvBlackList.Rows.Count; i++)
            {
                dgvBlackList.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslAllChecked()
        {
            for (int i = 0; i < dgvBlackList.Rows.Count; i++)
            {
                this.dgvBlackList.Rows[i].Selected = true;
            }
        }
        /// <summary>
        /// 分页控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "tslSelectAll")//全选
            {
                ISExecl = true;
                tslAllChecked();
                return;
            }
            if (e.ClickedItem.Name == "tslNotSelect")//取消全选
            {
                ISExecl = false;
                tslNotChecked();
                return;
            }
            if (e.ClickedItem.Name == "tslbExecl")//导出Execl
            {
                tsbExecl_Click();
                return;
            }
            if (e.ClickedItem.Name == "tslbInExecl")//导入Execl
            {
                InExecl();
                return;
            }
            LogInfoLoad(e.ClickedItem.Name);
        }
        /// <summary>
        /// 选择每页条数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tscbxPageSize2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogInfoLoad("");

        }

        /// <summary>
        /// 加载信息
        /// </summary>
        public void LogInfoLoad(string strClickedItemName)
        {
            page.BindBoundControl(dgvBlackList, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_BlackList_CarInfo_StaffInfo_CustomerInfo", "*", "Blacklist_ID", "Blacklist_ID", 0, sqlwhere, true);
        }
        #endregion


        bool isClick;
        /// <summary>
        /// 用户双击组件时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvBlackList_DoubleClick(object sender, EventArgs e)
        {
            if (this.dgvBlackList.SelectedRows.Count > 0)//选中行
            {
                if (dgvBlackList.SelectedRows.Count > 1)
                {
                    MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    this.btnUpdate.Enabled = true;
                    this.btnSave.Enabled = false;
                    this.btnDel.Enabled = true;
                    isUpdate = true;
                    isClick = true;
                    txtContor.Text = "";
                    txtContor.Tag = "";
                    myUsertv.StaffInfo_ID = "";
                    myUsertv.StaffInfo_Name = "";
                    isClick = true;
                    //修改的值
                    int ID = int.Parse(this.dgvBlackList.SelectedRows[0].Cells["Blacklist_ID"].Value.ToString());
                    Expression<Func<View_BlackList_CarInfo_StaffInfo_CustomerInfo, bool>> funviewinto = n => n.Blacklist_ID == ID;
                    foreach (var n in BlacklistDAL.Query(funviewinto))
                    {
                        if (n.CustomerInfo_ID != null)
                        {
                            myUsertv.S_ID = n.CustomerInfo_ID.ToString();
                            myUsertv.S_Name = n.CustomerInfo_Name;
                        }
                        if (n.StaffInfo_ID != null)
                        {
                            myUsertv.S_ID = n.StaffInfo_ID.ToString();
                            myUsertv.S_Name = n.StaffInfo_Name;
                            myUsertv.S_Indes = n.StaffInfo_Identity.ToString();
                            myUsertv.S_phone = n.StaffInfo_Phone;
                        }

                        if (n.Car_Name != null)
                        {
                            //车辆名称
                            this.txtContor.Text = n.Car_Name;
                            CommonalityEntity.tablename = "Car";
                        }
                        if (n.CustomerInfo_Name != null)
                        {
                            // 公司名称
                            this.txtContor.Text = n.CustomerInfo_Name;
                        }
                        if (n.StaffInfo_Name != null)
                        {
                            // 人员名称
                            SName = n.StaffInfo_Phone;
                            this.txtContor.Text = n.StaffInfo_Name;
                            txtStaffInfo_Identity.Text = n.StaffInfo_Identity;
                            txtPhone.Text = n.StaffInfo_Phone;
                        }
                        if (n.Blacklist_State != null)
                        {
                            // 黑名单状态
                            this.comboxBlacklist_State.Text = n.Blacklist_State;
                        }
                        if (n.Blacklist_Type != null)
                        {
                            // 黑名单类型
                            this.comboxBlacklist_Type.Text = n.Blacklist_Type;
                        }
                        if (n.Blacklist_Name != null)
                        {
                            // 黑名单原因
                            this.txtBlacklist_Name.Text = n.Blacklist_Name;
                        }
                        if (n.Blacklist_Remark != null)
                        {
                            //黑名单备注
                            this.txtBlacklist_Remark.Text = n.Blacklist_Remark;
                        }
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择要修改的行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        /// <summary>
        /// 保存黑名单的方法
        /// </summary>
        private void fangfa()
        {
            #region
            string Car = this.txtContor.Text.Trim();
            string Company = this.txtContor.Text.Trim();
            string People = this.txtContor.Text.Trim();
            if (this.txtContor.Text.Trim() == "车")
            {
                if (this.txtContor.Text.Trim() == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "车辆车牌号不能为空！", txtContor, this);
                    return;
                }
                Car = this.txtContor.Text.Trim();
                if (this.txtContor.Text != "")
                {
                    Company = this.txtContor.Text.Trim();
                }
                else
                {
                    Company = "0";
                }
                if (this.txtContor.Text != "")
                {
                    People = this.txtContor.Text.Trim();
                }
                else
                {
                    People = "0";
                }
            }
            if (this.comboxBlacklist_Type.Text == "公司")
            {
                if (this.txtContor.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "公司名称不能为空！", txtContor, this);
                    return;
                }
                Company = this.txtContor.Text.Trim();
                if (this.txtContor.Text.Trim() != "")
                {
                    Car = this.txtContor.Text.Trim();
                }
                else
                {
                    Car = "0";
                }
                if (this.txtContor.Text != "")
                {
                    People = this.txtContor.Text.Trim();
                }
                else
                {
                    People = "0";
                }
            }
            if (this.comboxBlacklist_Type.Text == "人员")
            {
                if (this.txtContor.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "人员姓名不能为空！", txtContor, this);
                    return;
                }
                People = this.txtContor.Text.Trim();
                if (this.txtContor.Text != "")
                {
                    Company = this.txtContor.Text.Trim();
                }
                else
                {
                    Company = "0";
                }
                if (this.txtContor.Text.Trim() != "")
                {
                    Car = this.txtContor.Text.Trim();
                }
                else
                {
                    Car = "0";
                }
            }
            #endregion
        }

        /// <summary>
        /// 黑名单等级设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBackAdd_Click(object sender, EventArgs e)
        {
            BlacklistSet bks = new BlacklistSet();
            bks.ShowDialog();
        }

        private void dgvBlackList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置
                    if (dgvBlackList.Rows[e.RowIndex].Selected == false)
                    {
                        dgvBlackList.ClearSelection();
                        dgvBlackList.Rows[e.RowIndex].Selected = true;
                    }
                    //只选中一行时设置活动单元格
                    if (dgvBlackList.SelectedRows.Count == 1)
                    {
                        dgvBlackList.CurrentCell = dgvBlackList.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }
                    //弹出操作菜单
                    comMuneStript.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        /// <summary>
        /// 升级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpgradeBacklist_Click(object sender, EventArgs e)
        {
            int type = 0;
            string strsql = "";
            int blacklistID = int.Parse(this.dgvBlackList.SelectedRows[0].Cells["Blacklist_ID"].Value.ToString());
            string strtype = this.dgvBlackList.SelectedRows[0].Cells["Blacklist_Type"].Value.ToString();
            if (strtype == "车")
            {
                type = 1;
                strsql = "select Blacklist_CarInfo_ID from Blacklist where Blacklist_ID=" + blacklistID;
            }
            if (strtype == "人员")
            {
                type = 2;
                strsql = "select Blacklist_StaffInfo_ID from Blacklist where Blacklist_ID=" + blacklistID;
            }
            if (strtype == "公司")
            {
                type = 3;
                strsql = "select Blacklist_CustomerInfo_ID from Blacklist where Blacklist_ID=" + blacklistID;

            }
            int ID = Convert.ToInt32(LinQBaseDao.GetSingle(strsql).ToString());
            if (ControlAttributes.ISBacklist(ID, type, true))
            {
                MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LogInfoLoad("");
        }

        /// <summary>
        /// 降低
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DowngradeBacklist_Click(object sender, EventArgs e)
        {
            int ID = int.Parse(this.dgvBlackList.SelectedRows[0].Cells["Blacklist_ID"].Value.ToString());
            Expression<Func<Blacklist, bool>> p = n => n.Blacklist_ID == int.Parse(this.dgvBlackList.SelectedRows[0].Cells["Blacklist_ID"].Value.ToString());
            Action<Blacklist> ap = s =>
            {
                if (s.Blacklist_DowngradeCount == null)
                {
                    s.Blacklist_DowngradeCount = 1;
                }
                else
                    s.Blacklist_DowngradeCount += 1;
            };
            if (BlacklistDAL.Update(p, ap))
            {
                MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 通告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoticeContext_Click(object sender, EventArgs e)
        {
            int ID = int.Parse(this.dgvBlackList.SelectedRows[0].Cells["Blacklist_ID"].Value.ToString());
            Expression<Func<Blacklist, bool>> p = n => n.Blacklist_ID == int.Parse(this.dgvBlackList.SelectedRows[0].Cells["Blacklist_ID"].Value.ToString());
            Action<Blacklist> ap = s =>
            {
                s.Blacklist_State = "通知";
                s.Blacklist_UpgradeCount = 0;
                s.Blacklist_DowngradeCount = 0;
                s.Blacklist_Dictionary_ID = Convert.ToInt32(LinQBaseDao.Query("select Dictionary_ID from Dictionary where Dictionary_Name='通知'").Tables[0].Rows[0][0]);
            };
            if (BlacklistDAL.Update(p, ap))
            {
                MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LogInfoLoad("");
        }

        /// <summary>
        /// 修改为正常数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStript_Click(object sender, EventArgs e)
        {
            int ID = int.Parse(this.dgvBlackList.SelectedRows[0].Cells["Blacklist_ID"].Value.ToString());
            Expression<Func<Blacklist, bool>> p = n => n.Blacklist_ID == int.Parse(this.dgvBlackList.SelectedRows[0].Cells["Blacklist_ID"].Value.ToString());
            Action<Blacklist> ap = s =>
            {
                s.Blacklist_State = "正常";
                s.Blacklist_UpgradeCount = 0;
                s.Blacklist_DowngradeCount = 0;
                s.Blacklist_Dictionary_ID = Convert.ToInt32(LinQBaseDao.Query("select Dictionary_ID from Dictionary where Dictionary_Name='正常'").Tables[0].Rows[0][0]);
            };
            if (BlacklistDAL.Update(p, ap))
            {
                MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LogInfoLoad("");
        }


        /// <summary>
        /// 警告
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WarningContext_Click(Object sender, EventArgs e)
        {
            int ID = int.Parse(this.dgvBlackList.SelectedRows[0].Cells["Blacklist_ID"].Value.ToString());
            Expression<Func<Blacklist, bool>> p = n => n.Blacklist_ID == int.Parse(this.dgvBlackList.SelectedRows[0].Cells["Blacklist_ID"].Value.ToString());
            Action<Blacklist> ap = s =>
            {
                s.Blacklist_State = "警告";
                s.Blacklist_UpgradeCount = 0;
                s.Blacklist_DowngradeCount = 0;
                s.Blacklist_Dictionary_ID = Convert.ToInt32(LinQBaseDao.Query("select Dictionary_ID from Dictionary where Dictionary_Name='警告'").Tables[0].Rows[0][0]);
            };
            if (BlacklistDAL.Update(p, ap))
            {
                MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LogInfoLoad("");
        }

        /// <summary>
        /// 拒绝入厂
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefuseContext_Click(object sender, EventArgs e)
        {
            int ID = int.Parse(this.dgvBlackList.SelectedRows[0].Cells["Blacklist_ID"].Value.ToString());
            Expression<Func<Blacklist, bool>> p = n => n.Blacklist_ID == int.Parse(this.dgvBlackList.SelectedRows[0].Cells["Blacklist_ID"].Value.ToString());
            Action<Blacklist> ap = s =>
            {
                s.Blacklist_State = "拒绝入厂";
                s.Blacklist_UpgradeCount = 0;
                s.Blacklist_DowngradeCount = 0;
                s.Blacklist_Dictionary_ID = Convert.ToInt32(LinQBaseDao.Query("select Dictionary_ID from Dictionary where Dictionary_Name='拒绝入厂'").Tables[0].Rows[0][0]);
            };
            if (BlacklistDAL.Update(p, ap))
            {
                MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            LogInfoLoad("");
        }

        private void dgvBlackList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }




        public void MyUserContro()
        {
            if (CommonalityEntity.tablename == "StaffInfo")
            {
                if (myUsertv.S_ID != "")
                {
                    SName = myUsertv.S_Name;
                    staffInfo_Id = myUsertv.S_ID;
                    prStaffInfo_Id = staffInfo_Id;
                    txtContor.Text = SName;
                    txtStaffInfo_Identity.Text = myUsertv.S_Indes;
                    txtPhone.Text = myUsertv.S_phone;
                }

            }
            if (CommonalityEntity.tablename == "CustomerInfo")
            {
                if (myUsertv.S_ID != "")
                {
                    CustomerInfo_ID = myUsertv.S_ID;
                    GName = myUsertv.S_Name;
                    txtContor.Text = GName;
                    prCustomerInfo_ID = CustomerInfo_ID;
                }
            }
            if (CommonalityEntity.tablename == "")
            {
                txtContor.Text = "";
                txtStaffInfo_Identity.Text = "";
                txtPhone.Text = "";
            }

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
            MyUserContro();
        }

        private void comboxBlacklist_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!comboxBlacklist_Type.Text.Trim().Equals("人员"))
            {
                lblStaffInfo_Identity.Visible = false;
                txtStaffInfo_Identity.Visible = false;
                lblPhone.Visible = false;
                txtPhone.Visible = false;
                button1.Visible = false;
                txtStaffInfo_Identity.Text = "";
                txtPhone.Text = "";
            }
            if (comboxBlacklist_Type.Text.Trim().Equals("车"))
            {

                label6.Text = " 车牌号：";
                txtStaffInfo_Identity.Text = "";
                txtPhone.Text = "";

            }
            else if (comboxBlacklist_Type.Text.Trim().Equals("公司"))
            {
                label6.Text = "公司名称：";

            }
            else if (comboxBlacklist_Type.Text.Trim().Equals("人员"))
            {
                label6.Text = "   姓名：";
                lblStaffInfo_Identity.Visible = true;
                txtStaffInfo_Identity.Visible = true;
                lblPhone.Visible = true;
                txtPhone.Visible = true;
                button1.Visible = true;
            }
        }

        private void txtContor_Click(object sender, EventArgs e)
        {
            if (comboxBlacklist_Type.Text.Trim().Equals("车"))
            {
                myUsertv.Visible = false;
            }
            else if (comboxBlacklist_Type.Text.Trim().Equals("公司"))
            {
                myUsertv.Visible = true;
                CommonalityEntity.tablename = "CustomerInfo";
                CommonalityEntity.tabcom1 = "CustomerInfo_Name";
                CommonalityEntity.tabcom2 = "";
                CommonalityEntity.tabcom3 = "";
                CommonalityEntity.tabid = "CustomerInfo_ID";
                CommonalityEntity.strlike = txtContor.Text.Trim();
                myUsertv.StaffInfo_Select();
            }
            else if (comboxBlacklist_Type.Text.Trim().Equals("人员"))
            {
                myUsertv.Visible = true;
                CommonalityEntity.tablename = "StaffInfo";
                CommonalityEntity.tabcom1 = "StaffInfo_Name";
                CommonalityEntity.tabcom2 = "StaffInfo_Identity";
                CommonalityEntity.tabcom3 = "StaffInfo_Phone";
                CommonalityEntity.tabid = "StaffInfo_ID";
                CommonalityEntity.strlike = txtContor.Text.Trim();
                myUsertv.StaffInfo_Select();
            }
        }

        private void txtContor_TextChanged(object sender, EventArgs e)
        {
            if (isClick == false)
            {

                if (comboxBlacklist_Type.Text.Trim().Equals("车"))
                {
                    myUsertv.Visible = false;
                    CommonalityEntity.tablename = "";
                }
                else if (comboxBlacklist_Type.Text.Trim().Equals("公司"))
                {
                    myUsertv.Visible = true;
                    CommonalityEntity.tablename = "CustomerInfo";
                    CommonalityEntity.tabcom1 = "CustomerInfo_Name";
                    CommonalityEntity.tabcom2 = "";
                    CommonalityEntity.tabcom3 = "";
                    CommonalityEntity.tabid = "CustomerInfo_ID";
                    CommonalityEntity.strlike = txtContor.Text.Trim();
                    myUsertv.StaffInfo_Select();
                }
                else if (comboxBlacklist_Type.Text.Trim().Equals("人员"))
                {
                    myUsertv.Visible = true;
                    CommonalityEntity.tablename = "StaffInfo";
                    CommonalityEntity.tabcom1 = "StaffInfo_Name";
                    CommonalityEntity.tabcom2 = "StaffInfo_Identity";
                    CommonalityEntity.tabcom3 = "StaffInfo_Phone";
                    CommonalityEntity.tabid = "StaffInfo_ID";
                    CommonalityEntity.strlike = txtContor.Text.Trim();
                    myUsertv.StaffInfo_Select();
                }
            }
            else
            {
                isClick = false;
            }

        }


        private void tsbExecl_Click()
        {
            string fileName = "黑名单Excel报表-" + CommonalityEntity.GetServersTime().ToShortDateString();

            if (!ISExecl)
            {
                tslExport_Excel(fileName, dgvBlackList);
            }
            else
            {
                btnSeach_Click(null, null);
                string strsql = "select Blacklist_Type as 黑名单类型,Blacklist_State as 黑名单状态,Car_Name as 车辆车牌号,CustomerInfo_Name as 公司名称,StaffInfo_Name as 姓名,StaffInfo_Identity as 身份证号码,StaffInfo_Phone as StaffInfo_Phone,Blacklist_UpgradeCount as 黑名单升级次数,Blacklist_UpgradeCount as 黑名单降级次数 ,Blacklist_Time as 黑名单登记时间,Blacklist_Name as 黑名单原因,Blacklist_Remark as 黑名单备注 from View_BlackList_CarInfo_StaffInfo_CustomerInfo where " + sqlwhere + " order by Blacklist_ID";
                daochu(fileName, strsql);
            }

        }
        /// <summary>
        /// 导出Excel 的方法
        /// </summary>
        private void tslExport_Excel(string fileName, DataGridView myDGV)
        {
            try
            {

                string saveFileName = "";
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.DefaultExt = "xls";
                saveDialog.Filter = "Excel文件|*.xls";
                saveDialog.FileName = fileName;
                saveDialog.ShowDialog();
                saveFileName = saveDialog.FileName;
                if (saveFileName.IndexOf(":") < 0) return; //被点了取消 

                ISdao = true;
                groupBox1.Visible = true;
                btnSet.Text = "取消导出";
                progressBar1.Maximum = myDGV.SelectedRows.Count;
                progressBar1.Value = 0;
                label18.Text = "正在导出：" + fileName;
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                if (xlApp == null)
                {
                    MessageBox.Show("无法创建Excel对象，可能您的机子未安装Excel");
                    return;
                }

                Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1 

                Microsoft.Office.Interop.Excel.Range range = worksheet.get_Range("A1", "Z" + (myDGV.SelectedRows.Count + 10));//把Execl设置问文本格式
                range.NumberFormatLocal = "@";
                //写入标题
                for (int i = 1; i < myDGV.ColumnCount; i++)
                {
                    worksheet.Cells[1, i] = myDGV.Columns[i].HeaderText;
                }
                //写入数值
                int s = 0;
                for (int r = 0; r < myDGV.SelectedRows.Count; r++)
                {
                    if (ISdao)
                    {
                        for (int i = 1; i < myDGV.ColumnCount; i++)
                        {
                            worksheet.Cells[s + 2, i] = myDGV.SelectedRows[r].Cells[i].Value;
                        }
                        System.Windows.Forms.Application.DoEvents();
                        s++;
                        progressBar1.Value++;
                    }
                    else
                    {
                        break;
                    }
                }
                worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
                Microsoft.Office.Interop.Excel.Range rang = worksheet.get_Range(worksheet.Cells[2, 1], worksheet.Cells[myDGV.SelectedRows.Count + 2, 2]);
                rang.NumberFormat = "000000000000";

                if (saveFileName != "")
                {
                    try
                    {
                        workbook.Saved = true;
                        workbook.SaveCopyAs(saveFileName);
                    }
                    catch
                    {

                    }

                }
                xlApp.Quit();
                GC.Collect();//强行销毁 
                if (progressBar1.Value >= progressBar1.Maximum)
                {
                    label18.Text = fileName;
                    btnSet.Text = "导出完成";
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        /// <summary>
        /// 导入Execl
        /// </summary>
        private void InExecl()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Execl文件(*.xls)|*.xls";
            openFileDialog.Multiselect = false;
            openFileDialog.RestoreDirectory = true;
            try
            {
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string path = openFileDialog.FileName;
                    setExcelout(path);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("BlacklistForm.InExecl异常:");
            }
        }


        /// <summary>
        ///  将Excel中的数据导入到SQL数据库中
        /// </summary>
        /// <param name="path">路径</param>
        private void setExcelout(string path)
        {
            DataTable table = new DataTable();
            OleDbConnection dbcon = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=Excel 8.0");
            if (dbcon.State == ConnectionState.Closed)
            {
                dbcon.Open();
            }
            string sql = "select * from [Sheet1$]";
            OleDbCommand cmd = new OleDbCommand(sql, dbcon);
            OleDbDataReader sdr = cmd.ExecuteReader();
            table.Load(sdr);
            string str = "";
            string strName = "";
            string strcar = "";
            int Dictionary_ID = 0;
            string Blacklist_UpgradeCount = "";
            string Blacklist_DowngradeCount = "";

            string fileName = System.IO.Path.GetFileName(path);// 
            ISdao = true;
            groupBox1.Visible = true;
            btnSet.Text = "取消导入";
            progressBar1.Maximum = table.Rows.Count;
            progressBar1.Value = 0;
            label18.Text = "正在导入：" + fileName;

            for (int i = 0; i < table.Rows.Count; i++)
            {
                if (ISdao)
                {
                    str = table.Rows[i][0].ToString().Trim();
                    if (str == "车")
                    {
                        strName = table.Rows[i][2].ToString().Trim();
                        DataTable dt = LinQBaseDao.Query("select Car_ID from Car where Car_Name='" + strName + "'").Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            strcar = dt.Rows[0][0].ToString();
                        }
                        else
                        {
                            strcar = LinQBaseDao.GetSingle("insert Car(Car_Name,Car_State,Car_CreateTime,Car_ISRegister,Car_ISStaffInfo) values('" + strName + "','启动',GETDATE(),0,0)  select @@identity").ToString();

                        }
                        DataTable dts = LinQBaseDao.Query("select * from Blacklist  where  Blacklist_CarInfo_ID=" + strcar).Tables[0];

                        DataTable dtd = LinQBaseDao.Query(" select * from Dictionary where Dictionary_Name='" + table.Rows[i][1].ToString().Trim() + "'").Tables[0];
                        if (dtd.Rows.Count > 0)
                        {
                            Dictionary_ID = Convert.ToInt32(dtd.Rows[0][0].ToString());
                        }
                        if (!string.IsNullOrEmpty(table.Rows[i][7].ToString().Trim()))
                        {
                            Blacklist_UpgradeCount = table.Rows[i][7].ToString().Trim();
                        }
                        else
                        {
                            Blacklist_UpgradeCount = "NULL";
                        }
                        if (!string.IsNullOrEmpty(table.Rows[i][8].ToString().Trim()))
                        {
                            Blacklist_DowngradeCount = table.Rows[i][8].ToString().Trim();
                        }
                        else
                        {
                            Blacklist_DowngradeCount = "NULL";
                        }
                        if (dts.Rows.Count > 0)
                        {
                            LinQBaseDao.Query("update Blacklist set Blacklist_Type='" + str + "',Blacklist_State='" + table.Rows[i][1].ToString().Trim() + "',Blacklist_UpgradeCount=" + Blacklist_UpgradeCount + ",Blacklist_DowngradeCount=" + Blacklist_DowngradeCount + ",Blacklist_Time='" + table.Rows[i][9].ToString().Trim() + "',Blacklist_Name='" + table.Rows[i][10].ToString().Trim() + "',Blacklist_Remark='" + table.Rows[i][11].ToString().Trim() + "',Blacklist_Dictionary_ID=" + Dictionary_ID + " where Blacklist_CarInfo_ID=" + strcar);
                        }
                        else
                        {
                            LinQBaseDao.Query("insert into Blacklist(Blacklist_CarInfo_ID,Blacklist_Type,Blacklist_State,Blacklist_UpgradeCount,Blacklist_DowngradeCount,Blacklist_Time,Blacklist_Name,Blacklist_Remark,Blacklist_Dictionary_ID)  values (" + strcar + ",'" + str + "','" + table.Rows[i][1].ToString().Trim() + "'," + Blacklist_UpgradeCount + "," + Blacklist_DowngradeCount + ",'" + table.Rows[i][9].ToString().Trim() + "','" + table.Rows[i][10].ToString().Trim() + "','" + table.Rows[i][11].ToString().Trim() + "'," + Dictionary_ID + ")");
                        }
                    }
                    if (str == "公司")
                    {
                        strName = table.Rows[i][3].ToString().Trim();
                        DataTable dt = LinQBaseDao.Query("select CustomerInfo_ID from CustomerInfo where CustomerInfo_Name='" + strName + "'").Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            strcar = dt.Rows[0][0].ToString();
                        }
                        else
                        {
                            strcar = LinQBaseDao.GetSingle("insert CustomerInfo(CustomerInfo_Name,CustomerInfo_State,CustomerInfo_Time) values('" + strName + "','启动',GETDATE())  select @@identity").ToString();

                        }
                        DataTable dts = LinQBaseDao.Query("select * from Blacklist  where  Blacklist_CustomerInfo_ID=" + strcar).Tables[0];

                        DataTable dtd = LinQBaseDao.Query(" select * from Dictionary where Dictionary_Name='" + table.Rows[i][1].ToString().Trim() + "'").Tables[0];
                        if (dtd.Rows.Count > 0)
                        {
                            Dictionary_ID = Convert.ToInt32(dtd.Rows[0][0].ToString());
                        }
                        if (!string.IsNullOrEmpty(table.Rows[i][7].ToString().Trim()))
                        {
                            Blacklist_UpgradeCount = table.Rows[i][7].ToString().Trim();
                        }
                        else
                        {
                            Blacklist_UpgradeCount = "NULL";
                        }
                        if (!string.IsNullOrEmpty(table.Rows[i][8].ToString().Trim()))
                        {
                            Blacklist_DowngradeCount = table.Rows[i][8].ToString().Trim();
                        }
                        else
                        {
                            Blacklist_DowngradeCount = "NULL";
                        }
                        if (dts.Rows.Count > 0)
                        {
                            LinQBaseDao.Query("update Blacklist set Blacklist_Type='" + str + "',Blacklist_State='" + table.Rows[i][1].ToString().Trim() + "',Blacklist_UpgradeCount=" + Blacklist_UpgradeCount + ",Blacklist_DowngradeCount=" + Blacklist_DowngradeCount + ",Blacklist_Time='" + table.Rows[i][9].ToString().Trim() + "',Blacklist_Name='" + table.Rows[i][10].ToString().Trim() + "',Blacklist_Remark='" + table.Rows[i][11].ToString().Trim() + "',Blacklist_Dictionary_ID=" + Dictionary_ID + " where Blacklist_CustomerInfo_ID=" + strcar);
                        }
                        else
                        {
                            LinQBaseDao.Query("insert into Blacklist(Blacklist_CustomerInfo_ID,Blacklist_Type,Blacklist_State,Blacklist_UpgradeCount,Blacklist_DowngradeCount,Blacklist_Time,Blacklist_Name,Blacklist_Remark,Blacklist_Dictionary_ID)  values (" + strcar + ",'" + str + "','" + table.Rows[i][1].ToString().Trim() + "'," + Blacklist_UpgradeCount + "," + Blacklist_DowngradeCount + ",'" + table.Rows[i][9].ToString().Trim() + "','" + table.Rows[i][10].ToString().Trim() + "','" + table.Rows[i][11].ToString().Trim() + "'," + Dictionary_ID + ")");
                        }
                    }
                    if (str == "人员")
                    {
                        strName = table.Rows[i][4].ToString().Trim();
                        DataTable dt = LinQBaseDao.Query("select StaffInfo_ID from StaffInfo where StaffInfo_Name='" + strName + "' and StaffInfo_Identity='" + table.Rows[i][5].ToString().Trim() + "'").Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            strcar = dt.Rows[0][0].ToString();
                            LinQBaseDao.Query("update StaffInfo  set  StaffInfo_Phone='" + table.Rows[i][6].ToString().Trim() + "'  where StaffInfo_ID=" + strcar);
                        }
                        else
                        {
                            strcar = LinQBaseDao.GetSingle("Insert into StaffInfo(StaffInfo_Type,StaffInfo_Name,StaffInfo_Phone,StaffInfo_Time,StaffInfo_State,StaffInfo_Identity) values('驾驶员','" + table.Rows[i][4].ToString().Trim() + "','" + table.Rows[i][6].ToString().Trim() + "',GETDATE(),'启动','" + table.Rows[i][5].ToString().Trim() + "')      select @@identity").ToString();
                        }

                        DataTable dts = LinQBaseDao.Query("select * from Blacklist  where  Blacklist_StaffInfo_ID=" + strcar).Tables[0];

                        DataTable dtd = LinQBaseDao.Query(" select * from Dictionary where Dictionary_Name='" + table.Rows[i][1].ToString().Trim() + "'").Tables[0];
                        if (dtd.Rows.Count > 0)
                        {
                            Dictionary_ID = Convert.ToInt32(dtd.Rows[0][0].ToString());
                        }
                        if (!string.IsNullOrEmpty(table.Rows[i][7].ToString().Trim()))
                        {
                            Blacklist_UpgradeCount = table.Rows[i][7].ToString().Trim();
                        }
                        else
                        {
                            Blacklist_UpgradeCount = "NULL";
                        }
                        if (!string.IsNullOrEmpty(table.Rows[i][8].ToString().Trim()))
                        {
                            Blacklist_DowngradeCount = table.Rows[i][8].ToString().Trim();
                        }
                        else
                        {
                            Blacklist_DowngradeCount = "NULL";
                        }
                        if (dts.Rows.Count > 0)
                        {
                            LinQBaseDao.Query("update Blacklist set Blacklist_Type='" + str + "',Blacklist_State='" + table.Rows[i][1].ToString().Trim() + "',Blacklist_UpgradeCount=" + Blacklist_UpgradeCount + ",Blacklist_DowngradeCount=" + Blacklist_DowngradeCount + ",Blacklist_Time='" + table.Rows[i][9].ToString().Trim() + "',Blacklist_Name='" + table.Rows[i][10].ToString().Trim() + "',Blacklist_Remark='" + table.Rows[i][11].ToString().Trim() + "',Blacklist_Dictionary_ID=" + Dictionary_ID + " where Blacklist_StaffInfo_ID=" + strcar);
                        }
                        else
                        {
                            LinQBaseDao.Query("insert into Blacklist(Blacklist_StaffInfo_ID,Blacklist_Type,Blacklist_State,Blacklist_UpgradeCount,Blacklist_DowngradeCount,Blacklist_Time,Blacklist_Name,Blacklist_Remark,Blacklist_Dictionary_ID)  values (" + strcar + ",'" + str + "','" + table.Rows[i][1].ToString().Trim() + "'," + Blacklist_UpgradeCount + "," + Blacklist_DowngradeCount + ",'" + table.Rows[i][9].ToString().Trim() + "','" + table.Rows[i][10].ToString().Trim() + "','" + table.Rows[i][11].ToString().Trim() + "'," + Dictionary_ID + ")");
                        }
                    }
                    progressBar1.Value++;
                }
                else
                {
                    break;
                }
            }
            if (dbcon.State == ConnectionState.Open)
            {
                dbcon.Close();
            }
            if (progressBar1.Value == table.Rows.Count)
            {
                label18.Text = fileName;
                btnSet.Text = "导入完成";
            }
            CommonalityEntity.WriteLogData("修改", "黑名单导入Execl信息", CommonalityEntity.USERNAME);
        }

        private void button1_Click(object sender, EventArgs e)
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
                txtContor.Text = dt.Rows[0]["姓名"].ToString();
                txtStaffInfo_Identity.Text = dt.Rows[0]["公民身份号码"].ToString();
                DBHelperAccess.Query("update iDRTable set [追加地址4]='1' where [追加地址4] is null");
            }
        }

        bool ISdao = true;
        /// <summary>
        /// 取消导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSet_Click(object sender, EventArgs e)
        {
            if (btnSet.Text == "取消导出")
            {
                ISdao = false;
            }
            else
            {
                btnSet.Text = "取消导出";
            }
            groupBox1.Visible = false;
        }

        private void dgvBlackList_Click(object sender, EventArgs e)
        {
            ISExecl = false;
        }

        /// <summary>
        /// 导出Execl 全选
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="tablename"></param>
        /// <param name="strsql"></param>
        private void daochu(string filename, string strsql)
        {
            try
            {
                string saveFileName = "";
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.DefaultExt = "xls";
                saveDialog.Filter = "Excel文件|*.xls";
                saveDialog.FileName = filename;
                saveDialog.ShowDialog();
                saveFileName = saveDialog.FileName;
                if (saveFileName.IndexOf(":") < 0) return; //被点了取消 

                ISdao = true;
                groupBox1.Visible = true;
                btnSet.Text = "取消导出";

                label18.Text = "正在导出：" + filename;

                ExcelFile excelFile = new ExcelFile();
                //ExcelWorksheet sheet = excelFile.Worksheets.Add(filename);
                DataTable table = LinQBaseDao.Query(strsql).Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;
                double ss = (double)rows / 60000;
                int ii = Convert.ToInt32(ss);
                if (ss > ii)
                {
                    ii++;
                }
                progressBar1.Maximum = ii + 1;
                progressBar1.Value = 0;
                int y = 0;
                for (int s = 1; s < ii + 1; s++)
                {
                    ExcelWorksheet sheet = excelFile.Worksheets.Add("Sheet" + s.ToString());
                    for (int j = 0; j < columns; j++)
                    {
                        sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                    }
                    for (int i = 0; i < columns; i++)
                    {
                        if (ISdao)
                        {
                            y = 0;
                            for (int j = (s - 1) * 60000; j < rows; j++)
                            {
                                y++;
                                if (y > 60000)
                                {
                                    y = 0;
                                    break;
                                }
                                sheet.Cells[y, i].Value = table.Rows[j][i].ToString();
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (!ISdao)
                    {
                        break;
                    }
                    progressBar1.Value++;
                }
                excelFile.SaveXls(saveFileName);
                progressBar1.Value++;
                if (progressBar1.Value >= progressBar1.Maximum)
                {
                    label18.Text = filename;
                    btnSet.Text = "导出完成";
                }
            }
            catch { }
        }

    }
}
