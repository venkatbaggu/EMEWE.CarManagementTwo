using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EMEWE.CarManagement.MyControl
{
    public partial class Execlbar : UserControl
    {
        public Execlbar()
        {
            InitializeComponent();
        }

        public static bool ISdao = true;
        public static int Count = 0;
        public static string fileName="";
        public static int i = 0;
        private void btnSet_Click(object sender, EventArgs e)
        {
            if (btnSet.Text == "取消导出")
            {
                ISdao = false;
            }
            else
            {
                btnSet.Text = "取消导出";
            }
        }
           
        private void Execlbar_Load(object sender, EventArgs e)
        {
            this.progressBar1.Maximum = Count;
            progressBar1.Value = i;
            lbfname.Text = "正在导出：" + fileName;
        }

    }
}
