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
using System.Linq.Expressions;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class CarPicInfoForm : Form
    {
        public CarPicInfoForm()
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
        private string sqlwhere = " 1=1 ";


        /// <summary>
        /// 加载时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarPicInfoForm_Load(object sender, EventArgs e)
        {
            userContext();
            tscbxPageSize.SelectedIndex = 2;
            ///LogInfoLoad("");
            StetaBingMethod();
            Time();
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
                tsbtnDel.Visible = ControlAttributes.BoolControl("tsbtnDel", "CarPicInfoForm", "Visible");
                tsbtnDel.Enabled = ControlAttributes.BoolControl("tsbtnDel", "CarPicInfoForm", "Enabled");
                tslbExecl.Visible = ControlAttributes.BoolControl("tslbExecl", "CarPicInfoForm", "Visible");
                tslbExecl.Enabled = ControlAttributes.BoolControl("tslbExecl", "CarPicInfoForm", "Enabled");
            }
        }
        private void LogInfoLoad(string strClickedItemName)
        {
            page.BindBoundControl(dgvCarPic, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_CarPic", "*", "CarPic_ID", "CarPic_ID", 0, sqlwhere, true);
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
                    cobPicImgType.DataSource = Pcob_DrivewayStrategy_State;
                    this.cobPicImgType.DisplayMember = "Dictionary_Name";
                    cobPicImgType.ValueMember = "Dictionary_ID";
                    cobPicImgType.SelectedIndex = 0;
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("CarPicInfoForm.StetaBingMethod()" );
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
            for (int i = 0; i < this.dgvCarPic.Rows.Count; i++)
            {
                dgvCarPic.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslAllChecked()
        {
            for (int i = 0; i < dgvCarPic.Rows.Count; i++)
            {
                this.dgvCarPic.Rows[i].Selected = true;
            }
        }
        /// <summary>
        /// 分页控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bindingNavigator1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "tslAllCheck")//全选
            {
                tslAllChecked();
                return;
            }
            if (e.ClickedItem.Name == "tslNotCheck")//取消全选
            {
                tslNotChecked();
                return;
            }
            if (e.ClickedItem.Name == "tsbtnDel")//删除
            {
                tsbtnDel_Click();
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
        /// 导出Excel
        /// </summary>
        private void tslbExecl_Click()
        {
            string fileName = "车辆照片导出Excel报表-" + CommonalityEntity.GetServersTime().ToShortDateString();
            tslExport_Excel(fileName, dgvCarPic);
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

            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1 

            //写入标题
            for (int i = 1; i < myDGV.ColumnCount; i++)
            {
                worksheet.Cells[1, i] = myDGV.Columns[i].HeaderText;
            }
            //写入数值
            int s = 0;
            for (int r = 0; r < myDGV.Rows.Count; r++)
            {
                if (Convert.ToBoolean(myDGV.Rows[r].Cells[0].Value))
                {
                    for (int i = 1; i < myDGV.ColumnCount; i++)
                    {
                        worksheet.Cells[s + 2, i] = myDGV.Rows[r].Cells[i].Value;
                    }
                    System.Windows.Forms.Application.DoEvents();
                    s++;
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
                    MessageBox.Show("导出文件时出错,文件可能正被打开！\n" );
                }

            }
            xlApp.Quit();
            GC.Collect();//强行销毁 
            MessageBox.Show(fileName + ",保存成功", "提示", MessageBoxButtons.OK);

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
        #endregion
        /// <summary>
        /// 删除
        /// </summary>
        private void tsbtnDel_Click()
        {
            try
            {
                if (dgvCarPic.SelectedRows.Count == 1)
                {
                    if (MessageBox.Show("确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        bool bo = false;
                        for (int i = 0; i < dgvCarPic.SelectedRows.Count; i++)
                        {
                            string CarPic_ID = dgvCarPic.SelectedRows[i].Cells["CarPic_ID"].Value.ToString();
                            Expression<Func<CarPic, bool>> funuserinfo = n => n.CarPic_ID == Convert.ToInt32(CarPic_ID);
                            if (CarPicDAL.DeleteToMany(funuserinfo))
                            {
                                bo = true;
                                CommonalityEntity.WriteLogData("删除", "删除了编号为" + CarPic_ID + "的照片", CommonalityEntity.USERNAME);//添加操作日志
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
                    MessageBox.Show("请选择一行数据进行删除");
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("CarPicInfoForm.tsbtnDel_Click()" );
            }

        }
        /// <summary>
        /// 设置默认起始时间离现在间隔一个月
        /// </summary>
        private void Time()
        {
            dateTimePicker1.Value = CommonalityEntity.GetServersTime().AddMonths(-1);
        }
        /// <summary>
        /// 绑定车辆启动状态
        /// </summary>
        private void comboxCarPic_State_DropDown()
        {
            try
            {
                this.cobPicImgType.DataSource = DictionaryDAL.GetValueDictionary("01");

                if (this.cobPicImgType.DataSource != null)
                {
                    this.cobPicImgType.DisplayMember = "Dictionary_Name";
                    this.cobPicImgType.ValueMember = "Dictionary_ID";
                    this.cobPicImgType.SelectedValue = -1;
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("CarPicInfo comboxCarPic_State_DropDown:" );
                return;
            }
        }
        /// <summary>
        /// 单击搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSeach_Click(object sender, EventArgs e)
        {
            sqlwhere = "1=1";
            if (!string.IsNullOrEmpty(cobPicImgType.Text.Trim()))
            {
                sqlwhere += " and CarPic_State = '" + cobPicImgType.Text.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(cobImgPic.Text.Trim()))
            {
                sqlwhere += " and CarPic_Type = '" + cobImgPic.Text.Trim() + "'";
            }
            if (!string.IsNullOrEmpty(txtPicOrder.Text.Trim()))
            {
                sqlwhere += " and CarPic_Add like '%" + txtPicOrder.Text.Trim()+"%'";
            }
            if (!string.IsNullOrEmpty(cobPicRemark.Text.Trim()))
            {
                sqlwhere += " and CarPic_Match = '" + cobPicRemark.Text.Trim() + "'";
            }
            //查询时间 必加
            sqlwhere += "and CarPic_Time between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "'";
            LogInfoLoad("");
        }
        /// <summary>
        /// 单击清空按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCler_Click(object sender, EventArgs e)
        {
            Empty();
        }
        /// <summary>
        /// 清空方法
        /// </summary>
        private void Empty()
        {
            txtPicOrder.Text = "";
            cobPicImgType.Text = "";
            cobImgPic.Text = "";
            cobPicRemark.Text = "";
        }

        /// <summary>
        /// 双击组件时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvCarPic_DoubleClick(object sender, EventArgs e)
        {
            if (dgvCarPic.SelectedRows.Count != 1)
            {
                MessageBox.Show("请选中一行数据查看照片");
                return;
            }
            CommonalityEntity.CarInfo_ID = dgvCarPic.SelectedRows[0].Cells["CarInfo_ID"].Value.ToString();
            CommonalityEntity.CarPic_ID = dgvCarPic.SelectedRows[0].Cells["CarPic_ID"].Value.ToString();
            CarImgInfoForm info = new CarImgInfoForm();
            info.ShowDialog();
        }

    }
}
