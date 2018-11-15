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

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class CheckInfo : Form
    {
        public CheckInfo()
        {
            InitializeComponent();
        }
        public static CheckInfo ci = null;
        //public MainForm mf = new MainForm();
        private void CheckInfo_Load(object sender, EventArgs e)
        {
            this.Location = new Point(700, 80);
            PormptInfo();
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        public void PormptInfo()
        {
            try
            {
                string strsnc = "";
                if (SystemClass.PositionName == "停车场")
                {
                    DataTable dt = LinQBaseDao.Query("select CarType_Name,CarInfo_Name,SmallTicket_SortNumber  ,StaffInfo_Phone from View_CarState where  SortNumberInfo_TongXing='排队中' and CarInOutRecord_InCheck='是' and SmallTicket_SortNumber!=''  order by  CarInOutRecord_InCheckTime").Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            strsnc += dt.Rows[i][0].ToString() + "  " + dt.Rows[i][1].ToString() + "  " + dt.Rows[i][2].ToString() + "  " + dt.Rows[i][3].ToString() + "\r\n";
                        }
                    }
                    else
                    {
                        strsnc = "";
                    }
                }
                else
                {
                    DataTable dt = LinQBaseDao.Query("select CarType_Name,CarInfo_Name,SmallTicket_SortNumber  ,StaffInfo_Phone from View_CarState where  SortNumberInfo_TongXing='排队中' and CarInOutRecord_InCheck='是' and SmallTicket_Position_ID=" + SystemClass.PositionID + " and SmallTicket_SortNumber!=''   order by  CarInOutRecord_InCheckTime").Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            strsnc += dt.Rows[i][0].ToString() + "  " + dt.Rows[i][1].ToString() + "  " + dt.Rows[i][2].ToString() + "  " + dt.Rows[i][3].ToString() + "\r\n";
                        }
                    }
                    else
                    {
                        strsnc = "";
                    }
                }
                txtPrompt.Text = strsnc;
            }
            catch (Exception)
            {
            }
        }
        private void CheckInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            CheckInfo.ci = null;
        }
    }
}
