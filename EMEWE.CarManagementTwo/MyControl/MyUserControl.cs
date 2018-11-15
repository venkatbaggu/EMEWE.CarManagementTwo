using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using EMEWE.CarManagement.SystemAdmin;

namespace EMEWE.CarManagement.MyControl
{
    public partial class MyUserControl : UserControl
    {
        public MyUserControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 返回查询出的ID
        /// </summary>
        public string staffInfo_Id = "";

        /// <summary>
        /// 公司名
        /// </summary>
        public string CustomerInfo_GName = "";

        /// <summary>
        /// 返回名称=TXT文本
        /// </summary>
        public string staffInfo_Name = "";

        //驾驶员分身证号(拼接过的)
        public string StaffInfo_identity = "";

        /// <summary>
        /// 驾驶员姓名集合
        /// </summary>
        public List<string> ListstaffInfo = new List<string>();

        /// <summary>
        /// 需查询的表名
        /// </summary>
        public string UserTbale = "StaffInfo";

        /// <summary>
        /// 需查询的条件
        /// </summary>
        public string UserWhere = "StaffInfo_State='启动'";

        /// <summary>
        /// 需查询的列名Name
        /// </summary>
        public string Field = "StaffInfo_Name";

        /// <summary>
        /// 需查询的列名ID
        /// </summary>
        public string Fields = "StaffInfo_ID";

        /// <summary>
        /// 驾驶员分身证号
        /// </summary>
        public string StaffInfo_identitys = "StaffInfo_identity";



        public List<string> ICNumber = new List<string>();
        private void txtJSYName_KeyUp(object sender, KeyEventArgs e)
        {
            show();

        }
        /// <summary>
        /// 单击用户控件文本时调用
        /// </summary>
        public void show()
        {
            if (UserTbale == "StaffInfo")
            {
                //查询驾驶员
                UserClaer();

                if (!string.IsNullOrEmpty(txtJSYName.Text))
                {
                    //模糊查询驾驶员
                    string sql = "Select Distinct(" + Field + ") from " + UserTbale + " where " + UserWhere + " and " + Field + " like '%" + txtJSYName.Text.Trim() + "%'";
                    DataSet dataset = LinQBaseDao.Query(sql);
                    chkListBoxs.Items.Clear();
                    foreach (DataRow item in dataset.Tables[0].Rows)
                    {
                        chkListBoxs.Items.Add(item[Field].ToString());
                        chkListBoxs.SelectedIndex = 0;
                    }
                    listboxStaff_Names.Visible = false;
                    chkListBoxs.Visible = true;
                }
                else
                {
                    //全查询驾驶员
                    string sql = "Select Distinct(" + Field + ") from " + UserTbale + " where " + UserWhere;
                    DataSet dataset = LinQBaseDao.Query(sql);
                    chkListBoxs.Items.Clear();
                    listboxStaff_Names.Visible = false;
                    foreach (DataRow item in dataset.Tables[0].Rows)
                    {
                        chkListBoxs.Items.Add(item[Field].ToString());
                        chkListBoxs.SelectedIndex = 0;
                    }
                    chkListBoxs.Visible = true;
                }
            }
            else if (UserTbale == "CustomerInfo")
            {
                //查询公司名
                UserClaer();

                if (!string.IsNullOrEmpty(txtJSYName.Text))
                {
                    string sql = "Select Distinct(" + Field + ") from " + UserTbale + " where " + UserWhere + " and " + Field + " like '%" + txtJSYName.Text.Trim() + "%'";
                    DataSet dataset = LinQBaseDao.Query(sql);
                    chkListBoxs.Items.Clear();
                    chkListBoxs.Visible=false;
                    //公司名绑定
                    if (dataset.Tables[0].Rows.Count > 0)//如存在相同名称的驾驶员，则显示驾驶员身份证，根据身份证确定驾驶员信息
                    {
                        groupBox1.Visible = true;
                        listboxStaff_Names.Visible = true;
                        listboxStaff_Names.DataSource = dataset.Tables[0];
                        listboxStaff_Names.ValueMember = "CustomerInfo_Name";
                        listboxStaff_Names.DisplayMember = "CustomerInfo_Name";
                        listboxStaff_Names.BringToFront();
                    }
                }
                else
                {
                    string sql = "Select Distinct(" + Field + ") from " + UserTbale + " where " + UserWhere;
                    DataSet dataset = LinQBaseDao.Query(sql);
                    chkListBoxs.Items.Clear();
                    chkListBoxs.Visible = false;
                    //公司名绑定
                    if (dataset.Tables[0].Rows.Count > 0)//如存在相同名称的驾驶员，则显示驾驶员身份证，根据身份证确定驾驶员信息
                    {
                        groupBox1.Visible = true;
                        listboxStaff_Names.Visible = true;
                        listboxStaff_Names.DataSource = dataset.Tables[0];
                        listboxStaff_Names.ValueMember = "CustomerInfo_Name";
                        listboxStaff_Names.DisplayMember = "CustomerInfo_Name";
                        listboxStaff_Names.BringToFront();
                    }
                }
            }
            else
            {

            }
        }
        private void listboxStaff_Name_DoubleClick(object sender, EventArgs e)
        {

            //得到选中的驾驶员身份证号 根据身份证号码得到驾驶员编号
            if (UserTbale == "StaffInfo")
            {
                if (string.IsNullOrEmpty(listboxStaff_Names.Text.Trim()))
                {
                    MessageBox.Show("请选择驾驶员信息");
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(listboxStaff_Names.Text.Trim()))
                {
                    MessageBox.Show("请选择公司信息");
                    return;
                }
            }

            string sql = "select * from " + UserTbale + " where " + StaffInfo_identitys + " ='" + listboxStaff_Names.Text + "' and  " + UserWhere + "";
            DataSet ds = LinQBaseDao.Query(sql);
            if (ds.Tables[0].Rows.Count == 1)
            {
                if (UserTbale == "StaffInfo")
                {
                   staffInfo_Id += ds.Tables[0].Rows[0][Fields].ToString() + ",";//得到人员编号
                   CustomerInfo_GName = listboxStaff_Names.Text;//得到人员名称
                }
                else if (UserTbale == "CustomerInfo")
                {
                    staffInfo_Id = ds.Tables[0].Rows[0][Fields].ToString();//得到公司编号
                    CustomerInfo_GName = listboxStaff_Names.Text;//得到公司名称
                }
            }
            else
            {
                return;
            }
            this.Visible = false;//隐藏当前窗体
        }

        /// <summary>
        /// 新增按钮跳转
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddJSY_Click(object sender, EventArgs e)
        {
            if (UserTbale=="StaffInfo")
            {
                StaffInfoForm sff = new StaffInfoForm();
                sff.Show();
            }
            else if (UserTbale == "CustomerInfo") 
            {
                CustomerInfoForm form = new CustomerInfoForm();
                form.Show();
            }
            else{}
        }

        /// <summary>
        ///点击取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            UserClaer();
        }


        /// <summary>
        /// 对控件数据进行清空
        /// </summary>
        public void UserClaer()
        {
            chkListBoxs.Items.Clear();
            chkListBoxs.Visible = true;
        }

        /// <summary>
        /// 点击确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //查询驾驶员姓名
                if (UserTbale == "StaffInfo")
                {
                    if (listboxStaff_Names.Visible)
                    {
                        //调用事件
                        listboxStaff_Name_DoubleClick(sender, e);
                        this.Visible = false;
                        listboxStaff_Names.DataSource = null;
                        chkListBoxs.Visible = false;
                        return;
                    }
                    chkListBoxs.SendToBack();
                    if (chkListBoxs.SelectedItems.Count > 0)
                    {
                        string stafnames = "";//驾驶员名称
                        ListstaffInfo = new List<string>();
                        foreach (var item in chkListBoxs.CheckedItems)
                        {
                            ListstaffInfo.Add(item.ToString());//驾驶员名称集合
                            staffInfo_Name += item.ToString() + ",";//驾驶员名称
                            stafnames += "'" + item.ToString() + "',";
                        }
                        if (staffInfo_Name != null)
                        {
                            //当用户选择驾驶员名称后显示同名的ID
                            listboxStaff_Names.Visible = true;
                        }
                        if (ListstaffInfo.Count() > 0)
                        {
                            //staffInfo_Id = "";
                            foreach (var item in ListstaffInfo)
                            {
                                string sql = "Select * from " + UserTbale + " where " + UserWhere + " and " + Field + "='" + item + "'";
                                DataSet ds = LinQBaseDao.Query(sql);
                                if (UserTbale == "StaffInfo")
                                {
                                    if (ds.Tables[0].Rows.Count <= 0)
                                    {
                                        DialogResult digresult = MessageBox.Show("不存在该驾驶员，是否进行登记？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                                        if (DialogResult.OK == digresult)
                                        {
                                            StaffInfoForm sff = new StaffInfoForm();
                                            sff.Show();
                                        }
                                        else
                                        {
                                            staffInfo_Name = "";
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                }

                                if (!string.IsNullOrEmpty(StaffInfo_identitys))
                                {
                                    if (ds.Tables[0].Rows.Count > 1)//如存在相同名称的驾驶员，则显示驾驶员身份证，根据身份证确定驾驶员信息
                                    {
                                        groupBox1.Visible = true;
                                        listboxStaff_Names.Visible = true;
                                        listboxStaff_Names.DataSource = ds.Tables[0];
                                        listboxStaff_Names.ValueMember = Fields;
                                        listboxStaff_Names.DisplayMember = StaffInfo_identitys;
                                        listboxStaff_Names.BringToFront();
                                    }
                                    else
                                    {
                                        staffInfo_Id += ds.Tables[0].Rows[0][0]+",";
                                        this.Visible = false;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (UserTbale == "CustomerInfo")
                {
                    /////////////////查询公司名///////////////////////////////
                    CustomerInfo_GName = "";
                    if (listboxStaff_Names.Visible)
                    {
                        //调用事件
                        listboxStaff_Name_DoubleClick(sender, e);
                        this.Visible = false;
                        listboxStaff_Names.DataSource = null;
                        return;
                    }
                    chkListBoxs.SendToBack();
                    if (chkListBoxs.SelectedItems.Count > 0)
                    {
                        string stafnames = "";//变量名称
                        ListstaffInfo = new List<string>();
                        foreach (var item in chkListBoxs.CheckedItems)
                        {
                            ListstaffInfo.Add(item.ToString());//公司名称集合
                            staffInfo_Name += item.ToString() + ",";//公司名称
                            stafnames += "'" + item.ToString() + "',";
                        }
                        if (staffInfo_Name != null)
                        {
                            //当用户选择驾驶员名称后显示同名的ID
                            listboxStaff_Names.Visible = true;
                        }
                        if (ListstaffInfo.Count() > 0)
                        {
                            //staffInfo_Id = "";
                            foreach (var item in ListstaffInfo)
                            {
                                string sql = "Select * from " + UserTbale + " where " + UserWhere + " and " + Field + "='" + item + "'";
                                DataSet ds = LinQBaseDao.Query(sql);
                                if (ds.Tables[0].Rows.Count <= 0)
                                {
                                    DialogResult digresult = MessageBox.Show("不存在该驾驶员，是否进行登记？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                                    if (DialogResult.OK == digresult)
                                    {
                                        StaffInfoForm sff = new StaffInfoForm();
                                        sff.Show();
                                    }
                                    else
                                    {
                                        staffInfo_Name = "";
                                        return;
                                    }
                                }
                                else
                                {

                                }
                            }
                        }
                    }
                }

            }
            catch 
            {
              
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //点击清空按钮
            //清空
            staffInfo_Id = "";
            staffInfo_Name = "";
            StaffInfo_identity = "";
            txtJSYName.Text = "";
            CustomerInfo_GName = "";
            listboxStaff_Names.DataSource = null;
            ListstaffInfo.Clear();
        }

        private void txtJSYName_Click(object sender, EventArgs e)
        {
            show();
        }
    }
}
