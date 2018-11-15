using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Linq.Expressions;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.CommonClass;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class CustomerInfoForm : Form
    {
        public CustomerInfoForm()
        {
            InitializeComponent();
        }


        public MainForm mf;//主窗体
        /// <summary>
        /// 返回一个空的预置皮重
        /// </summary>
        Expression<Func<CustomerInfo, bool>> expr = null;
        /// <summary>
        /// 实例化分页控件
        /// </summary>
        PageControl page = new PageControl();
        // 定义一个全局变量： 门岗编号
        int iPositionID = 0;
        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere;


        /// <summary>
        /// Load 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerInfoForm_Load(object sender, EventArgs e)
        {
            userContext();
            btnUpdate.Enabled = false;
            btnSeach_Click(btnSeach, null);  // 调用查询条件执行查询
            BindCompany();
            BindSearchCompany();

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
                btnSave.Visible = ControlAttributes.BoolControl("btnSave", "CustomerInfoForm", "Visible");
                btnSave.Enabled = ControlAttributes.BoolControl("btnSave", "CustomerInfoForm", "Enabled");

                btnDel.Visible = ControlAttributes.BoolControl("btnDel", "CustomerInfoForm", "Visible");
                btnDel.Enabled = ControlAttributes.BoolControl("btnDel", "CustomerInfoForm", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "CustomerInfoForm", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "CustomerInfoForm", "Enabled");
            }
        }
        /// <summary>
        /// 菜单栏加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.dgvCustomerInfo.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
                this.dgvCustomerInfo.DataSource = null;

                LogInfoLoad("");
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("日记管理 LoadData()" );
            }
        }
        /// <summary>
        /// 绑定公司状态
        /// </summary>
        private void BindCompany()
        {
            try
            {
                this.comboxCustomerInfo_State.DataSource = DictionaryDAL.GetValueDictionary("01").Where(n => n.Dictionary_Name != "全部").ToList();

                if (this.comboxCustomerInfo_State.DataSource != null)
                {
                    this.comboxCustomerInfo_State.DisplayMember = "Dictionary_Name";
                    this.comboxCustomerInfo_State.ValueMember = "Dictionary_ID";
                    this.comboxCustomerInfo_State.SelectedValue = -1;
                }
            }
            catch
            {
                MessageBox.Show("公司信息“公司状态”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 绑定公司状态
        /// </summary>
        private void BindSearchCompany()
        {
            try
            {
                this.comboxCustomerInfoState.DataSource = DictionaryDAL.GetValueStateDictionary("01");

                if (this.comboxCustomerInfoState.DataSource != null)
                {
                    this.comboxCustomerInfoState.DisplayMember = "Dictionary_Name";
                    this.comboxCustomerInfoState.ValueMember = "Dictionary_ID";
                    this.comboxCustomerInfoState.SelectedIndex = 3;
                }
            }
            catch
            {
                MessageBox.Show("公司信息“公司状态”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                string name = this.txtCustomerInfo_Name.Text;
                //判断名称是否已存在
                Expression<Func<CustomerInfo, bool>> funviewCustomerInfo1 = n => n.CustomerInfo_Name == name && n.CustomerInfo_Name != this.dgvCustomerInfo.SelectedRows[0].Cells["CustomerInfo_Name"].Value.ToString();
                if (CustomerInfoDAL.Query(funviewCustomerInfo1).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该公司名称已存在已存在", txtCustomerInfo_Name, this);
                    txtCustomerInfo_Name.Focus();
                    rbool = false;
                }
                return rbool;
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("人员信息管理 btnCheck()");
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
                if (this.txtCustomerInfo_Name.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "公司名称不能为空！", txtCustomerInfo_Name, this);
                    return;
                }
                if (!btnCheck()) return; // 去重复
                var CustomerInfoadd = new CustomerInfo
                {
                    CustomerInfo_Name = this.txtCustomerInfo_Name.Text.Trim(),
                    CustomerInfo_State = this.comboxCustomerInfo_State.Text.Trim(),
                    CustomerInfo_Type = this.txtCustomerInfo_Type.Text.Trim(),
                    CustomerInfo_Email = this.txtCustomerInfo_Email.Text.Trim(),
                    CustomerInfo_Contact = this.txtCustomerInfo_Contact.Text.Trim(),
                    CustomerInfo_ADD = this.txtCustomerInfo_ADD.Text.Trim(),
                    CustomerInfo_Remark = this.txtCustomerInfo_Remark.Text.Trim()
                };

                if (CustomerInfoDAL.InsertOneQCRecord(CustomerInfoadd))
                {
                    MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string strContent1 = "公司名称为：" + this.txtCustomerInfo_Name.Text.Trim();
                    CommonalityEntity.WriteLogData("新增", "新增 " + strContent1 + " 的信息", CommonalityEntity.USERNAME);//添加日志
                }
                else
                {
                    MessageBox.Show("添加失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }


            }
            catch 
            {
                CommonalityEntity.WriteTextLog("公司信息管理 btnSave_Click()" );
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
                if (this.txtCustomerInfo_Name.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "公司名称不能为空！", txtCustomerInfo_Name, this);
                    return;
                }
               
                if (this.dgvCustomerInfo.SelectedRows.Count > 0)//选中行
                {
                    if (dgvCustomerInfo.SelectedRows.Count > 1)
                    {
                        MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (!btnCheck()) return; // 去重复
                        #region 2012-04-23  选中行修改数据
                        Expression<Func<CustomerInfo, bool>> p = n => n.CustomerInfo_ID == int.Parse(this.dgvCustomerInfo.SelectedRows[0].Cells["CustomerInfo_ID"].Value.ToString());
                        string strfront = "";
                        string strContent ="";
                        Action<CustomerInfo> ap = s =>
                        {
                            strfront = s.CustomerInfo_Name + "," + s.CustomerInfo_State + "," + s.CustomerInfo_Type + "," + s.CustomerInfo_Email + "," + s.CustomerInfo_Contact + "," + s.CustomerInfo_ADD + "," + s.CustomerInfo_Remark;
                            s.CustomerInfo_Name = this.txtCustomerInfo_Name.Text.Trim();
                            s.CustomerInfo_State = this.comboxCustomerInfo_State.Text.Trim();
                            s.CustomerInfo_Type = this.txtCustomerInfo_Type.Text.Trim();
                            s.CustomerInfo_Email = this.txtCustomerInfo_Email.Text.Trim();
                            s.CustomerInfo_Contact = this.txtCustomerInfo_Contact.Text.Trim();
                            s.CustomerInfo_ADD = this.txtCustomerInfo_ADD.Text.Trim();
                            s.CustomerInfo_Remark = this.txtCustomerInfo_Remark.Text.Trim();
                            strContent = s.CustomerInfo_Name + "," + s.CustomerInfo_State + "," + s.CustomerInfo_Type + "," + s.CustomerInfo_Email + "," + s.CustomerInfo_Contact + "," + s.CustomerInfo_ADD + "," + s.CustomerInfo_Remark;
                        };

                        if (CustomerInfoDAL.Update(p, ap))
                        {
                            MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CommonalityEntity.WriteLogData("修改", "更新公司信息；修改前："+strfront+"；修改后：" + strContent, CommonalityEntity.USERNAME);//添加日志

                            Empty();
                        }
                        else
                        {
                            MessageBox.Show("修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        #endregion
                    }
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("公司信息管理 btnUpdate_Click()" );
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
            tbtnDelCustomerInfo();
        }
        /// <summary>
        /// 删除选中行数据的方法 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbtnDelCustomerInfo()
        {
            try
            {
                int j = 0;
                if (this.dgvCustomerInfo.SelectedRows.Count > 0)//选中删除
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //选中数量
                        int count = dgvCustomerInfo.SelectedRows.Count;
                        string id = "";
                        //遍历
                        for (int i = 0; i < count; i++)
                        {int customerinfo_id=int.Parse(this.dgvCustomerInfo.SelectedRows[i].Cells["CustomerInfo_ID"].Value.ToString());
                        Expression<Func<CustomerInfo, bool>> funuserinfo = n => n.CustomerInfo_ID == customerinfo_id;
                        string strContent = LinQBaseDao.Query("select CustomerInfo_Name from CustomerInfo where CustomerInfo_ID=" + customerinfo_id).Tables[0].Rows[0][0].ToString();
                            if (CustomerInfoDAL.DeleteToMany(funuserinfo))
                            {
                                j++;
                                CommonalityEntity.WriteLogData("删除", "删除公司名称为：" + strContent + " 的信息", CommonalityEntity.USERNAME);//添加日志
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

                CommonalityEntity.WriteTextLog("公司信息管理 tbtnDelCustomerInfo() 异常！+" );
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
            this.btnDel.Enabled = false;
            this.txtCustomerInfo_Name.Text = "";
            this.comboxCustomerInfo_State.SelectedValue = 1;
            this.txtCustomerInfo_Type.Text = "";
            this.txtCustomerInfo_Email.Text = "";
            this.txtCustomerInfo_Contact.Text = "";
            this.txtCustomerInfo_ADD.Text = "";
            this.txtCustomerInfo_Remark.Text = "";
        }

        /// <summary>
        /// “搜索” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSeach_Click(object sender, EventArgs e)
        {
            try
            {
                sqlwhere = "  1=1";
                string name = this.txtCustomerInfoName.Text.Trim();
                string state = this.comboxCustomerInfoState.Text.Trim();
                if (state != "全部")//公司状态
                {
                    sqlwhere += String.Format(" and CustomerInfo_State like  '%{0}%'", state);
                }
                if (!string.IsNullOrEmpty(name))//公司名称
                {
                    sqlwhere += String.Format(" and CustomerInfo_Name like  '%{0}%'", name);
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("CustomrInfoForm.GetDictionarySeach异常:" );
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
            for (int i = 0; i < this.dgvCustomerInfo.Rows.Count; i++)
            {
                dgvCustomerInfo.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslAllChecked()
        {
            for (int i = 0; i < dgvCustomerInfo.Rows.Count; i++)
            {
                this.dgvCustomerInfo.Rows[i].Selected = true;
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
            page.BindBoundControl(dgvCustomerInfo, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "CustomerInfo", "*", "CustomerInfo_ID", "CustomerInfo_ID", 0, sqlwhere, true);
        }
        #endregion

        /// <summary>
        /// 双击组件时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCustomerInfo_DoubleClick(object sender, EventArgs e)
        {
            this.btnUpdate.Enabled = true;
            this.btnSave.Enabled = false;
            if (this.dgvCustomerInfo.SelectedRows.Count > 0)//选中行
            {
                if (dgvCustomerInfo.SelectedRows.Count > 1)
                {
                    MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    btnDel.Enabled = true;
                    //修改的值
                    int ID = int.Parse(this.dgvCustomerInfo.SelectedRows[0].Cells["CustomerInfo_ID"].Value.ToString());
                    Expression<Func<CustomerInfo, bool>> funviewinto = n => n.CustomerInfo_ID == ID;
                    foreach (var n in CustomerInfoDAL.Query(funviewinto))
                    {
                        if (n.CustomerInfo_Name != null)
                        {
                            //公司名称
                            this.txtCustomerInfo_Name.Text = n.CustomerInfo_Name;
                        }
                        if (n.CustomerInfo_State != null)
                        {
                            // 公司状态
                            this.comboxCustomerInfo_State.Text = n.CustomerInfo_State;
                        }
                        if (n.CustomerInfo_Type != null)
                        {
                            // 公司电话
                            this.txtCustomerInfo_Type.Text = n.CustomerInfo_Type;
                        }
                        if (n.CustomerInfo_Email != null)
                        {
                            // 公司业务描述
                            this.txtCustomerInfo_Email.Text = n.CustomerInfo_Email;
                        }
                        if (n.CustomerInfo_Contact != null)
                        {
                            // 公司联系人
                            this.txtCustomerInfo_Contact.Text = n.CustomerInfo_Contact;
                        }
                        if (n.CustomerInfo_ADD != null)
                        {
                            // 公司地址
                            this.txtCustomerInfo_ADD.Text = n.CustomerInfo_ADD;
                        }
                        if (n.CustomerInfo_Remark != null)
                        {
                            // 公司备注
                            this.txtCustomerInfo_Remark.Text = n.CustomerInfo_Remark;
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

        private void txtCustomerInfo_Type_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(e.KeyChar >= '0' && e.KeyChar <= '9'))
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "联系电话只能为数字！", txtCustomerInfo_Type, this);
            }
        }

        private void dgvCustomerInfo_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }
    }
}
