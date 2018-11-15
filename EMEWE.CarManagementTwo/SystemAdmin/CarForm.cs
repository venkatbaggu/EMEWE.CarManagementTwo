using EMEWE.CarManagement.CommonClass;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using GemBox.ExcelLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class CarForm : Form
    {
        public CarForm()
        {
            InitializeComponent();
        }
        public static MainForm mf; // 主窗体
        /// <summary>
        /// 实例化分页控件
        /// </summary>
        PageControl page = new PageControl();
        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere = " 1=1";
        /// <summary>
        /// 驾驶员姓名集合
        /// </summary>
        private List<string> staffInfo = new List<string>();
        /// <summary>
        /// 驾驶员编号
        /// </summary>
        private string staffInfo_Id = "";
        /// <summary>
        /// 修改前驾驶员编号
        /// </summary>
        private string staid = "";
        /// <summary>
        ///  车信息ID
        /// </summary>
        private int carid = 0;
        /// <summary>
        /// 公司编号
        /// </summary>
        private string customerInfo_id = "";
        /// <summary>
        /// 是否导出所有
        /// </summary>
        private bool ISExecl = false;
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
                tlsbExecl.Enabled = true;
                tlsbExecl.Visible = true;
                tlsbInExecl.Enabled = true;
                tlsbInExecl.Visible = true;
            }
            else
            {
                btnSave.Visible = ControlAttributes.BoolControl("btnSave", "CarForm", "Visible");
                btnSave.Enabled = ControlAttributes.BoolControl("btnSave", "CarForm", "Enabled");

                btnDel.Visible = ControlAttributes.BoolControl("btnDel", "CarForm", "Visible");
                btnDel.Enabled = ControlAttributes.BoolControl("btnDel", "CarForm", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "CarForm", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "CarForm", "Enabled");

                tlsbExecl.Visible = ControlAttributes.BoolControl("tlsbExecl", "CarForm", "Visible");
                tlsbExecl.Enabled = ControlAttributes.BoolControl("tlsbExecl", "CarForm", "Enabled");

                tlsbInExecl.Visible = ControlAttributes.BoolControl("tlsbInExecl", "CarForm", "Visible");
                tlsbInExecl.Enabled = ControlAttributes.BoolControl("tlsbInExecl", "CarForm", "Enabled");
            }
        }

        private void CarForm_Load(object sender, EventArgs e)
        {
            mf = new MainForm();
            userContext();
            BindState();
            BindCarType();
            BindCarAttribute();
            BindDriSta();
            myUserTreeView2.Visible = false;
            myUserTree1.Visible = false;
            btnUpdate.Enabled = false;
            btnSave.Enabled = true;
            txtCarPic.Enabled = true;
            button1.Enabled = true;
            groupBox2.Visible = false;
            LogInfoLoad("");
        }

        /// <summary>
        /// 绑定状态
        /// </summary>
        private void BindState()
        {
            try
            {
                this.cbxCar_State.DataSource = DictionaryDAL.GetValueDictionary("01").Where(n => n.Dictionary_Name != "全部").ToList();

                if (this.cbxCar_State.DataSource != null)
                {
                    this.cbxCar_State.DisplayMember = "Dictionary_Name";
                    this.cbxCar_State.ValueMember = "Dictionary_ID";
                    this.cbxCar_State.SelectedValue = -1;
                }

                this.cbxState.DataSource = DictionaryDAL.GetValueDictionary("01").Where(n => n.Dictionary_Name != "全部").ToList();

                if (this.cbxState.DataSource != null)
                {
                    this.cbxState.DisplayMember = "Dictionary_Name";
                    this.cbxState.ValueMember = "Dictionary_ID";
                    this.cbxState.SelectedValue = -1;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarForm_BindState绑定车辆状态错误：");
                return;
            }
        }

        /// <summary>
        /// 车辆类型绑定
        /// </summary>
        private void BindCarType()
        {
            try
            {
                DataTable dt = LinQBaseDao.Query("select CarType_ID,CarType_Name from CarType where CarType_State='启动'").Tables[0];
                this.cmbCarType.DataSource = dt;
                if (this.cmbCarType.DataSource != null)
                {
                    this.cmbCarType.DisplayMember = "CarType_Name";
                    this.cmbCarType.ValueMember = "CarType_ID";
                    this.cmbCarType.SelectedIndex = 0;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarForm_BindCarType异常：");
                return;
            }
        }
        /// <summary>
        /// 车辆类型属性
        /// </summary>
        private void BindCarAttribute()
        {
            try
            {
                string sql = "select CarAttribute_ID,CarAttribute_Name from  CarAttribute where CarAttribute_HeightID=0 ";
                DataTable dt = LinQBaseDao.Query(sql).Tables[0];
                DataRow dr = dt.NewRow();
                dr["CarAttribute_ID"] = "0";
                dr["CarAttribute_Name"] = "全部";
                dt.Rows.InsertAt(dr, 0);
                cmbSX.DataSource = dt;
                this.cmbSX.DisplayMember = "CarAttribute_Name";
                this.cmbSX.ValueMember = "CarAttribute_ID";
                this.cmbSX.SelectedIndex = 0;
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarForm_BindCarAttribute异常：");
                return;
            }

        }
        /// <summary>
        /// 通行策略
        /// </summary>
        private void BindDriSta()
        {
            DataTable dt = LinQBaseDao.Query("select distinct (DrivewayStrategy_Name) from DrivewayStrategy where DrivewayStrategy_State='启动'").Tables[0];
            DataRow dr = dt.NewRow();
            dr["DrivewayStrategy_Name"] = "";
            dt.Rows.InsertAt(dr, 0);
            cmbDriSta.DataSource = dt;
            this.cmbDriSta.DisplayMember = "DrivewayStrategy_Name";
            this.cmbDriSta.ValueMember = "DrivewayStrategy_Name";
            this.cmbDriSta.SelectedIndex = 0;
        }
        /// <summary>
        /// 修改事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCar_Name.Text.Trim()))
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "车牌号不能为空！", txtCar_Name, this);
                    return;
                }
                if (chkStaName.Checked)
                {
                    if (string.IsNullOrEmpty(txtstaffInfo.Text.Trim()))
                    {
                        PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "选择驾驶员不能为空！", txtstaffInfo, this);
                        return;
                    }
                }

                string ICid = "";//IC卡号ID编号
                if (!string.IsNullOrEmpty(txtICCardID.Text.Trim()))
                {
                    object obj = LinQBaseDao.GetSingle(" select ICCard_ID from ICCard where ICCard_Value='" + txtICCardID.Text.Trim() + "' and ICCard_State='启动'");
                    if (obj != null)
                    {
                        ICid = obj.ToString();
                    }
                    else
                    {
                        PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "无该IC卡信息，请先添加IC卡！", txtICCardID, this);
                        return;
                    }
                    //DataTable dtcs = LinQBaseDao.Query("select Car_Name from Car where Car_ID !=" + carid + " and Car_ICCard_ID=" + ICid).Tables[0];
                    //if (dtcs.Rows.Count > 0)
                    //{
                    //    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "IC卡已跟其他车辆关联！", txtstaffInfo, this);
                    //    return;
                    //}
                }
                //DataTable dtc = LinQBaseDao.Query("select Car_Name from Car where Car_ID!=" + carid + " and Car_Name='" + txtCarName.Text.Trim() + "'").Tables[0];
                //if (dtc.Rows.Count > 0)
                //{
                //    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "车牌号已存在！", txtstaffInfo, this);
                //    return;
                //}
                DataTable dt = LinQBaseDao.Query("select CustomerInfo_ID from CustomerInfo where CustomerInfo_Name='" + txtCustomerInfo.Text.Trim() + "'").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    customerInfo_id = dt.Rows[0][0].ToString();
                }
                else
                {
                    customerInfo_id = LinQBaseDao.GetSingle("insert CustomerInfo(CustomerInfo_Name,CustomerInfo_State,CustomerInfo_Time) values('" + txtCustomerInfo.Text.Trim() + "','启动',GETDATE())  select @@identity").ToString();

                }

                Expression<Func<Car, bool>> pc = n => n.Car_ID == carid;
                string id = "";
                string strfront = "";
                string strcontent = "";
                string driids = "";
                Action<Car> ap = s =>
                {
                    strfront = s.Car_Name + "," + s.Car_State + "," + s.Car_ICCard_ID + "," + s.Car_ISRegister + "," + s.Car_ISStaffInfo + "," + s.Car_StaffInfo_Names + "," + s.Car_Remark;
                    s.Car_Name = txtCar_Name.Text.Trim();
                    s.Car_State = cbxCar_State.Text;
                    if (!string.IsNullOrEmpty(ICid))
                    {
                        s.Car_ICCard_ID = Convert.ToInt32(ICid);
                    }
                    else
                    {
                        s.Car_ICCard_ID = null;
                    }
                    s.Car_CarType_ID = Convert.ToInt32(cmbCarType.SelectedValue);
                    s.Car_CustomerInfo_ID = Convert.ToInt32(customerInfo_id);
                    s.Car_ISRegister = chkISRegister.Checked;
                    s.Car_ISStaffInfo = chkStaName.Checked;
                    if (!string.IsNullOrEmpty(staffInfo_Id))
                    {
                        s.Car_StaffInfo_IDS = staffInfo_Id.TrimEnd(',');
                        s.Car_StaffInfo_Names = txtstaffInfo.Text.TrimEnd(',');
                    }
                    s.Car_Remark = txtRemark.Text.Trim();
                    string pathStr = txtCarPic.Text;
                    if (!string.IsNullOrEmpty(pathStr))
                    {
                        string stryear = "Car" + SystemClass.PosistionValue + "\\" + CommonalityEntity.GetServersTime().Year + "\\" + CommonalityEntity.GetServersTime().ToString("yyyyMMdd") + "\\";
                        string Name = pathStr.Substring(pathStr.LastIndexOf('\\') + 1);
                        s.Car_AddPic = stryear + Name;
                        ImageFile.UpLoadFile(pathStr, SystemClass.SaveFile + stryear);//上传图片到指定路径
                    }
                    string drista = cmbDriSta.Text;
                    if (!string.IsNullOrEmpty(drista))
                    {
                        s.Car_DriSName = drista;
                        DataTable dtdrista = LinQBaseDao.Query("select DrivewayStrategy_ID from DrivewayStrategy where DrivewayStrategy_Name='" + drista + "' and DrivewayStrategy_State='启动' order by DrivewayStrategy_Sort ").Tables[0];
                        if (dtdrista.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtdrista.Rows.Count; i++)
                            {
                                driids += dtdrista.Rows[i][0].ToString() + ",";
                            }
                            s.Car_DriSids = driids.TrimEnd(',');
                        }
                    }
                    strcontent = s.Car_Name + "," + s.Car_State + "," + s.Car_ICCard_ID + "," + s.Car_ISRegister + "," + s.Car_ISStaffInfo + "," + s.Car_StaffInfo_Names + "," + s.Car_Remark;
                    id = s.Car_ID.ToString();
                };

                if (CarDAL.Update(pc, ap))
                {
                    string drista = cmbDriSta.Text;
                    if (!string.IsNullOrEmpty(drista))
                    {
                        DataTable dtcarinout = LinQBaseDao.Query("select top(1) CarInOutRecord_ID from View_CarState where CarInfo_Name='" + txtCar_Name.Text.Trim() + "' order by CarInOutRecord_ID desc ").Tables[0];
                        if (dtcarinout.Rows.Count > 0)
                        {
                            LinQBaseDao.Query("update CarInOutRecord set CarInOutRecord_DrivewayStrategyS='" + driids.TrimEnd(',') + "',CarInOutRecord_Remark='" + drista + "' where CarInOutRecord_ID=" + dtcarinout.Rows[0][0].ToString());
                        }
                    }
                    MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CommonalityEntity.WriteLogData("修改", "更新编号为：" + id + "的车辆信息；修改前：" + strfront + "；修改后：" + strcontent, CommonalityEntity.USERNAME);//添加操作日志
                }
                else
                {
                    MessageBox.Show("修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("车辆基础信息 btnUpdate_Click()");
            }
            finally
            {
                LogInfoLoad("");
                this.btnUpdate.Enabled = false;
                this.btnSave.Enabled = true;
                Empty();
            }
        }
        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                int j = 0;
                if (dgvCar.SelectedRows.Count > 0)//选中删除
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //选中数量
                        int count = dgvCar.SelectedRows.Count;
                        string id = "";
                        //遍历
                        for (int i = 0; i < count; i++)
                        {
                            int carid = int.Parse(this.dgvCar.SelectedRows[i].Cells["Car_ID"].Value.ToString());
                            string strContent = LinQBaseDao.GetSingle("select Car_Name from Car where Car_ID=" + carid).ToString();
                            DataTable dt = LinQBaseDao.Query("select * from CarInfo where CarInfo_Car_ID=" + carid).Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show("车牌号：" + strContent + "存在关联信息不能删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                            Expression<Func<Car, bool>> funuserinfo = n => n.Car_ID == carid;
                            if (CarDAL.DeleteToMany(funuserinfo))
                            {
                                j++;
                                CommonalityEntity.WriteLogData("删除", "删除车牌号为：" + strContent + "的信息", CommonalityEntity.USERNAME);//添加操作日志
                            }
                        }
                        if (j == count)
                        {
                            MessageBox.Show("成功删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                CommonalityEntity.WriteTextLog("CarForm.btnDel_Click()异常：");
            }
            finally
            {
                LogInfoLoad("");
            }
        }
        /// <summary>
        /// 清空事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmpty_Click(object sender, EventArgs e)
        {
            Empty();
        }
        /// <summary>
        /// 清空的方法
        /// </summary>
        private void Empty()
        {
            txtCar_Name.Text = "";
            txtICCardID.Text = "";
            cbxCar_State.SelectedIndex = 0;
            chkISRegister.Checked = false;
            chkStaName.Checked = false;
            txtCarPic.Text = "";
            txtRemark.Text = "";
            txtCustomerInfo.Text = "";
            txtstaffInfo.Text = "";
            btnUpdate.Enabled = false;
            btnSave.Enabled = true;
            txtCarPic.Enabled = true;
            button1.Enabled = true;
            this.myUserTreeView2.S_ID = "";
            this.myUserTreeView2.S_Name = "";
            myUserTree1.StaffInfo_Name = "";
            myUserTree1.StaffInfo_ID = "";
        }
        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCar_Name.Text.Trim()))
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "车牌号不能为空！", txtCar_Name, this);
                    return;
                }
                if (chkStaName.Checked)
                {
                    if (string.IsNullOrEmpty(txtstaffInfo.Text.Trim()))
                    {
                        PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "选择驾驶员不能为空！", txtstaffInfo, this);
                        return;
                    }
                }

                string ICid = "";//IC卡号ID编号
                if (!string.IsNullOrEmpty(txtICCardID.Text.Trim()))
                {
                    object obj = LinQBaseDao.GetSingle(" select ICCard_ID from ICCard where ICCard_Value='" + txtICCardID.Text.Trim() + "' and ICCard_State='启动'");
                    if (obj != null)
                    {
                        ICid = obj.ToString();
                    }
                    else
                    {
                        PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "无该IC卡信息，请先添加IC卡！", txtICCardID, this);
                        return;
                    }
                    DataTable dtcs = LinQBaseDao.Query("select Car_Name from Car where Car_ID !=" + carid + " and Car_ICCard_ID=" + ICid).Tables[0];
                    if (dtcs.Rows.Count > 0)
                    {
                        PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "IC卡已跟其他车辆关联！", txtstaffInfo, this);
                        return;
                    }
                }
                //DataTable dtc = LinQBaseDao.Query("select Car_Name from Car where Car_Name='" + txtCarName.Text.Trim() + "'").Tables[0];
                //if (dtc.Rows.Count > 0)
                //{
                //    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "车牌号已存在！", txtstaffInfo, this);
                //    return;
                //}
                #region 添加公司

                DataTable dt = LinQBaseDao.Query("select CustomerInfo_ID from CustomerInfo where CustomerInfo_Name='" + txtCustomerInfo.Text.Trim() + "'").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    customerInfo_id = dt.Rows[0][0].ToString();
                }
                else
                {
                    customerInfo_id = LinQBaseDao.GetSingle("insert CustomerInfo(CustomerInfo_Name,CustomerInfo_State,CustomerInfo_Time) values('" + txtCustomerInfo.Text.Trim() + "','启动',GETDATE())  select @@identity").ToString();

                }

                #endregion

                var Caradd = new Car();
                Caradd.Car_Name = txtCar_Name.Text.Trim();
                Caradd.Car_State = cbxCar_State.Text;
                if (!string.IsNullOrEmpty(ICid))
                {
                    Caradd.Car_ICCard_ID = Convert.ToInt32(ICid);
                }
                Caradd.Car_CarType_ID = Convert.ToInt32(cmbCarType.SelectedValue.ToString());
                Caradd.Car_ISRegister = chkISRegister.Checked;
                Caradd.Car_ISStaffInfo = chkStaName.Checked;
                if (!string.IsNullOrEmpty(customerInfo_id))
                {
                    Caradd.Car_CustomerInfo_ID = Convert.ToInt32(customerInfo_id);
                }
                if (!string.IsNullOrEmpty(staffInfo_Id))
                {
                    Caradd.Car_StaffInfo_IDS = staffInfo_Id.TrimEnd(',');
                    Caradd.Car_StaffInfo_Names = txtstaffInfo.Text.TrimEnd(',');
                }
                Caradd.Car_CreateTime = CommonalityEntity.GetServersTime();
                Caradd.Car_Remark = txtRemark.Text;
                string pathStr = txtCarPic.Text;
                if (!string.IsNullOrEmpty(pathStr))
                {
                    string stryear = "CarIn\\" + CommonalityEntity.GetServersTime().Year + "\\" + CommonalityEntity.GetServersTime().ToString("yyyyMMdd") + "\\";
                    string Name = pathStr.Substring(pathStr.LastIndexOf('\\') + 1);
                    Caradd.Car_AddPic = stryear + Name;
                    ImageFile.UpLoadFile(pathStr, SystemClass.SaveFile + stryear);//上传图片到指定路径
                }
                string drista = cmbDriSta.Text;
                if (!string.IsNullOrEmpty(drista))
                {
                    Caradd.Car_DriSName = drista;
                    DataTable dtdrista = LinQBaseDao.Query("select DrivewayStrategy_ID from DrivewayStrategy where DrivewayStrategy_Name='" + drista + "' and DrivewayStrategy_State='启动' order by DrivewayStrategy_Sort ").Tables[0];
                    if (dtdrista.Rows.Count > 0)
                    {
                        string driids = "";
                        for (int i = 0; i < dtdrista.Rows.Count; i++)
                        {
                            driids += dtdrista.Rows[i][0].ToString() + ",";
                        }
                        Caradd.Car_DriSids = driids.TrimEnd(',');
                    }
                }
                if (CarDAL.Insert(Caradd))
                {
                    CommonalityEntity.WriteLogData("新增", "新增车牌号为：" + txtCar_Name.Text.Trim() + "基础信息", CommonalityEntity.USERNAME);//添加操作日志
                    MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK);
                    Empty();
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarForm.btnSave_Click()异常：");
            }
            finally
            {
                LogInfoLoad("");
            }
        }

        /// <summary>
        /// 加载信息
        /// </summary>
        public void LogInfoLoad(string strClickedItemName)
        {
            page.BindBoundControl(dgvCar, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_Car_ICard_CarType", "case Car_ISRegister when 'true' then '是' else '否' end as ISRegister,case Car_ISStaffInfo when 'true' then '是' else '否' end as ISStaffInfo,*", "Car_ID", "Car_ID", 0, sqlwhere, true);
        }
        /// <summary>
        /// 查重方法
        /// </summary>
        /// <returns></returns>
        private bool btnCheck()
        {
            bool rbool = true;
            try
            {
                string Car_Name = this.txtCar_Name.Text.ToString();
                string ICCard = this.txtICCardID.Text.Trim();
                //判断名称是否已存在
                Expression<Func<Car, bool>> carname = n => n.Car_Name == Car_Name;
                if (CarDAL.Query(carname).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该车辆已存在", txtCar_Name, this);
                    txtCar_Name.Focus();
                    rbool = false;
                }
                if (chkISRegister.Checked)
                {
                    Expression<Func<ICCard, bool>> iccard = n => n.ICCard_Value == ICCard && n.ICCard_State == "启动";
                    if (ICCardDAL.Query(iccard).Count() == 0)
                    {
                        PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "IC卡信息不存在或已注销", txtICCardID, this);
                        txtICCardID.Focus();
                        rbool = false;
                    }
                }

                return rbool;

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarForm_ btnCheck()");
                rbool = false;
            }
            return rbool;
        }


        /// <summary>
        /// 选择浏览图片事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "JPG(*.JPG;*.JPEG);gif文件(*.GIF)|*.jpg;*.jpeg;*.gif";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fName = openFileDialog.FileName;
                txtCarPic.Text = fName;
                pictureBox1.ImageLocation = fName;
            }
        }
        /// <summary>
        /// 是否限定驾驶员选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkStaName_CheckedChanged(object sender, EventArgs e)
        {
            if (chkStaName.Checked)
            {
                txtstaffInfo.Enabled = true;
            }
            else
            {
                txtstaffInfo.Enabled = false;
            }
        }
        /// <summary>
        /// 搜索事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                sqlwhere = "  1=1";
                string carNum = this.txtCarName.Text.Trim();
                string state = this.cbxState.Text;
                string staffinfoname = this.txtStaffInfoName.Text;
                string iccardnum = this.txtICNum.Text.Trim();


                if (!string.IsNullOrEmpty(state))
                {
                    if (state != "全部")
                    {
                        sqlwhere += String.Format(" and Car_State =  '{0}'", state);
                    }
                }
                if (!string.IsNullOrEmpty(carNum))
                {
                    sqlwhere += String.Format(" and Car_Name like  '%{0}%'", carNum);
                }
                if (!string.IsNullOrEmpty(staffinfoname))//通道名称
                {
                    sqlwhere += String.Format(" and Car_StaffInfo_Names like  '%{0}%'", staffinfoname);
                }
                if (!string.IsNullOrEmpty(iccardnum))
                {
                    sqlwhere += String.Format(" and ICCard_Value like '%{0}%'", iccardnum);
                }
                if (cmbSX.Text.Trim() != "全部")
                {
                    sqlwhere += String.Format(" and CarType_OtherProperty  ='{0}'", cmbSX.Text.Trim());
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarForm.btnSelect_Click异常:");
            }
            finally
            {
                LogInfoLoad("");
            }
        }



        #region 分页和加载DataGridView
        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslNotSelect()
        {
            for (int i = 0; i < this.dgvCar.Rows.Count; i++)
            {
                dgvCar.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslSelectAll()
        {
            for (int i = 0; i < dgvCar.Rows.Count; i++)
            {
                this.dgvCar.Rows[i].Selected = true;
            }
        }
        /// <summary>
        /// 分页控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "tslAllCheck")//全选
            {
                ISExecl = true;
                tslSelectAll();
                return;
            }
            if (e.ClickedItem.Name == "tslNotCheck")//取消全选
            {
                ISExecl = false;
                tslNotSelect();
                return;
            }
            if (e.ClickedItem.Name == "tlsbExecl")//导出Execl
            {
                tsbExecl_Click();
                return;
            }
            if (e.ClickedItem.Name == "tlsbInExecl")//导入Execl
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
        private string MyCar_Name = "";
        #endregion
        /// <summary>
        /// 双击选中数据修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCar_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                this.btnUpdate.Enabled = true;
                this.btnSave.Enabled = false;
                if (this.dgvCar.SelectedRows.Count > 0)//选中行
                {
                    if (dgvCar.SelectedRows.Count > 1)
                    {
                        MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        int ID = int.Parse(this.dgvCar.SelectedRows[0].Cells["Car_ID"].Value.ToString());
                        carid = ID;
                        string sql = "Select * from Car where Car_ID=" + ID;
                        Car car = new Car();
                        car = LinQBaseDao.GetItemsForListing<Car>(sql).FirstOrDefault();


                        this.txtCar_Name.Text = car.Car_Name;
                        MyCar_Name = car.Car_Name;
                        this.txtICCardID.Text = this.dgvCar.SelectedRows[0].Cells["ICCard_Value"].Value.ToString();
                        this.cbxCar_State.Text = car.Car_State;
                        if (Convert.ToBoolean(car.Car_ISRegister))
                        {
                            chkISRegister.Checked = true;
                        }
                        else
                        {
                            chkISRegister.Checked = false;
                        }

                        this.txtCustomerInfo.Text = this.dgvCar.SelectedRows[0].Cells["CustomerInfo_Name"].Value.ToString();
                        this.txtstaffInfo.Text = car.Car_StaffInfo_Names;
                        this.cmbCarType.Text = this.dgvCar.SelectedRows[0].Cells["CarType_Name"].Value.ToString();
                        if (Convert.ToBoolean(car.Car_ISStaffInfo))
                        {
                            chkStaName.Checked = true;
                        }
                        else
                        {
                            chkStaName.Checked = false;
                        }
                        pictureBox1.ImageLocation = "";
                        this.txtRemark.Text = car.Car_Remark;
                        customerInfo_id = car.Car_CustomerInfo_ID.ToString();
                        staffInfo_Id = car.Car_StaffInfo_IDS;
                        staid = staffInfo_Id;
                        txtCarPic.Text = "";
                        myUserTreeView2.S_ID = customerInfo_id;
                        myUserTreeView2.S_Name = this.txtCustomerInfo.Text;
                        if (!string.IsNullOrEmpty(staffInfo_Id))
                        {
                            myUserTree1.StaffInfo_ID = staffInfo_Id + ",";
                        }
                        else
                        {
                            myUserTree1.StaffInfo_ID = "";
                        }
                        if (!string.IsNullOrEmpty(txtstaffInfo.Text))
                        {
                            myUserTree1.StaffInfo_Name = this.txtstaffInfo.Text + ",";
                        }
                        else
                        {
                            myUserTree1.StaffInfo_Name = "";
                        }
                        string pathadd = car.Car_AddPic;
                        cmbDriSta.Text = car.Car_DriSName;

                        if (!string.IsNullOrEmpty(pathadd))
                        {
                            string stryear = "CarIn\\" + CommonalityEntity.GetServersTime().Year + "\\" + CommonalityEntity.GetServersTime().ToString("yyyyMMdd") + "\\";
                            pictureBox1.ImageLocation = SystemClass.SaveFile + pathadd;

                        }
                    }
                }
                else
                {
                    MessageBox.Show("请选择要修改的行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarForm.dgvCar_DoubleClick异常:");
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            //公司名称赋值
            txtCustomerInfo.Text = myUserTreeView2.S_Name;
            customerInfo_id = myUserTreeView2.S_ID;
            //驾驶员赋值
            txtstaffInfo.Text = myUserTree1.StaffInfo_Name;
            staffInfo_Id = myUserTree1.StaffInfo_ID;
        }
        /// <summary>
        /// 驾驶员文本框双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtstaffInfo_DoubleClick(object sender, EventArgs e)
        {
            if (myUserTree1.Visible)
            {
                myUserTree1.Visible = false;
            }
            else
            {
                myUserTree1.Visible = true;
            }
        }

        private void chkISRegister_CheckedChanged(object sender, EventArgs e)
        {
            if (chkISRegister.Checked)
            {
                txtCarPic.Enabled = true;
                button1.Enabled = true;
            }
            else
            {
                txtCarPic.Enabled = false;
                button1.Enabled = false;

            }

        }

        private void txtICCardID_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CommonalityEntity.strCardNo))
            {
                txtICCardID.Text = CommonalityEntity.strCardNo;
            }

        }

        private void txtCustomerInfo_Click(object sender, EventArgs e)
        {
            CommonalityEntity.tablename = "CustomerInfo";
            CommonalityEntity.tabcom1 = "CustomerInfo_Name";
            CommonalityEntity.tabcom2 = "";
            CommonalityEntity.tabcom3 = "";
            CommonalityEntity.tabid = "CustomerInfo_ID";
            CommonalityEntity.strlike = txtCustomerInfo.Text.Trim();
            myUserTreeView2.StaffInfo_Select();
            myUserTreeView2.Visible = true;

        }

        private void txtCustomerInfo_KeyUp(object sender, KeyEventArgs e)
        {
            CommonalityEntity.tablename = "CustomerInfo";
            CommonalityEntity.tabcom1 = "CustomerInfo_Name";
            CommonalityEntity.tabcom2 = "";
            CommonalityEntity.tabcom3 = "";
            CommonalityEntity.tabid = "CustomerInfo_ID";
            CommonalityEntity.strlike = txtCustomerInfo.Text.Trim();
            myUserTreeView2.StaffInfo_Select();
            myUserTreeView2.Visible = true;

        }

        private void txtICCardID_KeyUp(object sender, KeyEventArgs e)
        {
            string iccard_id = txtICCardID.Text.Trim();
            if (iccard_id.Length == 10)
            {
                try
                {
                    txtICNum.Text = "0" + Convert.ToInt64(iccard_id).ToString("X");
                }
                catch
                {
                    txtICNum.Text = "";
                }
            }
        }

        private void txtICNum_KeyUp(object sender, KeyEventArgs e)
        {
            string iccard_id = txtICNum.Text.Trim();
            if (iccard_id.Length == 10)
            {
                try
                {
                    iccard_id = "0" + Convert.ToInt64(iccard_id).ToString("X");
                    txtICNum.Text = iccard_id;
                }
                catch
                {
                    txtICNum.Text = "";
                }
            }
        }


        private void tsbExecl_Click()
        {
            string fileName = "车辆Excel报表-" + CommonalityEntity.GetServersTime().ToShortDateString();
            if (!ISExecl)
            {
                tslExport_Excel(fileName, dgvCar);
            }
            else
            {
                btnSelect_Click(null, null);
                string strsql = "select Car_Name as 车牌号,CarType_Name as 车辆类型,ICCard_Value as IC卡号,CustomerInfo_Name as	公司名称,case Car_ISStaffInfo when 'true' then '是' else '否' end  as 是否限定驾驶员,Car_StaffInfo_NameS as 姓名,case Car_ISRegister when 'true' then '是' else '否' end as 是否只登记一次,Car_State as 车辆状态,CarType_OtherProperty as 车辆属性,ICCard_EffectiveType as IC卡有效类型,ICCard_count as 允许进厂次数,ICCard_HasCount as 已进厂次数 ,ICCard_BeginTime 开始时间,ICCard_EndTime as 结束时间,Car_CreateTime as 创建时间,Car_Remark as 备注,Car_DriSName as 通行策略 from View_Car_ICard_CarType where " + sqlwhere + " order by Car_ID";
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
                groupBox2.Visible = true;
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
                        MessageBox.Show("导出文件时出错,文件可能正被打开！\n" + "");
                    }

                }
                xlApp.Quit();
                GC.Collect();//强行销毁 
                if (progressBar1.Value == myDGV.SelectedRows.Count)
                {
                    label18.Text = fileName;
                    btnSet.Text = "导入完成";
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
                CommonalityEntity.WriteTextLog("PrinrotyCarInfoForm.InExecl异常:");
            }
        }


        /// <summary>
        ///  将Excel中的数据导入到SQL数据库中
        /// </summary>
        /// <param name="path">路径</param>
        private void setExcelout(string path)
        {
            try
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
                string strName = "";//车牌号
                string strCarTypeid = "";//车辆类型
                string strICNumid = "";//IC卡号
                string strCustid = "";//公司名称
                bool isstaffinfo = false;//是否限定驾驶员
                string strstaName = ""; // 驾驶员名称
                string strstaID = ""; // 驾驶员ID
                bool istrue = false;//是否每次登记
                string strstate = "";//状态
                string strother = "";//车辆最上级属性
                string strICType = "";//IC卡有效类型
                string ECount = "";//有效次数
                string HCount = "";//已使用次数
                string begintime = "NULL";//开始时间
                string endtime = "NULL";//结束时间
                string createtime = "";//创建时间
                string strrmark = "";//备注
                string Car_DriSName = "";//通行策略
                string fileName = System.IO.Path.GetFileName(path);// 
                ISdao = true;
                groupBox2.Visible = true;
                btnSet.Text = "取消导入";
                progressBar1.Maximum = table.Rows.Count;
                progressBar1.Value = 0;
                label18.Text = "正在导入：" + fileName;

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (ISdao)
                    {
                        strName = table.Rows[i][0].ToString().Trim();
                        if (!string.IsNullOrEmpty(strName))
                        {
                            if (!string.IsNullOrEmpty(table.Rows[i][1].ToString().Trim()))
                            {
                                strCarTypeid = LinQBaseDao.GetSingle("select CarType_ID from CarType where CarType_Name='" + table.Rows[i][1].ToString().Trim() + "'").ToString();
                            }
                            else
                            {
                                strCarTypeid = "NULL";
                            }
                            if (!string.IsNullOrEmpty(table.Rows[i][2].ToString().Trim()))
                            {
                                object objid = LinQBaseDao.GetSingle("select ICCard_ID from ICCard where ICCard_Value='" + table.Rows[i][2].ToString().Trim() + "'");
                                if (objid != null)
                                {
                                    strICNumid = objid.ToString();
                                }
                                else
                                {
                                    strICNumid = "NULL";
                                }
                            }
                            else
                            {
                                strICNumid = "NULL";
                            }
                            if (!string.IsNullOrEmpty(table.Rows[i][3].ToString().Trim()))
                            {
                                string custName = table.Rows[i][3].ToString().Trim();
                                DataTable dtcust = LinQBaseDao.Query("select CustomerInfo_ID from CustomerInfo where CustomerInfo_Name='" + custName + "'").Tables[0];
                                if (dtcust.Rows.Count > 0)
                                {
                                    strCustid = dtcust.Rows[0][0].ToString();
                                }
                                else
                                {
                                    strCustid = LinQBaseDao.GetSingle("insert CustomerInfo(CustomerInfo_Name,CustomerInfo_State,CustomerInfo_Time) values('" + custName + "','启动',GETDATE())  select @@identity").ToString();
                                }
                            }
                            else
                            {
                                strCustid = "NULL";
                            }
                            if (table.Rows[i][4].ToString().Trim() == "是")
                            {
                                isstaffinfo = true;
                            }
                            else
                            {
                                isstaffinfo = false;
                            }


                            if (!string.IsNullOrEmpty(table.Rows[i][5].ToString().Trim()))
                            {
                                strstaID = "";
                                strstaName = table.Rows[i][5].ToString().Trim();
                                string[] strnames = strstaName.Split(',');
                                foreach (var item in strnames)
                                {

                                    string staffinfo_name = item.ToString();
                                    DataTable dtStaffInfo = LinQBaseDao.Query("select StaffInfo_ID from StaffInfo where StaffInfo_Name='" + staffinfo_name + "'").Tables[0];
                                    if (dtStaffInfo.Rows.Count > 0)
                                    {
                                        for (int x = 0; x < dtStaffInfo.Rows.Count; x++)
                                        {
                                            strstaID += dtStaffInfo.Rows[x][0].ToString() + ",";
                                        }
                                    }
                                    else
                                    {
                                        strstaID += LinQBaseDao.GetSingle("Insert into StaffInfo(StaffInfo_Type,StaffInfo_Name,StaffInfo_Time,StaffInfo_State,StaffInfo_Identity) values('驾驶员','" + staffinfo_name + "',GETDATE(),'启动','500000000000000001')      select @@identity").ToString() + ",";
                                    }
                                }
                            }
                            else
                            {
                                strstaID = "";
                                strstaName = "";
                            }
                            if (!string.IsNullOrEmpty(strstaID))
                            {
                                strstaID = strstaID.TrimEnd(',');
                            }
                            if (table.Rows[i][6].ToString().Trim() == "是")//是否只登记一次
                            {
                                istrue = true;
                            }
                            else
                            {
                                istrue = false;
                            }
                            strstate = table.Rows[i][7].ToString().Trim();//状态
                            strother = table.Rows[i][8].ToString().Trim();
                            strICType = table.Rows[i][9].ToString().Trim();//IC卡有效类型
                            if (!string.IsNullOrEmpty(table.Rows[i][10].ToString().Trim()))//IC卡有效次数
                            {
                                ECount = table.Rows[i][10].ToString().Trim();
                            }
                            else
                            {
                                ECount = "NULL";
                            }
                            if (!string.IsNullOrEmpty(table.Rows[i][11].ToString().Trim()))//IC卡已使用次数
                            {
                                HCount = table.Rows[i][11].ToString().Trim();
                            }
                            else
                            {
                                HCount = "NULL";
                            }

                            begintime = table.Rows[i][12].ToString().Trim();
                            endtime = table.Rows[i][13].ToString().Trim();
                            if (string.IsNullOrEmpty(begintime) || string.IsNullOrEmpty(endtime))
                            {
                                begintime = "NULL";
                                endtime = "NULL";
                            }
                            createtime = table.Rows[i][14].ToString().Trim();
                            strrmark = table.Rows[i][15].ToString().Trim();
                            Car_DriSName = table.Rows[i][16].ToString().Trim();

                            //查看通行策略是否存在，不存在则为空
                            DataTable dtdris = LinQBaseDao.Query("select DrivewayStrategy_ID from DrivewayStrategy where DrivewayStrategy_Name='" + Car_DriSName + "'").Tables[0];
                            if (dtdris.Rows.Count <= 0)
                            {
                                Car_DriSName = "";
                            }
                            DataTable dt = LinQBaseDao.Query("select Car_ID from Car where Car_Name='" + strName + "'").Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                LinQBaseDao.Query("update Car set Car_ICCard_ID=" + strICNumid + ",Car_CarType_ID=" + strCarTypeid + ",Car_CustomerInfo_ID=" + strCustid + ",Car_Name='" + strName + "',Car_State='" + strstate + "',Car_StaffInfo_IDS='" + strstaID + "',Car_StaffInfo_Names='" + strstaName + "',Car_ISRegister='" + istrue + "',Car_ISStaffInfo='" + isstaffinfo + "',Car_CreateTime='" + createtime + "',Car_Remark ='" + strrmark + "',Car_DriSName='" + Car_DriSName + "' where Car_ID=" + dt.Rows[0][0].ToString());
                                if (!string.IsNullOrEmpty(strICNumid) && strICNumid != "NULL")
                                {
                                    if (begintime == "NULL")
                                    {
                                        LinQBaseDao.Query("update ICCard set ICCard_EffectiveType='" + strICType + "',ICCard_count=" + ECount + ",ICCard_HasCount=" + HCount + ",ICCard_BeginTime=" + begintime + ",ICCard_EndTime =" + endtime + " where  ICCard_ID=" + strICNumid);
                                    }
                                    else
                                    {
                                        LinQBaseDao.Query("update ICCard set ICCard_EffectiveType='" + strICType + "',ICCard_count=" + ECount + ",ICCard_HasCount=" + HCount + ",ICCard_BeginTime='" + begintime + "',ICCard_EndTime ='" + endtime + "' where  ICCard_ID=" + strICNumid);
                                    }
                                }
                            }
                            else
                            {
                                LinQBaseDao.Query("insert into Car(Car_ICCard_ID,Car_CarType_ID,Car_CustomerInfo_ID,Car_Name,Car_State,Car_StaffInfo_IDS,Car_StaffInfo_Names,Car_ISRegister,Car_ISStaffInfo,Car_CreateTime,Car_Remark,Car_DriSName) values (" + strICNumid + "," + strCarTypeid + "," + strCustid + ",'" + strName + "','" + strstate + "','" + strstaID + "','" + strstaName + "','" + istrue + "','" + isstaffinfo + "','" + createtime + "','" + strrmark + "','" + Car_DriSName + "')");
                                if (!string.IsNullOrEmpty(strICNumid) && strICNumid != "NULL")
                                {
                                    if (begintime == "NULL")
                                    {
                                        LinQBaseDao.Query("update ICCard set ICCard_EffectiveType='" + strICType + "',ICCard_count=" + ECount + ",ICCard_HasCount=" + HCount + ",ICCard_BeginTime=" + begintime + ",ICCard_EndTime =" + endtime + " where  ICCard_ID=" + strICNumid);
                                    }
                                    else
                                    {
                                        LinQBaseDao.Query("update ICCard set ICCard_EffectiveType='" + strICType + "',ICCard_count=" + ECount + ",ICCard_HasCount=" + HCount + ",ICCard_BeginTime='" + begintime + "',ICCard_EndTime ='" + endtime + "' where  ICCard_ID=" + strICNumid);
                                    }
                                }
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
                CommonalityEntity.WriteLogData("修改", "车辆基础信息导入Execl信息", CommonalityEntity.USERNAME);
            }
            catch
            {
                MessageBox.Show(this, "导入失败！");
                return;
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
            groupBox2.Visible = false;
        }

        private void dgvCar_Click(object sender, EventArgs e)
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
                groupBox2.Visible = true;
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
