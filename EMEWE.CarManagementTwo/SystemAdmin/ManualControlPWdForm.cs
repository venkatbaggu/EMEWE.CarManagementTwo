using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.HelpClass;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class ManualControlPWdForm : Form
    {
        public ManualControlPWdForm()
        {
            InitializeComponent();
        }

        public MainForm mf;

        /// <summary>
        /// 手动开闸管理  登录事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void btnControlPwd_Click(object sender, EventArgs e)
        //{
            //// 获取密码
            //string CurrentCOntrolPwd = this.txtCurrentControlPWD.Text.Trim();
            //// 写入common类里
            //common.CurrentControlPWD = CurrentCOntrolPwd;
            //// 调用是否打开手动开闸管理界面的方法
            //mf.SetgbManualControl();

            //// 启动手动开闸后隐藏开闸登录
            //this.Hide();
        //}

    }
}
