using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.HelpClass;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using System.Collections;
using EMEWE.CarManagement.Commons.CommonClass;
using System.Linq.Expressions;
using EMEWE.CarManagement.CommonClass;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class UpdateDrivewayStrategyForm : Form
    {
        DCCarManagementDataContext dc = new DCCarManagementDataContext();
        public UpdateDrivewayStrategyForm(string carname)
        {
            InitializeComponent();
            this.carname = carname;
            if (!string.IsNullOrEmpty(this.carname))
            {
                ///修改通行策略名称为车的车牌号
                dgv_Information.Columns[1].HeaderText = "车牌号";
                dgv_Information.Columns[1].DataPropertyName = "carinfo_name";
                txt_carInfo_Name.Enabled = false;
                txt_carInfo_Name.Text = this.carname;
            }
        }

        public UpdateDrivewayStrategyForm()
        { InitializeComponent(); }
        public string carname = "";
        public string DrivewayID = "";
        public string DrivewaySoutID = "";
        //public MainForm mf;

        /// <summary>
        /// 需要排序的SetSort集合
        /// </summary>
        List<SetSort> SetSortList = new List<SetSort>();
        public bool SortYesOrNo = true;
        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere = "";


        /// <summary>
        /// 分页控件行为标识（第一次："",上一页控件名称、下一页控件名称）
        /// </summary>
        private string strClickedItemName = "";


        /// <summary>
        /// 存放车辆类型编号（）
        /// </summary>
        public List<int> listCarTypeID = new List<int>();
        /// <summary>
        /// 车辆类型名称
        /// </summary>
        public string strCarTypeName = "";
        /// <summary>
        /// 存放旧的通行策略名称
        /// </summary>
        private string sriDrivewayStrategy_NameOld = "";
        /// <summary>
        ///当前要修改的通行策略ID
        /// </summary>
        private int intDrivewayStrategy_ID = 0;
        /// <summary>
        /// 当前通道管理ID
        /// </summary>
        private int intDriveway_ID = 0;
        /// <summary>
        ///存放当前通道门岗信息
        /// </summary>
        private List<View_DrivewayPosition> listDrivewayPosition = new List<View_DrivewayPosition>();
        /// <summary>
        ///  /// <summary>
        ///存放当前通道门岗信息【旧的】
        /// </summary>
        private List<View_DrivewayPosition> listDrivewayPositionaOld = new List<View_DrivewayPosition>();
        /// <summary>
        /// 存放当前选中的门岗名称、通道名称、类型、策略序号
        /// </summary>
        List<string> listDrivewayPositionName = new List<string>();
        /// <summary>
        /// 存放执行增删改的SQL语句
        /// </summary>
        ArrayList arraylist = new ArrayList();

        /// <summary>
        /// 存放当前要删除的通行策略ID
        /// </summary>
        List<int> listDrivewayStrategy_ID = new List<int>();
        /// <summary>
        /// 存放当前选中的门岗值
        /// </summary>
        List<string> listPosition_Value = new List<string>();

        /// <summary>
        /// 是否双击详细标识
        /// </summary>


        /// <summary>
        /// 存放当前选择的通道ID
        /// </summary>
        List<int> Driveway_IDList = new List<int>();


        private Point Position = new Point(0, 0);
        private PageControl pc;//分页类
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
                CommonalityEntity.WriteTextLog("DrivewayStrategyRecordForm.tscbxPageSize_SelectedIndexChanged()" + "".ToString());
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
                strClickedItemName = "";
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
                CommonalityEntity.WriteTextLog("DrivewayStrategyRecordForm.bdnInfo_ItemClicked()" + "".ToString());
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
                CommonalityEntity.WriteTextLog("DrivewayStrategyRecordForm.SelectAllMethod()" + "".ToString());
            }
        }
        /// <summary>
        /// 显示列表绑定数据
        /// </summary>
        private void BingMethod()
        {
            try
            {
                pc.BindBoundControl(dgv_Information, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_DrivewayStrategyRecord_Driveway_Position_CarInfo", "*", "DrivewayStrategyRecord_ID", "DrivewayStrategyRecord_ID", 1, sqlwhere, true);
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyRecordForm.BingMethod()" + "".ToString());
            }
        }
        /// <summary>
        /// 条件搜索方法
        /// </summary>
        private void SelectMethod()
        {
            try
            {

                sqlwhere = "1=1";

                if (!string.IsNullOrEmpty(cmbPositionID.Text) && cmbPositionID.Text != "全部")//门岗名称
                {
                    sqlwhere += String.Format(" and Position_Name like '%{0}%'", cmbPositionID.Text);
                }
                if (!string.IsNullOrEmpty(cmbDrivewayID.Text) && cmbDrivewayID.Text != "全部")//通道名称
                {
                    sqlwhere += String.Format(" and Driveway_Name like '%{0}%'", cmbDrivewayID.Text);
                }

                if (!string.IsNullOrEmpty(cob_SelectManagementStrategy_State.Text) && cob_SelectManagementStrategy_State.Text != "全部")//通行策略状态
                {
                    sqlwhere += String.Format(" and DrivewayStrategyRecord_State like '%{0}%'", cob_SelectManagementStrategy_State.Text);
                }

                if (!string.IsNullOrEmpty(txtCarName.Text))//车牌号
                {

                    sqlwhere += string.Format(" and carinfo_name like '%" + txtCarName.Text.Trim() + "%'");


                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyRecordForm.SelectMethod()" + "".ToString());
            }
            finally
            {
                BingMethod();//绑定数据
            }
        }

        #endregion

        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {

                btn_Delete.Enabled = true;
                btn_Delete.Visible = true;
                btn_Administration.Enabled = true;
                btn_Administration.Visible = true;
                btnUpdate.Enabled = true;
                btnUpdate.Visible = true;
                btnUpdate.Enabled = true;
            }
            else
            {

                btn_Delete.Enabled = ControlAttributes.BoolControl("btn_Delete", "UpdateDrivewayStrategyForm", "Enabled");
                btn_Delete.Visible = ControlAttributes.BoolControl("btn_Delete", "UpdateDrivewayStrategyForm", "Visible");

                btn_Administration.Enabled = ControlAttributes.BoolControl("btn_Administration", "UpdateDrivewayStrategyForm", "Enabled");
                btn_Administration.Visible = ControlAttributes.BoolControl("btn_Administration", "UpdateDrivewayStrategyForm", "Visible");

                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "UpdateDrivewayStrategyForm", "Enabled");
                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "UpdateDrivewayStrategyForm", "Visible");

            }
        }

        /// <summary>
        /// 状态绑定
        /// </summary>
        private void StetaBingMethod()
        {
            try
            {
                var p = DictionaryDAL.GetValueStateDictionary("01");
                int intcount = p.Count();

                if (p != null && intcount > 0)
                {
                    //cob_DrivewayStrategy_State1.DataSource = p;
                    //this.cob_DrivewayStrategy_State1.DisplayMember = "Dictionary_Name";
                    //cob_DrivewayStrategy_State1.ValueMember = "Dictionary_Value";
                    //cob_DrivewayStrategy_State1.SelectedIndex = intcount - 1;
                }

                var Pcob_DrivewayStrategy_State = p.Where(n => n.Dictionary_Name != "全部").ToList();
                if (Pcob_DrivewayStrategy_State != null && Pcob_DrivewayStrategy_State.Count() > 0)
                {
                    txt_carState.DataSource = Pcob_DrivewayStrategy_State;
                    this.txt_carState.DisplayMember = "Dictionary_Name";
                    txt_carState.ValueMember = "Dictionary_Value";
                    txt_carState.SelectedIndex = 0;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyRecordForm.StetaBingMethod()" + "".ToString());
            }

        }
        /// <summary>
        /// 清空控件方法
        /// </summary>
        private void ClearMethod()
        {
            try
            {

                //list.Clear();
                txt_carInfo_Name.ReadOnly = false;
                //txt_DrivewayStrategy_Sort.Text = "";
                //cob_DrivewayStrategy_State.Text = "";
                txt_Driveway_Name.Text = "";
                txt_DrivewayStrategyRecord_Remark.Text = "";
                txt_DrivewayStrategyRecord_OffReason.Text = "";


            }
            catch
            {

                CommonalityEntity.WriteTextLog("DrivewayStrategyRecordForm.ClearMethod()" + "".ToString());
            }
        }

        private void DrivewayStrategyForm_Load(object sender, EventArgs e)
        {

            if (CommonalityEntity.boolCopyDrivewayStrategy)
            {
                userContext();
                Initialization();
            }
            else
            {
                userContext();
                Initialization();
                DataTable dts = LinQBaseDao.Query("select CarInOutRecord_Update,CarInOutRecord_Remark, CarInfo_State from view_carstate where CarInfo_ID=" + CommonalityEntity.CarInfo_ID).Tables[0];

                if (dts.Rows.Count > 0)
                {
                    txtDrivewayStrategy.Text = dts.Rows[0]["CarInOutRecord_Remark"].ToString();
                    txt_carState.Text = dts.Rows[0]["CarInfo_State"].ToString();

                    if (!Convert.ToBoolean(dts.Rows[0]["CarInOutRecord_Update"].ToString()))
                    {
                        DataTable table = LinQBaseDao.Query(" SELECT * FROM Driveway INNER JOIN Position ON Driveway.Driveway_Position_ID = Position.Position_ID INNER JOIN DrivewayStrategy ON Driveway.Driveway_ID = DrivewayStrategy.DrivewayStrategy_Driveway_ID where DrivewayStrategy_Name='" + dts.Rows[0]["CarInOutRecord_Remark"].ToString() + "' and DrivewayStrategy_State='启动' order by DrivewayStrategy_Sort").Tables[0];
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            txt_Driveway_Name.Text += "【" + table.Rows[i]["Position_Name"] + "】" + table.Rows[i]["Driveway_Name"] + "【" + table.Rows[i]["Driveway_type"] + "】 ；";
                            txt_DrivewayStrategyRecord_Remark.Text = table.Rows[i]["DrivewayStrategy_Remark"].ToString();
                            txt_DrivewayStrategyRecord_OffReason.Text = table.Rows[i]["DrivewayStrategy_Reason"].ToString();
                            Driveway_IDList.Add(Convert.ToInt32(table.Rows[i]["Driveway_ID"]));
                        }
                    }
                    else
                    {
                        DataTable table = LinQBaseDao.Query(" SELECT * FROM Driveway INNER JOIN Position ON Driveway.Driveway_Position_ID = Position.Position_ID INNER JOIN DrivewayStrategyRecord ON Driveway.Driveway_ID = DrivewayStrategyRecord.DrivewayStrategyRecord_Driveway_ID where DrivewayStrategyRecord_CarInfo_ID=" + CommonalityEntity.CarInfo_ID + " and DrivewayStrategyRecord_State='启动' order by DrivewayStrategyRecord_Sort").Tables[0];
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            txt_Driveway_Name.Text += "【" + table.Rows[i]["Position_Name"] + "】" + table.Rows[i]["Driveway_Name"] + "【" + table.Rows[i]["Driveway_type"] + "】 ；";
                            txt_DrivewayStrategyRecord_Remark.Text = table.Rows[i]["DrivewayStrategyRecord_Remark"].ToString();
                            txt_DrivewayStrategyRecord_OffReason.Text = table.Rows[i]["DrivewayStrategyRecord_OffReason"].ToString();
                            Driveway_IDList.Add(Convert.ToInt32(table.Rows[i]["Driveway_ID"]));
                        }
                    }

                    BingMethod();
                    //mf = new MainForm();
                }
            }

        }


        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialization()
        {

            try
            {
                sqlwhere = "";


                DataTable dt = LinQBaseDao.Query("select Position_name,Position_ID from Position where Position_state='启动'").Tables[0];
                DataRow dr = dt.NewRow();
                dr["Position_ID"] = "0";
                dr["Position_name"] = "全部";
                dt.Rows.InsertAt(dr, 0);
                cmbPositionID.DataSource = dt;
                cmbPositionID.DisplayMember = "Position_name";
                cmbPositionID.ValueMember = "Position_ID";
                pc = new PageControl();
                if (!CommonalityEntity.boolCopyDrivewayStrategy)
                {
                    if (carname == "")
                    {
                        return;
                    }

                }
                panel2.Visible = false;
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyRecordForm.Initialization()" + "".ToString());
            }


        }

        /// <summary>
        /// 双击通道名称文本框事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_Driveway_Name_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (btn_Empty.Text != "清  空") return;
                string sqlCarTypeAttribute = "";
                if (panel2.Visible)
                {
                    panel2.Visible = false;
                }
                else
                {

                    trVDrivewayStrategy_Name.Nodes.Clear();
                    panel2.Visible = true;
                    sqlCarTypeAttribute = String.Format("select Position_Name,Driveway_Name,Driveway_Type,Driveway_State,Position_State,Driveway_ID from View_DrivewayPosition where driveway_warrantystate='正常' and driveway_state='启动'  order by Position_Name asc, Driveway_Value asc");
                    DataTable table1 = LinQBaseDao.Query(sqlCarTypeAttribute).Tables[0];
                    TreeNode tr1;
                    for (int i = 0; i < table1.Rows.Count; i++)
                    {
                        tr1 = new TreeNode();

                        tr1.Tag = table1.Rows[i]["Driveway_ID"];
                        tr1.Text = "【" + table1.Rows[i]["Position_Name"].ToString() + "】" + " " + table1.Rows[i]["Driveway_Name"].ToString() + " " + "【" + table1.Rows[i]["Driveway_Type"].ToString() + "】";
                        tr1.Checked = Driveway_IDList.Contains(Convert.ToInt32(table1.Rows[i]["Driveway_ID"])) == true ? true : false;
                        trVDrivewayStrategy_Name.Nodes.Add(tr1);
                    }

                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyRecordForm.txt_Driveway_Name_DoubleClick()" + "".ToString());
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
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_NotSelectAll_Click(object sender, EventArgs e)
        {
            SelectAll(false);
        }

        /// <summary>
        /// 全选
        /// </summary>
        private void SelectAll(bool chrbool)
        {
            foreach (TreeNode tnTemp in trVDrivewayStrategy_Name.Nodes)
            {
                if (tnTemp.Tag != null)
                {

                    tnTemp.Checked = chrbool;
                    tnTemp.ExpandAll();//展开所有子节点
                    SelectAllDiGui(tnTemp, chrbool);

                }
            }
        }

        private void SelectAllDiGui(TreeNode tn, bool chrbool)
        {
            foreach (TreeNode tnTemp in tn.Nodes)
            {
                if (tnTemp.Tag != null)
                {
                    tnTemp.Checked = chrbool;
                    SelectAllDiGui(tnTemp, chrbool);
                    tnTemp.ExpandAll();//展开所有子节点
                }
            }
        }

        /// <summary>
        /// 存放选择后的通道信息字符串
        /// </summary>
        private string strDriveway_Name = "";
        /// <summary>
        /// 查出选中的父级ID
        /// </summary>
        private void adds()
        {
            Driveway_IDList.Clear();
            if (trVDrivewayStrategy_Name != null)
            {
                foreach (TreeNode tnTemp in trVDrivewayStrategy_Name.Nodes)
                {
                    if (tnTemp.Checked == true)
                    {
                        Driveway_IDList.Add(Convert.ToInt32(tnTemp.Tag));
                        if (strDriveway_Name == null)
                        {
                            strDriveway_Name = tnTemp.Text.Trim().ToString();
                        }
                        else
                            strDriveway_Name += "；" + tnTemp.Text.Trim().ToString();
                    }
                    addDiGui(tnTemp);
                }
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
                            strDriveway_Name += tnTemp.Text + ",";
                            Driveway_IDList.Add(CommonalityEntity.GetInt(tnTemp.Name.ToString()));//存放入子级实际值
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

        /// <summary>
        /// 确认双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Confirmation_Click(object sender, EventArgs e)
        {
            try
            {
                strDriveway_Name = null;

                panel2.Visible = false;
                if (SetSortList.Count != Driveway_IDList.Count)
                {
                    setSort();
                }

                txt_Driveway_Name.Text = "";
                foreach (SetSort item in SetSortList)
                {
                    txt_Driveway_Name.Text += item.text + "；";
                }
            }
            catch (Exception)
            {

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
                CommonalityEntity.WriteTextLog("DrivewayStrategyRecordForm.btn_Cancel_Click()" + "".ToString());
            }
        }

        /// <summary>
        /// 管控策略按钮双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Administration_Click(object sender, EventArgs e)
        {

            try
            {
                UpdateManagementStrategyForm msf = new UpdateManagementStrategyForm();
                PublicClass.ShowChildForm(msf);
                //mf = new MainForm();
                //mf.ShowChildForm(msf, this);
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.btn_Administration_Click()" + "".ToString());
            }


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
        /// 通道管理双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Driveway_Click(object sender, EventArgs e)
        {
            DrivewayFrom df = new DrivewayFrom();
            PublicClass.ShowChildForm(df);
            //mf.ShowChildForm(df, this);
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
        /// <summary>
        /// 搜索双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Select_Click(object sender, EventArgs e)
        {
            try
            {
                if (btn_Select.Enabled)
                {
                    btn_Select.Enabled = false;
                    SelectMethod();
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyRecordForm.btn_Select_Click()" + "".ToString());
            }
            finally
            {
                btn_Select.Enabled = true;
            }
        }



        /// <summary>
        /// 根据车辆类型查询通道编号
        /// </summary>
        /// <returns></returns>
        private void SetDrivewayStrategy_Driveway_IDMethod(List<int> ListintCarTypeID)
        {
            try
            {
                listDrivewayPosition = new List<View_DrivewayPosition>();
                listDrivewayPositionaOld = new List<View_DrivewayPosition>();
                string strsql = String.Format("select DrivewayStrategyRecord_Driveway_ID,DrivewayStrategyRecord_CarType_ID from DrivewayStrategyRecord");
                var p = LinQBaseDao.Query(strsql).Tables[0];
                if (p.Rows.Count > 0)
                {
                    var pDrivewayStrategy_Driveway_ID = p.AsEnumerable().Where(n => ListintCarTypeID.Contains(n.Field<int>("DrivewayStrategyRecord_CarType_ID"))).Select(n => n.Field<int>("DrivewayStrategy_Driveway_ID"));
                    if (pDrivewayStrategy_Driveway_ID.Count() > 0)
                    {
                        foreach (var tem in pDrivewayStrategy_Driveway_ID.Distinct())
                        {
                            if (tem > 0)
                            {
                                View_DrivewayPosition vdp = new View_DrivewayPosition();
                                vdp.Driveway_ID = tem;
                                listDrivewayPosition.Add(vdp);
                            }
                        }
                    }
                }
                listDrivewayPositionaOld = listDrivewayPosition;
            }
            catch
            {

                CommonalityEntity.WriteTextLog("DrivewayStrategyRecordForm.dgv_Information_DoubleClick()" + "".ToString());
            }

        }



        /// <summary>
        /// 删除双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                string strmessagebox = "";
                listDrivewayStrategy_ID.Clear();
                arraylist.Clear();
                string strlog = "";
                string strDrivewayStrategy_name = "";
                if (btn_Delete.Enabled)
                {
                    btn_Delete.Enabled = false;
                }
                if (dgv_Information.SelectedRows.Count > 0)//判断是否选中行
                {
                    if (MessageBox.Show("删除，将会把所有关联删除，确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {

                        int count = dgv_Information.SelectedRows.Count;
                        //遍历
                        for (int i = 0; i < count; i++)
                        {
                            if (dgv_Information.SelectedRows[i].Cells["DrivewayStrategyRecord_ID"].Value != null)//通行策略ID
                            {
                                listDrivewayStrategy_ID.Add(CommonalityEntity.GetInt(dgv_Information.SelectedRows[i].Cells["DrivewayStrategyRecord_ID"].Value.ToString()));
                                if (dgv_Information.SelectedRows[i].Cells["DrivewayStrategyRecord_Name"].Value != null)//通行策略名称
                                {
                                    strDrivewayStrategy_name += dgv_Information.SelectedRows[i].Cells["DrivewayStrategyRecord_Name"].Value.ToString() + " ";
                                }
                            }
                        }
                        strlog = String.Format("删除通行策略:'{0}'", strDrivewayStrategy_name);

                        Expression<Func<DrivewayStrategyRecord, bool>> funDrivewayStrategy = n => listDrivewayStrategy_ID.Contains(n.DrivewayStrategyRecord_ID);
                        if (DrivewayStrategyRecordDAL.DeleteToMany(funDrivewayStrategy))
                        {
                            strmessagebox = "成功删除通行策略";
                            CommonalityEntity.WriteLogData("删除", strlog, CommonalityEntity.USERNAME);
                        }
                        else
                        {
                            strmessagebox = "通行策略删除失败";
                        }

                    }
                }
                else
                {
                    strmessagebox = "请选择要删除的行";
                }
                if (strmessagebox != "")
                {
                    MessageBox.Show(strmessagebox, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyRecordForm.btn_Delete_Click()" + "".ToString());
            }
            finally
            {
                btn_Delete.Enabled = true;
                pc = new PageControl();
                SelectMethod();//更新数据
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
                CommonalityEntity.WriteTextLog("DrivewayStrategyRecordForm.btn_Empty_Click()" + "".ToString());
            }
            finally
            {
                btn_Empty.Enabled = true;
                btn_Empty.Text = "清  空";
                listCarTypeID = new List<int>();
                arraylist = new ArrayList();
                Driveway_IDList.Clear();
            }
        }
        /// <summary>
        /// 设置通行策略顺序号文本框只能输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_DrivewayStrategy_Sort_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void dgv_Information_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            try
            {
                using (SolidBrush b = new SolidBrush(dgv_Information.RowHeadersDefaultCellStyle.ForeColor))
                {
                    e.Graphics.DrawString(Convert.ToString(e.RowIndex + 1, System.Globalization.CultureInfo.CurrentUICulture), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("DrivewayStrategyRecordForm.dgv_Information_RowPostPaint()" + "".ToString());
            }

        }

        private void btnSort_Click(object sender, EventArgs e)
        {


            panel1.Visible = true;
            groupBox6.Visible = true;
            dgvSetShort.Visible = true;
            Driveway_IDList = new List<int>();
            SetSortList = new List<SetSort>();
            Driveway_IDListADD();

            dgvSetShort.DataSource = SetSortList;
            dgvSetShort.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvSetShort.AutoGenerateColumns = false;
        }

        private void Driveway_IDListADD()
        {
            foreach (TreeNode tnTemp in trVDrivewayStrategy_Name.Nodes)
            {
                if (tnTemp.Checked == true)
                {
                    Driveway_IDList.Add(Convert.ToInt32(tnTemp.Tag));
                    if (strDriveway_Name == null)
                    {
                        strDriveway_Name = tnTemp.Text.Trim().ToString();
                    }
                    else
                        strDriveway_Name += "；" + tnTemp.Text.Trim().ToString();
                }
                addDiGui(tnTemp);
            }

            setSort();
        }

        private void setSort()
        {
            SetSort s = null;

            foreach (int item in Driveway_IDList)
            {

                string DrivewaySql = "select Position_Name,Driveway_Name,Driveway_Type,Driveway_State,Position_State,Driveway_ID from View_DrivewayPosition where Driveway_ID=" + item + "  order by Position_Name asc, Driveway_Name asc";
                DataTable tab = LinQBaseDao.Query(DrivewaySql).Tables[0];
                if (tab != null)
                {
                    s = new SetSort();
                    s.text = "【" + tab.Rows[0]["Position_Name"].ToString() + "】" + " " + tab.Rows[0]["Driveway_Name"].ToString();
                    s.sort = SetSortList.Count + 1;
                    s.id = Convert.ToInt32(tab.Rows[0]["Driveway_ID"]);
                    SetSortList.Add(s);

                }

            }
        }

        private void DrivewayInformation_Click(object sender, EventArgs e)
        {
            CarStrategyRecord carSR = new CarStrategyRecord();
            PublicClass.ShowChildForm(carSR);
            //mf.ShowChildForm(carSR, this);
        }

        private void trVDrivewayStrategy_Name_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode myNode = null;
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                myNode = (TreeNode)(e.Data.GetData(typeof(TreeNode)));
            }
            else
            {
                MessageBox.Show("错误操作！");
            }
            Position.X = e.X;
            Position.Y = e.Y;
            Position = trVDrivewayStrategy_Name.PointToClient(Position);
            TreeNode DropNode = this.trVDrivewayStrategy_Name.GetNodeAt(Position);
            // 1.目标节点不是空。2.目标节点不是被拖拽接点的字节点。3.目标节点不是被拖拽节点本身
            if (DropNode != null && DropNode.Parent != myNode && DropNode != myNode)
            {
                TreeNode DragNode = myNode;
                // 将被拖拽节点从原来位置删除。
                myNode.Remove();
                // 在目标节点下增加被拖拽节点
                DropNode.Nodes.Add(DragNode);
                //LinQBaseDao.Query("update MenuInfo set menu_otherid='" + Convert.ToInt32(DropNode.Tag) + "' where menu_id='" + Convert.ToInt32(myNode.Tag) + "'");
            }
            // 如果目标节点不存在，即拖拽的位置不存在节点，那么就将被拖拽节点放在根节点之下
            if (DropNode == null)
            {
                TreeNode DragNode = myNode;
                myNode.Remove();
                trVDrivewayStrategy_Name.Nodes.Add(DragNode);
                //LinQBaseDao.Query("update MenuInfo set menu_otherid=0 where menu_id='" + Convert.ToInt32(myNode.Tag) + "'");

            }
        }

        private void trVDrivewayStrategy_Name_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void trVDrivewayStrategy_Name_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }
        #region --选中父级菜单该菜单下的所有子级菜单自动选中--
        private void trVDrivewayStrategy_Name_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                //e.Node.ForeColor = Color.Yellow;
                SetNodeCheckStatus(e.Node, e.Node.Checked);
                SetNodeStyle(e.Node);
            }
        }
        private void SetNodeCheckStatus(TreeNode tn, bool Checked)
        {
            if (tn == null) return;
            foreach (TreeNode tnChild in tn.Nodes)
            {
                tnChild.Checked = Checked;
                SetNodeCheckStatus(tnChild, Checked);
            }
            TreeNode tnParent = tn;
        }

        private void SetNodeStyle(TreeNode Node)
        {
            int nNodeCount = 0;
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
                SetNodeStyle(Node.Parent);
        }
        #endregion


        private void cmbPositionID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(cmbPositionID.SelectedValue) >= 1)
                {
                    DataTable dt = LinQBaseDao.Query("select Driveway_name,Driveway_ID from Driveway where Driveway_state='启动' and Driveway_Position_ID=" + Convert.ToInt32(cmbPositionID.SelectedValue) + "").Tables[0];
                    DataRow dr = dt.NewRow();
                    dr["Driveway_ID"] = "-1";
                    dr["Driveway_name"] = "全部";
                    dt.Rows.InsertAt(dr, 0);
                    cmbDrivewayID.DataSource = dt;
                    cmbDrivewayID.DisplayMember = "Driveway_name";
                    cmbDrivewayID.ValueMember = "Driveway_ID";
                    cmbDrivewayID.SelectedValue = -1;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 保存，如果有同样车牌号的通行策略则把之前的状态注销
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            IList<DrivewayStrategyRecord> iedsr = null;
            try
            {
                if (string.IsNullOrEmpty(txt_carState.Text))
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "请选择状态！", txt_carState, this);
                    return;
                }
                string strsort = "";
                if (SortYesOrNo)
                {
                    strsort = "有序";
                }
                else
                {
                    strsort = "无序";
                }
                if (SetSortList.Count != Driveway_IDList.Count)
                {
                    SortYesOrNo = false;
                }
                else
                {
                    SortYesOrNo = true;
                }
                //得到车辆对象
                Expression<Func<CarInfo, bool>> fn = n => n.CarInfo_ID == Convert.ToInt32(CommonalityEntity.CarInfo_ID);
                CarInfo car = CarInfoDAL.Single(fn);


                LinQBaseDao.Query("delete ManagementStrategyRecord where ManagementStrategyRecord_CarInfo_ID=" + car.CarInfo_ID);
                LinQBaseDao.Query("delete DrivewayStrategyRecord where DrivewayStrategyRecord_CarInfo_ID=" + car.CarInfo_ID);

                //循环选择的通道
                iedsr = new List<DrivewayStrategyRecord>();

                if (SetSortList.Count == Driveway_IDList.Count)
                {
                    for (int i = 0; i < SetSortList.Count; i++)
                    {
                        DataTable tablesDrisn = LinQBaseDao.Query("select Position_Name,Driveway_Name,Driveway_type from View_DrivewayPosition where driveway_warrantystate='正常' and driveway_state='启动' and Driveway_ID=" + SetSortList[i].id + "  order by Position_Name asc, Driveway_Name asc").Tables[0];

                        DrivewayStrategyRecord Newdsr = new DrivewayStrategyRecord();
                        Newdsr.DrivewayStrategyRecord_Name = txtDrivewayStrategy.Text.Trim();
                        Newdsr.DrivewayStrategyRecord_Driveway_ID = SetSortList[i].id;//通道编号
                        Newdsr.DrivewayStrategyRecord_State = txt_carState.Text;
                        Newdsr.DrivewayStrategyRecord_Remark = txt_DrivewayStrategyRecord_Remark.Text.Trim();
                        Newdsr.DrivewayStrategyRecord_OffReason = txt_DrivewayStrategyRecord_OffReason.Text.Trim();
                        Newdsr.DrivewayStrategyRecord_OffTime = CommonalityEntity.GetServersTime();
                        Newdsr.DrivewayStrategyRecord_OffName = HelpClass.common.USERNAME;
                        Newdsr.DrivewayStrategyRecord_CarInfo_ID = car.CarInfo_ID;
                        Newdsr.DrivewayStrategyRecord_Sort = SortYesOrNo == true ? SetSortList[i].sort : 1;
                        Newdsr.DrivewayStrategyRecord_Record = "【" + txt_carInfo_Name.Text.ToString().Trim() + "】 " + tablesDrisn.Rows[0]["Position_Name"] + tablesDrisn.Rows[0]["Driveway_Name"];
                        iedsr.Add(Newdsr);
                    }

                    try
                    {
                        dc.DrivewayStrategyRecord.InsertAllOnSubmit(iedsr);
                        dc.SubmitChanges();



                        DialogResult dr = MessageBox.Show("操作已完成！请继续修改相应的管控策略否则修改无效", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                        if (dr == DialogResult.Yes)
                        {
                            string sids = "";

                            DataTable dt = LinQBaseDao.Query("select DrivewayStrategyRecord_ID, DrivewayStrategyRecord_Driveway_ID from DrivewayStrategyRecord where DrivewayStrategyRecord_CarInfo_ID=" + car.CarInfo_ID + " and DrivewayStrategyRecord_State='启动' order by DrivewayStrategyRecord_Sort ").Tables[0];
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                sids += dt.Rows[i][0].ToString() + ",";
                            }
                            int driid = Convert.ToInt32(dt.Rows[0][1].ToString());
                            sids = sids.TrimEnd(',');
                            string SmallID = LinQBaseDao.GetSingle("select SmallTicket_ID from SmallTicket where SmallTicket_CarInfo_ID=" + car.CarInfo_ID).ToString();

                            DataTable dtpd = LinQBaseDao.Query("select Position_ID, Position_Value,Driveway_Value from View_FVN_Driveway_Position where Driveway_ID=" + driid).Tables[0];
                            if (dtpd.Rows.Count > 0)
                            {
                                string str = "update CarInOutRecord set CarInOutRecord_DrivewayStrategyS='" + sids + "',CarInOutRecord_Update=1,CarInOutRecord_Driveway_ID=" + driid + ",CarInOutRecord_Remark='" + txtDrivewayStrategy.Text.Trim() + "',CarInOutRecord_Sort='" + strsort + "' where CarInOutRecord_CarInfo_ID=" + car.CarInfo_ID;
                                str += " update SortNumberInfo set  SortNumberInfo_PositionValue='" + dtpd.Rows[0][1].ToString() + "',SortNumberInfo_DrivewayValue='" + dtpd.Rows[0][2].ToString() + "' where  SortNumberInfo_SmallTicket_ID in (select SmallTicket_ID from SmallTicket where SmallTicket_CarInfo_ID=" + car.CarInfo_ID + ")";
                                LinQBaseDao.Query(str);

                                sids = "";
                                CommonalityEntity.boolCepyManagementStrategy = false;
                                UpdateManagementStrategyForm up = new UpdateManagementStrategyForm(carname);
                                up.Show();
                            }
                        }
                        BingMethod();

                    }
                    catch
                    {
                        MessageBox.Show("操作失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyRecordForm.btnUpdate_Click()" + "".ToString());
            }
            finally
            {
                SetSortList.Clear();
                Driveway_IDList.Clear();
            }
        }

        private void btnSetShortNo_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            groupBox6.Visible = false;
            dgvSetShort.Visible = false;
            SetSortList.Clear();
        }

        private void btnUp_Click_1(object sender, EventArgs e)
        {
            upDown("up");
        }

        private void btnDown_Click_1(object sender, EventArgs e)
        {
            upDown("down");
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

        private void btnSetShortOk_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            groupBox6.Visible = false;
            dgvSetShort.Visible = false;
        }

        private void btnNoSort_Click(object sender, EventArgs e)
        {
            SortYesOrNo = false;
        }

        private void UpdateDrivewayStrategyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CommonalityEntity.boolCopyDrivewayStrategy = true;
        }

    }

}
