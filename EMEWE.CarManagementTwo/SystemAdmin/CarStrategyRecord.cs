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
    public partial class CarStrategyRecord : Form
    {
        public CarStrategyRecord()
        {
            InitializeComponent();
        }

        private void CarStrategyRecord_Load(object sender, EventArgs e)
        {
            BindCarType();
        }
        #region 通行策略
        private void BindCarType()
        {
            stawayType.DataSource = LinQBaseDao.Query("select distinct (DrivewayStrategy_Name) from DrivewayStrategy where DrivewayStrategy_State='启动'").Tables[0];
            stawayType.DisplayMember = "DrivewayStrategy_Name";
            stawayType.ValueMember = "DrivewayStrategy_Name";
            stawayType.SelectedIndex = 0;
        }
        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //查询出车辆类型的详细策略
            treeView1.Nodes.Clear();
            try
            {
                DataTable table1 = LinQBaseDao.Query("select DrivewayStrategy_Sort,DrivewayStrategy_ID,DrivewayStrategy_Driveway_ID from DrivewayStrategy where DrivewayStrategy_Name='" + stawayType.Text.Trim() + "' and   DrivewayStrategy_State='启动' order by DrivewayStrategy_Sort ").Tables[0];
                TreeNode tr1;
                for (int i = 0; i < table1.Rows.Count; i++)
                {
                    tr1 = new TreeNode();
                    tr1.Tag = table1.Rows[i]["DrivewayStrategy_ID"].ToString();
                    string str = table1.Rows[i]["DrivewayStrategy_Sort"].ToString();
                    DataTable dt = LinQBaseDao.Query("select Driveway_Name,Position_Name,Driveway_Type from View_DrivewayPosition where Driveway_ID= " + table1.Rows[i]["DrivewayStrategy_Driveway_ID"].ToString()).Tables[0];
                    if (dt.Rows.Count>0)
                    {
                        str += " " + dt.Rows[0]["Position_Name"].ToString() + " " + dt.Rows[0]["driveway_name"].ToString() + " " + dt.Rows[0]["driveway_type"].ToString();
                    }
                    tr1.Text = str;
                    treeView1.Nodes.Add(tr1);
                }
            }
            catch 
            {

            }
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int checkNodes = Convert.ToInt32(treeView1.SelectedNode.Tag);
            if (checkNodes >= 1)
            {
                treeView2.Nodes.Clear();
                try
                {
                    DataTable table1 = LinQBaseDao.Query("select ManagementStrategy_ID,ManagementStrategy_Name from dbo.ManagementStrategy where ManagementStrategy_DrivewayStrategy_ID=" + checkNodes + " and ManagementStrategy_State='启动'").Tables[0];
                    TreeNode tr1;
                    for (int i = 0; i < table1.Rows.Count; i++)
                    {
                        tr1 = new TreeNode();
                        tr1.Tag = table1.Rows[i]["ManagementStrategy_ID"].ToString();
                        tr1.Text = table1.Rows[i]["ManagementStrategy_Name"].ToString();
                        treeView2.Nodes.Add(tr1);
                    }
                }
                catch 
                {

                }
            }
        }
    }
}
