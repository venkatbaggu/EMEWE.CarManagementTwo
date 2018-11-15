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
using System.Drawing.Printing;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class PrintSetForm : Form
    {
        public List<string> list = new List<string>();
        public bool isUpdate = false;
        public string where = "1=1 and printinfo.print_carType_id=cartype.carType_Id";
        public PrintSetForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //得到页面对应车辆类型的打印设置
                int carType_ID = int.Parse(comboxCartype.SelectedValue.ToString());
                //得到选择的打印内容
                string print_Content = "";
                if (chkCarNumber.Checked)
                {
                    print_Content += chkCarNumber.Text.Trim() + ",";
                }
                if (chkCarType.Checked)
                {
                    print_Content += chkCarType.Text.Trim() + ",";
                }
                if (chkStaff_Name.Checked)
                {
                    print_Content += chkStaff_Name.Text.Trim() + ",";
                }
                if (chkList.CheckedItems.Count > 0)
                {
                    foreach (var item in chkList.CheckedItems)
                    {
                        print_Content += item + ",";
                    }
                }
                PrintInfo print = new PrintInfo();
                print.Print_Content = print_Content.Substring(0, print_Content.Length - 1);
                print.Print_CarType_ID = carType_ID;
                print.Print_State = chkPrint_State.Text.Trim();
                print.Print_Attention = txtzhuyishixiang.Text.Trim();
                print.Print_Prompt = txttisi.Text.Trim();
                if (chkPrint_State.Text.Trim() == "启动")
                {
                    if (ChkPrintState())
                    {
                        DialogResult dlgResult = MessageBox.Show("已经存在启动状态的设置，是否替换?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dlgResult == DialogResult.OK)
                        {
                            //修改条件
                            Expression<Func<PrintInfo, bool>> funs = n => n.Print_State == "启动" && n.Print_CarType_ID == int.Parse(comboxCartype.SelectedValue.ToString());
                            //需要修改的内容
                            Action<PrintInfo> actions = p =>
                            {
                                p.Print_State = "暂停";
                            };
                            //执行更新
                            PrintInfoDAL.UpdatePrint(funs, actions);
                            PrintInfoDAL.InsertPrint(print);
                        }
                        else
                        {
                            print.Print_State = "暂停";
                            PrintInfoDAL.InsertPrint(print);
                        }
                    }
                    else
                    {
                        PrintInfoDAL.InsertPrint(print);
                    }
                }
                else
                {
                    PrintInfoDAL.InsertPrint(print);
                }
                DataTable dt = LinQBaseDao.Query("select PrintInfo_ID,PrintInfo_State from PrintInfo order by PrintInfo_ID desc ").Tables[0];
                string id = dt.Rows[0][0].ToString();
                string state = dt.Rows[0][1].ToString();
                if (state == "启动")
                    CommonalityEntity.WriteLogData("新增", "新增并启动编号为：" + id + "打印设置", CommonalityEntity.USERNAME);
                else
                    CommonalityEntity.WriteLogData("新增", "新增编号为：" + id + "打印设置", CommonalityEntity.USERNAME);
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("PrintSetForm btnAdd_Click()" );
            }
            finally
            {
                GetGriddataviewLoad("");
            }
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
            }
            else
            {
                btnAdd.Visible = ControlAttributes.BoolControl("btnAdd", "PrintSetForm", "Visible");
                btnAdd.Enabled = ControlAttributes.BoolControl("btnAdd", "PrintSetForm", "Enabled");

                btnDelete.Visible = ControlAttributes.BoolControl("btnDelete", "PrintSetForm", "Visible");
                btnDelete.Enabled = ControlAttributes.BoolControl("btnDelete", "PrintSetForm", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "PrintSetForm", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "PrintSetForm", "Enabled");

                btnLEDApplication.Visible = ControlAttributes.BoolControl("btnLEDApplication", "PrintSetForm", "Visible");
                btnLEDApplication.Enabled = ControlAttributes.BoolControl("btnLEDApplication", "PrintSetForm", "Enabled");

                btnLEDShow.Visible = ControlAttributes.BoolControl("btnLEDShow", "PrintSetForm", "Visible");
                btnLEDShow.Enabled = ControlAttributes.BoolControl("btnLEDShow", "PrintSetForm", "Enabled");
            }
        }

        /// <summary>
        /// 绑定车辆类型
        /// </summary>
        public void CarTypeLoad()
        {
            string sql = "Select CarType_ID,CarType_Name from CarType where CarType_State='启动'";

            DataTable dt = LinQBaseDao.Query(sql).Tables[0];

            if (dt.Rows.Count > 0)
            {
                comboxCartype.DataSource = dt;
                comboxCartype.ValueMember = "CarType_ID";
                comboxCartype.DisplayMember = "CarType_Name";

                DataTable typeds = LinQBaseDao.Query(sql).Tables[0];
                DataTable table = typeds;
                DataRow row = table.NewRow();
                row["CarType_ID"] = "0";
                row["CarType_Name"] = "全部";
                table.Rows.InsertAt(row, 0);
                chkPrint_CarType.DataSource = table;
                chkPrint_CarType.ValueMember = "CarType_ID";
                chkPrint_CarType.DisplayMember = "CarType_Name";
                chkPrint_CarType.SelectedIndex = 0;
            }

        }
        /// <summary>
        /// 确定选择打印内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            foreach (var item in chkList.CheckedItems)
            {
                list.Add(item.ToString());
            }
        }
        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintSetForm_Load(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            userContext();
            CarTypeLoad();
            Print_StateLoad();
            GetGriddataviewLoad("");
            btnUpdate.Enabled = false;
        }
        /// <summary>
        /// 选择更多打印项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lblMore_Click(object sender, EventArgs e)
        {
            chkList.Items.Clear();
            groupBox1.Visible = true;
            //获取所有的字段信息。
            List<string> lists = PositionLEDDAL.GetLEDShow("GetLEDView", "View_LEDShow_zj");
            foreach (var item in lists)
            {
                //去掉重复的字段
                if (item == "车牌号" || item == "车辆类型" || item == "小票号")
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
        /// <summary>
        /// 绑定状态
        /// </summary>
        public void Print_StateLoad()
        {
            chkPrint_State.DataSource = DictionaryDAL.GetValueStateDictionary("01").Where(n => n.Dictionary_Name != "全部").Select(n => n).ToList();

            chkPrint_State.ValueMember = "Dictionary_Value";
            chkPrint_State.DisplayMember = "Dictionary_Name";

            chkboxPrint_State.DataSource = DictionaryDAL.GetValueStateDictionary("01");
            chkboxPrint_State.ValueMember = "Dictionary_Value";
            chkboxPrint_State.DisplayMember = "Dictionary_Name";
            chkboxPrint_State.SelectedIndex = chkboxPrint_State.Items.Count - 1;
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                //得到页面对应车辆类型的打印设置
                int carType_ID = int.Parse(comboxCartype.SelectedValue.ToString());
                //得到选择的打印内容
                string print_Content = "";
                if (chkStaff_Name.Checked)
                {
                    print_Content += chkStaff_Name.Text.Trim() + ",";
                }
                if (chkCarNumber.Checked)
                {
                    print_Content += chkCarNumber.Text.Trim() + ",";
                }
                if (chkCarType.Checked)
                {
                    print_Content += chkCarType.Text.Trim() + ",";
                }
                if (chkList.CheckedItems.Count > 0)
                {
                    foreach (var item in chkList.CheckedItems)
                    {
                        print_Content += item + ",";
                    }
                }
                if (chkPrint_State.Text.Trim() == "启动")
                {
                    if (ChkPrintState())
                    {
                        DialogResult dlgResult = MessageBox.Show("已经存在启动状态的设置，是否替换?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dlgResult == DialogResult.OK)
                        {
                            //修改条件
                            Expression<Func<PrintInfo, bool>> funs = n => n.Print_State == "启动" && n.Print_CarType_ID == int.Parse(comboxCartype.SelectedValue.ToString());
                            //需要修改的内容
                            Action<PrintInfo> actions = p =>
                            {
                                p.Print_State = "暂停";
                            };
                            //执行更新
                            PrintInfoDAL.UpdatePrint(funs, actions);
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
                Expression<Func<PrintInfo, bool>> fun = p => p.Print_ID == int.Parse(this.lvwUserList.SelectedRows[0].Cells["Print_ID"].Value.ToString());
                Action<PrintInfo> action = print =>
                {
                    strfront = print.Print_Content + "," + print.Print_State + "," + print.Print_CarType_ID;
                    print.Print_Content = print_Content.Substring(0, print_Content.Length - 1);
                    print.Print_State = chkPrint_State.Text.Trim();
                    print.Print_CarType_ID = carType_ID;
                    print.Print_State = chkPrint_State.Text.Trim();
                    print.Print_Attention = txtzhuyishixiang.Text.Trim();
                    print.Print_Prompt = txttisi.Text.Trim();
                    strcontent = print.Print_Content + "," + print.Print_State + "," + print.Print_CarType_ID;
                    id = print.Print_ID.ToString();
                };
                PrintInfoDAL.UpdatePrint(fun, action);
                CommonalityEntity.WriteLogData("修改", "更新打印编号为：" + id + "的打印设置；修改前：" + strfront + "；修改后:" + strcontent, CommonalityEntity.USERNAME);

            }
            catch 
            {
                CommonalityEntity.WriteTextLog("PrintSetForm btnUpdate_Click()");//记录异常日志
            }
            finally
            {
                GetGriddataviewLoad("");
                btnAdd.Enabled = true;
                btnUpdate.Enabled = false;
            }
        }

        #region 分页和加载DataGridView
        /// <summary>
        /// 加载信息
        /// </summary>
        public void GetGriddataviewLoad(string strClickedItemName)
        {
            Page.BindBoundControl(lvwUserList, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "PrintInfo,carType", "*", "Print_ID", "Print_id", 0, where, true);
            where = "1=1 and printinfo.print_carType_id=cartype.carType_Id";
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
        /// 取消修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCel_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            list.Clear();
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (chkPrint_CarType.Text.Trim() != "" && chkPrint_CarType.Text != "全部")
            {
                where += " and Print_CarType_ID=" + chkPrint_CarType.SelectedValue + "";
            }
            else
            {
                where += " and 1=1";
            }
            if (chkPrint_State.Text.Trim() != "" && chkboxPrint_State.Text != "全部")
            {
                where += " and Print_State='" + chkboxPrint_State.Text.ToString() + "'";
            }
            else
            {
                where += " and 1=1";
            }
            GetGriddataviewLoad("");
        }
        /// <summary>
        /// 取消修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            chkPrint_State.SelectedIndex = -1;
            comboxCartype.SelectedIndex = -1;
            chkCarNumber.Checked = false;
            chkCarType.Checked = false;
            chkStaff_Name.Checked = false;
            isUpdate = false;
            //chkList.Items.Clear();
        }
        /// <summary>
        /// 应用打印设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLEDApplication_Click(object sender, EventArgs e)
        {
            try
            {
                CommonalityEntity.IsCancellation = false;
                isUpdate = true;
                int id = int.Parse(this.lvwUserList.SelectedRows[0].Cells["Print_ID"].Value.ToString());
                if (ChkPrintState())
                {
                    DialogResult dlgResult = MessageBox.Show("已经存在启动状态的设置，是否替换?", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dlgResult == DialogResult.OK)
                    {
                        //修改条件
                        Expression<Func<PrintInfo, bool>> funs = n => n.Print_State == "启动" && n.Print_CarType_ID == int.Parse(this.lvwUserList.SelectedRows[0].Cells["Print_CarType_ID"].Value.ToString());
                        //需要修改的内容
                        Action<PrintInfo> actions = p =>
                        {
                            p.Print_State = "暂停";
                        };
                        //执行更新
                        PrintInfoDAL.UpdatePrint(funs, actions);

                        //应用当前选中的设置
                        //条件
                        Expression<Func<PrintInfo, bool>> fun = n => n.Print_ID == id;
                        //需要的内容
                        Action<PrintInfo> action = p =>
                        {
                            p.Print_State = "启动";
                        };
                        //执行更新
                        PrintInfoDAL.UpdatePrint(fun, action);
                        CommonalityEntity.WriteLogData("修改", "启动打印编号为：" + id + "的打印设置", CommonalityEntity.USERNAME);
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
                    Expression<Func<PrintInfo, bool>> fun = n => n.Print_ID == id;
                    //需要的内容
                    Action<PrintInfo> action = p =>
                    {
                        p.Print_State = "启动";
                    };
                    //执行更新
                    PrintInfoDAL.UpdatePrint(fun, action);
                    CommonalityEntity.WriteLogData("修改", "启动打印编号为：" + id + "的打印设置", CommonalityEntity.USERNAME);
                }

            }
            catch 
            {
                CommonalityEntity.WriteTextLog("LEDSerForm btnLEDApplication_Click()");
            }
            finally
            {
                GetGriddataviewLoad("");//加载
            }
        }
        /// <summary>
        /// 验证LED设置状态是否重复
        /// </summary>
        /// <returns></returns>
        public bool ChkPrintState()
        {
            bool chkState = false;
            try
            {
                string sql = "";
                if (isUpdate)
                {
                    sql = "Select * from PrintInfo where print_State='启动' and print_ID!=" + int.Parse(this.lvwUserList.SelectedRows[0].Cells["print_id"].Value.ToString()) + "  and print_carType_ID = " + int.Parse(this.lvwUserList.SelectedRows[0].Cells["Print_CarType_Id"].Value.ToString()) + "";
                }
                else
                {
                    sql = "Select * from PrintInfo where print_State='启动'  and print_carType_ID = " + comboxCartype.SelectedValue + "";
                }
                chkState = PrintInfoDAL.ChkPrintState(sql);
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("PrintSetForm ChkPositionLEDState()");
            }
            return chkState;
        }
        /// <summary>
        /// 打印预览及打印模版
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLEDShow_Click(object sender, EventArgs e)
        {
            //获取选择的打印设置行，得到选择的打印的预览信息
            try
            {
                if (this.lvwUserList.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("请选择要预览的设置！");
                    return;
                }
                if (this.lvwUserList.SelectedRows[0].Cells["Print_ID"].Value.ToString() == "")
                {
                    MessageBox.Show("请选择要预览的设置！");
                    return;
                }
                else
                {
                    CommonalityEntity.Car_Type_ID = this.lvwUserList.SelectedRows[0].Cells["Print_CarType_Id"].Value.ToString();
                    string sql = "select * from PrintInfo where Print_ID='" + this.lvwUserList.SelectedRows[0].Cells["Print_ID"].Value.ToString() + "'";
                    PrintInfo pInfo = PrintInfoDAL.GetPrint(sql);
                    DataSet dataset = new DataSet();
                    string sqlContent = "Select top 1 ";
                    if (pInfo.Print_Content != "")
                    {
                        string[] str = pInfo.Print_Content.Split(',');
                        foreach (var item in str)
                        {
                            sqlContent += item + ",";
                        }
                        sqlContent = sqlContent.Substring(0, sqlContent.Length - 1);
                    }

                    string strsqlContent = sqlContent + " from View_LEDShow_zj where CarType_ID=" + CommonalityEntity.Car_Type_ID;
                    dataset = LinQBaseDao.Query(strsqlContent);
                    if (dataset.Tables[0].Rows.Count > 0)
                    {
                        CommonalityEntity.Serialnumber = dataset.Tables[0].Rows[0]["小票号"].ToString();
                        PrintInfoForm pi = new PrintInfoForm(dataset);
                        pi.Show();
                    }
                    else
                    {
                        strsqlContent = sqlContent + " from View_LEDShow_zj";
                        dataset = LinQBaseDao.Query(strsqlContent);
                        CommonalityEntity.Serialnumber = "130808000001";
                        PrintInfoForm pi = new PrintInfoForm(dataset);
                        pi.Show();
                    }
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("PrintSetForm btnLEDShow_Click()");//记录异常日志
            }
        }
        /// <summary>
        /// 删除选中设置  启动状态不能删除
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
                if (this.lvwUserList.SelectedRows[0].Cells["Print_ID"].Value.ToString() == "")
                {
                    MessageBox.Show("请选择要删除的项！");
                    return;
                }
                if (this.lvwUserList.SelectedRows[0].Cells["Print_State"].Value.ToString() == "启动")
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
                int id = int.Parse(this.lvwUserList.SelectedRows[0].Cells["Print_ID"].Value.ToString());
                Expression<Func<PrintInfo, bool>> fun = n => n.Print_ID == id && n.Print_State != "启动";
                PrintInfoDAL.DeletePrintInfo(fun);
                CommonalityEntity.WriteLogData("删除", "删除编号为" + id + "的打印设置", CommonalityEntity.USERNAME);//添加操作日志

            }
            catch 
            {
                CommonalityEntity.WriteTextLog("PrintSetForm btnDelete_Click()" );
            }
            finally
            {
                GetGriddataviewLoad("");//加载
            }
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
                if (this.lvwUserList.SelectedRows[0].Cells["Print_ID"].Value.ToString() == "")
                {
                    MessageBox.Show("请选择要修改的项！");
                    return;
                }
                else
                {
                    string sql = "select * from PrintInfo where Print_ID='" + this.lvwUserList.SelectedRows[0].Cells["Print_ID"].Value.ToString() + "'";
                    PrintInfo pInfo = PrintInfoDAL.GetPrint(sql);
                    comboxCartype.SelectedValue = pInfo.Print_CarType_ID;
                    chkPrint_State.Text = pInfo.Print_State;

                    //////////////////////////
                    txttisi.Text = lvwUserList.SelectedRows[0].Cells["Print_Attention"].Value.ToString();
                    txtzhuyishixiang.Text = lvwUserList.SelectedRows[0].Cells["Print_Prompt"].Value.ToString();

                    string[] content = pInfo.Print_Content.Split(',');
                    foreach (var item in content)
                    {
                        if (item == chkCarNumber.Text)
                        {
                            chkCarNumber.Checked = true;
                        }
                        if (item == chkStaff_Name.Text)
                        {
                            chkStaff_Name.Checked = true;
                        }
                        if (item == chkCarType.Text)
                        {
                            chkCarType.Checked = true;
                        }
                    }
                    btnAdd.Enabled = false;
                    btnUpdate.Enabled = true;
                }

            }
            catch 
            {
                CommonalityEntity.WriteTextLog("PrintSetForm lvwUserList_RowHeaderMouseDoubleClick()" );//记录异常日志
            }
        }
    }
}
