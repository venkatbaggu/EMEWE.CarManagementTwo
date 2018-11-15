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
    public partial class DrivewayStrategyForm : Form
    {

        public DrivewayStrategyForm()
        { InitializeComponent(); }
        public string strnames = "";
        public string DrivewayID = "";
        //public MainForm mf;

        /// <summary>
        /// 需要排序的SetSort集合
        /// </summary>
        List<SetSort> SetSortList = new List<SetSort>();
        public bool sortYesOrNo = true;//是否设置顺序


        /// <summary>
        /// 修改数量，是针对一条策略还是一个类型
        /// </summary>
        public bool isUpdateOne = false;
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
        /// 当前要修改的门岗值
        /// </summary>
        private string strPosition_Value = "";
        /// <summary>
        /// 是否双击详细标识
        /// </summary>


        /// <summary>
        /// 存放当前选择的通道ID
        /// </summary>
        List<int> Driveway_IDList = new List<int>();

        /// <summary>
        /// 修改前通行策略名称
        /// </summary>
        string drivewaystrategy_name = "";

        private int intInDetail = 0;
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
                CommonalityEntity.WriteTextLog

("DrivewayStrategyForm.tscbxPageSize_SelectedIndexChanged()"
);
            }
        }


        /// <summary>
        /// 分页控件响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdnInfo_ItemClicked(object sender,

ToolStripItemClickedEventArgs e)
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
                CommonalityEntity.WriteTextLog

("DrivewayStrategyForm.bdnInfo_ItemClicked()");
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
                CommonalityEntity.WriteTextLog("DrivewayStrategyForm.SelectAllMethod()");
            }
        }
        /// <summary>
        /// 显示列表绑定数据
        /// </summary>
        private void BingMethod()
        {
            try
            {
                pc.BindBoundControl(dgv_Information, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_DrivewayStrategy_Driveway_Position", "*", "DrivewayStrategy_ID", "DrivewayStrategy_Name", 1, sqlwhere, true);
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyForm.BingMethod()");
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

                if (!string.IsNullOrEmpty(txtDrivewayStrategy_Name.Text.Trim()))//通行策略名称
                {
                    sqlwhere += " and DrivewayStrategy_Name  like  '%" + txtDrivewayStrategy_Name.Text.Trim() + "%'";
                }
                if (cmbPositionID.SelectedIndex > 0)//门岗名称
                {
                    sqlwhere += String.Format(" and Position_Name = '{0}'", cmbPositionID.Text);
                }
                if (cmbDrivewayID.SelectedIndex > 0)//通道名称
                {
                    sqlwhere += String.Format(" and Driveway_Name = '{0}'", cmbDrivewayID.Text);
                }
                if (!string.IsNullOrEmpty(cob_SelectManagementStrategy_State.Text) && cob_SelectManagementStrategy_State.Text != "全部")//通行策略状态
                {
                    sqlwhere += " and DrivewayStrategy_State = '" + cob_SelectManagementStrategy_State.Text + "'";
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyForm.SelectMethod()");
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

                btn_Preservation.Enabled = true;
                btn_Preservation.Visible = true;
                btn_Delete.Enabled = true;
                btn_Delete.Visible = true;
                btn_Administration.Enabled = true;
                btn_Administration.Visible = true;
                btn_CarType.Enabled = true;
                btn_CarType.Visible = true;
                btn_Driveway.Enabled = true;
                btn_Driveway.Visible = true;
                btn_BusinessType.Enabled = true;
                btn_BusinessType.Visible = true;
            }
            else
            {
                btn_Preservation.Visible = ControlAttributes.BoolControl("btn_Preservation", "DrivewayStrategyForm", "Visible");
                btn_Preservation.Enabled = ControlAttributes.BoolControl("btn_Preservation", "DrivewayStrategyForm", "Enabled");

                btn_Delete.Enabled = ControlAttributes.BoolControl("btn_Delete", "DrivewayStrategyForm", "Enabled");
                btn_Delete.Visible = ControlAttributes.BoolControl("btn_Delete", "DrivewayStrategyForm", "Visible");

                btn_Administration.Enabled = ControlAttributes.BoolControl("btn_Administration", "DrivewayStrategyForm", "Enabled");
                btn_Administration.Visible = ControlAttributes.BoolControl("btn_Administration", "DrivewayStrategyForm", "Visible");

                btn_CarType.Enabled = ControlAttributes.BoolControl("btn_CarType", "DrivewayStrategyForm", "Enabled");
                btn_CarType.Visible = ControlAttributes.BoolControl("btn_CarType", "DrivewayStrategyForm", "Visible");

                btn_Driveway.Enabled = ControlAttributes.BoolControl("btn_Driveway", "DrivewayStrategyForm", "Enabled");
                btn_Driveway.Visible = ControlAttributes.BoolControl("btn_Driveway", "DrivewayStrategyForm", "Visible");

                btn_BusinessType.Enabled = ControlAttributes.BoolControl("btn_BusinessType", "DrivewayStrategyForm", "Enabled");
                btn_BusinessType.Visible = ControlAttributes.BoolControl("btn_BusinessType", "DrivewayStrategyForm", "Visible");
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
                var Pcob_DrivewayStrategy_State = p.Where(n => n.Dictionary_Name != "全部").ToList();
                if (Pcob_DrivewayStrategy_State != null && Pcob_DrivewayStrategy_State.Count() > 0)
                {
                    cob_DrivewayStrategy_State.DataSource = Pcob_DrivewayStrategy_State;
                    this.cob_DrivewayStrategy_State.DisplayMember = "Dictionary_Name";
                    cob_DrivewayStrategy_State.ValueMember = "Dictionary_Value";
                    cob_DrivewayStrategy_State.SelectedIndex = 0;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyForm.StetaBingMethod()");
            }

        }
        /// <summary>
        /// 清空控件方法
        /// </summary>
        private void ClearMethod()
        {
            try
            {
                btn_Preservation.Enabled = true;
                btnUpdate.Enabled = false;
                txt_DrivewayStrategy_Name.Text = "";

                txt_Driveway_Name.Text = "";
                txt_DrivewayStrategy_Reason.Text = "";
                txt_DrivewayStrategy_Remark.Text = "";
            }
            catch
            {

                CommonalityEntity.WriteTextLog("DrivewayStrategyForm.ClearMethod()");
            }
        }

        private void DrivewayStrategyForm_Load(object sender, EventArgs e)
        {
            userContext();
            Initialization();
            btn_Preservation.Enabled = true;
            btnUpdate.Enabled = false;
            //mf = new MainForm();
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
                cmbPositionID.SelectedIndex = -1;


                cob_DrivewayStrategy_State.SelectedIndex = 0;
                pc = new PageControl();
                if (!CommonalityEntity.boolCopyDrivewayStrategy)
                {
                    if (strnames == "")
                    {
                        return;
                    }
                    if (CommonalityEntity.IsValidIDs(strnames))
                    {
                        strnames = CommonalityEntity.FormatSQLIDs(strnames);
                        string[] arryIds = CommonalityEntity.GetArryIDs(strnames);

                        for (int i = 0; i < arryIds.Length; i++)
                        {
                            if (sqlwhere == "")
                            {
                                sqlwhere = "carinfo_name='" + arryIds[i].ToString() + "'";
                            }
                            else
                                sqlwhere += "or carinfo_name='" + arryIds[i].ToString() + "'";
                        }

                        BingMethod();

                    }

                }
                else
                {
                    if (sqlwhere == "")
                    {
                        sqlwhere = "1=1";
                        BingMethod();
                    }

                }
                panel2.Visible = false;
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyForm.Initialization()");
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
                CommonalityEntity.WriteTextLog("DrivewayStrategyForm.txt_Driveway_Name_DoubleClick()");
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
                            Driveway_IDList.Add(CommonalityEntity.GetInt

(tnTemp.Name.ToString()));//存放入子级实际值
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

        /// <summary>
        /// 确认双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Confirmation_Click(object sender, EventArgs e)
        {
            try
            {
                Driveway_IDList.Clear();//清空动态数组内的成员
                strDriveway_Name = null;
                adds();
                txt_Driveway_Name.Text = strDriveway_Name;
                panel2.Visible = false;
                if (Driveway_IDList.Count != SetSortList.Count)
                {
                    setSort();
                }
            }
            catch
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
                CommonalityEntity.WriteTextLog("DrivewayStrategyForm.btn_Cancel_Click()");
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
                ManagementStrategyForm msf = new ManagementStrategyForm(null);
                PublicClass.ShowChildForm(msf);
                //mf.ShowChildForm(msf, this);
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarTypeForm.btn_Administration_Click()");
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
                CommonalityEntity.WriteTextLog

("DrivewayStrategyForm.btn_Select_Click()");
            }
            finally
            {
                btn_Select.Enabled = true;
            }
        }

        /// <summary>
        /// 保存按钮单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Preservation_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cob_DrivewayStrategy_State.Text))
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "请选择状态！", cob_DrivewayStrategy_State, this);
                    return;
                }
                //if (SetSortList.Count != Driveway_IDList.Count)
                //{
                //    sortYesOrNo = false;
                //}
                //else
                //{
                //    sortYesOrNo = true;
                //}
                ///添加和修改策略
                ///1.通行车辆修改，判断数据是否存在；如果存在直接修改，不存在添加数据
                ///2.通行策略不能删除，不用的只能注销或者暂停
                ///3.通行策略和管控策略对应到每个通道上去
                ///3.通行策略修改后，相应的管控也要修改；如果通行策略被注销或者暂停了，管控就暂停
                ///4.添加了通行策略后，必须添加相应的管控策略。

                //判断车辆类型是否选择了
                #region --添加数据--

                string drisname = txt_DrivewayStrategy_Name.Text.Trim();
                //循环选择的通道
                DataTable dtw = LinQBaseDao.Query("select * from  DrivewayStrategy  where  DrivewayStrategy_Name='" + drisname + "'").Tables[0];
                if (dtw.Rows.Count > 0)
                {
                    MessageBox.Show(this, "通行策略名称已存在");
                    return;
                }

                for (int i = 0; i < SetSortList.Count; i++)
                {
                    DataTable tablesDris = LinQBaseDao.Query("select Position_Name,Driveway_Name,Driveway_type from View_DrivewayPosition where driveway_warrantystate='正常' and driveway_state='启动' and Driveway_ID=" + SetSortList[i].id + "  order by Position_Name asc, Driveway_Name asc").Tables[0];
                    if (tablesDris.Rows.Count > 0)
                    {
                        DrivewayStrategy ds = new DrivewayStrategy();
                        ds.DrivewayStrategy_Name = drisname;
                        ds.DrivewayStrategy_Driveway_ID = SetSortList[i].id;//通道编号
                        ds.DrivewayStrategy_State = cob_DrivewayStrategy_State.Text;
                        ds.DrivewayStrategy_Record = tablesDris.Rows[0]["Position_Name"].ToString() + tablesDris.Rows[0]["Driveway_Name"].ToString() + tablesDris.Rows[0]["Driveway_type"].ToString();
                        //判断是否排序
                        if (sortYesOrNo)
                        {
                            ds.DrivewayStrategy_Sort = SetSortList[i].sort;
                        }
                        else
                        {
                            ds.DrivewayStrategy_Sort = 1;
                        }
                        ds.DrivewayStrategy_Reason = txt_DrivewayStrategy_Reason.Text.Trim();
                        ds.DrivewayStrategy_Remark = txt_DrivewayStrategy_Remark.Text.Trim();
                        DrivewayStrategyDAL.InsertOneQCRecord(ds);
                    }
                }

                #region 2013-11-10 注释
                ////删除被暂停通行策略的管控策略
                //DataTable dtdid = LinQBaseDao.Query("select DrivewayStrategy_ID  from DrivewayStrategy where DrivewayStrategy_State = '暂停' and DrivewayStrategy_Name ='" + drisname + "'").Tables[0];
                //if (dtdid.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dtdid.Rows.Count; i++)
                //    {
                //        LinQBaseDao.Query("delete ManagementStrategy where ManagementStrategy_DrivewayStrategy_ID=" + dtdid.Rows[i][0].ToString());
                //    }
                //}

                ////删除被暂停的通行策略
                //LinQBaseDao.Query("delete DrivewayStrategy where DrivewayStrategy_State = '暂停' and  DrivewayStrategy_Name ='" + drisname + "'");
                //string sortr = "";
                //if (sortYesOrNo)
                //{
                //    sortr = "有序";
                //}
                //else
                //{
                //    sortr = "无序";
                //}

                ////查出通行策略ID、通道ID
                //DataTable dtdi = LinQBaseDao.Query(" select DrivewayStrategy_ID, DrivewayStrategy_Driveway_ID from DrivewayStrategy where DrivewayStrategy_State='启动' and DrivewayStrategy_Name ='" + drisname + "'  order by DrivewayStrategy_Sort").Tables[0];
                //string sids = "";//通行策略ID字符串
                //DataTable dtstate;
                //if (dtdi.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dtdi.Rows.Count; i++)
                //    {
                //        sids += dtdi.Rows[i][0].ToString() + ",";
                //    }
                //    int driid = Convert.ToInt32(dtdi.Rows[0][1].ToString());//通道ID
                //    sids = sids.TrimEnd(',');
                //    #region 排队中车辆

                //    dtstate = LinQBaseDao.Query("select CarInfo_ID,CarInOutRecord_ID,CarInOutRecord_DrivewayStrategyS,SortNumberInfo_ID,SortNumberInfo_PositionValue,SortNumberInfo_DrivewayValue,SmallTicket_ID from View_CarState where CarInOutRecord_Remark='" + drisname + "' and (SortNumberInfo_TongXing='排队中' or SortNumberInfo_TongXing='待通行') and CarInOutRecord_Update=0").Tables[0];
                //    if (dtstate.Rows.Count > 0)
                //    {
                //        //门岗值
                //        string sortnumberinfo_positionvalue = LinQBaseDao.GetSingle("select p.Position_Value from Position p,Driveway d where d.Driveway_Position_ID=p.Position_ID and d.Driveway_ID= " + driid).ToString();
                //        //通道值
                //        string sortnumberinfo_drivewayvalue = LinQBaseDao.GetSingle("select Driveway_Value from Driveway  where  Driveway_ID=" + driid).ToString();

                //        // 修改DrivewayStrategy  的CarInOutRecord_Update状态为1
                //        LinQBaseDao.Query("update View_CarState set CarInOutRecord_DrivewayStrategyS='" + sids + "',CarInOutRecord_Driveway_ID='" + driid + "',CarInOutRecord_Sort='" + sortr + "',SortNumberInfo_PositionValue='" + sortnumberinfo_positionvalue + "',SortNumberInfo_DrivewayValue='" + sortnumberinfo_drivewayvalue + "' where  CarInOutRecord_Remark='" + drisname + "' and (SortNumberInfo_TongXing='排队中' or SortNumberInfo_TongXing='待通行') and CarInOutRecord_Update=0");
                //    }

                //#endregion

                //    #region 已进厂车辆
                //    dtstate = LinQBaseDao.Query("select CarInOutRecord_ID,CarInOutRecord_DrivewayStrategyS from View_CarState where  CarInOutRecord_Remark='" + drisname + "' and SortNumberInfo_TongXing='已进厂' and CarInOutRecord_Update=0").Tables[0];
                //    if (dtstate.Rows.Count > 0)
                //    {
                //        string drivstr = "";
                //        string[] drivstrs = sids.Split(',');
                //        int count = 0;
                //        foreach (var item in drivstrs)
                //        {
                //            count++;
                //            drivstr += item.ToString() + ",";
                //        }
                //        drivstr = drivstr.TrimEnd(',');
                //        if (count == 2)
                //        {
                //            drivstr = drivstr.Substring(0, drivstr.IndexOf(','));
                //            LinQBaseDao.Query("update View_CarState set CarInOutRecord_DrivewayStrategyS='" + sids + "',CarInOutRecord_DrivewayStrategy_IDS='" + drivstr + "',CarInOutRecord_Sort='" + sortr + "' where  CarInOutRecord_Remark='" + drisname + "' and SortNumberInfo_TongXing='已进厂' and CarInOutRecord_Update=0");
                //        }
                //        else
                //        {
                //            LinQBaseDao.Query("update View_CarState set CarInOutRecord_DrivewayStrategyS='" + sids + "',CarInOutRecord_Sort='" + sortr + "' where  CarInOutRecord_Remark='" + drisname + "' and   SortNumberInfo_TongXing='已进厂' and CarInOutRecord_Update=0");
                //        }
                //    }
                //    #endregion

                //    #region 已出厂 业务未完成车辆
                //    dtstate = LinQBaseDao.Query("select CarInOutRecord_ID,CarInOutRecord_DrivewayStrategyS from View_CarState where  CarInOutRecord_Remark='" + drisname + "' and SortNumberInfo_TongXing='已出厂' and  CarInOutRecord_ISFulfill=0 and  CarInOutRecord_Update=0").Tables[0];
                //    if (dtstate.Rows.Count > 0)
                //    {
                //        LinQBaseDao.Query("update View_CarState set CarInOutRecord_DrivewayStrategyS='" + sids + "',CarInOutRecord_Sort='" + sortr + "' where  CarInOutRecord_Remark='" + drisname + "' and   SortNumberInfo_TongXing='已出厂' and CarInOutRecord_Update=0");
                //    }
                //    #endregion
                //}
                #endregion
                MessageBox.Show("操作已完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                #endregion
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyForm.YesNo_DrivewayStrategy_SortMethod()");
            }
            finally
            {
                SetSortList.Clear();
                pc = new PageControl();
                SelectMethod();//重新绑定数据
                ClearMethod();
            }
        }


        /// <summary>
        /// 双击显示列表中的单条记录修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_Information_DoubleClick(object sender, EventArgs e)
        {
            if (dgv_Information.SelectedRows.Count == 0)
            {
                return;
            }
            btn_Preservation.Enabled = false;
            btnUpdate.Enabled = true;
            strDrivewayStrategy_ID = dgv_Information.SelectedRows[0].Cells["DrivewayStrategy_ID"].Value.ToString();
            txt_Driveway_Name.Text = "";
            try
            {
                if (dgv_Information.SelectedRows.Count > 0)
                {
                    cob_DrivewayStrategy_State.Text = dgv_Information.SelectedRows[0].Cells["DrivewayStrategy_State"].Value.ToString();
                    string strdrisname = dgv_Information.SelectedRows[0].Cells["DrivewayStrategy_Name1"].Value.ToString();
                    txt_DrivewayStrategy_Name.Text = strdrisname;
                    drivewaystrategy_name = strdrisname;

                    DataTable tables = LinQBaseDao.Query(" SELECT Position_Name,Driveway_Name,Driveway_type,DrivewayStrategy_Driveway_ID  FROM Driveway INNER JOIN Position ON Driveway.Driveway_Position_ID = Position.Position_ID INNER JOIN DrivewayStrategy ON Driveway.Driveway_ID = DrivewayStrategy.DrivewayStrategy_Driveway_ID where DrivewayStrategy_Name='" + strdrisname + "'  order by DrivewayStrategy_Sort").Tables[0];
                    txt_Driveway_Name.Text = "";
                    Driveway_IDList.Clear();
                    SetSortList.Clear();
                    for (int i = 0; i < tables.Rows.Count; i++)
                    {
                        txt_Driveway_Name.Text += "【" + tables.Rows[i]["Position_Name"] + "】" + tables.Rows[i]["Driveway_Name"] + "【" + tables.Rows[i]["Driveway_type"] + "】 ；";
                        Driveway_IDList.Add(Convert.ToInt32(tables.Rows[i]["DrivewayStrategy_Driveway_ID"]));
                    }
                    setSort();

                    txt_DrivewayStrategy_Reason.Text = dgv_Information.SelectedRows[0].Cells["DrivewayStrategy_Reason"].Value.ToString();
                    txt_DrivewayStrategy_Remark.Text = dgv_Information.SelectedRows[0].Cells["DrivewayStrategy_Remark"].Value.ToString();
                    if (dgv_Information.SelectedRows[0].Cells["DrivewayStrategy_ID"].Value != null)
                    {
                        intDrivewayStrategy_ID = CommonalityEntity.GetInt(dgv_Information.SelectedRows[0].Cells["DrivewayStrategy_ID"].Value.ToString());
                        listDrivewayStrategy_ID.Add(intDrivewayStrategy_ID);
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyForm.dgv_Information_DoubleClick()");
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
                            if (dgv_Information.SelectedRows[i].Cells["DrivewayStrategy_ID"].Value != null)//通行策略ID
                            {
                                listDrivewayStrategy_ID.Add(CommonalityEntity.GetInt(dgv_Information.SelectedRows[i].Cells["DrivewayStrategy_ID"].Value.ToString()));
                                if (dgv_Information.SelectedRows[i].Cells["DrivewayStrategy_Name1"].Value != null)//通行策略名称
                                {
                                    strDrivewayStrategy_name += dgv_Information.SelectedRows[i].Cells["DrivewayStrategy_Name1"].Value.ToString() +

" ";
                                }
                            }
                        }
                        strlog = String.Format("删除通行策略:'{0}'",

strDrivewayStrategy_name);
                        //if (JudgeCarTypeUseMethod())//查看当前要删除的通行策略是否正在使用
                        //{
                        Expression<Func<DrivewayStrategy, bool>> funDrivewayStrategy = n => listDrivewayStrategy_ID.Contains(n.DrivewayStrategy_ID);
                        if (DrivewayStrategyDAL.DeleteToMany(funDrivewayStrategy))
                        {
                            foreach (var item in listDrivewayStrategy_ID)
                            {
                                Expression<Func<ManagementStrategy, bool>> funManagementStrategy = n => n.ManagementStrategy_DrivewayStrategy_ID == item;
                                ManagementStrategyDAL.DeleteToMany(funManagementStrategy);
                            }
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
                    MessageBox.Show(strmessagebox, "提示",

MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception err)
            {

                CommonalityEntity.WriteTextLog

("DrivewayStrategyForm.btn_Delete_Click()");
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

                ClearMethod();
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyForm.btn_Empty_Click()");
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

                CommonalityEntity.WriteTextLog("DrivewayStrategyForm.dgv_Information_RowPostPaint()");
            }

        }


        private void btnSort_Click(object sender, EventArgs e)
        {
            sortYesOrNo = true;
            panel1.Visible = true;
            groupBox6.Visible = true;
            dgvSetShort.Visible = true;
            Driveway_IDList = new List<int>();
            SetSortList = new List<SetSort>();
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
                    s.text = "【" + tab.Rows[0]["Position_Name"].ToString() + "】" + " " + tab.Rows[0]["Driveway_Name"].ToString() + " " + "【" + tab.Rows[0]["Driveway_Type"].ToString() + "】";
                    s.sort = SetSortList.Count + 1;
                    s.id = Convert.ToInt32(tab.Rows[0]["Driveway_ID"]);
                    s.ischeck = false;
                    SetSortList.Add(s);
                }

            }

            dgvSetShort.DataSource = SetSortList;
            dgvSetShort.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvSetShort.AutoGenerateColumns = false;
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
                if (isUpdateOne == true)
                {
                    TreeNodeCollection nodes = this.trVDrivewayStrategy_Name.Nodes;
                    if (e.Node.Parent != null)
                    {
                        nodes = e.Node.Parent.Nodes;
                    }
                    foreach (TreeNode node in nodes)
                    {
                        if (node != e.Node)
                        {
                            node.Checked = false;
                        }
                    }

                }
                else
                {

                    //e.Node.ForeColor = Color.Yellow;
                    SetNodeCheckStatus(e.Node, e.Node.Checked);
                    SetNodeStyle(e.Node);
                }
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
                    dr["Driveway_ID"] = "0";
                    dr["Driveway_name"] = "全部";
                    dt.Rows.InsertAt(dr, 0);
                    cmbDrivewayID.DataSource = dt;
                    cmbDrivewayID.DisplayMember = "Driveway_name";
                    cmbDrivewayID.ValueMember = "Driveway_ID";
                    cmbDrivewayID.SelectedIndex = 0;
                }
            }
            catch
            {
            }
        }
        string strDrivewayStrategy_ID = "";



        /// <summary>
        /// 修改整个类型的通行策略
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateToType()
        {

            try
            {
                if (string.IsNullOrEmpty(cob_DrivewayStrategy_State.Text))
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "请选择状态！", cob_DrivewayStrategy_State, this);
                    return;
                }
                ///把数据状态进行更改、
                LinQBaseDao.Query("update DrivewayStrategy set DrivewayStrategy_State = '暂停' where DrivewayStrategy_Name='" + drivewaystrategy_name + "'");
                string drisname = txt_DrivewayStrategy_Name.Text.Trim();//新的策略名称
                if (Driveway_IDList.Count >= 1)
                {
                    int num = 0;
                    string Driveway_IDs = "";

                    //循环选中同行策略
                    for (int i = 0; i < SetSortList.Count; i++)
                    {
                        if (i != SetSortList.Count - 1)
                        {
                            Driveway_IDs += SetSortList[i].id + ",";
                        }
                        else
                        {
                            Driveway_IDs += SetSortList[i].id.ToString();
                        }
                        //根据通道编号和车辆类型编号，查询是否有该通道的通行策略
                        DataTable tablesDris = LinQBaseDao.Query("select top(1)* from DrivewayStrategy where DrivewayStrategy_Name='" + drivewaystrategy_name + "' and DrivewayStrategy_Driveway_ID=" + SetSortList[i].id).Tables[0];

                        DataTable tablesDrisn = LinQBaseDao.Query("select Position_Name,Driveway_Name,Driveway_type from View_DrivewayPosition where driveway_warrantystate='正常' and driveway_state='启动' and Driveway_ID=" + Convert.ToInt32(Driveway_IDList[i].ToString()) + "  order by Position_Name asc, Driveway_Name asc").Tables[0];


                        //有该条策略就修改
                        if (tablesDris.Rows.Count == 1)
                        {
                            Expression<Func<DrivewayStrategy, bool>> upOneFn = n => n.DrivewayStrategy_ID == Convert.ToInt32(tablesDris.Rows[0]["DrivewayStrategy_ID"]);

                            Action<DrivewayStrategy> upOneAc = a =>
                            {
                                // a.DrivewayStrategy_State = "启动";
                                a.DrivewayStrategy_Name = drisname;
                                a.DrivewayStrategy_Record = tablesDrisn.Rows[0]["Position_Name"].ToString() + tablesDrisn.Rows[0]["Driveway_Name"].ToString() + tablesDrisn.Rows[0]["Driveway_type"].ToString();
                                if (sortYesOrNo)
                                {
                                    a.DrivewayStrategy_Sort = SetSortList[i].sort;
                                }
                                else
                                {
                                    a.DrivewayStrategy_Sort = 1;
                                }
                                a.DrivewayStrategy_State = cob_DrivewayStrategy_State.Text.Trim();
                                a.DrivewayStrategy_Reason = txt_DrivewayStrategy_Reason.Text.Trim();
                                a.DrivewayStrategy_Remark = txt_DrivewayStrategy_Remark.Text.Trim();
                            };
                            if (DrivewayStrategyDAL.Update(upOneFn, upOneAc))
                            {
                                CommonalityEntity.WriteLogData("修改", "添加通行策略：" + txt_Driveway_Name.Text + "的信息", CommonalityEntity.USERNAME);//添加操作日志
                            }

                            num++;
                        }
                        else
                        {

                            //没有该通道策略就添加
                            DrivewayStrategy ds = new DrivewayStrategy();
                            ds.DrivewayStrategy_Name = drisname;
                            ds.DrivewayStrategy_Driveway_ID = Convert.ToInt32(Driveway_IDList[i].ToString());//通道编号
                            ds.DrivewayStrategy_State = cob_DrivewayStrategy_State.Text;
                            ds.DrivewayStrategy_Record = tablesDrisn.Rows[0]["Position_Name"].ToString() + tablesDrisn.Rows[0]["Driveway_Name"].ToString() + tablesDrisn.Rows[0]["Driveway_type"].ToString();
                            //判断是否排序
                            if (sortYesOrNo)
                            {
                                ds.DrivewayStrategy_Sort = SetSortList[i].sort;
                            }
                            else
                            {
                                ds.DrivewayStrategy_Sort = 1;
                            }
                            ds.DrivewayStrategy_Reason = txt_DrivewayStrategy_Reason.Text.Trim();
                            ds.DrivewayStrategy_Remark = txt_DrivewayStrategy_Remark.Text.Trim();

                            if (DrivewayStrategyDAL.InsertOneQCRecord(ds))
                            {
                                CommonalityEntity.WriteLogData("新增", "新增通行策略：" + txt_Driveway_Name.Text + "的信息", CommonalityEntity.USERNAME);//添加操作日志
                            }
                            num++;
                        }
                    }

                    //删除被暂停通行策略的管控策略
                    DataTable dtdid = LinQBaseDao.Query("select DrivewayStrategy_ID  from DrivewayStrategy where DrivewayStrategy_State = '暂停' and DrivewayStrategy_Name='" + drivewaystrategy_name + "' ").Tables[0];
                    if (dtdid.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtdid.Rows.Count; i++)
                        {
                            LinQBaseDao.Query("delete ManagementStrategy where ManagementStrategy_DrivewayStrategy_ID=" + dtdid.Rows[i][0].ToString());
                        }
                    }
                    //删除被暂停的通行策略
                    LinQBaseDao.Query("delete DrivewayStrategy where DrivewayStrategy_State = '暂停'");

                    LinQBaseDao.Query("update Car set Car_DriSName='" + drisname + "' where  Car_DriSName='" + drivewaystrategy_name + "' update CarInOutRecord set CarInOutRecord_Remark='" + drisname + "' where CarInOutRecord_Remark='" + drivewaystrategy_name + "' update CarType set CarType_DriSName='" + drisname + "' where CarType_DriSName='" + drivewaystrategy_name + "'");

                    string sortr = "";
                    if (sortYesOrNo)
                    {
                        sortr = "有序";
                    }
                    else
                    {
                        sortr = "无序";
                    }


                    //查出通行策略ID、通道ID
                    DataTable dtdi = LinQBaseDao.Query(" select DrivewayStrategy_ID, DrivewayStrategy_Driveway_ID from DrivewayStrategy where DrivewayStrategy_State='启动' and DrivewayStrategy_Name='" + drisname + "'   order by DrivewayStrategy_Sort").Tables[0];
                    string sids = "";//通行策略ID字符串
                    if (dtdi.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtdi.Rows.Count; i++)
                        {
                            sids += dtdi.Rows[i][0].ToString() + ",";
                        }
                        int driid = Convert.ToInt32(dtdi.Rows[0][1].ToString());//通道ID
                        sids = sids.TrimEnd(',');
                        #region 排队中车辆
                        DataTable dtstate = LinQBaseDao.Query("select CarInfo_ID,CarInOutRecord_ID,CarInOutRecord_DrivewayStrategyS,SortNumberInfo_ID,SortNumberInfo_PositionValue,SortNumberInfo_DrivewayValue,SmallTicket_ID from View_CarState where CarInOutRecord_Remark='" + drisname + "' and (SortNumberInfo_TongXing='排队中' or SortNumberInfo_TongXing='待通行') and CarInOutRecord_Update=0").Tables[0];
                        if (dtstate.Rows.Count > 0)
                        {
                            //门岗值
                            string sortnumberinfo_positionvalue = LinQBaseDao.GetSingle("select p.Position_Value from Position p,Driveway d where d.Driveway_Position_ID=p.Position_ID and d.Driveway_ID= " + driid).ToString();
                            //通道值
                            string sortnumberinfo_drivewayvalue = LinQBaseDao.GetSingle("select Driveway_Value from Driveway  where  Driveway_ID=" + driid).ToString();

                            // 修改DrivewayStrategy  的CarInOutRecord_Update状态为1
                            LinQBaseDao.Query("update View_CarState set CarInOutRecord_DrivewayStrategyS='" + sids + "',CarInOutRecord_Driveway_ID='" + driid + "',CarInOutRecord_Sort='" + sortr + "' where   CarInOutRecord_Remark='" + drisname + "' and (SortNumberInfo_TongXing='排队中' or SortNumberInfo_TongXing='待通行') and CarInOutRecord_Update=0");
                            LinQBaseDao.Query("update View_CarState set SortNumberInfo_PositionValue='" + sortnumberinfo_positionvalue + "',SortNumberInfo_DrivewayValue='" + sortnumberinfo_drivewayvalue + "' where   CarInOutRecord_Remark='" + drisname + "' and SortNumberInfo_ISEffective=1  and (SortNumberInfo_TongXing='排队中' or SortNumberInfo_TongXing='待通行') and CarInOutRecord_Update=0");
                        }
                        #endregion

                        #region 已进厂车辆
                        //单次进出门岗，已进厂车辆修改出厂通行策略
                        dtstate = LinQBaseDao.Query("select CarInOutRecord_ID,CarInOutRecord_DrivewayStrategyS from View_CarState where  CarInOutRecord_Remark='" + drisname + "' and SortNumberInfo_TongXing='已进厂' and CarInOutRecord_Update=0").Tables[0];
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
                                LinQBaseDao.Query("update View_CarState set CarInOutRecord_DrivewayStrategyS='" + sids + "',CarInOutRecord_DrivewayStrategy_IDS='" + drivstr + "',CarInOutRecord_Sort='" + sortr + "' where  CarInOutRecord_Remark='" + drisname + "' and SortNumberInfo_TongXing='已进厂' and CarInOutRecord_Update=0");
                            }
                            else
                            {
                                LinQBaseDao.Query("update View_CarState set CarInOutRecord_DrivewayStrategyS='" + sids + "',CarInOutRecord_Sort='" + sortr + "' where  CarInOutRecord_Remark='" + drisname + "' and SortNumberInfo_TongXing='已进厂' and CarInOutRecord_Update=0");
                            }
                        }
                        #endregion

                        #region 已出厂 业务未完成车辆
                        dtstate = LinQBaseDao.Query("select CarInOutRecord_ID,CarInOutRecord_DrivewayStrategyS from View_CarState where  CarInOutRecord_Remark='" + drisname + "' and SortNumberInfo_TongXing='已出厂' and  CarInOutRecord_ISFulfill=0 and  CarInOutRecord_Update=0").Tables[0];
                        if (dtstate.Rows.Count > 0)
                        {
                            LinQBaseDao.Query("update View_CarState set CarInOutRecord_DrivewayStrategyS='" + sids + "',CarInOutRecord_Sort='" + sortr + "' where  CarInOutRecord_Remark='" + drisname + "' and   SortNumberInfo_TongXing='已出厂' and CarInOutRecord_Update=0");
                        }
                        #endregion
                    }
                    MessageBox.Show("操作已完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyForm.btnUpdate_Click()" + "".ToString());
            }
            finally
            {
                SelectMethod();//更新数据
            }
        }
        /// <summary>
        /// 通行策略修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateToType();
            ClearMethod();
        }

        private void btnSetShortNo_Click(object sender, EventArgs e)
        {

            panel1.Visible = false;
            groupBox6.Visible = false;
            dgvSetShort.Visible = false;
            SetSortList.Clear();
        }

        private void btnSetShortOk_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            groupBox6.Visible = false;
            dgvSetShort.Visible = false;

        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            upDown("up");
        }

        private void btnDown_Click(object sender, EventArgs e)
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

        private void BtnSetsortNo_Click(object sender, EventArgs e)
        {
            txt_DrivewayStrategy_Reason.Text = GetDefult();

            panel1.Visible = false;
            groupBox6.Visible = false;
            dgvSetShort.Visible = false;
            sortYesOrNo = false;
        }


        /// <summary>
        /// 新增默认通道的功能模块
        /// </summary>
        /// <returns></returns>
        private string GetDefult()
        {
            string rusult = "";
            for (int i = 0; i < dgvSetShort.Rows.Count; i++)
            {


                DataGridViewCheckBoxCell check = dgvSetShort.Rows[i].Cells[3] as DataGridViewCheckBoxCell;//假设你的第一列是checkbox,如果不是请自行更改
                if (check.Value != null)
                {
                    if ((bool)check.Value)//当选中时
                    {
                        rusult += dgvSetShort.Rows[i].Cells[1].Value.ToString() + "；";
                        //假设你要放到text中值时第一列的,如果不是也请自行更改
                    }
                }
            }
            //  string str = rusult.Substring(0, rusult.Length - 1);
            //这样就完成了。

            return rusult;
        }

    }
    public class SetSort
    {
        public int sort { get; set; }
        public string text { get; set; }
        public int id { get; set; }
        public bool ischeck { get; set; }
    }

}



