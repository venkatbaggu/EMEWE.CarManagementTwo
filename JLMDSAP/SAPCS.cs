using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using JLMDSAP.sappwd;

namespace JLMDSAP
{
    public partial class SAPCS : Form
    {
        public SAPCS()
        {
            InitializeComponent();
        }
  

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {

                if ((bool)dataGridView1.Rows[e.RowIndex].Cells[0].EditedFormattedValue == true)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[0].Value = false;
                    dataGridView1.Rows[e.RowIndex].Selected = false;
                    MessageBox.Show("当前选中行为:" + e.RowIndex.ToString());
                }
                else
                {
                    dataGridView1.Rows[e.RowIndex].Cells[0].Value = true;
                    dataGridView1.Rows[e.RowIndex].Selected = true;
                }
            }

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string carno = txtCarNo.Text.Trim();
            if (string.IsNullOrEmpty(carno))
            {
                MessageBox.Show(this, "请输入车牌号");
                return;
            }
            string cartype = cmbCarType.Text;
            BindLoadWeight(carno, cartype);
        }

        private void SAPCS_Load(object sender, EventArgs e)
        {
            cmbCarType.SelectedIndex = 0;
        }
        /// <summary>
        /// 获取SAP过磅数据
        /// </summary>
        /// <param name="carno">车牌号</param>
        /// <param name="cartype">车辆类型</param>
        private void BindLoadWeight(string carno, string cartype)
        {
            try
            {
                DT_pd_info_upload_reqRow dtr = new DT_pd_info_upload_reqRow();
                dtr.I_CARNO = carno;//车牌号
                dtr.I_VEHICLE_TYPE = cartype;//车辆类型

                DT_pd_info_upload_reqRow[] listdt = new DT_pd_info_upload_reqRow[1];
                listdt[0] = dtr;

                SAPQM sp = new SAPQM();
                DT_pd_info_upload_res dtc = sp.SIbdinfouploadreq(listdt);

                if (dtc == null)
                {
                    MessageBox.Show("无返回值，车牌号无效");
                    return;
                }
                else
                {
                    //if (dtc.I_FLAG == "Y")
                    //{

                        DT_pd_info_upload_resZLET_BD_DATE[] dtzlet = dtc.ZLET_BD_DATE;
                        DT_pd_info_upload_resZMMT_CPC_DAT[] dtzmmt = dtc.ZMMT_CPC_DAT;
                        if (dtzlet != null)
                        {
                            txttent.Text = "调用成功";
                            dataGridView1.DataSource = dtzlet;
                        }
                        if (dtzmmt != null)
                        {
                            txttent.Text = "调用成功";
                            dataGridView1.DataSource = dtzmmt;
                        }
                    //}
                    //else
                    //{
                    //    txttent.Text = "接口返回原因是：" + dtc.I_MSG;
                    //}
                }

            }
            catch (Exception ex)
            {
                txttent.Text = "调用失败，原因是：" + ex.Message.ToString();
            }
        }
    }
}
