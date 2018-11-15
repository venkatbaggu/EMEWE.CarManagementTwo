using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.CommonClass;
using EMEWE.CarManagementDAL;
using EMEWE.CarManagement.Commons;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using System.Speech.Synthesis;
using System.Timers;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.HelpClass;
using System.Xml;
using System.Threading;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class QueueForm : Form
    {
        /// <summary>
        /// 搜索条件
        /// </summary>
        private string where = " 排队号 != '' ";
        /// <summary>
        /// 异常记录表
        /// </summary>
        private UnusualRecord ur = null;
        /// <summary>
        /// 是否异常呼叫
        /// </summary>
        private bool isUnusual = false;
        /// <summary>
        /// 呼叫车辆数量，默认为10台车
        /// </summary>
        private int _LEDShowNumber = 2;
        private int _Driveway;//通道值
        private string _carType = "";
        public MainForm mf = new MainForm();
        public static QueueForm qf = null;

        EMEWE.CarManagement.Commons.CommonClass.PageControl Page = new EMEWE.CarManagement.Commons.CommonClass.PageControl();

        DataTable dtsortnumber;
        PositionVoice pv;//呼叫设置
        public QueueForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QueueForm_Load(object sender, EventArgs e)
        {
            combox_LEDShowNumber.SelectedIndex = 1;
            cmbtongxing.SelectedIndex = 1;
            lblDoor.Text = SystemClass.PositionName;//当前门岗
            tscbxPageSize.SelectedIndex = 2;
            GetDriveway();
            comboxDriveway_SelectionChangeCommitted(sender, e);
            userContext();
            bindcartpe();
            where += " and 通行状态='排队中'";
            GetGriddataviewLoad("");
            string voSql = "Select * from PositionVoice where PositionVoice_State='启动' and PositionVoice_Position_ID=" + SystemClass.PositionID + "";
            pv = PositionVoiceDAL.GetVoice(voSql);
        }

        private void bindcartpe()
        {
            // and CarType_State='启动'
            //select distinct(CarType_Name),CarType_ID from CarType where CarType_DriSName in (select DrivewayStrategy_Name from View_DrivewayStrategy_Driveway_Position where 1 = 1  and DrivewayStrategy_Name in (select ManagementStrategy_DriSName from ManagementStrategy where ManagementStrategy_Type = '排队'))";
            string sqls = "select distinct(CarType_Name),CarType_ID from CarType where CarType_DriSName in (select ManagementStrategy_DriSName from ManagementStrategy where ManagementStrategy_Type ='排队')";
            DataTable dt = LinQBaseDao.Query(sqls).Tables[0];
            DataRow dr = dt.NewRow();
            dr["CarType_ID"] = "0";
            dr["CarType_Name"] = "全部";
            dt.Rows.InsertAt(dr, 0);
            cmbcartype.DataSource = dt;
            cmbcartype.ValueMember = "CarType_ID";
            cmbcartype.DisplayMember = "CarType_Name";
        }
        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {
                btnCancellation.Enabled = true;
                btnCancellation.Visible = true;

                btnCall.Enabled = true;
                btnCall.Visible = true;

                btnSurIn.Enabled = true;
                btnSurIn.Visible = true;

                btnUnu.Enabled = true;
                btnUnu.Visible = true;
            }
            else
            {
                btnCancellation.Visible = ControlAttributes.BoolControl("btnCancellation", "QueueForm", "Visible");
                btnCancellation.Enabled = ControlAttributes.BoolControl("btnCancellation", "QueueForm", "Enabled");

                btnCall.Visible = ControlAttributes.BoolControl("btnCall", "QueueForm", "Visible");
                btnCall.Enabled = ControlAttributes.BoolControl("btnCall", "QueueForm", "Enabled");

                btnSurIn.Visible = ControlAttributes.BoolControl("btnSurIn", "QueueForm", "Visible");
                btnSurIn.Enabled = ControlAttributes.BoolControl("btnSurIn", "QueueForm", "Enabled");

                btnUnu.Visible = ControlAttributes.BoolControl("btnUnu", "QueueForm", "Visible");
                btnUnu.Enabled = ControlAttributes.BoolControl("btnUnu", "QueueForm", "Enabled");
            }
        }


        /// <summary>
        /// 绑定当前门岗的通道
        /// </summary>
        public void GetDriveway()
        {
            try
            {
                if (SystemClass.PositionID < 0)
                {
                    MessageBox.Show("当前门岗设置失败！");
                    return;
                }
                string sql = "select Driveway_Value,Driveway_Name from Driveway where Driveway_Position_id=" + SystemClass.PositionID + " and Driveway_State='启动' and Driveway_Type='进'";
                DataTable dt = LinQBaseDao.Query(sql).Tables[0];
                if (dt.Rows.Count > 0)
                {
                    comboxDriveway.DataSource = dt;
                    comboxDriveway.ValueMember = "Driveway_Value";
                    comboxDriveway.DisplayMember = "Driveway_Name";
                    comboxDriveway.SelectedIndex = 0;
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog("QueueFrom GetDriveway()");//记录异常
            }
        }

        #region 分页和加载DataGridView
        /// <summary>
        /// 加载信息
        /// </summary>
        public void GetGriddataviewLoad(string strClickedItemName)
        {
            Page.BindBoundControl(lvwUserList, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_LEDShow_zj", "*", "SortNumberInfo_ID", "CarInfo_ID", 0, where, true);
        }

        /// <summary>
        /// 选择每页条数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tscbxPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetGriddataviewLoad("");
        }
        /// <summary>
        /// 分页控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            GetGriddataviewLoad(e.ClickedItem.Name);
        }
        #endregion
        /// <summary>
        /// 呼叫选中车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCall_Click(object sender, EventArgs e)
        {
            int sortid = 0;
            Control.CheckForIllegalCrossThreadCalls = false;
            //没有显示数据则需要重新设置通道
            if (lvwUserList.Rows.Count <= 0)
            {
                MessageBox.Show("请先进行呼叫设置");
                return;
            }
            try
            {
                isUnusual = false;
                if (pv.PositionVoice_ID <= 0 || string.IsNullOrEmpty(_carType))
                {
                    MessageBox.Show("请先进行呼叫设置");
                    return;
                }
                if (this.lvwUserList.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("请选择需要呼叫的车辆！");
                    return;
                }
                if (CommonalityEntity.CarType.Count <= 0)
                {
                    MessageBox.Show("请先设置通道通行车辆类型");
                    return;
                }

                //状态已改变  直接呼叫下一条
                object objtongx = LinQBaseDao.GetSingle("select SortNumberInfo_TongXing from SortNumberInfo where SortNumberInfo_ID='" + this.lvwUserList.SelectedRows[0].Cells["sortNumberInfo_ID"].Value.ToString() + "'");
                if (objtongx.ToString() != "待通行" && objtongx.ToString() != "排队中")
                {
                    GetGriddataviewLoad("");// 重新加载数据
                    objtongx = LinQBaseDao.GetSingle("select SortNumberInfo_TongXing from SortNumberInfo where SortNumberInfo_ID='" + this.lvwUserList.SelectedRows[0].Cells["sortNumberInfo_ID"].Value.ToString() + "'");
                    if (objtongx.ToString() != "待通行" && objtongx.ToString() != "排队中")
                    {
                        MessageBox.Show("该车辆已不再排队中，请呼叫下一辆车！");
                        GetGriddataviewLoad("");// 重新加载数据
                    }
                }

                if (this.lvwUserList.SelectedRows[0].Cells["sortnumberinfo_state"].Value.ToString() == "注销")
                {
                    MessageBox.Show("车辆排队状态被注销，不能呼叫！");
                    return;
                }

                //emewe 103 取消条件：and Driveway_Position_ID=" + SystemClass.PositionID + ",and 通道名称='" + comboxDriveway.Text.Trim() + "' 
                string strslq = " select * from View_LEDShow_zj where  sortnumberinfo_state='启动'  and 车辆类型='" + _carType + "'   and CarInfo_State='启动'  and  小票号='" + this.lvwUserList.SelectedRows[0].Cells["Queue_Sur"].Value.ToString() + "'";
                DataTable dtled = LinQBaseDao.Query(strslq).Tables[0];
                if (dtled.Rows.Count <= 0)
                {
                    MessageBox.Show("请设置后进行呼叫车辆");
                    return;
                }
                string queuestate = this.lvwUserList.SelectedRows[0].Cells["Queue_State"].Value.ToString();
                if (queuestate != "待通行" && queuestate != "排队中")
                {
                    MessageBox.Show("请重新设定后呼叫车辆!");
                    GetGriddataviewLoad("");// 重新加载数据
                    return;
                }

                string ManagementStrategy_name = "";
                object objman = LinQBaseDao.GetSingle("select CarInOutRecord_Remark from CarInOutRecord where CarInOutRecord_ID=" + dtled.Rows[0]["CarInOutRecord_ID"].ToString());
                if (objman != null)
                {
                    ManagementStrategy_name = objman.ToString();
                }
                string sqlMan = "select ManagementStrategy_ID from  ManagementStrategy where ManagementStrategy_DriSName='" + ManagementStrategy_name + "' and ManagementStrategy_ControlInfo_ID in(select ControlInfo_ID from ControlInfo where ControlInfo_Name='进门授权校验')and ManagementStrategy_State='启动' ";
                DataTable dtMan = LinQBaseDao.Query(sqlMan).Tables[0];
                if (dtMan.Rows.Count > 0)
                {
                    if (this.lvwUserList.SelectedRows[0].Cells["CarInOutRecord_InCheck"].Value.ToString() != "是")
                    {
                        MessageBox.Show("该车辆还没有进门授权，不能呼叫！");
                        return;
                    }
                }

                //未呼叫的
                if (lvwUserList.SelectedRows[0].DefaultCellStyle.BackColor != System.Drawing.Color.Lime)
                {
                    int sum = int.Parse(combox_LEDShowNumber.Text);
                    strslq = " select top(" + sum + ") * from View_LEDShow_zj where  sortnumberinfo_state='启动'  and  车辆类型='" + _carType + "'   and   CarInfo_State='启动'  and   通行状态='排队中'  and 排队号 <> '' order by sortnumberinfo_ID";
                    ///dtsortnumber.Rows[i]["通行状态"].ToString() == "待通行"
                    dtsortnumber = LinQBaseDao.Query(strslq).Tables[0];
                    if (dtsortnumber.Rows.Count >= 1)
                    {
                        sortid = int.Parse(this.lvwUserList.SelectedRows[0].Cells["sortNumberInfo_ID"].Value.ToString());//选中当前的小票号
                    
                        CheckProperties.ce.sortValue = int.Parse(dtsortnumber.Rows[0]["SortNumberInfo_ID"].ToString());
                        //在所有没有呼叫中，必须选择最小的呼叫，否则不能呼叫
                        if (sortid != CheckProperties.ce.sortValue)
                        {
                            MessageBox.Show("按照排队序号进行呼叫");
                            return;
                        }
                        queueCar(this.lvwUserList.SelectedRows[0].Cells[2].Value.ToString(), sortid);
                        GetGriddataviewLoad("");// 重新加载数据
                    }
                    else
                    {
                        MessageBox.Show("该车辆业务类型，不可通行此门岗！！！");
                        return;
                    }
                }
                else//已经呼叫的
                {
                    DialogResult dr = MessageBox.Show("该车已做呼叫，确定重新呼叫该车辆吗？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        queueCar(this.lvwUserList.SelectedRows[0].Cells[2].Value.ToString(), sortid);
                        GetGriddataviewLoad("");// 重新加载数据
                    }
                    else
                    {
                        return;
                    }
                }
            }
            catch (Exception err)
            {
                CommonalityEntity.WriteTextLog("QueueForm btnCall_Click()");
            }
        }

        /// <summary>
        /// 呼叫车辆
        /// </summary>
        /// <param name="carName">被呼叫的车辆名称</param>
        /// <param name="queuIds">当前车辆的排队号</param>
        private void queueCar(string carName, int sortid)
        {
            try
            {
                //获取车辆排队号
                //根据排队号修改排队信息表中，当前通行状态(待通行),初始为排队中，进行信息校验时，将状态修改为通行中，通行完成，修改为已通行
                //根据门岗编号和通道编号获取车辆类型，显示通过该通道的所有的车辆类型，界面选择通行的车辆类型，开始通行。
                //获取车辆排队号
                string sortNumber = this.lvwUserList.SelectedRows[0].Cells[3].Value.ToString();
                //为管控所需参数赋值
                EMEWE.CarManagement.Commons.CommonClass.CheckProperties.CommonEntity comme = new EMEWE.CarManagement.Commons.CommonClass.CheckProperties.CommonEntity();

                comme.CarTypeKey = _Driveway.ToString();
                CheckProperties.ce = comme;


                string Queue_State = this.lvwUserList.SelectedRows[0].Cells["Queue_State"].Value.ToString();
                string strsql = " SortNumberInfo_TongXing ='待通行',SortNumberInfo_CallCount +=1,SortNumberInfo_Number='是'";

                if (Queue_State == "排队中")
                {
                    strsql += ",SortNumberInfo_CallTime ='" + CommonalityEntity.GetServersTime() + "'";
                }
                strsql = "update SortNumberInfo set " + strsql + " where SortNumberInfo_ID=" + sortid;
                LinQBaseDao.Query(strsql);

                if (Queue_State == "排队中")
                {
                    string Queue_Cartype = this.lvwUserList.SelectedRows[0].Cells["Queue_Id"].Value.ToString();
                    if (!string.IsNullOrEmpty(Queue_Cartype))
                    {
                        insertNoteRecord(Queue_Cartype);
                    }
                }
                //得到呼叫的内容
                GetData();
            }
            catch (Exception err)
            {

            }

        }

        /// <summary>
        /// 短信表添加待发送信息
        /// </summary>
        /// <param name="Queue_Cartype"></param>
        private void insertNoteRecord(string Queue_Cartype)
        {
            try
            {
                DataTable dtSort = LinQBaseDao.Query("select CarInfo_ID,CarInfo_Name,StaffInfo_Name,StaffInfo_Phone from View_CarState where SortNumberInfo_TongXing ='排队中'  and CarType_Name='" + Queue_Cartype + "' order by CarInOutRecord_ID ").Tables[0];

                if (dtSort.Rows.Count > 0)
                {
                    //间隔数量
                    DataTable dtJianGe = LinQBaseDao.Query("select PositionSMS_Remark,PositionSMS_Content from PositionSMS where PositionSMS_Position_ID =" + SystemClass.PositionID).Tables[0];
                    if (dtJianGe.Rows.Count > 0)
                    {
                        int JGcOUNT = Convert.ToInt32(dtJianGe.Rows[0]["PositionSMS_Remark"]);//间隔数量
                        string content = dtJianGe.Rows[0]["PositionSMS_Content"].ToString();//发送内容
                        if ((1 + JGcOUNT) <= dtSort.Rows.Count)
                        {
                            string CarInfo_Name = dtSort.Rows[JGcOUNT + 1]["CarInfo_Name"].ToString();
                            string CarInfo_ID = dtSort.Rows[JGcOUNT + 1]["CarInfo_ID"].ToString();
                            string StaffInfo_Name = dtSort.Rows[JGcOUNT + 1]["StaffInfo_Name"].ToString();
                            string StaffInfo_Phone = dtSort.Rows[JGcOUNT + 1]["StaffInfo_Phone"].ToString();
                            LinQBaseDao.Query("insert into NoteRecord values('" + StaffInfo_Name + "','" + StaffInfo_Phone + "'," + CarInfo_ID + ",'" + CarInfo_Name + "','" + content + "','" + CommonalityEntity.GetServersTime() + "',0)");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }
        /// <summary>
        /// 设置通道相关的车辆类型
        /// </summary>d
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboxDriveway_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ///emwe 103 20181018 取消查询条件： Position_ID=" + SystemClass.PositionID + "   and CarType_State='启动'
            string sqls = "select distinct(CarType_Name),CarType_ID from CarType where CarType_DriSName in (select DrivewayStrategy_Name from View_DrivewayStrategy_Driveway_Position where 1=1  and DrivewayStrategy_Name in (select ManagementStrategy_DriSName from ManagementStrategy where ManagementStrategy_Type ='排队'))";
            comboxCarType.DataSource = LinQBaseDao.Query(sqls).Tables[0];
            comboxCarType.ValueMember = "CarType_ID";
            comboxCarType.DisplayMember = "CarType_Name";
        }
        /// <summary>
        /// 设定通道通行的车辆类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                _carType = comboxCarType.Text.Trim();
                _Driveway = Convert.ToInt32(comboxDriveway.SelectedValue);
                _LEDShowNumber = Convert.ToInt32(combox_LEDShowNumber.Text.Trim());
            }
            catch
            {
                _LEDShowNumber = 2;
            }
            try
            {
                if (comboxDriveway.Text.Trim() == "")
                {
                    MessageBox.Show("请选择通道！");
                    return;
                }
                if (comboxCarType.Text.Trim() == "")
                {
                    MessageBox.Show("请选择车辆类型！");
                    return;
                }

                ///emewe 103 20181017 休息条件：sortnumberinfo_Positionvalue like =" + SystemClass.PosistionValue + " ，改成模糊查询，取消通道查询and 通道名称='" + comboxDriveway.Text.Trim() + "'  and  charindex('" + SystemClass.PosistionValue + "',sortnumberinfo_Positionvalue)>0 
                where = "sortnumberinfo_state='启动'  and  车辆类型='" + _carType + "'   and   CarInfo_State='启动'  and ( 通行状态='排队中' or 通行状态='待通行')  and  排队号 <> ''";
                GetGriddataviewLoad("");
                CommonalityEntity.CarType[comboxDriveway.SelectedValue.ToString()] = comboxCarType.Text.Trim();

            }
            catch
            {
                CommonalityEntity.WriteTextLog("QueueForm btnSet_Click()");
            }

        }

        #region 呼叫
        private void GetData()
        {
            try
            {
                if (CommonalityEntity.isvoic)
                {
                    if (pv.PositionVoice_Content.ToString().Length > 0)
                    {
                        string sbSpeak = string.Empty;
                        sbSpeak += "请,"+ this.lvwUserList.SelectedRows[0].Cells["Queue_Id"].Value.ToString()+","+ GetNumberGbkString(lvwUserList.SelectedRows[0].Cells["QueueCarNumber"].Value.ToString()) + ",";
                        string str = lvwUserList.SelectedRows[0].Cells["QueueShortNumber"].Value.ToString();//排队号
                        sbSpeak += GetNumberGbkString(str.Substring(str.Length - 4));
                        sbSpeak += ",进,";
                        sbSpeak += GetNumberGbkString(comboxDriveway.Text.Replace("#", "号"));

                        //呼叫准备车辆数量
                        _LEDShowNumber = Convert.ToInt32(combox_LEDShowNumber.Text.Trim());
                        int number = _LEDShowNumber;
                        //呼叫提示下一辆车
                        string strslq = " select top(" + number + ") * from View_LEDShow_zj where  sortnumberinfo_state='启动'  and  车辆类型='" + _carType + "'   and  CarInfo_State='启动'  and  通行状态='排队中'  and 排队号 <> '' order by sortnumberinfo_ID";
                        dtsortnumber = LinQBaseDao.Query(strslq).Tables[0];

                        //正常呼叫时呼叫准备入厂车辆，异常不呼叫准备车辆
                        if (!isUnusual)
                        {
                            if (dtsortnumber.Rows.Count > 1)
                            {
                                for (int i = 0; i < number; i++)
                                {
                                    if (i <= dtsortnumber.Rows.Count - 1)
                                    {
                                        sbSpeak += ",请," + GetNumberGbkString(dtsortnumber.Rows[i]["排队号"].ToString().Substring(str.Length - 4)) + ",做准备";
                                    }
                                }
                            }
                        }
                        //将呼叫内容保存到呼叫表中
                        VoiceCalls vc = new VoiceCalls();
                        vc.VoiceCalls_Content = sbSpeak;
                        vc.VoiceCalls_Number = pv.PositionVoice_Count;
                        vc.VoiceCalls_PositionName = SystemClass.PositionName;
                        vc.VoiceCalls_PositionValue = SystemClass.PosistionValue;
                        if (isUnusual)
                        {
                            vc.VoiceCalls_Remark = "异常呼叫";
                        }
                        else
                        {
                            vc.VoiceCalls_Remark = "";
                        }
                        vc.VoiceCalls_Time = CommonalityEntity.GetServersTime();
                        vc.VoiceCalls_ISVice = false;
                        if (VoiceCallsDAL.InsertVoiceCalls(vc))
                        {
                            CommonalityEntity.ishujiao = false;
                            Commons.CommonClass.VoiceReade.readCount = SystemClass.HuJiaoCount;
                            Commons.CommonClass.VoiceReade.readTxt = sbSpeak;
                            Thread myThread = new Thread(new ThreadStart(read));
                            myThread.Priority = ThreadPriority.Highest;
                            myThread.Start();
                            if (myThread.IsAlive)
                            {
                            }
                        }
                        else
                        {
                            MessageBox.Show("呼叫失败！");
                        }
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("QueueForm GetData()");
            }

        }
        private void read()
        {
            //Commons.CommonClass.VoiceReade.Read();
            Commons.CommonClass.VoiceReade.NewRead();
        }
        /// <summary>
        /// 转换为语音识别的数字
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public string GetNumberGbkString(string number)
        {

            StringBuilder convertGbk = new StringBuilder();
            foreach (char i in number)
            {
                switch (i)
                {
                    case '0':
                        convertGbk.Append("零");
                        break;
                    case '1':
                        convertGbk.Append("一");
                        break;
                    case '2':
                        convertGbk.Append("二");
                        break;
                    case '3':
                        convertGbk.Append("三");
                        break;
                    case '4':
                        convertGbk.Append("四");
                        break;
                    case '5':
                        convertGbk.Append("五");
                        break;
                    case '6':
                        convertGbk.Append("六");
                        break;
                    case '7':
                        convertGbk.Append("七");
                        break;
                    case '8':
                        convertGbk.Append("八");
                        break;
                    case '9':
                        convertGbk.Append("九");
                        break;
                    default:
                        convertGbk.Append(i.ToString());
                        break;
                }
            }
            return convertGbk.ToString();

        }
        #endregion


        //private void comboxDriveway_DropDown(object sender, EventArgs e)
        //{
        //    GetDriveway();
        //}
        /// <summary>
        /// 异常呼叫
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnu_Click(object sender, EventArgs e)
        {
            try
            {
                isUnusual = true;
                if (pv.PositionVoice_ID <= 0)
                {
                    MessageBox.Show("请先进行呼叫设置");
                    return;
                }
                if (this.lvwUserList.SelectedRows[0].Cells["sortnumberinfo_state"].Value.ToString() == "注销")
                {
                    MessageBox.Show("车辆排队状态被注销，不能呼叫！");
                    return;
                }
                string strslq = " select * from View_LEDShow_zj where  sortnumberinfo_state='启动'  and 车辆类型='" + _carType + "' and Driveway_Position_ID=" + SystemClass.PositionID + "    and CarInfo_State='启动' and 通道名称='" + comboxDriveway.Text.Trim() + "' and  通行状态='待通行'  and 排队号 <> ''";
                DataTable dtsort = LinQBaseDao.Query(strslq).Tables[0];
                if (dtsort.Rows.Count > 1)
                {
                    MessageBox.Show("已呼叫车辆未放行，不能继续呼叫！");
                    return;
                }
                DialogResult dlgR = MessageBox.Show("确认呼叫" + this.lvwUserList.SelectedRows[0].Cells["QueueCarNumber"].Value.ToString() + "车辆? 当前呼叫为异常呼叫!", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
                if (dlgR == DialogResult.OK)
                {
                    #region 记录呼叫异常
                    ur = new UnusualRecord();
                    ur.UnusualRecord_Operate = CommonalityEntity.USERNAME;
                    ur.UnusualRecord_Reason = "异常呼叫" + this.lvwUserList.SelectedRows[0].Cells["QueueCarNumber"].Value.ToString() + "车辆";
                    ur.UnusualRecord_Remark = "异常呼叫";
                    ur.UnusualRecord_State = "启动";
                    ur.UnusualRecord_Time = CommonalityEntity.GetServersTime();
                    ur.UnusualRecord_Type = "异常呼叫";
                    ur.UnusualRecord_UnusualType_ID = 1;
                    ur.UnusualRecord_Site = "排队信息";
                    ur.UnusualRecord_SiteID = int.Parse(this.lvwUserList.SelectedRows[0].Cells["sortNumberInfo_ID"].Value.ToString());
                    ur.UnusualRecord_CarInfo_ID = int.Parse(this.lvwUserList.SelectedRows[0].Cells["CarInfo_ID"].Value.ToString());
                    UnusualRecordDAL.InsertUnusualRecord(ur);//记录异常信息
                    #endregion
                }
                else
                {
                    return;
                }
                //得到排队信息表编号，修改排队状态

                string sortid = this.lvwUserList.SelectedRows[0].Cells["sortNumberInfo_ID"].Value.ToString();
                string strsql = " SortNumberInfo_TongXing ='待通行',SortNumberInfo_CallCount +=1,SortNumberInfo_Remark = '异常呼叫',SortNumberInfo_Number='是'";

                if (this.lvwUserList.SelectedRows[0].Cells["Queue_State"].Value.ToString() == "排队中")
                {
                    strsql += ",SortNumberInfo_CallTime ='" + CommonalityEntity.GetServersTime() + "'";
                }
                strsql = "update SortNumberInfo set " + strsql + " where SortNumberInfo_ID=" + sortid;
                LinQBaseDao.Query(strsql);
                //得到呼叫的内容
                GetData();

            }
            catch
            {
                CommonalityEntity.WriteTextLog("QueueForm btnUnu_Click():");
            }
            finally
            {
                GetGriddataviewLoad("");//重新加载数据
            }
        }
        /// <summary>
        /// 注销排队车辆
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancellation_Click(object sender, EventArgs e)
        {
            if (this.lvwUserList.SelectedRows.Count <= 0)
            {
                MessageBox.Show("请选择要注销排队的车辆信息");
                return;
            }
            DialogResult dr = MessageBox.Show("是否注销" + this.lvwUserList.SelectedRows[0].Cells["QueueCarNumber"].Value.ToString(), "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                //得到排队信息表编号，修改排队状态
                LinQBaseDao.Query("update SortNumberInfo set SortNumberInfo_TongXing='已注销' where SortNumberInfo_ID=" + this.lvwUserList.SelectedRows[0].Cells["sortNumberInfo_ID"].Value.ToString());
                CommonalityEntity.WriteLogData("修改", "注销了排队车辆:" + this.lvwUserList.SelectedRows[0].Cells["QueueCarNumber"].Value.ToString(), CommonalityEntity.USERNAME);//添加操作日志
            }
        }
        /// <summary>
        /// 小票进出厂
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSurIn_Click(object sender, EventArgs e)
        {
            if (CarInOutForm.cfin == null)
            {
                CarInOutForm.cfin = new CarInOutForm();
                //CarInOutForm.cfin.mf = this.mf;
                CarInOutForm.cfin.Show();
            }
            else
            {
                CarInOutForm.cfin.Activate();
            }
        }

        private void lvwUserList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            SolidBrush B = new SolidBrush(Color.Black);
            e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, B, e.RowBounds.Location.X + 20, e.RowBounds.Location.Y + 4);
        }

        private void lvwUserList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (this.lvwUserList.Rows.Count != 0)
            {
                for (int i = 0; i < this.lvwUserList.Rows.Count; i++)
                {
                    if (this.lvwUserList.Rows[i].Cells["Queue_State"].Value.ToString() == "待通行")
                    {
                        this.lvwUserList.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Lime;
                    }
                }
            }
        }

        private void btnSelect_Click_1(object sender, EventArgs e)
        {
            string sql = " 排队号<>'' ";
            if (!string.IsNullOrEmpty(txtSortNumberInfo_SortValue.Text.Trim()))
            {
                sql += " and 排队号 like '%" + txtSortNumberInfo_SortValue.Text.Trim() + "%' ";
            }
            if (!string.IsNullOrEmpty(txtCarInfoNumber.Text.Trim()))
            {
                sql += "and 车牌号 like '%" + txtCarInfoNumber.Text.Trim() + "%'";
            }
            if (cmbtongxing.Text != "全部")
            {
                sql += "and 通行状态 like '%" + cmbtongxing.Text.Trim() + "%'";
            }
            if (!string.IsNullOrEmpty(cmbcartype.Text))
            {
                if (cmbcartype.Text != "全部")
                {
                    sql += "and CarType_ID=" + cmbcartype.SelectedValue.ToString() + "";
                }
            }
            where = sql;
            GetGriddataviewLoad("");
        }

        private void QueueForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            QueueForm.qf = null;
        }

        private void comboxCarType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int s = Convert.ToInt32(comboxCarType.SelectedIndex.ToString());
            if (s >= 0)
            {
                if (comboxCarType.SelectedValue.ToString() != "System.Data.DataRowView")
                {
                    string busSql = " select Driveway_Name from Driveway where Driveway_ID in (select top(5) DrivewayStrategy_Driveway_ID from DrivewayStrategy where DrivewayStrategy_Name in (select CarType_DriSName from CarType where CarType_ID=" + comboxCarType.SelectedValue.ToString() + ") and charindex('" + SystemClass.PositionName + "',DrivewayStrategy_Record )>0 order by DrivewayStrategy_Sort) and Driveway_Type='进'";
                    object obj = LinQBaseDao.GetSingle(busSql);
                    if (obj != null)
                    {
                        comboxDriveway.Text = obj.ToString();
                    }
                }
            }
        }

        private void combox_LEDShowNumber_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboxDriveway_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
