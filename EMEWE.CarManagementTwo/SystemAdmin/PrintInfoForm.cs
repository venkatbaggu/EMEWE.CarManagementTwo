using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Commons.CommonClass;
using System.Drawing.Printing;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class PrintInfoForm : Form
    {
        public PrintInfoForm()
        {
            InitializeComponent();
        }
        public PrintInfoForm(DataSet ds)
        {
            InitializeComponent();
            dataGrid = ds;
        }
        private PrintDocument printDocument;
        private PageSetupDialog pageSetupDialog;
        private PrintPreviewDialog printPreviewDialog;
        private DataSet dataGrid;//数据源
        private int iWidth;
        private Bitmap bmp;
        private object prdObj = null;//排队序号
        private int i = 0;
        public string Serialnumber = "";
        public string carinfoid = "";
        public string cartypeid = "";
        public string position = "";

        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {

                btnsz.Enabled = true;
                btnsz.Visible = true;
                button2.Enabled = true;
                button2.Visible = true;
                btnDY.Enabled = true;
                btnDY.Visible = true;

            }
            else
            {
                btnsz.Visible = ControlAttributes.BoolControl("btnsz", "StaffInfoForm", "Visible");
                btnsz.Enabled = ControlAttributes.BoolControl("btnsz", "StaffInfoForm", "Enabled");

                button2.Visible = ControlAttributes.BoolControl("button2", "StaffInfoForm", "Visible");
                button2.Enabled = ControlAttributes.BoolControl("button2", "StaffInfoForm", "Enabled");

                btnDY.Visible = ControlAttributes.BoolControl("btnDY", "StaffInfoForm", "Visible");
                btnDY.Enabled = ControlAttributes.BoolControl("btnDY", "StaffInfoForm", "Enabled");

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

        private void PrintInfoForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.btnDY.Focus();
                GetCode(Serialnumber);
                bmp.MakeTransparent(Color.White);
            }
            catch (Exception)
            {
            }
        }


        private void Binddp()
        {
            try
            {
                DatagridPrint dp = new DatagridPrint(dataGrid);
                dp.GetCode(Serialnumber);
                dp.Serialnumber = Serialnumber;
                dp.carinfoid = carinfoid;
                dp.cartypeid = cartypeid;
                dp.Print();
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        ///  打印设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnsz_Click(object sender, EventArgs e)
        {
            CommonalityEntity.printindex = 0;
            Binddp();
        }
        /// <summary>
        /// 打印浏览
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            CommonalityEntity.printindex = 1;
            Binddp();
        }
        /// <summary>
        /// 确认打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDY_Click(object sender, EventArgs e)
        {
            CommonalityEntity.printindex = 2;
            Binddp();
            this.Close();
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                int rowCount = 0;
                int colCount = 0;
                int x = 20;
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
                #region 打印内容开始
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

                    string postionstr = "";
                    string drivewaystr = "";
                    string str = "";
                    ///获取车辆业务类型
                    string cartype = dataGrid.Tables[0].Rows[0][2].ToString();
                    ///获取默认门岗
                    string sql = " select D.DrivewayStrategy_Reason from CarType as C , DrivewayStrategy as D  where C.CarType_DriSName = D.DrivewayStrategy_Name and C.CarType_Name = '"+ cartype + "' group by D.DrivewayStrategy_Reason";
                    DataSet ds = LinQBaseDao.Query(sql);

                    if (ds.Tables[0].Rows.Count >0)
                    {
                        string strs = ds.Tables[0].Rows[0][0].ToString();

                        if (strs != "")
                        {
                            drivewaystr = ds.Tables[0].Rows[0][0].ToString();
                            postionstr = ds.Tables[0].Rows[0][0].ToString();
                            postionstr = postionstr.Substring(1, 4);
                        }
                        

                        //emewe 103 20181016 打印内容新增默认通行门岗         
                        y += colGap;
                        e.Graphics.DrawString("通行门岗" + ":", captionFont, brush, rowGap, y, StringFormat.GenericDefault);
                        x = 100;
                        e.Graphics.DrawString(postionstr, font, brush, x, y, StringFormat.GenericDefault);
                        y += colGap;
                        e.Graphics.DrawString("通行策略" + ":", captionFont, brush, rowGap, y, StringFormat.GenericDefault);
                        x = 15;
                        y += colGap;
                        e.Graphics.DrawString(drivewaystr, font, brush, x, y, StringFormat.GenericDefault);
                    }                                
                    //y += colGap;
                    //e.Graphics.DrawString("通行策略" + ":", captionFont, brush, rowGap, y, StringFormat.GenericDefault);
                    //x = 20;
                    //y += colGap;

                    //////策略内容
                    //////获取该车辆类型的策略


                    //string clSql = "select CarInOutRecord_Update from CarInOutRecord where CarInOutRecord_CarInfo_ID=" + carinfoid;
                    //object objdname = LinQBaseDao.GetSingle(clSql);
                    //if (objdname != null && Convert.ToBoolean(objdname))
                    //{
                    //    clSql = "select * from View_DrivewayStrategyRecord_Driveway_Position_CarInfo where DrivewayStrategyRecord_carinfo_id =" + carinfoid + " and DrivewayStrategyRecord_State='启动' order by DrivewayStrategyRecord_sort asc";
                    //    DataSet dataset = LinQBaseDao.Query(clSql);
                    //    int k = 0;
                    //    rowGap = 20;
                    //    foreach (DataRow dr in dataset.Tables[0].Rows)
                    //    {
                    //        k++;
                    //        if (k % 2 == 0)
                    //        {
                    //            rowGap = 140;
                    //        }
                    //        e.Graphics.DrawString(dr["DrivewayStrategyRecord_sort"].ToString() + "-" + dr["Position_Name"].ToString() + "-" + dr["Driveway_Name"].ToString() + "-" + dr["Driveway_Type"].ToString() + "\n", font, brush, rowGap, y, StringFormat.GenericDefault);
                    //        if (k % 2 == 0)
                    //        {
                    //            y += colGap;
                    //            rowGap = 20;
                    //        }
                    //    }
                    //}
                    //else
                    //{

                    //    clSql = "select CarInOutRecord_Remark from CarInOutRecord where CarInOutRecord_CarInfo_ID=" + carinfoid;
                    //    objdname = LinQBaseDao.GetSingle(clSql);
                    //    if (objdname != null)
                    //    {
                    //        clSql = "select * from View_DrivewayStrategy_Driveway_Position where DrivewayStrategy_Name ='" + objdname + "' and DrivewayStrategy_State='启动' order by DrivewayStrategy_sort asc";
                    //    }
                    //    else
                    //    {
                    //        clSql = "select * from View_DrivewayStrategy_Driveway_Position where DrivewayStrategy_Name in (select CarType_DriSName from CarType where CarType_ID=" + cartypeid + ") and DrivewayStrategy_State='启动' order by DrivewayStrategy_sort asc";
                    //    }

                    //    DataSet dataset = LinQBaseDao.Query(clSql);
                    //    int k = 0;
                    //    rowGap = 20;
                    //    foreach (DataRow dr in dataset.Tables[0].Rows)
                    //    {
                    //        k++;
                    //        if (k % 2 == 0)
                    //        {
                    //            rowGap = 140;
                    //        }
                    //        e.Graphics.DrawString(dr["DrivewayStrategy_sort"].ToString() + "-" + dr["Position_Name"].ToString() + "-" + dr["Driveway_Name"].ToString() + "-" + dr["Driveway_Type"].ToString() + "\n", font, brush, rowGap, y, StringFormat.GenericDefault);
                    //        if (k % 2 == 0)
                    //        {
                    //            y += colGap;
                    //            rowGap = 20;
                    //        }
                    //    }
                    //}

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

                #endregion

                y += colGap + ys * 15;
                e.Graphics.DrawImage(bmp, 8, y, 450, 120);
                y += colGap + 100;
                ////
                ////captionFont
                e.Graphics.DrawString(Serialnumber, captionFont, brush, rowGap, y, StringFormat.GenericDefault);              
                y += 15;
               // e.Graphics.DrawString("技术支持：", captionFont, brush, rowGap, y, StringFormat.GenericDefault);
               //  x = 87;
               //// y += 10;
               // //e.Graphics.DrawString("13632103800", font, brush, x, y, StringFormat.GenericDefault);
               // //y += 15;
               // //x = 80;
               // e.Graphics.DrawString("众乐软件 E-email:win@emewe.net", captionFont, brush, x, y, StringFormat.GenericDefault);
                #endregion

            }
            catch(Exception err)
            {
                CommonalityEntity.WriteTextLog("Printer printDocument_PrintPage()");
            }

        }

        private void btnDY_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CommonalityEntity.printindex = 2;
                Binddp();
                this.Close();
            }
        }

        private void PrintInfoForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CommonalityEntity.printindex = 2;
                Binddp();
                this.Close();
            }
        }

        private void btnsz_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CommonalityEntity.printindex = 2;
                Binddp();
                this.Close();
            }
        }

        private void button2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CommonalityEntity.printindex = 2;
                Binddp();
                this.Close();
            }
        }
    }
}
