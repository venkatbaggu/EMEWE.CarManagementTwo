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
    public partial class FVNForm : Form
    {
        public FVNForm()
        {
            InitializeComponent();
        }
        
        public static MainForm mf; // 主窗体
        /// <summary>
        /// 返回一个空的预置皮重
        /// </summary>
        Expression<Func<View_FVN_Driveway_Position, bool>> expr = null;
        /// <summary>
        /// 实例化分页控件
        /// </summary>
        PageControl page = new PageControl();
        // 定义一个公共的变量： 地感编号
        public int iFvnID = 0;
        // 定义一个公共的变量： 门岗编号
        public int iDrivewayPositionID = 0;
        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere = " 1=1";

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
                btnSave.Visible = ControlAttributes.BoolControl("btnSave", "FVNForm", "Visible");
                btnSave.Enabled = ControlAttributes.BoolControl("btnSave", "FVNForm", "Enabled");

                btnDel.Visible = ControlAttributes.BoolControl("btnDel", "FVNForm", "Visible");
                btnDel.Enabled = ControlAttributes.BoolControl("btnDel", "FVNForm", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "FVNForm", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "FVNForm", "Enabled");
            }
        }

        // 定义一个全局变量： 通道编号
        /// <summary>
        /// Load 加载 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FVNForm_Load(object sender, EventArgs e)
        {
            try
            {
                userContext();
                btnUpdate.Enabled = false;
                btnSelect_Click(btnSelect, null);
                mf = new MainForm();
                LoadData();// 调用显示DatagridView的方法

                BindMenGang(); // 调用绑定门岗的方法
                BindFVN();
                BindSearchFVN();
                BindFVNType();
                BindSearchFVNType();
                BindMenGang1();
                DrivewayName();
                if (iFvnID > 0)
                {
                    // 若通道管理界面中有选择“通道”再跳转的，则显示如下
                    Expression<Func<View_DrivewayPosition, bool>> funviewinto = n => n.Driveway_ID == iFvnID;
                    foreach (var n in DrivewayDAL.Query(funviewinto))
                    {
                        if (n.Position_Name != null && n.Driveway_Name != null)
                        {
                            this.cbxPositionName.Text = n.Position_Name;
                            this.cbxDrivewayName.Text = n.Driveway_Name + n.Driveway_Type;
                            //string PName = n.Position_Name; //获取数据库读取到的门岗名称
                            //string DName = n.Driveway_Name; 
                            //var name = DrivewayDAL.GetViewDrivewayName(String.Format("select * from View_DrivewayPosition where Position_Name='{0}' and Driveway_Name='{1}'", PName, DName));
                            //if (name != null)
                            //{
                            //    foreach (var item in name)
                            //    {
                            //        if (item.Driveway_Name != null)
                            //        {
                            //            if (item.Driveway_Type != null)
                            //            {
                            //                this.cbxDrivewayName.Text = item.Driveway_Name + item.Driveway_Type;
                            //                cbxDrivewayName.SelectedIndex = 0;
                            //            }
                            //        }
                            //    }
                            //}
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("加载地感信息有误，请查看与地感相关的信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 菜单栏加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.dgvFVN.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
                this.dgvFVN.DataSource = null;
                LogInfoLoad("");
            }
            catch
            {
                CommonalityEntity.WriteTextLog("日记管理 LoadData()" );
            }
        }
        /// <summary>
        /// 绑定门岗
        /// </summary>
        private void BindMenGang()
        {
            try
            {
                string sql = "select * from Position where Position_State='启动'";
                this.cbxPositionName.DataSource = PositionDAL.GetViewPosition(sql);

                if (PositionDAL.GetViewPosition(sql).Count() > 0)
                {
                    this.cbxPositionName.DisplayMember = "Position_Name";
                    this.cbxPositionName.ValueMember = "Position_ID";
                    cbxPositionName.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("门岗暂无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("地感管理“门岗”绑定有误，请查看门岗信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 绑定地感状态
        /// </summary>
        private void BindFVN()
        {
            try
            {
                this.cbxFVN_State.DataSource = DictionaryDAL.GetValueDictionary("01").Where(n => n.Dictionary_Name != "全部").ToList();
                
                if (this.cbxFVN_State.DataSource != null)
                {
                    this.cbxFVN_State.DisplayMember = "Dictionary_Name";
                    this.cbxFVN_State.ValueMember = "Dictionary_ID";
                    this.cbxFVN_State.SelectedValue = -1;
                }
            }
            catch
            {
                MessageBox.Show("地感管理“地感状态”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 绑定地感类型
        /// </summary>
        private void BindFVNType()
        {
            try
            {
                this.cbxFVN_Type.DataSource = DictionaryDAL.GetValueDictionary("17").Where(n => n.Dictionary_Name != "全部").ToList();
                
                if (this.cbxFVN_Type.DataSource != null)
                {
                    this.cbxFVN_Type.DisplayMember = "Dictionary_Name";
                    this.cbxFVN_Type.ValueMember = "Dictionary_ID";
                    this.cbxFVN_Type.SelectedValue = -1;
                }
            }
            catch
            {
                MessageBox.Show("地感管理“地感类型”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 搜索 --绑定地感类型
        /// </summary>
        private void BindSearchFVNType()
        {
            try
            {
                this.cbxFvnType.DataSource = DictionaryDAL.GetValueStateDictionary("17");
                
                if (this.cbxFvnType.DataSource != null)
                {
                    this.cbxFvnType.DisplayMember = "Dictionary_Name";
                    this.cbxFvnType.ValueMember = "Dictionary_ID";
                    this.cbxFvnType.SelectedIndex = 2;
                }
            }
            catch
            {
                MessageBox.Show("地感管理“地感类型”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 搜索 --绑定地感状态
        /// </summary>
        private void BindSearchFVN()
        {
            try
            {
                this.cbxFvnState.DataSource = DictionaryDAL.GetValueStateDictionary("01");
                
                if (this.cbxFvnState.DataSource != null)
                {
                    this.cbxFvnState.DisplayMember = "Dictionary_Name";
                    this.cbxFvnState.ValueMember = "Dictionary_ID";
                    this.cbxFvnState.SelectedIndex = 3;
                }
            }
            catch
            {
                MessageBox.Show("地感管理“地感状态”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 搜索 --绑定门岗
        /// </summary>
        private void BindMenGang1()
        {
            try
            {
                string sql = "select * from Position where Position_State='启动'";
                this.comboxMenGang.DataSource = PositionDAL.GetViewPosition(sql);

                if (comboxMenGang.DataSource != null)
                {
                    this.comboxMenGang.DisplayMember = "Position_Name";
                    this.comboxMenGang.ValueMember = "Position_ID";
                    comboxMenGang.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("门岗暂无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("地感管理“门岗”绑定有误，请查看门岗信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 搜索 --绑定通道
        /// </summary>
        private void DrivewayName()
        {
            try
            {
                string sql = "select distinct Driveway_Name from Driveway";
                this.comboxTongDao.DataSource = LinQBaseDao.Query(sql).Tables[0];

                if (this.comboxTongDao.DataSource != null)
                {
                    
                    this.comboxTongDao.DisplayMember = "Driveway_Name";
                    //this.comboxTongDao.ValueMember = "Driveway_ID";
                    comboxTongDao.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("通道暂无数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {
                MessageBox.Show("地感管理“通道”绑定有误，请查看通道信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 在门岗名称上更改SelectedValueChanged的属性值时引发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPositionName_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbxDrivewayName.Items.Count > 0)
            {
                this.cbxDrivewayName.Items.Clear();
            }
            if (cbxPositionName.Text == "") { return; }

            string PositionName = this.cbxPositionName.Text;
            var name = DrivewayDAL.GetViewDrivewayName(String.Format("select * from View_DrivewayPosition where Position_Name='{0}' and Driveway_State='启动'", PositionName));
            if (name != null)
            {
                foreach (var item in name)
                {
                    if (item.Driveway_Name != null)
                    {
                        if (item.Driveway_Type != null)
                        {
                            cbxDrivewayName.Items.Add(item.Driveway_Name);
                            cbxDrivewayName.SelectedIndex = 0;
                        }
                    }
                }
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
                string PositionName = this.cbxPositionName.Text.ToString();
                int lenth = cbxDrivewayName.Text.Trim().Length;
                string DrivewayName = cbxDrivewayName.Text.Trim().Substring(0, 4);
                string DrivewayType = cbxDrivewayName.Text.Trim().Substring(lenth - 1, 1);
                string name = this.txtFVN_Name.Text.Trim();
                int value = int.Parse(this.txtFVN_Value.Text.Trim());
                //判断名称是否已存在 
                Expression<Func<View_FVN_Driveway_Position, bool>>funviewFVNInfo = n => n.Position_Name == PositionName && n.Driveway_Name == DrivewayName && n.FVN_Name == name;
                if (FVNDAL.QueryView(funviewFVNInfo).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该地感名称已存在", txtFVN_Name, this);
                    this.txtFVN_Name.Focus();
                    rbool = false; ;
                }
                Expression<Func<View_FVN_Driveway_Position, bool>>funviewFVNInfo1 = n => n.Position_Name == PositionName && n.Driveway_Name == DrivewayName && n.FVN_Value == value;
                if (FVNDAL.QueryView(funviewFVNInfo1).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该地感值已存在", txtFVN_Value, this);
                    this.txtFVN_Value.Focus();
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
                string PositionName = this.cbxPositionName.Text.ToString();
                int lenth = cbxDrivewayName.Text.Trim().Length;
                string DrivewayName = cbxDrivewayName.Text.Trim().Substring(0, 4);
                string DrivewayType = cbxDrivewayName.Text.Trim().Substring(lenth - 1, 1);
                string name = this.txtFVN_Name.Text.Trim();
                int value = int.Parse(this.txtFVN_Value.Text.Trim());
                //判断名称是否已存在 
                Expression<Func<View_FVN_Driveway_Position, bool>>funviewFVNInfo = n => n.Position_Name == PositionName && n.Driveway_Name == DrivewayName && n.FVN_Name == name && n.FVN_Name != this.dgvFVN.SelectedRows[0].Cells["FVN_Name"].Value.ToString();
                if (FVNDAL.QueryView(funviewFVNInfo).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该地感名称已存在", txtFVN_Name, this);
                    this.txtFVN_Name.Focus();
                    rbool = false; ;
                }
                Expression<Func<View_FVN_Driveway_Position, bool>>funviewFVNInfo1 = n => n.Position_Name == PositionName && n.Driveway_Name == DrivewayName && n.FVN_Value == value && n.FVN_Value != int.Parse(this.dgvFVN.SelectedRows[0].Cells["FVN_Value"].Value.ToString());
                if (FVNDAL.QueryView(funviewFVNInfo1).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该地感值已存在", txtFVN_Value, this);
                    this.txtFVN_Value.Focus();
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
        /// “保存” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtFVN_Name.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "地感名称不能为空！", txtFVN_Name, this);
                    return;
                }
                if (this.txtFVN_Value.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "地感PLC值不能为空！", txtFVN_Value, this);
                    return;
                }
                string PositionName = this.cbxPositionName.Text.ToString();
                int lenth = cbxDrivewayName.Text.Trim().Length;
                string DrivewayName = cbxDrivewayName.Text.Trim().Substring(0, 4);
                string DrivewayType = cbxDrivewayName.Text.Trim().Substring(lenth - 1, 1);
                int DrivewayID = 0;
                var p = DrivewayDAL.GetViewDrivewayName(String.Format("select Driveway_ID from View_DrivewayPosition where Position_Name='{0}' and  Driveway_Name= '{1}' and Driveway_Type= '{2}'", PositionName, DrivewayName, DrivewayType));
                if (p != null)
                {
                    foreach (var n in p)
                    {
                        if (n.Driveway_ID > 0)
                        {
                            DrivewayID = n.Driveway_ID;
                        }
                        break;
                    }
                }
                if (!btnCheck()) return; // 去重复
                var FVNadd = new FVNInfo
                {
                    FVN_Driveway_ID = DrivewayID,
                    FVN_Value = int.Parse(this.txtFVN_Value.Text),
                    FVN_Name = this.txtFVN_Name.Text.Trim(),
                    FVN_State = this.cbxFVN_State.Text,
                    FVN_Remark = this.txtFVN_Remark.Text.Trim(),
                    FVN_Type = this.cbxFVN_Type.Text
                };
                if (FVNDAL.InsertOneQCRecord(FVNadd))
                {
                    MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string strContent1 = "地感名称为：" + this.txtFVN_Name.Text.Trim(); 
                    CommonalityEntity.WriteLogData("新增", "新增 " + strContent1 + " 的信息", common.NAME);//添加日志
                }
                else
                {
                    MessageBox.Show("添加失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("地感管理 btnSave_Click()");
            }
            finally
            {
                LogInfoLoad("");
            }
        }

        /// <summary>
        /// “修改” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtFVN_Name.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "地感名称不能为空！", txtFVN_Name, this);
                    return;
                }
                if (this.txtFVN_Value.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "地感PLC值不能为空！", txtFVN_Value, this);
                    return;
                }
                string PositionName = this.cbxPositionName.Text.ToString();
                int lenth = cbxDrivewayName.Text.Trim().Length;
                string DrivewayName = cbxDrivewayName.Text.Trim();
                string DrivewayType = cbxFVN_Type.Text.Trim();
                int DrivewayID = 0;
                var p = DrivewayDAL.GetViewDrivewayName(String.Format("select Driveway_ID from View_DrivewayPosition where Position_Name='{0}' and  Driveway_Name= '{1}'", PositionName, DrivewayName));
                if (p != null)
                {
                    foreach (var n in p)
                    {
                        if (n.Driveway_ID > 0)
                        {
                            DrivewayID = n.Driveway_ID;
                        }
                        break;
                    }
                }
                if (this.dgvFVN.SelectedRows.Count > 0)//选中行
                {
                    if (dgvFVN.SelectedRows.Count > 1)
                    {
                        MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (!btnCheckupdate()) return; // 去重复
                        #region  修改
                        string id = "";
                        string strfront = "";
                        string strcontent = "";
                        Expression<Func<FVNInfo, bool>> pf = n => n.FVN_ID == int.Parse(this.dgvFVN.SelectedRows[0].Cells["FVN_ID"].Value.ToString());
                        Action<FVNInfo> ap = s =>
                        {
                            strfront = s.FVN_Driveway_ID + "," + s.FVN_Value + "," + s.FVN_Name + "," + s.FVN_State + "," + s.FVN_Remark + "," + s.FVN_Type;
                            s.FVN_Driveway_ID = DrivewayID;
                            s.FVN_Value = int.Parse(this.txtFVN_Value.Text);
                            s.FVN_Name = this.txtFVN_Name.Text.Trim();
                            s.FVN_State = this.cbxFVN_State.Text;
                            s.FVN_Remark = this.txtFVN_Remark.Text.Trim();
                            s.FVN_Type = this.cbxFVN_Type.Text;
                            strcontent = s.FVN_Driveway_ID + "," + s.FVN_Value + "," + s.FVN_Name + "," + s.FVN_State + "," + s.FVN_Remark + "," + s.FVN_Type;
                            id = s.FVN_ID.ToString();
                        };
                        if (FVNDAL.Update(pf, ap))
                        {
                            MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CommonalityEntity.WriteLogData("修改", "更新编号为：" + id + "的地感信息；修改前：" + strfront + "；修改后：" + strcontent, CommonalityEntity.USERNAME);//添加日志
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
                CommonalityEntity.WriteTextLog("地感管理 btnUpdate_Click()" );
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
            tslDelFVN(); // 调用 删除选中行数据的方法
        }
        /// <summary>
        ///删除选中行数据的方法 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslDelFVN()
        {
            try
            {
                int j = 0;
                if (dgvFVN.SelectedRows.Count > 0)//选中删除
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //选中数量
                        int count = dgvFVN.SelectedRows.Count;
                        string id = "";
                        //遍历
                        for (int i = 0; i < count; i++)
                        {
                            int fvnid=int.Parse(this.dgvFVN.SelectedRows[i].Cells["FVN_ID"].Value.ToString());
                            Expression<Func<FVNInfo, bool>> funuserinfo = n => n.FVN_ID == fvnid;
                            string strContent = LinQBaseDao.Query("select FVN_Name from FVNInfo where FVN_ID=" + fvnid).Tables[0].Rows[0][0].ToString();
                            if (FVNDAL.DeleteToMany(funuserinfo))
                            {
                                j++;
                                CommonalityEntity.WriteLogData("删除", "删除地感名称为 " + strContent + " 的信息", common.USERNAME);//添加日志
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

                CommonalityEntity.WriteTextLog("地感管理 tslDelFVN()+" );
            }
            finally
            {
                LogInfoLoad("");
            }
        }
        
        /// <summary>
        /// 清空 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEmpty_Click(object sender, EventArgs e)
        {
            Empty(); // 调用清空的方法
            this.btnUpdate.Enabled = false;
            this.btnSave.Enabled = true;
        }
        /// <summary>
        /// 清空文本框的方法
        /// </summary>
        private void Empty()
        {            
            this.txtFVN_Value.Text = "";
            this.txtFVN_Name.Text = "";
            this.cbxFVN_State.SelectedValue = 1;
            this.txtFVN_Remark.Text = "";
            this.cbxFVN_Type.SelectedValue = 1;

            this.cbxPositionName.SelectedValue = 1;
            this.cbxDrivewayName.SelectedValue = 1;
            this.comboxMenGang.SelectedIndex = -1;
            this.comboxTongDao.SelectedIndex = -1;
        }

        /// <summary>
        /// 搜 索
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
                string name = this.txtFVNName.Text.Trim();
                string type = this.cbxFvnType.Text;
                string state = this.cbxFvnState.Text;
                string DrivewayName = this.comboxTongDao.Text.Trim();
                string PositionName = this.comboxMenGang.Text.Trim();

                if (!string.IsNullOrEmpty(type))//地感类型
                {
                    if (type == "全部")
                    {
                       
                    }
                    else
                    {
                        sqlwhere += String.Format(" and FVN_Type like  '%{0}%'", type);
                    }
                }
                if (!string.IsNullOrEmpty(state))//地感状态
                {
                    if (state == "全部")
                    {
                        
                    }
                    else
                    {
                        sqlwhere += String.Format(" and FVN_State like  '%{0}%'", state);
                    }
                }
                if (!string.IsNullOrEmpty(PositionName))//门岗名称
                {
                    if (PositionName == "")
                    {
                        
                    }
                    else
                    {
                        sqlwhere += String.Format(" and Position_Name like  '%{0}%'", PositionName);
                    }
                }
                if (!string.IsNullOrEmpty(DrivewayName))//通道名称
                {
                    if (DrivewayName == "")
                    {
                        
                    }
                    else
                    {
                        sqlwhere += String.Format(" and Driveway_Name like  '%{0}%'", DrivewayName);
                    }
                }
                if (!string.IsNullOrEmpty(name))//地感名称
                {
                    sqlwhere += String.Format(" and FVN_Name like  '%{0}%'", name);
                }
                
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("FVNForm.selectTJ异常:");
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
        private void NotCheck()
        {
            for (int i = 0; i < this.dgvFVN.Rows.Count; i++)
            {
                dgvFVN.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllCheck()
        {
            for (int i = 0; i < dgvFVN.Rows.Count; i++)
            {
                this.dgvFVN.Rows[i].Selected = true;
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
                AllCheck();
                return;
            }
            if (e.ClickedItem.Name == "tslNotCheck")//取消全选
            {
                NotCheck();
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
            page.BindBoundControl(dgvFVN, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_FVN_Driveway_Position", "*", "FVN_ID", "FVN_ID", 0, sqlwhere, true);
        }
        #endregion 

        /// <summary>
        /// 双击组件时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvFVN_DoubleClick(object sender, EventArgs e)
        {
            this.btnUpdate.Enabled = true;
            this.btnSave.Enabled = false;
            if (this.dgvFVN.SelectedRows.Count > 0)//选中行
            {
                if (dgvFVN.SelectedRows.Count > 1)
                {
                    MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //修改的值
                    int ID = int.Parse(this.dgvFVN.SelectedRows[0].Cells["FVN_ID"].Value.ToString());
                    Expression<Func<View_FVN_Driveway_Position, bool>>funviewinto = n => n.FVN_ID == ID;
                    foreach (var n in FVNDAL.QueryView(funviewinto))
                    {
                        if (n.FVN_Name != null)
                        {
                            // 地感名称
                            this.txtFVN_Name.Text = n.FVN_Name;
                        }
                        if (n.FVN_Type != null)
                        {
                            // 地感类型
                            this.cbxFVN_Type.Text = n.FVN_Type;
                        }
                        if (n.FVN_State != null)
                        {
                            // 地感状态
                            this.cbxFVN_State.Text = n.FVN_State;
                        }
                        if (n.FVN_Value != null)
                        {
                            // 地感PLC值
                            this.txtFVN_Value.Text = n.FVN_Value.ToString();
                        }
                        if (n.Position_Name != null)
                        {
                            // 门岗名称
                            this.cbxPositionName.Text = n.Position_Name;
                        }
                        if (n.FVN_Driveway_ID != null)
                        {
                            // 通道名称
                            this.cbxDrivewayName.Text = n.Driveway_Name + n.Driveway_Type;
                        }

                        if (n.FVN_Remark != null || n.FVN_Remark != "")
                        {
                            // 地感备注
                            this.txtFVN_Remark.Text = n.FVN_Remark;
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

        private bool nonNumberEntered = false;
        /// <summary>
        /// 地感的PLC输入值 键盘事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtFVN_Value_KeyPress(object sender, KeyPressEventArgs e)
        {
            //阻止从键盘输入键
            e.Handled = true;

            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == (char)8))
            {

                if ((e.KeyChar == (char)8)) 
                { e.Handled = false; return; }
                else
                {
                    int len = this.txtFVN_Value.Text.Length;
                    if (len < 2)
                    {
                        if (len == 0 && e.KeyChar != '0')
                        {
                            e.Handled = false; return;
                        }
                        e.Handled = false; return;
                    }
                    else
                    {
                        MessageBox.Show("地感PLC值最多只能输入2位数字！");
                    }
                }
            }
            else
            {
                MessageBox.Show("地感PLC值只能输入数字！");
            }
        }

        private void dgvFVN_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }
    }
}
