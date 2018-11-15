using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.HelpClass;
using EMEWE.CarManagement.DAL;
using System.Linq.Expressions;
using System.Data.Linq.SqlClient;
using EMEWE.CarManagementDAL;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.CommonClass;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class DrivewayFrom : Form
    {
        public MainForm mf;//主窗体
        /// <summary>
        /// 返回一个空的预置皮重
        /// </summary>
        Expression<Func<View_DrivewayPosition, bool>> expr = null;
        /// <summary>
        /// 实例化分页控件
        /// </summary>
        PageControl page = new PageControl();
        // 定义一个公共的变量： 门岗编号
        public int iDrivewayPositionID = 0;
        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere = " 1=1 ";
        // 定义一个全局变量： 通道编号
        int iDrivewayID = 0;
        /// <summary>
        /// 保存最大通道值
        /// </summary>
        private string MaxTDZ;

        private string bdwayState = "";

        public DrivewayFrom()
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
                btnDel.Enabled = true;
                btnDel.Visible = true;
                btnUpdate.Enabled = true;
                btnUpdate.Visible = true;
                btnFVN.Enabled = true;
                btnFVN.Visible = true;
                btnCamera.Enabled = true;
                btnCamera.Visible = true;
            }
            else
            {
                btnSave.Visible = ControlAttributes.BoolControl("btnSave", "DrivewayFrom", "Visible");
                btnSave.Enabled = ControlAttributes.BoolControl("btnSave", "DrivewayFrom", "Enabled");

                btnDel.Visible = ControlAttributes.BoolControl("btnDel", "DrivewayFrom", "Visible");
                btnDel.Enabled = ControlAttributes.BoolControl("btnDel", "DrivewayFrom", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "DrivewayFrom", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "DrivewayFrom", "Enabled");

                btnFVN.Visible = ControlAttributes.BoolControl("btnFVN", "DrivewayFrom", "Visible");
                btnFVN.Enabled = ControlAttributes.BoolControl("btnFVN", "DrivewayFrom", "Enabled");

                btnCamera.Visible = ControlAttributes.BoolControl("btnCamera", "DrivewayFrom", "Visible");
                btnCamera.Enabled = ControlAttributes.BoolControl("btnCamera", "DrivewayFrom", "Enabled");
            }
        }
        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrivewayFrom_Load(object sender, EventArgs e)
        {
            try
            {
                userContext();
                cbodizhima.SelectedIndex = 0;
                cbmcloseAdd.SelectedIndex = 1;
                btnUpdate.Enabled = false;
                btnSelect_Click(btnSelect, null);
                mf = new MainForm();
                tscbxPageSize.SelectedIndex = 1;
                // LoadData();
                BindDriveway();
                BindSearchDriveway();
                BindPositionName();
                BindSearchWarrantState(); // 绑定搜索的通道报修状态
                BindPositionType();
                BindDrivewayWarrantState();
                BindRemarkDriveway();
                Bindmengang();
                if (iDrivewayPositionID > 0)
                {
                    // 若门岗管理界面中有选择“门岗”再跳转的，则显示门岗如下
                    Expression<Func<Position, bool>> funviewinto = n => n.Position_ID == iDrivewayPositionID;
                    foreach (var n in PositionDAL.Query(funviewinto))
                    {
                        if (n.Position_Name != null)
                        {
                            //门岗名称
                            this.cbxDriveway_PositionName.Text = n.Position_Name;
                        }
                    }
                }

            }
            catch
            {
                MessageBox.Show("加载通道信息有误，请查看与通道相关的信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 菜单栏加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.dgvDrivewayList.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
                this.dgvDrivewayList.DataSource = null;
                LogInfoLoad("");
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayFrom LoadData()");
            }
        }
        #region  comboBox 下拉框的绑定
        /// <summary>
        /// 绑定通道状态
        /// </summary>
        private void BindDriveway()
        {
            try
            {
                this.cbxDriveway_State.DataSource = DictionaryDAL.GetValueDictionary("01").Where(n => n.Dictionary_Name != "全部").ToList();

                if (this.cbxDriveway_State.DataSource != null)
                {
                    this.cbxDriveway_State.DisplayMember = "Dictionary_Name";
                    this.cbxDriveway_State.ValueMember = "Dictionary_ID";
                    this.cbxDriveway_State.SelectedValue = -1;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayFrom BindDriveway()");
            }
        }
        /// <summary>
        /// 绑定备用通道
        /// </summary>
        private void BindRemarkDriveway()
        {
            try
            {
                string sql = "Select (Position_Name+Driveway_Name) as DrivewayPosition,Driveway_ID from dbo.View_DrivewayPosition where Driveway_State='启动' and Position_State='启动'";
                DataTable dt = LinQBaseDao.Query(sql).Tables[0];
                DataRow row = dt.NewRow();
                row["DrivewayPosition"] = "请选择";
                dt.Rows.InsertAt(row, 0);
                if (dt.Rows.Count > 0)
                {
                    this.cobReserve.DataSource = dt;
                    this.cobReserve.DisplayMember = "DrivewayPosition";
                    this.cobReserve.ValueMember = "Driveway_ID";
                    cobReserve.SelectedIndex = 0;
                }
                else
                {
                    return;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayFrom .BindRemarkDriveway()");
            }
        }
        /// <summary>
        /// 搜索--绑定通道状态
        /// </summary>
        private void BindSearchDriveway()
        {
            try
            {
                this.cbxDrivewayState.DataSource = DictionaryDAL.GetValueStateDictionary("01");

                if (this.cbxDrivewayState.DataSource != null)
                {
                    this.cbxDrivewayState.DisplayMember = "Dictionary_Name";
                    this.cbxDrivewayState.ValueMember = "Dictionary_ID";
                    this.cbxDrivewayState.SelectedIndex = -1;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayFrom .BindSearchDriveway()");
            }
        }
        /// <summary>
        /// 绑定操作的门岗名称
        /// </summary>
        bool boo = false;
        private void BindPositionName()
        {
            try
            {
                string sql = "select * from Position where Position_State='启动'";
                this.cbxDriveway_PositionName.DataSource = PositionDAL.GetViewPosition(sql);

                if (PositionDAL.GetViewPosition(sql).Count() > 0)
                {
                    this.cbxDriveway_PositionName.DisplayMember = "Position_Name";
                    this.cbxDriveway_PositionName.ValueMember = "Position_ID";
                    cbxDriveway_PositionName.SelectedIndex = 0;
                }
                else
                {
                    return;
                }

                //在绑定门岗完成后，绑定一个此门岗的最大通道值+1
                int drid = int.Parse(cbxDriveway_PositionName.SelectedValue.ToString());
                Expression<Func<Driveway, bool>> funuserinfo = p => p.Driveway_Position_ID == drid;

                DataTable dt = LinQBaseDao.Query("select MAX(Driveway_Value) from Driveway where Driveway_Position_ID=" + drid).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    int num = Convert.ToInt32(dt.Rows[0][0].ToString());
                    txtDriveway_Value.Text = "0" + (num + 1).ToString();
                }

                boo = true;

            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayFrom .BindPositionName()");
            }
        }
        private void Bindmengang()
        {
            try
            {
                string sql = "select * from Position where Position_State='启动'";
                DataTable dt = LinQBaseDao.Query(sql).Tables[0];
                DataRow dr = dt.NewRow();
                dr["Position_ID"] = "0";
                dr["Position_Name"] = "全部";
                dt.Rows.InsertAt(dr, 0);
                this.cmbmengang.DataSource = dt;
                this.cmbmengang.DisplayMember = "Position_Name";
                this.cmbmengang.ValueMember = "Position_ID";
                cmbmengang.SelectedIndex = 0;

            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayFrom .BindPositionName()");
            }
        }
        /// <summary>
        /// 通道类型的绑定
        /// </summary>
        private void BindPositionType()
        {
            try
            {
                this.cbxDriveway_Type.DataSource = DictionaryDAL.GetValueDictionary("16").Where(n => n.Dictionary_Name != "全部").ToList();

                if (this.cbxDriveway_Type.DataSource != null)
                {
                    this.cbxDriveway_Type.DisplayMember = "Dictionary_Name";
                    this.cbxDriveway_Type.ValueMember = "Dictionary_ID";
                    this.cbxDriveway_Type.SelectedValue = -1;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayFrom .BindPositionType()");
            }
        }
        /// <summary>
        /// 通道报修状态的绑定
        /// </summary>
        private void BindDrivewayWarrantState()
        {
            try
            {
                this.comboxDriveway_WarrantyState.DataSource = DictionaryDAL.GetValueDictionary("15").Where(n => n.Dictionary_Name != "全部").ToList();

                if (this.comboxDriveway_WarrantyState.DataSource != null)
                {
                    this.comboxDriveway_WarrantyState.DisplayMember = "Dictionary_Name";
                    this.comboxDriveway_WarrantyState.ValueMember = "Dictionary_ID";
                    this.comboxDriveway_WarrantyState.SelectedValue = -1;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayFrom .BindDrivewayWarrantState()");
            }
        }
        /// <summary>
        /// 通道报修状态的绑定
        /// </summary>
        private void BindSearchWarrantState()
        {
            try
            {
                this.comboxDrivewayWarrantyState.DataSource = DictionaryDAL.GetValueDictionary("15");

                if (this.comboxDrivewayWarrantyState.DataSource != null)
                {
                    this.comboxDrivewayWarrantyState.DisplayMember = "Dictionary_Name";
                    this.comboxDrivewayWarrantyState.ValueMember = "Dictionary_ID";
                    this.comboxDrivewayWarrantyState.SelectedValue = -1;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayFrom .BindSearchWarrantState()");
            }
        }
        /// <summary>
        /// 绑定查询门岗
        /// </summary>
        private void BindMG()
        {
            try
            {
                string sql = "select * from Position where Position_State='启动'";

                DataTable dt = LinQBaseDao.Query(sql).Tables[0];
                DataRow dr = dt.NewRow();
                dr["Position_ID"] = "0";
                dr["Position_Name"] = "全部";
                dt.Rows.InsertAt(dr, 0);
                this.cbxDriveway_PositionName.DataSource = dt;
                this.cbxDriveway_PositionName.DisplayMember = "Position_Name";
                this.cbxDriveway_PositionName.ValueMember = "Position_ID";
                cbxDriveway_PositionName.SelectedIndex = 0;
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
                //定义字段用以保存门岗名称和通道值，读卡器编号
                string Position_Name = this.cbxDriveway_PositionName.Text;//门岗名称
                string Driveway_Value = this.txtDriveway_Value.Text.Trim();//通道值
                string Driveway_Name = this.txtDriveway_Name.Text.Trim();
                string Driveway_ReadCardPort = this.txtDriveway_ReadCardPort.Text.Trim();//通道读卡器地址码
                //判断名称是否已存在
                Expression<Func<View_DrivewayPosition, bool>> funviewPosition = n => n.Position_Name == Position_Name && n.Driveway_Value == Driveway_Value;
                if (DrivewayDAL.Query(funviewPosition).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该通道值已存在", txtDriveway_Value, this);
                    txtDriveway_Value.Focus();
                    rbool = false; ;
                }
                Expression<Func<View_DrivewayPosition, bool>> funviewPosition2 = n => n.Position_Name == Position_Name && n.Driveway_Name == Driveway_Name;
                if (DrivewayDAL.Query(funviewPosition2).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该通道名称已存在", txtDriveway_Value, this);
                    txtDriveway_Value.Focus();
                    rbool = false; ;
                }
                Expression<Func<View_DrivewayPosition, bool>> funviewPosition1 = n => n.Position_Name == Position_Name && n.Driveway_ReadCardPort == Driveway_ReadCardPort;
                if (DrivewayDAL.Query(funviewPosition1).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该通道读卡器地址码已存在", txtDriveway_ReadCardPort, this);
                    txtDriveway_ReadCardPort.Focus();
                    rbool = false; ;
                }
                return rbool;

            }
            catch
            {
                CommonalityEntity.WriteTextLog("通道管理 btnCheck()");
                rbool = false;
            }
            return rbool;
        }
        /// <summary>
        /// 修改查重
        /// </summary>
        /// <returns></returns>
        private bool btnCheckupdate()
        {
            bool rbool = true;
            try
            {
                //定义字段用以保存门岗名称和通道值，读卡器编号
                string Position_Name = this.cbxDriveway_PositionName.Text;//门岗名称
                string Driveway_Value = this.txtDriveway_Value.Text.Trim();//通道值
                string Driveway_Name = this.txtDriveway_Name.Text.Trim();
                string Driveway_ReadCardPort = this.txtDriveway_ReadCardPort.Text.Trim();//通道读卡器地址码
                //判断名称是否已存在
                Expression<Func<View_DrivewayPosition, bool>> funviewPosition = n => n.Position_Name == Position_Name && n.Driveway_Value == Driveway_Value && n.Driveway_Value != this.dgvDrivewayList.SelectedRows[0].Cells["Driveway_Value"].Value.ToString();
                if (DrivewayDAL.Query(funviewPosition).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该通道值已存在", txtDriveway_Value, this);
                    txtDriveway_Value.Focus();
                    rbool = false; ;
                }
                Expression<Func<View_DrivewayPosition, bool>> funviewPosition2 = n => n.Position_Name == Position_Name && n.Driveway_Name == Driveway_Name && n.Driveway_Value != this.dgvDrivewayList.SelectedRows[0].Cells["Driveway_Value"].Value.ToString();
                if (DrivewayDAL.Query(funviewPosition2).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该通道名称已存在", txtDriveway_Value, this);
                    txtDriveway_Value.Focus();
                    rbool = false; ;
                }
                Expression<Func<View_DrivewayPosition, bool>> funviewPosition1 = n => n.Position_Name == Position_Name && n.Driveway_ReadCardPort == Driveway_ReadCardPort && n.Driveway_ReadCardPort != this.dgvDrivewayList.SelectedRows[0].Cells["Driveway_ReadCardPort"].Value.ToString();
                if (DrivewayDAL.Query(funviewPosition1).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该通道读卡器地址码已存在", txtDriveway_ReadCardPort, this);
                    txtDriveway_ReadCardPort.Focus();
                    rbool = false; ;
                }
                return rbool;

            }
            catch
            {
                CommonalityEntity.WriteTextLog("通道管理 btnCheck()");
                rbool = false;
            }
            return rbool;
        }
        int RDId;
        /// <summary>
        /// “保存” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtDriveway_Name.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "通道名称不能为空！", txtDriveway_Name, this);
                    return;
                }
                if (this.txtDriveway_ReadCardPort.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "通道读卡器地址码不能为空！", txtDriveway_ReadCardPort, this);
                    return;
                }
                //DataTable tables = LinQBaseDao.Query("select * from Driveway where Driveway_Value='" + txtDriveway_Value.Text.Trim() + "'").Tables[0];
                //if (tables.Rows.Count > 0)
                //{
                //    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "通道值重复！", txtDriveway_Name, this);
                //    return;
                //}
                //DataTable table = LinQBaseDao.Query("select * from Driveway where Driveway_Name='" + txtDriveway_Name.Text.Trim() + "' and Driveway_Position_ID=" + cbxDriveway_PositionName.SelectedValue).Tables[0];
                //if (table.Rows.Count > 0)
                //{
                //    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "通道名称重复！", txtDriveway_Name, this);
                //    return;
                //}

                if (!btnCheck()) return; // 去重复
                var Drivewayadd = new Driveway();

                Drivewayadd.Driveway_Name = this.txtDriveway_Name.Text.Trim();
                Drivewayadd.Driveway_Value = this.txtDriveway_Value.Text.Trim();
                Drivewayadd.Driveway_State = this.cbxDriveway_State.Text;
                Drivewayadd.Driveway_Type = this.cbxDriveway_Type.Text;
                Drivewayadd.Driveway_WarrantyState = this.comboxDriveway_WarrantyState.Text;
                if (cobReserve.SelectedIndex != 0 && cobReserve.SelectedIndex != -1)
                {
                    Drivewayadd.Driveway_Remark_Driveway_ID = Convert.ToInt32((cobReserve.SelectedValue.ToString()));
                }
                Drivewayadd.Driveway_Position_ID = int.Parse(this.cbxDriveway_PositionName.SelectedValue.ToString());
                Drivewayadd.Driveway_UserId = int.Parse(common.USERID);
                Drivewayadd.Driveway_CreatTime = Convert.ToDateTime(CommonalityEntity.GetServersTime().ToString());
                Drivewayadd.Driveway_Address = this.cbodizhima.Text.Trim();
                Drivewayadd.Driveway_ReadCardPort = this.txtDriveway_ReadCardPort.Text.Trim();
                Drivewayadd.Driveway_Remark = this.txtDriveway_Remark.Text.Trim();
                Drivewayadd.Driveway_CloseAddress = cbmcloseAdd.Text.Trim();
                if (DrivewayDAL.InsertOneQCRecord(Drivewayadd))
                {
                    MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string strContent1 = "通道名称为：" + this.cbxDriveway_PositionName.SelectedText + this.txtDriveway_Name.Text.Trim();
                    CommonalityEntity.WriteLogData("新增", "新增 " + strContent1 + " 的信息", common.USERNAME);//添加日志

                    //重新绑定通道值
                    tongdaoZ();
                }
                else
                {
                    MessageBox.Show("添加失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("通道管理 btnSave_Click()");
            }
            finally
            {
                LogInfoLoad("");
            }
        }

        /// <summary>
        /// “修改” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtDriveway_Name.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "通道名称不能为空！", txtDriveway_Name, this);
                    return;
                }
                if (this.txtDriveway_ReadCardPort.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "通道读卡器地址码不能为空！", txtDriveway_ReadCardPort, this);
                    return;
                }
                if (this.dgvDrivewayList.SelectedRows.Count > 0)//选中行
                {
                    if (dgvDrivewayList.SelectedRows.Count > 1)
                    {
                        MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (!btnCheckupdate()) return; // 去重复
                        int driveway_id = int.Parse(this.dgvDrivewayList.SelectedRows[0].Cells["Driveway_ID"].Value.ToString());
                        Expression<Func<Driveway, bool>> p = n => n.Driveway_ID == driveway_id;
                        string id = "";
                        string strfront = "";
                        string strContent = "";
                        Action<Driveway> ap = s =>
                        {
                            strfront = s.Driveway_Name + "," + s.Driveway_Value + "," + s.Driveway_State + "," + s.Driveway_Type + "," + s.Driveway_WarrantyState + "," + s.Driveway_Remark_Driveway_ID + "," + s.Driveway_Position_ID + "," + s.Driveway_UserId + "," + s.Driveway_CreatTime + "," + s.Driveway_Add + "," + s.Driveway_ReadCardPort + "," + s.Driveway_Remark;
                            s.Driveway_Name = this.txtDriveway_Name.Text.Trim();
                            s.Driveway_Value = this.txtDriveway_Value.Text.Trim();
                            s.Driveway_State = this.cbxDriveway_State.Text;
                            s.Driveway_Type = this.cbxDriveway_Type.Text;
                            s.Driveway_WarrantyState = this.comboxDriveway_WarrantyState.Text;
                            if (cobReserve.SelectedIndex != 0 && cobReserve.SelectedIndex != -1)
                            {
                                s.Driveway_Remark_Driveway_ID = Convert.ToInt32(cobReserve.SelectedValue.ToString());
                            }
                            s.Driveway_Position_ID = int.Parse(this.cbxDriveway_PositionName.SelectedValue.ToString());
                            s.Driveway_UserId = int.Parse(common.USERID);
                            s.Driveway_CreatTime = Convert.ToDateTime(CommonalityEntity.GetServersTime().ToString());
                            s.Driveway_Address = this.cbodizhima.Text.Trim();
                            s.Driveway_ReadCardPort = this.txtDriveway_ReadCardPort.Text.Trim();
                            s.Driveway_Remark = this.txtDriveway_Remark.Text.Trim();
                            s.Driveway_CloseAddress = cbmcloseAdd.Text.Trim();
                            strContent = s.Driveway_Name + "," + s.Driveway_Value + "," + s.Driveway_State + "," + s.Driveway_Type + "," + s.Driveway_WarrantyState + "," + s.Driveway_Remark_Driveway_ID + "," + s.Driveway_Position_ID + "," + s.Driveway_UserId + "," + s.Driveway_CreatTime + "," + s.Driveway_Add + "," + s.Driveway_ReadCardPort + "," + s.Driveway_Remark;
                            id = s.Driveway_ID.ToString();
                        };

                        if (DrivewayDAL.Update(p, ap))
                        {
                            #region 当通道报修时启用备用通道，修改通行策略
                            //if (comboxDriveway_WarrantyState.Text == "报修")
                            //{

                            //    if (cobReserve.SelectedIndex > 0)
                            //    {
                            //        string drisid = ""; int bstate = 0;
                            //        if (bdwayState == "报修")
                            //        {
                            //            bstate = 1;
                            //        }
                            //        else
                            //        {
                            //            bstate = 0;
                            //        }
                            //        DataTable dtdris = LinQBaseDao.Query("select DrivewayStrategy_ID from DrivewayStrategy where DrivewayStrategy_Driveway_ID=" + id).Tables[0];
                            //        if (dtdris.Rows.Count > 0)
                            //        {
                            //            # region 通道报修，记录要通行该通道的车辆通行策略，并修改ID关联
                            //            for (int i = 0; i < dtdris.Rows.Count; i++)
                            //            {
                            //                drisid += dtdris.Rows[i][0].ToString() + ",";
                            //            }
                            //            drisid = drisid.TrimEnd(',');
                            //            DateTime dtime = CommonalityEntity.GetServersTime();
                            //            LinQBaseDao.Query("insert into DrivewayRecord (DrivewayRecord_Driveway_ID,DrivewayRecord_Content,DrivewayRecord_State,DrivewayRecord_Time,DrivewayRecord_Person ) values(" + id + ",'" + drisid + "'," + bstate + ",'" + dtime + "','" + CommonalityEntity.USERNAME + "')");
                            //            LinQBaseDao.Query("update DrivewayStrategy set DrivewayStrategy_Driveway_ID=" + cobReserve.SelectedValue.ToString() + " where DrivewayStrategy_ID in (" + drisid + ")");
                            //            #endregion
                            //        }
                            //        else
                            //        {
                            //            #region  通道已是报修状态修改备用通道
                            //            if (bstate == 1)
                            //            {
                            //                if (cobReserve.SelectedIndex > -1)
                            //                {
                            //                    DataTable dtdrisrd = LinQBaseDao.Query("select DrivewayRecord_Content from DrivewayRecord where DrivewayRecord_Driveway_ID=" + id + " order by DrivewayRecord_ID desc").Tables[0];
                            //                    if (dtdrisrd.Rows.Count > 0)
                            //                    {
                            //                        string dcont = dtdrisrd.Rows[0][0].ToString();
                            //                        LinQBaseDao.Query("update DrivewayStrategy set DrivewayStrategy_Driveway_ID=" + cobReserve.SelectedValue.ToString() + " where DrivewayStrategy_ID in(" + dcont + ")");
                            //                    }
                            //                }
                            //            }
                            //            #endregion
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    #region 通道由报修状态修改为正常状态 修改通行策略里的关联ID
                            //    if (bdwayState != "正常")
                            //    {
                            //        DataTable dtdris = LinQBaseDao.Query("select DrivewayRecord_Content from DrivewayRecord where DrivewayRecord_State=0 and DrivewayRecord_Driveway_ID=" + id + " order by DrivewayRecord_ID desc").Tables[0];
                            //        if (dtdris.Rows.Count > 0)
                            //        {
                            //            string dcont = dtdris.Rows[0][0].ToString();
                            //            LinQBaseDao.Query("update DrivewayStrategy set DrivewayStrategy_Driveway_ID=" + id + " where DrivewayStrategy_ID in(" + dcont + ")");
                            //        }
                            //    }
                            //    #endregion 
                            //}
                            #endregion

                            MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CommonalityEntity.WriteLogData("修改", "更新编号为：" + id + "的通道信息；修改前：" + strfront + "；修改后：" + strContent, common.USERNAME);//添加日志
                        }
                        else
                        {
                            MessageBox.Show("修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("通道管理 btnUpdate_Click()");
            }
            finally
            {
                LogInfoLoad("");
                this.btnUpdate.Enabled = false;
                this.btnSave.Enabled = true;
            }
        }

        /// <summary>
        /// “删除” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            tbtnDelPosition(); // 调用 删除选中行数据的方法
        }
        /// <summary>
        /// 删除 选中行数据的方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbtnDelPosition()
        {
            try
            {
                int j = 0;
                if (dgvDrivewayList.SelectedRows.Count > 0)//选中删除
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //选中数量
                        int count = dgvDrivewayList.SelectedRows.Count;
                        string id = "";
                        //遍历
                        for (int i = 0; i < count; i++)
                        {
                            //-----------------------------------------------------------------------------------------
                            int drid = int.Parse(this.dgvDrivewayList.SelectedRows[i].Cells["Driveway_ID"].Value.ToString());
                            Expression<Func<Driveway, bool>> funuserinfo = n => n.Driveway_ID == drid;
                            string strContent = LinQBaseDao.Query("Select (Position_Name+Driveway_Name) as DrivewayPosition from View_DrivewayPosition where Driveway_ID=" + drid).Tables[0].Rows[0][0].ToString();
                            if (DrivewayDAL.DeleteToMany(funuserinfo))
                            {
                                j++;
                                CommonalityEntity.WriteLogData("删除", "删除通道名称为：" + strContent + " 的信息", common.USERNAME);//添加日志
                            }
                        }
                        if (j == count)
                        {
                            MessageBox.Show("成功删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            //重新绑定通道值
                            tongdaoZ();
                        }
                        else
                        {
                            MessageBox.Show("删除失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                    }
                }
                else//没有选中
                {
                    MessageBox.Show("请选择要删除的行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("通道管理 tbtnDelPosition()+");
            }
            finally
            {
                LogInfoLoad("");
            }
        }


        /// <summary>
        /// 清空 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            boo = false;
            Empty();
            this.btnUpdate.Enabled = false;
            this.btnSave.Enabled = true;
            boo = true;
        }
        /// <summary>
        /// 清空的方法
        /// </summary>
        private void Empty()
        {
            this.txtDriveway_Name.Text = "";
            //this.txtDriveway_Value.Text = "";
            tongdaoZ();
            this.cbxDriveway_State.SelectedIndex = 0;
            this.cbxDriveway_Type.SelectedIndex = 0;
            this.cobReserve.SelectedIndex = 0;
            this.cbxDriveway_PositionName.SelectedIndex = 0;
            this.cbodizhima.SelectedIndex = 0;
            this.txtDriveway_ReadCardPort.Text = "";
            this.txtDriveway_Remark.Text = "";
        }

        /// <summary>
        /// 搜 索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (btnSelect.Enabled)
            {
                selectTJ();
            }
        }
        /// <summary>
        /// 搜索的方法
        /// </summary>
        private void selectTJ()
        {

            try
            {
                sqlwhere = "  1=1";
                string name = this.txtDrivewayName.Text.Trim();
                string state = this.cbxDrivewayState.Text.Trim();
                string value = this.cmbmengang.Text.Trim();
                string WarrantyState = this.comboxDrivewayWarrantyState.Text.Trim();

                if (!string.IsNullOrEmpty(WarrantyState))//通道报修状态
                {
                    if (WarrantyState != "全部")
                    {
                        sqlwhere += String.Format(" and Driveway_WarrantyState =  '{0}'", WarrantyState);
                    }
                }
                if (!string.IsNullOrEmpty(state))//通道状态
                {
                    if (state != "全部")
                    {
                        sqlwhere += String.Format(" and Driveway_State =  '%{0}%'", state);
                    }
                }
                if (!string.IsNullOrEmpty(name))//通道名称
                {
                    sqlwhere += String.Format(" and Driveway_Name like  '%{0}%'", name);
                }
                if (!string.IsNullOrEmpty(value) && value != "全部")//门岗
                {
                    sqlwhere += String.Format(" and Position_Name =  '{0}'", value);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayForm.selectTJ异常:");
            }
            finally
            {
                LogInfoLoad("");
            }
        }

        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslNotSelect()
        {
            for (int i = 0; i < this.dgvDrivewayList.Rows.Count; i++)
            {
                dgvDrivewayList.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslSelectAll()
        {
            for (int i = 0; i < dgvDrivewayList.Rows.Count; i++)
            {
                this.dgvDrivewayList.Rows[i].Selected = true;
            }
        }

        #region 分页和加载DataGridView
        /// <summary>
        /// 分页控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "tslAllCheck")//全选
            {
                tslSelectAll();
                return;
            }
            if (e.ClickedItem.Name == "tslNotCheck")//取消全选
            {
                tslNotSelect();
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
            page.BindBoundControl(dgvDrivewayList, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_DrivewayPosition", "*", "Driveway_ID", "Driveway_ID", 0, sqlwhere, true);
        }
        #endregion


        /// <summary>
        /// “地感” 按钮的单击事件  点击跳转到地感管理界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFVN_Click(object sender, EventArgs e)
        {
            if (this.dgvDrivewayList.SelectedRows.Count > 0)//选中行
            {
                if (dgvDrivewayList.SelectedRows.Count > 1)
                {
                    MessageBox.Show("只能选中一行数据进行链接！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    iDrivewayID = int.Parse(this.dgvDrivewayList.SelectedRows[0].Cells["Driveway_ID"].Value.ToString());

                    FVNForm fvnf = new FVNForm();
                    fvnf.Owner = this;
                    fvnf.iFvnID = iDrivewayID; // 将选中数据的门岗编号ID赋值给通道外键的门岗编号ID
                    fvnf.Show();
                }
            }
            else
            {
                FVNForm fvnf = new FVNForm();
                fvnf.Owner = this;
                fvnf.Show();
            }
        }

        /// <summary>
        /// “摄像头” 按钮的单击事件  点击跳转到摄像头管理界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCamera_Click(object sender, EventArgs e)
        {
            if (this.dgvDrivewayList.SelectedRows.Count > 0)//选中行
            {
                if (dgvDrivewayList.SelectedRows.Count > 1)
                {
                    MessageBox.Show("只能选中一行数据进行链接！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    iDrivewayID = int.Parse(this.dgvDrivewayList.SelectedRows[0].Cells["Driveway_ID"].Value.ToString());

                    CameraForm cf = new CameraForm();
                    cf.Owner = this;
                    cf.iCameraID = iDrivewayID; // 将选中数据的门岗编号ID赋值给通道外键的门岗编号ID
                    cf.Show();
                }
            }
            else
            {
                CameraForm cf = new CameraForm();
                cf.Owner = this;
                cf.Show();
            }
        }

        /// <summary>
        /// 双击组件时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvDrivewayList_DoubleClick(object sender, EventArgs e)
        {
            boo = false;

            this.btnUpdate.Enabled = true;
            this.btnSave.Enabled = false;
            if (this.dgvDrivewayList.SelectedRows.Count > 0)//选中行
            {
                if (dgvDrivewayList.SelectedRows.Count > 1)
                {
                    MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //修改的值
                    int ID = int.Parse(this.dgvDrivewayList.SelectedRows[0].Cells["Driveway_ID"].Value.ToString());
                    Expression<Func<View_DrivewayPosition, bool>> funviewinto = n => n.Driveway_ID == ID;
                    foreach (var n in DrivewayDAL.Query(funviewinto))
                    {
                        if (n.Driveway_Name != null)
                        {
                            //通道名称
                            this.txtDriveway_Name.Text = n.Driveway_Name;
                        }
                        if (n.Driveway_Value != null)
                        {
                            // 通道值
                            this.txtDriveway_Value.Text = n.Driveway_Value;
                        }
                        if (n.Driveway_State != null)
                        {
                            // 通道状态
                            this.cbxDriveway_State.Text = n.Driveway_State;
                        }
                        if (n.Driveway_Type != null)
                        {
                            // 通道类型
                            this.cbxDriveway_Type.Text = n.Driveway_Type;
                        }
                        if (n.Driveway_WarrantyState != null)
                        {
                            // 通道报修状态
                            this.comboxDriveway_WarrantyState.Text = n.Driveway_WarrantyState;
                            bdwayState = n.Driveway_WarrantyState;
                        }
                        if (n.Driveway_Remark_Driveway_ID != null)
                        {
                            // 通道备用管理编号
                            this.cobReserve.SelectedValue = n.Driveway_Remark_Driveway_ID.ToString();
                        }
                        if (n.Position_Name != null)
                        {
                            // 门岗名称
                            this.cbxDriveway_PositionName.Text = n.Position_Name;
                        }
                        //if (n.Driveway_UserId != null)
                        //{
                        //    // 通道创建人
                        //    this.txtDriveway_UserId.Text = n.Driveway_UserId;
                        //}
                        //if (n.Driveway_CreatTime != null)
                        //{
                        //    // 通道创建时间
                        //    this.txtDriveway_Remark.Text = n.Driveway_CreatTime.ToString();
                        //}
                        if (n.Driveway_Address != null)
                        {
                            // 通道地址名称
                            this.cbodizhima.Text = n.Driveway_Address;
                        }
                        if (n.Driveway_ReadCardPort != null)
                        {
                            // 通道读卡器地址码
                            this.txtDriveway_ReadCardPort.Text = n.Driveway_ReadCardPort;
                        }
                        if (n.Driveway_CloseAddress != null)
                        {
                            cbmcloseAdd.Text = n.Driveway_CloseAddress;
                        }
                        if (n.Driveway_Remark != null)
                        {
                            // 通道备注
                            this.txtDriveway_Remark.Text = n.Driveway_Remark;
                        }
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择要修改的行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            boo = true;
        }

        ///// <summary>
        ///// 备用通道管理编号
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void txtDriveway_Remark_Driveway_ID_KeyPress(object sender, KeyPressEventArgs e)
        //{
        //    //阻止从键盘输入键
        //    e.Handled = true;

        //    if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == (char)8))
        //    {

        //        if ((e.KeyChar == (char)8)) { e.Handled = false; return; }
        //        else
        //        {
        //            int len = this.txtDriveway_Remark_Driveway_ID.Text.Length;
        //            if (len < 2)
        //            {
        //                if (len == 0 && e.KeyChar != '0')
        //                {
        //                    e.Handled = false; return;
        //                }
        //                //else if (len == 0)
        //                //{
        //                //    MessageBox.Show("编号不能以0开头！"); return;
        //                //}
        //                e.Handled = false; return;
        //            }
        //            else
        //            {
        //                MessageBox.Show("编号最多只能输入2位数字！");
        //            }
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("编号只能输入数字！");
        //    }


        //}
        /// <summary>
        /// 通道值只能是数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDriveway_Value_KeyPress(object sender, KeyPressEventArgs e)
        {
            //阻止从键盘输入键
            e.Handled = true;

            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == (char)8))
            {

                if ((e.KeyChar == (char)8)) { e.Handled = false; return; }
                else
                {
                    int len = this.txtDriveway_Value.Text.Length;
                    if (len < 2)
                    {
                        if (len == 0 && e.KeyChar != '0')
                        {
                            e.Handled = false; return;
                        }
                        //else if (len == 0)
                        //{
                        //    MessageBox.Show("编号不能以0开头！"); return;
                        //}
                        e.Handled = false; return;
                    }
                    else
                    {
                        MessageBox.Show("编号最多只能输入2位数字！");
                    }
                }
            }
            else
            {
                MessageBox.Show("编号只能输入数字！");
            }
        }

        private void dgvDrivewayList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        //改变选择的门岗时修改相应的通道值
        private void cbxDriveway_PositionName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (boo == true)
            {
                tongdaoZ();
            }
        }

        //查询当前选择的门岗下最大的通道值并加到通道值的txt里面
        private void tongdaoZ()
        {
            int drid = int.Parse(cbxDriveway_PositionName.SelectedValue.ToString());
            Expression<Func<Driveway, bool>> funuserinfo = p => p.Driveway_Position_ID == drid;
            string tongdaoz = LinQBaseDao.Query("select MAX(Driveway_Value) from Driveway where Driveway_Position_ID=" + drid).Tables[0].Rows[0][0].ToString();
            if (tongdaoz.Length > 0)
            {
                MaxTDZ = "0" + (Convert.ToInt32(tongdaoz) + 1).ToString();
                txtDriveway_Value.Text = MaxTDZ.ToString();
            }
            else
            {
                txtDriveway_Value.Text = "01";
            }
        }
    }
}
