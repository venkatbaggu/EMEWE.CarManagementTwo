using System;
using System.Drawing;
using System.Windows.Forms;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class PortFrom : Form
    {
        public PortFrom()
        {
            InitializeComponent();
        }
        public static PortFrom pr = null;
        public MainForm mf = new MainForm();
        bool isdown = false;
        /// <summary>
        /// 原来的鼠标点
        /// </summary>
        private Point _oldPoint;
        /// <summary>
        /// 原来窗口点
        /// </summary>
        private Point _oldForm;
        //然后写入鼠标进入事件、鼠标按下事件、鼠标松开事件、鼠标移动事件

        private void PortFrom_Load(object sender, EventArgs e)
        {
            this.Location = new Point(900, 20);
            lbstr.Size = new Size(5, 20);
        }

        private void PortFrom_FormClosing(object sender, FormClosingEventArgs e)
        {
            PortFrom.pr = null;
        }

        private void PortFrom_MouseDown(object sender, MouseEventArgs e)
        {
            isdown = true;
            _oldPoint = new Point();
            _oldPoint = e.Location;
            _oldForm = this.Location;

        }

        private void PortFrom_MouseUp(object sender, MouseEventArgs e)
        {
            isdown = false;
        }

        private void PortFrom_MouseMove(object sender, MouseEventArgs e)
        {
            if (isdown)
            {
                _oldForm.Offset(e.X - _oldPoint.X, e.Y - _oldPoint.Y);
                this.Location = _oldForm;
            }
        }

        private void lbstr_Click(object sender, EventArgs e)
        {
            if (CheckInfo.ci == null)
            {
                CheckInfo.ci = new CheckInfo();
                //CheckInfo.ci.mf = mf;
                CheckInfo.ci.Show();
            }
            else
            {
                CheckInfo.ci.Activate();
            }
        }

    }
}
