using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.Commons;
using EMEWE.CarManagement.Commons.CommonClass;
using System.Xml;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class VoiceSetForm : Form
    {
        /// <summary>
        /// 是否修改 true修改 false新增
        /// </summary>
        public bool isUpdate = false;
        public List<string> list = new List<string>();
        public VoiceSetForm()
        {
            InitializeComponent();
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
                btnAdd.Visible = ControlAttributes.BoolControl("btnAdd", "VoiceSetForm", "Visible");
                btnAdd.Enabled = ControlAttributes.BoolControl("btnAdd", "VoiceSetForm", "Enabled");

                btnDelete.Visible = ControlAttributes.BoolControl("btnDelete", "VoiceSetForm", "Visible");
                btnDelete.Enabled = ControlAttributes.BoolControl("btnDelete", "VoiceSetForm", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "VoiceSetForm", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "VoiceSetForm", "Enabled");

                btnSMSApplication.Visible = ControlAttributes.BoolControl("btnSMSApplication", "VoiceSetForm", "Visible");
                btnSMSApplication.Enabled = ControlAttributes.BoolControl("btnSMSApplication", "VoiceSetForm", "Enabled");
            }
        }

        /// <summary>
        /// 添加语音设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!ChkContent()) return;
                //添加数据到数据库
                PositionVoice pv = new PositionVoice();
                pv.PositionVoice_Position_ID = int.Parse(chkPositionVoice_Position_Id.SelectedValue.ToString());
                pv.PositionVoice_Remark = txtPositionVoice_Remark.Text.Trim();
                pv.PositionVoice_State = combokPositionVoice_State.Text.ToString();

                pv.PositionVoice_PassageState = cmbtxtPositionVoice_Type.Text;

                if (cmbtxtPositionVoice_Type.Text.Trim() == "欢迎语")
                {
                    pv.PositionVoice_Content = txtHY.Text;
                }
                else
                {
                    pv.PositionVoice_Count = int.Parse(txtPositionVoice_Count.Text.Trim());

                    string serialnumber = "";
                    string carType = "";
                    string carNumber = "";

                    if (chkCarNumber.Checked)
                    {
                        carNumber = chkCarNumber.Text.Trim() + ",";
                    }
                    if (chkCarType.Checked)
                    {
                        carType = chkCarType.Text.Trim() + ",";
                    }
                    if (chkSerialnumber.Checked)
                    {
                        serialnumber = chkSerialnumber.Text.Trim() + ",";
                    }
                    string chklists = "";
                    if (list.Count() > 0)
                    {
                        foreach (var item in list)
                        {
                            chklists += (item.ToString() + ",");
                        }
                        list.Clear();
                    }
                    string content = carType + carNumber + serialnumber + chklists;
                    pv.PositionVoice_Content = content;
                }
                pv.PositionVoice_Operate = CommonalityEntity.USERNAME;
                pv.PositionVoice_Time = CommonalityEntity.GetServersTime();
                if (pv.PositionVoice_State == "启动")
                {
                    if (ChkPositionVoiceState())
                    {
                        DialogResult dlgResult = MessageBox.Show("已经存在启动状态的设置，是否替换?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dlgResult == DialogResult.OK)
                        {

                            //修改条件
                            Expression<Func<PositionVoice, bool>> fun = n => n.PositionVoice_State == "启动" && n.PositionVoice_Position_ID == int.Parse(chkPositionVoice_Position_Id.SelectedValue.ToString());
                            //需要修改的内容
                            Action<PositionVoice> action = p =>
                            {
                                p.PositionVoice_State = "暂停";
                            };
                            //执行更新
                            PositionVoiceDAL.UpdatePositionVoice(fun, action);
                            PositionVoiceDAL.InsertPositionVoice(pv);
                        }
                        else
                        {
                            pv.PositionVoice_State = "暂停";
                            PositionVoiceDAL.InsertPositionVoice(pv);
                        }
                    }
                    else
                    {
                        PositionVoiceDAL.InsertPositionVoice(pv);
                    }
                }
                else
                {
                    PositionVoiceDAL.InsertPositionVoice(pv);
                }
                DataTable dt = LinQBaseDao.Query("select PositionVoice_ID,PositionVoice_State from PositionVoice order by PositionVoice_ID desc").Tables[0];
                string positionvoice_id = dt.Rows[0][0].ToString();
                string positionvoice_state = dt.Rows[0][1].ToString();
                if (positionvoice_state == "启动")
                {
                    CommonalityEntity.WriteLogData("新增", "新增并启用编号为：" + positionvoice_id + "的语音呼叫信息", CommonalityEntity.USERNAME);
                }
                else
                {
                    CommonalityEntity.WriteLogData("新增", "新增编号为：" + positionvoice_id + "的语音呼叫信息", CommonalityEntity.USERNAME);
                }

                MessageBox.Show("保存成功！");
            }
            catch 
            {
                //记录错误日志
                CommonalityEntity.WriteTextLog("VoiceSetForm btnAdd_Click()" + "");
            }
            finally
            {
                btnSelect_Click(sender, e);
                // GetGriddataviewLoad("");//绑定列表
                Empty();
            }
        }
        /// <summary>
        /// 修改语音设置
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
                //修改条件
                Expression<Func<PositionVoice, bool>> fun = n => n.PositionVoice_ID == int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionVoice_ID"].Value.ToString());
                string PositionVoice_State = combokPositionVoice_State.Text.Trim();
                if (combokPositionVoice_State.Text.Trim() == "启动")
                {
                    if (ChkPositionVoiceState())
                    {
                        DialogResult dlgResult = MessageBox.Show("已经存在启动状态的设置，是否替换?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dlgResult == DialogResult.OK)
                        {
                            //修改条件
                            Expression<Func<PositionVoice, bool>> funs = n => n.PositionVoice_State == "启动" && n.PositionVoice_Position_ID == int.Parse(chkPositionVoice_Position_Id.SelectedValue.ToString());
                            //需要修改的内容
                            Action<PositionVoice> actions = p =>
                            {
                                p.PositionVoice_State = "暂停";
                            };
                            //执行更新
                            PositionVoiceDAL.UpdatePositionVoice(funs, actions);

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
                Action<PositionVoice> action = pv =>
                {
                    strfront = pv.PositionVoice_Count + "," + pv.PositionVoice_Position_ID + "," + pv.PositionVoice_Remark + "," + pv.PositionVoice_State + "," + pv.PositionVoice_Content;
                    pv.PositionVoice_Position_ID = int.Parse(chkPositionVoice_Position_Id.SelectedValue.ToString());
                    pv.PositionVoice_Remark = txtPositionVoice_Remark.Text.Trim();
                    pv.PositionVoice_State = combokPositionVoice_State.Text.ToString();
                    pv.PositionVoice_PassageState = cmbtxtPositionVoice_Type.Text;
                    if (cmbtxtPositionVoice_Type.Text.Trim() == "欢迎语")
                    {
                        pv.PositionVoice_Content = txtHY.Text;
                    }
                    else
                    {
                        pv.PositionVoice_Count = int.Parse(txtPositionVoice_Count.Text.Trim());

                        string userialnumber = "";
                        string ucarType = "";
                        string ucarNumber = "";
                        if (chkCarNumber.Checked)
                        {
                            ucarNumber = chkCarNumber.Text.Trim() + ",";
                        }
                        if (chkCarType.Checked)
                        {
                            ucarType = chkCarType.Text.Trim() + ",";
                        }
                        if (chkSerialnumber.Checked)
                        {
                            userialnumber = chkSerialnumber.Text.Trim() + ",";
                        }
                        string uchklists = "";
                        if (list.Count() > 0)
                        {
                            foreach (var item in list)
                            {
                                uchklists += ("," + item.ToString());
                            }
                            list.Clear();
                        }
                        string content = ucarType + ucarNumber + userialnumber + uchklists;
                        pv.PositionVoice_Content = content;
                    }
                    id = pv.PositionVoice_ID.ToString();
                    strcontent = pv.PositionVoice_Count + "," + pv.PositionVoice_Position_ID + "," + pv.PositionVoice_Remark + "," + pv.PositionVoice_State + "," + pv.PositionVoice_Content;
                };
                //执行更新
                PositionVoiceDAL.UpdatePositionVoice(fun, action);
                isUpdate = false;

                txtjiange_end();

                CommonalityEntity.WriteLogData("修改", "更新编号为：" + id + "的语音呼叫信息,修改前：" + strfront + "；修改后：" + strcontent, CommonalityEntity.USERNAME);
                btnUpdate.Enabled = false;
                btnAdd.Enabled = true;


                MessageBox.Show("修改成功！");
            }
            catch 
            {
                //记录错误日志
                CommonalityEntity.WriteTextLog("VoiceSetForm btnAdd_Click()" + "");
            }
            finally
            {
                isUpdate = false;
                btnSelect_Click(sender, e);
                //GetGriddataviewLoad("");//绑定列表
                Empty();
            }
        }
        /// <summary>
        /// 清空事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Empty();
        }
        /// <summary>
        /// 清空方法
        /// </summary>
        private void Empty()
        {
            this.txtPositionVoice_Count.Text = "";
            this.txtPositionVoice_Remark.Text = "";
            combokPositionVoice_State.SelectedIndex = -1;

            chkCarNumber.Checked = false;
            chkCarType.Checked = false;
            chkSerialnumber.Checked = false;
            isUpdate = false;
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
        }
        /// <summary>
        /// 应用语音设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSMSApplication_Click(object sender, EventArgs e)
        {
            try
            {
                isUpdate = true;
                if (ChkPositionVoiceState())
                {
                    DialogResult dlgResult = MessageBox.Show("已经存在启动状态的设置，是否替换?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dlgResult == DialogResult.OK)
                    {
                        //修改条件
                        Expression<Func<PositionVoice, bool>> funs = n => n.PositionVoice_State == "启动" && n.PositionVoice_Position_ID == int.Parse(chkPositionVoice_Position_Id.SelectedValue.ToString());
                        //需要修改的内容
                        Action<PositionVoice> actions = p =>
                        {
                            p.PositionVoice_State = "暂停";
                        };
                        //执行更新
                        PositionVoiceDAL.UpdatePositionVoice(funs, actions);

                        //应用当前选中的设置
                        //条件
                        int id = int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionVoice_ID"].Value.ToString());
                        Expression<Func<PositionVoice, bool>> fun = n => n.PositionVoice_ID == id;
                        //需要的内容
                        Action<PositionVoice> action = p =>
                        {
                            p.PositionVoice_State = "启动";
                        };
                        //执行更新
                        PositionVoiceDAL.UpdatePositionVoice(fun, action);
                        CommonalityEntity.WriteLogData("启动", "启动编号为：" + id + "的语音呼叫信息", CommonalityEntity.USERNAME);
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
                    Expression<Func<PositionVoice, bool>> fun = n => n.PositionVoice_ID == int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionVoice_ID"].Value.ToString());
                    //需要的内容
                    Action<PositionVoice> action = p =>
                    {
                        p.PositionVoice_State = "启动";
                    };
                    //执行更新
                    PositionVoiceDAL.UpdatePositionVoice(fun, action);
                    CommonalityEntity.WriteLogData("启动", "启用语音编号为：" + int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionVoice_ID"].Value.ToString()), CommonalityEntity.USERNAME);
                }

            }
            catch 
            {
                CommonalityEntity.WriteTextLog("VoiceSetForm btnLEDApplication_Click()" + "");
            }
            finally
            {
                GetGriddataviewLoad("");//加载
            }
        }
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            string where = "1=1";
            if (chkboxSVoiceState.Text != "全部")
            {
                where += " and PositionVoice_State='" + chkboxSVoiceState.Text.Trim() + "'";
            }
            else
            {
                where += " and 1=1";
            }
            if (chkSPositionVoice_Position_Id.Text != "全部")
            {
                where += " and PositionVoice_Position_ID=" + chkSPositionVoice_Position_Id.SelectedValue.ToString() + "";
            }
            else
            {
                where += " and 1=1";
            }
            GetGriddataviewLoads("", where);
        }
        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <param name="strClickedItemName"></param>
        /// <param name="where"></param>
        public void GetGriddataviewLoads(string strClickedItemName, string where)
        {
            Page.BindBoundControl(lvwUserList, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "PositionVoice", "*", "PositionVoice_ID", "PositionVoice_ID", 0, where, true);
            where = " 1=1";
            //Page.BindBoundControl(lvwUserList,strClickedItemName,tstbPageCurrent,tslPageCount,tslNMax,tscbxPageSize,"","","","",0,"",true);
        }
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VoiceSetForm_Load(object sender, EventArgs e)
        {
            userContext();
            GetGriddataviewLoad("");//加载
            PositionLoad();
            PositionLED_StateLoad();
            txtjiange_Laod();
            btnUpdate.Enabled = false;
            groupBox2.Visible = false;
            groupBox3.Visible = false;
            cmbtxtPositionVoice_Type.SelectedIndex = 0;
        }

        /// <summary>
        /// 加载时绑定XML中的间隔数
        /// </summary>
        public void txtjiange_Laod()
        {

            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "SystemSet.xml");
            XmlNode xn = doc.SelectSingleNode("param");//查找<bookstore>
            XmlNodeList xnl = xn.ChildNodes;
            if (xnl.Count > 0)
            {
                foreach (XmlNode xnf in xnl)
                {
                    XmlElement xe = (XmlElement)xnf;
                    txtjiange.Text = xe.GetAttribute("HuJiaoJianGe").ToString();
                }
            }
        }

        /// <summary>
        /// 修改时修改XML中的间隔数
        /// </summary>
        public void txtjiange_end()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "SystemSet.xml");
            XmlNode xn = doc.SelectSingleNode("param");//查找<bookstore>
            XmlNodeList xnl = xn.ChildNodes;
            if (xnl.Count > 0)
            {
                foreach (XmlNode xnf in xnl)
                {
                    XmlElement xe = (XmlElement)xnf;
                    xe.SetAttribute("HuJiaoJianGe", txtjiange.Text.Trim());
                }
            }

        }

        /// <summary>
        /// 更多。。。。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblMore_Click(object sender, EventArgs e)
        {
            chkList.Items.Clear();
            groupBox3.Visible = true;
            //获取所有的字段信息。
            List<string> lists = PositionVoiceDAL.GetShow("GetLEDView", "View_ShowLED");
            foreach (var item in lists)
            {
                //去掉重复的字段
                if (item == "车牌号" || item == "排队号" || item == "车辆类型")
                {
                    continue;
                }
                //去掉英文字段
                System.Text.RegularExpressions.Regex chk = new System.Text.RegularExpressions.Regex(@"[a-z]");
                if (chk.IsMatch(item))
                {
                    continue;
                }
                chkList.Items.Add(item);
            }
            chkList.BringToFront();
        }
        #region 分页和加载DataGridView
        /// <summary>
        /// 加载信息
        /// </summary>
        public void GetGriddataviewLoad(string strClickedItemName)
        {
            Page.BindBoundControl(lvwUserList, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "PositionVoice", "*", "PositionVoice_ID", "PositionVoice_ID", 0, "", true);
            //Page.BindBoundControl(lvwUserList,strClickedItemName,tstbPageCurrent,tslPageCount,tslNMax,tscbxPageSize,"","","","",0,"",true);
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
                if (this.lvwUserList.SelectedRows[0].Cells["PositionVoice_ID"].Value.ToString() == "")
                {
                    MessageBox.Show("请选择要删除的项！");
                    return;
                }
                if (this.lvwUserList.SelectedRows[0].Cells["PositionVoice_State"].Value.ToString() == "启动")
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
                int ii = int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionVoice_ID"].Value.ToString());
                Expression<Func<PositionVoice, bool>> fun = n => n.PositionVoice_ID == ii;
                PositionVoiceDAL.DeletePositionVoice(fun);
                CommonalityEntity.WriteLogData("删除", "删除编号为" + ii + "的语音呼叫信息", CommonalityEntity.USERNAME);//添加操作日志
                MessageBox.Show("删除成功！");

            }
            catch 
            {
                CommonalityEntity.WriteTextLog("VoiceSetForm btnDelete_Click()" + "");
            }
            finally
            {
                btnSelect_Click(sender, e);
                //GetGriddataviewLoad("");//加载
            }
        }
        /// <summary>
        /// 保存选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
            foreach (var item in chkList.CheckedItems)
            {
                list.Add(item.ToString());
            }
        }
        /// <summary>
        /// 取消选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCel_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
            list.Clear();
        }
        /// <summary>
        /// 绑定门岗
        /// </summary>
        public void PositionLoad()
        {
            string sql = "Select * from Position where position_State='启动'";

            DataTable dt = LinQBaseDao.Query(sql).Tables[0];


            chkPositionVoice_Position_Id.DataSource = LinQBaseDao.Query(sql).Tables[0];

            chkPositionVoice_Position_Id.ValueMember = "Position_Id";
            chkPositionVoice_Position_Id.DisplayMember = "Position_Name";



            DataRow row = dt.NewRow();
            row["Position_Id"] = "0";
            row["Position_Name"] = "全部";
            dt.Rows.InsertAt(row, 0);

            chkSPositionVoice_Position_Id.DataSource = dt;
            chkSPositionVoice_Position_Id.ValueMember = "Position_Id";
            chkSPositionVoice_Position_Id.DisplayMember = "Position_Name";

        }
        public void PositionLED_StateLoad()
        {
            List<Dictionary> list = DictionaryDAL.GetValueStateDictionary("01").Where(n => n.Dictionary_Name != "全部").Select(n => n).ToList();
            combokPositionVoice_State.DataSource = DictionaryDAL.GetValueStateDictionary("01").Where(n => n.Dictionary_Name != "全部").Select(n => n).ToList();
            chkboxSVoiceState.ValueMember = "Dictionary_Value";
            chkboxSVoiceState.DisplayMember = "Dictionary_Name";


            Dictionary d = new Dictionary();
            d.Dictionary_Value = "";
            d.Dictionary_Name = "全部";
            list.Insert(0, d);

            chkboxSVoiceState.DataSource = list;
            combokPositionVoice_State.ValueMember = "Dictionary_Value";
            combokPositionVoice_State.DisplayMember = "Dictionary_Name";


        }
        /// <summary>
        /// 验证LED设置状态是否重复
        /// </summary>
        /// <returns></returns>
        public bool ChkPositionVoiceState()
        {
            bool chkState = false;
            try
            {
                string sql = "";
                if (isUpdate)
                {
                    sql = "Select * from PositionVoice where PositionVoice_State='启动' and PositionVoice_id!=" + int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionVoice_ID"].Value.ToString()) + "  and PositionVoice_Position_ID = " + int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionVoice_Position_ID"].Value.ToString()) + "";
                }
                else
                {
                    sql = "Select * from PositionVoice where PositionVoice_State='启动'  and PositionVoice_Position_ID = " + chkPositionVoice_Position_Id.SelectedValue + "";
                }
                chkState = PositionVoiceDAL.ChkPositionVoiceState(sql);
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("VoiceSetForm ChkPositionVoiceState()" + "");
            }
            return chkState;
        }
        private void lvwUserList_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.lvwUserList.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("请选择要修改的项！");
                    return;
                }
                if (this.lvwUserList.SelectedRows[0].Cells["PositionVoice_ID"].Value.ToString() == "")
                {
                    MessageBox.Show("请选择要修改的项！");
                    return;
                }
                else
                {
                    isUpdate = true;
                    string sql = "select * from PositionVoice where PositionVoice_ID='" + this.lvwUserList.SelectedRows[0].Cells["PositionVoice_ID"].Value.ToString() + "'";
                    PositionVoice pv = PositionVoiceDAL.GetVoice(sql);
                    txtPositionVoice_Count.Text = pv.PositionVoice_Count.ToString();
                    txtPositionVoice_Remark.Text = pv.PositionVoice_Remark.ToString();
                    cmbtxtPositionVoice_Type.Text = pv.PositionVoice_PassageState;
                    chkPositionVoice_Position_Id.SelectedValue = int.Parse(pv.PositionVoice_Position_ID.ToString());
                    combokPositionVoice_State.Text = pv.PositionVoice_State.ToString();
                    string[] content = pv.PositionVoice_Content.Split(',');

                    chkCarNumber.Checked = false;
                    chkSerialnumber.Checked = false;
                    chkCarType.Checked = false;
                    foreach (var item in content)
                    {
                        if (item == chkCarNumber.Text)
                        {
                            chkCarNumber.Checked = true;
                        }

                        if (item == chkSerialnumber.Text)
                        {
                            chkSerialnumber.Checked = true;
                        }

                        if (item == chkCarType.Text)
                        {

                            chkCarType.Checked = true;
                        }

                    }
                }
                btnAdd.Enabled = false;
                btnUpdate.Enabled = true;

            }
            catch 
            {
                CommonalityEntity.WriteTextLog("VoiceSetForm lvwUserList_DoubleClick()" + "");//记录异常日志
            }
        }

        private void lvwUserList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        private void cmbtxtPositionVoice_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbtxtPositionVoice_Type.Text.Trim() == "欢迎语")
            {
                groupBox2.Visible = true;
                gbSort.Visible = false;
                groupBox3.Visible = false;
            }
            else
            {
                gbSort.Visible = true;
                groupBox2.Visible = false;
            }
        }


    }
}
