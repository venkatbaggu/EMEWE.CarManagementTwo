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
    public partial class carInOutSatistics : Form
    {
        public PageControl pc = new PageControl();
        public string sqlwhere = " 1=1 ";
        public string strClickedItemName = "";
        public carInOutSatistics()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 是否导出所有
        /// </summary>
        private bool ISExecl = false;

        private bool isload = false;
        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {
                tsbtnDel.Enabled = true;
                tsbtnDel.Visible = true;
                tslbExecl.Enabled = true;
                tslbExecl.Visible = true;
            }
            else
            {
                tsbtnDel.Visible = ControlAttributes.BoolControl("tsbtnDel", "carInOutSatistics", "Visible");
                tsbtnDel.Enabled = ControlAttributes.BoolControl("tsbtnDel", "carInOutSatistics", "Enabled");
                tslbExecl.Visible = ControlAttributes.BoolControl("tslbExecl", "carInOutSatistics", "Visible");
                tslbExecl.Enabled = ControlAttributes.BoolControl("tslbExecl", "carInOutSatistics", "Enabled");
            }
        }
        private void carInOutSatistics_Load(object sender, EventArgs e)
        {
            userContext();
            LoadCarType();
            LoadPosition();
            groupBox2.Visible = false;
            isload = true;
            txtBeginTime.Text = CommonalityEntity.GetServersTime().ToString();
            txtEndTime.Text = CommonalityEntity.GetServersTime().AddDays(1).ToString();
            tscbxPageSize.SelectedIndex = 3;
            isload = false;
        }

        /// <summary>
        /// 绑定车辆类型 车辆类型状态为启动
        /// </summary>
        public void LoadPosition()
        {
            try
            {
                string sql = "Select * from Position where Position_State='启动' ";
                DataTable dt = LinQBaseDao.Query(sql).Tables[0];
                DataRow dr = dt.NewRow();
                dr["Position_ID"] = "0";
                dr["Position_Name"] = "";
                dt.Rows.InsertAt(dr, 0);
                cbxPosion.DataSource = dt;
                cbxPosion.ValueMember = "Position_ID";
                cbxPosion.DisplayMember = "Position_Name";
            }
            catch
            {
                CommonalityEntity.WriteTextLog("carInOutSatistics LoadPosition()");
            }
        }

        /// <summary>
        /// 绑定车辆类型 车辆类型状态为启动
        /// </summary>
        public void LoadCarType()
        {
            try
            {
                string sql = "Select * from CarType where CarType_State='启动' ";
                DataTable dt = LinQBaseDao.Query(sql).Tables[0];
                DataRow dr = dt.NewRow();
                dr["CarType_ID"] = "0";
                dr["CarType_Name"] = "";
                dt.Rows.InsertAt(dr, 0);
                cbxCarType.DataSource = dt;
                cbxCarType.ValueMember = "CarType_ID";
                cbxCarType.DisplayMember = "CarType_Name";
            }
            catch
            {
                CommonalityEntity.WriteTextLog("carInOutSatistics LoadCarType()");
            }
        }
        #region 分页和加载DataGridView

        /// <summary>
        ///设置分页控件每页显示的最大条数事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tscbxPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                pc = new PageControl();
                pc.pageSize = CommonalityEntity.GetInt(tscbxPageSize.SelectedItem.ToString());
                if (isload)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(SelectMethod), null);
                }
                else
                {
                    SelectMethod(null);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("carInOutSatistics.tscbxPageSize_SelectedIndexChanged()");
            }
        }
        /// <summary>
        /// 分页控件响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {

                if (e.ClickedItem.Name == "tslSelectAll")//全选
                {
                    ISExecl = true;
                    SelectAllMethod(true);
                    return;
                }
                if (e.ClickedItem.Name == "tslNotSelect")//取消全选
                {
                    ISExecl = false;
                    SelectAllMethod(false);
                    return;
                }
                if (e.ClickedItem.Name == "tsbtnDel")//删除
                {
                    tsbtnDel_Click();
                    BingMethod();
                    return;
                }
                if (e.ClickedItem.Name == "tslbExecl")//导出
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(tslbExecl_Click), null);
                    return;
                }
                else
                {
                    strClickedItemName = e.ClickedItem.Name.ToString();
                    BingMethod();
                }


            }
            catch
            {
                CommonalityEntity.WriteTextLog("carInOutSatistics.bdnInfo_ItemClicked()");
            }
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
                int j = 0;
                if (lvwUserList.SelectedRows.Count > 0)//选中删除
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //选中数量
                        int count = lvwUserList.SelectedRows.Count;
                        //遍历
                        for (int i = 0; i < count; i++)
                        {
                            int CarInOutInfoRecord_ID = int.Parse(this.lvwUserList.SelectedRows[i].Cells["CarInOutInfoRecord_ID"].Value.ToString());

                            Expression<Func<CarInOutInfoRecord, bool>> cirFN = n => n.CarInOutInfoRecord_ID == CarInOutInfoRecord_ID;
                            if (CarInOutInfoRecordDAL.DeleteToMany(cirFN))
                            {
                                j++;
                                CommonalityEntity.WriteLogData("删除", "删除通行详细记录编号为：" + CarInOutInfoRecord_ID + " 的通行详细记录", CommonalityEntity.USERNAME);//添加操作日志
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

                CommonalityEntity.WriteTextLog("车辆进出厂统计 tsbtnDel_Click()+");
            }
            finally
            {
                BingMethod();
            }
        }
        /// <summary>
        /// 导出Execl
        /// </summary>
        private void tslbExecl_Click(object obj)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            string fileName = "车辆进出厂统计 Excel报表-" + CommonalityEntity.GetServersTime().ToShortDateString();

            if (!ISExecl)
            {
                tslExport_Excel(fileName, lvwUserList);
            }
            else
            {
                //SelectMethod(null);
                string strsql = "select CarInOutInfoRecord_ID as 编号 ,Position_Name as 通行门岗,Driveway_Name as 通道名称,Driveway_Type as 通道类型,CarType_Name as	车辆类型,CarInfo_Name as 车牌号,SmallTicket_Serialnumber as 小票号,StaffInfo_Name as 姓名,CustomerInfo_Name as 公司名称,CarInOutInfoRecord_Remark as 通行通道 ,CarInOutInfoRecord_ICType as 放行IC卡类型,CarInOutInfoRecord_ICValue as 放行IC卡号,CarInOutInfoRecord_UserName as 刷卡放行人,CarInOutInfoRecord_Abnormal as 是否异常,CarInOutInfoRecord_Time as 记录时间,CarInOutRecord_Time as 登记时间 from View_carInOutSatistics where " + sqlwhere + " order by CarInOutInfoRecord_ID";
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

                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                if (xlApp == null)
                {
                    MessageBox.Show("无法创建Excel对象，可能您的机子未安装Excel");
                    return;
                }

                ISdao = true;
                groupBox2.Visible = true;
                btnSet.Text = "取消导出";
                progressBar1.Maximum = myDGV.SelectedRows.Count;
                progressBar1.Value = 0;
                label18.Text = "正在导出：" + fileName;
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
                Microsoft.Office.Interop.Excel.Range rang = worksheet.get_Range(worksheet.Cells[2, 1], worksheet.Cells[myDGV.Rows.Count + 2, 2]);
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
                        MessageBox.Show("导出文件时出错,文件可能正被打开！\n");
                    }

                }
                xlApp.Quit();
                GC.Collect();//强行销毁 
                if (progressBar1.Value == myDGV.SelectedRows.Count)
                {
                    label18.Text = fileName;
                    btnSet.Text = "导入完成";
                }
            }
            catch (System.Exception ex)
            {

            }
        }
        /// <summary>
        /// 全选\取消全选
        /// </summary>
        /// <param name="rbool">true:全选 false:取消全选</param>
        private void SelectAllMethod(bool rbool)
        {
            try
            {
                for (int i = 0; i < lvwUserList.Rows.Count; i++)
                {

                    lvwUserList.Rows[i].Selected = rbool;
                    lvwUserList.Rows[i].Cells[0].Value = rbool;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("carInOutSatistics.SelectAllMethod()");
            }

        }
        /// <summary>
        /// 显示列表绑定数据
        /// </summary>
        private void BingMethod()
        {
            try
            {
                pc.BindBoundControl(lvwUserList, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_carInOutSatistics", "*", "CarInOutInfoRecord_ID", "CarInOutInfoRecord_ID", 0, sqlwhere, true);

            }
            catch
            {
                CommonalityEntity.WriteTextLog("carInOutSatistics.BingMethod()");
            }
        }
        /// <summary>
        /// 条件搜索方法
        /// </summary>
        private void SelectMethod(object obj)
        {
            try
            {
                Control.CheckForIllegalCrossThreadCalls = false;
                sqlwhere = "  1=1";

                //车辆类型
                if (cbxCarType.Text.Trim() != "")
                {
                    sqlwhere += " and CarType_ID =" + cbxCarType.SelectedValue;
                }
                //进出类型
                if (cbxInOutType.Text.Trim() != "")
                {
                    sqlwhere += " and Driveway_Type ='" + cbxInOutType.Text + "'";
                }
                //门岗
                if (cbxPosion.Text.Trim() != "")
                {
                    sqlwhere += " and Position_ID=" + cbxPosion.SelectedValue;
                }

                string BeginTime = Convert.ToDateTime(txtBeginTime.Value).ToString("yyyy-MM-dd");
                string EndTime = Convert.ToDateTime(txtEndTime.Value).ToString("yyyy-MM-dd");
                if (BeginTime != EndTime)
                {
                    sqlwhere += " and CarInOutInfoRecord_Time between '" + BeginTime
+ "' and '" + EndTime + "'";
                }

                if (txtStaffName.Text.Trim() != "")
                {
                    sqlwhere += " and StaffInfo_Name like '%" + txtStaffName.Text.Trim() + "%'";

                }
                if (txtCustomerInfo.Text.Trim() != "")
                {
                    sqlwhere += " and CustomerInfo_Name like  '%" + txtCustomerInfo.Text.Trim() + "%'";

                }
                if (txtCarName.Text != "")
                {
                    sqlwhere += " and CarInfo_Name like '%" + txtCarName.Text.Trim() + "%' ";

                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("carInOutSatistics.SelectMethod()");
            }
            finally
            {
                BingMethod();//绑定数据
            }

        }

        #endregion


        /// <summary>
        /// 搜索单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            SelectMethod(null);
        }

        private void txtBeginTime_ValueChanged(object sender, EventArgs e)
        {
            this.txtBeginTime.Format = DateTimePickerFormat.Custom;
            this.txtBeginTime.CustomFormat = "yyyy-MM-dd 00:00:00";
        }

        private void txtEndTime_ValueChanged(object sender, EventArgs e)
        {
            this.txtEndTime.Format = DateTimePickerFormat.Custom;
            this.txtEndTime.CustomFormat = "yyyy-MM-dd 00:00:00";
        }

        private void lvwUserList_DoubleClick(object sender, EventArgs e)
        {
            if (lvwUserList.SelectedRows.Count == 1)
            {
                CommonalityEntity.CarInOutInfoRecord_ID = Convert.ToInt32(this.lvwUserList.SelectedRows[0].Cells["CarInOutInfoRecord_ID"].Value.ToString());
                CarInfoInfoRecordPic cp = new CarInfoInfoRecordPic();
                cp.Show();
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

        private void lvwUserList_Click(object sender, EventArgs e)
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
                groupBox2.Visible = true;
                btnSet.Text = "取消导出";

                label18.Text = "正在导出：" + filename;

                ExcelFile excelFile = new ExcelFile();
                // ExcelWorksheet sheet = excelFile.Worksheets.Add(filename);
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
                    label18.Text = filename;
                    btnSet.Text = "导出完成";
                }
            }
            catch { }
        }
    }
}
