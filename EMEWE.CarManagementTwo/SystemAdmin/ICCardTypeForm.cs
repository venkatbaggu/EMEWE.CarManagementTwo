using EMEWE.CarManagement.CommonClass;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class ICCardTypeForm : Form
    {
        public ICCardTypeForm()
        {
            InitializeComponent();
        }


        public MainForm mf;//主窗体
        /// <summary>
        /// 返回一个空的预置皮重
        /// </summary>
        Expression<Func<ICCardType,bool>> expr = null;
        /// <summary>
        /// 实例化分页控件
        /// </summary>
        PageControl page = new PageControl();
        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere;

        /// <summary>
        /// Load 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ICCardTypeForm_Load(object sender, EventArgs e)
        {
            GBPermissions.Visible = false;
            userContext();
            btnUpdate.Enabled = false;
            btnSearch_Click(btnSearch, null);  // 调用查询条件执行查询
            BindICCardType();
            BindSearchICCardType();
            this.comboxICCardType_State.Text = "启动";
            LoadData();
            mf = new MainForm();
        }
        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {

                btnSave.Enabled = true;
                btnSave.Visible = true;
                btnDel.Enabled = true;
                btnDel.Visible = true;
                btnUpdate.Enabled = true;
                btnUpdate.Visible = true;
            }
            else
            {
                btnSave.Visible = ControlAttributes.BoolControl("btnSave", "ICCardTypeForm", "Visible");
                btnSave.Enabled = ControlAttributes.BoolControl("btnSave", "ICCardTypeForm", "Enabled");

                btnDel.Visible = ControlAttributes.BoolControl("btnDel", "ICCardTypeForm", "Visible");
                btnDel.Enabled = ControlAttributes.BoolControl("btnDel", "ICCardTypeForm", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "ICCardTypeForm", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "ICCardTypeForm", "Enabled");
            }
        }
        /// <summary>
        /// 菜单栏加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.dgvICCardType.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
                this.dgvICCardType.DataSource = null;

                LogInfoLoad("");
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("日记管理 LoadData()" );
            }
        }
        /// <summary>
        /// 绑定IC卡类型状态
        /// </summary>
        private void BindICCardType()
        {
            try
            {
                this.comboxICCardType_State.DataSource = DictionaryDAL.GetValueDictionary("01").Where(n => n.Dictionary_Name != "全部").ToList();

                if (this.comboxICCardType_State.DataSource != null)
                {
                    this.comboxICCardType_State.DisplayMember = "Dictionary_Name";
                    this.comboxICCardType_State.ValueMember = "Dictionary_ID";
                    this.comboxICCardType_State.SelectedValue = -1;
                }
            }
            catch
            {
                MessageBox.Show("IC卡类型“状态”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 绑定IC卡类型状态
        /// </summary>
        private void BindSearchICCardType()
        {
            try
            {
                this.comboxICCardTypeState.DataSource = DictionaryDAL.GetValueStateDictionary("01");

                if (this.comboxICCardTypeState.DataSource != null)
                {
                    this.comboxICCardTypeState.DisplayMember = "Dictionary_Name";
                    this.comboxICCardTypeState.ValueMember = "Dictionary_ID";
                    this.comboxICCardTypeState.SelectedIndex = 3;
                }
            }
            catch
            {
                MessageBox.Show("IC卡类型“状态”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }


        /// <summary>
        /// 查重方法
        /// </summary>
        /// <returns></returns>
        private bool btnCheck()
        {
            bool rbool = true;
            try
            {
                //IC卡类型值
                string ValueName = this.txtICCardType_Value.Text.Trim();
                string ICType = this.txtICCardType_Name.Text.Trim();

                //判断名称是否已存在
                Expression<Func<ICCardType,bool>> funviewICCard = n => n.ICCardType_Name == ICType && n.ICCardType_Name != this.dgvICCardType.SelectedRows[0].Cells["ICCardType_Name"].Value.ToString();
                if (ICCardTypeDAL.Query(funviewICCard).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该IC卡类型名称已存在", txtICCardType_Name, this);
                    txtICCardType_Name.Focus();
                    rbool = false; ;
                }
                //判断名称是否已存在
                Expression<Func<ICCardType,bool>> funviewICCard1 = n => n.ICCardType_Value == ValueName && n.ICCardType_Value != this.dgvICCardType.SelectedRows[0].Cells["ICCardType_Value"].Value.ToString();
                if (ICCardTypeDAL.Query(funviewICCard1).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该IC卡类型值已存在", txtICCardType_Value, this);
                    txtICCardType_Value.Focus();
                    rbool = false; ;
                }
                return rbool;

            }
            catch 
            {
                CommonalityEntity.WriteTextLog("IC卡类型信息 btnCheck()");
                rbool = false;
            }
            return rbool;
        }

        /// <summary>
        /// “保 存” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //判断IC卡类型名称是否存在txtICCardType_Name
                if (LinQBaseDao.Query("select * from ICCardType where ICCardType_Name='" + txtICCardType_Name + "'").Tables[0].Rows.Count >= 1)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "IC类型名称已存在！", txtICCardType_Name, this);
                    return;
                }
                if (LinQBaseDao.Query("select * from ICCardType where ICCardType_Value='" + txtICCardType_Value + "'").Tables[0].Rows.Count >= 1)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "IC类型值已存在！", txtICCardType_Value, this);
                    return;
                }
                if (this.txtICCardType_Name.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "IC卡类型名称不能为空！", txtICCardType_Name, this);
                    return;
                }
                if (this.txtICCardType_Value.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "IC卡类型值不能为空！", txtICCardType_Value, this);
                    return;
                }
                if (!btnCheck()) return; // 去重复
                var ICCardTypeadd = new ICCardType
                {
                    ICCardType_Name = this.txtICCardType_Name.Text.Trim(),
                    ICCardType_State = this.comboxICCardType_State.Text.Trim(),
                    ICCardType_Value = this.txtICCardType_Value.Text.Trim(),
                    ICCardType_Description = this.txtICCardType_Description.Text.Trim(),
                    ICCardType_Remark = this.txtICCardType_Remark.Text.Trim(),
                    ICCardType_Permissions = this.ICCardPermissions.Text,
                    ICCardType_PermissionsValue = strview
                };

                if (ICCardTypeDAL.InsertOneICCardType(ICCardTypeadd))
                {
                    MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string strContent1 = "新增IC卡类型名称为： " + this.txtICCardType_Name.Text.Trim();
                    CommonalityEntity.WriteLogData("新增", strContent1, CommonalityEntity.USERNAME);//添加操作日志
                    Empty(); // 调用清空的方法
                }
                else
                {
                    MessageBox.Show("添加失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch 
            {
                CommonalityEntity.WriteTextLog("IC卡信息管理 btnSave_Click()" );
            }
            finally
            {
                LogInfoLoad("");
            }
        }

        /// <summary>
        /// “修改” 查看数据按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtICCardType_Name.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "IC卡类型名称不能为空！", txtICCardType_Name, this);
                    return;
                }
                if (this.txtICCardType_Value.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "IC卡类型值不能为空！", txtICCardType_Value, this);
                    return;
                }
                //判断IC卡类型名称是否存在txtICCardType_Name
                if (LinQBaseDao.Query("select * from ICCardType where ICCardType_Name='" + txtICCardType_Name + "'").Tables[0].Rows.Count >= 1)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "IC类型名称已存在！", txtICCardType_Name, this);
                    return;
                }
                if (LinQBaseDao.Query("select * from ICCardType where ICCardType_Value='" + txtICCardType_Value + "'").Tables[0].Rows.Count >= 1)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "IC类型值已存在！", txtICCardType_Value, this);
                    return;
                }

                if (!btnCheck()) return; // 去重复
                if (this.dgvICCardType.SelectedRows.Count > 0)//选中行
                {
                    if (dgvICCardType.SelectedRows.Count > 1)
                    {
                        MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        Expression<Func<ICCardType,bool>> p = n => n.ICCardType_ID == int.Parse(this.dgvICCardType.SelectedRows[0].Cells["ICCardType_ID"].Value.ToString());
                        string id = "";
                        string strfront = "";
                        string strContent = "";
                        Action<ICCardType> ap = s =>
                        {
                            strfront = s.ICCardType_Name + "," + s.ICCardType_State + "," + s.ICCardType_State + "," + s.ICCardType_Value + "," + s.ICCardType_Description + "," + s.ICCardType_Permissions + "," + s.ICCardType_Remark;
                            s.ICCardType_Name = this.txtICCardType_Name.Text.Trim();
                            s.ICCardType_State = this.comboxICCardType_State.Text.Trim();
                            s.ICCardType_Value = this.txtICCardType_Value.Text.Trim();
                            s.ICCardType_Description = this.txtICCardType_Description.Text.Trim();
                            if (!string.IsNullOrEmpty(this.ICCardPermissions.Text.Trim()))
                            {
                                s.ICCardType_Permissions = this.ICCardPermissions.Text.Trim();
                            }
                            s.ICCardType_Remark = this.txtICCardType_Remark.Text.Trim();
                            strContent = s.ICCardType_Name + "," + s.ICCardType_State + "," + s.ICCardType_State + "," + s.ICCardType_Value + "," + s.ICCardType_Description + "," + s.ICCardType_Permissions + "," + s.ICCardType_Remark;
                            id = s.ICCardType_ID.ToString();
                        };

                        if (ICCardTypeDAL.Update(p, ap))
                        {
                            MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CommonalityEntity.WriteLogData("修改", "更新编号为：" + id + "的IC卡类型；修改前：" + strfront + ";修改后：" + strContent, CommonalityEntity.USERNAME);//添加操作日志
                            Empty(); // 调用清空的方法
                        }
                        else
                        {
                            MessageBox.Show("修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("IC卡信息管理 btnUpdate_Click()" );
            }
            finally
            {
                LogInfoLoad("");
                this.btnUpdate.Enabled = false;
                this.btnSave.Enabled = true;
            }
        }

        /// <summary>
        /// “删除” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            tbtnDelICCardType();
        }
        /// <summary>
        /// 删除选中行数据的方法 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbtnDelICCardType()
        {
            try
            {
                int j = 0;
                if (this.dgvICCardType.SelectedRows.Count > 0)//选中删除
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //选中数量
                        int count = dgvICCardType.SelectedRows.Count;
                        string id = "";
                        //遍历
                        for (int i = 0; i < count; i++)
                        {
                            int iccardtype_id = int.Parse(this.dgvICCardType.SelectedRows[i].Cells["ICCardType_ID"].Value.ToString());
                            Expression<Func<ICCardType,bool>> funuserinfo = n => n.ICCardType_ID == iccardtype_id;
                            string strContent = LinQBaseDao.Query("select ICCardType_Name from ICCardType where ICCardType_ID=" + iccardtype_id).Tables[0].Rows[0][0].ToString();

                            DataTable dt = LinQBaseDao.Query("select * from ICCard where ICCard_ICCardType_ID =" + iccardtype_id).Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                MessageBox.Show(strContent + "存在关联信息不能删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                break;
                            }
                            if (ICCardTypeDAL.DeleteToMany(funuserinfo))
                            {
                                j++;
                                CommonalityEntity.WriteLogData("删除", "删除IC卡类型名称为：" + strContent + "的信息", CommonalityEntity.USERNAME);//添加操作日志
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

                CommonalityEntity.WriteTextLog("IC卡类型信息管理 tbtnDelICCardType() 异常！+" );
            }
            finally
            {
                LogInfoLoad("");
            }
        }

        /// <summary>
        /// “清空” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmpty_Click(object sender, EventArgs e)
        {
            Empty();
            this.btnUpdate.Enabled = false;
            this.btnSave.Enabled = true;
        }
        /// <summary>
        /// 清空的方法
        /// </summary>
        private void Empty()
        {
            this.txtICCardType_Name.Text = "";
            this.txtICCardType_Value.Text = "";
            this.comboxICCardType_State.Text = "启动";
            this.txtICCardType_Description.Text = "";
            this.txtICCardType_Remark.Text = "";
            ICCardPermissions.Clear();
        }

        /// <summary>
        /// “搜索” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                sqlwhere = "  1=1";
                string ICTypeName = this.txtICCardTypeName.Text.Trim();
                string state = this.comboxICCardTypeState.Text.Trim();
                string value = this.txtICCardTypeValue.Text.Trim();

                if (!string.IsNullOrEmpty(state))//IC卡类型状态
                {
                    if (state == "全部")
                    {
                        sqlwhere = "  1=1";
                    }
                    else
                    {
                        sqlwhere += String.Format(" and ICCardType_State like  '%{0}%'", state);
                    }
                }
                if (!string.IsNullOrEmpty(value))//IC卡类型值
                {
                    sqlwhere += String.Format(" and ICCardType_Value like  '%{0}%'", value);
                }
                if (!string.IsNullOrEmpty(ICTypeName))//IC卡类型名称
                {
                    sqlwhere += String.Format(" and ICCardType_Name like  '%{0}%'", ICTypeName);
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("ICCardTypeForm.btnSearch_Click异常:");
            }
            finally
            {
                LogInfoLoad("");
            }
        }

        #region 分页和加载DataGridView
        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslNotChecked()
        {
            for (int i = 0; i < this.dgvICCardType.Rows.Count; i++)
            {
                dgvICCardType.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslAllChecked()
        {
            for (int i = 0; i < dgvICCardType.Rows.Count; i++)
            {
                this.dgvICCardType.Rows[i].Selected = true;
            }
        }
        /// <summary>
        /// 分页控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "tslSelectAll")//全选
            {
                tslAllChecked();
                return;
            }
            if (e.ClickedItem.Name == "tslNotSelect")//取消全选
            {
                tslNotChecked();
                return;
            }
            LogInfoLoad(e.ClickedItem.Name);
        }
        /// <summary>
        /// 选择每页条数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tscbxPageSize2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogInfoLoad("");
        }

        /// <summary>
        /// 加载信息
        /// </summary>
        public void LogInfoLoad(string strClickedItemName)
        {
            page.BindBoundControl(dgvICCardType, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "ICCardType", "*", "ICCardType_ID", "ICCardType_ID", 0, sqlwhere, true);
        }
        #endregion

        /// <summary>
        /// 用户双击组件时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvICCardType_DoubleClick(object sender, EventArgs e)
        {
            this.btnUpdate.Enabled = true;
            this.btnSave.Enabled = false;
            if (this.dgvICCardType.SelectedRows.Count > 0)//选中行
            {
                if (dgvICCardType.SelectedRows.Count > 1)
                {
                    MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //修改的值
                    int ID = int.Parse(this.dgvICCardType.SelectedRows[0].Cells["ICCardType_ID"].Value.ToString());
                    Expression<Func<ICCardType,bool>> funviewinto = n => n.ICCardType_ID == ID;
                    foreach (var n in ICCardTypeDAL.Query(funviewinto))
                    {
                        if (n.ICCardType_Name != null)
                        {
                            //IC卡类型名称
                            this.txtICCardType_Name.Text = n.ICCardType_Name;
                        }
                        if (n.ICCardType_Value != null)
                        {
                            //IC卡类型值
                            this.txtICCardType_Value.Text = n.ICCardType_Value;
                        }
                        if (n.ICCardType_State != null)
                        {
                            // IC卡类型状态
                            this.comboxICCardType_State.Text = n.ICCardType_State;
                        }
                        if (n.ICCardType_Description != null)
                        {
                            // IC卡类型描述
                            this.txtICCardType_Description.Text = n.ICCardType_Description;
                        }
                        if (n.ICCardType_Remark != null)
                        {
                            // IC卡类型备注
                            this.txtICCardType_Remark.Text = n.ICCardType_Remark;
                        }
                        if (n.ICCardType_Permissions != null)
                        {
                            ICCardPermissions.Text = n.ICCardType_Permissions;
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

        /// <summary>
        /// 键盘输入事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtICCardType_Value_KeyPress(object sender, KeyPressEventArgs e)
        {
            //阻止从键盘输入键
            e.Handled = true;

            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == (char)8))
            {

                if ((e.KeyChar == (char)8)) { e.Handled = false; return; }
                else
                {
                    int len = this.txtICCardType_Value.Text.Length;
                    if (len < 10)
                    {
                        if (len == 0 && e.KeyChar != '0')
                        {
                            e.Handled = false; return;
                        }
                        //else if (len == 0)
                        //{
                        //    MessageBox.Show("地感PLC值不能以0开头！"); return;
                        //}
                        e.Handled = false; return;
                    }
                    else
                    {
                        PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "IC卡类型值最多只能输入10位数字！", txtICCardType_Value, this);
                    }
                }
            }
            else
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "IC卡类型值只能输入数字！", txtICCardType_Value, this);
            }
        }

        private void dgvICCardType_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        private void ICCardPermissions_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            treeViewIcP.Nodes.Clear();
            try
            {
                if (GBPermissions.Visible)
                {
                    GBPermissions.Visible = false;
                }
                else
                {
                    GBPermissions.Visible = true;
                    TreeNode tr1;
                    DataTable table3 = LinQBaseDao.Query("select Dictionary_Name,Dictionary_Value from Dictionary where Dictionary_OtherID in(select Dictionary_ID from dbo.Dictionary where Dictionary_Value='20')").Tables[0];
                    for (int s = 0; s < table3.Rows.Count; s++)
                    {
                        tr1 = new TreeNode();
                        tr1.Tag = table3.Rows[s]["Dictionary_Value"];
                        tr1.Text = table3.Rows[s]["Dictionary_Name"].ToString();
                        treeViewIcP.Nodes.Add(tr1);
                    }
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("ICCardForm txtICCard_Permissions_DoubleClick()");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GBPermissions.Visible = false;
        }

        ArrayList arraylist = new ArrayList();
        string str = null;
        /// <summary>
        /// 查出选中的父级ID
        /// </summary>
        private void add()
        {
            if (treeViewIcP != null)
            {
                foreach (TreeNode tnTemp in treeViewIcP.Nodes)
                {
                    if (tnTemp.Checked == true)
                    {
                        arraylist.Add(tnTemp.Tag);
                        str += tnTemp.Text.Trim().ToString() + "：";
                    }
                    addDiGui(tnTemp);
                    if (tnTemp.Checked == true)
                    {
                        str = str.TrimEnd(',');
                    }
                }
                str = str.TrimEnd('_');
                str = str.TrimEnd(',');
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
                        if (str == null)
                        {
                            str += tnTemp.Text.Trim().ToString();
                        }
                        else
                            str += tnTemp.Text.Trim().ToString() + ",";
                    }
                    addDiGui(tnTemp);
                }
            }
        }
        public string strview = string.Empty;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                arraylist.Clear();//清空动态数组内的成员
                str = null;
                add();
                ICCardPermissions.Text = str.TrimEnd('：'); ;
                GBPermissions.Visible = false;
            }
            catch 
            {

            }
        }
        /// <summary>    
        /// (GetText)：获取获取已选的文本    
        /// </summary>    
        /// <param name="sender"></param>    
        /// <param name="e"></param>    
        private string GetText(CheckedListBox chklb)
        {
            string checkedText = string.Empty;
            for (int i = 0; i < chklb.CheckedItems.Count; i++)
            {
                checkedText += (String.IsNullOrEmpty(checkedText) ? "" : ",") + chklb.GetItemText(chklb.Items[i]);
            }
            //this.DisplayText.Text = checkedText;
            return checkedText;
        }

    }
}
