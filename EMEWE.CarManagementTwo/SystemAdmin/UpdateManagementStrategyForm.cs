using EMEWE.CarManagement.CommonClass;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class UpdateManagementStrategyForm : Form
    {
        public UpdateManagementStrategyForm()
        {
            InitializeComponent();
        }
        public UpdateManagementStrategyForm(string strnames)
        {
            this.CarName = strnames;
            InitializeComponent();
        }
        DCCarManagementDataContext dc = new DCCarManagementDataContext();


        /// <summary>
        /// 需要排序的SetSort集合
        /// </summary>
        List<SetSort> SetSortList = new List<SetSort>();
        public bool SortYesOrNo = true;



        /// <summary>
        /// 已经设置管控策略的
        /// </summary>
        List<string> setdList = new List<string>();

        //用来查看是否设置过策略的，setdList因为添加好后要清空所以不能用
        List<string> setdListB = new List<string>();
        public string CarName = "";
        public string carID = "";
        /// <summary>
        /// 当前要修改通行的策略编号
        /// </summary>
        private int intDrivewayStrategyID = 0;
        /// <summary>
        /// 通行策略名称
        /// </summary>
        public string strDrivewayStrategy_Nane = "";
        /// <summary>
        /// ListBox使用标识
        /// </summary>
        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere;
        /// <summary>
        /// 分页控件行为标识（第一次："",上一页控件名称、下一页控件名称）
        /// </summary>
        private string strClickedItemName = "";
        private PageControl pc;//分页类
        //public MainForm mf;
        /// <summary>
        /// 存放要删除的管控策略编号
        /// </summary>
        List<int> listdelete = new List<int>();
        /// <summary>
        /// 管控信息的实际值(不能===该值)
        /// </summary>
        private string strControlInfo_Value = "";
        DataTable ModuleTable;//存放菜单集合
        /// <summary>
        /// 存放选中的管控信息实际值
        /// </summary>
        List<int> listControlInfo_ID = new List<int>();
        /// <summary>
        /// 存放修改前的管控策略信息实际值修改前
        /// </summary>
        List<int> listControlInfo_IDold = new List<int>();
        /// <summary>
        /// 存放通行策略编号（选多个通行策略时用）
        /// </summary>
        public List<int> listDrivewayStrategy_ID = new List<int>();
        string cartypeid = "";


        private void UpdateManagementStrategyForm_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(carID))
            {
                //txt_CarTypeName.Enabled = false;
                //txt_CarTypeName.Text = carID;
            }
            tscbxPageSize.SelectedIndex = 1;
            BindSel();
            userContext();
            lodBind();
            Initialization();
        }

        private void lodBind()
        {

            if (!CommonalityEntity.boolCepyManagementStrategy)
            {
                DataTable dts = LinQBaseDao.Query("select CarInOutRecord_ID,CarInOutRecord_Remark, CarInfo_CarType_ID from view_carstate where CarInfo_ID=" + CommonalityEntity.CarInfo_ID).Tables[0];

                if (dts.Rows.Count > 0)
                {
                    txtCarName.Text = this.CarName;
                    this.carID = CommonalityEntity.CarInfo_ID;
                    cartypeid = dts.Rows[0]["CarInfo_CarType_ID"].ToString();
                    txtDriName.Text = dts.Rows[0]["CarInOutRecord_Remark"].ToString();
                    txtDriName.Tag = dts.Rows[0]["CarInOutRecord_ID"].ToString();
                }
            }

            //车辆类型
            DataTable dt = LinQBaseDao.Query("select * from CarType").Tables[0];
            DataRow dr = dt.NewRow();
            dr["CarType_Name"] = "全部";
            dr["CarType_ID"] = "0";
            dt.Rows.InsertAt(dr, 0);
            cmbCarTypeNames.DataSource = dt;
            cmbCarTypeNames.DisplayMember = "CarType_Name";
            cmbCarTypeNames.ValueMember = "CarType_ID";
            cmbCarTypeNames.SelectedIndex = 0;

            //通行策略
            DataTable dtr = LinQBaseDao.Query("select  DrivewayStrategyRecord_Record,DrivewayStrategyRecord_ID from  DrivewayStrategyRecord ").Tables[0];
            DataRow drr = dtr.NewRow();
            drr["DrivewayStrategyRecord_Record"] = "全部";
            drr["DrivewayStrategyRecord_ID"] = "0";
            dtr.Rows.InsertAt(drr, 0);
            cbxDrivewayStrategy_Name.DataSource = dtr;
            cbxDrivewayStrategy_Name.DisplayMember = "DrivewayStrategyRecord_Record";
            cbxDrivewayStrategy_Name.ValueMember = "DrivewayStrategyRecord_ID";
            cbxDrivewayStrategy_Name.SelectedIndex = 0;
            //管控策略
            DataTable dtm = LinQBaseDao.Query("select ManagementStrategyRecord_ID,ManagementStrategyRecord_Name from ManagementStrategyRecord").Tables[0];
            DataRow drm = dtm.NewRow();
            drm["ManagementStrategyRecord_Name"] = "全部";
            drm["ManagementStrategyRecord_ID"] = "0";
            dtm.Rows.InsertAt(drm, 0);
            cbxManagementStrategy_Name.DataSource = dtm;
            cbxManagementStrategy_Name.DisplayMember = "ManagementStrategyRecord_Name";
            cbxManagementStrategy_Name.ValueMember = "ManagementStrategyRecord_ID";
            cbxManagementStrategy_Name.SelectedIndex = 0;
        }



        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {
                btn_Preservation.Enabled = true;
                btn_Preservation.Visible = true;
                btn_Delete.Enabled = true;
                btn_Delete.Visible = true;
            }
            else
            {
                btn_Preservation.Visible = ControlAttributes.BoolControl("btn_Preservation", "UpdateManagementStrategyForm", "Visible");
                btn_Preservation.Enabled = ControlAttributes.BoolControl("btn_Preservation", "UpdateManagementStrategyForm", "Enabled");

                btn_Delete.Enabled = ControlAttributes.BoolControl("btn_Delete", "UpdateManagementStrategyForm", "Enabled");
                btn_Delete.Visible = ControlAttributes.BoolControl("btn_Delete", "UpdateManagementStrategyForm", "Visible");
            }
        }
        /// <summary>
        /// 管控选择
        /// </summary>
        private void BindSel()
        {
            string[] strsel = new string[] { "通行策略", "车辆登记", "厂内控制" };
            foreach (var item in strsel)
            {
                cmbSelect.Items.Add(item);
            }
            cmbSelect.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialization()
        {
            try
            {
                if (listDrivewayStrategy_ID.Count() > 0)
                {
                    txt_DrivewayStrategy_Name.Text = strDrivewayStrategy_Nane;
                }
                pc = new PageControl();
                StetaBingMethod();
                //mf = new MainForm();
                panel1.Visible = false;
                btn_Select_Click(null, null);
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.Initialization()" + "".ToString());
            }
        }
        /// <summary>
        /// 绑定管控信息
        /// </summary>
        private void BingtreeviewMethod()
        {
            tv_ControlInfo.Nodes.Clear();
            DataSet ds = new DataSet();
            ds = LinQBaseDao.Query("select * from ControlInfo where ControlInfo_State='启动'");
            ModuleTable = ds.Tables[0];
            //加载TreeView菜单   
            LoadNode(tv_ControlInfo.Nodes, "0");
        }
        /// <summary>
        /// 递归创建TreeView菜单(模块列表)
        /// </summary>
        /// <param name="node">子菜单</param>
        /// <param name="MtID">子菜单上级ID</param>
        protected void LoadNode(TreeNodeCollection node, string MtID)
        {
            try
            {
                DataView dvList = new DataView(ModuleTable);
                if (MtID == "")
                {
                    dvList.RowFilter = " ControlInfo_HeightID is null";  //过滤父节点 
                }
                else
                {
                    dvList.RowFilter = " ControlInfo_HeightID=" + MtID;
                }
                TreeNode nodeTemp;
                foreach (DataRowView dv in dvList)
                {
                    string strControlInfo_ID = dv["ControlInfo_IDValue"].ToString();
                    string strControlInfo_Name = dv["ControlInfo_Name"].ToString();  //节点名称 
                    nodeTemp = new TreeNode();
                    nodeTemp.Tag = strControlInfo_ID;
                    nodeTemp.Text = strControlInfo_Name;
                    nodeTemp.Name = dv["ControlInfo_ID"].ToString();
                    //判断是否选择了选项，修改信息提供
                    if (listControlInfo_ID.Contains(Convert.ToInt32(strControlInfo_ID)) == true)
                    {
                        nodeTemp.Checked = true;
                    }
                    if (dv["ControlInfo_State"] != null)
                    {
                        if (dv["ControlInfo_State"].ToString() != "启动")
                        {
                            nodeTemp.ForeColor = Color.Gray;
                        }
                    }
                    node.Add(nodeTemp);  //加入节点 
                    this.LoadNode(nodeTemp.Nodes, nodeTemp.Name.ToString().Split(',')[0]);  //递归
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("ControlInfoForm.LoadNode()" + "".ToString());
            }

        }
        /// <summary>
        /// 状态绑定
        /// </summary>
        private void StetaBingMethod()
        {
            try
            {
                //绑定 管控策略状态：
                var p = DictionaryDAL.GetValueStateDictionary("01");
                int intcount = p.Count();
                cob_SelectManagementStrategy_State.DataSource = p.ToList();
                this.cob_SelectManagementStrategy_State.DisplayMember = "Dictionary_Name";
                cob_SelectManagementStrategy_State.ValueMember = "Dictionary_Value";
                if (cob_SelectManagementStrategy_State.DataSource != null)
                {
                    cob_SelectManagementStrategy_State.SelectedIndex = intcount - 1;
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.StetaBingMethod()" + "".ToString());
            }

        }
        /// <summary>
        /// 判断 车辆类型与管控策略的有效性只能同时存在一个
        /// </summary>
        private bool CarTypeSmallTicketMethod(string strControlInfo_ID)
        {
            bool rbool = true;
            try
            {
                string str = String.Format("select ControlInfo_IDValue from ControlInfo where   ControlInfo_ID='{0}'", strControlInfo_ID);
                if (LinQBaseDao.Query(str).Tables[0].Rows[0][0].ToString() == strControlInfo_Value)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "车辆类型与管控策略的有效性只能同时存在一个！", txt_ControlInfo_Name, this);
                    rbool = false;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.CarTypeSmallTicketMethod()" + "".ToString());
            }
            return rbool;
        }

        ///保存按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Preservation_Click(object sender, EventArgs e)
        {
            ///重复添加的方法记录
            string str = null;
            ///错误信息记录
            string strmessagebox = null;


            if (cmbSelect.SelectedIndex == 0 && string.IsNullOrEmpty(txt_DrivewayStrategy_Name.Text.Trim()))
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "通行策略名称不能为空！", txt_DrivewayStrategy_Name, this);
                return;
            }
            if (txt_ControlInfo_Name.Text.Trim() == "")
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "管控信息名称名称不能为空！", txt_DrivewayStrategy_Name, this);
                return;
            }
            try
            {

                #region --添加数据--

                List<ManagementStrategyRecord> msrList = new List<ManagementStrategyRecord>();

                for (int i = 0; i < arraylist.Count; i++)//根据通行策略来添加管控，循环通行策略
                {
                    //1.删除管控策略
                    string strsql = string.Format("delete ManagementStrategyRecord where  ManagementStrategyRecord_DrivewayStrategy_ID={0}", Convert.ToInt32(arraylist[i]));

                    LinQBaseDao.Query(strsql);

                    for (int s = 0; s < SetSortList.Count; s++) //循环管控信息来添加数据
                    {
                        DataTable tables = LinQBaseDao.Query("select * from dbo.ControlInfo where ControlInfo_ID=" + SetSortList[s].id + "  and ControlInfo_State='启动'").Tables[0];
                        ManagementStrategyRecord ms = new ManagementStrategyRecord();
                        ms.ManagementStrategyRecord_ControlInfo_ID = SetSortList[s].id;

                        if (cmbSelect.SelectedItem.ToString() != "通行策略")
                        {
                            ms.ManagementStrategyRecord_DrivewayStrategy_ID = null;
                        }
                        else
                        {
                            ms.ManagementStrategyRecord_DrivewayStrategy_ID = Convert.ToInt32(arraylist[i]);
                        }

                        ms.ManagementStrategyRecord_Menu_ID = cmbSelect.SelectedIndex;
                        ms.ManagementStrategyRecord_CarType_ID = Convert.ToInt32(cartypeid);
                        ms.ManagementStrategyRecord_State = "启动";
                        ms.ManagementStrategyRecord_Type = tables.Rows[0]["ControlInfo_Name"].ToString();
                        ms.ManagementStrategyRecord_Rule = tables.Rows[0]["ControlInfo_Rule"].ToString().Trim();
                        ms.ManagementStrategyRecord_Remark = txt_ManagementStrategy_Remark.Text;
                        ms.ManagementStrategyRecord_CarInfo_ID = Convert.ToInt32(this.carID);
                        ms.ManagementStrategyRecord_OffName = HelpClass.common.USERNAME;
                        ms.ManagementStrategyRecord_OffReason = txtManagementStrategyRecord_OffReason.Text;
                        ms.ManagementStrategyRecord_OffTime = CommonalityEntity.GetServersTime();
                        ms.ManagementStrategyRecord_No = SetSortList[s].sort;
                        int subIndex = txtDriName.Text.IndexOf("车") <= 0 ? 2 : txtDriName.Text.IndexOf("车");
                        ms.ManagementStrategyRecord_Name = "【" + txtDriName.Text.Substring(0, subIndex) + "】 " + tables.Rows[0]["ControlInfo_Name"].ToString();

                        msrList.Add(ms);
                    }

                    setdList.Add(arraylist[i].ToString());
                }

                #endregion

                dc.ManagementStrategyRecord.InsertAllOnSubmit(msrList);
                dc.SubmitChanges();


                for (int i = 0; i < arraylist.Count; i++)
                {
                    CommonalityEntity.WriteLogData("新增", "：" + LinQBaseDao.Query("select DrivewayStrategyRecord_Name from  dbo.DrivewayStrategyRecord where DrivewayStrategyRecord_ID=" + arraylist[i] + "").Tables[0].Rows[0][0].ToString(), CommonalityEntity.NAME);
                }

                strmessagebox = "管控策略添加成功";
                MessageBox.Show(strmessagebox);
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.btn_Preservation_Click()" + "".ToString());
            }
            finally
            {

                foreach (string item in setdList)
                {
                    setdListB.Add(item);
                }
                btn_Preservation.Enabled = true;
                btn_Preservation.Enabled = true;
                pc = new PageControl();
                btn_Select_Click(null, null);
                SetSortList.Clear();
                listControlInfo_ID.Clear();
                arraylist.Clear();
                setdList.Clear();
            }
        }
        /// <summary>
        /// 清空按钮双击事件
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
                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.btn_Empty_Click()" + "".ToString());
            }
            finally
            {
                txtDriName.Text = "";
                //intCarType_ID = 0;
                //intMuenID = 0;
                btn_Empty.Text = "清  空";
                btn_Preservation.Text = "保  存";
                btn_Empty.Enabled = true;
                panel1.Visible = false;
                listControlInfo_ID.Clear();
                txt_DrivewayStrategy_Name.Multiline = false;
                listDrivewayStrategy_ID = new List<int>();
                BingtreeviewMethod();
            }
        }
        /// <summary>
        /// 删除按钮双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                int j = 0;
                if (this.dgv_Information.SelectedRows.Count > 0)//选中删除
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //选中数量
                        int count = dgv_Information.SelectedRows.Count;
                        //遍历
                        for (int i = 0; i < count; i++)
                        {
                            int iccardid = int.Parse(this.dgv_Information.SelectedRows[i].Cells["ManagementStrategyRecord_ID"].Value.ToString());
                            Expression<Func<ManagementStrategyRecord, bool>> funuserinfo = n => n.ManagementStrategyRecord_ID == iccardid;
                            if (ManagementStrategyRecordDAL.DeleteToMany(funuserinfo))
                            {
                                j++;
                                CommonalityEntity.WriteLogData("删除", "删除管控策略：" + this.dgv_Information.SelectedRows[i].Cells["ManagementStrategyRecord_Name"].ToString() + "的信息", CommonalityEntity.USERNAME);//添加操作日志
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
                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.btn_Delete_Click()" + "".ToString());
            }
            finally
            {
                btn_Delete.Enabled = true;
                pc = new PageControl();
                btn_Select_Click(null, null);
            }
        }




        /// <summary>
        /// 通行策略名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_DrivewayStrategy_Name_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (btn_Empty.Text != "清  空") return;
                string sqlCarTypeAttribute = "";
                if (panel1.Visible)
                {
                    panel1.Visible = false;
                }
                else
                {

                    trVDrivewayStrategy_Name.Nodes.Clear();
                    panel1.Visible = true;
                    sqlCarTypeAttribute = String.Format("select distinct DrivewayStrategyRecord_Record,DrivewayStrategyRecord_ID from dbo.DrivewayStrategyRecord where DrivewayStrategyRecord_State='启动' and DrivewayStrategyRecord_CarInfo_ID={0}", this.carID);
                    DataTable table1 = LinQBaseDao.Query(sqlCarTypeAttribute).Tables[0];
                    TreeNode tr1;
                    for (int i = 0; i < table1.Rows.Count; i++)
                    {
                        tr1 = new TreeNode();
                        tr1.Tag = table1.Rows[i]["DrivewayStrategyRecord_ID"];
                        tr1.Text = table1.Rows[i]["DrivewayStrategyRecord_Record"].ToString();
                        trVDrivewayStrategy_Name.Nodes.Add(tr1);
                    }

                }
                foreach (var item in setdListB)
                {
                    foreach (TreeNode tn in trVDrivewayStrategy_Name.Nodes)
                    {
                        if (tn.Tag.ToString() == item.ToString())
                        {
                            tn.ForeColor = Color.Red;

                        }
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.txt_DrivewayStrategy_Name_DoubleClick()" + "".ToString());
            }
        }

        /// <summary>
        /// 清空文本框方法
        /// </summary>
        private void ClearMethod()
        {
            try
            {
                txtDriName.Text = "";
                txt_DrivewayStrategy_Name.Text = "";
                txtDriName.Text = "";
                txt_ManagementStrategy_Remark.Text = "";
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.ClearMethod()" + "".ToString());
            }
        }

        /// <summary>
        /// 查重方法
        /// </summary>
        private bool SelectRepeatMethod()
        {
            bool rbool = true;
            try
            {
                //string sqlselectrepeat = String.Format("select  * from ManagementStrategy where ManagementStrategy_Name='{0}'", txt_ManagementStrategy_Name.Text.Trim());
                string sqlselectrepeat = String.Format("select  * from ManagementStrategyRecord where ManagementStrategyRecord_Name='{0}'", "");
                DataSet ds = LinQBaseDao.Query(sqlselectrepeat);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    rbool = false;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.SelectRepeatMethod()" + "".ToString());
            }
            return rbool;
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
                BingMethod();
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.tscbxPageSize_SelectedIndexChanged()" + "".ToString());
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
                BingMethod();
                strClickedItemName = "";
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.bdnInfo_ItemClicked()" + "".ToString());
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
                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.SelectAllMethod()" + "".ToString());
            }
        }
        /// <summary>
        /// 显示列表绑定数据
        /// </summary>
        private void BingMethod()
        {
            try
            {

                pc.BindBoundControl(dgv_Information, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_ManagementStrategyRecordRecord_CarType_DrivewayStrategyRecordRecord_ControlInfo", "*", "ManagementStrategyRecord_ID ", "ManagementStrategyRecord_ID", 0, sqlwhere, true);

            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.BingMethod()" + "".ToString());
            }
        }

        #endregion
        private void txt_ManagementStrategy_Value_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (CommonalityEntity.DigitalMethod(e))
                {
                    // PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "管控策略默认值必须为纯数字！", txt_ManagementStrategy_Value, this);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyRecordForm.txt_ManagementStrategy_Value_KeyPress()" + "".ToString());
            }
        }

        private void txt_ManagementStrategy_No_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (CommonalityEntity.DigitalMethod(e))
                {
                    //PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "管控策略顺序号必须为纯数字！", txt_ManagementStrategy_No, this);
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("DrivewayStrategyRecordForm.txt_ManagementStrategy_No_KeyPress()" + "".ToString());
            }
        }
        /// <summary>
        /// 显示列表双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_Information_DoubleClick(object sender, EventArgs e)
        {
            //    try
            //    {
            //        DataTable tables;
            //        panel2.Visible = false;
            //        panel1.Visible = false;
            //        btn_Empty.Text = "取消修改";
            //        btn_Preservation.Text = "修改信息";
            //        bool isintid = false;
            //        try
            //        {
            //            intDrivewayStrategyID = CommonalityEntity.GetInt(dgv_Information.SelectedRows[0].Cells["DrivewayStrategyRecord_ID"].Value == null ? "" : dgv_Information.SelectedRows[0].Cells["DrivewayStrategyRecord_ID"].Value.ToString());
            //            isintid = true;
            //        }
            //        catch
            //        {
            //            isintid = false;
            //        }
            //        string strtype = dgv_Information.SelectedRows[0].Cells["ManagementStrategyRecord_Name"].Value.ToString();
            //        strtype = strtype.Substring(1, strtype.IndexOf('】') - 2);
            //        cmbSelect.Text = strtype;
            //        txt_ControlInfo_Name.Text = dgv_Information.SelectedRows[0].Cells["ControlInfo_Name"].Value == null ? "" : dgv_Information.SelectedRows[0].Cells["ControlInfo_Name"].Value.ToString(); //管控信息名称：
            //        if (isintid)
            //        {
            //            tables = LinQBaseDao.Query("select ManagementStrategyRecord_Type,ManagementStrategyRecord_ControlInfo_ID from ManagementStrategyRecord where ManagementStrategyRecord_DrivewayStrategy_ID=" + intDrivewayStrategyID + " and ManagementStrategyRecord_state='启动'").Tables[0];
            //        }
            //        else
            //        {
            //            tables = LinQBaseDao.Query("select ManagementStrategyRecord_Type,ManagementStrategyRecord_ControlInfo_ID from ManagementStrategyRecord where ManagementStrategyRecord_DrivewayStrategy_ID=" + intDrivewayStrategyID + " and ManagementStrategyRecord_state='启动'").Tables[0];
            //        }

            //        string controlinfo_Names = null;
            //        listControlInfo_ID.Clear();
            //        for (int i = 0; i < tables.Rows.Count; i++)
            //        {
            //            listControlInfo_ID.Add(Convert.ToInt32(tables.Rows[i]["ManagementStrategy_ControlInfo_ID"]));
            //            if (controlinfo_Names == null)
            //            {
            //                controlinfo_Names = tables.Rows[i]["ManagementStrategy_Type"].ToString();
            //            }
            //            else
            //                controlinfo_Names += "," + tables.Rows[i]["ManagementStrategyRecord_Type"].ToString();
            //        }
            //        txtCarTypeName.Text = dgv_Information.SelectedRows[0].Cells["CarType_Name"].Value.ToString();//管控策略状态：;
            //        txt_ControlInfo_Name.Text = controlinfo_Names;
            //        txt_DrivewayStrategy_Name.Text = dgv_Information.SelectedRows[0].Cells["DrivewayStrategyRecord_Name"].Value == null ? "" : dgv_Information.SelectedRows[0].Cells["DrivewayStrategyRecord_Name"].Value.ToString();//通行策略名称：
            //        txt_ManagementStrategy_Remark.Text = dgv_Information.SelectedRows[0].Cells["ManagementStrategyRecord_Remark"].Value == null ? "" : dgv_Information.SelectedRows[0].Cells["ManagementStrategyRecord_Remark"].Value.ToString(); //管控策略备注：
            //    }
            //    catch 
            //    {
            //        CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.dgv_Information_DoubleClick()" + "".ToString());
            //    }
        }

        /// <summary>
        /// 遍历一级菜单
        /// </summary>
        /// <param name="Menu_ID"></param>
        private void UpdateSelectRole(string strControlInfo_IDValue)
        {
            foreach (TreeNode tnTemp in tv_ControlInfo.Nodes)
            {
                if (tnTemp.Tag != null)
                {
                    if (tnTemp.Tag.ToString() == strControlInfo_IDValue)
                    {

                        tnTemp.Checked = true;
                        tnTemp.ExpandAll();//展开所有子节点

                    }
                    else
                    {
                        UpdateSelectRoleDiGui(tnTemp, strControlInfo_IDValue);
                    }
                }
            }
        }
        /// <summary>
        /// 递归出有权限的菜单并选中
        /// </summary>
        /// <param name="tn"></param>
        /// <param name="Menu_ID"></param>
        private void UpdateSelectRoleDiGui(TreeNode tn, string strControlInfo_IDValue)
        {
            foreach (TreeNode tnTemp in tn.Nodes)
            {
                if (tnTemp.Tag != null)
                {
                    if (tnTemp.Tag.ToString() == strControlInfo_IDValue)
                    {
                        tnTemp.Checked = true;
                        tnTemp.ExpandAll();//展开所有子节点
                        if (tnTemp.Text.ToString().ToUpper().Contains("SAP"))
                        {
                            tnTemp.Parent.Checked = true;
                        }
                        tnTemp.Parent.ExpandAll();
                    }
                    else
                    {
                        UpdateSelectRoleDiGui(tnTemp, strControlInfo_IDValue);
                    }
                }
            }
        }

        private void tv_ControlInfo_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }
        #region 选中父级菜单该菜单下的所有子级菜单自动选中
        private void tv_ControlInfo_AfterCheck(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Action != TreeViewAction.Unknown)
                {

                    if (e.Node.Text.ToUpper().Contains("SAP"))
                    {
                        SetNodeCheckStatus(e.Node, e.Node.Checked);
                        SetNodeStyle(e.Node);
                    }
                    else
                    {
                        SetNodeCheckStatus(e.Node, e.Node.Checked);
                        SetNodeStyle(e.Node);
                    }
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.tv_ControlInfo_AfterCheck()" + "".ToString());
            }

        }
        private void SetNodeCheckStatus(TreeNode tn, bool Checked)
        {
            try
            {
                if (tn == null) return;
                foreach (TreeNode tnChild in tn.Nodes)
                {

                    tnChild.Checked = Checked;

                    SetNodeCheckStatus(tnChild, Checked);

                }
                TreeNode tnParent = tn;
            }
            catch
            {

                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.SetNodeCheckStatus()" + "".ToString());
            }

        }

        private void SetNodeStyle(TreeNode Node)
        {
            int nNodeCount = 0;
            try
            {
                if (Node.Nodes.Count != 0)
                {
                    foreach (TreeNode tnTemp in Node.Nodes)
                    {

                        if (tnTemp.Checked == true)

                            nNodeCount++;
                    }

                    if (nNodeCount == Node.Nodes.Count)
                    {
                        Node.Checked = true;
                        Node.ExpandAll();
                        Node.ForeColor = Color.Black;
                    }
                    else if (nNodeCount == 0)
                    {
                        Node.Checked = false;
                        Node.Collapse();
                        Node.ForeColor = Color.Black;
                    }
                    else
                    {
                        Node.Checked = true;
                        Node.ForeColor = Color.Gray;
                    }
                }
                //当前节点选择完后，判断父节点的状态，调用此方法递归。   
                if (Node.Parent != null)
                {
                    SetNodeStyle(Node.Parent);
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.SetNodeStyle()" + "".ToString());
            }

        }
        /// <summary>
        /// 查出选中的父级ID
        /// </summary>
        private void add()
        {
            try
            {
                listControlInfo_ID.Clear();
                if (tv_ControlInfo != null)
                {
                    foreach (TreeNode tnTemp in tv_ControlInfo.Nodes)
                    {

                        if (tnTemp.Checked == true)
                        {
                            if (tnTemp.Text.ToUpper().Contains("SAP"))
                            { }
                            else
                            {
                                txt_ControlInfo_Name.Text += tnTemp.Text + ",";
                                listControlInfo_ID.Add(CommonalityEntity.GetInt(tnTemp.Name.ToString()));//存放入父级实际值
                            }
                        }
                        addDiGui(tnTemp);
                    }
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.add()" + "".ToString());
            }

        }
        /// <summary>
        /// 递归出所有选中的子级
        /// </summary>
        /// <param name="tn"></param>
        private void addDiGui(TreeNode tn)
        {
            try
            {
                if (tn != null)
                {
                    foreach (TreeNode tnTemp in tn.Nodes)
                    {

                        if (tnTemp.Checked == true)
                        {
                            txt_ControlInfo_Name.Text += tnTemp.Text + ",";
                            listControlInfo_ID.Add(CommonalityEntity.GetInt(tnTemp.Name.ToString()));//存放入子级实际值
                            if (!setdList.Contains(tnTemp.Name))
                            {
                                setdList.Add(tnTemp.Name);

                            }
                        }
                        addDiGui(tnTemp);

                    }
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.addDiGui()" + "".ToString());
            }

        }
        #endregion
        /// <summary>
        /// //确定按钮双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Determine_Click(object sender, EventArgs e)
        {
            try
            {
                SetSortList.Clear();
                listControlInfo_ID.Clear();
                txt_ControlInfo_Name.Text = "";
                add();
                panel2.Visible = false;
                if (SetSortList.Count != listControlInfo_ID.Count)
                {
                    setSort();
                }

                txt_ControlInfo_Name.Text = "";

                foreach (SetSort item in SetSortList)
                {
                    txt_ControlInfo_Name.Text += item.text + "；";
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm" + "".ToString());
            }

        }
        /// <summary>
        /// 详细双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lb_InDetail_Click(object sender, EventArgs e)
        {
            try
            {
                panel2.Visible = true;
            }
            catch
            {

                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.lb_InDetail_Click()" + "".ToString());
            }
        }
        /// <summary>
        /// 取消双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                panel2.Visible = false;
            }
            catch
            {

                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.btn_Cancel_Click" + "".ToString());
            }

        }

        /// <summary>
        /// 通行策略查看详细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lb_DrivewayStrategy_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_DrivewayStrategy_Name.Multiline)
                {
                    txt_DrivewayStrategy_Name.Multiline = false;
                    txt_DrivewayStrategy_Name.Size = new System.Drawing.Size(136, 21);
                }
                else
                {
                    txt_DrivewayStrategy_Name.Multiline = true;
                    txt_DrivewayStrategy_Name.Size = new System.Drawing.Size(136, 59);
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.lb_DrivewayStrategy_Click()" + "".ToString());
            }
        }

        private void dgv_Information_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dgv_Information.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString(Convert.ToString(e.RowIndex + 1, System.Globalization.CultureInfo.CurrentUICulture), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }
        string str = null;
        private void btnSumbit_Click(object sender, EventArgs e)
        {



            try
            {
                arraylist.Clear();//清空动态数组内的成员
                str = null;
                adds();
                txt_DrivewayStrategy_Name.Text = str;
                panel1.Visible = false;

                listControlInfo_ID.Clear();
                txt_ControlInfo_Name.Text = "";
            }
            catch (Exception)
            {

            }
        }
        ArrayList arraylist = new ArrayList();
        /// <summary>
        /// 查出选中的父级ID
        /// </summary>
        private void adds()
        {
            if (trVDrivewayStrategy_Name != null)
            {
                foreach (TreeNode tnTemp in trVDrivewayStrategy_Name.Nodes)
                {
                    if (tnTemp.Checked == true)
                    {
                        arraylist.Add(tnTemp.Tag);
                        if (str == null)
                        {
                            str = tnTemp.Text.Trim().ToString();
                        }
                        else
                            str += "," + tnTemp.Text.Trim().ToString();
                    }
                    addDiGui(tnTemp);
                }
            }
        }

        private void cbxDrivewayStrategy_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(cbxDrivewayStrategy_Name.SelectedValue) >= 1)
                {
                    string sqlCar = String.Format("select ManagementStrategyRecord_Name,ManagementStrategyRecord_ID from dbo.ManagementStrategyRecord where ManagementStrategyRecord_DrivewayStrategy_ID={0} and ManagementStrategyRecord_state='启动'", Convert.ToInt32(cbxDrivewayStrategy_Name.SelectedValue));
                    DataTable table1 = LinQBaseDao.Query(sqlCar).Tables[0];
                    cbxManagementStrategy_Name.DataSource = table1;
                    cbxManagementStrategy_Name.DisplayMember = "ManagementStrategyRecord_Name";
                    cbxManagementStrategy_Name.ValueMember = "ManagementStrategyRecord_ID";
                    cbxManagementStrategy_Name.SelectedIndex = -1;
                }
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 搜索按钮双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Select_Click(object sender, EventArgs e)
        {
            sqlwhere = "1=1";
            if (Convert.ToInt32(cmbCarTypeNames.SelectedValue) >= 1)
            {
                sqlwhere += " and DrivewayStrategyRecord_carType_id=" + Convert.ToInt32(cmbCarTypeNames.SelectedValue) + "";
            }
            if (Convert.ToInt32(cbxDrivewayStrategy_Name.SelectedValue) >= 1)
            {
                sqlwhere += " and ManagementStrategyRecord_DrivewayStrategy_ID=" + Convert.ToInt32(cbxDrivewayStrategy_Name.SelectedValue) + "";
            }
            if (!string.IsNullOrEmpty(cob_SelectManagementStrategy_State.Text) && cob_SelectManagementStrategy_State.Text.Trim() != "全部")
            {
                sqlwhere += " and ManagementStrategyRecord_state='" + cob_SelectManagementStrategy_State.Text.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(txtCarNo.Text.Trim()))
            {
                sqlwhere += " and CarInfo_Name like '%" + txtCarNo.Text.Trim() + "%'";
            }
            BingMethod();
        }

        /// <summary>
        /// 管控信息名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_ControlInfo_Name_DoubleClick_1(object sender, EventArgs e)
        {

            try
            {
                if (panel2.Visible)
                {
                    panel2.Visible = false;
                }
                else
                {
                    string strDrivewayStrategy_IDs = "";
                    for (int i = 0; i < arraylist.Count; i++)
                    {
                        if (i != arraylist.Count - 1)
                        {
                            strDrivewayStrategy_IDs += arraylist[i].ToString() + ",";
                        }
                        else
                        {
                            strDrivewayStrategy_IDs += arraylist[i].ToString();
                        }
                    }

                    if (cmbSelect.Text == "通行策略")
                    {
                        DataTable dt = LinQBaseDao.Query("select ManagementStrategyRecord_ControlInfo_ID from ManagementStrategyRecord m where m.ManagementStrategyRecord_CarInfo_ID =" + carID + " and m.ManagementStrategyRecord_DrivewayStrategy_ID in (" + strDrivewayStrategy_IDs + ")").Tables[0];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            listControlInfo_ID.Add(Convert.ToInt32(dt.Rows[i][0]));
                        }
                    }
                    else if (cmbSelect.Text == "车辆登记")
                    {
                        DataTable dt = LinQBaseDao.Query("select ManagementStrategyRecord_ControlInfo_ID from ManagementStrategyRecord m where m.ManagementStrategyRecord_CarInfo_ID =" + carID + " and m.ManagementStrategyRecord_Menu_ID =1").Tables[0];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            listControlInfo_ID.Add(Convert.ToInt32(dt.Rows[i][0]));
                        }
                    }
                    else
                    {
                        DataTable dt = LinQBaseDao.Query("select ManagementStrategyRecord_ControlInfo_ID from ManagementStrategyRecord m where m.ManagementStrategyRecord_CarInfo_ID =" + carID + " and m.ManagementStrategyRecord_Menu_ID =2").Tables[0];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            listControlInfo_ID.Add(Convert.ToInt32(dt.Rows[i][0]));
                        }
                    }
                    panel2.Visible = true;
                    BingtreeviewMethod();
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyRecordForm.txt_ControlInfo_Name_DoubleClick()" + "".ToString());
            }
        }
        /// <summary>
        /// 管控触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSelect.SelectedIndex == 0)
            {
                txt_DrivewayStrategy_Name.Visible = true;
            }
            else
            {
                txt_DrivewayStrategy_Name.Visible = false;
                txt_DrivewayStrategy_Name.Text = "";
            }
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
            groupBox6.Visible = true;
            dgvSetShort.Visible = true;
            setdList = new List<string>();
            SetSortList = new List<SetSort>();
            setSort();

            dgvSetShort.DataSource = SetSortList;
            dgvSetShort.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvSetShort.AutoGenerateColumns = false;
        }

        private void setSort()
        {
            foreach (TreeNode tnTemp in tv_ControlInfo.Nodes)
            {


                addDiGui(tnTemp);
            }

            SetSort s = null;

            foreach (string item in setdList)
            {

                string Sql = "select * from ControlInfo where ControlInfo_State='启动' and ControlInfo_ID=" + item;
                DataTable tab = LinQBaseDao.Query(Sql).Tables[0];
                if (tab != null)
                {
                    s = new SetSort();
                    s.text = tab.Rows[0]["ControlInfo_Name"].ToString();
                    s.sort = SetSortList.Count + 1;
                    s.id = Convert.ToInt32(tab.Rows[0]["ControlInfo_ID"]);
                    SetSortList.Add(s);

                }

            }
        }

        private void btnSetShortOk_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            groupBox6.Visible = false;
            dgvSetShort.Visible = false;
        }

        private void btnSetShortNo_Click(object sender, EventArgs e)
        {
            panel3.Visible = false;
            groupBox6.Visible = false;
            dgvSetShort.Visible = false;
            SetSortList.Clear();
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            upDown("up");
        }


        /// <summary>
        /// 上移 下移
        /// </summary>
        /// <param name="upOrDown"></param>
        private void upDown(string upOrDown)
        {

            if (dgvSetShort.SelectedRows.Count > 0)
            {

                int sort = Convert.ToInt32(dgvSetShort.SelectedRows[0].Cells[0].Value.ToString());
                if (upOrDown == "up")
                {
                    if (dgvSetShort.SelectedRows[0].Index != 0)
                    {
                        SetSort s = new SetSort();
                        s = SetSortList[sort - 1];
                        SetSortList[sort - 1] = SetSortList[sort - 2];
                        SetSortList[sort - 2] = s;
                        SetSortList[sort - 1].sort = SetSortList[sort - 2].sort;
                        SetSortList[sort - 2].sort = sort - 1;
                        dgvSetShort.AutoGenerateColumns = false;
                        dgvSetShort.DataSource = null;
                        dgvSetShort.DataSource = SetSortList;
                        dgvSetShort.Rows[dgvSetShort.SelectedRows[0].Index].Selected = false;
                        dgvSetShort.Rows[sort - 2].Selected = true;
                        dgvSetShort.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (dgvSetShort.SelectedRows[0].Index != dgvSetShort.Rows.Count - 1)
                    {
                        SetSort s = new SetSort();
                        s = SetSortList[sort - 1];
                        SetSortList[sort - 1] = SetSortList[sort];
                        SetSortList[sort] = s;
                        SetSortList[sort - 1].sort = sort;
                        SetSortList[sort].sort = sort + 1;
                        dgvSetShort.AutoGenerateColumns = false;
                        dgvSetShort.DataSource = null;
                        dgvSetShort.DataSource = SetSortList;
                        dgvSetShort.Rows[dgvSetShort.SelectedRows[0].Index].Selected = false;
                        dgvSetShort.Rows[sort].Selected = true;
                        dgvSetShort.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                    }
                    else
                    {
                        return;
                    }
                }

            }

        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            upDown("down");
        }

        private void btnNoSort_Click(object sender, EventArgs e)
        {
            SortYesOrNo = false;
        }

        private void UpdateManagementStrategyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CommonalityEntity.boolCepyManagementStrategy = true;
        }
    }
}
