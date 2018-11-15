using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.SystemAdmin;
using EMEWE.CarManagement.Commons.CommonClass;

namespace EMEWE.CarManagement.MyControl
{
    public partial class MyUserTree : UserControl
    {
        public MyUserTree()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 返回驾驶员名称组
        /// </summary>
        public string StaffInfo_Name = "";

        /// <summary>
        /// 返回驾驶员ID组
        /// </summary>
        public string StaffInfo_ID = "";

        /// <summary>
        /// 查询方法
        /// </summary>
        public void StaffInfo_Select()
        {
           string SqlWhere = "and 1=1 ";
            tv_StaffInfo.Nodes.Clear();
            if (txtJSYName.Text.Trim() != "")
            {
                //条件模糊查询
                string UserSql = "Select top(20) StaffInfo_ID,StaffInfo_Name,StaffInfo_Identity from StaffInfo where StaffInfo_Name like '%" + txtJSYName.Text.Trim() + "%' " + SqlWhere;
                DataSet ds = LinQBaseDao.Query(UserSql);
                TreeNode nodeTemp=null;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string SqlStaffInfo_identity = "Select top(20) StaffInfo_ID,StaffInfo_Name,StaffInfo_Identity  from StaffInfo where StaffInfo_Name='" + ds.Tables[0].Rows[i]["StaffInfo_Name"].ToString() + "' " + SqlWhere;
                    DataSet dst = LinQBaseDao.Query(SqlStaffInfo_identity);
                    if (dst.Tables[0].Rows.Count > 0)
                    {
                        nodeTemp = new TreeNode();
                        nodeTemp.Text = ds.Tables[0].Rows[i]["StaffInfo_Name"].ToString();//驾驶员名称
                        nodeTemp.Tag = ds.Tables[0].Rows[i]["StaffInfo_ID"].ToString();//驾驶员ID
                        nodeTemp.Name = ds.Tables[0].Rows[i]["StaffInfo_Name"].ToString();
                    }
                    //查询是否有相同姓名驾驶员
                    if (dst.Tables[0].Rows.Count>1)
                    {
                        TreeNode nodeTemps;
                        for (int j = 0; j < dst.Tables[0].Rows.Count; j++)
                        {
                            //添加子节点
                            nodeTemps = new TreeNode();
                            nodeTemps.Text = dst.Tables[0].Rows[j]["StaffInfo_Identity"].ToString();//驾驶员身份证
                            nodeTemps.Tag = dst.Tables[0].Rows[j]["StaffInfo_ID"].ToString();//驾驶员ID
                            nodeTemps.Name = dst.Tables[0].Rows[j]["StaffInfo_Identity"].ToString();
                            nodeTemp.Nodes.Add(nodeTemps);
                        }
                        SqlWhere += " and StaffInfo_Name not in('" + ds.Tables[0].Rows[i]["StaffInfo_Name"].ToString() + "')";
                        
                    }
                    if (dst.Tables[0].Rows.Count > 0)
                    {
                        tv_StaffInfo.Nodes.Add(nodeTemp);
                    }
                }
            }
            else
            {
                //全查询
                string UserSql = "Select top(20) StaffInfo_ID,StaffInfo_Name,StaffInfo_Identity  from StaffInfo";
                DataSet ds = LinQBaseDao.Query(UserSql);
                TreeNode nodeTemp = null;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string SqlStaffInfo_identity = "Select top(20) StaffInfo_ID,StaffInfo_Name,StaffInfo_Identity  from StaffInfo where StaffInfo_Name='" + ds.Tables[0].Rows[i]["StaffInfo_Name"].ToString() + "' " + SqlWhere;
                    DataSet dst = LinQBaseDao.Query(SqlStaffInfo_identity);
                    if (dst.Tables[0].Rows.Count > 0)
                    {
                        nodeTemp = new TreeNode();
                        nodeTemp.Text = ds.Tables[0].Rows[i]["StaffInfo_Name"].ToString();//驾驶员名称
                        nodeTemp.Tag = ds.Tables[0].Rows[i]["StaffInfo_ID"].ToString();//驾驶员ID
                        nodeTemp.Name = ds.Tables[0].Rows[i]["StaffInfo_Name"].ToString();
                    }
                    //查询是否有相同姓名驾驶员
                    if (dst.Tables[0].Rows.Count > 1)
                    {
                        TreeNode nodeTemps;
                        for (int j = 0; j < dst.Tables[0].Rows.Count; j++)
                        {
                            //添加子节点
                            nodeTemps = new TreeNode();
                            nodeTemps.Text = dst.Tables[0].Rows[j]["StaffInfo_Identity"].ToString();//驾驶员身份证
                            nodeTemps.Tag = dst.Tables[0].Rows[j]["StaffInfo_ID"].ToString();//驾驶员ID
                            nodeTemps.Name = dst.Tables[0].Rows[j]["StaffInfo_Identity"].ToString();
                            nodeTemp.Nodes.Add(nodeTemps);
                        }
                        SqlWhere += " and StaffInfo_Name not in('" + ds.Tables[0].Rows[i]["StaffInfo_Name"].ToString() + "')";

                    }
                    if (dst.Tables[0].Rows.Count > 0)
                    {
                        tv_StaffInfo.Nodes.Add(nodeTemp);
                    }
                }

            }
            SqlWhere = "and 1=1 ";
        }

        private void btncler_Click(object sender, EventArgs e)
        {
            txtJSYName.Text = "";
            tv_StaffInfo.Nodes.Clear();
            StaffInfo_Name="";
            StaffInfo_ID = "";
        }

        private void btncloes_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            txtJSYName.Text = "";
            tv_StaffInfo.Nodes.Clear();
        }

        /// <summary>
        /// 单击新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddJSY_Click(object sender, EventArgs e)
        {
            StaffInfoForm form = new StaffInfoForm();
            form.Show();
        }
        /// <summary>
        /// 点击确定组件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnqueding_Click(object sender, EventArgs e)
        {
            bool num = false;

            //循环跟节点
            foreach (TreeNode tnTemp in tv_StaffInfo.Nodes)
            {
                if (tnTemp.Checked == true)//选中的项
                {
                    if (tnTemp.Nodes.Count > 0)
                    {
                        num = false;
                    }
                    else
                    {
                        StaffInfo_Name += tnTemp.Text + ",";
                        num = false;
                    }
                }

                //循环子节点
                foreach (TreeNode tnTemps in tnTemp.Nodes)
                {
                    if (tnTemps.Checked == true)//选中的项
                    {
                        StaffInfo_Name += tnTemp.Text + ",";
                        StaffInfo_ID += tnTemps.Tag + ",";
                        num = true;
                    }
                }
                if (tnTemp.Checked == true)//选中的项
                {
                    if (num == false)
                    {
                        StaffInfo_ID += tnTemp.Tag + ",";
                    }
                }
            }
            tv_StaffInfo.Nodes.Clear();
            this.Visible = false;
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
        /// 选中父节点，子节点全选
        /// </summary>
        /// <param name="tn"></param>
        /// <param name="Checked"></param>
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

        /// <summary>
        /// 选中或取消复选框时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tv_StaffInfo_AfterCheck(object sender, TreeViewEventArgs e)
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

                CommonalityEntity.WriteTextLog("ManagementStrategyForm.tv_ControlInfo_AfterCheck()" );
            }
        }

        /// <summary>
        /// 单击文本框时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtJSYName_Click(object sender, EventArgs e)
        {
            StaffInfo_Select();
        }

       
        private void txtJSYName_TextChanged(object sender, EventArgs e)
        {
            StaffInfo_Select();
        }


    }
}
