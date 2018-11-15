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
using GemBox.ExcelLite;
using System.Threading;
using EMEWE.CarManagement.CommonClass;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class CarState : Form
    {
        public CarState()
        {
            InitializeComponent();
        }
        public int UserIDSearch;
        private string sqlwhere = "1=1";
        PageControl page = new PageControl();
        DateTime minTime = DateTime.Parse("1900-01-01 00:00:00.000");
        /// <summary>
        /// 是否导出所有
        /// </summary>
        private bool ISExecl = false;
        private void CarState_Load(object sender, EventArgs e)
        {
            CommonalityEntity.isLoad = 1;
            BindCarType();
            BindCarAttribute();
            BindcmbBusType();
            txtbeginTime.Value = CommonalityEntity.GetServersTime().AddDays(-1);
            txtendTime.Value = CommonalityEntity.GetServersTime();
            inTimeStart.Value = minTime;
            inTimeEnd.Value = minTime;
            outTimeStart.Value = minTime;
            outTimeEnd.Value = minTime;
            groupBox3.Visible = false;
            binWhere();
            tscbxPageSize.SelectedIndex = 2;

        }

        #region 车辆类型
        private void BindCarType()
        {
            DataTable dt = LinQBaseDao.Query("select * from CarType").Tables[0];
            DataRow dr = dt.NewRow();
            dr["CarType_ID"] = "0";
            dr["CarType_Name"] = "全部";
            dt.Rows.InsertAt(dr, 0);
            comboxCarType.DataSource = dt;
            comboxCarType.DisplayMember = "CarType_Name";
            comboxCarType.ValueMember = "CarType_ID";
            comboxCarType.SelectedIndex = 0;
        }

        #endregion



        /// <summary>
        /// 车辆类型属性
        /// </summary>
        private void BindCarAttribute()
        {
            try
            {
                string sql = "select CarAttribute_ID,CarAttribute_Name from  CarAttribute where CarAttribute_HeightID=0 ";
                DataTable dt = LinQBaseDao.Query(sql).Tables[0];
                DataRow dr = dt.NewRow();
                dr["CarAttribute_ID"] = "0";
                dr["CarAttribute_Name"] = "全部";
                dt.Rows.InsertAt(dr, 0);
                cmbSX.DataSource = dt;
                this.cmbSX.DisplayMember = "CarAttribute_Name";
                this.cmbSX.ValueMember = "CarAttribute_ID";
                this.cmbSX.SelectedIndex = 0;
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarForm_BindCarAttribute异常：");
                return;
            }
        }
        /// <summary>
        /// 业务类别
        /// </summary>
        private void BindcmbBusType()
        {
            try
            {
                string sql = "select distinct CarType_BusType  from CarType where CarType_BusType!=''";
                DataTable dt = LinQBaseDao.Query(sql).Tables[0];
                DataRow dr = dt.NewRow();
                dr["CarType_BusType"] = "0";
                dr["CarType_BusType"] = "全部";
                dt.Rows.InsertAt(dr, 0);
                cmbType.DataSource = dt;
                this.cmbType.DisplayMember = "CarType_BusType";
                this.cmbType.ValueMember = "CarType_BusType";
                this.cmbType.SelectedIndex = 0;
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarForm_BindCarAttribute异常：");
                return;
            }
        }
        /// <summary>
        /// 加载信息
        /// </summary>
        public void LogInfoLoad(object strClickedItemName)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            ///View_CarState1创建临时查询表查询“carstatek”
           page.BindBoundControl(gridevewCarState, strClickedItemName.ToString(), tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_CarState1", "*", "CarInOutRecord_ID", "CarInOutRecord_ID", 1, sqlwhere, true);
           CommonalityEntity.isLoad = 0;
        }

        private void binWhere()
        {
            sqlwhere = "1=1";
            if (!string.IsNullOrEmpty(cmbSX.Text.Trim()) && cmbSX.Text != "全部")
            {
                sqlwhere += " and CarType_OtherProperty = '" + this.cmbSX.Text.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(comboxCarType.Text) && comboxCarType.Text != "全部")
            {
                sqlwhere += " and CarType_Name='" + comboxCarType.Text.ToString() + "'";
                if (!string.IsNullOrEmpty(cmbBusType.Text) && cmbBusType.Text != "全部")
                {
                    sqlwhere += " and BusinessType_Name=" + cmbBusType.Text;
                }
            }
            if (cmbType.SelectedIndex > 0)
            {
                sqlwhere += " and  CarType_BusType='" + cmbType.Text.ToString() + "'";
            }
            if (!string.IsNullOrEmpty(comboxCarState.Text) && comboxCarState.Text != "全部")
            {
                sqlwhere += " and  CarInfo_State='" + comboxCarState.Text.ToString() + "'";
            }
            if (!string.IsNullOrEmpty(comboBox4.Text) && comboBox4.Text != "全部")
            {
                sqlwhere += " and  SortNumberInfo_TongXing='" + comboBox4.Text.ToString() + "'";
            }
            if (!string.IsNullOrEmpty(txtStaName.Text.Trim()))
            {
                sqlwhere += " and StaffInfo_Name like '%" + txtStaName.Text + "%'";
            }
            if (!string.IsNullOrEmpty(txtTel.Text.Trim()))
            {
                sqlwhere += " and StaffInfo_Phone like '%" + this.txtTel.Text.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(txtKeHu.Text.Trim()))
            {
                sqlwhere += " and  CustomerInfo_Name like '%" + txtKeHu.Text.Trim() + "%'"; ;
            }
            if (!string.IsNullOrEmpty(txtCarNum.Text.Trim()))
            {
                sqlwhere += " and CarInfo_Name like '%" + this.txtCarNum.Text.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(txtStaInds.Text.Trim()))
            {
                sqlwhere += " and StaffInfo_Identity like '%" + this.txtStaInds.Text.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(txtSamllSer.Text.Trim()))
            {
                sqlwhere += " and SmallTicket_Serialnumber like '%" + this.txtSamllSer.Text.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(txtSamllSort.Text.Trim()))
            {
                sqlwhere += " and SmallTicket_SortNumber like '%" + this.txtSamllSort.Text.Trim() + "%'";
            }
            if (txtbeginTime.Value.ToString("yyyyMMddhhmmss") != txtendTime.Value.ToString("yyyyMMddhhmmss"))
            {
                sqlwhere += " and CarInfo_Time >'" + txtbeginTime.Value + "' and CarInfo_Time <'" + txtendTime.Value + "'";
            }

            if (inTimeStart.Value != minTime)
            {
                sqlwhere += " and CarInoutRecord_InTime > '" + inTimeStart.Value + "'";
            }
            if (inTimeEnd.Value != minTime)
            {
                sqlwhere += " and CarInoutRecord_InTime < '" + inTimeEnd.Value + "'";
            }
            if (outTimeStart.Value != minTime)
            {
                sqlwhere += " and CarInoutRecord_OutTime > '" + outTimeStart.Value + "'";
            }
            if (outTimeEnd.Value != minTime)
            {
                sqlwhere += " and CarInoutRecord_OutTime < '" + outTimeEnd.Value + "'";
            }

        }
        private void gridevewCarState_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        private void bindingNavigator1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            //  LogInfoLoad(e.ClickedItem.Name);

            if (e.ClickedItem.Name == "tsbtnUpdate")//修改
            {
                if (tsbtnUpdate.Enabled)
                {
                    tsbtnUpdate.Enabled = false;
                    tsbtnUpdate.Enabled = true;
                }
                return;
            }
            if (e.ClickedItem.Name == "tsbtnDel")//删除
            {
                tbtnDelUser_delete();
                return;
            }
            if (e.ClickedItem.Name == "tslSelectAll")//全选
            {
                ISExecl = true;
                if (tslSelectAll.Enabled)
                {
                    tslSelectAll.Enabled = false;
                    tslSelectAll_Click();
                    tslSelectAll.Enabled = true;
                }
                return;
            }
            if (e.ClickedItem.Name == "tslNotSelect")//取消全选
            {
                ISExecl = false;
                if (tslNotSelect.Enabled)
                {
                    tslNotSelect.Enabled = false;
                    tslNotSelect_Click();
                    tslNotSelect.Enabled = true;
                }
                return;
            }
            if (e.ClickedItem.Name == "tslbExecl")//导出Execl
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(tsbExecl_Click), null);
                return;
            }
            LogInfoLoad(e.ClickedItem.Name);
        }
        private void tscbxPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CommonalityEntity.isLoad == 1)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(LogInfoLoad), "");
            }
            else
            {
                LogInfoLoad("");
            }
        }

        /// <summary>
        /// 取消全选
        /// </summary>
        private void tslNotSelect_Click()
        {

            for (int i = 0; i < gridevewCarState.Rows.Count; i++)
            {
                gridevewCarState.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>     
        private void tslSelectAll_Click()
        {
            for (int i = 0; i < gridevewCarState.Rows.Count; i++)
            {
                gridevewCarState.Rows[i].Selected = true;
            }
        }
        /// <summary>
        ///删除用户信息 
        /// </summary>
        private void tbtnDelUser_delete()
        {
            try
            {

            }
            catch
            {

                CommonalityEntity.WriteTextLog("用户管理 tbtnDelUser_delete()+");
            }
            finally
            {
            }
        }

        /// <summary>
        /// 设置每页显示最大条数事件
        /// </summary>
        private void tscbxPageSize2_SelectedIndexChanged(object sender, EventArgs e)
        {
            page = new PageControl();
        }

        private void lvwUserList_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.gridevewCarState.SelectedRows.Count > 0)//选中行
            {
                if (gridevewCarState.SelectedRows.Count > 1)
                {
                    MessageBox.Show("修改只能选中一行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //修改的值
                    UserIDSearch = int.Parse(this.gridevewCarState.SelectedRows[0].Cells["UserID"].Value.ToString());
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            binWhere();
            LogInfoLoad("");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridevewCarState_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.gridevewCarState.SelectedRows[0].Cells["CarInfo_ID"].Value.ToString() == "")
                {
                    MessageBox.Show("请选择要查看的单元格！");
                    return;
                }
                //得到车辆信息编号 保存车辆信息编号
                CommonalityEntity.CarInfo_ID = this.gridevewCarState.SelectedRows[0].Cells["CarInfo_ID"].Value.ToString();
                if (CommonalityEntity.CarInfo_ID == "")
                {
                    MessageBox.Show("请重试");
                    return;
                }
                else
                {
                    CommonalityEntity.CarInoutid = this.gridevewCarState.SelectedRows[0].Cells["CarInOutRecord_ID"].Value.ToString();
                    CarInfoInOutRecordForm cf = new CarInfoInOutRecordForm();
                    PublicClass.ShowChildForm(cf);
                    //MainForm mf = new MainForm();
                    //mf.ShowChildForm(cf, this);
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog(" CarState.gridevewCarState_DoubleClick()");
            }

        }
        /// <summary>
        /// 将“DataTiemPicker”控件赋值为null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtbeginTime_ValueChanged(object sender, EventArgs e)
        {
            this.txtbeginTime.Format = DateTimePickerFormat.Custom;
            this.txtbeginTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
        }
        private void txtendTime_ValueChanged(object sender, EventArgs e)
        {
            this.txtendTime.Format = DateTimePickerFormat.Custom;
            this.txtendTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
        }


        private void tsbExecl_Click(object obj)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            string fileName = "车辆状态查询Excel报表-" + CommonalityEntity.GetServersTime().ToShortDateString();
            if (!ISExecl)
            {
                tslExport_Excel(fileName, gridevewCarState);
            }
            else
            {
                binWhere();
                string str = "select CarInfo_ID as 编号 ,CarType_Name as	车辆类型,CarInfo_Name as 车牌号,SmallTicket_Serialnumber as 小票号,SmallTicket_SortNumber as 排队号 ,StaffInfo_Name as 姓名,StaffInfo_Identity as 身份证号码,StaffInfo_Phone as 手机号码,SortNumberInfo_TongXing as 通行状态,SortNumberInfo_State as 车辆呼叫状态 ,CarType_OtherProperty as 车辆属性,CustomerInfo_Name as 公司名称,CarInOutRecord_OutCheck as 出门授权,CarInOutRecord_InCheck as 进门授权,CarInfo_State as 车辆状态,CarInfo_Operate as 登记人,CarInfo_Time as 登记时间,CarInoutRecord_InTime as 进厂时间,CarInoutRecord_OutTime as	出厂时间	,CarInfo_Remark as 备注 from carstateK where " + sqlwhere + " order by CarInOutRecord_ID  desc";
                daochu(fileName, str);
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

        private void comboxCarType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (comboxCarType.SelectedIndex > 0)
                {
                    DataTable dt = LinQBaseDao.Query("select BusinessType_ID,BusinessType_Name from BusinessType where BusinessType_CarType_ID=" + comboxCarType.SelectedValue).Tables[0];
                    DataRow dr = dt.NewRow();
                    dr["BusinessType_ID"] = "0";
                    dr["BusinessType_Name"] = "全部";
                    dt.Rows.InsertAt(dr, 0);
                    cmbBusType.DataSource = dt;
                    this.cmbBusType.DisplayMember = "BusinessType_Name";
                    this.cmbBusType.ValueMember = "BusinessType_ID";
                    this.cmbBusType.SelectedIndex = 0;
                }
            }
            catch
            {

            }
        }
        /// <summary> 
        ///  导出Execl 全选
        /// </summary> 
        /// <param name="dt"></param> 
        protected void ExportExcel(string fileName, DataTable dt)
        {
            try
            {
                if (dt == null || dt.Rows.Count == 0) return;
                string saveFileName = "";
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.DefaultExt = "xls";
                saveDialog.Filter = "Excel文件|*.xls";
                saveDialog.FileName = fileName;
                saveDialog.ShowDialog();
                saveFileName = saveDialog.FileName;
                if (saveFileName.IndexOf(":") < 0) return; //被点了取消 

                ISdao = true;
                groupBox3.Visible = true;
                btnSet.Text = "取消导出";
                progressBar1.Maximum = dt.Columns.Count;
                progressBar1.Value = 0;
                label19.Text = "正在导出：" + fileName;

                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
                if (xlApp == null)
                {
                    MessageBox.Show("无法创建Excel对象，可能您的机子未安装Excel");
                    return;
                }
                Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1 

                Microsoft.Office.Interop.Excel.Range range = worksheet.get_Range("A1", "Z" + (dt.Rows.Count + 10));
                range.NumberFormatLocal = "@";
                //写入标题
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                }
                //写入数值
                int s = 0;
                for (int r = 0; r < dt.Columns.Count; r++)
                {
                    if (ISdao)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            worksheet.Cells[i + 2, r + 1] = dt.Rows[i][r].ToString();
                        }
                        System.Windows.Forms.Application.DoEvents();
                        progressBar1.Value++;


                    }
                    else
                    {
                        break;
                    }
                }
                worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
                Microsoft.Office.Interop.Excel.Range rang = worksheet.get_Range(worksheet.Cells[2, 1], worksheet.Cells[dt.Rows.Count + 2, 2]);
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
                        MessageBox.Show("导出文件时出错,文件可能正被打开！\n" + "");
                    }

                }
                xlApp.Quit();
                GC.Collect();//强行销毁 
                //MessageBox.Show(fileName + ",保存成功", "提示", MessageBoxButtons.OK);
                if (progressBar1.Value >= progressBar1.Maximum)
                {
                    label19.Text = fileName;
                    btnSet.Text = "导出完成";
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(fileName + ",导出错误", "提示", MessageBoxButtons.OK);
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

        private void gridevewCarState_Click(object sender, EventArgs e)
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
                    label19.Text = filename;
                    btnSet.Text = "导出完成";
                }
            }
            catch { }
        }

    }
}
