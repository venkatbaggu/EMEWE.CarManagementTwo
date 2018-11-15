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
    public partial class SAPRecordUpdate : Form
    {
        public SAPRecordUpdate()
        {
            InitializeComponent();
        }
        public SAPRecordUpdate(string sapid, string sernum)
        {
            this.sap_id = sapid;
            strSerialnumber = sernum;
            InitializeComponent();
        }
        private string sap_id = "";
        private string strSerialnumber = "";
        private void btnRecognition_Click(object sender, EventArgs e)
        {
            strSerialnumber = txtSmallSer.Text.Trim();
            if (!string.IsNullOrEmpty(strSerialnumber))
            {
                DataTable dt = LinQBaseDao.Query("select * from eh_SAPRecord where Sap_Serialnumber='" + strSerialnumber + "' ").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["Sap_Type"].ToString() == "送货车辆")
                        {
                            txtOldNum.Text = dt.Rows[i]["Sap_InNO"].ToString();
                            txtBiaozhi.Text = dt.Rows[i]["Sap_Identify"].ToString();
                            txtCarName.Text = dt.Rows[i]["Sap_InCarNumber"].ToString();
                            txtInOut.Text = dt.Rows[i]["Sap_InCRFLG"].ToString();
                            txtMiao.Text = dt.Rows[i]["Sap_OutMAKTX"].ToString();
                            txtGYS.Text = dt.Rows[i]["Sap_OutNAME1C"].ToString();
                            txtCust.Text = dt.Rows[i]["Sap_OutNAME1P"].ToString();
                            txtSamll.Text = dt.Rows[i]["Sap_Serialnumber"].ToString();
                            txtCarType.Text = dt.Rows[i]["Sap_Type"].ToString();
                            sap_id = dt.Rows[i]["Sap_ID"].ToString();
                        }
                    }
                }
                else
                {
                    MessageBox.Show(this, "请输入需要修改的送货车辆的小票号！");
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtNewNum.Text.Trim() == "")
            {
                MessageBox.Show(this, "请输入新的单号！");
                return;
            }
            if (txtRemark.Text.Trim() == "")
            {
                MessageBox.Show(this, "请输入修改原因！");
                return;
            }
            if (sap_id == "")
            {

                MessageBox.Show(this, "修改的数据标识失效，请重新数据小票获取！");
                return;
            }
            string rstr = "";
            if (CommonalityEntity.ISSapCheck(txtCarType.Text.Trim(), txtNewNum.Text.Trim(), out rstr))
            {
                string strSql = "update eh_SAPRecord set Sap_Remark='" + txtRemark.Text.Trim() + "' ,Sap_InNO='" + txtNewNum.Text.Trim() + "' where Sap_ID=" + sap_id;
                LinQBaseDao.Query(strSql);
                strSql = "update View_CarState set CarInfo_PO='" + txtNewNum.Text.Trim() + "' where SmallTicket_Serialnumber='" + txtNewNum.Text.Trim() + "'";
                LinQBaseDao.Query(strSql);
                string operateContent = "SAP手动修改单号将原单号" + txtOldNum.Text.Trim() + "改为新单号" + txtNewNum.Text.Trim() + ",修改原因：" + txtRemark.Text.Trim();

                MessageBox.Show(this, "SAP新单号修改成功！");
            }
            else
            {
                MessageBox.Show(this, "SAP新单号验证失败," + rstr);
            }



        }

        public void GetSapDate(string strSerialnumber)
        {
            string strSql = "select top 1 * from eh_SAPRecord where sap_state='1' and Sap_Serialnumber='" + strSerialnumber + "' and Sap_InCRFLG is null  order by Sap_ID desc";
            DataSet ds = LinQBaseDao.Query(strSql);
            if (ds != null)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    eh_SAPRecord sapobj = new eh_SAPRecord();

                    sapobj.Sap_ID = CommonalityEntity.GetInt(ds.Tables[0].Rows[0]["Sap_ID"]);
                    sapobj.Sap_Identify = ds.Tables[0].Rows[0]["Sap_Identify"].ToString();
                    sapobj.Sap_InCarNumber = ds.Tables[0].Rows[0]["Sap_InCarNumber"].ToString();
                    sapobj.Sap_InCarOperate = ds.Tables[0].Rows[0]["Sap_InCarOperate"].ToString();
                    sapobj.Sap_InCRFLG = ds.Tables[0].Rows[0]["Sap_InCarOperate"].ToString();
                    sapobj.Sap_InNO = ds.Tables[0].Rows[0]["Sap_InNO"].ToString();
                    sapobj.Sap_InTime = Convert.ToDateTime(ds.Tables[0].Rows[0]["Sap_InTime"]);
                    sapobj.Sap_OutEMSG = ds.Tables[0].Rows[0]["Sap_OutEMSG"].ToString();
                    sapobj.Sap_OutETYPE = ds.Tables[0].Rows[0]["Sap_OutETYPE"].ToString();
                    sapobj.Sap_OutHG = ds.Tables[0].Rows[0]["Sap_OutHG"].ToString();
                    sapobj.Sap_OutKDATB = Convert.ToDateTime(ds.Tables[0].Rows[0]["Sap_OutKDATB"]);
                    sapobj.Sap_OutKDATE = Convert.ToDateTime(ds.Tables[0].Rows[0]["Sap_OutKDATE"]);
                    sapobj.Sap_OutMAKTX = ds.Tables[0].Rows[0]["Sap_OutMAKTX"].ToString();
                    sapobj.Sap_OutNAME1C = ds.Tables[0].Rows[0]["Sap_OutNAME1C"].ToString();
                    sapobj.Sap_OutNAME1P = ds.Tables[0].Rows[0]["Sap_OutNAME1P"].ToString();
                    sapobj.Sap_OutOFLAG = ds.Tables[0].Rows[0]["Sap_OutOFLAG"].ToString();
                    sapobj.Sap_OutTELNUMBER = ds.Tables[0].Rows[0]["Sap_OutTELNUMBER"].ToString();
                    sapobj.Sap_OutXZ = ds.Tables[0].Rows[0]["Sap_OutXZ"].ToString();
                    sapobj.Sap_Remark = ds.Tables[0].Rows[0]["Sap_Remark"].ToString();
                    sapobj.Sap_Serialnumber = ds.Tables[0].Rows[0]["Sap_Serialnumber"].ToString();
                    sapobj.Sap_State = CommonalityEntity.GetInt(ds.Tables[0].Rows[0]["Sap_State"].ToString());
                    sapobj.Sap_Type = ds.Tables[0].Rows[0]["Sap_Type"].ToString();
                }

            }

        }

        private void SAPRecordUpdate_Load(object sender, EventArgs e)
        {
            txtSmallSer.Text = strSerialnumber;
            btnRecognition_Click(sender, e);
        }

    }
}
