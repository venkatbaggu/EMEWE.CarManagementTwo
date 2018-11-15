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
using EMEWE.CarManagement.HelpClass;
using EMEWE.CarManagementDAL;
using EMEWE.CarManagement.Entity;
using WindowsFormsApplication3;
using System.Threading;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class SAPCarInfoForm : Form
    {
        /// <summary>
        /// 执行管控验证
        /// </summary>
        private CheckProperties checkPr = new CheckProperties();
        private int j = 0;
        private int k = 0;
        public SAPCarInfoForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 校验登记
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChkSAP_Click(object sender, EventArgs e)
        {
            try
            {
                CommonalityEntity.IsUpdatedri = false;
                CheckProperties.ce.SapSangFeiTable = null;
                CheckProperties.ce.SapSongHuoTable = null;
                CheckProperties.ce.SapChengPinTable = null;
                CheckProperties.ce.SapSangFeiTable2 = null;
                CheckProperties.ce.SapSongHuoTable2 = null;
                CheckProperties.ce.SapChengPinTable2 = null;

                if (comboxCarType.Text.Trim() == "")
                {
                    MessageBox.Show("车辆类型不能为空！");
                    return;
                }
                if (txtNumber.Text.Trim() == "")
                {
                    MessageBox.Show(lblNumber.Text.ToString());
                    return;
                }
                CheckMethod.listMessage.Clear();
                //给需要执行的管控赋值
                EMEWE.CarManagement.Commons.CommonClass.CheckProperties.CommonEntity comm = new EMEWE.CarManagement.Commons.CommonClass.CheckProperties.CommonEntity();
                if (comboxCarType.Text == "送货车辆")
                {
                    comm.SongHuoNumber = txtNumber.Text.Trim();
                }
                else if (comboxCarType.Text == "成品车辆")
                {
                    comm.ChengPinNumber = txtNumber.Text.Trim();
                }
                else if (comboxCarType.Text == "三废车辆")
                {
                    comm.SangFeiNumber = txtNumber.Text.Trim();
                }
                comm.CarType_Name = comboxCarType.Text.Trim();
                CheckProperties.ce = comm;
                //执行管控
                if (comboxCarType.Text == "送货车辆")
                {
                    CheckMethod.ChkSongHuo();
                    CheckMethod.ChkSongHuoFour();
                }
                else if (comboxCarType.Text == "成品车辆")
                {
                    CheckMethod.ChkChengPin();
                }
                else if (comboxCarType.Text == "三废车辆")
                {
                    CheckMethod.ChkSanFei();
                }
                //执行结果
                if (CheckMethod.listMessage.Count > 0)
                {
                    string strmessage = "";
                    foreach (var item in CheckMethod.listMessage)
                    {
                        strmessage += item;
                    }
                    MessageBox.Show(this, strmessage); ;
                    return;
                }
                bool istru = false;
                if (CheckProperties.ce.SapSangFeiTable != null)
                {
                    lvwUserList.DataSource = CheckProperties.ce.SapSangFeiTable;
                    if (CheckProperties.ce.SapSangFeiTable2.Rows[0][0].ToString() == "S" && CheckProperties.ce.SapSangFeiTable.Rows.Count == 1)
                    {
                        istru = CommonalityEntity.AddSAPVBELNInfo(this.lvwUserList.SelectedRows[0].Cells["VBELN"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["NAME1_C"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["MAKTX"].Value.ToString(), 0, CheckProperties.ce.SapSangFeiTable2);
                    }
                }
                if (CheckProperties.ce.SapSongHuoTable != null)
                {
                    lvwUserList.DataSource = CheckProperties.ce.SapSongHuoTable;
                    if (CheckProperties.ce.SapSongHuoTable2.Rows[0][0].ToString() == "S" && CheckProperties.ce.SapSongHuoTable.Rows.Count == 1)
                    {
                        istru = CommonalityEntity.AddPOInfo(this.lvwUserList.SelectedRows[0].Cells["EBELN"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["NAME1_P"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["MAKTX"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["KDATB"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["KDATE"].Value.ToString(), 0, CheckProperties.ce.SapSongHuoTable2);
                    }
                }
                if (CheckProperties.ce.SapChengPinTable != null)
                {
                    lvwUserList.DataSource = CheckProperties.ce.SapChengPinTable;
                    if (CheckProperties.ce.SapChengPinTable2.Rows[0][0].ToString() == "S" && CheckProperties.ce.SapChengPinTable.Rows.Count == 1)
                    {
                        string carName = this.lvwUserList.SelectedRows[0].Cells["CARNO"].Value.ToString();
                        string wtdid = this.lvwUserList.SelectedRows[0].Cells["WTD_ID"].Value.ToString();
                        if (IsSapNo(carName, wtdid))
                        {
                            MessageBox.Show(this, "该车辆的订单号业务已完成，不能重复登记！");
                            return;
                        }
                        if (this.lvwUserList.SelectedRows[0].Cells["O_FLAG"].Value.ToString() != "X")
                        {
                            MessageBox.Show("没有装货通知单，不能登记！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                            return;
                        }
                        istru = CommonalityEntity.AddWTDIDInfo(this.lvwUserList.SelectedRows[0].Cells["CARNO"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["WTD_ID"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["O_FLAG"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["TEL_NUMBER"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["HG"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["XZ"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["KDATB"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["KDATE"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["NAME1_C"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["NAME1_P"].Value.ToString(), 0, this.lvwUserList.SelectedRows[0].Cells["Prodh"].Value.ToString(), CheckProperties.ce.SapChengPinTable2);

                    }
                }
                if (istru)
                {
                    istru = false;
                    CommonalityEntity.ISsap = true;
                    CarInfoForm cif = new CarInfoForm();
                    cif.Show();
                    this.Close();
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("SAPCarInfoForm btnChkSAP_Click()");
            }
        }
        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();//关闭当前窗体
        }


        private bool IsSapNo(string CarName, string CarInfo_PO)
        {
            try
            {
                bool istrue = false;
                DataTable dt = LinQBaseDao.Query("select * from eh_SAPRecord where Sap_InCarNumber='" + CarName + "' and Sap_State=1 order by Sap_InTime desc").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    string strNo = dt.Rows[0]["Sap_InNO"].ToString();
                    DateTime BTime = Convert.ToDateTime(dt.Rows[0]["Sap_OutKDATB"].ToString());
                    DateTime ETime = Convert.ToDateTime(dt.Rows[0]["Sap_OutKDATE"].ToString());
                    DateTime NewTime = CommonalityEntity.GetServersTime();
                    if (strNo == CarInfo_PO)
                    {
                        DataTable dtstate = LinQBaseDao.Query("select SortNumberInfo_TongXing from View_CarState where CarInfo_Name='" + CarName + "' and CarInfo_PO='" + CarInfo_PO + "' order by CarInfo_ID desc").Tables[0];
                        if (dtstate.Rows.Count > 0)
                        {
                            string tonging = dtstate.Rows[0][0].ToString();
                            if (tonging == "已出厂")
                            {
                                istrue = true;
                            }
                        }
                    }
                }
                return istrue;
            }
            catch (Exception)
            {
                return true;
            }
        }
        /// <summary>
        /// 双击选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvwUserList_DoubleClick(object sender, EventArgs e)
        {
            try
            {


                if (this.lvwUserList.SelectedRows.Count == 1)
                {
                    bool istru = false;
                    if (comboxCarType.Text.Trim() == "送货车辆")
                    {
                        istru = CommonalityEntity.AddPOInfo(this.lvwUserList.SelectedRows[0].Cells["EBELN"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["NAME1_P"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["MAKTX"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["KDATB"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["KDATE"].Value.ToString(), 0, CheckProperties.ce.SapSongHuoTable2);
                    }
                    if (comboxCarType.Text.Trim() == "成品车辆")
                    {
                        string carName = this.lvwUserList.SelectedRows[0].Cells["CARNO"].Value.ToString();
                        string wtdid = this.lvwUserList.SelectedRows[0].Cells["WTD_ID"].Value.ToString();
                        if (IsSapNo(carName, wtdid))
                        {
                            MessageBox.Show("该车辆的订单号业务已完成，不能重复登记！");
                            return;
                        }
                        if (this.lvwUserList.SelectedRows[0].Cells["O_FLAG"].Value.ToString() != "X")
                        {
                            MessageBox.Show("没有装货通知单，不能登记！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Question);
                            return;
                        }
                        istru = CommonalityEntity.AddWTDIDInfo(this.lvwUserList.SelectedRows[0].Cells["CARNO"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["WTD_ID"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["O_FLAG"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["TEL_NUMBER"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["HG"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["XZ"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["KDATB"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["KDATE"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["NAME1_C"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["NAME1_P"].Value.ToString(), 0, this.lvwUserList.SelectedRows[0].Cells["Prodh"].Value.ToString(), CheckProperties.ce.SapChengPinTable2);
                    }
                    if (comboxCarType.Text.Trim() == "三废车辆")
                    {
                        istru = CommonalityEntity.AddSAPVBELNInfo(this.lvwUserList.SelectedRows[0].Cells["VBELN"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["NAME1_C"].Value.ToString(), this.lvwUserList.SelectedRows[0].Cells["MAKTX"].Value.ToString(), 0, CheckProperties.ce.SapSangFeiTable2);
                    }
                    if (istru)
                    {
                        istru = false;
                        CommonalityEntity.ISsap = true;
                        CarInfoForm cif = new CarInfoForm();
                        cif.Show();
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("请选择车辆！");
                }
            }
            catch (Exception ex)
            {
                CommonalityEntity.WriteTextLog("SAP:" + ex.Message);
            }
        }

        private void comboxCarType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboxCarType.Text == "送货车辆")
            {
                this.lblNumber.Text = "    PO号:";
            }
            else if (comboxCarType.Text == "成品车辆")
            {
                this.lblNumber.Text = "  车牌号:";
            }
            else if (comboxCarType.Text == "三废车辆")
            {
                this.lblNumber.Text = "交货单号:";
            }
        }

        private void SAPCarInfoForm_Load(object sender, EventArgs e)
        {
            comboxCarType.SelectedIndex = 0;
        }

        private void btnChkSAP_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnChkSAP_Click(sender, e);
            }
        }

        private void txtNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnChkSAP_Click(sender, e);
            }
        }
    }

}
