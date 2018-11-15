using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.SystemAdmin;
using EMEWE.CarManagement.Commons.CommonClass;

namespace EMEWE.CarManagement.MyControl
{
    public partial class MyUserTreeView : UserControl
    {
        public MyUserTreeView()
        {
            InitializeComponent();
        }

        public string StaffInfo_ID = "";
        public string StaffInfo_Name = "";

        /// <summary>
        /// 返回选中列的ID
        /// </summary>
        public string S_ID = "";
 
        /// <summary>
        /// 返回选中字段一
        /// </summary>
        public string S_Name = "";
        // <summary>
        /// 返回选中字段二
        /// </summary>
        public string S_Indes = "";
        // <summary>
        /// 返回选中字段三
        /// </summary>
        public string S_phone = "";

        /// <summary>
        /// 查询数据条数
        /// </summary>
        public int pagecount = 10;
        /// <summary>
        /// SQL语句
        /// </summary>
        public string sqlwhere = "";
        public string sqlwhere1 = "";
        public string sqlwhere2 = "";
        /// <summary>
        /// 查询方法
        /// </summary>
        public void StaffInfo_Select()
        {
            string strcv = "";
            sqlwhere1 = "";
            sqlwhere2 = "";
            SqlWhere();
            tv_StaffInfo.Nodes.Clear();

            if (sqlwhere != "")
            {
                DataSet ds = LinQBaseDao.Query(sqlwhere);
                TreeNode nodeTemp = null;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    SqlWhere1(ds.Tables[0].Rows[i][0].ToString());
                    sqlwhere1 += sqlwhere2;
                    DataSet dst = LinQBaseDao.Query(sqlwhere1);
                    if (dst.Tables[0].Rows.Count > 0)
                    {
                        nodeTemp = new TreeNode();
                        if (!string.IsNullOrEmpty(CommonalityEntity.tabcom2))
                        {
                            nodeTemp.Name = ds.Tables[0].Rows[i][1].ToString();
                            nodeTemp.Tag = ds.Tables[0].Rows[i][2].ToString();
                        }
                        else
                        {
                            nodeTemp.Tag = ds.Tables[0].Rows[i][1].ToString();
                        }
                        nodeTemp.Text = ds.Tables[0].Rows[i][0].ToString();

                    }
                    //查询是否有相同姓名驾驶员

                    if (dst.Tables[0].Rows.Count > 1)
                    {
                        TreeNode nodeTemps;
                        for (int j = 0; j < dst.Tables[0].Rows.Count; j++)
                        {
                            //添加子节点
                            nodeTemps = new TreeNode();
                            if (!string.IsNullOrEmpty(CommonalityEntity.tabcom2))
                            {
                                nodeTemps.Text = dst.Tables[0].Rows[j][1].ToString();
                                string str = dst.Tables[0].Rows[j][2].ToString();
                                nodeTemps.Tag = dst.Tables[0].Rows[j][2].ToString();
                            }
                            else
                            {
                                nodeTemps.Tag = dst.Tables[0].Rows[j][1].ToString();
                            }
                            nodeTemps.Name = dst.Tables[0].Rows[j][0].ToString();
                            nodeTemp.Nodes.Add(nodeTemps);
                        }
                        if (string.IsNullOrEmpty(strcv))
                        {
                            strcv = "'" + ds.Tables[0].Rows[i][0].ToString() + "'";
                        }
                        else
                        {
                            strcv += ",'" + ds.Tables[0].Rows[i][0].ToString() + "'";
                        }
                        sqlwhere2 = " and " + CommonalityEntity.tabcom1 + " not in(" + strcv + " )";
                    }
                    if (dst.Tables[0].Rows.Count > 0)
                    {
                        tv_StaffInfo.Nodes.Add(nodeTemp);
                    }

                }
            }

        }



        private void SqlWhere()
        {
            if (!string.IsNullOrEmpty(CommonalityEntity.tabcom1) && !string.IsNullOrEmpty(CommonalityEntity.tablename) && !string.IsNullOrEmpty(CommonalityEntity.tabid))
            {
                sqlwhere = "select top ("+pagecount+") ";
                sqlwhere += CommonalityEntity.tabcom1;
                if (!string.IsNullOrEmpty(CommonalityEntity.tabcom2))
                {
                    sqlwhere += ", " + CommonalityEntity.tabcom2;
                }
                sqlwhere += ", " + CommonalityEntity.tabid;
                sqlwhere += " from " + CommonalityEntity.tablename;
                if (!string.IsNullOrEmpty(CommonalityEntity.strlike))
                {
                    sqlwhere += " where " + CommonalityEntity.tabcom1 + " like '%" + CommonalityEntity.strlike + "%'";
                }
            }
            else
            {
                sqlwhere = "";
            }

        }

        private void SqlWhere1(string str)
        {
            if (!string.IsNullOrEmpty(CommonalityEntity.tabcom1) && !string.IsNullOrEmpty(CommonalityEntity.tablename) && !string.IsNullOrEmpty(CommonalityEntity.tabid))
            {
                sqlwhere1 = "select  ";
                sqlwhere1 += CommonalityEntity.tabcom1;
                if (!string.IsNullOrEmpty(CommonalityEntity.tabcom2))
                {
                    sqlwhere1 += ", " + CommonalityEntity.tabcom2;
                }
                sqlwhere1 += ", " + CommonalityEntity.tabid;
                sqlwhere1 += " from " + CommonalityEntity.tablename;
                sqlwhere1 += " where " + CommonalityEntity.tabcom1 + " = '" + str + "'";
            }
            else
            {
                sqlwhere1 = "";
            }

        }

        private void btncloes_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            tv_StaffInfo.Nodes.Clear();
        }

        /// <summary>
        /// 点击确定组件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnqueding_Click(object sender, EventArgs e)
        {
            if (tv_StaffInfo.Nodes.Count > 0)
            {
                if (tv_StaffInfo.SelectedNode != null)
                {

                    S_ID = tv_StaffInfo.SelectedNode.Tag.ToString();

                    string str = comunt();
                    if (str != "")
                    {
                        DataTable dt = LinQBaseDao.Query(str).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            S_phone = dt.Rows[0][0].ToString();
                        }
                    }
                    if (tv_StaffInfo.SelectedNode.Parent == null)//父节点
                    {
                        S_Name = tv_StaffInfo.SelectedNode.Text;
                        S_Indes = tv_StaffInfo.SelectedNode.Name;
                    }
                    else
                    {
                        S_Name = tv_StaffInfo.SelectedNode.Name;
                        S_Indes = tv_StaffInfo.SelectedNode.Text;
                    }
                }
                tv_StaffInfo.Nodes.Clear();
            }
            this.Visible = false;

        }


        private void tv_StaffInfo_DoubleClick(object sender, EventArgs e)
        {
            if (tv_StaffInfo.Nodes.Count > 0)
            {
                if (tv_StaffInfo.SelectedNode != null)
                {
                    S_ID = tv_StaffInfo.SelectedNode.Tag.ToString();
                    string str = comunt();
                    if (str != "")
                    {
                        DataTable dt = LinQBaseDao.Query(str).Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            S_phone = dt.Rows[0][0].ToString();
                        }
                    }
                    if (tv_StaffInfo.SelectedNode.Parent == null)//父节点
                    {
                        S_Name = tv_StaffInfo.SelectedNode.Text;
                        S_Indes = tv_StaffInfo.SelectedNode.Name;
                    }
                    else
                    {
                        S_Name = tv_StaffInfo.SelectedNode.Name;
                        S_Indes = tv_StaffInfo.SelectedNode.Text;
                    }
                }
                tv_StaffInfo.Nodes.Clear();
            }
            this.Visible = false;
        }

        private string comunt()
        {
            string str = "";

            if (CommonalityEntity.tabcom3 != "")
            {
                str = "select " + CommonalityEntity.tabcom3 + " from " + CommonalityEntity.tablename + " where " + CommonalityEntity.tabid + " =" + S_ID;
            }
            return str;
        }

    }
}
