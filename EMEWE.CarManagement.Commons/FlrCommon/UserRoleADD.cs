using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagementDAL;
using EMEWE.CarManagement.Commons.CommonClass;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.Commons.FlrCommon
{
    public partial class UserRoleADD : Form
    {
        public UserRoleADD()
        {
            InitializeComponent();
        }
        //public bool rbool;//true：添加  false:修改
        public int RoleID;//角色ID
        ArrayList arraylist = new ArrayList();
        private void UserRoleADD_Load(object sender, EventArgs e)
        {
            InitUser();
        }
        /// <summary>
        /// 加载用户
        /// </summary>
        private void InitUser()
        {
            InitMenu();
            if (!CommonalityEntity.YesNoBoolRoleUser)
            {
                btnUpdateRole();//角色绑定权限
            }
            else
            {
                btnUpdateUser();//用户绑定权限
            }
        }

        protected void treeViewPositionlist(TreeNodeCollection node, string MtID)
        {
            ///获取当前拥有的权限
            DataTable table;
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {
                table = LinQBaseDao.Query("select * from MenuInfo where Menu_OtherID=" + MtID + "and Menu_Type=1").Tables[0];
            }
            else
                table = LinQBaseDao.Query("select distinct(Permissions_Menu_ID),Menu_ID,Menu_ControlText,menu_Order  from View_MenuInfo_P where Menu_Type=1 and (Permissions_UserId=" + CommonalityEntity.USERID + " or Permissions_Role_Id=" + CommonalityEntity.USERRoleID + ") and  Menu_FromName !='PCountSetForm'  and Menu_OtherID=" + MtID).Tables[0];
            TreeNode nodeTemp;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                nodeTemp = new TreeNode();
                nodeTemp.Tag = table.Rows[i]["Menu_ID"];
                if (table.Rows[i]["Menu_ControlText"] != null)
                {
                    nodeTemp.Text = table.Rows[i]["Menu_ControlText"].ToString();
                }
                if (nodeTemp != null)
                {
                    node.Add(nodeTemp);  //加入节点 
                }
                this.treeViewPositionlist(nodeTemp.Nodes, nodeTemp.Tag.ToString().Split(',')[0]);
            }
        }

        /// <summary>
        /// 初始化菜单
        /// </summary>
        protected void InitMenu()
        {
            //加载TreeView菜单   
            treeViewPositionlist(tv_MenuInfo.Nodes, "0");
        }

        #region 选中父级菜单该菜单下的所有子级菜单自动选中

        private void tv_MenuInfo_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
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

        /// <summary>
        /// 查出选中的父级ID
        /// </summary>
        private void add()
        {
            if (tv_MenuInfo != null)
            {
                foreach (TreeNode tnTemp in tv_MenuInfo.Nodes)
                {

                    if (tnTemp.Checked == true)
                    {
                        arraylist.Add(tnTemp.Tag);
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
            if (tn != null)
            {
                foreach (TreeNode tnTemp in tn.Nodes)
                {

                    if (tnTemp.Checked == true)
                    {
                        arraylist.Add(tnTemp.Tag);
                    }
                    addDiGui(tnTemp);

                }
            }
        }
        /// <summary>
        /// 确认添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            btnUpdate_Click();
            CommonalityEntity.WriteLogData("修改", "用户编号为" + CommonalityEntity.USERID + "权限信息", CommonalityEntity.USERNAME); //操作日志
        }
        private void btnUpdate_Click()
        {
            try
            {
                arraylist.Clear();//清空动态数组内的成员
                add();
                int j = 0;
                bool Menu_Enabled = true;
                bool Menu_Visible = true;
                if (!chb_Menu_Visible.Checked)
                {
                    Menu_Visible = false;
                }
                if (!chb_Menu_Enabled.Checked)
                {
                    Menu_Enabled = false;
                }
                if (CommonalityEntity.YesNoBoolRoleUser)
                {
                    //删除用户权限，只能删除我拥有的，已经配置的权限
                    if (CommonalityEntity.UserID <= 0) return;

                    DataTable dt = LinQBaseDao.Query("select Permissions_ID from View_MenuInfo_P where Menu_Type=1 and Permissions_UserId=" + CommonalityEntity.UserID + "").Tables[0];
                    string pids = "";
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            pids += dt.Rows[i][0].ToString()+",";
                        }
                        LinQBaseDao.Query("delete PermissionsInfo where Permissions_ID in (" + pids.TrimEnd(',') + ")");
                    }

                    for (int i = 0; i < arraylist.Count; i++)
                    {
                        var permissionsInfo = new PermissionsInfo
                        {
                            Permissions_Menu_ID = CommonalityEntity.GetInt(arraylist[i].ToString()),
                            Permissions_Visible = Menu_Visible,
                            Permissions_Enabled = Menu_Enabled,
                            Permissions_Type = 1,//1为winfrom程序菜单
                            Permissions_UserId = CommonalityEntity.UserID
                        };
                        if (!PermissionsInfoDAL.InsertOneQCRecord(permissionsInfo))//是否添加失败
                        {
                            j++;
                        }
                    }
                }
                else
                {
                    //删除用户的所有权限
                    Expression<Func<PermissionsInfo, bool>> funmenuinfo = n => n.Permissions_Role_Id == CommonalityEntity.RoleID;
                    if (!PermissionsInfoDAL.DeleteToMany(funmenuinfo))//用户权限菜单是否删除失败
                    {
                        j++;
                    }
                    for (int i = 0; i < arraylist.Count; i++)
                    {
                        var permissionsInfo = new PermissionsInfo
                        {
                            Permissions_Menu_ID = CommonalityEntity.GetInt(arraylist[i].ToString()),
                            //Dictionary_State = DictionaryDAL.GetDictionaryID("启动"),
                            Permissions_Visible = Menu_Visible,
                            Permissions_Enabled = Menu_Enabled,
                            Permissions_Type = 1,
                            Permissions_Role_Id = CommonalityEntity.RoleID
                        };
                        if (!PermissionsInfoDAL.InsertOneQCRecord(permissionsInfo))//是否添加失败
                        {
                            j++;
                        }
                    }
                }
                if (j == 0)
                {
                    MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CommonalityEntity.WriteLogData("新增", "新增角色编号为：" + CommonalityEntity.RoleID + "权限的信息", CommonalityEntity.USERNAME);
                }
                else
                {
                    MessageBox.Show("添加失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch 
            {
                MessageBox.Show("添加失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CommonalityEntity.WriteTextLog("UserRoleADD.btnUpdate_Click()" );
            }
            finally
            {
                this.Close();
            }
            try
            {
                arraylist.Clear();//清空动态数组内的成员
                add();
                CommonalityEntity.arraylist = arraylist;
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("UserRoleADD.btnUpdate_Click()" );
            }
            finally
            {
                this.Close();
            }
        }
        /// <summary>
        /// 根据角色ID查数据
        /// </summary>
        private void btnUpdateRole()
        {
            if (CommonalityEntity.RoleID > 0)
            {
                Expression<Func<PermissionsInfo, bool>> funPermissionsInfo = n => n.Permissions_Role_Id == CommonalityEntity.RoleID;
                var permissionsInfo = PermissionsInfoDAL.Query(funPermissionsInfo);//根据角色ID查出菜单ID
                if (permissionsInfo == null) return;//没有数据跳出方法
                foreach (var m in permissionsInfo)
                {
                    if (m.Permissions_Menu_ID.Value > 0)
                    {
                        UpdateSelectRole(m.Permissions_Menu_ID.Value);
                    }
                }

            }
        }
        /// <summary>
        /// 根据用户ID查数据
        /// </summary>
        private void btnUpdateUser()
        {
            if (CommonalityEntity.UserID > 0 || CommonalityEntity.RoleID > 0)
            {
                Expression<Func<PermissionsInfo, bool>> funPermissionsInfo = n => n.Permissions_UserId == CommonalityEntity.UserID || n.Permissions_Role_Id == CommonalityEntity.RoleID;
                var permissionsInfo = PermissionsInfoDAL.Query(funPermissionsInfo);//根据用户ID查出菜单ID
                if (permissionsInfo == null) return;//没有数据跳出方法
                foreach (var m in permissionsInfo)
                {
                    if (m.Permissions_Menu_ID.Value > 0)
                    {
                        UpdateSelectRole(m.Permissions_Menu_ID.Value);
                    }
                }
            }
        }
        /// <summary>
        /// 遍历一级菜单
        /// </summary>
        /// <param name="Menu_ID"></param>
        private void UpdateSelectRole(int Menu_ID)
        {
            foreach (TreeNode tnTemp in tv_MenuInfo.Nodes)
            {
                if (tnTemp.Tag != null)
                {
                    if (CommonalityEntity.GetInt(tnTemp.Tag.ToString()) == Menu_ID)
                    {

                        tnTemp.Checked = true;
                        tnTemp.ExpandAll();//展开所有子节点
                    }
                    else
                    {
                        UpdateSelectRoleDiGui(tnTemp, Menu_ID);
                    }
                }
            }
        }
        /// <summary>
        /// 递归出有权限的菜单并选中
        /// </summary>
        /// <param name="tn"></param>
        /// <param name="Menu_ID"></param>
        private void UpdateSelectRoleDiGui(TreeNode tn, int Menu_ID)
        {
            foreach (TreeNode tnTemp in tn.Nodes)
            {
                if (tnTemp.Tag != null)
                {
                    if (int.Parse(tnTemp.Tag.ToString()) == Menu_ID)
                    {
                        tnTemp.Checked = true;
                        tnTemp.ExpandAll();//展开所有子节点
                    }
                    else
                    {
                        UpdateSelectRoleDiGui(tnTemp, Menu_ID);
                    }
                }
            }
        }
        /// <summary>
        /// 取消修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancle_Click(object sender, EventArgs e)
        {

            this.Close();
        }



        private void chb_SelectAll_Click(object sender, EventArgs e)
        {
            if (chb_SelectAll.Checked)
            {
                SelectAll(true);
            }
            else
            {
                SelectAll(false);
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        private void SelectAll(bool chrbool)
        {
            foreach (TreeNode tnTemp in tv_MenuInfo.Nodes)
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
    }
}
