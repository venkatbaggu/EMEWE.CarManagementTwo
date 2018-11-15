using EMEWE.CarManagement.CommonClass;
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

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class CarAttributeForm : Form
    {
        public CarAttributeForm()
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
        Expression<Func<CarAttribute,bool>> expr = null;
        /// <summary>
        /// 实例化分页控件
        /// </summary>
        PageControl page = new PageControl();

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarAttributeForm_Load(object sender, EventArgs e)
        {
            userContext();
            btnUpdate.Enabled = false;
            mf = new MainForm();
            sqlwhere = "  1=1";
            BindCarAttribute();
            BindSearchCarAttribute();
            BindHeightIDAttribute();
            tscbxPageSize.SelectedIndex = 2;
           // LoadData(); // 调用显示DatagridView的方法
            this.cbxCarAttribute_State.Text = "全部";
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
                btnSave.Visible = ControlAttributes.BoolControl("btnSave", "CarAttributeForm", "Visible");
                btnSave.Enabled = ControlAttributes.BoolControl("btnSave", "CarAttributeForm", "Enabled");

                btnDel.Visible = ControlAttributes.BoolControl("btnDel", "CarAttributeForm", "Visible");
                btnDel.Enabled = ControlAttributes.BoolControl("btnDel", "CarAttributeForm", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "CarAttributeForm", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "CarAttributeForm", "Enabled");
            }
        }
        /// <summary>
        /// 菜单栏加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.dgvCarAttribute.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
                this.dgvCarAttribute.DataSource = null;
                LogInfoLoad("");
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("日记管理 LoadData()" );
            }
        }
        /// <summary>
        /// 绑定车辆属性状态
        /// </summary>
        private void BindCarAttribute()
        {
            try
            {
                this.cbxCarAttribute_State.DataSource = DictionaryDAL.GetValueDictionary("01").Where(n => n.Dictionary_Name != "全部").ToList();

                if (this.cbxCarAttribute_State.DataSource != null)
                {
                    this.cbxCarAttribute_State.DisplayMember = "Dictionary_Name";
                    this.cbxCarAttribute_State.ValueMember = "Dictionary_ID";
                    this.cbxCarAttribute_State.SelectedValue = -1;
                }
                else
                {
                    MessageBox.Show("车辆属性状态暂无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("车辆属性“车辆属性状态”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 搜索 -- 绑定车辆属性状态
        /// </summary>
        private void BindSearchCarAttribute()
        {
            try
            {
                this.cbxTypeState.DataSource = DictionaryDAL.GetValueDictionary("01");

                if (this.cbxTypeState.DataSource != null)
                {
                    this.cbxTypeState.DisplayMember = "Dictionary_Name";
                    this.cbxTypeState.ValueMember = "Dictionary_ID";
                    this.cbxTypeState.SelectedValue = -1;
                }
                else
                {
                    MessageBox.Show("车辆属性状态暂无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("车辆属性“车辆属性状态”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 绑定车辆属性上级编号
        /// </summary>
        private void BindHeightIDAttribute()
        {
            try
            {
                string sql = String.Format("select * from CarAttribute where CarAttribute_State='启动'");
                this.comboxCarAttribute_HeightID.DataSource = CarAttributeDAL.GetViewCarAttributeName(sql);
                if (CarAttributeDAL.GetViewCarAttributeName(sql).Count() > 0)
                {

                    this.comboxCarAttribute_HeightID.DisplayMember = "CarAttribute_Name";
                    this.comboxCarAttribute_HeightID.ValueMember = "CarAttribute_ID";
                    this.comboxCarAttribute_HeightID.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("车辆属性下级编号暂无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("车辆属性“车辆属性下级编号”绑定有误，请查看车辆属性信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                if (LinQBaseDao.Query("select * from CarAttribute where CarAttribute_Name='" + this.txtCarAttribute_Name.Text.ToString() + "'").Tables[0].Rows.Count > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该车辆属性名称已存在", txtCarAttribute_Name, this);
                    this.txtCarAttribute_Name.Focus();
                    rbool = false; ;
                }
                else
                    return rbool;

            }
            catch 
            {
                CommonalityEntity.WriteTextLog("地感管理 btnCheck()");
                rbool = false;
            }
            return rbool;
        }

        int LowerID; // 定义一个下级编号的变量
        /// <summary>
        /// “保存” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtCarAttribute_Name.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "车辆属性名称不能为空！", txtCarAttribute_Name, this);
                    return;
                }
                if (this.comboxCarAttribute_HeightID.Text != "")
                {
                    LowerID = int.Parse(this.comboxCarAttribute_HeightID.SelectedValue.ToString());
                }
                else
                {
                    LowerID = 0;
                }
                if (!btnCheck()) return; // 去重复
                var CarAttributeadd = new CarAttribute
                {
                    CarAttribute_Name = this.txtCarAttribute_Name.Text.Trim(),
                    CarAttribute_State = this.cbxCarAttribute_State.Text,
                    CarAttribute_HeightID = LowerID
                };

                if (CarAttributeDAL.InsertOneCarAttribute(CarAttributeadd))
                {
                    MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("添加失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                string strContent1 = "车辆属性名称为：" + this.txtCarAttribute_Name.Text.Trim(); ;
                CommonalityEntity.WriteLogData("添加", strContent1, CommonalityEntity.USERNAME);//添加操作日志

            }
            catch
            {
                CommonalityEntity.WriteTextLog("车辆属性管理 btnSave_Click()");
            }
            finally
            {
                LogInfoLoad("");
                BindHeightIDAttribute();
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
                if (id == 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "请选择修改的数据！", txtCarAttribute_Name, this);
                    return;
                }
                if (this.txtCarAttribute_Name.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "车辆属性名称不能为空！", txtCarAttribute_Name, this);
                    return;
                }
                if (this.comboxCarAttribute_HeightID.Text != "" && this.dgvCarAttribute.SelectedRows[0].Cells["CarAttribute_ID"].Value.ToString() != this.comboxCarAttribute_HeightID.Text)
                {
                    LowerID = int.Parse(this.comboxCarAttribute_HeightID.SelectedValue.ToString());
                }
                else
                {
                    LowerID = 0;
                }
                if (this.dgvCarAttribute.SelectedRows[0].Cells["CarAttribute_ID"].Value.ToString() == this.comboxCarAttribute_HeightID.Text)
                {
                    MessageBox.Show("车辆属性的上级编号不能使本身编号！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }


                #region 修改
                Expression<Func<CarAttribute,bool>> pc = n => n.CarAttribute_ID == id;

                Action<CarAttribute> ap = s =>
                {
                    s.CarAttribute_Name = this.txtCarAttribute_Name.Text.Trim();
                    s.CarAttribute_State = this.cbxCarAttribute_State.Text;
                    s.CarAttribute_HeightID = LowerID;
                };

                if (CarAttributeDAL.Update(pc, ap))
                {
                    MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else
                {
                    MessageBox.Show("修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                string strContent = "车辆属性名称为：" + this.txtCarAttribute_Name.Text.Trim(); ;
                CommonalityEntity.WriteLogData("修改", strContent, CommonalityEntity.USERNAME);//添加操作日志
                #endregion
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("车辆属性管理 btnUpdate_Click()");
            }
            finally
            {
                LogInfoLoad("");
                BindHeightIDAttribute();
                this.btnUpdate.Enabled = false;
                this.btnSave.Enabled = true;
                this.txtCarAttribute_Name.Enabled = true;
                Empty(); // 调用清空的方法
            }
        }

        /// <summary>
        /// “删除” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            tslDelCarAttribute(); // 调用 删除选中数据信息的方法
        }
        /// <summary>
        ///删除选中数据信息的方法 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslDelCarAttribute()
        {
            try
            {
                int j = 0;
                if (dgvCarAttribute.SelectedRows.Count > 0)//选中删除
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //选中数量
                        int count = dgvCarAttribute.SelectedRows.Count;
                        string id = "";
                        //遍历
                        for (int i = 0; i < count; i++)
                        {
                            Expression<Func<CarAttribute,bool>> funuserinfo = n => n.CarAttribute_ID == int.Parse(this.dgvCarAttribute.SelectedRows[i].Cells["CarAttribute_ID"].Value.ToString());

                            if (!CarAttributeDAL.DeleteToMany(funuserinfo))
                            {
                                j++;
                            }
                        }
                        if (j == 0)
                        {
                            MessageBox.Show("成功删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            BindHeightIDAttribute(); // 刷新绑定上级编号
                        }
                        else
                        {
                            MessageBox.Show("删除失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        string strContent = "车辆属性编号为：" + this.dgvCarAttribute.SelectedRows[0].Cells["CarAttribute_ID"].Value.ToString();
                        CommonalityEntity.WriteLogData("删除", strContent, CommonalityEntity.USERNAME);//添加操作日志
                    }
                }
                else//没有选中
                {
                    MessageBox.Show("请选择要删除的行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch 
            {

                CommonalityEntity.WriteTextLog("车辆属性管理 tslDelCarAttribute()+" );
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
            Empty();// 清空的方法
            this.btnUpdate.Enabled = false;
            this.btnSave.Enabled = true;
            this.txtCarAttribute_Name.Enabled = true;
            this.txtCarAttribute_Name.Visible = true;
        }
        /// <summary>
        /// 清空的方法
        /// </summary>
        private void Empty()
        {
            this.txtCarAttribute_Name.Text = "";
            this.cbxCarAttribute_State.Text = "启动";
        }

        #region 分页和加载DataGridView
        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslNotSelect()
        {
            for (int i = 0; i < this.dgvCarAttribute.Rows.Count; i++)
            {
                dgvCarAttribute.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslSelectAll()
        {
            for (int i = 0; i < dgvCarAttribute.Rows.Count; i++)
            {
                this.dgvCarAttribute.Rows[i].Selected = true;
            }
        }
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
            page.BindBoundControl(dgvCarAttribute, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "CarAttribute", "*", "CarAttribute_ID", "CarAttribute_ID", 0, sqlwhere, true);
            DataBindingComplete();

        }
        #endregion



        private void DataBindingComplete()
        {
            if (this.dgvCarAttribute.Rows.Count != 0)
            {
                for (int i = 0; i < this.dgvCarAttribute.Rows.Count; i++)
                {
                    string id = dgvCarAttribute.Rows[i].Cells["CarAttribute_HeightID"].Value.ToString();
                    if (id != "0")
                    {
                        string str = " select CarAttribute_Name from CarAttribute where CarAttribute_ID=" + id;
                        DataTable dt = LinQBaseDao.Query(str).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            dgvCarAttribute.Rows[i].Cells["CarAttribute_N"].Value = dt.Rows[0][0].ToString();
                        }
                    }
                    else
                    {
                        dgvCarAttribute.Rows[i].Cells["CarAttribute_N"].Value = "";
                    }
                }
            }
        }

        private int id = 0;
        /// <summary>
        /// 双击组件时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCarAttribute_DoubleClick(object sender, EventArgs e)
        {
            this.btnUpdate.Enabled = true;
            this.btnSave.Enabled = false;
            if (this.dgvCarAttribute.SelectedRows.Count > 0)//选中行
            {
                if (dgvCarAttribute.SelectedRows.Count > 1)
                {
                    MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //修改的值
                    int ID = int.Parse(this.dgvCarAttribute.SelectedRows[0].Cells["CarAttribute_ID"].Value.ToString());
                    Expression<Func<CarAttribute,bool>> funviewinto = n => n.CarAttribute_ID == ID;
                    foreach (var n in CarAttributeDAL.QueryView(funviewinto))
                    {
                        id = n.CarAttribute_ID;
                        this.txtCarAttribute_Name.Enabled = false;
                        if (n.CarAttribute_Name != null)
                        {
                            // 车辆属性名称
                            this.txtCarAttribute_Name.Text = n.CarAttribute_Name;
                        }
                        if (n.CarAttribute_State != null)
                        {
                            //车辆属性状态
                            this.cbxCarAttribute_State.Text = n.CarAttribute_State;
                        }
                        if (n.CarAttribute_HeightID != null)
                        {
                            //车辆属性下级编号
                            this.comboxCarAttribute_HeightID.Text = n.CarAttribute_Name;
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

        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                sqlwhere = "  1=1";
                string name = this.txtName.Text.Trim();
                string state = this.cbxTypeState.Text;

                if (!string.IsNullOrEmpty(state))//车辆属性状态 
                {
                    if (state == "全部")
                    {
                        sqlwhere = "  1=1";
                    }
                    else
                    {
                        sqlwhere += String.Format(" and CarAttribute_State like  '%{0}%'", state);
                    }
                }
                if (!string.IsNullOrEmpty(name))//车辆属性名称
                {
                    sqlwhere += String.Format(" and CarAttribute_Name like  '%{0}%'", name);
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("BusinessTypeForm.selectTJ异常:" );
            }
            finally
            {
                LogInfoLoad("");
            }
        }

        private void dgvCarAttribute_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }
    }
}
