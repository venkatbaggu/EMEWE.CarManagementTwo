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

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class QueueResult : Form
    {
        public QueueResult()
        {
            InitializeComponent();
        }

        private string sqlwhere = " 1=1 ";

        PageControl page = new PageControl();
        private void CarState_Load(object sender, EventArgs e)
        {
            txtSQBeginTime.Value = CommonalityEntity.GetServersTime();
            txtSQEndTime.Value = CommonalityEntity.GetServersTime();
            cmbType.SelectedIndex = 0;
            tscbxPageSize.SelectedIndex = 2;
            userContext();
        }
        //控件权限
        public void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {
                tsbExecl.Visible = true;
                tsbExecl.Enabled = true;
                tsbshenpi.Visible = true;
                tsbshenpi.Enabled = true;
            }
            else
            {
                tsbExecl.Enabled = ControlAttributes.BoolControl("tsbExecl", "QueueResult", "Enabled");
                tsbExecl.Visible = ControlAttributes.BoolControl("tsbExecl", "QueueResult", "Visible");

                tsbshenpi.Enabled = ControlAttributes.BoolControl("tsbshenpi", "QueueResult", "Enabled");
                tsbshenpi.Visible = ControlAttributes.BoolControl("tsbshenpi", "QueueResult", "Visible");
            }
        }
        /// <summary>
        /// 加载信息
        /// </summary>
        public void LogInfoLoad(string strClickedItemName)
        {
            page.BindBoundControl(gridevewSortReset, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "sortReset", "case sortReset_Sortjg when '1' then '已审批' else '未审批' end as sortsp,*", "sortReset_ID", "sortReset_ID", 0, sqlwhere, true);
        }

        //分页工具事件
        private void tscbxPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogInfoLoad("");
        }


        //查询待处理项
        private void btnSelect_Click(object sender, EventArgs e)
        {
            Where();
            LogInfoLoad("");
        }

        /// <summary>
        /// 查询条件组合
        /// </summary>
        public void Where()
        {
            sqlwhere = " 1=1 ";

            if (txtxiaopiao.Text.Trim() != "")
            {
                sqlwhere += " and sortReset_SmallTicket_Serialnumber like '%" + txtxiaopiao.Text.Trim() + "%'";
            }
            if (txtchepai.Text.Trim() != "")
            {
                sqlwhere += " and sortReset_CarInfo_Name like '%" + txtchepai.Text.Trim() + "%'";
            }
            if (txtUser.Text.Trim() != "")
            {
                sqlwhere += " and sortReset_ShenQingRen like '%" + txtUser.Text.Trim() + "%'";
            }
            if (txtSQBeginTime.Value.ToString("yyyyMMddhhmmss") != txtSQEndTime.Value.ToString("yyyyMMddhhmmss"))
            {
                sqlwhere += " and sortReset_ShenQingTime >= '" + txtSQBeginTime.Value + "' and sortReset_ShenQingTime <= '" + txtSQBeginTime.Value + "'";
            }
            if (cmbType.Text == "已审批")
            {
                sqlwhere += " and sortReset_Sortjg=1 ";
            }
            if (cmbType.Text == "未审批")
            {
                sqlwhere += " and sortReset_Sortjg=0 ";
            }
        }

        //分页控件事件
        private void bindingNavigator1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "tslAllCheck")//全选
            {
                AllCheck();
                return;
            }
            if (e.ClickedItem.Name == "tslNotCheck")//取消全选
            {
                NotCheck();
                return;
            }
            if (e.ClickedItem.Name == "tsbExecl")//导出Execl
            {
                tsbExecl_Click();
                return;
            } if (e.ClickedItem.Name == "tsbshenpi")
            {
                btnshenpi();
                return;
            }

            LogInfoLoad(e.ClickedItem.Name);
        }
        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotCheck()
        {
            for (int i = 0; i < this.gridevewSortReset.Rows.Count; i++)
            {
                gridevewSortReset.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AllCheck()
        {
            for (int i = 0; i < gridevewSortReset.Rows.Count; i++)
            {
                this.gridevewSortReset.Rows[i].Selected = true;
            }
        }

        //申请批准
        private void btnshenpi()
        {
            bool ab = true;
            bool bb = true;
            bool cb = true;
            if (gridevewSortReset.SelectedRows.Count <= 0)
            {
                MessageBox.Show(this, "请选择需要审批的行！");
                return;
            }
            if (gridevewSortReset.SelectedRows.Count > 1)
            {
                MessageBox.Show(this, "只能选择一行进行审批！");
                return;
            }

            string carname = gridevewSortReset.SelectedRows[0].Cells["sortReset_CarInfo_Name"].Value.ToString();
            string carinfoid = gridevewSortReset.SelectedRows[0].Cells["sortReset_CarInfo_ID"].Value.ToString();
            string smallid = gridevewSortReset.SelectedRows[0].Cells["sortReset_SortNumberInfo_SmallTicket_ID"].Value.ToString();
            string htongxing = gridevewSortReset.SelectedRows[0].Cells["sortReset_HTongxing"].Value.ToString();
            string sortid = gridevewSortReset.SelectedRows[0].Cells["sortReset_ID"].Value.ToString();
            string qtongxing = gridevewSortReset.SelectedRows[0].Cells["sortReset_QTongxing"].Value.ToString();
            //将修改前的呼叫时间储存下来
            string sqlsntime = "select SortNumberInfo_CallTime from SortNumberInfo where SortNumberInfo_SmallTicket_ID = " + smallid + "";

            DataTable dtTime = LinQBaseDao.Query(sqlsntime).Tables[0];

            DateTime sntime = DateTime.Parse("1900-01-01 00:00:00.000");//默认最早时间
            if (dtTime.Rows.Count > 0)
            {
                sntime = Convert.ToDateTime(dtTime.Rows[0]["SortNumberInfo_CallTime"]);
            }

            try
            {
                string strsql = "";
                if (gridevewSortReset.SelectedRows[0].Cells["sortsp"].Value.ToString() == "已审批")
                {
                    MessageBox.Show(this, "已审批，无需重复审批");
                    return;
                }
                if (htongxing == "待通行" || htongxing == "排队中")
                {
                    //是从其它所有的状态改为带通行时，将这个车辆的呼叫时间修改为当前时间
                    if (htongxing == "待通行")
                    {
                        strsql = "update SortNumberInfo set SortNumberInfo_TongXing='" + htongxing + "' where SortNumberInfo_SmallTicket_ID =" + smallid + "\r\n";
                    }
                    else
                    {
                        strsql = "update SortNumberInfo set SortNumberInfo_TongXing='" + htongxing + "',SortNumberInfo_CallTime='" + CommonalityEntity.GetServersTime() + "' where SortNumberInfo_SmallTicket_ID =" + smallid + "\r\n";
                    }
                    strsql += "   update CarInOutRecord set CarInOutRecord_DrivewayStrategy_IDS=null,CarInOutRecord_ISFulfill=0  where CarInOutRecord_CarInfo_ID=" + carinfoid + "\r\n";
                    strsql += " update SmallTicket set SmallTicket_Allowcounted=0 where SmallTicket_CarInfo_ID=" + carinfoid;
                    LinQBaseDao.Query(strsql);
                }
                else if (htongxing == "已进厂")
                {
                    //是从其它所有的状态改为已出厂时，将业务完成标识改为false
                    strsql = " update CarInOutRecord set CarInOutRecord_ISFulfill=0 where CarInOutRecord_CarInfo_ID= " + carinfoid + "\r\n";
                    //将这个车辆的状态修改为需要的状态
                    strsql += " update SortNumberInfo set SortNumberInfo_TongXing='" + htongxing + "' where SortNumberInfo_SmallTicket_ID= " + smallid;
                    LinQBaseDao.Query(strsql);

                    //单次进出门岗，已进厂车辆修改出厂通行策略
                    DataTable dtstate = LinQBaseDao.Query("select CarInOutRecord_ID,CarInOutRecord_DrivewayStrategyS,CarInOutRecord_DrivewayStrategy_IDS from CarInOutRecord where CarInOutRecord_CarInfo_ID=" + carinfoid).Tables[0];
                    if (dtstate.Rows.Count > 0)
                    {
                        string carinoutid = dtstate.Rows[0]["CarInOutRecord_ID"].ToString();
                        string drivstr = dtstate.Rows[0]["CarInOutRecord_DrivewayStrategyS"].ToString();
                        string[] drivstrs = drivstr.Split(',');
                        int count = 0;
                        foreach (var item in drivstrs)
                        {
                            count++;
                        }
                        if (count == 2)
                        {
                            drivstr = drivstr.Substring(0, drivstr.IndexOf(','));
                            LinQBaseDao.Query("update CarInOutRecord set CarInOutRecord_DrivewayStrategy_IDS='" + drivstr + "',CarInOutRecord_ISFulfill=0 where CarInOutRecord_ID=" + carinoutid);
                            LinQBaseDao.Query("update SmallTicket set SmallTicket_Allowcounted=0 where SmallTicket_CarInfo_ID=" + carinfoid);
                        }
                        else if (count > 2)
                        {
                            string drivstrids = dtstate.Rows[0]["CarInOutRecord_DrivewayStrategy_IDS"].ToString();
                            if (!string.IsNullOrEmpty(drivstrids))
                            {
                                int ssy = drivstrids.LastIndexOf(',');
                                if (ssy > 0)
                                {
                                    string stasd = drivstrids.Substring(ssy + 1, drivstrids.Length - (ssy + 1));
                                    if (!string.IsNullOrEmpty(stasd))
                                    {
                                        DataTable dtv = LinQBaseDao.Query("select Driveway_Type from dbo.View_DrivewayStrategy_Driveway_Position where DrivewayStrategy_ID=" + stasd).Tables[0];
                                        if (dtv.Rows.Count > 0)
                                        {
                                            string dtype = dtv.Rows[0][0].ToString();
                                            if (dtype == "出")
                                            {
                                                drivstrids = drivstrids.Substring(0, ssy);
                                                LinQBaseDao.Query("update CarInOutRecord set CarInOutRecord_DrivewayStrategy_IDS='" + drivstrids + "',CarInOutRecord_ISFulfill=0 where CarInOutRecord_ID=" + carinoutid);
                                                LinQBaseDao.Query("update SmallTicket set SmallTicket_Allowcounted=0 where SmallTicket_CarInfo_ID=" + carinfoid);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            LinQBaseDao.Query("update CarInOutRecord set CarInOutRecord_DrivewayStrategy_IDS=CarInOutRecord_DrivewayStrategyS,CarInOutRecord_ISFulfill=1 where CarInOutRecord_ID=" + carinoutid);
                            LinQBaseDao.Query("update SmallTicket set SmallTicket_Allowcounted=1 where SmallTicket_CarInfo_ID=" + carinfoid);
                        }
                    }
                }
                else if (htongxing == "已出厂")
                {
                    //是从已出厂改为其它所有的状态，将业务完成标识改为true
                    strsql = " update CarInOutRecord set CarInOutRecord_ISFulfill=1,CarInOutRecord_FulfillTime='" + CommonalityEntity.GetServersTime() + "' where CarInOutRecord_CarInfo_ID= " + carinfoid + "\r\n";
                    //将这个车辆的状态修改为需要的状态
                    strsql += " update SortNumberInfo set SortNumberInfo_TongXing='" + htongxing + "' where SortNumberInfo_SmallTicket_ID= " + smallid;
                    LinQBaseDao.Query(strsql);
                }
                else
                {
                    //将这个车辆的状态修改为需要的状态
                    strsql = " update SortNumberInfo set SortNumberInfo_TongXing='" + htongxing + "' where SortNumberInfo_SmallTicket_ID= " + smallid;
                    LinQBaseDao.Query(strsql);
                }

                //修改车辆的申请信息的标识字段（0为未修改，1为已修改）
                strsql = "update sortReset set sortReset_Sortjg=1,sortReset_Sortren='" + CommonalityEntity.USERNAME + "',sortReset_SortTime='" + CommonalityEntity.GetServersTime() + "' where sortReset_ID=" + sortid;
                LinQBaseDao.Query(strsql);

                MessageBox.Show("审批成功！！！", "提示");
                CommonalityEntity.WriteLogData("修改", "车牌:" + carname + "重置状态为:" + htongxing + ",通过了审批", CommonalityEntity.USERNAME);
                LogInfoLoad("");
            }
            catch
            {
                MessageBox.Show("系统繁忙请稍后再试！！！", "提示");
                CommonalityEntity.WriteTextLog("车牌:" + carname + "重置状态为:" + htongxing + ",审批失败");
            }
        }
        /// <summary>
        /// 将“DataTiemPicker”控件赋值为null
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSQBeginTime_ValueChanged(object sender, EventArgs e)
        {
            this.txtSQBeginTime.Format = DateTimePickerFormat.Custom;
            this.txtSQBeginTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
        }

        private void txtSQEndTime_ValueChanged(object sender, EventArgs e)
        {
            this.txtSQEndTime.Format = DateTimePickerFormat.Custom;
            this.txtSQEndTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
        }
        private void tsbExecl_Click()
        {
            string fileName = "车辆状态重置Excel报表-" + CommonalityEntity.GetServersTime().ToShortDateString();
            tslExport_Excel(fileName, gridevewSortReset);
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
                    MessageBox.Show("导出文件时出错,文件可能正被打开！\n");
                }

            }
            //else
            //{
            //    fileSaved = false;
            //}
            xlApp.Quit();
            GC.Collect();//强行销毁 
            MessageBox.Show(fileName + ",保存成功", "提示", MessageBoxButtons.OK);

        }


    }
}
