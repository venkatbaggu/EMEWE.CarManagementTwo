using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using System.Xml;

namespace EMEWE.CarManagement.Commons.CommonClass
{
    /**/
    /// <summary> 
    /// Summary description for DatagridPrint. 
    /// </summary> 
    public class DatagridPrint
    {
        private DataSet dataGrid;//数据源
        private PrintDocument printDocument;
        private PageSetupDialog pageSetupDialog;
        private PrintPreviewDialog printPreviewDialog;
        private int iWidth;
        private Bitmap bmp;
        private object prdObj = null;//排队序号
        private int i = 0;
        public string Serialnumber = "";
        public string carinfoid = "";
        public string cartypeid = "";
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="dataGrid"></param>
        public DatagridPrint(DataSet dataGrid)
        {
            this.dataGrid = dataGrid;
            printDocument = new PrintDocument();
            printPreviewDialog = new PrintPreviewDialog();
            printDocument.PrintPage += new PrintPageEventHandler(this.printDocument_PrintPage);
            printDocument.EndPrint += new PrintEventHandler(printDocument_EndPrint);
        }
        private void PriviewKeyDown(object obj, MouseEventArgs a)
        {
            if (printDocument != null)
            {
                printDocument.Print();
            }

        }
        /// <summary>
        /// 打印结束事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printDocument_EndPrint(object sender, PrintEventArgs e)
        {
            //打印结束后相关操作
            if (i == 1)
            {
                AddPrintRecord();
            }
            i++;
        }
        /// <summary>
        /// 打印事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            try
            {
                int rowCount = 0;
                int colCount = 0;
                int x = 0;
                int y = 0;
                int rowGap = 20;
                int colGap = 22;
                Font font = new Font("宋体", 9);
                Font headingFont = new Font("宋体", 10, FontStyle.Underline);
                Font captionFont = new Font("宋体", 10, FontStyle.Bold);
                Font captionFontT = new Font("宋体", 14, FontStyle.Bold);
                Brush brush = new SolidBrush(Color.Black);
                string cellValue = "";
                rowCount = dataGrid.Tables[0].Rows.Count;
                colCount = dataGrid.Tables[0].Columns.Count;

                #region 打印内容
                int ys = 0;
                e.Graphics.DrawString("玖龙公司进出凭证", captionFontT, brush, 80, y, StringFormat.GenericDefault);
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        cellValue = dataGrid.Tables[0].Rows[i][j].ToString();
                        x = rowGap;
                        y += colGap;//打印标题
                        if (dataGrid.Tables[0].Columns[j].ColumnName == "排队号")
                        {
                            //获取该打印记录中打印小票的排队号
                            string prdSql = dataGrid.Tables[0].Rows[0]["排队号"].ToString();
                            if (string.IsNullOrEmpty(prdSql))
                            {
                                cellValue = "优先进厂";
                            }
                        }
                        e.Graphics.DrawString(dataGrid.Tables[0].Columns[j].ColumnName + ":", captionFont, brush, x, y, StringFormat.GenericDefault);

                        x = 95;//打印内容
                        e.Graphics.DrawString(cellValue, font, brush, x, y, StringFormat.GenericDefault);

                    }
                    y += colGap;
                    e.Graphics.DrawString("通行策略" + ":", captionFont, brush, rowGap, y, StringFormat.GenericDefault);
                    x = 20;
                    y += colGap;

                    //策略内容
                    //获取该车辆类型的策略



                    string clSql = "select CarInOutRecord_Update from CarInOutRecord where CarInOutRecord_CarInfo_ID=" + carinfoid;
                    object objdname = LinQBaseDao.GetSingle(clSql);
                    if (objdname != null && Convert.ToBoolean(objdname))
                    {
                        clSql = "select * from View_DrivewayStrategyRecord_Driveway_Position_CarInfo where DrivewayStrategyRecord_carinfo_id =" + carinfoid + " and DrivewayStrategyRecord_State='启动' order by DrivewayStrategyRecord_sort asc";
                        DataSet dataset = LinQBaseDao.Query(clSql);
                        int k = 0;
                        rowGap = 20;
                        foreach (DataRow dr in dataset.Tables[0].Rows)
                        {
                            k++;
                            if (k % 2 == 0)
                            {
                                rowGap = 140;
                            }
                            e.Graphics.DrawString(dr["DrivewayStrategyRecord_sort"].ToString() + "-" + dr["Position_Name"].ToString() + "-" + dr["Driveway_Name"].ToString() + "-" + dr["Driveway_Type"].ToString() + "\n", font, brush, rowGap, y, StringFormat.GenericDefault);
                            if (k % 2 == 0)
                            {
                                y += colGap;
                                rowGap = 20;
                            }
                        }
                    }
                    else
                    {
                        clSql = "select CarInOutRecord_Remark from CarInOutRecord where CarInOutRecord_CarInfo_ID=" + carinfoid;
                        objdname = LinQBaseDao.GetSingle(clSql);
                        if (objdname != null)
                        {
                            clSql = "select * from View_DrivewayStrategy_Driveway_Position where DrivewayStrategy_Name ='" + objdname + "' and DrivewayStrategy_State='启动' order by DrivewayStrategy_sort asc";
                        }
                        else
                        {
                            clSql = "select * from View_DrivewayStrategy_Driveway_Position where DrivewayStrategy_Name in (select CarType_DriSName from CarType where CarType_ID=" + cartypeid+ ") and DrivewayStrategy_State='启动' order by DrivewayStrategy_sort asc";
                        }
                        DataSet dataset = LinQBaseDao.Query(clSql);
                        int k = 0;
                        rowGap = 20;
                        foreach (DataRow dr in dataset.Tables[0].Rows)
                        {
                            k++;
                            if (k % 2 == 0)
                            {
                                rowGap = 140;
                            }
                            e.Graphics.DrawString(dr["DrivewayStrategy_sort"].ToString() + "-" + dr["Position_Name"].ToString() + "-" + dr["Driveway_Name"].ToString() + "-" + dr["Driveway_Type"].ToString() + "\n", font, brush, rowGap, y, StringFormat.GenericDefault);
                            if (k % 2 == 0)
                            {
                                y += colGap;
                                rowGap = 20;
                            }
                        }
                    }
                    rowGap = 20;
                    y += rowGap;
                    e.Graphics.DrawString("注意事项" + ":", captionFont, brush, rowGap, y, StringFormat.GenericDefault);
                    x = 95;
                    string prtSql = "select top 1 * from PrintInfo where Print_State='启动' and Print_CarType_ID=" + cartypeid + "";
                    DataTable dtprt = LinQBaseDao.Query(prtSql).Tables[0];
                    int ss = 0;
                    if (dtprt.Rows.Count > 0)
                    {
                        string strattent = dtprt.Rows[0]["Print_Attention"].ToString();
                        if (!string.IsNullOrEmpty(strattent))
                        {
                            string message = strattent;
                            for (int s = 0; s < message.Length; s++)
                            {
                                if (s % 12 == 0 && s != 0)
                                {
                                    strattent = message.Insert(s, "\r\n");
                                    ss++;
                                }
                            }
                            e.Graphics.DrawString(strattent, font, brush, x, y, StringFormat.GenericDefault);
                        }
                    }
                    y += colGap + ss * 15;
                    e.Graphics.DrawString("温馨提示" + ":", captionFont, brush, rowGap, y, StringFormat.GenericDefault);
                    x = 95;
                    if (dtprt.Rows.Count > 0)
                    {
                        string strattent = dtprt.Rows[0]["Print_Prompt"].ToString();
                        if (!string.IsNullOrEmpty(strattent))
                        {
                            string messages = strattent;
                            for (int s = 0; s < messages.Length; s++)
                            {
                                if (s % 12 == 0 && s != 0)
                                {
                                    strattent = strattent.Insert(s, "\r\n");
                                    ys++;
                                }
                            }
                            e.Graphics.DrawString(strattent, font, brush, x, y, StringFormat.GenericDefault);
                        }
                    }
                }
                y += colGap + ys * 15;
                e.Graphics.DrawImage(bmp, 8, y, 450, 120);
                y += colGap + 100;
                e.Graphics.DrawString(Serialnumber, captionFont, brush, rowGap, y, StringFormat.GenericDefault);
                #endregion
            }
            catch
            {
                CommonalityEntity.WriteTextLog("Printer printDocument_PrintPage()");
            }
        }
        /// <summary>
        /// 打印
        /// </summary>
        public void Print()
        {
            try
            {
                if (CommonalityEntity.printindex == 0)
                {
                    pageSetupDialog = new PageSetupDialog();
                    pageSetupDialog.Document = printDocument;
                    pageSetupDialog.ShowDialog();
                }
                if (CommonalityEntity.printindex == 1)
                {
                    printPreviewDialog = new PrintPreviewDialog();
                    printPreviewDialog.Document = printDocument;
                    printPreviewDialog.Height = 400;
                    printPreviewDialog.Width = 300;
                    printPreviewDialog.AutoSize = true;
                    printPreviewDialog.ShowDialog();
                }
                if (CommonalityEntity.printindex == 2)
                {
                    printDocument.Print();
                    AddPrintRecord();
                }
            }
            catch
            {
                //MessageBox.Show("没有找到打印机");
                CommonalityEntity.WriteTextLog("Printer error:");
            }
        }
        /// <summary>
        /// 记录打印记录
        /// </summary>
        private void AddPrintRecord()
        {
            try
            {

                string strsql = "   select SortNumberInfo_ID from SortNumberInfo where SortNumberInfo_SmallTicket_ID in (select SmallTicket_ID from SmallTicket where SmallTicket_CarInfo_ID='" + carinfoid + "') order by SortNumberInfo_ID desc ";
                prdObj = LinQBaseDao.GetSingle(strsql);
                //记录打印
                PrintRecord prd = new PrintRecord();
                prd.PrintRecord_Time = CommonalityEntity.GetServersTime();
                prd.PrintRecord_Remark = "";
                prd.PrintRecord_Operate = CommonalityEntity.USERNAME;
                if (!string.IsNullOrEmpty(prdObj.ToString()))
                {
                    prd.PrintRecord_SortNumberInfo_ID = int.Parse(prdObj.ToString());
                }
                PrintInfoDAL.InsertPrint(prd);
                printPreviewDialog.Close();
            }
            catch (Exception)
            {
            }
        }
        /// <summary>
        /// 生成条码
        /// </summary>
        /// <param name="code"></param>
        public void GetCode(string code)
        {
            try
            {
                if (String.IsNullOrEmpty(code))
                {
                    MessageBox.Show("请输入条形码文本！");
                    return;
                }
                //MyBarCode barcode = new MyBarCode(code);
                //bmp = barcode.PreviewBarCode;
                //iWidth = barcode.BarCodeWidth;
                bmp = DrawCode.getEAN13(code, 3, 100);
            }
            catch
            {
                CommonalityEntity.WriteTextLog("Printer GetCode()");
            }
        }
    }

}
