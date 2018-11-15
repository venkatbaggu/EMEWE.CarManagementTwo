using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.DAL;
using System.Collections;
using EMEWE.CarManagement.HelpClass;
using EMEWE.CarManagement.Commons.CommonClass;
using System.Linq.Expressions;
using EMEWE.CarManagement.CommonClass;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class CarTypeForm : Form
    {
        //public MainForm mf;//主窗体

        public CarTypeForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 用以判断双击那个文本框（0：车辆类型属性 1：车辆业务类型）
        /// </summary>
        private int intidex = 0;
        /// <summary>
        /// 旧的车辆类型编码值
        /// </summary>
        private string strOldCarTypeValue = "";
        /// <summary>
        /// 存放第几项被选中
        /// </summary>
        private List<int> listcheckedListBox = new List<int>();
        /// <summary>
        /// 存放车辆属性编号
        /// </summary>
        private List<int> listCarTypeAttribute = new List<int>();
        /// <summary>
        /// 存放要添加的车辆业务类型编号
        /// </summary>
        private List<int> listCarBusinessType = new List<int>();
        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere;
        /// <summary>
        /// ListBox使用标识
        /// </summary>
        private int listboxIndex = 0;
        /// <summary>
        /// 车辆类型属性ID
        /// </summary>
        private int intCarTypeAttribute = 0;
        /// <summary>
        /// 车辆类型和业务类别关联表编号(修改时用到)
        /// </summary>
        private int intAssociateType_ID = 0;
        /// <summary>
        /// 车辆业务类型ID
        /// </summary>
        private int int_CarBusinessType = 0;

        /// <summary>
        /// 当前修改车辆类型ID
        /// </summary>
        private int intCarTypeID = 0;
        /// <summary>
        /// 存放旧的车辆类型名称
        /// </summary>
        private string strCarTypeName = "";
        /// <summary>
        /// 旧的数据【添加操作日志用到】
        /// </summary>
        private string strlogOld = "";
        /// <summary>
        /// 新的数据【添加操作日志用到】
        /// </summary>
        private string strlogNew = "";
        /// <summary>
        /// 分页控件行为标识（第一次："",上一页控件名称、下一页控件名称）
        /// </summary>
        private string strClickedItemName = "";
        /// <summary>
        /// 修改前的车辆类型值
        /// </summary>
        private string strCarType_Valueold = "";
        private PageControl pc;//分页类
        /// <summary>
        /// 存放要执行的SQL语句
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        ArrayList arraylist = new ArrayList();
        /// <summary>
        /// 要删除的车辆类型ID
        /// </summary>
        private string strCarType_ID = "";
        /// <summary>
        /// 要删除的车辆类型编号
        /// </summary>
        ArrayList alist = new ArrayList();
        /// <summary>
        /// 通行策略名称
        /// </summary>
        string drisname = "";

        private void CarTypeForm_Load(object sender, EventArgs e)
        {
            tscbxPageSize.SelectedIndex = 1;
            userContext();
            Initialization();
            BindCarAttribute();
            BindType();
            BindDriSta();
            btnUpdate.Enabled = false;
        }
        //绑定车辆属性
        private void BindCarAttribute()
        {
            DataTable dt = LinQBaseDao.Query("select * from CarAttribute where CarAttribute_State='启动'").Tables[0];
            if (dt.Rows.Count > 0)
            {
                cbxCarAttribute.DataSource = dt;
                cbxCarAttribute.DisplayMember = "CarAttribute_Name";
                cbxCarAttribute.ValueMember = "CarAttribute_ID";
                cbxCarAttribute.SelectedIndex = 0;
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
        private void BindType()
        {
            string sql = "";

            sql = "Select CarType_ID,CarType_Name from CarType ";

            DataTable dt = LinQBaseDao.Query(sql).Tables[0];
            DataRow dr = dt.NewRow();
            dr["CarType_ID"] = "0";
            dr["CarType_Name"] = "全部";
            dt.Rows.InsertAt(dr, 0);
            cmbType.DataSource = dt;
            cmbType.ValueMember = "CarType_ID";
            cmbType.DisplayMember = "CarType_Name";
        }
        private void ColumnsWidth()
        {
            this.dgv_Information.Columns[0].Width = 1;
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialization()
        {
            try
            {
                ColumnsWidth();
                strClickedItemName = "";
                pc = new PageControl();
                StateMethos();//状态绑定
                SelectMethod();//进入窗体绑定数据
                //mf = new MainForm();
                SelectAllMethod(false);
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.Initialization()");
            }


        }


        /// <summary>
        /// 得到当前新类型值
        /// </summary>
        private void CarTypeValueBind()
        {
            DataTable dt = LinQBaseDao.Query("select top(1) CarType_Value from CarType order by CarType_Value desc").Tables[0];

            if (dt.Rows.Count > 0)
            {
                txt_CarTypeValue.Text = (Convert.ToInt32(dt.Rows[0][0]) + 1).ToString();
            }
        }
        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {

                btnUpdate.Enabled = true;
                btnUpdate.Visible = true;

                btn_Save.Enabled = true;
                btn_Save.Visible = true;

                btn_Delete.Enabled = true;
                btn_Delete.Visible = true;

                btn_CurrentAdministration.Enabled = true;
                btn_CurrentAdministration.Visible = true;

                btn_Administration.Enabled = true;
                btn_Administration.Visible = true;

                btn_CarAttribute.Enabled = true;
                btn_CarAttribute.Visible = true;

                btn_BusinessType.Enabled = true;
                btn_BusinessType.Visible = true;
            }
            else
            {

                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "CarTypeForm", "Enabled");
                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "CarTypeForm", "Visible");

                btn_Save.Enabled = ControlAttributes.BoolControl("btn_Save", "CarTypeForm", "Enabled");
                btn_Save.Visible = ControlAttributes.BoolControl("btn_Save", "CarTypeForm", "Visible");

                btn_Delete.Enabled = ControlAttributes.BoolControl("btn_Delete", "CarTypeForm", "Enabled");
                btn_Delete.Visible = ControlAttributes.BoolControl("btn_Delete", "CarTypeForm", "Visible");

                btn_CurrentAdministration.Enabled = ControlAttributes.BoolControl("btn_CurrentAdministration", "CarTypeForm", "Enabled");
                btn_CurrentAdministration.Visible = ControlAttributes.BoolControl("btn_CurrentAdministration", "CarTypeForm", "Visible");

                btn_Administration.Enabled = ControlAttributes.BoolControl("btn_Administration", "CarTypeForm", "Enabled");
                btn_Administration.Visible = ControlAttributes.BoolControl("btn_Administration", "CarTypeForm", "Visible");

                btn_CarAttribute.Enabled = ControlAttributes.BoolControl("btn_CarAttribute", "CarTypeForm", "Enabled");
                btn_CarAttribute.Visible = ControlAttributes.BoolControl("btn_CarAttribute", "CarTypeForm", "Visible");

                btn_BusinessType.Enabled = ControlAttributes.BoolControl("btn_BusinessType", "CarTypeForm", "Enabled");
                btn_BusinessType.Visible = ControlAttributes.BoolControl("btn_BusinessType", "CarTypeForm", "Visible");

            }
        }
        /// <summary>
        /// 状态绑定
        /// </summary>
        private void StateMethos()
        {
            var p = DictionaryDAL.GetValueStateDictionary("01");
            int intcount = p.Count();

            if (p != null && p.Count() > 0)
            {
                cob_TypeState.DataSource = p;
                cob_TypeState.DisplayMember = "Dictionary_Name";
                cob_TypeState.ValueMember = "Dictionary_Value";
                cob_TypeState.SelectedIndex = intcount - 1;
            }

            var Pcob_CarTypeState = p.Where(n => n.Dictionary_Name != "全部").Select(n => n).ToList();
            if (Pcob_CarTypeState != null && Pcob_CarTypeState.Count() > 0)
            {
                cob_CarTypeState.DataSource = Pcob_CarTypeState;
                cob_CarTypeState.DisplayMember = "Dictionary_Name";
                cob_CarTypeState.ValueMember = "Dictionary_Value";
                cob_CarTypeState.SelectedIndex = 0;
            }

            p = DictionaryDAL.GetValueStateDictionary("13");
            intcount = p.Count();
            var Pcob_CarTypeEffective = p.Where(n => n.Dictionary_Name != "全部").Select(n => n).ToList();
            if (Pcob_CarTypeEffective != null && Pcob_CarTypeEffective.Count() > 0)
            {
                cob_CarTypeEffective.DataSource = Pcob_CarTypeEffective;
                cob_CarTypeEffective.DisplayMember = "Dictionary_Name";
                cob_CarTypeEffective.ValueMember = "Dictionary_Value";
                cob_CarTypeEffective.SelectedIndex = 0;
            }
        }



        /// <summary>
        /// 清空控件方法
        /// </summary>
        private void ClearMethod()
        {
            try
            {
                txt_CarTypeNmae.Text = "";
                cob_CarTypeEffective.Text = "";
                cob_CarTypeTemporary.Text = "";
                txt_CarTemporaryValue.Text = "";
                txt_CarTypeValue.Text = "";
                cob_CarTypeState.Text = "";
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.ClearMethod()");
            }
        }



        /// <summary>
        /// 清空
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Empty_Click(object sender, EventArgs e)
        {
            try
            {
                if (btn_Empty.Enabled)
                {
                    btn_Empty.Enabled = false;
                }

                ClearMethod();

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.btn_Empty_Click()");
            }
            finally
            {
                btn_Empty.Enabled = true;
                txt_CarTypeNmae.ReadOnly = false;
                btn_Empty.Text = "清 空";
                listCarBusinessType = new List<int>();
                listcheckedListBox = new List<int>();

            }
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Select_Click(object sender, EventArgs e)
        {
            if (btn_Select.Enabled)
            {
                btn_Select.Enabled = false;
            }
            SelectMethod();
            btn_Select.Enabled = true;
        }

        /// <summary>
        /// 车辆类型名称查重方法
        /// </summary>
        private bool SelectRepeatMethod()
        {
            bool rbool = true;
            try
            {
                string sqlselectrepeat = String.Format("select  * from CarType where CarType_Name='{0}'", txt_CarTypeNmae.Text.Trim());

                DataSet ds = LinQBaseDao.Query(sqlselectrepeat);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    return rbool = false;
                }
                return rbool = SelectRepeatCarTypeValueMethod();
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.SelectRepeatMethod()");
            }
            return rbool;
        }
        /// <summary>
        /// 车辆类型值查重
        /// </summary>
        /// <returns></returns>
        private bool SelectRepeatCarTypeValueMethod()
        {
            bool rbool = true;
            try
            {
                string sqlselectrepeat = String.Format("select  * from CarType where  CarType_Value='{0}'", txt_CarTypeValue.Text.Trim());
                DataSet ds = LinQBaseDao.Query(sqlselectrepeat);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    rbool = false;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.SelectRepeatMethod()");
            }
            return rbool;
        }
        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (NullHandleMethod())
                {
                    OthCarAttribute("", cbxCarAttribute.Text.Trim());
                    ssy = 0;
                    CarType ct = new CarType();
                    ct.CarType_UserId = CommonalityEntity.GetInt(CommonalityEntity.USERID);
                    ct.CarType_Name = txt_CarTypeNmae.Text.Trim();
                    ct.CarType_Content = txt_CarTypeContent.Text.Trim();
                    ct.CarType_State = cob_CarTypeState.Text.Trim();
                    ct.CarType_Property = cbxCarAttribute.Text.Trim();
                    ct.CarType_Remark = txt_CarTypeRemarks.Text.Trim();
                    ct.CarType_Validity = cob_CarTypeEffective.Text.Trim();
                    ct.CarType_ValidityTemporary = cob_CarTypeTemporary.Text.Trim();
                    ct.CarType_OtherProperty = butname.Trim();
                    if (!string.IsNullOrEmpty(txtInOutTime.Text.Trim()))
                    {
                        ct.CarType_InOutTime = CommonalityEntity.GetInt(txtInOutTime.Text.Trim());
                    }

                    if (!string.IsNullOrEmpty(txt_CarTemporaryValue.Text.Trim()))
                    {
                        ct.CarType_ValidityValue = CommonalityEntity.GetInt(txt_CarTemporaryValue.Text.Trim());
                    }
                    ct.CarType_Value = txt_CarTypeValue.Text.Trim();
                    ct.CarType_CreatTime = CommonalityEntity.GetServersTime();
                    ct.CarType_UserId = CommonalityEntity.USERID;
                    ct.CarType_DriSName = cmbDriSta.Text;
                    ct.CarType_BusType = txtBusType.Text.Trim();
                    if ((CarTypeDAL.InsertOneQCRecord(ct)))
                    {
                        btn_Empty_Click(btn_Empty, null); //调用清空按钮事件
                        CommonalityEntity.WriteLogData("新增", "新增车辆类型：" + txt_CarTypeNmae.Text.Trim(), CommonalityEntity.USERNAME);//添加操作日志
                        MessageBox.Show(this, "添加成功");
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.JudgeCarTypeUseMethod()");
                MessageBox.Show(this, "添加失败");
            }
            finally
            {
                BingMethod();
            }
        }


        string butname = "";
        int ssy = 0;

        /// <summary>
        /// 车辆属性最上级：内部、外部
        /// </summary>
        /// <param name="cbxcarattributename"></param>
        private void OthCarAttribute(string otherid, string cbxcarattributename)
        {
            DataTable dt;
            if (ssy > 0)
            {
                dt = LinQBaseDao.Query("select CarAttribute_HeightID,CarAttribute_Name from CarAttribute  where CarAttribute_ID=" + otherid).Tables[0];
            }
            else
            {
                dt = LinQBaseDao.Query("select CarAttribute_HeightID,CarAttribute_Name from CarAttribute  where CarAttribute_Name='" + cbxcarattributename + "'").Tables[0];
            }

            if (dt.Rows.Count > 0)
            {
                otherid = dt.Rows[0][0].ToString();
                butname = dt.Rows[0][1].ToString();
                ssy++;
                if (otherid != "0")
                {
                    OthCarAttribute(otherid, butname);
                }
            }
        }
        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                string strmessagebox = "";
                string Operationlog = "";
                strCarTypeName = "";
                strCarType_ID = "";
                alist = new ArrayList();
                if (btn_Delete.Enabled)
                {
                    btn_Delete.Enabled = false;
                }
                if (dgv_Information.SelectedRows.Count > 0)//判断是否选中行
                {
                    if (MessageBox.Show("删除将会把关联删除，确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        int count = dgv_Information.SelectedRows.Count;
                        //遍历
                        for (int i = 0; i < count; i++)
                        {
                            if (dgv_Information.SelectedRows[i].Cells["CarType_ID"].Value != null)
                            {

                                strCarType_ID += dgv_Information.SelectedRows[i].Cells["CarType_ID"].Value.ToString();
                                strCarTypeName += dgv_Information.SelectedRows[i].Cells["CarType_Name"].Value.ToString();
                                if (i < count - 1)
                                {
                                    strCarType_ID += ",";
                                }
                                alist.Add(CommonalityEntity.GetInt(dgv_Information.SelectedRows[i].Cells["CarType_ID"].Value.ToString()));
                            }
                            if (JudgeCarTypeUseMethod())//判断当前要操作的车辆类型是否正在使用
                            {
                                DCCarManagementDataContext db = new DCCarManagementDataContext();
                              //  Expression<Func<CarType, bool>> funCarType = n => alist.Contains(n.CarType_ID);

                                Expression<Func<CarType, bool>> funCarType = n => alist.ToArray().Contains(n.CarType_ID);

                                if (LinQBaseDao.ADD_Delete_UpdateMethod<CarType>(db, 2, null, funCarType, null, true, null))
                                {
                                    strmessagebox = "成功删除车辆类型";
                                    Operationlog = String.Format("删除车辆类型：{0}", strCarTypeName);
                                    CommonalityEntity.WriteLogData("删除", Operationlog, CommonalityEntity.USERNAME);
                                }
                                else
                                {
                                    strmessagebox = "删除车辆类型失败";
                                }
                            }
                            else
                            {
                                strmessagebox = "该车辆类型正在使用不能删除";
                            }
                            if (strmessagebox != "")
                            {
                                MessageBox.Show(strmessagebox, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch(Exception err)
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.btn_Delete_Click()");
            }
            finally
            {
                btn_Delete.Enabled = true;
                pc = new PageControl();
                SelectMethod();//更新数据
            }
        }
        /// <summary>
        /// 车辆类型有效期选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cob_CarTypeEffective_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cob_CarTypeEffective.Text.Trim() == "永久")
            {
                cob_CarTypeTemporary.Enabled = false;
                txt_CarTemporaryValue.Enabled = false;

            }
            if (cob_CarTypeEffective.Text.Trim() == "临时")
            {
                cob_CarTypeTemporary.Enabled = true;
                txt_CarTemporaryValue.Enabled = true;
            }
        }
        /// <summary>
        /// 判断控件为空及查重
        /// </summary>
        private bool NullHandleMethod()
        {
            bool rbool = true;
            try
            {
                string strtxt_CarTypeValue = txt_CarTypeValue.Text.Trim();
                if (string.IsNullOrEmpty(txt_CarTypeNmae.Text.Trim()))
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "车辆类型名称不能为空！", txt_CarTypeNmae, this);
                    return false;
                }
                if (string.IsNullOrEmpty(cob_CarTypeEffective.Text.Trim()))
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "车辆类型有效期不能为空！", cob_CarTypeEffective, this);
                    return false;
                }
                if (string.IsNullOrEmpty(strtxt_CarTypeValue))
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "车辆类型编码值不能为空！", txt_CarTypeValue, this);
                    return false;
                }
                if (btn_Empty.Text != "取消修改")
                {
                    if (SelectRepeatMethod())//车辆类型查重
                    {
                        PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该车辆类型已存在！", txt_CarTypeNmae, this);
                        return false;
                    }
                }
                if (strCarType_Valueold != strtxt_CarTypeValue)
                {
                    if (SelectRepeatCarTypeValueMethod())//车辆类型值查重
                    {
                        PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该车辆类型值已存在！", txt_CarTypeValue, this);
                        return false;
                    }
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.NullHandleMethod()");
            }
            return rbool;

        }
        /// <summary>
        /// 判断当前操作的车辆类型是否正在使用
        /// </summary>
        private bool JudgeCarTypeUseMethod()
        {

            bool rbool = true;
            arraylist.Clear();
            try
            {
                string strsql = String.Format("select * from dbo.ManagementStrategy where ManagementStrategy_CarType_ID in({0}) ", strCarType_ID);
                arraylist.Add(strsql);
                strsql = String.Format("select * from DrivewayStrategy where DrivewayStrategy_CarType_ID in({0}) ", strCarType_ID);
                arraylist.Add(strsql);
                strsql = String.Format("select * from PrintInfo where Print_CarType_ID in({0}) ", strCarType_ID);
                arraylist.Add(strsql);
                strsql = String.Format("select * from CarInfo where CarInfo_CarType_ID in({0}) ", strCarType_ID);
                arraylist.Add(strsql);
                if (LinQBaseDao.GetCount(arraylist) > 0)
                {
                    rbool = false;
                }


            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.JudgeCarTypeUseMethod()");
            }

            return rbool;


        }
        /// <summary>
        /// 双击进入修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_Information_DoubleClick(object sender, EventArgs e)
        {
            try
            {


                if (dgv_Information.SelectedRows.Count > 0)//选中行
                {
                    if (dgv_Information.SelectedRows.Count > 1)//判断是否选中多行
                    {
                        MessageBox.Show("只能选择一行车辆类型信息进行修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        btn_Save.Enabled = false;
                        btnUpdate.Enabled = true;
                        btn_Empty.Text = "取消修改";
                        intCarTypeID = CommonalityEntity.GetInt(dgv_Information.SelectedRows[0].Cells["CarType_ID"].Value.ToString());
                        txt_CarTypeNmae.Text = strCarTypeName = strCarTypeName = dgv_Information.SelectedRows[0].Cells["CarType_Name"].Value.ToString();
                        cob_CarTypeEffective.Text = dgv_Information.SelectedRows[0].Cells["CarType_Validity"].Value.ToString();
                        cob_CarTypeTemporary.Text = dgv_Information.SelectedRows[0].Cells["CarType_ValidityTemporary"].Value.ToString();
                        txt_CarTemporaryValue.Text = dgv_Information.SelectedRows[0].Cells["CarType_ValidityValue"].Value.ToString();
                        txt_CarTypeValue.Text = strCarType_Valueold = dgv_Information.SelectedRows[0].Cells["CarType_Value"].Value.ToString();
                        cob_CarTypeState.Text = dgv_Information.SelectedRows[0].Cells["CarType_State"].Value.ToString();
                        cbxCarAttribute.Text = dgv_Information.SelectedRows[0].Cells["CarType_Property"].Value.ToString();
                        txt_CarTypeContent.Text = dgv_Information.SelectedRows[0].Cells["CarType_Content"].Value.ToString();
                        txt_CarTypeRemarks.Text = dgv_Information.SelectedRows[0].Cells["CarType_Remark"].Value.ToString();
                        txtInOutTime.Text = dgv_Information.SelectedRows[0].Cells["CarType_InOutTime"].Value.ToString();
                        cmbDriSta.Text = dgv_Information.SelectedRows[0].Cells["CarType_DriSName"].Value.ToString();
                        txtBusType.Text = dgv_Information.SelectedRows[0].Cells["CarType_BusType"].Value.ToString();
                        drisname = cmbDriSta.Text;
                    }
                }
                else
                {
                    MessageBox.Show("请选中行！！！！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.btn_Update_Click()");
            }
        }
        /// <summary>
        /// 通行策略按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CurrentAdministration_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> liststring = new List<string>();
                DrivewayStrategyForm dsf = new DrivewayStrategyForm();
                int count = dgv_Information.SelectedRows.Count;
                string strCarTypeName = "";
                //遍历
                for (int i = 0; i < count; i++)
                {
                    if (dgv_Information.SelectedRows[i].Cells["CarType_ID"].Value != null)
                    {
                        strCarTypeName = dgv_Information.SelectedRows[i].Cells["CarType_Name"].Value == null ? "" : dgv_Information.SelectedRows[i].Cells["CarType_Name"].Value.ToString();
                        liststring.Add(strCarTypeName);
                        dsf.listCarTypeID.Add(CommonalityEntity.GetInt(dgv_Information.SelectedRows[i].Cells["CarType_ID"].Value.ToString()));
                    }
                }
                strCarTypeName = "";
                foreach (var tem in liststring.Distinct())
                {
                    strCarTypeName += tem + "," + "\r\n";
                }
                dsf.strCarTypeName = strCarTypeName;
                PublicClass.ShowChildForm(dsf);
                //mf.ShowChildForm(dsf, this);
            }
            catch
            {

                CommonalityEntity.WriteTextLog("CarTypeForm.btn_CurrentAdministration_Click()");
            }

        }
        /// <summary>
        /// 车辆类型属性
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CarAttribute_Click(object sender, EventArgs e)
        {
            CarAttributeForm caf = new CarAttributeForm();
            PublicClass.ShowChildForm(caf);
            //mf.ShowChildForm(caf, this);
        }
        /// <summary>
        /// 车辆业务类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_BusinessType_Click(object sender, EventArgs e)
        {
            BusinessTypeForm btf = new BusinessTypeForm();
            PublicClass.ShowChildForm(btf);
            //mf.ShowChildForm(btf, this);
        }


        #region 分页和加载DataGridView

        /// <summary>
        ///设置分页控件每页显示的最大条数事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tscbxPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                pc = new PageControl();
                pc.pageSize = CommonalityEntity.GetInt(tscbxPageSize.SelectedItem.ToString());
                SelectMethod();
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.tscbxPageSize_SelectedIndexChanged()");
            }
        }
        /// <summary>
        /// 分页控件响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {

                if (e.ClickedItem.Name == "tslSelectAll1")//全选
                {
                    SelectAllMethod(true);
                    return;
                }
                if (e.ClickedItem.Name == "tslNotSelect1")//取消全选
                {
                    SelectAllMethod(false);
                    return;
                }
                strClickedItemName = e.ClickedItem.Name.ToString();
                SelectMethod();
                strClickedItemName = "";
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.bdnInfo_ItemClicked()");
            }
        }
        /// <summary>
        /// 全选\取消全选
        /// </summary>
        /// <param name="rbool">true:全选 false:取消全选</param>
        private void SelectAllMethod(bool rbool)
        {
            try
            {
                for (int i = 0; i < dgv_Information.Rows.Count; i++)
                {

                    dgv_Information.Rows[i].Selected = rbool;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.SelectAllMethod()");
            }

        }
        /// <summary>
        /// 显示列表绑定数据
        /// </summary>
        private void BingMethod()
        {
            try
            {
                pc.BindBoundControl(dgv_Information, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_CarType_User", "*", "CarType_ID", "CarType_ID", 0, sqlwhere, true);
                CarTypeValueBind();
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.BingMethod()");
            }
        }
        /// <summary>
        /// 条件搜索方法
        /// </summary>
        private void SelectMethod()
        {
            try
            {
                sqlwhere = "  1=1";
                string strEstablish = txt_SelectCarBusinessType.Text.Trim();
                string strTypeName = cmbType.Text.Trim();
                string strTypeAttribute = txt_TypeAttribute.Text.Trim();
                string strtate = cob_TypeState.Text.Trim();
                if (!string.IsNullOrEmpty(strEstablish))//车辆业务类型
                {
                    sqlwhere += String.Format(" and BusinessType_Name like  '%{0}%'", strEstablish);
                }
                if (!string.IsNullOrEmpty(strTypeName) && strTypeName != "全部")//车辆类型名称
                {
                    sqlwhere += String.Format(" and CarType_Name like '%{0}%'", strTypeName);
                }
                if (!string.IsNullOrEmpty(strTypeAttribute))//车辆类型属性
                {
                    sqlwhere += String.Format(" and CarAttribute_Name like '%{0}%'", strTypeAttribute);
                }
                if (!string.IsNullOrEmpty(strtate) && strtate != "全部")
                {
                    sqlwhere += String.Format(" and CarType_State like '%{0}%'", strtate);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.SelectMethod()");
            }
            finally
            {
                BingMethod();//绑定数据
            }

        }

        #endregion
        /// <summary>
        /// 管控策略按钮双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Administration_Click(object sender, EventArgs e)
        {
            ManagementStrategyForm msf = new ManagementStrategyForm(null);
            PublicClass.ShowChildForm(msf);
            //mf.ShowChildForm(msf, this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_CarTemporaryValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (CommonalityEntity.DigitalMethod(e))
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "车辆类型有效期临时值必须为纯数字！", txt_CarTemporaryValue, this);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyForm.txt_CarTemporaryValue_KeyPress()");
            }


        }

        private void txt_CarTypeValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (CommonalityEntity.DigitalMethod(e))
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "车辆类型编码值必须为纯数字！", txt_CarTypeValue, this);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.txt_CarTypeValue_KeyPress()");
            }
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (NullHandleMethod())
                {
                    if (intCarTypeID > 0)
                    {
                        OthCarAttribute("", cbxCarAttribute.Text.Trim());
                        ssy = 0;
                        Expression<Func<CarType, bool>> funCartype = n => n.CarType_ID == intCarTypeID;
                        Action<CarType> action = n =>
                        {
                            n.CarType_UserId = CommonalityEntity.GetInt(CommonalityEntity.USERID);
                            n.CarType_Name = txt_CarTypeNmae.Text.Trim();
                            n.CarType_Content = txt_CarTypeContent.Text.Trim();
                            n.CarType_State = cob_CarTypeState.Text.Trim();
                            n.CarType_Property = cbxCarAttribute.Text.Trim();
                            n.CarType_Remark = txt_CarTypeRemarks.Text.Trim();
                            n.CarType_Validity = cob_CarTypeEffective.Text.Trim();
                            n.CarType_ValidityTemporary = cob_CarTypeTemporary.Text.Trim();
                            n.CarType_OtherProperty = butname.Trim();
                            if (!string.IsNullOrEmpty(txt_CarTemporaryValue.Text.Trim()))
                            {
                                n.CarType_ValidityValue = CommonalityEntity.GetInt(txt_CarTemporaryValue.Text.Trim());
                            }
                            if (!string.IsNullOrEmpty(txtInOutTime.Text.Trim()))
                            {
                                n.CarType_InOutTime = CommonalityEntity.GetInt(txtInOutTime.Text.Trim());
                            }
                            n.CarType_Value = txt_CarTypeValue.Text.Trim();
                            n.CarType_DriSName = cmbDriSta.Text;
                            n.CarType_BusType = txtBusType.Text.Trim();
                        };
                        if ((CarTypeDAL.Update(funCartype, action)))
                        {
                            //查出通行策略ID、通道ID

                            updateDri(cmbDriSta.Text);
                            btn_Empty_Click(btn_Empty, null); //调用清空按钮事件
                            CommonalityEntity.WriteLogData("修改", "车辆类型：" + txt_CarTypeNmae.Text.Trim(), CommonalityEntity.USERNAME);//添加操作日志
                            btn_Empty.Text = "清 空";
                            MessageBox.Show(this, "修改成功");
                        }
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.btnUpdate_Click()");
                MessageBox.Show(this, "修改失败");
            }
            finally
            {
                btn_Save.Enabled = true;
                btnUpdate.Enabled = false;
                BingMethod();
            }
        }
        /// <summary>
        /// 修改通行策略
        /// </summary>
        private void updateDri(string dname)
        {
            //通行策略没有改变
            if (drisname == dname)
            {
                return;
            }
            //该车辆类型之前没有通行策略，现在才配置
            if (string.IsNullOrEmpty(drisname))
            {
                return;
            }
            DataTable dtdi = LinQBaseDao.Query(" select DrivewayStrategy_ID, DrivewayStrategy_Driveway_ID,DrivewayStrategy_Sort from DrivewayStrategy where DrivewayStrategy_State='启动' and DrivewayStrategy_Name='" + dname + "'   order by DrivewayStrategy_Sort").Tables[0];
            string sids = "";//通行策略ID字符串
            string sortr = "";
            if (dtdi.Rows.Count > 0)
            {
                for (int i = 0; i < dtdi.Rows.Count; i++)
                {
                    sids += dtdi.Rows[i][0].ToString() + ",";
                    if (dtdi.Rows[i][2].ToString() == "1")
                    {
                        sortr = "无序";
                    }
                    else
                    {
                        sortr = "有序";
                    }
                }
                int driid = Convert.ToInt32(dtdi.Rows[0][1].ToString());//通道ID
                sids = sids.TrimEnd(',');

                #region 排队中车辆
                DataTable dtstate = LinQBaseDao.Query("select CarInfo_ID,CarInOutRecord_ID,CarInOutRecord_DrivewayStrategyS,SortNumberInfo_ID,SortNumberInfo_PositionValue,SortNumberInfo_DrivewayValue,SmallTicket_ID from View_CarState where CarInOutRecord_Update=0 and  CarType_ID=" + intCarTypeID + " and (SortNumberInfo_TongXing='排队中' or SortNumberInfo_TongXing='待通行') ").Tables[0];
                if (dtstate.Rows.Count > 0)
                {
                    //门岗值
                    string sortnumberinfo_positionvalue = LinQBaseDao.GetSingle("select p.Position_Value from Position p,Driveway d where d.Driveway_Position_ID=p.Position_ID and d.Driveway_ID= " + driid).ToString();
                    //通道值
                    string sortnumberinfo_drivewayvalue = LinQBaseDao.GetSingle("select Driveway_Value from Driveway  where  Driveway_ID=" + driid).ToString();
                    // 修改DrivewayStrategy  的CarInOutRecord_Update状态为1
                    LinQBaseDao.Query("update View_CarState set CarInOutRecord_DrivewayStrategyS='" + sids + "',CarInOutRecord_Driveway_ID='" + driid + "',CarInOutRecord_Sort='" + sortr + "',CarInOutRecord_Remark='" + dname + "' where  CarInOutRecord_Update=0 and  CarType_ID=" + intCarTypeID + "  and (SortNumberInfo_TongXing='排队中' or SortNumberInfo_TongXing='待通行') ");
                    LinQBaseDao.Query("update View_CarState set SortNumberInfo_PositionValue='" + sortnumberinfo_positionvalue + "',SortNumberInfo_DrivewayValue='" + sortnumberinfo_drivewayvalue + "' where CarInOutRecord_Update=0 and    CarType_ID=" + intCarTypeID + " and SortNumberInfo_ISEffective=1  and (SortNumberInfo_TongXing='排队中' or SortNumberInfo_TongXing='待通行')");
                }
                #endregion

                #region 已进厂车辆
                //单次进出门岗，已进厂车辆修改出厂通行策略
                dtstate = LinQBaseDao.Query("select CarInOutRecord_ID,CarInOutRecord_DrivewayStrategyS from View_CarState where  CarInOutRecord_Update=0 and CarType_ID=" + intCarTypeID + " and SortNumberInfo_TongXing='已进厂'").Tables[0];
                if (dtstate.Rows.Count > 0)
                {
                    string drivstr = "";
                    string[] drivstrs = sids.Split(',');
                    int count = 0;
                    foreach (var item in drivstrs)
                    {
                        count++;
                        drivstr += item.ToString() + ",";
                    }
                    drivstr = drivstr.TrimEnd(',');
                    if (count == 2)
                    {
                        drivstr = drivstr.Substring(0, drivstr.IndexOf(','));
                        LinQBaseDao.Query("update View_CarState set CarInOutRecord_DrivewayStrategyS='" + sids + "',CarInOutRecord_DrivewayStrategy_IDS='" + drivstr + "',CarInOutRecord_Sort='" + sortr + "',CarInOutRecord_Remark='" + dname + "' where CarInOutRecord_Update=0 and    CarType_ID=" + intCarTypeID + " and SortNumberInfo_TongXing='已进厂' ");
                    }
                    else
                    {
                        LinQBaseDao.Query("update View_CarState set CarInOutRecord_DrivewayStrategyS='" + sids + "',CarInOutRecord_Sort='" + sortr + "',CarInOutRecord_Remark='" + dname + "' where CarInOutRecord_Update=0 and   CarType_ID=" + intCarTypeID + " and SortNumberInfo_TongXing='已进厂' ");
                    }
                }
                #endregion

                #region 已出厂 业务未完成车辆
                dtstate = LinQBaseDao.Query("select CarInOutRecord_ID,CarInOutRecord_DrivewayStrategyS from View_CarState where  CarInOutRecord_Update=0 and  CarType_ID=" + intCarTypeID + " and SortNumberInfo_TongXing='已出厂' and  CarInOutRecord_ISFulfill=0 ").Tables[0];
                if (dtstate.Rows.Count > 0)
                {
                    LinQBaseDao.Query("update View_CarState set CarInOutRecord_DrivewayStrategyS='" + sids + "',CarInOutRecord_Sort='" + sortr + "',CarInOutRecord_Remark='" + dname + "' where CarInOutRecord_Update=0 and   CarType_ID=" + intCarTypeID + " and   SortNumberInfo_TongXing='已出厂' ");
                }
                #endregion
            }
        }

    }
}
