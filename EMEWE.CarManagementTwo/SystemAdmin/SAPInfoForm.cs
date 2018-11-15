using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using GemBox.ExcelLite;
using System.Linq.Expressions;
using System.Threading;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class SAPInfoForm : Form
    {
        public SAPInfoForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 实例化分页控件
        /// </summary>
        PageControl page = new PageControl();

        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere = "1=1";

        /// <summary>
        /// 是否导出所有
        /// </summary>
        private bool ISExecl = false;

        /// <summary>
        /// 加载时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SAPInfoForm_Load(object sender, EventArgs e)
        {
            tscbxPageSize.SelectedIndex = 2;
            groupBox3.Visible = false;
            StetaBingMethod();
            //CarInfoBingMethod();
            Time();
            userContext();
            LogInfoLoad("");
        }
        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {
                tsbtnUpdate.Enabled = true;
                tsbtnUpdate.Visible = true;
                tsbtnDel.Enabled = true;
                tsbtnDel.Visible = true;
                tslbExecl.Enabled = true;
                tslbExecl.Visible = true;
            }
            else
            {
                tsbtnUpdate.Visible = ControlAttributes.BoolControl("tsbtnUpdate", "SAPInfoForm", "Visible");
                tsbtnUpdate.Enabled = ControlAttributes.BoolControl("tsbtnUpdate", "SAPInfoForm", "Enabled");
                tsbtnDel.Visible = ControlAttributes.BoolControl("tsbtnDel", "SAPInfoForm", "Visible");
                tsbtnDel.Enabled = ControlAttributes.BoolControl("tsbtnDel", "SAPInfoForm", "Enabled");
                tslbExecl.Visible = ControlAttributes.BoolControl("tslbExecl", "SAPInfoForm", "Visible");
                tslbExecl.Enabled = ControlAttributes.BoolControl("tslbExecl", "SAPInfoForm", "Enabled");
            }
        }
        /// <summary>
        /// 设置默认起始时间
        /// </summary>
        private void Time()
        {
            dateTimePicker1.Value = CommonalityEntity.GetServersTime().AddMonths(-1);
        }
        /// <summary>
        /// 数据加载
        /// </summary>
        /// <param name="strClickedItemName"></param>
        private void LogInfoLoad(string strClickedItemName)
        {
            page.BindBoundControl(dgvSAPInfo, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "eh_SAPRecord", "*", "Sap_ID", "Sap_ID", 0, sqlwhere, true);
        }
        /// <summary>
        /// 状态绑定
        /// </summary>
        private void StetaBingMethod()
        {
            try
            {
                var p = DictionaryDAL.GetValueStateDictionary("01");
                int intcount = p.Count();
                var Pcob_DrivewayStrategy_State = p.Where(n => n.Dictionary_Name != "全部").ToList();
                if (Pcob_DrivewayStrategy_State != null && Pcob_DrivewayStrategy_State.Count() > 0)
                {
                    cobSAPInfo_State.DataSource = Pcob_DrivewayStrategy_State;
                    this.cobSAPInfo_State.DisplayMember = "Dictionary_Name";
                    cobSAPInfo_State.ValueMember = "Dictionary_ID";
                    cobSAPInfo_State.SelectedIndex = -1;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyForm.StetaBingMethod()");
            }

        }
        /// <summary>
        /// 车辆类型绑定
        /// </summary>
        private void CarInfoBingMethod()
        {
            try
            {
                //查询所有的车辆类型绑定到comboxcar_type
                string sql = "Select * from CarType";
                DataTable tab = LinQBaseDao.Query(sql).Tables[0];
                DataRow row = tab.NewRow();
                row["CarType_Name"] = "全部";
                row["CarType_ID"] = 0;
                tab.Rows.InsertAt(row, 0);
                cobSAPCarInfo.DataSource = tab;
                cobSAPCarInfo.ValueMember = "CarType_ID";
                cobSAPCarInfo.DisplayMember = "CarType_Name";
                this.cobSAPCarInfo.SelectedIndex = 0;
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarInfoManager comboxCar_Type_DropDown:");
            }
        }

        #region 分页
        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslNotChecked()
        {
            for (int i = 0; i < this.dgvSAPInfo.Rows.Count; i++)
            {
                dgvSAPInfo.Rows[i].Selected = false;
                this.dgvSAPInfo.Rows[i].Cells[0].Value = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslAllChecked()
        {
            for (int i = 0; i < dgvSAPInfo.Rows.Count; i++)
            {
                this.dgvSAPInfo.Rows[i].Selected = true;
            }
        }
        /// <summary>
        /// 分页控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bindingNavigator1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
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
            if (e.ClickedItem.Name == "tsbtnUpdate")//修改
            {
               // tsbtnUpdate_Click(null, null);
                return;
            }
            if (e.ClickedItem.Name == "tsbtnDel")//删除
            {
                tsbtnDel_Click();
                return;
            }
            if (e.ClickedItem.Name == "tslbExecl")//到出
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(tslbExecl_Click), null);
                return;
            }
            LogInfoLoad(e.ClickedItem.Name);
        }

        #endregion
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
        /// 删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtnDel_Click()
        {
            try
            {
                //删除
                if (dgvSAPInfo.SelectedRows.Count > 0)//单击只能选择一行
                {
                    if (MessageBox.Show("确定要删除选中数据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        bool bo = false;
                        for (int i = 0; i < dgvSAPInfo.SelectedRows.Count; i++)
                        {
                            string Sap_ID = dgvSAPInfo.SelectedRows[i].Cells["Sap_ID"].Value.ToString();
                            Expression<Func<eh_SAPRecord, bool>> funuserinfo = n => n.Sap_ID == Convert.ToInt32(Sap_ID);
                            if (eh_SAPRecordDAL.DeleteToMany(funuserinfo))
                            {
                                bo = true;
                                CommonalityEntity.WriteLogData("删除", "删除了编号为" + Sap_ID + "的SAP效验", CommonalityEntity.USERNAME);//添加操作日志
                            }
                        }
                        if (bo)
                        {
                            MessageBox.Show("删除成功");
                            LogInfoLoad("");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请选择数据进行删除");
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("SAPInfoForm.tsbtnDel_Click()");
            }
        }
        private void tslbExecl_Click(object obj)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            string fileName = "SAP效验Excel报表-" + CommonalityEntity.GetServersTime().ToShortDateString();

            if (!ISExecl)
            {
                tslExport_Excel(fileName, dgvSAPInfo);
            }
            else
            {
                btnSerch_Click(null, null);
                string strsql = "select Sap_Type as 车辆类型,Sap_InNO as 单号,Sap_InCRFLG as 出入标识,Sap_InTime as 操作时间,Sap_InCarOperate as 车辆放入人,Sap_InCarNumber as 车牌号,Sap_OutNAME1P as 供应商名称,Sap_OutMAKTX as 物料描述,Sap_OutKDATB as 有效起始日期,Sap_OutKDATE as 有效截止日期,Sap_OutOFLAG as 是否开装货通知单,Sap_OutTELNUMBER as 电话,Sap_OutHG as 货高,Sap_OutXZ as 限重,Sap_OutNAME1C as 客户名称,Sap_OutETYPE as 消息类型,Sap_OutEMSG as 消息,Sap_Remark as 备注,Sap_Identify as 标识,Sap_State as 状态,Sap_Serialnumber as 凭证编号  from eh_SAPRecord where " + sqlwhere + " order by Sap_ID";
                daochu(fileName, strsql);
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
            groupBox3.Visible = true;
            btnSet.Text = "取消导出";
            progressBar1.Maximum = myDGV.SelectedRows.Count;
            progressBar1.Value = 0;
            label19.Text = "正在导出：" + fileName;

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
                label19.Text = fileName;
                btnSet.Text = "导出完成";
            }

        }

        /// <summary>
        /// 单机搜索按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSerch_Click(object sender, EventArgs e)
        {
            sqlwhere = "1=1";
            if (!string.IsNullOrEmpty(cobSAPCarInfo.Text.Trim()) && cobSAPCarInfo.Text.Trim() != "全部")//车辆类型
            {
                sqlwhere += " and Sap_Type='" + cobSAPCarInfo.Text.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(txtSap_InCarNumber.Text.Trim()))//车牌号
            {
                sqlwhere += " and Sap_InCarNumber like '%" + txtSap_InCarNumber.Text.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(txtPhone.Text.Trim()))//电话
            {
                sqlwhere += " and Sap_InNO like '%" + txtPhone.Text.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(cobSAPInfo_State.Text.Trim()))//启动状态
            {
                sqlwhere += " and Sap_State='" + cobSAPInfo_State.Text.Trim() + "'";
            }
            //查询时间 必加
            sqlwhere += "and Sap_InTime between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "'";
            LogInfoLoad("");
            string i = SystemClass.SaveFile;//服务器路径地址
        }
        /// <summary>
        /// 单击清空按钮
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCler_Click(object sender, EventArgs e)
        {
            Empty();
        }
        /// <summary>
        /// 清空
        /// </summary>
        private void Empty()
        {
            txtPhone.Text = "";
            txtSap_InCarNumber.Text = "";
            cobSAPCarInfo.SelectedIndex = -1;
            cobSAPInfo_State.SelectedIndex = -1;
        }

        private void tsbtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvSAPInfo.SelectedRows.Count > 0)//选中行
                {
                    if (dgvSAPInfo.SelectedRows.Count > 1)
                    {
                        MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        string sapid = this.dgvSAPInfo.SelectedRows[0].Cells["Sap_ID"].Value.ToString();
                        string sernum = this.dgvSAPInfo.SelectedRows[0].Cells["Sap_Serialnumber"].Value.ToString();
                        SAPRecordUpdate su = new SAPRecordUpdate(sapid, sernum);
                        su.Show();
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("SAPInfoForm tsbtnUpdate_Click()" + "".ToString());
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
            groupBox3.Visible = false;
        }

        private void dgvSAPInfo_Click(object sender, EventArgs e)
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
                groupBox3.Visible = true;
                btnSet.Text = "取消导出";

                label19.Text = "正在导出：" + filename;

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
                    ExcelWorksheet sheet = excelFile.Worksheets.Add(filename + s.ToString());
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
                    label19.Text = filename;
                    btnSet.Text = "导出完成";
                }
            }
            catch { }
        }
    }
}
