using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagementDAL;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace EMEWE.CarManagement.Commons.FlrCommon
{
    public partial class UserManagement : Form
    {
        public int UserIDSearch;

        public UserManagement()
        {
            InitializeComponent();
        }
        private string sqlwhere;
        PageControl mf = new PageControl();
        Expression<Func<UserInfo, bool>> expr = null;
        PageControl page = new PageControl();
        private void UserManagement_Load(object sender, EventArgs e)
        {
            userContext();
            InitUser();
            btnModify.Enabled = false;
            btnAdd.Enabled = true;
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
                btnModify.Enabled = true;
                btnModify.Visible = true;
                btnDel.Enabled = true;
                btnDel.Visible = true;
                toolStripLabel1.Enabled = true;
                toolStripLabel1.Visible = true;
                btnRestPW.Enabled = true;
                btnRestPW.Visible = true;
            }
            else
            {
                btnAdd.Visible = ControlAttributes.BoolControl("btnAdd", "UserManagement", "Visible");
                btnAdd.Enabled = ControlAttributes.BoolControl("btnAdd", "UserManagement", "Enabled");

                btnModify.Visible = ControlAttributes.BoolControl("btnModify", "UserManagement", "Visible");
                btnModify.Enabled = ControlAttributes.BoolControl("btnModify", "UserManagement", "Enabled");

                btnDel.Visible = ControlAttributes.BoolControl("btnDel", "UserManagement", "Visible");
                btnDel.Enabled = ControlAttributes.BoolControl("btnDel", "UserManagement", "Enabled");

                toolStripLabel1.Visible = ControlAttributes.BoolControl("toolStripLabel1", "UserManagement", "Visible");
                toolStripLabel1.Enabled = ControlAttributes.BoolControl("toolStripLabel1", "UserManagement", "Enabled");

                btnRestPW.Visible = ControlAttributes.BoolControl("btnRestPW", "UserManagement", "Visible");
                btnRestPW.Enabled = ControlAttributes.BoolControl("btnRestPW", "UserManagement", "Enabled");
            }
        }
        /// <summary>
        /// 加载用户
        /// </summary>
        private void InitUser()
        {
            expr = PredicateExtensionses.True<UserInfo>();
            expr = expr.And(n => n.UserLoginId != "emewe");
            this.lvwUserList.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
            tscbxPageSize.SelectedIndex = 2;
            //LoadData();
            cmbRoleUserDate();
        }

        /// <summary>
        /// 绑定角色
        /// </summary>
        private void cmbRoleUserDate()
        {
            DataTable dt = LinQBaseDao.Query("select * from  RoleInfo").Tables[0];
            if (dt.Rows.Count > 0)
            {
                cmbRoleUser.DataSource = dt;
                cmbRoleUser.DisplayMember = "Role_Name";
                cmbRoleUser.ValueMember = "Role_Id";
                cmbRoleUser.SelectedIndex = 0;

            }
        }

        private void UserClick()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {
                sqlwhere = "UserLoginId<>'emewe'";
            }
            else
                sqlwhere = "UserLoginId<>'admin' and  UserLoginId<>'emewe'";
        }

        /// <summary>
        /// 菜单栏加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.lvwUserList.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
                this.lvwUserList.DataSource = null;
                LogInfoLoad("");
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("用户管理 LoadData()" );
            }
        }

        /// <summary>
        /// 加载信息
        /// </summary>
        public void LogInfoLoad(string strClickedItemName)
        {
            if (sqlwhere == null)
            {
                UserClick();
            }
            page.BindBoundControl(lvwUserList, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "UserInfo_RoleInfo", "*", "UserId", "UserId", 0, sqlwhere, true);
        }

        //取消修改
        private void btnUserCancle_Click(object sender, EventArgs e)
        {
            txtUserLoginID.Enabled = true;
            btnModify.Enabled = false;
            btnAdd.Enabled = true;
            btnUserCancle.Text = "清    空";
            ShowAddButton();
        }

        private void ShowAddButton()
        {
            txtUserLoginID.Text = "";//登陆ID
            txtPassword.Text = "";  //登陆密码
            txtName.Text = "";          //姓名
            txtPhone.Text = "";        //电话
            txtAddress.Text = "";    //地址
            txtRemark.Text = "";    //备注信息
            cmbUserState.Text = "";
            label3.Visible = true;
            cmbRoleUser.Visible = true;
        }

        //添加
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Enabled)
            {
                btnAdd.Enabled = false;
                btnAdd_Click();
                btnAdd.Enabled = true;
            }
        }

        #region --添加用户，验证用户--
        private void btnAdd_Click()
        {
            try
            {
                if (cmbUserState.SelectedItem == null)
                {
                    MessageBox.Show("请选择状态！");
                    return;
                }
                int j = 0;
                int rint = 0;
                if (!btnCheck()) return;
                var useradd = new UserInfo
                {
                    UserLoginId = txtUserLoginID.Text.Trim(),//登陆ID
                    UserLoginPwd = CommonalityEntity.EncryptDES(txtPassword.Text.Trim()),  //登陆密码 加密
                    UserName = txtName.Text.Trim(),          //姓名
                    UserPhone = txtPhone.Text.Trim(),        //电话
                    UserAddress = txtAddress.Text.Trim(),    //地址
                    UserCreateTime = DateTime.Now,           //创建时间
                    UserCreateName = CommonalityEntity.USERNAME,         //创建人
                    UserRemark = txtRemark.Text.Trim(),     //备注信息
                    UserSate = cmbUserState.SelectedItem.ToString(),
                    UserRoleID = Convert.ToInt32(cmbRoleUser.SelectedValue),
                };

                if (UserInfoDAL.InsertOneQCRecord(useradd, out rint))
                {
                    MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                string Log_Content = String.Format("用户名称：{0} ", txtUserLoginID.Text.Trim());
                CommonalityEntity.WriteLogData("新增", "新增" + Log_Content + "的权限", CommonalityEntity.USERNAME);//添加日志
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("用户管理 btnAdd_Click()" + "".ToString());
                MessageBox.Show("添加失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                page = new PageControl();
                //page.PageMaxCount = tscbxPageSize2.SelectedItem.ToString();
                LoadData();
            }
        }

        private bool btnCheck()
        {

            bool rbool = true;
            try
            {

                var LoginId = txtUserLoginID.Text.Trim();
                if (LoginId == "")
                {
                    mf.ShowToolTip(ToolTipIcon.Info, "提示", "登录名不能为空！", txtUserLoginID, this);
                    txtUserLoginID.Text = "";
                    txtUserLoginID.Focus();
                    rbool = false;
                }
                Expression<Func<UserInfo, bool>> funview_userinfo = n => n.UserLoginId == LoginId;
                if (UserInfoDAL.Query(funview_userinfo).Count() > 0)
                {
                    mf.ShowToolTip(ToolTipIcon.Info, "提示", "该用户名已存在", txtUserLoginID, this);
                    txtUserLoginID.Focus();
                    rbool = false; ;
                }
                return rbool;
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("用户管理 btnCheck()");
                rbool = false;
            }
            return rbool;
        }
        #endregion

        /// <summary>
        /// 修改
        /// </summary>
        private void btnModify_Click(object sender, EventArgs e)
        {
            if (txtUserLoginID.Enabled == false)
            {
                ModifyUser_Click();
                btnModify.Enabled = false;
                txtUserLoginID.Enabled = true;
                btnModify.Enabled = true;
                btnUserCancle.Text = "清    空";
            }
            else
                MessageBox.Show("请双击列表中的数据修改！");
        }

        private void ModifyUser_Click()
        {
            try
            {
                Expression<Func<UserInfo, bool>> p = n => n.UserLoginId == txtUserLoginID.Text.Trim();
                Action<UserInfo> ap = n =>
                {
                    n.UserLoginId = txtUserLoginID.Text.Trim();//登陆ID
                    n.UserLoginPwd = CommonalityEntity.EncryptDES(txtPassword.Text.Trim());  //登陆密码
                    n.UserName = txtName.Text.Trim();          //姓名
                    n.UserPhone = txtPhone.Text.Trim();        //电话
                    n.UserAddress = txtAddress.Text.Trim();    //地址
                    n.UserCreateTime = DateTime.Now;           //创建时间
                    n.UserCreateName = CommonalityEntity.USERNAME;         //创建人
                    n.UserRemark = txtRemark.Text.Trim();     //备注信息
                    n.UserSate = cmbUserState.SelectedItem.ToString();
                    if (txtUserLoginID.Text.Trim() != "emewe")
                    {
                        n.UserRoleID = Convert.ToInt32(cmbRoleUser.SelectedValue);
                    }
                };
                if (!UserInfoDAL.Update(p, ap))//用户是否修改失败
                {
                    MessageBox.Show(" 修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                string Log_Content = String.Format("用户名称：{0} ", txtUserLoginID.Text.Trim());
                //common.WriteLogData("修改", Log_Content, common.USERNAME);//添加日志
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("用户管理 bntUpUser_Click()" );
            }
            finally
            {
                page = new PageControl();
                //page.PageMaxCount = tscbxPageSize2.SelectedItem.ToString();
                LoadData();
                ShowAddButton();
            }
        }
        /// <summary>
        /// 查重
        /// </summary>
        private void btn_Check_Click(object sender, EventArgs e)
        {
            if (btnCheck())
            {
                mf.ShowToolTip(ToolTipIcon.Info, "提示", "该用户名可用", txtUserLoginID, this);
            }
        }


        #region 搜索
        /// <summary>
        /// 搜索
        /// </summary>
        private void selectTJ()
        {
            UserClick();
            try
            {
                if (txtSearchUser.Text.Trim() != "")//用户名
                {
                    sqlwhere += "and UserLoginId like '%" + txtSearchUser.Text.Trim() + "%'";
                }
                if (txt_Name.Text.Trim() != "")//真实姓名
                {
                    sqlwhere += "and UserName like '%" + txt_Name.Text.Trim() + "%'";
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("用户管理 selectTJ()" );
            }
            finally
            {
                page = new PageControl();
                LogInfoLoad("");
                LoadData();
            }
        }

        ///条件查询
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (btnSearch.Enabled)
            {
                btnSearch.Enabled = false;
                selectTJ();
                btnSearch.Enabled = true;
            }

        }
        #endregion

        /// <summary>
        /// 取消全选
        /// </summary>
        private void tslNotSelect_Click()
        {

            for (int i = 0; i < lvwUserList.Rows.Count; i++)
            {
                //((DataGridViewCheckBoxCell)dgvRoleList.Rows[i].Cells[0]).Value = false;
                lvwUserList.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>     
        private void tslSelectAll_Click()
        {
            for (int i = 0; i < lvwUserList.Rows.Count; i++)
            {
                //((DataGridViewCheckBoxCell)lvwUserList.Rows[i].Cells[0]).Value = true;
                lvwUserList.Rows[i].Selected = true;
            }
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
                            Expression<Func<UserInfo, bool>> funuserinfo = n => n.UserId == Convert.ToInt32(lvwUserList.SelectedRows[i].Cells["UserID"].Value);
                            if (!UserInfoDAL.DeleteToMany(funuserinfo))
                            {
                                j++;
                            }
                            else
                            {
                                CommonalityEntity.WriteLogData("删除", "删除用户名为：" + lvwUserList.SelectedRows[i].Cells["UserLoginId"].Value + "的信息", CommonalityEntity.USERNAME);//添加日志
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

                CommonalityEntity.WriteTextLog("用户管理 tbtnDelUser_delete()+" );
            }
            finally
            {
                page = new PageControl();
                LoadData();//更新
            }
        }

        /// <summary>
        /// 设置每页显示最大条数事件
        /// </summary>
        private void tscbxPageSize2_SelectedIndexChanged(object sender, EventArgs e)
        {
            page = new PageControl();
            LoadData();
        }

        /// <summary>
        /// 用户权限配置
        /// </summary>
        private void lb_UserRole_Click(object sender, EventArgs e)
        {
            UserRoleADD uradd = new UserRoleADD();
            CommonalityEntity.rbools = true;
            CommonalityEntity.YesNoBoolRoleUser = true;
            uradd.ShowDialog();
        }

        /// <summary>
        /// 权限设置
        /// </summary>
        /// <param name="?"></param>
        private void tsl_Role_Click()
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
                        if (lvwUserList.SelectedRows[0].Cells["Userid"].Value == null) return;
                        if (lvwUserList.SelectedRows[0].Cells["Role_Id"].Value == null) return;

                        if (Convert.ToInt32(lvwUserList.SelectedRows[0].Cells["Userid"].Value.ToString()) > 0)
                        {
                            UserRoleADD uradd = new UserRoleADD();
                            CommonalityEntity.YesNoBoolRoleUser = true;
                            CommonalityEntity.rbools = false;
                            //common.UserID = common.GetInt(lvwUserList.SelectedRows[0].Cells["Userid"].Value.ToString());
                            //common.RoleID = common.GetInt(lvwUserList.SelectedRows[0].Cells["Role_Id"].Value.ToString());
                            //mf.ShowChildForm(uradd);
                            //uradd.ShowDialog();
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
                CommonalityEntity.WriteTextLog("UserRole.tsl_Role_Click()" );
            }
        }

        private void lvwUserList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        private void lvwUserList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            //if (this.lvwUserList.Rows.Count != 0)
            //{
            //    for (int i = 0; i < this.lvwUserList.Rows.Count; i++)
            //    {
            //        if (i % 2 == 0)
            //        {
            //            this.lvwUserList.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Lime;
            //        }
            //        else
            //        {
            //            this.lvwUserList.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Gold;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 鼠标双击事件，获取双击项的信息，提供修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvwUserList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            txtUserLoginID.Enabled = false;
            btnModify.Enabled = true;
            btnAdd.Enabled = false;
            if (this.lvwUserList.SelectedRows.Count > 0)//选中行
            {
                if (lvwUserList.SelectedRows.Count > 1)
                {
                    MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    btnUserCancle.Text = "取消修改";
                    //修改的值
                    int ID = int.Parse(this.lvwUserList.SelectedRows[0].Cells["UserID"].Value.ToString());
                    Expression<Func<UserInfo, bool>> funviewinto = n => n.UserId == ID;
                    foreach (var n in UserInfoDAL.Query(funviewinto))
                    {
                        if (n.UserId != 0)
                        {
                            this.txtUserLoginID.Text = n.UserLoginId;
                        }
                        if (n.UserName != null)
                        {
                            // IC卡类型状态
                            this.txtName.Text = n.UserName;
                        }
                        if (n.UserPhone != null)
                        {
                            // IC卡类型权限
                            this.txtPhone.Text = n.UserPhone;
                        }
                        if (n.UserAddress != null)
                        {
                            // IC卡类型描述
                            this.txtAddress.Text = n.UserAddress;
                        }
                        if (n.UserRemark != null)
                        {
                            // IC卡类型备注
                            this.txtRemark.Text = n.UserRemark;
                        }
                        if (n.UserSate != null)
                        {
                            this.cmbUserState.Text = n.UserSate;
                        }
                        if (n.UserRoleID != null)
                        {
                            this.cmbRoleUser.SelectedValue = n.UserRoleID;
                        }
                        if (n.UserRoleID != null)
                        {
                            this.cmbRoleUser.SelectedValue = n.UserRoleID;
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

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvwUserList.SelectedRows.Count > 0)//选择行
                {
                    if (UserIDSearch == 0) return;
                    if (UserIDSearch > 0)
                    {
                        if (UserIDSearch == CommonalityEntity.USERID)
                        {
                            MessageBox.Show("不能给自己配置权限！"); return;
                        }
                        UserRoleADD uradd = new UserRoleADD();
                        CommonalityEntity.YesNoBoolRoleUser = true;
                        CommonalityEntity.rbools = false;
                        CommonalityEntity.UserID = UserIDSearch;
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
                CommonalityEntity.WriteTextLog("UserRole.tsl_Role_Click()" );
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
                    UserIDSearch = int.Parse(this.lvwUserList.SelectedRows[0].Cells["UserID"].Value.ToString());
                    DataTable dt = LinQBaseDao.Query("select * from RoleInfo where  Role_Name='" + this.lvwUserList.SelectedRows[0].Cells["Role_Name"].Value.ToString() + "'").Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        CommonalityEntity.RoleID = Convert.ToInt32(dt.Rows[0][0].ToString());
                    }
                    else
                    {
                        CommonalityEntity.RoleID = 0;
                    }
                }
            }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            tbtnDelUser_delete();
        }

        private void btnRestPW_Click(object sender, EventArgs e)
        {
            try
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
                        int uid = int.Parse(this.lvwUserList.SelectedRows[0].Cells["UserID"].Value.ToString());
                        LinQBaseDao.Query(" update UserInfo  set UserLoginPwd='vHQ5bmcYPhc=' where UserID=" + uid);
                        MessageBox.Show(this, "重置密码成功");
                    }
                }
            }
            catch 
            {

                return;
            }
        }

    }
}