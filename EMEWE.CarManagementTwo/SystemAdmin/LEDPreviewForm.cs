using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.Commons;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Commons.CommonClass;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class LEDPreviewForm : Form
    {
        PositionLED pLED = new PositionLED();
        public int type = 0;//保存车辆类型的数量
        public int count = 0;//循环标识
        //截取配置的显示颜色
        public string yanse;
        public DataSet dataset;//保存车辆类型
        private string cartypeid = "";//不显示LED车辆类型ID

        public LEDPreviewForm()
        {
            InitializeComponent();
        }
        public static LEDPreviewForm led = null;
        public MainForm mf;

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LEDPreviewForm_Load(object sender, EventArgs e)
        {
            //读取LED配置项
            string sql = "select * from PositionLED where PositionLED_State='启动' and PositionLED_Position_ID=" + SystemClass.PositionID + "";
            pLED = PositionLEDDAL.GetLED(sql);

            //加载时将窗体显示位置修改到指定位置
            int X = Convert.ToInt32(pLED.PositionLED_X);
            int Y = Convert.ToInt32(pLED.PositionLED_Y);
            this.Location = new Point(X, Y);

            //判断当前门岗是否有LED的配置信息
            if (pLED.PositionLED_ID <= 0)
            {
                lbltishi.Visible = true;
                lbltishi.Text = "当前登录门岗\r\n暂无LED显示配置信息，\r\n请先配置对应门岗显示信息";
                return;
            }
            else
            {

                DataTable dts = LinQBaseDao.Query("select CarType_ID from CarType where CarType_State='启动'").Tables[0];
                if (dts.Rows.Count > 0)
                {
                    for (int i = 0; i < dts.Rows.Count; i++)
                    {
                        // and ManagementStrategy_State='启动'
                        int kk = Convert.ToInt32(LinQBaseDao.GetSingle("select count(0) from  ManagementStrategy where ManagementStrategy_CarType_ID=" + dts.Rows[i][0].ToString() + " and ManagementStrategy_ControlInfo_ID in(select ControlInfo_ID from ControlInfo where ControlInfo_Name='排队')").ToString());
                        if (kk == 0)
                        {
                            cartypeid += dts.Rows[i][0].ToString() + ",";
                        }
                    }
                }
                cartypeid = cartypeid.TrimEnd(',');

                //读取配置的高宽（高度与宽度是反的）
                int weight = Convert.ToInt32(pLED.PositionLED_ScreenWeight);
                int heifht = Convert.ToInt32(pLED.PositionLED_ScreenHeight);
                this.Size = new Size(weight, heifht);

                //读取配置的字体颜色
                string s = pLED.PositionLED_Color;
                int cd = s.Length - 8;
                yanse = s.Substring(7, cd);

                if (pLED.PositionLED_Type == 1)//1为显示排队信息
                {
                    try
                    {
                        BindCount();
                        if (type == 0)
                        {
                            lbltishi.Visible = true;
                            lbltishi.Text = "一级防火单位，严禁将烟火带入厂区，否则引发事故将移送公安机关处理！！！";
                        }
                        else
                        {
                            BindType();
                            timer1.Enabled = false;//将刷类型的闹钟关闭
                            lbltishi.Visible = false;//将提示语的LBL关闭
                            TimerLED.Enabled = true;//只有在显示排队信息时才将闹钟打开
                        }
                    }
                    catch 
                    {
                        count = count + 1;
                        if (count == type)
                        {
                            count = 0;
                        }
                        BindType();
                    }
                }
                else if (pLED.PositionLED_Type == 2)//2为显示欢迎语
                {
                    lblhuanyingyu.Visible = true;
                    lblhuanyingyu.Text = pLED.PositionLED_Remark;
                }
            }
        }

        private void BindCount()
        {
            try
            {
                //查询车辆类型
                dataset = LinQBaseDao.Query("select CarType_Name,CarType_ID,CarType_Value from CarType where CarType_Value in( select distinct (SortNumberInfo_CarTypeValue) from SortNumberInfo where SortNumberInfo_PositionValue='" + SystemClass.PosistionValue + "' and SortNumberInfo_SortValue<>'' and SortNumberInfo_CarTypeValue in (select CarType_Value from CarType where CarType_ID not in (" + cartypeid + ")))");

                type = dataset.Tables[0].Rows.Count;
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("LEDPreviewform BindCount()");
            }
        }

        /// <summary>
        /// 加载显示内容
        /// </summary>
        private void BindType()
        {
            try
            {
                //绑定车辆类型
                string carType_Name = "";
                string carType_Id = "";
                string cattype_value = "";
                if (dataset.Tables[0].Rows.Count > 0)
                {
                    carType_Name = dataset.Tables[0].Rows[count]["CarType_Name"].ToString();
                    carType_Id = dataset.Tables[0].Rows[count]["CarType_ID"].ToString();
                    cattype_value = dataset.Tables[0].Rows[count]["CarType_Value"].ToString();
                }
                else
                {
                    return;
                }
                //绑定车辆通道
                DataSet datasettongdao = PositionLEDDAL.GetCarType(" select  Driveway_Name from Driveway where Driveway_Position_ID=" + SystemClass.PositionID + " and  Driveway_Value in( select top(1) SortNumberInfo_DrivewayValue from  SortNumberInfo where SortNumberInfo_CarTypeValue='" + cattype_value + "' and   (SortNumberInfo_DrivewayValue != '' and  SortNumberInfo_DrivewayValue is not null)  order by SortNumberInfo_ID desc)");
                string tongDao = "";
                int tongdaocount = datasettongdao.Tables[0].Rows.Count;
                if (tongdaocount > 0)
                {
                    if (tongdaocount > 1)
                    {
                        for (int i = 0; i < tongdaocount; i++)
                        {
                            DataTable dttdcou = LinQBaseDao.Query("select count(*) from View_LEDShow_zj where 通道名称='" + datasettongdao.Tables[0].Rows[i]["Driveway_Name"].ToString() + "' and 通行状态='排队中' and 车辆类型='" + carType_Name + "' and 排队号 !=''").Tables[0];
                            if (dttdcou.Rows.Count > 0)
                            {
                                tongDao = datasettongdao.Tables[0].Rows[i]["Driveway_Name"].ToString();
                            }
                        }
                    }
                    else
                    {
                        tongDao = datasettongdao.Tables[0].Rows[0]["Driveway_Name"].ToString();
                    }
                    if (tongDao == "")
                    {
                        tongDao = datasettongdao.Tables[0].Rows[0]["Driveway_Name"].ToString();
                    }
                    //在通道有的情况下才绑定类型与通道号，否则就跳出方法
                    lbltongdao.Text = tongDao;
                    LblCarType.Text = carType_Name;
                }
                else
                {
                    return;
                }

                //绑定通行中的车辆信息
                string str = pLED.PositionLED_Content.Insert(pLED.PositionLED_Content.IndexOf("from"), ",SortNumberInfo_ID ");
                string carSql = str + " where 车辆类型='" + carType_Name + "' and 通行状态='待通行' and 通道类型='进' and SortNumberInfo_State='启动' and 通道名称='" + tongDao + "' and SortNumberInfo_PositionValue= '" + SystemClass.PosistionValue + "' and 排队号<>''   order by SortNumberInfo_ID";
                DataSet dscartx = PositionLEDDAL.GetCarType(carSql);
                int n = dscartx.Tables[0].Rows.Count;

                int dscartxCount = int.Parse(pLED.PositionLED_Count.ToString());//设置显示数据条数
                //如果查询出了存在正在通行的车辆，就绑定一个通行中的车辆
                if (dscartx.Tables[0].Rows.Count > 0)
                {
                    label1.ForeColor = Color.Green; label6.ForeColor = Color.Green; label11.ForeColor = Color.Green;

                    label1.Text = "待通行"; label6.Text = dscartx.Tables[0].Rows[0][0].ToString(); label11.Text = dscartx.Tables[0].Rows[0][1].ToString();

                    dscartxCount = dscartxCount - 1;
                }

                //绑定待通行车辆
                carSql = str + " where 车辆类型='" + carType_Name + "'  and  通行状态='排队中'  and  通道类型='进'  and  SortNumberInfo_State='启动' and  通道名称='" + tongDao + "' and  SortNumberInfo_PositionValue = '" + SystemClass.PosistionValue + "' and  排队号<>''  order by SortNumberInfo_ID";
                DataSet dscar = PositionLEDDAL.GetCarType(carSql);
                int m = dscar.Tables[0].Rows.Count;

                #region 跟据显示的条数进行启用或者是禁用LABEL
                //此处是绑定待通行的车辆
                if (dscar.Tables[0].Rows.Count > 0)
                {
                    if (label1.Text == "")//如果没有通行中的车辆，就绑定5个全为待通行
                    {
                        //循环修改颜色
                        foreach (Control item in this.Controls)
                        {
                            if (item is Label && item.Name != "LblCarType" && item.Name != "lbltongdao")
                            {
                                item.ForeColor = Color.FromName(yanse);
                            }
                        }

                        if (dscar.Tables[0].Rows.Count == 1 && (dscartxCount == 1 || dscartxCount >= 1))
                        {
                            label1.Text = "排队中"; label6.Text = dscar.Tables[0].Rows[0][0].ToString(); label11.Text = dscar.Tables[0].Rows[0][1].ToString();
                        }
                        else if (dscar.Tables[0].Rows.Count == 2 && (dscartxCount == 2 || dscartxCount >= 2))
                        {
                            label1.Text = "排队中";
                            label6.Text = dscar.Tables[0].Rows[0][0].ToString(); label11.Text = dscar.Tables[0].Rows[0][1].ToString();
                            label7.Text = dscar.Tables[0].Rows[1][0].ToString(); label12.Text = dscar.Tables[0].Rows[1][1].ToString();
                        }
                        else if (dscar.Tables[0].Rows.Count == 3 && (dscartxCount == 3 || dscartxCount >= 3))
                        {
                            label1.Text = "排队中";
                            label6.Text = dscar.Tables[0].Rows[0][0].ToString(); label11.Text = dscar.Tables[0].Rows[0][1].ToString();
                            label7.Text = dscar.Tables[0].Rows[1][0].ToString(); label12.Text = dscar.Tables[0].Rows[1][1].ToString();
                            label8.Text = dscar.Tables[0].Rows[2][0].ToString(); label13.Text = dscar.Tables[0].Rows[2][1].ToString();
                        }
                        else if (dscar.Tables[0].Rows.Count == 4 && (dscartxCount == 4 || dscartxCount >= 4))
                        {
                            label1.Text = "排队中";
                            label6.Text = dscar.Tables[0].Rows[0][0].ToString(); label11.Text = dscar.Tables[0].Rows[0][1].ToString();
                            label7.Text = dscar.Tables[0].Rows[1][0].ToString(); label12.Text = dscar.Tables[0].Rows[1][1].ToString();
                            label8.Text = dscar.Tables[0].Rows[2][0].ToString(); label13.Text = dscar.Tables[0].Rows[2][1].ToString();
                            label9.Text = dscar.Tables[0].Rows[3][0].ToString(); label14.Text = dscar.Tables[0].Rows[3][1].ToString();
                        }
                        else if (dscar.Tables[0].Rows.Count >= 5 && dscartxCount == 5)
                        {
                            label1.Text = "排队中";
                            label6.Text = dscar.Tables[0].Rows[0][0].ToString(); label11.Text = dscar.Tables[0].Rows[0][1].ToString();
                            label7.Text = dscar.Tables[0].Rows[1][0].ToString(); label12.Text = dscar.Tables[0].Rows[1][1].ToString();
                            label8.Text = dscar.Tables[0].Rows[2][0].ToString(); label13.Text = dscar.Tables[0].Rows[2][1].ToString();
                            label9.Text = dscar.Tables[0].Rows[3][0].ToString(); label14.Text = dscar.Tables[0].Rows[3][1].ToString();
                            label10.Text = dscar.Tables[0].Rows[4][0].ToString(); label15.Text = dscar.Tables[0].Rows[4][1].ToString();
                        }

                    }
                    else
                    {
                        //循环修改颜色
                        foreach (Control item in this.Controls)
                        {
                            if (item is Label && item.Name != "LblCarType" && item.Name != "lbltongdao" && item.Name != "label7" && item.Name != "label12" && item.Name != "label1" && item.Name != "label6" && item.Name != "label11")
                            {
                                item.ForeColor = Color.FromName(yanse);
                            }
                        }
                        //有通行中的车辆已绑定则绑定待通行
                        if (dscar.Tables[0].Rows.Count == 1 && (dscartxCount == 1 || dscartxCount >= 1))
                        {
                            label2.Text = "排队中";
                            label7.Text = dscar.Tables[0].Rows[0][0].ToString(); label12.Text = dscar.Tables[0].Rows[0][1].ToString();
                        }
                        if (dscar.Tables[0].Rows.Count == 2 && (dscartxCount == 2 || dscartxCount >= 2))
                        {
                            label2.Text = "排队中";
                            label7.Text = dscar.Tables[0].Rows[0][0].ToString(); label12.Text = dscar.Tables[0].Rows[0][1].ToString();
                            label8.Text = dscar.Tables[0].Rows[1][0].ToString(); label13.Text = dscar.Tables[0].Rows[1][1].ToString();
                        }
                        else if (dscar.Tables[0].Rows.Count == 3 && (dscartxCount == 3 || dscartxCount >= 3))
                        {
                            label2.Text = "排队中";
                            label7.Text = dscar.Tables[0].Rows[0][0].ToString(); label12.Text = dscar.Tables[0].Rows[0][1].ToString();
                            label8.Text = dscar.Tables[0].Rows[1][0].ToString(); label13.Text = dscar.Tables[0].Rows[1][1].ToString();
                            label9.Text = dscar.Tables[0].Rows[2][0].ToString(); label14.Text = dscar.Tables[0].Rows[2][1].ToString();
                        }
                        else if (dscar.Tables[0].Rows.Count >= 4 && dscartxCount >= 4)
                        {
                            label2.Text = "排队中";
                            label7.Text = dscar.Tables[0].Rows[0][0].ToString(); label12.Text = dscar.Tables[0].Rows[0][1].ToString();
                            label8.Text = dscar.Tables[0].Rows[1][0].ToString(); label13.Text = dscar.Tables[0].Rows[1][1].ToString();
                            label9.Text = dscar.Tables[0].Rows[2][0].ToString(); label14.Text = dscar.Tables[0].Rows[2][1].ToString();
                            label10.Text = dscar.Tables[0].Rows[3][0].ToString(); label15.Text = dscar.Tables[0].Rows[3][1].ToString();
                        }
                    }
                }
                #endregion

                //控制类型与通道的循环变量
                count = count + 1;
                if (count == type)
                {
                    count = 0;
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("LEDPreviewForm BindType()");
            }
        }

        //闹钟循环
        private void TimerLED_Tick(object sender, EventArgs e)
        {
            try
            {
                lbltongdao.Text = "";
                LblCarType.Text = "";
                label1.Text = ""; label6.Text = ""; label11.Text = "";
                label2.Text = ""; label7.Text = ""; label12.Text = "";
                label3.Text = ""; label8.Text = ""; label13.Text = "";
                label4.Text = ""; label9.Text = ""; label14.Text = "";
                label5.Text = ""; label10.Text = ""; label15.Text = "";

                BindCount();
                BindType();
            }
            catch
            {
                CommonalityEntity.WriteTextLog("LEDPreviewForm TimerLED_Tick()" );
                count = count + 1;
                if (count == type)
                {
                    count = 0;
                }
                BindType();
            }
        }

        ////第二个闹钟，循环查询当前门岗有无车辆通行信息
        //private void timer1_Tick(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //查询车辆类型
        //        dataset = LinQBaseDao.Query("select CarType_Name,CarType_ID,CarType_Value from CarType where CarType_Value in( select distinct (SortNumberInfo_CarTypeValue)  from SortNumberInfo where SortNumberInfo_PositionValue='" + SystemClass.PosistionValue + "' and SortNumberInfo_SortValue<>'' and SortNumberInfo_CarTypeValue in (select CarType_Value from CarType where CarType_ID not in (" + cartypeid + ")))");

        //        type = dataset.Tables[0].Rows.Count;
        //        LEDPreviewForm_Load(null, null);
        //    }
        //    catch 
        //    {
        //        CommonalityEntity.WriteTextLog("LEDPreviewform BindCount()" + "");
        //    }
        //}
    }
}
