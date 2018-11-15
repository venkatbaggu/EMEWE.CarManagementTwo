using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.Commons.CommonClass;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            CommonalityEntity.tablename = "CustomerInfo";
            CommonalityEntity.tabcom1 = "CustomerInfo_Name";
            CommonalityEntity.tabcom2 = "";
            CommonalityEntity.tabid = "CustomerInfo_ID";
            CommonalityEntity.strlike = textBox2.Text.Trim();
            myUserTreeView2.StaffInfo_Select();
            string sdf = myUserTreeView2.S_Name;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            CommonalityEntity.tablename = "StaffInfo";
            CommonalityEntity.tabcom1 = "StaffInfo_Name";
            CommonalityEntity.tabcom2 = "StaffInfo_Identity";
            CommonalityEntity.tabid = "StaffInfo_ID";
            CommonalityEntity.strlike = textBox1.Text.Trim();
            myUserTreeView1.StaffInfo_Select();
            string sdf = myUserTreeView1.S_Name;
            string re = myUserTreeView1.S_ID;
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            CommonalityEntity.tablename = "StaffInfo";
            CommonalityEntity.tabcom1 = "StaffInfo_Name";
            CommonalityEntity.tabcom2 = "StaffInfo_Identity";
            CommonalityEntity.tabid = "StaffInfo_ID";
            CommonalityEntity.strlike = textBox1.Text.Trim();
            myUserTreeView1.StaffInfo_Select();
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            CommonalityEntity.tablename = "CustomerInfo";
            CommonalityEntity.tabcom1 = "CustomerInfo_Name";
            CommonalityEntity.tabcom2 = "";
            CommonalityEntity.tabid = "CustomerInfo_ID";
            CommonalityEntity.strlike = textBox2.Text.Trim();
            myUserTreeView2.StaffInfo_Select();
        }
    }
}
