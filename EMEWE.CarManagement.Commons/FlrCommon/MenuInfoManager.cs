using EMEWE.CarManagement.Commons.CommonClass;
using EMEWE.CarManagement.DAL;
using EMEWE.CarManagement.Entity;
using System;
using System.Data;
using System.Drawing;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace EMEWE.CarManagement.Commons.FlrCommon
{
    public partial class MenuInfoManager : Form
    {
        public MenuInfoManager()
        {
            InitializeComponent();
        }
        private Point Position = new Point(0, 0);
        private void comboxMenuTypeOrderBind()
        {
            int maxID = Convert.ToInt32(LinQBaseDao.GetSingle("select max(MenuType_ID+1)  From dbo.MenuType"));
            int[] arr = new int[maxID + 1];
            for (int i = 0; i <= maxID; i++)
            {
                arr[i] = i + 1;
            }
            cmkMenuOderID.DataSource = arr;
        }
        /// <summary>
        /// 加载用户
        /// </summary>
        private void InitUser()
        {
            InitMenu();//绑定菜单
            Menu_ControlType();//绑定菜单类型
        }
        /// <summary>
        /// 初始化菜单
        /// </summary>
        protected void InitMenu()
        {
            //加载TreeView菜单    

            LoadNode(TV_MenuInfo.Nodes, "0");
        }

        /// <summary>
        /// 用户权限，添加删除，修改
        /// </summary>
        private void userContext()
        {
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {

                btn_Add.Enabled = true;
                btn_Add.Visible = true;
                btn_Delete.Enabled = true;
                btn_Delete.Visible = true;
                btn_Update.Enabled = true;
                btn_Update.Visible = true;
            }
            else
            {
                btn_Add.Visible = ControlAttributes.BoolControl("btn_Add", "MenuInfoManager", "Visible");
                btn_Add.Enabled = ControlAttributes.BoolControl("btn_Add", "MenuInfoManager", "Enabled");

                btn_Delete.Visible = ControlAttributes.BoolControl("btn_Delete", "MenuInfoManager", "Visible");
                btn_Delete.Enabled = ControlAttributes.BoolControl("btn_Delete", "MenuInfoManager", "Enabled");

                btn_Update.Visible = ControlAttributes.BoolControl("btn_Update", "MenuInfoManager", "Visible");
                btn_Update.Enabled = ControlAttributes.BoolControl("btn_Update", "MenuInfoManager", "Enabled");
            }
        }

        /// <summary>
        /// 递归创建TreeView菜单(模块列表)
        /// </summary>
        /// <param name="node">子菜单</param>
        /// <param name="MtID">子菜单上级ID</param>
        protected void LoadNode(TreeNodeCollection node, string MtID)
        {
            ///获取当前拥有的权限
            DataTable table;
            if (CommonalityEntity.USERNAME.ToLower() == "emewe")
            {
                table = LinQBaseDao.Query("select * from MenuInfo where Menu_OtherID=" + MtID + "and Menu_Type=1 order by Menu_Order").Tables[0];
            }
            else
                table = LinQBaseDao.Query("SELECT MenuInfo.* FROM PermissionsInfo INNER JOIN MenuInfo ON PermissionsInfo.Permissions_Menu_ID = MenuInfo.Menu_ID and MenuInfo.Menu_Type=1 and Permissions_UserId=" + CommonalityEntity.USERID + " and MenuInfo.Menu_OtherID=" + MtID + " order by Menu_Order").Tables[0];
            TreeNode nodeTemp;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                nodeTemp = new TreeNode();
                nodeTemp.Tag = table.Rows[i]["Menu_ID"];
                if (table.Rows[i]["Menu_ControlText"] != null)
                {
                    nodeTemp.Text = table.Rows[i]["Menu_ControlText"].ToString();
                }
                if (Convert.ToBoolean(table.Rows[i]["Menu_Visible"]) == false)
                {
                    nodeTemp.BackColor = Color.Yellow;
                }
                //DataTable tableMenuID = LinQBaseDao.Query("select MenuType_ID from dbo.MenuType where MenuType_name ='Button'").Tables[0];
                //if (tableMenuID.Rows.Count >= 1)
                //{
                //    if (Convert.ToInt32(table.Rows[0]["Menu_MenuType_ID"]) == Convert.ToInt32(tableMenuID.Rows[0]["MenuType_ID"]))
                //    {
                //        nodeTemp.BackColor = Color.SandyBrown;
                //    }
                //}


                if (nodeTemp != null)
                {
                    node.Add(nodeTemp);  //加入节点 
                }
                this.LoadNode(nodeTemp.Nodes, nodeTemp.Tag.ToString().Split(',')[0]);
            }
        }

        /// <summary>
        /// 绑定菜单类型
        /// </summary>
        private void Menu_ControlType()
        {
            cob_Menu_ControlType.DataSource = MenuTypeDAL.Query();
            cob_Menu_ControlType.DisplayMember = "MenuType_Name";
            cob_Menu_ControlType.ValueMember = "MenuType_ID";
            if (cob_Menu_ControlType.DataSource != null)
            {
                cob_Menu_ControlType.SelectedIndex = -1;
            }
        }
        private void MenuInfoADD_Load(object sender, EventArgs e)
        {
            userContext();
            comboxMenuTypeOrderBind();
            InitUser();
        }
        /// <summary>
        /// 添加菜单
        /// </summary>
        private void btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                if (TV_MenuInfo.SelectedNode != null)
                {
                    if (GetInt(TV_MenuInfo.SelectedNode.Tag.ToString()) < 0)
                    {
                        MessageBox.Show("请选中节点 ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                if (TV_MenuInfo.SelectedNode == null)
                {
                    MessageBox.Show("请选中节点 ", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                PageControl mf = new PageControl();
                if (string.IsNullOrEmpty(cob_Menu_ControlType.Text))
                {
                    mf.ShowToolTip(ToolTipIcon.Info, "提示", "控件类型不能为空！", cob_Menu_ControlType, this);
                    return;
                }
                if (string.IsNullOrEmpty(txt_Menu_ControlText.Text))
                {
                    mf.ShowToolTip(ToolTipIcon.Info, "提示", "菜单名称不能为空！", txt_Menu_ControlText, this);
                    return;
                }

                var mi = new MenuInfo
                {
                    Menu_Order = Convert.ToInt32(cmkMenuOderID.Text),
                    Menu_MenuType_ID = Convert.ToInt32(cob_Menu_ControlType.SelectedValue),
                    Menu_ControlText = txt_Menu_ControlText.Text,             //菜单名称txt_Menu_ControlText
                    Menu_OtherID = Convert.ToInt32(cob_Menu_ControlType.Text.ToString().Trim() == "一级菜单" ? 0 : Convert.ToInt32(TV_MenuInfo.SelectedNode.Tag.ToString())),              //父级ID
                    Menu_ControlType = cob_Menu_ControlType.SelectedValue.ToString(),                             //控件类型
                    Menu_ControlName = txt_Menu_ControlName.Text.ToString(),
                    Menu_FromName = txt_Menu_FromName.Text.ToString(),
                    Menu_FromText = txt_Menu_FromText.Text,
                    Menu_Enabled = radioButtons.Checked == true ? true : false,            //控件是否启用
                    Menu_Visible = radioButton2.Checked == true ? true : false,              //控件是否是否可见
                    Menu_Type = 1
                };
                if (MenuInfoDAL.InsertOneQCRecord(mi) == false)
                {
                    MessageBox.Show("添加失败！", "系统提示！", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    string Log_Content = String.Format("角色名称：{0}", Name);
                    CommonalityEntity.WriteLogData("新增", Log_Content, CommonalityEntity.USERNAME);//添加日志
                    return;
                }
                else
                    clean(this);
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("MenuInfoADD.btn_Add_Click()" );
            }
            finally
            {
                TV_MenuInfo.Nodes.Clear();
                InitMenu();//重新绑定控件
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (TV_MenuInfo.SelectedNode.Tag.ToString() != null)
                {
                    int menuID = Convert.ToInt32(TV_MenuInfo.SelectedNode.Tag.ToString());
                    #region --查询模块--
                    if (menuID <= -1)
                    {
                        MessageBox.Show("请选择节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    bool Menu_Enabledtxt = false;
                    bool cob_Menu_Visibletxt = false;
                    Expression<Func<MenuInfo, bool>> funviewinto = n => n.Menu_ControlName == TV_MenuInfo.SelectedNode.Name.ToString();
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

                    if (txt_Menu_ControlText.ToString() == "" || txt_Menu_ControlText == null)
                    {
                        TV_MenuInfo_NodeMouseDoubleClick(sender, null);
                    }

                    //1.删除菜单时，检查菜单是否配置；若配置了不能删除，需要先解除配置
                    //检测删除的节点是否存在
                    //检测菜单是否可以删除（等待）
                    //检测菜单是否可以编辑
                    //检测菜单是否已经配置
                    //检测是否有删除菜单的权限（等待）不可编辑
                    if (Convert.ToInt32(TV_MenuInfo.SelectedNode.Tag.ToString()) > 0)
                    {
                        Expression<Func<MenuInfo, bool>> fun = n => n.Menu_ID == Convert.ToInt32(TV_MenuInfo.SelectedNode.Tag.ToString());
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
                    TV_MenuInfo.Nodes.Clear();
                    InitMenu();//重新绑定控件
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
        /// <summary>
        /// 修改
        /// </summary>
        private void btn_Update_Click(object sender, EventArgs e)
        {
            try
            {
                if (TV_MenuInfo.SelectedNode.Tag.ToString() != null)
                {
                    int menuID = Convert.ToInt32(TV_MenuInfo.SelectedNode.Tag.ToString());
                    if (menuID <= 0)
                    {
                        MessageBox.Show("请先修改的节点！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    //1.修改数据不会修改Menu_MenuType_ID

                    //修改MenuInfo信息
                    Action<MenuInfo> action = n =>
                    {
                        n.Menu_ControlType = cob_Menu_ControlType.SelectedValue.ToString();  //控件类型  
                        if (cob_Menu_ControlType.SelectedText.ToString() == "一级菜单")
                        {
                            n.Menu_OtherID = 0;
                        }
                        n.Menu_ControlText = txt_Menu_ControlText.Text.Trim();
                        n.Menu_ControlName = txt_Menu_ControlName.Text.ToString();
                        n.Menu_FromName = txt_Menu_FromName.Text.ToString();
                        n.Menu_FromText = txt_Menu_FromText.Text;
                        n.Menu_Enabled = radioButtons.Checked == true ? true : false;            //控件是否启用
                        n.Menu_Visible = radioButton2.Checked == true ? true : false;               //控件是否是否可见
                    };
                    Expression<Func<MenuInfo, bool>> menuinfo = n => n.Menu_ID.ToString() == menuID.ToString();
                    if (MenuInfoDAL.Update(menuinfo, action) == true)//角色是否修改失败
                    {
                        MessageBox.Show("修改成功", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clean(this);
                    }
                    else
                    {
                        MessageBox.Show(" 修改失败", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    string Log_Contents = String.Format("角色名称：{0}", Name);
                    CommonalityEntity.WriteLogData("修改", Log_Contents, CommonalityEntity.USERNAME);//添加日志}
                }
            }
            catch 
            {
                CommonalityEntity.WriteTextLog("菜单管理MenuInfoManager btnUpdate_Click" );
            }
            finally
            {
                PageControl page = new PageControl();
                TV_MenuInfo.Nodes.Clear();
                InitMenu();
            }
        }


        #region --选中父级菜单该菜单下的所有子级菜单自动选中--
        private void tv_MenuInfo_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                e.Node.ForeColor = Color.Yellow;
                SetNodeCheckStatus(e.Node, e.Node.Checked);
                SetNodeStyle(e.Node);
            }
        }
        private void SetNodeCheckStatus(TreeNode tn, bool Checked)
        {
            if (tn == null) return;
            foreach (TreeNode tnChild in tn.Nodes)
            {
                tnChild.Checked = Checked;
                SetNodeCheckStatus(tnChild, Checked);
            }
            TreeNode tnParent = tn;
        }

        private void SetNodeStyle(TreeNode Node)
        {
            int nNodeCount = 0;
            if (Node.Nodes.Count != 0)
            {
                foreach (TreeNode tnTemp in Node.Nodes)
                {
                    if (tnTemp.Checked == true)
                        nNodeCount++;
                }

                if (nNodeCount == Node.Nodes.Count)
                {
                    Node.Checked = true;
                    Node.ExpandAll();
                    Node.ForeColor = Color.Black;
                }
                else if (nNodeCount == 0)
                {
                    Node.Checked = false;
                    Node.Collapse();
                    Node.ForeColor = Color.Black;
                }
                else
                {
                    Node.Checked = true;
                    Node.ForeColor = Color.Gray;
                }
            }
            //当前节点选择完后，判断父节点的状态，调用此方法递归。   
            if (Node.Parent != null)
                SetNodeStyle(Node.Parent);
        }
        #endregion

        #region--清除所有空间内容--
        public static void clean(Form form) //接收Form窗体
        {
            for (int i = 0; i < form.Controls.Count; i++)
            {
                foreach (Control control in form.Controls[i].Controls)
                {
                    if (control is TextBox)
                    {
                        (control as TextBox).Text = "";
                    }
                    if (control is CheckBox)
                    {
                        (control as CheckBox).Checked = false;
                    }
                    if (control is ComboBox)
                    {
                        (control as ComboBox).SelectedIndex = 0;
                    }
                    //其它控件类似以上操作即可
                }
            }
        }

        #endregion

        private void TV_MenuInfo_Click(object sender, EventArgs e)
        {
            TV_MenuInfo_NodeMouseDoubleClick(null, null);
        }

        #region --鼠标双击节点信息，可以进行修改和删除操作--
        private void TV_MenuInfo_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {

                btn_Add.Enabled = false;
                btn_Update.Enabled = true;
                clean(this.FindForm());
                int nodes = Convert.ToInt32(TV_MenuInfo.SelectedNode.Tag.ToString());//当前选择节点
                Expression<Func<MenuInfo, bool>> menuinfoFunc = n => n.Menu_ID == nodes;
                foreach (var n in MenuInfoDAL.Query(menuinfoFunc))
                {
                    if (n.Menu_ControlText != null)
                    {
                        txt_Menu_ControlText.Text = n.Menu_ControlText;
                    }
                    if (n.Menu_ControlType != "")
                    {
                        cob_Menu_ControlType.SelectedValue = Convert.ToInt32(n.Menu_ControlType);
                    }
                    if (n.Menu_ControlName != null)
                    {
                        txt_Menu_ControlName.Text = n.Menu_ControlName;
                    }
                    if (n.Menu_ControlName != null)
                    {
                        txt_Menu_ControlName.Text = n.Menu_ControlName;
                    }
                    if (n.Menu_FromName != null)
                    {
                        txt_Menu_FromName.Text = n.Menu_FromName;
                    }
                    if (n.Menu_FromText != null)
                    {
                        txt_Menu_FromText.Text = n.Menu_FromText;
                    }
                    if (n.Menu_Enabled != null)
                    {
                        radioButtons.Checked = n.Menu_Enabled == true ? true : false;
                        if (!radioButtons.Checked)
                        {
                            radioButton1.Checked = true;
                        }
                    }
                    if (n.Menu_Visible != null)
                    {
                        radioButton2.Checked = n.Menu_Visible == true ? true : false;
                        if (!radioButtons.Checked)
                        {
                            radioButton3.Checked = true;
                        }
                    }
                }

            }
            catch 
            {

            }
        }

        #endregion

        #region --鼠标单击事件，弹出菜单选择操作--
        private void TV_MenuInfo_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right && TV_MenuInfo.SelectedNode != null)//判断你点的是不是右键
            {
                Point ClickPoint = new Point(e.X, e.Y);
                TreeNode CurrentNode = TV_MenuInfo.GetNodeAt(ClickPoint);
                if (CurrentNode != null)//判断你点的是不是一个节点
                {
                    ContextMenuStrip formMenu = new ContextMenuStrip();
                    TV_MenuInfo.ContextMenuStrip = formMenu;
                    TV_MenuInfo.SelectedNode = CurrentNode;//选中这个节点
                    //TV_MenuInfo.SelectedNode.ForeColor =Color.Lime; ;//节点颜色设置
                    ToolStripMenuItem[] formMenuItemList = new ToolStripMenuItem[5];
                    formMenuItemList[0] = new ToolStripMenuItem("上移", null, new EventHandler(menuinfo_next));
                    formMenuItemList[1] = new ToolStripMenuItem("下移", null, new EventHandler(menuinfonext));
                    formMenuItemList[2] = new ToolStripMenuItem("修改", null, new EventHandler(TV_MenuInfo_Click));//---修改的方法为添加
                    formMenuItemList[3] = new ToolStripMenuItem("刷新", null, new EventHandler(InitMenu));//---修改的方法为添加
                    formMenuItemList[4] = new ToolStripMenuItem("删除", null, new EventHandler(btn_Delete_Click));
                    formMenu.Items.AddRange(formMenuItemList);
                    this.ContextMenuStrip = formMenu;
                }
            }
        }
        #endregion

        private void menuinfo_next(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(TV_MenuInfo.SelectedNode.Tag.ToString()))
            {
                int menuID = Convert.ToInt32(TV_MenuInfo.SelectedNode.Tag.ToString());
                DataTable table = LinQBaseDao.Query("select * from dbo.MenuInfo where menu_id='" + menuID + "'").Tables[0];
                int MenuOtherId = Convert.ToInt32(table.Rows[0]["Menu_OtherID"]);
                int MenuOderId = Convert.ToInt32(table.Rows[0]["Menu_Order"]);
                int nextIderId = MenuOderId - 1;
                if (MenuOderId > 1)//Menu_OderID为1的不能上移
                {
                    LinQBaseDao.Query("update MenuInfo set Menu_Order=" + MenuOderId + " where Menu_OtherID=" + MenuOtherId + " and Menu_Order=" + (MenuOderId - 1) + "");
                    LinQBaseDao.Query("update MenuInfo set Menu_Order=" + nextIderId + " where Menu_ID=" + menuID);
                }
                TV_MenuInfo.Nodes.Clear();
                InitMenu();//重新绑定控件
            }
            else
                MessageBox.Show("请选择节点！");
        }

        private void menuinfonext(object sender, EventArgs e)
        {
            if (TV_MenuInfo.SelectedNode.Tag.ToString() != null)
            {
                int menuID = Convert.ToInt32(TV_MenuInfo.SelectedNode.Tag.ToString());
                DataTable table = LinQBaseDao.Query("select * from dbo.MenuInfo where menu_id='" + menuID + "'").Tables[0];
                int MenuOtherId = Convert.ToInt32(table.Rows[0]["Menu_OtherID"]);
                int MenuOderId = Convert.ToInt32(table.Rows[0]["Menu_Order"]);
                int nextIderId = MenuOderId + 1;
                if (LinQBaseDao.Query("select * from dbo.MenuInfo where Menu_OtherID=" + MenuOtherId + " and Menu_Order=" + (MenuOderId + 1) + "").Tables[0].Rows.Count >= 1)//Menu_OderID当前施法最大
                {
                    LinQBaseDao.Query("update MenuInfo set Menu_Order=" + MenuOderId + " where Menu_OtherID=" + MenuOtherId + " and Menu_Order=" + (MenuOderId + 1) + "");
                    LinQBaseDao.Query("update MenuInfo set Menu_Order=" + nextIderId + " where Menu_ID=" + menuID);
                }
                TV_MenuInfo.Nodes.Clear();
                InitMenu();//重新绑定控件
            }
            else
                MessageBox.Show("请选择节点！");
        }

        private void InitMenu(object sender, EventArgs e)
        {
            TV_MenuInfo.Nodes.Clear();
            InitMenu();
        }

        //修改，删除和添加菜单类型
        private void cob_Menu_ControlType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //menuTypeId = Convert.ToInt32(TV_MenuInfo.SelectedNode.Tag.ToString());
            //menuTypeId = Convert.ToInt32(z);
            //菜单可以修改，添加或者编辑；
            if (cobMenuControlType.SelectedItem.ToString() == "<修改>")
            {
                Cob_Menu_ControlTypes = Convert.ToInt32(cob_Menu_ControlType.SelectedValue);
                MenuTypeId = 1; MenuTypeManager mty = new MenuTypeManager();
                mty.ShowDialog();
            } if (cobMenuControlType.SelectedItem.ToString() == "<添加>")
            {
                MenuTypeId = 2;
                MenuTypeManager mty = new MenuTypeManager();
                mty.ShowDialog();
            } if (cobMenuControlType.SelectedItem.ToString() == "<删除>")
            {
                Cob_Menu_ControlTypes = Convert.ToInt32(cob_Menu_ControlType.SelectedValue);
                MenuTypeId = 3; MenuTypeManager mty = new MenuTypeManager();
                mty.ShowDialog();
            }
            return;
        }

        private static int menuTypeId;

        public static int MenuTypeId
        {
            get { return MenuInfoManager.menuTypeId; }
            set { MenuInfoManager.menuTypeId = value; }
        }
        private static int cob_Menu_ControlTypes;

        public static int Cob_Menu_ControlTypes
        {
            get { return MenuInfoManager.cob_Menu_ControlTypes; }
            set { MenuInfoManager.cob_Menu_ControlTypes = value; }
        }

        public static int GetInt(object num)
        {
            try
            {
                return int.Parse(num.ToString());
            }
            catch 
            {
                return 0;
            }
        }

        private void TV_MenuInfo_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }
        public int tags = 0;
        private void TV_MenuInfo_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void TV_MenuInfo_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode myNode = null;
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                myNode = (TreeNode)(e.Data.GetData(typeof(TreeNode)));
            }
            else
            {
                MessageBox.Show("错误操作！");
            }
            Position.X = e.X;
            Position.Y = e.Y;
            Position = TV_MenuInfo.PointToClient(Position);
            TreeNode DropNode = this.TV_MenuInfo.GetNodeAt(Position);
            // 1.目标节点不是空。2.目标节点不是被拖拽接点的字节点。3.目标节点不是被拖拽节点本身
            if (DropNode != null && DropNode.Parent != myNode && DropNode != myNode)
            {
                TreeNode DragNode = myNode;
                // 将被拖拽节点从原来位置删除。
                myNode.Remove();
                // 在目标节点下增加被拖拽节点
                DropNode.Nodes.Add(DragNode);
                LinQBaseDao.Query("update MenuInfo set menu_otherid='" + Convert.ToInt32(DropNode.Tag) + "' where menu_id='" + Convert.ToInt32(myNode.Tag) + "'");
            }
            // 如果目标节点不存在，即拖拽的位置不存在节点，那么就将被拖拽节点放在根节点之下
            if (DropNode == null)
            {
                TreeNode DragNode = myNode;
                myNode.Remove();
                TV_MenuInfo.Nodes.Add(DragNode);
                LinQBaseDao.Query("update MenuInfo set menu_otherid=0 where menu_id='" + Convert.ToInt32(myNode.Tag) + "'");

            }
        }

        /// <summary>
        /// 取消修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetNull_Click(object sender, EventArgs e)
        {
            btn_Update.Enabled = false;
            btn_Add.Enabled = true;
            clean(this);
        }
    }
}
