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
using EMEWE.CarManagement.Entity;
using System.Linq.Expressions;

namespace EMEWE.CarManagement.SystemAdmin
{
    public partial class BlacklistSet : Form
    {
        public BlacklistSet()
        {
            InitializeComponent();
        }

        private void BlacklistSet_Load(object sender, EventArgs e)
        {
            userContext();
            button1.Enabled = false;
            button4.Enabled = false;
            treeViewPositionlist(tv_BackList.Nodes, "0");
        }

        /// <summary>
        /// 是否是系统字段
        /// </summary>
        private string Dictionary_IsSystem = "";
        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {

                btnSet.Enabled = true;
                btnSet.Visible = true;
                button1.Enabled = true;
                button1.Visible = true;
                button4.Enabled = true;
                button4.Visible = true;
            }
            else
            {
                btnSet.Visible = ControlAttributes.BoolControl("btnSet", "BlacklistSet", "Visible");
                btnSet.Enabled = ControlAttributes.BoolControl("btnSet", "BlacklistSet", "Enabled");

                button1.Visible = ControlAttributes.BoolControl("button1", "BlacklistSet", "Visible");
                button1.Enabled = ControlAttributes.BoolControl("button1", "BlacklistSet", "Enabled");

                button4.Visible = ControlAttributes.BoolControl("button4", "BlacklistSet", "Visible");
                button4.Enabled = ControlAttributes.BoolControl("button4", "BlacklistSet", "Enabled");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSet_Click(object sender, EventArgs e)
        {
            try
            {
                string Sql = "select max(Dictionary_Value),MAX(Dictionary_Sort) from Dictionary where Dictionary_OtherID=(select Dictionary_ID from Dictionary where Dictionary_Value='09')";
                DataSet ds = LinQBaseDao.Query(Sql);
                string Dictionary_Value = ds.Tables[0].Rows[0][0].ToString();
                string Dictionary_Sort = ds.Tables[0].Rows[0][1].ToString();
                Dictionary dic = new Dictionary();
                dic.Dictionary_Value = (Convert.ToInt32("0"+Dictionary_Value) + 1).ToString();
                dic.Dictionary_Sort = Convert.ToInt32(Dictionary_Sort) + 1;
                dic.Dictionary_Name = txtBackContext.Text;
                dic.Dictionary_OtherID = 15;
                dic.Dictionary_State = true;
                dic.Dictionary_Remark = null;
                dic.Dictionary_ISLower = false;
                dic.Dictionary_IsSystem = false;
                dic.Dictionary_Spare_int1 = Convert.ToInt32(txtDictionary_Spare_int1.Text.Trim());
                dic.Dictionary_Spare_int2 = Convert.ToInt32(txtDictionary_Spare_int2.Text.Trim());
                dic.Dictionary_Spare_vchar1 = null;
                dic.Dictionary_Spare_vchar2 = null;
                if (DictionaryDAL.InsertOneDictionary(dic))
                {
                    MessageBox.Show("新增成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    treeViewPositionlist(tv_BackList.Nodes, "0");
                    Dictionary_Cler();
                    button4.Enabled = false;
                    button1.Enabled = false;
                }
                else
                {
                    MessageBox.Show("新增失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch 
            {

                
            }
            finally
            {
                string strContent1 = "新增黑名单状态为： " + this.txtBackContext.Text.Trim();
                CommonalityEntity.WriteLogData("添加", strContent1, CommonalityEntity.USERNAME);//添加操作日志  
            }



        }

        public void Dictionary_Cler()
        {
            txtBackContext.Text = "";
            txtBackContext.Tag = "";
            txtDictionary_Spare_int1.Text = "";
            txtDictionary_Spare_int2.Text = "";
        }
        protected void treeViewPositionlist(TreeNodeCollection node, string MtID)
        {
            node.Clear();
            ///获取当前拥有的权限
            DataTable table;
            table = LinQBaseDao.Query("select * from Dictionary where Dictionary_OtherID=(select Dictionary_ID from Dictionary where Dictionary_Value='09') order by Dictionary_Sort asc").Tables[0];
            TreeNode nodeTemp;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                nodeTemp = new TreeNode();
                nodeTemp.Tag = table.Rows[i]["Dictionary_ID"];
                if (table.Rows[i]["Dictionary_Name"] != null)
                {
                    nodeTemp.Text = table.Rows[i]["Dictionary_Name"].ToString();
                    nodeTemp.Name = table.Rows[i]["Dictionary_ID"].ToString();
                }
                if (Convert.ToBoolean(table.Rows[i]["Dictionary_State"]) == false)
                {
                    nodeTemp.BackColor = Color.Yellow;
                }
                if (nodeTemp != null)
                {
                    node.Add(nodeTemp);  //加入节点 
                }
            }
        }

        private void BackList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)//判断你点的是不是右键
            {
                Point ClickPoint = new Point(e.X, e.Y);
                TreeNode CurrentNode = tv_BackList.GetNodeAt(ClickPoint);
                if (CurrentNode != null)//判断你点的是不是一个节点
                {
                    tv_BackList.SelectedNode = CurrentNode;//选中这个节点
                    //BackList.SelectedNode.ForeColor =Color.Lime; ;//节点颜色设置
                    ToolStripMenuItem[] formMenuItemList = new ToolStripMenuItem[4];
                    formMenuItemList[0] = new ToolStripMenuItem("上移", null, new EventHandler(menuinfo_next));
                    formMenuItemList[1] = new ToolStripMenuItem("下移", null, new EventHandler(menuinfonext));
                    formMenuItemList[2] = new ToolStripMenuItem("刷新", null, new EventHandler(InitMenu));//---修改的方法为添加
                    formMenuItemList[3] = new ToolStripMenuItem("删除", null, new EventHandler(btn_Delete_Click));

                    ContextMenuStrip formMenu = new ContextMenuStrip();
                    formMenu.Items.AddRange(formMenuItemList);
                    this.ContextMenuStrip = formMenu;
                }
            }
        }
        private void menuinfo_next(object sender,EventArgs e)
        {
            if (tv_BackList.SelectedNode.Tag.ToString() != null)
            {
                int menuID = Convert.ToInt32(tv_BackList.SelectedNode.Tag.ToString());
                DataTable table = LinQBaseDao.Query("select * from dbo.MenuInfo where menu_id='" + menuID + "'").Tables[0];
                int MenuOtherId = Convert.ToInt32(table.Rows[0]["Menu_OtherID"]);
                int MenuOderId = Convert.ToInt32(table.Rows[0]["Menu_Order"]);
                int nextIderId = MenuOderId - 1;
                if (MenuOderId > 1)//Menu_OderID为1的不能上移
                {
                    LinQBaseDao.Query("update MenuInfo set Menu_Order=" + MenuOderId + " where Menu_OtherID=" + MenuOtherId + " and Menu_Order=" + (MenuOderId - 1) + "");
                    LinQBaseDao.Query("update MenuInfo set Menu_Order=" + nextIderId + " where Menu_ID=" + menuID);
                }
                tv_BackList.Nodes.Clear();
                InitMenu(sender,e);//重新绑定控件
            }
            else
                MessageBox.Show("请选择节点！");
        }
        private void menuinfonext(object sender, EventArgs e)
        {
            if (tv_BackList.SelectedNode.Tag.ToString() != null)
            {
                int menuID = Convert.ToInt32(tv_BackList.SelectedNode.Tag.ToString());
                DataTable table = LinQBaseDao.Query("select * from dbo.MenuInfo where menu_id='" + menuID + "'").Tables[0];
                int MenuOtherId = Convert.ToInt32(table.Rows[0]["Menu_OtherID"]);
                int MenuOderId = Convert.ToInt32(table.Rows[0]["Menu_Order"]);
                int nextIderId = MenuOderId + 1;
                if (LinQBaseDao.Query("select * from dbo.MenuInfo where Menu_OtherID=" + MenuOtherId + " and Menu_Order=" + (MenuOderId + 1) + "").Tables[0].Rows.Count >= 1)//Menu_OderID当前施法最大
                {
                    LinQBaseDao.Query("update MenuInfo set Menu_Order=" + MenuOderId + " where Menu_OtherID=" + MenuOtherId + " and Menu_Order=" + (MenuOderId + 1) + "");
                    LinQBaseDao.Query("update MenuInfo set Menu_Order=" + nextIderId + " where Menu_ID=" + menuID);
                }
                tv_BackList.Nodes.Clear();
                InitMenu(sender, e);//重新绑定控件
            }
            else
                MessageBox.Show("请选择节点！");
        }
        private void InitMenu(object sender, EventArgs e)
        {
            tv_BackList.Nodes.Clear();
            treeViewPositionlist(tv_BackList.Nodes, "0");
        }
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (tv_BackList.SelectedNode.Tag.ToString() != null)
                {
                    int menuID = Convert.ToInt32(tv_BackList.SelectedNode.Tag.ToString());
                    #region --查询模块--
                    if (menuID <= -1)
                    {
                        MessageBox.Show("请选择节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    bool Menu_Enabledtxt = false;
                    bool cob_Menu_Visibletxt = false;
                    Expression<Func<MenuInfo, bool>> funviewinto = n => n.Menu_ControlName == tv_BackList.SelectedNode.Name.ToString();
                    foreach (var n in MenuInfoDAL.Query(funviewinto))
                    {

                        if (n.Menu_Enabled != null)
                        {
                            Menu_Enabledtxt = Convert.ToBoolean(n.Menu_Enabled);
                        }
                        if (n.Menu_Visible != null)
                        {
                            //字典备注
                            cob_Menu_Visibletxt = Convert.ToBoolean(n.Menu_Visible);
                        }
                        break;
                    }

                    #endregion

                    //1.删除菜单时，检查菜单是否配置；若配置了不能删除，需要先解除配置
                    //检测删除的节点是否存在
                    //检测菜单是否可以删除（等待）
                    //检测菜单是否可以编辑
                    //检测菜单是否已经配置
                    //检测是否有删除菜单的权限（等待）不可编辑
                    if (Convert.ToInt32(tv_BackList.SelectedNode.Tag.ToString()) > 0)
                    {
                        Expression<Func<MenuInfo, bool>> fun = n => n.Menu_ID == Convert.ToInt32(tv_BackList.SelectedNode.Tag.ToString());
                    }
                    DataTable table = LinQBaseDao.Query("select * from MenuInfo where Menu_OtherID=" + menuID + "").Tables[0];
                    if (table.Rows.Count > 0)
                    {
                        MessageBox.Show("请先删除子节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (Menu_Enabledtxt == true || cob_Menu_Visibletxt == true)
                    {
                        LinQBaseDao.Query("update  menuinfo set Menu_MenuType_ID=NULL where   Menu_ID=" + menuID + "");
                        LinQBaseDao.Query("DELETE  PermissionsInfo where Permissions_Menu_ID=" + menuID + "");
                        LinQBaseDao.Query("DELETE  menuinfo where  Menu_ID=" + menuID + "");
                        MessageBox.Show("删除成功！");
                        CommonalityEntity.WriteLogData("删除", "btn_Delete_Click", CommonalityEntity.USERNAME);//添加日志
                    }
                    tv_BackList.Nodes.Clear();
                    treeViewPositionlist(tv_BackList.Nodes, "0");
                }
                else
                    MessageBox.Show("请选择节点！");
            }
            catch 
            {
                MessageBox.Show("无法删除！");
                CommonalityEntity.WriteTextLog("菜单管理MenuInfoManager btn_Delete_Click" );
            }
        }

        private void BackList_MouseLeave(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 单击修改组件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                button1.Enabled = false;
                if (txtDictionary_Spare_int1.Text.Trim() == "")
                {
                    MessageBox.Show("请输入每几次升级的次数");
                }
                else if (txtDictionary_Spare_int2.Text.Trim() == "")
                {
                    MessageBox.Show("请输入每几次降级的次数");
                }
                else
                {
                    //开始修改
                    Expression<Func<Dictionary, bool>> p = n => n.Dictionary_ID == int.Parse(this.txtBackContext.Tag.ToString());
                    Action<Dictionary> ap = s =>
                    {
                        s.Dictionary_Name = txtBackContext.Text;
                        s.Dictionary_OtherID = 15;
                        s.Dictionary_State = true;
                        s.Dictionary_Remark = null;
                        s.Dictionary_ISLower = false;
                        s.Dictionary_IsSystem = false;
                        s.Dictionary_Spare_int1 = Convert.ToInt32(txtDictionary_Spare_int1.Text.Trim());
                        s.Dictionary_Spare_int2 = Convert.ToInt32(txtDictionary_Spare_int2.Text.Trim());
                        s.Dictionary_Spare_vchar1 = null;
                        s.Dictionary_Spare_vchar2 = null;
                    };
                    if (DictionaryDAL.Update(p, ap))
                    {
                        MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        treeViewPositionlist(tv_BackList.Nodes, "0");
                        Dictionary_Cler();
                    }
                    else
                    {
                        MessageBox.Show("修改失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }

            }
            catch
            {
            }
        }

        /// <summary>
        /// 单击删除组件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (tv_BackList.SelectedNode != null)
                {
                    if (txtBackContext.Text != "")
                    {
                        if (Dictionary_IsSystem == "否")
                        {
                            string Sql = "delete Dictionary where Dictionary_ID=" + txtBackContext.Tag;
                            Expression<Func<Dictionary, bool>> funuserinfo = n => n.Dictionary_ID == int.Parse(this.txtBackContext.Tag.ToString());
                            //Dictionary_IsSystem
                            if (!DictionaryDAL.DeleteToMany(funuserinfo))
                            {
                                MessageBox.Show("此状态在使用中，不能删除", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show("删除成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                treeViewPositionlist(tv_BackList.Nodes, "0");
                                Dictionary_Cler();
                            }
                        }
                        else
                        {
                            MessageBox.Show("此状态为系统所有，不能删除");
                        }

                    }
                    else
                    {
                        MessageBox.Show("此状态为系统所有，不能修改");
                    }
                }
                else
                {
                    MessageBox.Show("请选择需要删除的黑名单状态名称");
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 选择节点时发生
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tv_BackList_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            button1.Enabled = true;
            button4.Enabled = true;
            string id = e.Node.Name;
            string Backlist_Sql = "select * from Dictionary where Dictionary_ID=" + id + " ";
            DataSet ds = LinQBaseDao.Query(Backlist_Sql);
            if ((bool)ds.Tables[0].Rows[0][8])
            {
                Dictionary_IsSystem = "是";
            }
            else
            {
                Dictionary_IsSystem = "否";
            }
            txtBackContext.Text = ds.Tables[0].Rows[0][1].ToString();
            txtBackContext.Tag = ds.Tables[0].Rows[0][0].ToString();
            txtDictionary_Spare_int1.Text = ds.Tables[0].Rows[0][9].ToString();
            txtDictionary_Spare_int2.Text = ds.Tables[0].Rows[0][10].ToString();
        }

        /// <summary>
        /// 下移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPagex_Click(object sender, EventArgs e)
        {
            try
            {
                int index = 0;
                if (tv_BackList.SelectedNode != null)
                {
                    int j = tv_BackList.SelectedNode.Index;
                    if ((tv_BackList.Nodes.Count - 1) == tv_BackList.SelectedNode.Index)
                    {
                        MessageBox.Show("已经是最下层的了");
                    }
                    else
                    {
                        //获取下一节点的ID
                        string Prev_ID = tv_BackList.SelectedNode.NextNode.Name;
                        //获取当前节点ID
                        string Selecte_ID = tv_BackList.SelectedNode.Name;
                        string SqlNode = "select Dictionary_Sort from Dictionary where Dictionary_OtherID=(select Dictionary_ID from Dictionary where Dictionary_Value='09') and Dictionary_ID=" + Selecte_ID + " or Dictionary_ID=" + Prev_ID + " order by Dictionary_Sort asc";
                        DataSet ds = LinQBaseDao.Query(SqlNode);
                        //修改上一节点的顺序号
                        Expression<Func<Dictionary, bool>> p = n => n.Dictionary_ID == int.Parse(Prev_ID);
                        Action<Dictionary> ap = s =>
                        {
                            s.Dictionary_Sort = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                        };
                        if (DictionaryDAL.Update(p, ap))
                        {
                            //修改一完成
                            index = 1;
                        }
                        if (index == 1)
                        {
                            //修改选中节点的顺序号
                            Expression<Func<Dictionary, bool>> ps = n => n.Dictionary_ID == int.Parse(Selecte_ID);
                            Action<Dictionary> aps = s =>
                            {
                                s.Dictionary_Sort = Convert.ToInt32(ds.Tables[0].Rows[1][0]);
                            };
                            if (DictionaryDAL.Update(ps, aps))
                            {
                                //修改一完成
                                index = 2;
                            }
                        }
                        if (index == 2)
                        {
                            treeViewPositionlist(tv_BackList.Nodes, "0");
                        }
                        else
                        {
                            MessageBox.Show("异常，无法换行");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请选择一项黑名单状态名");
                }
            }
            catch 
            {
                
            }
        }

        /// <summary>
        /// 上移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPages_Click(object sender, EventArgs e)
        {
            try
            {
                int index = 0;
                if (tv_BackList.SelectedNode != null)
                {
                    if (tv_BackList.SelectedNode.Index == 0)
                    {
                        MessageBox.Show("已经是最上层的了");
                    }
                    else
                    {
                        //获取上一节点的ID
                        string Prev_ID = tv_BackList.SelectedNode.PrevNode.Name;
                        //获取当前节点ID
                        string Selecte_ID = tv_BackList.SelectedNode.Name;

                        string SqlNode = "select Dictionary_Sort from Dictionary where Dictionary_OtherID=(select Dictionary_ID from Dictionary where Dictionary_Value='09') and Dictionary_ID=" + Prev_ID + " or Dictionary_ID=" + Selecte_ID + " order by Dictionary_Sort asc";
                        DataSet ds = LinQBaseDao.Query(SqlNode);
                        //修改上一节点的顺序号
                        Expression<Func<Dictionary, bool>> p = n => n.Dictionary_ID == int.Parse(Prev_ID);
                        Action<Dictionary> ap = s =>
                        {
                            s.Dictionary_Sort = Convert.ToInt32(ds.Tables[0].Rows[1][0]);
                        };
                        if (DictionaryDAL.Update(p, ap))
                        {
                            //修改一完成
                            index = 1;
                        }
                        if (index == 1)
                        {
                            //修改选中节点的顺序号
                            Expression<Func<Dictionary, bool>> ps = n => n.Dictionary_ID == int.Parse(Selecte_ID);
                            Action<Dictionary> aps = s =>
                            {
                                s.Dictionary_Sort = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                            };
                            if (DictionaryDAL.Update(ps, aps))
                            {
                                //修改一完成
                                index = 2;
                            }
                        }
                        if (index == 2)
                        {
                            treeViewPositionlist(tv_BackList.Nodes, "0");
                        }
                        else
                        {
                            MessageBox.Show("异常，无法换行");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("请选择一项黑名单状态名");
                }
            }
            catch 
            {
               
            }
        }

        /// <summary>
        /// 判断是否输入数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDictionary_Spare_int2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtDictionary_Spare_int2.Text.Trim() != "")
                {
                    int i = Convert.ToInt32(txtDictionary_Spare_int2.Text.Trim());
                }
            }
            catch
            {
                MessageBox.Show("请输入数字");
                txtDictionary_Spare_int2.Text = "";
            }
        }

        /// <summary>
        /// 判断输入是否是数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtDictionary_Spare_int1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtDictionary_Spare_int1.Text.Trim() != "")
                {
                    int i = Convert.ToInt32(txtDictionary_Spare_int1.Text.Trim());
                }
            }
            catch 
            {
                MessageBox.Show("请输入数字");
                txtDictionary_Spare_int1.Text = "";
            }
        }
    }
}
