using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Commons;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.Commons.CommonClass;
using System.Xml;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class SMSSetForm : Form
    {
        public bool isUpdate = false;
        public SMSSetForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //chkList.CheckOnClick = true;
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
                btnDelete.Enabled = true;
                btnDelete.Visible = true;
                btnUpdate.Enabled = true;
                btnUpdate.Visible = true;
                btnSMSApplication.Enabled = true;
                btnSMSApplication.Visible = true;
            }
            else
            {
                btnAdd.Visible = ControlAttributes.BoolControl("btnAdd", "SMSSetForm", "Visible");
                btnAdd.Enabled = ControlAttributes.BoolControl("btnAdd", "SMSSetForm", "Enabled");

                btnDelete.Visible = ControlAttributes.BoolControl("btnDelete", "SMSSetForm", "Visible");
                btnDelete.Enabled = ControlAttributes.BoolControl("btnDelete", "SMSSetForm", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "SMSSetForm", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "SMSSetForm", "Enabled");

                btnSMSApplication.Visible = ControlAttributes.BoolControl("btnSMSApplication", "SMSSetForm", "Visible");
                btnSMSApplication.Enabled = ControlAttributes.BoolControl("btnSMSApplication", "SMSSetForm", "Enabled");
            }
        }
        /// <summary>
        /// 绑定门岗
        /// </summary>
        public void PositionLoad()
        {
            string sql = "Select * from Position where Position_State='启动'";
            chkPositionSMS_Position_Id.DataSource = PositionDAL.GetPositionID(sql);
            chkPositionSMS_Position_Id.ValueMember = "Position_Id";
            chkPositionSMS_Position_Id.DisplayMember = "Position_Name";

            chkSPositionLED_Position_Id.DataSource = PositionDAL.GetPositionID(sql);
            chkSPositionLED_Position_Id.ValueMember = "Position_Id";
            chkSPositionLED_Position_Id.DisplayMember = "Position_Name";
        }

        private void SMSSetForm_Load(object sender, EventArgs e)
        {
            userContext();
            PositionLoad();//绑定门岗
            GetGriddataviewLoad("");//绑定列表显示数据
            SelectPositionSMS_StateLoad();//绑定状态
            //txtjiange_setLoad();
            btnUpdate.Enabled = false;
            //txtjiange_Laod();

        }

        private void btnSMSApplication_Click(object sender, EventArgs e)
        {
            try
            {
                isUpdate = true;
                if (ChkPositionSMSState())
                {
                    DialogResult dlgResult = MessageBox.Show("已经存在启动状态的设置，是否替换?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                    if (dlgResult == DialogResult.OK)
                    {
                        //修改条件
                        Expression<Func<PositionSMS, bool>> funs = n => n.PositionSMS_State == "启动" && n.PositionSMS_Position_ID == int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionSMS_Position_ID"].Value.ToString());
                        //需要修改的内容
                        Action<PositionSMS> actions = p =>
                        {
                            p.PositionSMS_State = "暂停";
                        };
                        //执行更新
                        PositionSMSDAL.UpdatePositionSMS(funs, actions);

                        //应用当前选中的设置
                        //条件
                        int positionsms_id = int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionSMS_ID"].Value.ToString());
                        Expression<Func<PositionSMS, bool>> fun = n => n.PositionSMS_ID == positionsms_id;
                        //需要的内容
                        Action<PositionSMS> action = p =>
                        {
                            p.PositionSMS_State = "启动";
                        };
                        //执行更新
                        PositionSMSDAL.UpdatePositionSMS(fun, action);
                        CommonalityEntity.WriteLogData("启动", "启动编号为： " + positionsms_id + "的信息", CommonalityEntity.USERNAME);//添加操作日志
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    //应用当前选中的设置
                    //条件
                    int positionsms_id = int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionSMS_ID"].Value.ToString());
                    Expression<Func<PositionSMS, bool>> fun = n => n.PositionSMS_ID == positionsms_id;
                    //需要的内容
                    Action<PositionSMS> action = p =>
                    {
                        p.PositionSMS_State = "启动";
                    };
                    //执行更新
                    PositionSMSDAL.UpdatePositionSMS(fun, action);
                    CommonalityEntity.WriteLogData("启动", "启动编号为： " + positionsms_id + "短信提示信息", CommonalityEntity.USERNAME);//添加操作日志
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("SMSSetForm btnSMSApplication_Click()" + "");
            }
            finally
            {
                GetGriddataviewLoad("");//加载
            }
        }
        /// <summary>
        /// 保存当前设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtPositionSMS_Count.Text.Trim()))
                {
                    MessageBox.Show("发送间隔不能为空！");
                    return;
                }
                int scount = Convert.ToInt32(txtPositionSMS_Count.Text.Trim());
                //得到输入的数据
                PositionSMS ps = new PositionSMS();
                ps.PositionSMS_Content = txtPositionSMS_Content.Text.Trim();
                ps.PositionSMS_Count = scount;
                ps.PositionSMS_Operate = CommonalityEntity.USERNAME;
                ps.PositionSMS_Position_ID = int.Parse(chkPositionSMS_Position_Id.SelectedValue.ToString());
                //ps.PositionSMS_Type = txtPositionSMS_Type.Text.Trim();
                ps.PositionSMS_Time = CommonalityEntity.GetServersTime();
                ps.PositionSMS_State = combokPositionSMS_State.Text.ToString();
                if (combokPositionSMS_State.Text.Trim() == "启动")
                {
                    if (ChkPositionSMSState())
                    {
                        DialogResult dlgResult = MessageBox.Show("已经存在启动状态的设置，是否替换?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dlgResult == DialogResult.OK)
                        {
                            //修改条件
                            Expression<Func<PositionSMS, bool>> fun = n => n.PositionSMS_State == "启动" && n.PositionSMS_Position_ID == int.Parse(chkPositionSMS_Position_Id.SelectedValue.ToString());
                            //需要修改的内容
                            Action<PositionSMS> action = p =>
                            {
                                p.PositionSMS_State = "暂停";
                            };
                            //执行更新
                            PositionSMSDAL.UpdatePositionSMS(fun, action);
                            PositionSMSDAL.InsertPositionSMS(ps);
                        }
                        else
                        {
                            ps.PositionSMS_State = "暂停";
                            PositionSMSDAL.InsertPositionSMS(ps);
                        }
                    }
                    else
                    {
                        PositionSMSDAL.InsertPositionSMS(ps);
                    }
                }
                else
                {
                    PositionSMSDAL.InsertPositionSMS(ps);
                }
                DataTable dt = LinQBaseDao.Query("select PositionSMS_ID,PositionSMS_State from PositionSMS order by PositionSMS_ID desc ").Tables[0];
                string positionsms_id = dt.Rows[0][0].ToString();
                string positionsms_state = dt.Rows[0][1].ToString();
                if (positionsms_state == "启动")
                    CommonalityEntity.WriteLogData("新增", "新增并启动编号为： " + positionsms_id + "的短信提示信息", CommonalityEntity.USERNAME);//添加操作日志
                else
                    CommonalityEntity.WriteLogData("新增", "新增编号为： " + positionsms_id + "的短信提示信息", CommonalityEntity.USERNAME);//添加操作日志
            }
            catch
            {
                CommonalityEntity.WriteTextLog("SMSSetForm btnAdd_Click()" + "");
            }
            finally
            {
                GetGriddataviewLoad("");
                Cler();
            }
        }
        /// <summary>
        /// 验证设置状态是否重复
        /// </summary>
        /// <returns></returns>
        public bool ChkPositionSMSState()
        {
            bool chkState = false;
            try
            {
                string sql = "";
                if (isUpdate)
                {
                    sql = "Select * from PositionSMS where PositionSMS_State='启动' and PositionSMS_id!=" + int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionSMS_ID"].Value.ToString()) + "  and PositionSMS_Position_ID = " + int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionSMS_Position_ID"].Value.ToString()) + "";
                }
                else
                {
                    sql = "Select * from PositionSMS where PositionSMS_State='启动'  and PositionSMS_Position_ID = " + int.Parse(chkPositionSMS_Position_Id.SelectedValue.ToString()) + "";
                }
                chkState = PositionSMSDAL.ChkPositionSMSState(sql);
            }
            catch
            {
                CommonalityEntity.WriteTextLog("SMSSetForm ChkPositionSMSState()" + "");
            }
            return chkState;
        }
        /// <summary>
        /// 修改设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.lvwUserList.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("请选择要修改的项！");
                    return;
                }
                if (string.IsNullOrEmpty(txtPositionSMS_Count.Text.Trim()))
                {
                    MessageBox.Show("发送间隔不能为空！");
                    return;
                }
                int scount = Convert.ToInt32(txtPositionSMS_Count.Text.Trim());
                //修改条件
                Expression<Func<PositionSMS, bool>> fun = n => n.PositionSMS_ID == int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionSMS_ID"].Value.ToString());
                if (combokPositionSMS_State.Text.Trim() == "启动")
                {
                    if (ChkPositionSMSState())
                    {
                        DialogResult dlgResult = MessageBox.Show("已经存在启动状态的设置，是否替换?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dlgResult == DialogResult.OK)
                        {
                            //修改条件
                            Expression<Func<PositionSMS, bool>> funs = n => n.PositionSMS_State == "启动" && n.PositionSMS_Position_ID == int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionSMS_ID"].Value.ToString());
                            //需要修改的内容
                            Action<PositionSMS> actions = p =>
                            {
                                p.PositionSMS_State = "暂停";
                            };
                            //执行更新
                            if (PositionSMSDAL.UpdatePositionSMS(funs, actions))
                            {
                                MessageBox.Show("修改成功");
                            }
                            else
                            {
                                MessageBox.Show("修改失败！");
                            }

                        }
                        else
                        {
                            return;
                        }
                    }
                }
                string id = "";
                string strfront = "";
                string strcontent = "";
                //需要修改的内容
                Action<PositionSMS> action = ps =>
                {
                    strfront = ps.PositionSMS_Content + "," + ps.PositionSMS_Count + "," + ps.PositionSMS_Position_ID + "," + ps.PositionSMS_State;
                    ps.PositionSMS_Content = txtPositionSMS_Content.Text.Trim();
                    ps.PositionSMS_Position_ID = int.Parse(chkPositionSMS_Position_Id.SelectedValue.ToString());
                    ps.PositionSMS_Count = scount;
                    //ps.PositionSMS_Type = txtPositionSMS_Type.Text.Trim();
                    ps.PositionSMS_State = combokPositionSMS_State.Text.ToString();
                    strcontent = ps.PositionSMS_Content + "," + ps.PositionSMS_Count + "," + ps.PositionSMS_Position_ID + "," + ps.PositionSMS_State;
                    id = ps.PositionSMS_ID.ToString();
                };
                //执行更新
                if (PositionSMSDAL.UpdatePositionSMS(fun, action))
                {
                    MessageBox.Show("修改成功");
                }
                else
                {
                    MessageBox.Show("修改失败！");
                }
                CommonalityEntity.WriteLogData("修改", "更新编号为：" + id + "的短信提示信息,,修改前:" + strfront + ";修改后：" + strcontent + "", CommonalityEntity.USERNAME);

                //更新成功更更新XML的间隔数
                //txtjiange_setLoad();
            }
            catch
            {
                CommonalityEntity.WriteTextLog("SMSSetForm btnAdd_Click()" + "");
            }
            finally
            {
                GetGriddataviewLoad("");
                btnUpdate.Enabled = false;
                btnAdd.Enabled = true;
                Cler();
            }
        }
        #region 分页和加载DataGridView
        /// <summary>
        /// 加载信息
        /// </summary>
        public void GetGriddataviewLoad(string strClickedItemName)
        {
            Page.BindBoundControl(lvwUserList, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "PositionSMS", "*", "PositionSMS_ID", "PositionSMS_ID", 0, "", true);
        }

        EMEWE.CarManagement.Commons.CommonClass.PageControl Page = new EMEWE.CarManagement.Commons.CommonClass.PageControl();
        /// <summary>
        /// 选择每页条数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tscbxPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetGriddataviewLoad("");
        }
        /// <summary>
        /// 分页控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            GetGriddataviewLoad(e.ClickedItem.Name);
        }
        #endregion
        /// <summary>
        /// 绑定状态
        /// </summary>
        public void SelectPositionSMS_StateLoad()
        {
            combokPositionSMS_State.DataSource = DictionaryDAL.GetValueStateDictionary("01").Where(n => n.Dictionary_Name != "全部").Select(n => n).ToList();
            combokPositionSMS_State.ValueMember = "Dictionary_Value";
            combokPositionSMS_State.DisplayMember = "Dictionary_Name";

            chkboxSLEDState.DataSource = DictionaryDAL.GetValueStateDictionary("01");
            chkboxSLEDState.ValueMember = "Dictionary_Value";
            chkboxSLEDState.DisplayMember = "Dictionary_Name";
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Cler();
        }
        private void Cler()
        {
            txtPositionSMS_Content.Text = "";
            txtPositionSMS_Count.Text = "";
            chkPositionSMS_Position_Id.SelectedIndex = -1;
            combokPositionSMS_State.SelectedIndex = -1;
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            isUpdate = false;
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            string where = "1=1";
            if (chkboxSLEDState.Text.Trim() != "")
            {
                where += " and PositionSMS_State='" + chkboxSLEDState.Text.Trim() + "'";
            }
            else
            {
                where += " and 1=1";
            }
            if (chkSPositionLED_Position_Id.Text.Trim() != "")
            {
                where += " and PositionSMS_Position_ID=" + chkSPositionLED_Position_Id.SelectedValue.ToString() + "";
            }
            else
            {
                where += " and 1=1";
            }
            GetGriddataviewLoads("", where);
        }
        public void GetGriddataviewLoads(string strClickedItemName, string where)
        {
            Page.BindBoundControl(lvwUserList, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "PositionSMS", "*", "PositionSMS_ID", "PositionSMS_ID", 0, where, true);
            where = " 1=1";
            //Page.BindBoundControl(lvwUserList,strClickedItemName,tstbPageCurrent,tslPageCount,tslNMax,tscbxPageSize,"","","","",0,"",true);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.lvwUserList.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("请选择要删除的项！");
                    return;
                }
                if (this.lvwUserList.SelectedRows[0].Cells["PositionSMS_ID"].Value.ToString() == "")
                {
                    MessageBox.Show("请选择要删除的项！");
                    return;
                }
                if (this.lvwUserList.SelectedRows[0].Cells["PositionSMS_State"].Value.ToString() == "启动")
                {
                    MessageBox.Show("启用状态的打印设置不能删除！");
                    return;
                }
                DialogResult dlgResult = MessageBox.Show("确定删除选中的数据?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dlgResult == DialogResult.Cancel)
                {
                    return;
                }
                //删除条件
                int smsid = int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionSMS_ID"].Value.ToString());
                Expression<Func<PositionSMS, bool>> fun = n => n.PositionSMS_ID == smsid;
                if (PositionSMSDAL.DeletePositionSMS(fun))
                {
                    MessageBox.Show("删除成功");
                    CommonalityEntity.WriteLogData("删除", "删除短信编号为： " + smsid + "的信息", CommonalityEntity.USERNAME);//添加操作日志
                }
                else
                {
                    MessageBox.Show("删除失败！");
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("SMSSetForm btnDelete_Click()" + "");
            }
            finally
            {
                GetGriddataviewLoad("");//加载
                Cler();
            }
        }
        /// <summary>
        /// 选择修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvwUserList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.lvwUserList.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("请选择要修改的项！");
                    return;
                }
                if (this.lvwUserList.SelectedRows[0].Cells["PositionSMS_ID"].Value.ToString() == "")
                {
                    MessageBox.Show("请选择要修改的项！");
                    return;
                }
                else
                {
                    isUpdate = true;
                    string sql = "select * from PositionSMS where PositionSMS_ID='" + this.lvwUserList.SelectedRows[0].Cells["PositionSMS_ID"].Value.ToString() + "'";
                    PositionSMS ps = PositionSMSDAL.GetSMS(sql);
                    txtPositionSMS_Content.Text = ps.PositionSMS_Content.ToString();
                    txtPositionSMS_Count.Text = ps.PositionSMS_Remark.ToString();
                    combokPositionSMS_State.Text = ps.PositionSMS_State;
                    chkPositionSMS_Position_Id.SelectedIndex = int.Parse(ps.PositionSMS_Position_ID.ToString()) - 1;
                }
                btnUpdate.Enabled = true;
                btnDelete.Enabled = true;
                btnAdd.Enabled = false;
            }
            catch
            {
                CommonalityEntity.WriteTextLog("LEDSetForm lvwUserList_DoubleClick()" + "");//记录异常日志
            }
        }

        private void lvwUserList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

    }
}
