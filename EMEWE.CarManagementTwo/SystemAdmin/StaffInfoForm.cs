using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.Commons.CommonClass;
using System.Linq.Expressions;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.HelpClass;
using System.Data.OleDb;
using EMEWE.CarManagement.MyControl;
using GemBox.ExcelLite;
using EMEWE.CarManagement.CommonClass;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class StaffInfoForm : Form
    {
        public StaffInfoForm()
        {
            InitializeComponent();
        }

        public MainForm mf;//主窗体
        /// <summary>
        /// 返回一个空的预置皮重
        /// </summary>
        Expression<Func<View_StaffInfo_ICCard, bool>> expr = null;
        /// <summary>
        /// 实例化分页控件
        /// </summary>
        PageControl page = new PageControl();
        // 定义一个全局变量： 门岗编号
        List<MenuInfo> list = new List<MenuInfo>();
        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere = "stafflnfo='驾驶员'";
        private string strMenu_Value = "";
        /// <summary>
        /// 驾驶员ID
        /// </summary>
        private int staffinid = 0;
        /// <summary>
        /// 身份证号
        /// </summary>
        private string idns = "";
        /// <summary>
        /// 是否导出所有
        /// </summary>
        private bool ISExecl = false;
        /// <summary>
        /// Load 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StaffInfoForm_Load(object sender, EventArgs e)
        {
            userContext();
            btnUpdate.Enabled = false;
            btnSeach_Click(btnSeach, null);  // 调用查询条件执行查询
            BindStaffInfoState();
            BindStaffInfoType();
            BindSearchStaffInfoState1();
            BindSearchStaffInfoType1();
            this.comboxStaffInfo_Sex.Text = "男";
            tscbxPageSize.SelectedIndex = 1;
            groupBox1.Visible = false;
            //LoadData();
            mf = new MainForm();
        }
        /// <summary>
        /// 菜单栏加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.dgvStaffInfo.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
                this.dgvStaffInfo.DataSource = null;

                LogInfoLoad("");
            }
            catch
            {
                CommonalityEntity.WriteTextLog("日记管理 LoadData()" + "".ToString());
            }
        }
        #region  comboBox的绑定
        /// <summary>
        /// 绑定人员状态
        /// </summary>
        private void BindStaffInfoState()
        {
            try
            {
                this.comboxStaffInfo_State.DataSource = DictionaryDAL.GetValueDictionary("01").Where(n => n.Dictionary_Name != "全部").ToList();

                if (this.comboxStaffInfo_State.DataSource != null)
                {
                    this.comboxStaffInfo_State.DisplayMember = "Dictionary_Name";
                    this.comboxStaffInfo_State.ValueMember = "Dictionary_ID";
                    this.comboxStaffInfo_State.SelectedValue = -1;
                }
            }
            catch
            {
                MessageBox.Show("人员信息“人员类型”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 绑定人员类型
        /// </summary>
        private void BindStaffInfoType()
        {
            try
            {
                this.comboxStaffInfo_Type.DataSource = DictionaryDAL.GetValueDictionary("07").Where(n => n.Dictionary_Name != "全部").ToList();

                if (this.comboxStaffInfo_Type.DataSource != null)
                {
                    this.comboxStaffInfo_Type.DisplayMember = "Dictionary_Name";
                    this.comboxStaffInfo_Type.ValueMember = "Dictionary_ID";
                    this.comboxStaffInfo_Type.SelectedValue = -1;
                }
            }
            catch
            {
                MessageBox.Show("人员信息“人员类型”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// 搜索--绑定人员状态
        /// </summary>
        private void BindSearchStaffInfoState1()
        {
            try
            {
                this.comboxStaffInfoState.DataSource = DictionaryDAL.GetValueStateDictionary("01");

                if (this.comboxStaffInfoState.DataSource != null)
                {
                    this.comboxStaffInfoState.DisplayMember = "Dictionary_Name";
                    this.comboxStaffInfoState.ValueMember = "Dictionary_ID";
                    this.comboxStaffInfoState.SelectedIndex = 3;
                }
            }
            catch
            {
                MessageBox.Show("人员信息“人员状态”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 搜索--绑定人员类型
        /// </summary>
        private void BindSearchStaffInfoType1()
        {
            try
            {
                this.comboxStaffInfoType.DataSource = DictionaryDAL.GetValueStateDictionary("07");

                if (this.comboxStaffInfoType.DataSource != null)
                {
                    this.comboxStaffInfoType.DisplayMember = "Dictionary_Name";
                    this.comboxStaffInfoType.ValueMember = "Dictionary_ID";
                    this.comboxStaffInfoType.SelectedIndex = 3;
                }
            }
            catch
            {
                MessageBox.Show("人员信息“人员类型”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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

                btnSave.Enabled = true;
                btnSave.Visible = true;
                btnDel.Enabled = true;
                btnDel.Visible = true;
                btnUpdate.Enabled = true;
                btnUpdate.Visible = true;
                tlsbExecl.Enabled = true;
                tlsbExecl.Visible = true;
                tlsbInExecl.Enabled = true;
                tlsbInExecl.Visible = true;
            }
            else
            {
                btnSave.Visible = ControlAttributes.BoolControl("btnSave", "StaffInfoForm", "Visible");
                btnSave.Enabled = ControlAttributes.BoolControl("btnSave", "StaffInfoForm", "Enabled");

                btnDel.Visible = ControlAttributes.BoolControl("btnDel", "StaffInfoForm", "Visible");
                btnDel.Enabled = ControlAttributes.BoolControl("btnDel", "StaffInfoForm", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "StaffInfoForm", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "StaffInfoForm", "Enabled");

                tlsbExecl.Visible = ControlAttributes.BoolControl("tlsbExecl", "StaffInfoForm", "Visible");
                tlsbExecl.Enabled = ControlAttributes.BoolControl("tlsbExecl", "StaffInfoForm", "Enabled");

                tlsbInExecl.Visible = ControlAttributes.BoolControl("tlsbInExecl", "StaffInfoForm", "Visible");
                tlsbInExecl.Enabled = ControlAttributes.BoolControl("tlsbInExecl", "StaffInfoForm", "Enabled");
            }
        }

        /// <summary>
        /// “保 存” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            int ids = 0;
            try
            {
                StaffInfo StaffInfoadd = new StaffInfo();
                if (this.txtStaffInfo_Name.Text.Trim() == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "人员名称不能为空！", txtStaffInfo_Name, this);
                    return;
                }

                if (this.txtStaffInfo_Identity.Text.Trim() == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "身份证号码不能为空！", txtStaffInfo_Identity, this);
                    return;
                }
                if (this.txtStaffInfo_Phone.Text.Trim() == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "手机号码不能为空！", txtStaffInfo_Identity, this);
                    return;
                }
                bool flag = true;
                if (this.comboxStaffInfo_Sex.Text.Trim() == "男")
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }
                DataTable dtsta = LinQBaseDao.Query(" select * from StaffInfo where StaffInfo_Identity='" + txtStaffInfo_Identity.Text.Trim() + "'").Tables[0];
                if (dtsta.Rows.Count > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "身份证号码已存在！", txtStaffInfo_Identity, this);
                    return; // 去重复
                }

                if (txtICCardValue.Text.Trim() != "")
                {
                    DataTable dtic = LinQBaseDao.Query("select ICCard_ID from ICCard where ICCard_Value='" + txtICCardValue.Text.Trim() + "'").Tables[0];
                    if (dtic.Rows.Count > 0)
                    {
                        ids = Convert.ToInt32(dtic.Rows[0][0].ToString());
                    }
                    else
                    {
                        MessageBox.Show("IC卡信息不存在，请先配置IC卡！");
                        return;
                    }
                }

                string icty = "";
                DataTable dt = LinQBaseDao.Query("select * from View_StaffInfo_ICCard where StaffInfo_ICCard_ID=" + ids + " order by StaffInfo_ID desc").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("此IC卡已关联信息！");
                    return;
                }
                StaffInfoadd.StaffInfo_Name = this.txtStaffInfo_Name.Text;
                if (ids != 0)
                {
                    StaffInfoadd.StaffInfo_ICCard_ID = ids;
                    ids = 0;
                }
                StaffInfoadd.StaffInfo_Type = this.comboxStaffInfo_Type.Text;
                StaffInfoadd.StaffInfo_Sex = flag;
                StaffInfoadd.StaffInfo_State = this.comboxStaffInfo_State.Text.Trim();
                StaffInfoadd.StaffInfo_License = this.txtStaffInfo_License.Text.Trim();
                StaffInfoadd.StaffInfo_Identity = this.txtStaffInfo_Identity.Text.Trim();
                StaffInfoadd.StaffInfo_Time = CommonalityEntity.GetServersTime();
                StaffInfoadd.StaffInfo_Phone = this.txtStaffInfo_Phone.Text.Trim();
                StaffInfoadd.StaffInfo_Add = this.txtStaffInfo_Add.Text.Trim();
                StaffInfoadd.StaffInfo_Remark = this.txtStaffInfo_Remark.Text.Trim();
                StaffInfoadd.StaffInfo_Number = txtNumber.Text.Trim();

                if (StaffInfoDAL.InsertOneQCRecord(StaffInfoadd))
                {
                    MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string strContent1 = "新增驾驶员名称为：" + this.txtStaffInfo_Name.Text.Trim() + "的信息";
                    CommonalityEntity.WriteLogData("新增", strContent1, CommonalityEntity.USERNAME);//添加操作日志
                    Empty();
                }
                else
                {
                    MessageBox.Show("添加失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("人员基本信息管理 btnSave_Click()" + "".ToString());
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
                if (this.txtStaffInfo_Name.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "人员名称不能为空！", txtStaffInfo_Name, this);
                    return;
                }
                if (this.txtStaffInfo_Identity.Text.Trim() == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "身份证号码不能为空！", txtStaffInfo_Identity, this);
                    return;
                }
                if (this.txtStaffInfo_Identity.Text.Trim() == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "身份证号码不能为空！", txtStaffInfo_Identity, this);
                    return;
                }
                bool flag = true;
                if (this.comboxStaffInfo_Sex.Text.Trim() == "男")
                {
                    flag = true;
                }
                else
                {
                    flag = false;
                }

                DataTable dtsta = LinQBaseDao.Query(" select * from StaffInfo where StaffInfo_Identity='" + txtStaffInfo_Identity.Text.Trim() + "'  and StaffInfo_ID!=" + staffinid).Tables[0];
                if (dtsta.Rows.Count > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "身份证号码已存在！", txtStaffInfo_Identity, this);
                    return; // 去重复
                }
                #region   选中行修改数据
                string cvalue = "";
                string iccardid = "";

                if (this.txtICCardValue.Text.Trim() != "")
                {
                    cvalue = this.txtICCardValue.Text.Trim();
                    try
                    {
                        iccardid = LinQBaseDao.Query("select ICCard_ICCardType_ID from ICCard where ICCard_Value='" + cvalue + "'").Tables[0].Rows[0][0].ToString();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("IC卡信息不存在！");
                        return;
                    }
                }
                string id = "";
                string strfront = "";
                string strContent = "";
                Expression<Func<StaffInfo, bool>> p = n => n.StaffInfo_ID == staffinid;
                Action<StaffInfo> ap = s =>
                {
                    strfront = s.StaffInfo_ICCard_ID + "," + s.StaffInfo_Name + "," + s.StaffInfo_Type + "," + s.StaffInfo_Sex + "," + s.StaffInfo_State + "," + s.StaffInfo_CarType + "," + s.StaffInfo_License + "," + s.StaffInfo_Identity + "," + s.StaffInfo_Phone + "," + s.StaffInfo_Add + "," + s.StaffInfo_Remark;
                    if (iccardid != "")
                    {
                        s.StaffInfo_ICCard_ID = Convert.ToInt32(iccardid);
                        iccardid = "";
                    }
                    s.StaffInfo_Name = this.txtStaffInfo_Name.Text.Trim();
                    s.StaffInfo_Type = this.comboxStaffInfo_Type.Text.Trim();
                    s.StaffInfo_Sex = flag;
                    s.StaffInfo_State = this.comboxStaffInfo_State.Text.Trim();
                    s.StaffInfo_License = this.txtStaffInfo_License.Text.Trim();
                    s.StaffInfo_Identity = this.txtStaffInfo_Identity.Text.Trim();
                    s.StaffInfo_Phone = this.txtStaffInfo_Phone.Text.Trim();
                    s.StaffInfo_Add = this.txtStaffInfo_Add.Text.Trim();
                    s.StaffInfo_Remark = this.txtStaffInfo_Remark.Text.Trim();
                    s.StaffInfo_Number = this.txtNumber.Text.Trim();
                    strContent = s.StaffInfo_ICCard_ID + "," + s.StaffInfo_Name + "," + s.StaffInfo_Type + "," + s.StaffInfo_Sex + "," + s.StaffInfo_State + "," + s.StaffInfo_CarType + "," + s.StaffInfo_License + "," + s.StaffInfo_Identity + "," + s.StaffInfo_Phone + "," + s.StaffInfo_Add + "," + s.StaffInfo_Remark;
                    id = s.StaffInfo_ID.ToString();
                };
                if (StaffInfoDAL.Update(p, ap))
                {
                    MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CommonalityEntity.WriteLogData("修改", "更新驾驶员信息；编号为：" + id + "；修改前：" + strfront + "；修改后：" + strContent, CommonalityEntity.USERNAME);//添加操作日志
                    Empty();
                }
                else
                {
                    MessageBox.Show("修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                #endregion
            }
            catch
            {
                CommonalityEntity.WriteTextLog("人员基本信息管理 btnUpdate_Click()" + "".ToString());
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
            tbtnDelStaffInfo();
        }
        /// <summary>
        /// 删除选中行数据的方法 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbtnDelStaffInfo()
        {
            try
            {
                int j = 0;
                if (this.dgvStaffInfo.SelectedRows.Count > 0)//选中删除
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //选中数量
                        int count = dgvStaffInfo.SelectedRows.Count;
                        //遍历
                        for (int i = 0; i < count; i++)
                        {
                            int staffinfo_id = int.Parse(this.dgvStaffInfo.SelectedRows[i].Cells["StaffInfo_ID"].Value.ToString());
                            Expression<Func<StaffInfo, bool>> funuserinfo = n => n.StaffInfo_ID == staffinfo_id;
                            string strContent = LinQBaseDao.Query("select StaffInfo_Name from StaffInfo where StaffInfo_ID=" + staffinfo_id).Tables[0].Rows[0][0].ToString();
                            if (StaffInfoDAL.DeleteToMany(funuserinfo))
                            {
                                j++;
                                CommonalityEntity.WriteLogData("删除", "删除驾驶员名称为：" + strContent + " 的信息", CommonalityEntity.USERNAME);//添加操作日志
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

                CommonalityEntity.WriteTextLog("人员信息管理 tbtnDelStaffInfo() 异常！+" + "".ToString());
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
            this.txtICCardValue.Text = "";
            this.txtStaffInfo_Name.Text = "";
            this.comboxStaffInfo_Type.SelectedIndex = 0;
            this.comboxStaffInfo_Sex.Text = "男";
            this.comboxStaffInfo_State.SelectedIndex = 0; ;
            this.txtStaffInfo_License.Text = "";
            this.txtStaffInfo_Identity.Text = "";
            this.txtStaffInfo_Phone.Text = "";
            this.txtStaffInfo_Add.Text = "";
            this.txtStaffInfo_Remark.Text = "";
            this.txtNumber.Text = "";
            txtICCardValue.Enabled = true;
            CommonalityEntity.strCardNo = "";
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
                string name = this.txtStaffInfoName.Text.Trim();
                string state = this.comboxStaffInfoState.Text.Trim();
                string type = this.comboxStaffInfoType.Text.Trim();

                if (!string.IsNullOrEmpty(state))//人员状态
                {
                    if (state != "全部")
                    {
                        sqlwhere += String.Format(" and StaffInfo_State like  '%{0}%'", state);
                    }
                }
                if (!string.IsNullOrEmpty(type))//人员类型
                {
                    if (type != "全部")
                    {
                        sqlwhere += String.Format(" and StaffInfo_Type like  '%{0}%'", type);
                    }
                }
                if (!string.IsNullOrEmpty(name))//人员名称
                {
                    sqlwhere += String.Format(" and StaffInfo_Name like  '%{0}%'", name);
                }
                if (!string.IsNullOrEmpty(txtSNumber.Text.Trim()))//工号
                {
                    sqlwhere += String.Format(" and StaffInfo_Number like  '%{0}%'", txtSNumber.Text.Trim());
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("StaffInfoForm.btnSeach_Click异常:" + "".ToString());
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
            for (int i = 0; i < this.dgvStaffInfo.Rows.Count; i++)
            {
                dgvStaffInfo.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslAllChecked()
        {
            for (int i = 0; i < dgvStaffInfo.Rows.Count; i++)
            {
                this.dgvStaffInfo.Rows[i].Selected = true;
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
                ISExecl = true;
                tslAllChecked();
                return;
            }
            if (e.ClickedItem.Name == "tslNotSelect")//取消全选
            {
                ISExecl = false;
                tslNotChecked();
                return;
            }
            if (e.ClickedItem.Name == "tlsbExecl")//导出Execl
            {

                tsbExecl_Click();
                return;
            }
            if (e.ClickedItem.Name == "tlsbInExecl")//导入Execl
            {
                InExecl();
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
            page.BindBoundControl(dgvStaffInfo, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_StaffInfo_ICCard", " case StaffInfo_Sex when 'False' then '女' else '男' end as StaffInfoSex,*", "StaffInfo_ID", "StaffInfo_ID", 0, sqlwhere, true);
        }
        #endregion

        /// <summary>
        /// 用户双击组件时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvStaffInfo_DoubleClick(object sender, EventArgs e)
        {
            this.btnUpdate.Enabled = true;
            this.btnSave.Enabled = false;
            if (this.dgvStaffInfo.SelectedRows.Count > 0)//选中行
            {
                if (dgvStaffInfo.SelectedRows.Count > 1)
                {
                    MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //修改的值
                    int ID = int.Parse(this.dgvStaffInfo.SelectedRows[0].Cells["StaffInfo_ID"].Value.ToString());
                    staffinid = ID;
                    Expression<Func<View_StaffInfo_ICCard, bool>> funviewinto = n => n.StaffInfo_ID == ID;
                    foreach (var n in StaffInfoDAL.Query(funviewinto))
                    {
                        // IC卡号
                        this.txtICCardValue.Text = n.ICCard_Value;
                        CommonalityEntity.strCardNo = n.ICCard_Value;

                        // 人员名称
                        this.txtStaffInfo_Name.Text = n.StaffInfo_Name;

                        // 人员类型
                        this.comboxStaffInfo_Type.Text = n.StaffInfo_Type;

                        // 人员性别
                        if (true == n.StaffInfo_Sex)
                        {
                            this.comboxStaffInfo_Sex.Text = "男";
                        }
                        else
                        {
                            this.comboxStaffInfo_Sex.Text = "女";
                        }

                        // 人员状态
                        this.comboxStaffInfo_State.Text = n.StaffInfo_State;
                        if (n.StaffInfo_License != null)
                        {
                            // 驾驶执照
                            this.txtStaffInfo_License.Text = n.StaffInfo_License;
                        }
                        if (n.StaffInfo_Identity != null)
                        {
                            // 身份证号
                            this.txtStaffInfo_Identity.Text = n.StaffInfo_Identity;
                            idns = n.StaffInfo_Identity;
                        }
                        if (n.StaffInfo_Phone != null)
                        {
                            // 人员电话
                            this.txtStaffInfo_Phone.Text = n.StaffInfo_Phone;
                        }
                        if (n.StaffInfo_Add != null)
                        {
                            // 人员地址
                            this.txtStaffInfo_Add.Text = n.StaffInfo_Add;
                        }
                        if (n.StaffInfo_Remark != null)
                        {
                            // 人员备注
                            this.txtStaffInfo_Remark.Text = n.StaffInfo_Remark;
                        }
                        if (this.dgvStaffInfo.SelectedRows[0].Cells["StaffInfo_Number"].Value != null)
                        {
                            txtNumber.Text = this.dgvStaffInfo.SelectedRows[0].Cells["StaffInfo_Number"].Value.ToString();
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
        /// 读卡信息是否正在使用。
        /// </summary>
        public bool ISCardTime = false;

        /// <summary>    
        /// (GetValue)：获取已选的实际值    
        /// </summary>    
        /// <param name="sender"></param>    
        /// <param name="e"></param>    
        private string GetValue(CheckedListBox chklb)
        {
            string checkedText = string.Empty;
            for (int i = 0; i < chklb.Items.Count; i++)
            {
                if (chklb.GetItemChecked(i))
                {
                    chklb.SetSelected(i, true);
                    checkedText += (String.IsNullOrEmpty(checkedText) ? "" : ",") + chklb.SelectedValue.ToString();
                }
            }
            return checkedText;
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
            return checkedText;
        }

        private void txtICCardValue_KeyUp(object sender, KeyEventArgs e)
        {
            string str = txtICCardValue.Text.Trim();
            if (str.Length == 10)
            {
                try
                {
                    txtICCardValue.Text = "0" + Convert.ToInt64(str).ToString("X");
                }
                catch
                {
                    txtICCardValue.Text = "";
                }
            }
            if (str.Length > 10)
            {
                txtICCardValue.Text = "";
            }
        }

        private void dgvStaffInfo_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        private void txtICCardValue_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CommonalityEntity.strCardNo))
            {
                txtICCardValue.Text = CommonalityEntity.strCardNo;
            }
        }
        /// <summary>
        /// 导出Excel 的方法
        /// </summary>
        private void tslExport_Excel(string fileName, DataGridView myDGV)
        {
            string saveFileName = "";
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xls";
            saveDialog.Filter = "Excel文件|*.xls";
            saveDialog.FileName = fileName;
            saveDialog.ShowDialog();
            saveFileName = saveDialog.FileName;
            if (saveFileName.IndexOf(":") < 0) return; //被点了取消 

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("无法创建Excel对象，可能您的机子未安装Excel");
                return;
            }
            ISdao = true;
            groupBox1.Visible = true;
            btnSet.Text = "取消导出";
            progressBar1.Maximum = myDGV.SelectedRows.Count;
            progressBar1.Value = 0;
            label18.Text = "正在导出：" + fileName;

            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1 

            Microsoft.Office.Interop.Excel.Range range = worksheet.get_Range("A1", "Z" + (myDGV.SelectedRows.Count + 10));
            range.NumberFormatLocal = "@";
            //写入标题
            for (int i = 1; i < myDGV.ColumnCount; i++)
            {
                worksheet.Cells[1, i] = myDGV.Columns[i].HeaderText;
            }
            //写入数值
            int s = 0;
            for (int r = 0; r < myDGV.SelectedRows.Count; r++)
            {
                if (ISdao)
                {
                    for (int i = 1; i < myDGV.ColumnCount; i++)
                    {
                        worksheet.Cells[s + 2, i] = myDGV.Rows[r].Cells[i].Value;
                    }
                    System.Windows.Forms.Application.DoEvents();
                    s++;
                    progressBar1.Value++;
                }
                else
                {
                    break;
                }
            }
            worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
            Microsoft.Office.Interop.Excel.Range rang = worksheet.get_Range(worksheet.Cells[2, 1], worksheet.Cells[myDGV.Rows.Count + 2, 2]);
            rang.NumberFormat = "000000000000";

            if (saveFileName != "")
            {
                try
                {
                    workbook.Saved = true;
                    workbook.SaveCopyAs(saveFileName);
                    //fileSaved = true;
                }
                catch
                {
                    //fileSaved = false;
                    MessageBox.Show("导出文件时出错,文件可能正被打开！\n");
                }

            }
            //else
            //{
            //    fileSaved = false;
            //}
            xlApp.Quit();
            GC.Collect();//强行销毁 
            if (progressBar1.Value >= progressBar1.Maximum)
            {
                label18.Text = fileName;
                btnSet.Text = "导出完成";
            }

        }

        /// <summary>
        /// 导入Execl
        /// </summary>
        private void InExecl()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "Execl文件(*.xls)|*.xls";
            openFileDialog.Multiselect = false;
            openFileDialog.RestoreDirectory = true;
            try
            {
                if (openFileDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string path = openFileDialog.FileName;
                    setExcelout(path);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("StaffInfoForm.InExecl异常:" + "".ToString());
            }
        }


        /// <summary>
        ///  将Excel中的数据导入到SQL数据库中
        /// </summary>
        /// <param name="path">路径</param>
        private void setExcelout(string path)
        {
            try
            {


                DataTable table = new DataTable();
                OleDbConnection dbcon = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path + ";Extended Properties=Excel 8.0");
                if (dbcon.State == ConnectionState.Closed)
                {
                    dbcon.Open();
                }
                string sql = "select * from [Sheet1$]";
                OleDbCommand cmd = new OleDbCommand(sql, dbcon);
                OleDbDataReader sdr = cmd.ExecuteReader();
                table.Load(sdr);
                string strstaffinfotype = "";//人员类型
                string strnumber = "";//工号
                string strstaName = ""; // 驾驶员名称
                string strIndes = "";//身份证号
                string strPhone = "";//手机
                bool issex = false;//性别
                string strstaffinfo_license = "";//驾驶证号
                string strICNumid = "";//IC卡号
                string strstate = "";//状态
                string strICType = "";//IC卡有效类型
                string ECount = "";//有效次数
                string HCount = "";//已使用次数
                string begintime = "NULL";//开始时间
                string endtime = "NULL";//结束时间
                string createtime = "";//创建时间
                string strAddress = "";//地址
                string strrmark = "";//备注

                string fileName = System.IO.Path.GetFileName(path);// 
                ISdao = true;
                groupBox1.Visible = true;
                btnSet.Text = "取消导入";
                progressBar1.Maximum = table.Rows.Count;
                progressBar1.Value = 0;
                label18.Text = "正在导入：" + fileName;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (ISdao)
                    {
                        strstaffinfotype = table.Rows[i][0].ToString().Trim();
                        strnumber = table.Rows[i][1].ToString().Trim();
                        strstaName = table.Rows[i][2].ToString().Trim();
                        if (!string.IsNullOrEmpty(strstaName))
                        {
                            strIndes = table.Rows[i][3].ToString().Trim();
                            strPhone = table.Rows[i][4].ToString().Trim();
                            if (table.Rows[i][5].ToString().Trim() == "男")
                            {
                                issex = true;
                            }
                            else
                            {
                                issex = false;
                            }
                            strstate = table.Rows[i][6].ToString().Trim();
                            strstaffinfo_license = table.Rows[i][7].ToString().Trim();

                            if (!string.IsNullOrEmpty(table.Rows[i][8].ToString().Trim()))
                            {
                                object objid = LinQBaseDao.GetSingle("select ICCard_ID from ICCard where ICCard_Value='" + table.Rows[i][8].ToString().Trim() + "'");
                                if (objid != null)
                                {
                                    strICNumid = objid.ToString();
                                }
                                else
                                {
                                    strICNumid = "NULL";
                                }
                            }
                            else
                            {
                                strICNumid = "NULL";
                            }

                            strICType = table.Rows[i][9].ToString().Trim();//IC卡有效类型
                            if (!string.IsNullOrEmpty(table.Rows[i][10].ToString().Trim()))//IC卡有效次数
                            {
                                ECount = table.Rows[i][10].ToString().Trim();
                            }
                            else
                            {
                                ECount = "NULL";
                            }
                            if (!string.IsNullOrEmpty(table.Rows[i][11].ToString().Trim()))//IC卡已使用次数
                            {
                                HCount = table.Rows[i][11].ToString().Trim();
                            }
                            else
                            {
                                HCount = "NULL";
                            }
                            if (!string.IsNullOrEmpty(table.Rows[i][12].ToString().Trim()))//IC卡有效开始时间
                            {
                                begintime = table.Rows[i][12].ToString().Trim();
                            }
                            else
                            {
                                begintime = "NULL";
                            }
                            if (!string.IsNullOrEmpty(table.Rows[i][13].ToString().Trim()))//IC卡有效结束时间
                            {
                                endtime = table.Rows[i][13].ToString().Trim();
                            }
                            else
                            {
                                endtime = "NULL";
                            }
                            strAddress = table.Rows[i][14].ToString().Trim();
                            createtime = table.Rows[i][15].ToString().Trim();
                            strrmark = table.Rows[i][16].ToString().Trim();
                            DataTable dt = LinQBaseDao.Query("select StaffInfo_ID from StaffInfo where StaffInfo_Name='" + strstaName + "' and StaffInfo_Identity='" + strIndes + "'").Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                LinQBaseDao.Query("update StaffInfo set StaffInfo_ICCard_ID=" + strICNumid + ",StaffInfo_Type='" + strstaffinfotype + "',StaffInfo_Name='" + strstaName + "',StaffInfo_Sex='" + issex + "',StaffInfo_Identity='" + strIndes + "',StaffInfo_State='" + strstate + "',StaffInfo_Phone='" + strPhone + "',StaffInfo_Add='" + strAddress + "',StaffInfo_License='" + strstaffinfo_license + "',StaffInfo_Time='" + createtime + "',StaffInfo_Remark='" + strrmark + "',StaffInfo_Number='" + strnumber + "' where StaffInfo_ID=" + dt.Rows[0][0].ToString());
                                if (!string.IsNullOrEmpty(strICNumid) && strICNumid != "NULL")
                                {
                                    if (begintime == "NULL")
                                    {
                                        LinQBaseDao.Query("update ICCard set ICCard_EffectiveType='" + strICType + "',ICCard_count=" + ECount + ",ICCard_HasCount=" + HCount + ",ICCard_BeginTime=" + begintime + ",ICCard_EndTime =" + endtime + " where  ICCard_ID=" + strICNumid);
                                    }
                                    else
                                    {
                                        LinQBaseDao.Query("update ICCard set ICCard_EffectiveType='" + strICType + "',ICCard_count=" + ECount + ",ICCard_HasCount=" + HCount + ",ICCard_BeginTime='" + begintime + "',ICCard_EndTime ='" + endtime + "' where  ICCard_ID=" + strICNumid);
                                    }
                                }
                            }
                            else
                            {
                                LinQBaseDao.Query("insert into StaffInfo(StaffInfo_ICCard_ID,StaffInfo_Type,StaffInfo_Name,StaffInfo_Sex,StaffInfo_Identity,StaffInfo_State,StaffInfo_Phone,StaffInfo_Add,StaffInfo_License,StaffInfo_Time,StaffInfo_Remark,StaffInfo_Number ) values (" + strICNumid + ",'" + strstaffinfotype + "','" + strstaName + "','" + issex + "','" + strIndes + "','" + strstate + "','" + strPhone + "','" + strAddress + "','" + strstaffinfo_license + "','" + createtime + "','" + strrmark + "','" + strnumber + "')");
                                if (!string.IsNullOrEmpty(strICNumid) && strICNumid != "NULL")
                                {
                                    if (begintime == "NULL")
                                    {
                                        LinQBaseDao.Query("update ICCard set ICCard_EffectiveType='" + strICType + "',ICCard_count=" + ECount + ",ICCard_HasCount=" + HCount + ",ICCard_BeginTime=" + begintime + ",ICCard_EndTime =" + endtime + " where  ICCard_ID=" + strICNumid);
                                    }
                                    else
                                    {
                                        LinQBaseDao.Query("update ICCard set ICCard_EffectiveType='" + strICType + "',ICCard_count=" + ECount + ",ICCard_HasCount=" + HCount + ",ICCard_BeginTime='" + begintime + "',ICCard_EndTime ='" + endtime + "' where  ICCard_ID=" + strICNumid);
                                    }
                                }
                            }
                        }
                        progressBar1.Value++;
                    }
                    else
                    {
                        break;
                    }
                }
                if (dbcon.State == ConnectionState.Open)
                {
                    dbcon.Close();
                }
                //MessageBox.Show(this, "导入成功！");
                if (progressBar1.Value == table.Rows.Count)
                {
                    label18.Text = fileName;
                    btnSet.Text = "导入完成";
                }
                CommonalityEntity.WriteLogData("修改", "驾驶员信息导入Execl信息", CommonalityEntity.USERNAME);
            }
            catch
            {
                MessageBox.Show(this, "导入失败！");
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = DBHelperAccess.GetFile();
            if (!string.IsNullOrEmpty(str))
            {
                MessageBox.Show(this, str);
                return;
            }
            DataTable dt = DBHelperAccess.Query("select * from iDRTable where [追加地址4] is null order by 编号 desc").Tables[0];
            if (dt.Rows.Count > 0)
            {
                txtStaffInfo_Name.Text = dt.Rows[0]["姓名"].ToString();
                txtStaffInfo_Identity.Text = dt.Rows[0]["公民身份号码"].ToString();
                txtStaffInfo_Add.Text = dt.Rows[0]["住址"].ToString();
                DBHelperAccess.Query("update iDRTable set [追加地址4]='1' where [追加地址4] is null");
            }
        }


        private void tsbExecl_Click()
        {
            string fileName = "人员信息Excel报表-" + CommonalityEntity.GetServersTime().ToShortDateString();
            if (!ISExecl)
            {
                tslExport_Excel(fileName, dgvStaffInfo);
            }
            else
            {
                btnSeach_Click(null, null);
                string strsql = "select StaffInfo_Type as 人员类型,StaffInfo_Number as 工号,StaffInfo_Name as 姓名,StaffInfo_Identity as 身份证号码,StaffInfo_Phone as 手机号, case StaffInfo_Sex when 'False' then '女' else '男' end  as 性别,StaffInfo_State as 人员状态,StaffInfo_License as 驾驶执照,ICCard_Value as IC卡卡号,ICCard_EffectiveType as IC卡有效类型,ICCard_count as 允许进厂次数,ICCard_HasCount as 已进厂次数,ICCard_BeginTime as 开始时间,ICCard_EndTime as 结束时间,StaffInfo_Add as 联系地址,StaffInfo_Time as 登记时间, StaffInfo_Remark as 人员备注 from View_StaffInfo_ICCard where  " + sqlwhere + " order by StaffInfo_ID ";
                daochu(fileName, strsql);
            }
        }

        bool ISdao = true;
        /// <summary>
        /// 取消导出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSet_Click(object sender, EventArgs e)
        {
            if (btnSet.Text == "取消导出")
            {
                ISdao = false;
            }
            else
            {
                btnSet.Text = "取消导出";
            }
            groupBox1.Visible = false;
        }

        private void dgvStaffInfo_Click(object sender, EventArgs e)
        {
            ISExecl = false;
        }
        /// <summary>
        /// 导出Execl 全选
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="tablename"></param>
        /// <param name="strsql"></param>
        private void daochu(string filename, string strsql)
        {
            try
            {
                string saveFileName = "";
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.DefaultExt = "xls";
                saveDialog.Filter = "Excel文件|*.xls";
                saveDialog.FileName = filename;
                saveDialog.ShowDialog();
                saveFileName = saveDialog.FileName;
                if (saveFileName.IndexOf(":") < 0) return; //被点了取消 

                ISdao = true;
                groupBox1.Visible = true;
                btnSet.Text = "取消导出";

                label18.Text = "正在导出：" + filename;

                ExcelFile excelFile = new ExcelFile();
                //ExcelWorksheet sheet = excelFile.Worksheets.Add(filename);
                DataTable table = LinQBaseDao.Query(strsql).Tables[0];
                int columns = table.Columns.Count;
                int rows = table.Rows.Count;
                double ss = (double)rows / 60000;
                int ii = Convert.ToInt32(ss);
                if (ss > ii)
                {
                    ii++;
                }
                progressBar1.Maximum = ii + 1;
                progressBar1.Value = 0;
                int y = 0;
                for (int s = 1; s < ii + 1; s++)
                {
                    ExcelWorksheet sheet = excelFile.Worksheets.Add("Sheet" + s.ToString());
                    for (int j = 0; j < columns; j++)
                    {
                        sheet.Cells[0, j].Value = table.Columns[j].ColumnName;
                    }
                    for (int i = 0; i < columns; i++)
                    {
                        if (ISdao)
                        {
                            y = 0;
                            for (int j = (s - 1) * 60000; j < rows; j++)
                            {
                                y++;
                                if (y > 60000)
                                {
                                    y = 0;
                                    break;
                                }
                                sheet.Cells[y, i].Value = table.Rows[j][i].ToString();
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (!ISdao)
                    {
                        break;
                    }
                    progressBar1.Value++;
                }
                excelFile.SaveXls(saveFileName);
                progressBar1.Value++;
                if (progressBar1.Value >= progressBar1.Maximum)
                {
                    label18.Text = filename;
                    btnSet.Text = "导出完成";
                }
            }
            catch { }
        }
    }
}
