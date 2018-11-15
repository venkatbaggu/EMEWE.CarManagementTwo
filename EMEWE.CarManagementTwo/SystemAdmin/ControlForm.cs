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

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class ControlForm :Form
    {
        public ControlForm()
        {
            InitializeComponent();
        }
        public MainForm mf ;
        public static ControlForm cf=null;
        private void btnSave_Click(object sender, EventArgs e)
        {
            string password = txtPassWord.Text.Trim();
            if (!string.IsNullOrEmpty(password))
            {
                if (password == SystemClass.ControlPWD)
                {
                    if (btnSave.Text == "打开")
                    {
                        CommonalityEntity.iscontrol = 1;
                        btnSave.Text = "关闭";
                        mf.INOUTCONTORL();

                    }
                    else
                    {
                        CommonalityEntity.iscontrol = 2;
                        btnSave.Text = "打开";
                        mf.INOUTCONTORL();

                    }
                }
                else
                {
                    MessageBox.Show(this, "密码错误！");
                }
            }
        }

        private void ControlForm_Load(object sender, EventArgs e)
        {
            if (CommonalityEntity.iscontrol==1)
            {
                btnSave.Text = "关闭";
            }
            else
            {
                btnSave.Text = "打开";
            }
        }

        private void ControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            ControlForm.cf = null;
        }

        private void txtPassWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave_Click(sender, e);
            }
        }

        private void btnSave_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSave_Click(sender, e);
            }
        }
    }
}
