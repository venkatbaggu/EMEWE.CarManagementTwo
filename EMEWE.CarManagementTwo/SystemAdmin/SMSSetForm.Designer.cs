namespace EMEWE.CarManagement.SystemAdmin
{
    partial class SMSSetForm
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbButton = new System.Windows.Forms.GroupBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnSMSApplication = new System.Windows.Forms.Button();
            this.lvwUserList = new System.Windows.Forms.DataGridView();
            this.PositionSMS_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PositionSMS_Position_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PositionSMS_Content = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PositionSMS_State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PositionSMS_Operate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PositionSMS_Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PositionSMS_Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbList = new System.Windows.Forms.GroupBox();
            this.bdnInfo = new System.Windows.Forms.BindingNavigator(this.components);
            this.tslHomPage1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.tslPreviousPage1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.tstbPageCurrent = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tslPageCount = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.tslNMax = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.tslNextPage1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.tslLastPage1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel8 = new System.Windows.Forms.ToolStripLabel();
            this.tscbxPageSize = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.gbSelect = new System.Windows.Forms.GroupBox();
            this.chkboxSLEDState = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkSPositionLED_Position_Id = new System.Windows.Forms.ComboBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPositionSMS_Count = new System.Windows.Forms.TextBox();
            this.chkPositionSMS_Position_Id = new System.Windows.Forms.ComboBox();
            this.combokPositionSMS_State = new System.Windows.Forms.ComboBox();
            this.txtPositionSMS_Content = new System.Windows.Forms.TextBox();
            this.gbContent = new System.Windows.Forms.GroupBox();
            this.gbButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lvwUserList)).BeginInit();
            this.gbList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdnInfo)).BeginInit();
            this.bdnInfo.SuspendLayout();
            this.gbSelect.SuspendLayout();
            this.gbContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(437, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 28;
            this.btnCancel.Text = "清空";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gbButton
            // 
            this.gbButton.Controls.Add(this.btnDelete);
            this.gbButton.Controls.Add(this.btnCancel);
            this.gbButton.Controls.Add(this.btnAdd);
            this.gbButton.Controls.Add(this.btnUpdate);
            this.gbButton.Controls.Add(this.btnSMSApplication);
            this.gbButton.Location = new System.Drawing.Point(1, 105);
            this.gbButton.Name = "gbButton";
            this.gbButton.Size = new System.Drawing.Size(769, 47);
            this.gbButton.TabIndex = 4;
            this.gbButton.TabStop = false;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(308, 15);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 29;
            this.btnDelete.Text = "删 除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(169, 15);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 27;
            this.btnAdd.Text = "保存";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(37, 15);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 26;
            this.btnUpdate.Text = "修改";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnSMSApplication
            // 
            this.btnSMSApplication.Location = new System.Drawing.Point(561, 15);
            this.btnSMSApplication.Name = "btnSMSApplication";
            this.btnSMSApplication.Size = new System.Drawing.Size(75, 23);
            this.btnSMSApplication.TabIndex = 24;
            this.btnSMSApplication.Text = "短信应用";
            this.btnSMSApplication.UseVisualStyleBackColor = true;
            this.btnSMSApplication.Click += new System.EventHandler(this.btnSMSApplication_Click);
            // 
            // lvwUserList
            // 
            this.lvwUserList.AllowUserToAddRows = false;
            this.lvwUserList.AllowUserToDeleteRows = false;
            this.lvwUserList.AllowUserToResizeRows = false;
            this.lvwUserList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lvwUserList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PositionSMS_Id,
            this.PositionSMS_Position_Id,
            this.PositionSMS_Content,
            this.PositionSMS_State,
            this.PositionSMS_Operate,
            this.PositionSMS_Time,
            this.PositionSMS_Remark});
            this.lvwUserList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lvwUserList.Location = new System.Drawing.Point(3, 0);
            this.lvwUserList.Name = "lvwUserList";
            this.lvwUserList.ReadOnly = true;
            this.lvwUserList.RowTemplate.Height = 23;
            this.lvwUserList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.lvwUserList.Size = new System.Drawing.Size(766, 282);
            this.lvwUserList.TabIndex = 72;
            this.lvwUserList.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.lvwUserList_RowPostPaint);
            this.lvwUserList.DoubleClick += new System.EventHandler(this.lvwUserList_DoubleClick);
            // 
            // PositionSMS_Id
            // 
            this.PositionSMS_Id.DataPropertyName = "PositionSMS_Id";
            this.PositionSMS_Id.HeaderText = "短信编号";
            this.PositionSMS_Id.Name = "PositionSMS_Id";
            this.PositionSMS_Id.ReadOnly = true;
            // 
            // PositionSMS_Position_Id
            // 
            this.PositionSMS_Position_Id.DataPropertyName = "PositionSMS_Position_Id";
            this.PositionSMS_Position_Id.HeaderText = "门岗编号";
            this.PositionSMS_Position_Id.Name = "PositionSMS_Position_Id";
            this.PositionSMS_Position_Id.ReadOnly = true;
            // 
            // PositionSMS_Content
            // 
            this.PositionSMS_Content.DataPropertyName = "PositionSMS_Content";
            this.PositionSMS_Content.HeaderText = "短信配置内容";
            this.PositionSMS_Content.Name = "PositionSMS_Content";
            this.PositionSMS_Content.ReadOnly = true;
            this.PositionSMS_Content.Width = 200;
            // 
            // PositionSMS_State
            // 
            this.PositionSMS_State.DataPropertyName = "PositionSMS_State";
            this.PositionSMS_State.HeaderText = "短信配置状态";
            this.PositionSMS_State.Name = "PositionSMS_State";
            this.PositionSMS_State.ReadOnly = true;
            this.PositionSMS_State.Width = 120;
            // 
            // PositionSMS_Operate
            // 
            this.PositionSMS_Operate.DataPropertyName = "PositionSMS_Operate";
            this.PositionSMS_Operate.HeaderText = "短信配置人";
            this.PositionSMS_Operate.Name = "PositionSMS_Operate";
            this.PositionSMS_Operate.ReadOnly = true;
            this.PositionSMS_Operate.Width = 120;
            // 
            // PositionSMS_Time
            // 
            this.PositionSMS_Time.DataPropertyName = "PositionSMS_Time";
            this.PositionSMS_Time.HeaderText = "短信配置时间";
            this.PositionSMS_Time.Name = "PositionSMS_Time";
            this.PositionSMS_Time.ReadOnly = true;
            this.PositionSMS_Time.Width = 120;
            // 
            // PositionSMS_Remark
            // 
            this.PositionSMS_Remark.DataPropertyName = "PositionSMS_Remark";
            this.PositionSMS_Remark.HeaderText = "短信间隔数";
            this.PositionSMS_Remark.Name = "PositionSMS_Remark";
            this.PositionSMS_Remark.ReadOnly = true;
            this.PositionSMS_Remark.Width = 120;
            // 
            // gbList
            // 
            this.gbList.Controls.Add(this.lvwUserList);
            this.gbList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gbList.Location = new System.Drawing.Point(0, 241);
            this.gbList.Name = "gbList";
            this.gbList.Size = new System.Drawing.Size(772, 285);
            this.gbList.TabIndex = 3;
            this.gbList.TabStop = false;
            // 
            // bdnInfo
            // 
            this.bdnInfo.AddNewItem = null;
            this.bdnInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.bdnInfo.AutoSize = false;
            this.bdnInfo.BackColor = System.Drawing.Color.Gainsboro;
            this.bdnInfo.CountItem = null;
            this.bdnInfo.DeleteItem = null;
            this.bdnInfo.Dock = System.Windows.Forms.DockStyle.None;
            this.bdnInfo.Font = new System.Drawing.Font("宋体", 9F);
            this.bdnInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bdnInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslHomPage1,
            this.toolStripSeparator8,
            this.tslPreviousPage1,
            this.toolStripSeparator9,
            this.toolStripLabel4,
            this.tstbPageCurrent,
            this.toolStripLabel1,
            this.tslPageCount,
            this.toolStripLabel2,
            this.toolStripSeparator1,
            this.toolStripLabel3,
            this.tslNMax,
            this.toolStripLabel5,
            this.toolStripSeparator10,
            this.tslNextPage1,
            this.toolStripSeparator11,
            this.tslLastPage1,
            this.toolStripSeparator12,
            this.toolStripLabel8,
            this.tscbxPageSize,
            this.toolStripSeparator13});
            this.bdnInfo.Location = new System.Drawing.Point(3, 206);
            this.bdnInfo.MoveFirstItem = null;
            this.bdnInfo.MoveLastItem = null;
            this.bdnInfo.MoveNextItem = null;
            this.bdnInfo.MovePreviousItem = null;
            this.bdnInfo.Name = "bdnInfo";
            this.bdnInfo.PositionItem = null;
            this.bdnInfo.Size = new System.Drawing.Size(766, 32);
            this.bdnInfo.TabIndex = 73;
            this.bdnInfo.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.bdnInfo_ItemClicked);
            // 
            // tslHomPage1
            // 
            this.tslHomPage1.Name = "tslHomPage1";
            this.tslHomPage1.Size = new System.Drawing.Size(29, 29);
            this.tslHomPage1.Text = "首页";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 32);
            // 
            // tslPreviousPage1
            // 
            this.tslPreviousPage1.Name = "tslPreviousPage1";
            this.tslPreviousPage1.Size = new System.Drawing.Size(41, 29);
            this.tslPreviousPage1.Text = "上一页";
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 32);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(53, 29);
            this.toolStripLabel4.Text = "当前页数";
            // 
            // tstbPageCurrent
            // 
            this.tstbPageCurrent.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tstbPageCurrent.Name = "tstbPageCurrent";
            this.tstbPageCurrent.Size = new System.Drawing.Size(50, 32);
            this.tstbPageCurrent.Text = "1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(17, 29);
            this.toolStripLabel1.Text = "共";
            // 
            // tslPageCount
            // 
            this.tslPageCount.Name = "tslPageCount";
            this.tslPageCount.Size = new System.Drawing.Size(11, 29);
            this.tslPageCount.Text = "0";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(17, 29);
            this.toolStripLabel2.Text = "页";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 32);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(17, 29);
            this.toolStripLabel3.Text = "共";
            // 
            // tslNMax
            // 
            this.tslNMax.Name = "tslNMax";
            this.tslNMax.Size = new System.Drawing.Size(11, 29);
            this.tslNMax.Text = "0";
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(17, 29);
            this.toolStripLabel5.Text = "条";
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 32);
            // 
            // tslNextPage1
            // 
            this.tslNextPage1.Name = "tslNextPage1";
            this.tslNextPage1.Size = new System.Drawing.Size(41, 29);
            this.tslNextPage1.Text = "下一页";
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(6, 32);
            // 
            // tslLastPage1
            // 
            this.tslLastPage1.Name = "tslLastPage1";
            this.tslLastPage1.Size = new System.Drawing.Size(29, 29);
            this.tslLastPage1.Text = "尾页";
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(6, 32);
            // 
            // toolStripLabel8
            // 
            this.toolStripLabel8.Name = "toolStripLabel8";
            this.toolStripLabel8.Size = new System.Drawing.Size(53, 29);
            this.toolStripLabel8.Text = "每页条数";
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
            this.tscbxPageSize.Size = new System.Drawing.Size(75, 32);
            this.tscbxPageSize.Text = "10";
            this.tscbxPageSize.SelectedIndexChanged += new System.EventHandler(this.tscbxPageSize_SelectedIndexChanged);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            this.toolStripSeparator13.Size = new System.Drawing.Size(6, 32);
            // 
            // gbSelect
            // 
            this.gbSelect.Controls.Add(this.chkboxSLEDState);
            this.gbSelect.Controls.Add(this.label7);
            this.gbSelect.Controls.Add(this.label5);
            this.gbSelect.Controls.Add(this.chkSPositionLED_Position_Id);
            this.gbSelect.Controls.Add(this.btnSelect);
            this.gbSelect.Location = new System.Drawing.Point(2, 158);
            this.gbSelect.Name = "gbSelect";
            this.gbSelect.Size = new System.Drawing.Size(769, 45);
            this.gbSelect.TabIndex = 5;
            this.gbSelect.TabStop = false;
            // 
            // chkboxSLEDState
            // 
            this.chkboxSLEDState.FormattingEnabled = true;
            this.chkboxSLEDState.Items.AddRange(new object[] {
            "正常",
            "注销",
            "暂停"});
            this.chkboxSLEDState.Location = new System.Drawing.Point(331, 16);
            this.chkboxSLEDState.Name = "chkboxSLEDState";
            this.chkboxSLEDState.Size = new System.Drawing.Size(70, 20);
            this.chkboxSLEDState.TabIndex = 74;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(266, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 73;
            this.label7.Text = "短信状态:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(45, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 72;
            this.label5.Text = "门岗:";
            // 
            // chkSPositionLED_Position_Id
            // 
            this.chkSPositionLED_Position_Id.FormattingEnabled = true;
            this.chkSPositionLED_Position_Id.Location = new System.Drawing.Point(86, 16);
            this.chkSPositionLED_Position_Id.Name = "chkSPositionLED_Position_Id";
            this.chkSPositionLED_Position_Id.Size = new System.Drawing.Size(100, 20);
            this.chkSPositionLED_Position_Id.TabIndex = 71;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(560, 15);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 31;
            this.btnSelect.Text = "搜 索";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 46;
            this.label4.Text = "门岗:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 50;
            this.label2.Text = "短信内容:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(286, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 60;
            this.label6.Text = "短信状态:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(497, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 12);
            this.label8.TabIndex = 60;
            this.label8.Text = "发送间隔:";
            // 
            // txtPositionSMS_Count
            // 
            this.txtPositionSMS_Count.Location = new System.Drawing.Point(567, 17);
            this.txtPositionSMS_Count.Name = "txtPositionSMS_Count";
            this.txtPositionSMS_Count.Size = new System.Drawing.Size(59, 21);
            this.txtPositionSMS_Count.TabIndex = 66;
            // 
            // chkPositionSMS_Position_Id
            // 
            this.chkPositionSMS_Position_Id.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.chkPositionSMS_Position_Id.FormattingEnabled = true;
            this.chkPositionSMS_Position_Id.Location = new System.Drawing.Point(99, 17);
            this.chkPositionSMS_Position_Id.Name = "chkPositionSMS_Position_Id";
            this.chkPositionSMS_Position_Id.Size = new System.Drawing.Size(100, 20);
            this.chkPositionSMS_Position_Id.TabIndex = 70;
            // 
            // combokPositionSMS_State
            // 
            this.combokPositionSMS_State.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combokPositionSMS_State.FormattingEnabled = true;
            this.combokPositionSMS_State.Location = new System.Drawing.Point(351, 17);
            this.combokPositionSMS_State.Name = "combokPositionSMS_State";
            this.combokPositionSMS_State.Size = new System.Drawing.Size(70, 20);
            this.combokPositionSMS_State.TabIndex = 71;
            // 
            // txtPositionSMS_Content
            // 
            this.txtPositionSMS_Content.Location = new System.Drawing.Point(99, 46);
            this.txtPositionSMS_Content.Multiline = true;
            this.txtPositionSMS_Content.Name = "txtPositionSMS_Content";
            this.txtPositionSMS_Content.Size = new System.Drawing.Size(610, 47);
            this.txtPositionSMS_Content.TabIndex = 73;
            // 
            // gbContent
            // 
            this.gbContent.Controls.Add(this.txtPositionSMS_Content);
            this.gbContent.Controls.Add(this.combokPositionSMS_State);
            this.gbContent.Controls.Add(this.chkPositionSMS_Position_Id);
            this.gbContent.Controls.Add(this.txtPositionSMS_Count);
            this.gbContent.Controls.Add(this.label8);
            this.gbContent.Controls.Add(this.label6);
            this.gbContent.Controls.Add(this.label2);
            this.gbContent.Controls.Add(this.label4);
            this.gbContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbContent.Location = new System.Drawing.Point(0, 0);
            this.gbContent.Name = "gbContent";
            this.gbContent.Size = new System.Drawing.Size(772, 99);
            this.gbContent.TabIndex = 2;
            this.gbContent.TabStop = false;
            // 
            // SMSSetForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 526);
            this.Controls.Add(this.bdnInfo);
            this.Controls.Add(this.gbSelect);
            this.Controls.Add(this.gbContent);
            this.Controls.Add(this.gbButton);
            this.Controls.Add(this.gbList);
            this.Name = "SMSSetForm";
            this.Text = "短信设置";
            this.Load += new System.EventHandler(this.SMSSetForm_Load);
            this.gbButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lvwUserList)).EndInit();
            this.gbList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bdnInfo)).EndInit();
            this.bdnInfo.ResumeLayout(false);
            this.bdnInfo.PerformLayout();
            this.gbSelect.ResumeLayout(false);
            this.gbSelect.PerformLayout();
            this.gbContent.ResumeLayout(false);
            this.gbContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox gbButton;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnSMSApplication;
        private System.Windows.Forms.DataGridView lvwUserList;
        private System.Windows.Forms.GroupBox gbList;
        private System.Windows.Forms.BindingNavigator bdnInfo;
        private System.Windows.Forms.ToolStripLabel tslHomPage1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripLabel tslPreviousPage1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripTextBox tstbPageCurrent;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel tslPageCount;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripLabel tslNMax;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripLabel tslNextPage1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripLabel tslLastPage1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripLabel toolStripLabel8;
        private System.Windows.Forms.ToolStripComboBox tscbxPageSize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.GroupBox gbSelect;
        private System.Windows.Forms.ComboBox chkboxSLEDState;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox chkSPositionLED_Position_Id;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtPositionSMS_Count;
        private System.Windows.Forms.ComboBox chkPositionSMS_Position_Id;
        private System.Windows.Forms.ComboBox combokPositionSMS_State;
        private System.Windows.Forms.TextBox txtPositionSMS_Content;
        private System.Windows.Forms.GroupBox gbContent;
        private System.Windows.Forms.DataGridViewTextBoxColumn PositionSMS_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn PositionSMS_Position_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn PositionSMS_Content;
        private System.Windows.Forms.DataGridViewTextBoxColumn PositionSMS_State;
        private System.Windows.Forms.DataGridViewTextBoxColumn PositionSMS_Operate;
        private System.Windows.Forms.DataGridViewTextBoxColumn PositionSMS_Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn PositionSMS_Remark;
    }
}