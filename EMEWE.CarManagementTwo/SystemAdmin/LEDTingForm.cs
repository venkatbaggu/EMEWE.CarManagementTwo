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
    public partial class LEDTingForm : Form
    {
        PositionLED pled = new PositionLED();
        public int typeting = 0;//储存车辆类型有几个
        public int typecountting = 0;//车辆类型循环标识

        public DataSet saTypeting;//储存类型

        int CheLiangCountting = 0;//查出来的数据有几条
        public DataSet dscarting;//查询到的数据

        public string yanse;//截取配置的显示颜色

        private string cartypeid = "";
        int fontsize = 40;

        public LEDTingForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LEDPreviewForm_Load(object sender, EventArgs e)
        {
            try
            {


                DataTable dts = LinQBaseDao.Query("select CarType_ID,CarType_DriSName from CarType where CarType_State='启动'").Tables[0];
                if (dts.Rows.Count > 0)
                {
                    for (int i = 0; i < dts.Rows.Count; i++)
                    {
                        int kk = Convert.ToInt32(LinQBaseDao.GetSingle("select count(0) from  ManagementStrategy where ManagementStrategy_DriSName='" + dts.Rows[i][1].ToString() + "' and ManagementStrategy_ControlInfo_ID in(select ControlInfo_ID from ControlInfo where ControlInfo_Name='排队') and ManagementStrategy_State='启动' ").ToString());
                        if (kk == 0)
                        {
                            cartypeid += dts.Rows[i][0].ToString() + ",";
                        }
                    }
                }
                cartypeid = cartypeid.TrimEnd(',');

                //读取LED配置项
                string sql = "select * from PositionLED where PositionLED_State='启动' and PositionLED_Position_ID=" + SystemClass.PositionID + "";
                pled = PositionLEDDAL.GetLED(sql);

                //加载时将窗体显示位置修改到指定位置
                int X = Convert.ToInt32(pled.PositionLED_X);
                int Y = Convert.ToInt32(pled.PositionLED_Y);
                this.Location = new Point(X, Y);
                fontsize = Convert.ToInt32(pled.PositionLED_FontSize);

                //判断当前门岗是否有LED的配置信息
                if (pled.PositionLED_ID <= 0)
                {
                    lbltishi.Visible = true;
                    lblleixingting.Visible = false;
                    lblleixingzhi.Visible = false;
                    lblChePaiting.Visible = false;
                    lblpaiduihaoting.Visible = false;
                    lblMenGangting.Visible = false;
                    lblTongDaoting.Visible = false;
                    lbltishi.Text = "当前登录门岗\r\n暂无LED显示配置信息，\r\n请先配置对应门岗显示信息！！！";
                    return;
                }
                else
                {
                    try
                    {
                        //读取配置的高宽（高度与宽度是反的）
                        int weight = Convert.ToInt32(pled.PositionLED_ScreenWeight);
                        int heifht = Convert.ToInt32(pled.PositionLED_ScreenHeight);
                        this.Size = new Size(weight, heifht);

                        //读取配置的字体颜色
                        string s = pled.PositionLED_Color;
                        int cd = s.Length - 8;
                        yanse = s.Substring(7, cd);

                        if (pled.PositionLED_Type == 1)
                        {
                            timer1.Enabled = true;

                            BindCountting();
                            BindTypeting();
                            Bindting();
                        }
                        else if (pled.PositionLED_Type == 2)
                        {
                            lblhuanyingyu.Visible = true;
                            lblleixingting.Visible = false;
                            lblleixingzhi.Visible = false;
                            lblChePaiting.Visible = false;
                            lblpaiduihaoting.Visible = false;
                            lblMenGangting.Visible = false;
                            lblTongDaoting.Visible = false;
                            lblhuanyingyu.Text = pled.PositionLED_Remark.ToString();
                            return;
                        }
                    }
                    catch
                    {
                        //将类型的循环标识加1
                        typecountting = typecountting + 1;
                        if (typecountting >= typeting)
                        {
                            typecountting = 0;
                        }
                        BindTypeting();
                        Bindting();
                    }
                }
            }
            catch 
            {

            }
        }

        //查询出当前启用的车辆类型与启用的门岗有几个
        private void BindCountting()
        {
            saTypeting = LinQBaseDao.Query("select CarType_Name,CarType_ID,CarType_Value from CarType where CarType_Value in( select distinct (SortNumberInfo_CarTypeValue) from SortNumberInfo where SortNumberInfo_SortValue<>'' and SortNumberInfo_CarTypeValue in (select CarType_Value from CarType where CarType_ID not in (" + cartypeid + ")))");
            typeting = saTypeting.Tables[0].Rows.Count;
        }

        //绑定车辆信息
        int numting = 0;//集合比较标识
        public void Bindting()
        {
            if (CheLiangCountting > 0)
            {
                numting = 0;
                if (CheLiangCountting >= 5)
                {
                    label21ting.Text = dscarting.Tables[0].Rows[numting]["排队号"].ToString();
                    label11ting.Text = dscarting.Tables[0].Rows[numting]["Position_Name"].ToString();
                    label6ting.Text = dscarting.Tables[0].Rows[numting]["车牌号"].ToString();
                    label16ting.Text = dscarting.Tables[0].Rows[numting]["通道名称"].ToString();
                    label21ting.Font = new System.Drawing.Font("宋体", Convert.ToInt32(pled.PositionLED_FontSize));
                    label11ting.Font = new System.Drawing.Font("宋体", fontsize);
                    label6ting.Font = new System.Drawing.Font("宋体", fontsize);
                    label16ting.Font = new System.Drawing.Font("宋体", fontsize);

                    label22ting.Text = dscarting.Tables[0].Rows[numting + 1]["排队号"].ToString(); label12ting.Text = dscarting.Tables[0].Rows[numting + 1]["Position_Name"].ToString();
                    label7ting.Text = dscarting.Tables[0].Rows[numting + 1]["车牌号"].ToString(); label17ting.Text = dscarting.Tables[0].Rows[numting + 1]["通道名称"].ToString();
                    label22ting.Font = new System.Drawing.Font("宋体", fontsize); label12ting.Font = new System.Drawing.Font("宋体", fontsize);
                    label7ting.Font = new System.Drawing.Font("宋体", fontsize); label17ting.Font = new System.Drawing.Font("宋体", fontsize);

                    label23ting.Text = dscarting.Tables[0].Rows[numting + 2]["排队号"].ToString(); label13ting.Text = dscarting.Tables[0].Rows[numting + 2]["Position_Name"].ToString();
                    label8ting.Text = dscarting.Tables[0].Rows[numting + 2]["车牌号"].ToString(); label18ting.Text = dscarting.Tables[0].Rows[numting + 2]["通道名称"].ToString();
                    label23ting.Font = new System.Drawing.Font("宋体", fontsize); label13ting.Font = new System.Drawing.Font("宋体", fontsize);
                    label8ting.Font = new System.Drawing.Font("宋体", fontsize); label18ting.Font = new System.Drawing.Font("宋体", fontsize);

                    label24ting.Text = dscarting.Tables[0].Rows[numting + 3]["排队号"].ToString(); label14ting.Text = dscarting.Tables[0].Rows[numting + 3]["Position_Name"].ToString();
                    label9ting.Text = dscarting.Tables[0].Rows[numting + 3]["车牌号"].ToString(); label19ting.Text = dscarting.Tables[0].Rows[numting + 3]["通道名称"].ToString();
                    label24ting.Font = new System.Drawing.Font("宋体", fontsize); label14ting.Font = new System.Drawing.Font("宋体", fontsize);
                    label9ting.Font = new System.Drawing.Font("宋体", fontsize); label19ting.Font = new System.Drawing.Font("宋体", fontsize);

                    label25ting.Text = dscarting.Tables[0].Rows[numting + 4]["排队号"].ToString(); label15ting.Text = dscarting.Tables[0].Rows[numting + 4]["Position_Name"].ToString();
                    label10ting.Text = dscarting.Tables[0].Rows[numting + 4]["车牌号"].ToString(); label20tingting.Text = dscarting.Tables[0].Rows[numting + 4]["通道名称"].ToString();
                    label25ting.Font = new System.Drawing.Font("宋体", fontsize); label15ting.Font = new System.Drawing.Font("宋体", fontsize);
                    label10ting.Font = new System.Drawing.Font("宋体", fontsize); label20tingting.Font = new System.Drawing.Font("宋体", fontsize);

                    CheLiangCountting -= 5;
                    numting += 5;
                }
                else if (CheLiangCountting == 4)
                {
                    label21ting.Text = dscarting.Tables[0].Rows[numting]["排队号"].ToString(); label11ting.Text = dscarting.Tables[0].Rows[numting]["Position_Name"].ToString();
                    label6ting.Text = dscarting.Tables[0].Rows[numting]["车牌号"].ToString(); label16ting.Text = dscarting.Tables[0].Rows[numting]["通道名称"].ToString();
                    label21ting.Font = new System.Drawing.Font("宋体", fontsize); label11ting.Font = new System.Drawing.Font("宋体", fontsize);
                    label6ting.Font = new System.Drawing.Font("宋体", fontsize); label16ting.Font = new System.Drawing.Font("宋体", fontsize);

                    label22ting.Text = dscarting.Tables[0].Rows[numting + 1]["排队号"].ToString(); label12ting.Text = dscarting.Tables[0].Rows[numting + 1]["Position_Name"].ToString();
                    label7ting.Text = dscarting.Tables[0].Rows[numting + 1]["车牌号"].ToString(); label17ting.Text = dscarting.Tables[0].Rows[numting + 1]["通道名称"].ToString();
                    label22ting.Font = new System.Drawing.Font("宋体", fontsize); label12ting.Font = new System.Drawing.Font("宋体", fontsize);
                    label7ting.Font = new System.Drawing.Font("宋体", fontsize); label17ting.Font = new System.Drawing.Font("宋体", fontsize);

                    label23ting.Text = dscarting.Tables[0].Rows[numting + 2]["排队号"].ToString(); label13ting.Text = dscarting.Tables[0].Rows[numting + 2]["Position_Name"].ToString();
                    label8ting.Text = dscarting.Tables[0].Rows[numting + 2]["车牌号"].ToString(); label18ting.Text = dscarting.Tables[0].Rows[numting + 2]["通道名称"].ToString();
                    label23ting.Font = new System.Drawing.Font("宋体", fontsize); label13ting.Font = new System.Drawing.Font("宋体", fontsize);
                    label8ting.Font = new System.Drawing.Font("宋体", fontsize); label18ting.Font = new System.Drawing.Font("宋体", fontsize);

                    label24ting.Text = dscarting.Tables[0].Rows[numting + 3]["排队号"].ToString(); label14ting.Text = dscarting.Tables[0].Rows[numting + 3]["Position_Name"].ToString();
                    label9ting.Text = dscarting.Tables[0].Rows[numting + 3]["车牌号"].ToString(); label19ting.Text = dscarting.Tables[0].Rows[numting + 3]["通道名称"].ToString();
                    label24ting.Font = new System.Drawing.Font("宋体", fontsize); label14ting.Font = new System.Drawing.Font("宋体", fontsize);
                    label9ting.Font = new System.Drawing.Font("宋体", fontsize); label19ting.Font = new System.Drawing.Font("宋体", fontsize);

                    CheLiangCountting -= 4;
                    numting += 4;
                }
                else if (CheLiangCountting == 3)
                {
                    label21ting.Text = dscarting.Tables[0].Rows[numting]["排队号"].ToString(); label11ting.Text = dscarting.Tables[0].Rows[numting]["Position_Name"].ToString();
                    label6ting.Text = dscarting.Tables[0].Rows[numting]["车牌号"].ToString(); label16ting.Text = dscarting.Tables[0].Rows[numting]["通道名称"].ToString();
                    label21ting.Font = new System.Drawing.Font("宋体", fontsize); label11ting.Font = new System.Drawing.Font("宋体", fontsize);
                    label6ting.Font = new System.Drawing.Font("宋体", fontsize); label16ting.Font = new System.Drawing.Font("宋体", fontsize);

                    label22ting.Text = dscarting.Tables[0].Rows[numting + 1]["排队号"].ToString(); label12ting.Text = dscarting.Tables[0].Rows[numting + 1]["Position_Name"].ToString();
                    label7ting.Text = dscarting.Tables[0].Rows[numting + 1]["车牌号"].ToString(); label17ting.Text = dscarting.Tables[0].Rows[numting + 1]["通道名称"].ToString();
                    label22ting.Font = new System.Drawing.Font("宋体", fontsize); label12ting.Font = new System.Drawing.Font("宋体", fontsize);
                    label7ting.Font = new System.Drawing.Font("宋体", fontsize); label17ting.Font = new System.Drawing.Font("宋体", fontsize);

                    label23ting.Text = dscarting.Tables[0].Rows[numting + 2]["排队号"].ToString(); label13ting.Text = dscarting.Tables[0].Rows[numting + 2]["Position_Name"].ToString();
                    label8ting.Text = dscarting.Tables[0].Rows[numting + 2]["车牌号"].ToString(); label18ting.Text = dscarting.Tables[0].Rows[numting + 2]["通道名称"].ToString();
                    label23ting.Font = new System.Drawing.Font("宋体", fontsize); label13ting.Font = new System.Drawing.Font("宋体", fontsize);
                    label8ting.Font = new System.Drawing.Font("宋体", fontsize); label18ting.Font = new System.Drawing.Font("宋体", fontsize);
                    CheLiangCountting -= 3;
                    numting += 3;
                }
                else if (CheLiangCountting == 2)
                {
                    label21ting.Text = dscarting.Tables[0].Rows[numting]["排队号"].ToString(); label11ting.Text = dscarting.Tables[0].Rows[numting]["Position_Name"].ToString();
                    label6ting.Text = dscarting.Tables[0].Rows[numting]["车牌号"].ToString(); label16ting.Text = dscarting.Tables[0].Rows[numting]["通道名称"].ToString();
                    label21ting.Font = new System.Drawing.Font("宋体", fontsize); label11ting.Font = new System.Drawing.Font("宋体", fontsize);
                    label6ting.Font = new System.Drawing.Font("宋体", fontsize); label16ting.Font = new System.Drawing.Font("宋体", fontsize);

                    label22ting.Text = dscarting.Tables[0].Rows[numting + 1]["排队号"].ToString(); label12ting.Text = dscarting.Tables[0].Rows[numting + 1]["Position_Name"].ToString();
                    label7ting.Text = dscarting.Tables[0].Rows[numting + 1]["车牌号"].ToString(); label17ting.Text = dscarting.Tables[0].Rows[numting + 1]["通道名称"].ToString();
                    label22ting.Font = new System.Drawing.Font("宋体", fontsize); label12ting.Font = new System.Drawing.Font("宋体", fontsize);
                    label7ting.Font = new System.Drawing.Font("宋体", fontsize); label17ting.Font = new System.Drawing.Font("宋体", fontsize);
                    CheLiangCountting -= 2;
                    numting += 2;
                }
                else if (CheLiangCountting == 1)
                {
                    label21ting.Text = dscarting.Tables[0].Rows[numting]["排队号"].ToString(); label11ting.Text = dscarting.Tables[0].Rows[numting]["Position_Name"].ToString();
                    label6ting.Text = dscarting.Tables[0].Rows[numting]["车牌号"].ToString(); label16ting.Text = dscarting.Tables[0].Rows[numting]["通道名称"].ToString();
                    label21ting.Font = new System.Drawing.Font("宋体", fontsize); label11ting.Font = new System.Drawing.Font("宋体", fontsize);
                    label6ting.Font = new System.Drawing.Font("宋体", fontsize); label16ting.Font = new System.Drawing.Font("宋体", fontsize);

                    CheLiangCountting -= 1;
                    numting += 1;
                }
            }
            //将类型的循环标识加1
            typecountting = typecountting + 1;
            if (typecountting >= typeting)
            {
                typecountting = 0;
                numting = 0;
            }
        }

        //按条件查询车辆信息
        private void BindTypeting()
        {
            try
            {


                lblleixingzhi.Font = new System.Drawing.Font("宋体", fontsize + 3);
                lblleixingting.Font = new System.Drawing.Font("宋体", fontsize + 3);
                lblpaiduihaoting.Font = new System.Drawing.Font("宋体", fontsize);
                lblChePaiting.Font = new System.Drawing.Font("宋体", fontsize);
                lblMenGangting.Font = new System.Drawing.Font("宋体", fontsize);
                lblTongDaoting.Font = new System.Drawing.Font("宋体", fontsize);
                if (typeting > 0)
                {
                    if (saTypeting.Tables[0].Rows.Count == 1)
                    {
                        dscarting = LinQBaseDao.Query("Select top (5) 车牌号,Position_Name,通道名称,排队号,SortNumberInfo_ID from View_LEDShow_zj where 车辆类型='" + saTypeting.Tables[0].Rows[0]["CarType_Name"] + "' and 通行状态='排队中' and SortNumberInfo_State='启动' and 排队号 is not null order by SortNumberInfo_ID");
                    }
                    else
                    {
                        dscarting = LinQBaseDao.Query("Select top (5) 车牌号,Position_Name,通道名称,排队号,SortNumberInfo_ID from View_LEDShow_zj where 车辆类型='" + saTypeting.Tables[0].Rows[typecountting]["CarType_Name"] + "' and 通行状态='排队中' and SortNumberInfo_State='启动' and 排队号 is not null order by SortNumberInfo_ID");

                    }
                    if (dscarting.Tables[0].Rows.Count > 0)
                    {
                        CheLiangCountting = dscarting.Tables[0].Rows.Count;
                        lblleixingzhi.Text = saTypeting.Tables[0].Rows[typecountting]["CarType_Name"].ToString();

                    }
                }

            }
            catch
            {
                CommonalityEntity.WriteTextLog(" LEDTingForm.BindTypeting异常：" );
            }


        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                foreach (Control ctl in this.Controls)
                {
                    if (ctl is Label)
                    {
                        if (ctl.Name != "lblCarTypeting" && ctl.Name != "lblChePaiting" && ctl.Name != "lblMenGangting" && ctl.Name != "lblTongDaoting" && ctl.Name != "lblpaiduihaoting" && ctl.Name != "lblleixingting" && ctl.Name != "lblleixingzhiting")
                        {
                            ctl.Text = "";
                        }
                    }
                }
                if (CheLiangCountting == 0)
                {
                    BindCountting();
                    BindTypeting();
                    Bindting();
                }
                else
                {
                    Bindting();
                }
            }
            catch
            {
                //将类型的循环标识加1
                typecountting = typecountting + 1;
                if (typecountting >= typeting)
                {
                    typecountting = 0;
                }
                BindTypeting();
                Bindting();
            }
        }
    }
}
