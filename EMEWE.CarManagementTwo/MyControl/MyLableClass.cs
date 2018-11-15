using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;


namespace EMEWE.CarManagement.MyControl
{
    class MyLableClass : Label
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.ForeColor = Color.LimeGreen;

        }
    }
}
