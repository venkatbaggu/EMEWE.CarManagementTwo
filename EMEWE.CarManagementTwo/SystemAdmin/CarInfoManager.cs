using EMEWE.CarManagement.CommonClass;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Windows.Forms;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class CarInfoManager : Form
    {
        DateTime stime;
        /// <summary>
        /// 搜索条件
        /// </summary>
        private string where = " 1=1 ";
        //public MainForm mf;
        public CarInfoManager()
        {
            InitializeComponent();
        }

        private void CarInfoManager_Load(object sender, EventArgs e)
        {
            userContext();
            Page = new PageControl();
            Page.pageSize = CommonalityEntity.GetInt(tscbxPageSize.SelectedItem.ToString());
            BindCarAttribute();
            comboxCar_Type_DropDown();
            comboxCar_State_DropDown();
            cmbtongxing.SelectedIndex = 0;
            cbxBusinessIn.SelectedIndex = 0;
            cbxBusinessOut.SelectedIndex = 0;
            stime = CommonalityEntity.GetServersTime().AddMonths(-1);
            where = " 1=1  and CarInfo_Time> '" + stime + "'";
            tscbxPageSize.SelectedIndex = 2;
            //  GetGriddataviewLoad("");//加载
        }
        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {

                btnDel.Enabled = true;
                btnDel.Visible = true;

                btnUpdate.Enabled = true;
                btnUpdate.Visible = true;

                btnADDCar.Enabled = true;
                btnADDCar.Visible = true;

                btnChkSAP.Enabled = true;
                btnChkSAP.Visible = true;

                btnDelete.Enabled = true;
                btnDelete.Visible = true;

                btnSel.Enabled = true;
                btnSel.Visible = true;

                btnSelQuan.Enabled = true;
                btnSelQuan.Visible = true;

                btnSelOut.Enabled = true;
                btnSelOut.Visible = true;

                btn_CopyDrivewayStrategy.Enabled = true;
                btn_CopyDrivewayStrategy.Visible = true;

                btnState.Enabled = true;
                btnState.Visible = true;

                btnCel.Enabled = true;
                btnCel.Visible = true;

                btnChksSAP.Enabled = true;
                btnChksSAP.Visible = true;
            }
            else
            {
                btnDel.Enabled = ControlAttributes.BoolControl("btnDel", "CarInfoManager", "Enabled");
                btnDel.Visible = ControlAttributes.BoolControl("btnDel", "CarInfoManager", "Visible");

                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "CarInfoManager", "Enabled");
                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "CarInfoManager", "Visible");

                btnADDCar.Enabled = ControlAttributes.BoolControl("btnADDCar", "CarInfoManager", "Enabled");
                btnADDCar.Visible = ControlAttributes.BoolControl("btnADDCar", "CarInfoManager", "Visible");

                btnChkSAP.Enabled = ControlAttributes.BoolControl("btnChkSAP", "CarInfoManager", "Enabled");
                btnChkSAP.Visible = ControlAttributes.BoolControl("btnChkSAP", "CarInfoManager", "Visible");

                btnDelete.Enabled = ControlAttributes.BoolControl("btnDelete", "CarInfoManager", "Enabled");
                btnDelete.Visible = ControlAttributes.BoolControl("btnDelete", "CarInfoManager", "Visible");

                btnSel.Enabled = ControlAttributes.BoolControl("btnSel", "CarInfoManager", "Enabled");
                btnSel.Visible = ControlAttributes.BoolControl("btnSel", "CarInfoManager", "Visible");

                btnSelQuan.Enabled = ControlAttributes.BoolControl("btnSelQuan", "CarInfoManager", "Enabled");
                btnSelQuan.Visible = ControlAttributes.BoolControl("btnSelQuan", "CarInfoManager", "Visible");

                btnSelOut.Enabled = ControlAttributes.BoolControl("btnSelOut", "CarInfoManager", "Enabled");
                btnSelOut.Visible = ControlAttributes.BoolControl("btnSelOut", "CarInfoManager", "Visible");

                btn_CopyDrivewayStrategy.Enabled = ControlAttributes.BoolControl("btn_CopyDrivewayStrategy", "CarInfoManager", "Enabled");
                btn_CopyDrivewayStrategy.Visible = ControlAttributes.BoolControl("btn_CopyDrivewayStrategy", "CarInfoManager", "Visible");

                btnState.Enabled = ControlAttributes.BoolControl("btnState", "CarInfoManager", "Enabled");
                btnState.Visible = ControlAttributes.BoolControl("btnState", "CarInfoManager", "Visible");

                btnCel.Enabled = ControlAttributes.BoolControl("btnCel", "CarInfoManager", "Enabled");
                btnCel.Visible = ControlAttributes.BoolControl("btnCel", "CarInfoManager", "Visible");

                btnChksSAP.Enabled = ControlAttributes.BoolControl("btnChksSAP", "CarInfoManager", "Enabled");
                btnChksSAP.Visible = ControlAttributes.BoolControl("btnChksSAP", "CarInfoManager", "Visible");
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
                this.dgvCarInfo.Rows[i].Selected = false;
                this.dgvCarInfo.Rows[i].Cells[0].Value = false;
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
                this.dgvCarInfo.Rows[i].Cells[0].Value = true;
            }

        }
        /// <summary>
        /// 加载信息
        /// </summary>
        public void GetGriddataviewLoad(object strClickedItemName)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            Page.BindBoundControl(dgvCarInfo, strClickedItemName.ToString(), tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "case CarInfo_Bail when 'False' then '是' else '否' end as CarInfoBail ,*", where, "CarInOutRecord_ID desc");
        }

        EMEWE.CarManagement.Commons.CommonClass.PageControl Page = new EMEWE.CarManagement.Commons.CommonClass.PageControl();
        /// <summary>
        /// 选择每页条数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tscbxPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Page = new PageControl();
                Page.pageSize = CommonalityEntity.GetInt(tscbxPageSize.SelectedItem.ToString());
                ThreadPool.QueueUserWorkItem(new WaitCallback(GetGriddataviewLoad), "");
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoManager.tscbxPageSize_SelectedIndexChanged()");
            }
        }
        /// <summary>
        /// 返回显示列表中选中的车牌号
        /// </summary>
        /// <returns></returns>
        private string GetCarInfo_IDMethod()
        {
            string strCarInfo_Name = "";
            try
            {

                for (int i = 0; i < dgvCarInfo.Rows.Count; i++)
                {

                    if ((bool)dgvCarInfo.Rows[i].Cells[0].EditedFormattedValue)// (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells[0];//判断是否选中复选框
                    {
                        strCarInfo_Name = dgvCarInfo.Rows[i].Cells["CarInfo_Name"].Value.ToString();
                        CommonalityEntity.CarInfo_ID = dgvCarInfo.Rows[i].Cells["CarInfo_ID"].Value.ToString();
                    }
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("CarInfoManager.GetCarInfo_IDMethod()");
            }
            return strCarInfo_Name;
        }
        /// <summary>
        /// 复制策略（该类型的通行策略和管控策略）
        /// </summary>
        private void CopyStrategyMethod()
        {
            int intCarInfo_ID = 0;
            int intCarType_ID = 0;
            string strbox = "";
            string strsql = "";

            List<int> listCarType_ID = new List<int>();
            List<DrivewayStrategyRecord> listDrivewayStrategyRecord = new List<DrivewayStrategyRecord>();
            List<ManagementStrategyRecord> listManagementStrategyRecord = new List<ManagementStrategyRecord>();
            try
            {
                for (int i = 0; i < dgvCarInfo.Rows.Count; i++)
                {

                    if ((bool)dgvCarInfo.Rows[i].Cells[0].EditedFormattedValue)// (DataGridViewCheckBoxCell)dataGridView1.Rows[i].Cells[0];//判断是否选中复选框
                    {

                        intCarInfo_ID = dgvCarInfo.Rows[i].Cells["CarInfo_ID"].Value == null ? 0 : CommonalityEntity.GetInt(dgvCarInfo.Rows[i].Cells["CarInfo_ID"].Value.ToString());//车辆编号
                        intCarType_ID = dgvCarInfo.Rows[i].Cells["CarType_ID"].Value == null ? 0 : CommonalityEntity.GetInt(dgvCarInfo.Rows[i].Cells["CarType_ID"].Value.ToString());
                        listCarType_ID.Add(intCarType_ID);
                        //根据车辆类型编号查通行策略
                        strsql = string.Format(" select * from dbo.DrivewayStrategy where DrivewayStrategy_CarType_ID={0} ", intCarType_ID);
                        var PDrivewayStrategy = LinQBaseDao.Query(strsql);
                        DataTable dtDrivewayStrategy = PDrivewayStrategy.Tables[0];
                        if (PDrivewayStrategy != null && dtDrivewayStrategy.Rows.Count > 0)
                        {
                            foreach (var tem in dtDrivewayStrategy.AsEnumerable())
                            {
                                DrivewayStrategyRecord dsr = new DrivewayStrategyRecord();
                                dsr.DrivewayStrategyRecord_CarInfo_ID = intCarInfo_ID;
                                dsr.DrivewayStrategyRecord_CarType_ID = intCarType_ID;
                                if (tem.Field<int>("DrivewayStrategy_Driveway_ID") > 0)
                                {
                                    dsr.DrivewayStrategyRecord_Driveway_ID = tem.Field<int>("DrivewayStrategy_Driveway_ID");
                                }
                                dsr.DrivewayStrategyRecord_Name = tem.Field<string>("DrivewayStrategy_Name") == null ? null : tem.Field<string>("DrivewayStrategy_Name");
                                dsr.DrivewayStrategyRecord_Sort = tem.Field<int>("DrivewayStrategy_Sort") > 0 ? tem.Field<int>("DrivewayStrategy_Sort") : 0;
                                dsr.DrivewayStrategyRecord_State = tem.Field<string>("DrivewayStrategy_State") == null ? null : tem.Field<string>("DrivewayStrategy_State");
                                dsr.DrivewayStrategyRecord_Reason = tem.Field<string>("DrivewayStrategy_Reason") == null ? null : tem.Field<string>("DrivewayStrategy_Reason");
                                dsr.DrivewayStrategyRecord_Remark = tem.Field<string>("DrivewayStrategy_Remark") == null ? null : tem.Field<string>("DrivewayStrategy_Remark");
                                listDrivewayStrategyRecord.Add(dsr);
                            }
                        }

                        //根据车辆类型编号查管控策略

                        strsql = string.Format(" select * from dbo.ManagementStrategy where ManagementStrategy_CarType_ID={0} ", intCarType_ID);
                        var PManagementStrategy = LinQBaseDao.Query(strsql);
                        if (PManagementStrategy != null && PManagementStrategy.Tables[0].Rows.Count > 0)
                        {
                            foreach (var tem in PManagementStrategy.Tables[0].AsEnumerable())
                            {
                                ManagementStrategyRecord msr = new ManagementStrategyRecord();
                                msr.ManagementStrategyRecord_CarInfo_ID = intCarInfo_ID;
                                msr.ManagementStrategyRecord_CarType_ID = intCarType_ID;
                                if (tem.Field<int>("ManagementStrategy_ControlInfo_ID") > 0)
                                {
                                    msr.ManagementStrategyRecord_ControlInfo_ID = tem.Field<int>("ManagementStrategy_ControlInfo_ID");
                                }
                                if (tem.Field<int>("ManagementStrategy_DrivewayStrategy_ID") > 0)
                                {
                                    msr.ManagementStrategyRecord_DrivewayStrategy_ID = tem.Field<int>("ManagementStrategy_DrivewayStrategy_ID");
                                }
                                try
                                {
                                    if (tem.Field<int>("ManagementStrategy_Menu_ID") > 0)
                                    {
                                        msr.ManagementStrategyRecord_Menu_ID = tem.Field<int>("ManagementStrategy_Menu_ID");
                                    }
                                }
                                catch
                                {
                                    msr.ManagementStrategyRecord_Menu_ID = null;
                                }
                                msr.ManagementStrategyRecord_Name = tem.Field<string>("ManagementStrategy_Name") == null ? null : tem.Field<string>("ManagementStrategy_Name");
                                if (tem.Field<int>("ManagementStrategy_No") > 0)
                                {
                                    msr.ManagementStrategyRecord_No = tem.Field<int>("ManagementStrategy_No");
                                }
                                msr.ManagementStrategyRecord_Rule = tem.Field<string>("ManagementStrategy_Rule") == null ? null : tem.Field<string>("ManagementStrategy_Rule");
                                msr.ManagementStrategyRecord_Value = tem.Field<string>("ManagementStrategy_Value") == null ? null : tem.Field<string>("ManagementStrategy_Value");
                                msr.ManagementStrategyRecord_Type = tem.Field<string>("ManagementStrategy_Type") == null ? null : tem.Field<string>("ManagementStrategy_Type");
                                msr.ManagementStrategyRecord_Remark = tem.Field<string>("ManagementStrategy_Remark") == null ? null : tem.Field<string>("ManagementStrategy_Remark");
                                listManagementStrategyRecord.Add(msr);
                            }
                        }

                    }
                }
                DCCarManagementDataContext db = new DCCarManagementDataContext();
                //根据车辆类型查车辆总记录
                strsql = String.Format(" select CarInOutRecord_ID from CarInOutRecord where CarInOutRecord_CarInfo_ID={0} order by CarInOutRecord_ID desc", intCarInfo_ID);
                var PCarInOutRecord = LinQBaseDao.Query(strsql);
                DataTable dtCarInOutRecord = PCarInOutRecord.Tables[0];
                if (PCarInOutRecord != null && dtCarInOutRecord.Rows.Count > 0)
                {
                    if (dtCarInOutRecord.Rows[0][0] != null)
                    {
                        int intCarInOutRecord_ID = CommonalityEntity.GetInt(dtCarInOutRecord.Rows[0][0].ToString());
                        Expression<Func<CarInOutRecord, bool>> funCarInOutRecord = n => n.CarInOutRecord_ID == intCarInOutRecord_ID;
                        Action<CarInOutRecord> action = n =>
                        {
                            n.CarInOutRecord_Update = true;//（1：修改状态）
                        };
                        if (listManagementStrategyRecord.Count() != 1 || listDrivewayStrategyRecord.Count() != 1)
                        {
                            strbox = "只能选择一辆车进行策略复制";
                        }
                        else
                        {
                            LinQBaseDao.ADD_Delete_UpdateMethod<CarInOutRecord>(db, 1, null, funCarInOutRecord, action, false, null);
                            LinQBaseDao.ADD_Delete_UpdateMethod(db, 3, null, null, null, false, listManagementStrategyRecord);
                            if (LinQBaseDao.ADD_Delete_UpdateMethod(db, 3, null, null, null, true, listManagementStrategyRecord))
                            {
                                strbox = "成功复制策略";
                            }
                            else
                            {
                                strbox = "复制策略失败，请检查！！！！！";
                            }
                        }
                    }
                }
                else
                {
                    strbox = "复制策略失败，请检查！！！！！";
                }
            }
            catch
            {
                strbox = "复制策略失败，请检查！！！！！";
                CommonalityEntity.WriteTextLog("CarInfoManager.CopyStrategyMethod()");
            }
            finally
            {
                if (strbox != "")
                {
                    MessageBox.Show(strbox, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        /// <summary>
        /// 分页控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (e.ClickedItem.Name == "tslSelectAll1")//全选
            {
                tslSelectAlls();
                return;
            }
            if (e.ClickedItem.Name == "tslNotCheck1")//取消全选
            {
                tslNotSelects();
                return;
            }
            GetGriddataviewLoad(e.ClickedItem.Name);
        }
        #endregion



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
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSeach_Click(object sender, EventArgs e)
        {
            try
            {
                strwhere();
                GetGriddataviewLoad("");
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoForm btnSeach_Click()");
            }
        }
        private void strwhere()
        {
            where = " 1=1  ";
            if (comboxCar_State.Text.Trim() != "")
            {
                if (comboxCar_State.Text.Trim() != "全部")
                {
                    where += " and CarInfo_State='" + comboxCar_State.Text.Trim() + "'";
                }
            }
            if (comboxCar_Type.Text.Trim() != "")
            {
                if (comboxCar_Type.Text.Trim() != "全部")
                {
                    where += " and CarType_Name='" + comboxCar_Type.Text.ToString() + "'";
                }
            }
            if (comboxCu_Name.Text.Trim() != "")
            {
                where += " and CustomerInfo_Name like '%" + comboxCu_Name.Text.ToString() + "%'";
            }
            if (txtCar_Name.Text.Trim() != "")
            {
                where += " and CarInfo_Name like '%" + txtCar_Name.Text.Trim() + "%'";
            }
            if (cmbtongxing.Text.Trim() != "")
            {
                if (cmbtongxing.Text.Trim() != "全部")
                {
                    where += " and SortNumberInfo_TongXing = '" + cmbtongxing.Text.Trim() + "'";
                }
            }
            else
            {
                where += " and (SortNumberInfo_TongXing = '排队中' or SortNumberInfo_TongXing = '待通行')";
            }
            if (!string.IsNullOrEmpty(txtSamllSer.Text.Trim()))
            {
                where += " and SmallTicket_Serialnumber like '%" + this.txtSamllSer.Text.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(txtSamllSort.Text.Trim()))
            {
                where += " and SmallTicket_SortNumber like '%" + this.txtSamllSort.Text.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(cmbSX.Text.Trim()) && cmbSX.Text.Trim() != "全部")
            {
                where += " and CarType_OtherProperty = '" + this.cmbSX.Text.Trim() + "'";
            }
            if (cbxBusinessIn.Text.Trim() != "全部")
            {
                string str = cbxBusinessIn.Text.Trim() == "已授权" ? "是" : "否";
                if (str.Equals("否"))
                {
                    where += " and (CarInOutRecord_InCheck='" + str + "' or CarInOutRecord_InCheck is null)";
                }
                else
                {
                    where += " and CarInOutRecord_InCheck='" + str + "'";

                }
            }

            if (cbxBusinessOut.Text.Trim() != "全部")
            {
                string str = cbxBusinessOut.Text.Trim() == "已授权" ? "是" : "否";

                if (str.Equals("否"))
                {
                    where += " and ( CarInOutRecord_OutCheck='" + str + "' or CarInOutRecord_OutCheck is null)";
                }
                else
                {
                    where += " and CarInOutRecord_OutCheck='" + str + "'";

                }
            }
        }
        /// <summary>
        /// 普通登记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnADDCar_Click(object sender, EventArgs e)
        {
            CommonalityEntity.ISlogin = false;
            CarInfoForm cif = new CarInfoForm();
            PublicClass.ShowChildForm(cif);
            //mf = new MainForm();
            //mf.ShowChildForm(cif, this);
        }
        /// <summary>
        /// SAP校验登记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCarInfoADD_Click(object sender, EventArgs e)
        {
            SAPCarInfoForm sif = new SAPCarInfoForm();
            PublicClass.ShowChildForm(sif);
            //mf = new MainForm();
            //mf.ShowChildForm(sif, this);
        }
        /// <summary>
        /// 打印小票
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvCarInfo.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("请选择需要打印小票的车辆！");
                    return;
                }
                string carinfoid = this.dgvCarInfo.SelectedRows[0].Cells["CarInfo_ID"].Value.ToString();
                //判断是否选中
                if (carinfoid == "")
                {
                    MessageBox.Show("请选择需要打印小票的车辆！");
                    return;
                }
                //根据选择的车辆编号，查询该车辆的进出凭证，如果凭证有小票，则进行打印，无则提示，
                string sql = "Select SmallTicket_SerialNumber from SmallTicket where SmallTicket_CarInfo_ID=" + carinfoid + "";

                object obj = LinQBaseDao.GetSingle(sql);
                if (obj == null)
                {
                    return;
                }
                string SerialNumber = obj.ToString();
                string cartypeid = this.dgvCarInfo.SelectedRows[0].Cells["CarType_ID"].Value.ToString();
                //获取打印设置
                string prtSql = "select top 1 * from PrintInfo where Print_State='启动' and Print_CarType_ID=" + cartypeid + "";
                //设置车辆类型
                PrintInfo pinfo = PrintInfoDAL.GetPrint(prtSql);
                if (pinfo.Print_ID > 0)
                {
                    string prSql = "Select Top 1 " + pinfo.Print_Content + " from View_LEDShow_zj where 小票号='" + SerialNumber + "' and smallTicket_State='有效' order by CarInfo_ID desc";
                    DataSet ds = LinQBaseDao.Query(prSql);
                    PrintInfoForm pi = new PrintInfoForm(ds);
                    pi.Serialnumber = SerialNumber;
                    pi.carinfoid = carinfoid;
                    pi.cartypeid = cartypeid;
                    pi.Show();
                    CommonalityEntity.WriteLogData("新增", "新增打印小票号：" + SerialNumber, CommonalityEntity.USERNAME);
                }
                else
                {
                    MessageBox.Show("没有进行打印设置，请设置打印后，重新打印");
                    return;
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoManager btnDelete_Click：");
            }
        }
        /// <summary>
        /// 修改车辆登记基本信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (this.dgvCarInfo.SelectedRows.Count <= 0)
            {
                MessageBox.Show("请选择要修改的项！");
                return;
            }
            //判断是否选中
            if (this.dgvCarInfo.SelectedRows[0].Cells["CarInfo_ID"].Value == null)
            {
                MessageBox.Show("请选择要修改的项！");
                return;
            }
            //得到车辆信息编号 保存车辆信息编号
            CommonalityEntity.CarInfo_ID = this.dgvCarInfo.SelectedRows[0].Cells["CarInfo_ID"].Value.ToString();
            if (CommonalityEntity.CarInfo_ID == "")
            {
                MessageBox.Show("请重试");
                return;
            }
            else
            {
                CommonalityEntity.UpdateCar = true;
                CarInfoForm cdf = new CarInfoForm();
                PublicClass.ShowChildForm(cdf);
                //mf = new MainForm();
                //mf.ShowChildForm(cdf, this);
                //cdf.Show();//跳转到显示详细信息
            }
        }
        /// <summary>
        /// 将选中的车辆注销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCel_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvCarInfo.SelectedRows[0].Cells["CarInfo_ID"].Value.ToString() == "")
                {
                    MessageBox.Show("请选择要注销的项！");
                    return;
                }
                if (this.dgvCarInfo.SelectedRows.Count > 0)
                {
                    for (int i = 0; i < dgvCarInfo.SelectedRows.Count; i++)
                    {
                        int dgvCarInfo_ID = int.Parse(dgvCarInfo.SelectedRows[i].Cells["CarInfo_ID"].Value.ToString());
                        LinQBaseDao.Query(" update  View_CarState set SortNumberInfo_TongXing='已注销' where CarInfo_ID= " + dgvCarInfo_ID);
                        CommonalityEntity.WriteLogData("修改", "更新车辆信息,车牌号为：" + dgvCarInfo.SelectedRows[i].Cells["CarInfo_Name"].Value.ToString() + ";状态更改为：启动", CommonalityEntity.USERNAME);

                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoManager  btnCel_Click()");
            }
            finally
            {//重新加载数据
                GetGriddataviewLoad("");
            }
        }
        /// <summary>
        /// 将车辆的状态启动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnState_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.dgvCarInfo.SelectedRows[0].Cells["CarInfo_ID"].Value.ToString() == "")
                {
                    MessageBox.Show("请选择要启动的项！");
                    return;
                }
                if (this.dgvCarInfo.SelectedRows.Count > 0)
                {
                    for (int i = 0; i < dgvCarInfo.SelectedRows.Count; i++)
                    {
                        //修改车辆状态为注销状态
                        Expression<Func<CarInfo, bool>> fun = p => p.CarInfo_ID == int.Parse(this.dgvCarInfo.SelectedRows[i].Cells["CarInfo_ID"].Value.ToString());
                        //得到修改内容
                        string carname = "";
                        Action<CarInfo> action = a =>
                        {
                            a.CarInfo_State = "启动";
                            carname = a.CarInfo_Name;
                        };
                        if (CarInfoDAL.Update(fun, action))
                        {
                            CommonalityEntity.WriteLogData("修改", "更新车辆信息,车牌号为：" + carname + ";状态更改为：启动", CommonalityEntity.USERNAME);
                        }
                        else
                        {
                            MessageBox.Show("请重试");
                        }
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoManager  btnState_Click()");
            }
            finally
            {//重新加载数据
                GetGriddataviewLoad("");
            }
        }
        /// <summary>
        /// SAP登记 跳转到SAP登记页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChkSAP_Click(object sender, EventArgs e)
        {
            SAPCarInfoForm sapf = new SAPCarInfoForm();
            PublicClass.ShowChildForm(sapf);
            //MainForm mf = new MainForm();
            //mf.ShowChildForm(sapf, this);
        }
        /// <summary>
        /// 双击查看详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCarInfo_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvCarInfo.SelectedRows[0].Cells["CarInfo_ID"].Value.ToString() == "")
                {
                    MessageBox.Show("请选择要查看的单元格！");
                    return;
                }
                //得到车辆信息编号 保存车辆信息编号
                CommonalityEntity.CarInfo_ID = this.dgvCarInfo.SelectedRows[0].Cells["CarInfo_ID"].Value.ToString();
                if (CommonalityEntity.CarInfo_ID == "")
                {
                    MessageBox.Show("请重试");
                    return;
                }
                else
                {
                    CarInfoDetailForm cdf = new CarInfoDetailForm();
                    PublicClass.ShowChildForm(cdf);
                    //mf = new MainForm();
                    //mf.ShowChildForm(cdf, this);
                    //cdf.Show();//跳转到显示详细信息
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoManager  dgvCarInfo_DoubleClick()");
            }
        }
        /// <summary>
        /// 绑定车辆类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboxCar_Type_DropDown()
        {
            try
            {
                //查询所有的车辆类型绑定到comboxcar_type
                string sql = "Select * from CarType";
                DataTable tab = LinQBaseDao.Query(sql).Tables[0];


                DataRow row = tab.NewRow();



                row["CarType_Name"] = "全部";
                row["CarType_ID"] = 0;
                tab.Rows.InsertAt(row, 0);


                comboxCar_Type.DataSource = tab;
                comboxCar_Type.ValueMember = "CarType_ID";
                comboxCar_Type.DisplayMember = "CarType_Name";
                this.comboxCar_Type.SelectedIndex = 0;


            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoManager comboxCar_Type_DropDown:");
            }
        }
        /// <summary>
        /// 绑定车辆状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboxCar_State_DropDown()
        {
            try
            {
                this.comboxCar_State.DataSource = DictionaryDAL.GetValueDictionary("01");

                if (this.comboxCar_State.DataSource != null)
                {
                    this.comboxCar_State.DisplayMember = "Dictionary_Name";
                    this.comboxCar_State.ValueMember = "Dictionary_ID";
                    this.comboxCar_State.SelectedValue = -1;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoManager comboxCar_State_DropDown:");
                return;
            }
        }
        /// <summary>
        /// 删除车辆信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvCarInfo.SelectedRows[0].Cells["CarInfo_ID"].Value.ToString() == "")
                {
                    MessageBox.Show("请选择要删除的车辆！");
                    return;
                }
                else//进行删除
                {
                    DialogResult dr = MessageBox.Show("确定删除车辆信息吗？删除之后将会删除车辆的所有信息！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        for (int k = 0; k < this.dgvCarInfo.SelectedRows.Count; k++)
                        {
                            int carinfoid = Convert.ToInt32(this.dgvCarInfo.SelectedRows[k].Cells["CarInfo_ID"].Value.ToString());
                            string carinid = this.dgvCarInfo.SelectedRows[k].Cells["carInOutRecord_ID"].Value.ToString();
                            string carname = this.dgvCarInfo.SelectedRows[k].Cells["CarInfo_Name"].Value.ToString();

                            string carInOutRecordSql = "Select * from CarInOutRecord where CarInOutRecord_CarInfo_ID=" + carinfoid + " order by CarInOutRecord_ID desc";
                            DataSet carInOutRecordDataset = LinQBaseDao.Query(carInOutRecordSql);
                            if (carInOutRecordDataset.Tables[0].Rows.Count > 0)
                            {
                                if (Convert.ToBoolean(carInOutRecordDataset.Tables[0].Rows[0]["CarInOutRecord_ISFulfill"].ToString()) == false)
                                {
                                    DialogResult drt = MessageBox.Show("车辆：" + carname + "业务未完成，是否继续删除？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                                    if (drt != DialogResult.OK)
                                    { return; }
                                }
                                string delSql = "delete BusinessRecord where BusinessRecord_carInOutRecord_ID=" + carinid;//业务记录
                                LinQBaseDao.Query(delSql);
                                delSql = "delete CarInOutInfoRecord where CarInOutInfoRecord_CarInOutRecord_ID=" + carinid;//通行详细记录
                                LinQBaseDao.Query(delSql);
                                delSql = "delete CarInOutInfoRecord where CarInOutInfoRecord_ID=" + carinid;//通行总记录
                                LinQBaseDao.Query(delSql);
                                delSql = "delete CarPic where CarPic_CarInfo_ID=" + carinfoid;//图片记录
                                LinQBaseDao.Query(delSql);

                                string smallticketSql = "Select smallTicket_ID from SmallTicket where SmallTicket_CarInfo_ID=" + carinfoid;
                                object smallticketObj = LinQBaseDao.GetSingle(smallticketSql);
                                if (smallticketObj != null)
                                {
                                    string sortNumberInfoSql = "Select SortNumberInfo_ID from SortNumberInfo where SortNumberInfo_SmallTicket_ID=" + int.Parse(smallticketObj.ToString());
                                    object sortNumberInfoObj = LinQBaseDao.GetSingle(sortNumberInfoSql);
                                    if (sortNumberInfoObj != null)
                                    {
                                        delSql = "delete PrintRecord where PrintRecord_SortNumberInfo_ID=" + sortNumberInfoObj;//打印记录
                                        LinQBaseDao.Query(delSql);
                                    }
                                    delSql = "delete SortNumberInfo where SortNumberInfo_SmallTicket_ID=" + smallticketObj;//图片记录
                                    LinQBaseDao.Query(delSql);

                                    delSql = "delete StaffInfo_CarInfo where StaffInfo_CarInfo_SmallTicket_ID=" + smallticketObj;//删除车辆人员关联表
                                    LinQBaseDao.Query(delSql);

                                    delSql = "delete SmallTicket where SmallTicket_ID=" + smallticketObj;//删除凭证信息
                                    LinQBaseDao.Query(delSql);
                                }
                                delSql = "delete UnusualRecord where UnusualRecord_CarInfo_ID=" + carinfoid;//删除车辆异常信息
                                LinQBaseDao.Query(delSql);

                                delSql = "delete CarInfo where CarInfo_ID=" + carinfoid;//删除车辆登记信息
                                LinQBaseDao.Query(delSql);

                                CommonalityEntity.WriteLogData("删除", "删除了车牌号为：" + carname + "的所有信息", CommonalityEntity.USERNAME);//日志
                                MessageBox.Show(this, "删除成功！");
                            }
                        }
                    }

                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoManager  btnDel_Click()");
            }
            finally
            {
                //重新加载数据
                GetGriddataviewLoad("");
            }
        }
        /// <summary>
        /// 手动生成排队号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSel_Click(object sender, EventArgs e)
        {
            if (this.dgvCarInfo.SelectedRows.Count <= 0)
            {
                MessageBox.Show("请选择要生成排队号的车辆！");
                return;
            }
            if (this.dgvCarInfo.SelectedRows[0].Cells["CarInfo_ID"].Value == null)
            {
                MessageBox.Show("请选择要生成排队号的车辆！");
                return;
            }
            string CarInfo_ID = this.dgvCarInfo.SelectedRows[0].Cells["CarInfo_ID"].Value.ToString();
            string carname = this.dgvCarInfo.SelectedRows[0].Cells["CarInfo_Name"].Value.ToString();
            string SmallTicket_SortNumber = this.dgvCarInfo.SelectedRows[0].Cells["SmallTicket_SortNumber"].Value.ToString();
            if (string.IsNullOrEmpty(SmallTicket_SortNumber))
            {
                MessageBox.Show("该车辆没有进行排队，不能生成排队号");
                return;
            }
            GetSortNumber(sender, e, CarInfo_ID, carname);
        }
        /// <summary>
        /// 重新生成排队号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="CarInfo_ID"></param>
        private void GetSortNumber(object sender, EventArgs e, string CarInfo_ID, string carname)
        {
            //06 07 22 15 11 14 07
            EMEWE.CarManagement.Commons.CommonClass.CheckProperties.CommonEntity comme = new EMEWE.CarManagement.Commons.CommonClass.CheckProperties.CommonEntity();

            //得到凭证信息
            string smalSql = "Select * from SmallTicket where SmallTicket_CarInfo_ID=" + CarInfo_ID + " order by SmallTicket_ID desc";
            SmallTicket small = new SmallTicket();
            small = LinQBaseDao.GetItemsForListing<SmallTicket>(smalSql).FirstOrDefault();
            if (small.SmallTicket_SortNumber == null && small.SmallTicket_SortNumber == "")
            {
                return;
            }
            if (small.SmallTicket_SortNumber.Length > 4)
            {
                //获取选中车辆的凭证编号，根据凭证编号得到排队编号
                string sql = "Select * from SortNumberInfo where SortNumberInfo_SmallTicket_ID='" + small.SmallTicket_ID + "'";
                SortNumberInfo sort = new SortNumberInfo();
                sort = LinQBaseDao.GetItemsForListing<SortNumberInfo>(sql).FirstOrDefault();
                if (sort == null)
                {
                    MessageBox.Show("该车辆没有进行排队，不能生成排队号");
                    return;
                }
                if (sort.SortNumberInfo_SortValue <= 0 || sort.SortNumberInfo_SortValue == null)
                {
                    MessageBox.Show("该车辆没有进行排队，不能生成排队号");
                    return;
                }
                //得到当前车辆的下一通行策略，根据通行策略得到车辆通行策略中下一门岗，根据门岗得到下一门岗的该车辆类型的最大排队号
                //得到车辆类型编号
                string CarTypeSQL = "Select CarInfo_CarType_ID from CarInfo where CarInfo_ID='" + CarInfo_ID + "'";
                object objCarType_ID = null;
                objCarType_ID = LinQBaseDao.GetSingle(CarTypeSQL);
                if (objCarType_ID == null)
                {
                    MessageBox.Show("该车辆信息错误，没有关联车辆类型");
                    return;
                }
                string strsql = "  select CarInOutRecord_ISFulfill from CarInOutRecord where CarInOutRecord_CarInfo_ID=" + small.SmallTicket_CarInfo_ID;
                string str = LinQBaseDao.GetSingle(strsql).ToString();


                bool isful = Convert.ToBoolean(str);
                if (isful)
                {
                    MessageBox.Show("该车辆不需要重新生成排队号");
                    return;
                }
                else
                {
                    strsql = "select * from SortNumberInfo where SortNumberInfo_TongXing!='已注销'  and SortNumberInfo_SmallTicket_ID=" + small.SmallTicket_ID + " order by SortNumberInfo_ID desc";
                    DataTable dts = LinQBaseDao.Query(strsql).Tables[0];
                    if (dts.Rows.Count > 0)
                    {
                        MessageBox.Show("该车辆不需要重新生成排队号");
                        return;
                    }
                }
                CommonalityEntity.Position_Value = sort.SortNumberInfo_PositionValue;
                //得到最大排队号
                string SortNumberInfoSQL = "Select SortNumberInfo_SortValue from SortNumberInfo where SortNumberInfo_PositionValue='" + CommonalityEntity.Position_Value + "' and sortnumberinfo_CarTypeValue='" + sort.SortNumberInfo_CarTypeValue + "'";
                object objSortnumberInfo = null;
                objSortnumberInfo = LinQBaseDao.GetSingle(SortNumberInfoSQL);
                if (objSortnumberInfo == null)
                {
                    objSortnumberInfo = 0;
                    return;
                }
                else
                {
                    CheckProperties.ce.isSort = true;
                    CheckProperties.ce.carType_Value = LinQBaseDao.GetSingle("select  CarType_Value from CarType where CarType_ID=" + objCarType_ID).ToString();
                    CheckMethod.GetSortNumber();
                    objSortnumberInfo = CheckProperties.ce.sort_Value;
                }

                //作废之前排队号
                Expression<Func<SortNumberInfo, bool>> SortNumber = s => s.SortNumberInfo_SmallTicket_ID == small.SmallTicket_ID && s.SortNumberInfo_ISEffective == true;
                Action<SortNumberInfo> actions = f =>
                {
                    f.SortNumberInfo_ISEffective = false;
                };
                LinQBaseDao.Update(new DCCarManagementDataContext(), SortNumber, actions);

                //修改凭证信息状态
                if (sort != null)
                {
                    SortNumberInfo s = new SortNumberInfo();
                    s.SortNumberInfo_CallCount = sort.SortNumberInfo_CallCount;
                    s.SortNumberInfo_CarTypeValue = sort.SortNumberInfo_CarTypeValue;
                    s.SortNumberInfo_DrivewayValue = sort.SortNumberInfo_DrivewayValue;
                    s.SortNumberInfo_LEDCount = sort.SortNumberInfo_LEDCount;
                    s.SortNumberInfo_Operate = sort.SortNumberInfo_Operate;
                    s.SortNumberInfo_PositionValue = sort.SortNumberInfo_PositionValue;
                    s.SortNumberInfo_Reasons = sort.SortNumberInfo_Reasons;
                    s.SortNumberInfo_Remark = sort.SortNumberInfo_Remark;
                    s.SortNumberInfo_SmallTicket_ID = small.SmallTicket_ID;
                    s.SortNumberInfo_SMSCount = sort.SortNumberInfo_SMSCount;
                    s.SortNumberInfo_SortValue = (int)objSortnumberInfo + 1;
                    s.SortNumberInfo_State = "启动";
                    s.SortNumberInfo_Time = CommonalityEntity.GetServersTime();
                    s.SortNumberInfo_TongXing = "排队中";
                    s.SortNumberInfo_Type = sort.SortNumberInfo_Type;
                    s.SortNumberInfo_ISEffective = true;
                    s.SortNumberInfo_CallTime = Convert.ToDateTime("1900-01-01 00:00:00.000");
                    if (LinQBaseDao.InsertOne<SortNumberInfo>(new DCCarManagementDataContext(), s))
                    {
                        Expression<Func<SmallTicket, bool>> funs = f => f.SmallTicket_ID == small.SmallTicket_ID;
                        Action<SmallTicket> action = a =>
                        {
                            string number = CommonalityEntity.SortNumber(CheckProperties.ce.sort_Value);
                            a.SmallTicket_SortNumber = CommonalityEntity.Position_Value + sort.SortNumberInfo_CarTypeValue + number;
                        };
                        LinQBaseDao.Update(new DCCarManagementDataContext(), funs, action);
                        LinQBaseDao.Query("update CarInfo set CarInfo_Remark='手动重新生成排队号' where CarInfo_ID=" + CarInfo_ID);
                        CommonalityEntity.WriteLogData("修改", "车牌号为：" + carname + "重新生成排队号", CommonalityEntity.USERNAME);//日志
                        btnDelete_Click(sender, e);
                    }
                }
            }
            else
            {
                MessageBox.Show("车辆信息异常，请重新登记");
            }
        }
        /// <summary>
        /// 进门授权
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelQuan_Click(object sender, EventArgs e)
        {
            try
            {
                int s = 0;
                for (int i = 0; i < dgvCarInfo.Rows.Count; i++)
                {
                    if ((bool)dgvCarInfo.Rows[i].Cells[0].EditedFormattedValue)// 判断是否选中复选框
                    {
                        s++;
                    }
                }
                if (s == 0)
                {
                    MessageBox.Show("请选择要授权的车辆！");
                    return;
                }
                int sss = dgvCarInfo.Rows.Count;
                for (int i = dgvCarInfo.Rows.Count - 1; i >= 0; i--)
                {
                    if ((bool)dgvCarInfo.Rows[i].Cells[0].EditedFormattedValue)// 判断是否选中复选框
                    {
                        //为选中车辆授权
                        object objCarInOutRecord_ID = this.dgvCarInfo.Rows[i].Cells["CarInOutRecord_ID"].Value.ToString();
                        string sortnum = this.dgvCarInfo.Rows[i].Cells["SmallTicket_SortNumber"].Value.ToString();

                        //DataTable dt = LinQBaseDao.Query("select top(1) SortNumberInfo_ID from view_carstate where SortNumberInfo_TongXing='排队中' and CarInOutRecord_InCheck !='是'  and CarType_Name ='" + this.dgvCarInfo.Rows[i].Cells["CarType_Name"].Value.ToString() + "'").Tables[0];

                        //DataTable dtst = LinQBaseDao.Query("select top(1) SortNumberInfo_ID from  view_carstate where CarInOutRecord_ID=" + objCarInOutRecord_ID).Tables[0];
                        //if (dt.Rows.Count > 0 && dtst.Rows.Count > 0)
                        //{
                        //    if (!string.IsNullOrEmpty(sortnum))
                        //    {
                        //        int ssortid = int.Parse(dt.Rows[0][0].ToString());
                        //        int bsortid = int.Parse(dtst.Rows[0][0].ToString());
                        //        if (ssortid < bsortid)
                        //        {
                        //            MessageBox.Show(this, "请按排队顺序授权！");
                        //            return;
                        //        }
                        //    }
                        //}

                        string carname = this.dgvCarInfo.Rows[i].Cells["CarInfo_Name"].Value.ToString();
                        if (objCarInOutRecord_ID != null)
                        {
                            //不能为车辆重复授权
                            string busSql = "select CarInOutRecord_InCheck,CarInOutRecord_OutCheck from CarInOutRecord where  CarInOutRecord_ID=" + objCarInOutRecord_ID.ToString();
                            DataSet dataset = LinQBaseDao.Query(busSql);
                            if (dataset.Tables[0].Rows.Count > 0)
                            {
                                if (dataset.Tables[0].Rows[0]["CarInOutRecord_InCheck"].ToString() == "是")
                                {
                                    MessageBox.Show(this.dgvCarInfo.Rows[0].Cells["CarInfo_Name"].Value.ToString() + "车辆已授权");
                                    continue;
                                }
                            }
                            if (objCarInOutRecord_ID != null)
                            {
                                LinQBaseDao.Query("update CarInOutRecord set CarInOutRecord_InCheck='是',CarInOutRecord_InCheckUser='" + CommonalityEntity.USERNAME + "',CarInOutRecord_InCheckTime='" + CommonalityEntity.GetServersTime() + "' where CarInOutRecord_ID =" + objCarInOutRecord_ID);
                                CommonalityEntity.WriteLogData("修改", "修改业务记录，对车牌号为：" + carname + "进门授权", CommonalityEntity.USERNAME);
                            }
                        }
                        else
                        {
                            MessageBox.Show("车辆总记录为空！");
                        }
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoManager.btnSelOut_Click()");
            }
            finally
            {
                //重新加载数据
                GetGriddataviewLoad("");
            }
        }
        /// <summary>
        /// 出门授权
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelOut_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvCarInfo.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("请选择要授权的车辆！");
                    return;
                }
                if (this.dgvCarInfo.SelectedRows[0].Cells["CarInfo_ID"].Value == null)
                {
                    MessageBox.Show("请选择要授权的车辆！");
                    return;
                }
                string carname = this.dgvCarInfo.SelectedRows[0].Cells["CarInfo_Name"].Value.ToString();
                for (int i = 0; i < dgvCarInfo.SelectedRows.Count; i++)
                {
                    //为选中车辆授权

                    object objCarInOutRecord_ID = this.dgvCarInfo.SelectedRows[i].Cells["CarInOutRecord_ID"].Value.ToString();
                    if (objCarInOutRecord_ID != null)
                    {
                        //不能为车辆重复授权
                        string busSql = "select CarInOutRecord_InCheck,CarInOutRecord_OutCheck from CarInOutRecord where  CarInOutRecord_ID=" + objCarInOutRecord_ID.ToString() + " and CarInOutRecord_OutCheck='是'";
                        DataSet dataset = LinQBaseDao.Query(busSql);
                        if (dataset.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show(carname + "车辆已授权");
                            continue;
                        }
                        busSql = "Select count(0) from BusinessRecord where BusinessRecord_CarInOutRecord_ID='" + int.Parse(objCarInOutRecord_ID.ToString()) + "'and BusinessRecord_Type = '" + CommonalityEntity.outWeight + "'";
                        string unloadempower = LinQBaseDao.GetSingle(busSql).ToString();
                        if (unloadempower == "0")
                        {
                            if (MessageBox.Show("业务未完成，是否授权？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                if (objCarInOutRecord_ID != null)
                                {
                                    //BusinessRecord busr = new BusinessRecord();
                                    //busr.BusinessRecord_CarInOutRecord_ID = int.Parse(objCarInOutRecord_ID.ToString());
                                    //busr.BusinessRecord_UnloadEmpower = 1;
                                    //busr.BusinessRecord_UnloadEmpowerPerson = CommonalityEntity.USERNAME;
                                    //busr.BusinessRecord_UnloadEmpowerTime = CommonalityEntity.GetServersTime();
                                    //busr.BusinessRecord_Type = "出门授权";
                                    //busr.BusinessRecord_State = "启动";
                                    ////新增一条记录
                                    //if (LinQBaseDao.InsertOne<BusinessRecord>(new DCCarManagementDataContext(), busr))
                                    //{
                                    LinQBaseDao.Query("update CarInOutRecord set CarInOutRecord_OutCheck='是',CarInOutRecord_OutCheckUser='" + CommonalityEntity.USERNAME + "',CarInOutRecord_OutCheckTime='" + CommonalityEntity.GetServersTime() + "' where CarInOutRecord_ID =" + objCarInOutRecord_ID);
                                    CommonalityEntity.WriteLogData("新增", "新增业务记录，对车牌号为：" + carname + "出门授权", CommonalityEntity.USERNAME);
                                    //}
                                }
                            }
                        }
                        else
                        {
                            if (objCarInOutRecord_ID != null)
                            {
                                BusinessRecord busr = new BusinessRecord();
                                busr.BusinessRecord_CarInOutRecord_ID = int.Parse(objCarInOutRecord_ID.ToString());
                                busr.BusinessRecord_UnloadEmpower = 1;
                                busr.BusinessRecord_UnloadEmpowerPerson = CommonalityEntity.USERNAME;
                                busr.BusinessRecord_UnloadEmpowerTime = CommonalityEntity.GetServersTime();
                                busr.BusinessRecord_Type = "出门授权";
                                busr.BusinessRecord_State = "启动";
                                //新增一条记录
                                LinQBaseDao.InsertOne<BusinessRecord>(new DCCarManagementDataContext(), busr);
                                LinQBaseDao.Query("update CarInOutRecord set CarInOutRecord_OutCheck='是' where CarInOutRecord_ID =" + objCarInOutRecord_ID);
                                CommonalityEntity.WriteLogData("新增", "新增业务记录，对车牌号为：" + carname + "出门授权", CommonalityEntity.USERNAME);
                            }
                        }
                    }

                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoManager.btnSelOut_Click()");
            }
            finally
            {
                //重新加载数据
                GetGriddataviewLoad("");
            }
        }

        private void btn_CopyDrivewayStrategy_Click(object sender, EventArgs e)
        {
            try
            {
                string strname = GetCarInfo_IDMethod();
                if (strname.Split(',').Length > 1)
                {
                    MessageBox.Show("只能选择一行数据进行修改！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else if (strname == "")
                {
                    MessageBox.Show("请选择一行数据进行修改！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                UpdateDrivewayStrategyForm dsf = new UpdateDrivewayStrategyForm(strname);
                CommonalityEntity.boolCopyDrivewayStrategy = false;
                if (strname == "") return;
                PublicClass.ShowChildForm(dsf);
                //mf = new MainForm();
                //mf.ShowChildForm(dsf, this);
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoManager.btn_CopyDrivewayStrategy_Click()");
            }
        }

        private void dgvCarInfo_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }
        //单击单元格
        private void dgvCarInfo_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                int a = e.RowIndex;
                if (!Convert.ToBoolean(this.dgvCarInfo.Rows[a].Cells[0].Value))
                {
                    this.dgvCarInfo.Rows[a].Selected = true;
                    this.dgvCarInfo.Rows[a].Cells[0].Value = true;
                }
                else
                {
                    this.dgvCarInfo.Rows[a].Selected = false;
                    this.dgvCarInfo.Rows[a].Cells[0].Value = false;
                }
            }
            catch
            {

            }

        }

        private void btnChksSAP_Click(object sender, EventArgs e)
        {
            CommonalityEntity.ISlogin = true;
            CarInfoForm sapf = new CarInfoForm();
            PublicClass.ShowChildForm(sapf);
            //MainForm mf = new MainForm();
            //mf.ShowChildForm(sapf, this);
        }

    }
}
