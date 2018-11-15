using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EMEWE.CarManagement.Commons.FlrCommon;
using System.Linq.Expressions;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagementDAL;
using System.Data.Linq.SqlClient;
using EMEWE.CarManagement.Commons;
using EMEWE.CarManagement.Commons.CommonClass;

namespace EMEWE.CarManagement.Commons.FlrCommon
{
    public partial class UserRole : Form
    {
        public UserRole()
        {
            InitializeComponent();
        }
        private string sqlwhere = "1=1";
        public int UserIDSearch;
        UserRoleADD uradd = new UserRoleADD();
        Expression<Func<RoleInfo, bool>> expr = null;
        PageControl page = new PageControl();       //分页控件

        /// <summary>
        /// 用户信息加载
        /// </summary>
        private void InitUser()
        {
            tscbxPageSize.SelectedIndex = 2;
            this.lvwUserList.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
            LoadData();
        }

        /// <summary>
        /// 菜单栏加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.lvwUserList.DataSource = null;
                LogInfoLoad("");
            }
            catch
            {
                CommonalityEntity.WriteTextLog("用户管理 LoadData()");
            }
        }

        /// <summary>
        /// 加载信息
        /// </summary>
        public void LogInfoLoad(string strClickedItemName)
        {
            page.BindBoundControl(lvwUserList, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "RoleInfo", "*", "Role_Id", "Role_Id", 0, sqlwhere, true);
        }

        /// <summary>
        /// Search
        /// </summary>
        private void SelectShere()
        {
            sqlwhere = "1=1";
            try
            {
                string SearchName = txtSearchName.Text.Trim();
                if (SearchName != "")//根据角色名查询
                {
                    sqlwhere += String.Format(" and Role_Name like '%{0}%'", SearchName);
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("UserRole.SelectShere()");
            }
            finally
            {
                page = new PageControl();
                LogInfoLoad("");
            }
        }

        /// <summary>
        /// 查重方法
        /// </summary>
        private bool btnCheck()
        {
            bool rbool = true;
            try
            {

                var RoleNmae = txtRoleName.Text.Trim();
                if (RoleNmae == "")
                {
                    txtRoleName.Text = "";
                    txtRoleName.Focus();
                    rbool = false;
                }
                DataTable dt = LinQBaseDao.Query("select * from RoleInfo where Role_Name='" + RoleNmae + "'").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    rbool = false;
                }
                return rbool;
            }
            catch
            {
                CommonalityEntity.WriteTextLog("角色管理 btnCheck()");
                rbool = false;
            }
            return rbool;
        }

        private void UserRole_Load(object sender, EventArgs e)
        {
            userContext();
            InitUser();
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
                btnUpdate.Visible = true;
                btnUpdate.Enabled = true;
                toolStripLabel1.Enabled = true;
                toolStripLabel1.Visible = true;
            }
            else
            {
                btnAdd.Visible = ControlAttributes.BoolControl("btnAdd", "UserRole", "Visible");
                btnAdd.Enabled = ControlAttributes.BoolControl("btnAdd", "UserRole", "Enabled");

                btnDel.Visible = ControlAttributes.BoolControl("btnDel", "UserRole", "Visible");
                btnDel.Enabled = ControlAttributes.BoolControl("btnDel", "UserRole", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "UserRole", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "UserRole", "Enabled");

                toolStripLabel1.Enabled = ControlAttributes.BoolControl("toolStripLabel1", "UserRole", "Visible");
                toolStripLabel1.Visible = ControlAttributes.BoolControl("toolStripLabel1", "UserRole", "Enabled");
            }
        }

        /// <summary>
        /// 修改事件
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtRoleName.Text))
            {
                MessageBox.Show(this, "角色名称不能为空！");
                return;
            }
            if (comboxState.SelectedIndex < 0 )
            {
                MessageBox.Show(this, "请选择状态！");
                return;
            }
            string name = txtRoleName.Text; //姓名  
            string remark = txtRemark.Text; //备注
            string state = comboxState.SelectedItem.ToString();//状态
            string id = lblId.Text;         //id
            int j = 0;
            Action<RoleInfo> action = n =>
            {
                n.Role_Name = name;
                n.Role_Remark = remark;
                n.Role_State = state;
                n.Role_Permission = "";
            };
            try
            {
                Expression<Func<RoleInfo, bool>> funroleinfo = n => n.Role_Id.ToString() == id;
                if (!RoleInfoAdd.UpdateOneRoleInfo(funroleinfo, action))//角色是否修改失败
                {
                    j++;
                }
                if (j == 0)
                {
                    MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string Log_Content = String.Format("角色名称：{0}", name);
                    CommonalityEntity.WriteLogData("修改", "修改" + Log_Content + "的信息", CommonalityEntity.USERNAME);
                }
                else
                {
                    MessageBox.Show(" 修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("角色管理 btnUpdate_Click");
            }
            finally
            {
                page = new PageControl();
                LoadData();
                userContext();
                lblId.Text = "";
                txtRemark.Text = "";
                txtRoleName.Text = "";
                comboxState.Text = "";
            }
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            #region --验证用户名是否重复--
            DataTable table = LinQBaseDao.Query(" SELECT * FROM [RoleInfo] where Role_Name='" + txtRoleName.Text.Trim() + "'").Tables[0];
            if (table.Rows.Count > 0)
            {
                MessageBox.Show("角色名称已存在！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (comboxState.SelectedItem == null)
            {
                MessageBox.Show("请选择状态！");
                return;
            }

            #endregion
            int j = 0;
            int rint = 0;

            if (string.IsNullOrEmpty(txtRoleName.Text))
            {
                MessageBox.Show(this, "角色名称不能为空！");
                return;
            }
            if (comboxState.SelectedIndex < 0)
            {
                MessageBox.Show(this, "请选择状态！");
                return;
            }
            string name = txtRoleName.Text; //角色名
            string remark = txtRemark.Text; //备注
            try
            {
                if (!btnCheck()) return;
                var rf = new RoleInfo
                    {
                        Role_Name = name,
                        Role_Remark = remark,
                        Role_State = comboxState.SelectedItem.ToString(),
                        Role_Permission = ""
                    };
                if (!RoleInfoAdd.InsertOneRoleInfo(rf, out rint))
                {
                    j++;
                }

                if (j == 0)
                {
                    MessageBox.Show("成功增加", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string Log_Content = String.Format("角色名称：{0}", name);
                    CommonalityEntity.WriteLogData("新增", "新增" + Log_Content + "的信息", CommonalityEntity.USERNAME);
                }
                else
                {
                    MessageBox.Show("成功失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("角色管理 btnAdd_Click()");
            }
            finally
            {
                page = new PageControl();
                LoadData();//更新数据
            }
        }

        /// <summary>
        /// 取消事件
        /// </summary>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            btnAdd.Visible = true;
            txtRoleName.Text = "";
            comboxState.Text = "";
            txtRemark.Text = "";
            btnUpdate.Enabled = false;
        }

        /// <summary>
        /// 搜索任务信息
        /// </summary>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            SelectShere();
        }

        /// <summary>
        /// 全选
        /// </summary>
        private void tslSelectAll_Click()
        {
            for (int i = 0; i < lvwUserList.Rows.Count; i++)
            {
                lvwUserList.Rows[i].Selected = true;
            }
        }
        /// <summary>
        /// 取消全选
        /// </summary>
        private void tslNotSelect_Click()
        {
            for (int i = 0; i < lvwUserList.Rows.Count; i++)
            {
                lvwUserList.Rows[i].Selected = false;
            }
        }

        private void tsl_Role_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvwUserList.SelectedRows.Count > 0)//选择行
                {
                    if (lvwUserList.SelectedRows.Count > 1)
                    {
                        MessageBox.Show("只能选择一行进行权限设置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (lvwUserList.SelectedRows[0].Cells["Role_Id"].Value == null) return;

                        if (int.Parse(lvwUserList.SelectedRows[0].Cells["Role_Id"].Value.ToString()) > 0)
                        {
                            UserRoleADD uradd = new UserRoleADD();
                            CommonalityEntity.YesNoBoolRoleUser = false;
                            CommonalityEntity.rbools = false;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请选择行", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("UserRole.tsl_Role_Click()");
            }
        }


        private void tscbxPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogInfoLoad("");
        }

        private void bindingNavigator1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

            if (e.ClickedItem.Name == "tslSelectAll")//全选
            {
                if (tslSelectAll.Enabled)
                {
                    tslSelectAll.Enabled = false;
                    tslSelectAll_Click();
                    tslSelectAll.Enabled = true;
                }
                return;
            }
            if (e.ClickedItem.Name == "tslNotSelect")//取消全选
            {
                if (tslNotSelect.Enabled)
                {
                    tslNotSelect.Enabled = false;
                    tslNotSelect_Click();
                    tslNotSelect.Enabled = true;
                }
                return;
            }
            LogInfoLoad(e.ClickedItem.Name);

        }

        /// <summary>
        ///删除用户信息 
        /// </summary>
        private void tbtnDelUser_delete()
        {
            try
            {
                int j = 0;
                UserIDSearch = lvwUserList.SelectedRows.Count;
                if (UserIDSearch > 0)//选中删除
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //选中数量
                        //遍历
                        for (int i = 0; i < UserIDSearch; i++)
                        {

                            Expression<Func<RoleInfo, bool>> funuserinfo = n => n.Role_Id == Convert.ToInt32(lvwUserList.SelectedRows[i].Cells["Role_Id"].Value); ;

                            if (!RoleInfoAdd.DeleteToMany(funuserinfo))
                            {
                                j++;
                            }
                            else
                            {
                                CommonalityEntity.WriteLogData("删除", "删除角色名称为：" + lvwUserList.SelectedRows[i].Cells["Role_Name"].Value + "的信息", CommonalityEntity.USERNAME);
                            }
                        }
                        if (j == 0)
                        {
                            MessageBox.Show("成功删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        else
                        {
                            MessageBox.Show("删除失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else//没有选中
                {
                    MessageBox.Show("请选择要删除的行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("用户管理 tbtnDelUser_delete()+");
            }
            finally
            {
                page = new PageControl();
                //page.PageMaxCount = tscbxPageSize2.SelectedItem.ToString();
                LoadData();//更新
            }
        }

        private void ModifyUser_Click()
        {
            try
            {
                Expression<Func<RoleInfo, bool>> p = n => n.Role_Id == Convert.ToInt32(lblId.Text);
                Action<RoleInfo> ap = n =>
                {
                    n.Role_Name = txtRoleName.Text.Trim();
                    n.Role_State = comboxState.Text.Trim();
                    n.Role_Remark = txtRemark.Text.Trim();

                };
                if (!RoleInfoAdd.UpdateOneRoleInfo(p, ap))//用户是否修改失败
                {
                    MessageBox.Show(" 修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string Log_Content = String.Format("用户名称：{0} ", lblId.Text.Trim());
                    CommonalityEntity.WriteLogData("修改", "修改" + Log_Content + "的信息", CommonalityEntity.USERNAME);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("用户管理 bntUpUser_Click()");
            }
            finally
            {
                page = new PageControl();
                LoadData();
                userContext();
            }
        }
        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvwUserList.SelectedRows.Count > 0)//选择行
                {
                    if (UserIDSearch == 0) return;
                    if (UserIDSearch > 0)
                    {
                        UserRoleADD uradd = new UserRoleADD();
                        CommonalityEntity.YesNoBoolRoleUser = false;
                        CommonalityEntity.rbools = true;
                        CommonalityEntity.RoleID = UserIDSearch;
                        UserRoleADD ur = new UserRoleADD();
                        ur.ShowDialog(uradd);
                    }
                }
                else
                {
                    MessageBox.Show("请选择行", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("UserRole.tsl_Role_Click()");
            }
        }

        private void lvwUserList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.lvwUserList.Rows.Count != 0)
            {
                for (int i = 0; i < this.lvwUserList.Rows.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        this.lvwUserList.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Lime;
                    }
                    else
                    {
                        this.lvwUserList.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Gold;
                    }
                }
            }
        }

        private void lvwUserList_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.lvwUserList.SelectedRows.Count > 0)//选中行
            {
                if (lvwUserList.SelectedRows.Count > 1)
                {
                    MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //修改的值
                    UserIDSearch = int.Parse(this.lvwUserList.SelectedRows[0].Cells["Role_Id"].Value.ToString());
                }
            }
        }

        private void lvwUserList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        private void lvwUserList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            label3.Visible = false;
            btnAdd.Visible = false;
            btnUpdate.Enabled = true;
            if (this.lvwUserList.SelectedRows.Count > 0)//选中行
            {
                if (lvwUserList.SelectedRows.Count > 1)
                {
                    MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //修改的值
                    int ID = int.Parse(this.lvwUserList.SelectedRows[0].Cells["Role_Id"].Value.ToString());
                    Expression<Func<RoleInfo, bool>> funviewinto = n => n.Role_Id == ID;
                    foreach (var n in RoleInfoAdd.Query(funviewinto))
                    {
                        if (n.Role_Id != 0)
                        {
                            this.lblId.Text = n.Role_Id.ToString();
                        }
                        if (n.Role_Name != null)
                        {
                            // IC卡类型状态
                            this.txtRoleName.Text = n.Role_Name;
                        }
                        if (n.Role_State != null)
                        {
                            // IC卡类型权限
                            this.comboxState.Text = n.Role_State;
                        }
                        if (n.Role_Remark != null)
                        {
                            // IC卡类型描述
                            this.txtRemark.Text = n.Role_Remark;
                        }
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择要修改的行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            tbtnDelUser_delete();
        }
    }
}