using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.HelpClass;
using EMEWE.CarManagement.DAL;
using System.Linq.Expressions;
using System.Data.Linq.SqlClient;
using EMEWE.CarManagementDAL;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.CommonClass;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class PositionFrom : Form
    {
        public MainForm mf;//主窗体
        /// <summary>
        /// 返回一个空的预置皮重
        /// </summary>
        Expression<Func<View_Position_UserInfo, bool>> expr = null;
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
        /// 存放当前最大的ID数
        /// </summary>
        private int countID;
        public PositionFrom()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 门岗加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PositionFrom_Load(object sender, EventArgs e)
        {
            userContext();
            btnUpdate.Enabled = false;
            btnSelect_Click(btnSelect, null);  // 调用查询条件执行查询
            BindPosition();
            BindSearchPosition();
            mf = new MainForm();
            LoadData();
            this.cbxPosition_State.Text = "启动";
            this.txtPositionUserID.Text = common.NAME;

            countID = PositionDAL.MaxID("select MAX(Position_Value) from Position");
            txtPosition_Value.Text = "0" + (countID + 1);
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
                btnDriveway.Enabled = true;
                btnDriveway.Visible = true;
            }
            else
            {
                btnSave.Visible = ControlAttributes.BoolControl("btnSave", "PositionFrom", "Visible");
                btnSave.Enabled = ControlAttributes.BoolControl("btnSave", "PositionFrom", "Enabled");

                btnDel.Visible = ControlAttributes.BoolControl("btnDel", "PositionFrom", "Visible");
                btnDel.Enabled = ControlAttributes.BoolControl("btnDel", "PositionFrom", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "PositionFrom", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "PositionFrom", "Enabled");

                btnDriveway.Visible = ControlAttributes.BoolControl("btnDriveway", "PositionFrom", "Visible");
                btnDriveway.Enabled = ControlAttributes.BoolControl("btnDriveway", "PositionFrom", "Enabled");
            }
        }
        /// <summary>
        /// 菜单栏加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.dgvPositionList.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
                this.dgvPositionList.DataSource = null;

                LogInfoLoad("");
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("日记管理 LoadData()" );
            }
        }
        /// <summary>
        /// 绑定门岗状态
        /// </summary>
        private void BindPosition()
        {
            try
            {
                this.cbxPosition_State.DataSource = DictionaryDAL.GetValueDictionary("01").Where(n => n.Dictionary_Name != "全部").ToList();

                if (this.cbxPosition_State.DataSource != null)
                {
                    this.cbxPosition_State.DisplayMember = "Dictionary_Name";
                    this.cbxPosition_State.ValueMember = "Dictionary_ID";
                    this.cbxPosition_State.SelectedValue = -1;
                }
            }
            catch
            {
                MessageBox.Show("门岗管理“门岗状态”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 绑定门岗状态
        /// </summary>
        private void BindSearchPosition()
        {
            try
            {
                this.cbxState.DataSource = DictionaryDAL.GetValueStateDictionary("01");

                if (this.cbxState.DataSource != null)
                {
                    this.cbxState.DisplayMember = "Dictionary_Name";
                    this.cbxState.ValueMember = "Dictionary_ID";
                    this.cbxState.SelectedIndex = 3;
                }
            }
            catch
            {
                MessageBox.Show("门岗管理“门岗状态”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                //定义字段用以保存门岗名称和门岗值
                string PositionName = this.txtPosition_Name.Text.Trim();
                string PositionValue = this.txtPosition_Value.Text.Trim();
                //判断名称是否已存在
                Expression<Func<Position, bool>> funviewPosition = n => n.Position_Name == PositionName && n.Position_Name != this.dgvPositionList.SelectedRows[0].Cells["Position_Name"].Value.ToString();
                if (PositionDAL.Query(funviewPosition).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该门岗名称已存在", txtPosition_Name, this);
                    txtPosition_Name.Focus();
                    rbool = false; ;
                }
                Expression<Func<Position, bool>> funviewPosition1 = n => n.Position_Value == PositionValue && n.Position_Value != this.dgvPositionList.SelectedRows[0].Cells["Position_Value"].Value.ToString();
                if (PositionDAL.Query(funviewPosition1).Count() > 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "该门岗值已存在", txtPosition_Value, this);
                    txtPosition_Value.Focus();
                    rbool = false; ;
                }
                return rbool;

            }
            catch
            {
                CommonalityEntity.WriteTextLog("门岗管理 btnCheck()" );
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
                if (this.txtPosition_Name.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "门岗名称不能为空！", txtPosition_Name, this);
                    return;
                }

                int num = dgvPositionList.Rows.Count;
                int mgz = Convert.ToInt32(dgvPositionList.Rows[num - 1].Cells["Position_Value"].Value);
                txtPosition_Value.Text = "0" + (mgz + 1);


                int count = Convert.ToInt32(LinQBaseDao.GetSingle("select count(0) from Position").ToString());
                if (count >= SystemClass.postionCount)
                {
                    MessageBox.Show(this, "已超出可添加门岗上限！");
                    return;
                }
                if (!btnCheck()) return; // 去重复

                var Positionadd = new Position
                {
                    Position_Name = this.txtPosition_Name.Text.Trim(),
                    Position_Value = this.txtPosition_Value.Text.Trim(),
                    Position_State = this.cbxPosition_State.Text,
                    Position_UserId = int.Parse(common.USERID), // 用户编号
                    Position_Add = this.txtPosition_ADD.Text.Trim(),
                    Position_Phone = this.txtPosition_Phone.Text.Trim(),
                    Position_CreatTime = Convert.ToDateTime(CommonalityEntity.GetServersTime().ToString()),
                    Position_CameraValue = this.txtPosition_CameraValue.Text.Trim(),
                    Position_Remark = this.txtPosition_Remark.Text.Trim()
                };

                if (PositionDAL.InsertOneQCRecord(Positionadd))
                {
                    MessageBox.Show("添加成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string strContent1 = "门岗名称为：" + this.txtPosition_Name.Text.Trim(); ;
                    CommonalityEntity.WriteLogData("新增", "新增 " + strContent1 + " 的信息", common.USERNAME);//添加日志);  

                    //重新查询门岗值的最大值
                    countID = PositionDAL.MaxID("select MAX(Position_Value) from Position");
                    txtPosition_Value.Text = "0" + (countID + 1);
                }
                else
                {
                    MessageBox.Show("添加失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch 
            {
                common.WriteTextLog("门岗管理 btnSave_Click()");
            }
            finally
            {
                LogInfoLoad("");
                Empty();
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
                if (this.dgvPositionList.SelectedRows.Count > 0)//选中行
                {
                    if (dgvPositionList.SelectedRows.Count > 1)
                    {
                        MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        if (this.txtPosition_Name.Text == "")
                        {
                            PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "门岗名称不能为空！", txtPosition_Name, this);
                            return;
                        }
                        if (!btnCheck()) return; // 去重复
                        Expression<Func<Position, bool>> p = n => n.Position_ID == int.Parse(this.dgvPositionList.SelectedRows[0].Cells["Position_ID"].Value.ToString());
                        string id = "";
                        string strfront = "";
                        string strContent = "";
                        Action<Position> ap = s =>
                        {
                            strfront = s.Position_Name + "," + s.Position_Value + "," + s.Position_State + "," + s.Position_UserId + "," + s.Position_Add + "," + s.Position_Phone + "," + s.Position_CameraValue + "," + s.Position_Remark;
                            s.Position_Name = this.txtPosition_Name.Text.Trim();
                            s.Position_Value = this.txtPosition_Value.Text.Trim();
                            s.Position_State = this.cbxPosition_State.Text;
                            s.Position_UserId = int.Parse(common.USERID); // 用户编号
                            s.Position_Add = this.txtPosition_ADD.Text.Trim();
                            s.Position_Phone = this.txtPosition_Phone.Text.Trim();
                            s.Position_CameraValue = this.txtPosition_CameraValue.Text.Trim();
                            s.Position_Remark = this.txtPosition_Remark.Text.Trim();
                            strContent = s.Position_Name + "," + s.Position_Value + "," + s.Position_State + "," + s.Position_UserId + "," + s.Position_Add + "," + s.Position_Phone + "," + s.Position_CameraValue + "," + s.Position_Remark;
                        };

                        if (PositionDAL.Update(p, ap))
                        {
                            MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CommonalityEntity.WriteLogData("修改", "更新编号为：" + id + "的门岗信息；修改前：" + strfront + "；修改后：" + strContent, common.USERNAME);//添加日志
                            Empty();
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
                common.WriteTextLog("门岗管理 btnUpdate_Click()" );
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
            tbtnDelPosition(); // 调用 删除选中行数据的方法
        }
        /// <summary>
        /// 删除选中行数据的方法 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbtnDelPosition()
        {
            try
            {
                int j = 0;
                if (dgvPositionList.SelectedRows.Count > 0)//选中删除
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //选中数量
                        int count = dgvPositionList.SelectedRows.Count;
                        string id = "";
                        //遍历
                        for (int i = 0; i < count; i++)
                        {
                            int position_id = int.Parse(this.dgvPositionList.SelectedRows[i].Cells["Position_ID"].Value.ToString());
                            Expression<Func<Position, bool>> funuserinfo = n => n.Position_ID == position_id;
                            string strContent = LinQBaseDao.GetSingle("select Position_Name from Position where Position_ID=" + position_id).ToString();
                            if (PositionDAL.DeleteToMany(funuserinfo))
                            {
                                j++;
                                CommonalityEntity.WriteLogData("删除", "删除门岗名称为：" + strContent + " 的信息", common.USERNAME);//添加日志
                            }
                        }
                        if (j == 0)
                        {
                            MessageBox.Show("删除失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            MessageBox.Show("成功删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            //重新查询门岗值的最大值
                            countID = PositionDAL.MaxID("select MAX(Position_Value) from Position");
                            txtPosition_Value.Text = "0" + (countID + 1);
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

                common.WriteTextLog("门岗管理 tbtnDelPosition()+");
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
        private void btnCancel_Click(object sender, EventArgs e)
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
            this.txtPosition_Name.Text = "";
            this.txtPosition_Value.Text = "0" + (countID + 1);
            this.cbxPosition_State.SelectedItem = 0;

            this.txtPosition_ADD.Text = "";
            this.txtPosition_Phone.Text = "";
            this.txtPosition_CameraValue.Text = "";

            this.txtPosition_Remark.Text = "";
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
                GetDictionarySeach();
                //LogInfoDAL.AddLoginfo("查询", "检测项目查询", CommonalityEntity.USERNAME);//操作日志
                btnSelect.Enabled = true;
            }
        }
        /// <summary>
        /// 搜索的方法
        /// </summary>
        private void GetDictionarySeach()
        {
            try
            {
                sqlwhere = "  1=1";
                string name = this.txtPositionName.Text.Trim();
                string value = this.txtPositionValue.Text.Trim();
                string state = this.cbxState.Text;

                if (!string.IsNullOrEmpty(state))//门岗状态
                {
                    if (state == "全部")
                    {
                        sqlwhere = "  1=1";
                    }
                    else
                    {
                        sqlwhere += String.Format(" and Position_State like  '%{0}%'", state);
                    }
                }
                if (!string.IsNullOrEmpty(name))//门岗名称
                {
                    sqlwhere += String.Format(" and Position_Name like  '%{0}%'", name);
                }
                if (!string.IsNullOrEmpty(value))//门岗值
                {
                    sqlwhere += String.Format(" and Position_Value like  '%{0}%'", value);
                }

            }
            catch 
            {
                CommonalityEntity.WriteTextLog("PositionForm.GetDictionarySeach异常:" );
            }
            finally
            {
                LogInfoLoad("");
            }
        }
        private void BindDictionary(string itemName)
        {

            try
            {
                GetDictionarySeach();
                this.dgvPositionList.AutoGenerateColumns = false;
                ////dgvDictioanry.DataSource = page.BindBoundControl<Dictionary>(itemName, txtCurrentPage1, lblPageCount1, expr);
                //dgvDictioanry.DataSource = "select * from Dictionary";
                LogInfoLoad("");
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("PositionForm.BindDictionary异常:");
            }

        }

        #region 分页和加载DataGridView
        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslNotSelect()
        {
            for (int i = 0; i < this.dgvPositionList.Rows.Count; i++)
            {
                dgvPositionList.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslSelectAll()
        {
            for (int i = 0; i < dgvPositionList.Rows.Count; i++)
            {
                this.dgvPositionList.Rows[i].Selected = true;
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
            page.BindBoundControl(dgvPositionList, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_Position_UserInfo", "*", "Position_ID", "Position_ID", 0, sqlwhere, true);
        }
        #endregion

        /// <summary>
        /// “通道” 按钮的单击事件  点击跳转到通道管理界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDriveway_Click(object sender, EventArgs e)
        {
            if (this.dgvPositionList.SelectedRows.Count > 0)//选中行
            {
                if (dgvPositionList.SelectedRows.Count > 1)
                {
                    MessageBox.Show("只能选中一行数据进行链接！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    iPositionID = int.Parse(this.dgvPositionList.SelectedRows[0].Cells["Position_ID"].Value.ToString());

                    DrivewayFrom df = new DrivewayFrom();
                    df.Owner = this;
                    df.iDrivewayPositionID = iPositionID; // 将选中数据的门岗编号ID赋值给通道外键的门岗编号ID
                    df.Show();
                }
            }
            else
            {
                DrivewayFrom df = new DrivewayFrom();
                df.Owner = this;
                df.Show();
            }
        }

        /// <summary>
        /// 用户双击组件时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvPositionList_DoubleClick(object sender, EventArgs e)
        {
            this.btnUpdate.Enabled = true;
            this.btnSave.Enabled = false;
            if (this.dgvPositionList.SelectedRows.Count > 0)//选中行
            {
                if (dgvPositionList.SelectedRows.Count > 1)
                {
                    MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //修改的值
                    int ID = int.Parse(this.dgvPositionList.SelectedRows[0].Cells["Position_ID"].Value.ToString());
                    Expression<Func<Position, bool>> funviewinto = n => n.Position_ID == ID;
                    foreach (var n in PositionDAL.Query(funviewinto))
                    {
                        if (n.Position_Name != null)
                        {
                            //门岗名称
                            this.txtPosition_Name.Text = n.Position_Name;
                        }
                        if (n.Position_Value != null)
                        {
                            // 门岗值
                            this.txtPosition_Value.Text = n.Position_Value;
                        }
                        //if (n.UserInfo.UserName != null)
                        //{
                        //    // 门岗创建人
                        //    this.txtPositionUserID.Text = n.UserInfo.UserName;
                        //}
                        if (n.Position_State != null)
                        {
                            // 门岗状态
                            this.cbxPosition_State.Text = n.Position_State;
                        }
                        if (n.Position_Add != null)
                        {
                            // 门岗地址
                            this.txtPosition_ADD.Text = n.Position_Add;
                        }
                        if (n.Position_Phone != null)
                        {
                            // 门岗电话
                            this.txtPosition_Phone.Text = n.Position_Phone;
                        }
                        if (n.Position_CameraValue != null)
                        {
                            // 登记拍照摄像头
                            this.txtPosition_CameraValue.Text = n.Position_CameraValue;
                        }
                        if (n.Position_Remark != null)
                        {
                            // 门岗备注
                            this.txtPosition_Remark.Text = n.Position_Remark;
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
        /// 门岗值只能为数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPosition_Value_KeyPress(object sender, KeyPressEventArgs e)
        {
            //阻止从键盘输入键
            e.Handled = true;

            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == (char)8))
            {

                if ((e.KeyChar == (char)8)) { e.Handled = false; return; }
                else
                {
                    int len = this.txtPosition_Value.Text.Length;
                    if (len < 2)
                    {
                        if (len == 0 && e.KeyChar != '0')
                        {
                            e.Handled = false; return;
                        }
                        //else if (len == 0)
                        //{
                        //    MessageBox.Show("编号不能以0开头！"); return;
                        //}
                        e.Handled = false; return;
                    }
                    else
                    {
                        MessageBox.Show("编号最多只能输入2位数字！");
                    }
                }
            }
            else
            {
                MessageBox.Show("编号只能输入数字！");
            }
        }
    }
}
