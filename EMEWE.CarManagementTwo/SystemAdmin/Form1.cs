using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.DAL;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           

        }


        private string strselect="select ";
        private string strtabelName = "";
        private string strtabelNtext = "";

        Form ff = new Form();
        private void Form1_Load(object sender, EventArgs e)
        {
          

            GetUsetBingMethod();
        }
        private void GetUsetBingMethod()
        {
            string strsql = "SELECT d.name,f.value FROM syscolumns a inner join sysobjects d on a.id = d.id and d.xtype = 'U' and d.name <> 'sys.extended_properties' and d.name<>'sysdiagrams' left join sys.extended_properties f on a.id = f.major_id and f.minor_id = 0  where colorder=1 ";
            lb_CarTypeAttribute.DataSource = LinQBaseDao.Query(strsql).Tables[0];
            lb_CarTypeAttribute.DisplayMember = "name";
            lb_CarTypeAttribute.ValueMember = "value";
        }

        private void lb_CarTypeAttribute_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {

            }
        }
        private CheckedListBox clb = null;
        private DataTable dt=null;
        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Form f = new Form();
            f.Text = strtabelNtext;
            f.Name = strtabelName;
            f.Width = 200;
            f.Height = 150;
            f.Show();
            f.TopLevel = false;
            f.Visible = true;
            this.groupBox2.Controls.Add(f);

            string strsql = String.Format("select a.name,g.value from syscolumns a  left join systypes b on a.xtype=b.xusertype inner join sysobjects d on a.id=d.id and d.xtype='U' and d.name='{0}' left join sys.extended_properties g on a.id=g.major_id AND a.colid = g.minor_id order by a.id,a.colorder", strtabelName);
             dt= LinQBaseDao.Query(strsql).Tables[0];
            dt.Rows[0]["name"] = "*";
            dt.Rows[0]["value"] = "（*）所有字段";

            clb = new CheckedListBox();
          
            clb.DataSource = dt;
            clb.DisplayMember = "value";
            clb.ValueMember = "name";
            f.Controls.Add(clb); 
           
            clb.Dock = DockStyle.Fill;
            clb.DoubleClick += new System.EventHandler(this.checkedListBox1_Click); 


           
        }

        private void lb_CarTypeAttribute_Click(object sender, EventArgs e)
        {
            strtabelName = lb_CarTypeAttribute.SelectedValue.ToString();
            strtabelNtext = lb_CarTypeAttribute.Text;
        }

        private void checkedListBox1_Click(object sender, EventArgs e)
        {

            if (this.clb.SelectedValue != null)
            {
                if (this.clb.SelectedValue.ToString() == "*")
                {
                    GetvalueMethod(true);

                }
                else
                { 
                GetvalueMethod(false);
                }
            }
             
        }
        private void GetvalueMethod(bool rbool)
        {
            string checkedText = string.Empty;
            for (int i = 0; i < clb.Items.Count; i++)
            {
                if (rbool)
                {
                    this.clb.SetSelected(i, true);
                    checkedText += (String.IsNullOrEmpty(checkedText) ? "" : ",") + this.clb.SelectedValue.ToString() + " as " + this.clb.Text.ToString();
                }
                else
                {
                    if (this.clb.GetItemChecked(i))
                    {
                        this.clb.SetSelected(i, true);
                        checkedText += (String.IsNullOrEmpty(checkedText) ? "" : ",") + this.clb.SelectedValue.ToString() + " as " + this.clb.Text.ToString();
                    }
                }
            }
            textBox1.Text = String.Format("select {0} ", checkedText) + "\r\n" + String.Format(" from {0}", strtabelName);
        }
    
     

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
       dataGridView1.DataSource = LinQBaseDao.Query(textBox1.Text).Tables[0];
        }

       

       

     
      



        
       
    }
}
