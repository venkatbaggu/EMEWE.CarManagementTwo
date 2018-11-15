using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.HelpClass;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class UnLoadSmallDiscern : Form
    {
        public UnLoadSmallDiscern()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 执行管控验证
        /// </summary>
        private CheckProperties checkPr = new CheckProperties();
        public string sql = "";
        public string abnormal = "";

        string WeightInfo_ID = null;
        string WeightStrategy_ID = null;
        /// <summary>
        /// 根据IC卡(小票)号取出车辆信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRecognition_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            strsql();
            if (sql == "")
            {
                MessageBox.Show("没有该IC卡(小票)号的信息！");
                return;
            }
            DataTable ds = LinQBaseDao.Query(sql).Tables[0];
            if (ds.Rows.Count < 1)
            {
                Empty();
                return;
            }

            txtInWeight.Text = "";
            txtOutWeight.Text = "";
            txtCartype_Name.Text = ds.Rows[0]["CarType_Name"].ToString();
            txtCartype_Name.Tag = ds.Rows[0]["CarType_ID"].ToString();
            txtCarregistration_Customer.Text = ds.Rows[0]["CustomerInfo_Name"].ToString();
            txtCarregistration_Name.Text = ds.Rows[0]["StaffInfo_Name"].ToString();
            txtCarregistration_Name.Tag = ds.Rows[0]["StaffInfo_ID"].ToString();
            txtCarregistration_Carnumber.Text = ds.Rows[0]["CarInfo_Name"].ToString();
            txtCarregistration_Carnumber.Tag = ds.Rows[0]["CarInfo_ID"].ToString();
            CommonalityEntity.CarInfo_ID = ds.Rows[0]["CarInfo_ID"].ToString();
            // txtCarregistration_License.Text = ds.Rows[0]["StaffInfo_License"].ToString();
            txtCarregistration_Phone.Text = ds.Rows[0]["StaffInfo_Phone"].ToString();
            txtCarregistration_IdentityCard.Text = ds.Rows[0]["StaffInfo_Identity"].ToString();
            txtCarregistration_Carriage.Text = ds.Rows[0]["CarInfo_Carriage"].ToString();
            txtCarregistration_Weight.Text = ds.Rows[0]["CarInfo_Weight"].ToString();
            txtCarregistration_Height.Text = ds.Rows[0]["CarInfo_Height"].ToString();
            if (Convert.ToBoolean(ds.Rows[0]["CarInfo_Bail"].ToString()) == true)
            {
                ckbCarInfo_Bail.Checked = true;
            }
            else
            {
                ckbCarInfo_Bail.Checked = false;
            }
            txtOperateTime.Text = ds.Rows[0]["CarInfo_Time"].ToString();
            txtOperateUser.Text = ds.Rows[0]["CarInfo_Operate"].ToString();
            txtRemark.Text = ds.Rows[0]["CarInfo_Remark"].ToString();

            Abnormal();
            txtExecPtion.Text = abnormal;
            if (ISFill(ds) == false)
            {
                return;
            }

            ///////////////
            string caroutid = ds.Rows[0]["CarInOutRecord_ID"].ToString();
            if (ds.Rows.Count > 0)
            {
                DataTable dt = LinQBaseDao.Query("select * from BusinessRecord where BusinessRecord_CarInOutRecord_ID=" + caroutid + "").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string type = dt.Rows[i]["BusinessRecord_Type"].ToString();
                        if (type == CommonalityEntity.unloadFirstWeight)
                        {
                            txtInWeight.Text = dt.Rows[i]["BusinessRecord_Weight"].ToString();
                        }
                        if (type == CommonalityEntity.unloadSecondWeight)
                        {
                            txtOutWeight.Text = dt.Rows[i]["BusinessRecord_Weight"].ToString();
                        }
                    }
                }
            }
            string sqll = "Select * from CarPic where CarPic_CarInfo_ID=" + ds.Rows[0]["CarInfo_ID"].ToString();
            List<CarPic> list = new List<CarPic>();
            list = LinQBaseDao.GetItemsForListing<CarPic>(sqll).ToList();
            int k = 0;
            string path = SystemClass.SaveFile;
            foreach (var pathStr in list)
            {
                if (k < 7)
                {
                    PictureBox pb = new PictureBox();
                    int x = (20 + 165 * k);
                    int y = 20;
                    pb.Location = new Point(x, y);
                    pb.Width = 145;
                    pb.Height = 160;
                    pb.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage; ;
                    pb.MouseHover += new System.EventHandler(this.pbInImageOne_MouseHover);
                    pb.MouseLeave += new System.EventHandler(this.pbInImageOne_MouseLeave);
                    pb.Name = "pictureBox" + (k + 1).ToString();
                    pb.Tag = path + pathStr.CarPic_Add.ToString();
                    pb.ImageLocation = path + pathStr.CarPic_Add.ToString();
                    this.GroBoxContext.Controls.Add(pb);
                    k++;
                }
            }
            btnSave.Enabled = true;
        }

        /// <summary>
        /// 对地磅策略判断   该磅房信息
        /// </summary>
        private bool WeighStrategyIf()
        {
            try
            {
                string WeighStrategy_ID = cobWeighStrategyRecord.SelectedValue.ToString();//地磅编号
                string WeighStrategy_Name = cobWeighStrategyRecord.Text.ToString();//地磅名称
                int indexs = 0;
                int mark = 0;
                DataSet Ifds = LinQBaseDao.Query("select * from View_WeighStrategy where WeighStrategy_CarType_ID=" + txtCartype_Name.Tag.ToString() + " and WeighStrategy_State='启动'");
                for (int i = 0; i < Ifds.Tables[0].Rows.Count; i++)
                {
                    if (Convert.ToInt32(Ifds.Tables[0].Rows[i]["WeighStrategy_Sort"]) == 1)
                    {
                        mark++;
                    }
                }
                if (mark == Ifds.Tables[0].Rows.Count)
                {
                    #region 此乃无序
                    //查是否有此策略
                    string Sql = "select * from View_WeighStrategy where CarType_Name='" + txtCartype_Name.Text.Trim() + "' and WeighStrategy_State='启动'";
                    DataSet dst = LinQBaseDao.Query(Sql);
                    if (dst.Tables[0].Rows.Count == 0)
                    {
                        MessageBox.Show("改类型车未配置地磅策略");
                        return false;
                    }
                    WeightInfo_ID = null;
                    WeightStrategy_ID = null;
                    string CarType_Name = null;//车辆类型
                    string Sort = "";//排序号
                    for (int i = 0; i < dst.Tables[0].Rows.Count; i++)
                    {
                        WeightInfo_ID += dst.Tables[0].Rows[i]["WeighInfo_ID"] + ",";
                        WeightStrategy_ID += dst.Tables[0].Rows[i]["WeighStrategy_ID"] + ",";
                        CarType_Name += dst.Tables[0].Rows[i]["CarType_Name"] + ",";
                        Sort += dst.Tables[0].Rows[i]["WeighStrategy_Sort"] + ",";
                    }

                    string[] CarType_Names = CarType_Name.Split(',');
                    string[] infoId = WeightInfo_ID.Split(',');
                    string[] ID = WeightStrategy_ID.Split(',');
                    string[] Car_Sort = Sort.Split(',');
                    for (int i = 0; i < infoId.Count() - 1; i++)
                    {
                        if (infoId[i] == WeighStrategy_ID)//进入说明存在此策略
                        {
                            //此策略是否已通过
                            string SqlRecord = "select * from view_WeighStrategyRecord where WeighStrategyRecord_WeighInfo_ID=" + cobWeighStrategyRecord.SelectedValue.ToString() + " and WeighStrategyRecord_CarType='" + txtCartype_Name.Text.Trim() + "'";
                            DataSet ds = LinQBaseDao.Query(SqlRecord);
                            if (ds.Tables[0].Rows.Count > 0)//判断前面是否有数据，没有说明未正常通行
                            {
                                MessageBox.Show("该车辆未按正常地磅策略顺序通行(此地磅已验过，请更换地磅)！");
                                return false;
                            }
                            indexs++;
                        }
                    }
                    if (indexs == 0)
                    {
                        MessageBox.Show("无此策略，该车未正常通行");
                        return false;
                    }
                    return true;
                    #endregion
                }
                else
                {
                    #region 有序策略查询
                    string Sql = "select * from View_WeighStrategy where CarType_Name='" + txtCartype_Name.Text.Trim() + "' order by WeighStrategy_Sort asc";
                    DataSet dst = LinQBaseDao.Query(Sql);
                    WeightInfo_ID = null;
                    WeightStrategy_ID = null;
                    string CarType_Name = null;//车辆类型
                    string Sort = "";//排序号
                    for (int i = 0; i < dst.Tables[0].Rows.Count; i++)
                    {
                        WeightInfo_ID += dst.Tables[0].Rows[i]["WeighInfo_ID"] + ",";
                        WeightStrategy_ID += dst.Tables[0].Rows[i]["WeighStrategy_ID"] + ",";
                        CarType_Name += dst.Tables[0].Rows[i]["CarType_Name"] + ",";
                        Sort += dst.Tables[0].Rows[i]["WeighStrategy_Sort"] + ",";
                    }

                    string[] CarType_Names = CarType_Name.Split(',');
                    string[] infoId = WeightInfo_ID.Split(',');
                    string[] ID = WeightStrategy_ID.Split(',');
                    string[] Car_Sort = Sort.Split(',');
                    int index = 0;
                    for (int i = 0; i < infoId.Count() - 1; i++)
                    {
                        if (infoId[i] == WeighStrategy_ID)//进入说明存在此策略
                        {
                            #region 地磅策略判断
                            if (i > 0)//此磅房至少为2号磅房
                            {
                                //查询前面一个地磅策略是否通过 排序号 车辆类型名称   txtCarregistration_Carnumber.Tag.ToString()
                                string MyCarType = CarType_Names[i - 1];
                                string MySort = Car_Sort[i - 1];
                                RraderSort = MySort;
                                string SqlRecord = "select * from view_WeighStrategyRecord where WeighStrategyRecord_StaffInfo_ID=" + txtCarregistration_Name.Tag.ToString() + " and WeighInfo_ID=" + infoId[i - 1] + " and WeighStrategyRecord_CarType='" + MyCarType + "' and WeighStrategyRecord_CarInfo_ID=" + txtCarregistration_Carnumber.Tag.ToString();
                                DataSet ds = LinQBaseDao.Query(SqlRecord);
                                if (ds.Tables[0].Rows.Count > 0)//判断前面是否有数据，没有说明未正常通行
                                {
                                    if (!(bool)(ds.Tables[0].Rows[0]["WeighStrategyRecord_mark"]))//判断标识是否通过
                                    {
                                        MessageBox.Show("该车辆未按正常地磅策略顺序通行！");
                                        return false;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("未按地磅策略正常通行");
                                    return false;
                                }

                                //判断当前地磅策略是否执行
                                string Sqls = "select * from view_WeighStrategyRecord where WeighStrategyRecord_StaffInfo_ID=" + txtCarregistration_Name.Tag.ToString() + " and WeighInfo_ID=" + infoId[i] + " and WeighStrategyRecord_CarType='" + MyCarType + "'and WeighStrategyRecord_CarInfo_ID=" + txtCarregistration_Carnumber.Tag.ToString();
                                DataSet dsts = LinQBaseDao.Query(Sqls);
                                if (dsts.Tables[0].Rows.Count > 0)//判断前面是否有数据，有说明未正常通行
                                {
                                    MessageBox.Show("此磅房策略以通行，请更换磅房名");
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                //第一次进入，判断是否选择第一次进入磅房，查询有数据则异常
                                string SqlRecord = "select * from view_WeighStrategyRecord where WeighStrategyRecord_StaffInfo_ID=" + txtCarregistration_Name.Tag.ToString() + " and WeighStrategyRecord_CarType='" + txtCartype_Name.Text.Trim() + "'and WeighStrategyRecord_CarInfo_ID=" + txtCarregistration_Carnumber.Tag.ToString();
                                DataSet ds = LinQBaseDao.Query(SqlRecord);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    MessageBox.Show("此磅房验证已验过,请更换地磅房");
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }
                            #endregion
                            index++;
                        }
                    }
                    if (index == 0)//没有此策略地磅
                    {
                        MessageBox.Show("未按正常策略通行");
                        return false;
                    }
                    return true;
                    #endregion
                }
            }
            catch
            {
                Console.WriteLine("");
                return false;
            }
        }

        /// <summary>
        /// 策略序号记录
        /// </summary>
        string RraderSort = null;

        WeighStrategyRecord record = null;
        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string txtserialnumber = txtSerialnumber.Text.ToString().Trim();
                string txtinweight = txtInWeight.Text.ToString().Trim();
                string txtoutweight = txtOutWeight.Text.ToString().Trim();
                strsql();
                if (sql == "")
                {
                    MessageBox.Show("没有该IC卡(小票)号的信息！");
                    return;
                }
                DataTable table = LinQBaseDao.Query(sql).Tables[0];
                if (table.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(table.Rows[0]["CarInOutRecord_ISFulfill"].ToString()))
                    {
                        MessageBox.Show("该车辆通行已完成！");
                        return;
                    }
                    if (ISFill(table) == false)
                    {
                        return;
                    }

                    string carinoutrecord = table.Rows[0]["CarInOutRecord_ID"].ToString();
                    string weight1 = "";
                    string weight2 = "";
                    string type = "";
                    DataTable tableB = LinQBaseDao.Query("select * from BusinessRecord where BusinessRecord_CarInOutRecord_ID=" + carinoutrecord).Tables[0];
                    for (int i = 0; i < tableB.Rows.Count; i++)
                    {
                        if (tableB.Rows[i]["BusinessRecord_Type"].ToString() == CommonalityEntity.unloadFirstWeight)
                        {
                            weight1 = tableB.Rows[i]["BusinessRecord_Weight"].ToString();
                        }
                        if (tableB.Rows[i]["BusinessRecord_Type"].ToString() == CommonalityEntity.unloadSecondWeight)
                        {
                            weight2 = tableB.Rows[i]["BusinessRecord_Weight"].ToString();
                        }
                        if (tableB.Rows[i]["BusinessRecord_Type"].ToString() == CommonalityEntity.upWeight)
                        {
                            type = tableB.Rows[i]["BusinessRecord_Type"].ToString();
                        }
                    }
                    //对地磅策略判断   该磅房信息
                    if (Convert.ToInt32(cobWeighStrategyRecord.SelectedValue) < 1)
                    {
                        MessageBox.Show("请选择此磅房名称");
                        return;
                    }
                    if (!WeighStrategyIf())
                    {
                        return;
                    }
                    //判断进门地磅是否重复记录
                    if (txtinweight != "" && txtinweight != weight1)
                    {
                        DataTable ds = LinQBaseDao.Query("select * from BusinessRecord where BusinessRecord_Type='" + CommonalityEntity.unloadFirstWeight + "' and BusinessRecord_CarInOutRecord_ID=" + carinoutrecord + " order by BusinessRecord_ID desc").Tables[0];
                        if (ds.Rows.Count > 0)
                        {
                            if (MessageBox.Show("是否修改" + CommonalityEntity.unloadFirstWeight + "重量？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                LinQBaseDao.Query("update BusinessRecord set BusinessRecord_Weight=" + txtinweight + " where BusinessRecord_CarInOutRecord_ID=" + carinoutrecord + " and BusinessRecord_Type='" + CommonalityEntity.unloadFirstWeight + "'");
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (type == CommonalityEntity.upWeight)
                            {
                                string carinfo_po = table.Rows[0]["CarInfo_PO"].ToString();
                                save(carinoutrecord, carinfo_po, CommonalityEntity.unloadFirstWeight, txtinweight);
                                MessageBox.Show(this, "保存成功");
                                CommonalityEntity.strCardNo = "";
                                //记录地磅策略已通过
                                record = new WeighStrategyRecord();
                                record.WeighStrategyRecord_WeighInfo_ID = Convert.ToInt32(cobWeighStrategyRecord.SelectedValue);
                                record.WeighStrategyRecord_StaffInfo_ID = Convert.ToInt32(txtCarregistration_Name.Tag.ToString());
                                record.WeighStrategyRecord_CarInfo_ID = Convert.ToInt32(txtCarregistration_Carnumber.Tag.ToString());
                                record.WeighStrategyRecord_Carnumber = txtCarregistration_Carnumber.Text.Trim();
                                record.WeighStrategyRecord_StaffInfo_Name = txtCarregistration_Name.Text.Trim();
                                record.WeighStrategyRecord_CarType = txtCartype_Name.Text.Trim();
                                record.WeighStrategyRecord_mark = true;
                                record.WeighStrategyRecord_RecordTime = CommonalityEntity.GetServersTime();
                                record.WeighStrategyRecord_Name = cobWeighStrategyRecord.Text.Trim();
                                record.WeighStrategyRecord_order = RraderSort;
                                record.WeighStrategyRecord_Remark = txtRemark.Text.Trim();
                                if (!WeighStrategyRecordDAL.InsertOneQCRecord(record))
                                {
                                    //MessageBox.Show("地磅房策略通行记录失败！");
                                }
                            }
                            else
                            {
                                MessageBox.Show(this, "保存失败，没有" + CommonalityEntity.upWeight);
                            }
                        }
                    }
                    //判断出门地磅是否重复记录
                    if (txtoutweight != "" && txtoutweight != weight2)
                    {
                        DataTable ds = LinQBaseDao.Query("select * from BusinessRecord where BusinessRecord_Type='" + CommonalityEntity.unloadSecondWeight + "' and BusinessRecord_CarInOutRecord_ID=" + carinoutrecord + " order by BusinessRecord_ID desc").Tables[0];
                        if (ds.Rows.Count > 0)
                        {
                            if (MessageBox.Show("是否修改" + CommonalityEntity.unloadSecondWeight + "重量？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                LinQBaseDao.Query(" update BusinessRecord set BusinessRecord_Weight=" + txtoutweight + " where BusinessRecord_CarInOutRecord_ID=" + carinoutrecord + " and BusinessRecord_Type='" + CommonalityEntity.unloadSecondWeight + "'");
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            DataTable tab = LinQBaseDao.Query("select * from BusinessRecord where BusinessRecord_Type='" + CommonalityEntity.unloadFirstWeight + "' and BusinessRecord_CarInOutRecord_ID=" + carinoutrecord + " order by BusinessRecord_ID desc").Tables[0];
                            if (tab.Rows[0]["BusinessRecord_Type"].ToString() == CommonalityEntity.unloadFirstWeight)
                            {
                                string carinfo_po = tab.Rows[0]["CarInfo_PO"].ToString();
                                save(carinoutrecord, carinfo_po, CommonalityEntity.unloadSecondWeight, txtoutweight);
                                MessageBox.Show(this, "保存成功");
                                CommonalityEntity.strCardNo = "";
                                //记录地磅策略已通过
                                record = new WeighStrategyRecord();
                                record.WeighStrategyRecord_WeighInfo_ID = Convert.ToInt32(cobWeighStrategyRecord.SelectedValue);
                                record.WeighStrategyRecord_StaffInfo_ID = Convert.ToInt32(txtCarregistration_Name.Tag.ToString());
                                record.WeighStrategyRecord_CarInfo_ID = Convert.ToInt32(txtCarregistration_Carnumber.Tag.ToString());
                                record.WeighStrategyRecord_Carnumber = txtCarregistration_Carnumber.Text.Trim();
                                record.WeighStrategyRecord_StaffInfo_Name = txtCarregistration_Name.Text.Trim();
                                record.WeighStrategyRecord_CarType = txtCartype_Name.Text.Trim();
                                record.WeighStrategyRecord_mark = true;
                                record.WeighStrategyRecord_RecordTime = CommonalityEntity.GetServersTime();
                                record.WeighStrategyRecord_Name = cobWeighStrategyRecord.Text.Trim();
                                record.WeighStrategyRecord_order = RraderSort;
                                record.WeighStrategyRecord_Remark = txtRemark.Text.Trim();
                                if (!WeighStrategyRecordDAL.InsertOneQCRecord(record))
                                {
                                    MessageBox.Show("地磅房策略通行记录失败！");
                                }
                            }
                            else
                            {
                                MessageBox.Show(this, "保存失败，没有" + CommonalityEntity.unloadFirstWeight);
                            }
                        }
                    }
                }
            }
            catch
            {
                CommonalityEntity.WriteTextLog("卸货点过磅信息 btnSave_Click()" + "".ToString());
            }
        }
        /// <summary>
        /// 保存过磅信息
        /// </summary>
        /// <param name="carinoutid">车辆通行总记录ID</param>
        /// <param name="content">送货车辆EBELN(PO号)、成品车辆WTD_ID（委托单）、三废车辆VBELN(交货单号)</param>
        /// <param name="type">业务类型(进门地磅、出门地磅、装货第一次过磅、卸货第一次过磅、装货第二次过磅、卸货第二次过磅)</param>
        /// <param name="wie">过磅重量</param>
        private void save(string carinoutid, string content, string type, string wie)
        {
            BusinessRecord busin = new BusinessRecord();
            busin.BusinessRecord_CarInOutRecord_ID = Convert.ToInt32(carinoutid);
            busin.BusinessRecord_Content = content;
            busin.BusinessRecord_State = "启动";
            busin.BusinessRecord_Type = type;
            busin.BusinessRecord_Weight = Convert.ToDouble(wie);
            busin.BusinessRecord_WeightTime = CommonalityEntity.GetServersTime();
            busin.BusinessRecord_WeightPerson = CommonalityEntity.USERNAME;

            if (LinQBaseDao.InsertOne<BusinessRecord>(new DCCarManagementDataContext(), busin))
            {
                CommonalityEntity.WriteLogData("记录", "记录IC卡(小票)号为：" + txtSerialnumber.Text.Trim() + "的" + type + "信息", CommonalityEntity.USERNAME);//添加操作日志
            }
        }
        /// <summary>
        /// 判断小票是否过期
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private bool ISFill(DataTable table)
        {
            string serialnumber = txtSerialnumber.Text.Trim();
            DateTime smallticket_time = Convert.ToDateTime(table.Rows[0]["SmallTicket_Time"].ToString());
            string smallticket_state = table.Rows[0]["SmallTicket_State"].ToString();
            int smallticket_allowcounted = Convert.ToInt32(table.Rows[0]["SmallTicket_Allowcounted"].ToString());
            int smallticket_allowcount = Convert.ToInt32(table.Rows[0]["SmallTicket_Allowcount"].ToString());
            int smallticket_allowhour = Convert.ToInt32(table.Rows[0]["SmallTicket_Allowhour"].ToString());
            if (smallticket_state != "有效")
            {
                MessageBox.Show("IC卡(小票)号无效！");
                return false;
            }
            else if (smallticket_allowcounted == smallticket_allowcount && smallticket_allowcount != 0)
            {
                if (serialnumber.Length >= 12)
                {
                    if (!iscarful(serialnumber.Substring(0, 12)))
                    {
                        MessageBox.Show("小票已过期!");
                        return false;
                    }
                }
            }
            else if (smallticket_time.AddHours(smallticket_allowhour) < CommonalityEntity.GetServersTime())
            {
                if (serialnumber.Length >= 12)
                {
                    if (!iscarful(serialnumber.Substring(0, 12)))
                    {
                        MessageBox.Show("小票已过期!");
                        return false;
                    }
                }
            }
            else
            {
                return true;
            }
            return true;
        }
        private bool iscarful(string serialnumber)
        {
            DataTable dtfull = LinQBaseDao.Query("select CarInOutRecord_ISFulfill from View_CarState where SmallTicket_Serialnumber='" + serialnumber + "'").Tables[0];
            if (!string.IsNullOrEmpty(dtfull.Rows[0][0].ToString()))
            {
                bool isfull = Convert.ToBoolean(dtfull.Rows[0][0].ToString());
                if (isfull)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 输入的IC卡小票给sql附值
        /// </summary>
        private void strsql()
        {
            string iccard_id = txtSerialnumber.Text.Trim();
            if (iccard_id.Length == 10)
            {
                iccard_id = Convert.ToInt64(iccard_id).ToString("X");
            }
            if (iccard_id.Length > 12)
            {
                iccard_id = iccard_id.Substring(0, 12);
            }
            if (iccard_id == "" || iccard_id == null)
            {
                return;
            }
            DataTable dt = LinQBaseDao.Query("select ICCard_ID from dbo.ICCard WHERE ICCard_Value='" + iccard_id + "'").Tables[0];
            //刷IC卡
            if (dt.Rows.Count > 0)
            {
                sql = "select top(1) * from View_CarState where SmallTicket_ICCard_ID='" + Convert.ToInt32(dt.Rows[0][0].ToString()) + "' order by SmallTicket_ID desc";
            }
            //刷小票
            else
            {
                sql = "select * from View_CarState where SmallTicket_Serialnumber='" + iccard_id + "' order by SmallTicket_ID desc";
            }
        }
        /// <summary>
        /// 清空
        /// </summary>
        private void Empty()
        {
            txtInWeight.Text = "";
            txtOutWeight.Text = "";
            txtCartype_Name.Text = "";
            txtCarregistration_Customer.Text = "";
            txtCarregistration_Name.Text = "";
            txtCarregistration_Carnumber.Text = "";
            txtCarregistration_License.Text = "";
            txtCarregistration_Phone.Text = "";
            txtCarregistration_IdentityCard.Text = "";
            txtCarregistration_Carriage.Text = "";
            txtCarregistration_Weight.Text = "";
            txtCarregistration_Height.Text = "";
            txtOperateTime.Text = "";
            txtOperateUser.Text = "";
            txtRemark.Text = "";
        }
        /// <summary>
        /// 授权
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOutPass_Click(object sender, EventArgs e)
        {
            strsql();
            if (sql == "")
            {
                MessageBox.Show("没有该IC卡(小票)号的信息！");
                return;
            }
            DataTable ds = LinQBaseDao.Query(sql).Tables[0];

            if (ds.Rows.Count > 1)
            {
                if (ISFill(ds) == false)
                {
                    return;
                }
                string loadtype = "";
                string type = "";
                double wegiht1 = 0;
                double wegiht2 = 0;
                string carinout = ds.Rows[0]["CarInOutRecord_ID"].ToString();
                DataTable tableB = LinQBaseDao.Query("select * from BusinessRecord where BusinessRecord_CarInOutRecord_ID=" + carinout).Tables[0];
                for (int i = 0; i < tableB.Rows.Count; i++)
                {
                    if (tableB.Rows[i]["BusinessRecord_Type"].ToString() == CommonalityEntity.unloadSecondWeight)
                    {
                        if (tableB.Rows[i]["BusinessRecord_UnloadEmpower"].ToString() == "1")
                        {
                            MessageBox.Show("车辆已卸货确认！");
                            return;
                        }
                        loadtype = CommonalityEntity.unloadSecondWeight;
                        wegiht2 = Convert.ToDouble(tableB.Rows[i]["BusinessRecord_Weight"].ToString());
                    }
                    if (tableB.Rows[i]["BusinessRecord_Type"].ToString() == CommonalityEntity.unloadFirstWeight)
                    {
                        type = CommonalityEntity.unloadFirstWeight;
                        wegiht1 = Convert.ToDouble(tableB.Rows[i]["BusinessRecord_Weight"].ToString());
                    }
                }
                if (loadtype == CommonalityEntity.unloadSecondWeight && type == "")
                {
                    MessageBox.Show("没有卸货点第一次过磅");
                    return;
                }

                if (loadtype == CommonalityEntity.unloadSecondWeight && type == CommonalityEntity.unloadFirstWeight)
                {
                    Expression<Func<BusinessRecord, bool>> p = n => n.BusinessRecord_CarInOutRecord_ID == int.Parse(carinout) && n.BusinessRecord_Type == CommonalityEntity.unloadSecondWeight;
                    Action<BusinessRecord> ap = s =>
                    {
                        s.BusinessRecord_UnloadEmpowerPerson = CommonalityEntity.USERNAME;
                        s.BusinessRecord_UnloadEmpowerTime = CommonalityEntity.GetServersTime();
                        s.BusinessRecord_PrintinvoiceTime = CommonalityEntity.GetServersTime();
                        s.BusinessRecord_UnloadEmpower = 1;
                        s.BusinessRecord_Remark = txtRemark.Text.ToString().Trim();
                    };

                    if (BusinessRecordDAL.Update(p, ap))
                    {
                        CommonalityEntity.WriteLogData("新增", "地磅房:车辆" + ds.Rows[0]["CarInfo_Name"].ToString() + "卸货确认", CommonalityEntity.USERNAME);//添加操作日志
                        MessageBox.Show("确认成功");
                    }
                }
            }
            else
            {
                MessageBox.Show("车辆没有过磅，不能卸货确认！");
                return;
            }

        }
        /// <summary>
        /// 地磅房名称绑定
        /// </summary>
        private void WeighStrategyName()
        {
            DataSet ds = LinQBaseDao.Query("select WeighInfo_ID,WeighInfo_Name from WeighInfo where WeighInfo_State='启动'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cobWeighStrategyRecord.DataSource = ds.Tables[0];
                this.cobWeighStrategyRecord.DisplayMember = "WeighInfo_Name";
                cobWeighStrategyRecord.ValueMember = "WeighInfo_ID";
                cobWeighStrategyRecord.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// 获取车辆异常信息
        /// </summary>
        private void Abnormal()
        {
            DataTable ds = LinQBaseDao.Query(sql).Tables[0];
            if (ds.Rows.Count > 0)
            {
                string caroutid = ds.Rows[0]["CarInOutRecord_ID"].ToString();
                DataSet das = LinQBaseDao.Query("select CarInOutInfoRecord_Abnormal from CarInOutInfoRecord where CarInOutInfoRecord_CarInOutRecord_ID=" + caroutid + " and CarInOutInfoRecord_Abnormal!='正常'");
                if (das.Tables[0].Rows.Count > 0)
                {
                    for (int s = 0; s < das.Tables[0].Rows.Count; s++)
                    {
                        abnormal += das.Tables[0].Rows[s][0].ToString() + "\r\n";
                    }
                }
            }
        }
        private void UnLoadSmallDiscern_Load(object sender, EventArgs e)
        {
            userContext();
            WeighStrategyName();
            txtSerialnumber.Text = CommonalityEntity.strCardNo;
        }
        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {

                btnSave.Enabled = true;
                btnSave.Visible = true;
                btnUnload.Enabled = true;
                btnUnload.Visible = true;
                btnOutPass.Enabled = true;
                btnOutPass.Visible = true;
            }
            else
            {

                btnSave.Visible = ControlAttributes.BoolControl("btnSave", "UnLoadSmallDiscern", "Visible");
                btnSave.Enabled = ControlAttributes.BoolControl("btnSave", "UnLoadSmallDiscern", "Enabled");

                btnUnload.Visible = ControlAttributes.BoolControl("btnUnload", "UnLoadSmallDiscern", "Visible");
                btnUnload.Enabled = ControlAttributes.BoolControl("btnUnload", "UnLoadSmallDiscern", "Enabled");

                btnOutPass.Enabled = ControlAttributes.BoolControl("btnOutPass", "UnLoadSmallDiscern", "Enabled");
                btnOutPass.Visible = ControlAttributes.BoolControl("btnOutPass", "UnLoadSmallDiscern", "Visible");
            }
        }
        /// <summary>
        /// 卸货授权
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnload_Click(object sender, EventArgs e)
        {
            strsql();
            if (sql == "")
            {
                MessageBox.Show("没有该IC卡(小票)号的信息！");
                return;
            }
            DataTable ds = LinQBaseDao.Query(sql).Tables[0];

            if (ds.Rows.Count > 1)
            {
                if (ISFill(ds) == false)
                {
                    return;
                }
                if (Convert.ToBoolean(ds.Rows[0]["CarInOutRecord_ISFulfill"].ToString()))
                {
                    MessageBox.Show("该车辆已通行完成，无需授权！");
                    return;
                }
                string carinout = ds.Rows[0]["CarInOutRecord_ID"].ToString();
                DataTable tableB = LinQBaseDao.Query("select * from BusinessRecord where BusinessRecord_CarInOutRecord_ID=" + carinout).Tables[0];
                for (int i = 0; i < tableB.Rows.Count; i++)
                {
                    if (tableB.Rows[i]["BusinessRecord_Type"].ToString() == CommonalityEntity.unloadFirstWeight)
                    {
                        if (tableB.Rows[i]["BusinessRecord_UnloadEmpower"].ToString() == "1")
                        {
                            MessageBox.Show("车辆已授权！");
                            return;
                        }
                        CheckMethod.AbnormalInformation = "";
                        CommonalityEntity.CarInoutid = carinout;
                        if (Convert.ToBoolean(ds.Rows[0]["CarInOutRecord_Update"].ToString()))
                        {
                            CommonalityEntity.IsUpdatedri = true;
                            sql = "Select * from ManagementStrategyRecord where ManagementStrategyRecord_State='启动' and ManagementStrategyRecord_CarType_ID in(select CarType_ID from CarType where CarType_Name='" + ds.Rows[0]["CarType_Name"].ToString() + "') and ManagementStrategyRecord_ControlInfo_ID in (select ControlInfo_ID from ControlInfo where  ControlInfo_IDValue='450201'  )";
                        }
                        else
                        {
                            CommonalityEntity.IsUpdatedri = false;
                            sql = "Select * from ManagementStrategy where ManagementStrategy_State='启动' and ManagementStrategy_CarType_ID in(select CarType_ID from CarType where CarType_Name='" + ds.Rows[0]["CarType_Name"].ToString() + "') and ManagementStrategy_ControlInfo_ID in (select ControlInfo_ID from ControlInfo where  ControlInfo_IDValue='450201'  )";
                        }
                        checkPr.ExecutionMethod(sql);
                        if (CheckMethod.AbnormalInformation != "")
                        {
                            MessageBox.Show("过磅重量异常，授权失败！");
                            return;
                        }
                        Expression<Func<BusinessRecord, bool>> p = n => n.BusinessRecord_CarInOutRecord_ID == int.Parse(carinout) && n.BusinessRecord_Type == CommonalityEntity.unloadFirstWeight;
                        Action<BusinessRecord> ap = s =>
                        {
                            s.BusinessRecord_UnloadEmpowerPerson = CommonalityEntity.USERNAME;
                            s.BusinessRecord_UnloadEmpowerTime = CommonalityEntity.GetServersTime();
                            s.BusinessRecord_PrintinvoiceTime = CommonalityEntity.GetServersTime();
                            s.BusinessRecord_UnloadEmpower = 1;
                            s.BusinessRecord_Remark = txtRemark.Text.ToString().Trim();
                        };

                        if (BusinessRecordDAL.Update(p, ap))
                        {
                            CommonalityEntity.WriteLogData("新增", "地磅房:车辆" + ds.Rows[0]["CarInfo_Name"].ToString() + "卸货授权", CommonalityEntity.USERNAME);//添加操作日志
                            MessageBox.Show("授权成功");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("车辆没有过磅，不能授权！");
                return;
            }

        }

        private void txtSerialnumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
            }
            string iccard_id = txtSerialnumber.Text.Trim();
            if (iccard_id.Length == 10)
            {
                try
                {
                    txtSerialnumber.Text = "0" + Convert.ToInt64(iccard_id).ToString("X");
                }
                catch (Exception)
                {
                    txtSerialnumber.Text = "";
                }
            }
        }

        private void cobWeighStrategyRecord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
            }
        }

        private void btnRecognition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadData();
            }
        }
        #region 图片放大缩小事件
        /// <summary>
        /// 鼠标离开控件可见部分时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbInImageOne_MouseLeave(object sender, EventArgs e)
        {
            ShowD();
        }
        /// <summary>
        /// 鼠标悬停一定时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pbInImageOne_MouseHover(object sender, EventArgs e)
        {
            try
            {
                if (sender != null)
                {
                    PictureBox pic = sender as PictureBox;
                    if (pic.Tag != null)
                    {
                        ShowD(pic.Tag.ToString());
                    }
                }
            }
            catch
            {
                common.WriteTextLog("CarInformationForm.pbInImageOne_MouseHover()" + "");
            }
        }
        public Bitmap b = null;
        public void ShowD(string FileName)
        {
            groupBox5.BringToFront();
            b = new Bitmap(FileName);
            picBox.Image = b;
            groupBox5.Visible = true;

        }
        /// <summary>
        /// 改变图片大小(放大)
        /// </summary>
        /// <param name="pb"></param>
        public void ShowD()
        {
            try
            {
                groupBox5.Visible = false;
                b.Dispose();
                //移至底层
                //groupBox1.SendToBack();
            }
            catch
            {
                //common.WriteTextLog("ShowD()" + "");
            }
        }
        #endregion

        private void groupBox2_Enter(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(CommonalityEntity.strCardNo))
            {
                txtSerialnumber.Text = CommonalityEntity.strCardNo;
            }
        }


    }
}
