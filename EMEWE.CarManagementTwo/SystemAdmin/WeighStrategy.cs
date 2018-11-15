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
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class WeighStrategyFrom : Form
    {
        public WeighStrategyFrom()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 实例化分页控件
        /// </summary>
        PageControl page = new PageControl();
        // 定义一个全局变量： 门岗编号
        int iPositionID = 0;
        /// <summary>
        /// 存放查询条件
        /// </summary>
        private string sqlwhere = " 1=1 ";
        /// <summary>
        /// 修改前策略
        /// </summary>
        List<string> WeighStrategyAgo = null;
        /// <summary>
        /// 新选策略
        /// </summary>
        List<string> WeighStrategyBack = null;

        /// <summary>
        /// 车辆类型ID
        /// <summary>
        private string CarType_IDs="";

        private void WeighStrategy_Load(object sender, EventArgs e)
        {
            userContext();
            btnUpdate.Enabled = false;
            BtnDelete.Enabled = false;
            grpcount.Visible = false;
            this.grpWeight.Visible = false;
            StetaBingMethod();
            CarInfoBingMethod();
            LogInfoLoad("");
        }
        List<WeighStrategy> ListWeighStrategy = null;

        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {

                btnAdd.Enabled = true;
                btnAdd.Visible = true;
                BtnDelete.Enabled = true;
                BtnDelete.Visible = true;
                btnUpdate.Enabled = true;
                btnUpdate.Visible = true;
            }
            else
            {
                btnAdd.Visible = ControlAttributes.BoolControl("btnAdd", "WeighStrategyFrom", "Visible");
                btnAdd.Enabled = ControlAttributes.BoolControl("btnAdd", "WeighStrategyFrom", "Enabled");

                btnUpdate.Visible = ControlAttributes.BoolControl("btnUpdate", "WeighStrategyFrom", "Visible");
                btnUpdate.Enabled = ControlAttributes.BoolControl("btnUpdate", "WeighStrategyFrom", "Enabled");

                BtnDelete.Visible = ControlAttributes.BoolControl("BtnDelete", "WeighStrategyFrom", "Visible");
                BtnDelete.Enabled = ControlAttributes.BoolControl("BtnDelete", "WeighStrategyFrom", "Enabled");
            }
        }
        /// <summary>
        /// 加载发生
        /// </summary>
        private void LogInfoLoad(string strClickedItemName)
        {

            page.BindBoundControl(dgvWeighStrategy, strClickedItemName, tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_WeighStrategy", "*", "CarType_ID,WeighStrategy_Sort", "WeighStrategy_ID", 0, sqlwhere, true);
            //查询，在查询时附加一列顺序
            
        }

        /// <summary>
        /// 状态绑定
        /// </summary>
        private void StetaBingMethod()
        {
            try
            {
                var p = DictionaryDAL.GetValueStateDictionary("01");
                int intcount = p.Count();
                var Pcob_DrivewayStrategy_State = p.Where(n => n.Dictionary_Name != "全部").ToList();
                if (Pcob_DrivewayStrategy_State != null && Pcob_DrivewayStrategy_State.Count() > 0)
                {
                    cobSate.DataSource = Pcob_DrivewayStrategy_State;
                    this.cobSate.DisplayMember = "Dictionary_Name";
                    cobSate.ValueMember = "Dictionary_Value";
                    cobSate.SelectedIndex = 0;

                    cobseekSate.DataSource = Pcob_DrivewayStrategy_State;
                    this.cobseekSate.DisplayMember = "Dictionary_Name";
                    cobseekSate.ValueMember = "Dictionary_Value";
                    cobseekSate.SelectedIndex = 0;

                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("DrivewayStrategyForm.StetaBingMethod()" + "".ToString());
            }

        }

        /// <summary>
        /// 车辆类型绑定
        /// </summary>
        private void CarInfoBingMethod()
        {
            cobCarInfo.DataSource = LinQBaseDao.Query("select cartype_name,carType_ID from cartype where cartype_state='启动'").Tables[0];
            cobCarInfo.DisplayMember = "cartype_name";
            cobCarInfo.ValueMember = "carType_ID";
            cobCarInfo.SelectedIndex = -1;

            cobseekCarInfo.DataSource = LinQBaseDao.Query("select cartype_name,carType_ID from cartype where cartype_state='启动'").Tables[0];
            cobseekCarInfo.DisplayMember = "cartype_name";
            cobseekCarInfo.ValueMember = "carType_ID";
            cobseekCarInfo.SelectedIndex = -1;
        }

        /// <summary>
        /// 车辆类型发生改变时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cobCarInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(cobCarInfo.SelectedValue) >= 1)
                {
                    int i = cobCarInfo.Text.IndexOf("车") <= 0 ? 2 : cobCarInfo.Text.IndexOf("车");
                    txt_Name.Text = cobCarInfo.Text.Substring(0, i);
                }
                if (string.IsNullOrEmpty(cobCarInfo.Text))
                {
                    txt_Name.Text = "";
                }
                else
                {
                    txtWeighStrategy_Name.Text = "";
                    txtWeighStrategy_Name.Tag = null;
                    DataSet ds = LinQBaseDao.Query("select * from View_WeighStrategy where WeighStrategy_State='启动' and WeighStrategy_CarType_ID=" + cobCarInfo.SelectedValue.ToString() + " order by WeighStrategy_Sort asc");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        txtWeighStrategy_Name.Text += ds.Tables[0].Rows[i]["WeighStrategy_Name"].ToString() + ",";
                        txtWeighStrategy_Name.Tag += ds.Tables[0].Rows[i]["WeighInfo_ID"].ToString() + ",";
                    }
                    cobCarInfo.Text = ds.Tables[0].Rows[0]["CarType_Name"].ToString();
                    CarType_IDs = cobCarInfo.SelectedValue.ToString();
                }
                
                
            }
            catch 
            {
                Console.WriteLine("");
            }
        }
        #region 分页和加载DataGridView
        /// <summary>
        /// 取消全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslNotChecked()
        {
            for (int i = 0; i < this.dgvWeighStrategy.Rows.Count; i++)
            {
                dgvWeighStrategy.Rows[i].Selected = false;
            }
        }
        /// <summary>
        /// 全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tslAllChecked()
        {
            for (int i = 0; i < dgvWeighStrategy.Rows.Count; i++)
            {
                this.dgvWeighStrategy.Rows[i].Selected = true;
            }
        }
        /// <summary>
        /// 分页控件事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bdnInfo_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Name == "tslSelectAll")//全选
            {
                tslAllChecked();
                return;
            }
            if (e.ClickedItem.Name == "tslNotSelect")//取消全选
            {
                tslNotChecked();
                return;
            }
            LogInfoLoad(e.ClickedItem.Name);
        }
        /// <summary>
        /// 选择每页条数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tscbxPageSize2_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogInfoLoad("");
        }
        #endregion
        /// <summary>
        /// 双击策略发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtWeighStrategy_Name_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (cobCarInfo.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择车辆类型");
                    return;
                }
                else
                {
                    WeighStrategyAgo = new List<string>();
                    string[] num = null;
                    if (txtWeighStrategy_Name.Tag != null)
                    {
                        num = txtWeighStrategy_Name.Tag.ToString().Split(',');//保存之前策略数据
                    }
                    if (num != null)
                    {
                        for (int i = 0; i < num.Count()-1; i++)
                        {
                            WeighStrategyAgo.Add(num[i]);
                        }
                    }
                    //trWeightInfo_Name 控件数据绑定
                    trWeightInfo_Name.Nodes.Clear();
                    this.grpWeight.Visible = true;
                    WeighInfoNode();
                }
            }
            catch 
            {
                Console.WriteLine("");
            }
        }
        /// <summary>
        /// 绑定树形菜单
        /// </summary>
        private void WeighInfoNode()
        {
            TreeNode nodeTemp = null;
            DataSet ds = LinQBaseDao.Query("select WeighInfo_ID,WeighInfo_Name from WeighInfo where WeighInfo_State='启动'");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string num = ds.Tables[0].Rows[i]["WeighInfo_ID"].ToString();
                    nodeTemp = new TreeNode();
                    nodeTemp.Name = ds.Tables[0].Rows[i]["WeighInfo_ID"].ToString();
                    nodeTemp.Text = ds.Tables[0].Rows[i]["WeighInfo_Name"].ToString();
                    trWeightInfo_Name.Nodes.Add(nodeTemp);
                    DataSet dsc = LinQBaseDao.Query("select * from WeighStrategy where WeighStrategy_CarType_ID=" + cobCarInfo.SelectedValue.ToString() + " and WeighStrategy_WeighInfo_ID=" + ds.Tables[0].Rows[i]["WeighInfo_ID"] + " and WeighStrategy_State='启动'");
                    if (dsc.Tables[0].Rows.Count > 0)
                    {
                        nodeTemp.Checked = true;
                    }
                    else
                    {
                        nodeTemp.Checked = false;
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.grpWeight.Visible = false;
        }

        private string WeightStrategy_Name = "";
        private string WeightStrategy_ID = "";
        /// <summary>
        /// 复选框选择地磅房名称信息单击确定时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnWeightQD_Click(object sender, EventArgs e)
        {
            try
            {
                if (trWeightInfo_Name.Nodes.Count > 0)
                {
                    //选中了数据
                    WeightStrategy_Name = "";
                    WeightStrategy_ID = "";
                    CarType_IDs = cobCarInfo.SelectedValue.ToString();
                    foreach (TreeNode tnTemp in trWeightInfo_Name.Nodes)
                    {
                        if (tnTemp.Checked)
                        {
                            WeightStrategy_Name += tnTemp.Text + ",";
                            WeightStrategy_ID += tnTemp.Name + ",";
                        }
                    }
                    WeighStrategyBack = new List<string>();
                    string[] nums = WeightStrategy_ID.Split(',');
                    string sqlwhere = null;
                    if (nums.Count() > 0)//选择后
                    {
                        for (int i = 0; i < nums.Count() - 1; i++)
                        {
                            WeighStrategyBack.Add(nums[i]);
                            if (sqlwhere != null)
                            {
                                sqlwhere += "," + nums[i];
                            }
                            else
                            {
                                sqlwhere += nums[i];
                            }
                        }
                        txtWeighStrategy_Name.Text = WeightStrategy_Name;
                        txtWeighStrategy_Name.Tag = WeightStrategy_ID;
                        string Sql = "select WeighStrategy_ID,WeighInfo_Name from View_WeighStrategy where WeighStrategy_CarType_ID=" + cobCarInfo.SelectedValue.ToString() + " and WeighInfo_ID in(" + sqlwhere + ")  order by WeighStrategy_Sort asc";
                        DataSet ds = LinQBaseDao.Query(Sql);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            WeightStrategy_Name = "";
                            WeightStrategy_ID = "";
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                WeightStrategy_Name += ds.Tables[0].Rows[i]["WeighInfo_Name"] + ",";
                                WeightStrategy_ID += ds.Tables[0].Rows[i]["WeighStrategy_ID"] + ",";
                            }
                        }
                    }

                }
                Shift();

            }
            catch 
            {
                MessageBox.Show("");
            }
            finally
            {
                grpWeight.Visible = false;
            }
        }

        /// <summary>
        /// 单击保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                WeighStrategy weig;
                if (cobCarInfo.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择车辆类型");
                    return;
                }
                if (cobSate.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择启动状态");
                    return;
                }
                if (string.IsNullOrEmpty(txtWeighStrategy_Name.Text))
                {
                    MessageBox.Show("请选择过磅策略");
                    return;
                }
                if (txtRemark.Text.Trim().Length > 200)
                {
                    MessageBox.Show("超过最大字符，限制字数200字内");
                    return;
                }
                string[] Name = txtWeighStrategy_Name.Text.Trim().Split(',');
                string[] ID = txtWeighStrategy_Name.Tag.ToString().Split(',');
                if (Name.Count() < 1)
                {
                    MessageBox.Show("策略必须至少选择一项");
                    return;
                }
                DataSet ds = LinQBaseDao.Query("select * from WeighStrategy where WeighStrategy_CarType_ID=" + cobCarInfo.SelectedValue.ToString());
                //修改所有未暂停
                LinQBaseDao.Query("update WeighStrategy set WeighStrategy_State='暂停' where WeighStrategy_CarType_ID=" + cobCarInfo.SelectedValue.ToString());
                //新增新数据，已存在修改为启动
                for (int i = 0; i < ListWeighStrategy.Count; i++)
                {
                    DataSet SelectDs = LinQBaseDao.Query("select * from WeighStrategy where WeighStrategy_CarType_ID=" + cobCarInfo.SelectedValue.ToString() + " and WeighStrategy_WeighInfo_ID=" + ListWeighStrategy[i].WeighStrategy_WeighInfo_ID.ToString());
                    if (SelectDs.Tables[0].Rows.Count > 0)
                    {
                        //存在 修改为启动
                        Expression<Func<WeighStrategy, bool>> funcs = n => n.WeighStrategy_CarType_ID == Convert.ToInt32(cobCarInfo.SelectedValue) && n.WeighStrategy_WeighInfo_ID == ListWeighStrategy[i].WeighStrategy_WeighInfo_ID;
                        Action<WeighStrategy> actions = a =>
                        {
                            a.WeighStrategy_State = "启动";//启动状态
                            a.WeighStrategy_Sort = ListWeighStrategy[i].WeighStrategy_Sort;
                        };
                        if (!WeighStrategyDAL.Update(funcs, actions))
                        {
                            MessageBox.Show("新增失败");
                            return;
                        }
                    }
                    else
                    {
                        //不存在。新增
                        weig = new WeighStrategy();
                        weig.WeighStrategy_WeighInfo_ID = Convert.ToInt32(ListWeighStrategy[i].WeighStrategy_WeighInfo_ID);
                        weig.WeighStrategy_CarType_ID = Convert.ToInt32(cobCarInfo.SelectedValue);
                        weig.WeighStrategy_Name = ListWeighStrategy[i].WeighStrategy_Name;
                        weig.WeighStrategy_Sort = ListWeighStrategy[i].WeighStrategy_Sort;
                        weig.WeighStrategy_State = "启动";
                        weig.WeighStrategy_CreatTime = CommonalityEntity.GetServersTime();
                        weig.WeighStrategy_Remark = txtRemark.Text.Trim();
                        if (!WeighStrategyDAL.InsertOneQCRecord(weig))
                        {
                            MessageBox.Show("保存失败！");
                            return;
                        }
                        CommonalityEntity.WriteLogData("新增", "新增过策略：" + Name[i].ToString(), CommonalityEntity.USERNAME);//添加操作日志
                    }
                }
                MessageBox.Show("保存成功!");
                LogInfoLoad("");
                Empty();
            }
            catch 
            {
                MessageBox.Show("");
            }
        }

        /// <summary>
        /// 单击清空按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnempty_Click(object sender, EventArgs e)
        {
            Empty();
        }

        /// <summary>
        /// 清空方法
        /// </summary>
        private void Empty()
        {
            cobCarInfo.SelectedIndex = -1;
            cobSate.SelectedIndex = 0;
            txtWeighStrategy_Name.Text = "";
            txtRemark.Text = "";
            cobseekCarInfo.Text = "";
            cobseekSate.Text="";
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
            BtnDelete.Enabled = false;
        }

        /// <summary>
        /// 单击搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnseek_Click(object sender, EventArgs e)
        {
            string SqlWhere = " 1=1 ";
            if (!string.IsNullOrEmpty(cobseekCarInfo.Text.Trim()))
            {
                SqlWhere += " and CarType_Name='" + cobseekCarInfo.Text.Trim()+"'";
            }
            if (!string.IsNullOrEmpty(cobseekSate.Text.Trim()))
            {
                SqlWhere += " and WeighStrategy_State='" + cobseekSate.Text.Trim()+"'";
            }
            page.BindBoundControl(dgvWeighStrategy, "", tstbPageCurrent, tslPageCount, tslNMax, tscbxPageSize, "View_WeighStrategy", "*", "CarType_ID,WeighStrategy_Sort", "WeighStrategy_ID", 0, SqlWhere, true);
           
        }
        /// <summary>
        /// 查询
        /// </summary>
        private void SelectWeight()
        {
            DataSet ds = LinQBaseDao.Query("select * from View_WeighStrategy order by CarType_Name,WeighStrategy_Sort");

        }

        private void btnCarInfo_Click(object sender, EventArgs e)
        {
            CarTypeForm info = new CarTypeForm();
            info.Show();
        }

        private void btnWeighInfo_Click(object sender, EventArgs e)
        {
            WeighInfoFrom info = new WeighInfoFrom();
            info.ShowDialog();
        }
        WeighStrategy weig = null;
        /// <summary>
        /// 单击修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvWeighStrategy.SelectedRows.Count < 1)
                {
                    MessageBox.Show("请双击选中需要修改的数据");
                    return;
                }
                if (string.IsNullOrEmpty(txtWeighStrategy_Name.Text.Trim()))
                {
                    MessageBox.Show("请选择地磅策略！");
                    return;
                }
                if (cobSate.Text.Trim() != "启动" && cobCarInfo.Text.Trim() != "")
                {
                    //直接修改策略状态
                    string Sql = "select * from View_WeighStrategy where CarType_ID=" + CarType_IDs;
                    DataSet ds = LinQBaseDao.Query(Sql);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Expression<Func<WeighStrategy, bool>> func = n => n.WeighStrategy_ID == Convert.ToInt32(ds.Tables[0].Rows[i]["WeighStrategy_ID"]) && n.WeighStrategy_CarType_ID == Convert.ToInt32(CarType_IDs);
                        Action<WeighStrategy> action = a =>
                        {
                            a.WeighStrategy_State = cobSate.Text.Trim();//修改状态
                            a.WeighStrategy_Remark = txtRemark.Text;
                        };
                        if (!WeighStrategyDAL.Update(func, action))
                        {
                            MessageBox.Show("修改失败！");
                            return;
                        }
                    }
                }
                else if (cobCarInfo.Text.Trim() != "")
                {
                    int demo = 0;
                    List<string> test = WeighStrategyAgo;//备份新选的策略
                    int indexs = 0;
                    for (int i = 0; i < WeighStrategyBack.Count; i++)//循环选中后的
                    {
                        indexs = 0;
                        for (int j = 0; j < WeighStrategyAgo.Count; j++)//循环选中前的
                        {
                            if (WeighStrategyBack[i] == WeighStrategyAgo[j])
                            {
                                //进入说明相等，不用操作
                                indexs++;
                                test.RemoveAt(j);
                            }
                        }

                        #region 说明选中的当前策略不相等，新增、修改为启动
                        if (indexs == 0)//选中不同时进入
                        {
                            //说明选中的当前策略不相等，新增、修改为启动
                            DataSet ds = LinQBaseDao.Query("select * from View_WeighStrategy where CarType_ID=" + CarType_IDs + " and WeighInfo_ID=" + WeighStrategyBack[i]);//查询该车辆类型所有数据
                            //int Infoid = Convert.ToInt32(ds.Tables[0].Rows[0][1]); for:Infoid == Convert.ToInt32(WeighStrategyBack[i])
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                //说明选中的当前策略不相等，新增、修改为启动
                                Expression<Func<WeighStrategy, bool>> func = n => n.WeighStrategy_WeighInfo_ID == Convert.ToInt32(WeighStrategyBack[i]) && n.WeighStrategy_CarType_ID == Convert.ToInt32(CarType_IDs);
                                Action<WeighStrategy> action = a =>
                                {
                                    a.WeighStrategy_State = "启动";//启动状态
                                    a.WeighStrategy_Remark = txtRemark.Text;
                                };
                                if (WeighStrategyDAL.Update(func, action))
                                {
                                    indexs++;//修改成功
                                }
                                else
                                {
                                    MessageBox.Show("修改失败！");
                                    return;
                                }

                            }
                            else
                            {
                                //新增
                                DataSet dsc = LinQBaseDao.Query("select * from WeighInfo where  WeighInfo_ID=" + WeighStrategyBack[i]);//查询磅房名
                                DataSet dsmax = LinQBaseDao.Query("select MAX(WeighStrategy_Sort) from WeighStrategy where WeighStrategy_CarType_ID='" + CarType_IDs + "'");
                                weig = new WeighStrategy();
                                weig.WeighStrategy_WeighInfo_ID = Convert.ToInt32(WeighStrategyBack[i]);
                                weig.WeighStrategy_CarType_ID = Convert.ToInt32(CarType_IDs);
                                weig.WeighStrategy_Name = dsc.Tables[0].Rows[0][1].ToString();
                                weig.WeighStrategy_Sort = Convert.ToInt32(dsmax.Tables[0].Rows[0][0]) + 1;
                                weig.WeighStrategy_State = "启动";
                                weig.WeighStrategy_CreatTime = CommonalityEntity.GetServersTime();
                                weig.WeighStrategy_Remark = txtRemark.Text.Trim();
                                if (!WeighStrategyDAL.InsertOneQCRecord(weig))
                                {
                                    MessageBox.Show("修改失败！");//新增失败
                                    return;
                                }
                            }
                        }
                        else
                        {
                            //两次选中有相同策略时

                        }

                        #endregion
                    }
                    #region 暂停多余数据
                    for (int i = 0; i < test.Count; i++)
                    {
                        DataSet ds = LinQBaseDao.Query("select * from View_WeighStrategy where CarType_ID=" + CarType_IDs + " and WeighInfo_ID=" + WeighStrategyAgo[i]);//查询该车辆类型所有数据
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //修改多余数据 暂停
                            Expression<Func<WeighStrategy, bool>> func = n => n.WeighStrategy_WeighInfo_ID == Convert.ToInt32(WeighStrategyAgo[i]) && n.WeighStrategy_CarType_ID == Convert.ToInt32(CarType_IDs);
                            Action<WeighStrategy> action = a =>
                            {
                                a.WeighStrategy_State = "暂停";//启动状态
                            };
                            if (WeighStrategyDAL.Update(func, action))
                            {
                                indexs++;//修改成功
                            }
                            else
                            {
                                MessageBox.Show("修改失败！");
                                return;
                            }

                        }
                    }
                    #endregion
                }

                MessageBox.Show("修改成功");
            }
            catch 
            {
                Console.WriteLine("");
            }
            finally
            {
                LogInfoLoad("");
                btnUpdate.Enabled = false;
                BtnDelete.Enabled = false;
                btnAdd.Enabled = true;
                Empty();
            }

        }

        /// <summary>
        /// 双击组件时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvWeighStrategy_DoubleClick(object sender, EventArgs e)
        {
            txtWeighStrategy_Name.Text="";
            txtWeighStrategy_Name.Tag=null;
            try
            {
                if (dgvWeighStrategy.SelectedRows.Count > 1)
                {
                    MessageBox.Show("双击只允许选中一条数据");
                    return;
                }
                btnUpdate.Enabled = true;
                BtnDelete.Enabled = true;
                int CarType_ID = Convert.ToInt32(dgvWeighStrategy.SelectedRows[0].Cells["WeighStrategy_CarType_ID"].Value.ToString());
                DataSet ds = LinQBaseDao.Query("select * from View_WeighStrategy where WeighStrategy_State='启动' and WeighStrategy_CarType_ID=" + CarType_ID + " order by WeighStrategy_Sort asc");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        txtWeighStrategy_Name.Text += ds.Tables[0].Rows[i]["WeighStrategy_Name"].ToString() + ",";
                        txtWeighStrategy_Name.Tag += ds.Tables[0].Rows[i]["WeighInfo_ID"].ToString() + ",";//WeighStrategy_ID
                    }
                    cobCarInfo.Text = ds.Tables[0].Rows[0]["CarType_Name"].ToString();
                    CarType_IDs = dgvWeighStrategy.SelectedRows[0].Cells["WeighStrategy_CarType_ID"].Value.ToString();
                    cobSate.Text = ds.Tables[0].Rows[0]["WeighStrategy_State"].ToString();
                    txtRemark.Text = ds.Tables[0].Rows[0]["WeighStrategy_Remark"].ToString();

                    #region 保存之前策略数据
                    WeighStrategyAgo = new List<string>();
                    string[] num = null;
                    if (txtWeighStrategy_Name.Tag != null)
                    {
                        num = txtWeighStrategy_Name.Tag.ToString().Split(',');//保存之前策略数据
                    }
                    if (num != null)
                    {
                        for (int i = 0; i < num.Count() - 1; i++)
                        {
                            WeighStrategyAgo.Add(num[i]);
                        }
                    }
                    #endregion
                }
                else
                {
                    //入此说明无配置策略或者是策略全部为非启动状态
                    DataSet dsc = LinQBaseDao.Query("select * from View_WeighStrategy where WeighStrategy_State != '启动' and WeighStrategy_CarType_ID=" + CarType_ID + " order by WeighStrategy_Sort asc");
                    if (dsc.Tables[0].Rows.Count > 0)
                    {
                        cobCarInfo.Text = dsc.Tables[0].Rows[0]["CarType_Name"].ToString();
                        cobSate.Text = dsc.Tables[0].Rows[0]["WeighStrategy_State"].ToString();
                    }
                }

            }
            catch 
            {
                Console.WriteLine("");
            }
        }

        /// <summary>
        /// 单击删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            int index = 0;
            try
            {
                if (MessageBox.Show("删除，将会把所有关联删除，确定要删除吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int CarType_ID = Convert.ToInt32(dgvWeighStrategy.SelectedRows[0].Cells["WeighStrategy_CarType_ID"].Value.ToString());
                    DataSet ds = LinQBaseDao.Query("select WeighStrategy_ID from WeighStrategy where WeighStrategy_CarType_ID=" + CarType_ID);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Expression<Func<WeighStrategy, bool>> funuserinfo = n => n.WeighStrategy_ID == int.Parse(ds.Tables[0].Rows[i][0].ToString());
                        if (WeighStrategyDAL.DeleteToMany(funuserinfo))
                        {
                            index++;
                        }
                        else
                        {
                            index = 0;
                        }
                    }
                    if (index > 0)
                    {
                        MessageBox.Show("删除成功");
                        Empty();
                        LogInfoLoad("");
                    }
                    else
                    {
                        MessageBox.Show("删除失败");
                    }
                }
 
            }
            catch 
            {
                Console.WriteLine("");
            }
        }
        private void cobCarInfo_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            grpcount.Visible = true;
            grpWeight.Visible = false;
            Shift();
        }

        private void btnclos_Click(object sender, EventArgs e)
        {
            grpcount.Visible = false;
            trWeightInfo_Name.Nodes.Clear();
            WeighInfoNode();
            TxtWeightInfo_Name();
        }
        /// <summary>
        /// 显示地磅文本
        /// </summary>
        private void TxtWeightInfo_Name()
        {
            //txtWeighStrategy_Name.Text += ds.Tables[0].Rows[i]["WeighStrategy_Name"].ToString() + ",";
            //txtWeighStrategy_Name.Tag += ds.Tables[0].Rows[i]["WeighInfo_ID"].ToString() + ",";
            txtWeighStrategy_Name.Text = "";
            txtWeighStrategy_Name.Tag = null;
            for (int i = 0; i < ListWeighStrategy.Count; i++)
            {
                txtWeighStrategy_Name.Text += ListWeighStrategy[i].WeighStrategy_Name+",";
                txtWeighStrategy_Name.Tag += ListWeighStrategy[i].WeighStrategy_WeighInfo_ID.ToString()+",";
            }
        }
        /// <summary>
        /// 向上移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnS_Click(object sender, EventArgs e)
        {
            if (dgvCount.SelectedRows.Count != 1)
            {
                MessageBox.Show("请选择一项");
                return;
            }
            string WeighStratry_ID = dgvCount.SelectedRows[0].Cells["dgv_WeighStrategy_ID"].Value.ToString();//策略编号
            string Sort = dgvCount.SelectedRows[0].Cells["dgv_WeighStrategy_Sort"].Value.ToString();//策略顺序号
            string a = dgvCount.Rows[0].Cells["dgv_WeighStrategy_Sort"].Value.ToString();
            string b = dgvCount.SelectedRows[0].Cells["dgv_WeighStrategy_Sort"].Value.ToString();
            if (dgvCount.Rows[0].Cells["dgv_WeighStrategy_Sort"].Value.ToString() == dgvCount.SelectedRows[0].Cells["dgv_WeighStrategy_Sort"].Value.ToString())
            {
                //判断当前值是否是排序第一位
                MessageBox.Show("已经是最上级了,或未设置为有序！");
                return;

            }
            //排序号替换 
            for (int i = 0; i < ListWeighStrategy.Count; i++)
            {
                if (ListWeighStrategy[i].WeighStrategy_Sort == Convert.ToInt32(dgvCount.SelectedRows[0].Cells["dgv_WeighStrategy_Sort"].Value.ToString()))
                {
                    int Sorts = Convert.ToInt32(ListWeighStrategy[i].WeighStrategy_Sort);
                    ListWeighStrategy[i].WeighStrategy_Sort = ListWeighStrategy[i - 1].WeighStrategy_Sort;
                    ListWeighStrategy[i - 1].WeighStrategy_Sort = Sorts;
                }
            }
            //对集合进行冒泡排序
            for (int i = 0; i < ListWeighStrategy.Count; i++)
            {
                for (int j = 0; j < ListWeighStrategy.Count; j++)
                {
                    if (ListWeighStrategy[i].WeighStrategy_Sort < ListWeighStrategy[j].WeighStrategy_Sort)
                    {
                        WeighStrategy ws = ListWeighStrategy[j];
                        ListWeighStrategy[j] = ListWeighStrategy[i];
                        ListWeighStrategy[i] = ws;

                    }
                }
            }
            dgvCount.DataSource = ListWeighStrategy;
            dgvCount.Refresh();//更新页面数据，重要
        }

        /// <summary>
        /// 刷新、绑定设置顺序数据
        /// </summary>
        private void Shift()
        {
            //绑定
            ListWeighStrategy = new List<WeighStrategy>();
            WeighStrategy WeighSt = null;
            //没有策略
            DataSet NoDs = LinQBaseDao.Query("select * from WeighStrategy where WeighStrategy_CarType_ID=" + cobCarInfo.SelectedValue);
            if (NoDs.Tables[0].Rows.Count == 0)
            {
                //无数据 新增
                int maxsort = 1;
                foreach (TreeNode tnTemp in trWeightInfo_Name.Nodes)
                {
                    if (tnTemp.Checked)
                    {
                        WeighSt = new WeighStrategy();
                        //当前最大排序号
                        WeighSt.WeighStrategy_WeighInfo_ID = Convert.ToInt32(tnTemp.Name);
                        WeighSt.WeighStrategy_CarType_ID = Convert.ToInt32(cobCarInfo.SelectedValue);//车辆类型ID
                        WeighSt.WeighStrategy_Sort = maxsort;
                        WeighSt.WeighStrategy_Name = tnTemp.Text.Trim();
                        ListWeighStrategy.Add(WeighSt);
                        maxsort++;
                    }
                }
                //对集合进行冒泡排序
                for (int i = 0; i < ListWeighStrategy.Count; i++)
                {
                    for (int j = 0; j < ListWeighStrategy.Count; j++)
                    {
                        if (ListWeighStrategy[i].WeighStrategy_Sort < ListWeighStrategy[j].WeighStrategy_Sort)
                        {
                            WeighStrategy ws = ListWeighStrategy[j];
                            ListWeighStrategy[j] = ListWeighStrategy[i];
                            ListWeighStrategy[i] = ws;

                        }
                    }
                }
                dgvCount.DataSource = ListWeighStrategy;
                dgvCount.Refresh();//更新页面数据，重要
            }
            else
            {
                foreach (TreeNode tnTemp in trWeightInfo_Name.Nodes)
                {
                    if (tnTemp.Checked)
                    {
                        WeighSt = new WeighStrategy();
                        //当前最大排序号
                        DataSet maxsort = LinQBaseDao.Query("select MAX(WeighStrategy_Sort),MAX(WeighStrategy_ID) from WeighStrategy where WeighStrategy_CarType_ID=" + cobCarInfo.SelectedValue);
                        DataSet ds = LinQBaseDao.Query("select * from WeighStrategy where WeighStrategy_CarType_ID=" + cobCarInfo.SelectedValue + " and WeighStrategy_WeighInfo_ID=" + tnTemp.Name);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            //有此策略
                            WeighSt.WeighStrategy_Sort = Convert.ToInt32(ds.Tables[0].Rows[0]["WeighStrategy_Sort"]);
                            WeighSt.WeighStrategy_ID = Convert.ToInt32(ds.Tables[0].Rows[0]["WeighStrategy_ID"]);
                        }
                        else
                        {
                            //新策略
                            WeighSt.WeighStrategy_Sort = Convert.ToInt32(maxsort.Tables[0].Rows[0][0]) + 1;
                            WeighSt.WeighStrategy_ID = Convert.ToInt32(maxsort.Tables[0].Rows[0][1]) + 1;
                        }
                        WeighSt.WeighStrategy_WeighInfo_ID = Convert.ToInt32(tnTemp.Name);
                        WeighSt.WeighStrategy_CarType_ID = Convert.ToInt32(cobCarInfo.SelectedValue);//车辆类型ID
                        WeighSt.WeighStrategy_Name = tnTemp.Text.Trim();
                        ListWeighStrategy.Add(WeighSt);
                    }
                }
                //对集合进行冒泡排序
                for (int i = 0; i < ListWeighStrategy.Count; i++)
                {
                    for (int j = 0; j < ListWeighStrategy.Count; j++)
                    {
                        if (ListWeighStrategy[i].WeighStrategy_Sort < ListWeighStrategy[j].WeighStrategy_Sort)
                        {
                            WeighStrategy ws = ListWeighStrategy[j];
                            ListWeighStrategy[j] = ListWeighStrategy[i];
                            ListWeighStrategy[i] = ws;

                        }
                    }
                }
                dgvCount.DataSource = ListWeighStrategy;
                dgvCount.Refresh();//更新页面数据，重要
            }

        }
        /// <summary>
        /// 向下移动
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnX_Click(object sender, EventArgs e)
        {
            if (dgvCount.SelectedRows.Count != 1)
            {
                MessageBox.Show("请选择一项");
                return;
            }
            string WeighStratry_ID = dgvCount.SelectedRows[0].Cells["dgv_WeighStrategy_ID"].Value.ToString();//策略编号
            string Sort = dgvCount.SelectedRows[0].Cells["dgv_WeighStrategy_Sort"].Value.ToString();//策略顺序号
            string a = dgvCount.Rows[0].Cells["dgv_WeighStrategy_Sort"].Value.ToString();
            string b = dgvCount.SelectedRows[0].Cells["dgv_WeighStrategy_Sort"].Value.ToString();
            if (dgvCount.Rows[dgvCount.Rows.Count - 1].Cells["dgv_WeighStrategy_Sort"].Value.ToString() == dgvCount.SelectedRows[0].Cells["dgv_WeighStrategy_Sort"].Value.ToString())
            {
                //判断当前值是否是排序第一位
                MessageBox.Show("已经是最下级了！");
                return;
            }
            //排序号替换 
            for (int i = 0; i < ListWeighStrategy.Count; i++)
            {
                if (ListWeighStrategy[i].WeighStrategy_Sort == Convert.ToInt32(dgvCount.SelectedRows[0].Cells["dgv_WeighStrategy_Sort"].Value.ToString()))
                {
                    int Sorts = Convert.ToInt32(ListWeighStrategy[i].WeighStrategy_Sort);
                    ListWeighStrategy[i].WeighStrategy_Sort = ListWeighStrategy[i + 1].WeighStrategy_Sort;
                    ListWeighStrategy[i + 1].WeighStrategy_Sort = Sorts;
                }
            }
            //对集合进行冒泡排序
            for (int i = 0; i < ListWeighStrategy.Count; i++)
            {
                for (int j = 0; j < ListWeighStrategy.Count; j++)
                {
                    if (ListWeighStrategy[i].WeighStrategy_Sort < ListWeighStrategy[j].WeighStrategy_Sort)
                    {
                        WeighStrategy ws = ListWeighStrategy[j];
                        ListWeighStrategy[j] = ListWeighStrategy[i];
                        ListWeighStrategy[i] = ws;

                    }
                }
            }
            dgvCount.DataSource = ListWeighStrategy;
            dgvCount.Refresh();//更新页面数据，重要
        }

        /// <summary>
        /// 设置无序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNoSort_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ListWeighStrategy.Count; i++)
            {
                ListWeighStrategy[i].WeighStrategy_Sort = 1;
            }
            dgvCount.DataSource = ListWeighStrategy;
            dgvCount.Refresh();//更新页面数据，重要
        }
        /// <summary>
        /// 设置有序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnYouSort_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ListWeighStrategy.Count; i++)
            {
                ListWeighStrategy[i].WeighStrategy_Sort = i+1;
            }
            //对集合进行冒泡排序
            for (int i = 0; i < ListWeighStrategy.Count; i++)
            {
                for (int j = 0; j < ListWeighStrategy.Count; j++)
                {
                    if (ListWeighStrategy[i].WeighStrategy_Sort < ListWeighStrategy[j].WeighStrategy_Sort)
                    {
                        WeighStrategy ws = ListWeighStrategy[j];
                        ListWeighStrategy[j] = ListWeighStrategy[i];
                        ListWeighStrategy[i] = ws;
                    }
                }
            }
            dgvCount.DataSource = ListWeighStrategy;
            dgvCount.Refresh();//更新页面数据，重要
        }
    }
}
