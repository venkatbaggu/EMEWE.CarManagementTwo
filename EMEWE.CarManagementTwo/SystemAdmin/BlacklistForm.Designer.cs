namespace EMEWE.CarManagement.SystemAdmin
{
    partial class BlacklistForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dgvBlackList = new System.Windows.Forms.DataGridView();
            this.comMuneStript = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.contextMenuStript = new System.Windows.Forms.ToolStripMenuItem();
            this.UpgradeBacklist = new System.Windows.Forms.ToolStripMenuItem();
            this.DowngradeBacklist = new System.Windows.Forms.ToolStripMenuItem();
            this.NoticeContext = new System.Windows.Forms.ToolStripMenuItem();
            this.WarningContext = new System.Windows.Forms.ToolStripMenuItem();
            this.RefuseContext = new System.Windows.Forms.ToolStripMenuItem();
            this.gbSelect = new System.Windows.Forms.GroupBox();
            this.txtStaffName = new System.Windows.Forms.TextBox();
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.txtCarNo = new System.Windows.Forms.TextBox();
            this.comboxBlacklistState = new System.Windows.Forms.ComboBox();
            this.btnSeach = new System.Windows.Forms.Button();
            this.comboxBlacklistType = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.gbButton = new System.Windows.Forms.GroupBox();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEmpty = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnBackAdd = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.lblPhone = new System.Windows.Forms.Label();
            this.txtStaffInfo_Identity = new System.Windows.Forms.TextBox();
            this.lblStaffInfo_Identity = new System.Windows.Forms.Label();
            this.txtContor = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.comboxBlacklist_Type = new System.Windows.Forms.ComboBox();
            this.comboxBlacklist_State = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtBlacklist_Remark = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtBlacklist_Name = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.bdnInfo = new System.Windows.Forms.BindingNavigator(this.components);
            this.tslHomPage1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tslPreviousPage1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.tstbPageCurrent = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.tslPageCount = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel6 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel7 = new System.Windows.Forms.ToolStripLabel();
            this.tslNMax = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel9 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tslNextPage1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tslLastPage1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel12 = new System.Windows.Forms.ToolStripLabel();
            this.tscbxPageSize = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tslNotSelect = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator16 = new System.Windows.Forms.ToolStripSeparator();
            this.tslSelectAll = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator17 = new System.Windows.Forms.ToolStripSeparator();
            this.tslbExecl = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.tslbInExecl = new System.Windows.Forms.ToolStripLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSet = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.myUsertv = new EMEWE.CarManagement.MyControl.MyUserTreeView();
            this.Blacklist_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Blacklist_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Blacklist_State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CarInfo_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomerInfo_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StaffInfo_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StaffInfo_Identity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StaffInfo_Phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Blacklist_UpgradeCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Blacklist_DowngradeCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Blacklist_Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Blacklist_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Blacklist_Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlackList)).BeginInit();
            this.comMuneStript.SuspendLayout();
            this.gbSelect.SuspendLayout();
            this.gbButton.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdnInfo)).BeginInit();
            this.bdnInfo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvBlackList
            // 
            this.dgvBlackList.AllowUserToAddRows = false;
            this.dgvBlackList.AllowUserToDeleteRows = false;
            this.dgvBlackList.BackgroundColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.dgvBlackList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Blacklist_ID,
            this.Blacklist_Type,
            this.Blacklist_State,
            this.CarInfo_Name,
            this.CustomerInfo_Name,
            this.StaffInfo_Name,
            this.StaffInfo_Identity,
            this.StaffInfo_Phone,
            this.Blacklist_UpgradeCount,
            this.Blacklist_DowngradeCount,
            this.Blacklist_Time,
            this.Blacklist_Name,
            this.Blacklist_Remark});
            this.dgvBlackList.ContextMenuStrip = this.comMuneStript;
            this.dgvBlackList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvBlackList.Location = new System.Drawing.Point(0, 280);
            this.dgvBlackList.Name = "dgvBlackList";
            this.dgvBlackList.ReadOnly = true;
            this.dgvBlackList.RowTemplate.Height = 23;
            this.dgvBlackList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBlackList.Size = new System.Drawing.Size(770, 244);
            this.dgvBlackList.TabIndex = 102;
            this.dgvBlackList.DoubleClick += new System.EventHandler(this.dgvBlackList_DoubleClick);
            this.dgvBlackList.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvBlackList_CellMouseDown);
            this.dgvBlackList.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvBlackList_RowPostPaint);
            this.dgvBlackList.Click += new System.EventHandler(this.dgvBlackList_Click);
            // 
            // comMuneStript
            // 
            this.comMuneStript.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.contextMenuStript,
            this.UpgradeBacklist,
            this.DowngradeBacklist,
            this.NoticeContext,
            this.WarningContext,
            this.RefuseContext});
            this.comMuneStript.Name = "comMuneStript";
            this.comMuneStript.Size = new System.Drawing.Size(119, 136);
            // 
            // contextMenuStript
            // 
            this.contextMenuStript.Name = "contextMenuStript";
            this.contextMenuStript.Size = new System.Drawing.Size(118, 22);
            this.contextMenuStript.Text = "正常";
            this.contextMenuStript.Click += new System.EventHandler(this.contextMenuStript_Click);
            // 
            // UpgradeBacklist
            // 
            this.UpgradeBacklist.Name = "UpgradeBacklist";
            this.UpgradeBacklist.Size = new System.Drawing.Size(118, 22);
            this.UpgradeBacklist.Text = "升级";
            this.UpgradeBacklist.Click += new System.EventHandler(this.UpgradeBacklist_Click);
            // 
            // DowngradeBacklist
            // 
            this.DowngradeBacklist.Name = "DowngradeBacklist";
            this.DowngradeBacklist.Size = new System.Drawing.Size(118, 22);
            this.DowngradeBacklist.Text = "降级";
            this.DowngradeBacklist.Click += new System.EventHandler(this.DowngradeBacklist_Click);
            // 
            // NoticeContext
            // 
            this.NoticeContext.Name = "NoticeContext";
            this.NoticeContext.Size = new System.Drawing.Size(118, 22);
            this.NoticeContext.Text = "通知";
            this.NoticeContext.Click += new System.EventHandler(this.NoticeContext_Click);
            // 
            // WarningContext
            // 
            this.WarningContext.Name = "WarningContext";
            this.WarningContext.Size = new System.Drawing.Size(118, 22);
            this.WarningContext.Text = "警告";
            this.WarningContext.Click += new System.EventHandler(this.WarningContext_Click);
            // 
            // RefuseContext
            // 
            this.RefuseContext.Name = "RefuseContext";
            this.RefuseContext.Size = new System.Drawing.Size(118, 22);
            this.RefuseContext.Text = "拒绝入场";
            this.RefuseContext.Click += new System.EventHandler(this.RefuseContext_Click);
            // 
            // gbSelect
            // 
            this.gbSelect.Controls.Add(this.txtStaffName);
            this.gbSelect.Controls.Add(this.txtCustomer);
            this.gbSelect.Controls.Add(this.txtCarNo);
            this.gbSelect.Controls.Add(this.comboxBlacklistState);
            this.gbSelect.Controls.Add(this.btnSeach);
            this.gbSelect.Controls.Add(this.comboxBlacklistType);
            this.gbSelect.Controls.Add(this.label9);
            this.gbSelect.Controls.Add(this.label16);
            this.gbSelect.Controls.Add(this.label17);
            this.gbSelect.Controls.Add(this.label1);
            this.gbSelect.Controls.Add(this.label3);
            this.gbSelect.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbSelect.Location = new System.Drawing.Point(0, 180);
            this.gbSelect.Name = "gbSelect";
            this.gbSelect.Size = new System.Drawing.Size(770, 70);
            this.gbSelect.TabIndex = 100;
            this.gbSelect.TabStop = false;
            this.gbSelect.Text = "搜  索";
            // 
            // txtStaffName
            // 
            this.txtStaffName.Location = new System.Drawing.Point(592, 15);
            this.txtStaffName.Name = "txtStaffName";
            this.txtStaffName.Size = new System.Drawing.Size(130, 21);
            this.txtStaffName.TabIndex = 45;
            // 
            // txtCustomer
            // 
            this.txtCustomer.Location = new System.Drawing.Point(350, 13);
            this.txtCustomer.Name = "txtCustomer";
            this.txtCustomer.Size = new System.Drawing.Size(112, 21);
            this.txtCustomer.TabIndex = 44;
            // 
            // txtCarNo
            // 
            this.txtCarNo.Location = new System.Drawing.Point(113, 11);
            this.txtCarNo.Name = "txtCarNo";
            this.txtCarNo.Size = new System.Drawing.Size(111, 21);
            this.txtCarNo.TabIndex = 43;
            // 
            // comboxBlacklistState
            // 
            this.comboxBlacklistState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxBlacklistState.FormattingEnabled = true;
            this.comboxBlacklistState.Location = new System.Drawing.Point(113, 41);
            this.comboxBlacklistState.Name = "comboxBlacklistState";
            this.comboxBlacklistState.Size = new System.Drawing.Size(111, 20);
            this.comboxBlacklistState.TabIndex = 42;
            // 
            // btnSeach
            // 
            this.btnSeach.Location = new System.Drawing.Point(647, 41);
            this.btnSeach.Name = "btnSeach";
            this.btnSeach.Size = new System.Drawing.Size(75, 23);
            this.btnSeach.TabIndex = 6;
            this.btnSeach.Text = "搜索";
            this.btnSeach.UseVisualStyleBackColor = true;
            this.btnSeach.Click += new System.EventHandler(this.btnSeach_Click);
            // 
            // comboxBlacklistType
            // 
            this.comboxBlacklistType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxBlacklistType.FormattingEnabled = true;
            this.comboxBlacklistType.Location = new System.Drawing.Point(350, 38);
            this.comboxBlacklistType.Name = "comboxBlacklistType";
            this.comboxBlacklistType.Size = new System.Drawing.Size(112, 20);
            this.comboxBlacklistType.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(543, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 38;
            this.label9.Text = "姓名：";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(26, 46);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 12);
            this.label16.TabIndex = 2;
            this.label16.Text = "黑名单状态：";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(267, 41);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(77, 12);
            this.label17.TabIndex = 0;
            this.label17.Text = "黑名单类型：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "车牌号：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(279, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "公司名称：";
            // 
            // gbButton
            // 
            this.gbButton.Controls.Add(this.btnDel);
            this.gbButton.Controls.Add(this.btnSave);
            this.gbButton.Controls.Add(this.btnEmpty);
            this.gbButton.Controls.Add(this.btnUpdate);
            this.gbButton.Controls.Add(this.btnBackAdd);
            this.gbButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbButton.Location = new System.Drawing.Point(0, 138);
            this.gbButton.Name = "gbButton";
            this.gbButton.Size = new System.Drawing.Size(770, 42);
            this.gbButton.TabIndex = 99;
            this.gbButton.TabStop = false;
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(453, 12);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 47;
            this.btnDel.Text = "删 除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(195, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 46;
            this.btnSave.Text = "保 存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnEmpty
            // 
            this.btnEmpty.Location = new System.Drawing.Point(324, 12);
            this.btnEmpty.Name = "btnEmpty";
            this.btnEmpty.Size = new System.Drawing.Size(75, 23);
            this.btnEmpty.TabIndex = 39;
            this.btnEmpty.Text = "清  空";
            this.btnEmpty.UseVisualStyleBackColor = true;
            this.btnEmpty.Click += new System.EventHandler(this.btnEmpty_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Location = new System.Drawing.Point(66, 12);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 45;
            this.btnUpdate.Text = "修  改";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnBackAdd
            // 
            this.btnBackAdd.Location = new System.Drawing.Point(571, 12);
            this.btnBackAdd.Name = "btnBackAdd";
            this.btnBackAdd.Size = new System.Drawing.Size(105, 23);
            this.btnBackAdd.TabIndex = 2;
            this.btnBackAdd.Text = "黑名单等级设置";
            this.btnBackAdd.UseVisualStyleBackColor = true;
            this.btnBackAdd.Click += new System.EventHandler(this.btnBackAdd_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.txtPhone);
            this.groupBox2.Controls.Add(this.lblPhone);
            this.groupBox2.Controls.Add(this.txtStaffInfo_Identity);
            this.groupBox2.Controls.Add(this.lblStaffInfo_Identity);
            this.groupBox2.Controls.Add(this.txtContor);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.comboxBlacklist_Type);
            this.groupBox2.Controls.Add(this.comboxBlacklist_State);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.txtBlacklist_Remark);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtBlacklist_Name);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(770, 138);
            this.groupBox2.TabIndex = 98;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "黑名单信息";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(710, 11);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 22);
            this.button1.TabIndex = 48;
            this.button1.Text = "获取";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(590, 51);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(120, 21);
            this.txtPhone.TabIndex = 47;
            this.txtPhone.Visible = false;
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.ForeColor = System.Drawing.Color.Black;
            this.lblPhone.Location = new System.Drawing.Point(519, 54);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(65, 12);
            this.lblPhone.TabIndex = 46;
            this.lblPhone.Text = "手机号码：";
            this.lblPhone.Visible = false;
            // 
            // txtStaffInfo_Identity
            // 
            this.txtStaffInfo_Identity.Location = new System.Drawing.Point(590, 12);
            this.txtStaffInfo_Identity.Name = "txtStaffInfo_Identity";
            this.txtStaffInfo_Identity.Size = new System.Drawing.Size(120, 21);
            this.txtStaffInfo_Identity.TabIndex = 47;
            this.txtStaffInfo_Identity.Visible = false;
            // 
            // lblStaffInfo_Identity
            // 
            this.lblStaffInfo_Identity.AutoSize = true;
            this.lblStaffInfo_Identity.ForeColor = System.Drawing.Color.Black;
            this.lblStaffInfo_Identity.Location = new System.Drawing.Point(507, 17);
            this.lblStaffInfo_Identity.Name = "lblStaffInfo_Identity";
            this.lblStaffInfo_Identity.Size = new System.Drawing.Size(77, 12);
            this.lblStaffInfo_Identity.TabIndex = 46;
            this.lblStaffInfo_Identity.Text = "身份证号码：";
            this.lblStaffInfo_Identity.Visible = false;
            // 
            // txtContor
            // 
            this.txtContor.Location = new System.Drawing.Point(359, 11);
            this.txtContor.Name = "txtContor";
            this.txtContor.Size = new System.Drawing.Size(120, 21);
            this.txtContor.TabIndex = 45;
            this.txtContor.TextChanged += new System.EventHandler(this.txtContor_TextChanged);
            this.txtContor.Click += new System.EventHandler(this.txtContor_Click);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(359, 51);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(120, 21);
            this.textBox1.TabIndex = 44;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(300, 54);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 43;
            this.label12.Text = "登记人：";
            // 
            // comboxBlacklist_Type
            // 
            this.comboxBlacklist_Type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxBlacklist_Type.FormattingEnabled = true;
            this.comboxBlacklist_Type.Location = new System.Drawing.Point(98, 17);
            this.comboxBlacklist_Type.Name = "comboxBlacklist_Type";
            this.comboxBlacklist_Type.Size = new System.Drawing.Size(120, 20);
            this.comboxBlacklist_Type.TabIndex = 42;
            this.comboxBlacklist_Type.SelectedIndexChanged += new System.EventHandler(this.comboxBlacklist_Type_SelectedIndexChanged);
            // 
            // comboxBlacklist_State
            // 
            this.comboxBlacklist_State.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxBlacklist_State.FormattingEnabled = true;
            this.comboxBlacklist_State.Location = new System.Drawing.Point(98, 51);
            this.comboxBlacklist_State.Name = "comboxBlacklist_State";
            this.comboxBlacklist_State.Size = new System.Drawing.Size(120, 20);
            this.comboxBlacklist_State.TabIndex = 41;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(451, 336);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(0, 12);
            this.label11.TabIndex = 30;
            // 
            // txtBlacklist_Remark
            // 
            this.txtBlacklist_Remark.Location = new System.Drawing.Point(465, 85);
            this.txtBlacklist_Remark.Multiline = true;
            this.txtBlacklist_Remark.Name = "txtBlacklist_Remark";
            this.txtBlacklist_Remark.Size = new System.Drawing.Size(245, 41);
            this.txtBlacklist_Remark.TabIndex = 29;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(382, 88);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 28;
            this.label10.Text = "黑名单备注：";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "黑名单类型：";
            // 
            // txtBlacklist_Name
            // 
            this.txtBlacklist_Name.Location = new System.Drawing.Point(98, 85);
            this.txtBlacklist_Name.Multiline = true;
            this.txtBlacklist_Name.Name = "txtBlacklist_Name";
            this.txtBlacklist_Name.Size = new System.Drawing.Size(259, 40);
            this.txtBlacklist_Name.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "黑名单状态：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "黑名单原因：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(291, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 6;
            this.label6.Text = "车牌号：";
            // 
            // bdnInfo
            // 
            this.bdnInfo.AddNewItem = null;
            this.bdnInfo.AutoSize = false;
            this.bdnInfo.BackColor = System.Drawing.Color.Gainsboro;
            this.bdnInfo.CountItem = null;
            this.bdnInfo.DeleteItem = null;
            this.bdnInfo.Font = new System.Drawing.Font("宋体", 9F);
            this.bdnInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bdnInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslHomPage1,
            this.toolStripSeparator1,
            this.tslPreviousPage1,
            this.toolStripSeparator2,
            this.toolStripLabel3,
            this.tstbPageCurrent,
            this.toolStripLabel5,
            this.tslPageCount,
            this.toolStripLabel6,
            this.toolStripSeparator3,
            this.toolStripLabel7,
            this.tslNMax,
            this.toolStripLabel9,
            this.toolStripSeparator4,
            this.tslNextPage1,
            this.toolStripSeparator5,
            this.tslLastPage1,
            this.toolStripSeparator6,
            this.toolStripLabel12,
            this.tscbxPageSize,
            this.toolStripSeparator7,
            this.tslNotSelect,
            this.toolStripSeparator16,
            this.tslSelectAll,
            this.toolStripSeparator17,
            this.tslbExecl,
            this.toolStripSeparator8,
            this.tslbInExecl});
            this.bdnInfo.Location = new System.Drawing.Point(0, 250);
            this.bdnInfo.MoveFirstItem = null;
            this.bdnInfo.MoveLastItem = null;
            this.bdnInfo.MoveNextItem = null;
            this.bdnInfo.MovePreviousItem = null;
            this.bdnInfo.Name = "bdnInfo";
            this.bdnInfo.PositionItem = null;
            this.bdnInfo.Size = new System.Drawing.Size(770, 27);
            this.bdnInfo.TabIndex = 103;
            this.bdnInfo.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.bdnInfo_ItemClicked);
            // 
            // tslHomPage1
            // 
            this.tslHomPage1.Name = "tslHomPage1";
            this.tslHomPage1.Size = new System.Drawing.Size(29, 24);
            this.tslHomPage1.Text = "首页";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // tslPreviousPage1
            // 
            this.tslPreviousPage1.Name = "tslPreviousPage1";
            this.tslPreviousPage1.Size = new System.Drawing.Size(41, 24);
            this.tslPreviousPage1.Text = "上一页";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(53, 24);
            this.toolStripLabel3.Text = "当前页数";
            // 
            // tstbPageCurrent
            // 
            this.tstbPageCurrent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tstbPageCurrent.Name = "tstbPageCurrent";
            this.tstbPageCurrent.Size = new System.Drawing.Size(50, 27);
            this.tstbPageCurrent.Text = "1";
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(17, 24);
            this.toolStripLabel5.Text = "共";
            // 
            // tslPageCount
            // 
            this.tslPageCount.Name = "tslPageCount";
            this.tslPageCount.Size = new System.Drawing.Size(11, 24);
            this.tslPageCount.Text = "0";
            // 
            // toolStripLabel6
            // 
            this.toolStripLabel6.Name = "toolStripLabel6";
            this.toolStripLabel6.Size = new System.Drawing.Size(17, 24);
            this.toolStripLabel6.Text = "页";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel7
            // 
            this.toolStripLabel7.Name = "toolStripLabel7";
            this.toolStripLabel7.Size = new System.Drawing.Size(17, 24);
            this.toolStripLabel7.Text = "共";
            // 
            // tslNMax
            // 
            this.tslNMax.Name = "tslNMax";
            this.tslNMax.Size = new System.Drawing.Size(11, 24);
            this.tslNMax.Text = "0";
            // 
            // toolStripLabel9
            // 
            this.toolStripLabel9.Name = "toolStripLabel9";
            this.toolStripLabel9.Size = new System.Drawing.Size(17, 24);
            this.toolStripLabel9.Text = "条";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
            // 
            // tslNextPage1
            // 
            this.tslNextPage1.Name = "tslNextPage1";
            this.tslNextPage1.Size = new System.Drawing.Size(41, 24);
            this.tslNextPage1.Text = "下一页";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
            // 
            // tslLastPage1
            // 
            this.tslLastPage1.Name = "tslLastPage1";
            this.tslLastPage1.Size = new System.Drawing.Size(29, 24);
            this.tslLastPage1.Text = "尾页";
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel12
            // 
            this.toolStripLabel12.Name = "toolStripLabel12";
            this.toolStripLabel12.Size = new System.Drawing.Size(53, 24);
            this.toolStripLabel12.Text = "每页条数";
            // 
            // tscbxPageSize
            // 
            this.tscbxPageSize.Items.AddRange(new object[] {
            "10",
            "15",
            "20",
            "25",
            "30",
            "35",
            "40",
            "45",
            "50",
            "55",
            "60",
            "65",
            "70",
            "90",
            "100",
            "200",
            "300",
            "500",
            "800",
            "1000"});
            this.tscbxPageSize.Name = "tscbxPageSize";
            this.tscbxPageSize.Size = new System.Drawing.Size(75, 27);
            this.tscbxPageSize.Text = "10";
            this.tscbxPageSize.SelectedIndexChanged += new System.EventHandler(this.tscbxPageSize2_SelectedIndexChanged);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 27);
            // 
            // tslNotSelect
            // 
            this.tslNotSelect.Name = "tslNotSelect";
            this.tslNotSelect.Size = new System.Drawing.Size(53, 24);
            this.tslNotSelect.Text = "取消全选";
            // 
            // toolStripSeparator16
            // 
            this.toolStripSeparator16.Name = "toolStripSeparator16";
            this.toolStripSeparator16.Size = new System.Drawing.Size(6, 27);
            // 
            // tslSelectAll
            // 
            this.tslSelectAll.Name = "tslSelectAll";
            this.tslSelectAll.Size = new System.Drawing.Size(29, 24);
            this.tslSelectAll.Text = "全选";
            // 
            // toolStripSeparator17
            // 
            this.toolStripSeparator17.Name = "toolStripSeparator17";
            this.toolStripSeparator17.Size = new System.Drawing.Size(6, 27);
            // 
            // tslbExecl
            // 
            this.tslbExecl.Name = "tslbExecl";
            this.tslbExecl.Size = new System.Drawing.Size(59, 24);
            this.tslbExecl.Text = "导出Execl";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 27);
            // 
            // tslbInExecl
            // 
            this.tslbInExecl.Name = "tslbInExecl";
            this.tslbInExecl.Size = new System.Drawing.Size(59, 24);
            this.tslbInExecl.Text = "导入Execl";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSet);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.progressBar1);
            this.groupBox1.Location = new System.Drawing.Point(185, 250);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(401, 100);
            this.groupBox1.TabIndex = 167;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "导出信息";
            // 
            // btnSet
            // 
            this.btnSet.Location = new System.Drawing.Point(142, 73);
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(75, 23);
            this.btnSet.TabIndex = 2;
            this.btnSet.Text = "取消导出";
            this.btnSet.UseVisualStyleBackColor = true;
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(46, 21);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(47, 12);
            this.label18.TabIndex = 1;
            this.label18.Text = "label18";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(6, 44);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(389, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // myUsertv
            // 
            this.myUsertv.Location = new System.Drawing.Point(296, 39);
            this.myUsertv.Name = "myUsertv";
            this.myUsertv.Size = new System.Drawing.Size(236, 326);
            this.myUsertv.TabIndex = 48;
            // 
            // Blacklist_ID
            // 
            this.Blacklist_ID.DataPropertyName = "Blacklist_ID";
            this.Blacklist_ID.HeaderText = "黑名单编号";
            this.Blacklist_ID.Name = "Blacklist_ID";
            this.Blacklist_ID.ReadOnly = true;
            this.Blacklist_ID.Visible = false;
            // 
            // Blacklist_Type
            // 
            this.Blacklist_Type.DataPropertyName = "Blacklist_Type";
            this.Blacklist_Type.HeaderText = "黑名单类型";
            this.Blacklist_Type.Name = "Blacklist_Type";
            this.Blacklist_Type.ReadOnly = true;
            this.Blacklist_Type.Width = 120;
            // 
            // Blacklist_State
            // 
            this.Blacklist_State.DataPropertyName = "Blacklist_State";
            this.Blacklist_State.HeaderText = "黑名单状态";
            this.Blacklist_State.Name = "Blacklist_State";
            this.Blacklist_State.ReadOnly = true;
            this.Blacklist_State.Width = 120;
            // 
            // CarInfo_Name
            // 
            this.CarInfo_Name.DataPropertyName = "Car_Name";
            this.CarInfo_Name.HeaderText = "车辆车牌号";
            this.CarInfo_Name.Name = "CarInfo_Name";
            this.CarInfo_Name.ReadOnly = true;
            // 
            // CustomerInfo_Name
            // 
            this.CustomerInfo_Name.DataPropertyName = "CustomerInfo_Name";
            this.CustomerInfo_Name.HeaderText = "公司名称";
            this.CustomerInfo_Name.Name = "CustomerInfo_Name";
            this.CustomerInfo_Name.ReadOnly = true;
            this.CustomerInfo_Name.Width = 120;
            // 
            // StaffInfo_Name
            // 
            this.StaffInfo_Name.DataPropertyName = "StaffInfo_Name";
            this.StaffInfo_Name.HeaderText = "姓名";
            this.StaffInfo_Name.Name = "StaffInfo_Name";
            this.StaffInfo_Name.ReadOnly = true;
            this.StaffInfo_Name.Width = 120;
            // 
            // StaffInfo_Identity
            // 
            this.StaffInfo_Identity.DataPropertyName = "StaffInfo_Identity";
            this.StaffInfo_Identity.HeaderText = "身份证号码";
            this.StaffInfo_Identity.Name = "StaffInfo_Identity";
            this.StaffInfo_Identity.ReadOnly = true;
            // 
            // StaffInfo_Phone
            // 
            this.StaffInfo_Phone.DataPropertyName = "StaffInfo_Phone";
            this.StaffInfo_Phone.HeaderText = "手机号码";
            this.StaffInfo_Phone.Name = "StaffInfo_Phone";
            this.StaffInfo_Phone.ReadOnly = true;
            // 
            // Blacklist_UpgradeCount
            // 
            this.Blacklist_UpgradeCount.DataPropertyName = "Blacklist_UpgradeCount";
            this.Blacklist_UpgradeCount.HeaderText = "黑名单升级次数";
            this.Blacklist_UpgradeCount.Name = "Blacklist_UpgradeCount";
            this.Blacklist_UpgradeCount.ReadOnly = true;
            // 
            // Blacklist_DowngradeCount
            // 
            this.Blacklist_DowngradeCount.DataPropertyName = "Blacklist_UpgradeCount";
            this.Blacklist_DowngradeCount.HeaderText = "黑名单降级次数";
            this.Blacklist_DowngradeCount.Name = "Blacklist_DowngradeCount";
            this.Blacklist_DowngradeCount.ReadOnly = true;
            // 
            // Blacklist_Time
            // 
            this.Blacklist_Time.DataPropertyName = "Blacklist_Time";
            this.Blacklist_Time.HeaderText = "黑名单登记时间";
            this.Blacklist_Time.Name = "Blacklist_Time";
            this.Blacklist_Time.ReadOnly = true;
            this.Blacklist_Time.Width = 150;
            // 
            // Blacklist_Name
            // 
            this.Blacklist_Name.DataPropertyName = "Blacklist_Name";
            this.Blacklist_Name.HeaderText = "黑名单原因";
            this.Blacklist_Name.Name = "Blacklist_Name";
            this.Blacklist_Name.ReadOnly = true;
            this.Blacklist_Name.Width = 150;
            // 
            // Blacklist_Remark
            // 
            this.Blacklist_Remark.DataPropertyName = "Blacklist_Remark";
            this.Blacklist_Remark.HeaderText = "黑名单备注";
            this.Blacklist_Remark.Name = "Blacklist_Remark";
            this.Blacklist_Remark.ReadOnly = true;
            this.Blacklist_Remark.Width = 180;
            // 
            // BlacklistForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 524);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.myUsertv);
            this.Controls.Add(this.bdnInfo);
            this.Controls.Add(this.gbSelect);
            this.Controls.Add(this.gbButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.dgvBlackList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "BlacklistForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "黑名单信息";
            this.Load += new System.EventHandler(this.BlacklistForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlackList)).EndInit();
            this.comMuneStript.ResumeLayout(false);
            this.gbSelect.ResumeLayout(false);
            this.gbSelect.PerformLayout();
            this.gbButton.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdnInfo)).EndInit();
            this.bdnInfo.ResumeLayout(false);
            this.bdnInfo.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvBlackList;
        private System.Windows.Forms.GroupBox gbSelect;
        private System.Windows.Forms.Button btnSeach;
        private System.Windows.Forms.ComboBox comboxBlacklistType;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.GroupBox gbButton;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnEmpty;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtBlacklist_Remark;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBlacklist_Name;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboxBlacklist_Type;
        private System.Windows.Forms.ComboBox comboxBlacklist_State;
        private System.Windows.Forms.ComboBox comboxBlacklistState;
        private System.Windows.Forms.BindingNavigator bdnInfo;
        private System.Windows.Forms.ToolStripLabel tslHomPage1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel tslPreviousPage1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripTextBox tstbPageCurrent;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripLabel tslPageCount;
        private System.Windows.Forms.ToolStripLabel toolStripLabel6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel7;
        private System.Windows.Forms.ToolStripLabel tslNMax;
        private System.Windows.Forms.ToolStripLabel toolStripLabel9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel tslNextPage1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripLabel tslLastPage1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripLabel toolStripLabel12;
        private System.Windows.Forms.ToolStripComboBox tscbxPageSize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripLabel tslNotSelect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator16;
        private System.Windows.Forms.ToolStripLabel tslSelectAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator17;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip comMuneStript;
        private System.Windows.Forms.ToolStripMenuItem UpgradeBacklist;
        private System.Windows.Forms.ToolStripMenuItem DowngradeBacklist;
        private System.Windows.Forms.ToolStripMenuItem WarningContext;
        private System.Windows.Forms.ToolStripMenuItem RefuseContext;
        private System.Windows.Forms.ToolStripMenuItem NoticeContext;
        private System.Windows.Forms.Button btnBackAdd;
        private System.Windows.Forms.ToolStripMenuItem contextMenuStript;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtContor;
        private System.Windows.Forms.TextBox txtCarNo;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.TextBox txtStaffName;
        private System.Windows.Forms.TextBox txtStaffInfo_Identity;
        private System.Windows.Forms.Label lblStaffInfo_Identity;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblPhone;
        private EMEWE.CarManagement.MyControl.MyUserTreeView myUsertv;
        private System.Windows.Forms.ToolStripLabel tslbExecl;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripLabel tslbInExecl;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSet;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Blacklist_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Blacklist_Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Blacklist_State;
        private System.Windows.Forms.DataGridViewTextBoxColumn CarInfo_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn CustomerInfo_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn StaffInfo_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn StaffInfo_Identity;
        private System.Windows.Forms.DataGridViewTextBoxColumn StaffInfo_Phone;
        private System.Windows.Forms.DataGridViewTextBoxColumn Blacklist_UpgradeCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Blacklist_DowngradeCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn Blacklist_Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Blacklist_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Blacklist_Remark;
    }
}