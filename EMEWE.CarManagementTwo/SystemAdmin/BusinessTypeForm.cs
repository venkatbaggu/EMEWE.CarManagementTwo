using EMEWE.CarManagement.CommonClass;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.HelpClass;
using EMEWE.CarManagementDAL;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class BusinessTypeForm : Form
    {
        public BusinessTypeForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere;
        public static MainForm mf; // 主窗体
        /// <summary>
        /// 返回一个空的预置皮重
        /// </summary>
        Expression<Func<BusinessType, bool>> expr = null;
        /// <summary>
        /// 实例化分页控件
        /// </summary>
        PageControl page = new PageControl();

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BusinessTypeForm_Load(object sender, EventArgs e)
        {
            userContext();
            btnUpdate.Enabled = false;
            btnSelect_Click(btnSelect, null);
            BindBusinessType();
            BindSearchBusinessType();
            mf = new MainForm();
            tscbxPageSize.SelectedIndex = 2;
            //LoadData(); // 调用显示DatagridView的方法
            this.cbxBusinessType_Loaded.Text = "装货";
            BindCarType();
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
                btnSave.Visible = ControlAttributes.BoolControl("btnSave", "BusinessTypeForm", "Visible");
                btnSave.Enabled = ControlAttributes.BoolControl("btnSave", "BusinessTypeForm", "Enabled");

                btnDel.Visible = ControlAttributes.BoolControl("btnDel", "BusinessTypeForm", "Visible");
                btnDel.Enabled = ControlAttributes.BoolControl("btnDel", "BusinessTypeForm", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "BusinessTypeForm", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "BusinessTypeForm", "Enabled");
            }
        }
        /// <summary>
        /// 菜单栏加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.dgvBusinessType.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
                this.dgvBusinessType.DataSource = null;
                LogInfoLoad("");
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("日记管理 LoadData()" );
            }
        }


        private void BindCarType()
        {
            DataTable dt = LinQBaseDao.Query("select CarType_Name,CarType_ID from CarType where CarType_State='启动'").Tables[0];
            cmbCarType.DataSource = dt;
            cmbCarType.DisplayMember = "CarType_Name";
            cmbCarType.ValueMember = "CarType_ID";
            cmbCarType.SelectedIndex = 0;
        }
        /// <summary>
        /// 绑定业务类别状态
        /// </summary>
        private void BindBusinessType()
        {
            try
            {
                this.cbxBusinessType_State.DataSource = DictionaryDAL.GetValueDictionary("01").Where(n => n.Dictionary_Name != "全部").ToList();

                if (this.cbxBusinessType_State.DataSource != null)
                {
                    this.cbxBusinessType_State.DisplayMember = "Dictionary_Name";
                    this.cbxBusinessType_State.ValueMember = "Dictionary_ID";
                    this.cbxBusinessType_State.SelectedValue = -1;
                }
                else
                {
                    MessageBox.Show("业务类别状态暂无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("业务类别管理“业务类别状态”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 搜索--绑定业务类别状态
        /// </summary>
        private void BindSearchBusinessType()
        {
            try
            {
                this.cbxBusinessTypeState.DataSource = DictionaryDAL.GetValueStateDictionary("01");

                if (this.cbxBusinessTypeState.DataSource != null)
                {
                    this.cbxBusinessTypeState.DisplayMember = "Dictionary_Name";
                    this.cbxBusinessTypeState.ValueMember = "Dictionary_ID";
                    this.cbxBusinessTypeState.SelectedIndex = 3;
                }
                else
                {
                    MessageBox.Show("业务类别状态暂无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("业务类别管理“业务类别状态”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                string BusinessType_Name = this.txtBusinessType_Name.Text.ToString();
                //判断名称是否已存在 
                Expression<Func<BusinessType, bool>> funviewFVNInfo = n => n.BusinessType_Name == BusinessType_Name;
                if (BusinessTypeDAL.QueryView(funviewFVNInfo).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该业务类型名称已存在", txtBusinessType_Name, this);
                    this.txtBusinessType_Name.Focus();
                    rbool = false; ;
                }
                return rbool;

            }
            catch 
            {
                CommonalityEntity.WriteTextLog("地感管理 btnCheck()" );
                rbool = false;
            }
            return rbool;
        }

        /// <summary>
        /// 查重方法
        /// </summary>
        /// <returns></returns>
        private bool btnCheckupdate()
        {
            bool rbool = true;
            try
            {
                string BusinessType_Name = this.txtBusinessType_Name.Text.ToString();
                //判断名称是否已存在 
                Expression<Func<BusinessType, bool>> funviewFVNInfo = n => n.BusinessType_Name == BusinessType_Name && n.BusinessType_Name != this.dgvBusinessType.SelectedRows[0].Cells["BusinessType_Name"].Value.ToString();
                if (BusinessTypeDAL.QueryView(funviewFVNInfo).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该业务类型名称已存在", txtBusinessType_Name, this);
                    this.txtBusinessType_Name.Focus();
                    rbool = false; ;
                }
                return rbool;

            }
            catch 
            {
                CommonalityEntity.WriteTextLog("地感管理 btnCheck()");
                rbool = false;
            }
            return rbool;
        }

        /// <summary>
        /// “保存” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtBusinessType_Name.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "业务类别名称不能为空！", txtBusinessType_Name, this);
                    return;
                }
                if (!btnCheck()) return; // 去重复
                var BusinessTypeadd = new BusinessType
                {
                    BusinessType_UserId = int.Parse(common.USERID),
                    BusinessType_Name = this.txtBusinessType_Name.Text.Trim(),
                    BusinessType_Content = this.txtBusinessType_Content.Text.Trim(),
                    BusinessType_CreatTime = Convert.ToDateTime(CommonalityEntity.GetServersTime().ToString()),
                    BusinessType_Remark = this.txtBusinessType_Remark.Text.Trim(),
                    BusinessType_Use = this.txtBusinessType_Use.Text.Trim(),
                    BusinessType_State = this.cbxBusinessType_State.Text,
                    BusinessType_Loaded = this.cbxBusinessType_Loaded.Text,
                    BusinessType_CarType_ID = Convert.ToInt32(cmbCarType.SelectedValue.ToString())
                };

                if (BusinessTypeDAL.InsertOneBusinessType(BusinessTypeadd))
                {
                    string strContent1 = "业务类别名称为：" + this.txtBusinessType_Name.Text.Trim(); ;
                    CommonalityEntity.WriteLogData("新增", "新增 " + strContent1 + " 的信息", common.USERNAME);//添加日志
                    MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("添加失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch 
            {
                CommonalityEntity.WriteTextLog("业务类别管理 btnAdd_Click()" );
            }
            finally
            {
                LogInfoLoad("");
                Empty(); // 调用清空的方法
            }
        }

        /// <summary>
        /// “修改”  按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtBusinessType_Name.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "业务类别名称不能为空！", txtBusinessType_Name, this);
                    return;
                }
                if (this.dgvBusinessType.SelectedRows.Count > 0)//选中行
                {
                    if (dgvBusinessType.SelectedRows.Count > 1)
                    {
                        MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (!btnCheckupdate()) return; // 去重复
                        Expression<Func<BusinessType, bool>> pc = n => n.BusinessType_ID == int.Parse(this.dgvBusinessType.SelectedRows[0].Cells["BusinessType_ID"].Value.ToString());

                        Action<BusinessType> ap = s =>
                        {
                            s.BusinessType_UserId = CommonalityEntity.USERID;
                            s.BusinessType_Name = this.txtBusinessType_Name.Text.Trim();
                            s.BusinessType_Content = this.txtBusinessType_Content.Text.Trim();
                            s.BusinessType_CreatTime = Convert.ToDateTime(CommonalityEntity.GetServersTime().ToString());
                            s.BusinessType_Remark = this.txtBusinessType_Remark.Text.Trim();
                            s.BusinessType_Use = this.txtBusinessType_Use.Text.Trim();
                            s.BusinessType_State = this.cbxBusinessType_State.Text;
                            s.BusinessType_Loaded = this.cbxBusinessType_Loaded.Text;
                            s.BusinessType_CarType_ID = Convert.ToInt32(cmbCarType.SelectedValue.ToString());
                        };

                        if (BusinessTypeDAL.Update(pc, ap))
                        {
                            string strContent = "业务类别名称为：" + this.txtBusinessType_Name.Text.Trim(); ;
                            CommonalityEntity.WriteLogData("修改", "修改 " + strContent + " 的信息", common.USERNAME);//添加日志
                            MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                CommonalityEntity.WriteTextLog("业务类别管理 btnAdd_Click()");
            }
            finally
            {
                LogInfoLoad("");
                this.btnUpdate.Enabled = false;
                this.btnSave.Enabled = true;
                Empty(); // 调用清空的方法
            }
        }

        /// <summary>
        /// “清空” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmpty_Click(object sender, EventArgs e)
        {
            Empty(); // 清空的方法
            this.btnUpdate.Enabled = false;
            this.btnSave.Enabled = true;
        }
        /// <summary>
        /// 清空的方法
        /// </summary>
        private void Empty()
        {
            this.txtBusinessType_Name.Text = "";
            this.txtBusinessType_Content.Text.Trim();
            this.txtBusinessType_Remark.Text = "";
            this.txtBusinessType_Use.Text = "";
            this.cbxBusinessType_State.Text = "启动";
            this.cbxBusinessType_Loaded.Text = "装货";
        }

        /// <summary>
        /// “删除” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            tslDelBusinessType();//删除选中数据信息的方法  
        }
        /// <summary>
        /// 删除选中数据信息的方法 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslDelBusinessType()
        {
            try
            {
                int j = 0;
                if (this.dgvBusinessType.SelectedRows.Count > 0)//选中删除
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //选中数量
                        int count = dgvBusinessType.SelectedRows.Count;
                        string id = "";
                        //遍历
                        for (int i = 0; i < count; i++)
                        {
                            Expression<Func<BusinessType, bool>> funuserinfo = n => n.BusinessType_ID == int.Parse(this.dgvBusinessType.SelectedRows[i].Cells["BusinessType_ID"].Value.ToString());

                            if (BusinessTypeDAL.DeleteToMany(funuserinfo))
                            {
                                j++;
                                string strContent = "业务类别编号为：" + this.dgvBusinessType.SelectedRows[0].Cells["BusinessType_ID"].Value.ToString();
                                CommonalityEntity.WriteLogData("删除", "删除 " + strContent + " 的信息", common.USERNAME);//添加日志
                            }
                        }
                    }
                    if (j != 0)
                    {
                        MessageBox.Show("成功删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    else
                    {
                        MessageBox.Show("删除失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                }
                else//没有选中
                {
                    MessageBox.Show("请选择要删除的行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch(Exception err) 
            {

                CommonalityEntity.WriteTextLog("业务类别管理 tslDelBusinessType()+" );
            }
            finally
            {
                LogInfoLoad("");
            }
        }

        /// <summary>
        /// “搜索” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (btnSelect.Enabled)
            {
                btnSelect.Enabled = false;
                selectTJ();
                //LogInfoDAL.loginfoadd("查询","检测项目查询",Common.USERNAME);//操作日志
                btnSelect.Enabled = true;
            }
        }
        /// <summary>
        /// 搜索的方法
        /// </summary>
        private void selectTJ()
        {
            try
            {
                sqlwhere = "  1=1";
                string name = this.txtBusinessTypeName.Text.Trim();
                string state = this.cbxBusinessTypeState.Text;

                if (!string.IsNullOrEmpty(state))//业务类别状态
                {
                    if (state == "全部")
                    {
                        sqlwhere = "  1=1";
                    }
                    else
                    {
                        sqlwhere += String.Format(" and BusinessType_State like  '%{0}%'", state);
                    }
                }
                if (!string.IsNullOrEmpty(name))//业务类别名称
                {
                    sqlwhere += String.Format(" and BusinessType_Name like  '%{0}%'", name);
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("BusinessTypeForm.selectTJ异常:" + "".ToString());
            }
            finally
            {
                LogInfoLoad("");
            }
        }


        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslNotSelect()
        {
            for (int i = 0; i < this.dgvBusinessType.Rows.Count; i++)
            {
                dgvBusinessType.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslSelectAll()
        {
            for (int i = 0; i < dgvBusinessType.Rows.Count; i++)
            {
                this.dgvBusinessType.Rows[i].Selected = true;
            }
        }

        #region 分页和加载DataGridView
        /// <summary>
        /// 分页控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "tslAllCheck")//全选
            {
                tslSelectAll();
                return;
            }
            if (e.ClickedItem.Name == "tslNotCheck")//取消全选
            {
                tslNotSelect();
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
            page.BindBoundControl(dgvBusinessType, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_BusinessType_UserInfo", "*", "BusinessType_ID", "BusinessType_ID", 0, sqlwhere, true);
        }
        #endregion

        /// <summary>
        /// 双击组件时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvBusinessType_DoubleClick(object sender, EventArgs e)
        {
            this.btnUpdate.Enabled = true;
            this.btnSave.Enabled = false;
            if (this.dgvBusinessType.SelectedRows.Count > 0)//选中行
            {
                if (dgvBusinessType.SelectedRows.Count > 1)
                {
                    MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //修改的值
                    int ID = int.Parse(this.dgvBusinessType.SelectedRows[0].Cells["BusinessType_ID"].Value.ToString());
                    Expression<Func<BusinessType, bool>> funviewinto = n => n.BusinessType_ID == ID;
                    foreach (var n in BusinessTypeDAL.QueryView(funviewinto))
                    {
                        if (n.BusinessType_Name != null)
                        {
                            // 业务类别名称
                            this.txtBusinessType_Name.Text = n.BusinessType_Name;
                        }
                        if (n.BusinessType_CarType_ID != null && n.BusinessType_CarType_ID > 0)
                        {
                            cmbCarType.Text = LinQBaseDao.GetSingle("select CarType_Name from CarType where CarType_ID=" + n.BusinessType_CarType_ID).ToString();
                        }
                        if (n.BusinessType_State != null)
                        {
                            // 业务类别状态
                            this.cbxBusinessType_State.Text = n.BusinessType_State;
                        }
                        if (n.BusinessType_Loaded != null)
                        {
                            //装货类型
                            this.cbxBusinessType_Loaded.Text = n.BusinessType_Loaded;
                        }
                        if (n.BusinessType_Use != null)
                        {
                            //业务类别用途
                            this.txtBusinessType_Use.Text = n.BusinessType_Use;
                        }
                        if (n.BusinessType_Content != null)
                        {
                            // 业务类别描述
                            this.txtBusinessType_Content.Text = n.BusinessType_Content;
                        }
                        if (n.BusinessType_Remark != null)
                        {
                            // 业务类别备注
                            this.txtBusinessType_Remark.Text = n.BusinessType_Remark;
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

        private void dgvBusinessType_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }
    }
}
