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
    public partial class ResetApplication : Form
    {

        /// <summary>
        /// 实例化分页控件
        /// </summary>
        PageControl page = new PageControl();
        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere = " 1=1";
        /// <summary>
        /// 新增申请数据的条件
        /// </summary>
        private string addsqlwhere = " where 1=1";

        DateTime stime;

        public ResetApplication()
        {
            InitializeComponent();
        }

        //加载方法
        private void ResetApplication_Load(object sender, EventArgs e)
        {
            stime = CommonalityEntity.GetServersTime().AddMonths(-1);
            sqlwhere = " 1=1  and CarInfo_Time> '" + stime + "'";
            LoadData();
            userContext();
        }

        //控件权限
        public void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {
                btnshenqing.Visible = true;
                btnshenqing.Enabled = true;
                btn_cz.Visible = true;
                btn_cz.Enabled = true;
            }
            else
            {
                btnshenqing.Enabled = ControlAttributes.BoolControl("btnshenqing", "ResetApplication", "Enabled");
                btnshenqing.Visible = ControlAttributes.BoolControl("btnshenqing", "ResetApplication", "Visible");

                btn_cz.Enabled = ControlAttributes.BoolControl("btn_cz", "ResetApplication", "Enabled");
                btn_cz.Visible = ControlAttributes.BoolControl("btn_cz", "ResetApplication", "Visible");
            }
        }

        /// <summary>
        /// 组合查询条件
        /// </summary>
        public void Where()
        {
            sqlwhere = " 1=1";

            if (txtchepai.Text.Trim() != "")
            {
                sqlwhere += " and CarInfo_Name like '%" + txtchepai.Text.Trim() + "%'";
            }
            if (txtmengang.Text.Trim() != "")
            {
                sqlwhere += " and StaffInfo_Name like '%" + txtmengang.Text.Trim() + "%'";
            }
            if (txtpaiduihao.Text.Trim() != "")
            {
                sqlwhere += " and SmallTicket_SortNumber like '%" + txtpaiduihao.Text.Trim() + "%'";
            }
            if (txtxiaopiao.Text.Trim() != "")
            {
                sqlwhere += " and SmallTicket_Serialnumber like '%" + txtxiaopiao.Text.Trim() + "%'";
            }
        }

        /// <summary>
        /// 菜单栏加载数据
        /// </summary>
        private void LoadData()
        {
            try
            {
                this.gridevewSortReset.AutoGenerateColumns = false;//设置只显示列表控件绑定的列
                this.gridevewSortReset.DataSource = null;
                LogInfoLoad("");
            }
            catch
            {
                CommonalityEntity.WriteTextLog("日记管理 LoadData()");
            }
        }
        //分页工具事件
        private void tscbxPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogInfoLoad("");
        }
        /// <summary>
        /// 加载数据
        /// </summary>
        public void LogInfoLoad(string strClickedItemName)
        {
            page.BindBoundControl(gridevewSortReset, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, " CarInfo_Name,CarInfo_ID,SortNumberInfo_SmallTicket_ID,SortNumberInfo_TongXing,CarType_Name,SmallTicket_SortNumber,SmallTicket_Serialnumber,ICCard_ID,ICCard_Value,StaffInfo_Name,StaffInfo_Identity,CustomerInfo_Name ", sqlwhere, "CarInOutRecord_ID desc");
        }

        //搜索方法
        private void btnsousuo_Click(object sender, EventArgs e)
        {
            try
            {
                cbohou.Enabled = false;
                btnshenqing.Enabled = false;
                Where();
                LoadData();
            }
            catch
            {
                CommonalityEntity.WriteTextLog("状态重置 搜索方法");
            }
        }

        //申请重置
        private void btnshenqing_Click(object sender, EventArgs e)
        {
            addsqlwhere = " where 1=1";
            if (txtRemark.Text.Trim() == "")
            {
                MessageBox.Show("重置原因不能为空。", "提示");
                return;
            }
            if (gridevewSortReset.SelectedRows.Count > 1)
            {
                MessageBox.Show("每次只能对一辆车进行重置申请。", "提示");
                return;
            }
            else if (gridevewSortReset.SelectedRows.Count == 0)
            {
                MessageBox.Show("请选择一个车辆信息进行重置申请。", "提示");
                return;
            }
            if (txtqian.Text.ToString() == cbohou.SelectedText.ToString())
            {
                MessageBox.Show("重置后状态不能与重置前是相同的状态。", "提示");
                return;
            }
            if (string.IsNullOrEmpty(cbohou.Text))
            {
                MessageBox.Show("请选择重置后状态。", "提示");
                return;
            }
            if (gridevewSortReset.SelectedRows.Count == 1)
            {
                string sqlcc = "select sortReset_SortNumberInfo_SmallTicket_ID from sortReset where sortReset_CarInfo_ID = " + gridevewSortReset.SelectedRows[0].Cells["CarInfo_ID"].Value.ToString() + " and sortReset_Sortjg = 0";
                int count = LinQBaseDao.Query(sqlcc).Tables[0].Rows.Count;
                if (count >= 1)
                {
                    MessageBox.Show("该车辆已有一条未审批的记录，不能重复添加。", "提示");
                }
                else if (count == 0)
                {
                    addsqlwhere += " and CarInfo_Name = '" + gridevewSortReset.SelectedRows[0].Cells["CarInfo_Name"].Value.ToString() + "'";

                    string sql = "select CarInfo_ID,CarInfo_Name,SmallTicket_ID,SmallTicket_Serialnumber,SmallTicket_SortNumber,ICCard_ID,ICCard_Value from View_CarState " + addsqlwhere + "";
                    DataTable table = LinQBaseDao.Query(sql).Tables[0];

                    sortReset sr = new sortReset();
                    sr.sortReset_CarInfo_ID = Convert.ToInt32(table.Rows[0]["CarInfo_ID"].ToString());
                    sr.sortReset_CarInfo_Name = table.Rows[0]["CarInfo_Name"].ToString();
                    sr.sortReset_QTongxing = txtqian.Text.Trim();
                    sr.sortReset_HTongxing = cbohou.Text.Trim();

                    //判断是有小票的还是是有IC卡的车辆信息
                    if (table.Rows[0]["ICCard_Value"].ToString() == "" && table.Rows[0]["ICCard_ID"].ToString() == "")
                    {
                        sr.sortReset_SmallTicket_Serialnumber = table.Rows[0]["SmallTicket_Serialnumber"].ToString();
                    }
                    else if (table.Rows[0]["SmallTicket_Serialnumber"].ToString() == "")
                    {
                        sr.sortReset_SmallTicket_ICCard_ID = Convert.ToInt32(table.Rows[0]["ICCard_ID"].ToString());
                        sr.sortReset_SmallTicket_ICCard_Value = table.Rows[0]["ICCard_Value"].ToString();
                    }

                    sr.sortReset_SmallTicket_SortNumber = table.Rows[0]["SmallTicket_SortNumber"].ToString();
                    sr.sortReset_SortNumberInfo_SmallTicket_ID = Convert.ToInt32(table.Rows[0]["SmallTicket_ID"].ToString());
                    sr.sortReset_ShenQingTime = CommonalityEntity.GetServersTime();
                    sr.sortReset_ShenQingRen = CommonalityEntity.USERNAME;
                    sr.sortReset_Sortjg = 0;
                    sr.sortReset_Remark = txtRemark.Text.Trim();

                    if (LinQBaseDao.InsertOne<sortReset>(new DCCarManagementDataContext(), sr))
                    {
                        MessageBox.Show("申请成功！！！", "提示");

                        txtqian.Text = "";
                        txtchepai.Text = "";
                        txtmengang.Text = "";
                        txtpaiduihao.Text = "";
                        txtxiaopiao.Text = "";
                        cbohou.SelectedIndex = 0;
                        cbohou.Enabled = false;
                        btnshenqing.Enabled = false;
                    }
                    else
                    {
                        CommonalityEntity.WriteLogData("增加", "申请了车牌为" + table.Rows[0]["CarInfo_Name"].ToString() + "的状态重置申请。", CommonalityEntity.USERNAME);
                        MessageBox.Show("申请失败！！！", "提示");
                        txtchepai.Text = "";
                        txtmengang.Text = "";
                        txtpaiduihao.Text = "";
                        txtxiaopiao.Text = "";
                    }
                }
            }
        }

        //清空
        private void btnqingkong_Click(object sender, EventArgs e)
        {
            cbohou.Enabled = false;
            btnshenqing.Enabled = false;
            txtchepai.Text = "";
            txtmengang.Text = "";
            txtpaiduihao.Text = "";
            txtxiaopiao.Text = "";
        }

        private void bindingNavigator1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            LogInfoLoad(e.ClickedItem.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridevewSortReset_Click(object sender, EventArgs e)
        {
            if (gridevewSortReset.SelectedRows.Count >= 1)
            {
                txtqian.Text = gridevewSortReset.SelectedRows[0].Cells["SortNumberInfo_TongXing"].Value.ToString();
            }
            cbohou.Enabled = true;
            btnshenqing.Enabled = true;
        }

        private void txtchepai_Click(object sender, EventArgs e)
        {
            cbohou.Enabled = false;
            btnshenqing.Enabled = false;
        }

        private void txtmengang_Click(object sender, EventArgs e)
        {
            cbohou.Enabled = false;
            btnshenqing.Enabled = false;
        }

        private void txtpaiduihao_Click(object sender, EventArgs e)
        {
            cbohou.Enabled = false;
            btnshenqing.Enabled = false;
        }

        private void txtxiaopiao_Click(object sender, EventArgs e)
        {
            cbohou.Enabled = false;
            btnshenqing.Enabled = false;
        }

        //特权直接重置状态
        private void btn_cz_Click(object sender, EventArgs e)
        {
            btnshenpi();
        }
        //直接审批方法
        private void btnshenpi()
        {
            try
            {
                bool ab = true;
                bool bb = true;
                if (txtRemark.Text.Trim() == "")
                {
                    MessageBox.Show("重置原因不能为空。", "提示");
                    return;
                }
                if (gridevewSortReset.SelectedRows.Count <= 0)
                {
                    MessageBox.Show(this, "请选择需要重置的行！");
                    return;
                }
                if (gridevewSortReset.SelectedRows.Count > 1)
                {
                    MessageBox.Show(this, "只能选择一行进行重置！");
                    return;
                }
                string carname = gridevewSortReset.SelectedRows[0].Cells["CarInfo_Name"].Value.ToString();
                //车俩ID
                string carinfoid = gridevewSortReset.SelectedRows[0].Cells["CarInfo_ID"].Value.ToString();
                //小票号ID
                string smallid = gridevewSortReset.SelectedRows[0].Cells["SortNumberInfo_SmallTicket_ID"].Value.ToString();
                //重置后状态
                string htongxing = cbohou.Text.ToString();
                //重置前状态
                string qtongxing = txtqian.Text.ToString();
                //将修改前的呼叫时间储存下来
                string sqlsntime = "select SortNumberInfo_CallTime from SortNumberInfo where SortNumberInfo_SmallTicket_ID = " + smallid + "";
                DateTime sntime = Convert.ToDateTime(LinQBaseDao.Query(sqlsntime).Tables[0].Rows[0]["SortNumberInfo_CallTime"]);
                try
                {
                    string strsql = "";
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
                        //是从其它所有的状态改为已出厂时，将业务完成标识改为true
                        strsql = " update CarInOutRecord set CarInOutRecord_ISFulfill=0 where CarInOutRecord_CarInfo_ID= " + carinfoid + "\r\n";
                        //将这个车辆的状态修改为需要的状态
                        strsql += " update SortNumberInfo set SortNumberInfo_TongXing='" + htongxing + "' where SortNumberInfo_SmallTicket_ID= " + smallid;
                        LinQBaseDao.Query(strsql);
                        //单次进出门岗，已进厂车辆修改出厂通行策略
                        DataTable dtstate = LinQBaseDao.Query("select CarInOutRecord_ID,CarInOutRecord_DrivewayStrategyS,CarInOutRecord_DrivewayStrategy_IDS from CarInOutRecord where CarInOutRecord_CarInfo_ID=" + carinfoid).Tables[0];
                        if (dtstate.Rows.Count > 0)
                        {
                            string drivstr = dtstate.Rows[0]["CarInOutRecord_DrivewayStrategyS"].ToString();
                            string carinoutid = dtstate.Rows[0]["CarInOutRecord_ID"].ToString();
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


                    MessageBox.Show("重置成功！！！", "提示");
                    CommonalityEntity.WriteLogData("修改", "车牌:" + carname + "重置状态为:" + htongxing + ",通过了审批", CommonalityEntity.USERNAME);
                    addcz();
                    LogInfoLoad("");
                }
                catch
                {
                    MessageBox.Show("系统繁忙请稍后再试！！！", "提示");
                    CommonalityEntity.WriteTextLog("车牌:" + carname + "重置状态为:" + htongxing + ",审批失败");

                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("ResetApplication. btnshenpi()异常：");

            }
        }

        //将重置好了的信息添加到重置申请表，状态为已审批
        public void addcz()
        {
            addsqlwhere = " where 1=1";

            addsqlwhere += " and CarInfo_Name = '" + gridevewSortReset.SelectedRows[0].Cells["CarInfo_Name"].Value.ToString() + "'";

            string sql = "select CarInfo_ID,CarInfo_Name,SmallTicket_ID,SmallTicket_Serialnumber,SmallTicket_SortNumber,ICCard_ID,ICCard_Value from View_CarState " + addsqlwhere + "";
            DataTable table = LinQBaseDao.Query(sql).Tables[0];

            sortReset sr = new sortReset();
            //组装对象
            sr.sortReset_CarInfo_ID = Convert.ToInt32(table.Rows[0]["CarInfo_ID"].ToString());
            sr.sortReset_CarInfo_Name = table.Rows[0]["CarInfo_Name"].ToString();
            sr.sortReset_QTongxing = txtqian.Text.Trim();
            sr.sortReset_HTongxing = cbohou.Text.Trim();
            //判断是有小票的还是是有IC卡的车辆信息

            if (!string.IsNullOrEmpty(table.Rows[0]["SmallTicket_Serialnumber"].ToString()))
            {
                sr.sortReset_SmallTicket_Serialnumber = table.Rows[0]["SmallTicket_Serialnumber"].ToString();
            }
            else if (!string.IsNullOrEmpty(table.Rows[0]["ICCard_Value"].ToString()) && !string.IsNullOrEmpty(table.Rows[0]["ICCard_ID"].ToString()))
            {
                sr.sortReset_SmallTicket_ICCard_ID = Convert.ToInt32(table.Rows[0]["ICCard_ID"].ToString());
                sr.sortReset_SmallTicket_ICCard_Value = table.Rows[0]["ICCard_Value"].ToString();
            }

            sr.sortReset_SmallTicket_SortNumber = table.Rows[0]["SmallTicket_SortNumber"].ToString();
            sr.sortReset_SortNumberInfo_SmallTicket_ID = Convert.ToInt32(table.Rows[0]["SmallTicket_ID"].ToString());
            sr.sortReset_ShenQingTime = CommonalityEntity.GetServersTime();
            sr.sortReset_SortTime = CommonalityEntity.GetServersTime();
            sr.sortReset_ShenQingRen = CommonalityEntity.USERNAME;
            sr.sortReset_Sortren = CommonalityEntity.USERNAME;
            sr.sortReset_Sortjg = 1;//0为未处理，1为已处理
            sr.sortReset_Remark = txtRemark.Text.Trim();
            if (LinQBaseDao.InsertOne<sortReset>(new DCCarManagementDataContext(), sr))
            {
                CommonalityEntity.WriteLogData("修改", "特权重置了车牌为" + table.Rows[0]["CarInfo_Name"].ToString() + "的车辆通行状态。", CommonalityEntity.USERNAME);
            }
        }
    }
}
