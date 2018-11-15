using EMEWE.CarManagement.CommonClass;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class LEDSetForm : Form
    {
        public List<string> list = new List<string>();
        /// <summary>
        /// 是否是修改 0修改 1新增 -1应用
        /// </summary>
        public int isUpdate = 1;
        public MainForm mf;
        public LEDSetForm()
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
                btnLEDApplication.Enabled = true;
                btnLEDApplication.Visible = true;
                btnLEDShow.Enabled = true;
                btnLEDShow.Visible = true;
                btnCancellation.Enabled = true;
                btnCancellation.Visible = true;

            }
            else
            {
                btnAdd.Visible = ControlAttributes.BoolControl("btnAdd", "LEDSetForm", "Visible");
                btnAdd.Enabled = ControlAttributes.BoolControl("btnAdd", "LEDSetForm", "Enabled");

                btnDelete.Visible = ControlAttributes.BoolControl("btnDelete", "LEDSetForm", "Visible");
                btnDelete.Enabled = ControlAttributes.BoolControl("btnDelete", "LEDSetForm", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "LEDSetForm", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "LEDSetForm", "Enabled");

                btnLEDApplication.Visible = ControlAttributes.BoolControl("btnLEDApplication", "LEDSetForm", "Visible");
                btnLEDApplication.Enabled = ControlAttributes.BoolControl("btnLEDApplication", "LEDSetForm", "Enabled");

                btnLEDShow.Visible = ControlAttributes.BoolControl("btnLEDShow", "LEDSetForm", "Visible");
                btnLEDShow.Enabled = ControlAttributes.BoolControl("btnLEDShow", "LEDSetForm", "Enabled");

                btnCancellation.Visible = ControlAttributes.BoolControl("btnCancellation", "LEDSetForm", "Visible");
                btnCancellation.Enabled = ControlAttributes.BoolControl("btnCancellation", "LEDSetForm", "Enabled");
            }
        }
        /// <summary>
        /// 保存用户操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                isUpdate = 1;
                if (!ChkContent()) return;
                //判断是否修改

                //添加数据到数据库
                PositionLED pLED = new PositionLED();

                #region 获取页面的数据
                pLED.PositionLED_Position_ID = int.Parse(chkPositionLED_Position_Id.SelectedValue.ToString());
                if (cboleixing.Text == "排队信息")
                {
                    pLED.PositionLED_Type = 1;
                }
                else if (cboleixing.Text == "欢迎语")
                {
                    pLED.PositionLED_Type = 2;
                }
                pLED.PositionLED_ScreenHeight = int.Parse(txtPositionLED_ScreenHeight.Text.Trim());
                pLED.PositionLED_ScreenWeight = int.Parse(txtPositionLED_ScreenWeight.Text.Trim());
                pLED.PositionLED_X = int.Parse(txtPositionLED_X.Text.Trim());
                pLED.PositionLED_Y = int.Parse(txtPositionLED_Y.Text.Trim());
                pLED.PositionLED_Count = int.Parse(cbotiaoshu.Text.Trim());
                pLED.PositionLED_Remark = txtPositionLED_Remark.Text.Trim();
                //pLED.PositionLED_PassageState = chkboxLEDPassState.Text.ToString();


                //保存选择的字体设置项目
                pLED.PositionLED_Font = fontdlgFont.Font.Name.ToString();
                pLED.PositionLED_FontSize = fontdlgFont.Font.Size.ToString();
                pLED.PositionLED_Color = colordlgFont.Color.ToString();

                if (cboleixing.Text == "欢迎语")
                {
                    pLED.PositionLED_Content = "";
                    pLED.PositionLED_Operate = CommonalityEntity.USERNAME;
                    pLED.PositionLED_Time = CommonalityEntity.GetServersTime();
                #endregion
                    pLED.PositionLED_State = chkboxLEDState.Text.Trim();
                    if (chkboxLEDState.Text.Trim() == "启动")
                    {
                        if (ChkPositionLEDState())
                        {
                            DialogResult dlgResult = MessageBox.Show("已经存在启动状态的设置，是否替换?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                            if (dlgResult == DialogResult.OK)
                            {

                                //修改条件
                                Expression<Func<PositionLED, bool>> fun = n => n.PositionLED_State == "启动" && n.PositionLED_Position_ID == int.Parse(chkPositionLED_Position_Id.SelectedValue.ToString());
                                //需要修改的内容
                                Action<PositionLED> action = p =>
                                {
                                    p.PositionLED_State = "暂停";
                                };
                                //执行更新
                                PositionLEDDAL.UpdatePositionLED(fun, action);
                                PositionLEDDAL.InsertPositionLED(pLED);
                            }
                            else
                            {
                                pLED.PositionLED_State = "暂停";
                                PositionLEDDAL.InsertPositionLED(pLED);
                            }
                        }
                        else
                        {
                            PositionLEDDAL.InsertPositionLED(pLED);
                        }
                    }
                    else
                    {
                        PositionLEDDAL.InsertPositionLED(pLED);
                    }
                    DataTable dt = dt = LinQBaseDao.Query("select PositionLED_ID,PositionLED_State from PositionLED order by PositionLED_ID desc").Tables[0];
                    string positionled_id = dt.Rows[0][0].ToString();
                    string state = dt.Rows[0][1].ToString();
                    if (state == "启动")
                        CommonalityEntity.WriteLogData("新增", "新增并启动编号为：" + positionled_id + "的LED显示信息", CommonalityEntity.USERNAME);//添加操作日志
                    else
                        CommonalityEntity.WriteLogData("新增", "新增编号为：" + positionled_id + "的LED显示信息", CommonalityEntity.USERNAME);//添加操作日志
                }
                else if (cboleixing.Text == "排队信息")
                {
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
                        serialnumber = "distinct(" + chkSerialnumber.Text.Trim() + "),";
                    }
                    string chklists = "";
                    if (list.Count() > 1)
                    {
                        foreach (var item in list)
                        {
                            chklists += (item.ToString() + ",");
                        }
                        list.Clear();
                    }
                    string listsql = serialnumber + carNumber + carType + chklists;
                    listsql = listsql.TrimEnd(',');
                    //限定选择的显示内容只能为3个
                    string[] split = listsql.Split(',');
                    int num = 0;
                    for (int i = 0; i < split.Length; i++)
                    {
                        num++;
                    }
                    if (num > 3)
                    {
                        MessageBox.Show("排队信息显示的项目最多选择3项", "错误提示");
                        return;
                    }
                    else
                    {
                        string sql = "Select " + listsql + " from View_LEDShow_zj";
                        pLED.PositionLED_Content = sql;
                        pLED.PositionLED_Operate = CommonalityEntity.USERNAME;
                        pLED.PositionLED_Time = CommonalityEntity.GetServersTime();
                        pLED.PositionLED_State = chkboxLEDState.Text.Trim();
                        if (chkboxLEDState.Text.Trim() == "启动")
                        {
                            if (ChkPositionLEDState())
                            {
                                DialogResult dlgResult = MessageBox.Show("已经存在启动状态的设置，是否替换?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                                if (dlgResult == DialogResult.OK)
                                {

                                    //修改条件
                                    Expression<Func<PositionLED, bool>> fun = n => n.PositionLED_State == "启动" && n.PositionLED_Position_ID == int.Parse(chkPositionLED_Position_Id.SelectedValue.ToString());
                                    //需要修改的内容
                                    Action<PositionLED> action = p =>
                                    {
                                        p.PositionLED_State = "暂停";
                                    };
                                    //执行更新
                                    PositionLEDDAL.UpdatePositionLED(fun, action);
                                    PositionLEDDAL.InsertPositionLED(pLED);
                                }
                                else
                                {
                                    pLED.PositionLED_State = "暂停";
                                    PositionLEDDAL.InsertPositionLED(pLED);
                                }
                            }
                            else
                            {
                                PositionLEDDAL.InsertPositionLED(pLED);
                            }
                        }
                        else
                        {
                            PositionLEDDAL.InsertPositionLED(pLED);
                        }
                        DataTable dt = dt = LinQBaseDao.Query("select PositionLED_ID,PositionLED_State from PositionLED order by PositionLED_ID desc").Tables[0];
                        string positionled_id = dt.Rows[0][0].ToString();
                        string state = dt.Rows[0][1].ToString();
                        if (state == "启动")
                            CommonalityEntity.WriteLogData("新增", "新增并启动编号为：" + positionled_id + "的LED显示信息", CommonalityEntity.USERNAME);//添加操作日志
                        else
                            CommonalityEntity.WriteLogData("新增", "新增编号为：" + positionled_id + "的LED显示信息", CommonalityEntity.USERNAME);//添加操作日志
                    }
                }
            }
            catch 
            {
                //记录错误日志
                CommonalityEntity.WriteTextLog("LEDSetForm btnAdd_Click()");
            }
            finally
            {
                GetGriddataviewLoad("");//绑定列表
                Empty();
            }
        }
        /// <summary>
        /// 更多选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblMore_Click(object sender, EventArgs e)
        {
            try
            {

                chkList.Items.Clear();
                chkList.Visible = true;
                btnOK.Visible = true;
                btnCel.Visible = true;
                //获取所有的字段信息。
                List<string> lists = PositionLEDDAL.GetLEDShow("GetLEDView", "View_ShowLED");
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

            }
            catch 
            {
            }
        }
        /// <summary>
        /// 确定选中的字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            btnCel.Visible = false;
            btnOK.Visible = false;
            chkList.Visible = false;
            foreach (var item in chkList.CheckedItems)
            {
                list.Add(item.ToString());
            }
        }
        /// <summary>
        /// 取消选中的字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCel_Click(object sender, EventArgs e)
        {
            btnCel.Visible = false;
            btnOK.Visible = false;
            chkList.Visible = false;
            list.Clear();
        }
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LEDForm_Load(object sender, EventArgs e)
        {
            label2.Visible = true;
            chkSerialnumber.Visible = true;
            chkCarNumber.Visible = true;
            chkCarType.Visible = true;
            lblMore.Visible = true;
            label1.Visible = true;
            cbotiaoshu.Visible = true;


            userContext();
            mf = new MainForm();
            //lvwUserList.Rows[0].Selected = false;
            PositionLoad();//绑定门岗
            GetGriddataviewLoad("");//绑定列表
            PositionLED_StateLoad();//绑定状态
            SelectPositionLoad();//绑定状态
            btnUpdate.Enabled = false;
            cbotiaoshu.SelectedIndex = 0;
            //SelectPositionLED_StateLoad();//绑定门岗

        }
        /// <summary>
        /// 绑定门岗
        /// </summary>
        public void PositionLoad()
        {
            string sql = "Select Position_Id,Position_Name from Position where Position_State='启动'";
            chkPositionLED_Position_Id.DataSource = PositionDAL.GetPositionID(sql);
            chkPositionLED_Position_Id.ValueMember = "Position_Id";
            chkPositionLED_Position_Id.DisplayMember = "Position_Name";

            chkSPositionLED_Position_Id.DataSource = PositionDAL.GetPositionID(sql);
            chkSPositionLED_Position_Id.ValueMember = "Position_Id";
            chkSPositionLED_Position_Id.DisplayMember = "Position_Name";
        }
        /// <summary>
        /// 绑定状态
        /// </summary>
        public void PositionLED_StateLoad()
        {
            chkboxLEDState.DataSource = DictionaryDAL.GetValueStateDictionary("01").Where(n => n.Dictionary_Name != "全部").Select(n => n).ToList();

            chkboxLEDState.ValueMember = "Dictionary_Value";
            chkboxLEDState.DisplayMember = "Dictionary_Name";
        }
        /// <summary>
        /// 选中列表中的项进行修改
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
                else if (!ChkContent()) return;
                //修改条件
                Expression<Func<PositionLED, bool>> fun = n => n.PositionLED_ID == int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionLED_ID"].Value.ToString());
                string positionLED_State = chkboxLEDState.Text.Trim();
                if (chkboxLEDState.Text.Trim() == "启动")
                {
                    if (ChkPositionLEDState())
                    {
                        DialogResult dlgResult = MessageBox.Show("已经存在启动状态的设置，是否替换?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dlgResult == DialogResult.OK)
                        {
                            //修改条件
                            Expression<Func<PositionLED, bool>> funs = n => n.PositionLED_State == "启动" && n.PositionLED_Position_ID == int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionLED_Position_ID"].Value.ToString());
                            //需要修改的内容
                            Action<PositionLED> actions = p =>
                            {
                                p.PositionLED_State = "暂停";
                            };
                            //执行更新
                            PositionLEDDAL.UpdatePositionLED(funs, actions);
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
                Action<PositionLED> action = pLED =>
                {
                    strfront = pLED.PositionLED_Position_ID + "," + pLED.PositionLED_ScreenHeight + "," + pLED.PositionLED_ScreenWeight + "," + pLED.PositionLED_X + "," + pLED.PositionLED_Y + "," + pLED.PositionLED_IntervalX + "," + pLED.PositionLED_IntervalY + "," + pLED.PositionLED_Count + "," + pLED.PositionLED_Remark + "," + pLED.PositionLED_State + "," + pLED.PositionLED_Font + "," + pLED.PositionLED_FontSize + "," + pLED.PositionLED_Color + "," + pLED.PositionLED_Content;
                    pLED.PositionLED_Position_ID = int.Parse(chkPositionLED_Position_Id.SelectedValue.ToString());
                    if (cboleixing.Text == "排队信息")
                    {
                        pLED.PositionLED_Type = 1;
                    }
                    else if (cboleixing.Text == "欢迎语")
                    {

                        pLED.PositionLED_Type = 2;
                    }
                    pLED.PositionLED_ScreenHeight = int.Parse(txtPositionLED_ScreenHeight.Text.Trim());
                    pLED.PositionLED_ScreenWeight = int.Parse(txtPositionLED_ScreenWeight.Text.Trim());
                    pLED.PositionLED_X = int.Parse(txtPositionLED_X.Text.Trim());
                    pLED.PositionLED_Y = int.Parse(txtPositionLED_Y.Text.Trim());
                    pLED.PositionLED_Count = int.Parse(cbotiaoshu.Text.Trim());
                    pLED.PositionLED_Remark = txtPositionLED_Remark.Text.Trim();
                    //pLED.PositionLED_PassageState = chkboxLEDPassState.Text.ToString();
                    pLED.PositionLED_State = chkboxLEDState.Text.Trim();
                    pLED.PositionLED_Font = fontdlgFont.Font.Name.ToString();
                    pLED.PositionLED_FontSize = fontdlgFont.Font.Size.ToString();
                    pLED.PositionLED_Color = colordlgFont.Color.ToString();
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
                        userialnumber = "distinct(" + chkSerialnumber.Text.Trim() + "),";
                    }
                    string uchklists = "";
                    if (list.Count() > 0)
                    {
                        foreach (var item in list)
                        {
                            uchklists += (item.ToString() + ",");
                        }
                        list.Clear();
                    }
                    string ulistsql = userialnumber + ucarNumber + ucarType + uchklists;

                    string sql = "Select " + ulistsql.TrimEnd(',') + " from View_LEDShow_zj";
                    pLED.PositionLED_Content = sql;
                    strcontent = pLED.PositionLED_Position_ID + "," + pLED.PositionLED_ScreenHeight + "," + pLED.PositionLED_ScreenWeight + "," + pLED.PositionLED_X + "," + pLED.PositionLED_Y + "," + pLED.PositionLED_IntervalX + "," + pLED.PositionLED_IntervalY + "," + pLED.PositionLED_Count + "," + pLED.PositionLED_Remark + "," + pLED.PositionLED_State + "," + pLED.PositionLED_Font + "," + pLED.PositionLED_FontSize + "," + pLED.PositionLED_Color + "," + pLED.PositionLED_Content;
                    id = pLED.PositionLED_ID.ToString();
                };
                //执行更新
                PositionLEDDAL.UpdatePositionLED(fun, action);
                CommonalityEntity.WriteLogData("修改", "更新编号为：" + id + "的LED显示信息,修改前：" + strfront + "；修改后：" + strcontent, CommonalityEntity.USERNAME);
                btnUpdate.Enabled = false;
                btnAdd.Enabled = true;
            }
            catch
            {
                //记录错误日志
                CommonalityEntity.WriteTextLog("LEDSetForm btnAdd_Click()" );
            }
            finally
            {
                GetGriddataviewLoad("");//绑定列表
                Empty();
            }
        }
        #region 分页和加载DataGridView
        /// <summary>
        /// 加载信息
        /// </summary>
        public void GetGriddataviewLoad(string strClickedItemName)
        {
            Page.BindBoundControl(lvwUserList, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "PositionLED", "*", "PositionLED_ID", "PositionLED_ID", 0, "", true);
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
        /// 设置LED屏显示的字体样式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFontSet_Click(object sender, EventArgs e)
        {
            fontdlgFont.ShowDialog();
            colordlgFont.ShowDialog();
        }
        /// <summary>
        /// 验证数据输入是否正确，以及是否已经输入
        /// </summary>
        /// <returns></returns>
        public bool ChkContent()
        {
            //去掉英文字段
            System.Text.RegularExpressions.Regex chk = new System.Text.RegularExpressions.Regex(@"[a-z]");
            if (string.IsNullOrEmpty(cbotiaoshu.Text.Trim()) || chk.IsMatch(cbotiaoshu.Text.Trim()))
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "LED数据条数不能为空且只能为数字！", cbotiaoshu, this);
                return false;
            }
            if (string.IsNullOrEmpty(txtPositionLED_X.Text.Trim()) || chk.IsMatch(txtPositionLED_X.Text.Trim()))
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "LED显示横坐标不能为空且只能为数字！", txtPositionLED_X, this);
                return false;
            }
            if (string.IsNullOrEmpty(txtPositionLED_Y.Text.Trim()) || chk.IsMatch(txtPositionLED_Y.Text.Trim()))
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "LED显示纵坐标不能为空且只能为数字！", txtPositionLED_Y, this);
                return false;
            }
            if (string.IsNullOrEmpty(txtPositionLED_ScreenHeight.Text.Trim()) || chk.IsMatch(txtPositionLED_ScreenHeight.Text.Trim()))
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "LED屏幕高度不能为空且只能为数字！", txtPositionLED_ScreenHeight, this);
                return false;
            }
            if (string.IsNullOrEmpty(txtPositionLED_ScreenWeight.Text.Trim()) || chk.IsMatch(txtPositionLED_ScreenWeight.Text.Trim()))
            {
                PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "LED屏幕宽度不能为空且只能为数字！", txtPositionLED_ScreenWeight, this);
                return false;
            }
            if (cboleixing.SelectedIndex == 1)
            {
                if (string.IsNullOrEmpty(txtPositionLED_Remark.Text.Trim()) || chk.IsMatch(txtPositionLED_Remark.Text.Trim()))
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "LED欢迎语不能为空！", txtPositionLED_ScreenWeight, this);
                    return false;
                }
            }
            else if (cboleixing.SelectedIndex == 0)
            {
                if (chkCarNumber.Checked == false && chkCarType.Checked == false && chkSerialnumber.Checked == false && chkList.CheckedItems.Count <= 0)
                {
                    PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "LED显示内容不能为空！", chkSerialnumber, this);
                    return false;
                }
            }
            try
            {
                Int32 i = Convert.ToInt32(cbotiaoshu.Text);
                Int32 j = Convert.ToInt32(txtPositionLED_X.Text);
                Int32 k = Convert.ToInt32(txtPositionLED_Y.Text);
                Int32 l = Convert.ToInt32(txtPositionLED_ScreenHeight.Text);
                Int32 m = Convert.ToInt32(txtPositionLED_ScreenWeight.Text);
            }
            catch 
            {
                // PublicClass.ShowToolTip(ToolTipIcon.Info, "提示", "LED显示内容不能为空！", txtPositionLED_Y, this);
                return false;
            }
            return true;
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
                if (this.lvwUserList.SelectedRows[0].Cells["PositionLED_ID"].Value.ToString() == "")
                {
                    MessageBox.Show("请选择要删除的项！");
                    return;
                }
                for (int i = 0; i < this.lvwUserList.SelectedRows.Count; i++)
                {
                    if (this.lvwUserList.SelectedRows[i].Cells["PositionLED_State"].Value.ToString() == "启动")
                    {
                        MessageBox.Show("启动状态的LED设置，不能删除！");
                        return;
                    }
                    //删除条件
                    int positionled_id = int.Parse(this.lvwUserList.SelectedRows[i].Cells["positionLED_ID"].Value.ToString());
                    Expression<Func<PositionLED, bool>> fun = n => n.PositionLED_ID == positionled_id;
                    PositionLEDDAL.DeletePositionLED(fun);
                    CommonalityEntity.WriteLogData("删除", "删除编号为：" + positionled_id + "的LED显示信息", CommonalityEntity.USERNAME);//添加操作日志
                }

            }
            catch 
            {
                CommonalityEntity.WriteTextLog("LEDSetForm btnDelete_Click()" );
            }
            finally
            {
                GetGriddataviewLoad("");//加载
            }
        }
        /// <summary>
        /// 清空按钮
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
            this.cbotiaoshu.SelectedIndex = 0;
            this.txtPositionLED_Remark.Text = "";
            this.txtPositionLED_ScreenHeight.Text = "";
            this.txtPositionLED_ScreenWeight.Text = "";
            this.cboleixing.SelectedIndex = 0;
            this.txtPositionLED_X.Text = "";
            this.txtPositionLED_Y.Text = "";
            //chkboxLEDPassState.Items.Clear();
            //chkboxLEDState.Items.Clear();
            //chkboxLEDPassState.SelectedIndex = -1;
            chkboxLEDState.SelectedIndex = -1;
            chkCarNumber.Checked = false;
            chkCarType.Checked = false;
            chkSerialnumber.Checked = false;
            isUpdate = 1;
            chkSPositionLED_Position_Id.SelectedIndex = -1;
            chkboxSLEDState.SelectedIndex = -1;
            btnUpdate.Enabled = false;
            btnAdd.Enabled = true;
        }
        /// <summary>
        /// 应用当前选中的LED设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLEDApplication_Click(object sender, EventArgs e)
        {
            try
            {
                CommonalityEntity.IsCancellation = false;
                isUpdate = -1;
                if (ChkPositionLEDState())
                {
                    DialogResult dlgResult = MessageBox.Show("已经存在启动状态的设置，是否替换?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dlgResult == DialogResult.OK)
                    {
                        //修改条件
                        Expression<Func<PositionLED, bool>> funs = n => n.PositionLED_State == "启动" && n.PositionLED_Position_ID == int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionLED_Position_ID"].Value.ToString());
                        //需要修改的内容
                        Action<PositionLED> actions = p =>
                        {
                            p.PositionLED_State = "暂停";
                        };
                        //执行更新
                        PositionLEDDAL.UpdatePositionLED(funs, actions);

                        //应用当前选中的设置
                        //条件
                        int id = int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionLED_ID"].Value.ToString());
                        Expression<Func<PositionLED, bool>> fun = n => n.PositionLED_ID == id;
                        //需要的内容
                        Action<PositionLED> action = p =>
                        {
                            p.PositionLED_State = "启动";
                        };
                        //执行更新
                        PositionLEDDAL.UpdatePositionLED(fun, action);
                        CommonalityEntity.WriteLogData("修改", "启动编号为：" + id + "的LED显示信息", CommonalityEntity.USERNAME);//操作日志
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
                    int id = int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionLED_ID"].Value.ToString());
                    Expression<Func<PositionLED, bool>> fun = n => n.PositionLED_ID == id;
                    //需要的内容
                    Action<PositionLED> action = p =>
                    {
                        p.PositionLED_State = "启动";
                    };
                    //执行更新
                    PositionLEDDAL.UpdatePositionLED(fun, action);
                    CommonalityEntity.WriteLogData("修改", "启动编号为：" + id + "的LED显示信息", CommonalityEntity.USERNAME);//操作日志
                }

            }
            catch 
            {
                CommonalityEntity.WriteTextLog("LEDSerForm btnLEDApplication_Click()" );
            }
            finally
            {
                GetGriddataviewLoad("");//加载
            }
        }
        /// <summary>
        /// 取消LED显示 不显示LED信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancellation_Click(object sender, EventArgs e)
        {
            CommonalityEntity.IsCancellation = true;
            CommonalityEntity.WriteLogData("修改", "注销了LED显示", CommonalityEntity.USERNAME);//操作日志
        }
        /// <summary>
        /// 验证LED设置状态是否重复
        /// </summary>
        /// <returns></returns>
        public bool ChkPositionLEDState()
        {
            bool chkState = false;
            try
            {
                string sql = "";
                if (isUpdate == 0)
                {
                    sql = "Select * from PositionLED where PositionLED_State='启动' and PositionLED_id!=" + int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionLED_id"].Value.ToString()) + "  and PositionLED_Position_ID = " + chkPositionLED_Position_Id.SelectedValue + "";
                }
                else if (isUpdate == -1)
                {
                    sql = "Select * from PositionLED where PositionLED_State='启动' and PositionLED_id!=" + int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionLED_id"].Value.ToString()) + "  and PositionLED_Position_ID = " + int.Parse(this.lvwUserList.SelectedRows[0].Cells["PositionLED_Position_ID"].Value.ToString()) + "";
                }
                else
                {
                    sql = "Select * from PositionLED where PositionLED_State='启动'  and PositionLED_Position_ID = " + chkPositionLED_Position_Id.SelectedValue + "";
                }
                chkState = PositionLEDDAL.ChkPositionLEDState(sql);
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("LEDSetForm ChkPositionLEDState()" );
            }
            return chkState;
        }
        /// <summary>
        /// LED预览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLEDShow_Click(object sender, EventArgs e)
        {
            try
            {
                CommonalityEntity.PositionLED_ID = int.Parse(this.lvwUserList.SelectedRows[0].Cells[0].Value.ToString());
                LEDPreviewForm led = new LEDPreviewForm();
                PublicClass.ShowChildForm(led);
                //mf = new MainForm();
                //mf.ShowChildForm(l, this);
                //l.Show();

            }
            catch
            {
                CommonalityEntity.WriteTextLog("LEDSetForm btnLEDShow_Click()" );
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
            if (chkboxSLEDState.Text.Trim() != "")
            {
                where += " and PositionLED_State='" + chkboxSLEDState.Text.Trim() + "'";
            }
            if (chkSPositionLED_Position_Id.Text.Trim() != "")
            {
                where += " and PositionLED_Position_ID=" + chkSPositionLED_Position_Id.SelectedValue.ToString() + "";
            }
            GetGriddataviewLoads("", where);
        }
        public void GetGriddataviewLoads(string strClickedItemName, string where)
        {
            Page.BindBoundControl(lvwUserList, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "PositionLED", "*", "PositionLED_ID", "PositionLED_ID", 0, where, true);
            where = " 1=1";
            //Page.BindBoundControl(lvwUserList,strClickedItemName,tstbPageCurrent,tslPageCount,tslNMax,tscbxPageSize,"","","","",0,"",true);
        }
        /// <summary>
        /// 绑定门岗
        /// </summary>
        public void SelectPositionLoad()
        {
            string sql = "Select * from Position";
            chkSPositionLED_Position_Id.DataSource = PositionDAL.GetPositionID(sql);
            chkSPositionLED_Position_Id.ValueMember = "Position_Id";
            chkSPositionLED_Position_Id.DisplayMember = "Position_Name";
        }
        /// <summary>
        /// 绑定状态
        /// </summary>
        public void SelectPositionLED_StateLoad()
        {
            chkboxSLEDState.DataSource = DictionaryDAL.GetValueStateDictionary("01");
            chkboxSLEDState.ValueMember = "Dictionary_Value";
            chkboxSLEDState.DisplayMember = "Dictionary_Name";
        }

        //将LV中的数据显示到上方
        private void lvwUserList_DoubleClick(object sender, EventArgs e)
        {
            #region 备用
            //try
            //{
            //    if (this.lvwUserList.SelectedRows.Count <= 0)
            //    {
            //        MessageBox.Show("请选择要修改的项！");
            //        return;
            //    }
            //    if (this.lvwUserList.SelectedRows[0].Cells["PositionLED_ID"].Value.ToString() == "")
            //    {
            //        MessageBox.Show("请选择要修改的项！");
            //        return;
            //    }
            //    else
            //    {
            //        btnAdd.Enabled = false;
            //        btnUpdate.Enabled = true;
            //        isUpdate = 0;
            //        string sql = "select * from PositionLED where PositionLED_ID='" + this.lvwUserList.SelectedRows[0].Cells["PositionLED_ID"].Value.ToString() + "'";
            //        PositionLED pLED = PositionLEDDAL.GetLED(sql);
            //        this.cbotiaoshu.Text = pLED.PositionLED_Count.ToString();
            //        this.txtPositionLED_IntervalX.Text = pLED.PositionLED_IntervalX.ToString();
            //        this.txtPositionLED_IntervalY.Text = pLED.PositionLED_IntervalY.ToString();
            //        this.txtPositionLED_Remark.Text = pLED.PositionLED_Remark.ToString();
            //        this.txtPositionLED_ScreenHeight.Text = pLED.PositionLED_ScreenHeight.ToString();
            //        this.txtPositionLED_ScreenWeight.Text = pLED.PositionLED_ScreenWeight.ToString();
            //        if (pLED.PositionLED_Type == 1)
            //        {
            //            this.cboleixing.SelectedIndex = 0;
            //        }
            //        else if (pLED.PositionLED_Type == 2)
            //        {
            //            this.cboleixing.SelectedIndex = 1;
            //        }
            //        this.txtPositionLED_X.Text = pLED.PositionLED_X.ToString();
            //        this.txtPositionLED_Y.Text = pLED.PositionLED_Y.ToString();
            //        //chkboxLEDPassState.Text = pLED.PositionLED_PassageState.ToString();
            //        chkboxLEDState.Text = pLED.PositionLED_State.ToString();
            //        //chkPositionLED_Position_Id.ValueMember = pLED.PositionLED_ID.ToString();
            //        chkPositionLED_Position_Id.SelectedValue = (int)pLED.PositionLED_Position_ID;
            //        chkCarNumber.Checked = true;
            //        chkCarType.Checked = true;
            //        chkSerialnumber.Checked = true;
            //    }
            #endregion

            if (lvwUserList.SelectedRows.Count > 0)
            {
                try
                {
                    btnAdd.Enabled = false;
                    btnUpdate.Enabled = true;
                    this.cbotiaoshu.SelectedIndex = Convert.ToInt32(lvwUserList.SelectedRows[0].Cells[4].Value) - 1;
                    this.txtPositionLED_Remark.Text = lvwUserList.SelectedRows[0].Cells[14].Value.ToString();
                    this.txtPositionLED_ScreenHeight.Text = lvwUserList.SelectedRows[0].Cells[6].Value.ToString();
                    this.txtPositionLED_ScreenWeight.Text = lvwUserList.SelectedRows[0].Cells[5].Value.ToString();
                    if (Convert.ToInt32(lvwUserList.SelectedRows[0].Cells[15].Value) == 1)
                    {
                        this.cboleixing.SelectedIndex = 0;
                    }
                    else if (Convert.ToInt32(lvwUserList.SelectedRows[0].Cells[15].Value) == 2)
                    {
                        this.cboleixing.SelectedIndex = 1;
                    }
                    this.txtPositionLED_X.Text = lvwUserList.SelectedRows[0].Cells[7].Value.ToString();
                    this.txtPositionLED_Y.Text = lvwUserList.SelectedRows[0].Cells[8].Value.ToString();
                    chkboxLEDState.Text = lvwUserList.SelectedRows[0].Cells[2].Value.ToString();
                    chkPositionLED_Position_Id.SelectedIndex = Convert.ToInt32(lvwUserList.SelectedRows[0].Cells[1].Value) - 1;

                    chkCarNumber.Checked = true;
                    chkCarType.Checked = true;
                    chkSerialnumber.Checked = true;
                }
                catch 
                {
                    
                }
            }
            else
            {
                MessageBox.Show("只能选中一行", "提示");
            }
        }

        private void lvwUserList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        //更改显示内容时隐藏控件
        private void cboleixing_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cboleixing.SelectedIndex == 0)
            {
                label2.Visible = true;
                chkSerialnumber.Visible = true;
                chkCarNumber.Visible = true;
                chkCarType.Visible = true;
                lblMore.Visible = true;
                label1.Visible = true;
                cbotiaoshu.Visible = true;
                label11.Visible = false;
                txtPositionLED_Remark.Visible = false;
            }
            else if (cboleixing.SelectedIndex == 1)
            {
                label2.Visible = false;
                chkSerialnumber.Visible = false;
                chkCarNumber.Visible = false;
                chkCarType.Visible = false;
                lblMore.Visible = false;
                label1.Visible = false;
                cbotiaoshu.Visible = false;
                label11.Visible = true;
                txtPositionLED_Remark.Visible = true;
            }
        }
    }
}
