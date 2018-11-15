using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class MessageBoxForm : Form
    {
        public MessageBoxForm()
        {
            InitializeComponent();
        }
         string strinfo = "";
         public MessageBoxForm(string str)
        {
            InitializeComponent();
            strinfo = str;
        }
        Timer timer = new Timer();
        private void MessageBoxForm_Load(object sender, EventArgs e)
        {
            label1.Text = strinfo;
            timer.Interval = 2000; //5秒启动
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Enabled = true;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
            }
        }
        private void MessageBoxForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
            }
        }
    }
}
