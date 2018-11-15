using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.Commons.CommonClass;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using GemBox.ExcelLite;
using System.Collections;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class PriorityCarInfoForm : Form
    {
        public string where = " 1=1 ";
        /// <summary>
        /// 是否修改
        /// </summary>
        public bool isUpdate = false;
        /// <summary>
        /// 是否导出所有
        /// </summary>
        private bool ISExecl = false;
        public PriorityCarInfoForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PriorityCarInfoForm_Load(object sender, EventArgs e)
        {
            userContext();
            CarStateLoad();
            PriorType();
            tscbxPageSize.SelectedIndex = 1;
            groupBox3.Visible = false;
            btnUpdate.Enabled = false;
            panel1.Visible = false;
            myCname.Visible = false;
            cbxType.SelectedIndex = 0;
            cbxTypeSel.SelectedIndex = 0;
            txtBeginTime.Value = CommonalityEntity.GetServersTime();
            txtEndTime.Value = CommonalityEntity.GetServersTime();
        }

        /// <summary>
        /// 绑定优先车辆状态
        /// </summary>
        public void CarStateLoad()
        {
            //绑定优先车辆状态
            try
            {
                comboxCarState.DataSource = DictionaryDAL.GetValueStateDictionary("01").Where(d => d.Dictionary_Name != "全部").ToList();
                comboxCarState.ValueMember = "Dictionary_Value";
                comboxCarState.DisplayMember = "Dictionary_Name";

                comboxSState.DataSource = DictionaryDAL.GetValueStateDictionary("01");
                comboxSState.ValueMember = "Dictionary_Value";
                comboxSState.DisplayMember = "Dictionary_Name";

                myUsertv.Visible = false;
            }
            catch
            {
                CommonalityEntity.WriteTextLog("PriorityCarInfoForm CarStateLoad()");
            }
        }

        /// <summary>
        /// 绑定优先类型
        /// </summary>
        private void PriorType()
        {
            try
            {
                string str = "车,人员,公司,其他";
                string[] strs = str.Split(',');
                foreach (var item in strs)
                {
                    cbxType.Items.Add(item);
                    cbxTypeSel.Items.Add(item);
                }
                cbxTypeSel.Items.Insert(0, "全部");

            }
            catch
            {
                CommonalityEntity.WriteTextLog("PriorityCarInfoForm PriorType()");
            }
        }


        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {

                btnAdd.Enabled = true;
                btnAdd.Visible = true;
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
                btnAdd.Visible = ControlAttributes.BoolControl("btnAdd", "PriorityCarInfoForm", "Visible");
                btnAdd.Enabled = ControlAttributes.BoolControl("btnAdd", "PriorityCarInfoForm", "Enabled");

                btnDel.Visible = ControlAttributes.BoolControl("btnDel", "PriorityCarInfoForm", "Visible");
                btnDel.Enabled = ControlAttributes.BoolControl("btnDel", "PriorityCarInfoForm", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "PriorityCarInfoForm", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "PriorityCarInfoForm", "Enabled");


                tlsbExecl.Visible = ControlAttributes.BoolControl("tlsbExecl", "PriorityCarInfoForm", "Visible");
                tlsbExecl.Enabled = ControlAttributes.BoolControl("tlsbExecl", "PriorityCarInfoForm", "Enabled");

                tlsbInExecl.Visible = ControlAttributes.BoolControl("tlsbInExecl", "PriorityCarInfoForm", "Visible");
                tlsbInExecl.Enabled = ControlAttributes.BoolControl("tlsbInExecl", "PriorityCarInfoForm", "Enabled");
            }
        }


        #region 分页和加载DataGridView
        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslNotSelects()
        {
            for (int i = 0; i < this.dgvCarInfo.Rows.Count; i++)
            {
                dgvCarInfo.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslSelectAlls()
        {
            for (int i = 0; i < dgvCarInfo.Rows.Count; i++)
            {
                this.dgvCarInfo.Rows[i].Selected = true;
            }
        }
        /// <summary>
        /// 加载信息
        /// </summary>
        public void GetGriddataviewLoad(string strClickedItemName)
        {
            Page.BindBoundControl(dgvCarInfo, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_CarPrecedence", "*", "CarPrecedence_ID", "CarPrecedence_ID", 0, where, true);
        }

        EMEWE.CarManagement.Commons.CommonClass.PageControl Page = new EMEWE.CarManagement.Commons.CommonClass.PageControl();
        /// <summary>
        /// 选择每页条数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tscbxPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetGriddataviewLoad("");
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
                tslSelectAlls();
                return;
            }
            if (e.ClickedItem.Name == "tslNotCheck")//取消全选
            {
                ISExecl = false;
                tslNotSelects();
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
            GetGriddataviewLoad(e.ClickedItem.Name);
        }
        #endregion

        /// <summary>
        /// 新增优先车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {

                CarPrecedence cpd = new CarPrecedence(); ;

                if (cbxType.Text.Trim() == "")
                {
                    MessageBox.Show("请选择优先类型");
                    return;
                }
                if (!Checkblack())
                {
                    return;
                }
                if (cbxType.Text.Trim().Equals("人员"))
                {
                    #region 选择优先类型为人员时

                    if (string.IsNullOrEmpty(txtUname.Text.Trim()))
                    {
                        MessageBox.Show("请选择人员！");
                        return;
                    }
                    if (txtPhone.Text.Trim() == "" || txtStaffInfo_Identity.Text.Trim() == "")
                    {
                        MessageBox.Show("手机号码或身份证不能为空！");
                        return;
                    }
                    string phone = "[1]+\\d{10}$";
                    if (!Regex.IsMatch(txtPhone.Text.Trim(), phone))
                    {
                        MessageBox.Show("手机号码格式不正确！", "提示");
                        return;
                    }
                    if (!Regex.IsMatch(txtStaffInfo_Identity.Text.Trim(), @"(^\d{17}(?:\d{1}|x|X)$)|(^\d{15}$)"))
                    {
                        MessageBox.Show("身份证号码格式不正确！", "提示");
                        return;
                    }
                    isADDStaffInfo();//人员没有就添加

                    cpd.CarPrecedence_StaffInfo_ID = Convert.ToInt32(myUsertv.S_ID);
                    #endregion
                }
                else if (cbxType.Text.Trim().Equals("公司"))
                {
                    #region 选择优先类型为公司时

                    if (string.IsNullOrEmpty(txtCname.Text.Trim()))
                    {
                        MessageBox.Show("请选择公司信息！");
                        return;
                    }

                    isADDCustomerInfo();
                    cpd.CarPrecedence_CustomerInfo_ID = Convert.ToInt32(myCname.S_ID);
                    #endregion
                }
                else if (cbxType.Text.Trim().Equals("车"))
                {
                    #region 选择优先类型为车时

                    if (string.IsNullOrEmpty(txtContor.Text))
                    {
                        MessageBox.Show("请选择车牌号信息！");
                        return;
                    }

                    isADDCar();
                    cpd.CarPrecedence_CarNO = txtContor.Text.Trim();
                    #endregion
                }
                else if (cbxType.Text.Trim().Equals("其他"))
                {
                    #region 选择优先类型为其他时

                    if (string.IsNullOrEmpty(txtContor.Text.Trim()) && string.IsNullOrEmpty(txtCname.Text.Trim()) && string.IsNullOrEmpty(txtUname.Text.Trim()))
                    {
                        MessageBox.Show("请填写信息！");
                        return;
                    }
                    if (!string.IsNullOrEmpty(txtContor.Text.Trim()))
                    {
                        isADDCar();
                        cpd.CarPrecedence_CarNO = txtContor.Text.Trim();
                    }
                    if (!string.IsNullOrEmpty(txtCname.Text.Trim()))
                    {
                        isADDCustomerInfo();
                        cpd.CarPrecedence_CustomerInfo_ID = Convert.ToInt32(myCname.S_ID);
                    }
                    if (!string.IsNullOrEmpty(txtUname.Text.Trim()))
                    {
                        if (txtPhone.Text.Trim() == "" || txtStaffInfo_Identity.Text.Trim() == "")
                        {
                            MessageBox.Show("手机号码或身份证不能为空！");
                            return;
                        }
                        string phone = "[1]+\\d{10}$";
                        if (!Regex.IsMatch(txtPhone.Text.Trim(), phone))
                        {
                            MessageBox.Show("手机号码格式不正确！", "提示");
                            return;
                        }
                        if (!Regex.IsMatch(txtStaffInfo_Identity.Text.Trim(), @"(^\d{17}(?:\d{1}|x|X)$)|(^\d{15}$)"))
                        {
                            MessageBox.Show("身份证号码格式不正确！", "提示");
                            return;
                        }
                        isADDStaffInfo();//人员没有就添加

                        cpd.CarPrecedence_StaffInfo_ID = Convert.ToInt32(myUsertv.S_ID);
                    }
                    #endregion
                }
                cpd.CarPrecedence_Remark = txtCarRemark.Text.Trim();
                cpd.CarPrecedence_Sate = comboxCarState.Text.Trim();
                cpd.CarPrecedence_Time = CommonalityEntity.GetServersTime();
                cpd.CarPrecedence_UserName = CommonalityEntity.USERNAME;
                cpd.CarPrecedence_Type = cbxType.Text;
                if (cmbTypeTime.Text.Trim() == "次数" && txtCount.Text.Trim() != "")
                {
                    cpd.CarPrecedence_TotalCount = Convert.ToInt32(txtCount.Text.Trim());
                    cpd.CarPrecedence_TotalCountED = 0;
                }
                if (cmbTypeTime.Text.Trim() == "有效期")
                {

                    cpd.CarPrecedence_BeginTime = Convert.ToDateTime(txtBeginTime.Value);
                    cpd.CarPrecedence_EndTime = Convert.ToDateTime(txtEndTime.Value);
                }
                cpd.CarPrecedence_CarTypeNames = txtCarTypeNames.Text.Trim();

                if (LinQBaseDao.InsertOne<CarPrecedence>(new DCCarManagementDataContext(), cpd))
                {
                    MessageBox.Show("保存成功");
                    Empty();
                    return;
                }
                else
                {
                    MessageBox.Show("请重试!");
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("PriortyCarInfoForm btnAdd_Click");
            }
            finally
            {
                GetGriddataviewLoad("");
            }
            CommonalityEntity.WriteLogData("新增", "新增优先车辆车牌号为：" + txtContor.Text.Trim(), CommonalityEntity.USERNAME);//添加操作日志
        }



        private bool Checkblack()
        {
            bool istrue = true;
            string strsql = "";
            if (cbxType.Text.Trim().Equals("人员"))
            {
                strsql = " select Blacklist_ID from Blacklist where Blacklist_StaffInfo_ID in( select StaffInfo_ID from  StaffInfo where StaffInfo_Name='" + txtContor.Text.Trim() + "' and StaffInfo_Identity='" + txtStaffInfo_Identity.Text.Trim() + "')";

                DataTable dt = LinQBaseDao.Query(strsql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show(this, "该人员存在黑名单中！");
                    istrue = false;
                }
            }
            else if (cbxType.Text.Trim().Equals("公司"))
            {
                strsql = " select Blacklist_ID from Blacklist where Blacklist_CustomerInfo_ID in (select CustomerInfo_ID from CustomerInfo where CustomerInfo_Name='" + txtContor.Text.Trim() + "')";
                DataTable dt = LinQBaseDao.Query(strsql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show(this, "该公司存在黑名单中！");
                    istrue = false;
                }
            }
            else
            {
                strsql = "select Blacklist_ID from Blacklist where Blacklist_CarInfo_ID in (select Car_ID from Car where Car_Name='" + txtContor.Text.Trim() + "')";
                DataTable dt = LinQBaseDao.Query(strsql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show(this, "该车存在黑名单中！");
                    istrue = false;
                }
            }
            return istrue;
        }
        /// <summary>
        /// 驾驶员没有就添加
        /// </summary>
        private void isADDStaffInfo()
        {

            DataTable dt = LinQBaseDao.Query("select StaffInfo_ID from StaffInfo where StaffInfo_Name ='" + txtUname.Text.Trim() + "' and StaffInfo_Identity='" + txtStaffInfo_Identity.Text.Trim() + "'").Tables[0];
            if (dt.Rows.Count <= 0)
            {
                string StaffInfoSql = "Insert into StaffInfo(StaffInfo_Type,StaffInfo_Name,StaffInfo_Phone,StaffInfo_Time,StaffInfo_State,StaffInfo_Identity) values('驾驶员','" + txtUname.Text.Trim() + "','" + txtPhone.Text.Trim() + "',GETDATE(),'启动','" + txtStaffInfo_Identity.Text.Trim() + "')      select @@identity";
                myUsertv.S_ID = LinQBaseDao.GetSingle(StaffInfoSql).ToString();
                myUsertv.S_Name = txtUname.Text.Trim();

            }
            else
            {
                LinQBaseDao.Query("update StaffInfo set StaffInfo_Phone='" + txtPhone.Text.Trim() + "' where StaffInfo_ID=" + dt.Rows[0][0].ToString());
                myUsertv.S_ID = dt.Rows[0][0].ToString();
                myUsertv.S_Name = txtUname.Text.Trim();
            }
        }
        /// <summary>
        /// 公司没有就添加
        /// </summary>
        private void isADDCustomerInfo()
        {
            if (txtCname.Text.Trim() != myCname.S_Name)
            {
                DataTable dt = LinQBaseDao.Query("select CustomerInfo_ID from CustomerInfo where CustomerInfo_Name='" + txtCname.Text.Trim() + "'").Tables[0];
                if (dt.Rows.Count <= 0)
                {

                    string CustomerInfo_ID = LinQBaseDao.GetSingle("insert CustomerInfo(CustomerInfo_Name,CustomerInfo_State,CustomerInfo_Time) values('" + txtCname.Text.Trim() + "','启动',GETDATE())  select @@identity").ToString();

                    myCname.S_ID = CustomerInfo_ID;
                    myCname.S_Name = txtCname.Text;
                }
                else
                {
                    myCname.S_ID = dt.Rows[0][0].ToString();
                    myCname.S_Name = txtCname.Text;
                }
            }
        }

        /// <summary>
        /// 车牌号没有就添加车辆基本信息
        /// </summary>
        private void isADDCar()
        {
            DataTable dt = LinQBaseDao.Query("select Car_ID from Car where Car_Name='" + txtContor.Text.Trim() + "'").Tables[0];

            if (dt.Rows.Count <= 0)
            {
                string car_id = LinQBaseDao.GetSingle("insert into Car(Car_Name,Car_State,Car_CreateTime,Car_ISRegister,Car_ISStaffInfo) values('" + txtContor.Text.Trim() + "','启动',GETDATE(),0,0) select @@identity").ToString();
            }
        }
        /// <summary>
        /// 修改优先车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (isUpdate == false)
                {
                    MessageBox.Show("请选择修改行,(双击即可选中修改的行)");
                    return;
                }

                if (this.dgvCarInfo.SelectedRows.Count > 1)
                {
                    MessageBox.Show("一次性只能修改一条数据");
                    return;
                }
                if (this.dgvCarInfo.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("请选择修改的行");
                    return;
                }
                if (cbxType.Text == "人员")
                {

                    if (txtPhone.Text.Trim() == "" || txtStaffInfo_Identity.Text.Trim() == "")
                    {
                        MessageBox.Show("手机号码或身份证不能为空！");
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
                }
                if (comboxCarState.Text == "启动")
                {
                    if (!Checkblack())
                    {
                        return;
                    }
                }
                Action<CarPrecedence> action = cpd =>
                {

                    cpd.CarPrecedence_Remark = txtCarRemark.Text.Trim();
                    cpd.CarPrecedence_Sate = comboxCarState.Text.Trim();
                    if (!string.IsNullOrEmpty(cbxType.Text))
                    {
                        cpd.CarPrecedence_Type = cbxType.Text;
                    }
                    switch (cbxType.Text)
                    {

                        case "人员":
                            if (string.IsNullOrEmpty(txtUname.Text.Trim()))
                            {
                                MessageBox.Show("人员姓名不能为空！");
                                return;
                            }
                            isADDStaffInfo();
                            cpd.CarPrecedence_StaffInfo_ID = Convert.ToInt32(myUsertv.S_ID);
                            cpd.CarPrecedence_CustomerInfo_ID = null;
                            cpd.CarPrecedence_CarNO = "";
                            break;
                        case "车":
                            if (string.IsNullOrEmpty(txtContor.Text.Trim()))
                            {
                                MessageBox.Show("车牌号不能为空！");
                                return;
                            }
                            isADDCar();
                            cpd.CarPrecedence_CarNO = txtContor.Text.Trim();
                            cpd.CarPrecedence_StaffInfo_ID = null;
                            cpd.CarPrecedence_CustomerInfo_ID = null;
                            break;
                        case "公司":
                            if (string.IsNullOrEmpty(txtCname.Text.Trim()))
                            {
                                MessageBox.Show("公司不能为空！");
                                return;
                            }
                            isADDCustomerInfo();
                            cpd.CarPrecedence_CustomerInfo_ID = Convert.ToInt32(myCname.S_ID);
                            cpd.CarPrecedence_CarNO = "";
                            cpd.CarPrecedence_StaffInfo_ID = null;
                            break;
                        case "其他":
                            if (string.IsNullOrEmpty(txtContor.Text.Trim()) && string.IsNullOrEmpty(txtUname.Text.Trim()) && string.IsNullOrEmpty(txtCname.Text.Trim()))
                            {
                                MessageBox.Show("请填写信息！");
                                return;
                            }
                            if (!string.IsNullOrEmpty(txtContor.Text.Trim()))
                            {
                                isADDCar();
                                cpd.CarPrecedence_CarNO = txtContor.Text.Trim();
                            }
                            else
                            {
                                cpd.CarPrecedence_CarNO = "";
                            }
                            if (!string.IsNullOrEmpty(txtCname.Text.Trim()))
                            {
                                isADDCustomerInfo();
                                cpd.CarPrecedence_CustomerInfo_ID = Convert.ToInt32(myCname.S_ID);
                            }
                            else
                            {
                                cpd.CarPrecedence_CustomerInfo_ID = null;
                            }
                            if (!string.IsNullOrEmpty(txtUname.Text.Trim()))
                            {
                                if (txtPhone.Text.Trim() == "" || txtStaffInfo_Identity.Text.Trim() == "")
                                {
                                    MessageBox.Show("手机号码或身份证不能为空！");
                                    return;
                                }
                                string phone = "[1]+\\d{10}$";
                                if (!Regex.IsMatch(txtPhone.Text.Trim(), phone))
                                {
                                    MessageBox.Show("手机号码格式不正确！", "提示");
                                    return;
                                }
                                if (!Regex.IsMatch(txtStaffInfo_Identity.Text.Trim(), @"(^\d{17}(?:\d{1}|x|X)$)|(^\d{15}$)"))
                                {
                                    MessageBox.Show("身份证号码格式不正确！", "提示");
                                    return;
                                }
                                isADDStaffInfo();
                                cpd.CarPrecedence_StaffInfo_ID = Convert.ToInt32(myUsertv.S_ID);
                            }
                            else
                            {
                                cpd.CarPrecedence_StaffInfo_ID = null;
                            }


                            break;
                    }

                    if (cmbTypeTime.Text.Trim() == "次数" && txtCount.Text.Trim() != "")
                    {
                        cpd.CarPrecedence_TotalCount = Convert.ToInt32(txtCount.Text.Trim());
                        string counted = this.dgvCarInfo.SelectedRows[0].Cells["CarPrecedence_TotalCountED"].Value.ToString();
                        if (string.IsNullOrEmpty(counted))
                        {
                            cpd.CarPrecedence_TotalCountED = 0;
                        }
                        cpd.CarPrecedence_BeginTime = null;
                        cpd.CarPrecedence_EndTime = null;
                    }
                    else if (cmbTypeTime.Text.Trim() == "有效期")
                    {
                        cpd.CarPrecedence_BeginTime = Convert.ToDateTime(txtBeginTime.Value);
                        cpd.CarPrecedence_EndTime = Convert.ToDateTime(txtEndTime.Value);
                        cpd.CarPrecedence_TotalCount = null;
                    }
                    else
                    {
                        cpd.CarPrecedence_BeginTime = null;
                        cpd.CarPrecedence_EndTime = null;
                        cpd.CarPrecedence_TotalCount = null;
                    }

                    cpd.CarPrecedence_UserName = CommonalityEntity.USERNAME;
                    cpd.CarPrecedence_CarTypeNames = txtCarTypeNames.Text.Trim();
                };
                string s = this.dgvCarInfo.SelectedRows[0].Cells["CarPrecedence_ID"].Value.ToString();
                Expression<Func<CarPrecedence, bool>> fun = cp => cp.CarPrecedence_ID == int.Parse(this.dgvCarInfo.SelectedRows[0].Cells["CarPrecedence_ID"].Value.ToString());
                if (LinQBaseDao.Update<CarPrecedence>(new DCCarManagementDataContext(), fun, action))
                {
                    MessageBox.Show("修改成功");
                    CommonalityEntity.WriteLogData("修改", "修改优先车辆车牌号为：" + txtContor.Text.Trim(), CommonalityEntity.USERNAME);//添加操作日志
                }
                else
                {
                    MessageBox.Show("修改失败");
                    CommonalityEntity.WriteLogData("修改", "修改优先车辆车牌号为：" + txtContor.Text.Trim(), CommonalityEntity.USERNAME);//添加操作日志
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("PriortyCarInfoForm btnUpdate_Click");
            }
            finally
            {
                btnUpdate.Enabled = false;
                btnAdd.Enabled = true;
                isUpdate = false;
                GetGriddataviewLoad("");
                Empty();
            }
        }
        /// <summary>
        /// 注销优先车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCel_Click(object sender, EventArgs e)
        {
            try
            {

                if (this.dgvCarInfo.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("请选择修改的行");
                    return;
                }
                for (int i = 0; i < this.dgvCarInfo.SelectedRows.Count; i++)
                {
                    Action<CarPrecedence> action = cpd =>
                    {
                        cpd.CarPrecedence_Sate = "注销";
                    };
                    Expression<Func<CarPrecedence, bool>> fun = cp => cp.CarPrecedence_ID == int.Parse(this.dgvCarInfo.SelectedRows[i].Cells["CarPrecedence_ID"].Value.ToString());
                    LinQBaseDao.Update<CarPrecedence>(new DCCarManagementDataContext(), fun, action);
                    CommonalityEntity.WriteLogData("修改", "注销优先车辆车牌号为：" + this.dgvCarInfo.SelectedRows[i].Cells["CarPrecedence_CarNO"].Value, CommonalityEntity.USERNAME);//添加操作日志
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("PriortyCarInfoForm btnCel_Click");
            }
            finally
            {
                GetGriddataviewLoad("");
            }
        }
        /// <summary>
        /// 启动优先车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSta_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvCarInfo.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("请选择修改的行");
                    return;
                }
                if (this.dgvCarInfo.SelectedRows.Count > 1)
                {
                    MessageBox.Show("请选择修改的行");
                    return;
                }
                if (!Checkblack())
                {
                    return;
                }
                for (int i = 0; i < this.dgvCarInfo.SelectedRows.Count; i++)
                {

                    Action<CarPrecedence> action = cpd =>
                    {
                        cpd.CarPrecedence_Sate = "启动";
                    };
                    Expression<Func<CarPrecedence, bool>> fun = cp => cp.CarPrecedence_ID == int.Parse(this.dgvCarInfo.SelectedRows[i].Cells["CarPrecedence_ID"].Value.ToString());
                    LinQBaseDao.Update<CarPrecedence>(new DCCarManagementDataContext(), fun, action);
                    CommonalityEntity.WriteLogData("修改", "启动优先车辆车牌号为：" + this.dgvCarInfo.SelectedRows[i].Cells["CarPrecedence_CarNO"].Value, CommonalityEntity.USERNAME);//添加操作日志
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("PriortyCarInfoForm btnSta_Click");
            }
            finally
            {
                GetGriddataviewLoad("");
            }
        }
        /// <summary>
        /// 删除优先车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("是否删除该选中信息?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Cancel)
                {
                    return;
                }
                if (this.dgvCarInfo.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("请选择删除的行");
                    return;
                }
                for (int i = 0; i < this.dgvCarInfo.SelectedRows.Count; i++)
                {
                    Expression<Func<CarPrecedence, bool>> fun = cp => cp.CarPrecedence_ID == int.Parse(this.dgvCarInfo.SelectedRows[i].Cells["CarPrecedence_ID"].Value.ToString());
                    LinQBaseDao.DeleteToMany<CarPrecedence>(new DCCarManagementDataContext(), fun);
                    CommonalityEntity.WriteLogData("删除", "删除优先车辆车牌号为：" + this.dgvCarInfo.SelectedRows[i].Cells["CarPrecedence_CarNO"].Value, CommonalityEntity.USERNAME);//添加操作日志
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("PriortyCarInfoForm btnDel_Click");
            }
            finally
            {
                GetGriddataviewLoad("");
            }
        }
        private string IsCarName = "";
        /// <summary>
        /// 双击选择修改行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCarInfo_DoubleClick(object sender, EventArgs e)
        {
            if (this.dgvCarInfo.SelectedRows.Count <= 0)
            {
                MessageBox.Show("请选择修改的行");
                return;
            }
            btnDel.Enabled = true;
            comboxCarState.Text = (this.dgvCarInfo.SelectedRows[0].Cells["CarPrecedence_Sate"].Value.ToString());
            txtCarRemark.Text = this.dgvCarInfo.SelectedRows[0].Cells["CarPrecedence_Remark"].Value.ToString();
            txtCount.Text = this.dgvCarInfo.SelectedRows[0].Cells["CarPrecedence_TotalCount"].Value.ToString();
            txtCarTypeNames.Text = this.dgvCarInfo.SelectedRows[0].Cells["CarPrecedence_CarTypeNames"].Value.ToString();

            if (txtCount.Text.Trim() != "")
            {
                cmbTypeTime.Text = "次数";
                lblbeginTime.Visible = false;
                lblEndTime.Visible = false;
                txtBeginTime.Visible = false;
                txtEndTime.Visible = false;
            }
            else if (this.dgvCarInfo.SelectedRows[0].Cells["CarPrecedence_BeginTime"].Value.ToString() != "")
            {
                cmbTypeTime.Text = "有效期";
                txtBeginTime.Text = this.dgvCarInfo.SelectedRows[0].Cells["CarPrecedence_BeginTime"].Value.ToString();
                txtEndTime.Text = this.dgvCarInfo.SelectedRows[0].Cells["CarPrecedence_EndTime"].Value.ToString();
                label12.Visible = false;
                txtCount.Visible = false;
            }
            else
            {
                cmbTypeTime.Text = "永久";
                label12.Visible = false;
                txtCount.Visible = false;
                lblbeginTime.Visible = false;
                lblEndTime.Visible = false;
                txtBeginTime.Visible = false;
                txtEndTime.Visible = false;
            }

            cbxType.Text = this.dgvCarInfo.SelectedRows[0].Cells["CarPrecedence_Type"].Value.ToString();
            myUsertv.S_Indes = "";
            myUsertv.S_Name = "";
            myUsertv.S_ID = "";
            myUsertv.S_Name = "";
            switch (cbxType.Text)
            {
                case "车":
                    IsCarName = this.dgvCarInfo.SelectedRows[0].Cells["CarPrecedence_CarNo"].Value.ToString();
                    txtContor.Text = IsCarName;
                    myUsertv.S_ID = "";
                    myUsertv.S_Name = "";
                    myUsertv.S_Indes = "";
                    myUsertv.S_phone = "";
                    myCname.S_ID = "";
                    myCname.S_Name = "";
                    myCname.S_Indes = "";
                    myCname.S_phone = "";
                    break;
                case "人员":
                    txtUname.Text = this.dgvCarInfo.SelectedRows[0].Cells["StaffInfo_Name"].Value.ToString();
                    myUsertv.S_ID = this.dgvCarInfo.SelectedRows[0].Cells["StaffInfo_ID"].Value.ToString();
                    txtPhone.Text = this.dgvCarInfo.SelectedRows[0].Cells["StaffInfo_Phone"].Value.ToString();
                    txtStaffInfo_Identity.Text = this.dgvCarInfo.SelectedRows[0].Cells["StaffInfo_Identity"].Value.ToString();
                    myUsertv.S_Name = txtUname.Text;
                    myUsertv.S_Indes = txtStaffInfo_Identity.Text;
                    myUsertv.S_phone = txtPhone.Text;
                    break;
                case "公司":
                    txtCname.Text = this.dgvCarInfo.SelectedRows[0].Cells["CustomerInfo_Name"].Value.ToString();
                    myCname.S_ID = this.dgvCarInfo.SelectedRows[0].Cells["CustomerInfo_ID"].Value.ToString();
                    myCname.S_Name = txtContor.Text;
                    break;
                case "其他":
                    if (this.dgvCarInfo.SelectedRows[0].Cells["CarPrecedence_CarNo"].Value != null)
                    {
                        IsCarName = this.dgvCarInfo.SelectedRows[0].Cells["CarPrecedence_CarNo"].Value.ToString();
                        txtContor.Text = IsCarName;
                    }
                    if (this.dgvCarInfo.SelectedRows[0].Cells["CustomerInfo_Name"].Value != null)
                    {
                        txtCname.Text = this.dgvCarInfo.SelectedRows[0].Cells["CustomerInfo_Name"].Value.ToString();
                        myCname.S_ID = this.dgvCarInfo.SelectedRows[0].Cells["CustomerInfo_ID"].Value.ToString();
                        myCname.S_Name = txtCname.Text;
                    }
                    else
                    {
                        myCname.S_ID = "";
                        myCname.S_Name = "";
                        myCname.S_Indes = "";
                        myCname.S_phone = "";
                    }
                    if (this.dgvCarInfo.SelectedRows[0].Cells["StaffInfo_Name"].Value != null)
                    {
                        txtUname.Text = this.dgvCarInfo.SelectedRows[0].Cells["StaffInfo_Name"].Value.ToString();
                        myUsertv.S_ID = this.dgvCarInfo.SelectedRows[0].Cells["StaffInfo_ID"].Value.ToString();
                        txtPhone.Text = this.dgvCarInfo.SelectedRows[0].Cells["StaffInfo_Phone"].Value.ToString();
                        txtStaffInfo_Identity.Text = this.dgvCarInfo.SelectedRows[0].Cells["StaffInfo_Identity"].Value.ToString();
                        myUsertv.S_Name = txtUname.Text;
                        myUsertv.S_Indes = txtStaffInfo_Identity.Text;
                        myUsertv.S_phone = txtPhone.Text;
                    }
                    else
                    {
                        myUsertv.S_ID = "";
                        myUsertv.S_Name = "";
                        myUsertv.S_Indes = "";
                        myUsertv.S_phone = "";
                    }
                    break;
            }
            isUpdate = true;
            btnUpdate.Enabled = true;
            btnAdd.Enabled = false;
        }
        /// <summary>
        /// 取消修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCelUp_Click(object sender, EventArgs e)
        {
            Empty();
        }

        /// <summary>
        /// 清空方法
        /// </summary>
        private void Empty()
        {
            this.btnDel.Enabled = false;
            txtContor.Text = "";
            txtCarRemark.Text = "";
            txtCount.Text = "";
            cbxType.SelectedIndex = -1;
            comboxCarState.SelectedIndex = 0;
            isUpdate = false;
            btnUpdate.Enabled = false;
            btnAdd.Enabled = true;
            myUsertv.S_ID = "";
            myUsertv.S_Name = "";
            myUsertv.S_phone = "";
            myUsertv.S_Indes = "";
            myCname.S_ID = "";
            myCname.S_Name = "";
            myCname.S_Indes = "";
            myCname.S_phone = "";
            txtPhone.Text = "";
            txtStaffInfo_Identity.Text = "";
            txtCarTypeNames.Text = "";
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSel_Click(object sender, EventArgs e)
        {
            where = " 1=1 ";

            if (!string.IsNullOrEmpty(cbxTypeSel.Text.Trim()))
            {
                if (cbxTypeSel.Text.Trim() != "全部")
                {
                    where += " and CarPrecedence_Type like '%" + cbxTypeSel.Text.Trim() + "%'";

                }
            }
            if (!string.IsNullOrEmpty(comboxSState.Text.Trim()))
            {
                if (comboxSState.Text.Trim() != "全部")
                {
                    where += " and CarPrecedence_sate='" + comboxSState.Text.Trim() + "'";
                }
            }
            if (txtCarNmae.Text.Trim() != "")
            {
                where += " and CarPrecedence_CarNO like '%" + txtCarNmae.Text.Trim() + "%'";

            }
            if (txtcust.Text.Trim() != "")
            {
                where += " and CustomerInfo_Name like '%" + txtcust.Text.Trim() + "%'";
            }
            if (txtstaname.Text.Trim() != "")
            {
                where += " and StaffInfo_Name like '%" + txtstaname.Text.Trim() + "%'";
            }
            if (txtIndes.Text.Trim() != "")
            {
                where += " and StaffInfo_Identity like '%" + txtIndes.Text.Trim() + "%'";
            }
            GetGriddataviewLoad("");
        }

        string cbxTypeOldStr = "";
        /// <summary>
        /// 优先类型选择改变事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxType_TextChanged(object sender, EventArgs e)
        {
            if (!cbxType.Text.Trim().Equals(cbxTypeOldStr))
            {
                if (cbxType.Text.Trim().Equals("车"))
                {
                    lbCar.Visible = true;
                    txtContor.Visible = true;
                    lbCname.Visible = false;
                    txtCname.Visible = false;
                    lbUname.Visible = false;
                    txtUname.Visible = false;
                    lblStaffInfo_Identity.Visible = false;
                    txtStaffInfo_Identity.Visible = false;
                    lblPhone.Visible = false;
                    txtPhone.Visible = false;
                    button1.Visible = false;
                }
                else if (cbxType.Text.Trim().Equals("公司"))
                {
                    lbCar.Visible = false;
                    txtContor.Visible = false;
                    lbCname.Visible = true;
                    txtCname.Visible = true;
                    lbUname.Visible = false;
                    txtUname.Visible = false;
                    lblStaffInfo_Identity.Visible = false;
                    txtStaffInfo_Identity.Visible = false;
                    lblPhone.Visible = false;
                    txtPhone.Visible = false;
                    button1.Visible = false;
                }
                else if (cbxType.Text.Trim().Equals("人员"))
                {
                    lbCar.Visible = false;
                    txtContor.Visible = false;
                    lbCname.Visible = false;
                    txtCname.Visible = false;
                    lbUname.Visible = true;
                    txtUname.Visible = true;
                    lblStaffInfo_Identity.Visible = true;
                    txtStaffInfo_Identity.Visible = true;
                    lblPhone.Visible = true;
                    txtPhone.Visible = true;
                    button1.Visible = true;
                }
                else if (cbxType.Text.Trim().Equals("其他"))
                {
                    lbCar.Visible = true;
                    txtContor.Visible = true;
                    lbCname.Visible = true;
                    txtCname.Visible = true;
                    lbUname.Visible = true;
                    txtUname.Visible = true;
                    lblStaffInfo_Identity.Visible = true;
                    txtStaffInfo_Identity.Visible = true;
                    lblPhone.Visible = true;
                    txtPhone.Visible = true;
                    button1.Visible = true;
                }
                cbxTypeOldStr = cbxType.Text.Trim();
            }

        }
        /// <summary>
        /// 单击组件（驾驶员名称）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtContor_Click(object sender, EventArgs e)
        {
            try
            {

                myUsertv.Visible = true;
                CommonalityEntity.tablename = "StaffInfo";
                CommonalityEntity.tabcom1 = "StaffInfo_Name";
                CommonalityEntity.tabcom2 = "StaffInfo_Identity";
                CommonalityEntity.tabcom3 = "StaffInfo_Phone";
                CommonalityEntity.tabid = "StaffInfo_ID";
                CommonalityEntity.strlike = txtUname.Text.Trim();
                myUsertv.StaffInfo_Select();
            }
            catch
            {
                CommonalityEntity.WriteTextLog("PrinrotyCarInfoForm txtContor_DoubleClick()");
            }
        }

        /// <summary>
        /// 单击组件（车牌号），没有方法体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCarName_Click(object sender, EventArgs e) { }
        /// <summary>
        /// 单击组件（公司名）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtContorCustomerInfo_Click(object sender, EventArgs e)
        {
            try
            {

                myCname.Visible = true;
                CommonalityEntity.tablename = "CustomerInfo";
                CommonalityEntity.tabcom1 = "CustomerInfo_Name";
                CommonalityEntity.tabcom2 = "";
                CommonalityEntity.tabcom3 = "";
                CommonalityEntity.tabid = "CustomerInfo_ID";
                CommonalityEntity.strlike = txtCname.Text.Trim();
                myCname.StaffInfo_Select();
            }
            catch
            {
                CommonalityEntity.WriteTextLog("PrinrotyCarInfoForm txtContorCustomerInfo_Click()");
            }
        }

        private void gbContent_Enter(object sender, EventArgs e)
        {

            if (myUsertv.S_Name != "")
            {
                txtUname.Text = myUsertv.S_Name;
                txtPhone.Text = myUsertv.S_phone;
                txtStaffInfo_Identity.Text = myUsertv.S_Indes;
            }
            if (myCname.S_Name != "")
            {
                txtCname.Text = myCname.S_Name;
            }
        }

        private void PriorityCarInfoForm_Enter(object sender, EventArgs e)
        {
            if (myUsertv.S_Name != "")
            {
                txtUname.Text = myUsertv.S_Name;
                txtPhone.Text = myUsertv.S_phone;
                txtStaffInfo_Identity.Text = myUsertv.S_Indes;
            }
            if (myCname.S_Name != "")
            {
                txtCname.Text = myCname.S_Name;
            }
        }


        private void tsbExecl_Click()
        {
            string fileName = "优先车Excel报表-" + CommonalityEntity.GetServersTime().ToShortDateString();
            if (!ISExecl)
            {
                tslExport_Excel(fileName, dgvCarInfo);
            }
            else
            {
                btnSel_Click(null, null);
                string strsql = "select CarPrecedence_Type as 优先类型,CarPrecedence_CarNo as 车牌号,CustomerInfo_Name as 公司名称,StaffInfo_Name as 姓名,StaffInfo_Identity as 身份证号码,StaffInfo_Phone as 手机号码,CarPrecedence_Sate as 优先状态,CarPrecedence_CarTypeNames as 优先车辆类型,CarPrecedence_TotalCount as 优先有效次数,CarPrecedence_TotalCountED as 已优先次数,CarPrecedence_BeginTime as 优先开始日期,CarPrecedence_EndTime as 优先结束时间,CarPrecedence_Time as 登记时间 ,CarPrecedence_UserName as 登记人,CarPrecedence_Remark as 备注,CustomerInfo_ID as 公司编号,StaffInfo_ID as 人员编号 from View_CarPrecedence where " + where + " order by CarPrecedence_ID  desc";
                daochu(fileName, strsql);
            }
        }
        /// <summary>
        /// 导出Excel 的方法
        /// </summary>
        private void tslExport_Excel(string fileName, DataGridView myDGV)
        {
            string saveFileName = "";
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xls";
            saveDialog.Filter = "Excel文件|*.xls";
            saveDialog.FileName = fileName;
            saveDialog.ShowDialog();
            saveFileName = saveDialog.FileName;
            if (saveFileName.IndexOf(":") < 0) return; //被点了取消 

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("无法创建Excel对象，可能您的机子未安装Excel");
                return;
            }
            ISdao = true;
            groupBox3.Visible = true;
            btnSet.Text = "取消导出";
            progressBar1.Maximum = myDGV.SelectedRows.Count;
            progressBar1.Value = 0;
            label19.Text = "正在导出：" + fileName;

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
                        worksheet.Cells[s + 2, i] = myDGV.Rows[r].Cells[i].Value;
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
            Microsoft.Office.Interop.Excel.Range rang = worksheet.get_Range(worksheet.Cells[2, 1], worksheet.Cells[myDGV.Rows.Count + 2, 2]);
            rang.NumberFormat = "000000000000";

            if (saveFileName != "")
            {
                try
                {
                    workbook.Saved = true;
                    workbook.SaveCopyAs(saveFileName);
                    //fileSaved = true;
                }
                catch
                {
                    //fileSaved = false;
                    MessageBox.Show("导出文件时出错,文件可能正被打开！\n");
                }

            }
            //else
            //{
            //    fileSaved = false;
            //}
            xlApp.Quit();
            GC.Collect();//强行销毁 
            if (progressBar1.Value >= progressBar1.Maximum)
            {
                label19.Text = fileName;
                btnSet.Text = "导出完成";
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
                string str = "";
                string strName = "";
                string strcar = "";
                string ECount = "";
                string HCount = "";
                string fileName = System.IO.Path.GetFileName(path);// 

                ISdao = true;
                groupBox3.Visible = true;
                btnSet.Text = "取消导入";
                progressBar1.Maximum = table.Rows.Count;
                progressBar1.Value = 0;
                label19.Text = "正在导入：" + fileName;

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (ISdao)
                    {
                        str = table.Rows[i][0].ToString().Trim();
                        if (str == "车")
                        {
                            strName = table.Rows[i][1].ToString().Trim();
                            DataTable dt = LinQBaseDao.Query("select Car_ID from Car where Car_Name='" + strName + "'").Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                strcar = dt.Rows[0][0].ToString();
                            }
                            else
                            {
                                strcar = LinQBaseDao.GetSingle("insert Car(Car_Name,Car_State,Car_CreateTime,Car_ISRegister,Car_ISStaffInfo) values('" + strName + "','启动',GETDATE(),0,0)  select @@identity").ToString();

                            }
                            DataTable dts = LinQBaseDao.Query("select * from CarPrecedence  where  CarPrecedence_CarNO='" + strName + "'").Tables[0];
                            if (!string.IsNullOrEmpty(table.Rows[i][8].ToString().Trim()))
                            {
                                ECount = table.Rows[i][8].ToString().Trim();
                            }
                            else
                            {
                                ECount = "NULL";
                            }
                            if (!string.IsNullOrEmpty(table.Rows[i][9].ToString().Trim()))
                            {
                                HCount = table.Rows[i][9].ToString().Trim();
                            }
                            else
                            {
                                HCount = "NULL";
                            }
                            if (dts.Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(table.Rows[i][10].ToString().Trim()))
                                {
                                    LinQBaseDao.Query("update CarPrecedence set CarPrecedence_Sate='" + table.Rows[i][6].ToString().Trim() + "',CarPrecedence_CarTypeNames='" + table.Rows[i][7].ToString().Trim() + "',CarPrecedence_TotalCount=" + ECount + ",CarPrecedence_TotalCountED=" + HCount + ",CarPrecedence_BeginTime='" + table.Rows[i][10].ToString().Trim() + "',CarPrecedence_EndTime='" + table.Rows[i][11].ToString().Trim() + "',CarPrecedence_Time='" + table.Rows[i][12].ToString().Trim() + "',CarPrecedence_UserName='" + table.Rows[i][13].ToString().Trim() + "',CarPrecedence_Remark='" + table.Rows[i][14].ToString().Trim() + "' where  CarPrecedence_CarNO='" + strName + "'");
                                }
                                else
                                {
                                    LinQBaseDao.Query("update CarPrecedence set CarPrecedence_Sate='" + table.Rows[i][6].ToString().Trim() + "',CarPrecedence_CarTypeNames='" + table.Rows[i][7].ToString().Trim() + "',CarPrecedence_TotalCount=" + ECount + ",CarPrecedence_TotalCountED=" + HCount + ",CarPrecedence_BeginTime=null,CarPrecedence_EndTime=null,CarPrecedence_Time='" + table.Rows[i][12].ToString().Trim() + "',CarPrecedence_UserName='" + table.Rows[i][13].ToString().Trim() + "',CarPrecedence_Remark='" + table.Rows[i][14].ToString().Trim() + "' where  CarPrecedence_CarNO='" + strName + "'");
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(table.Rows[i][10].ToString().Trim()))
                                {
                                    LinQBaseDao.Query(" insert into CarPrecedence(CarPrecedence_Type, CarPrecedence_CarNO,CarPrecedence_Sate,CarPrecedence_CarTypeNames,CarPrecedence_TotalCount,CarPrecedence_TotalCountED,CarPrecedence_BeginTime,CarPrecedence_EndTime,CarPrecedence_Time,CarPrecedence_UserName,CarPrecedence_Remark) values('" + str + "','" + strName + "','" + table.Rows[i][6].ToString().Trim() + "','" + table.Rows[i][7].ToString().Trim() + "'," + ECount + "," + HCount + ",'" + table.Rows[i][10].ToString().Trim() + "','" + table.Rows[i][11].ToString().Trim() + "','" + table.Rows[i][12].ToString().Trim() + "','" + table.Rows[i][13].ToString().Trim() + "','" + table.Rows[i][14].ToString().Trim() + "')");
                                }
                                else
                                {
                                    LinQBaseDao.Query(" insert into CarPrecedence(CarPrecedence_Type, CarPrecedence_CarNO,CarPrecedence_Sate,CarPrecedence_CarTypeNames,CarPrecedence_TotalCount,CarPrecedence_TotalCountED,CarPrecedence_BeginTime,CarPrecedence_EndTime,CarPrecedence_Time,CarPrecedence_UserName,CarPrecedence_Remark) values('" + str + "','" + strName + "','" + table.Rows[i][6].ToString().Trim() + "','" + table.Rows[i][7].ToString().Trim() + "'," + ECount + "," + HCount + ",null,null,'" + table.Rows[i][12].ToString().Trim() + "','" + table.Rows[i][13].ToString().Trim() + "','" + table.Rows[i][14].ToString().Trim() + "')");
                                }
                            }
                        }
                        if (str == "公司")
                        {
                            strName = table.Rows[i][2].ToString().Trim();
                            DataTable dt = LinQBaseDao.Query("select CustomerInfo_ID from CustomerInfo where CustomerInfo_Name='" + strName + "'").Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                strcar = dt.Rows[0][0].ToString();
                            }
                            else
                            {
                                strcar = LinQBaseDao.GetSingle("insert CustomerInfo(CustomerInfo_Name,CustomerInfo_State,CustomerInfo_Time) values('" + strName + "','启动',GETDATE())  select @@identity").ToString();

                            }
                            DataTable dts = LinQBaseDao.Query("select * from CarPrecedence  where  CarPrecedence_CustomerInfo_ID=" + strcar).Tables[0];

                            if (!string.IsNullOrEmpty(table.Rows[i][8].ToString().Trim()))
                            {
                                ECount = table.Rows[i][8].ToString().Trim();
                            }
                            else
                            {
                                ECount = "NULL";
                            }
                            if (!string.IsNullOrEmpty(table.Rows[i][9].ToString().Trim()))
                            {
                                HCount = table.Rows[i][9].ToString().Trim();
                            }
                            else
                            {
                                HCount = "NULL";
                            }
                            if (dts.Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(table.Rows[i][10].ToString().Trim()))
                                {
                                    LinQBaseDao.Query("update CarPrecedence set CarPrecedence_Sate='" + table.Rows[i][6].ToString().Trim() + "',CarPrecedence_CarTypeNames='" + table.Rows[i][7].ToString().Trim() + "',CarPrecedence_TotalCount=" + ECount + ",CarPrecedence_TotalCountED=" + HCount + ",CarPrecedence_BeginTime='" + table.Rows[i][10].ToString().Trim() + "',CarPrecedence_EndTime='" + table.Rows[i][11].ToString().Trim() + "',CarPrecedence_Time='" + table.Rows[i][12].ToString().Trim() + "',CarPrecedence_UserName='" + table.Rows[i][13].ToString().Trim() + "',CarPrecedence_Remark='" + table.Rows[i][14].ToString().Trim() + "' where  CarPrecedence_CustomerInfo_ID=" + strcar);
                                }
                                else
                                {
                                    LinQBaseDao.Query("update CarPrecedence set CarPrecedence_Sate='" + table.Rows[i][6].ToString().Trim() + "',CarPrecedence_CarTypeNames='" + table.Rows[i][7].ToString().Trim() + "',CarPrecedence_TotalCount=" + ECount + ",CarPrecedence_TotalCountED=" + HCount + ",CarPrecedence_BeginTime=null,CarPrecedence_EndTime=null,CarPrecedence_Time='" + table.Rows[i][12].ToString().Trim() + "',CarPrecedence_UserName='" + table.Rows[i][13].ToString().Trim() + "',CarPrecedence_Remark='" + table.Rows[i][14].ToString().Trim() + "' where  CarPrecedence_CustomerInfo_ID=" + strcar);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(table.Rows[i][10].ToString().Trim()))
                                {
                                    LinQBaseDao.Query(" insert into CarPrecedence(CarPrecedence_Type, CarPrecedence_CustomerInfo_ID,CarPrecedence_Sate,CarPrecedence_CarTypeNames,CarPrecedence_TotalCount,CarPrecedence_TotalCountED,CarPrecedence_BeginTime,CarPrecedence_EndTime,CarPrecedence_Time,CarPrecedence_UserName,CarPrecedence_Remark) values('" + str + "'," + strcar + ",'" + table.Rows[i][6].ToString().Trim() + "','" + table.Rows[i][7].ToString().Trim() + "'," + ECount + "," + HCount + ",'" + table.Rows[i][10].ToString().Trim() + "','" + table.Rows[i][11].ToString().Trim() + "','" + table.Rows[i][12].ToString().Trim() + "','" + table.Rows[i][13].ToString().Trim() + "','" + table.Rows[i][14].ToString().Trim() + "')");
                                }
                                else
                                {
                                    LinQBaseDao.Query(" insert into CarPrecedence(CarPrecedence_Type, CarPrecedence_CustomerInfo_ID,CarPrecedence_Sate,CarPrecedence_CarTypeNames,CarPrecedence_TotalCount,CarPrecedence_TotalCountED,CarPrecedence_BeginTime,CarPrecedence_EndTime,CarPrecedence_Time,CarPrecedence_UserName,CarPrecedence_Remark) values('" + str + "'," + strcar + ",'" + table.Rows[i][6].ToString().Trim() + "','" + table.Rows[i][7].ToString().Trim() + "'," + ECount + "," + HCount + ",null,null,'" + table.Rows[i][12].ToString().Trim() + "','" + table.Rows[i][13].ToString().Trim() + "','" + table.Rows[i][14].ToString().Trim() + "')");
                                }
                            }
                        }
                        if (str == "人员")
                        {
                            strName = table.Rows[i][3].ToString().Trim();
                            DataTable dt = LinQBaseDao.Query("select StaffInfo_ID from StaffInfo where StaffInfo_Name='" + strName + "' and StaffInfo_Identity='" + table.Rows[i][4].ToString().Trim() + "'").Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                strcar = dt.Rows[0][0].ToString();
                                LinQBaseDao.Query("update StaffInfo  set  StaffInfo_Phone='" + table.Rows[i][5].ToString().Trim() + "'  where StaffInfo_ID=" + strcar);
                            }
                            else
                            {
                                strcar = LinQBaseDao.GetSingle("Insert into StaffInfo(StaffInfo_Type,StaffInfo_Name,StaffInfo_Phone,StaffInfo_Time,StaffInfo_State,StaffInfo_Identity) values('驾驶员','" + table.Rows[i][3].ToString().Trim() + "','" + table.Rows[i][5].ToString().Trim() + "',GETDATE(),'启动','" + table.Rows[i][4].ToString().Trim() + "')      select @@identity").ToString();
                            }

                            DataTable dts = LinQBaseDao.Query("select * from CarPrecedence  where  CarPrecedence_StaffInfo_ID=" + strcar).Tables[0];


                            if (!string.IsNullOrEmpty(table.Rows[i][8].ToString().Trim()))
                            {
                                ECount = table.Rows[i][8].ToString().Trim();
                            }
                            else
                            {
                                ECount = "NULL";
                            }
                            if (!string.IsNullOrEmpty(table.Rows[i][9].ToString().Trim()))
                            {
                                HCount = table.Rows[i][9].ToString().Trim();
                            }
                            else
                            {
                                HCount = "NULL";
                            }
                            if (dts.Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(table.Rows[i][10].ToString().Trim()))
                                {
                                    LinQBaseDao.Query("update CarPrecedence set CarPrecedence_Sate='" + table.Rows[i][6].ToString().Trim() + "',CarPrecedence_CarTypeNames='" + table.Rows[i][7].ToString().Trim() + "',CarPrecedence_TotalCount=" + ECount + ",CarPrecedence_TotalCountED=" + HCount + ",CarPrecedence_BeginTime='" + table.Rows[i][10].ToString().Trim() + "',CarPrecedence_EndTime='" + table.Rows[i][11].ToString().Trim() + "',CarPrecedence_Time='" + table.Rows[i][12].ToString().Trim() + "',CarPrecedence_UserName='" + table.Rows[i][13].ToString().Trim() + "',CarPrecedence_Remark='" + table.Rows[i][14].ToString().Trim() + "' where  CarPrecedence_CustomerInfo_ID=" + strcar);
                                }
                                else
                                {
                                    LinQBaseDao.Query("update CarPrecedence set CarPrecedence_Sate='" + table.Rows[i][6].ToString().Trim() + "',CarPrecedence_CarTypeNames='" + table.Rows[i][7].ToString().Trim() + "',CarPrecedence_TotalCount=" + ECount + ",CarPrecedence_TotalCountED=" + HCount + ",CarPrecedence_BeginTime=null,CarPrecedence_EndTime=null,CarPrecedence_Time='" + table.Rows[i][12].ToString().Trim() + "',CarPrecedence_UserName='" + table.Rows[i][13].ToString().Trim() + "',CarPrecedence_Remark='" + table.Rows[i][14].ToString().Trim() + "' where  CarPrecedence_CustomerInfo_ID=" + strcar);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(table.Rows[i][10].ToString().Trim()))
                                {
                                    LinQBaseDao.Query(" insert into CarPrecedence(CarPrecedence_Type, CarPrecedence_StaffInfo_ID,CarPrecedence_Sate,CarPrecedence_CarTypeNames,CarPrecedence_TotalCount,CarPrecedence_TotalCountED,CarPrecedence_BeginTime,CarPrecedence_EndTime,CarPrecedence_Time,CarPrecedence_UserName,CarPrecedence_Remark) values('" + str + "'," + strcar + ",'" + table.Rows[i][6].ToString().Trim() + "','" + table.Rows[i][7].ToString().Trim() + "'," + ECount + "," + HCount + ",'" + table.Rows[i][10].ToString().Trim() + "','" + table.Rows[i][11].ToString().Trim() + "','" + table.Rows[i][12].ToString().Trim() + "','" + table.Rows[i][13].ToString().Trim() + "','" + table.Rows[i][14].ToString().Trim() + "')");
                                }
                                else
                                {
                                    LinQBaseDao.Query(" insert into CarPrecedence(CarPrecedence_Type, CarPrecedence_StaffInfo_ID,CarPrecedence_Sate,CarPrecedence_CarTypeNames,CarPrecedence_TotalCount,CarPrecedence_TotalCountED,CarPrecedence_BeginTime,CarPrecedence_EndTime,CarPrecedence_Time,CarPrecedence_UserName,CarPrecedence_Remark) values('" + str + "'," + strcar + ",'" + table.Rows[i][6].ToString().Trim() + "','" + table.Rows[i][7].ToString().Trim() + "'," + ECount + "," + HCount + ",null,null,'" + table.Rows[i][12].ToString().Trim() + "','" + table.Rows[i][13].ToString().Trim() + "','" + table.Rows[i][14].ToString().Trim() + "')");
                                }
                            }
                        }
                        if (str == "其他")
                        {
                            string carName = table.Rows[i][1].ToString().Trim();
                            string CName = table.Rows[i][2].ToString().Trim();
                            string UName = table.Rows[i][3].ToString().Trim();
                            string cusid = "";
                            string stufid = "";
                            string psql = "";
                            DataTable dt;
                            if (!string.IsNullOrEmpty(carName))
                            {
                                dt = LinQBaseDao.Query("select Car_ID from Car where Car_Name='" + carName + "'").Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    strcar = dt.Rows[0][0].ToString();
                                }
                                else
                                {
                                    strcar = LinQBaseDao.GetSingle("insert Car(Car_Name,Car_State,Car_CreateTime,Car_ISRegister,Car_ISStaffInfo) values('" + carName + "','启动',GETDATE(),0,0)  select @@identity").ToString();
                                }
                                psql = " CarPrecedence_CarNO='" + carName + "'";
                            }
                            if (!string.IsNullOrEmpty(CName))
                            {
                                dt = LinQBaseDao.Query("select CustomerInfo_ID from CustomerInfo where CustomerInfo_Name='" + CName + "'").Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    cusid = dt.Rows[0][0].ToString();
                                }
                                else
                                {
                                    cusid = LinQBaseDao.GetSingle("insert CustomerInfo(CustomerInfo_Name,CustomerInfo_State,CustomerInfo_Time) values('" + CName + "','启动',GETDATE())  select @@identity").ToString();
                                }
                                if (!string.IsNullOrEmpty(psql))
                                {
                                    psql += " and  CarPrecedence_CustomerInfo_ID=" + cusid;
                                }
                                else
                                {
                                    psql = " CarPrecedence_CustomerInfo_ID=" + cusid;
                                }
                            }
                            else
                            {
                                cusid = "NULL";
                            }
                            if (!string.IsNullOrEmpty(UName))
                            {
                                dt = LinQBaseDao.Query("select StaffInfo_ID from StaffInfo where StaffInfo_Name='" + UName + "' and StaffInfo_Identity='" + table.Rows[i][4].ToString().Trim() + "'").Tables[0];
                                if (dt.Rows.Count > 0)
                                {
                                    stufid = dt.Rows[0][0].ToString();
                                    LinQBaseDao.Query("update StaffInfo  set  StaffInfo_Phone='" + table.Rows[i][5].ToString().Trim() + "'  where StaffInfo_ID=" + stufid);
                                }
                                else
                                {
                                    stufid = LinQBaseDao.GetSingle("Insert into StaffInfo(StaffInfo_Type,StaffInfo_Name,StaffInfo_Phone,StaffInfo_Time,StaffInfo_State,StaffInfo_Identity) values('驾驶员','" + table.Rows[i][3].ToString().Trim() + "','" + table.Rows[i][5].ToString().Trim() + "',GETDATE(),'启动','" + table.Rows[i][4].ToString().Trim() + "')      select @@identity").ToString();
                                }
                                if (!string.IsNullOrEmpty(psql))
                                {
                                    psql += " and   CarPrecedence_StaffInfo_ID=" + stufid;
                                }
                                else
                                {
                                    psql = "  CarPrecedence_StaffInfo_ID=" + stufid;
                                }
                            }
                            else
                            {
                                stufid = "NULL";
                            }

                            DataTable dts = LinQBaseDao.Query("select * from CarPrecedence  where  " + psql).Tables[0];

                            if (!string.IsNullOrEmpty(table.Rows[i][8].ToString().Trim()))
                            {
                                ECount = table.Rows[i][8].ToString().Trim();
                            }
                            else
                            {
                                ECount = "NULL";
                            }
                            if (!string.IsNullOrEmpty(table.Rows[i][9].ToString().Trim()))
                            {
                                HCount = table.Rows[i][9].ToString().Trim();
                            }
                            else
                            {
                                HCount = "NULL";
                            }
                            if (dts.Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(table.Rows[i][10].ToString().Trim()))
                                {
                                    LinQBaseDao.Query("update CarPrecedence set CarPrecedence_Sate='" + table.Rows[i][6].ToString().Trim() + "',CarPrecedence_CarTypeNames='" + table.Rows[i][7].ToString().Trim() + "',CarPrecedence_TotalCount=" + ECount + ",CarPrecedence_TotalCountED=" + HCount + ",CarPrecedence_BeginTime='" + table.Rows[i][10].ToString().Trim() + "',CarPrecedence_EndTime='" + table.Rows[i][11].ToString().Trim() + "',CarPrecedence_Time='" + table.Rows[i][12].ToString().Trim() + "',CarPrecedence_UserName='" + table.Rows[i][13].ToString().Trim() + "',CarPrecedence_Remark='" + table.Rows[i][14].ToString().Trim() + "' where  " + psql);
                                }
                                else
                                {
                                    LinQBaseDao.Query("update CarPrecedence set CarPrecedence_Sate='" + table.Rows[i][6].ToString().Trim() + "',CarPrecedence_CarTypeNames='" + table.Rows[i][7].ToString().Trim() + "',CarPrecedence_TotalCount=" + ECount + ",CarPrecedence_TotalCountED=" + HCount + ",CarPrecedence_BeginTime=null,CarPrecedence_EndTime=null,CarPrecedence_Time='" + table.Rows[i][12].ToString().Trim() + "',CarPrecedence_UserName='" + table.Rows[i][13].ToString().Trim() + "',CarPrecedence_Remark='" + table.Rows[i][14].ToString().Trim() + "' where   " + psql);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(table.Rows[i][10].ToString().Trim()))
                                {
                                    LinQBaseDao.Query(" insert into CarPrecedence(CarPrecedence_Type, CarPrecedence_CarNO,CarPrecedence_CustomerInfo_ID,CarPrecedence_StaffInfo_ID,CarPrecedence_Sate,CarPrecedence_CarTypeNames,CarPrecedence_TotalCount,CarPrecedence_TotalCountED,CarPrecedence_BeginTime,CarPrecedence_EndTime,CarPrecedence_Time,CarPrecedence_UserName,CarPrecedence_Remark) values('" + str + "','" + carName + "'," + cusid + "," + stufid + ",'" + table.Rows[i][6].ToString().Trim() + "','" + table.Rows[i][7].ToString().Trim() + "'," + ECount + "," + HCount + ",'" + table.Rows[i][10].ToString().Trim() + "','" + table.Rows[i][11].ToString().Trim() + "','" + table.Rows[i][12].ToString().Trim() + "','" + table.Rows[i][13].ToString().Trim() + "','" + table.Rows[i][14].ToString().Trim() + "')");
                                }
                                else
                                {
                                    LinQBaseDao.Query(" insert into CarPrecedence(CarPrecedence_Type, CarPrecedence_CarNO,CarPrecedence_CustomerInfo_ID,CarPrecedence_StaffInfo_ID,CarPrecedence_Sate,CarPrecedence_CarTypeNames,CarPrecedence_TotalCount,CarPrecedence_TotalCountED,CarPrecedence_BeginTime,CarPrecedence_EndTime,CarPrecedence_Time,CarPrecedence_UserName,CarPrecedence_Remark) values('" + str + "','" + carName + "'," + cusid + "," + stufid + ",'" + table.Rows[i][6].ToString().Trim() + "','" + table.Rows[i][7].ToString().Trim() + "'," + ECount + "," + HCount + ",null,null,'" + table.Rows[i][12].ToString().Trim() + "','" + table.Rows[i][13].ToString().Trim() + "','" + table.Rows[i][14].ToString().Trim() + "')");
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
                if (progressBar1.Value >= progressBar1.Maximum)
                {
                    label19.Text = fileName;
                    btnSet.Text = "导出完成";
                }
                CommonalityEntity.WriteLogData("修改", "优先车导入Execl信息", CommonalityEntity.USERNAME);
            }
            catch (System.Exception ex)
            {

            }
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

        private void cmbTypeTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbTypeTime.Text.Trim() == "永久")
                {
                    label12.Visible = false;
                    txtCount.Visible = false;
                    lblbeginTime.Visible = false;
                    lblEndTime.Visible = false;
                    txtBeginTime.Visible = false;
                    txtEndTime.Visible = false;
                }
                else if (cmbTypeTime.Text.Trim() == "次数")
                {
                    label12.Visible = true;
                    txtCount.Visible = true;
                    lblbeginTime.Visible = false;
                    lblEndTime.Visible = false;
                    txtBeginTime.Visible = false;
                    txtEndTime.Visible = false;
                }
                else
                {
                    label12.Visible = false;
                    txtCount.Visible = false;
                    lblbeginTime.Visible = true;
                    lblEndTime.Visible = true;
                    txtBeginTime.Visible = true;
                    txtEndTime.Visible = true;
                }
            }
            catch
            {

            }
        }

        private void txtBeginTime_ValueChanged(object sender, EventArgs e)
        {
            this.txtBeginTime.Format = DateTimePickerFormat.Custom;
            this.txtBeginTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
        }

        private void txtEndTime_ValueChanged(object sender, EventArgs e)
        {
            this.txtEndTime.Format = DateTimePickerFormat.Custom;
            this.txtEndTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
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
            groupBox3.Visible = false;
        }

        private void dgvCarInfo_Click(object sender, EventArgs e)
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
                groupBox3.Visible = true;
                btnSet.Text = "取消导出";

                label19.Text = "正在导出：" + filename;

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
                    label19.Text = filename;
                    btnSet.Text = "导出完成";
                }
            }
            catch { }
        }
        /// <summary>
        /// 双击加载车辆类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCarTypeNames_DoubleClick(object sender, EventArgs e)
        {
            treeViewIcP.Nodes.Clear();
            try
            {
                if (panel1.Visible)
                {
                    panel1.Visible = false;
                }
                else
                {
                    panel1.Visible = true;
                    DataTable table1 = LinQBaseDao.Query("select CarType_ID,CarType_Name from CarType where CarType_State='启动'").Tables[0];
                    TreeNode tr1;
                    for (int i = 0; i < table1.Rows.Count; i++)
                    {
                        tr1 = new TreeNode();
                        tr1.Tag = table1.Rows[i]["CarType_ID"];
                        tr1.Text = table1.Rows[i]["CarType_Name"].ToString();
                        treeViewIcP.Nodes.Add(tr1);
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ICCardForm txtICCard_Permissions_DoubleClick()");
            }
        }
        string str = null;
        ArrayList arraylist = new ArrayList();
        /// <summary>
        /// 确定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                arraylist.Clear();//清空动态数组内的成员
                str = null;
                add();
                txtCarTypeNames.Text = str;
                panel1.Visible = false;
            }
            catch
            {

            }
        }
        /// <summary>
        /// 查出选中的父级ID
        /// </summary>
        private void add()
        {
            if (treeViewIcP != null)
            {
                foreach (TreeNode tnTemp in treeViewIcP.Nodes)
                {
                    if (tnTemp.Checked == true)
                    {
                        arraylist.Add(tnTemp.Tag);
                        str += tnTemp.Text.Trim().ToString() + ";";
                    }
                }
                if (!string.IsNullOrEmpty(str))
                {
                    str = str.TrimEnd(';');
                }
            }
        }
        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_NotSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                SelectAll(false);
                txtCarTypeNames.Text = "";
                //treeViewIcP.Nodes.Clear();
                //tnTemp.ExpandAll();//展开所有子节点
            }
            catch
            {

            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectAll_Click(object sender, EventArgs e)
        {
            SelectAll(true);
        }
        /// <summary>
        /// 全选
        /// </summary>
        private void SelectAll(bool chrbool)
        {
            foreach (TreeNode tnTemp in treeViewIcP.Nodes)
            {
                if (tnTemp.Tag != null)
                {
                    tnTemp.Checked = chrbool;
                    tnTemp.ExpandAll();//展开所有子节点
                }
            }
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonVisible_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }
    }
}