using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using EMEWE.CarManagement.Commons.CommonClass;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class ControlPassForm : Form
    {
        public ControlPassForm()
        {
            InitializeComponent();
        }

        private void btnEditPwd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPwd1.Text.Trim().ToLower() != txtpwd2.Text.Trim().ToLower())
                {
                    MessageBox.Show(this, "新密码不一致!");
                    return;
                }
                if (string.IsNullOrEmpty(txtpwd2.Text.Trim()))
                {
                    MessageBox.Show(this, "新密码不能为空");
                    return;
                }

                if (string.IsNullOrEmpty(txtOlePwd.Text.Trim()))
                {
                    MessageBox.Show(this, "旧密码不能为空");
                    return;
                }

                string controlpwd = txtpwd2.Text.Trim();
                string oldpwd = "";
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
                        oldpwd = xe.GetAttribute("ControlPWD").ToString();
                        if (oldpwd == txtOlePwd.Text.Trim())
                        {
                            xe.SetAttribute("ControlPWD", controlpwd);//
                        }
                        else
                        {
                            MessageBox.Show(this, "旧密码输入错误");
                            return;
                        }
                    }
                    xmlDoc.Save(filepath);
                    MessageBox.Show(this, "修改成功！");
                }
                SystemClass.ControlPWD = controlpwd;
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("手动控制密码修改 btnEditPwd_Click()");
            }
        }
    }
}
