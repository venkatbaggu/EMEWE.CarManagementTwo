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
    public partial class ManagementStrategyForm : Form
    {
        public ManagementStrategyForm()
        {
            InitializeComponent();
        }
        public ManagementStrategyForm(string strnames)
        {
            this.carID = strnames;
            InitializeComponent();
        }


        public bool SortYesOrNo = true;
        /// <summary>
        /// 需要排序的SetSort集合
        /// </summary>
        List<SetSort> SetSortList = new List<SetSort>();
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
        private void ManagementStrategyForm_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(carID))
            {
                //txt_CarTypeName.Enabled = false;
                //txt_CarTypeName.Text = carID;
            }
            tscbxPageSize.SelectedIndex = 1;
            BindDriSta();
            BindSel();
            userContext();
            Initialization();
        }

        /// <summary>
        /// 通行策略
        /// </summary>
        private void BindDriSta()
        {
            DataTable dt = LinQBaseDao.Query("select distinct (DrivewayStrategy_Name) from DrivewayStrategy where DrivewayStrategy_State='启动'").Tables[0];
            cmbDriSta.DataSource = dt;
            this.cmbDriSta.DisplayMember = "DrivewayStrategy_Name";
            this.cmbDriSta.ValueMember = "DrivewayStrategy_Name";
            this.cmbDriSta.SelectedIndex = 0;

             dt = LinQBaseDao.Query("select distinct (DrivewayStrategy_Name) from DrivewayStrategy where DrivewayStrategy_State='启动'").Tables[0];
            DataRow dr = dt.NewRow();
            dr["DrivewayStrategy_Name"] = "全部";
            dt.Rows.InsertAt(dr, 0);
            cbxDrivewayStrategy_Name.DataSource = dt;
            this.cbxDrivewayStrategy_Name.DisplayMember = "DrivewayStrategy_Name";
            this.cbxDrivewayStrategy_Name.ValueMember = "DrivewayStrategy_Name";
            this.cbxDrivewayStrategy_Name.SelectedIndex = 0;
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
                btn_ControlInfo.Enabled = true;
                btn_ControlInfo.Visible = true;
                btn_DrivewayStrategy.Enabled = true;
                btn_DrivewayStrategy.Visible = true;
                btn_CarType.Enabled = true;
                btn_CarType.Visible = true;

            }
            else
            {
                btn_Preservation.Visible = ControlAttributes.BoolControl("btn_Preservation", "ManagementStrategyForm", "Visible");
                btn_Preservation.Enabled = ControlAttributes.BoolControl("btn_Preservation", "ManagementStrategyForm", "Enabled");

                btn_Delete.Enabled = ControlAttributes.BoolControl("btn_Delete", "ManagementStrategyForm", "Enabled");
                btn_Delete.Visible = ControlAttributes.BoolControl("btn_Delete", "ManagementStrategyForm", "Visible");

                btn_ControlInfo.Enabled = ControlAttributes.BoolControl("btn_ControlInfo", "ManagementStrategyForm", "Enabled");
                btn_ControlInfo.Visible = ControlAttributes.BoolControl("btn_ControlInfo", "ManagementStrategyForm", "Visible");

                btn_DrivewayStrategy.Enabled = ControlAttributes.BoolControl("btn_DrivewayStrategy", "ManagementStrategyForm", "Enabled");
                btn_DrivewayStrategy.Visible = ControlAttributes.BoolControl("btn_DrivewayStrategy", "ManagementStrategyForm", "Visible");

                btn_CarType.Enabled = ControlAttributes.BoolControl("btn_CarType", "ManagementStrategyForm", "Enabled");
                btn_CarType.Visible = ControlAttributes.BoolControl("btn_CarType", "ManagementStrategyForm", "Visible");
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
                CommonalityEntity.WriteTextLog("ManagementStrategyForm.Initialization()");
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
                    foreach (int item in listControlInfo_ID)
                    {
                        if (item == Convert.ToInt32(nodeTemp.Name))
                        {
                            nodeTemp.Checked = true;
                        }
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
                CommonalityEntity.WriteTextLog("ControlInfoForm.LoadNode()");
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
                cob_ManagementStrategy_State.DataSource = p.Where(n => n.Dictionary_Name != "全部").ToList();
                this.cob_ManagementStrategy_State.DisplayMember = "Dictionary_Name";
                cob_ManagementStrategy_State.ValueMember = "Dictionary_Value";
                if (cob_ManagementStrategy_State.DataSource != null)
                {
                    cob_ManagementStrategy_State.SelectedIndex = 0;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyForm.StetaBingMethod()");
            }

        }
        /// <summary>
        /// 管控信息按钮双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ControlInfo_Click(object sender, EventArgs e)
        {
            ControlInfoForm cif = new ControlInfoForm();
            PublicClass.ShowChildForm(cif);
            //mf.ShowChildForm(cif, this);
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
                CommonalityEntity.WriteTextLog("ManagementStrategyForm.CarTypeSmallTicketMethod()");
            }
            return rbool;
        }

        ///保存按钮双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Preservation_Click(object sender, EventArgs e)
        {
            ///重复添加的方法记录
            string str = null;
            ///错误信息记录
            string strmessagebox = null;

            if (string.IsNullOrEmpty(txt_ManagementStrategy_Name.Text.Trim()))
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "管控策略名称不能为空！", txt_ManagementStrategy_Name, this);
                return;
            }
            if (string.IsNullOrEmpty(cob_ManagementStrategy_State.Text.Trim()))
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "管控策略状态不能为空！", cob_ManagementStrategy_State, this);
                return;
            }
            if (string.IsNullOrEmpty(txt_ControlInfo_Name.Text.Trim()))
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "管控信息名称不能为空！", txt_ControlInfo_Name, this);
                return;
            }

            if (SetSortList.Count != listControlInfo_ID.Count)
            {
                SortYesOrNo = false;
            }
            else
            {
                SortYesOrNo = true;
            }
            if (SetSortList.Count != listControlInfo_ID.Count)
            {
                listControlInfo_ID = new List<int>();
                SetSortList = new List<SetSort>();
                ADDlistControlInfo_IDAndSetSortList();
            }
            try
            {

                if (SetSortList.Count == listControlInfo_ID.Count)
                {

                    for (int i = 0; i < (arraylist.Count == 0 ? 1 : arraylist.Count); i++)//根据通行策略来添加管控，循环通行策略
                    {

                        //1.删除相应的管控策略
                        string strsql = string.Format(" delete ManagementStrategy where ManagementStrategy_DriSName='{0}'", cmbDriSta.Text);
                        object objstr = arraylist.Count == 0 ? "null" : arraylist[i];

                        if (cmbSelect.Text == "通行策略")
                        {
                            if (objstr.ToString() != "null")
                            {
                                strsql += " and ManagementStrategy_DrivewayStrategy_ID= " + objstr;

                            }
                            else
                            {
                                MessageBox.Show("请选择通行策略！");
                                return;
                            }

                        }
                        else
                        {
                            strsql += "  and ManagementStrategy_Menu_ID=" + cmbSelect.SelectedIndex + " ";
                        }

                        DataSet ds = LinQBaseDao.Query(strsql);
                        for (int j = 0; j < SetSortList.Count; j++)
                        {

                            DataTable tables = LinQBaseDao.Query("select * from dbo.ControlInfo where ControlInfo_ID=" + SetSortList[j].id + "  and ControlInfo_State='启动'").Tables[0];

                            ManagementStrategy ms = new ManagementStrategy();
                            ms.ManagementStrategy_ControlInfo_ID = SetSortList[j].id;
                            ms.ManagementStrategy_Name = "【" + txt_ManagementStrategy_Name.Text.ToString().Trim() + "】 " + tables.Rows[0]["ControlInfo_Name"].ToString();
                            if (arraylist.Count != 0)
                            {
                                ms.ManagementStrategy_DrivewayStrategy_ID = Convert.ToInt32(arraylist[i]);
                            }
                            ms.ManagementStrategy_State = "启动";
                            ms.ManagementStrategy_Type = tables.Rows[0]["ControlInfo_Name"].ToString();
                            ms.ManagementStrategy_Rule = tables.Rows[0]["ControlInfo_Rule"].ToString().Trim();
                            ms.ManagementStrategy_Remark = txt_ManagementStrategy_Remark.Text;
                            ms.ManagementStrategy_No = SortYesOrNo == true ? SetSortList[j].sort : 1;
                            if (cmbSelect.SelectedItem.ToString() != "通行策略")
                            {
                                ms.ManagementStrategy_DrivewayStrategy_ID = null;
                            }
                            else
                            {
                                ms.ManagementStrategy_DrivewayStrategy_ID = Convert.ToInt32(arraylist[i]);
                            }

                            ms.ManagementStrategy_Menu_ID = cmbSelect.SelectedIndex;
                            ms.ManagementStrategy_DriSName = cmbDriSta.Text;

                            if (ManagementStrategyDAL.InsertOneQCRecord(ms))
                            {
                                strmessagebox = "管控策略添加成功";
                                CommonalityEntity.WriteLogData("新增", "新增管控策略：" + ms.ManagementStrategy_Name, CommonalityEntity.NAME);
                            }
                            else
                            {
                                if (strmessagebox == null)
                                {
                                    strmessagebox = tables.Rows[0]["ControlInfo_Name"].ToString();
                                }
                                else
                                    strmessagebox += "," + tables.Rows[0]["ControlInfo_Name"].ToString();

                            }
                        }

                    }

                }

                MessageBox.Show("保存成功！", "提示");
                arraylist.Clear();
                txt_DrivewayStrategy_Name.Text = "";
                txt_ControlInfo_Name.Text = "";
                SetSortList.Clear();
                listControlInfo_ID.Clear();
            }
            catch
            {
                arraylist.Clear();
                CommonalityEntity.WriteTextLog("ManagementStrategyForm.btn_Preservation_Click()");
            }
            finally
            {
                btn_Preservation.Enabled = true;
                btn_Preservation.Enabled = true;
                pc = new PageControl();
                btn_Select_Click(null, null);
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
                CommonalityEntity.WriteTextLog("ManagementStrategyForm.btn_Empty_Click()");
            }
            finally
            {
                cmbDriSta.Text = "";
                //intCarType_ID = 0;
                //intMuenID = 0;
                btn_Empty.Text = "清  空";
                btn_Preservation.Text = "保  存";
                btn_Empty.Enabled = true;
                panel1.Visible = false;
                listControlInfo_ID.Clear();
                SetSortList.Clear();
                arraylist.Clear();
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
                            int iccardid = int.Parse(this.dgv_Information.SelectedRows[i].Cells["ManagementStrategy_ID"].Value.ToString());
                            Expression<Func<ManagementStrategy, bool>> funuserinfo = n => n.ManagementStrategy_ID == iccardid;
                            if (ManagementStrategyDAL.DeleteToMany(funuserinfo))
                            {
                                j++;
                                CommonalityEntity.WriteLogData("删除", "删除管控策略：" + this.dgv_Information.SelectedRows[i].Cells["ManagementStrategy_Name"].ToString() + "的信息", CommonalityEntity.USERNAME);//添加操作日志
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
                CommonalityEntity.WriteTextLog("ManagementStrategyForm.btn_Delete_Click()");
            }
            finally
            {
                btn_Delete.Enabled = true;
                pc = new PageControl();
                btn_Select_Click(null, null);
            }
        }

        /// <summary>
        /// 通行策略双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_DrivewayStrategy_Click(object sender, EventArgs e)
        {
            DrivewayStrategyForm dsf = new DrivewayStrategyForm();
            PublicClass.ShowChildForm(dsf);
            //mf.ShowChildForm(dsf, this);
        }
        /// <summary>
        /// 车辆类型双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CarType_Click(object sender, EventArgs e)
        {
            CarTypeForm ctf = new CarTypeForm();
            PublicClass.ShowChildForm(ctf);
            //mf.ShowChildForm(ctf, this);
        }

        /// <summary>
        /// ListBox绑定数据——没用上（ZJ）
        /// </summary>
        /// <param name="sqlCarTypeAttributem">控件名称</param>
        /// <param name="strDisplayMember">显示值</param>
        /// <param name="strValueMember">实际绑定值</param>
        private void UserBing(string sqlCarTypeAttributem, string strDisplayMember, string strValueMember)
        {
            try
            {
                //绑定车辆信息
                if (strDisplayMember.Trim().Length > 0)
                {
                    DataTable dt = LinQBaseDao.Query(sqlCarTypeAttributem).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        lb_CarTypeAttribute.DataSource = dt;
                        lb_CarTypeAttribute.DisplayMember = strDisplayMember;
                        lb_CarTypeAttribute.ValueMember = strValueMember;
                        panel1.Visible = true;
                    }

                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyForm.UserBing()");
            }
        }

        /// <summary>
        /// 判断车辆类型有效性
        /// </summary>
        private void CarTypeSmallTicketMethod(int intCarTypeId)
        {
            string str = LinQBaseDao.Query(String.Format(" Select CarType_Validity from CarType where  CarType_ID={0}", intCarTypeId)).Tables[0].Rows[0][0].ToString();
            if (str == "临时")
            {
                strControlInfo_Value = "5001";//永久
            }
            if (str == "永久")
            {
                strControlInfo_Value = "5002";//临时 
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
                string sqlCarTypeAttribute = "";
                if (panel1.Visible)
                {
                    panel1.Visible = false;
                }
                else
                {
                    trVDrivewayStrategy_Name.Nodes.Clear();
                    panel1.Visible = true;
                    sqlCarTypeAttribute = String.Format("select  DrivewayStrategy_Record,DrivewayStrategy_ID from DrivewayStrategy where DrivewayStrategy_Name='{0}'  and DrivewayStrategy_State='启动' order by DrivewayStrategy_Sort", cmbDriSta.Text);
                    DataTable table1 = LinQBaseDao.Query(sqlCarTypeAttribute).Tables[0];
                    TreeNode tr1;
                    for (int i = 0; i < table1.Rows.Count; i++)
                    {
                        tr1 = new TreeNode();
                        tr1.Tag = table1.Rows[i]["DrivewayStrategy_ID"];
                        tr1.Text = table1.Rows[i]["DrivewayStrategy_Record"].ToString();
                        trVDrivewayStrategy_Name.Nodes.Add(tr1);
                    }
                    foreach (var item in arraylist)
                    {
                        foreach (TreeNode tn in trVDrivewayStrategy_Name.Nodes)
                        {
                            if (tn.Tag.ToString() == item.ToString())
                            {
                                tn.ForeColor = Color.Red;
                            }
                        }
                    }
                    if (trVDrivewayStrategy_Name.Nodes.Count > 13)
                    {
                        tr1 = new TreeNode();
                        trVDrivewayStrategy_Name.Nodes.Add(tr1);
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyForm.txt_DrivewayStrategy_Name_DoubleClick()");
            }
        }

        /// <summary>
        /// 清空文本框方法
        /// </summary>
        private void ClearMethod()
        {
            try
            {
                cmbDriSta.Text = "";
                txt_ManagementStrategy_Name.Text = "";
                txt_ControlInfo_Name.Text = "";
                txt_DrivewayStrategy_Name.Text = "";
                cmbDriSta.Text = "";
                txt_ManagementStrategy_Remark.Text = "";
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyForm.ClearMethod()");
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
                string sqlselectrepeat = String.Format("select  * from ManagementStrategy where ManagementStrategy_Name='{0}'", txt_ManagementStrategy_Name.Text.Trim());

                DataSet ds = LinQBaseDao.Query(sqlselectrepeat);
                if (ds.Tables[0].Rows.Count <= 0)
                {
                    rbool = false;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyForm.SelectRepeatMethod()");
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
                CommonalityEntity.WriteTextLog("ManagementStrategyForm.tscbxPageSize_SelectedIndexChanged()");
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
                CommonalityEntity.WriteTextLog("ManagementStrategyForm.bdnInfo_ItemClicked()");
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
                CommonalityEntity.WriteTextLog("ManagementStrategyForm.SelectAllMethod()");
            }
        }
        /// <summary>
        /// 显示列表绑定数据
        /// </summary>
        private void BingMethod()
        {
            try
            {

                pc.BindBoundControl(dgv_Information, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_ManagementStrategy_CarType_DrivewayStrategy_ControlInfo", "*", "ManagementStrategy_ID ", "ManagementStrategy_ID", 0, sqlwhere, true);

            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyForm.BingMethod()");
            }
        }

        #endregion
        /// <summary>
        /// 显示列表双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_Information_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                DataTable tables;
                panel2.Visible = false;
                panel1.Visible = false;
                btn_Empty.Text = "取消修改";
                btn_Preservation.Text = "修改信息";

                intDrivewayStrategyID = CommonalityEntity.GetInt(dgv_Information.SelectedRows[0].Cells["DrivewayStrategy_ID"].Value == null ? "" : dgv_Information.SelectedRows[0].Cells["DrivewayStrategy_ID"].Value.ToString());

                string strtype = dgv_Information.SelectedRows[0].Cells["ManagementStrategy_Name"].Value.ToString();
                strtype = strtype.Substring(1, strtype.IndexOf('】') - 2);
                cob_ManagementStrategy_State.Text = dgv_Information.SelectedRows[0].Cells["ManagementStrategy_State"].Value == null ? "" : dgv_Information.SelectedRows[0].Cells["ManagementStrategy_State"].Value.ToString();//管控策略状态：
                txt_ControlInfo_Name.Text = dgv_Information.SelectedRows[0].Cells["ControlInfo_Name"].Value == null ? "" : dgv_Information.SelectedRows[0].Cells["ControlInfo_Name"].Value.ToString(); //管控信息名称：
                string strsql = "";
                string str = "0";
                if (intDrivewayStrategyID == 0)
                {
                    str = LinQBaseDao.GetSingle("select ManagementStrategy_Menu_ID from  ManagementStrategy where ManagementStrategy_ID =" + dgv_Information.SelectedRows[0].Cells["ManagementStrategy_ID"].Value.ToString()).ToString();
                    strsql = "select ManagementStrategy_Type,ManagementStrategy_ControlInfo_ID from ManagementStrategy where ManagementStrategy_Menu_ID=" + str + " and ManagementStrategy_state='启动' and ManagementStrategy_DriSName = '" + dgv_Information.SelectedRows[0].Cells["ManagementStrategy_DriSName"].Value.ToString() + "'";
                }
                else
                {
                    arraylist.Add(intDrivewayStrategyID);
                    strsql = "select ManagementStrategy_Type,ManagementStrategy_ControlInfo_ID from ManagementStrategy where ManagementStrategy_DrivewayStrategy_ID=" + intDrivewayStrategyID + " and ManagementStrategy_state='启动'";
                }
                tables = LinQBaseDao.Query(strsql).Tables[0];

                string controlinfo_Names = null;
                listControlInfo_ID.Clear();
                for (int i = 0; i < tables.Rows.Count; i++)
                {
                    listControlInfo_ID.Add(Convert.ToInt32(tables.Rows[i]["ManagementStrategy_ControlInfo_ID"]));
                    if (controlinfo_Names == null)
                    {
                        controlinfo_Names = tables.Rows[i]["ManagementStrategy_Type"].ToString();
                    }
                    else
                        controlinfo_Names += "," + tables.Rows[i]["ManagementStrategy_Type"].ToString();
                }
                cmbDriSta.Text = dgv_Information.SelectedRows[0].Cells["ManagementStrategy_DriSName"].Value.ToString();
                txt_ControlInfo_Name.Text = controlinfo_Names;
                cmbSelect.SelectedIndex = Convert.ToInt32(str);
                if (str != "0")
                {
                    txt_DrivewayStrategy_Name.Visible = false;
                }
                else
                {
                    DataTable dtret = LinQBaseDao.Query("select  DrivewayStrategy_Record from DrivewayStrategy where DrivewayStrategy_ID=" + intDrivewayStrategyID).Tables[0];
                    if (dtret.Rows.Count > 0)
                    {
                        txt_DrivewayStrategy_Name.Text = dtret.Rows[0][0].ToString();
                    }
                }
                int s = cmbDriSta.Text.IndexOf("车");
                if (s > 0)
                {
                    txt_ManagementStrategy_Name.Text = cmbDriSta.Text.Substring(0, s);
                }
                else
                {
                    txt_ManagementStrategy_Name.Text = cmbDriSta.Text;
                }

                txt_ManagementStrategy_Remark.Text = dgv_Information.SelectedRows[0].Cells["ManagementStrategy_Remark"].Value == null ? "" : dgv_Information.SelectedRows[0].Cells["ManagementStrategy_Remark"].Value.ToString(); //管控策略备注：
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyForm.dgv_Information_DoubleClick()");
            }
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

                CommonalityEntity.WriteTextLog("ManagementStrategyForm.tv_ControlInfo_AfterCheck()");
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

                CommonalityEntity.WriteTextLog("ManagementStrategyForm.SetNodeCheckStatus()");
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

                CommonalityEntity.WriteTextLog("ManagementStrategyForm.SetNodeStyle()");
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

                CommonalityEntity.WriteTextLog("ManagementStrategyForm.add()");
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
                        }
                        addDiGui(tnTemp);

                    }
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("ManagementStrategyForm.addDiGui()");
            }

        }
        #endregion
        /// <summary>
        /// //确定按钮单击事件
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
                if (SetSortList.Count != listControlInfo_ID.Count)
                {
                    setSort();
                }
                panel2.Visible = false;
            }
            catch
            {

                CommonalityEntity.WriteTextLog("ManagementStrategyForm");
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

                CommonalityEntity.WriteTextLog("ManagementStrategyForm.lb_InDetail_Click()");
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

                CommonalityEntity.WriteTextLog("ManagementStrategyForm.btn_Cancel_Click");
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

                CommonalityEntity.WriteTextLog("ManagementStrategyForm.lb_DrivewayStrategy_Click()");
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
            }
            catch
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

        private void cmbCarTypeName_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_DrivewayStrategy_Name.Text = "";
            panel1.Visible = false;
            try
            {
                if (string.IsNullOrEmpty(cmbDriSta.Text))
                {
                    txt_ManagementStrategy_Name.Text = "";
                }
                else
                {
                    int i = cmbDriSta.Text.IndexOf("车") <= 0 ? 2 : cmbDriSta.Text.IndexOf("车");
                    txt_ManagementStrategy_Name.Text = cmbDriSta.Text.Substring(0, i);
                }
            }
            catch
            {

            }
        }

        private void cbxDrivewayStrategy_Name_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbxDrivewayStrategy_Name.SelectedIndex >= 1)
                {
                    string sqlCar = String.Format("select ManagementStrategy_Name,ManagementStrategy_ID from dbo.ManagementStrategy where ManagementStrategy_DriSName='{0}' and ManagementStrategy_state='启动'",cbxDrivewayStrategy_Name.Text);
                    DataTable table1 = LinQBaseDao.Query(sqlCar).Tables[0];
                    DataRow dr = table1.NewRow();
                    dr["ManagementStrategy_Name"] = "全部";
                    dr["ManagementStrategy_ID"] = "0";
                    table1.Rows.InsertAt(dr, 0);
                    cbxManagementStrategy_Name.DataSource = table1;
                    cbxManagementStrategy_Name.DisplayMember = "ManagementStrategy_Name";
                    cbxManagementStrategy_Name.ValueMember = "ManagementStrategy_ID";
                    cbxManagementStrategy_Name.SelectedIndex = -1;
                }
            }
            catch
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
            if (cbxDrivewayStrategy_Name.SelectedIndex >= 1)
            {
                sqlwhere += " and ManagementStrategy_DriSName='" +cbxDrivewayStrategy_Name.Text + "'";
            }
            if (!string.IsNullOrEmpty(cob_SelectManagementStrategy_State.Text) && cob_SelectManagementStrategy_State.Text.Trim() != "全部")
            {
                sqlwhere += " and ManagementStrategy_state='" + cob_SelectManagementStrategy_State.Text.Trim() + "'";
            }
            if (Convert.ToInt32(cbxManagementStrategy_Name.SelectedValue) > 0)
            {
                sqlwhere += " and ManagementStrategy_ID=" + Convert.ToInt32(cbxManagementStrategy_Name.SelectedValue) + "";
            }
            //pc.BindBoundControl(dgv_Information, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_ManagementStrategy_CarType_DrivewayStrategy_ControlInfo", "*", "ManagementStrategy_ID ", "ManagementStrategy_ID", 0, sqlwhere, true);
            pc.BindBoundControl(dgv_Information, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_DriManControlInfo", "*", "ManagementStrategy_ID ", "ManagementStrategy_ID", 0, sqlwhere, true);
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
                    panel2.Visible = true;
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
                        DataTable dt = LinQBaseDao.Query("select ManagementStrategy_ControlInfo_ID from ManagementStrategy m where m.ManagementStrategy_DriSName ='" + cmbDriSta.Text + "' and m.ManagementStrategy_State='启动'  and m.ManagementStrategy_DrivewayStrategy_ID in (" + strDrivewayStrategy_IDs + ")").Tables[0];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            listControlInfo_ID.Add(Convert.ToInt32(dt.Rows[i][0]));
                        }
                    }
                    else
                    {
                        DataTable dt = LinQBaseDao.Query("select ManagementStrategy_ControlInfo_ID from ManagementStrategy m where m.ManagementStrategy_DriSName ='" + cmbDriSta.Text + "' and m.ManagementStrategy_State='启动'  and m.ManagementStrategy_Menu_ID=" + cmbSelect.SelectedIndex).Tables[0];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            listControlInfo_ID.Add(Convert.ToInt32(dt.Rows[i][0]));
                        }
                    }
                    BingtreeviewMethod();
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ManagementStrategyForm.txt_ControlInfo_Name_DoubleClick()");
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
                txt_ControlInfo_Name.Text = "";
            }
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            SortYesOrNo = true;
            panel4.Visible = true;
            groupBox6.Visible = true;
            dgvSetShort.Visible = true;
            SetSortList = new List<SetSort>();
            listControlInfo_ID = new List<int>();
            ADDlistControlInfo_IDAndSetSortList();

            dgvSetShort.DataSource = SetSortList;
            dgvSetShort.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvSetShort.AutoGenerateColumns = false;
        }
        private void ADDlistControlInfo_IDAndSetSortList()
        {
            foreach (TreeNode tnTemp in tv_ControlInfo.Nodes)
            {

                if (tnTemp.Checked == true)
                {

                    if (tnTemp.Text.ToUpper().Contains("SAP"))
                    { }
                    else
                        listControlInfo_ID.Add(Convert.ToInt32(tnTemp.Name));

                }
                addDiGui(tnTemp);
            }
            if (SetSortList.Count != listControlInfo_ID.Count)
            {
                setSort();

            }

        }

        private void setSort()
        {
            SetSort s = null;

            foreach (int item in listControlInfo_ID)
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
            panel4.Visible = false;
            groupBox6.Visible = false;
            dgvSetShort.Visible = false;
        }

        private void btnSetShortNo_Click(object sender, EventArgs e)
        {
            panel4.Visible = false;
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
            panel4.Visible = false;
            groupBox6.Visible = false;
            dgvSetShort.Visible = false;
        }

        private void txt_DrivewayStrategy_Name_TextChanged(object sender, EventArgs e)
        {
            listControlInfo_ID.Clear();
            txt_ControlInfo_Name.Text = "";
        }

    }
}
