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
using GemBox.ExcelLite;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class ICRecordForm : Form
    {
        public ICRecordForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere;
        public static MainForm mf; // 主窗体
        /// <summary>
        /// 返回一个空的预置皮重
        /// </summary>
        Expression<Func<CarAttribute, bool>> expr = null;
        /// <summary>
        /// 实例化分页控件
        /// </summary>
        PageControl page = new PageControl();
        /// <summary>
        /// 是否导出所有
        /// </summary>
        private bool ISExecl = false;

        private void ICRecordForm_Load(object sender, EventArgs e)
        {
            try
            {
                tscbxPageSize.SelectedIndex = 1;
                userContext();//控制权限
                mf = new MainForm();
                sqlwhere = " 1=1";
                selectmengang();
                ICCardType();
                //LoadData();
                txtbeginTime.Value = CommonalityEntity.GetServersTime();
                txtendTime.Value = CommonalityEntity.GetServersTime();
                groupBox1.Visible = false;
                groupBox3.Visible = false;
            }
            catch
            {

            }
        }

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
                tsbtnDel.Visible = ControlAttributes.BoolControl("tsbtnDel", "ICRecordForm", "Visible");
                tsbtnDel.Enabled = ControlAttributes.BoolControl("tsbtnDel", "ICRecordForm", "Enabled");
                tslbExecl.Visible = ControlAttributes.BoolControl("tslbExecl", "ICRecordForm", "Visible");
                tslbExecl.Enabled = ControlAttributes.BoolControl("tslbExecl", "ICRecordForm", "Enabled");
            }
        }

        /// <summary>
        /// 绑定查询项目上的门岗
        /// </summary>
        private void selectmengang()
        {
            string sql = "Select Position_Id,Position_Name from Position where Position_State='启动'";
            DataTable dt = LinQBaseDao.Query(sql).Tables[0];
            DataRow dr = dt.NewRow();
            dr["Position_Name"] = "全部";
            dr["Position_Id"] = "0";
            dt.Rows.InsertAt(dr, 0);
            cbomengang.DataSource = dt;
            cbomengang.ValueMember = "Position_Id";
            cbomengang.DisplayMember = "Position_Name";
            num = 1;
        }

        /// <summary>
        /// 绑定查询项目上的通道
        /// </summary>
        private void selecttongdao()
        {
            string sql = "Select Driveway_ID,Driveway_Name from Driveway where Driveway_Position_ID=" + cbomengang.SelectedValue + "";
            DataTable dt = LinQBaseDao.Query(sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                cbotongdao.DataSource = dt;
                DataRow dr = dt.NewRow();
                dr["Driveway_Name"] = "全部";
                dr["Driveway_ID"] = "0";
                dt.Rows.InsertAt(dr, 0);
                cbotongdao.DataSource = dt;
                cbotongdao.ValueMember = "Driveway_ID";
                cbotongdao.DisplayMember = "Driveway_Name";
                cbotongdao.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// ic卡类型
        /// </summary>
        private void ICCardType()
        {
            string sql = "select ICCardType_ID,ICCardType_Name from ICCardType";
            DataTable dt = LinQBaseDao.Query(sql).Tables[0];
            DataRow dr = dt.NewRow();
            dr["ICCardType_Name"] = "全部";
            dr["ICCardType_ID"] = "0";
            dt.Rows.InsertAt(dr, 0);
            cmbICtype.DataSource = dt;
            cmbICtype.ValueMember = "ICCardType_ID";
            cmbICtype.DisplayMember = "ICCardType_Name";
        }

        /// <summary>
        /// 菜单栏加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.dgvCarAttribute.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
                this.dgvCarAttribute.DataSource = null;
                LogInfoLoad("");
            }
            catch
            {
                CommonalityEntity.WriteTextLog("日记管理 LoadData()");
            }
        }

        /// <summary>
        /// 加载信息
        /// </summary>
        public void LogInfoLoad(string strClickedItemName)
        {
            page.BindBoundControl(dgvCarAttribute, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_CardInfo_Driveway", "*", "CardInfo_ID", "CardInfo_ID", 0, sqlwhere, true);
        }

        int num = 0;
        /// <summary>
        /// 门岗值选择变更后绑定通道
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbomengang_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbomengang.SelectedIndex > 0)
            {
                selecttongdao();
            }
            else
            {
                return;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            selectReadInfo();
        }
        /// <summary>
        /// 搜索的方法
        /// </summary>
        private void selectReadInfo()
        {
            try
            {
                sqlwhere = "  1=1";
                string ICbianhao = this.txtICkabianhaoxia.Text.Trim();
                string mengang = this.cbomengang.SelectedValue.ToString();
                string ICcardType = this.cmbICtype.Text;
                string StaName = txt_StaName.Text.ToString();
                if (!string.IsNullOrEmpty(ICbianhao))//IC卡编号
                {
                    sqlwhere += " and ICCard_Value  like '%" + ICbianhao + "%'";
                }
                if (cbomengang.SelectedIndex > 0)//门岗编号
                {
                    sqlwhere += " and Position_ID =" + mengang + "";
                    string tongdao = this.cbotongdao.SelectedValue.ToString();
                    if (cbotongdao.SelectedIndex > 0)//通道名称
                    {
                        sqlwhere += " and Driveway_ID =" + tongdao + "";
                    }
                }

                if (ICcardType != "全部")//IC卡类型
                {
                    sqlwhere += " and CardInfo_Type  = '" + ICcardType + "'";
                }
                if (txtbeginTime.Value.ToString("yyyyMMddhhmmss") != txtendTime.Value.ToString("yyyyMMddhhmmss"))//刷卡时间
                {
                    sqlwhere += " and CardInfo_Time>'" + txtbeginTime.Value + "' and CardInfo_Time<'" + txtendTime.Value + "'";
                }
                if (!string.IsNullOrEmpty(StaName))
                {
                    sqlwhere += " and CardInfo_StaName like  '%" + StaName + "%'";
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ICRecordForm.selectReadInfo异常:");
            }
            finally
            {
                LogInfoLoad("");
            }
        }

        /// <summary>
        /// 删除选中行数据的方法 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteReadInfo()
        {
            try
            {
                int j = 0;
                if (this.dgvCarAttribute.SelectedRows.Count > 0)//选中删除
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        //选中数量
                        int count = dgvCarAttribute.SelectedRows.Count;
                        string id = "";
                        //遍历
                        for (int i = 0; i < count; i++)
                        {
                            Expression<Func<CardInfo, bool>> funuserinfo = n => n.CardInfo_ID == int.Parse(this.dgvCarAttribute.SelectedRows[i].Cells["CardInfo_ID"].Value.ToString());

                            if (!CardInfoDAL.DeleteToMany(funuserinfo))
                            {
                                j++;
                            }
                        }
                        if (j == 0)
                        {
                            MessageBox.Show("成功删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LogInfoLoad("");
                        }
                        else
                        {
                            MessageBox.Show("删除失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        string strContent = "IC卡编号：" + this.dgvCarAttribute.SelectedRows[0].Cells["Driveway_ID"].Value.ToString();
                        CommonalityEntity.WriteLogData("删除", strContent, CommonalityEntity.USERNAME);//添加操作日志
                    }
                }
                else//没有选中
                {
                    MessageBox.Show("请选择要删除的行！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch
            {

                CommonalityEntity.WriteTextLog("刷卡记录查询 DeleteReadInfo() 异常！+");
            }
            finally
            {
                LogInfoLoad("");
            }
        }

        //导出Execl
        private void tslbExecl_Click()
        {
            string fileName = "刷卡记录-" + CommonalityEntity.GetServersTime().ToShortDateString();
            if (!ISExecl)
            {
                tslExport_Excel(fileName, dgvCarAttribute);
            }
            else
            {
                selectReadInfo();
                string strsql = "select CardInfo_ID as 刷卡记录编号 ,Position_Name as 刷卡门岗,Driveway_Name as 刷卡通道,CardInfo_Type as IC卡类型,ICCard_Value as IC卡号 ,CardInfo_StaName as [关联人/车],CardInfo_Time as 刷卡时间,CardInfo_InOut as 备注 from View_CardInfo_Driveway where " + sqlwhere + " order by CardInfo_ID ";
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
                    MessageBox.Show("无法创建Excel对象，可能您的电脑未安装Excel");
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
            catch (System.Exception ex)
            {

            }
        }

        //改变每页显示条数
        private void tscbxPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogInfoLoad("");
        }

        //单击分页控件的事件
        private void CredInfoNavigator1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "tslAllCheck")//全选
            {
                ISExecl = true;
                tslSelectAll();
                return;
            }
            if (e.ClickedItem.Name == "tslNotCheck")//取消全选
            {
                ISExecl = false;
                tslNotSelect();
                return;
            }
            if (e.ClickedItem.Name == "tsbtnDel")//删除
            {
                DeleteReadInfo();
                return;
            }
            if (e.ClickedItem.Name == "tslbExecl")//导出
            {
                tslbExecl_Click();
                return;
            }
            LogInfoLoad(e.ClickedItem.Name);
        }

        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslNotSelect()
        {
            for (int i = 0; i < this.dgvCarAttribute.Rows.Count; i++)
            {
                dgvCarAttribute.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslSelectAll()
        {
            for (int i = 0; i < dgvCarAttribute.Rows.Count; i++)
            {
                this.dgvCarAttribute.Rows[i].Selected = true;
            }
        }

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

        private void dgvCarAttribute_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvCarAttribute.SelectedRows[0].Cells["CardInfo_ID"].Value.ToString() == "")
                {
                    MessageBox.Show("请选择要查看的单元格！");
                    return;
                }
                groupBox1.Visible = true;
                //得到车辆信息编号 保存车辆信息编号
                string cardinfo_id = this.dgvCarAttribute.SelectedRows[0].Cells["CardInfo_ID"].Value.ToString();

                DataTable dt = LinQBaseDao.Query(" select  CardInfo_Pic,CardInfo_Remark from CardInfo where CardInfo_ID=" + cardinfo_id).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string CardInfo_Pic = dt.Rows[0][0].ToString();
                    string CardInfo_Remark = dt.Rows[0][1].ToString();
                    pic1.ImageLocation = SystemClass.SaveFile + CardInfo_Pic;
                    pic2.ImageLocation = SystemClass.SaveFile + CardInfo_Remark;
                }
                else
                {
                    pic1.ImageLocation = "";
                    pic2.ImageLocation = "";
                }
            }
            catch
            {
                groupBox1.Visible = false;
                pic1.ImageLocation = "";
                pic2.ImageLocation = "";
                CommonalityEntity.WriteTextLog(" ICRecordForm.dgvCarAttribute_DoubleClick()");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
            pic1.ImageLocation = "";
            pic2.ImageLocation = "";
        }

        private void txtICkabianhaoxia_KeyUp(object sender, KeyEventArgs e)
        {
            if (txtICkabianhaoxia.Text.Length == 10)
            {
                try
                {
                    txtICkabianhaoxia.Text = "0" + Convert.ToInt64(txtICkabianhaoxia.Text.Trim()).ToString("X");
                }
                catch
                {
                    txtICkabianhaoxia.Text = "";
                }
            }
            if (txtICkabianhaoxia.Text.Length > 10)
            {
                txtICkabianhaoxia.Text = "";
            }
        }

        bool ISdao = true;
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

        private void dgvCarAttribute_Click(object sender, EventArgs e)
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
                if (ss>ii)
                {
                    ii++;
                }
                progressBar1.Maximum = ii+1;
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
