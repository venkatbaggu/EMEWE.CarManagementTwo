namespace EMEWE.CarManagement.SystemAdmin
{
    partial class ICCardTypeForm
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
            this.dgvICCardType = new System.Windows.Forms.DataGridView();
            this.ICCardType_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ICCardType_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ICCardType_Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ICCardType_State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ICCardType_Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IC卡类型权限 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ICCardType_Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbSelect = new System.Windows.Forms.GroupBox();
            this.txtICCardTypeName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.comboxICCardTypeState = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtICCardTypeValue = new System.Windows.Forms.TextBox();
            this.gbButton = new System.Windows.Forms.GroupBox();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnEmpty = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtICCardType_Description = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtICCardType_Remark = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboxICCardType_State = new System.Windows.Forms.ComboBox();
            this.ICCardPermissions = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtICCardType_Value = new System.Windows.Forms.TextBox();
            this.txtICCardType_Name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
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
            this.GBPermissions = new System.Windows.Forms.GroupBox();
            this.treeViewIcP = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvICCardType)).BeginInit();
            this.gbSelect.SuspendLayout();
            this.gbButton.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            this.GBPermissions.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvICCardType
            // 
            this.dgvICCardType.AllowUserToAddRows = false;
            this.dgvICCardType.AllowUserToDeleteRows = false;
            this.dgvICCardType.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvICCardType.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ICCardType_ID,
            this.ICCardType_Name,
            this.ICCardType_Value,
            this.ICCardType_State,
            this.ICCardType_Description,
            this.IC卡类型权限,
            this.ICCardType_Remark});
            this.dgvICCardType.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dgvICCardType.Location = new System.Drawing.Point(0, 338);
            this.dgvICCardType.Name = "dgvICCardType";
            this.dgvICCardType.ReadOnly = true;
            this.dgvICCardType.RowTemplate.Height = 23;
            this.dgvICCardType.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvICCardType.Size = new System.Drawing.Size(770, 186);
            this.dgvICCardType.TabIndex = 98;
            this.dgvICCardType.DoubleClick += new System.EventHandler(this.dgvICCardType_DoubleClick);
            this.dgvICCardType.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvICCardType_RowPostPaint);
            // 
            // ICCardType_ID
            // 
            this.ICCardType_ID.DataPropertyName = "ICCardType_ID";
            this.ICCardType_ID.HeaderText = "IC卡类型编号";
            this.ICCardType_ID.Name = "ICCardType_ID";
            this.ICCardType_ID.ReadOnly = true;
            this.ICCardType_ID.Visible = false;
            this.ICCardType_ID.Width = 120;
            // 
            // ICCardType_Name
            // 
            this.ICCardType_Name.DataPropertyName = "ICCardType_Name";
            this.ICCardType_Name.HeaderText = "IC卡类型名称";
            this.ICCardType_Name.Name = "ICCardType_Name";
            this.ICCardType_Name.ReadOnly = true;
            this.ICCardType_Name.Width = 150;
            // 
            // ICCardType_Value
            // 
            this.ICCardType_Value.DataPropertyName = "ICCardType_Value";
            this.ICCardType_Value.HeaderText = "IC卡类型值";
            this.ICCardType_Value.Name = "ICCardType_Value";
            this.ICCardType_Value.ReadOnly = true;
            this.ICCardType_Value.Width = 120;
            // 
            // ICCardType_State
            // 
            this.ICCardType_State.DataPropertyName = "ICCardType_State";
            this.ICCardType_State.HeaderText = "IC卡类型状态";
            this.ICCardType_State.Name = "ICCardType_State";
            this.ICCardType_State.ReadOnly = true;
            this.ICCardType_State.Width = 120;
            // 
            // ICCardType_Description
            // 
            this.ICCardType_Description.DataPropertyName = "ICCardType_Description";
            this.ICCardType_Description.HeaderText = "IC卡类型描述";
            this.ICCardType_Description.Name = "ICCardType_Description";
            this.ICCardType_Description.ReadOnly = true;
            this.ICCardType_Description.Width = 220;
            // 
            // IC卡类型权限
            // 
            this.IC卡类型权限.DataPropertyName = "ICCardType_Permissions";
            this.IC卡类型权限.HeaderText = "IC卡类型权限";
            this.IC卡类型权限.Name = "IC卡类型权限";
            this.IC卡类型权限.ReadOnly = true;
            this.IC卡类型权限.Width = 150;
            // 
            // ICCardType_Remark
            // 
            this.ICCardType_Remark.DataPropertyName = "ICCardType_Remark";
            this.ICCardType_Remark.HeaderText = "IC卡类型备注";
            this.ICCardType_Remark.Name = "ICCardType_Remark";
            this.ICCardType_Remark.ReadOnly = true;
            this.ICCardType_Remark.Width = 200;
            // 
            // gbSelect
            // 
            this.gbSelect.Controls.Add(this.txtICCardTypeName);
            this.gbSelect.Controls.Add(this.label4);
            this.gbSelect.Controls.Add(this.label10);
            this.gbSelect.Controls.Add(this.btnSearch);
            this.gbSelect.Controls.Add(this.comboxICCardTypeState);
            this.gbSelect.Controls.Add(this.label11);
            this.gbSelect.Controls.Add(this.txtICCardTypeValue);
            this.gbSelect.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbSelect.Location = new System.Drawing.Point(0, 257);
            this.gbSelect.Name = "gbSelect";
            this.gbSelect.Size = new System.Drawing.Size(770, 51);
            this.gbSelect.TabIndex = 97;
            this.gbSelect.TabStop = false;
            this.gbSelect.Text = "搜  索";
            // 
            // txtICCardTypeName
            // 
            this.txtICCardTypeName.Location = new System.Drawing.Point(91, 22);
            this.txtICCardTypeName.Name = "txtICCardTypeName";
            this.txtICCardTypeName.Size = new System.Drawing.Size(100, 21);
            this.txtICCardTypeName.TabIndex = 18;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(206, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 20;
            this.label4.Text = "IC卡类型值：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(420, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(89, 12);
            this.label10.TabIndex = 17;
            this.label10.Text = "IC卡类型状态：";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(643, 21);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 20;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // comboxICCardTypeState
            // 
            this.comboxICCardTypeState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxICCardTypeState.FormattingEnabled = true;
            this.comboxICCardTypeState.Location = new System.Drawing.Point(515, 23);
            this.comboxICCardTypeState.Name = "comboxICCardTypeState";
            this.comboxICCardTypeState.Size = new System.Drawing.Size(93, 20);
            this.comboxICCardTypeState.TabIndex = 19;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(5, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(89, 12);
            this.label11.TabIndex = 16;
            this.label11.Text = "IC卡类型名称：";
            // 
            // txtICCardTypeValue
            // 
            this.txtICCardTypeValue.Location = new System.Drawing.Point(284, 22);
            this.txtICCardTypeValue.Name = "txtICCardTypeValue";
            this.txtICCardTypeValue.Size = new System.Drawing.Size(119, 21);
            this.txtICCardTypeValue.TabIndex = 13;
            this.txtICCardTypeValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtICCardType_Value_KeyPress);
            // 
            // gbButton
            // 
            this.gbButton.Controls.Add(this.btnDel);
            this.gbButton.Controls.Add(this.btnSave);
            this.gbButton.Controls.Add(this.btnUpdate);
            this.gbButton.Controls.Add(this.btnEmpty);
            this.gbButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbButton.Location = new System.Drawing.Point(0, 211);
            this.gbButton.Name = "gbButton";
            this.gbButton.Size = new System.Drawing.Size(770, 46);
            this.gbButton.TabIndex = 96;
            this.gbButton.TabStop = false;
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(377, 17);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(75, 23);
            this.btnDel.TabIndex = 47;
            this.btnDel.Text = "删 除";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(147, 16);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 46;
            this.btnSave.Text = "保 存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Enabled = false;
            this.btnUpdate.Location = new System.Drawing.Point(32, 16);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 45;
            this.btnUpdate.Text = "修  改";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnEmpty
            // 
            this.btnEmpty.Location = new System.Drawing.Point(262, 17);
            this.btnEmpty.Name = "btnEmpty";
            this.btnEmpty.Size = new System.Drawing.Size(75, 23);
            this.btnEmpty.TabIndex = 39;
            this.btnEmpty.Text = "清  空";
            this.btnEmpty.UseVisualStyleBackColor = true;
            this.btnEmpty.Click += new System.EventHandler(this.btnEmpty_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtICCardType_Description);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtICCardType_Remark);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.comboxICCardType_State);
            this.groupBox1.Controls.Add(this.ICCardPermissions);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtICCardType_Value);
            this.groupBox1.Controls.Add(this.txtICCardType_Name);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(770, 211);
            this.groupBox1.TabIndex = 95;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "IC卡类型信息";
            // 
            // txtICCardType_Description
            // 
            this.txtICCardType_Description.Location = new System.Drawing.Point(109, 108);
            this.txtICCardType_Description.Multiline = true;
            this.txtICCardType_Description.Name = "txtICCardType_Description";
            this.txtICCardType_Description.Size = new System.Drawing.Size(499, 35);
            this.txtICCardType_Description.TabIndex = 21;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(17, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 20;
            this.label7.Text = "IC卡权限设置：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(463, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 20;
            this.label1.Text = "IC卡类型值：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 12);
            this.label6.TabIndex = 20;
            this.label6.Text = "IC卡类型名称：";
            // 
            // txtICCardType_Remark
            // 
            this.txtICCardType_Remark.Location = new System.Drawing.Point(109, 155);
            this.txtICCardType_Remark.Multiline = true;
            this.txtICCardType_Remark.Name = "txtICCardType_Remark";
            this.txtICCardType_Remark.Size = new System.Drawing.Size(499, 37);
            this.txtICCardType_Remark.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 158);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 18;
            this.label5.Text = "IC卡类型备注：";
            // 
            // comboxICCardType_State
            // 
            this.comboxICCardType_State.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboxICCardType_State.FormattingEnabled = true;
            this.comboxICCardType_State.Location = new System.Drawing.Point(369, 27);
            this.comboxICCardType_State.Name = "comboxICCardType_State";
            this.comboxICCardType_State.Size = new System.Drawing.Size(83, 20);
            this.comboxICCardType_State.TabIndex = 16;
            // 
            // ICCardPermissions
            // 
            this.ICCardPermissions.Location = new System.Drawing.Point(109, 67);
            this.ICCardPermissions.Name = "ICCardPermissions";
            this.ICCardPermissions.ReadOnly = true;
            this.ICCardPermissions.Size = new System.Drawing.Size(499, 21);
            this.ICCardPermissions.TabIndex = 13;
            this.ICCardPermissions.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ICCardPermissions_MouseDoubleClick);
            this.ICCardPermissions.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtICCardType_Value_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "IC卡类型描述：";
            // 
            // txtICCardType_Value
            // 
            this.txtICCardType_Value.Location = new System.Drawing.Point(543, 27);
            this.txtICCardType_Value.Name = "txtICCardType_Value";
            this.txtICCardType_Value.Size = new System.Drawing.Size(65, 21);
            this.txtICCardType_Value.TabIndex = 13;
            this.txtICCardType_Value.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtICCardType_Value_KeyPress);
            // 
            // txtICCardType_Name
            // 
            this.txtICCardType_Name.Location = new System.Drawing.Point(109, 27);
            this.txtICCardType_Name.Name = "txtICCardType_Name";
            this.txtICCardType_Name.Size = new System.Drawing.Size(140, 21);
            this.txtICCardType_Name.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(270, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "IC卡类型状态：";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(111, 282);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 282);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.AutoSize = false;
            this.bindingNavigator1.BackColor = System.Drawing.Color.Gainsboro;
            this.bindingNavigator1.CountItem = null;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Font = new System.Drawing.Font("宋体", 9F);
            this.bindingNavigator1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
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
            this.toolStripSeparator17});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 308);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.Size = new System.Drawing.Size(770, 27);
            this.bindingNavigator1.TabIndex = 100;
            this.bindingNavigator1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.bdnInfo_ItemClicked);
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
            // GBPermissions
            // 
            this.GBPermissions.Controls.Add(this.treeViewIcP);
            this.GBPermissions.Controls.Add(this.button2);
            this.GBPermissions.Controls.Add(this.button1);
            this.GBPermissions.Location = new System.Drawing.Point(229, 63);
            this.GBPermissions.Name = "GBPermissions";
            this.GBPermissions.Size = new System.Drawing.Size(200, 310);
            this.GBPermissions.TabIndex = 101;
            this.GBPermissions.TabStop = false;
            // 
            // treeViewIcP
            // 
            this.treeViewIcP.CheckBoxes = true;
            this.treeViewIcP.Dock = System.Windows.Forms.DockStyle.Top;
            this.treeViewIcP.Location = new System.Drawing.Point(3, 17);
            this.treeViewIcP.Name = "treeViewIcP";
            this.treeViewIcP.Size = new System.Drawing.Size(194, 259);
            this.treeViewIcP.TabIndex = 4;
            // 
            // ICCardTypeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 524);
            this.Controls.Add(this.GBPermissions);
            this.Controls.Add(this.bindingNavigator1);
            this.Controls.Add(this.dgvICCardType);
            this.Controls.Add(this.gbSelect);
            this.Controls.Add(this.gbButton);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "ICCardTypeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IC卡类型";
            this.Load += new System.EventHandler(this.ICCardTypeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvICCardType)).EndInit();
            this.gbSelect.ResumeLayout(false);
            this.gbSelect.PerformLayout();
            this.gbButton.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            this.GBPermissions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvICCardType;
        private System.Windows.Forms.GroupBox gbSelect;
        private System.Windows.Forms.GroupBox gbButton;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnEmpty;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtICCardType_Description;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtICCardType_Remark;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboxICCardType_State;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtICCardTypeName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ComboBox comboxICCardTypeState;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
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
        private System.Windows.Forms.TextBox txtICCardType_Name;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtICCardType_Value;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtICCardTypeValue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox ICCardPermissions;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox GBPermissions;
        private System.Windows.Forms.TreeView treeViewIcP;
        private System.Windows.Forms.DataGridViewTextBoxColumn ICCardType_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ICCardType_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn ICCardType_Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn ICCardType_State;
        private System.Windows.Forms.DataGridViewTextBoxColumn ICCardType_Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn IC卡类型权限;
        private System.Windows.Forms.DataGridViewTextBoxColumn ICCardType_Remark;
    }
}