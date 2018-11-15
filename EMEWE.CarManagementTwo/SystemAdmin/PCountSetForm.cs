using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.Commons.CommonClass;
using System.Xml;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class PCountSetForm : Form
    {
        public PCountSetForm()
        {
            InitializeComponent();
        }

        private void PCountSetForm_Load(object sender, EventArgs e)
        {
            txtCount.Text = SystemClass.postionCount.ToString();
        }

        private void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCount.Text.Trim()))
                {
                    MessageBox.Show(this, "门岗数量不能为空");
                    return;
                }
                string str = txtCount.Text.Trim();
                int count = Convert.ToInt32(str);

                string filepath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\SystemSet.xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(filepath);
                XmlNode xn = xmlDoc.SelectSingleNode("param");//查找<bookstore>
                XmlNodeList xnl = xn.ChildNodes;
                if (xnl.Count > 0)
                {
                    foreach (XmlNode xnf in xnl)
                    {
                        XmlElement xe = (XmlElement)xnf;
                        xe.SetAttribute("PCount", count.ToString());//
                    }
                    xmlDoc.Save(filepath);
                    MessageBox.Show(this, "设置成功！");
                }
                SystemClass.postionCount = count;
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("PCountSetForm. btnSet_Click()异常：");
            }
        }
    }
}
