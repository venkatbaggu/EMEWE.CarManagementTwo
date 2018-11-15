using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.HelpClass;
using EMEWE.CarManagement.Entity;
using System.Collections;
using EMEWE.CarManagement.CommonClass;
using EMEWE.CarManagement.Commons.CommonClass;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class ControlInfoForm : Form
    {
        public ControlInfoForm()
        {
            InitializeComponent();
        }
        public int m_MouseClicks = 0; //记录鼠标在myTreeView控件上按下的次数 
        DataTable ModuleTable;//存放菜单集合
        /// <summary>
        /// 存放要删除的管控信息名称
        /// </summary>
        private string strControlInfo_Name = "";
        /// <summary>
        /// 存放当前管控信息编号
        /// </summary>
        private int intControlInfo_ID = 0;
        /// <summary>
        /// 存放当前管控信息上级编号
        /// </summary>
        private int intControlInfo_HeightID = 0;
        /// <summary>
        /// 存放要执行的SQL语句
        /// </summary>
        ArrayList arraylist = new ArrayList();
        /// <summary>
        /// 存放要删除的管控信息编程
        /// </summary>
        List<int> list = new List<int>();
        /// <summary>
        /// 存放TREEVIEW名称用状态（true：展开 false：叠起）
        /// </summary>
        Dictionary<int, bool> dictionary = null;
        //public MainForm mf;
        //存放旧的管控信息实际值
        private string stroldControlInfo_IDValue = "";
        private void ControlInfoForm1_Load(object sender, EventArgs e)
        {
            userContext();
            Initialization();
            userContext();
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

                btn_delete.Enabled = true;
                btn_delete.Visible = true;

                btn_ManagementStrategy.Enabled = true;
                btn_ManagementStrategy.Visible = true;
            }
            else
            {
                //btn_Preservation.Visible = ControlAttributes.BoolControl("btn_Preservation", "ControlInfoForm", "Visible");
                //btn_Preservation.Enabled = ControlAttributes.BoolControl("btn_Preservation", "ControlInfoForm", "Enabled");

                //btn_delete.Visible = ControlAttributes.BoolControl("btn_delete", "ControlInfoForm", "Visible");
                //btn_delete.Enabled = ControlAttributes.BoolControl("btn_delete", "ControlInfoForm", "Enabled");

                //btn_ManagementStrategy.Enabled = ControlAttributes.BoolControl("btn_ManagementStrategy", "ControlInfoForm", "Enabled");
                //btn_ManagementStrategy.Visible = ControlAttributes.BoolControl("btn_ManagementStrategy", "ControlInfoForm", "Visible");
            }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        private void Initialization()
        {
            try
            {
                InitMenu();
                StateMethos();//状态绑定
                //mf = new MainForm();

            }
            catch
            {
                CommonalityEntity.WriteTextLog("ControlInfoForm.Initialization()");
            }


        }
        /// <summary>
        /// 状态绑定
        /// </summary>
        private void StateMethos()
        {
            try
            {
                var p = DictionaryDAL.GetValueStateDictionary("01");
                int intcount = p.Count();

                cob_ControlInfo_State.DataSource = p.Where(n => n.Dictionary_Name != "全部").Select(n => n).ToList();
                this.cob_ControlInfo_State.DisplayMember = "Dictionary_Name";
                cob_ControlInfo_State.ValueMember = "Dictionary_Value";
                if (cob_ControlInfo_State.DataSource != null)
                {
                    cob_ControlInfo_State.SelectedIndex = 0;
                }
                comboxType.DataSource = DictionaryDAL.GetValueDictionary("03");
                comboxType.DisplayMember = "Dictionary_Name";
                comboxType.ValueMember = "Dictionary_Value";
                comboxType.SelectedValue = -1;
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ControlInfoForm.StateMethos()");
            }

        }
        /// <summary>
        /// 初始化菜单
        /// </summary>
        /// 
        protected void InitMenu()
        {
            try
            {
                tv_ControlInfo.Nodes.Clear();
                dictionary = new Dictionary<int, bool>();
                TreeNode nodeTemp = new TreeNode();
                nodeTemp.Text = "根节点";
                nodeTemp.Tag = 0;
                tv_ControlInfo.Nodes.Add(nodeTemp);
                DataSet ds = new DataSet();
                ds = LinQBaseDao.Query("select * from ControlInfo");
                ModuleTable = ds.Tables[0];
                //加载TreeView菜单   
                LoadNode(tv_ControlInfo.Nodes, "0");

            }
            catch
            {
                CommonalityEntity.WriteTextLog("ControlInfoForm.InitMenu()");
            }

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
                    dvList.RowFilter = "ControlInfo_HeightID is null";  //过滤父节点 
                }
                else
                {
                    dvList.RowFilter = "ControlInfo_HeightID=" + MtID;  //过滤父节点 
                }
                TreeNode nodeTemp;
                foreach (DataRowView dv in dvList)
                {
                    string strControlInfo_ID = dv["ControlInfo_ID"].ToString();
                    string strControlInfo_Name = dv["ControlInfo_Name"].ToString();  //节点名称 

                    dictionary.Add(common.GetInt(strControlInfo_ID), true);
                    nodeTemp = new TreeNode();


                    nodeTemp.Tag = strControlInfo_ID;
                    nodeTemp.Text = strControlInfo_Name;
                    if (dv["ControlInfo_State"] != null)
                    {
                        if (dv["ControlInfo_State"].ToString() != "启动")
                        {
                            nodeTemp.ForeColor = Color.Gray;
                        }
                    }
                    node.Add(nodeTemp);  //加入节点 
                    this.LoadNode(nodeTemp.Nodes, nodeTemp.Tag.ToString().Split(',')[0]);  //递归

                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("ControlInfoForm.LoadNode()");
            }

        }
        /// <summary>
        /// 双击节点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tv_ControlInfo_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                ClearMethod();
                string sql = "";
                string srtControlInfo_HeightName = "";
                if (e.Node.Tag == null || e.Node.Text == null) return;
                {
                    if (e.Node.Parent == null)
                    {
                        srtControlInfo_HeightName = "根节点";

                    }
                    else
                    {
                        srtControlInfo_HeightName = e.Node.Parent.Text;//上级名称
                    }
                    txt_ControlInfo_HeightName.Text = srtControlInfo_HeightName;
                    intControlInfo_ID = common.GetInt(e.Node.Tag.ToString());
                    sql = String.Format("select * from ControlInfo where ControlInfo_ID={0}", common.GetInt(e.Node.Tag.ToString()));
                    DataTable dt = LinQBaseDao.Query(sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        btn_Empty.Text = "取消修改";
                        txt_ControlInfo_Name.ReadOnly = true;
                        txt_ControlInfo_Name.Text = dt.Rows[0]["ControlInfo_Name"] == null ? "" : dt.Rows[0]["ControlInfo_Name"].ToString();
                        txt_ControlInfo_Value.Text = dt.Rows[0]["ControlInfo_Value"] == null ? "" : dt.Rows[0]["ControlInfo_Value"].ToString();
                        txt_ControlInfo_Rule.Text = dt.Rows[0]["ControlInfo_Rule"] == null ? "" : dt.Rows[0]["ControlInfo_Rule"].ToString();
                        cob_ControlInfo_State.Text = dt.Rows[0]["ControlInfo_State"] == null ? "" : dt.Rows[0]["ControlInfo_State"].ToString();
                        txt_ControlInfo_Content.Text = dt.Rows[0]["ControlInfo_Content"] == null ? "" : dt.Rows[0]["ControlInfo_Content"].ToString();
                        txt_ControlInfo_Remark.Text = dt.Rows[0]["ControlInfo_Remark"] == null ? "" : dt.Rows[0]["ControlInfo_Remark"].ToString();
                        txt_ControlInfo_IDValue.Text = stroldControlInfo_IDValue = dt.Rows[0]["ControlInfo_IDValue"] == null ? "" : dt.Rows[0]["ControlInfo_IDValue"].ToString();
                        //comboxType.SelectedValue = dt.Rows[0]["ControlInfo_Type"] == null ? "" :dt.Rows[0]["ControlInfo_Type"].ToString();
                        comboxType.Text = dt.Rows[0]["ControlInfo_Type"] == null ? "" : dt.Rows[0]["ControlInfo_Type"].ToString();
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ControlInfoForm.tv_ControlInfo_NodeMouseDoubleClick()");
            }
        }
        /// <summary>
        /// 单击节点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tv_ControlInfo_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            DataTable dt = new DataTable();
            try
            {
                strControlInfo_Name = "";
                if (e.Node.Tag == null || e.Node.Text == null) return;
                list.Clear();
                intControlInfo_HeightID = common.GetInt(e.Node.Tag.ToString());//获取父级编号
                //dt=LinQBaseDao.Query("select max(ControlInfo_ID)+1 from ControlInfo where ControlInfo_HeightID is not null").Tables[0];
                //int strControlInfo_ID=dt.Rows[0][0]==null?0:CommonalityEntity.GetInt( dt.Rows[0][0].ToString());//当前管控信息最大编号
                if (e.Node.Text != "根节点")
                {
                    txt_ControlInfo_HeightName.Text = e.Node.Text;//上级名称
                    //dt = LinQBaseDao.Query(String.Format("select count(*)+1 from ControlInfo where ControlInfo_HeightID={0}", intControlInfo_HeightID)).Tables[0];
                    //string str = "0" + (dt.Rows[0][0] == null ? 0 : CommonalityEntity.GetInt(dt.Rows[0][0].ToString()));
                    //txt_ControlInfo_IDValue.Text = intControlInfo_HeightID.ToString() + str.Substring(str.Length - 2, 2).ToString();

                }
                //else
                //{
                //    txt_ControlInfo_IDValue.Text = strControlInfo_ID.ToString();

                //}
                strControlInfo_Name += e.Node.Text;
                list.Add(common.GetInt(e.Node.Tag.ToString()));
                SetNodeCheckStatus(e.Node);

                // int intParent = common.GetInt(e.Node.Tag);
                // if (GetrboolMethod(intParent))
                // {

                //     SetNodeStyle(e.Node,false);
                //     TraverseDictionaryMethod(intParent, false);
                // }
                // else
                // {
                //     SetNodeStyle(e.Node, true);
                //     TraverseDictionaryMethod(intParent, true);
                // }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ControlInfoForm.tv_ControlInfo_NodeMouseClick()");
            }
        }

        /// <summary>
        /// 展开/叠起节点
        /// </summary>
        /// <param name="Node"></param>
        /// <param name="rbool">true:展开 false</param>
        private void SetNodeStyle(TreeNode Node, bool rbool)
        {

            if (!rbool)
            {

                Node.Collapse();
            }
            else
            {
                Node.ExpandAll();
            }
            foreach (TreeNode tnTemp in Node.Nodes)
            {

                TraverseDictionaryMethod(common.GetInt(tnTemp.Tag), rbool);
                strControlInfo_Name += ",";
                list.Add(common.GetInt(tnTemp.Tag.ToString()));
                strControlInfo_Name += String.Format("'{0}'", tnTemp.Text.ToString());
                SetNodeStyle(tnTemp, rbool);

            }

        }
        /// <summary>
        /// 判断节点状态
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool GetrboolMethod(int key)
        {

            bool rbool = false;
            foreach (KeyValuePair<int, bool> item in dictionary)
            {
                if (item.Key == key)
                {

                    rbool = item.Value;
                    break;
                }

            }
            return rbool;
        }
        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="rbool"></param>
        private void TraverseDictionaryMethod(int key, bool rbool)
        {

            foreach (KeyValuePair<int, bool> item in dictionary)
            {
                if (item.Key == key)
                {
                    dictionary.Remove(item.Key);
                    dictionary.Add(item.Key, rbool);
                    break;
                }
            }
        }
        /// <summary>
        /// 遍历当前节点及子节点
        /// </summary>
        /// <param name="tn"></param>
        private void SetNodeCheckStatus(TreeNode tn)
        {
            try
            {
                if (tn == null) return;

                foreach (TreeNode tnChild in tn.Nodes)
                {
                    strControlInfo_Name += ",";
                    list.Add(common.GetInt(tnChild.Tag.ToString()));
                    strControlInfo_Name += String.Format("'{0}'", tnChild.Text.ToString());
                    SetNodeCheckStatus(tnChild);

                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ControlInfoForm.SetNodeCheckStatus()");
            }

        }

        /// <summary>
        /// 保存按钮双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Preservation_Click(object sender, EventArgs e)
        {
            try
            {
                if (!NullHandleMethod()) return;//查重
                string strControlInfo_Name = txt_ControlInfo_Name.Text.Trim();
                string strControlInfo_Value = txt_ControlInfo_Value.Text.Trim();
                string strControlInfo_State = cob_ControlInfo_State.Text.Trim();
                string strControlInfo_Content = txt_ControlInfo_Content.Text.Trim();
                string strControlInfo_Remark = txt_ControlInfo_Remark.Text.Trim();
                string strControlInfo_Rule = txt_ControlInfo_Rule.Text.Trim();
                string strControlInfo_IDValue = txt_ControlInfo_IDValue.Text.Trim();
                string strmessagebox = "";
                string strlog = "";
                if (btn_Preservation.Enabled)
                {
                    btn_Preservation.Enabled = false;
                }

                if (btn_Empty.Text == "清  空")
                {

                    ControlInfo ci = new ControlInfo();
                    ci.ControlInfo_Name = strControlInfo_Name;
                    ci.ControlInfo_Value = strControlInfo_Value;
                    ci.ControlInfo_State = strControlInfo_State;
                    ci.ControlInfo_Content = strControlInfo_Content;
                    ci.ControlInfo_Remark = strControlInfo_Remark;
                    ci.ControlInfo_Rule = strControlInfo_Rule;
                    ci.ControlInfo_IDValue = strControlInfo_IDValue;

                    //ci.ControlInfo_Type = comboxType.SelectedValue.ToString();//用管控信息实际值
                    ci.ControlInfo_Type = comboxType.Text;//用管控信息类型名称

                    if (intControlInfo_HeightID > 0)
                    {
                        ci.ControlInfo_HeightID = intControlInfo_HeightID;
                    }
                    if (ControlInfoDAL.InsertOneQCRecord(ci))
                    {
                        strlog = String.Format("添加管控信息：'{0}'", txt_ControlInfo_Name.Text.Trim());
                        strmessagebox = "添加管控信息成功";
                        intControlInfo_HeightID = 0;
                        CommonalityEntity.WriteLogData("新增", strlog, CommonalityEntity.USERNAME);
                        ClearMethod();

                    }
                    else
                    {
                        strmessagebox = "添加管控信息失败";
                    }
                }
                else
                {


                    Expression<Func<ControlInfo, bool>> fun = n => n.ControlInfo_ID == intControlInfo_ID;
                    Action<ControlInfo> action = n =>
                    {
                        n.ControlInfo_Value = strControlInfo_Value;
                        n.ControlInfo_State = strControlInfo_State;
                        n.ControlInfo_Content = strControlInfo_Content;
                        n.ControlInfo_Remark = strControlInfo_Remark;
                        n.ControlInfo_Rule = strControlInfo_Rule;
                        n.ControlInfo_IDValue = strControlInfo_IDValue;
                        //ci.ControlInfo_Type = comboxType.SelectedValue.ToString();//用管控信息实际值
                        n.ControlInfo_Type = comboxType.Text;//用管控信息类型名称

                    };
                    if (ControlInfoDAL.Update(fun, action))
                    {
                        strlog = String.Format("修改管控信息：'{0}'", txt_ControlInfo_Name.Text.Trim());
                        strmessagebox = "管控信息修改成功";
                        intControlInfo_ID = 0;
                        CommonalityEntity.WriteLogData("修改", strlog, CommonalityEntity.USERNAME);
                        ClearMethod();
                        txt_ControlInfo_Name.ReadOnly = false;
                        btn_Empty.Text = "清 空";
                        InitMenu();//更新
                    }
                    else
                    {
                        strmessagebox = "管控信息修改失败";
                    }
                }

                if (strmessagebox != "")
                {
                    MessageBox.Show(strmessagebox, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ControlInfoForm.btn_Preservation_Click()");
            }
            finally
            {
                btn_Preservation.Enabled = true;


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
                if (string.IsNullOrEmpty(txt_ControlInfo_Name.Text.Trim()))
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "管控信息名称不能为空！", txt_ControlInfo_Name, this);
                    return false;
                }
                if (string.IsNullOrEmpty(cob_ControlInfo_State.Text.Trim()))
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "管控信息状态不能为空！", cob_ControlInfo_State, this);
                    return false;
                }
                if (string.IsNullOrEmpty(comboxType.Text.Trim()))
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "管控类型不能为空！", comboxType, this);
                    return false;
                }
                //if (btn_Empty.Text == "清  空")
                //{
                //if (SelectRepeatMethod())//管控信息查重
                //{
                //    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该管控信息已存在！", txt_ControlInfo_Name, this);
                //    return false;
                //}
                //}
                if (!SelectRepeatMethod())
                {
                    return false;
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("ControlInfoForm.NullHandleMethod()");
            }
            return rbool;

        }
        /// <summary>
        /// 查重方法
        /// </summary>
        private bool SelectRepeatMethod()
        {
            bool rbool = true;
            try
            {
                DataSet ds = new DataSet();
                string sqlselectrepeat = "";
                string strControlInfo_Name = txt_ControlInfo_Name.Text.Trim();
                string strControlInfoIDValue = txt_ControlInfo_IDValue.Text.Trim();
                if (btn_Empty.Text == "清  空")
                {
                    //管控信息名称查重
                    if (intControlInfo_HeightID > 0)
                    {
                        sqlselectrepeat = String.Format("select  * from ControlInfo where ControlInfo_Name='{0}' and ControlInfo_HeightID={1}", strControlInfo_Name, intControlInfo_HeightID);
                    }
                    else
                    {
                        sqlselectrepeat = String.Format("select  * from ControlInfo where ControlInfo_Name='{0}' and ControlInfo_HeightID is null", strControlInfo_Name);
                    }
                    ds = LinQBaseDao.Query(sqlselectrepeat);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该管控信息名称已存在！", txt_ControlInfo_Name, this);

                        rbool = false;
                        return false;
                    }
                }
                //管控信息实际值查重
                if (stroldControlInfo_IDValue != strControlInfoIDValue)
                {
                    sqlselectrepeat = String.Format("select * from ControlInfo where ControlInfo_IDValue='{0}'", strControlInfoIDValue);
                    ds = new DataSet();
                    ds = LinQBaseDao.Query(sqlselectrepeat);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该管控信息管控信息实际值已存在！", txt_ControlInfo_IDValue, this);
                        rbool = false;
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ControlInfoForm.SelectRepeatMethod()");
            }
            return rbool;
        }

        /// <summary>
        /// 清空控件方法
        /// </summary>
        private void ClearMethod()
        {
            try
            {

                txt_ControlInfo_Rule.Text = "";
                txt_ControlInfo_HeightName.Text = "";
                txt_ControlInfo_Name.Text = "";
                txt_ControlInfo_Value.Text = "";
                txt_ControlInfo_IDValue.Text = "";
                txt_ControlInfo_Content.Text = "";
                txt_ControlInfo_Remark.Text = "";

            }
            catch
            {
                CommonalityEntity.WriteTextLog("ControlInfoForm.ClearMethod()");
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
                CommonalityEntity.WriteTextLog("ControlInfoForm.btn_Empty_Click()");
            }
            finally
            {
                intControlInfo_HeightID = 0;
                btn_Empty.Text = "清  空";
                txt_ControlInfo_Name.ReadOnly = false;
                btn_Empty.Enabled = true;
            }
        }
        /// <summary>
        /// 删除按钮双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_delete_Click(object sender, EventArgs e)
        {
            try
            {
                string strmessagebox = "";
                string strlog = "";
                if (btn_delete.Enabled)
                {
                    btn_delete.Enabled = false;
                }

                if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {


                    if (JudgeCarTypeUseMethod())//判断要删除的管控信息是否正在使用
                    {
                        Expression<Func<ControlInfo, bool>> fun = n => list.Contains(common.GetInt(n.ControlInfo_ID));
                        if (ControlInfoDAL.DeleteToMany(fun))
                        {
                            strmessagebox = "成功删除管控信息";
                            list.Clear();
                            strlog = String.Format("删除管控信息：{0}", strControlInfo_Name);
                            CommonalityEntity.WriteLogData("删除", strlog, CommonalityEntity.USERNAME);
                        }
                        else
                        {
                            strmessagebox = "删除管控信息失败";
                        }
                    }
                    else
                    {
                        strmessagebox = "该管控信息正在使用不能删除";
                    }
                }

                if (strmessagebox != "")
                {
                    MessageBox.Show(strmessagebox, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            catch
            {
                CommonalityEntity.WriteTextLog("ControlInfoForm.btn_delete_Click()");
            }
            finally
            {

                btn_delete.Enabled = true;

                InitMenu();//更新
            }
        }
        /// <summary>
        /// 判断当前操作的车辆类型是否正在使用
        /// </summary>
        private bool JudgeCarTypeUseMethod()
        {

            bool rbool = true;
            try
            {
                if (LinQBaseDao.Query("select ManagementStrategy_DrivewayStrategy_ID FROM  dbo.ControlInfo INNER JOIN dbo.ManagementStrategy ON dbo.ControlInfo.ControlInfo_ID = dbo.ManagementStrategy.ManagementStrategy_ControlInfo_ID ").Tables[0].AsEnumerable().Where(n => list.Contains(n.Field<int>("ManagementStrategy_DrivewayStrategy_ID"))).Count() > 0)
                {
                    rbool = false;
                }
                //string sql = String.Format("select * from dbo.ManagementStrategy where ManagementStrategy_DrivewayStrategy_ID in(select ControlInfo_ID from dbo.ControlInfo where ControlInfo_Name in ('{0}')) and ManagementStrategy_State='启动'", strControlInfo_Name);
                //arraylist.Add(sql);
                //if (LinQBaseDao.GetCount(arraylist) > 0)
                //{
                //    rbool = false;
                //}
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ControlInfoForm.JudgeCarTypeUseMethod()");
            }

            return rbool;


        }
        /// <summary>
        /// 管控策略按钮双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ManagementStrategy_Click(object sender, EventArgs e)
        {
            ManagementStrategyForm msf = new ManagementStrategyForm(null);
            PublicClass.ShowChildForm(msf);
            //mf.ShowChildForm(msf, this);
        }



        //获取鼠标在myTreeView控件按下的次数，并赋给全局变量m_MouseClicks
        private void tv_ControlInfo_MouseDown(object sender, MouseEventArgs e)
        {
            this.m_MouseClicks = e.Clicks;

        }
        //myTreeView控件节点折叠之前判断鼠标按下的次数，并进行控制
        private void tv_ControlInfo_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            if (this.m_MouseClicks > 1)
            {
                //如果是鼠标双击则禁止结点折叠
                e.Cancel = true;
            }
            else
            {
                //如果是鼠标单击则允许结点折叠
                e.Cancel = false;
            }
        }
        //myTreeView控件节点展开之前判断鼠标按下的次数，并进行控制
        private void tv_ControlInfo_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (this.m_MouseClicks > 1)
            {
                //如果是鼠标双击则禁止结点展开
                e.Cancel = true;
            }
            else
            {
                //如果是鼠标单击则允许结点展开
                e.Cancel = false;
            }
        }
        /// <summary>
        /// 管控信息默认规则：
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txt_ControlInfo_Rule_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (panel1.Visible)
                {
                    panel1.Visible = false;
                }
                else
                {
                    List<RulesXML> listRulesXML = new List<RulesXML>();
                    string filepath = System.IO.Directory.GetCurrentDirectory() + "\\EMEWE.CarManagement.Commons.xml";
                    XDocument xml = XDocument.Load(filepath);
                    CheckProperties cp = new CheckProperties();
                    List<string> list = cp.GetMethodsReflect();
                    var p = xml.Elements("doc").Elements("members").Elements("member");


                    foreach (var l in list)
                    {
                        foreach (var m in p)
                        {

                            if (m.Attribute("name").ToString().Contains(l))
                            {
                                RulesXML rx = new RulesXML();
                                rx.Name1 = m.Value.ToString().Trim();
                                rx.Value1 = l.ToString();
                                listRulesXML.Add(rx);
                                break;
                            }


                        }
                    }
                    lb_CarTypeAttribute.DataSource = listRulesXML;
                    lb_CarTypeAttribute.DisplayMember = "Name1";
                    lb_CarTypeAttribute.ValueMember = "Value1";
                    panel1.Visible = true;

                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ControlInfoForm.txt_ControlInfo_Rule_DoubleClick()");
            }


        }

        private void lb_CarTypeAttribute_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string strname = "";
                string strvalue = "";
                for (int i = 0; i < lb_CarTypeAttribute.SelectedItems.Count; i++)
                {
                    //多选、
                    //txt_CarTypeAttribute.Text += lb_CarTypeAttribute.GetItemText(lb_CarTypeAttribute.SelectedItems[i])+",";

                    //单选
                    strname = lb_CarTypeAttribute.GetItemText(lb_CarTypeAttribute.SelectedItems[i]);
                    strvalue = lb_CarTypeAttribute.SelectedValue.ToString();
                }
                txt_ControlInfo_Rule.Text = strvalue;
                panel1.Visible = false;
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ControlInfoForm.lb_CarTypeAttribute_DoubleClick()");
            }
        }

        private void txt_ControlInfo_Value_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                string str = txt_ControlInfo_Value.Text.Trim();
                if (str != "")
                {
                    double.Parse(str);
                }
            }
            catch
            {
                txt_ControlInfo_Value.Text = "";
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "管控信息默认值必须是纯数字！", txt_ControlInfo_Value, this);
                CommonalityEntity.WriteTextLog("CarTypeForm.txt_ControlInfo_Value_KeyPress()");
            }
        }

        private void txt_ControlInfo_IDValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (CommonalityEntity.DigitalMethod(e))
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "管控信息实际值必须是纯数字！", txt_ControlInfo_IDValue, this);
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("CarTypeForm.txt_ControlInfo_IDValue_KeyPress()");
            }
        }
        private void TV_MenuInfo_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        public int tags = 0;
        private void TV_MenuInfo_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }
        private Point Position = new Point(0, 0);
        private void ControlInfoForm_DragDrop(object sender, DragEventArgs e)
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
            Position = tv_ControlInfo.PointToClient(Position);
            TreeNode DropNode = this.tv_ControlInfo.GetNodeAt(Position);
            // 1.目标节点不是空。2.目标节点不是被拖拽接点的字节点。3.目标节点不是被拖拽节点本身
            if (DropNode != null && DropNode.Parent != myNode && DropNode != myNode)
            {
                TreeNode DragNode = myNode;
                // 将被拖拽节点从原来位置删除。
                myNode.Remove();
                // 在目标节点下增加被拖拽节点
                DropNode.Nodes.Add(DragNode);
                LinQBaseDao.Query("update ControlInfo set ControlInfo_IDValue='" + Convert.ToInt32(DropNode.Tag) + "' where ControlInfo_ID='" + Convert.ToInt32(myNode.Tag) + "'");
            }
            // 如果目标节点不存在，即拖拽的位置不存在节点，那么就将被拖拽节点放在根节点之下
            if (DropNode == null)
            {
                TreeNode DragNode = myNode;
                myNode.Remove();
                tv_ControlInfo.Nodes.Add(DragNode);
                LinQBaseDao.Query("update ControlInfo set ControlInfo_IDValue=0 where ControlInfo_ID='" + Convert.ToInt32(myNode.Tag) + "'");
            }
        }
    }
}
