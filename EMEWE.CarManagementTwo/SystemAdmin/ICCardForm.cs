using EMEWE.CarManagement.CommonClass;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using GemBox.ExcelLite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class ICCardForm : Form
    {
        public ICCardForm()
        {
            InitializeComponent();
        }

        //public MainForm mf;//主窗体
        /// <summary>
        /// 返回一个空的预置皮重
        /// </summary>
        Expression<Func<View_ICCard_ICCardType, bool>> expr = null;
        /// <summary>
        /// 实例化分页控件
        /// </summary>
        PageControl page = new PageControl();
        List<MenuInfo> list = new List<MenuInfo>();
        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere;

        /// <summary>
        /// IC卡ID
        /// </summary>
        private int iccarid = 0;
        private bool isbool = false;
        /// <summary>
        /// 是否导出所有
        /// </summary>
        private bool ISExecl = false;
        /// <summary>
        /// Load 加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ICCardForm_Load(object sender, EventArgs e)
        {
            userContext();
            txtICCard_Value.Text = CommonalityEntity.strCardNo;
            btnUpdate.Enabled = false;
            this.panel1.Visible = false;
            // btnSearch_Click(btnSearch, null);  // 调用查询条件执行查询
            BindICTypeName();
            BindICCard();
            BindICType();
            BindSearchICCard();
            this.comboxICCard_State.Text = "启动";
            tscbxPageSize.SelectedIndex = 1;
            groupBox2.Visible = false;
            //LoadData();
            //mf = new MainForm();
            txtICCard_Value.Text = CommonalityEntity.strCardNo;
            cmbEType.SelectedIndex = 0;
            MyUserCar.Visible = false;
            MyUserStaName.Visible = false;
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
                btnICTypeForm.Enabled = true;
                btnICTypeForm.Visible = true;
                tslbExecl.Enabled = true;
                tslbExecl.Visible = true;
                tslbInExecl.Enabled = true;
                tslbInExecl.Visible = true;
            }
            else
            {
                btnSave.Visible = ControlAttributes.BoolControl("btnSave", "ICCardForm", "Visible");
                btnSave.Enabled = ControlAttributes.BoolControl("btnSave", "ICCardForm", "Enabled");

                btnICTypeForm.Visible = ControlAttributes.BoolControl("btnICTypeForm", "ICCardForm", "Visible");
                btnICTypeForm.Enabled = ControlAttributes.BoolControl("btnICTypeForm", "ICCardForm", "Enabled");

                btnDel.Visible = ControlAttributes.BoolControl("btnDel", "ICCardForm", "Visible");
                btnDel.Enabled = ControlAttributes.BoolControl("btnDel", "ICCardForm", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "ICCardForm", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "ICCardForm", "Enabled");

                tslbExecl.Visible = ControlAttributes.BoolControl("tslbExecl", "ICCardForm", "Visible");
                tslbExecl.Enabled = ControlAttributes.BoolControl("tslbExecl", "ICCardForm", "Enabled");

                tslbInExecl.Visible = ControlAttributes.BoolControl("tslbInExecl", "ICCardForm", "Visible");
                tslbInExecl.Enabled = ControlAttributes.BoolControl("tslbInExecl", "ICCardForm", "Enabled");
            }
        }
        /// <summary>
        /// 菜单栏加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.dgvICInfo.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
                this.dgvICInfo.DataSource = null;
                LogInfoLoad("");
            }
            catch
            {
                CommonalityEntity.WriteTextLog("日记管理 LoadData()");
            }
        }
        /// <summary>
        /// 绑定IC卡类型
        /// </summary>
        private void BindICTypeName()
        {
            try
            {
                string sql = "select * from ICCardType where ICCardType_State='启动'";
                this.comboxICCard_ICCardType_ID.DataSource = ICCardTypeDAL.GetViewICCardTypeName(sql);
                if (this.comboxICCard_ICCardType_ID.DataSource != null)
                {
                    this.comboxICCard_ICCardType_ID.DisplayMember = "ICCardType_Name";
                    this.comboxICCard_ICCardType_ID.ValueMember = "ICCardType_ID";
                    comboxICCard_ICCardType_ID.SelectedIndex = -1;
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("ICCardForm. BindICTypeName异常：");
            }
        }


        /// <summary>
        /// 绑定IC卡类型
        /// </summary>
        private void BindICType()
        {
            try
            {
                string sql = "select * from ICCardType where ICCardType_State='启动'";
                DataTable dt = LinQBaseDao.Query(sql).Tables[0];
                DataRow dr = dt.NewRow();
                dr["ICCardType_Name"] = "全部";
                dr["ICCardType_ID"] = "0";
                dt.Rows.InsertAt(dr, 0);
                this.cmbICType.DataSource = dt;
                this.cmbICType.DisplayMember = "ICCardType_Name";
                this.cmbICType.ValueMember = "ICCardType_ID";
                cmbICType.SelectedIndex = 0;
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ICCardForm. BindICType异常：");
                return;
            }
        }
        /// <summary>
        /// 绑定IC卡信息状态
        /// </summary>
        private void BindICCard()
        {
            try
            {
                this.comboxICCard_State.DataSource = DictionaryDAL.GetValueDictionary("01").Where(n => n.Dictionary_Name != "全部").ToList();

                if (this.comboxICCard_State.DataSource != null)
                {
                    this.comboxICCard_State.DisplayMember = "Dictionary_Name";
                    this.comboxICCard_State.ValueMember = "Dictionary_ID";
                    this.comboxICCard_State.SelectedValue = -1;
                }
            }
            catch
            {
                MessageBox.Show("IC卡信息“IC卡信息状态”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        /// <summary>
        /// 绑定IC卡信息状态
        /// </summary>
        private void BindSearchICCard()
        {
            try
            {
                this.comboxICCardState.DataSource = DictionaryDAL.GetValueStateDictionary("01");

                if (this.comboxICCardState.DataSource != null)
                {
                    this.comboxICCardState.DisplayMember = "Dictionary_Name";
                    this.comboxICCardState.ValueMember = "Dictionary_ID";
                    this.comboxICCardState.SelectedIndex = 3;
                }
            }
            catch
            {
                MessageBox.Show("IC卡信息“IC卡信息状态”绑定有误，请查看字典信息！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        /// <summary>
        /// “保 存” 按钮的单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            string strmessbox = "";
            try
            {
                DataTable table = LinQBaseDao.Query("select * from ICCard where ICCard_Value='" + txtICCard_Value.Text.Trim() + "'").Tables[0];
                if (table.Rows.Count > 0)
                {
                    MessageBox.Show("IC卡卡号已存在!");
                    return;
                }
                string type = cmbEType.Text;
                if (type == "次数")
                {
                    if (txtCount.Text == "")
                    {
                        MessageBox.Show("有效次数不能为空!");
                        return;
                    }
                }
                else if (type == "有效期")
                {
                    if (BeginTime.Value >= EndTime.Value)
                    {
                        MessageBox.Show("开始时间不能大于结束时间!");
                        return;
                    }
                }
                if (txtCarName.Text.Trim() != "")
                {
                    if (string.IsNullOrEmpty(MyUserCar.S_ID))
                    {
                        MessageBox.Show("请选择正确的车牌号!");
                        return;
                    }
                }
                if (txtStaName.Text.Trim() != "")
                {
                    if (string.IsNullOrEmpty(MyUserStaName.S_ID))
                    {
                        MessageBox.Show("请选择正确的驾驶员!");
                        return;
                    }
                }


                ICCard ic = new ICCard();
                if (comboxICCard_ICCardType_ID.Text.Trim() == "")
                {
                    ic.ICCard_ICCardType_ID = null;
                }
                else
                {
                    ic.ICCard_ICCardType_ID = CommonalityEntity.GetInt(comboxICCard_ICCardType_ID.SelectedValue.ToString());
                }

                ic.ICCard_Value = txtICCard_Value.Text.Trim();
                ic.ICCard_State = comboxICCard_State.Text.Trim() == "" ? "启动" : comboxICCard_State.Text;
                ic.ICCard_Remark = txtICCard_Remark.Text.Trim();
                ic.ICCard_Permissions = txtICCard_Permissions.Text.Trim();
                if (type == "次数")
                {
                    ic.ICCard_count = Convert.ToInt32(txtCount.Text);
                }
                else if (type == "有效期")
                {
                    ic.ICCard_BeginTime = BeginTime.Value;
                    ic.ICCard_EndTime = EndTime.Value;
                }
                ic.ICCard_EffectiveType = type;
                if (LinQBaseDao.InsertOne<ICCard>(new DCCarManagementDataContext(), ic))
                {
                    string ICCardid = LinQBaseDao.GetSingle("select ICCard_ID from ICCard where ICCard_Value='" + txtICCard_Value.Text.Trim() + "' ").ToString();
                    MessageBox.Show("成功添加IC卡", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CommonalityEntity.WriteLogData("新增", "新增IC卡卡号：" + txtICCard_Value.Text.Trim(), CommonalityEntity.USERNAME);//添加操作日志
                    if (txtCarName.Text.Trim() != "")
                    {
                        object icid = LinQBaseDao.GetSingle("select Car_ICCard_ID from Car where Car_Name='" + txtCarName.Text.Trim() + "'");
                        if (icid != null)
                        {
                            if (MessageBox.Show(txtCarName.Text.Trim() + "已关联IC卡，确定修改吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                LinQBaseDao.Query("update Car set Car_ICCard_ID=" + ICCardid + " where Car_Name='" + txtCarName.Text.Trim() + "'");
                                CommonalityEntity.WriteLogData("修改", "车牌号：" + txtCarName.Text.Trim() + "，配置IC卡：" + txtICCard_Value.Text.Trim(), CommonalityEntity.USERNAME);//添加操作日志
                            }
                        }
                        else
                        {
                            LinQBaseDao.Query("update Car set Car_ICCard_ID=" + ICCardid + " where Car_Name='" + txtCarName.Text.Trim() + "'");
                            CommonalityEntity.WriteLogData("修改", "车牌号：" + txtCarName.Text.Trim() + "，配置IC卡：" + txtICCard_Value.Text.Trim(), CommonalityEntity.USERNAME);//添加操作日志
                        }

                    }
                    if (txtStaName.Text.Trim() != "")
                    {
                        object icid = LinQBaseDao.GetSingle("select StaffInfo_ICCard_ID  from Car where StaffInfo_Name='" + txtStaName.Text.Trim() + "'  and StaffInfo_ID=" + MyUserStaName.S_ID);
                        if (icid != null)
                        {
                            if (MessageBox.Show(txtStaName.Text.Trim() + "已关联IC卡，确定修改吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                LinQBaseDao.Query("update StaffInfo set StaffInfo_ICCard_ID=" + ICCardid + " where StaffInfo_Name='" + txtStaName.Text.Trim() + "' and StaffInfo_ID=" + MyUserStaName.S_ID);
                                CommonalityEntity.WriteLogData("修改", "驾驶员：" + txtCarName.Text.Trim() + "，配置IC卡：" + txtICCard_Value.Text.Trim(), CommonalityEntity.USERNAME);//添加操作日志
                            }
                        }
                        else
                        {
                            LinQBaseDao.Query("update StaffInfo set StaffInfo_ICCard_ID=" + ICCardid + " where StaffInfo_Name='" + txtStaName.Text.Trim() + "' and StaffInfo_ID=" + MyUserStaName.S_ID);
                            CommonalityEntity.WriteLogData("修改", "驾驶员：" + txtCarName.Text.Trim() + "，配置IC卡：" + txtICCard_Value.Text.Trim(), CommonalityEntity.USERNAME);//添加操作日志
                        }

                    }

                }
                else
                {
                    MessageBox.Show("IC卡添加失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("IC卡信息 btnSave_Click()");
            }
            finally
            {
                LogInfoLoad("");
                Empty(); // 调用清空的方法
            }
        }

        private void chkCheckList()
        {
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
                if (this.txtICCard_Value.Text == "")
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "IC卡卡号不能为空！", txtICCard_Value, this);
                    return;
                }



                DataTable table = LinQBaseDao.Query("select * from ICCard where  ICCard_Value='" + txtICCard_Value.Text.Trim() + "' and ICCard_ID !=" + iccarid).Tables[0];
                if (table.Rows.Count > 0)
                {
                    MessageBox.Show("IC卡卡号已存在!");
                    return;
                }
                string type = cmbEType.Text;
                if (type == "次数")
                {
                    if (txtCount.Text == "")
                    {
                        MessageBox.Show("有效次数不能为空!");
                        return;
                    }
                }
                else if (type == "有效期")
                {
                    if (BeginTime.Value >= EndTime.Value)
                    {
                        MessageBox.Show("开始时间不能大于结束时间!");
                        return;
                    }
                }
                if (txtCarName.Text.Trim() != "")
                {
                    if (string.IsNullOrEmpty(MyUserCar.S_ID))
                    {
                        MessageBox.Show("请选择正确的车牌号!");
                        return;
                    }
                }
                if (txtStaName.Text.Trim() != "")
                {
                    if (string.IsNullOrEmpty(MyUserStaName.S_ID))
                    {
                        MessageBox.Show("请选择正确的驾驶员!");
                        return;
                    }
                }

                Expression<Func<ICCard, bool>> p = n => n.ICCard_ID == iccarid;
                string id = "";
                string strfront = "";
                string strContent = "";
                Action<ICCard> ap = s =>
                {
                    strfront = s.ICCard_ICCardType_ID + "," + s.ICCard_Value + "," + s.ICCard_State + "," + s.ICCard_Permissions + "," + s.ICCard_Remark;
                    s.ICCard_ICCardType_ID = int.Parse(this.comboxICCard_ICCardType_ID.SelectedValue.ToString());
                    s.ICCard_Value = this.txtICCard_Value.Text.Trim();
                    s.ICCard_State = this.comboxICCard_State.Text;
                    s.ICCard_Permissions = this.txtICCard_Permissions.Text.Trim();
                    s.ICCard_Remark = this.txtICCard_Remark.Text.Trim();
                    type = cmbEType.Text;
                    if (type == "次数")
                    {
                        s.ICCard_count = Convert.ToInt32(txtCount.Text);
                    }
                    else if (type == "有效期")
                    {
                        s.ICCard_BeginTime = BeginTime.Value;
                        s.ICCard_EndTime = EndTime.Value;
                    }
                    s.ICCard_EffectiveType = type;

                    strContent = s.ICCard_ICCardType_ID + "," + s.ICCard_Value + "," + s.ICCard_State + "," + s.ICCard_Permissions + "," + s.ICCard_Remark;
                    id = s.ICCard_ID.ToString();
                };

                if (ICCardDAL.Update(p, ap))
                {
                    MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CommonalityEntity.WriteLogData("修改", "更新编号为：" + id + "的IC卡信息；修改前：" + strfront + "；修改后：" + strContent, CommonalityEntity.USERNAME);//添加操作日志
                    if (txtCarName.Text.Trim() != "")
                    {
                        object icid = LinQBaseDao.GetSingle("select Car_ICCard_ID from Car where Car_Name !='" + txtCarName.Text.Trim() + "' and Car_ICCard_ID=" + iccarid);
                        if (icid != null)
                        {
                            if (icid.ToString() == iccarid.ToString())
                            {
                                if (MessageBox.Show("IC卡：" + txtICCard_Value.Text.Trim() + "已关联其他车牌号，确定修改吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    LinQBaseDao.Query("update Car set Car_ICCard_ID=NULL where  Car_ICCard_ID=" + iccarid);
                                    LinQBaseDao.Query("update Car set Car_ICCard_ID=" + iccarid + " where Car_Name='" + txtCarName.Text.Trim() + "'");
                                    CommonalityEntity.WriteLogData("修改", "车牌号：" + txtCarName.Text.Trim() + "，配置IC卡：" + txtICCard_Value.Text.Trim(), CommonalityEntity.USERNAME);//添加操作日志
                                }
                            }
                        }
                        else
                        {
                            LinQBaseDao.Query("update Car set Car_ICCard_ID=" + iccarid + " where Car_Name='" + txtCarName.Text.Trim() + "'");
                            CommonalityEntity.WriteLogData("修改", "车牌号：" + txtCarName.Text.Trim() + "，配置IC卡：" + txtICCard_Value.Text.Trim(), CommonalityEntity.USERNAME);//添加操作日志
                        }

                    }
                    else
                    {
                        DataTable dts = LinQBaseDao.Query("select Car_Name from Car where Car_ICCard_ID =" + iccarid).Tables[0];
                        if (dts.Rows.Count > 0)
                        {
                            LinQBaseDao.Query("update Car set Car_ICCard_ID=NULL where Car_ICCard_ID=" + iccarid);
                            CommonalityEntity.WriteLogData("修改", "车辆：" + dts.Rows[0]["Car_Name"].ToString() + " 取消IC卡配置信息", CommonalityEntity.USERNAME);//添加操作日志
                        }
                    }
                    if (txtStaName.Text.Trim() != "")
                    {
                        object icid = LinQBaseDao.GetSingle("select StaffInfo_ICCard_ID  from StaffInfo where  StaffInfo_ID !=" + MyUserStaName.S_ID + " and StaffInfo_ICCard_ID=" + iccarid);
                        if (icid != null)
                        {
                            if (icid.ToString() == iccarid.ToString())
                            {
                                if (MessageBox.Show("IC卡：" + txtICCard_Value.Text.Trim() + "已关联其他驾驶员，确定修改吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    LinQBaseDao.Query("update StaffInfo set StaffInfo_ICCard_ID=NULL where  StaffInfo_ICCard_ID=" + iccarid);
                                    LinQBaseDao.Query("update StaffInfo set StaffInfo_ICCard_ID=" + iccarid + " where  StaffInfo_ID=" + MyUserStaName.S_ID);
                                    CommonalityEntity.WriteLogData("修改", "驾驶员：" + txtCarName.Text.Trim() + "，配置IC卡：" + txtICCard_Value.Text.Trim(), CommonalityEntity.USERNAME);//添加操作日志
                                }
                            }
                        }
                        else
                        {
                            LinQBaseDao.Query("update StaffInfo set StaffInfo_ICCard_ID=" + iccarid + " where StaffInfo_ID=" + MyUserStaName.S_ID);
                            CommonalityEntity.WriteLogData("修改", "驾驶员：" + txtCarName.Text.Trim() + "，配置IC卡：" + txtICCard_Value.Text.Trim(), CommonalityEntity.USERNAME);//添加操作日志
                        }

                    }
                    else
                    {
                        DataTable dts = LinQBaseDao.Query("select StaffInfo_Name from StaffInfo where StaffInfo_ICCard_ID =" + iccarid).Tables[0];

                        if (dts.Rows.Count > 0)
                        {
                            LinQBaseDao.Query("update StaffInfo set StaffInfo_ICCard_ID=NULL where StaffInfo_ICCard_ID=" + iccarid);
                            CommonalityEntity.WriteLogData("修改", "驾驶员：" + dts.Rows[0]["StaffInfo_Name"].ToString() + " 取消IC卡配置信息", CommonalityEntity.USERNAME);//添加操作日志
                        }
                    }

                    Empty(); // 调用清空的方法
                }
                else
                {
                    MessageBox.Show("修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("IC卡信息管理 btnUpdate_Click()");
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
            tbtnDelICCard();
        }
        /// <summary>
        /// 删除选中行数据的方法 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbtnDelICCard()
        {
            try
            {
                int j = 0;
                if (this.dgvICInfo.SelectedRows.Count > 0)//选中删除
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //选中数量
                        int count = dgvICInfo.SelectedRows.Count;
                        //遍历
                        for (int i = 0; i < count; i++)
                        {
                            int iccardid = int.Parse(this.dgvICInfo.SelectedRows[i].Cells["ICCard_ID"].Value.ToString());
                            Expression<Func<ICCard, bool>> funuserinfo = n => n.ICCard_ID == iccardid;
                            string strContent = LinQBaseDao.Query("select ICCard_Value from ICCard where ICCard_ID=" + iccardid).Tables[0].Rows[0][0].ToString();
                            if (ICCardDAL.DeleteToMany(funuserinfo))
                            {
                                j++;
                                CommonalityEntity.WriteLogData("删除", "删除IC卡卡号为：" + strContent + "的信息", CommonalityEntity.USERNAME);//添加操作日志
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
                CommonalityEntity.WriteTextLog("IC卡信息管理 tbtnDelICCard() 异常！+");
            }
            finally
            {
                CommonalityEntity.strCardNo = "";
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
            isbool = false;
            this.comboxICCard_ICCardType_ID.SelectedIndex = 0;
            this.txtICCard_Value.Text = "";
            this.comboxICCard_State.Text = "启动";
            this.txtICCard_Permissions.Text = "";
            this.txtICCard_Remark.Text = "";
            this.txtStaName.Text = "";
            this.txtCarName.Text = "";
            list.Clear();
            MyUserStaName.S_ID = "";
            MyUserStaName.S_Name = "";
            MyUserStaName.S_Indes = "";
            MyUserCar.S_ID = "";
            MyUserCar.S_Name = "";
            CommonalityEntity.strCardNo = "";
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
                Where();
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ICCardForm.btnSearch_Click异常:");
            }
            finally
            {
                CommonalityEntity.strCardNo = "";
                LogInfoLoad("");
            }
        }

        private void Where()
        {
            sqlwhere = "  1=1";
            string value = this.txtICCardValue.Text.Trim();
            string state = this.comboxICCardState.Text.Trim();
            string ictype = this.cmbICType.Text.Trim();
            if (ictype != "全部")
            {
                sqlwhere += String.Format(" and ICCardType_Name =  '{0}'", ictype);
            }
            if (state != "全部")
            {
                sqlwhere += String.Format(" and ICCard_State =  '{0}'", state);
            }
            if (!string.IsNullOrEmpty(value))//IC卡卡号
            {
                sqlwhere += String.Format(" and ICCard_Value like  '%{0}%'", value);
            }
            if (!string.IsNullOrEmpty(textBox1.Text.Trim()))//IC卡卡号
            {
                sqlwhere += String.Format(" and Car_Name like  '%{0}%'", textBox1.Text.Trim());
            }
            if (!string.IsNullOrEmpty(textBox2.Text.Trim()))//IC卡卡号
            {
                sqlwhere += String.Format(" and StaffInfo_Name like  '%{0}%'", textBox2.Text.Trim());
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
            for (int i = 0; i < this.dgvICInfo.Rows.Count; i++)
            {
                dgvICInfo.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslAllChecked()
        {
            for (int i = 0; i < dgvICInfo.Rows.Count; i++)
            {
                this.dgvICInfo.Rows[i].Selected = true;
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
            if (e.ClickedItem.Name == "tslbExecl")//导出Execl
            {
                tsbExecl_Click();
                return;
            }
            if (e.ClickedItem.Name == "tslbInExecl")//导入Execl
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
            page.BindBoundControl(dgvICInfo, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_ICCard_ICCardType", "*", "ICCard_ID", "ICCard_ID", 0, sqlwhere, true);
        }
        #endregion

        /// <summary>
        /// 双击组件时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvICInfo_DoubleClick(object sender, EventArgs e)
        {
            isbool = false;
            this.btnUpdate.Enabled = true;
            this.btnSave.Enabled = false;
            if (this.dgvICInfo.SelectedRows.Count > 0)//选中行
            {
                if (dgvICInfo.SelectedRows.Count > 1)
                {
                    MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //修改的值
                    int ID = int.Parse(this.dgvICInfo.SelectedRows[0].Cells["ICCard_ID"].Value.ToString());
                    iccarid = ID;
                    Expression<Func<ICCard, bool>> funviewinto = n => n.ICCard_ID == ID;
                    foreach (var n in ICCardDAL.Query(funviewinto))
                    {
                        if (n.ICCard_ICCardType_ID != null)
                        {
                            string strtype = LinQBaseDao.GetSingle("select ICCardType_Name from ICCardType where ICCardType_ID =" + n.ICCard_ICCardType_ID).ToString();
                            //IC卡类型
                            this.comboxICCard_ICCardType_ID.Text = strtype;
                        }
                        if (n.ICCard_Value != null)
                        {
                            // IC卡卡号
                            this.txtICCard_Value.Text = n.ICCard_Value;
                        }
                        if (n.ICCard_State != null)
                        {
                            // IC卡状态
                            this.comboxICCard_State.Text = n.ICCard_State;
                        }
                        if (n.ICCard_Permissions != null)
                        {
                            // IC卡权限
                            this.txtICCard_Permissions.Text = n.ICCard_Permissions;
                        }
                        if (n.ICCard_Remark != null)
                        {
                            // IC卡备注
                            this.txtICCard_Remark.Text = n.ICCard_Remark;
                        }
                        string type = n.ICCard_EffectiveType;
                        cmbEType.Text = type;
                        if (type == "永久")
                        {
                            txtCount.Enabled = false;
                            BeginTime.Enabled = false;
                            EndTime.Enabled = false;
                        }
                        else if (type == "次数")
                        {
                            txtCount.Enabled = true;
                            BeginTime.Enabled = false;
                            EndTime.Enabled = false;
                        }
                        else if (type == "有效期")
                        {
                            txtCount.Enabled = false;
                            BeginTime.Enabled = true;
                            EndTime.Enabled = true;
                        }
                        DataTable ct = LinQBaseDao.Query("  select * from dbo.View_ICCard_ICCardType  where  ICCard_ID=" + iccarid).Tables[0];
                        string carname = ct.Rows[0]["Car_Name"].ToString();
                        string staname = ct.Rows[0]["StaffInfo_Name"].ToString();
                        if (!string.IsNullOrEmpty(carname))
                        {
                            MyUserCar.S_ID = ct.Rows[0]["Car_ID"].ToString();
                            MyUserCar.S_Name = carname;
                            MyUserCar.Visible = false;
                        }
                        else
                        {
                            MyUserCar.S_ID = "";
                            MyUserCar.S_Name = "";
                            MyUserCar.Visible = false;
                        }
                        if (!string.IsNullOrEmpty(staname))
                        {
                            MyUserStaName.S_ID = ct.Rows[0]["StaffInfo_ID"].ToString();
                            MyUserStaName.S_Name = staname;
                            MyUserStaName.S_Indes = ct.Rows[0]["StaffInfo_Identity"].ToString();
                            MyUserStaName.Visible = false;
                        }
                        else
                        {
                            MyUserStaName.S_ID = "";
                            MyUserStaName.S_Name = "";
                            MyUserStaName.S_Indes = "";
                            MyUserStaName.Visible = false;
                        }
                        txtCarName.Text = carname;
                        txtStaName.Text = staname;
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
        /// “IC卡类型”的链接事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnICTypeForm_Click(object sender, EventArgs e)
        {
            ICCardTypeForm itf = new ICCardTypeForm();
            PublicClass.ShowChildForm(itf);
            //mf = new MainForm();
            //mf.ShowChildForm(itf, this);
        }

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
            //this.DisplayText.Text = checkedText;
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
            //this.DisplayText.Text = checkedText;
            return checkedText;
        }

        /// <summary>
        /// 双击文本框"IC卡权限"的配置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtICCard_Permissions_DoubleClick(object sender, EventArgs e)
        {
            treeViewIcP.Nodes.Clear();
            try
            {
                if (panel1.Visible)
                {
                    panel1.Visible = false;
                }
                else
                {
                    panel1.Visible = true;
                    DataTable table1 = LinQBaseDao.Query("select Position_Name,Position_Value,Position_ID from Position where  Position_State='启动' order by Position_Name").Tables[0];
                    TreeNode tr1, tr2;
                    for (int i = 0; i < table1.Rows.Count; i++)
                    {
                        tr1 = new TreeNode();
                        tr1.Tag = table1.Rows[i]["Position_Value"];
                        tr1.Text = table1.Rows[i]["Position_Name"].ToString();
                        treeViewIcP.Nodes.Add(tr1);
                        DataTable table2 = LinQBaseDao.Query("select Driveway_Name,Driveway_Value from Driveway where Driveway_State='启动' and Driveway_Position_ID='" + table1.Rows[i]["Position_ID"] + "' order by Driveway_Name").Tables[0];
                        for (int n = 0; n < table2.Rows.Count; n++)
                        {
                            tr2 = new TreeNode();
                            tr2.Tag = table2.Rows[n]["Driveway_Value"];
                            tr2.Text = table2.Rows[n]["Driveway_Name"].ToString();
                            tr1.Nodes.Add(tr2);
                        }
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ICCardForm txtICCard_Permissions_DoubleClick()");
            }
        }

        /// <summary>
        /// 读卡信息是否正在使用。
        /// </summary>
        public bool ISCardTime = false;
        /// <summary>
        /// 定时绑定IC卡信息  IC卡卡号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ICNumbertimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!ISCardTime)
                {
                    ISCardTime = true;
                    var plistdc = MainForm.listDC.Where(n => n.DeviceControl_PositionValue == SystemClass.PosistionValue && n.DeviceControl_ReadValue == SystemClass.ReadValue && n.DeviceControl_CardNo.ToString().Trim() != "").Select(n => n).ToList();
                    if (plistdc.Count > 0)
                    {
                        foreach (var item in plistdc)
                        {
                            txtICCard_Value.Text = item.DeviceControl_CardNo;
                            break;
                        }
                    }
                    //删除DeviceControl 中的IC卡
                    DCCarManagementDataContext db = new DCCarManagementDataContext();
                    for (int i = 0; i < CommonalityEntity.listCarIC.Count(); i++)
                    //for (int i = 1; i < plistdc.Count(); i++)
                    {
                        string str = CommonalityEntity.listCarIC[i].DeviceControl_CardNo;
                        Expression<Func<DeviceControl, bool>> fun = n => n.DeviceControl_ID == CommonalityEntity.listCarIC[i].DeviceControl_ID;
                        Action<DeviceControl> acton = m =>
                        {
                            m.DeviceControl_CardNo = null;
                        };
                        if (i < CommonalityEntity.listCarIC.Count() - 1)
                        {
                            LinQBaseDao.ADD_Delete_UpdateMethod(db, 2, null, fun, acton, false, null);
                        }
                        else
                        {
                            LinQBaseDao.ADD_Delete_UpdateMethod(db, 2, null, fun, acton, true, null);
                        }
                    }
                    ICNumbertimer.Enabled = false;
                }
            }
            catch
            {
                MessageBox.Show("IC卡卡号读取失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 全选及取消全选
        /// </summary>
        /// <param name="rbool">>true:全选 false:取消全选</param>
        /// <param name="dgv">显示列表控件</param>
        /// <param name="checkboxbool">true:有复选框 false:无复选框</param>
        private void SelectAllMethod(bool rbool, DataGridView dgv, bool checkboxbool)
        {
            try
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if (checkboxbool)
                    {
                        ((DataGridViewCheckBoxCell)dgv.Rows[i].Cells[0]).Value = rbool;
                    }
                    dgv.Rows[i].Selected = rbool;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyForm.SelectAllMethod()");
            }
        }

        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_SelectAll_Click(object sender, EventArgs e)
        {
            SelectAll(true);
        }

        /// <summary>
        /// 全选
        /// </summary>
        private void SelectAll(bool chrbool)
        {
            foreach (TreeNode tnTemp in treeViewIcP.Nodes)
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

        #region 选中父级菜单该菜单下的所有子级菜单自动选中
        private void treeViewIcP_AfterCheck(object sender, TreeViewEventArgs e)
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
        string str = null;
        string strPermissions = null;
        private void buttonVisible_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
        }

        ArrayList arraylist = new ArrayList();
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
                        strPermissions += "";
                    }
                    addDiGui(tnTemp);
                    if (tnTemp.Checked == true)
                    {
                        str = str.TrimEnd(',') + ".";
                    }
                }
                str = str.TrimEnd('.');
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                arraylist.Clear();//清空动态数组内的成员
                str = null;
                add();
                txtICCard_Permissions.Text = str;
                panel1.Visible = false;
            }
            catch
            {

            }
        }
        private void btn_NotSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                SelectAll(false);
                treeViewIcP.Nodes.Clear();
                //tnTemp.ExpandAll();//展开所有子节点
            }
            catch
            {

            }
        }

        private void dgvICInfo_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        private void txtICCard_Value_KeyUp(object sender, KeyEventArgs e)
        {
            string str = txtICCard_Value.Text.Trim();
            if (str.Length == 10)
            {
                try
                {
                    txtICCard_Value.Text = "0" + Convert.ToInt64(str).ToString("X");
                }
                catch
                {
                    txtICCard_Value.Text = "";
                }
            }
            if (str.Length > 10)
            {
                txtICCard_Value.Text = "";
            }
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CommonalityEntity.strCardNo))
            {
                txtICCard_Value.Text = CommonalityEntity.strCardNo;
            }
            if (this.ActiveControl.Name != "txtCarName" || this.ActiveControl.Name != "MyUserCar")
            {
                MyUserCar.Visible = false;
            }
            if (this.ActiveControl.Name != "txtStaName" || this.ActiveControl.Name != "MyUserStaName")
            {
                MyUserStaName.Visible = false;
            }
            txtCarName.Text = MyUserCar.S_Name;
            txtStaName.Text = MyUserStaName.S_Name;
        }

        private void ICCardForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //清空IC卡卡号记录
            CommonalityEntity.strCardNo = "";
        }

        private void txtICCard_Value_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CommonalityEntity.strCardNo))
            {
                txtICCard_Value.Text = CommonalityEntity.strCardNo;
            }
        }

        private void cmbEType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string type = cmbEType.Text;
            if (type == "永久")
            {
                txtCount.Enabled = false;
                BeginTime.Enabled = false;
                EndTime.Enabled = false;
            }
            else if (type == "次数")
            {
                txtCount.Enabled = true;
                BeginTime.Enabled = false;
                EndTime.Enabled = false;
            }
            else if (type == "有效期")
            {
                txtCount.Enabled = false;
                BeginTime.Enabled = true;
                EndTime.Enabled = true;
            }
        }

        private void txtCarName_TextChanged(object sender, EventArgs e)
        {
            if (isbool)
            {
                MyUserCar.Visible = true;
                CommonalityEntity.tablename = "Car";
                CommonalityEntity.tabcom1 = "Car_Name";
                CommonalityEntity.tabcom2 = "";
                CommonalityEntity.tabcom3 = "";
                CommonalityEntity.tabid = "Car_ID";
                CommonalityEntity.strlike = txtCarName.Text.Trim();
                MyUserCar.StaffInfo_Select();
            }
        }

        private void txtStaName_TextChanged(object sender, EventArgs e)
        {
            if (isbool)
            {
                MyUserStaName.Visible = true;
                CommonalityEntity.tablename = "StaffInfo";
                CommonalityEntity.tabcom1 = "StaffInfo_Name";
                CommonalityEntity.tabcom2 = "StaffInfo_Identity";
                CommonalityEntity.tabcom3 = "";
                CommonalityEntity.tabid = "StaffInfo_ID";
                CommonalityEntity.strlike = txtStaName.Text.Trim();
                MyUserStaName.StaffInfo_Select();
            }
        }

        private void txtCarName_Click(object sender, EventArgs e)
        {
            MyUserCar.Visible = true;
            CommonalityEntity.tablename = "Car";
            CommonalityEntity.tabcom1 = "Car_Name";
            CommonalityEntity.tabcom2 = "";
            CommonalityEntity.tabcom3 = "";
            CommonalityEntity.tabid = "Car_ID";
            CommonalityEntity.strlike = txtCarName.Text.Trim();
            MyUserCar.StaffInfo_Select();
            isbool = true;
        }

        private void txtStaName_Click(object sender, EventArgs e)
        {
            MyUserStaName.Visible = true;
            CommonalityEntity.tablename = "StaffInfo";
            CommonalityEntity.tabcom1 = "StaffInfo_Name";
            CommonalityEntity.tabcom2 = "StaffInfo_Identity";
            CommonalityEntity.tabcom3 = "";
            CommonalityEntity.tabid = "StaffInfo_ID";
            CommonalityEntity.strlike = txtStaName.Text.Trim();
            MyUserStaName.StaffInfo_Select();
            isbool = true;
        }

        private void ICCardForm_Enter(object sender, EventArgs e)
        {
            if (this.ActiveControl.Name != "txtCarName" || this.ActiveControl.Name != "MyUserCar")
            {
                MyUserCar.Visible = false;
            }
            if (this.ActiveControl.Name != "txtStaName" || this.ActiveControl.Name != "MyUserStaName")
            {
                MyUserStaName.Visible = false;
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
            groupBox2.Visible = false;
        }

        private void tsbExecl_Click()
        {
            string fileName = "IC卡信息Excel报表-" + CommonalityEntity.GetServersTime().ToShortDateString();

            if (!ISExecl)
            {
                tslExport_Excel(fileName, dgvICInfo);
            }
            else
            {
                Where();
                string strsql = "select ICCardType_Name as IC卡类型名称,ICCard_Value as IC卡卡号,ICCard_State as IC卡状态 ,ICCard_Permissions as IC卡的权限,Car_Name as 关联车牌号,StaffInfo_Name as 关联人姓名,StaffInfo_Identity as 关联人身份证,ICCard_EffectiveType as IC卡有效类型,ICCard_count as 允许进厂次数,ICCard_HasCount as 已进厂次数,ICCard_BeginTime as 开始时间,ICCard_EndTime as 结束时间,ICCard_Remark as IC卡备注 from  View_ICCard_ICCardType where " + sqlwhere + " order by ICCard_ID";
                daochu(fileName, strsql);
            }

        }
        /// <summary>
        /// 导出Excel 的方法
        /// </summary>
        private void tslExport_Excel(string fileName, DataGridView myDGV)
        {
            try
            {

                string saveFileName = "";
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.DefaultExt = "xls";
                saveDialog.Filter = "Excel文件|*.xls";
                saveDialog.FileName = fileName;
                saveDialog.ShowDialog();
                saveFileName = saveDialog.FileName;
                if (saveFileName.IndexOf(":") < 0) return; //被点了取消 

                ISdao = true;
                groupBox2.Visible = true;
                btnSet.Text = "取消导出";
                progressBar1.Maximum = myDGV.SelectedRows.Count;
                progressBar1.Value = 0;
                label18.Text = "正在导出：" + fileName;
                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                if (xlApp == null)
                {
                    MessageBox.Show("无法创建Excel对象，可能您的机子未安装Excel");
                    return;
                }

                Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1 

                Microsoft.Office.Interop.Excel.Range range = worksheet.get_Range("A1", "Z" + (myDGV.SelectedRows.Count + 10));//把Execl设置问文本格式
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
                            worksheet.Cells[s + 2, i] = myDGV.SelectedRows[r].Cells[i].Value;
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
                Microsoft.Office.Interop.Excel.Range rang = worksheet.get_Range(worksheet.Cells[2, 1], worksheet.Cells[myDGV.SelectedRows.Count + 2, 2]);
                rang.NumberFormat = "000000000000";

                if (saveFileName != "")
                {
                    try
                    {
                        workbook.Saved = true;
                        workbook.SaveCopyAs(saveFileName);
                    }
                    catch
                    {

                    }

                }
                xlApp.Quit();
                GC.Collect();//强行销毁 
                if (progressBar1.Value >= progressBar1.Maximum)
                {
                    label18.Text = fileName;
                    btnSet.Text = "导出完成";
                }
            }
            catch (System.Exception ex)
            {

            }
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
                groupBox2.Visible = true;
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
                CommonalityEntity.WriteTextLog("ICCardForm.InExecl异常:");
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
                string strICCardType_Name = "";//IC卡类型
                string strICCard_Value = "";//IC卡号
                string strICCard_State = "";//IC卡状态
                string strICCard_Permissions = "";//IC卡权限
                string strCarName = "";//车牌号
                string strStaffInfo_Name = "";//关联人姓名
                string strStaffInfo_Identity = "";//身份证号
                string strICCard_EffectiveType = "";//IC卡有效类型
                string strICCard_count = "";//允许进厂次数
                string strICCard_HasCount = "";//已进厂次数
                string strICCard_BeginTime = "";//开始时间
                string strICCard_EndTime = "";//结束时间
                string strICCard_Remark = "";//IC卡备注

                string ictypeid = "";//卡类型ID
                string carid = "";//车辆ID
                string staffinfoid = "";//人员ID
                string fileName = System.IO.Path.GetFileName(path);// 
                ISdao = true;
                groupBox2.Visible = true;
                btnSet.Text = "取消导入";
                progressBar1.Maximum = table.Rows.Count;
                progressBar1.Value = 0;
                label18.Text = "正在导入：" + fileName;

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (ISdao)
                    {
                        strICCardType_Name = table.Rows[i][0].ToString().Trim();
                        strICCard_Value = table.Rows[i][1].ToString().Trim();
                        strICCard_State = table.Rows[i][2].ToString().Trim();
                        strICCard_Permissions = table.Rows[i][3].ToString().Trim();
                        strCarName = table.Rows[i][4].ToString().Trim();
                        strStaffInfo_Name = table.Rows[i][5].ToString().Trim();
                        strStaffInfo_Identity = table.Rows[i][6].ToString().Trim();
                        strICCard_EffectiveType = table.Rows[i][7].ToString().Trim();
                        strICCard_count = table.Rows[i][8].ToString().Trim();
                        strICCard_HasCount = table.Rows[i][9].ToString().Trim();
                        strICCard_BeginTime = table.Rows[i][10].ToString().Trim();
                        strICCard_EndTime = table.Rows[i][11].ToString().Trim();
                        strICCard_Remark = table.Rows[i][12].ToString().Trim();

                        DataTable dt = LinQBaseDao.Query("select ICCardType_ID from ICCardType where ICCardType_Name='" + strICCardType_Name + "'").Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            ictypeid = dt.Rows[0][0].ToString();
                        }

                        DataTable dts = LinQBaseDao.Query("select ICCard_ID from ICCard  where  ICCard_Value='" + strICCard_Value + "'").Tables[0];
                        if (string.IsNullOrEmpty(strICCard_count))
                        {
                            strICCard_count = "0";
                        }
                        if (string.IsNullOrEmpty(strICCard_HasCount))
                        {
                            strICCard_HasCount = "0";
                        }
                        string Car_ICCard_ID = "";
                        if (dts.Rows.Count > 0)
                        {
                            Car_ICCard_ID = dts.Rows[0][0].ToString();
                            if (string.IsNullOrEmpty(strICCard_EndTime))
                            {
                                LinQBaseDao.Query("update ICCard set ICCard_ICCardType_ID=" + ictypeid + " ,ICCard_Value='" + strICCard_Value + "',ICCard_State='" + strICCard_State + "',ICCard_Permissions='" + strICCard_Permissions + "',ICCard_EffectiveType='" + strICCard_EffectiveType + "',ICCard_count=" + strICCard_count + ",ICCard_HasCount=" + strICCard_HasCount + ",ICCard_Remark='" + strICCard_Remark + "' where ICCard_ID=" + Car_ICCard_ID);
                            }
                            else
                            {
                                LinQBaseDao.Query("update ICCard set ICCard_ICCardType_ID=" + ictypeid + " ,ICCard_Value='" + strICCard_Value + "',ICCard_State='" + strICCard_State + "',ICCard_Permissions='" + strICCard_Permissions + "',ICCard_EffectiveType='" + strICCard_EffectiveType + "',ICCard_count=" + strICCard_count + ",ICCard_HasCount=" + strICCard_HasCount + ",ICCard_BeginTime='" + strICCard_BeginTime + "',ICCard_EndTime='" + strICCard_EndTime + "' ,ICCard_Remark='" + strICCard_Remark + "' where ICCard_ID=" + Car_ICCard_ID);
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(strICCard_EndTime))
                            {
                                Car_ICCard_ID = LinQBaseDao.GetSingle("insert into ICCard(ICCard_ICCardType_ID,ICCard_Value,ICCard_State,ICCard_Permissions,ICCard_EffectiveType,ICCard_count,ICCard_HasCount,ICCard_Remark)  values (" + ictypeid + ",'" + strICCard_Value + "','" + strICCard_State + "','" + strICCard_Permissions + "','" + strICCard_EffectiveType + "'," + strICCard_count + "," + strICCard_HasCount + ",'" + strICCard_Remark + "')      select @@identity").ToString();
                            }
                            else
                            {
                                Car_ICCard_ID = LinQBaseDao.GetSingle("insert into ICCard(ICCard_ICCardType_ID,ICCard_Value,ICCard_State,ICCard_Permissions,ICCard_EffectiveType,ICCard_count,ICCard_HasCount,ICCard_BeginTime,ICCard_EndTime,ICCard_Remark)  values (" + ictypeid + ",'" + strICCard_Value + "','" + strICCard_State + "','" + strICCard_Permissions + "','" + strICCard_EffectiveType + "'," + strICCard_count + "," + strICCard_HasCount + ",'" + strICCard_BeginTime + "','" + strICCard_EndTime + "','" + strICCard_Remark + "')      select @@identity").ToString();
                            }
                        }

                        if (!string.IsNullOrEmpty(strCarName))
                        {
                            dt = LinQBaseDao.Query("select Car_ID from Car where Car_Name='" + strCarName + "'").Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                carid = dt.Rows[0][0].ToString();
                                if (dt.Rows.Count == 1)
                                {
                                    LinQBaseDao.Query("update Car set Car_ICCard_ID=" + Car_ICCard_ID + " where Car_ID=" + carid);
                                }
                            }
                            else
                            {
                                LinQBaseDao.Query("insert Car(Car_Name,Car_ICCard_ID,Car_State,Car_CreateTime,Car_ISRegister,Car_ISStaffInfo) values('" + strCarName + "'" + Car_ICCard_ID + ",,'启动',GETDATE(),0,0)");
                            }
                        }
                        if (!string.IsNullOrEmpty(strStaffInfo_Name))
                        {
                            dt = LinQBaseDao.Query("select StaffInfo_ID from StaffInfo where StaffInfo_Name='" + strStaffInfo_Name + "' and StaffInfo_Identity='" + strStaffInfo_Identity + "'").Tables[0];
                            if (dt.Rows.Count > 0)
                            {
                                staffinfoid = dt.Rows[0][0].ToString();
                                LinQBaseDao.Query("update StaffInfo set StaffInfo_ICCard_ID=" + Car_ICCard_ID + " where StaffInfo_ID=" + staffinfoid);
                            }
                            else
                            {
                                LinQBaseDao.Query("Insert into StaffInfo(StaffInfo_Type,StaffInfo_Name,StaffInfo_ICCard_ID,StaffInfo_Time,StaffInfo_State,StaffInfo_Identity) values('驾驶员','" + strStaffInfo_Name + "'," + Car_ICCard_ID + ",GETDATE(),'启动','" + strStaffInfo_Identity + "')");
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
                if (progressBar1.Value == table.Rows.Count)
                {
                    label18.Text = fileName;
                    btnSet.Text = "导入完成";
                }
                CommonalityEntity.WriteLogData("修改", "IC卡信息导入Execl信息", CommonalityEntity.USERNAME);

            }
            catch (Exception)
            {

            }
        }

        //遍历str在数组中是否存在
        private bool StrInArray(string str, string[] strarry)
        {
            if (str == null)
                return false;
            if (strarry == null || strarry.Length == 0)
                return false;
            for (int i = 0; i < strarry.Length; i++)
            {
                if (strarry[i] == null)
                    continue;
                if (str == strarry[i])
                    return true;
            }
            return false;
        }
        private void dgvICInfo_Click(object sender, EventArgs e)
        {
            ISExecl = false;
        }


    }
}
